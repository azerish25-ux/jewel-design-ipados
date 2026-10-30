using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Cutters;

[CSBlock("AzureCutter", "{9244180E-5132-4641-9E08-3604B66EA2A5}")]
public sealed class CSAzureCutter : CSBlock
{
	public CSInputChannel iGem { get; }

	public CSInputChannel iProfile { get; }

	public CSInputChannel iRotation { get; }

	public CSInputChannel iGirdleSpacing { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iTopX { get; }

	public CSInputChannel iTopZ { get; }

	public CSInputChannel iSeatX { get; }

	public CSInputChannel iSeatZ { get; }

	public CSInputChannel iBottomX { get; }

	public CSInputChannel iBottomZ { get; }

	public CSInputChannel iProfileBottom { get; }

	public CSOutputChannel oBrep { get; }

	public CSAzureCutter(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(GetPropertyName(() => iGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRotation = new CSInputChannel(GetPropertyName(() => iRotation), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGirdleSpacing = new CSInputChannel(GetPropertyName(() => iGirdleSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTopX = new CSInputChannel(GetPropertyName(() => iTopX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTopZ = new CSInputChannel(GetPropertyName(() => iTopZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSeatX = new CSInputChannel(GetPropertyName(() => iSeatX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSeatZ = new CSInputChannel(GetPropertyName(() => iSeatZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBottomX = new CSInputChannel(GetPropertyName(() => iBottomX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBottomZ = new CSInputChannel(GetPropertyName(() => iBottomZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfileBottom = new CSInputChannel(GetPropertyName(() => iProfileBottom), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		CSDataArray flattenArray = iGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Gems");
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
		double value9 = 0.0;
		iRotation.GetDouble(ref value9);
		OnNurbsCurve nurbsCurve = iProfile.GetNurbsCurve();
		OnNurbsCurve val = null;
		if (iProfileBottom != null)
		{
			val = iProfileBottom.GetNurbsCurve();
		}
		if (val == null && nurbsCurve != null)
		{
			val = nurbsCurve.Duplicate();
		}
		OnBoundingBox val2 = new OnBoundingBox();
		OnBoundingBox val3 = new OnBoundingBox();
		if (nurbsCurve != null && !((OnGeometry)nurbsCurve).GetTightBoundingBox(ref val2))
		{
			return UpdateError("Profile Bounding Box");
		}
		if (val != null && !((OnGeometry)val).GetTightBoundingBox(ref val3))
		{
			return UpdateError("Profile Bottom Bounding Box");
		}
		OnXform val4 = new OnXform();
		if (nurbsCurve != null)
		{
			val4.Scale(1.0 / val2.Max().x, 1.0 / val2.Max().y, 1.0);
		}
		OnXform val5 = new OnXform();
		val5.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value9), OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform val6 = new OnXform();
		if (val != null)
		{
			val6.Scale(1.0 / val3.Max().x, 1.0 / val3.Max().y, 1.0);
		}
		if (nurbsCurve != null)
		{
			nurbsCurve.Transform((IOnXform)(object)(val5 * (IOnXform)(object)val4));
		}
		if (val != null)
		{
			val.Transform((IOnXform)(object)(val5 * (IOnXform)(object)val6));
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				continue;
			}
			OnNurbsCurve val7 = nurbsCurve;
			OnNurbsCurve val8 = val;
			CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)cSGemData.Xform.Convert(), out var xUp, out var xDown);
			if (val7 == null)
			{
				val7 = cSGemData.GetNurbsCurve().Convert();
				if (val7 == null)
				{
					continue;
				}
				val7.Transform((IOnXform)(object)xDown);
				((OnGeometry)val7).GetTightBoundingBox(ref val2);
				val4.Scale(1.0 / val2.Max().x, 1.0 / val2.Max().y, 1.0);
				val7.Transform((IOnXform)(object)(val5 * (IOnXform)(object)val4));
			}
			if (val8 == null)
			{
				val8 = val7.Duplicate();
				((OnGeometry)val8).GetTightBoundingBox(ref val3);
				val6.Scale(1.0 / val3.Max().x, 1.0 / val3.Max().y, 1.0);
				val8.Transform((IOnXform)(object)(val5 * (IOnXform)(object)val6));
			}
			double num = Math.Min(cSGemData.GemXMax, cSGemData.GemYMax);
			OnNurbsCurve val9 = val7.Duplicate();
			((OnGeometry)val9).Scale(num);
			OnNurbsCurve val10 = val8.Duplicate();
			((OnGeometry)val10).Scale(num);
			double num2 = (value + num) / num;
			if (num2 <= 0.01)
			{
				num2 = 0.01;
			}
			if (!OffsetZ(val9, num2, value2, out var outputCurve))
			{
				return UpdateError("OffsetZ Failed");
			}
			double num3 = (value + value3 + num) / num;
			if (num3 <= 0.01)
			{
				num3 = 0.01;
			}
			if (!OffsetZ(val9, num3, value2 + value4, out var outputCurve2))
			{
				return UpdateError("OffsetZ Failed");
			}
			double num4 = (value + value5 + num) / num;
			if (num4 <= 0.01)
			{
				num4 = 0.01;
			}
			if (!OffsetZ(val10, num4, value2 + value6, out var outputCurve3))
			{
				return UpdateError("OffsetZ Failed");
			}
			double num5 = (value + value5 + value7 + num) / num;
			if (num5 <= 0.01)
			{
				num5 = 0.01;
			}
			if (!OffsetZ(val10, num5, value2 + value6 + value8, out var outputCurve4))
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
			OnBrep val11 = CSUtilSweep.LoftMatchCurves(profiles, (eLoftType)3, capped: true);
			((OnGeometry)val11).Transform((IOnXform)(object)xUp);
			cSDataArray.Append(new CSOnGeometryData(val11.Convert(), xUp.Convert(), applyTransform: false));
		}
		oBrep.SetData(cSDataArray);
		return base.Update();
	}

	private static bool OffsetZ(OnNurbsCurve inputCurve, double scale, double placement, out OnNurbsCurve outputCurve)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		OnXform val = new OnXform();
		val.Scale(scale, scale, 1.0);
		OnXform val2 = new OnXform();
		val2.Translation(0.0, 0.0, placement);
		outputCurve = inputCurve.Duplicate();
		outputCurve.Transform((IOnXform)(object)(val2 * (IOnXform)(object)val));
		return true;
	}
}
