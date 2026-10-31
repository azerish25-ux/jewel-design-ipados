using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;
internal static class ProfileCapGraphAssertions
{
 static void Check(bool value,string message){if(!value)throw new Exception(message);}
 static void Near(CandidatePoint3 a,CandidatePoint3 b){Check(Math.Abs(a.X-b.X)<1e-8&&Math.Abs(a.Y-b.Y)<1e-8&&Math.Abs(a.Z-b.Z)<1e-8,"Saved cap point differs: "+a+" / "+b);}
 static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
 static LegacyCsg4Document Wire(LegacyCsg4Document document,int index,params (int Node,int Channel)[] links){for(int i=0;i<document.Records[index].SimpleBlock!.ObjectIndices.Count;i++)document=document.WithInputLink(index,i,i<links.Length?links[i].Node:-1,i<links.Length?links[i].Channel:-5);return document;}
 public static string Run(){
  using var stream=typeof(ProfileCapGraphAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;using var memory=new MemoryStream();stream.CopyTo(memory);var original=memory.ToArray();var document=LegacyCsg4Codec.Read(original);
  Check(document.Records[138].SimpleBlock is not null&&document.Records[139].SimpleBlock is not null,"Missing saved ProfileCap headers138/139");
  Check(document.Records[138].SimpleBlock!.ObjectIndices.SequenceEqual(new[]{115,136})&&document.Records[139].SimpleBlock!.ChannelIndices.SequenceEqual(new[]{0,1}),"Saved cap bindings differ");
  Check(LegacyCsg4Codec.Write(document).AsSpan().SequenceEqual(original),"Cap header decode changed original bytes");
  using var context=CandidateContext.Create();int caps=0,boundaries=0;
  CandidatePoint3 Sample(CandidateShape face,double a,double b){var d=context.InspectSurface(face);return context.PointAtSurface(face,d.FirstU+(d.LastU-d.FirstU)*a,d.FirstV+(d.LastV-d.FirstV)*b);}
  void Cap(CandidateRailGraphSession graph,int index,double amount){
   Check(graph.Supports(index)&&graph.OutputChannelCount(index)==1,"Saved cap producer/output count missing");var data=graph.ReadOutput(index,0);Check(data.Brep.HasValue&&!data.Curve.HasValue,"Saved cap output kind differs");Check(graph.ReadOutputIdentifier(index,0)=="oBrep","Source cap channel identifier differs");
   using var snapshot=graph.CaptureOutput(index);using var face=snapshot.RetainBrep(snapshot.Value);using var curve=graph.RetainCurve(index==138?115:122);Check(context.Inspect(face).IsValid,"Saved native cap invalid");var d=context.InspectCurve(curve);
   for(int i=0;i<=32;i++){double t=i/32.0;Near(Sample(face,0,t),context.PointAt(curve,d.FirstParameter+(d.LastParameter-d.FirstParameter)*t));boundaries++;}
   Check(Math.Abs(context.InspectSurface(face).LastU-(Math.Abs(amount)<.005?.5:2))<1e-12,"Source cap dome threshold/domain changed");var m=snapshot.ReadTransform(snapshot.Value);double apexHeight=Math.Abs(amount)<.005?0:amount;Near(Sample(face,1,.5),new(m.M13*apexHeight+m.M14,m.M23*apexHeight+m.M24,m.M33*apexHeight+m.M34));Throws<NotSupportedException>(()=>snapshot.RetainCurve(snapshot.Value));caps++;
  }
  CandidateDataSnapshot retained;
  using(var graph=new CandidateRailGraphSession(context,document,new(1e-7,1e-7,1e-7))){
   graph.Generate(138,139);double positive=0,negative=0;Check(graph.Values.ReadOutput(136,0).GetDouble(ref positive)&&graph.Values.ReadOutput(136,1).GetDouble(ref negative)&&positive==.3&&negative==-.3,"Saved thickness channels changed");Cap(graph,138,.3);Cap(graph,139,-.3);
   retained=graph.CaptureOutput(138);using var oldFace=retained.RetainBrep(retained.Value);var oldPoint=Sample(oldFace,.6,.4);Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(original),"Generating caps changed archive");
   graph.Values.SetValue(136,.6);graph.Values.SetValue(2,document.Records[2].Scalar!.Value+.25);graph.Generate(138,139);Cap(graph,138,.6);Cap(graph,139,-.6);Near(Sample(oldFace,.6,.4),oldPoint);using(var changed=graph.RetainBrep(138))Check(Sample(changed,.6,.4)!=oldPoint,"Native cap unchanged after parameter edits");
   var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()));using(var loaded=new CandidateRailGraphSession(context,saved,new(1e-7,1e-7,1e-7))){loaded.Generate(138,139);Cap(loaded,138,.6);Cap(loaded,139,-.6);foreach(int index in new[]{138,139}){using var a=graph.RetainBrep(index);using var b=loaded.RetainBrep(index);for(int i=0;i<5;i++)Near(Sample(a,.2*i,.37),Sample(b,.2*i,.37));}}
   var cached=graph.ReadOutput(138,0);ulong entries=context.DiagnosticLiveShapeCount;graph.Values.SetValue(136,double.NaN);Throws<CandidateGeometryException>(()=>graph.Generate(138,139));Check(ReferenceEquals(cached,graph.ReadOutput(138,0))&&entries==context.DiagnosticLiveShapeCount,"Failed cap generation replaced cache or leaked native entries");
   graph.Values.SetValue(136,.3);graph.Values.SetValue(2,document.Records[2].Scalar!.Value);graph.Generate(138,139);Cap(graph,138,.3);Cap(graph,139,-.3);Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(original),"Cap edit reversal changed original archive");
   foreach(double height in new[]{0.0,.004999,.005,-.004999,-.005,-2.0,2.0}){graph.Values.SetValue(136,height);graph.Generate(138,139);double effective=height==.005?.004999:height==-.005?-.004999:height,actual=0;Check(graph.Values.ReadOutput(136,0).GetDouble(ref actual)&&actual==effective,"Source1e-5 value edit deadband changed");Cap(graph,138,effective);Cap(graph,139,0.0-effective);}
   graph.Values.SetValue(136,.3);graph.Generate(138,139);Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(original),"Cap boundary edit reversal changed archive");
  }
  using(var survivor=retained.RetainBrep(retained.Value))Check(context.Inspect(survivor).IsValid,"Cap snapshot lost native geometry after graph disposal");retained.Dispose();retained.Dispose();Check(context.DiagnosticLiveShapeCount==0,"Saved caps retained native entries");
  var nested=Wire(Wire(Wire(Wire(document,61),187,(61,0),(115,0)),238,(187,0),(122,0)),138,(238,0),(136,0));
  using(var graph=new CandidateRailGraphSession(context,nested,new(1e-7,1e-7,1e-7))){graph.Generate(138);Cap(graph,138,.3);}
  foreach(double stored in new[]{-.005,.005})using(var graph=new CandidateRailGraphSession(context,document.WithScalarValue(136,stored),new(1e-7,1e-7,1e-7))){graph.Generate(138,139);Cap(graph,138,stored);Cap(graph,139,0.0-stored);}
  var arrayThickness=Wire(document,187,(8,0),(115,0));
  foreach(var alternative in new[]{Wire(document,138,(115,0),(-1,-5)),Wire(document,138,(115,0),(4,0)),Wire(arrayThickness,138,(115,0),(187,0))})using(var graph=new CandidateRailGraphSession(context,alternative,new(1e-7,1e-7,1e-7))){graph.Generate(138);Cap(graph,138,.5);}
  var numericFirst=Wire(Wire(document,187,(8,0),(115,0)),138,(187,0),(136,0));
  foreach(var invalid in new[]{Wire(document,138,(-1,-5),(136,0)),Wire(document,138,(8,0),(136,0)),Wire(document,138,(134,0),(136,0)),numericFirst})using(var graph=new CandidateRailGraphSession(context,invalid,new(1e-7,1e-7,1e-7))){ulong baseline=context.DiagnosticLiveShapeCount;Throws<NotSupportedException>(()=>graph.Generate(138));Check(context.DiagnosticLiveShapeCount==baseline,"Missing/noncurve cap input leaked entries");}
  var cyclic=Wire(Wire(document,138,(139,0),(136,0)),139,(138,0),(136,1));using(var graph=new CandidateRailGraphSession(context,cyclic,new(1e-7,1e-7,1e-7)))Throws<InvalidOperationException>(()=>graph.Generate(138));
  Check(context.DiagnosticLiveShapeCount==0,"Cap routing/failure cleanup leaked entries");return "PASS saved ProfileCap graph:"+caps+" native cap states and"+boundaries+" source-profile boundary samples, actual138/139 signed136/default/flatten-first/name/metadata, edit-save-reopen/reversal/retained generations/failure/cycle/cleanup; join/original-PC/full feature parity separate.";
 }
}
