using System;

namespace FluentCRUD.Abstraction;

public interface IStepBuilder<TStep> where TStep : IGenerationStep, new()
{
	public IStepBuilder<TStep> For<TEntity>(Action<IEntityBuilder<TEntity>> configure);
}