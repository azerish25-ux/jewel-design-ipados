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
		return CaptureViewportToBitmapForTargetedBlocks(new List<Guid>(), captureSize, cameraLocation, targetLocation, camera35MmLensLength, cameraUp);
	}

	public static Bitmap CaptureViewportToBitmap(Size captureSize)
	{
		RhinoViewport mainViewport = RhinoDoc.ActiveDoc.Views.ActiveView.MainViewport;
		Point3d cameraLocation = mainViewport.CameraLocation;
		Point3d targetLocation = mainViewport.CameraTarget;
		double camera35MmLensLength = mainViewport.Camera35mmLensLength;
		Vector3d cameraUp = mainViewport.CameraUp;
		switch (CSApplicationHelper.Application.CurrentCameraViewName)
		{
		case CameraView.Top:
			camera35MmLensLength = 50.0;
			cameraLocation = new Point3d(0.0, 0.0, 80.0);
			targetLocation = Point3d.Origin;
			cameraUp = Vector3d.YAxis;
			break;
		case CameraView.Front:
			camera35MmLensLength = 50.0;
			cameraLocation = new Point3d(0.0, -80.0, 0.0);
			targetLocation = Point3d.Origin;
			cameraUp = Vector3d.ZAxis;
			break;
		case CameraView.Side:
			camera35MmLensLength = 50.0;
			cameraLocation = new Point3d(80.0, 0.0, 0.0);
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
		lock (_viewportCaptureLock)
		{
			Bitmap screenbitmap = null;
			try
			{
				Guid activeViewportID = RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewportID;
				Guid screenCaptureViewGuid = CreateScreenCaptureViewGuid();
				RhinoViewport activeViewport = RhinoDoc.ActiveDoc.Views.Find(screenCaptureViewGuid).ActiveViewport;
				activeViewport.Size = captureSize;
				bool enabled = CSApplicationHelper.Application.Conduit.Enabled;
				CSDisplayConduit conduit = CSApplicationHelper.Application.Conduit;
				conduit.Enabled = false;
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
				_csDisplayConduitCapture.Enabled = true;
				activeViewport.ConstructionGridVisible = false;
				activeViewport.WorldAxesVisible = false;
				activeViewport.ConstructionAxesVisible = false;
				activeViewport.SetWallpaper(string.Empty, grayscale: false, visible: false);
				_csDisplayConduitCapture.BackgroundColor = Color.White;
				activeViewport.ChangeToPerspectiveProjection(symmetricFrustum: true, camera35MmLensLength);
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
							if (csData != null)
							{
								BoundingBox boundingBox = BoundingBox.Unset;
								if (csData.GetTightBoundingBoxNew(ref boundingBox))
								{
									boundingBox.Transform(_csDisplayConduitCapture.GetDesignTransform());
									blockBoundingBox.Union(boundingBox);
								}
							}
						});
					});
					BoundingBox box = new BoundingBox(new Point3d(blockBoundingBox.Min.X, blockBoundingBox.Min.Y, blockBoundingBox.Min.Z), new Point3d(blockBoundingBox.Max.X, blockBoundingBox.Max.Y, blockBoundingBox.Max.Z));
					if (num)
					{
						double xAmount = ((box.Diagonal.X + zoomToObjectsPadding > 0.0) ? zoomToObjectsPadding : 0.0);
						double yAmount = ((box.Diagonal.Y + zoomToObjectsPadding > 0.0) ? zoomToObjectsPadding : 0.0);
						double zAmount = ((box.Diagonal.Z + zoomToObjectsPadding > 0.0) ? zoomToObjectsPadding : 0.0);
						box.Inflate(xAmount, yAmount, zAmount);
					}
					activeViewport.SetCameraLocations(box.Center, cameraLocation);
					activeViewport.ZoomBoundingBox(box);
				}
				if (!screenCaptureViewGuid.Equals(Guid.Empty))
				{
					Dispatcher.CurrentDispatcher.Invoke(delegate
					{
						screenbitmap = RhinoDoc.ActiveDoc.Views.Find(screenCaptureViewGuid).CaptureToBitmap(captureSize);
					});
				}
				_csDisplayConduitCapture.Enabled = false;
				_csDisplayConduitCapture.BackgroundColor = null;
				CSApplicationHelper.Application.Conduit = conduit;
				if (enabled)
				{
					CSApplicationHelper.Application.Conduit.Enabled = true;
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
		Point3d cameraLocation = new Point3d(45.0, -75.0, 50.0);
		Point3d origin = Point3d.Origin;
		Vector3d zAxis = Vector3d.ZAxis;
		return CaptureViewportToBitmapForTargetedBlocks(targetedBlocks, captureSize, cameraLocation, origin, 50.0, zAxis, zoomToObjects, zoomToObjectsPadding);
	}

	public static Bitmap CaptureViewportToBitmapForTargetedBlocksAtFront(List<Guid> targetedBlocks, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		Point3d cameraLocation = new Point3d(0.0, -80.0, 0.0);
		Point3d origin = Point3d.Origin;
		Vector3d zAxis = Vector3d.ZAxis;
		return CaptureViewportToBitmapForTargetedBlocks(targetedBlocks, captureSize, cameraLocation, origin, 50.0, zAxis, zoomToObjects, zoomToObjectsPadding);
	}

	public static Bitmap CaptureViewportToBitmapForTargetedBlocksAtLibraryPerspective(List<Guid> targetedBlocks, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		Point3d cameraLocation = new Point3d(20.135, -48.585, 122.895);
		Point3d targetLocation = new Point3d(0.0, 1.5, 5.0);
		Vector3d cameraUp = new Vector3d(-0.339, 0.843, 0.416);
		if (CSUtil.DisplayComponentsInFile().Contains(DisplayComponents.Pendant))
		{
			cameraLocation = new Point3d(16.5, -93.0, -36.5);
			targetLocation = new Point3d(0.0, 0.0, 0.0);
			cameraUp = new Vector3d(-0.2922108718447111, -0.3950387203817095, 0.87095190210191);
		}
		return CaptureViewportToBitmapForTargetedBlocks(targetedBlocks, captureSize, cameraLocation, targetLocation, 100.0, cameraUp, zoomToObjects, zoomToObjectsPadding);
	}

	public static Bitmap CaptureViewportToBitmapForTargetedBlocksAtSide(List<Guid> targetedBlocks, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		Point3d cameraLocation = new Point3d(80.0, 0.0, 0.0);
		Point3d origin = Point3d.Origin;
		Vector3d zAxis = Vector3d.ZAxis;
		return CaptureViewportToBitmapForTargetedBlocks(targetedBlocks, captureSize, cameraLocation, origin, 50.0, zAxis, zoomToObjects, zoomToObjectsPadding);
	}

	public static Bitmap CaptureViewportToBitmapForTargetedBlocksAtTop(List<Guid> targetedBlocks, Size captureSize, bool zoomToObjects = false, double zoomToObjectsPadding = 0.5)
	{
		Point3d cameraLocation = new Point3d(0.0, 0.0, 80.0);
		Point3d origin = Point3d.Origin;
		Vector3d yAxis = Vector3d.YAxis;
		return CaptureViewportToBitmapForTargetedBlocks(targetedBlocks, captureSize, cameraLocation, origin, 50.0, yAxis, zoomToObjects, zoomToObjectsPadding);
	}

	public static void CaptureViewportToFile(string path, Size captureSize, Point3d cameraLocation, Point3d targetLocation, double camera35MmLensLength, Vector3d cameraUp)
	{
		CaptureViewportToFileForTargetedBlocks(new List<Guid>(), path, captureSize, cameraLocation, targetLocation, camera35MmLensLength, cameraUp);
	}

	public static void CaptureViewportToFile(string path, Size captureSize)
	{
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
		RhinoView rhinoView = RhinoDoc.ActiveDoc.Views.Find("screencapture", compareCase: false);
		if (rhinoView != null)
		{
			return rhinoView;
		}
		RhinoView activeView = RhinoDoc.ActiveDoc.Views.ActiveView;
		rhinoView = RhinoDoc.ActiveDoc.Views.Add("ScreenCapture", DefinedViewportProjection.Perspective, new Rectangle(0, 0, 0, 0), floating: false);
		if (activeView != null)
		{
			RhinoDoc.ActiveDoc.Views.ActiveView = activeView;
		}
		return rhinoView;
	}

	public static Guid CreateScreenCaptureViewGuid()
	{
		RhinoView rhinoView = RhinoDoc.ActiveDoc.Views.Find("screencapture", compareCase: false);
		if (rhinoView == null)
		{
			RhinoView activeView = RhinoDoc.ActiveDoc.Views.ActiveView;
			rhinoView = RhinoDoc.ActiveDoc.Views.Add("screencapture", DefinedViewportProjection.Perspective, new Rectangle(0, 0, 0, 0), floating: false);
			RhUtil.RhinoApp().SetActiveViewport(activeView.ActiveViewportID);
		}
		if (!CSApplicationHelper.IsCounterSketchStudio)
		{
			return rhinoView.ActiveViewportID;
		}
		RhinoView rhinoView2 = RhinoDoc.ActiveDoc.Views.Find("perspective", compareCase: false);
		if (rhinoView2 != null)
		{
			RhUtil.RhinoApp().SetActiveViewport(rhinoView2.ActiveViewportID);
		}
		return rhinoView.ActiveViewportID;
	}

	public static void ResetViewports()
	{
		if (CSApplicationHelper.IsCounterSketchStudio)
		{
			return;
		}
		RhinoView activeView = RhinoDoc.ActiveDoc.Views.ActiveView;
		foreach (RhinoView item in RhinoDoc.ActiveDoc.Views.Where((RhinoView view) => view.ActiveViewport.Name.ToLowerInvariant() != "screencapture"))
		{
			if (item.ActiveViewportID != activeView.ActiveViewportID)
			{
				RhUtil.RhinoApp().SetActiveViewport(item.ActiveViewportID);
			}
		}
		RhUtil.RhinoApp().SetActiveViewport(activeView.ActiveViewportID);
	}
}
