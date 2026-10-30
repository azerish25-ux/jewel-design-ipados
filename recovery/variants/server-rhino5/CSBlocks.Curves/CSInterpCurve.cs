using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Collections;
using Rhino.Geometry;

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
		if (!(iPointArray.Data is ICSArray iCSArray))
		{
			return UpdateMissing("Missing Points");
		}
		double value = 3.0;
		iDegree.GetDouble(ref value);
		double value2 = 0.0;
		bool flag = iKnotStyle.GetDouble(ref value2);
		Point3dList point3dList = new Point3dList();
		for (int i = 0; i < iCSArray.GetArrayCount(); i++)
		{
			Point3d value3 = default(Point3d);
			if (CSDataHelper.GetPoint(iCSArray.GetArrayItem(i), out value3))
			{
				point3dList.Add(value3);
			}
		}
		if (!flag && point3dList.First == point3dList.Last)
		{
			value2 = 3.0;
		}
		int num = (int)value2;
		NurbsCurve nurbsCurve = null;
		switch (num)
		{
		case 0:
			nurbsCurve = Curve.CreateInterpolatedCurve(point3dList, (int)value, CurveKnotStyle.Uniform).ToNurbsCurve();
			break;
		case 1:
			nurbsCurve = Curve.CreateInterpolatedCurve(point3dList, (int)value, CurveKnotStyle.Chord).ToNurbsCurve();
			break;
		case 2:
			nurbsCurve = Curve.CreateInterpolatedCurve(point3dList, (int)value, CurveKnotStyle.ChordSquareRoot).ToNurbsCurve();
			break;
		case 3:
			nurbsCurve = Curve.CreateInterpolatedCurve(point3dList, (int)value, CurveKnotStyle.UniformPeriodic).ToNurbsCurve();
			break;
		case 4:
			nurbsCurve = Curve.CreateInterpolatedCurve(point3dList, (int)value, CurveKnotStyle.ChordPeriodic).ToNurbsCurve();
			break;
		case 5:
			nurbsCurve = Curve.CreateInterpolatedCurve(point3dList, (int)value, CurveKnotStyle.ChordSquareRootPeriodic).ToNurbsCurve();
			break;
		default:
			UpdateError("Error getting knotStyle");
			break;
		}
		if (nurbsCurve == null)
		{
			return UpdateError("Unable to calculate curve");
		}
		if (!nurbsCurve.IsValid)
		{
			return UpdateError("Invalid Curve");
		}
		oCurve.SetData(new CSOnGeometryData(nurbsCurve));
		return base.Update();
	}
}
