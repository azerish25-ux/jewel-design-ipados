using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.UDT;

[CSBlock("RingTaper", "{BA800E01-0B5C-45af-80C4-570D05774CEB}")]
public sealed class CSRingTaper : CSBlock
{
	private sealed class CSRingTaperMorph : CSSpaceMorph
	{
		private readonly double _start;

		private readonly double _end;

		private readonly double _amount;

		public CSRingTaperMorph(double zStart, double zEnd, double amount)
		{
			_start = zStart;
			_end = zEnd;
			_amount = amount;
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
			double num = (point.Z - _end) / (_start - _end);
			return new Point3d(point.X, point.Y * (1.0 * num + _amount * (1.0 - num)), point.Z);
		}
	}

	private sealed class CSRingTaperCurveMorph : CSSpaceMorph
	{
		private readonly double _start;

		private readonly double _end;

		private readonly NurbsCurve _amountCurve;

		public CSRingTaperCurveMorph(double zStart, double zEnd, double amount1, double amount2, double amount3)
		{
			_start = zStart;
			_end = zEnd;
			List<Point3d> points = new List<Point3d>
			{
				new Point3d(0.0, 0.0, amount1),
				new Point3d(0.5, 0.0, amount2),
				new Point3d(1.0, 0.0, amount3)
			};
			_amountCurve = Curve.CreateInterpolatedCurve(points, 3, CurveKnotStyle.Chord, Vector3d.Unset, Vector3d.XAxis).ToNurbsCurve();
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
			double z = _amountCurve.PointAt(t).Z;
			return new Point3d(point.X, point.Y * z, point.Z);
		}
	}

	public CSInputChannel iObject { get; }

	public CSInputChannel iAmount { get; }

	public CSInputChannel iAmount2 { get; }

	public CSInputChannel iAmount3 { get; }

	public CSOutputChannel oObjects { get; }

	public CSRingTaper(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(GetPropertyName(() => iObject), this, ChannelDataType.Any, ChannelArrayType.Any);
		iAmount = new CSInputChannel(GetPropertyName(() => iAmount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAmount2 = new CSInputChannel(GetPropertyName(() => iAmount2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAmount3 = new CSInputChannel(GetPropertyName(() => iAmount3), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(GetPropertyName(() => oObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		if (iObject == null)
		{
			return UpdateMissing("Missing Object");
		}
		CSData data = iObject.Data;
		if (data == null)
		{
			return UpdateMissing("Invalid Object");
		}
		double value = 0.5;
		if (iAmount != null)
		{
			iAmount.GetDouble(ref value);
		}
		bool flag = false;
		bool flag2 = false;
		double value2 = 0.5;
		double value3 = 0.5;
		if (iAmount2 != null)
		{
			flag = iAmount2.GetDouble(ref value2);
		}
		if (iAmount3 != null)
		{
			flag2 = iAmount3.GetDouble(ref value3);
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		data.GetTightBoundingBox(ref boundingBox);
		CSSpaceMorph cSSpaceMorph = ((!(flag || flag2)) ? ((CSSpaceMorph)new CSRingTaperMorph(boundingBox.Max.Z, boundingBox.Min.Z, value)) : ((CSSpaceMorph)new CSRingTaperCurveMorph(boundingBox.Max.Z, boundingBox.Min.Z, value, value2, value3)));
		if (!cSSpaceMorph.IsValid())
		{
			return UpdateError("Invalid Morph");
		}
		oObjects.SetData(data.Duplicate());
		cSSpaceMorph.Tolerance = 0.05;
		oObjects.Data.Morph(cSSpaceMorph);
		return base.Update();
	}
}
