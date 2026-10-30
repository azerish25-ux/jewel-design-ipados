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
			_valueHandleMouseWatcher.Enabled = _isHandleMouseWatcherEnabled;
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
		if (_arrDispBitmap == null)
		{
			_arrDispBitmap = new DisplayBitmap[27];
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
		Mesh mesh = MeshCylinder(p0, p1, radius);
		if (mesh != null)
		{
			displayPipeline.DrawMeshShaded(mesh, mat);
			mesh.Dispose();
		}
	}

	public static Mesh MeshCylinder(Point3d p0, Point3d p1, double radius)
	{
		Vector3d normal = p1 - p0;
		if (normal.Length < 0.001)
		{
			return null;
		}
		Plane plane = new Plane(p0, normal);
		Circle baseCircle = new Circle(plane, 2.0 / radius);
		return Mesh.CreateFromCylinder(new Cylinder(baseCircle, normal.Length), 1, 36);
	}

	public static void DrawMeshSphere(Point3d p0, double r, double l, DisplayPipeline displayPipeline, DisplayMaterial mat)
	{
		Transform transform = ChangeBasis(p0, new Vector3d(r / l, 0.0, 0.0), new Vector3d(0.0, r / l, 0.0), new Vector3d(0.0, 0.0, r / l));
		Transform modelTransform = displayPipeline.ModelTransform;
		displayPipeline.ModelTransform = modelTransform * transform;
		displayPipeline.DrawMeshShaded(MeshSphere, mat);
		displayPipeline.ModelTransform = modelTransform;
	}

	public static Mesh MeshCurve(Curve ac, int majorAxis, int minorAxis, double radius)
	{
		if (!ac.TryGetPlane(out var plane))
		{
			return new Mesh();
		}
		Mesh mesh = new Mesh();
		Interval domain = ac.Domain;
		double num = domain.Length / (double)majorAxis;
		Vector3d normal = plane.Normal;
		for (int i = 0; i <= majorAxis; i++)
		{
			double t = domain.Min + num * (double)i;
			Point3d point3d = ac.PointAt(t);
			Vector3d rotationAxis = ac.TangentAt(t);
			Point3d point3d2 = new Point3d(normal);
			point3d2 *= radius;
			point3d2 += point3d;
			Transform xform = Transform.Rotation(360.0 / (double)minorAxis * CSBlockUtil.CSUtilMath.On_DEGREES_TO_RADIANS, rotationAxis, point3d);
			for (int j = 0; j <= minorAxis; j++)
			{
				mesh.Vertices.Add(point3d2);
				mesh.Normals.Add((point3d2 - point3d) * -1.0);
				point3d2.Transform(xform);
			}
		}
		for (int k = 0; k <= majorAxis - 1; k++)
		{
			for (int l = 0; l <= minorAxis - 1; l++)
			{
				mesh.Faces.AddFace(k * (minorAxis + 1) + l, (k + 1) * (minorAxis + 1) + l, (k + 1) * (minorAxis + 1) + l + 1, k * (minorAxis + 1) + l + 1);
			}
		}
		mesh.Compact();
		return mesh;
	}

	public static Mesh MeshTorus(Torus torus, int majorAxis, int minorAxis)
	{
		Mesh mesh = new Mesh();
		NurbsSurface nurbsSurface = torus.ToNurbsSurface();
		Interval interval = nurbsSurface.Domain(0);
		Interval interval2 = nurbsSurface.Domain(1);
		for (int i = 0; i <= majorAxis; i++)
		{
			double u = interval.ParameterAt((double)i / (double)majorAxis);
			for (int j = 0; j <= minorAxis; j++)
			{
				double v = interval2.ParameterAt((double)j / (double)minorAxis);
				mesh.Vertices.Add(nurbsSurface.PointAt(u, v));
				mesh.Normals.Add(nurbsSurface.NormalAt(u, v));
			}
		}
		for (int k = 0; k <= majorAxis - 1; k++)
		{
			for (int l = 0; l <= minorAxis - 1; l++)
			{
				mesh.Faces.AddFace(k * (minorAxis + 1) + l, (k + 1) * (minorAxis + 1) + l, (k + 1) * (minorAxis + 1) + l + 1, k * (minorAxis + 1) + l + 1);
			}
		}
		mesh.Compact();
		return mesh;
	}

	private static void BuildArrow()
	{
		if (_meshArrow == null)
		{
			_meshArrow = Mesh.CreateFromCone(new Cone(new Plane(new Point3d(1.3, 0.0, 0.0), Vector3d.YAxis, Vector3d.ZAxis), -2.5, 1.0), 1, 36);
		}
	}

	private static void BuildBendRotate()
	{
		if (_meshBendRotate != null)
		{
			return;
		}
		using Mesh mesh = Mesh.CreateFromSphere(new Sphere(Point3d.Origin, 2.0), 20, 36);
		_meshBendRotate = mesh.DuplicateMesh();
	}

	private static void BuildCube()
	{
		if (_meshCube == null)
		{
			_meshCube = Mesh.CreateFromBox(new List<Point3d>
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
		if (_meshRotate != null)
		{
			return;
		}
		_meshRotate = new Mesh();
		using (Mesh mesh = Mesh.CreateFromCone(new Cone(new Plane(new Point3d(0.5, 0.0, 5.0), Vector3d.XAxis, Vector3d.YAxis), -2.5, 1.0), 1, 36))
		{
			Transform xform = Transform.Rotation(-20.0 * CSBlockUtil.CSUtilMath.On_DEGREES_TO_RADIANS, Vector3d.YAxis, Point3d.Origin);
			mesh.Transform(xform);
			_meshRotate.Append(mesh);
		}
		using (Mesh mesh2 = Mesh.CreateFromCone(new Cone(new Plane(new Point3d(0.5, 0.0, -5.0), Vector3d.XAxis, Vector3d.YAxis), 2.5, 1.0), 1, 36))
		{
			Transform xform2 = Transform.Rotation(20.0 * CSBlockUtil.CSUtilMath.On_DEGREES_TO_RADIANS, Vector3d.YAxis, Point3d.Origin);
			mesh2.Transform(xform2);
			_meshRotate.Append(mesh2);
		}
		using Mesh other = MeshCurve(new ArcCurve(new Arc(new Point3d(-0.5, 0.0, 3.0), new Point3d(0.0, 0.0, 0.0), new Point3d(-0.5, 0.0, -3.0))), 20, 20, 0.5);
		_meshRotate.Append(other);
	}

	private static void BuildScale()
	{
		if (_meshScale == null)
		{
			Plane plane = new Plane(new Point3d(-0.5, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0));
			Circle baseCircle = new Circle(plane, 2.0);
			_meshScale = Mesh.CreateFromCylinder(new Cylinder(baseCircle, 1.0), 1, 36);
		}
	}

	private static void BuildSphere()
	{
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
		return GetCameraDirection(dp.Viewport);
	}

	public static Vector3d GetCameraDirection(RhinoViewport vp)
	{
		Vector3d cameraDirection = vp.CameraDirection;
		CSApplicationHelper.Application.Conduit.GetDesignTransform().TryGetInverse(out var inverseTransform);
		cameraDirection.Transform(inverseTransform);
		cameraDirection.Unitize();
		return cameraDirection;
	}

	public static Line GetFrustrumLine(RhinoViewport vp, System.Drawing.Point viewWndPoint)
	{
		vp.GetFrustumLine(viewWndPoint.X, viewWndPoint.Y, out var worldLine);
		CSApplicationHelper.Application.Conduit.GetDesignTransform().TryGetInverse(out var inverseTransform);
		worldLine.Transform(inverseTransform);
		return worldLine;
	}

	public static double GetWorldToScreenScale(DisplayPipeline dp, Point3d p)
	{
		return GetWorldToScreenScale(dp.Viewport, p);
	}

	public static double GetWorldToScreenScale(RhinoViewport vp, Point3d p)
	{
		Point3d pointInFrustum = new Point3d(p);
		Transform designTransform = CSApplicationHelper.Application.Conduit.GetDesignTransform();
		pointInFrustum.Transform(designTransform);
		vp.GetWorldToScreenScale(pointInFrustum, out var pixelsPerUnit);
		return pixelsPerUnit / ScreenScaleOverride;
	}

	public static Transform WorldToScreen(DisplayPipeline dp)
	{
		return WorldToScreen(dp.Viewport);
	}

	public static Transform WorldToScreen(RhinoViewport vp)
	{
		Transform transform = vp.GetTransform(CoordinateSystem.World, CoordinateSystem.Screen);
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
		return ChangeBasis(initialLocation, initialBaseX, initialBaseY, initialBaseZ, Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis, Vector3d.ZAxis);
	}

	private static Transform ChangeBasis(Point3d initialLocation, Vector3d initialBaseX, Vector3d initialBaseY, Vector3d initialBaseZ, Point3d finalLocation, Vector3d finalBaseX, Vector3d finalBaseY, Vector3d finalBaseZ)
	{
		Transform transform = Transform.Translation(new Vector3d(finalLocation) * -1.0);
		Transform transform2 = Transform.ChangeBasis(initialBaseX, initialBaseY, initialBaseZ, finalBaseX, finalBaseY, finalBaseZ);
		return Transform.Translation(new Vector3d(initialLocation)) * transform2 * transform;
	}

	public static void LineInt(out Point3d outpoint, Point3d point0, Vector3d vector0, Point3d point1, Vector3d vector1)
	{
		outpoint = Point3d.Unset;
		Vector3d vector3d = point1 - point0;
		if (SymSolve(out var x, out var y, vector0.X, 0.0 - vector1.X, vector3d.X, vector0.Z, 0.0 - vector1.Z, vector3d.Z))
		{
			outpoint = point0 + vector0 * x;
		}
		else if (SymSolve(out x, out y, vector0.X, 0.0 - vector1.X, vector3d.X, vector0.Y, 0.0 - vector1.Y, vector3d.Y))
		{
			outpoint = point0 + vector0 * x;
		}
		else if (SymSolve(out x, out y, vector0.Y, 0.0 - vector1.Y, vector3d.Y, vector0.Z, 0.0 - vector1.Z, vector3d.Z))
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
