using System;
using System.Collections.Generic;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CS.CSValues;

[CSBlock("ValueGemOutput", "{C15C2398-3C27-4F99-AFD3-081BE9E2EB1F}")]
public sealed class CSValueGemOutput : CSValueBase
{
	public List<CSGemData> Gems { get; } = new List<CSGemData>();

	public CSInputChannel InGems { get; }

	public override bool UIChanged
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public CSValueGemOutput(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(GetPropertyName(() => InGems), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual, ChannelFlag.NotRequired);
		base.InIsUIDisabled = new CSInputChannel(GetPropertyName(() => InIsUIDisabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
	}

	public void SetFromUI(string title, bool studioVisible, bool studioVisibleAdvanced, bool studioVisibleDesignOption)
	{
		try
		{
			if (title != base.Identifier || studioVisible != base.StudioVisible || studioVisibleAdvanced != base.StudioVisibleAdvanced || studioVisibleDesignOption != base.StudioVisibleDesignOption)
			{
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetFromUI");
				base.Identifier = title;
				base.StudioVisible = studioVisible;
				base.StudioVisibleAdvanced = studioVisibleAdvanced;
				base.StudioVisibleDesignOption = studioVisibleDesignOption;
				CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetFromUI");
				CSApplicationHelper.Application.Document.Graph.Generate();
				CSUtil.RedrawUI();
			}
		}
		catch (Exception ex)
		{
			CSUtil.Print(ex.Message + "\n");
			CSUtil.Print(ex.StackTrace + "\n");
		}
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InGems.GetFlattenArray();
		Gems.Clear();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (flattenArray.GetArrayItem(i) is CSGemData item)
			{
				Gems.Add(item);
			}
		}
		return base.Update();
	}
}
