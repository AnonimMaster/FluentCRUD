using FluentCodeGenTool.Abstractions;

namespace FluentCRUD.Abstraction;

public interface IGenerationStep
{
	IGenerationContext Generate(IStepContext context, IGenerationContext generationContext);
}