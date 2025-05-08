using Microsoft.CodeAnalysis;

namespace FluentCRUD.Generator;

public class ModelInfo
{
	public string TypeName { get; set; } = "";
	public string Namespace { get; set; } = "";
	public List<IPropertySymbol> Properties { get; set; } = new List<IPropertySymbol>();
}