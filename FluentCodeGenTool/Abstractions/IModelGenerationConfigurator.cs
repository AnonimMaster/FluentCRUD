namespace FluentCodeGenTool.Abstractions;

public interface IModelGenerationConfigurator
{
	public void Configuration(GenerationPipeline builder);
}