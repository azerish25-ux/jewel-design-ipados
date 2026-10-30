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
		On3dVector on3dVector = new On3dVector(source1);
		On3dVector arg = new On3dVector(source2);
		On3dVector on3dVector2 = OnUtil.ON_CrossProduct(on3dVector, arg);
		arg = OnUtil.ON_CrossProduct(on3dVector2, on3dVector);
		on3dVector.Unitize();
		arg.Unitize();
		on3dVector2.Unitize();
		OnXform onXform = new OnXform();
		onXform.set_m_xform(0, 0, on3dVector.x);
		onXform.set_m_xform(0, 1, on3dVector.y);
		onXform.set_m_xform(0, 2, on3dVector.z);
		onXform.set_m_xform(1, 0, arg.x);
		onXform.set_m_xform(1, 1, arg.y);
		onXform.set_m_xform(1, 2, arg.z);
		onXform.set_m_xform(2, 0, on3dVector2.x);
		onXform.set_m_xform(2, 1, on3dVector2.y);
		onXform.set_m_xform(2, 2, on3dVector2.z);
		onXform.set_m_xform(0, 3, (0.0 - source0.x) * on3dVector.x - source0.y * on3dVector.y - source0.z * on3dVector.z);
		onXform.set_m_xform(1, 3, (0.0 - source0.x) * arg.x - source0.y * arg.y - source0.z * arg.z);
		onXform.set_m_xform(2, 3, (0.0 - source0.x) * on3dVector2.x - source0.y * on3dVector2.y - source0.z * on3dVector2.z);
		On3dVector on3dVector3 = new On3dVector(destination1);
		On3dVector arg2 = new On3dVector(destination2);
		On3dVector on3dVector4 = OnUtil.ON_CrossProduct(on3dVector3, arg2);
		arg2 = OnUtil.ON_CrossProduct(on3dVector4, on3dVector3);
		on3dVector3.Unitize();
		arg2.Unitize();
		on3dVector4.Unitize();
		return new OnXform
		{
			[0, 0] = on3dVector3.x,
			[1, 0] = on3dVector3.y,
			[2, 0] = on3dVector3.z,
			[0, 1] = arg2.x,
			[1, 1] = arg2.y,
			[2, 1] = arg2.z,
			[0, 2] = on3dVector4.x,
			[1, 2] = on3dVector4.y,
			[2, 2] = on3dVector4.z,
			[0, 3] = destination0.x,
			[1, 3] = destination0.y,
			[2, 3] = destination0.z
		} * onXform;
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
		On3dVector rotation_axis = CSConvert.Vector3dCopyToOn3dVector(rotationAxis);
		On3dPoint rotation_center = CSConvert.Point3dCopyToOn3dPoint(rotationCenter);
		OnXform onXform = new OnXform();
		onXform.Identity();
		onXform.Rotation(angleRadians, rotation_axis, rotation_center);
		return CSConvert.XformCopyToTransform(onXform);
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
