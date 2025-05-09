using System.Linq.Expressions;

namespace FluentCRUD.Abstraction;

public interface IStepBuilder<TStep>
	where TStep : IGenerationStep
{
	IStepBuilder<TStep> WithOption(string key, object value);
	IPropertyBuilder Property<T>(Expression<Func<T, object>> propertyExpression);
}