using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CS.CSObjects;
using CS.CSValues;
using CS.CSValues.Interfaces;
using Newtonsoft.Json;

namespace CS.Iteration;

[Serializable]
public sealed class GCOptionValue : GCOption
{
	[XmlIgnore]
	[JsonIgnore]
	public CSValueBase _valueBase;

	public List<double> values = new List<double>();

	public double orginalValue;

	public GCOptionValue()
	{
	}

	public GCOptionValue(CSValueBase v)
		: base(v)
	{
		_valueBase = v;
		StoreOrginalFromObject();
	}

	public bool AddValue(double value)
	{
		if (values.Contains(value))
		{
			return false;
		}
		values.Add(value);
		return true;
	}

	public override int GetIterationCount()
	{
		return values.Count;
	}

	public override bool RestoreOrginalToObject()
	{
		if (!(_valueBase is ICSValue<double> iCSValue))
		{
			return false;
		}
		iCSValue.SetValue(orginalValue);
		return true;
	}

	public override bool SetFromIteration(int newIndex)
	{
		if (newIndex < 0 || newIndex >= values.Count)
		{
			return false;
		}
		if (!(_valueBase is ICSValue<double> iCSValue))
		{
			return false;
		}
		CurrentValueString = "Value:" + values[newIndex];
		return iCSValue.SetValue(values[newIndex]);
	}

	public override void SetGuid(Dictionary<Guid, CSObject> guidDictionary)
	{
		base.SetGuid(guidDictionary);
		_valueBase = obj as CSValueBase;
	}

	public override bool StoreOrginalFromObject()
	{
		if (!(_valueBase is ICSValue<double> iCSValue))
		{
			return false;
		}
		orginalValue = iCSValue.Value;
		return true;
	}
}
