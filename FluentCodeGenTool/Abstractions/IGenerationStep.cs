namespace FluentCodeGenTool.Abstractions;

public interface IGenerationStep
{
	GenerationContext Execute(GenerationContext context);
}