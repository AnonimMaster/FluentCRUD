namespace FluentCRUD.Abstraction;

public interface IPropertyBuilder
{
	IPropertyBuilder HasName(string name);
	IPropertyBuilder Ignore(bool shouldIgnore = true);
}