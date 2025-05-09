namespace FluentCRUD.Abstraction;

public interface IGenerationStep
{
	void Generate(StepContext context);
}