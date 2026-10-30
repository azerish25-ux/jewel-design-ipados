using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Builders;

[CSBlock("GemsInChannelOnRingRail", "{32E2B07E-7666-4D4C-833F-0368726C216E}")]
public sealed class CSGemsInChannelOnRingRail : CSBlock
{
	public CSInputChannel iInnerRail { get; }

	public CSInputChannel iOuterRail { get; }

	public CSInputChannel iGemDefinition { get; }

	public CSInputChannel iGemStart { get; }

	public CSInputChannel iGemEnd { get; }

	public CSInputChannel iGemSpacing { get; }

	public CSInputChannel iZOffsetPercent { get; }

	public CSInputChannel iChannelSpacing { get; }

	public CSInputChannel iChannelEndSpacing { get; }

	public CSInputChannel iShowChannel { get; }

	public CSInputChannel iOrientMethod { get; }

	public CSInputChannel iGemHeightOffset { get; }

	public CSInputChannel iSpacingMethod { get; }

	public CSInputChannel iGemRotation { get; }

	public CSOutputChannel oGems { get; }

	public CSOutputChannel oParamStart { get; }

	public CSOutputChannel oParamEnd { get; }

	public CSOutputChannel oParamFirstGem { get; }

	public CSOutputChannel oParamLastGem { get; }

	public CSOutputChannel oGemSizeVector { get; }

