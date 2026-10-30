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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(source1);
		Vector3d val2 = default(Vector3d);
		((Vector3d)(ref val2))._002Ector(source2);
		Vector3d val3 = Vector3d.CrossProduct(val, val2);
		val2 = Vector3d.CrossProduct(val3, val);
		((Vector3d)(ref val)).Unitize();
		((Vector3d)(ref val2)).Unitize();
		((Vector3d)(ref val3)).Unitize();
		Transform val4 = default(Transform);
		((Transform)(ref val4))._002Ector(1.0);
		((Transform)(ref val4)).M00 = ((Vector3d)(ref val)).X;
		((Transform)(ref val4)).M01 = ((Vector3d)(ref val)).Y;
		((Transform)(ref val4)).M02 = ((Vector3d)(ref val)).Z;
		((Transform)(ref val4)).M10 = ((Vector3d)(ref val2)).X;
		((Transform)(ref val4)).M11 = ((Vector3d)(ref val2)).Y;
		((Transform)(ref val4)).M12 = ((Vector3d)(ref val2)).Z;
		((Transform)(ref val4)).M20 = ((Vector3d)(ref val3)).X;
		((Transform)(ref val4)).M21 = ((Vector3d)(ref val3)).Y;
		((Transform)(ref val4)).M22 = ((Vector3d)(ref val3)).Z;
		((Transform)(ref val4)).M03 = (0.0 - ((Point3d)(ref source0)).X) * ((Vector3d)(ref val)).X - ((Point3d)(ref source0)).Y * ((Vector3d)(ref val)).Y - ((Point3d)(ref source0)).Z * ((Vector3d)(ref val)).Z;
		((Transform)(ref val4)).M13 = (0.0 - ((Point3d)(ref source0)).X) * ((Vector3d)(ref val2)).X - ((Point3d)(ref source0)).Y * ((Vector3d)(ref val2)).Y - ((Point3d)(ref source0)).Z * ((Vector3d)(ref val2)).Z;
		((Transform)(ref val4)).M23 = (0.0 - ((Point3d)(ref source0)).X) * ((Vector3d)(ref val3)).X - ((Point3d)(ref source0)).Y * ((Vector3d)(ref val3)).Y - ((Point3d)(ref source0)).Z * ((Vector3d)(ref val3)).Z;
		Transform val5 = val4;
		Vector3d val6 = default(Vector3d);
		((Vector3d)(ref val6))._002Ector(destination1);
		Vector3d val7 = default(Vector3d);
		((Vector3d)(ref val7))._002Ector(destination2);
		Vector3d val8 = Vector3d.CrossProduct(val6, val7);
		val7 = Vector3d.CrossProduct(val8, val6);
		((Vector3d)(ref val6)).Unitize();
		((Vector3d)(ref val7)).Unitize();
		((Vector3d)(ref val8)).Unitize();
		((Transform)(ref val4))._002Ector(1.0);
		((Transform)(ref val4)).M00 = ((Vector3d)(ref val6)).X;
		((Transform)(ref val4)).M10 = ((Vector3d)(ref val6)).Y;
		((Transform)(ref val4)).M20 = ((Vector3d)(ref val6)).Z;
		((Transform)(ref val4)).M01 = ((Vector3d)(ref val7)).X;
		((Transform)(ref val4)).M11 = ((Vector3d)(ref val7)).Y;
		((Transform)(ref val4)).M21 = ((Vector3d)(ref val7)).Z;
		((Transform)(ref val4)).M02 = ((Vector3d)(ref val8)).X;
		((Transform)(ref val4)).M12 = ((Vector3d)(ref val8)).Y;
		((Transform)(ref val4)).M22 = ((Vector3d)(ref val8)).Z;
		((Transform)(ref val4)).M03 = ((Point3d)(ref destination0)).X;
		((Transform)(ref val4)).M13 = ((Point3d)(ref destination0)).Y;
		((Transform)(ref val4)).M23 = ((Point3d)(ref destination0)).Z;
		return val4 * val5;
	}

	public static Transform Rotation(double angleRadians, Vector3d rotationAxis, Point3d rotationCenter)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
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
