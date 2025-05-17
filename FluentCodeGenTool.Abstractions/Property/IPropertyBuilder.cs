using System;
using System.Reflection;

namespace FluentCRUD.Abstraction;

public interface IPropertyBuilder
{
	public PropertyInfo PropertyInfo { get; }
	public string? CustomName { get; }
	public string? CustomType { get; }
	public bool Ignored { get; }
	IPropertyBuilder HasName(string name);
	IPropertyBuilder Ignore(bool shouldIgnore = true);
	public IPropertyBuilder Type(Type type);
	public IPropertyBuilder Type(string type);
}