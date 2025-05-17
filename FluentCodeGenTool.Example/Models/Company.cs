using FluentCodeGenTool.Abstractions;
using FluentCRUD.Abstraction;
using FluentCRUD.Tool.Example.Generators;

namespace FluentCRUD.Tool.Example.Models;

public class Company
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public List<User> Users { get; set; }
	
	internal class Configurator: IModelGenerationConfigurator
	{
		public void Configuration(GenerationPipeline pipeline)
		{
			pipeline
				.Step<ProxyGenerator>(step => step.For<Company>(entity =>
				{
					entity.WithNamespace("FluentCodeGenTool.Example");
					entity.Property(a => a.Users).Type("UserProxy");
				}));
		}
	}
}