using System;
using CS;
using CSCompatibility;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.UDT;

public sealed class PullToRailMorph : CSSpaceMorph
{
	private readonly OnXform xNegativeRotation = new OnXform();

	private readonly OnXform xPositiveRotation = new OnXform();

	private readonly bool pullTop;

	private readonly double angle;

	private readonly double railRadius;

	private readonly double zDrop;

	private readonly double zHeight;

	private readonly double zMin;

	public PullToRailMorph(double iangle, bool inputPullTop, double inputRailRadius, OnBoundingBox inputBoundingBox)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		angle = iangle;
		xNegativeRotation.Rotation(0.0 - angle, OnUtil.On_yaxis, OnUtil.On_origin);
		xPositiveRotation.Rotation(angle, OnUtil.On_yaxis, OnUtil.On_origin);
		pullTop = inputPullTop;
		railRadius = inputRailRadius;
		if (railRadius <= 0.0)
		{
			railRadius = 10.0;
		}
		zDrop = inputBoundingBox.Min().z - railRadius;
		zHeight = inputBoundingBox.Max().z - inputBoundingBox.Min().z;
		zMin = inputBoundingBox.Min().z;
	}

	public override Point3d MorphPoint(Point3d inputPoint)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		On3dPoint val = new On3dPoint((IOn3dPoint)(object)inputPoint.Convert());
		if (railRadius > 0.0)
		{
			val.Transform((IOnXform)(object)xNegativeRotation);
			double num = railRadius * railRadius - val.x * val.x;
			if (num >= 0.0)
			{
				double num2 = railRadius - Math.Sqrt(num);
				double num3 = 1.0 - (val.z - zMin) / zHeight;
				if (pullTop)
				{
					val.z -= num2 + zDrop * num3;
				}
				else
				{
					val.z -= (num2 + zDrop) * num3;
				}
			}
			val.Transform((IOnXform)(object)xPositiveRotation);
		}
		return val.Convert();
	}

	public static OnXform TopCenterXformFromAngle(double angle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		OnXform val = new OnXform();
		val.Rotation(0.0 - angle, OnUtil.On_yaxis, OnUtil.On_origin);
		return val;
	}
}
