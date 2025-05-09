using FluentCodeGenTool.Property;

namespace FluentCRUD.Abstraction;

public class StepContext
{
	public string Namespace { get; }
	public Type StepType { get; }
	public Type EntityType { get; }
	public IReadOnlyList<PropertyBuilder> Properties { get; }

	public StepContext(Type stepType, Type entityType, string ns, IEnumerable<PropertyBuilder> props)
	{
		Namespace = ns;
		StepType = stepType;
		EntityType = entityType;
		Properties = new List<PropertyBuilder>(props);
	}
}