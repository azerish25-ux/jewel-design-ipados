using System;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSDatas;

public static class CSUtilMath
{
	public static Transform GetObjectDisplayXform(BoundingBox boundingBox)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		Transform result = Transform.Identity;
		if (!((BoundingBox)(ref boundingBox)).IsValid)
		{
			return result;
		}
		Transform val = Transform.Rotation(ToRadians(-90.0), Vector3d.XAxis, ((BoundingBox)(ref boundingBox)).Center);
		Transform val2 = Transform.Rotation(ToRadians(-90.0), Vector3d.ZAxis, ((BoundingBox)(ref boundingBox)).Center);
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		if (((Vector3d)(ref diagonal)).Z > 0.1 && ((Vector3d)(ref diagonal)).X > ((Vector3d)(ref diagonal)).Y)
		{
			result = val;
		}
		else if (((Vector3d)(ref diagonal)).Z > 0.1 && ((Vector3d)(ref diagonal)).Y > ((Vector3d)(ref diagonal)).X)
		{
			result = val * val2;
		}
		return result;
	}

	public static bool GetTransformNoArray(CSData data, out Transform value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		xUp = Transform.Identity;
		if (xOriginal == Transform.Unset)
		{
			return false;
		}
		Vector3d xAxis = Vector3d.XAxis;
		((Vector3d)(ref xAxis)).Transform(xOriginal);
		Vector3d yAxis = Vector3d.YAxis;
		((Vector3d)(ref yAxis)).Transform(xOriginal);
		Vector3d zAxis = Vector3d.ZAxis;
		((Vector3d)(ref zAxis)).Transform(xOriginal);
		double length = ((Vector3d)(ref xAxis)).Length;
		double length2 = ((Vector3d)(ref yAxis)).Length;
		double length3 = ((Vector3d)(ref zAxis)).Length;
		if (length < 0.0001 || length2 < 0.0001 || length3 < 0.0001)
		{
			return false;
		}
		Transform val = Transform.Scale(Plane.WorldXY, 1.0 / length, 1.0 / length2, 1.0 / length3);
		xUp = xOriginal * val;
		return true;
	}

	public static bool GetUnscaledXforms(Transform xOriginal, out Transform xUp, out Transform xDown)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (GetUnscaledXform(xOriginal, out xUp))
		{
			return ((Transform)(ref xUp)).TryGetInverse(ref xDown);
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Vector3d xAxis = Vector3d.XAxis;
		((Vector3d)(ref xAxis)).Transform(transform);
		Vector3d yAxis = Vector3d.YAxis;
		((Vector3d)(ref yAxis)).Transform(transform);
		Vector3d zAxis = Vector3d.ZAxis;
		((Vector3d)(ref zAxis)).Transform(transform);
		return Vector3d.CrossProduct(xAxis, yAxis) * zAxis < 0.0;
	}
}