	public CSGemsInChannelOnRingRail(string identifier)
		: base(identifier)
	{
		iInnerRail = new CSInputChannel(GetPropertyName(() => iInnerRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOuterRail = new CSInputChannel(GetPropertyName(() => iOuterRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iGemDefinition = new CSInputChannel(GetPropertyName(() => iGemDefinition), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iGemStart = new CSInputChannel(GetPropertyName(() => iGemStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemEnd = new CSInputChannel(GetPropertyName(() => iGemEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemSpacing = new CSInputChannel(GetPropertyName(() => iGemSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZOffsetPercent = new CSInputChannel(GetPropertyName(() => iZOffsetPercent), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelSpacing = new CSInputChannel(GetPropertyName(() => iChannelSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelEndSpacing = new CSInputChannel(GetPropertyName(() => iChannelEndSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iShowChannel = new CSInputChannel(GetPropertyName(() => iShowChannel), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iOrientMethod = new CSInputChannel(GetPropertyName(() => iOrientMethod), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemHeightOffset = new CSInputChannel(GetPropertyName(() => iGemHeightOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSpacingMethod = new CSInputChannel(GetPropertyName(() => iSpacingMethod), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemRotation = new CSInputChannel(GetPropertyName(() => iGemRotation), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGems = new CSOutputChannel(GetPropertyName(() => oGems), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		oParamStart = new CSOutputChannel(GetPropertyName(() => oParamStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParamEnd = new CSOutputChannel(GetPropertyName(() => oParamEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParamFirstGem = new CSOutputChannel(GetPropertyName(() => oParamFirstGem), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParamLastGem = new CSOutputChannel(GetPropertyName(() => oParamLastGem), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oGemSizeVector = new CSOutputChannel(GetPropertyName(() => oGemSizeVector), this, ChannelDataType.Vector, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iInnerRail.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Curve");
		}
		OnCurve onCurve = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		if (onCurve == null)
		{
			return UpdateMissing("Curve");
		}
		CSDataArray flattenArray2 = iOuterRail.GetFlattenArray();
		if (flattenArray2 == null || flattenArray2.GetArrayCount() < 1)
		{
			return UpdateMissing("Curve");
		}
		OnCurve onCurve2 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		if (onCurve2 == null)
		{
			return UpdateMissing("Curve");
		}
		if (!(iGemDefinition.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Gem");
		}
		double value = 0.0;
		iGemStart.GetDouble(ref value);
		double value2 = 0.0;
		iGemEnd.GetDouble(ref value2);
		double value3 = 0.0;
		iGemSpacing.GetDouble(ref value3);
		double value4 = 0.0;
		iZOffsetPercent.GetDouble(ref value4);
		double value5 = 0.0;
		iChannelSpacing.GetDouble(ref value5);
		double value6 = 0.0;
		iChannelEndSpacing.GetDouble(ref value6);
		bool value7 = true;
		iShowChannel.GetBoolean(ref value7);
		int value8 = 0;
		iOrientMethod.GetInteger(ref value8);
		double value9 = 0.0;
		iGemHeightOffset.GetDouble(ref value9);
		int value10 = 0;
		iSpacingMethod.GetInteger(ref value10);
		CSData cSData = cSGemData.Duplicate();
		double value11 = 0.0;
		if (iGemRotation != null)
		{
			iGemRotation.GetDouble(ref value11);
		}
		OnXform onXform = new OnXform();
		onXform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value11), OnUtil.On_zaxis, OnUtil.On_origin);
		BoundingBox boundingBox = BoundingBox.Unset;
		CSDataHelper.Transform(cSData, onXform.Convert());
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Gem Bounding Box");
		}
		Vector3d diagonal = boundingBox.Diagonal;
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, CSUtilGem.EGemPlacementMethod.Table, ref zOffset);
		double t = 0.0;
		if (!onCurve2.GetNormalizedArcLengthPoint(value, ref t))
		{
			return UpdateError("Error Getting Start Point");
		}
		double t2 = 0.0;
		if (!onCurve2.GetNormalizedArcLengthPoint(value2, ref t2))
		{
			return UpdateError("Error Getting End Point");
		}
		if (!value7)
		{
			oGems.SetData(null);
			oParamStart.SetData(new CSNumberData(t));
			oParamEnd.SetData(new CSNumberData(t2));
			oParamFirstGem.SetData(null);
			oParamLastGem.SetData(null);
			oGemSizeVector.SetData(null);
			return base.Update();
		}
		OnCurve onCurve3 = onCurve2.Duplicate();
		onCurve3.Trim(new OnInterval(t, t2));
		double length = 0.0;
		onCurve3.GetLength(ref length);
		double num = 0.0;
		num += value6 + diagonal.Y / 2.0;
		CSDataArray cSDataArray = new CSDataArray();
		List<On3dPoint> list = new List<On3dPoint>();
		List<On3dPoint> list2 = new List<On3dPoint>();
		List<double> list3 = new List<double>();
		double num2 = diagonal.Y + value3;
		for (; num + diagonal.Y / 2.0 + value6 <= length; num += value3 + diagonal.Y)
		{
			double t3 = 0.0;
			On3dPoint on3dPoint = new On3dPoint(0.0, 0.0, 0.0);
			OnXform onXform2 = null;
			double num3 = 0.0;
			do
			{
				if (Math.Abs(num3) > 0.01)
				{
					num -= num3 * 0.2;
				}
				if (onCurve3.GetNormalizedArcLengthPoint(num / length, ref t3))
				{
					On3dPoint on3dPoint2 = onCurve3.PointAt(t3);
					list.Add(on3dPoint2);
					ClosestPoint(onCurve3, onCurve, t3, value8, out var paramOnInner);
					On3dPoint on3dPoint3 = onCurve.PointAt(paramOnInner);
					double num4 = (on3dPoint2 - on3dPoint3).Length();
					OnXform onXform3 = new OnXform();
					onXform3.Translation(0.0, 0.0, zOffset - num4 * (1.0 - value4) + value9);
					On3dVector on3dVector = on3dPoint3 - on3dPoint2;
					on3dVector.Unitize();
					onXform2 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, cSGemData.North.Convert(), cSGemData.Down.Convert(), on3dPoint2, onCurve3.TangentAt(t3), on3dVector) * onXform * onXform3;
					on3dPoint = new On3dPoint(0.0, 0.0, 0.0);
					on3dPoint.Transform(onXform2);
					if (list2.Count > 0)
					{
						num3 = (on3dPoint - list2[list2.Count - 1]).Length() - num2;
					}
				}
			}
			while (list2.Count != 0 && Math.Abs(num3) > 0.01);
			list2.Add(on3dPoint);
			CSData data = cSGemData.Duplicate();
			CSDataHelper.Transform(data, onXform2.Convert());
			cSDataArray.Append(data);
			list3.Add(num);
		}
		num -= value3 + diagonal.Y;
		if (value10 == 1 && list3.Count > 1)
		{
			list2.Clear();
			cSDataArray = new CSDataArray();
			num += diagonal.Y / 2.0;
			double num5 = length - value6 - num;
			num5 /= (double)(list3.Count - 1);
			for (int i = 0; i < list3.Count; i++)
			{
				double t4 = 0.0;
				On3dPoint on3dPoint4 = new On3dPoint(0.0, 0.0, 0.0);
				OnXform onXform4 = null;
				num = list3[i] + (double)(i - 1) * num5;
				if (onCurve3.GetNormalizedArcLengthPoint(num / length, ref t4))
				{
					On3dPoint on3dPoint5 = onCurve3.PointAt(t4);
					list.Add(on3dPoint5);
					ClosestPoint(onCurve3, onCurve, t4, value8, out var paramOnInner2);
					On3dPoint on3dPoint6 = onCurve.PointAt(paramOnInner2);
					double num6 = (on3dPoint5 - on3dPoint6).Length();
					OnXform onXform5 = new OnXform();
					onXform5.Translation(0.0, 0.0, zOffset - num6 * (1.0 - value4) + value9);
					On3dVector on3dVector2 = on3dPoint6 - on3dPoint5;
					on3dVector2.Unitize();
					onXform4 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, cSGemData.North.Convert(), cSGemData.Down.Convert(), on3dPoint5, onCurve3.TangentAt(t4), on3dVector2) * onXform5;
					on3dPoint4 = new On3dPoint(0.0, 0.0, 0.0);
					on3dPoint4.Transform(onXform4);
					list2.Add(on3dPoint4);
					CSData data2 = cSGemData.Duplicate();
					CSDataHelper.Transform(data2, onXform4.Convert());
					cSDataArray.Append(data2);
				}
			}
		}
		num += diagonal.Y / 2.0 + value6;
		if (num > length)
		{
			num = length;
		}
		double t5 = 0.0;
		onCurve3.GetNormalizedArcLengthPoint(num / length, ref t5);
		On3dPoint test_point = onCurve3.PointAt(t5);
		double t6 = 0.0;
		onCurve2.GetClosestPoint(test_point, ref t6);
		double t7 = 0.0;
		double t8 = 0.0;
		if (list.Count > 0)
		{
			onCurve2.GetClosestPoint(list[0], ref t7);
			onCurve2.GetClosestPoint(list[list.Count - 1], ref t8);
		}
		oGems.SetData(cSDataArray);
		oParamStart.SetData(new CSNumberData(t));
		oParamEnd.SetData(new CSNumberData(t6));
		oParamFirstGem.SetData(new CSNumberData(t7));
		oParamLastGem.SetData(new CSNumberData(t8));
		oGemSizeVector.SetData(new CSVectorData(diagonal));
		return base.Update();
	}

	private bool ClosestPoint(OnCurve Outer, OnCurve Inner, double paramOnOuter, int orientMethod, out double paramOnInner)
	{
		paramOnInner = 0.0;
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		On3dPoint on3dPoint = Outer.PointAt(paramOnOuter);
		On3dVector dir = Outer.TangentAt(paramOnOuter);
		OnSurface surfaceB = makesrf(on3dPoint, dir, orientMethod);
		Inner.IntersectSurface(surfaceB, ref x, CSApplicationHelper.Application.Tolerance);
		if (x.Count() == 0)
		{
			return false;
		}
		double num = 99999.0;
		for (int i = 0; i < x.Count(); i++)
		{
			double num2 = on3dPoint.DistanceTo(x[i].get_m_pointA(0));
			if (num2 < num)
			{
				num = num2;
				paramOnInner = x[i].get_m_a(0);
			}
		}
		return true;
	}

	private OnSurface makesrf(On3dPoint Center, On3dVector Dir, int orientMethod)
	{
		On3dVector on3dVector;
		On3dVector on3dVector2;
		if (orientMethod == 0)
		{
			on3dVector = new On3dVector(Center.x, 0.0, Center.z);
			on3dVector.Unitize();
			on3dVector2 = new On3dVector(0.0, 1.0, 0.0);
		}
		else
		{
			OnPlane onPlane = new OnPlane(Center, Dir);
			on3dVector2 = new On3dVector(onPlane.GetXaxis());
			on3dVector = new On3dVector(onPlane.GetYaxis());
		}
		On3dPoint a = new On3dPoint(Center + on3dVector2 * 50.0 + on3dVector * 50.0);
		On3dPoint b = new On3dPoint(Center + on3dVector2 * 50.0 - on3dVector * 50.0);
		On3dPoint c = new On3dPoint(Center - on3dVector2 * 50.0 - on3dVector * 50.0);
		On3dPoint d = new On3dPoint(Center - on3dVector2 * 50.0 + on3dVector * 50.0);
		return RhUtil.RhinoCreateSurfaceFromCorners(a, b, ref c, ref d);
	}
}
