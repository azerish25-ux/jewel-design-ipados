using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CS.CSObjects.CSBlocks;
using CS.CSValues.CSValueHandle.Interfaces;
using CSBlockUtil;
using CSDatas;
using CSMaterials;
using GV.CounterSketch.Shared.Model.Objects;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Runtime;
using Rhino.UI;

namespace CS.CSValues.CSValueHandle.HandleSupportClasses;

public static class CSValueHandleHelper
{
	public enum ArrayDirection
	{
		Off,
		Xy,
		Xz,
		Yz
	}

	public enum WSelected
	{
		PNone,
		MoveX,
		MoveY,
		MoveZ,
		MoveXNegative,
		MoveYNegative,
		MoveZNegative,
		RotateX,
		RotateY,
		RotateZ,
		ScaleX,
		ScaleY,
		ScaleZ,
		Scale3D,
		TogglePoint,
		BendPoint0,
		BendPoint1,
		BendPoint2,
		MirrorModeBmp,
		ArrayEnd,
		ArrayCount,
		ArrayStart,
		ArrayTypeBmp,
		ArrayMirrorBmp,
		ArrayShapeBmp,
		ArrayMx,
		ArrayMy,
		ArrayMz,
		ArrayRx,
		ArrayRy,
		ArrayRz,
		LinkedMx,
		LinkedMy,
		LinkedMz,
		LinkedMmx,
		LinkedMmy,
		LinkedMmz
	}

	public enum WMode
	{
		Move,
		Rotate,
		Scale
	}

	public enum VPCHImages
	{
		Array0S,
		Array0,
		Array1S,
		Array1,
		Array2S,
		Array2,
		Array3S,
		Array3,
		ArrayMirrorOnS,
		ArrayMirrorOn,
		ArrayMirrorOffS,
		ArrayMirrorOff,
		Bend0S,
		Bend0,
		Bend1S,
		Bend1,
		Bend2S,
		Bend2,
		ArrayRound,
		ArrayCushion,
		ArrayOval,
		ArrayEmerald,
		ArrayAsscher,
		ArrayHeart,
		ArrayMarquise,
		ArrayPear,
		ArraySquare,
		Last
	}

	private static bool _isFreehandCommonCenterModeEnabled = true;

	private static bool _isHandleMouseWatcherEnabled;

	private static CSValueHandleMouseWatcher _valueHandleMouseWatcher;

	private static DisplayBitmap[] _arrDispBitmap;

	public static readonly DisplayMaterial MatBendBasePoint = new DisplayMaterial
	{
		Diffuse = Color.FromArgb(255, 96, 96, 255),
		Transparency = 0.6,
		Emission = Color.FromArgb(0, 0, 0),
		Ambient = Color.FromArgb(40, 40, 40)
	};

	public static readonly DisplayMaterial MatBendBendPoint = new DisplayMaterial
	{
		Diffuse = Color.FromArgb(255, 255, 144, 64),
		Transparency = 0.6,
		Emission = Color.FromArgb(0, 0, 0),
		Ambient = Color.FromArgb(40, 40, 40)
	};

	public static readonly DisplayMaterial MatBendEndPoint = new DisplayMaterial
	{
		Diffuse = Color.FromArgb(255, 255, 255, 96),
		Transparency = 0.6,
		Emission = Color.FromArgb(0, 0, 0),
		Ambient = Color.FromArgb(40, 40, 40)
	};

	public static readonly CSDisplayMaterial MatBendShadow = new CSDisplayMaterial(Color.FromArgb(255, 64, 128, 128), Color.FromArgb(40, 40, 40), 0.8, "Shadow");

	public static readonly DisplayMaterial MatSecondType1 = new DisplayMaterial
	{
		Diffuse = Color.FromArgb(255, 255, 255, 0),
		Transparency = 0.7,
		Emission = Color.FromArgb(0, 0, 0),
		Ambient = Color.FromArgb(40, 40, 40)
	};

	public static readonly DisplayMaterial MatSecondType2 = new DisplayMaterial
	{
		Diffuse = Color.FromArgb(255, 0, 255, 255),
		Transparency = 0.7,
		Emission = Color.FromArgb(0, 0, 0),
		Ambient = Color.FromArgb(40, 40, 40)
	};

