using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Cutters.Obsolete;

[Obsolete]
[CSBlock("GemCutter_V1", "{9A8A3F2B-8316-4c02-BD78-CAEABF56704A}")]
public sealed class CSGemCutter1 : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iGirdleSpacing { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iTopX { get; }

	public CSInputChannel iTopZ { get; }

	public CSInputChannel iSeatX { get; }

	public CSInputChannel iSeatZ { get; }

	public CSInputChannel iBottomX { get; }

	public CSInputChannel iBottomZ { get; }

	public CSOutputChannel oBrep { get; }

	public CSGemCutter1(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(GetPropertyName(() => iGirdleSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTopX = new CSInputChannel(GetPropertyName(() => iTopX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTopZ = new CSInputChannel(GetPropertyName(() => iTopZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSeatX = new CSInputChannel(GetPropertyName(() => iSeatX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSeatZ = new CSInputChannel(GetPropertyName(() => iSeatZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBottomX = new CSInputChannel(GetPropertyName(() => iBottomX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBottomZ = new CSInputChannel(GetPropertyName(() => iBottomZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		CSDataArray flattenArray = iGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gem Cutter:  Missing Gem\n");
		}
		double value = 0.0;
		iGirdleSpacing.GetDouble(ref value);
		double value2 = 0.0;
		iPlacement.GetDouble(ref value2);
		double value3 = 0.0;
		iTopX.GetDouble(ref value3);
		double value4 = 0.0;
		iTopZ.GetDouble(ref value4);
		double value5 = 0.0;
		iSeatX.GetDouble(ref value5);
		double value6 = 0.0;
		iSeatZ.GetDouble(ref value6);
		double value7 = 0.0;
		iBottomX.GetDouble(ref value7);
		double value8 = 0.0;
		iBottomZ.GetDouble(ref value8);
		OnNurbsCurve outputCurve = new OnNurbsCurve();
		OnNurbsCurve outputCurve2 = new OnNurbsCurve();
		OnNurbsCurve outputCurve3 = new OnNurbsCurve();
		OnNurbsCurve outputCurve4 = new OnNurbsCurve();
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSGemData cSGemData = flattenArray.GetArrayItem(i) as CSGemData;
			OnXform xDown = null;
			OnXform xUp = null;
			if (cSGemData == null)
			{
				return UpdateError("Gem Data Missing");
			}
			CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)cSGemData.Xform.Convert(), out xUp, out xDown);
			OnNurbsCurve obj = cSGemData.GetNurbsCurve().Convert();
			obj.Transform((IOnXform)(object)xDown);
			double num = 0.0;
			double num2 = 0.0;
			num = 0.0 - cSGemData.GemXMax;
			num2 = 0.0 - cSGemData.GemYMax;
			if (value < num)
			{
				value = num + 0.01;
			}
			else if (value < num2)
			{
				value = num2 + 0.01;
			}
			if (!OffsetZ(obj, value, value2, ref outputCurve))
			{
				return UpdateError("OffsetZ Failed");
			}
			num = 0.0 - ((OnGeometry)outputCurve).BoundingBox().Max().x;
			num2 = 0.0 - ((OnGeometry)outputCurve).BoundingBox().Max().y;
			if (value3 < num)
			{
				value3 = num + 0.01;
			}
			else if (value3 < num2)
			{
				value3 = num2 + 0.01;
			}
			if (!OffsetZ(outputCurve, value3, value4, ref outputCurve2))
			{
				return UpdateError("OffsetZ Failed");
			}
			if (value5 < num)
			{
				value5 = num + 0.01;
			}
			else if (value5 < num2)
			{
				value5 = num2 + 0.01;
			}
			if (!OffsetZ(outputCurve, value5, value6, ref outputCurve3))
			{
				return UpdateError("OffsetZ Failed");
			}
			num = 0.0 - ((OnGeometry)outputCurve3).BoundingBox().Max().x;
			num2 = 0.0 - ((OnGeometry)outputCurve3).BoundingBox().Max().y;
			if (value7 < num)
			{
				value7 = num + 0.01;
			}
			else if (value7 < num2)
			{
				value7 = num2 + 0.01;
			}
			if (!OffsetZ(outputCurve3, value7, value8, ref outputCurve4))
			{
				return UpdateError("OffsetZ Failed");
			}
			IOnCurve[] profiles = (IOnCurve[])(object)new OnCurve[4]
			{
				(OnCurve)outputCurve2,
				(OnCurve)outputCurve,
				(OnCurve)outputCurve3,
				(OnCurve)outputCurve4
			};
			OnBrep val = CSUtilSweep.Loft(profiles, (eLoftType)3, capped: true);
			((OnGeometry)val).Transform((IOnXform)(object)xUp);
			cSDataArray.Append(new CSOnGeometryData(val.Convert(), xUp.Convert(), applyTransform: false));
		}
		oBrep.SetData(cSDataArray);
		return base.Update();
	}

	private static bool OffsetZ(OnNurbsCurve curveNC, double girdleSpacing, double placement, ref OnNurbsCurve outputCurve)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		OnCurve val = CSUtilGeometry.OriginOffsetXY((IOnCurve)(object)curveNC, girdleSpacing, closed: true);
		if (val == null)
		{
			CS.CSUtil.Print("Gem Cutter Offset Curve Unable to Create Output\n");
			return false;
		}
		val.GetNurbForm(ref outputCurve);
		OnXform val2 = new OnXform();
		val2.Translation(0.0, 0.0, placement);
		outputCurve.Transform((IOnXform)(object)val2);
		return true;
	}
}
