using System.Linq.Expressions;
using System.Reflection;
using FluentCodeGenTool.Property;

namespace FluentCRUD.Abstraction;

public class EntityBuilder<TEntity>: IEntityBuilder<TEntity>
{
	private readonly Dictionary<string, PropertyBuilder> _properties = new();
	public string? Namespace { get; private set; }

	public IReadOnlyCollection<PropertyBuilder> Properties => _properties.Values;

	// Конструктор для автоматического добавления всех свойств
	public EntityBuilder()
	{
		// Добавляем все публичные свойства типа TEntity
		var entityType = typeof(TEntity);
		foreach (var property in entityType.GetProperties())
		{
			var propertyBuilder = new PropertyBuilder(property);
			_properties[property.Name] = propertyBuilder;
		}
	}
	
	public PropertyBuilder Property<TProp>(Expression<Func<TEntity, TProp>> expr)
	{
		var member = expr.Body is MemberExpression m ? m.Member
			: expr.Body is UnaryExpression u && u.Operand is MemberExpression m2 ? m2.Member
			: throw new InvalidOperationException("Invalid property expression");

		if (member is not PropertyInfo pi)
			throw new InvalidOperationException("Expression must select a property");

		if (!_properties.TryGetValue(pi.Name, out var builder))
		{
			builder = new PropertyBuilder(pi);
			_properties[pi.Name] = builder;
		}

		return builder;
	}

	public IEntityBuilder<TEntity> WithNamespace(string ns)
	{
		Namespace = ns;
		return this;
	}
}