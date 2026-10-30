using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Collections;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlocks.Curves;

[CSBlock("InterpCurve", "{43FBB6CC-0353-4781-A7B0-E61F5F3B7076}")]
public sealed class CSInterpCurve : CSBlock
{
	public readonly CSInputChannel iPointArray;

	public readonly CSInputChannel iDegree;

	public readonly CSInputChannel iKnotStyle;

	public readonly CSOutputChannel oCurve;

	public CSInterpCurve(string identifier)
		: base(identifier)
	{
		iPointArray = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Array);
		iDegree = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iKnotStyle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		if (!(iPointArray.Data is ICSArray iCSArray))
		{
			return UpdateMissing("Missing Points");
		}
		double value = 3.0;
		iDegree.GetDouble(ref value);
		double value2 = 0.0;
		bool flag = iKnotStyle.GetDouble(ref value2);
		Point3dList val = new Point3dList();
		for (int i = 0; i < iCSArray.GetArrayCount(); i++)
		{
			Point3d value3 = default(Point3d);
			if (CSDataHelper.GetPoint(iCSArray.GetArrayItem(i), out value3))
			{
				((RhinoList<Point3d>)(object)val).Add(value3);
			}
		}
		if (!flag && ((RhinoList<Point3d>)(object)val).First == ((RhinoList<Point3d>)(object)val).Last)
		{
			value2 = 3.0;
		}
		int num = (int)value2;
		NurbsCurve val2 = null;
		switch (num)
		{
		case 0:
			val2 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)val, (int)value, (CurveKnotStyle)0).ToNurbsCurve();
			break;
		case 1:
			val2 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)val, (int)value, (CurveKnotStyle)1).ToNurbsCurve();
			break;
		case 2:
			val2 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)val, (int)value, (CurveKnotStyle)2).ToNurbsCurve();
			break;
		case 3:
			val2 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)val, (int)value, (CurveKnotStyle)3).ToNurbsCurve();
			break;
		case 4:
			val2 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)val, (int)value, (CurveKnotStyle)4).ToNurbsCurve();
			break;
		case 5:
			val2 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)val, (int)value, (CurveKnotStyle)5).ToNurbsCurve();
			break;
		default:
			UpdateError("Error getting knotStyle");
			break;
		}
		if (val2 == null)
		{
			return UpdateError("Unable to calculate curve");
		}
		if (!((CommonObject)val2).IsValid)
		{
			return UpdateError("Invalid Curve");
		}
		oCurve.SetData(new CSOnGeometryData((GeometryBase)(object)val2));
		return base.Update();
	}
}
