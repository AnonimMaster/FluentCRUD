using FluentCRUD.Abstraction;

namespace FluentCRUD.Tool.Example.Generators;

public class ProxyGenerator: IGenerationStep
{
	public void Generate(StepContext context)
	{
		var code = $"namespace {context.Namespace} \n public static class {context.EntityType.Name}Meta {{ public const string Name = \"{context.EntityType.Name}\"; }}";
		context.AddFile($"{context.EntityType.Name}Proxy",code);
		return context;
	}
}