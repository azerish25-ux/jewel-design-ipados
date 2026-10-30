using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.UDT;

[CSBlock("CurvedTaperXY", "{F0AC7075-E6BF-41fe-AA28-5DAC47E561BB}")]
public sealed class CSCurvedTaperXY : CSBlock
{
	private sealed class CSCurvedTaperXYMorph : CSSpaceMorph
	{
		private readonly NurbsCurve _amountCurve;

		private readonly double _start;

		private readonly double _end;

		public CSCurvedTaperXYMorph(double zStart, double zEnd, double amount1X, double amount2X, double amount3X, double amount1Y, double amount2Y, double amount3Y)
		{
			_start = zStart;
			_end = zEnd;
			_amountCurve = Curve.CreateInterpolatedCurve(new Point3d[3]
			{
				new Point3d(amount1X, amount1Y, 0.0),
				new Point3d(amount2X, amount2Y, 0.5),
				new Point3d(amount3X, amount3Y, 1.0)
			}, 3, CurveKnotStyle.Chord, Vector3d.Unset, Vector3d.ZAxis).ToNurbsCurve();
			_amountCurve.Domain = new Interval(0.0, 1.0);
		}

		public override bool IsValid()
		{
			return !(Math.Abs(_start - _end) < 0.01);
		}

		public override Transform MorphFixed(Transform transform)
		{
			return transform;
		}

		public override Point3d MorphPoint(Point3d point)
		{
			double t = (point.Z - _end) / (_start - _end);
			Point3d point3d = _amountCurve.PointAt(t);
			return new Point3d(point.X * point3d.X, point.Y * point3d.Y, point.Z);
		}
	}

	public CSInputChannel InObject { get; }

	public CSInputChannel InAmount1x { get; }

	public CSInputChannel InAmount2X { get; }

	public CSInputChannel InAmount3X { get; }

	public CSInputChannel InAmount1Y { get; }

	public CSInputChannel InAmount2Y { get; }

	public CSInputChannel InAmount3Y { get; }

	public CSInputChannel InAdjustBoundingBoxZ { get; }

	public CSOutputChannel OutObjects { get; }

	public CSCurvedTaperXY(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(GetPropertyName(() => InObject), this, ChannelDataType.Any, ChannelArrayType.Individual);
		InAmount1x = new CSInputChannel(GetPropertyName(() => InAmount1x), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAmount2X = new CSInputChannel(GetPropertyName(() => InAmount2X), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAmount3X = new CSInputChannel(GetPropertyName(() => InAmount3X), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAmount1Y = new CSInputChannel(GetPropertyName(() => InAmount1Y), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAmount2Y = new CSInputChannel(GetPropertyName(() => InAmount2Y), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAmount3Y = new CSInputChannel(GetPropertyName(() => InAmount3Y), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAdjustBoundingBoxZ = new CSInputChannel(GetPropertyName(() => InAdjustBoundingBoxZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutObjects = new CSOutputChannel(GetPropertyName(() => OutObjects), this, ChannelDataType.Any, ChannelArrayType.Any);
	}

	public override bool Update()
	{
		CSData data = InObject.Data;
		if (data == null)
		{
			return UpdateMissing("Invalid Data");
		}
		double value = 0.5;
		InAmount1x.GetDouble(ref value);
		double value2 = 0.5;
		InAmount2X.GetDouble(ref value2);
		double value3 = 0.5;
		InAmount3X.GetDouble(ref value3);
		double value4 = value;
		InAmount1Y.GetDouble(ref value4);
		double value5 = value2;
		InAmount2Y.GetDouble(ref value5);
		double value6 = value3;
		InAmount3Y.GetDouble(ref value6);
		double value7 = 0.0;
		InAdjustBoundingBoxZ.GetDouble(ref value7);
		BoundingBox boundingBox = BoundingBox.Unset;
		data.GetTightBoundingBox(ref boundingBox);
		if (value7 < 0.0)
		{
			value7 = 0.0;
		}
		CSCurvedTaperXYMorph cSCurvedTaperXYMorph = new CSCurvedTaperXYMorph(boundingBox.Max.Z, boundingBox.Min.Z - value7, value, value2, value3, value4, value5, value6);
		if (!cSCurvedTaperXYMorph.IsValid())
		{
			return UpdateError("Invalid Morph");
		}
		OutObjects.SetData(data.Duplicate());
		cSCurvedTaperXYMorph.Tolerance = 0.05;
		OutObjects.Data.Morph(cSCurvedTaperXYMorph);
		return base.Update();
	}
}
