namespace FluentCRUD.Generator;

public class GenerationConfig
{
	public string? OverrideName { get; set; }
	public string TargetNamespace { get; set; } = "Generated";
	
	public ModelInfo ModelInfo { get; private set; }

	public GenerationConfig(ModelInfo modelInfo)
	{
		ModelInfo = modelInfo;
	}
}