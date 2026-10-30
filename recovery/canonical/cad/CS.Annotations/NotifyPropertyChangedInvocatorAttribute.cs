using System;

namespace CS.Annotations;

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
