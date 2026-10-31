using System.Globalization;
using JewelDesign.Rendering.Candidate;
using R=CandidateGeometryContractChecks.Recovered.CameraPresets;
namespace CandidateGeometryContractChecks;
/// <summary>Compares original managed wrapper decisions only; observation types do not implement Rhino projection.</summary>
public static class LegacyCameraPresetAssertions
{
 static void Check(bool value,string message){if(!value)throw new Exception(message);}
 static T Throws<T>(Action action)where T:Exception{try{action();}catch(T error){return error;}throw new Exception("Expected "+typeof(T).Name);}
 public static string Run(){var previous=CultureInfo.CurrentCulture;int cases=0;try{
  foreach(var culture in new[]{CultureInfo.InvariantCulture,new CultureInfo("en-US"),new CultureInfo("tr-TR")}){
   CultureInfo.CurrentCulture=culture;
   foreach(var name in new[]{"perspective","front","top","side","unknown","","PERSPECTİVE","perSPECTIve","top\0","\u200btop"})
   foreach(var value in new[]{name,name.ToUpperInvariant()," "+name+"\t","\u2003"+name+"\u00a0"}){
    R.Observation.Trace.Clear();R.RhinoDoc.ActiveDoc=new();R.CSApplicationHelper.Application=new();
    var first=new R.RhinoView("perspective");var second=new R.RhinoView("perspective");R.RhinoDoc.ActiveDoc.Views.StandardViews=new[]{first,second};
    R.CSUtil.DocumentSetCameraView(value);var plan=LegacyCameraPreset.Resolve(value);var source=first.MainViewport;
    Check(source.Camera.X==plan.CameraLocation.X&&source.Camera.Y==plan.CameraLocation.Y&&source.Camera.Z==plan.CameraLocation.Z,"Camera source pose differs");
    Check(source.Target.X==plan.Target.X&&source.Target.Y==plan.Target.Y&&source.Target.Z==plan.Target.Z,"Camera source target differs");
    Check(source.CameraUp.X==plan.Up.X&&source.CameraUp.Y==plan.Up.Y&&source.CameraUp.Z==plan.Up.Z,"Camera source up differs");
    Check(source.ConstructionPlane.ToString()==plan.ConstructionPlane.ToString()&&R.CSApplicationHelper.Application.CurrentCameraViewName.ToString()==plan.View.ToString(),"Camera source plane/state differs");Check((int)R.CSApplicationHelper.Application.CurrentCameraViewName==(int)plan.View,"Original camera enum identity differs");
    Check(source.IsParallelProjection==plan.UseParallelProjection&&source.Camera35mmLensLength==plan.LensMillimeters&&plan.SymmetricFrustum,"Camera projection request differs");
    Check(plan.NormalizedName==value.Trim().ToLowerInvariant(),"Camera normalization differs");
    var expected=new List<string>{"perspective:True:50","plane:"+plan.ConstructionPlane,"state:"+plan.View,"locations","lens","up"};if(plan.UseParallelProjection)expected.Add("parallel:True");expected.Add("redraw");
    Check(R.Observation.Trace.SequenceEqual(expected),"Source wrapper call sequence differs");Check(second.MainViewport.Camera==default&&R.RhinoDoc.ActiveDoc.Views.ActiveView.MainViewport.Camera==default,"Source did not choose first perspective standard viewport");cases++;
   }
   R.Observation.Trace.Clear();Throws<NullReferenceException>(()=>R.CSUtil.DocumentSetCameraView(null!));Throws<NullReferenceException>(()=>LegacyCameraPreset.Resolve(null!));Check(R.Observation.Trace.Count==0,"Null name touched viewport before failure");
   R.RhinoDoc.ActiveDoc=new();var upper=new R.RhinoView("PERSPECTIVE");R.RhinoDoc.ActiveDoc.Views.StandardViews=new[]{upper};R.CSUtil.DocumentSetCameraView("top");
   Check((upper.MainViewport.Camera.Z==80)==(culture.Name!="tr-TR"),"Original viewport-name lookup culture changed");
   Check((R.RhinoDoc.ActiveDoc.Views.ActiveView.MainViewport.Camera.Z==80)==(culture.Name=="tr-TR"),"Original active viewport fallback changed");
  }
  var top=LegacyCameraPreset.Resolve("top");var front=LegacyCameraPreset.Resolve("front");var side=LegacyCameraPreset.Resolve("side");var perspective=LegacyCameraPreset.Resolve("perspective");var unknown=LegacyCameraPreset.Resolve("no-such-view");
  Check(top.CameraLocation.Z==80&&front.CameraLocation.Y==-80&&side.CameraLocation.X==80,"Original cardinal camera constants changed");Check(perspective.CameraLocation.X==45&&perspective.CameraLocation.Y==-75&&perspective.CameraLocation.Z==50&&!perspective.UseParallelProjection,"Original perspective constant changed");Check(unknown.View==LegacyCameraView.Perspective&&unknown.UseParallelProjection,"Unknown input loses original pose/state versus projection distinction");
 }finally{CultureInfo.CurrentCulture=previous;}
 return "PASS source camera presets:"+cases+" unchanged-wrapper comparisons across invariant/en-US/tr-TR, original pose/up/plane/lens/projection, null and unknown-name behavior; source call-order/viewport preference observed, native frustum/zoom/UI/PC parity separate.";
 }
}
