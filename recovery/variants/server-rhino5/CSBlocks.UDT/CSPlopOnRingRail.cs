using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.UDT;

[CSBlock("PlopOnRingRail", "{0438D8DB-8536-44af-9C61-1F2350F086BC}")]
public sealed class CSPlopOnRingRail : CSBlock
{
	private sealed class PlopOnRingRail : CSSpaceMorph
	{
		private readonly double _railRadius;

		private Interval _sourceDomainX;

		private Interval _angleDomain;

		private Vector3d _up = Vector3d.ZAxis;

		public PlopOnRingRail(double inputRailRadius, BoundingBox bBox, double startAngle, double endAngle)
		{
			_railRadius = inputRailRadius;
			if (_railRadius <= 0.0)
			{
				_railRadius = 10.0;
			}
			_sourceDomainX = new Interval(bBox.Min.X, bBox.Max.X);
			_angleDomain = new Interval(startAngle, endAngle);
		}

		public override Point3d MorphPoint(Point3d inputPoint)
		{
			Point3d point3d = inputPoint;
			double normalizedParameter = _sourceDomainX.NormalizedParameterAt(inputPoint.X);
			double angleRadians = _angleDomain.ParameterAt(normalizedParameter);
			_up = Vector3d.ZAxis;
			_up.Rotate(angleRadians, Vector3d.YAxis);
			Point3d result = new Point3d(_up * (_railRadius + point3d.Z));
			result.Y = inputPoint.Y;
			return result;
		}
	}

	public CSInputChannel iRailRadius { get; }

	public CSInputChannel iSurface { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iSizeAngle { get; }

	public CSOutputChannel oSurface { get; }

	public CSPlopOnRingRail(string identifier)
		: base(identifier)
	{
		iRailRadius = new CSInputChannel(GetPropertyName(() => iRailRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSurface = new CSInputChannel(GetPropertyName(() => iSurface), this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSizeAngle = new CSInputChannel(GetPropertyName(() => iSizeAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(GetPropertyName(() => oSurface), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		double value = 10.0;
		iRailRadius.GetDouble(ref value);
		double value2 = 0.0;
		iAngle.GetDouble(ref value2);
		double value3 = 40.0;
		iSizeAngle.GetDouble(ref value3);
		BoundingBox boundingBox = BoundingBox.Unset;
		iSurface.Data.GetTightBoundingBox(ref boundingBox);
		if (iSurface == null)
		{
			return UpdateMissing("Missing Object");
		}
		if (iSurface.Data == null)
		{
			return UpdateMissing("Missing Object");
		}
		CSData cSData = iSurface.Data.Duplicate();
		if (cSData == null)
		{
			return UpdateMissing("Missing Object");
		}
		double num = CSBlockUtil.CSUtilMath.ToRadians(value2);
		double startAngle = num - CSBlockUtil.CSUtilMath.ToRadians(value3 / 2.0);
		double endAngle = num + CSBlockUtil.CSUtilMath.ToRadians(value3 / 2.0);
		PlopOnRingRail spaceMorph = new PlopOnRingRail(value, boundingBox, startAngle, endAngle)
		{
			Tolerance = 0.05
		};
		cSData.Morph(spaceMorph);
		oSurface.SetData(cSData);
		return base.Update();
	}
}
