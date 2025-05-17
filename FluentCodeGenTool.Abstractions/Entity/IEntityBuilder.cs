using System;
using System.Linq.Expressions;

namespace FluentCRUD.Abstraction;

public interface IEntityBuilder<T>
{
	IPropertyBuilder Property<TProperty>(Expression<Func<T, TProperty>> propertyExpression);
	IEntityBuilder<T> WithNamespace(string ns);
}