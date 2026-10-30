using System.Reflection;
using CS;
using CS.CSValues;
using CS.Parametric.Channels;
using CSBlocks.Transforms;
using CSBlocks.Utilities;

namespace GV.CSStudioPatchWire;

public static class PatchWire
{
	public static void CSMirror(CSOutputChannel channel, CSUICategory cat, ref int sortIndex)
	{
		CSValue cSValue = new CSValue("One", 1.0);
		CSMirror cSMirror = new CSMirror("Mirror");
		cSMirror.InObj.SetLink(channel);
		cSMirror.InY.SetLink(cSValue.OutValue);
		CSMirror cSMirror2 = new CSMirror("MirrorXY");
		cSMirror2.InObj.SetLink(channel);
		cSMirror2.InX.SetLink(cSValue.OutValue);
		cSMirror2.InY.SetLink(cSValue.OutValue);
		CSMirror cSMirror3 = new CSMirror("MirrorQuad");
		cSMirror3.InObj.SetLink(channel);
		cSMirror3.InDuplicate.SetLink(cSValue.OutValue);
		cSMirror3.InX.SetLink(cSValue.OutValue);
		cSMirror3.InY.SetLink(cSValue.OutValue);
		CSValueOnOff cSValueOnOff = new CSValueOnOff("Mirror Front and Back", onOffvalue: false)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueOnOff.Categories.Add(cat);
		cSValueOnOff.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueOnOff cSValueOnOff2 = new CSValueOnOff("Mirror Opposite Side", onOffvalue: false)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueOnOff2.Categories.Add(cat);
		cSValueOnOff2.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSValueOnOff cSValueOnOff3 = new CSValueOnOff("Mirror Quad", onOffvalue: false)
		{
			UIVisible = true,
			StudioVisible = true
		};
		cSValueOnOff3.Categories.Add(cat);
		cSValueOnOff3.UserData.AddKey("UI", "SortIndex", sortIndex++.ToString());
		CSIfElse cSIfElse = new CSIfElse("Mirror");
		cSIfElse.InCondition.SetLink(cSValueOnOff.OutValue);
		cSIfElse.InIfConditionTrue.SetLink(cSMirror.OutObj);
		CSUtil.AddToDisplayAndGenerate(cSIfElse);
		CSIfElse cSIfElse2 = new CSIfElse("MirrorXY");
		cSIfElse2.InCondition.SetLink(cSValueOnOff2.OutValue);
		cSIfElse2.InIfConditionTrue.SetLink(cSMirror2.OutObj);
		CSUtil.AddToDisplayAndGenerate(cSIfElse2);
		CSIfElse cSIfElse3 = new CSIfElse("MirrorQuad");
		cSIfElse3.InCondition.SetLink(cSValueOnOff3.OutValue);
		cSIfElse3.InIfConditionTrue.SetLink(cSMirror3.OutObj);
		CSUtil.AddToDisplayAndGenerate(cSIfElse3);
	}

	public static Assembly GetCurrentAssembly()
	{
		return Assembly.GetExecutingAssembly();
	}
}
