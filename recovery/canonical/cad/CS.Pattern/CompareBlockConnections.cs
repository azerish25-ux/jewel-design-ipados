using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSValues;
using CS.Parametric;
using CS.Parametric.Channels;
using CSMaterials;

namespace CS.Pattern;

[Serializable]
[XmlRoot("CompareBlockData")]
public sealed class CompareBlockConnections
{
	public Guid guid;

	public List<CompareChannelConnections> inputs = new List<CompareChannelConnections>();

	public string name;

	[XmlIgnore]
	public CSUserData userData = new CSUserData();

	public List<string> userDataString;

	public CompareBlockConnections()
	{
	}

	public CompareBlockConnections(CSObject obj, CSBlockList blockList, int start, int end)
	{
		name = obj.Identifier + ":" + obj.GetType().Name;
		CSBlockDescription cSBlockDescription = blockList.FindBlockFrom(obj);
		if (cSBlockDescription != null)
		{
			guid = cSBlockDescription.Guid;
		}
		if (obj is CSBlock cSBlock)
		{
			for (int i = 0; i < cSBlock.InputChannelCount; i++)
			{
				CSInputChannel inputChannel = cSBlock.GetInputChannel(i);
				if (inputChannel != null && inputChannel.OutputChannelBlock != null && inputChannel.OutputChannelBlock.TableIndex >= start && inputChannel.OutputChannelBlock.TableIndex <= end)
				{
					inputs.Add(new CompareChannelConnections(i, inputChannel.OutputChannelBlock.TableIndex - cSBlock.TableIndex, inputChannel.OutputChannelIndex));
				}
			}
		}
		userData = new CSUserData();
		if (obj.StudioVisible || obj.StudioVisibleAdvanced || obj.StudioVisibleDesignOption)
		{
			if (obj is CSValueBase cSValueBase)
			{
				string text = "";
				foreach (CSUICategory category in cSValueBase.Categories)
				{
					text = text + category.CategoryName + ",";
				}
				obj.UserData.AddKeyNoReplace("CategoryNames", text, " ");
				List<UserDataElement> list = new List<UserDataElement>();
				foreach (UserDataElement item in cSValueBase.UserData.ElementsThatMatchType("Spec"))
				{
					string key = item.Key;
					string value = item.Value;
					string type = item.Type;
					list.Add(new UserDataElement(type + "Cat", key, value + " Cat:" + text));
				}
				foreach (UserDataElement item2 in list)
				{
					userData.AddKeyNoReplace(item2.Type, item2.Key, item2.Value);
				}
				if (cSValueBase is CSValueRange cSValueRange)
				{
					userData.AddKeyNoReplace("Min", cSValueRange.MinValue.ToString(), " ");
					userData.AddKeyNoReplace("Max", cSValueRange.MaxValue.ToString(), " ");
					userData.AddKeyNoReplace("Step", cSValueRange.Step.ToString(), " ");
				}
			}
			userData.Add(obj.UserData);
			return;
		}
		if (obj is CSValueBase cSValueBase2)
		{
			List<UserDataElement> list2 = new List<UserDataElement>();
			foreach (UserDataElement item3 in cSValueBase2.UserData.ElementsThatMatchType("Spec"))
			{
				string key2 = item3.Key;
				string value2 = item3.Value;
				string type2 = item3.Type;
				list2.Add(new UserDataElement(type2 + "NotInCS", key2, value2));
			}
			foreach (UserDataElement item4 in list2)
			{
				userData.AddKeyNoReplace(item4.Type, item4.Key, item4.Value);
			}
			if (cSValueBase2 is CSValueRange cSValueRange2)
			{
				userData.AddKeyNoReplace("Min", cSValueRange2.MinValue.ToString(), " ");
				userData.AddKeyNoReplace("Max", cSValueRange2.MaxValue.ToString(), " ");
				userData.AddKeyNoReplace("Step", cSValueRange2.Step.ToString(), " ");
			}
		}
		if (obj.OutputDisplayMaterialClassification != MaterialClassification.UseOriginal)
		{
			userData.AddKeyNoReplace("MaterialUsed", obj.OutputDisplayMaterialClassification.ToString(), " ");
		}
		if (obj.ForceHide)
		{
			userData.AddKeyNoReplace("Display", "ForceHide", " ");
		}
		if (obj.ForceVisible)
		{
			userData.AddKeyNoReplace("Display", "ForceVisible", " ");
		}
		if (obj.InDisplay)
		{
			userData.AddKeyNoReplace("Display", "InDisplay", " ");
		}
		userData.Add(obj.UserData);
	}

	public void ConvertUserDataToStrings()
	{
		userDataString = new List<string>();
		foreach (UserDataElement item in userData.AllKeys())
		{
			userDataString.Add(item.Type + ":" + item.Key + ":" + item.Value);
		}
		userDataString.Sort();
	}

	public override bool Equals(object obj)
	{
		CompareBlockConnections compareBlockConnections = obj as CompareBlockConnections;
		if (!compareBlockConnections.guid.Equals(guid))
		{
			return false;
		}
		if (compareBlockConnections.inputs.Count != inputs.Count)
		{
			return false;
		}
		for (int i = 0; i < compareBlockConnections.inputs.Count; i++)
		{
			if (!inputs[i].Equals(compareBlockConnections.inputs[i]))
			{
				return false;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		int hashCode = guid.GetHashCode();
		hashCode ^= inputs.Count;
		for (int i = 0; i < inputs.Count; i++)
		{
			hashCode ^= inputs[i].blockInputNumber;
			hashCode ^= inputs[i].channelObjOffset;
			hashCode ^= inputs[i].channelOutput;
		}
		return hashCode;
	}
}
