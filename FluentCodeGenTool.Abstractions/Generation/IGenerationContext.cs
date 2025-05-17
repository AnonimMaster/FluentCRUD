using System.Collections.Generic;

namespace FluentCodeGenTool.Abstractions;

public interface IGenerationContext
{
	public string OutputFilePath { get; }
	
	public List<IGenerationFile> Files { get; }

	public IGenerationContext AddFile(string fileName, string @namespace, string content, string outputFilePath = "");
}