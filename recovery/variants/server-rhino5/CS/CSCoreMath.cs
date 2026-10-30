using System;
using CSCompatibility;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CS;

public static class CSCoreMath
{
	private static double On_RADIANS_TO_DEGREES => 180.0 / Math.PI;

	private static double On_DEGREES_TO_RADIANS => Math.PI / 180.0;

	public static OnXform Orient3Point(IOn3dPoint source0, IOn3dVector source1, IOn3dVector source2, IOn3dPoint destination0, IOn3dVector destination1, IOn3dVector destination2)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Expected O, but got Unknown
		On3dVector val = new On3dVector(source1);
		On3dVector val2 = new On3dVector(source2);
		On3dVector val3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val, (IOn3dVector)(object)val2);
		val2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val3, (IOn3dVector)(object)val);
		val.Unitize();
		val2.Unitize();
		val3.Unitize();
		OnXform val4 = new OnXform();
		val4[0, 0] = val.x;
		val4[0, 1] = val.y;
		val4[0, 2] = val.z;
		val4[1, 0] = val2.x;
		val4[1, 1] = val2.y;
		val4[1, 2] = val2.z;
		val4[2, 0] = val3.x;
		val4[2, 1] = val3.y;
		val4[2, 2] = val3.z;
		val4[0, 3] = (0.0 - source0.x) * val.x - source0.y * val.y - source0.z * val.z;
		val4[1, 3] = (0.0 - source0.x) * val2.x - source0.y * val2.y - source0.z * val2.z;
		val4[2, 3] = (0.0 - source0.x) * val3.x - source0.y * val3.y - source0.z * val3.z;
		On3dVector val5 = new On3dVector(destination1);
		On3dVector val6 = new On3dVector(destination2);
		On3dVector val7 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val5, (IOn3dVector)(object)val6);
		val6 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val7, (IOn3dVector)(object)val5);
		val5.Unitize();
		val6.Unitize();
		val7.Unitize();
		return new OnXform
		{
			[0, 0] = val5.x,
			[1, 0] = val5.y,
			[2, 0] = val5.z,
			[0, 1] = val6.x,
			[1, 1] = val6.y,
			[2, 1] = val6.z,
			[0, 2] = val7.x,
			[1, 2] = val7.y,
			[2, 2] = val7.z,
			[0, 3] = destination0.x,
			[1, 3] = destination0.y,
			[2, 3] = destination0.z
		} * (IOnXform)(object)val4;
	}

	public static Transform Orient3Point(Point3d source0, Vector3d source1, Vector3d source2, Point3d destination0, Vector3d destination1, Vector3d destination2)
	{
		Vector3d vector3d = new Vector3d(source1);
		Vector3d a = Vector3d.CrossProduct(b: new Vector3d(source2), a: vector3d);
		Vector3d vector3d2 = Vector3d.CrossProduct(a, vector3d);
		vector3d.Unitize();
		vector3d2.Unitize();
		a.Unitize();
		Transform transform = new Transform(1.0);
		transform.M00 = vector3d.X;
		transform.M01 = vector3d.Y;
		transform.M02 = vector3d.Z;
		transform.M10 = vector3d2.X;
		transform.M11 = vector3d2.Y;
		transform.M12 = vector3d2.Z;
		transform.M20 = a.X;
		transform.M21 = a.Y;
		transform.M22 = a.Z;
		transform.M03 = (0.0 - source0.X) * vector3d.X - source0.Y * vector3d.Y - source0.Z * vector3d.Z;
		transform.M13 = (0.0 - source0.X) * vector3d2.X - source0.Y * vector3d2.Y - source0.Z * vector3d2.Z;
		transform.M23 = (0.0 - source0.X) * a.X - source0.Y * a.Y - source0.Z * a.Z;
		Transform transform2 = transform;
		Vector3d vector3d3 = new Vector3d(destination1);
		Vector3d a2 = Vector3d.CrossProduct(b: new Vector3d(destination2), a: vector3d3);
		Vector3d vector3d4 = Vector3d.CrossProduct(a2, vector3d3);
		vector3d3.Unitize();
		vector3d4.Unitize();
		a2.Unitize();
		transform = new Transform(1.0);
		transform.M00 = vector3d3.X;
		transform.M10 = vector3d3.Y;
		transform.M20 = vector3d3.Z;
		transform.M01 = vector3d4.X;
		transform.M11 = vector3d4.Y;
		transform.M21 = vector3d4.Z;
		transform.M02 = a2.X;
		transform.M12 = a2.Y;
		transform.M22 = a2.Z;
		transform.M03 = destination0.X;
		transform.M13 = destination0.Y;
		transform.M23 = destination0.Z;
		return transform * transform2;
	}

	public static Transform Rotation(double angleRadians, Vector3d rotationAxis, Point3d rotationCenter)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		On3dVector val = CSConvert.Vector3dCopyToOn3dVector(rotationAxis);
		On3dPoint val2 = CSConvert.Point3dCopyToOn3dPoint(rotationCenter);
		OnXform val3 = new OnXform();
		val3.Identity();
		val3.Rotation(angleRadians, (IOn3dVector)(object)val, (IOn3dPoint)(object)val2);
		return CSConvert.XformCopyToTransform((IOnXform)val3);
	}

	public static double ToDegrees(double angleRadians)
	{
		return On_RADIANS_TO_DEGREES * angleRadians;
	}

	public static double ToRadians(double angleDegrees)
	{
		return On_DEGREES_TO_RADIANS * angleDegrees;
	}
}
