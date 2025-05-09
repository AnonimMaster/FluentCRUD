using System.Reflection;
using FluentCodeGenTool.Abstractions;
using FluentCRUD.Abstraction;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

if (args.Length < 2)
{
	Console.WriteLine("Usage: FluentCodeGenTool <AssemblyPath> <OutputPath>");
	return;
}

var assemblyPath = args[0];
var outputPath = args[1];

var asm = Assembly.LoadFrom(assemblyPath);

foreach (var modelType  in asm.GetTypes())
{
	var mapType = modelType.GetNestedType("Configurator", 
		BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

	if (mapType == null || !typeof(IModelGenerationConfigurator).IsAssignableFrom(mapType))
		continue;

	if (Activator.CreateInstance(mapType) is not IModelGenerationConfigurator mapInstance)
		continue;

	var context = new GenerationContext(outputPath);

	var pipeline = new GenerationPipeline(context);
	mapInstance.Configuration(pipeline);
	context = pipeline.ExecuteAll(context);
	
	foreach (var generationFile in context.Files)
	{
		Directory.CreateDirectory(generationFile.OutputFilePath);
		var filePath = Path.Combine(generationFile.OutputFilePath, $"{generationFile.FileName}.g.cs");
		File.WriteAllText(filePath, generationFile.Contents);
	}

	foreach (var generationFile in context.Files)
	{
		var filePath = Path.Combine(generationFile.OutputFilePath, $"{generationFile.FileName}.g.cs");
		
		// === Проверка валидности файла через Roslyn ===
		var syntaxTree = CSharpSyntaxTree.ParseText(generationFile.Contents);
		var compilation = CSharpCompilation.Create("Validation")
			.AddSyntaxTrees(syntaxTree)
			.AddReferences(
				MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
				// Можно добавить другие стандартные референсы если нужно
			)
			.WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		using var ms = new MemoryStream();
		var result = compilation.Emit(ms);

		if (!result.Success)
		{
			File.Delete(filePath);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"Invalid file removed: {filePath}");
			foreach (var diag in result.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error))
			{
				Console.WriteLine($"[Error] {diag.GetMessage()}");
			}
			Console.ResetColor();
		}
		else
		{
			Console.WriteLine($"Generated: {filePath}");
		}
	}
}