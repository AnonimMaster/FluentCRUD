using FluentCRUD.Generator;
using Microsoft.CodeAnalysis;

namespace FluentCRUD.Abstractions;

public interface ICrudPipelineStep
{
	string Generate(GenerationConfig config, SourceProductionContext context);
}