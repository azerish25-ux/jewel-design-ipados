using System;

namespace GV.CounterSketch.Shared.Annotations;

[AttributeUsage(AttributeTargets.Method)]
public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
{
	public string ParameterName { get; private set; }

	public NotifyPropertyChangedInvocatorAttribute()
	{
	}

	public NotifyPropertyChangedInvocatorAttribute(string parameterName)
	{
		ParameterName = parameterName;
	}
}
