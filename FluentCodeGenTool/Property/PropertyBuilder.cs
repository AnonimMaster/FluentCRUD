using System.Reflection;
using FluentCRUD.Abstraction;

namespace FluentCodeGenTool.Property;

public class PropertyBuilder: IPropertyBuilder
{
	public PropertyInfo PropertyInfo { get; }
	public string? CustomName { get; private set; }
	public string? CustomType { get; private set; }
	public bool Ignored { get; private set; }

	public PropertyBuilder(PropertyInfo pi)
	{
		PropertyInfo = pi;
	}

	public IPropertyBuilder HasName(string name)
	{
		CustomName = name;
		return this;
	}

	public IPropertyBuilder Type(Type type)
	{
		CustomType = type.Name;
		return this;
	}
	
	public IPropertyBuilder Type(string type)
	{
		CustomType = type;
		return this;
	}

	public IPropertyBuilder Ignore(bool shouldIgnore = true)
	{
		Ignored = shouldIgnore;
		return this;
	}
}