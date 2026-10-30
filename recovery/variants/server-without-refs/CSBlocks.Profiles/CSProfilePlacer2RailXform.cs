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
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
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
		OnNurbsCurve val = rail.Duplicate();
		double value = 0.0;
		InParameter.GetDouble(ref value);
		double value2 = 0.0;
		InOffset.GetDouble(ref value2);
		value += value2;
		OnNurbsCurve nurbsCurve = InFromObject.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			if (((OnCurve)rail).IsClosed())
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
		double num = lengthParameter;
		if (lengthParameter >= 1.0)
		{
			num = ((OnCurve)rail).Domain().Max();
		}
		else if (value <= 0.0)
		{
			num = ((OnCurve)rail).Domain().Min();
		}
		else if (!((OnCurve)rail).GetNormalizedArcLengthPoint(lengthParameter, ref num))
		{
			return UpdateError("GetNormalizedArcLengthPoint failed");
		}
		double num2 = lengthParameter;
		double number = num;
		if (nurbsCurve != null)
		{
			((OnCurve)val).GetClosestPoint((IOn3dPoint)(object)((OnCurve)rail).PointAt(num), ref num2);
			double num3 = 1.0;
			((OnCurve)val).GetLength(ref num3);
			((OnCurve)val).Trim((IOnInterval)new OnInterval(((OnCurve)val).Domain().Min(), num2));
			double num4 = 1.0;
			((OnCurve)val).GetLength(ref num4);
			number = num4 / num3;
		}
		OutParam.SetData(new CSNumberData(number));
		OnXform xOrient = new OnXform();
		if (!CSUtilGeometry.OrientProfileToRailNalp((IOnCurve)(object)rail, value, ref xOrient))
		{
			return UpdateError("OrientProfileToRailNalp failed");
		}
		OutXform.SetData(new CSXformData(xOrient.Convert()));
		OnNurbsCurve val2 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		if (val2 != null)
		{
			OnLineCurve val3 = new OnLineCurve(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(0.0, 30.0, 0.0));
			val3.Transform((IOnXform)(object)xOrient);
			ArrayOnX_EVENT val4 = new ArrayOnX_EVENT();
			((OnCurve)val3).IntersectCurve((IOnCurve)(object)val2, ref val4);
			if (val4.Count() > 0)
			{
				On3dPoint obj = ((OnCurve)val3).PointAtStart();
				On3dPoint val5 = val4[0][0];
				double num5 = obj.DistanceTo((IOn3dPoint)(object)val5);
				if (num5 < 0.01)
				{
					num5 = 0.01;
				}
				OutYSize.SetData(new CSNumberData(num5));
				double num6 = val4[0][0];
				if (Math.Abs(value - ((OnCurve)rail).Domain().Min()) < 0.0001 && Math.Abs(num6 - ((OnCurve)val2).Domain().Max()) < 0.0001)
				{
					num6 = ((OnCurve)val2).Domain().Min();
				}
				if (Math.Abs(value - ((OnCurve)rail).Domain().Max()) < 0.0001 && Math.Abs(num6 - ((OnCurve)val2).Domain().Min()) < 0.0001)
				{
					num6 = ((OnCurve)val2).Domain().Max();
				}
				OutParam2.SetData(new CSNumberData(num6));
			}
		}
		return base.Update();
	}
}
