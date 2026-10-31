#nullable disable
// Exact recovered wrapper with observation-only viewport types; no native projection implementation.
using System;
using System.Collections.Generic;
using System.Linq;
namespace CandidateGeometryContractChecks.Recovered.CameraPresets;
public enum CameraView
{
	Perspective,
	Front,
	Top,
	Side
}
internal enum Plane { WorldXY,WorldZX,WorldYZ }
internal readonly record struct Point3d(double X,double Y,double Z) { internal static Point3d Origin=>new(0,0,0); }
internal readonly record struct Vector3d(double X,double Y,double Z) { internal static Vector3d YAxis=>new(0,1,0);internal static Vector3d ZAxis=>new(0,0,1); }
internal static class Observation {internal static readonly List<string> Trace=new();}
internal sealed class RhinoViewport {
 internal string Name;
 internal bool IsParallelProjection;
 internal Plane ConstructionPlane;
 internal Point3d Target,Camera;
 private double lens;private Vector3d up;
 internal RhinoViewport(string name){Name=name;}
 internal bool ChangeToPerspectiveProjection(bool symmetricFrustum,double lensLength){Observation.Trace.Add("perspective:"+symmetricFrustum+":"+lensLength);IsParallelProjection=false;return true;}
 internal bool ChangeToParallelProjection(bool symmetricFrustum){Observation.Trace.Add("parallel:"+symmetricFrustum);IsParallelProjection=true;return true;}
 internal void SetConstructionPlane(Plane plane){ConstructionPlane=plane;Observation.Trace.Add("plane:"+plane);}
 internal void SetCameraLocations(Point3d target,Point3d camera){Target=target;Camera=camera;Observation.Trace.Add("locations");}
 internal double Camera35mmLensLength {get=>lens;set{lens=value;Observation.Trace.Add("lens");}}
 internal Vector3d CameraUp {get=>up;set{up=value;Observation.Trace.Add("up");}}
}
internal sealed class RhinoView {internal RhinoViewport MainViewport;internal RhinoView(string name){MainViewport=new(name);}}
internal sealed class ViewTable {internal RhinoView ActiveView=new("active");internal RhinoView[] StandardViews=Array.Empty<RhinoView>();internal IEnumerable<RhinoView> GetStandardRhinoViews()=>StandardViews;}
internal sealed class Document {internal ViewTable Views=new();}
internal static class RhinoDoc {internal static Document ActiveDoc=new();}
internal static class CSApplicationHelper {internal sealed class App {private CameraView view;internal CameraView CurrentCameraViewName{get=>view;set{view=value;Observation.Trace.Add("state:"+value);}}}internal static App Application=new();}

internal static class CSUtil
{
private static void Redraw()=>Observation.Trace.Add("redraw");
public static void DocumentSetCameraView(string cameraViewName)
	{
		cameraViewName = cameraViewName.Trim().ToLowerInvariant();
		RhinoViewport mainViewport = RhinoDoc.ActiveDoc.Views.ActiveView.MainViewport;
		RhinoView rhinoView = RhinoDoc.ActiveDoc.Views.GetStandardRhinoViews().FirstOrDefault((RhinoView view) => view.MainViewport.Name.ToLower() == "perspective");
		if (rhinoView != null)
		{
			mainViewport = rhinoView.MainViewport;
		}
		mainViewport.ChangeToPerspectiveProjection(symmetricFrustum: true, 50.0);
		Point3d cameraLocation;
		Point3d origin;
		Vector3d cameraUp;
		switch (cameraViewName)
		{
		case "top":
			mainViewport.SetConstructionPlane(Plane.WorldXY);
			cameraLocation = new Point3d(0.0, 0.0, 80.0);
			origin = Point3d.Origin;
			cameraUp = Vector3d.YAxis;
			CSApplicationHelper.Application.CurrentCameraViewName = CameraView.Top;
			break;
		case "front":
			mainViewport.SetConstructionPlane(Plane.WorldZX);
			cameraLocation = new Point3d(0.0, -80.0, 0.0);
			origin = Point3d.Origin;
			cameraUp = Vector3d.ZAxis;
			CSApplicationHelper.Application.CurrentCameraViewName = CameraView.Front;
			break;
		case "side":
			mainViewport.SetConstructionPlane(Plane.WorldYZ);
			cameraLocation = new Point3d(80.0, 0.0, 0.0);
			origin = Point3d.Origin;
			cameraUp = Vector3d.ZAxis;
			CSApplicationHelper.Application.CurrentCameraViewName = CameraView.Side;
			break;
		default:
			mainViewport.SetConstructionPlane(Plane.WorldXY);
			cameraLocation = new Point3d(45.0, -75.0, 50.0);
			origin = Point3d.Origin;
			cameraUp = Vector3d.ZAxis;
			CSApplicationHelper.Application.CurrentCameraViewName = CameraView.Perspective;
			break;
		}
		mainViewport.SetCameraLocations(origin, cameraLocation);
		mainViewport.Camera35mmLensLength = 50.0;
		mainViewport.CameraUp = cameraUp;
		if (cameraViewName != "perspective" && !mainViewport.IsParallelProjection)
		{
			mainViewport.ChangeToParallelProjection(symmetricFrustum: true);
		}
		Redraw();
	}
}
