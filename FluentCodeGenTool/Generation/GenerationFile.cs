using FluentCodeGenTool.Abstractions;

namespace FluentCodeGenTool.Generation;

public class GenerationFile: IGenerationFile
{
	public string FileName { get; set; }
	public string NameSpace { get; set; }
	public string OutputFilePath { get; set; }
	public string Contents { get; set; }
}