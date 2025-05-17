using System.Reflection;
using FluentCodeGenTool.Abstractions;
using FluentCRUD.Abstraction;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public static class Program
{
    private static Assembly? _loadedAssembly;
    private static string _assemblyPath;

    public static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: FluentCodeGenTool <AssemblyPath> <OutputPath>");
            return;
        }

        _assemblyPath = args[0];
        var outputPath = args[1];

        _loadedAssembly = Assembly.LoadFrom(_assemblyPath);
        var context = new GenerationContext(outputPath);

        ProcessAssembly(_loadedAssembly, context);
        InsertUsings(context);
    }

    private static void ProcessAssembly(Assembly asm, GenerationContext context)
    {
        foreach (var modelType in asm.GetTypes())
        {
            var mapType = modelType.GetNestedType("Configurator",
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            if (mapType == null || !typeof(IModelGenerationConfigurator).IsAssignableFrom(mapType))
                continue;

            if (Activator.CreateInstance(mapType) is not IModelGenerationConfigurator mapInstance)
                continue;

            var pipeline = new GenerationPipeline(context);
            mapInstance.Configuration(pipeline);
            context = pipeline.ExecuteAll(context);
            
            foreach (var generationFile in context.Files)
            {
                Directory.CreateDirectory(generationFile.OutputFilePath);
                var filePath = Path.Combine(generationFile.OutputFilePath, $"{generationFile.FileName}.g.cs");

                File.WriteAllText(filePath, generationFile.Contents);
            }
        }
    }

    private static void InsertUsings(GenerationContext context)
    {
        foreach (var file in context.Files)
        {
            // Используем Roslyn для анализа типов в файле
            var syntaxTree = CSharpSyntaxTree.ParseText(file.Contents);
            var root = syntaxTree.GetRoot() as CompilationUnitSyntax;

            var semanticModel = CSharpCompilation.Create("Validation")
                .AddSyntaxTrees(syntaxTree)
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .GetSemanticModel(syntaxTree);

            // Проходим по всем типам в файле
            var classes = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(t => t != null)
                .Distinct();
            
            var usings = new HashSet<string> { "System" };

            // Добавляем нужные using для каждого типа
            foreach (var classDecl in classes)
            {
                var properties = classDecl.Members.OfType<PropertyDeclarationSyntax>();
                
                foreach (var prop in properties)
                {
                    var typeInfo = semanticModel.GetTypeInfo(prop.Type);
                    var symbol = typeInfo.Type as INamedTypeSymbol;
                    if (symbol == null) continue;

                    if (!symbol.ContainingNamespace.IsGlobalNamespace)
                        usings.Add(symbol.ContainingNamespace.ToDisplayString());
                }
            }
            
            var unit = SyntaxFactory.CompilationUnit()
                .AddUsings(usings
                    .Select(u => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(u)))
                    .ToArray())
                .NormalizeWhitespace();
            
            var codeResult = unit.ToFullString();
            
            var filePath = Path.Combine(file.OutputFilePath, $"{file.FileName}.g.cs");
            var newFileContent = InsertUsingDirectives(file.Contents, codeResult);
            File.WriteAllText(filePath, newFileContent);
            Console.WriteLine($"Inserted using statements in: {file}");
        }
    }
    
    private static string InsertUsingDirectives(string fileContent, string newUsingsText)
    {
        var tree = CSharpSyntaxTree.ParseText(fileContent);
        var root = tree.GetRoot() as CompilationUnitSyntax;
        if (root == null)
            throw new Exception("Failed to parse file.");

        // Парсим новый текст using-ов как отдельное синтаксическое дерево
        var usingsTree = CSharpSyntaxTree.ParseText(newUsingsText);
        var usingsRoot = usingsTree.GetRoot() as CompilationUnitSyntax;
        if (usingsRoot == null)
            throw new Exception("Failed to parse usings.");

        var newUsings = usingsRoot.Usings;

        // Заменяем using-и
        var newRoot = root.WithUsings(newUsings);

        var formatted = newRoot.NormalizeWhitespace().ToFullString();
        return formatted;
    }
}
