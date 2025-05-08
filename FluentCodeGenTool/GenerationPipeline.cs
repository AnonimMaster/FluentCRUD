using System.Text;
using FluentCodeGenTool.Abstractions;

namespace FluentCodeGenTool;

public class GenerationPipeline
{
	private GenerationContext _context;

	public GenerationPipeline(GenerationContext context)
	{
		_context = context;
	}

	public List<IGenerationStep> Steps { get; } = new();

	public GenerationPipeline WithNameSpace(string name)
	{
		_context.Namespace = name;
		return this;
	}

	public GenerationPipeline AddStep(IGenerationStep step)
	{
		Steps.Add(step);
		return this;
	}

	public GenerationContext Build()
	{
		foreach (var step in Steps)
		{
			_context = step.Execute(_context);
		}

		return _context;
	}
}
