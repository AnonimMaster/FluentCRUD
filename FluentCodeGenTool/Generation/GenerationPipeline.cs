using System.Reflection;
using FluentCodeGenTool.Abstractions;

namespace FluentCRUD.Abstraction;

public class GenerationPipeline: IGenerationPipeline
{
	private readonly IGenerationContext _baseContext;
	private readonly List<object> _stepBuilders = new();
	
	public GenerationPipeline(IGenerationContext baseContext)
	{
		_baseContext = baseContext;
	}

	/// <summary>
	/// Регистрирует новый шаг генерации с конфигурацией.
	/// </summary>
	public IGenerationPipeline Step<TStep>(Action<IStepBuilder<TStep>> configure)
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
	public IGenerationContext ExecuteAll(IGenerationContext context)
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
				context = step.Generate(ctx, context);
			}
		}

		return context;
	}
}

