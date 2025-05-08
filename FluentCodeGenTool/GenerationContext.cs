namespace FluentCodeGenTool;

public class GenerationContext
{
	public Type ModelType { get; set; }
	public string TypeName { get; set; }
	public string Namespace { get; set; }
	public string InputFilePath { get; set; }
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