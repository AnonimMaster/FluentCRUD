using FluentCodeGenTool;
using FluentCodeGenTool.Abstractions;

namespace FluentCRUD.Tool.Example.Generators;

public class ProxyGenerator: IGenerationStep
{
	public GenerationContext Execute(GenerationContext context)
	{
		var code = $"namespace {context.Namespace} \n public static class {context.TypeName}Meta {{ public const string Name = \"{context.TypeName}\"; }}";
		context.AddFile($"{context.TypeName}Proxy",code);
		return context;
	}
}