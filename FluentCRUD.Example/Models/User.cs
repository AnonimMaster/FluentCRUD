using FluentCRUD.Abstractions;
using FluentCRUD.Example.Extensions;

namespace FluentCRUD.Example.Models;

public class User
{
	public Guid Id { get; set; }
	public string Name { get; set; } = "";
	public string Password { get; set; } = "";
	
	internal class Map : IModelGenerationBuilder
	{
		public void Build(GenerationPipeline builder)
		{
			builder
				.AddStep(new ProxyGenerator())
				.WithNamespace("test");
		}
	}
}