using FluentCodeGenTool;
using FluentCodeGenTool.Abstractions;
using FluentCRUD.Tool.Example.Generators;

namespace FluentCRUD.Tool.Example.Models;

public class User
{
	internal class Configurator: IModelGenerationConfigurator
	{
		public void Configuration(GenerationPipeline builder)
		{
			builder
				.WithNameSpace("FluentCRUD")
				.AddStep(new ProxyGenerator());
		}
	}
}