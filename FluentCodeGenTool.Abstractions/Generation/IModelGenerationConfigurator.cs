using FluentCRUD.Abstraction;

namespace FluentCodeGenTool.Abstractions;

public interface IModelGenerationConfigurator
{
	public void Configuration(IGenerationPipeline builder);
}