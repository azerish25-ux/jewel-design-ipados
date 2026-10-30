using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Threading;
using CS.CSObjects.CSBlocks;
using CS.CSValues.CSValueHandle;
using CS.Parametric;
using CSDatas;
using GV.CounterSketch.Shared;
using RMA.Rhino;
using Rhino;
using Rhino.Display;
using Rhino.Geometry;

namespace CS;

public static class CSDisplayCaptureHelper
{
	private static readonly object _viewportCaptureLock;

	private static CSDisplayConduit _csDisplayConduitCapture;

	public static bool IsHighlightingEnabled { get; set; }

	static CSDisplayCaptureHelper()
	{
		_viewportCaptureLock = new object();
	}

	public static Bitmap CaptureViewportToBitmap(Size captureSize, Point3d cameraLocation, Point3d targetLocation, double camera35MmLensLength, Vector3d cameraUp)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return CaptureViewportToBitmapForTargetedBlocks(new List<Guid>(), captureSize, cameraLocation, targetLocation, camera35MmLensLength, cameraUp);
	}

	public static Bitmap CaptureViewportToBitmap(Size captureSize)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		RhinoViewport mainViewport = RhinoDoc.ActiveDoc.Views.ActiveView.MainViewport;
		Point3d cameraLocation = mainViewport.CameraLocation;
		Point3d targetLocation = mainViewport.CameraTarget;
		double camera35MmLensLength = mainViewport.Camera35mmLensLength;
		Vector3d cameraUp = mainViewport.CameraUp;
		switch (CSApplicationHelper.Application.CurrentCameraViewName)
		{
		case CameraView.Top:
			camera35MmLensLength = 50.0;
			((Point3d)(ref cameraLocation))._002Ector(0.0, 0.0, 80.0);
			targetLocation = Point3d.Origin;
			cameraUp = Vector3d.YAxis;
			break;
		case CameraView.Front:
			camera35MmLensLength = 50.0;
			((Point3d)(ref cameraLocation))._002Ector(0.0, -80.0, 0.0);
			targetLocation = Point3d.Origin;
			cameraUp = Vector3d.ZAxis;
			break;
		case CameraView.Side:
			camera35MmLensLength = 50.0;
			((Point3d)(ref cameraLocation))._002Ector(80.0, 0.0, 0.0);
			targetLocation = Point3d.Origin;
			cameraUp = Vector3d.ZAxis;
			break;
		}
		return CaptureViewportToBitmapForTargetedBlocks(new List<Guid>(), captureSize, cameraLocation, targetLocation, camera35MmLensLength, cameraUp);
	}

	public static Bitmap CaptureViewportToBitmapAtDefaultPerspective(Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		return CaptureViewportToBitmapForTargetedBlocksAtDefaultPerspective(new List<Guid>(), captureSize, zoomToObjects, zoomToObjectsPadding);
	}

	public static Bitmap CaptureViewportToBitmapAtFront(Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		return CaptureViewportToBitmapForTargetedBlocksAtFront(new List<Guid>(), captureSize, zoomToObjects, zoomToObjectsPadding);
	}

	public static Bitmap CaptureViewportToBitmapAtLibraryPerspective(Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		return CaptureViewportToBitmapForTargetedBlocksAtLibraryPerspective(new List<Guid>(), captureSize, zoomToObjects, zoomToObjectsPadding);
	}

	public static Bitmap CaptureViewportToBitmapAtSide(Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		return CaptureViewportToBitmapForTargetedBlocksAtSide(new List<Guid>(), captureSize, zoomToObjects, zoomToObjectsPadding);
	}

	public static Bitmap CaptureViewportToBitmapAtTop(Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		return CaptureViewportToBitmapForTargetedBlocksAtTop(new List<Guid>(), captureSize, zoomToObjects, zoomToObjectsPadding);
	}

	public static Bitmap CaptureViewportToBitmapForTargetedBlocks(List<Guid> targetedBlocks, Size captureSize, Point3d cameraLocation, Point3d targetLocation, double camera35MmLensLength, Vector3d cameraUp, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		lock (_viewportCaptureLock)
		{
			Bitmap screenbitmap = null;
			try
			{
				Guid activeViewportID = RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewportID;
				Guid screenCaptureViewGuid = CreateScreenCaptureViewGuid();
				RhinoViewport activeViewport = RhinoDoc.ActiveDoc.Views.Find(screenCaptureViewGuid).ActiveViewport;
				activeViewport.Size = captureSize;
				bool enabled = ((DisplayConduit)CSApplicationHelper.Application.Conduit).Enabled;
				CSDisplayConduit conduit = CSApplicationHelper.Application.Conduit;
				((DisplayConduit)conduit).Enabled = false;
				if (_csDisplayConduitCapture == null)
				{
					_csDisplayConduitCapture = new CSDisplayConduit
					{
						DisplayMaterials = conduit.DisplayMaterials
					};
				}
				_csDisplayConduitCapture.TargetedBlockInstanceIds = targetedBlocks;
				_csDisplayConduitCapture.IsDrawingTargetedBlocksOnly = targetedBlocks.Any();
				_csDisplayConduitCapture._isDisplayHighlightEnabled = IsHighlightingEnabled;
				_csDisplayConduitCapture.IsViewportHandlesVisible = false;
				CSApplicationHelper.Application.Conduit = _csDisplayConduitCapture;
				((DisplayConduit)_csDisplayConduitCapture).Enabled = true;
				activeViewport.ConstructionGridVisible = false;
				activeViewport.WorldAxesVisible = false;
				activeViewport.ConstructionAxesVisible = false;
				activeViewport.SetWallpaper(string.Empty, false, false);
				_csDisplayConduitCapture.BackgroundColor = Color.White;
				activeViewport.ChangeToPerspectiveProjection(true, camera35MmLensLength);
				activeViewport.SetCameraLocations(targetLocation, cameraLocation);
				activeViewport.Camera35mmLensLength = camera35MmLensLength;
				activeViewport.CameraUp = cameraUp;
				if (zoomToObjects)
				{
					BoundingBox blockBoundingBox = BoundingBox.Unset;
					List<CSBlock> list = ((!CSApplicationHelper.IsCounterSketchStudio) ? (from csblock in CSApplicationHelper.Application.Document.Graph.AllObjects().OfType<CSBlock>()
						where (!targetedBlocks.Any() && csblock.DisplayVisible) || targetedBlocks.Contains(csblock.InstanceGuid)
						select csblock).ToList() : (from csblock in CSApplicationHelper.Application.Document.Graph.DisplayObjects()
						where csblock != null && ((!targetedBlocks.Any() && csblock.DisplayVisible) || targetedBlocks.Contains(csblock.InstanceGuid))
						select csblock).ToList());
					bool num = list.OfType<CSValueXFormObject>().All((CSValueXFormObject vpch) => vpch.MirrorMode != 0);
					list.ForEach(delegate(CSBlock csBlock)
					{
						csBlock.OutputChannelDatas.ForEach(delegate(CSData csData)
						{
							//IL_0004: Unknown result type (might be due to invalid IL or missing references)
							//IL_0009: Unknown result type (might be due to invalid IL or missing references)
							//IL_001c: Unknown result type (might be due to invalid IL or missing references)
							//IL_002d: Unknown result type (might be due to invalid IL or missing references)
							if (csData != null)
							{
								BoundingBox boundingBox = BoundingBox.Unset;
								if (csData.GetTightBoundingBoxNew(ref boundingBox))
								{
									((BoundingBox)(ref boundingBox)).Transform(_csDisplayConduitCapture.GetDesignTransform());
									((BoundingBox)(ref blockBoundingBox)).Union(boundingBox);
								}
							}
						});
					});
					Point3d val = ((BoundingBox)(ref blockBoundingBox)).Min;
					double x = ((Point3d)(ref val)).X;
					val = ((BoundingBox)(ref blockBoundingBox)).Min;
					double y = ((Point3d)(ref val)).Y;
					val = ((BoundingBox)(ref blockBoundingBox)).Min;
					Point3d val2 = new Point3d(x, y, ((Point3d)(ref val)).Z);
					val = ((BoundingBox)(ref blockBoundingBox)).Max;
					double x2 = ((Point3d)(ref val)).X;
					val = ((BoundingBox)(ref blockBoundingBox)).Max;
					double y2 = ((Point3d)(ref val)).Y;
					val = ((BoundingBox)(ref blockBoundingBox)).Max;
					BoundingBox val3 = default(BoundingBox);
					((BoundingBox)(ref val3))._002Ector(val2, new Point3d(x2, y2, ((Point3d)(ref val)).Z));
					if (num)
					{
						Vector3d diagonal = ((BoundingBox)(ref val3)).Diagonal;
						double num2 = ((((Vector3d)(ref diagonal)).X + zoomToObjectsPadding > 0.0) ? zoomToObjectsPadding : 0.0);
						diagonal = ((BoundingBox)(ref val3)).Diagonal;
						double num3 = ((((Vector3d)(ref diagonal)).Y + zoomToObjectsPadding > 0.0) ? zoomToObjectsPadding : 0.0);
						diagonal = ((BoundingBox)(ref val3)).Diagonal;
						double num4 = ((((Vector3d)(ref diagonal)).Z + zoomToObjectsPadding > 0.0) ? zoomToObjectsPadding : 0.0);
						((BoundingBox)(ref val3)).Inflate(num2, num3, num4);
					}
					activeViewport.SetCameraLocations(((BoundingBox)(ref val3)).Center, cameraLocation);
					activeViewport.ZoomBoundingBox(val3);
				}
				if (!screenCaptureViewGuid.Equals(Guid.Empty))
				{
					Dispatcher.CurrentDispatcher.Invoke(delegate
					{
						screenbitmap = RhinoDoc.ActiveDoc.Views.Find(screenCaptureViewGuid).CaptureToBitmap(captureSize);
					});
				}
				((DisplayConduit)_csDisplayConduitCapture).Enabled = false;
				_csDisplayConduitCapture.BackgroundColor = null;
				CSApplicationHelper.Application.Conduit = conduit;
				if (enabled)
				{
					((DisplayConduit)CSApplicationHelper.Application.Conduit).Enabled = true;
					CSUtil.Redraw();
				}
				RhUtil.RhinoApp().SetActiveViewport(activeViewportID);
			}
			catch (Exception ex)
			{
				LogHelper.Log.WriteLog("Error capturing Viewport - " + ex.Message + ".");
			}
			return screenbitmap ?? new Bitmap(100, 100);
		}
	}

	public static Bitmap CaptureViewportToBitmapForTargetedBlocksAtDefaultPerspective(List<Guid> targetedBlocks, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		Point3d cameraLocation = default(Point3d);
		((Point3d)(ref cameraLocation))._002Ector(45.0, -75.0, 50.0);
		Point3d origin = Point3d.Origin;
		Vector3d zAxis = Vector3d.ZAxis;
		return CaptureViewportToBitmapForTargetedBlocks(targetedBlocks, captureSize, cameraLocation, origin, 50.0, zAxis, zoomToObjects, zoomToObjectsPadding);
	}

	public static Bitmap CaptureViewportToBitmapForTargetedBlocksAtFront(List<Guid> targetedBlocks, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		Point3d cameraLocation = default(Point3d);
		((Point3d)(ref cameraLocation))._002Ector(0.0, -80.0, 0.0);
		Point3d origin = Point3d.Origin;
		Vector3d zAxis = Vector3d.ZAxis;
		return CaptureViewportToBitmapForTargetedBlocks(targetedBlocks, captureSize, cameraLocation, origin, 50.0, zAxis, zoomToObjects, zoomToObjectsPadding);
	}

	public static Bitmap CaptureViewportToBitmapForTargetedBlocksAtLibraryPerspective(List<Guid> targetedBlocks, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		Point3d cameraLocation = default(Point3d);
		((Point3d)(ref cameraLocation))._002Ector(20.135, -48.585, 122.895);
		Point3d targetLocation = default(Point3d);
		((Point3d)(ref targetLocation))._002Ector(0.0, 1.5, 5.0);
		Vector3d cameraUp = default(Vector3d);
		((Vector3d)(ref cameraUp))._002Ector(-0.339, 0.843, 0.416);
		if (CSUtil.DisplayComponentsInFile().Contains(DisplayComponents.Pendant))
		{
			((Point3d)(ref cameraLocation))._002Ector(16.5, -93.0, -36.5);
			((Point3d)(ref targetLocation))._002Ector(0.0, 0.0, 0.0);
			((Vector3d)(ref cameraUp))._002Ector(-0.2922108718447111, -0.3950387203817095, 0.87095190210191);
		}
		return CaptureViewportToBitmapForTargetedBlocks(targetedBlocks, captureSize, cameraLocation, targetLocation, 100.0, cameraUp, zoomToObjects, zoomToObjectsPadding);
	}

	public static Bitmap CaptureViewportToBitmapForTargetedBlocksAtSide(List<Guid> targetedBlocks, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		Point3d cameraLocation = default(Point3d);
		((Point3d)(ref cameraLocation))._002Ector(80.0, 0.0, 0.0);
		Point3d origin = Point3d.Origin;
		Vector3d zAxis = Vector3d.ZAxis;
		return CaptureViewportToBitmapForTargetedBlocks(targetedBlocks, captureSize, cameraLocation, origin, 50.0, zAxis, zoomToObjects, zoomToObjectsPadding);
	}

	public static Bitmap CaptureViewportToBitmapForTargetedBlocksAtTop(List<Guid> targetedBlocks, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		Point3d cameraLocation = default(Point3d);
		((Point3d)(ref cameraLocation))._002Ector(0.0, 0.0, 80.0);
		Point3d origin = Point3d.Origin;
		Vector3d yAxis = Vector3d.YAxis;
		return CaptureViewportToBitmapForTargetedBlocks(targetedBlocks, captureSize, cameraLocation, origin, 50.0, yAxis, zoomToObjects, zoomToObjectsPadding);
	}

	public static void CaptureViewportToFile(string path, Size captureSize, Point3d cameraLocation, Point3d targetLocation, double camera35MmLensLength, Vector3d cameraUp)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		CaptureViewportToFileForTargetedBlocks(new List<Guid>(), path, captureSize, cameraLocation, targetLocation, camera35MmLensLength, cameraUp);
	}

	public static void CaptureViewportToFile(string path, Size captureSize)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		RhinoViewport mainViewport = RhinoDoc.ActiveDoc.Views.ActiveView.MainViewport;
		Point3d cameraLocation = mainViewport.CameraLocation;
		Point3d cameraTarget = mainViewport.CameraTarget;
		double camera35mmLensLength = mainViewport.Camera35mmLensLength;
		Vector3d cameraUp = mainViewport.CameraUp;
		CaptureViewportToFileForTargetedBlocks(new List<Guid>(), path, captureSize, cameraLocation, cameraTarget, camera35mmLensLength, cameraUp);
	}

	public static void CaptureViewportToFileAtDefaultPerspective(string path, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		CaptureViewportToFileForTargetedBlocksAtDefaultPerspective(new List<Guid>(), path, captureSize, zoomToObjects, zoomToObjectsPadding);
	}

	public static void CaptureViewportToFileAtFront(string path, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		CaptureViewportToFileForTargetedBlocksAtTop(new List<Guid>(), path, captureSize, zoomToObjects, zoomToObjectsPadding);
	}

	public static void CaptureViewportToFileAtLibraryPerspective(string path, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		CaptureViewportToFileForTargetedBlocksAtLibraryPerspective(new List<Guid>(), path, captureSize, zoomToObjects, zoomToObjectsPadding);
	}

	public static void CaptureViewportToFileAtSide(string path, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		CaptureViewportToFileForTargetedBlocksAtSide(new List<Guid>(), path, captureSize, zoomToObjects, zoomToObjectsPadding);
	}

	public static void CaptureViewportToFileAtTop(string path, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		CaptureViewportToFileForTargetedBlocksAtFront(new List<Guid>(), path, captureSize, zoomToObjects, zoomToObjectsPadding);
	}

	public static void CaptureViewportToFileForTargetedBlocks(List<Guid> targetedBlocks, string path, Size captureSize, Point3d cameraLocation, Point3d targetLocation, double camera35MmLensLength, Vector3d cameraUp)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		CaptureViewportToBitmapForTargetedBlocks(targetedBlocks, captureSize, cameraLocation, targetLocation, camera35MmLensLength, cameraUp).Save(path);
	}

	public static void CaptureViewportToFileForTargetedBlocksAtDefaultPerspective(List<Guid> targetedBlocks, string path, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		CaptureViewportToBitmapForTargetedBlocksAtDefaultPerspective(targetedBlocks, captureSize, zoomToObjects, zoomToObjectsPadding).Save(path);
	}

	public static void CaptureViewportToFileForTargetedBlocksAtFront(List<Guid> targetedBlocks, string path, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		CaptureViewportToBitmapForTargetedBlocksAtFront(targetedBlocks, captureSize, zoomToObjects, zoomToObjectsPadding).Save(path);
	}

	public static void CaptureViewportToFileForTargetedBlocksAtLibraryPerspective(List<Guid> targetedBlocks, string path, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		CaptureViewportToBitmapForTargetedBlocksAtLibraryPerspective(targetedBlocks, captureSize, zoomToObjects, zoomToObjectsPadding).Save(path);
	}

	public static void CaptureViewportToFileForTargetedBlocksAtSide(List<Guid> targetedBlocks, string path, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		CaptureViewportToBitmapForTargetedBlocksAtSide(targetedBlocks, captureSize, zoomToObjects, zoomToObjectsPadding).Save(path);
	}

	public static void CaptureViewportToFileForTargetedBlocksAtTop(List<Guid> targetedBlocks, string path, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		CaptureViewportToBitmapForTargetedBlocksAtTop(targetedBlocks, captureSize, zoomToObjects, zoomToObjectsPadding).Save(path);
	}

	public static RhinoView CreateScreenCaptureView()
	{
		RhinoView val = RhinoDoc.ActiveDoc.Views.Find("screencapture", false);
		if (val != null)
		{
			return val;
		}
		RhinoView activeView = RhinoDoc.ActiveDoc.Views.ActiveView;
		val = RhinoDoc.ActiveDoc.Views.Add("ScreenCapture", (DefinedViewportProjection)7, new Rectangle(0, 0, 0, 0), false);
		if (activeView != null)
		{
			RhinoDoc.ActiveDoc.Views.ActiveView = activeView;
		}
		return val;
	}

	public static Guid CreateScreenCaptureViewGuid()
	{
		RhinoView val = RhinoDoc.ActiveDoc.Views.Find("screencapture", false);
		if (val == null)
		{
			RhinoView activeView = RhinoDoc.ActiveDoc.Views.ActiveView;
			val = RhinoDoc.ActiveDoc.Views.Add("screencapture", (DefinedViewportProjection)7, new Rectangle(0, 0, 0, 0), false);
			RhUtil.RhinoApp().SetActiveViewport(activeView.ActiveViewportID);
		}
		if (!CSApplicationHelper.IsCounterSketchStudio)
		{
			return val.ActiveViewportID;
		}
		RhinoView val2 = RhinoDoc.ActiveDoc.Views.Find("perspective", false);
		if (val2 != null)
		{
			RhUtil.RhinoApp().SetActiveViewport(val2.ActiveViewportID);
		}
		return val.ActiveViewportID;
	}

	public static void ResetViewports()
	{
		if (CSApplicationHelper.IsCounterSketchStudio)
		{
			return;
		}
		RhinoView activeView = RhinoDoc.ActiveDoc.Views.ActiveView;
		foreach (RhinoView item in ((IEnumerable<RhinoView>)RhinoDoc.ActiveDoc.Views).Where((RhinoView view) => view.ActiveViewport.Name.ToLowerInvariant() != "screencapture"))
		{
			if (item.ActiveViewportID != activeView.ActiveViewportID)
			{
				RhUtil.RhinoApp().SetActiveViewport(item.ActiveViewportID);
			}
		}
		RhUtil.RhinoApp().SetActiveViewport(activeView.ActiveViewportID);
	}
}
