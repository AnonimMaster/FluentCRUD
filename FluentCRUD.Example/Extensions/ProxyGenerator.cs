using System.Text;
using FluentCRUD.Abstractions;
using FluentCRUD.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace FluentCRUD.Example.Extensions;

public class ProxyGenerator : ICrudPipelineStep
{
	public string Generate(GenerationConfig config, SourceProductionContext context)
	{
		var domainName = config.ModelInfo.TypeName;
		var proxyName = $"{domainName}Proxy";

		var sb = new StringBuilder();

		sb.AppendLine("namespace Generated.Proxies");
		sb.AppendLine("{");
		sb.AppendLine($"    public class {proxyName}");
		sb.AppendLine("    {");

		foreach (var member in config.ModelInfo.Properties)
		{
			sb.AppendLine($"        public {member.Type.ToDisplayString()} {member.Name} {{ get; set; }}");
		}

		sb.AppendLine("    }");
		sb.AppendLine("}");
		
		context.AddSource("Debug_Info", SourceText.From($"// Generated at {DateTime.Now}", Encoding.UTF8));

		return sb.ToString();
	}
}