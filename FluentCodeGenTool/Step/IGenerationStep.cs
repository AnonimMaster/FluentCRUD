namespace FluentCRUD.Abstraction;

public interface IGenerationStep
{
	GenerationContext Generate(StepContext context, GenerationContext generationContext);
}