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

namespace CSBlocks.Bails.Obsolete;

[Obsolete]
[CSBlock("BailRail_V2", "{BAED5A7F-0A27-482c-A456-470C6061770C}")]
public sealed class CSBailRail2 : CSBlock
{
	private sealed class EndPinchMorph : OnSpaceMorph
	{
		private double _opening;

		private double _length;

		public override On3dPoint MorphPoint(IOn3dPoint inputPoint)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			On3dPoint val = new On3dPoint(inputPoint);
			val.z += _opening * Math.Cos(CSBlockUtil.CSUtilMath.ToRadians(val.y / _length * 90.0));
			return val;
		}

		public void SetUp(double newOpening, double newLength)
		{
			_opening = newOpening;
			_length = newLength;
		}
	}

	private sealed class AngleMorph : OnSpaceMorph
	{
		private double _distance;

		private double _length;

		public override On3dPoint MorphPoint(IOn3dPoint inputPoint)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			On3dPoint val = new On3dPoint(inputPoint);
			if (val.y > 0.0)
			{
				val.x += _distance * (1.0 - Math.Cos(CSBlockUtil.CSUtilMath.ToRadians(val.y / _length * 90.0)));
			}
			return val;
		}

		public void SetUp(double newDistance, double newLength)
		{
			_distance = newDistance;
			_length = newLength;
		}
	}

	public CSInputChannel iRail { get; }

	public CSInputChannel iLength { get; }

	public CSInputChannel iOpening { get; }

	public CSInputChannel iEndPinch { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iClosed { get; }

	public CSInputChannel iBlendAmount { get; }

	public CSInputChannel iBlendAmount1 { get; }

	public CSInputChannel iBlendAmount2 { get; }

	public CSOutputChannel oRail { get; }

	public CSOutputChannel oParam1 { get; }

	public CSOutputChannel oParam2 { get; }

	public CSOutputChannel oParam3 { get; }

	public CSOutputChannel oBailOpeningInsideHeight { get; }

	public CSOutputChannel oBailOpeningInsideDepth { get; }

	public CSBailRail2(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iLength = new CSInputChannel(GetPropertyName(() => iLength), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOpening = new CSInputChannel(GetPropertyName(() => iOpening), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndPinch = new CSInputChannel(GetPropertyName(() => iEndPinch), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iClosed = new CSInputChannel(GetPropertyName(() => iClosed), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iBlendAmount = new CSInputChannel(GetPropertyName(() => iBlendAmount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAmount1 = new CSInputChannel(GetPropertyName(() => iBlendAmount1), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAmount2 = new CSInputChannel(GetPropertyName(() => iBlendAmount2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oRail = new CSOutputChannel(GetPropertyName(() => oRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oParam1 = new CSOutputChannel(GetPropertyName(() => oParam1), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParam2 = new CSOutputChannel(GetPropertyName(() => oParam2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParam3 = new CSOutputChannel(GetPropertyName(() => oParam3), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBailOpeningInsideHeight = new CSOutputChannel("BailOpeningInsideHeight", this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBailOpeningInsideDepth = new CSOutputChannel("BailOpeningInsideDepth", this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_01a9: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Expected O, but got Unknown
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Expected O, but got Unknown
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Expected O, but got Unknown
		OnNurbsCurve nurbsCurve = iRail.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing rail");
		}
		double value = 0.0;
		iLength.GetDouble(ref value);
		double value2 = 0.0;
		iOpening.GetDouble(ref value2);
		double value3 = 0.0;
		iEndPinch.GetDouble(ref value3);
		double value4 = 0.0;
		iAngle.GetDouble(ref value4);
		value4 = CSBlockUtil.CSUtilMath.ToRadians(value4);
		bool value5 = false;
		iClosed.GetBoolean(ref value5);
		double value6 = 0.0;
		iBlendAmount.GetDouble(ref value6);
		double value7 = 0.0;
		iBlendAmount1.GetDouble(ref value7);
		double value8 = 0.0;
		iBlendAmount2.GetDouble(ref value8);
		nurbsCurve.RemoveShortSegments(0.2);
		nurbsCurve.RepairBadKnots();
		RhUtil.RhinoRepairCurve((OnCurve)(object)nurbsCurve, 0.2, 3);
		OnBoundingBox val = new OnBoundingBox();
		if (!((OnGeometry)nurbsCurve).GetTightBoundingBox(ref val))
		{
			return UpdateError("Missing rail bounding box");
		}
		OnXform val2 = new OnXform();
		val2.Scale(1.0, value / (val.Max().y - val.Min().y), value2 / (val.Max().z - val.Min().z));
		nurbsCurve.Transform((IOnXform)(object)val2);
		OnLineCurve val3 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, value, value2), (IOn3dPoint)new On3dPoint(0.0, value, 0.0));
		ArrayOnX_EVENT val4 = new ArrayOnX_EVENT();
		((OnCurve)nurbsCurve).IntersectCurve((IOnCurve)(object)val3, ref val4);
		if (val4.Count() == 0)
		{
			return UpdateError("Error Intersect");
		}
		double num = val4[0][0];
		On3dPoint val5 = ((OnCurve)nurbsCurve).PointAt(num);
		foreach (OnX_EVENT item in val4)
		{
			OnX_EVENT val6 = item;
			if (val6[0].z > val5.z)
			{
				num = val6[0];
				val5 = ((OnCurve)nurbsCurve).PointAt(num);
			}
		}
		OnCurve val7 = null;
		OnCurve val8 = null;
		if (!((OnCurve)nurbsCurve).Split(num, ref val7, ref val8))
		{
			return UpdateError("Error Split");
		}
		if (val7 == null || val8 == null)
		{
			return UpdateError("Error Split");
		}
		EndPinchMorph endPinchMorph = new EndPinchMorph();
		endPinchMorph.SetUp(value3, val5.y);
		((OnSpaceMorph)endPinchMorph).SetTolerance(0.05);
		if (!((OnGeometry)val7).Morph((IOnSpaceMorph)(object)endPinchMorph))
		{
			return UpdateError("Error Morph");
		}
		IOnCurve[] obj = new IOnCurve[2]
		{
			(IOnCurve)val7,
			(IOnCurve)val8
		};
		OnCurve[] array = null;
		if (!RhUtil.RhinoMergeCurves((IOnCurve[])(object)obj, ref array))
		{
			return UpdateError("Error Merge Curves");
		}
		if (array.Length != 1)
		{
			return UpdateError("Error Merge Curves");
		}
		OnNurbsCurve val9 = new OnNurbsCurve();
		array[0].GetNurbForm(ref val9);
		OnNurbsCurve val10 = val9.Duplicate();
		if (value4 != 0.0)
		{
			double num2 = -1.0 * Math.Tan(value4) * value;
			AngleMorph angleMorph = new AngleMorph();
			angleMorph.SetUp(num2, value);
			((OnSpaceMorph)angleMorph).SetTolerance(0.05);
			if (!val10.Morph((IOnSpaceMorph)(object)angleMorph))
			{
				return UpdateError("Error Morph Angle Rail");
			}
			On3dPoint obj2 = val5;
			obj2.x += num2;
		}
		OnNurbsCurve val11 = new OnNurbsCurve();
		double num3 = 0.0;
		double num4 = 0.0;
		double num5 = 0.0;
		if (value5)
		{
			OnNurbsCurve val12 = CSUtilCurves.BlendCurve((IOn3dPoint)(object)((OnCurve)val9).PointAtStart(), (IOn3dPoint)(object)((OnCurve)val9).PointAtEnd(), (IOn3dVector)(object)(-((OnCurve)val9).TangentAt(((OnCurve)val9).Domain().Min())), (IOn3dVector)(object)((OnCurve)val9).TangentAt(((OnCurve)val9).Domain().Max()), value6, value7, value8);
			IOnCurve[] obj3 = new IOnCurve[2]
			{
				(IOnCurve)val10,
				(IOnCurve)val12
			};
			OnCurve[] array2 = null;
			if (!RhUtil.RhinoMergeCurves((IOnCurve[])(object)obj3, ref array2))
			{
				return UpdateError("Error Merge Curves");
			}
			if (array2.Length != 1)
			{
				return UpdateError("Error Merge Curves");
			}
			array2[0].GetNurbForm(ref val11);
			((OnCurve)val11).GetClosestPoint((IOn3dPoint)(object)((OnCurve)val10).PointAtStart(), ref num3);
			((OnCurve)val11).GetClosestPoint((IOn3dPoint)(object)((OnCurve)val10).PointAtEnd(), ref num4);
			((OnCurve)val11).GetClosestPoint((IOn3dPoint)(object)val5, ref num5);
			double num6 = 0.0;
			((OnCurve)val11).GetLength(ref num6);
			double num7 = 0.0;
			((OnCurve)val11).GetLength(ref num7, CSApplicationHelper.Application.Tolerance, (IOnInterval)new OnInterval(((OnCurve)val11).Domain().Min(), num3));
			num3 = num7 / num6;
			double num8 = 0.0;
			((OnCurve)val11).GetLength(ref num8, CSApplicationHelper.Application.Tolerance, (IOnInterval)new OnInterval(((OnCurve)val11).Domain().Min(), num4));
			num4 = num8 / num6;
			double num9 = 0.0;
			((OnCurve)val11).GetLength(ref num9, CSApplicationHelper.Application.Tolerance, (IOnInterval)new OnInterval(((OnCurve)val11).Domain().Min(), num5));
			num5 = num9 / num6;
		}
		else
		{
			val11 = val10;
			num3 = ((OnCurve)val11).Domain().Min();
			num4 = ((OnCurve)val11).Domain().Max();
			((OnCurve)val11).GetClosestPoint((IOn3dPoint)(object)val5, ref num5);
		}
		OnBoundingBox val13 = new OnBoundingBox();
		if (!((OnGeometry)val11).GetTightBoundingBox(ref val13))
		{
			return UpdateError("Error Get Tight Bounding Box from Output Rail");
		}
		double number = val13.Max().y + val13.Min().y;
		double number2 = val13.Max().z + val13.Min().z;
		oRail.SetData(new CSOnGeometryData(val11.Convert()));
		oParam1.SetData(new CSNumberData(num3));
		oParam2.SetData(new CSNumberData(num4));
		oParam3.SetData(new CSNumberData(num5));
		oBailOpeningInsideHeight.SetData(new CSNumberData(number));
		oBailOpeningInsideDepth.SetData(new CSNumberData(number2));
		return base.Update();
	}
}
