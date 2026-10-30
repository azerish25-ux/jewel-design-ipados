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
		On3dPoint on3dPoint = new On3dPoint(inputPoint.Convert());
		if (railRadius > 0.0)
		{
			on3dPoint.Transform(xNegativeRotation);
			double num = railRadius * railRadius - on3dPoint.x * on3dPoint.x;
			if (num >= 0.0)
			{
				double num2 = railRadius - Math.Sqrt(num);
				double num3 = 1.0 - (on3dPoint.z - zMin) / zHeight;
				if (pullTop)
				{
					on3dPoint.z -= num2 + zDrop * num3;
				}
				else
				{
					on3dPoint.z -= (num2 + zDrop) * num3;
				}
			}
			on3dPoint.Transform(xPositiveRotation);
		}
		return on3dPoint.Convert();
	}

	public static OnXform TopCenterXformFromAngle(double angle)
	{
		OnXform onXform = new OnXform();
		onXform.Rotation(0.0 - angle, OnUtil.On_yaxis, OnUtil.On_origin);
		return onXform;
	}
}
