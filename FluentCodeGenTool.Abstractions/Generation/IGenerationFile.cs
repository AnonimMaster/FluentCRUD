namespace FluentCodeGenTool.Abstractions;

public interface IGenerationFile
{
	public string NameSpace { get; set; }
	public string FileName { get; set; }
	public string OutputFilePath { get; set; }
	public string Contents { get; set; }
}