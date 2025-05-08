using System.Reflection;
using FluentCodeGenTool;
using FluentCodeGenTool.Abstractions;

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
	
	var context = new GenerationContext
	{
		ModelType = modelType
	};

	var pipeline = new GenerationPipeline(context);
	mapInstance.Configuration(pipeline);
	var generationContext = pipeline.Build();
	
	foreach (var generationFile in generationContext.Files)
	{
		Directory.CreateDirectory(generationFile.OutputFilePath);
		var filePath = Path.Combine(generationFile.OutputFilePath, $"{generationFile.FileName}.g.cs");
		File.WriteAllText(filePath, generationFile.Contents);
		Console.WriteLine($"Generated: {filePath}");
	}
}