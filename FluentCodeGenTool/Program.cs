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

		AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

		_loadedAssembly = Assembly.LoadFrom(_assemblyPath);
		var context = new GenerationContext(outputPath);

		ProcessAssembly(_loadedAssembly, context);
		InsertUsings(context);

		AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
	}

	private static Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
	{
		var assemblyName = new AssemblyName(args.Name);
		var dependencyPath = Path.Combine(Path.GetDirectoryName(_assemblyPath)!, assemblyName.Name + ".dll");

		if (File.Exists(dependencyPath))
		{
			return Assembly.LoadFrom(dependencyPath);
		}

		return null;
	}

	private static void ProcessAssembly(Assembly asm, IGenerationContext context)
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
		Dictionary<string, string> dictUsing = context.Files.ToDictionary(f => f.FileName, f => f.NameSpace);

		var syntaxTrees = context.Files
			.Select(f => CSharpSyntaxTree.ParseText(f.Contents))
			.ToList();

		var compilation = CSharpCompilation.Create("Generated")
			.AddSyntaxTrees(syntaxTrees)
			.AddReferences(
				MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
			)
			.WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		foreach (var file in context.Files)
		{
			var tree = syntaxTrees.First(t => t.ToString() == file.Contents);
			var root = tree.GetRoot() as CompilationUnitSyntax;
			if (root == null) continue;

			var semanticModel = compilation.GetSemanticModel(tree);

			var classes = root.DescendantNodes()
				.OfType<ClassDeclarationSyntax>()
				.Distinct();

			var usings = new HashSet<string> { "System" };

			foreach (var classDecl in classes)
			{
				var properties = classDecl.Members.OfType<PropertyDeclarationSyntax>();

				foreach (var prop in properties)
				{
					var typeInfo = semanticModel.GetTypeInfo(prop.Type);
					var symbol = typeInfo.Type as INamedTypeSymbol;
					if (symbol == null) continue;

					if (typeInfo.Type is IErrorTypeSymbol)
					{
						if (prop.Type is GenericNameSyntax generic)
						{
							foreach (var typeArg in generic.TypeArgumentList.Arguments)
							{
								var typeArgInfo = semanticModel.GetTypeInfo(typeArg);
								if (dictUsing.TryGetValue(typeArgInfo.Type.Name, out var ns))
								{
									usings.Add(ns);
								}
							}
						}
						else if (dictUsing.TryGetValue(typeInfo.Type.Name, out var ns))
						{
							usings.Add(ns);
						}
					}
					else
					{
						var ns = symbol.ContainingNamespace?.ToDisplayString();
						if (!string.IsNullOrEmpty(ns) && !symbol.ContainingNamespace.IsGlobalNamespace)
						{
							usings.Add(ns);
						}
					}
				}
			}

			var unit = SyntaxFactory.CompilationUnit()
				.AddUsings(usings
					.OrderBy(u => u)
					.Select(u => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(u)))
					.ToArray())
				.NormalizeWhitespace();

			var newFileContent = InsertUsingDirectives(file.Contents, unit.ToFullString());

			var filePath = Path.Combine(file.OutputFilePath, $"{file.FileName}.g.cs");
			File.WriteAllText(filePath, newFileContent);
			Console.WriteLine($"✔ Inserted usings in: {filePath}");
		}
	}

	private static string InsertUsingDirectives(string fileContent, string newUsingsText)
	{
		var tree = CSharpSyntaxTree.ParseText(fileContent);
		var root = tree.GetRoot() as CompilationUnitSyntax;
		if (root == null)
			throw new Exception("Failed to parse file.");

		var usingsTree = CSharpSyntaxTree.ParseText(newUsingsText);
		var usingsRoot = usingsTree.GetRoot() as CompilationUnitSyntax;
		if (usingsRoot == null)
			throw new Exception("Failed to parse usings.");

		var newUsings = usingsRoot.Usings;

		var newRoot = root.WithUsings(newUsings);

		var formatted = newRoot.NormalizeWhitespace().ToFullString();
		return formatted;
	}
}