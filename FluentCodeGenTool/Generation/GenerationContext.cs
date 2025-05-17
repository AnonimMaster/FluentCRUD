using FluentCodeGenTool.Abstractions;
using FluentCodeGenTool.Generation;

namespace FluentCRUD.Abstraction;

public class GenerationContext: IGenerationContext
{
	public GenerationContext(string outputFilePath)
	{
		OutputFilePath = outputFilePath;
	}

	public string OutputFilePath { get; }
	
	public List<IGenerationFile> Files { get; } = [];

	public IGenerationContext AddFile(string fileName, string @namespace, string content, string outputFilePath = "")
	{
		Files.Add(new GenerationFile()
		{
			FileName = fileName,
			NameSpace = @namespace,
			OutputFilePath = string.IsNullOrEmpty(outputFilePath) ? OutputFilePath : outputFilePath,
			Contents =content
		});

		return this;
	}
}