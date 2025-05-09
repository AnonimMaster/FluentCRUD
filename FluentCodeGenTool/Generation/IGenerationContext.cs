namespace FluentCodeGenTool.Abstractions;

public interface IGenerationContext
{
	public string OutputFilePath { get; set; }
	
	public List<IGenerationFile> Files { get; }

	public void AddFile(string fileName, string content, string outputFilePath = "");
}