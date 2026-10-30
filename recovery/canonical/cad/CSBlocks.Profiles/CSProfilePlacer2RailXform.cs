using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Profiles;

[CSBlock("ProfilePlacer2RailXform", "{9F010B9E-5EE3-4fb1-B693-4E4906C444D5}")]
public sealed class CSProfilePlacer2RailXform : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InOutsideRail;

	public readonly CSInputChannel InParameter;

	public readonly CSInputChannel InOffset;

	public readonly CSInputChannel InFromObject;

	public readonly CSOutputChannel OutXform;

	public readonly CSOutputChannel OutParam;

	public readonly CSOutputChannel OutParam2;

	public readonly CSOutputChannel OutYSize;

	public CSProfilePlacer2RailXform(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InOutsideRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InParameter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InFromObject = new CSInputChannel(this, ChannelDataType.Point | ChannelDataType.Curve, ChannelArrayType.Individual);
		OutXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutParam2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InRail.GetFlattenArray();
		CSDataArray flattenArray2 = InOutsideRail.GetFlattenArray();
		if (flattenArray.GetArrayCount() > 1)
		{
			return UpdateError("More then 1 Rail supplied");
		}
		if (flattenArray2.GetArrayCount() > 1)
		{
			return UpdateError("More then 1 Outside Rail supplied");
		}
		CSData arrayItem = flattenArray.GetArrayItem(0);
		if (arrayItem == null)
		{
			return UpdateMissing("Ring Rail Data");
		}
		OnNurbsCurve rail = arrayItem.GetNurbsCurve().Convert();
		if (rail == null)
		{
			return UpdateMissing("Ring Rail");
		}
		OnNurbsCurve onNurbsCurve = rail.Duplicate();
		double value = 0.0;
		InParameter.GetDouble(ref value);
		double value2 = 0.0;
		InOffset.GetDouble(ref value2);
		value += value2;
		OnNurbsCurve nurbsCurve = InFromObject.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			if (rail.IsClosed())
			{
				if (value < 0.0)
				{
					value += 1.0;
				}
				if (value > 1.0)
				{
					value -= 1.0;
				}
				if (value == 1.0)
				{
					value = 0.0;
				}
			}
			else
			{
				if (value < 0.0)
				{
					value = 0.0;
				}
				if (value > 1.0)
				{
					value = 1.0;
				}
			}
		}
		double lengthParameter = value;
		bool flip = false;
		CSUtilGeometry.AdjustRailFromCurve(nurbsCurve, ref rail, ref lengthParameter, ref flip);
		double t = lengthParameter;
		if (lengthParameter >= 1.0)
		{
			t = rail.Domain().Max();
		}
		else if (value <= 0.0)
		{
			t = rail.Domain().Min();
		}
		else if (!rail.GetNormalizedArcLengthPoint(lengthParameter, ref t))
		{
			return UpdateError("GetNormalizedArcLengthPoint failed");
		}
		double t2 = lengthParameter;
		double number = t;
		if (nurbsCurve != null)
		{
			onNurbsCurve.GetClosestPoint(rail.PointAt(t), ref t2);
			double length = 1.0;
			onNurbsCurve.GetLength(ref length);
			onNurbsCurve.Trim(new OnInterval(onNurbsCurve.Domain().Min(), t2));
			double length2 = 1.0;
			onNurbsCurve.GetLength(ref length2);
			number = length2 / length;
		}
		OutParam.SetData(new CSNumberData(number));
		OnXform xOrient = new OnXform();
		if (!CSUtilGeometry.OrientProfileToRailNalp(rail, value, ref xOrient))
		{
			return UpdateError("OrientProfileToRailNalp failed");
		}
		OutXform.SetData(new CSXformData(xOrient.Convert()));
		OnNurbsCurve onNurbsCurve2 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		if (onNurbsCurve2 != null)
		{
			OnLineCurve onLineCurve = new OnLineCurve(OnUtil.On_origin, new On3dPoint(0.0, 30.0, 0.0));
			onLineCurve.Transform(xOrient);
			ArrayOnX_EVENT x = new ArrayOnX_EVENT();
			onLineCurve.IntersectCurve(onNurbsCurve2, ref x);
			if (x.Count() > 0)
			{
				On3dPoint on3dPoint = onLineCurve.PointAtStart();
				On3dPoint arg = x[0].get_m_pointA(0);
				double num = on3dPoint.DistanceTo(arg);
				if (num < 0.01)
				{
					num = 0.01;
				}
				OutYSize.SetData(new CSNumberData(num));
				double num2 = x[0].get_m_b(0);
				if (Math.Abs(value - rail.Domain().Min()) < 0.0001 && Math.Abs(num2 - onNurbsCurve2.Domain().Max()) < 0.0001)
				{
					num2 = onNurbsCurve2.Domain().Min();
				}
				if (Math.Abs(value - rail.Domain().Max()) < 0.0001 && Math.Abs(num2 - onNurbsCurve2.Domain().Min()) < 0.0001)
				{
					num2 = onNurbsCurve2.Domain().Max();
				}
				OutParam2.SetData(new CSNumberData(num2));
			}
		}
		return base.Update();
	}
}
