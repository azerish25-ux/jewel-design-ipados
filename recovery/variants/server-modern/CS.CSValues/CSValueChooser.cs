using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSObjects.CSTreeObjects;
using CS.CSValues.Interfaces;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CS.CSValues;

[CSBlock("ValueChooser", "{6C85FE71-927A-4d28-9335-8F1EB8713979}")]
public sealed class CSValueChooser : CSValue, ICSChooser
{
	public readonly CSInputChannel InObjects;

	public readonly CSOutputChannel OutObject;

	private List<string> _overriddenPreviewImageInputs = new List<string>();

	public bool ShowSweepPoints
	{
		get
		{
			bool result = false;
			string text = base.UserData.FindValue("SweepPoints", "Show");
			if (text != null && text.Length > 1)
			{
				result = true;
			}
			return result;
		}
		set
		{
			base.UserData.RemoveKey("SweepPoints", "Show");
			if (value)
			{
				base.UserData.AddKey("SweepPoints", "Show", "True");
			}
		}
	}

	public CSValueChooser(string identifier)
		: base(identifier)
	{
		InObjects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any, ChannelFlag.NotRequired);
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public bool CopyCurrent()
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return false;
		}
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("CopyCurrent");
		bool result = chooserSource.CopySelected((int)base.Value);
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("CopyCurrent");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.RedrawUI();
		return result;
	}

	public void DeleteObjects(int[] indexes)
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return;
		}
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("DeleteObjects");
		Array.Sort(indexes);
		int num = indexes.Length - 1;
		while (num >= 0 && ChooserCount() != 1)
		{
			if (chooserSource.DeleteObject(indexes[num]) && base.Value >= (double)indexes[num])
			{
				double num2 = base.Value - 1.0;
				if (num2 < 0.0)
				{
					num2 = 0.0;
				}
				SetValue(num2);
			}
			num--;
		}
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("DeleteObjects");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
	}

	public CSData GetChooserObject(int index)
	{
		return GetChooserSource()?.GetOutputChannelData(index);
	}

	public string GetChooserObjectIdentifier(int index)
	{
		return GetChooserSource()?.GetOutputChannelIdentifier(index);
	}

	public bool IsInputChannelImageOverridden(int channel)
	{
		return _overriddenPreviewImageInputs.Contains(channel.ToString(CultureInfo.InvariantCulture));
	}

	public bool SetBitmapOfCurrent(Bitmap bitmap)
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return false;
		}
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetBitmapOfCurrent");
		bool num = chooserSource.SetBitmapToChannel((int)base.Value, bitmap);
		if (num)
		{
			_overriddenPreviewImageInputs.Add(base.Value.ToString(CultureInfo.InvariantCulture));
			string value = string.Join(",", _overriddenPreviewImageInputs);
			base.UserData.AddKey("InputImageOverrides", base.Identifier, value);
		}
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetBitmapOfCurrent");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.RedrawAllUI();
		return num;
	}

	public void SetFromUI(string title, bool editPoints, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		if (base.Identifier != title || ShowSweepPoints != editPoints || studioVisible != base.StudioVisible || studioVisibleAdvanced != base.StudioVisibleAdvanced || studioVisibleDesignOption != base.StudioVisibleDesignOption)
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetFromUI");
			PreChange("SetFromUI");
			ShowSweepPoints = editPoints;
			Change();
			base.Identifier = title;
			base.StudioVisible = studioVisible;
			base.StudioVisibleAdvanced = studioVisibleAdvanced;
			base.StudioVisibleDesignOption = studioVisibleDesignOption;
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetFromUI");
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
	}

	public void SetValueFromUI(double value)
	{
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetValueFromUI");
			SetValue(value);
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetValueFromUI");
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
		catch (Exception ex)
		{
			CSUtil.Print(ex.Message);
			CSUtil.Print(ex.StackTrace);
		}
	}

	public override bool AddObjectFromCurrentSelection()
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return false;
		}
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("AddObjectFromCurrentSelection");
		bool num = chooserSource.AddObjectFromCurrentSelection();
		if (num)
		{
			double num2 = chooserSource.OutputChannelCount - 1;
			if (num2 < 0.0)
			{
				num2 = 0.0;
			}
			SetValue(num2);
		}
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("AddObjectFromCurrentSelection");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
		return num;
	}

	public override bool AddObjectFromFile(string fileName)
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return false;
		}
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("AddObjectFromFile");
		bool num = chooserSource.AddObjectFromFile(fileName);
		if (num)
		{
			double num2 = chooserSource.OutputChannelCount - 1;
			if (num2 < 0.0)
			{
				num2 = 0.0;
			}
			SetValue(num2);
		}
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("AddObjectFromFile");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
		return num;
	}

	public override bool DeleteObject(int index)
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return false;
		}
		if (ChooserCount() == 1)
		{
			return false;
		}
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("DeleteObject");
		bool num = chooserSource.DeleteObject(index);
		if (num && base.Value >= (double)index)
		{
			double num2 = base.Value - 1.0;
			if (num2 < 0.0)
			{
				num2 = 0.0;
			}
			SetValue(num2);
		}
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("DeleteObject");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
		return num;
	}

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		if (!IsOn())
		{
			return null;
		}
		if (outputChannelIndex < 0 || outputChannelIndex >= base.OutputChannels.Count)
		{
			return base.GetOutputChannel(outputChannelIndex);
		}
		if (base.OutputChannels[outputChannelIndex] != OutObject)
		{
			return base.GetOutputChannel(outputChannelIndex);
		}
		CSBlock chooserSource = GetChooserSource();
		CSData data = ((chooserSource == null) ? new CSData() : chooserSource.GetOutputChannelData((int)base.Value));
		OutObject.SetData(data);
		return OutObject;
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		if (!IsOn())
		{
			return new CSData();
		}
		if (outputChannelIndex < 0 || outputChannelIndex >= base.OutputChannels.Count)
		{
			return base.GetOutputChannelData(outputChannelIndex);
		}
		if (base.OutputChannels[outputChannelIndex] != OutObject)
		{
			return base.GetOutputChannelData(outputChannelIndex);
		}
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource != null)
		{
			return chooserSource.GetOutputChannelData((int)base.Value);
		}
		return new CSData();
	}

	public override bool ReadJson(JObject jObject, CSParaGraph graph, int major, int minor)
	{
		bool result = base.ReadJson(jObject, graph, major, minor);
		string text = base.UserData.FindValue("InputImageOverrides", base.Identifier);
		if (text != null)
		{
			_overriddenPreviewImageInputs = new List<string>(text.Split(','));
		}
		return result;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		bool result = base.Read(ref archive, graph, major, minor);
		string text = base.UserData.FindValue("InputImageOverrides", base.Identifier);
		if (text != null)
		{
			_overriddenPreviewImageInputs = new List<string>(text.Split(','));
		}
		return result;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		GetType().IsInstanceOfType(setObject);
		return true;
	}

	public override bool Update()
	{
		if (!CSApplicationHelper.IsCounterSketchStudio)
		{
			return base.Update();
		}
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return base.Update();
		}
		for (int i = 0; i < chooserSource.OutputChannelCount; i++)
		{
			if (!chooserSource.GetOutputChannelData((int)base.Value).Disabled)
			{
				break;
			}
			SetValue((base.Value + 1.0) % (double)chooserSource.OutputChannelCount);
		}
		return base.Update();
	}

	public override bool UpdateGroupObject(int index, NurbsCurve curve, List<double> points)
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return false;
		}
		chooserSource.UpdateGroupObject(index, curve, points);
		return false;
	}

	public override bool UpdateGroupObjectFromUI(int which, NurbsCurve curve, List<double> points, bool mouseDown)
	{
		return base.UpdateGroupObjectFromUI(which, curve, points, mouseDown);
	}

	public int ChooserCount()
	{
		return GetChooserSource()?.OutputChannelCount ?? 0;
	}

	public bool DeleteUnused()
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return true;
		}
		int num = ChooserCount();
		int num2 = (int)base.Value;
		if (num > 1 && !base.StudioVisible && !base.StudioVisibleAdvanced && !base.StudioVisibleDesignOption && base.ParentPatchWire != null)
		{
			int num3 = CSApplicationHelper.Application.Document.Graph.GraphList.FindIndex((CSObject check) => base.ParentPatchWire.Equals(check));
			if (num3 < 0)
			{
				return false;
			}
			if (num3 == int.MaxValue)
			{
				return false;
			}
			int num4 = CSApplicationHelper.Application.Document.Graph.GraphList.Count - 1;
			if (num3 >= CSApplicationHelper.Application.Document.Graph.GraphList.Count)
			{
				return false;
			}
			for (int num5 = num3 + 1; num5 < CSApplicationHelper.Application.Document.Graph.GraphList.Count; num5++)
			{
				if (CSApplicationHelper.Application.Document.Graph.GraphList[num5] is CSPatchWireStart)
				{
					num4 = num5 - 1;
					break;
				}
			}
			CSObject outputChannelBlock = InObjects.OutputChannelBlock;
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<CSValueChooser> list3 = new List<CSValueChooser>();
			if (base.InOverride.OutputChannelBlock != null)
			{
				return false;
			}
			if (outputChannelBlock == null)
			{
				return false;
			}
			if (outputChannelBlock.ParentPatchWire != base.ParentPatchWire)
			{
				return false;
			}
			for (int num6 = num3 + 1; num6 < num4; num6++)
			{
				if (this == CSApplicationHelper.Application.Document.Graph.GraphList[num6] || !(CSApplicationHelper.Application.Document.Graph.GraphList[num6] is CSValueChooser))
				{
					continue;
				}
				CSValueChooser cSValueChooser = CSApplicationHelper.Application.Document.Graph.GraphList[num6] as CSValueChooser;
				if (cSValueChooser.InObjects.OutputChannelBlock == outputChannelBlock)
				{
					if (cSValueChooser.StudioVisible || cSValueChooser.StudioVisibleAdvanced || cSValueChooser.StudioVisibleDesignOption)
					{
						return false;
					}
					list3.Add(cSValueChooser);
					list.Add((int)cSValueChooser.Value);
				}
			}
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("DeleteUnused");
			if (list3.Count > 0)
			{
				list3.Add(this);
				list.Add((int)base.Value);
				list2.AddRange(list);
				list2.Sort();
				for (int num7 = ChooserCount() - 1; num7 >= 0; num7--)
				{
					if (!list.Contains(num7))
					{
						chooserSource.DeleteObject(num7);
					}
				}
				for (int num8 = 0; num8 < list2.Count - 1; num8++)
				{
					if (list2[num8] == list2[num8 + 1])
					{
						list2.RemoveAt(num8);
						num8--;
					}
				}
				for (int num9 = 0; num9 < list2.Count; num9++)
				{
					if (list2[num9] <= num9)
					{
						continue;
					}
					int num10 = list2[num9];
					for (int num11 = 0; num11 < list3.Count; num11++)
					{
						if ((int)list3[num11].Value == num10)
						{
							list3[num11].SetValue(num9);
						}
					}
				}
			}
			else
			{
				if (num2 != 0)
				{
					SetValue(0.0);
				}
				for (int num12 = ChooserCount() - 1; num12 >= 0; num12--)
				{
					if (num12 != num2)
					{
						chooserSource.DeleteObject(num12);
					}
				}
			}
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("DeleteUnused");
			return true;
		}
		return false;
	}

	public CSBlock GetChooserSource()
	{
		if (InObjects != null)
		{
			return InObjects.OutputChannelBlock;
		}
		return null;
	}

	public void SetChooserSource(CSBlock csBlock, bool isMaintainingUndo = true)
	{
		if (isMaintainingUndo)
		{
			PreChange("SetChooserSource");
		}
		InObjects.OutputChannelBlock = csBlock;
		InObjects.OutputChannelIndex = -1;
		if (isMaintainingUndo)
		{
			Change();
		}
	}
}
