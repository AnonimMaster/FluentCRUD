using System;
using System.Collections.Generic;

namespace FluentCRUD.Abstraction;

public interface IStepContext
{
	public string Namespace { get; }
	public Type StepType { get; }
	public Type EntityType { get; }
	public IReadOnlyList<IPropertyBuilder> Properties { get; }
}