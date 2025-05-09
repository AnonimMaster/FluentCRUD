using System.Reflection;

namespace FluentCRUD.Abstraction;

public class PropertyBuilder
{
	public PropertyInfo PropertyInfo { get; }
	public string? CustomName { get; private set; }
	public bool Ignored { get; private set; }

	public PropertyBuilder(PropertyInfo pi)
	{
		PropertyInfo = pi;
	}

	public PropertyBuilder HasName(string name)
	{
		CustomName = name;
		return this;
	}

	public PropertyBuilder Ignore(bool shouldIgnore = true)
	{
		Ignored = shouldIgnore;
		return this;
	}
}