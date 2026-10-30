using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CS.CSObjects;
using Newtonsoft.Json;

namespace CS.Iteration;

[Serializable]
[XmlRoot("GCOption")]
[XmlInclude(typeof(GCOptionValue))]
[XmlInclude(typeof(GCOptionGemSizeAndShape))]
[XmlInclude(typeof(GCOptionRingSize))]
public abstract class GCOption
{
	public static readonly string UserDataGuidName = "GUID";

	public static readonly string UserDataType = "GCOption";

	[XmlIgnore]
	[JsonIgnore]
	public CSObject obj;

	public Guid guid = Guid.NewGuid();

	public int index = -1;

	public int Id;

	public string CurrentValueString = "None";

	public GCOption()
	{
	}

	public GCOption(CSObject o)
	{
		obj = o;
		if (obj == null)
		{
			return;
		}
		string text = obj.UserData.FindValue(UserDataType, UserDataGuidName);
		try
		{
			if (text != null && text.Length > 4)
			{
				guid = new Guid(text);
			}
		}
		catch (FormatException)
		{
		}
		obj.UserData.AddKey(UserDataType, UserDataGuidName, guid.ToString());
	}

	public abstract int GetIterationCount();

	public virtual bool RestoreOrginalToObject()
	{
		return true;
	}

	public abstract bool SetFromIteration(int index);

	public virtual void SetGuid(Dictionary<Guid, CSObject> guidDictionary)
	{
		if (guidDictionary.ContainsKey(guid))
		{
			obj = guidDictionary[guid];
		}
	}

	public virtual bool StoreOrginalFromObject()
	{
		return true;
	}

	public override string ToString()
	{
		return $"\n Option - Guid: {guid}, Id: {Id}, ValueString: {CurrentValueString}";
	}
}
