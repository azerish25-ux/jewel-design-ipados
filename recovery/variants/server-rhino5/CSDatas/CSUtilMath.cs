using System;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSDatas;

public static class CSUtilMath
{
	public static Transform GetObjectDisplayXform(BoundingBox boundingBox)
	{
		Transform result = Transform.Identity;
		if (!boundingBox.IsValid)
		{
			return result;
		}
		Transform transform = Transform.Rotation(ToRadians(-90.0), Vector3d.XAxis, boundingBox.Center);
		Transform transform2 = Transform.Rotation(ToRadians(-90.0), Vector3d.ZAxis, boundingBox.Center);
		Vector3d diagonal = boundingBox.Diagonal;
		if (diagonal.Z > 0.1 && diagonal.X > diagonal.Y)
		{
			result = transform;
		}
		else if (diagonal.Z > 0.1 && diagonal.Y > diagonal.X)
		{
			result = transform * transform2;
		}
		return result;
	}

	public static bool GetTransformNoArray(CSData data, out Transform value)
	{
		value = Transform.Unset;
		if (data == null)
		{
			return false;
		}
		if (data is CSDataArray)
		{
			return false;
		}
		if (data is ICSTransform iCSTransform)
		{
			return iCSTransform.GetTransform(out value);
		}
		return false;
	}

	public static bool GetUnscaledXform(Transform xOriginal, out Transform xUp)
	{
		xUp = Transform.Identity;
		if (xOriginal == Transform.Unset)
		{
			return false;
		}
		Vector3d xAxis = Vector3d.XAxis;
		xAxis.Transform(xOriginal);
		Vector3d yAxis = Vector3d.YAxis;
		yAxis.Transform(xOriginal);
		Vector3d zAxis = Vector3d.ZAxis;
		zAxis.Transform(xOriginal);
		double length = xAxis.Length;
		double length2 = yAxis.Length;
		double length3 = zAxis.Length;
		if (length < 0.0001 || length2 < 0.0001 || length3 < 0.0001)
		{
			return false;
		}
		Transform transform = Transform.Scale(Plane.WorldXY, 1.0 / length, 1.0 / length2, 1.0 / length3);
		xUp = xOriginal * transform;
		return true;
	}

	public static bool GetUnscaledXforms(Transform xOriginal, out Transform xUp, out Transform xDown)
	{
		if (GetUnscaledXform(xOriginal, out xUp))
		{
			return xUp.TryGetInverse(out xDown);
		}
		xDown = Transform.Identity;
		return false;
	}

	private static double ToRadians(double angleDegrees)
	{
		return Math.PI / 180.0 * angleDegrees;
	}

	public static bool IsFlipTransform(Transform transform)
	{
		Vector3d xAxis = Vector3d.XAxis;
		xAxis.Transform(transform);
		Vector3d yAxis = Vector3d.YAxis;
		yAxis.Transform(transform);
		Vector3d zAxis = Vector3d.ZAxis;
		zAxis.Transform(transform);
		return Vector3d.CrossProduct(xAxis, yAxis) * zAxis < 0.0;
	}
}
