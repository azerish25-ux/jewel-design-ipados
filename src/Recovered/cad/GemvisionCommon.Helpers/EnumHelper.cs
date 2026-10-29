using System;
using System.ComponentModel;
using System.Reflection;

namespace GemvisionCommon.Helpers;

public static class EnumHelper
{
	public static string GetDescription(this Enum value)
	{
		FieldInfo field = value.GetType().GetField(value.ToString());
		if (field == null)
		{
			return null;
		}
		return (!(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute descriptionAttribute)) ? value.ToString() : descriptionAttribute.Description;
	}
}