	public static readonly DisplayMaterial MatSelected = new DisplayMaterial
	{
		Diffuse = Color.FromArgb(255, 247, 19, 255),
		Transparency = 0.0,
		Emission = Color.FromArgb(0, 0, 0),
		Ambient = Color.FromArgb(40, 40, 40)
	};

	private static Mesh _meshArrow;

	private static Mesh _meshBendRotate;

	private static Mesh _meshCube;

	private static Mesh _meshRotate;

	private static Mesh _meshScale;

	private static Mesh _meshSphere;

	private static double _screenScaleOverride = 1.0;

	private const double _selectionSizePixels = 20.0;

	public static Mesh MeshArrow
	{
		get
		{
			if (_meshArrow == null)
			{
				BuildArrow();
			}
			return _meshArrow;
		}
	}

	public static Mesh MeshSphere
	{
		get
		{
			if (_meshSphere == null)
			{
				BuildSphere();
			}
			return _meshSphere;
		}
	}

	public static Mesh MeshRotate
	{
		get
		{
			if (_meshRotate == null)
			{
				BuildRotate();
			}
			return _meshRotate;
		}
	}

	public static Mesh MeshBendRotate
	{
		get
		{
			if (_meshBendRotate == null)
			{
				BuildBendRotate();
			}
			return _meshBendRotate;
		}
	}

	public static Mesh MeshScale
	{
		get
		{
			if (_meshScale == null)
			{
				BuildScale();
			}
			return _meshScale;
		}
	}

	public static Mesh MeshCube
	{
		get
		{
			if (_meshCube == null)
			{
				BuildCube();
			}
			return _meshCube;
		}
	}

	public static List<ICSVHandle> SelectedHandles { get; } = new List<ICSVHandle>();

	public static ICSVHandle CurrentHandle { get; set; }

	public static bool IsHandleMouseWatcherEnabled
	{
		get
		{
			return _isHandleMouseWatcherEnabled;
		}
		set
		{
			_isHandleMouseWatcherEnabled = value;
			if (_valueHandleMouseWatcher == null)
			{
				_valueHandleMouseWatcher = new CSValueHandleMouseWatcher();
			}
			((MouseCallback)_valueHandleMouseWatcher).Enabled = _isHandleMouseWatcherEnabled;
		}
	}

	public static ICSVHandle TemporaryHandle { get; set; }

	public static double ScreenScaleOverride
	{
		get
		{
			return _screenScaleOverride;
		}
		set
		{
			_screenScaleOverride = value;
			_arrDispBitmap = null;
		}
	}

	public static double SelectionSizePixels => 20.0;

	public static bool IsMouseDown { get; set; }

	public static WMode MsrMode(WSelected selId)
	{
		switch (selId)
		{
		case WSelected.MoveX:
		case WSelected.MoveY:
		case WSelected.MoveZ:
		case WSelected.MoveXNegative:
		case WSelected.MoveYNegative:
		case WSelected.MoveZNegative:
		case WSelected.ArrayMx:
		case WSelected.ArrayMy:
		case WSelected.ArrayMz:
			return WMode.Move;
		case WSelected.RotateX:
		case WSelected.RotateY:
		case WSelected.RotateZ:
		case WSelected.ArrayRx:
		case WSelected.ArrayRy:
		case WSelected.ArrayRz:
			return WMode.Rotate;
		case WSelected.ScaleX:
		case WSelected.ScaleY:
		case WSelected.ScaleZ:
		case WSelected.Scale3D:
			return WMode.Scale;
		default:
			return WMode.Move;
		}
	}

	public static bool IsArrayMove(WSelected selId)
	{
		if ((uint)(selId - 25) <= 5u)
		{
			return true;
		}
		return false;
	}

	public static bool IsLinkedMove(WSelected selId)
	{
		if ((uint)(selId - 31) <= 5u)
		{
			return true;
		}
		return false;
	}

