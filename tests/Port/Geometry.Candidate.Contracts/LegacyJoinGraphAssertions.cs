using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;
internal static class LegacyJoinGraphAssertions
{
 static void Check(bool value,string message){if(!value)throw new Exception(message);}
 static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
 static LegacyCsg4Document Wire(LegacyCsg4Document d,int index,params (int Node,int Channel)[] links){for(int i=0;i<d.Records[index].SimpleBlock!.ObjectIndices.Count;i++)d=d.WithInputLink(index,i,i<links.Length?links[i].Node:-1,i<links.Length?links[i].Channel:-5);return d;}
 public static string Run(){
  using var stream=typeof(LegacyJoinGraphAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;using var memory=new MemoryStream();stream.CopyTo(memory);var original=memory.ToArray();var document=LegacyCsg4Codec.Read(original);Check(document.Records[140].SimpleBlock is not null,"Missing saved legacy join140 header");Check(document.Records[140].SimpleBlock!.ObjectIndices.SequenceEqual(new[]{134,138,139}),"Original join operand links changed");
  using var context=CandidateContext.Create();int states=0;
  void Joined(CandidateRailGraphSession graph,uint faces,uint parts,uint closed){Check(graph.Supports(140)&&graph.OutputChannelCount(140)==1&&graph.ReadOutputIdentifier(140,0)=="OutJoined","Legacy join output declaration differs");Check(graph.ReadOutput(140,0).Brep.HasValue,"Legacy join did not produce BRep data");var info=graph.ReadSewingDiagnostics(140);Check(info.Faces==faces&&info.Parts==parts&&info.ClosedShells==closed,"Legacy candidate join topology differs: "+info);if(closed>0)Check(info.FreeEdges==0,"Closed joined design has free edges");using var snapshot=graph.CaptureOutput(140);Check(snapshot.ReadTransform(snapshot.Value)==LegacyAffineTransform.Identity,"Legacy join should reset metadata to identity");using var shape=snapshot.RetainBrep(snapshot.Value);Check(context.Inspect(shape).IsValid,"Joined native geometry invalid");Throws<NotSupportedException>(()=>snapshot.RetainCurve(snapshot.Value));states++;}
  CandidateDataSnapshot retained;
  using(var graph=new CandidateRailGraphSession(context,document,new(1e-7,1e-7,1e-7))){
   graph.Generate(140);Joined(graph,3,1,1);retained=graph.CaptureOutput(140);using var before=retained.RetainBrep(retained.Value);var initial=context.GetShapeBounds(before,CandidateShapeBoundsMode.Accurate);Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(original),"Generating joined design changed file bytes");
   graph.Values.SetValue(2,document.Records[2].Scalar!.Value+.25);graph.Values.SetValue(136,.6);graph.Generate(140);Joined(graph,3,1,1);using(var changed=graph.RetainBrep(140))Check(context.GetShapeBounds(changed,CandidateShapeBoundsMode.Accurate)!=initial,"Joined geometry did not change after edits");Check(context.GetShapeBounds(before,CandidateShapeBoundsMode.Accurate)==initial,"Old joined snapshot mutated");
   var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()));using(var loaded=new CandidateRailGraphSession(context,saved,new(1e-7,1e-7,1e-7))){loaded.Generate(140);Joined(loaded,3,1,1);Check(loaded.ReadSewingDiagnostics(140)==graph.ReadSewingDiagnostics(140),"Reopened join diagnostics differ");using var a=graph.RetainBrep(140);using var b=loaded.RetainBrep(140);Check(Math.Abs(context.Inspect(a).Volume-context.Inspect(b).Volume)<1e-7,"Reopened join volume differs");}
   var old=graph.ReadOutput(140,0);ulong entries=context.DiagnosticLiveShapeCount;graph.Values.SetValue(136,double.NaN);Throws<CandidateGeometryException>(()=>graph.Generate(140));Check(ReferenceEquals(old,graph.ReadOutput(140,0))&&context.DiagnosticLiveShapeCount==entries,"Failed joined generation replaced cache or leaked");
   graph.Values.SetValue(136,.3);graph.Values.SetValue(2,document.Records[2].Scalar!.Value);graph.Generate(140);Joined(graph,3,1,1);Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(original),"Joined edit reversal changed original bytes");
  }
  using(var survivor=retained.RetainBrep(retained.Value))Check(context.Inspect(survivor).IsValid,"Joined snapshot lost geometry after graph disposal");retained.Dispose();Check(context.DiagnosticLiveShapeCount==0,"Joined snapshots leaked entries");
  var nested=Wire(Wire(Wire(document,187,(138,0),(8,0)),238,(187,0),(139,0)),140,(134,0),(238,0));using(var graph=new CandidateRailGraphSession(context,nested,new(1e-7,1e-7,1e-7))){graph.Generate(140);Joined(graph,3,1,1);}
  using(var graph=new CandidateRailGraphSession(context,Wire(document,140,(134,0),(138,0)),new(1e-7,1e-7,1e-7))){graph.Generate(140);Joined(graph,2,1,0);Check(graph.ReadSewingDiagnostics(140).FreeEdges>0,"Open join was forcibly closed");}
  using(var graph=new CandidateRailGraphSession(context,Wire(document,140,(138,0),(139,0)),new(1e-7,1e-7,1e-7))){graph.Generate(140);Joined(graph,2,2,0);}
  // Explicit source-update routing fixture, not a claim that these rewires pass original UI link filters.
  var conditional=Wire(Wire(Wire(document,138,(7,0),(136,0)),16,(138,0),(8,0),(41,0)),140,(16,0));
  using(var graph=new CandidateRailGraphSession(context,conditional,new(1e-7,1e-7,1e-7))){
   graph.Values.SetValue(41,1);graph.Generate(140);Check(!graph.ReadOutput(140,0).Brep.HasValue&&!graph.ReadOutput(140,0).IsMissing,"Initial empty join output differs");Throws<NotSupportedException>(()=>graph.ReadSewingDiagnostics(140));
   graph.Values.SetValue(41,0);graph.Generate(140);Joined(graph,1,1,0);var old=graph.ReadOutput(140,0);var oldInfo=graph.ReadSewingDiagnostics(140);using var snapshot=graph.CaptureOutput(140);using var shape=snapshot.RetainBrep(snapshot.Value);var bounds=context.GetShapeBounds(shape,CandidateShapeBoundsMode.Accurate);ulong entries=context.DiagnosticLiveShapeCount;
   graph.Values.SetValue(41,1);graph.Generate(140);Check(ReferenceEquals(old,graph.ReadOutput(140,0))&&oldInfo==graph.ReadSewingDiagnostics(140),"Native-empty null join cleared/replaced previous OutJoined");Joined(graph,1,1,0);Check(context.DiagnosticLiveShapeCount==entries&&context.GetShapeBounds(shape,CandidateShapeBoundsMode.Accurate)==bounds,"Null join lost retained native geometry/ownership");
   graph.Values.SetValue(41,0);graph.Generate(140);Joined(graph,1,1,0);Check(!ReferenceEquals(old,graph.ReadOutput(140,0)),"Nonempty join failed to replace previous data");
  }
  using(var graph=new CandidateRailGraphSession(context,Wire(document,140),new(1e-7,1e-7,1e-7))){graph.Generate(140);Check(graph.ReadOutput(140,0)==LegacyChannelValue.Empty,"Disconnected initial join should stay empty");}
  foreach(var emptySource in new[]{Wire(document,140,(4,0),(8,0)),Wire(document,140,(134,-1))})using(var graph=new CandidateRailGraphSession(context,emptySource,new(1e-7,1e-7,1e-7))){graph.Generate(140);Check(graph.ReadOutput(140,0)==LegacyChannelValue.Empty,"Non-BRep/negative channel was expanded into join geometry");}
  using(var graph=new CandidateRailGraphSession(context,Wire(document,140,(140,0)),new(1e-7,1e-7,1e-7)))Throws<InvalidOperationException>(()=>graph.Generate(140));
  Check(context.DiagnosticLiveShapeCount==0,"Legacy join graph cleanup leaked entries");return "PASS saved legacy join140:"+states+" native states, actual closed3-face design/edit-save-reopen/reversal, mixed arrays/open/disconnected parts, binary-proven empty retention/output identity and native ownership/failure/cycle guards; private repair/original-PC/full feature parity separate.";
 }
}
