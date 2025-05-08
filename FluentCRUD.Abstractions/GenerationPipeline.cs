namespace FluentCRUD.Abstractions;

public class GenerationPipeline
{
	public List<ICrudPipelineStep> Steps { get; } = new();
	public string? Namespace { get; private set; }

	public GenerationPipeline AddStep(ICrudPipelineStep step)
	{
		Steps.Add(step);
		return this;
	}

	public GenerationPipeline WithNamespace(string ns)
	{
		Namespace = ns;
		return this;
	}
}