	public static DisplayBitmap GetDispBm(VPCHImages w)
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		if (_arrDispBitmap == null)
		{
			_arrDispBitmap = (DisplayBitmap[])(object)new DisplayBitmap[27];
		}
		if (_arrDispBitmap[(int)w] != null)
		{
			return _arrDispBitmap[(int)w];
		}
		Bitmap image = GetImage(w);
		int num = 30;
		int num2 = 30;
		num = (int)((double)num * ScreenScaleOverride);
		num2 = (int)((double)num2 * ScreenScaleOverride);
		Bitmap bitmap = new Bitmap(num, num2);
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			graphics.DrawImage(image, new Rectangle(0, 0, num, num2));
		}
		_arrDispBitmap[(int)w] = new DisplayBitmap(bitmap);
		return _arrDispBitmap[(int)w];
	}

	private static Bitmap GetImage(VPCHImages w)
	{
		return w switch
		{
			VPCHImages.Array0S => CS.CSValues.CSValueHandle.VPCHImages.array0s, 
			VPCHImages.Array0 => CS.CSValues.CSValueHandle.VPCHImages.array0, 
			VPCHImages.Array1S => CS.CSValues.CSValueHandle.VPCHImages.array1s, 
			VPCHImages.Array1 => CS.CSValues.CSValueHandle.VPCHImages.array1, 
			VPCHImages.Array2S => CS.CSValues.CSValueHandle.VPCHImages.array2s, 
			VPCHImages.Array2 => CS.CSValues.CSValueHandle.VPCHImages.array2, 
			VPCHImages.Array3S => CS.CSValues.CSValueHandle.VPCHImages.array3s, 
			VPCHImages.Array3 => CS.CSValues.CSValueHandle.VPCHImages.array3, 
			VPCHImages.ArrayMirrorOnS => CS.CSValues.CSValueHandle.VPCHImages.arrayMirrorOnS, 
			VPCHImages.ArrayMirrorOn => CS.CSValues.CSValueHandle.VPCHImages.arrayMirrorOn, 
			VPCHImages.ArrayMirrorOffS => CS.CSValues.CSValueHandle.VPCHImages.arrayMirrorOffS, 
			VPCHImages.ArrayMirrorOff => CS.CSValues.CSValueHandle.VPCHImages.arrayMirrorOff, 
			VPCHImages.Bend0S => CS.CSValues.CSValueHandle.VPCHImages.Bend0S, 
			VPCHImages.Bend0 => CS.CSValues.CSValueHandle.VPCHImages.Bend0, 
			VPCHImages.Bend1S => CS.CSValues.CSValueHandle.VPCHImages.Bend1S, 
			VPCHImages.Bend1 => CS.CSValues.CSValueHandle.VPCHImages.Bend1, 
			VPCHImages.Bend2S => CS.CSValues.CSValueHandle.VPCHImages.Bend2S, 
			VPCHImages.Bend2 => CS.CSValues.CSValueHandle.VPCHImages.Bend2, 
			VPCHImages.ArrayRound => CS.CSValues.CSValueHandle.VPCHImages.Round, 
			VPCHImages.ArrayCushion => CS.CSValues.CSValueHandle.VPCHImages.Cushion, 
			VPCHImages.ArrayOval => CS.CSValues.CSValueHandle.VPCHImages.Oval, 
			VPCHImages.ArrayEmerald => CS.CSValues.CSValueHandle.VPCHImages.Emerald, 
			VPCHImages.ArrayAsscher => CS.CSValues.CSValueHandle.VPCHImages.Asscher, 
			VPCHImages.ArrayHeart => CS.CSValues.CSValueHandle.VPCHImages.Heart, 
			VPCHImages.ArrayMarquise => CS.CSValues.CSValueHandle.VPCHImages.Marquise, 
			VPCHImages.ArrayPear => CS.CSValues.CSValueHandle.VPCHImages.Pear, 
			VPCHImages.ArraySquare => CS.CSValues.CSValueHandle.VPCHImages.Square, 
			_ => null, 
		};
	}

	public static void DrawMeshCylinder(Point3d p0, Point3d p1, double radius, DisplayPipeline displayPipeline, DisplayMaterial mat)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Mesh val = MeshCylinder(p0, p1, radius);
		if (val != null)
		{
			displayPipeline.DrawMeshShaded(val, mat);
			((CommonObject)val).Dispose();
		}
	}

	public static Mesh MeshCylinder(Point3d p0, Point3d p1, double radius)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = p1 - p0;
		if (((Vector3d)(ref val)).Length < 0.001)
		{
			return null;
		}
		Plane val2 = default(Plane);
		((Plane)(ref val2))._002Ector(p0, val);
		Circle val3 = default(Circle);
		((Circle)(ref val3))._002Ector(val2, 2.0 / radius);
		return Mesh.CreateFromCylinder(new Cylinder(val3, ((Vector3d)(ref val)).Length), 1, 36);
	}

	public static void DrawMeshSphere(Point3d p0, double r, double l, DisplayPipeline displayPipeline, DisplayMaterial mat)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		Transform val = ChangeBasis(p0, new Vector3d(r / l, 0.0, 0.0), new Vector3d(0.0, r / l, 0.0), new Vector3d(0.0, 0.0, r / l));
		Transform modelTransform = displayPipeline.ModelTransform;
		displayPipeline.ModelTransform = modelTransform * val;
		displayPipeline.DrawMeshShaded(MeshSphere, mat);
		displayPipeline.ModelTransform = modelTransform;
	}

	public static Mesh MeshCurve(Curve ac, int majorAxis, int minorAxis, double radius)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		Plane val = default(Plane);
		if (!ac.TryGetPlane(ref val))
		{
			return new Mesh();
		}
		Mesh val2 = new Mesh();
		Interval domain = ac.Domain;
		double num = ((Interval)(ref domain)).Length / (double)majorAxis;
		Vector3d normal = ((Plane)(ref val)).Normal;
		Point3d val5 = default(Point3d);
		for (int i = 0; i <= majorAxis; i++)
		{
			double num2 = ((Interval)(ref domain)).Min + num * (double)i;
			Point3d val3 = ac.PointAt(num2);
			Vector3d val4 = ac.TangentAt(num2);
			((Point3d)(ref val5))._002Ector(normal);
			val5 *= radius;
			val5 += val3;
			Transform val6 = Transform.Rotation(360.0 / (double)minorAxis * CSBlockUtil.CSUtilMath.On_DEGREES_TO_RADIANS, val4, val3);
			for (int j = 0; j <= minorAxis; j++)
			{
				val2.Vertices.Add(val5);
				val2.Normals.Add((val5 - val3) * -1.0);
				((Point3d)(ref val5)).Transform(val6);
			}
		}
		for (int k = 0; k <= majorAxis - 1; k++)
		{
			for (int l = 0; l <= minorAxis - 1; l++)
			{
				val2.Faces.AddFace(k * (minorAxis + 1) + l, (k + 1) * (minorAxis + 1) + l, (k + 1) * (minorAxis + 1) + l + 1, k * (minorAxis + 1) + l + 1);
			}
		}
		val2.Compact();
		return val2;
	}

	public static Mesh MeshTorus(Torus torus, int majorAxis, int minorAxis)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		Mesh val = new Mesh();
		NurbsSurface val2 = ((Torus)(ref torus)).ToNurbsSurface();
		Interval val3 = ((Surface)val2).Domain(0);
		Interval val4 = ((Surface)val2).Domain(1);
		for (int i = 0; i <= majorAxis; i++)
		{
			double num = ((Interval)(ref val3)).ParameterAt((double)i / (double)majorAxis);
			for (int j = 0; j <= minorAxis; j++)
			{
				double num2 = ((Interval)(ref val4)).ParameterAt((double)j / (double)minorAxis);
				val.Vertices.Add(((Surface)val2).PointAt(num, num2));
				val.Normals.Add(((Surface)val2).NormalAt(num, num2));
			}
		}
		for (int k = 0; k <= majorAxis - 1; k++)
		{
			for (int l = 0; l <= minorAxis - 1; l++)
			{
				val.Faces.AddFace(k * (minorAxis + 1) + l, (k + 1) * (minorAxis + 1) + l, (k + 1) * (minorAxis + 1) + l + 1, k * (minorAxis + 1) + l + 1);
			}
		}
		val.Compact();
		return val;
	}

	private static void BuildArrow()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		if (_meshArrow == null)
		{
			_meshArrow = Mesh.CreateFromCone(new Cone(new Plane(new Point3d(1.3, 0.0, 0.0), Vector3d.YAxis, Vector3d.ZAxis), -2.5, 1.0), 1, 36);
		}
	}

	private static void BuildBendRotate()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (_meshBendRotate != null)
		{
			return;
		}
		Mesh val = Mesh.CreateFromSphere(new Sphere(Point3d.Origin, 2.0), 20, 36);
		try
		{
			_meshBendRotate = val.DuplicateMesh();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private static void BuildCube()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		if (_meshCube == null)
		{
			_meshCube = Mesh.CreateFromBox((IEnumerable<Point3d>)new List<Point3d>
			{
				new Point3d(-1.0, -1.0, -1.0),
				new Point3d(1.0, -1.0, -1.0),
				new Point3d(1.0, 1.0, -1.0),
				new Point3d(-1.0, 1.0, -1.0),
				new Point3d(-1.0, -1.0, 1.0),
				new Point3d(1.0, -1.0, 1.0),
				new Point3d(1.0, 1.0, 1.0),
				new Point3d(-1.0, 1.0, 1.0)
			}, 1, 1, 1);
		}
	}

	private static void BuildRotate()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		if (_meshRotate != null)
		{
			return;
		}
		_meshRotate = new Mesh();
		Mesh val = Mesh.CreateFromCone(new Cone(new Plane(new Point3d(0.5, 0.0, 5.0), Vector3d.XAxis, Vector3d.YAxis), -2.5, 1.0), 1, 36);
		try
		{
			Transform val2 = Transform.Rotation(-20.0 * CSBlockUtil.CSUtilMath.On_DEGREES_TO_RADIANS, Vector3d.YAxis, Point3d.Origin);
			((GeometryBase)val).Transform(val2);
			_meshRotate.Append(val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Mesh val3 = Mesh.CreateFromCone(new Cone(new Plane(new Point3d(0.5, 0.0, -5.0), Vector3d.XAxis, Vector3d.YAxis), 2.5, 1.0), 1, 36);
		try
		{
			Transform val4 = Transform.Rotation(20.0 * CSBlockUtil.CSUtilMath.On_DEGREES_TO_RADIANS, Vector3d.YAxis, Point3d.Origin);
			((GeometryBase)val3).Transform(val4);
			_meshRotate.Append(val3);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		Mesh val5 = MeshCurve((Curve)new ArcCurve(new Arc(new Point3d(-0.5, 0.0, 3.0), new Point3d(0.0, 0.0, 0.0), new Point3d(-0.5, 0.0, -3.0))), 20, 20, 0.5);
		try
		{
			_meshRotate.Append(val5);
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
	}

	private static void BuildScale()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		if (_meshScale == null)
		{
			Plane val = default(Plane);
			((Plane)(ref val))._002Ector(new Point3d(-0.5, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0));
			Circle val2 = default(Circle);
			((Circle)(ref val2))._002Ector(val, 2.0);
			_meshScale = Mesh.CreateFromCylinder(new Cylinder(val2, 1.0), 1, 36);
		}
	}

	private static void BuildSphere()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (_meshSphere == null)
		{
			_meshSphere = Mesh.CreateFromSphere(new Sphere(Point3d.Origin, 1.0), 20, 36);
		}
	}

	public static void SetFreehandCommonCenterModeEnabled(bool isFreehandCommonCenterModeEnabled)
	{
		_isFreehandCommonCenterModeEnabled = isFreehandCommonCenterModeEnabled;
	}

	public static void SetLinked(WSelected type, double value, Transform xTrans)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		foreach (ICSVHandle selectedHandle in SelectedHandles)
		{
			selectedHandle.SetLinked(type, value, xTrans);
		}
	}

	public static void UIUpdateSelectedHandles(ICSVHandle handle, bool requiresUICallback = true)
	{
		using (CSValueXFormObject cSValueXFormObject = handle as CSValueXFormObject)
		{
			if (cSValueXFormObject != null && CurrentHandle != cSValueXFormObject && CurrentHandle != null)
			{
				int num = SelectedHandles.IndexOf(cSValueXFormObject);
				if (num >= 0)
				{
					if (cSValueXFormObject.IsFreehandCommonCenterModeEnabled != _isFreehandCommonCenterModeEnabled)
					{
						cSValueXFormObject.IsFreehandCommonCenterModeEnabled = _isFreehandCommonCenterModeEnabled;
					}
					else
					{
						SelectedHandles.RemoveAt(num);
					}
				}
				else
				{
					SelectedHandles.Add(cSValueXFormObject);
					cSValueXFormObject.IsFreehandCommonCenterModeEnabled = _isFreehandCommonCenterModeEnabled;
				}
			}
		}
		CSUtil.Redraw();
		CSApplicationHelper.Application.RaiseFreehandObjectSelectionChangedEvent(CurrentHandle, SelectedHandles, requiresUICallback);
	}

	public static void UpdateViewportControlHandles(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions)
	{
		if (!_isHandleMouseWatcherEnabled)
		{
			return;
		}
		foreach (ICSVHandle item in CSApplicationHelper.Application.Document.Graph.ActiveObjects().OfType<ICSVHandle>())
		{
			CSBlock blockObjectItem = item as CSBlock;
			if (blockObjectItem == null || CSApplicationHelper.Application.Document.Graph.ImportedCSGParts == null || !CSApplicationHelper.Application.Document.Graph.ImportedCSGParts.Any((ImportGroup importGroup) => importGroup.BlockIdentifiers.Contains(blockObjectItem.InstanceGuid) && (!importGroup.IsViewportVisible || !CSApplicationHelper.Application.IsFreehandPartsVisible)))
			{
				item.DrawHandle(displayPipeline, displayOptions);
			}
		}
	}

	public static void UpdateViewportControlHandlesBackGround(DisplayPipeline displayPipeline)
	{
		if (!_isHandleMouseWatcherEnabled)
		{
			return;
		}
		foreach (ICSVHandle item in CSApplicationHelper.Application.Document.Graph.ActiveObjects().OfType<ICSVHandle>())
		{
			CSBlock blockObjectItem = item as CSBlock;
			if (blockObjectItem == null || CSApplicationHelper.Application.Document.Graph.ImportedCSGParts == null || !CSApplicationHelper.Application.Document.Graph.ImportedCSGParts.Any((ImportGroup importGroup) => importGroup.BlockIdentifiers.Contains(blockObjectItem.InstanceGuid) && (!importGroup.IsViewportVisible || !CSApplicationHelper.Application.IsFreehandPartsVisible)))
			{
				item.DrawBackGround(displayPipeline);
			}
		}
	}

	public static void UpdateViewportControlHandlesOverLayObjects(DisplayPipeline displayPipeline)
	{
		if (!_isHandleMouseWatcherEnabled)
		{
			return;
		}
		foreach (ICSVHandle item in CSApplicationHelper.Application.Document.Graph.ActiveObjects().OfType<ICSVHandle>())
		{
			CSBlock blockObjectItem = item as CSBlock;
			if (blockObjectItem == null || CSApplicationHelper.Application.Document.Graph.ImportedCSGParts == null || !CSApplicationHelper.Application.Document.Graph.ImportedCSGParts.Any((ImportGroup importGroup) => importGroup.BlockIdentifiers.Contains(blockObjectItem.InstanceGuid) && (!importGroup.IsViewportVisible || !CSApplicationHelper.Application.IsFreehandPartsVisible)))
			{
				item.DrawOverLay(displayPipeline);
			}
		}
	}

	public static Vector3d GetCameraDirection(DisplayPipeline dp)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return GetCameraDirection(dp.Viewport);
	}

	public static Vector3d GetCameraDirection(RhinoViewport vp)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Vector3d cameraDirection = vp.CameraDirection;
		Transform designTransform = CSApplicationHelper.Application.Conduit.GetDesignTransform();
		Transform val = default(Transform);
		((Transform)(ref designTransform)).TryGetInverse(ref val);
		((Vector3d)(ref cameraDirection)).Transform(val);
		((Vector3d)(ref cameraDirection)).Unitize();
		return cameraDirection;
	}

	public static Line GetFrustrumLine(RhinoViewport vp, Point viewWndPoint)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Line result = default(Line);
		vp.GetFrustumLine((double)viewWndPoint.X, (double)viewWndPoint.Y, ref result);
		Transform designTransform = CSApplicationHelper.Application.Conduit.GetDesignTransform();
		Transform val = default(Transform);
		((Transform)(ref designTransform)).TryGetInverse(ref val);
		((Line)(ref result)).Transform(val);
		return result;
	}

	public static double GetWorldToScreenScale(DisplayPipeline dp, Point3d p)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return GetWorldToScreenScale(dp.Viewport, p);
	}

	public static double GetWorldToScreenScale(RhinoViewport vp, Point3d p)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(p);
		Transform designTransform = CSApplicationHelper.Application.Conduit.GetDesignTransform();
		((Point3d)(ref val)).Transform(designTransform);
		double num = default(double);
		vp.GetWorldToScreenScale(val, ref num);
		return num / ScreenScaleOverride;
	}

	public static Transform WorldToScreen(DisplayPipeline dp)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return WorldToScreen(dp.Viewport);
	}

	public static Transform WorldToScreen(RhinoViewport vp)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		Transform transform = vp.GetTransform((CoordinateSystem)0, (CoordinateSystem)3);
		Transform designTransform = CSApplicationHelper.Application.Conduit.GetDesignTransform();
		return transform * designTransform;
	}

	public static bool MouseButtonControl(MouseCallbackEventArgs e)
	{
		return (Control.ModifierKeys & Keys.Control) == Keys.Control;
	}

	public static bool MouseButtonLeft(MouseCallbackEventArgs e)
	{
		return (e.Button & MouseButtons.Left) != 0;
	}

	public static bool MouseButtonRight(MouseCallbackEventArgs e)
	{
		return (e.Button & MouseButtons.Right) != 0;
	}

	public static bool MouseButtonNone(MouseCallbackEventArgs e)
	{
		if ((e.Button & MouseButtons.Left) == 0)
		{
			return (e.Button & MouseButtons.Right) == 0;
		}
		return false;
	}

	public static bool MouseButtonShift(MouseCallbackEventArgs e)
	{
		return (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
	}

	public static double AngleBetweenVectors(Vector3d inputVector1, Vector3d inputVector2)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(inputVector1);
		Vector3d val2 = default(Vector3d);
		((Vector3d)(ref val2))._002Ector(inputVector2);
		((Vector3d)(ref val)).Unitize();
		((Vector3d)(ref val2)).Unitize();
		double num = Vector3d.Multiply(val2, val);
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

	public static double AngleOfPointD(double x, double y)
	{
		if (Math.Abs(y) < 0.0001)
		{
			if (x > 0.0)
			{
				return Math.PI / 2.0;
			}
			return 4.71238898038469;
		}
		if (y < 0.0)
		{
			return Math.Atan(x / y) + Math.PI;
		}
		if (x < 0.0)
		{
			return Math.Atan(x / y) + Math.PI * 2.0;
		}
		return Math.Atan(x / y);
	}

	public static Transform ChangeBasis(Point3d initialLocation, Vector3d initialBaseX, Vector3d initialBaseY, Vector3d initialBaseZ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		return ChangeBasis(initialLocation, initialBaseX, initialBaseY, initialBaseZ, Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis, Vector3d.ZAxis);
	}

	private static Transform ChangeBasis(Point3d initialLocation, Vector3d initialBaseX, Vector3d initialBaseY, Vector3d initialBaseZ, Point3d finalLocation, Vector3d finalBaseX, Vector3d finalBaseY, Vector3d finalBaseZ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Transform val = Transform.Translation(new Vector3d(finalLocation) * -1.0);
		Transform val2 = Transform.ChangeBasis(initialBaseX, initialBaseY, initialBaseZ, finalBaseX, finalBaseY, finalBaseZ);
		return Transform.Translation(new Vector3d(initialLocation)) * val2 * val;
	}

	public static void LineInt(out Point3d outpoint, Point3d point0, Vector3d vector0, Point3d point1, Vector3d vector1)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		outpoint = Point3d.Unset;
		Vector3d val = point1 - point0;
		if (SymSolve(out var x, out var y, ((Vector3d)(ref vector0)).X, 0.0 - ((Vector3d)(ref vector1)).X, ((Vector3d)(ref val)).X, ((Vector3d)(ref vector0)).Z, 0.0 - ((Vector3d)(ref vector1)).Z, ((Vector3d)(ref val)).Z))
		{
			outpoint = point0 + vector0 * x;
		}
		else if (SymSolve(out x, out y, ((Vector3d)(ref vector0)).X, 0.0 - ((Vector3d)(ref vector1)).X, ((Vector3d)(ref val)).X, ((Vector3d)(ref vector0)).Y, 0.0 - ((Vector3d)(ref vector1)).Y, ((Vector3d)(ref val)).Y))
		{
			outpoint = point0 + vector0 * x;
		}
		else if (SymSolve(out x, out y, ((Vector3d)(ref vector0)).Y, 0.0 - ((Vector3d)(ref vector1)).Y, ((Vector3d)(ref val)).Y, ((Vector3d)(ref vector0)).Z, 0.0 - ((Vector3d)(ref vector1)).Z, ((Vector3d)(ref val)).Z))
		{
			outpoint = point0 + vector0 * x;
		}
	}

	private static bool SymSolve(out double x, out double y, double a, double b, double c, double d, double e, double f)
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
}
