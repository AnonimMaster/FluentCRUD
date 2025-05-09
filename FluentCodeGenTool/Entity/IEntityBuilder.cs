using System.Linq.Expressions;
using FluentCodeGenTool.Property;

namespace FluentCRUD.Abstraction;

public interface IEntityBuilder<T>
{
	PropertyBuilder Property<TProperty>(Expression<Func<T, TProperty>> propertyExpression);
	IEntityBuilder<T> WithNamespace(string ns);
}