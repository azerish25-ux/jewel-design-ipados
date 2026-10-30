using System;
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
		Point3d point3d;
		Vector3d vector3d;
		if (value3)
		{
			point3d = value.PointAtStart;
			vector3d = -value.TangentAt(value.Domain.Min);
		}
		else
		{
			point3d = value.PointAtEnd;
			vector3d = value.TangentAt(value.Domain.Max);
		}
		Point3d point3d2;
		Vector3d vector3d2;
		if (value4)
		{
			point3d2 = value2.PointAtStart;
			vector3d2 = -value2.TangentAt(value2.Domain.Min);
		}
		else
		{
			point3d2 = value2.PointAtEnd;
			vector3d2 = value2.TangentAt(value2.Domain.Max);
		}
		double num = point3d.DistanceTo(point3d2) / 5.0;
		Point3dList point3dList = new Point3dList();
		point3dList.Add(new Point3d(point3d));
		if (value5 >= 1)
		{
			point3dList.Add(new Point3d(point3d + vector3d * value7 * num));
		}
		if (value5 >= 2)
		{
			point3dList.Add(new Point3d(point3d + vector3d * (value7 + value8) * num));
		}
		if (value5 >= 3)
		{
			point3dList.Add(new Point3d(point3d + vector3d * (value7 + value8 + value9) * num));
		}
		if (value6 >= 3)
		{
			point3dList.Add(new Point3d(point3d2 + vector3d2 * (value10 + value11 + value12) * num));
		}
		if (value6 >= 2)
		{
			point3dList.Add(new Point3d(point3d2 + vector3d2 * (value10 + value11) * num));
		}
		if (value6 >= 1)
		{
			point3dList.Add(new Point3d(point3d2 + vector3d2 * value10 * num));
		}
		point3dList.Add(new Point3d(point3d2));
		NurbsCurve geometry = new BezierCurve(point3dList).ToNurbsCurve();
		oCurve.SetData(new CSOnGeometryData(geometry));
		return base.Update();
	}
}
