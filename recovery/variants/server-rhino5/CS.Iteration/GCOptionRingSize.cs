using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using CS.CSObjects;
using CS.CSValues;
using Newtonsoft.Json;

namespace CS.Iteration;

[Serializable]
public sealed class GCOptionRingSize : GCOption
{
	[XmlIgnore]
	[JsonIgnore]
	public CSValueRingSize size;

	public List<double> ringDiameterMM = new List<double>();

	public double orginalValue;

	public GCOptionRingSize()
	{
	}

	public GCOptionRingSize(CSValueRingSize s)
		: base(s)
	{
		size = s;
		Id = 39;
		StoreOrginalFromObject();
	}

	public bool AddRingSize(double diameterMM)
	{
		if (ContainsRingSize(diameterMM))
		{
			return false;
		}
		ringDiameterMM.Add(diameterMM);
		return true;
	}

	public bool ContainsRingSize(double diameterMM)
	{
		foreach (double item in ringDiameterMM)
		{
			if (Math.Abs(diameterMM - item) < 0.0001)
			{
				return true;
			}
		}
		return false;
	}

	public bool ContainsRingSize(double diameterMM, ref double outDiameterMM)
	{
		foreach (double item in ringDiameterMM)
		{
			if (Math.Abs(diameterMM - item) < 0.0001)
			{
				outDiameterMM = item;
				return true;
			}
		}
		return false;
	}

	public string GeCurrentValueString(int index)
	{
		if (index < 0 || index >= ringDiameterMM.Count)
		{
			return "";
		}
		return string.Format(CultureInfo.InvariantCulture, "RingDiameterMM:{0:R}", new object[1] { ringDiameterMM[index] });
	}

	public override int GetIterationCount()
	{
		return ringDiameterMM.Count;
	}

	public bool RemoveRingSize(double diameterMM)
	{
		double outDiameterMM = 0.0;
		if (ContainsRingSize(diameterMM, ref outDiameterMM))
		{
			ringDiameterMM.Remove(outDiameterMM);
			return true;
		}
		return false;
	}

	public override bool RestoreOrginalToObject()
	{
		if (size == null)
		{
			return false;
		}
		size.SetValue(orginalValue / 2.0);
		return true;
	}

	public override bool SetFromIteration(int index)
	{
		if (index < 0 || index >= ringDiameterMM.Count)
		{
			return false;
		}
		if (size == null)
		{
			return false;
		}
		CurrentValueString = GeCurrentValueString(index);
		size.SetValue(ringDiameterMM[index] / 2.0);
		return true;
	}

	public override void SetGuid(Dictionary<Guid, CSObject> guidDictionary)
	{
		base.SetGuid(guidDictionary);
		size = obj as CSValueRingSize;
	}

	public override bool StoreOrginalFromObject()
	{
		if (size == null)
		{
			return false;
		}
		orginalValue = size.Value * 2.0;
		return true;
	}
}
