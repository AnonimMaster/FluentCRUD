using System.Reflection;

namespace FluentCRUD.Abstraction;

public class GenerationPipeline
{
	private readonly GenerationContext _baseContext;
	private readonly List<object> _stepBuilders = new();
	
	public GenerationPipeline(GenerationContext baseContext)
	{
		_baseContext = baseContext;
	}

	/// <summary>
	/// Регистрирует новый шаг генерации с конфигурацией.
	/// </summary>
	public GenerationPipeline Step<TStep>(Action<StepBuilder<TStep>> configure)
		where TStep : IGenerationStep, new()
	{
		var step = new TStep();
		var builder = new StepBuilder<TStep>(step);
		configure(builder);
		_stepBuilders.Add(builder);
		return this;
	}

	/// <summary>
	/// Запуск всех зарегистрированных шагов.
	/// </summary>
	public void ExecuteAll()
	{
		foreach (var obj in _stepBuilders)
		{
			var type = obj.GetType();
			// Получаем контексты для каждого шага и сущности
			var contexts =
				(IEnumerable<StepContext>)type.GetMethod("BuildContexts",
						BindingFlags.Instance | BindingFlags.NonPublic)!
					.Invoke(obj, null)!;

			// Получаем сам шаг
			var step = (IGenerationStep)type.GetProperty("Step", BindingFlags.Instance | BindingFlags.Public)!
				.GetValue(obj)!;

			// Выполняем шаг для каждой сущности
			foreach (var ctx in contexts)
			{
				step.Generate(ctx);
			}
		}
	}
}

