namespace FluentCRUD.Abstraction;

public class GenerationContext
{
	public GenerationContext(string outputFilePath)
	{
		OutputFilePath = outputFilePath;
	}

	public string OutputFilePath { get; }
	
	public List<GenerationFile> Files { get; } = [];

	public GenerationContext AddFile(string fileName, string content, string outputFilePath = "")
	{
		Files.Add(new GenerationFile()
		{
			FileName = fileName,
			OutputFilePath = string.IsNullOrEmpty(outputFilePath) ? OutputFilePath : outputFilePath,
			Contents =content
		});

		return this;
	}
}

public class GenerationFile
{
	public string FileName { get; set; }
	public string OutputFilePath { get; set; }
	public string Contents { get; set; }
}