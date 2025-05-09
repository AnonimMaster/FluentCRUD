using System.Linq.Expressions;

namespace FluentCRUD.Abstraction;

public interface IEntityBuilder<T>
{
	PropertyBuilder Property<TProperty>(Expression<Func<T, TProperty>> propertyExpression);
	IEntityBuilder<T> WithNamespace(string ns);
}