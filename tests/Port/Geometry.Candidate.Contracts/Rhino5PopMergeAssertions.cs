using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Views.Candidate;
namespace CandidateGeometryContractChecks;
public static class Rhino5PopMergeAssertions
{
 public static string Run()
 {
  int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
  var runtime=new Rhino5UserDataRuntime();var binding=Rhino5UserStringArchiveBinding.Register(runtime);
  var body=LegacyUserStringListCodec.Write(new[]{new KeyValuePair<string,string>("saved","値")},50);
  using var current=Rhino5ViewDefaults.Create(runtime);using var saved=Rhino5ViewDefaults.Create(runtime);using var result=Rhino5ViewDefaults.Create(runtime);
  var currentClip=new[]{new Rhino5ClippingPlaneInfo(new(1,2,3,4),Id(10),true)};
  current.SetConvertedValues(current.Viewport,current.TargetMirror,current.Details,current.Metadata with {Name="current",ProjectionLocked=true,ViewType=2},currentClip);
  saved.SetConvertedValues(saved.Viewport with {Target=new(1,2,3),Projection=99,SymmetryFlags=0x83},new(9,8,7),saved.Details with {ViewportId=Id(11),OpaqueCopyByte=0xaa,ScreenPort=new(1,7,8,2,3,9,true)},saved.Metadata with {Name="saved",ProjectionLocked=false},[]);
  var data=new Rhino5UserStringList(runtime);binding.ReadInto(data,body,50);saved.AttachUserData(data);
  result.SetConvertedValues(result.Viewport,result.TargetMirror,result.Details with {OpaqueCopyByte=0x42},result.Metadata,[]);
  int before=runtime.DiagnosticCount;
  Rhino5PopViewMerge.Apply(saved,current,result);
  Check(result.Viewport==saved.Viewport&&result.Details.ViewportId==Id(11)&&result.Details.OpaqueCopyByte==0x42,"Saved viewport overlays current metadata while preserving destination opaque byte");
  Check(result.Metadata==(current.Metadata with {Name="saved"})&&result.ClippingPlaneCount==1&&result.GetClippingPlane(0)==currentClip[0],"Merge keeps current metadata/clipping, adopting only eligible saved name");
  Check(saved.TargetMirror==saved.Viewport.Target&&result.TargetMirror==saved.Viewport.Target&&runtime.DiagnosticCount==before+1,"Saved target getter repairs stale source mirror and result setter synchronizes it");
  Check(result.Details.ScreenPort==saved.Details.ScreenPort,"Zero-port call is rejected without clearing copied saved screen state");
  Check(result.Head is Rhino5UserStringList strings&&strings.CopyCount==2&&binding.WriteForArchive(strings,50).SequenceEqual(body)&&ReferenceEquals(strings.Owner,result),"Actual saved userdata/provenance replaces current data through native viewport assignment");
  using(var alias=Rhino5ViewDefaults.Create(runtime))
  {
   alias.SetConvertedValues(alias.Viewport,alias.TargetMirror,alias.Details,alias.Metadata with {Name="alias"},[]);
   Rhino5PopViewMerge.Apply(saved,alias,alias);Check(alias.Metadata.Name=="alias"&&alias.Head is null,"Current=result alias suppresses all copy/merge effects");
   Rhino5PopViewMerge.Apply(alias,alias,result);Check(result.Metadata.Name=="alias"&&result.Head is null,"Saved=current alias performs only initial current assignment");
   Rhino5PopViewMerge.Apply(result,alias,result);Check(result.Metadata.Name=="alias","Saved=result alias performs initial replacement but no saved overlay");
   Rhino5PopViewMerge.Apply(null,current,result);Check(result.Metadata==current.Metadata&&result.Viewport==current.Viewport,"Null saved performs one current whole assignment only");
  }
  foreach(var row in new[]{("",false),(" ",false),("\tA",false),("A\n",false),("A",true),("A B",true),("\vA\v",true),("\fA\f",true),("\u00a0A\u00a0",true),("\u3000A\u3000",true),("A\0  ",true),("\0A",false),(" A\0B",false),("A \0",true),("A\0B\t",true)})
  {
   Check(Rhino5PopViewMerge.IsEligibleSavedName(row.Item1)==row.Item2,"Native four-character trim/length/NUL compare name eligibility");
   saved.SetConvertedValues(saved.Viewport,saved.TargetMirror,saved.Details,saved.Metadata with {Name=row.Item1},[]);
   Rhino5PopViewMerge.Apply(saved,current,result);Check(result.Metadata.Name==(row.Item2?row.Item1:"current"),"Merge adopts original untrimmed name only when native predicate accepts");
  }
  current.SetConvertedValues(current.Viewport,current.TargetMirror,current.Details,current.Metadata with {ViewType=1},current.CopyClippingPlanes());
  saved.SetConvertedValues(saved.Viewport,saved.TargetMirror,saved.Details,saved.Metadata with {Name="page saved"},[]);Rhino5PopViewMerge.Apply(saved,current,result);
  Check(result.Metadata.Name=="current"&&result.Metadata.ViewType==1,"Page result does not adopt saved name");
  using(var port=Rhino5ViewDefaults.Create(runtime))
  {
   port.TrySetScreenPort(1,2,3,4,7,9);var prior=port.Details.ScreenPort;
   Check(!port.TrySetScreenPort(0,0,0,1,0,0)&&port.Details.ScreenPort==prior,"Equal horizontal bounds reject without mutation");
   Check(!port.TrySetScreenPort(0,1,2,2,0,0)&&port.Details.ScreenPort==prior,"Equal vertical bounds reject without mutation");
   Check(port.TrySetScreenPort(9,-2,7,-3,0,0)&&port.Details.ScreenPort==new Rhino5ScreenPort(9,-2,7,-3,7,9,true),"Reversed bounds accepted; both zero depth values retain prior near/far");
   Check(port.TrySetScreenPort(0,1,0,1,0,-5)&&port.Details.ScreenPort.Near==0&&port.Details.ScreenPort.Far== -5,"Either nonzero depth request writes both native depth values");
   var invalid=new CandidatePoint3(double.NaN,1,2);port.SetConvertedValues(port.Viewport,new(8,8,8),port.Details,port.Metadata,[]);
   Check(!port.TrySetTargetPoint(invalid)&&port.TargetMirror==port.Viewport.Target,"Rejected target setter still synchronizes resulting core target mirror");
   port.SetConvertedValues(port.Viewport with {Target=new(-0d,0,0)},new(0,0,0),port.Details,port.Metadata,[]);int errors=runtime.DiagnosticCount;var read=port.ReadTargetPoint();
   Check(BitConverter.DoubleToInt64Bits(read.X)<0&&BitConverter.DoubleToInt64Bits(port.TargetMirror.X)==0&&runtime.DiagnosticCount==errors,"Equal signed zeros do not repair mirror but getter returns raw viewport target");
   port.SetConvertedValues(port.Viewport with {Target=invalid},invalid,port.Details,port.Metadata,[]);port.ReadTargetPoint();port.ReadTargetPoint();Check(runtime.DiagnosticCount==errors+2,"NaN mirror comparison diagnoses on every native getter call");
  }
  using(var countedCurrent=Rhino5ViewDefaults.Create(runtime))using(var countedSaved=Rhino5ViewDefaults.Create(runtime))using(var countedResult=Rhino5ViewDefaults.Create(runtime))
  {
   var source=new Observer(runtime);countedCurrent.AttachUserData(source);
   Rhino5PopViewMerge.Apply(countedSaved,countedCurrent,countedResult);
   Check(source.Copies==2&&source.Clones.Count==2&&source.Clones.All(c=>c.IsDisposed)&&countedResult.Head is null,"Distinct merge performs two current clones/disposals before replacing with saved viewport data");
  }
  using(var sourceA=Captured(runtime,"A",body))using(var sourceB=Captured(runtime,"B",body))using(var mixed=Rhino5ViewDefaults.Create(runtime))
  {
   var a=sourceA.OriginalArchiveCapture;var b=sourceB.OriginalArchiveCapture;Rhino5PopViewMerge.Apply(sourceB,sourceA,mixed);
   var origins=mixed.CopyArchiveOrigins();Check(origins.Length==2&&ReferenceEquals(origins[0],a)&&ReferenceEquals(origins[1],b)&&ReferenceEquals(mixed.OriginalArchiveCapture,a),"Hybrid viewport/current merge retains both immutable archive origins without calling either an updated save image");
   origins[0]=null!;Check(mixed.CopyArchiveOrigins()[0] is not null,"Origin arrays are independently owned");
   using var copy=mixed.Duplicate();using var assigned=Rhino5ViewDefaults.Create(runtime);assigned.AssignFrom(mixed);sourceA.Dispose();sourceB.Dispose();
   Check(copy.CopyArchiveOrigins().Length==2&&assigned.CopyArchiveOrigins().Length==2&&copy.CopyArchiveOrigins()[1].GetUserData(0).CopyPayload().SequenceEqual(body),"Mixed source bytes survive duplication/assignment and source disposal");
   mixed.Dispose();Check(mixed.CopyArchiveOrigins().Length==0&&copy.CopyArchiveOrigins().Length==2,"Disposal releases only the object's provenance references");
  }
  return "PASS original-derived Pop merge:"+checks+" actual-view/userdata/alias/target/name/port/provenance checks; full Pop/SetView/document/original-PC/physical parity separate.";
 }
 private static Guid Id(int n)=>new(n,0,0,0,0,0,0,0,0,0,0);
 private static Rhino5RuntimeView Captured(Rhino5UserDataRuntime runtime,string name,byte[] body)
 {
  using var defaults=Rhino5ViewDefaults.Create(runtime);var record=new Rhino5ViewUserData(Rhino5UserStringList.ClassId,Rhino5UserStringList.ClassId,Rhino5UserStringList.OpenNurbs4Id,1,Rhino5Matrix4.Identity,body);
  var capture=new Rhino5ViewData(defaults.Viewport,defaults.TargetMirror,defaults.Details,defaults.Metadata with {Name=name},[],new[]{record},new[]{new Rhino5PreservedViewChunk(123,0,new byte[]{1,2,3})});
  var view=Rhino5RuntimeView.FromCapturedUnknownBodies(runtime,capture,new(50,201508103));view.GetUserData(Rhino5UserStringList.ClassId);return view;
 }
 private sealed class Observer:Rhino5RuntimeUserData
 {
  public int Copies;public List<Rhino5UserDataOwner> Clones=new();
  public override Guid RuntimeClassId=>Id(900);
  public Observer(Rhino5UserDataRuntime runtime):base(runtime){UserDataId=RuntimeClassId;CopyCount=1;}
  private Observer(Observer source):base(source){}
  public override Rhino5UserDataOwner Duplicate(){Copies++;var clone=new Observer(this);Clones.Add(clone);return clone;}
 }
}
