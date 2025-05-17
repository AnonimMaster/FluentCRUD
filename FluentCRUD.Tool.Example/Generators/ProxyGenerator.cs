using FluentCodeGenTool;
using FluentCRUD.Abstraction;
using Scriban;

namespace FluentCRUD.Tool.Example.Generators;

public class ProxyGenerator: IGenerationStep
{
	public GenerationContext Generate(StepContext context, GenerationContext generationContext)
	{
		var templateContent = File.ReadAllText("E:\\Rider Projects\\FluentCRUD\\FluentCRUD.Tool.Example\\Templates\\ProxyTemplate.scriban");
		var template = Template.Parse(templateContent);
		
		if (template.HasErrors)
		{
			throw new InvalidOperationException($"Template errors: {string.Join(", ", template.Messages.Select(m => m.Message))}");
		}
		
		var model = new
		{
			entity = new
			{
				name = context.EntityType.Name,
				properties = context.Properties
					.Where(p => !p.Ignored)
					.Select(p => new
					{
						name = p.CustomName ?? p.PropertyInfo.Name,
						type = p.CustomType ?? p.PropertyInfo.PropertyType.Name,
					}).ToList()
			},
			@namespace = context.Namespace
		};
		
		var result = template.Render(model);
		
		return generationContext.AddFile($"{context.EntityType.Name}Proxy",context.Namespace,result);
	}
}