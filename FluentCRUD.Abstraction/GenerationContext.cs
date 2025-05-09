namespace FluentCRUD.Abstraction;

public class GenerationContext
{ 
	public string OutputFilePath { get; set; }
	
	public List<GenerationFile> Files { get; } = new();

	public void AddFile(string fileName, string content, string outputFilePath = "")
	{
		Files.Add(new GenerationFile()
		{
			FileName = fileName,
			OutputFilePath = string.IsNullOrEmpty(outputFilePath) ? OutputFilePath : outputFilePath,
			Contents =content
		});
	}
}

public class GenerationFile
{
	public string FileName { get; set; }
	public string OutputFilePath { get; set; }
	public string Contents { get; set; }
}