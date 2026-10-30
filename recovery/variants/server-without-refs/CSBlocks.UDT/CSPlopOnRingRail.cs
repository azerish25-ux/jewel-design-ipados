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
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			_railRadius = inputRailRadius;
			if (_railRadius <= 0.0)
			{
				_railRadius = 10.0;
			}
			Point3d val = ((BoundingBox)(ref bBox)).Min;
			double x = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref bBox)).Max;
			_sourceDomainX = new Interval(x, ((Point3d)(ref val)).X);
			_angleDomain = new Interval(startAngle, endAngle);
		}

		public override Point3d MorphPoint(Point3d inputPoint)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			Point3d result = inputPoint;
			double num = ((Interval)(ref _sourceDomainX)).NormalizedParameterAt(((Point3d)(ref inputPoint)).X);
			double num2 = ((Interval)(ref _angleDomain)).ParameterAt(num);
			_up = Vector3d.ZAxis;
			((Vector3d)(ref _up)).Rotate(num2, Vector3d.YAxis);
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(_up * (_railRadius + ((Point3d)(ref result)).Z));
			((Point3d)(ref val)).Y = ((Point3d)(ref inputPoint)).Y;
			result = val;
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
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
		PlopOnRingRail plopOnRingRail = new PlopOnRingRail(value, boundingBox, startAngle, endAngle);
		((SpaceMorph)plopOnRingRail).Tolerance = 0.05;
		PlopOnRingRail spaceMorph = plopOnRingRail;
		cSData.Morph((SpaceMorph)(object)spaceMorph);
		oSurface.SetData(cSData);
		return base.Update();
	}
}
