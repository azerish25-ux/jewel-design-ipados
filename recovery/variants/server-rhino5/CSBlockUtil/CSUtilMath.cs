using System;
using System.Collections.Generic;
using System.Linq;
using CSCompatibility;
using RMA.OpenNURBS;
using Rhino;
using Rhino.Geometry;

namespace CSBlockUtil;

public static class CSUtilMath
{
	public static double On_RADIANS_TO_DEGREES => 180.0 / Math.PI;

	public static double On_DEGREES_TO_RADIANS => Math.PI / 180.0;

	public static bool AngleBetweenPoints(On3dPoint originPoint, On3dPoint xPoint, On3dPoint yPoint, out double angle)
	{
		angle = 0.0;
		if (originPoint == (IOn3dPoint)(object)xPoint || originPoint == (IOn3dPoint)(object)yPoint)
		{
			return false;
		}
		On3dVector inputVector = yPoint - (IOn3dPoint)(object)originPoint;
		On3dVector inputVector2 = xPoint - (IOn3dPoint)(object)originPoint;
		angle = AngleBetweenVectors((IOn3dVector)(object)inputVector, (IOn3dVector)(object)inputVector2);
		return true;
	}

	public static double AngleBetweenVectors(IOn3dVector inputVector1, IOn3dVector inputVector2)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		On3dVector val = new On3dVector(inputVector1);
		On3dVector val2 = new On3dVector(inputVector2);
		val.Unitize();
		val2.Unitize();
		double num = OnUtil.ON_DotProduct((IOn3dVector)val2, (IOn3dVector)(object)val);
		if (num < -1.0)
		{
			num = -1.0;
		}
		else if (num > 1.0)
		{
			num = 1.0;
		}
		return Math.Acos(num);
	}

	public static double AngleBetweenVectors(Vector3d inputVector1, Vector3d inputVector2)
	{
		Vector3d vector = new Vector3d(inputVector1);
		Vector3d vector2 = new Vector3d(inputVector2);
		vector.Unitize();
		vector2.Unitize();
		double num = Vector3d.Multiply(vector2, vector);
		if (num < -1.0)
		{
			num = -1.0;
		}
		else if (num > 1.0)
		{
			num = 1.0;
		}
		return Math.Acos(num);
	}

	public static double AngleBetweenVectorsWithDirection(IOn3dVector inputVector1, IOn3dVector inputVector2, IOn3dVector inputDirection)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		On3dVector val = new On3dVector(inputVector1);
		val.Unitize();
		On3dVector val2 = new On3dVector(inputVector2);
		val2.Unitize();
		On3dVector val3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val2, (IOn3dVector)(object)val);
		if (val3.Length() < 1E-05)
		{
			return 0.0;
		}
		val3.Unitize();
		double num = OnUtil.ON_DotProduct((IOn3dVector)(object)val3, inputDirection);
		double num2 = OnUtil.ON_DotProduct((IOn3dVector)(object)val2, (IOn3dVector)(object)val);
		if (num2 < -1.0)
		{
			num2 = -1.0;
		}
		else if (num2 > 1.0)
		{
			num2 = 1.0;
		}
		double num3 = Math.Acos(num2);
		if (num < 0.0)
		{
			num3 = 0.0 - num3;
		}
		return num3;
	}

	public static double AngleBetweenVectorsWithDirection(Vector3d inputVector1, Vector3d inputVector2, Vector3d inputDirection)
	{
		Vector3d vector3d = new Vector3d(inputVector1);
		vector3d.Unitize();
		Vector3d vector3d2 = new Vector3d(inputVector2);
		vector3d2.Unitize();
		Vector3d vector3d3 = Vector3d.CrossProduct(vector3d2, vector3d);
		if (vector3d3.Length < 1E-05)
		{
			return 0.0;
		}
		vector3d3.Unitize();
		double num = vector3d3 * inputDirection;
		double num2 = vector3d2 * vector3d;
		if (num2 < -1.0)
		{
			num2 = -1.0;
		}
		else if (num2 > 1.0)
		{
			num2 = 1.0;
		}
		double num3 = Math.Acos(num2);
		if (num < 0.0)
		{
			num3 = 0.0 - num3;
		}
		return num3;
	}

	public static double AngleOfPointD(double x, double y)
	{
		if (y == 0.0)
		{
			if (x > 0.0)
			{
				return OnUtil.On_PI * 0.5;
			}
			return OnUtil.On_PI * 1.5;
		}
		if (y < 0.0)
		{
			return Math.Atan(x / y) + OnUtil.On_PI;
		}
		if (x < 0.0)
		{
			return Math.Atan(x / y) + OnUtil.On_PI * 2.0;
		}
		return Math.Atan(x / y);
	}

	public static On3dVector AverageNormalAt(IOnCurve curve, IOn3dVector planarNormal, IOn3dPoint pt, double tolerance = 0.01)
	{
		On3dVector val = AverageTangentAt(curve, pt, tolerance);
		val.Unitize();
		return AverageNormalAt(planarNormal, (IOn3dVector)(object)val);
	}

	public static Vector3d AverageNormalAt(Curve curve, Vector3d planarNormal, Point3d pt, double tolerance = 0.01)
	{
		Vector3d curveTangent = AverageTangentAt(curve, pt, tolerance);
		curveTangent.Unitize();
		return AverageNormalAt(planarNormal, curveTangent);
	}

	public static On3dVector AverageNormalAt(IOnCurve curve, IOn3dVector planarNormal, double param, double tolerance = 0.01)
	{
		On3dVector val = AverageTangentAt(curve, param, tolerance);
		val.Unitize();
		return AverageNormalAt(planarNormal, (IOn3dVector)(object)val);
	}

	public static Vector3d AverageNormalAt(Curve curve, Vector3d planarNormal, double param, double tolerance = 0.01)
	{
		Vector3d curveTangent = AverageTangentAt(curve, param, tolerance);
		curveTangent.Unitize();
		return AverageNormalAt(planarNormal, curveTangent);
	}

	public static On3dVector AverageNormalAt(IOn3dVector planarNormal, IOn3dVector curveTangent)
	{
		On3dVector obj = OnUtil.ON_CrossProduct(planarNormal, curveTangent);
		obj.Unitize();
		return obj;
	}

	public static Vector3d AverageNormalAt(Vector3d planarNormal, Vector3d curveTangent)
	{
		Vector3d result = Vector3d.CrossProduct(planarNormal, curveTangent);
		result.Unitize();
		return result;
	}

	public static bool AveragePlaneOfCurve(IOnCurve curve, out OnPlane plane)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		plane = new OnPlane();
		if (curve.IsPlanar(plane))
		{
			return true;
		}
		On3dPoint val = curve.PointAtStart();
		double[] array = new double[5] { 0.25, 0.33, 0.5, 0.66, 0.75 };
		double[] array2 = new double[array.Length];
		curve.GetNormalizedArcLengthPoints(array, ref array2);
		List<On3dPoint> list = ((IEnumerable<double>)array2).Select((Func<double, On3dPoint>)curve.PointAt).ToList();
		On3dVector val2 = new On3dVector(0.0, 0.0, 0.0);
		int num = 0;
		for (int i = 0; i < array2.Length - 1; i++)
		{
			On3dVector val3 = list[i] - (IOn3dPoint)(object)val;
			val3.Unitize();
			On3dVector val4 = list[i + 1] - (IOn3dPoint)(object)val;
			val4.Unitize();
			double num2 = OnUtil.ON_DotProduct((IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
			if (!(num2 < 0.01) && !(num2 > 3.14))
			{
				On3dVector val5 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
				val5.Unitize();
				val2 += (IOn3dVector)(object)val5;
				num++;
			}
		}
		if (num == 0)
		{
			return false;
		}
		val2 /= (double)num;
		plane = new OnPlane((IOn3dPoint)(object)val, (IOn3dVector)(object)val2);
		return true;
	}

	public static bool AveragePlaneOfCurve(Curve curve, out Plane plane)
	{
		if (curve.TryGetPlane(out plane))
		{
			return true;
		}
		Point3d pointAtStart = curve.PointAtStart;
		double[] array = new double[5] { 0.25, 0.33, 0.5, 0.66, 0.75 };
		List<Point3d> list = array.Select(curve.PointAtNormalizedLength).ToList();
		Vector3d normal = new Vector3d(0.0, 0.0, 0.0);
		int num = 0;
		for (int i = 0; i < array.Length - 1; i++)
		{
			Vector3d vector3d = list[i] - pointAtStart;
			vector3d.Unitize();
			Vector3d vector3d2 = list[i + 1] - pointAtStart;
			vector3d2.Unitize();
			double num2 = vector3d * vector3d2;
			if (!(num2 < 0.01) && !(num2 > 3.14))
			{
				Vector3d vector3d3 = Vector3d.CrossProduct(vector3d, vector3d2);
				vector3d3.Unitize();
				normal += vector3d3;
				num++;
			}
		}
		if (num == 0)
		{
			return false;
		}
		normal /= (double)num;
		plane = new Plane(pointAtStart, normal);
		return true;
	}

	public static Vector3d AverageTangentAt(Curve curve, Point3d pt, double tolerance = 0.01)
	{
		if (!curve.ClosestPoint(pt, out var t))
		{
			return Vector3d.Unset;
		}
		return AverageTangentAt(curve, t, tolerance);
	}

	public static Vector3d AverageTangentAt(Curve curve, double param, double tolerance = 0.01)
	{
		double num = param - tolerance;
		if (num < curve.Domain.Min)
		{
			num += curve.Domain.Max - curve.Domain.Min;
		}
		Vector3d vector3d = curve.TangentAt(num);
		double num2 = param + tolerance;
		if (num2 > curve.Domain.Max)
		{
			num2 -= curve.Domain.Max - curve.Domain.Min;
		}
		Vector3d vector3d2 = curve.TangentAt(num2);
		return vector3d + vector3d2;
	}

	public static On3dVector AverageTangentAt(IOnCurve curve, IOn3dPoint pt, double tolerance = 0.01)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		double param = -1.23432101234321E+308;
		if (!curve.GetClosestPoint(pt, ref param))
		{
			return new On3dVector(OnUtil.On_UNSET_VECTOR);
		}
		return AverageTangentAt(curve, param, tolerance);
	}

	public static On3dVector AverageTangentAt(IOnCurve curve, double param, double tolerance = 0.01)
	{
		double num = param - tolerance;
		if (num < curve.Domain().Min())
		{
			num += curve.Domain().Max() - curve.Domain().Min();
		}
		On3dVector obj = curve.TangentAt(num);
		double num2 = param + tolerance;
		if (num2 > curve.Domain().Max())
		{
			num2 -= curve.Domain().Max() - curve.Domain().Min();
		}
		On3dVector val = curve.TangentAt(num2);
		return obj + (IOn3dVector)(object)val;
	}

	public static Transform ChangeBasis(Point3d initialLocation, Vector3d initialBaseX, Vector3d initialBaseY, Vector3d initialBaseZ)
	{
		return ChangeBasis(initialLocation, initialBaseX, initialBaseY, initialBaseZ, Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis, Vector3d.ZAxis);
	}

	public static Transform ChangeBasis(Point3d initialLocation, Vector3d initialBaseX, Vector3d initialBaseY, Vector3d initialBaseZ, Point3d finalLocation, Vector3d finalBaseX, Vector3d finalBaseY, Vector3d finalBaseZ)
	{
		Transform transform = Transform.Translation(new Vector3d(finalLocation) * -1.0);
		Transform transform2 = Transform.ChangeBasis(initialBaseX, initialBaseY, initialBaseZ, finalBaseX, finalBaseY, finalBaseZ);
		return Transform.Translation(new Vector3d(initialLocation)) * transform2 * transform;
	}

	public static double Clamp(double value, double lowerBound, double upperBound)
	{
		if (value < lowerBound)
		{
			return lowerBound;
		}
		if (value > upperBound)
		{
			return upperBound;
		}
		return value;
	}

	public static int Clamp(int value, int lowerBound, int upperBound)
	{
		if (value < lowerBound)
		{
			return lowerBound;
		}
		if (value > upperBound)
		{
			return upperBound;
		}
		return value;
	}

	public static bool CurveClockwise(IOnCurve curve)
	{
		bool result = false;
		double num = 0.0;
		double num2 = curve.Domain().Min();
		double num3 = curve.Domain().Max();
		On3dPoint val = ((IOnGeometry)curve).BoundingBox().Center();
		double num4 = (num3 - num2) / 20.0;
		double num5 = 0.0;
		while (num5 < num3)
		{
			On3dPoint val2 = curve.PointAt(num5);
			num5 += num4;
			On3dPoint val3 = curve.PointAt(num5);
			double num6 = Math.Atan2(val.y - val2.y, val.x - val2.x);
			double num7 = Math.Atan2(val3.y - val2.y, val3.x - val2.y);
			num += num6 - num7;
		}
		if (num < 0.0)
		{
			result = true;
		}
		return result;
	}

	public static double DistanceSquared(IOn3dPoint point1, IOn3dPoint point2)
	{
		double num = point1.x - point2.x;
		double num2 = point1.y - point2.y;
		double num3 = point1.z - point2.z;
		return num * num + num2 * num2 + num3 * num3;
	}

	public static bool DoubleEqual(double a, double b)
	{
		double num = 0.0001;
		if (b - num < a)
		{
			return a < b + num;
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

	public static bool GetUnscaledXform(IOnXform xOriginal, out OnXform xUp)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		xUp = new OnXform();
		xUp.Identity();
		if (xOriginal == null)
		{
			return false;
		}
		On3dVector val = new On3dVector(OnUtil.On_xaxis);
		val.Transform(xOriginal);
		On3dVector val2 = new On3dVector(OnUtil.On_yaxis);
		val2.Transform(xOriginal);
		On3dVector val3 = new On3dVector(OnUtil.On_zaxis);
		val3.Transform(xOriginal);
		double num = val.Length();
		double num2 = val2.Length();
		double num3 = val3.Length();
		if (num < 0.0001 || num2 < 0.0001 || num3 < 0.0001)
		{
			return false;
		}
		OnXform val4 = new OnXform();
		val4.Scale(1.0 / num, 1.0 / num2, 1.0 / num3);
		xUp = new OnXform(xOriginal) * (IOnXform)(object)val4;
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

	public static bool GetUnscaledXforms(IOnXform xOriginal, out OnXform xUp, out OnXform xDown)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		xUp = new OnXform();
		xUp.Identity();
		xDown = new OnXform();
		xDown.Identity();
		if (xOriginal == null)
		{
			return false;
		}
		On3dVector val = new On3dVector(OnUtil.On_xaxis);
		val.Transform(xOriginal);
		On3dVector val2 = new On3dVector(OnUtil.On_yaxis);
		val2.Transform(xOriginal);
		On3dVector val3 = new On3dVector(OnUtil.On_zaxis);
		val3.Transform(xOriginal);
		double num = val.Length();
		double num2 = val2.Length();
		double num3 = val3.Length();
		if (num < 0.0001 || num2 < 0.0001 || num3 < 0.0001)
		{
			return false;
		}
		OnXform val4 = new OnXform();
		val4.Scale(1.0 / num, 1.0 / num2, 1.0 / num3);
		xUp = new OnXform(xOriginal) * (IOnXform)(object)val4;
		xDown = xUp.Inverse();
		return true;
	}

	public static bool IntersectLineAndCircle(IOn3dPoint inputC, IOn3dVector inputVC, double radius, ref On3dPoint intersectPoint)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		On3dVector val = new On3dVector(inputVC);
		val.Unitize();
		double x = inputC.x;
		double z = inputC.z;
		double x2 = val.x;
		double z2 = val.z;
		double num = x2 * x2 + z2 * z2;
		double num2 = 2.0 * x * x2 + 2.0 * z * z2;
		double num3 = x * x + z * z - radius * radius;
		double num4 = num2 * num2 - 4.0 * num * num3;
		if (num4 < 0.0)
		{
			return false;
		}
		num4 = Math.Sqrt(num4);
		double num5 = (0.0 - num2 + num4) / (2.0 * num);
		double num6 = (0.0 - num2 - num4) / (2.0 * num);
		double num7 = ((Math.Abs(num5) < Math.Abs(num6)) ? num5 : num6);
		intersectPoint = new On3dPoint(inputC) + (IOn3dVector)(object)(val * num7);
		return true;
	}

	public static bool IntersectLineAndCircle(Point3d inputC, Vector3d inputVC, double radius, ref Point3d intersectPoint)
	{
		Vector3d vector3d = new Vector3d(inputVC);
		vector3d.Unitize();
		double x = inputC.X;
		double z = inputC.Z;
		double x2 = vector3d.X;
		double z2 = vector3d.Z;
		double num = x2 * x2 + z2 * z2;
		double num2 = 2.0 * x * x2 + 2.0 * z * z2;
		double num3 = x * x + z * z - radius * radius;
		double num4 = num2 * num2 - 4.0 * num * num3;
		if (num4 < 0.0)
		{
			return false;
		}
		num4 = Math.Sqrt(num4);
		double num5 = (0.0 - num2 + num4) / (2.0 * num);
		double num6 = (0.0 - num2 - num4) / (2.0 * num);
		double num7 = ((Math.Abs(num5) < Math.Abs(num6)) ? num5 : num6);
		intersectPoint = new Point3d(inputC) + vector3d * num7;
		return true;
	}

	public static bool LineInt(out On3dPoint outpoint, IOn3dPoint point0, IOn3dVector vector0, IOn3dPoint point1, IOn3dVector vector1)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0078: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c6: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0114: Expected O, but got Unknown
		outpoint = new On3dPoint();
		double x = 0.0;
		double y = 0.0;
		On3dPoint val = On3dPoint.op_Implicit((IOn3dVector)(object)(new On3dPoint(point1) - point0));
		if (SymSolve(out x, out y, vector0.x, 0.0 - vector1.x, val.x, vector0.z, 0.0 - vector1.z, val.z))
		{
			outpoint = new On3dPoint(point0) + (IOn3dVector)(object)(new On3dVector(vector0) * x);
			return true;
		}
		if (SymSolve(out x, out y, vector0.x, 0.0 - vector1.x, val.x, vector0.y, 0.0 - vector1.y, val.y))
		{
			outpoint = new On3dPoint(point0) + (IOn3dVector)(object)(new On3dVector(vector0) * x);
			return true;
		}
		if (SymSolve(out x, out y, vector0.y, 0.0 - vector1.y, val.y, vector0.z, 0.0 - vector1.z, val.z))
		{
			outpoint = new On3dPoint(point0) + (IOn3dVector)(object)(new On3dVector(vector0) * x);
			return true;
		}
		return false;
	}

	public static bool LineInt(out Point3d outpoint, Point3d point0, Vector3d vector0, Point3d point1, Vector3d vector1)
	{
		outpoint = Point3d.Unset;
		double x = 0.0;
		double y = 0.0;
		Vector3d vector3d = point1 - point0;
		if (SymSolve(out x, out y, vector0.X, 0.0 - vector1.X, vector3d.X, vector0.Z, 0.0 - vector1.Z, vector3d.Z))
		{
			outpoint = point0 + vector0 * x;
			return true;
		}
		if (SymSolve(out x, out y, vector0.X, 0.0 - vector1.X, vector3d.X, vector0.Y, 0.0 - vector1.Y, vector3d.Y))
		{
			outpoint = point0 + vector0 * x;
			return true;
		}
		if (SymSolve(out x, out y, vector0.Y, 0.0 - vector1.Y, vector3d.Y, vector0.Z, 0.0 - vector1.Z, vector3d.Z))
		{
			outpoint = point0 + vector0 * x;
			return true;
		}
		return false;
	}

	public static bool MatrixCompare(IOnXform x0, IOnXform x1)
	{
		if (x0 == null || x1 == null)
		{
			return false;
		}
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				if (Math.Abs(x0[i, j] - x1[i, j]) > 1E-06)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static bool MatrixCompare(Transform x0, Transform x1)
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				if (!RhinoMath.EpsilonEquals(x0[i, j], x1[i, j], 1E-06))
				{
					return false;
				}
			}
		}
		return true;
	}

	public static OnXform Orient3Point(IOn3dPoint source0, IOn3dPoint source1, IOn3dPoint source2, IOn3dPoint destination0, IOn3dPoint destination1, IOn3dPoint destination2)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		On3dVector val = new On3dPoint(source1) - source0;
		On3dVector val2 = new On3dPoint(source2) - source0;
		On3dVector val3 = new On3dPoint(destination1) - destination0;
		On3dVector val4 = new On3dPoint(destination2) - destination0;
		if (destination0.Equals(destination1))
		{
			On3dVector val5 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val2, (IOn3dVector)(object)val4);
			double num = Math.Acos(Clamp(OnUtil.ON_DotProduct((IOn3dVector)(object)val2, (IOn3dVector)(object)val4), -1.0, 1.0));
			val3 = new On3dVector((IOn3dVector)(object)val);
			val3.Rotate(num, (IOn3dVector)(object)val5);
		}
		else if (destination0.Equals(destination2))
		{
			On3dVector val6 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val, (IOn3dVector)(object)val3);
			double num2 = Math.Acos(Clamp(OnUtil.ON_DotProduct((IOn3dVector)(object)val, (IOn3dVector)(object)val3), -1.0, 1.0));
			val4 = new On3dVector((IOn3dVector)(object)val2);
			val4.Rotate(num2, (IOn3dVector)(object)val6);
		}
		return Orient3Point(source0, (IOn3dVector)(object)val, (IOn3dVector)(object)val2, destination0, (IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
	}

	public static Transform Orient3Point(Point3d source0, Point3d source1, Point3d source2, Point3d destination0, Point3d destination1, Point3d destination2)
	{
		Vector3d vector3d = new Point3d(source1) - source0;
		Vector3d vector3d2 = new Point3d(source2) - source0;
		Vector3d vector3d3 = new Point3d(destination1) - destination0;
		Vector3d vector3d4 = new Point3d(destination2) - destination0;
		if (destination0.Equals(destination1))
		{
			Vector3d rotationAxis = Vector3d.CrossProduct(vector3d2, vector3d4);
			double angleRadians = Math.Acos(Clamp(Vector3d.Multiply(vector3d2, vector3d4), -1.0, 1.0));
			vector3d3 = vector3d;
			vector3d3.Rotate(angleRadians, rotationAxis);
		}
		else if (destination0.Equals(destination2))
		{
			Vector3d rotationAxis2 = Vector3d.CrossProduct(vector3d, vector3d3);
			double angleRadians2 = Math.Acos(Clamp(Vector3d.Multiply(vector3d, vector3d3), -1.0, 1.0));
			vector3d4 = vector3d2;
			vector3d4.Rotate(angleRadians2, rotationAxis2);
		}
		return Orient3Point(source0, vector3d, vector3d2, destination0, vector3d3, vector3d4);
	}

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

	public static OnXform Orient3Point_BackwardCompatibility(IOn3dPoint source0, IOn3dPoint source1, IOn3dPoint source2, IOn3dPoint destination0, IOn3dPoint destination1, IOn3dPoint destination2)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		On3dVector val = new On3dPoint(source1) - source0;
		On3dVector val2 = new On3dPoint(source2) - source0;
		On3dVector val3 = new On3dPoint(destination1) - destination0;
		On3dVector val4 = new On3dPoint(destination2) - destination0;
		if (destination0.Equals(destination1))
		{
			On3dVector val5 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val2, (IOn3dVector)(object)val4);
			double num = Math.Acos(Clamp(OnUtil.ON_DotProduct((IOn3dVector)(object)val2, (IOn3dVector)(object)val4), -1.0, 1.0));
			val3 = val;
			val3.Rotate(num, (IOn3dVector)(object)val5);
		}
		else if (destination0.Equals(destination2))
		{
			On3dVector val6 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val, (IOn3dVector)(object)val3);
			double num2 = Math.Acos(Clamp(OnUtil.ON_DotProduct((IOn3dVector)(object)val, (IOn3dVector)(object)val3), -1.0, 1.0));
			val4 = val2;
			val4.Rotate(num2, (IOn3dVector)(object)val6);
		}
		return Orient3Point(source0, (IOn3dVector)(object)val, (IOn3dVector)(object)val2, destination0, (IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
	}

	public static Transform Orient3Point_BackwardCompatibility(Point3d source0, Point3d source1, Point3d source2, Point3d destination0, Point3d destination1, Point3d destination2)
	{
		Vector3d vector3d = new Point3d(source1) - source0;
		Vector3d vector3d2 = new Point3d(source2) - source0;
		Vector3d vector3d3 = new Point3d(destination1) - destination0;
		Vector3d vector3d4 = new Point3d(destination2) - destination0;
		if (destination0.Equals(destination1))
		{
			Vector3d rotationAxis = Vector3d.CrossProduct(vector3d2, vector3d4);
			double angleRadians = Math.Acos(Clamp(Vector3d.Multiply(vector3d2, vector3d4), -1.0, 1.0));
			vector3d.Rotate(angleRadians, rotationAxis);
			vector3d3 = vector3d;
		}
		else if (destination0.Equals(destination2))
		{
			Vector3d rotationAxis2 = Vector3d.CrossProduct(vector3d, vector3d3);
			double angleRadians2 = Math.Acos(Clamp(Vector3d.Multiply(vector3d, vector3d3), -1.0, 1.0));
			vector3d2.Rotate(angleRadians2, rotationAxis2);
			vector3d4 = vector3d2;
		}
		return Orient3Point(source0, vector3d, vector3d2, destination0, vector3d3, vector3d4);
	}

	public static double PointAngleInPlane(IOnPlane plane, IOn3dPoint pt)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		double num = 0.0;
		double num2 = 0.0;
		plane.ClosestPointTo(pt, ref num, ref num2);
		On2dVector val = new On2dVector(num, num2);
		val.Unitize();
		On2dVector val2 = new On2dVector(1.0, 0.0);
		if (OnUtil.ON_DotProduct((IOn2dVector)new On2dVector(0.0, 1.0), (IOn2dVector)(object)val) > 0.0)
		{
			return Math.Acos(OnUtil.ON_DotProduct((IOn2dVector)(object)val2, (IOn2dVector)(object)val));
		}
		return 2.0 * OnUtil.On_PI - Math.Acos(OnUtil.ON_DotProduct((IOn2dVector)(object)val2, (IOn2dVector)(object)val));
	}

	public static double RadiusOfPointD(double x, double y)
	{
		return Math.Sqrt(x * x + y * y);
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

	public static Transform Rotation(Vector3d dirStart, Vector3d dirEnd, Point3d rotationCenter)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		On3dVector val = CSConvert.Vector3dCopyToOn3dVector(dirStart);
		On3dVector val2 = CSConvert.Vector3dCopyToOn3dVector(dirEnd);
		On3dPoint val3 = CSConvert.Point3dCopyToOn3dPoint(rotationCenter);
		OnXform val4 = new OnXform();
		val4.Identity();
		val4.Rotation((IOn3dVector)(object)val, (IOn3dVector)(object)val2, (IOn3dPoint)(object)val3);
		return CSConvert.XformCopyToTransform((IOnXform)val4);
	}

	public static bool SymSolve(out double x, out double y, double a, double b, double c, double d, double e, double f)
	{
		x = 0.0;
		y = 0.0;
		double num = b * d - e * a;
		if (Math.Abs(num) < 0.01)
		{
			return false;
		}
		x = (b * f - e * c) / num;
		y = (c - x * a) / b;
		return true;
	}

	public static double ToDegrees(double angleRadians)
	{
		return On_RADIANS_TO_DEGREES * angleRadians;
	}

	public static double ToRadians(double angleDegrees)
	{
		return On_DEGREES_TO_RADIANS * angleDegrees;
	}

	public static OnXform Transform2PointToOrigin(IOn3dPoint sp0, IOn3dPoint sp1)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		On3dVector val = new On3dPoint(sp1) - sp0;
		On3dVector val2 = new On3dVector(1.0, 0.0, 0.0);
		On3dVector val3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val, (IOn3dVector)(object)val2);
		if (val3 == (IOn3dVector)null || !val3.IsValid() || val3.IsZero())
		{
			val2 = new On3dVector(0.0, 1.0, 0.0);
			val3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val, (IOn3dVector)(object)val2);
		}
		val2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val3, (IOn3dVector)(object)val);
		return Transform3PointToOrigin(sp0, (IOn3dVector)(object)val, (IOn3dVector)(object)val2, (IOn3dVector)(object)val3);
	}

	public static Transform Transform2PointToOrigin(Point3d sp0, Point3d sp1)
	{
		Vector3d vector3d = new Point3d(sp1) - sp0;
		Vector3d b = new Vector3d(1.0, 0.0, 0.0);
		Vector3d vector3d2 = Vector3d.CrossProduct(vector3d, b);
		if (vector3d2 == Vector3d.Unset || !vector3d2.IsValid || vector3d2.IsZero)
		{
			b = new Vector3d(0.0, 1.0, 0.0);
			vector3d2 = Vector3d.CrossProduct(vector3d, b);
		}
		b = Vector3d.CrossProduct(vector3d2, vector3d);
		return Transform3PointToOrigin(sp0, vector3d, b, vector3d2);
	}

	public static OnXform Transform3PointToOrigin(IOn3dPoint sp0, IOn3dPoint sp1, IOn3dPoint sp2)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		On3dVector val = new On3dPoint(sp1) - sp0;
		On3dVector val2 = new On3dPoint(sp2) - sp0;
		On3dVector val3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val, (IOn3dVector)(object)val2);
		val2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val3, (IOn3dVector)(object)val);
		return Transform3PointToOrigin(sp0, (IOn3dVector)(object)val, (IOn3dVector)(object)val2, (IOn3dVector)(object)val3);
	}

	public static Transform Transform3PointToOrigin(Point3d sp0, Point3d sp1, Point3d sp2)
	{
		Vector3d vector3d = new Point3d(sp1) - sp0;
		Vector3d b = new Point3d(sp2) - sp0;
		Vector3d vector3d2 = Vector3d.CrossProduct(vector3d, b);
		b = Vector3d.CrossProduct(vector3d2, vector3d);
		return Transform3PointToOrigin(sp0, vector3d, b, vector3d2);
	}

	public static OnXform Transform3PointToOrigin(IOn3dPoint sourcePoint, IOn3dVector u, IOn3dVector v, IOn3dVector w)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		On3dVector val = new On3dVector(u);
		On3dVector val2 = new On3dVector(v);
		On3dVector val3 = new On3dVector(w);
		val.Unitize();
		val2.Unitize();
		val3.Unitize();
		OnXform val4 = new OnXform();
		val4.Identity();
		val4[0, 0] = val.x;
		val4[0, 1] = val.y;
		val4[0, 2] = val.z;
		val4[1, 0] = val2.x;
		val4[1, 1] = val2.y;
		val4[1, 2] = val2.z;
		val4[2, 0] = val3.x;
		val4[2, 1] = val3.y;
		val4[2, 2] = val3.z;
		val4[0, 3] = (0.0 - sourcePoint.x) * val.x - sourcePoint.y * val.y - sourcePoint.z * val.z;
		val4[1, 3] = (0.0 - sourcePoint.x) * val2.x - sourcePoint.y * val2.y - sourcePoint.z * val2.z;
		val4[2, 3] = (0.0 - sourcePoint.x) * val3.x - sourcePoint.y * val3.y - sourcePoint.z * val3.z;
		return val4;
	}

	public static Transform Transform3PointToOrigin(Point3d sourcePoint, Vector3d u, Vector3d v, Vector3d w)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		Vector3d vector3d = new Vector3d(u);
		Vector3d vector3d2 = new Vector3d(v);
		Vector3d vector3d3 = new Vector3d(w);
		vector3d.Unitize();
		vector3d2.Unitize();
		vector3d3.Unitize();
		OnXform val = new OnXform();
		val.Identity();
		val[0, 0] = vector3d.X;
		val[0, 1] = vector3d.Y;
		val[0, 2] = vector3d.Z;
		val[1, 0] = vector3d2.X;
		val[1, 1] = vector3d2.Y;
		val[1, 2] = vector3d2.Z;
		val[2, 0] = vector3d3.X;
		val[2, 1] = vector3d3.Y;
		val[2, 2] = vector3d3.Z;
		val[0, 3] = (0.0 - sourcePoint.X) * vector3d.X - sourcePoint.Y * vector3d.Y - sourcePoint.Z * vector3d.Z;
		val[1, 3] = (0.0 - sourcePoint.X) * vector3d2.X - sourcePoint.Y * vector3d2.Y - sourcePoint.Z * vector3d2.Z;
		val[2, 3] = (0.0 - sourcePoint.X) * vector3d3.X - sourcePoint.Y * vector3d3.Y - sourcePoint.Z * vector3d3.Z;
		return CSConvert.XformCopyToTransform((IOnXform)val);
	}

	public static OnXform TransformOriginTo2Point(IOn3dPoint sp0, IOn3dPoint sp1)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		On3dVector val = new On3dPoint(sp1) - sp0;
		On3dVector val2 = new On3dVector(0.0, 1.0, 0.0);
		On3dVector val3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val, (IOn3dVector)(object)val2);
		if (val3 == (IOn3dVector)null || !val3.IsValid() || val3.IsZero())
		{
			val2 = new On3dVector(1.0, 0.0, 0.0);
			val3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val, (IOn3dVector)(object)val2);
		}
		val2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val3, (IOn3dVector)(object)val);
		return TransformOriginTo3Point(sp0, (IOn3dVector)(object)val, (IOn3dVector)(object)val2, (IOn3dVector)(object)val3);
	}

	public static Transform TransformOriginTo2Point(Point3d sp0, Point3d sp1)
	{
		Vector3d vector3d = new Point3d(sp1) - sp0;
		Vector3d b = new Vector3d(0.0, 1.0, 0.0);
		Vector3d vector3d2 = Vector3d.CrossProduct(vector3d, b);
		if (vector3d2 == Vector3d.Unset || !vector3d2.IsValid || vector3d2.IsZero)
		{
			b = new Vector3d(1.0, 0.0, 0.0);
			vector3d2 = Vector3d.CrossProduct(vector3d, b);
		}
		b = Vector3d.CrossProduct(vector3d2, vector3d);
		return TransformOriginTo3Point(sp0, vector3d, b, vector3d2);
	}

	public static OnXform TransformOriginTo3Point(IOn3dPoint source0, IOn3dPoint source1, IOn3dPoint source3)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		On3dVector val = new On3dPoint(source1) - source0;
		On3dVector val2 = new On3dPoint(source3) - source0;
		On3dVector val3 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val, (IOn3dVector)(object)val2);
		val2 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val3, (IOn3dVector)(object)val);
		return TransformOriginTo3Point(source0, (IOn3dVector)(object)val, (IOn3dVector)(object)val2, (IOn3dVector)(object)val3);
	}

	public static OnXform TransformOriginTo3Point(IOn3dPoint source0, IOn3dVector u, IOn3dVector v, IOn3dVector w)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		On3dVector val = new On3dVector(u);
		On3dVector val2 = new On3dVector(v);
		On3dVector val3 = new On3dVector(w);
		val.Unitize();
		val2.Unitize();
		val3.Unitize();
		OnXform val4 = new OnXform();
		val4.Identity();
		val4[0, 0] = val.x;
		val4[1, 0] = val.y;
		val4[2, 0] = val.z;
		val4[0, 1] = val2.x;
		val4[1, 1] = val2.y;
		val4[2, 1] = val2.z;
		val4[0, 2] = val3.x;
		val4[1, 2] = val3.y;
		val4[2, 2] = val3.z;
		val4[0, 3] = source0.x;
		val4[1, 3] = source0.y;
		val4[2, 3] = source0.z;
		return val4;
	}

	public static Transform TransformOriginTo3Point(Point3d source0, Point3d source1, Point3d source3)
	{
		Vector3d vector3d = new Point3d(source1) - source0;
		Vector3d b = new Point3d(source3) - source0;
		Vector3d vector3d2 = Vector3d.CrossProduct(vector3d, b);
		b = Vector3d.CrossProduct(vector3d2, vector3d);
		return TransformOriginTo3Point(source0, vector3d, b, vector3d2);
	}

	public static Transform TransformOriginTo3Point(Point3d source0, Vector3d u, Vector3d v, Vector3d w)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		Vector3d vector3d = new Vector3d(u);
		Vector3d vector3d2 = new Vector3d(v);
		Vector3d vector3d3 = new Vector3d(w);
		vector3d.Unitize();
		vector3d2.Unitize();
		vector3d3.Unitize();
		OnXform val = new OnXform();
		val.Identity();
		val[0, 0] = vector3d.X;
		val[1, 0] = vector3d.Y;
		val[2, 0] = vector3d.Z;
		val[0, 1] = vector3d2.X;
		val[1, 1] = vector3d2.Y;
		val[2, 1] = vector3d2.Z;
		val[0, 2] = vector3d3.X;
		val[1, 2] = vector3d3.Y;
		val[2, 2] = vector3d3.Z;
		val[0, 3] = source0.X;
		val[1, 3] = source0.Y;
		val[2, 3] = source0.Z;
		return CSConvert.XformCopyToTransform((IOnXform)val);
	}

	public static OnXform TransformUnscale(IOnXform transform)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_00cf: Expected O, but got Unknown
		On3dPoint val = new On3dPoint(0.0, 0.0, 0.0);
		On3dPoint val2 = new On3dPoint(1.0, 0.0, 0.0);
		On3dPoint val3 = new On3dPoint(0.0, 1.0, 0.0);
		On3dPoint val4 = new On3dPoint((IOn3dPoint)val);
		val4.Transform(transform);
		On3dPoint val5 = new On3dPoint((IOn3dPoint)(object)val2);
		val5.Transform(transform);
		On3dPoint val6 = new On3dPoint((IOn3dPoint)(object)val3);
		val6.Transform(transform);
		On3dVector val7 = val5 - (IOn3dPoint)(object)val4;
		val7.Unitize();
		On3dVector val8 = val6 - (IOn3dPoint)(object)val4;
		val8.Unitize();
		val5 = val4 + (IOn3dVector)(object)val7;
		val6 = val4 + (IOn3dVector)(object)val8;
		return Orient3Point((IOn3dPoint)val, (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3, (IOn3dPoint)(object)val4, (IOn3dPoint)(object)val5, (IOn3dPoint)(object)val6);
	}

	public static Transform TransformUnscale(Transform transform)
	{
		Point3d point3d = new Point3d(0.0, 0.0, 0.0);
		Point3d point3d2 = new Point3d(1.0, 0.0, 0.0);
		Point3d point3d3 = new Point3d(0.0, 1.0, 0.0);
		Point3d point3d4 = new Point3d(point3d);
		point3d4.Transform(transform);
		Point3d point3d5 = new Point3d(point3d2);
		point3d5.Transform(transform);
		Point3d point3d6 = new Point3d(point3d3);
		point3d6.Transform(transform);
		Vector3d vector3d = point3d5 - point3d4;
		vector3d.Unitize();
		Vector3d vector3d2 = point3d6 - point3d4;
		vector3d2.Unitize();
		point3d5 = point3d4 + vector3d;
		point3d6 = point3d4 + vector3d2;
		return Orient3Point(point3d, point3d2, point3d3, point3d4, point3d5, point3d6);
	}

	public static OnXform TransformUnscale_BackwardCompatibility(IOnXform transform)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_00cf: Expected O, but got Unknown
		On3dPoint val = new On3dPoint(0.0, 0.0, 0.0);
		On3dPoint val2 = new On3dPoint(1.0, 0.0, 0.0);
		On3dPoint val3 = new On3dPoint(0.0, 1.0, 0.0);
		On3dPoint val4 = new On3dPoint((IOn3dPoint)val);
		val4.Transform(transform);
		On3dPoint val5 = new On3dPoint((IOn3dPoint)(object)val2);
		val5.Transform(transform);
		On3dPoint val6 = new On3dPoint((IOn3dPoint)(object)val3);
		val6.Transform(transform);
		On3dVector val7 = val5 - (IOn3dPoint)(object)val4;
		val7.Unitize();
		On3dVector val8 = val6 - (IOn3dPoint)(object)val4;
		val8.Unitize();
		val5 = val4 + (IOn3dVector)(object)val7;
		val6 = val4 + (IOn3dVector)(object)val8;
		return Orient3Point_BackwardCompatibility((IOn3dPoint)val, (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3, (IOn3dPoint)(object)val4, (IOn3dPoint)(object)val5, (IOn3dPoint)(object)val6);
	}

	public static Transform TransformUnscale_BackwardCompatibility(Transform transform)
	{
		Point3d point3d = new Point3d(0.0, 0.0, 0.0);
		Point3d point3d2 = new Point3d(1.0, 0.0, 0.0);
		Point3d point3d3 = new Point3d(0.0, 1.0, 0.0);
		Point3d point3d4 = new Point3d(point3d);
		point3d4.Transform(transform);
		Point3d point3d5 = new Point3d(point3d2);
		point3d5.Transform(transform);
		Point3d point3d6 = new Point3d(point3d3);
		point3d6.Transform(transform);
		Vector3d vector3d = point3d5 - point3d4;
		vector3d.Unitize();
		Vector3d vector3d2 = point3d6 - point3d4;
		vector3d2.Unitize();
		point3d5 = point3d4 + vector3d;
		point3d6 = point3d4 + vector3d2;
		return Orient3Point_BackwardCompatibility(point3d, point3d2, point3d3, point3d4, point3d5, point3d6);
	}

	public static double TriangleX(double b, double l1, double l2)
	{
		return (l1 * l1 - l2 * l2 + b * b) / (2.0 * b);
	}

	public static double TriangleY(double tx, double l1)
	{
		if (!(tx > l1))
		{
			return Math.Sqrt(l1 * l1 - tx * tx);
		}
		return 0.0;
	}
}
