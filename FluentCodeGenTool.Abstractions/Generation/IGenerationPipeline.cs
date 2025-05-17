using System;
using FluentCRUD.Abstraction;

namespace FluentCodeGenTool.Abstractions;

public interface IGenerationPipeline
{
	IGenerationPipeline Step<TStep>(Action<IStepBuilder<TStep>> configure)
		where TStep : IGenerationStep, new();

	IGenerationContext ExecuteAll(IGenerationContext context);
}