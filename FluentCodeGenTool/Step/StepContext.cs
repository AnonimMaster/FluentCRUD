using FluentCodeGenTool.Property;

namespace FluentCRUD.Abstraction;

public class StepContext: IStepContext
{
	public string Namespace { get; }
	public Type StepType { get; }
	public Type EntityType { get; }
	public IReadOnlyList<IPropertyBuilder> Properties { get; }

	public StepContext(Type stepType, Type entityType, string ns, IEnumerable<IPropertyBuilder> props)
	{
		Namespace = ns;
		StepType = stepType;
		EntityType = entityType;
		Properties = new List<IPropertyBuilder>(props);
	}
}