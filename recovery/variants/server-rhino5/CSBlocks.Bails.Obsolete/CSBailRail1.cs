using System;
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
[CSBlock("BailRail_V1", "{6A5C5084-E59F-480f-973D-4082A55B5810}")]
public sealed class CSBailRail1 : CSBlock
{
	private sealed class BailRail : OnSpaceMorph
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

	public CSInputChannel iRail { get; }

	public CSInputChannel iEndPinch { get; }

	public CSOutputChannel oRail { get; }

	public CSBailRail1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iEndPinch = new CSInputChannel(GetPropertyName(() => iEndPinch), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oRail = new CSOutputChannel(GetPropertyName(() => oRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00dd: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		CSData data = iRail.Data;
		if (data == null)
		{
			return UpdateMissing("Rail input mssing");
		}
		OnNurbsCurve val = data.GetNurbsCurve().Convert();
		if (val == null)
		{
			return UpdateError("Rail input is not a curve");
		}
		double value = 0.0;
		iEndPinch.GetDouble(ref value);
		val.RemoveShortSegments(0.2);
		val.RepairBadKnots();
		RhUtil.RhinoRepairCurve((OnCurve)(object)val, 0.2, 3);
		OnBoundingBox val2 = new OnBoundingBox();
		if (!((OnGeometry)val).GetTightBoundingBox(ref val2))
		{
			return UpdateError("Bounding Box failed");
		}
		OnLineCurve val3 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, val2.Max().y, val2.Max().z), (IOn3dPoint)new On3dPoint(0.0, val2.Max().y, 0.0));
		ArrayOnX_EVENT val4 = new ArrayOnX_EVENT();
		((OnCurve)val).IntersectCurve((IOnCurve)(object)val3, ref val4);
		if (val4.Count() == 0)
		{
			return UpdateError("Intersection failed");
		}
		double num = val4[0][0];
		On3dPoint val5 = ((OnCurve)val).PointAt(num);
		foreach (OnX_EVENT item in val4)
		{
			OnX_EVENT val6 = item;
			if (val6[0].z > val5.z)
			{
				num = val6[0];
				val5 = ((OnCurve)val).PointAt(num);
			}
		}
		OnCurve val7 = null;
		OnCurve val8 = null;
		if (!((OnCurve)val).Split(num, ref val7, ref val8))
		{
			return UpdateError("Split failed");
		}
		if (val7 == null || val8 == null)
		{
			return UpdateError("Split curves failed");
		}
		BailRail bailRail = new BailRail();
		bailRail.SetUp(value, val5.y);
		((OnSpaceMorph)bailRail).SetTolerance(0.05);
		if (!((OnGeometry)val7).Morph((IOnSpaceMorph)(object)bailRail))
		{
			return UpdateError("RailLhs Morph Failed");
		}
		OnCurve[] obj = new OnCurve[2] { val7, val8 };
		OnCurve[] array = null;
		IOnCurve[] array2 = (IOnCurve[])(object)obj;
		if (!RhUtil.RhinoMergeCurves(array2, ref array))
		{
			return UpdateError("Merge Curves Failed");
		}
		if (array.Length != 1)
		{
			return UpdateError("Merge Curves Failed Multi Curves returned");
		}
		OnCurve crv = array[0];
		oRail.SetData(new CSOnGeometryData(crv.Convert()));
		return base.Update();
	}
}
