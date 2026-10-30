using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CSGemInstance.GemTableData;

public static class GemTableHelper
{
	private static readonly List<IGemTable> _gemList;

	static GemTableHelper()
	{
		_gemList = new List<IGemTable>();
		Type[] types = Assembly.GetExecutingAssembly().GetTypes();
		foreach (Type type in types)
		{
			if (type == null)
			{
				continue;
			}
			object[] customAttributes = type.GetCustomAttributes(inherit: false);
			for (int j = 0; j < customAttributes.Length; j++)
			{
				if (customAttributes[j] is CSGem)
				{
					ConstructorInfo constructorInfo = type.GetConstructors()[0];
					if (constructorInfo == null)
					{
						throw new Exception("Missing Default Constructor");
					}
					_gemList.Add((IGemTable)constructorInfo.Invoke(null));
				}
			}
		}
	}

	public static int GetCurrentGemTableVersion(string type, string shape)
	{
		GemTableType valueFromDescription = GetValueFromDescription(type, GemTableType.Unknown);
		GemTableShape valueFromDescription2 = GetValueFromDescription(shape, GemTableShape.Unknown);
		return GetCurrentGemTableVersion(valueFromDescription, valueFromDescription2);
	}

	public static int GetCurrentGemTableVersion(GemTableType gemTableType, GemTableShape gemTableShape)
	{
		return GetGem(gemTableType, gemTableShape)?.CurrentVersion ?? 0;
	}

	public static string GetDescriptionFromValue<T>(T value)
	{
		Type typeFromHandle = typeof(T);
		if (!typeFromHandle.IsEnum)
		{
			throw new InvalidOperationException();
		}
		FieldInfo field = typeFromHandle.GetField(value.ToString());
		if (field != null && Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute descriptionAttribute)
		{
			return descriptionAttribute.Description;
		}
		return value.ToString();
	}

	public static double GetFudgeFactor(string type, string shape)
	{
		GemTableType valueFromDescription = GetValueFromDescription(type, GemTableType.Unknown);
		GemTableShape valueFromDescription2 = GetValueFromDescription(shape, GemTableShape.Unknown);
		return GetFudgeFactor(valueFromDescription, valueFromDescription2);
	}

	public static double GetFudgeFactor(GemTableType gemTableType, GemTableShape gemTableShape)
	{
		double result = 0.0;
		if (gemTableType != GemTableType.Unknown && gemTableShape != GemTableShape.Unknown)
		{
			IGemTable gem = GetGem(gemTableType, gemTableShape);
			if (gem != null)
			{
				result = gem.FudgeFactor;
			}
		}
		return result;
	}

	public static IGemTable GetGem(string type, string shape)
	{
		GemTableType valueFromDescription = GetValueFromDescription(type, GemTableType.Unknown);
		GemTableShape valueFromDescription2 = GetValueFromDescription(shape, GemTableShape.Unknown);
		return GetGem(valueFromDescription, valueFromDescription2);
	}

	public static IGemTable GetGem(GemTableType gemTableType, GemTableShape gemTableShape)
	{
		return _gemList.FirstOrDefault((IGemTable gt) => gt.Shape == gemTableShape && gt.Type == gemTableType);
	}

	public static List<GemSize> GetGemSizes(string type, string shape, int version = 0)
	{
		GemTableType valueFromDescription = GetValueFromDescription(type, GemTableType.Unknown);
		GemTableShape valueFromDescription2 = GetValueFromDescription(shape, GemTableShape.Unknown);
		return GetGemSizes(valueFromDescription, valueFromDescription2, version);
	}

	public static List<GemSize> GetGemSizes(GemTableType gemTableType, GemTableShape gemTableShape, int version = 0)
	{
		if (gemTableType == GemTableType.Unknown || gemTableShape == GemTableShape.Unknown)
		{
			return new List<GemSize>();
		}
		IGemTable gem = GetGem(gemTableType, gemTableShape);
		if (gem == null)
		{
			return new List<GemSize>();
		}
		return gem.SizeTable(version) ?? new List<GemSize>();
	}

	public static GemSize GetGemTableSizeForShapeTypeIndex(string type, string shape, int index)
	{
		GemTableType valueFromDescription = GetValueFromDescription(type, GemTableType.Unknown);
		GemTableShape valueFromDescription2 = GetValueFromDescription(shape, GemTableShape.Unknown);
		return GetGemTableSizeForShapeTypeIndex(valueFromDescription, valueFromDescription2, index);
	}

	public static GemSize GetGemTableSizeForShapeTypeIndex(GemTableType gemTableType, GemTableShape gemTableShape, int index)
	{
		List<GemSize> gemSizes = GetGemSizes(gemTableType, gemTableShape);
		GemSize result = null;
		if (gemSizes != null)
		{
			result = gemSizes.ElementAtOrDefault(index);
		}
		return result;
	}

	public static GemTableLayer GetLayer(string type, string shape)
	{
		GemTableType valueFromDescription = GetValueFromDescription(type, GemTableType.Unknown);
		GemTableShape valueFromDescription2 = GetValueFromDescription(shape, GemTableShape.Unknown);
		return GetLayer(valueFromDescription, valueFromDescription2);
	}

	public static GemTableLayer GetLayer(GemTableType gemTableType, GemTableShape gemTableShape)
	{
		GemTableLayer result = GemTableLayer.Unknown;
		if (gemTableType == GemTableType.Unknown || gemTableShape == GemTableShape.Unknown)
		{
			return result;
		}
		IGemTable gem = GetGem(gemTableType, gemTableShape);
		if (gem != null)
		{
			result = gem.Layer;
		}
		return result;
	}

	public static double GetRotationAngle(string type, string shape)
	{
		GemTableType valueFromDescription = GetValueFromDescription(type, GemTableType.Unknown);
		GemTableShape valueFromDescription2 = GetValueFromDescription(shape, GemTableShape.Unknown);
		return GetRotationAngle(valueFromDescription, valueFromDescription2);
	}

	public static double GetRotationAngle(GemTableType gemTableType, GemTableShape gemTableShape)
	{
		double result = 0.0;
		if (gemTableType != GemTableType.Unknown && gemTableShape != GemTableShape.Unknown)
		{
			IGemTable gem = GetGem(gemTableType, gemTableShape);
			if (gem != null)
			{
				result = gem.RotationAngle;
			}
		}
		return result;
	}

	public static T GetValueFromDescription<T>(string description, T retVal)
	{
		Type typeFromHandle = typeof(T);
		if (!typeFromHandle.IsEnum)
		{
			throw new InvalidOperationException();
		}
		FieldInfo[] fields = typeFromHandle.GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) is DescriptionAttribute descriptionAttribute)
			{
				if (descriptionAttribute.Description == description)
				{
					return (T)fieldInfo.GetValue(null);
				}
			}
			else if (fieldInfo.Name == description)
			{
				return (T)fieldInfo.GetValue(null);
			}
		}
		return retVal;
	}
}
