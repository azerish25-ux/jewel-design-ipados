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
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return transform;
		}

		public override Point3d MorphPoint(Point3d point)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			double num = (((Point3d)(ref point)).Z - _end) / (_start - _end);
			return new Point3d(((Point3d)(ref point)).X, ((Point3d)(ref point)).Y * (1.0 * num + _amount * (1.0 - num)), ((Point3d)(ref point)).Z);
		}
	}

	private sealed class CSRingTaperCurveMorph : CSSpaceMorph
	{
		private readonly double _start;

		private readonly double _end;

		private readonly NurbsCurve _amountCurve;

		public CSRingTaperCurveMorph(double zStart, double zEnd, double amount1, double amount2, double amount3)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			_start = zStart;
			_end = zEnd;
			List<Point3d> list = new List<Point3d>
			{
				new Point3d(0.0, 0.0, amount1),
				new Point3d(0.5, 0.0, amount2),
				new Point3d(1.0, 0.0, amount3)
			};
			_amountCurve = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)list, 3, (CurveKnotStyle)1, Vector3d.Unset, Vector3d.XAxis).ToNurbsCurve();
			((Curve)_amountCurve).Domain = new Interval(0.0, 1.0);
		}

		public override bool IsValid()
		{
			return !(Math.Abs(_start - _end) < 0.01);
		}

		public override Transform MorphFixed(Transform transform)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return transform;
		}

		public override Point3d MorphPoint(Point3d point)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			double num = (((Point3d)(ref point)).Z - _end) / (_start - _end);
			Point3d val = ((Curve)_amountCurve).PointAt(num);
			double z = ((Point3d)(ref val)).Z;
			return new Point3d(((Point3d)(ref point)).X, ((Point3d)(ref point)).Y * z, ((Point3d)(ref point)).Z);
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
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
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
		Point3d val;
		CSSpaceMorph cSSpaceMorph;
		if (flag || flag2)
		{
			val = ((BoundingBox)(ref boundingBox)).Max;
			double z = ((Point3d)(ref val)).Z;
			val = ((BoundingBox)(ref boundingBox)).Min;
			cSSpaceMorph = new CSRingTaperCurveMorph(z, ((Point3d)(ref val)).Z, value, value2, value3);
		}
		else
		{
			val = ((BoundingBox)(ref boundingBox)).Max;
			double z2 = ((Point3d)(ref val)).Z;
			val = ((BoundingBox)(ref boundingBox)).Min;
			cSSpaceMorph = new CSRingTaperMorph(z2, ((Point3d)(ref val)).Z, value);
		}
		if (!cSSpaceMorph.IsValid())
		{
			return UpdateError("Invalid Morph");
		}
		oObjects.SetData(data.Duplicate());
		((SpaceMorph)cSSpaceMorph).Tolerance = 0.05;
		oObjects.Data.Morph((SpaceMorph)(object)cSSpaceMorph);
		return base.Update();
	}
}
