using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Collections;
using Rhino.Geometry;

namespace CSBlocks.Curves.Obsolete;

[Obsolete]
[CSBlock("BlendCurvesAdvanced_V1", "{151D91E6-6AEF-4709-A69D-D03F144C1984}")]
public sealed class CSBlendCurvesAdvanced1 : CSBlock
{
	public readonly CSInputChannel iCurve1;

	public readonly CSInputChannel iCurve2;

	public readonly CSInputChannel iStartCurve1;

	public readonly CSInputChannel iStartCurve2;

	public readonly CSInputChannel ig1;

	public readonly CSInputChannel ig2;

	public readonly CSInputChannel iAmount1a;

	public readonly CSInputChannel iAmount1b;

	public readonly CSInputChannel iAmount1c;

	public readonly CSInputChannel iAmount2a;

	public readonly CSInputChannel iAmount2b;

	public readonly CSInputChannel iAmount2c;

	public readonly CSOutputChannel oCurve;

	public CSBlendCurvesAdvanced1(string identifier)
		: base(identifier)
	{
		iCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		iCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		iStartCurve1 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iStartCurve2 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		ig1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		ig2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAmount1a = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAmount1b = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAmount1c = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAmount2a = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAmount2b = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAmount2c = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iCurve1.GetFlattenArray();
		CSDataArray flattenArray2 = iCurve2.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0 || flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Curves");
		}
		CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(0), out var value);
		CSDataHelper.GetNurbsCurve(flattenArray2.GetArrayItem(0), out var value2);
		if (value == null || value2 == null)
		{
			return UpdateMissing("Missing Curves");
		}
		bool value3 = true;
		iStartCurve1.GetBoolean(ref value3);
		bool value4 = true;
		iStartCurve2.GetBoolean(ref value4);
		int value5 = 1;
		ig1.GetInteger(ref value5);
		int value6 = 1;
		ig2.GetInteger(ref value6);
		double value7 = 1.0;
		iAmount1a.GetDouble(ref value7);
		double value8 = 1.0;
		iAmount1b.GetDouble(ref value8);
		double value9 = 1.0;
		iAmount1c.GetDouble(ref value9);
		double value10 = 1.0;
		iAmount2a.GetDouble(ref value10);
		double value11 = 1.0;
		iAmount2b.GetDouble(ref value11);
		double value12 = 1.0;
		iAmount2c.GetDouble(ref value12);
		Point3d val;
		Interval domain;
		Vector3d val2;
		if (value3)
		{
			val = ((Curve)value).PointAtStart;
			NurbsCurve obj = value;
			domain = ((Curve)value).Domain;
			val2 = -((Curve)obj).TangentAt(((Interval)(ref domain)).Min);
		}
		else
		{
			val = ((Curve)value).PointAtEnd;
			NurbsCurve obj2 = value;
			domain = ((Curve)value).Domain;
			val2 = ((Curve)obj2).TangentAt(((Interval)(ref domain)).Max);
		}
		Point3d val3;
		Vector3d val4;
		if (value4)
		{
			val3 = ((Curve)value2).PointAtStart;
			NurbsCurve obj3 = value2;
			domain = ((Curve)value2).Domain;
			val4 = -((Curve)obj3).TangentAt(((Interval)(ref domain)).Min);
		}
		else
		{
			val3 = ((Curve)value2).PointAtEnd;
			NurbsCurve obj4 = value2;
			domain = ((Curve)value2).Domain;
			val4 = ((Curve)obj4).TangentAt(((Interval)(ref domain)).Max);
		}
		double num = ((Point3d)(ref val)).DistanceTo(val3) / 5.0;
		Point3dList val5 = new Point3dList();
		((RhinoList<Point3d>)(object)val5).Add(new Point3d(val));
		if (value5 >= 1)
		{
			((RhinoList<Point3d>)(object)val5).Add(new Point3d(val + val2 * value7 * num));
		}
		if (value5 >= 2)
		{
			((RhinoList<Point3d>)(object)val5).Add(new Point3d(val + val2 * (value7 + value8) * num));
		}
		if (value5 >= 3)
		{
			((RhinoList<Point3d>)(object)val5).Add(new Point3d(val + val2 * (value7 + value8 + value9) * num));
		}
		if (value6 >= 3)
		{
			((RhinoList<Point3d>)(object)val5).Add(new Point3d(val3 + val4 * (value10 + value11 + value12) * num));
		}
		if (value6 >= 2)
		{
			((RhinoList<Point3d>)(object)val5).Add(new Point3d(val3 + val4 * (value10 + value11) * num));
		}
		if (value6 >= 1)
		{
			((RhinoList<Point3d>)(object)val5).Add(new Point3d(val3 + val4 * value10 * num));
		}
		((RhinoList<Point3d>)(object)val5).Add(new Point3d(val3));
		NurbsCurve geometry = new BezierCurve((IEnumerable<Point3d>)val5).ToNurbsCurve();
		oCurve.SetData(new CSOnGeometryData((GeometryBase)(object)geometry));
		return base.Update();
	}
}
