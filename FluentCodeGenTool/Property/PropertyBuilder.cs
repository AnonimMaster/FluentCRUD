using System.Reflection;

namespace FluentCodeGenTool.Property;

public class PropertyBuilder
{
	public PropertyInfo PropertyInfo { get; }
	public string? CustomName { get; private set; }
	public string? CustomType { get; private set; }
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

	public PropertyBuilder Type(Type type)
	{
		CustomType = type.Name;
		return this;
	}
	
	public PropertyBuilder Type(string type)
	{
		CustomType = type;
		return this;
	}

	public PropertyBuilder Ignore(bool shouldIgnore = true)
	{
		Ignored = shouldIgnore;
		return this;
	}
}