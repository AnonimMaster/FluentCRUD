using FluentCodeGenTool.Abstractions;
using FluentCRUD.Abstraction;
using FluentCRUD.Tool.Example.Generators;

namespace FluentCRUD.Tool.Example.Models;

public class User
{
	public Guid Id { get; set; }
	public string Name { get; set; } = "";
	public string Password { get; set; } = "";
	
	internal class Configurator: IModelGenerationConfigurator
	{
		public void Configuration(IGenerationPipeline pipeline)
		{
			pipeline
				.Step<ProxyGenerator>(step => step.For<User>(user =>
				{
					user.WithNamespace("TestNamespace");
				}));
		}
	}
}