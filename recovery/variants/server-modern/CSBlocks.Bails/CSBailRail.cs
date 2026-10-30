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
using Rhino.Geometry;

namespace CSBlocks.Bails;

[CSBlock("BailRail", "{006F6CB4-2C39-4CC1-BC61-72CFC5469FE6}")]
public sealed class CSBailRail : CSBlock
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

	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InLength;

	public readonly CSInputChannel InOpening;

	public readonly CSInputChannel InEndPinch;

	public readonly CSInputChannel InAngle;

	public readonly CSInputChannel InClosed;

	public readonly CSInputChannel InBlendAmount;

	public readonly CSInputChannel InBlendAmount1;

	public readonly CSInputChannel InBlendAmount2;

	public readonly CSOutputChannel OutRail;

	public readonly CSOutputChannel OutParam1;

	public readonly CSOutputChannel OutParam2;

	public readonly CSOutputChannel OutParam3;

	public readonly CSOutputChannel OutBailOpeningInsideHeight;

	public readonly CSOutputChannel OutBailOpeningInsideDepth;

	public CSBailRail(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOpening = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndPinch = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InClosed = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InBlendAmount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendAmount1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InBlendAmount2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRail = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutParam1 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutParam2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutParam3 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutBailOpeningInsideHeight = new CSOutputChannel("BailOpeningInsideHeight", this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutBailOpeningInsideDepth = new CSOutputChannel("BailOpeningInsideDepth", this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		OnNurbsCurve nurbsCurve = InRail.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing rail");
		}
		double value = 0.0;
		InLength.GetDouble(ref value);
		double value2 = 0.0;
		InOpening.GetDouble(ref value2);
		double value3 = 0.0;
		InEndPinch.GetDouble(ref value3);
		double value4 = 0.0;
		InAngle.GetDouble(ref value4);
		value4 = CSBlockUtil.CSUtilMath.ToRadians(value4);
		bool value5 = false;
		InClosed.GetBoolean(ref value5);
		double value6 = 0.0;
		InBlendAmount.GetDouble(ref value6);
		double value7 = 0.0;
		InBlendAmount1.GetDouble(ref value7);
		double value8 = 0.0;
		InBlendAmount2.GetDouble(ref value8);
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
		double number = val13.Max().y - val13.Min().y;
		double number2 = val13.Max().z - val13.Min().z;
		OutRail.SetData(new CSOnGeometryData((GeometryBase)(object)val11.Convert()));
		OutParam1.SetData(new CSNumberData(num3));
		OutParam2.SetData(new CSNumberData(num4));
		OutParam3.SetData(new CSNumberData(num5));
		OutBailOpeningInsideHeight.SetData(new CSNumberData(number));
		OutBailOpeningInsideDepth.SetData(new CSNumberData(number2));
		return base.Update();
	}
}
