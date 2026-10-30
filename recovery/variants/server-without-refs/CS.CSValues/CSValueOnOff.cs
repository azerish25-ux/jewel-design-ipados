using System;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CS.CSValues;

[CSBlock("ValueOnOff", "{A5D6B280-42FE-4188-8FF0-9AC6CAED7831}")]
public sealed class CSValueOnOff : CSValue
{
	private bool _hasCreatedUndoTransaction;

	public CSOutputChannel OutGroupOnOff { get; }

	public CSOutputChannel OutOnOff { get; }

	public CSValueOnOff(string identifier)
		: base(identifier)
	{
		_previousValue = 1.0;
		_doubleValue = 1.0;
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutOnOff = new CSOutputChannel(GetPropertyName(() => OutOnOff), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutGroupOnOff = new CSOutputChannel(GetPropertyName(() => OutGroupOnOff), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
	}

	public CSValueOnOff(string identifier, bool onOffvalue)
		: base(identifier)
	{
		_doubleValue = (_previousValue = (onOffvalue ? 1.0 : 0.0));
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutOnOff = new CSOutputChannel(GetPropertyName(() => OutOnOff), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutGroupOnOff = new CSOutputChannel(GetPropertyName(() => OutGroupOnOff), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
	}

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count)
		{
			CSOutputChannel cSOutputChannel = base.OutputChannels[outputChannelIndex];
			if (cSOutputChannel == OutGroupOnOff)
			{
				CSData data = (IsOn() ? new CSNumberData(base.Value) : new CSNumberData(0.0));
				OutGroupOnOff.SetData(data);
				return OutGroupOnOff;
			}
			if (cSOutputChannel == OutOnOff)
			{
				CSData data2 = (IsOn() ? new CSNumberData(base.Value) : new CSData());
				OutOnOff.SetData(data2);
				return OutOnOff;
			}
		}
		return base.GetOutputChannel(outputChannelIndex);
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count)
		{
			CSOutputChannel cSOutputChannel = base.OutputChannels[outputChannelIndex];
			if (cSOutputChannel == OutGroupOnOff)
			{
				if (!IsOn())
				{
					return new CSNumberData(0.0);
				}
				return new CSNumberData(base.Value);
			}
			if (cSOutputChannel == OutOnOff)
			{
				if (!IsOn())
				{
					return new CSData();
				}
				return new CSNumberData(base.Value);
			}
		}
		return base.GetOutputChannelData(outputChannelIndex);
	}

	public void SetValueFromUI(double value, bool mouseDown)
	{
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			if (!_hasCreatedUndoTransaction)
			{
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetValueFromUI");
				PreChange("SetValueFromUI");
				_hasCreatedUndoTransaction = true;
			}
			if (mouseDown)
			{
				CSUtil.ToggleMouseDown(this);
				SetValue(value);
			}
			else
			{
				CSUtil.RemoveMouseDown();
				SetValue(value);
				CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetValueFromUI");
				_hasCreatedUndoTransaction = false;
			}
			CSApplicationHelper.Application.Document.Graph.Generate(mouseDown);
			CSUtil.Redraw();
			if (!mouseDown)
			{
				CSUtil.RedrawUI();
			}
		}
		catch (Exception ex)
		{
			CSUtil.Print(ex.Message + "\n");
			CSUtil.Print(ex.StackTrace + "\n");
		}
	}

	public void SetValueFromUI(bool value, string text, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetValueFromUI");
			PreChange("SetValueFromUI");
			SetValue(value ? 1.0 : 0.0);
			SetIdentifier(text);
			Change();
			base.StudioVisible = studioVisible;
			base.StudioVisibleAdvanced = studioVisibleAdvanced;
			base.StudioVisibleDesignOption = studioVisibleDesignOption;
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetValueFromUI");
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
		catch (Exception ex)
		{
			CSUtil.Print(ex.Message + "\n");
			CSUtil.Print(ex.StackTrace + "\n");
		}
	}
}
