namespace FluentCRUD.Abstractions;

public interface IModelGenerationBuilder
{
	public void Build(GenerationPipeline builder);
}