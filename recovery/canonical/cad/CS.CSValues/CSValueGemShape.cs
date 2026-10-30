using System;
using System.Collections.Generic;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.CSObjects.CSTreeObjects;
using CS.CSValues.Interfaces;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CS.CSValues;

[CSBlock("ValueGemShape", "{4A68C4D5-EBBD-4C24-86CE-35DB057E0CDF}")]
public sealed class CSValueGemShape : CSValue, ICSChooser
{
	public readonly CSInputChannel InGems;

	public readonly CSInputChannel InOnOff;

	public readonly CSOutputChannel OutGem;

	public GemSettingType MountingType
	{
		get
		{
			string value = base.UserData.FindValue("CSValueGem", "Mounting");
			if (string.IsNullOrWhiteSpace(value))
			{
				return GemSettingType.XX;
			}
			if (!Enum.TryParse<GemSettingType>(value, out var result))
			{
				return GemSettingType.XX;
			}
			return result;
		}
		set
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			Change();
			base.UserData.AddKey("CSValueGem", "Mounting", value.ToString());
			CSApplicationHelper.Application.Document.Graph.Generate();
		}
	}

	public string GemShape => GetCurrentCSGemData()?.GemShape;

	public string GemType => GetCurrentCSGemData()?.GemType;

	public CSValueGemShape(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InOnOff = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		base.InIsUIDisabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public int GetSizeTableCount()
	{
		CSBlock chooserSource = GetChooserSource();
		if (chooserSource == null)
		{
			return 0;
		}
		CSGemData cSGemData = chooserSource.GetOutputChannelData((int)base.RealValue) as CSGemData;
		if (cSGemData?.Def == null)
		{
			return 0;
		}
		return cSGemData.Sizes?.Count ?? 0;
	}

	private CSGemData GetCurrentCSGemData()
	{
		return GetChooserSource()?.GetOutputChannelData((int)base.Value) as CSGemData;
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
			CSObject outputChannelBlock = InGems.OutputChannelBlock;
			List<CSValueGemShape> list = new List<CSValueGemShape>();
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
				if (this == CSApplicationHelper.Application.Document.Graph.GraphList[num6] || !(CSApplicationHelper.Application.Document.Graph.GraphList[num6] is CSValueGemShape))
				{
					continue;
				}
				CSValueGemShape cSValueGemShape = CSApplicationHelper.Application.Document.Graph.GraphList[num6] as CSValueGemShape;
				if (cSValueGemShape.InGems.OutputChannelBlock == outputChannelBlock)
				{
					if (cSValueGemShape.StudioVisible || cSValueGemShape.StudioVisibleAdvanced || cSValueGemShape.StudioVisibleDesignOption)
					{
						return false;
					}
					list.Add(cSValueGemShape);
				}
			}
			if (list.Count > 0)
			{
				return false;
			}
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("DeleteUnused");
			for (int num7 = ChooserCount() - 1; num7 >= 0; num7--)
			{
				if (num7 != num2)
				{
					chooserSource.DeleteObject(num7);
				}
			}
			if (num2 != 0)
			{
				SetValue(0.0);
			}
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("DeleteUnused");
			return true;
		}
		return false;
	}

	public CSBlock GetChooserSource()
	{
		return InGems.OutputChannelBlock;
	}

	public void SetChooserSource(CSBlock csBlock, bool isMaintainingUndo = true)
	{
		if (isMaintainingUndo)
		{
			PreChange("SetChooserSource");
		}
		InGems.OutputChannelBlock = csBlock;
		InGems.OutputChannelIndex = -1;
		if (isMaintainingUndo)
		{
			Change();
		}
	}

	public CSData GetChooserObject(int index)
	{
		return GetChooserSource()?.GetOutputChannelData(index);
	}

	public string GetChooserObjectIdentifier(int index)
	{
		return GetChooserSource()?.GetOutputChannelIdentifier(index);
	}

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count && base.OutputChannels[outputChannelIndex] == OutGem)
		{
			if (GetChooserSource() == null)
			{
				return null;
			}
			bool value = true;
			InOnOff.GetBoolean(ref value);
			if (!value)
			{
				OutGem.SetData(new CSData());
				return OutGem;
			}
			CSGemData currentCSGemData = GetCurrentCSGemData();
			if (currentCSGemData == null)
			{
				return null;
			}
			currentCSGemData.SettingType = MountingType;
			CSGemData cSGemData = new CSGemData(currentCSGemData);
			if (base.IsOutputDisplayMaterialClassificationAssigned)
			{
				cSGemData.DefaultDisplayMaterialClassification = base.OutputDisplayMaterialClassification;
			}
			OutGem.SetData(cSGemData);
			return OutGem;
		}
		return base.GetOutputChannel(outputChannelIndex);
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count && base.OutputChannels[outputChannelIndex] == OutGem)
		{
			if (GetChooserSource() == null)
			{
				return null;
			}
			bool value = true;
			InOnOff.GetBoolean(ref value);
			if (!value)
			{
				return new CSData();
			}
			CSGemData currentCSGemData = GetCurrentCSGemData();
			if (currentCSGemData == null)
			{
				return null;
			}
			currentCSGemData.SettingType = MountingType;
			CSGemData cSGemData = new CSGemData(currentCSGemData);
			if (base.IsOutputDisplayMaterialClassificationAssigned)
			{
				cSGemData.DefaultDisplayMaterialClassification = base.OutputDisplayMaterialClassification;
			}
			return cSGemData;
		}
		return base.GetOutputChannelData(outputChannelIndex);
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
		if (!chooserSource.DeleteObject(index))
		{
			return false;
		}
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("DeleteObject");
		double num = base.Value - 1.0;
		if (num < 0.0)
		{
			num = 0.0;
		}
		SetValue(num);
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("DeleteObject");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
		return true;
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

	public void SetInfoFromUI(string title, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetInfoFromUI");
		base.Identifier = title;
		base.StudioVisible = studioVisible;
		base.StudioVisibleAdvanced = studioVisibleAdvanced;
		base.StudioVisibleDesignOption = studioVisibleDesignOption;
		CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetInfoFromUI");
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
	}

	public void SetValueFromUI(double value)
	{
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetValueFromUI");
			if (Math.Abs(value - base.RealValue) > CSApplicationHelper.Application.Tolerance)
			{
				PreChange("SetValueFromUI");
				_previousValue = value;
				_doubleValue = value;
				Change();
			}
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

	public override bool SetFileValue(CSObject setObject)
	{
		base.SetFileValue(setObject);
		return true;
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		return true;
	}
}
