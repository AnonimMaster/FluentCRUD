namespace FluentCRUD.Abstraction;

public interface IGenerationBuilder
{
	IGenerationBuilder Entity<T>(Action<IEntityBuilder<T>> configure);
	IGenerationBuilder AddStep<TStep>(Action<IStepBuilder<TStep>> configure)
		where TStep : class, IGenerationStep, new();
}