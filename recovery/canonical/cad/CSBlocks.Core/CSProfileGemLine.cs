using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("ProfileGemLine", "{F85CAE09-4B93-40ba-B296-E736936F3741}")]
public sealed class CSProfileGemLine : CSBlock
{
	public readonly CSInputChannel iCurve1;

	public readonly CSInputChannel iCurve2;

	public readonly CSInputChannel iSurface;

	public readonly CSInputChannel iPoint1;

	public readonly CSInputChannel iPoint2;

	public readonly CSOutputChannel oLine;

	public CSProfileGemLine(string identifier)
		: base(identifier)
	{
		iCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		iPoint1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPoint2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oLine = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(iCurve1.GetFlattenArray().GetArrayItem(0), out var value);
		CSDataHelper.GetNurbsCurve(iCurve2.GetFlattenArray().GetArrayItem(0), out var value2);
		CSData data = iSurface.Data;
		if (data == null)
		{
			return UpdateMissing("Surface missing");
		}
		CSDataHelper.GetBrep(data, out var value3);
		BrepFace brepFace = value3.Faces[0];
		double value4 = 0.0;
		double value5 = 0.0;
		if (iPoint1 != null)
		{
			iPoint1.GetDouble(ref value4);
		}
		if (iPoint2 != null)
		{
			iPoint2.GetDouble(ref value5);
		}
		double t = 0.0;
		double t2 = 0.0;
		value.NormalizedLengthParameter(value4, out t);
		value2.NormalizedLengthParameter(value5, out t2);
		Point3d unset = Point3d.Unset;
		Point3d unset2 = Point3d.Unset;
		unset = value.PointAt(t);
		unset2 = value2.PointAt(t2);
		double u = 0.0;
		double v = 0.0;
		double u2 = 0.0;
		double v2 = 0.0;
		brepFace.ClosestPoint(unset, out u, out v);
		brepFace.ClosestPoint(unset2, out u2, out v2);
		LineCurve curve2d = new LineCurve(new Line(new Point3d(u, v, 0.0), new Point3d(u2, v2, 0.0)));
		Curve geometry = brepFace.Pushup(curve2d, 0.01);
		oLine.SetData(new CSOnGeometryData(geometry));
		return base.Update();
	}
}
