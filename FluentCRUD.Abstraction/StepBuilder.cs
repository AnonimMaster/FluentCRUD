using System.Reflection;

namespace FluentCRUD.Abstraction;

public class StepBuilder<TStep> where TStep : IGenerationStep
{
	public TStep Step { get; }
	private readonly Dictionary<Type, object> _entities = new();

	public StepBuilder(TStep step)
	{
		Step = step;
	}

	/// <summary>
	/// Конфигурация генерации для сущности TEntity.
	/// </summary>
	public StepBuilder<TStep> For<TEntity>(Action<EntityBuilder<TEntity>> configure)
	{
		if (!_entities.TryGetValue(typeof(TEntity), out var obj))
		{
			obj = new EntityBuilder<TEntity>();
			_entities[typeof(TEntity)] = obj;
		}

		configure((EntityBuilder<TEntity>)obj);
		return this;
	}

	// Внутренний метод для создания контекстов выполнения
	internal IEnumerable<StepContext> BuildContexts()
	{
		var stepType = typeof(TStep);
		
		foreach (var kv in _entities)
		{
			var entityType = kv.Key;
			var builder = kv.Value;
			var props = (IEnumerable<PropertyBuilder>)builder
				.GetType()
				.GetProperty("Properties", BindingFlags.Instance | BindingFlags.Public)!
				.GetValue(builder)!;
			
			// Получаем Namespace из EntityBuilder через свойство Namespace
			// Предполагается, что у каждого builder есть публичное string? Namespace { get; }
			var nsProp = builder.GetType()
				             .GetProperty("Namespace", BindingFlags.Instance | BindingFlags.Public)
			             ?? throw new InvalidOperationException("EntityBuilder не содержит Namespace");
			var ns = (string?)nsProp.GetValue(builder);
			
			yield return new StepContext(
				stepType,
				entityType,
				ns,
				props
			);
		}
	}
}
