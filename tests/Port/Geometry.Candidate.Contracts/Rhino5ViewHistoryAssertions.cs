using JewelDesign.Documents;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Views.Candidate;
namespace CandidateGeometryContractChecks;
public static class Rhino5ViewHistoryAssertions
{
 public static string Run()
 {
  int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
  void Reject<T>(Action action,string why)where T:Exception{bool rejected=false;try{action();}catch(T){rejected=true;}Check(rejected,why);}
  var runtime=new Rhino5UserDataRuntime();var binding=Rhino5UserStringArchiveBinding.Register(runtime);
  using var current=Rhino5ViewDefaults.Create(runtime);var v=current.Viewport;var m=current.Metadata;var d=current.Details;
  Check(v.Frame.Location==new JewelDesign.Geometry.Candidate.CandidatePoint3(0,0,100)&&v.Frame.Direction.Z== -1&&BitConverter.DoubleToInt64Bits(v.Frame.Direction.X)==long.MinValue&&BitConverter.DoubleToInt64Bits(v.Frame.Direction.Y)==long.MinValue,"Original initialize location and signed-zero negative-Z request");
  Check(v.Frame.CameraValid&&!v.Frame.LocationLocked&&!v.Frame.DirectionLocked&&!v.Frame.UpLocked&&v.FrustumValid&&v.Projection==1&&v.SymmetryFlags==0,"Original validity/projection/lock/symmetry defaults");
  Check(v.Bounds==new CandidateFrustumBounds(-20,20,-20,20,.0001,1000),"Original frustum constants");
  Check(BitConverter.DoubleToInt64Bits(v.Target.X)==unchecked((long)0xffe5f8bd9e331dd2)&&v.Target==current.TargetMirror,"Original unset target and independent mirror initializer");
  Check(d.ViewportId==Guid.Empty&&d.ScreenPort==new Rhino5ScreenPort(0,1000,0,1000,0,1,false)&&d.ClipModification.IsIdentity()&&d.InverseClipModification.IsIdentity()&&d.PerspectiveMinNearDist==.0001&&d.PerspectiveMinNearOverFar==.0001&&d.OpaqueCopyByte==0,"Original screen/UUID/matrix/near/opaque defaults");
  Check(!m.ProjectionLocked&&m.Name==""&&m.DisplayModeId==Guid.Empty&&m.LegacyV3DisplayMode==1&&m.ViewType==0,"Original view identity/mode defaults");
  Check(m.Position==new Rhino5ViewPosition(0,1,0,1,0,0,0,0,0)&&m.PageSettings==new Rhino5PageSettings(0,0,0,0,0,0,0,""),"Original position/page/printer defaults");
  Check(m.ConstructionPlane.GridSpacing==1&&m.ConstructionPlane.SnapSpacing==1&&m.ConstructionPlane.GridLineCount==70&&m.ConstructionPlane.ThickLineFrequency==5&&m.ConstructionPlane.DepthBuffered&&m.ConstructionPlane.Name=="","Original construction plane grid defaults");
  Check(m.ShowConstructionGrid&&m.ShowConstructionAxes&&m.ShowWorldAxes&&m.TraceImage.Width==0&&m.TraceImage.Height==0&&m.TraceImage.Filename==""&&m.TraceImage.GrayScale&&!m.TraceImage.Hidden&&!m.TraceImage.Filtered&&m.Wallpaper==new Rhino5WallpaperImage("",true,false),"Original image/visibility defaults without synthesized files");
  Check(m.TraceImage.Plane==m.ConstructionPlane.Plane&&m.TraceImage.Plane.Equation==new Rhino5PlaneEquation(0,0,1,-0d)&&BitConverter.DoubleToInt64Bits(m.TraceImage.Plane.Equation.D)==long.MinValue,"Runtime-initialized XY plane keeps negative-zero equation constant");
  Check(current.Head is null&&current.ClippingPlaneCount==0&&current.OriginalArchiveCapture is null,"Fresh defaults invent no runtime/archive data");
  var body=LegacyUserStringListCodec.Write(new[]{new KeyValuePair<string,string>("key","value")},50);
  var sourceData=new Rhino5UserStringList(runtime);binding.ReadInto(sourceData,body,50);current.AttachUserData(sourceData);
  void Edit(string name,byte raw=0xa5)=>current.SetConvertedValues(current.Viewport,current.TargetMirror,current.Details with {OpaqueCopyByte=raw},current.Metadata with {Name=name},current.CopyClippingPlanes());
  uint Copies(Rhino5RuntimeView view)=>view.GetUserData(Rhino5UserStringList.ClassId)!.CopyCount;
  using(var a=new Rhino5RuntimeViewArray(runtime))
  {
   Edit("A");a.Append(current);Check(a.Count==1&&a.Capacity==4&&a[0].Metadata.Name=="A"&&a[0].Details.OpaqueCopyByte==0&&Copies(a[0])==2,"Append assigns into real default slot, retaining default opaque byte and copying once");
   Check(binding.WriteForArchive((Rhino5UserStringList)a[0].Head!,50).SequenceEqual(body),"History array holds actual codec data/provenance");
   var first=a[0];a.Reserve(8);Check(ReferenceEquals(first,a[0])&&Copies(a[0])==2&&a.Capacity==8,"Reserve/growth does not clone existing data");
   Edit("B");a.Append(current);Edit("C");a.Append(current);var middle=a[1];var last=a[2];
   a.RemoveAt(0);Check(first.IsDisposed&&ReferenceEquals(a[0],middle)&&ReferenceEquals(a[1],last)&&Copies(a[0])==2&&Copies(a[1])==2,"Remove destroys selected element and relocates remaining logical views without assignment/copy increments");
   a.RemoveLast();Check(last.IsDisposed&&a.Count==1&&a.Capacity==8,"Last removal destroys active data without shrinking capacity");
   Edit("D",0xff);a.Append(current);Check(a[1].Details.OpaqueCopyByte==0&&Copies(a[1])==2,"Removed tail reconstructed as a true default before later append");
   var reused=a[1];a.Empty();Check(middle.IsDisposed&&reused.IsDisposed&&a.Count==0&&a.Capacity==8,"Empty destroys/reconstructs active elements and retains capacity");
   var fresh=a.AppendNew();Check(fresh.Head is null&&fresh.Metadata==m&&fresh.Details.OpaqueCopyByte==0&&a.Count==1,"AppendNew uses fresh native defaults");
   a.SetCapacity(0);Check(fresh.IsDisposed&&a.Count==0&&a.Capacity==0,"Zero capacity destroys all reserved storage");
  }
  using(var self=new Rhino5RuntimeViewArray(runtime))
  {
   for(int i=0;i<4;i++){Edit(i.ToString());self.Append(current);}var original=self[0];self.Append(original);
   Check(self.Count==5&&self.Capacity==8&&Copies(self[4])==4&&Copies(original)==2,"Full-capacity alias path performs native default-temporary assignment and final assignment (two copies)");
   var tail=self[4];self.SetCapacity(2);Check(tail.IsDisposed&&self.Count==2&&self.Capacity==2&&Copies(self[0])==2,"Shrink destroys tail storage without cloning retained entries");
   self.RemoveAt(-1);self.RemoveAt(99);Check(self.Count==2,"Invalid remove indices are native no-ops");
  }
  foreach(var expected in new[]{(0,4),(1,4),(2,4),(3,6),(8,16),(239674,479348),(239675,479350),(239682,479364),(239683,479365)})
   Check(Rhino5RuntimeViewArray.NativeNextCapacity(expected.Item1)==expected.Item2,"Original1120-byte growth threshold and capped increment");
  using(var bounded=new Rhino5RuntimeViewArray(runtime,4))
  {
   for(int i=0;i<4;i++)bounded.Append(current);var before=bounded[0];
   Reject<InvalidDataException>(()=>bounded.Append(current),"Managed capacity budget is explicit");Check(bounded.Count==4&&ReferenceEquals(before,bounded[0])&&!before.IsDisposed,"Budget rejection leaves stored data intact");
  }
  using(var entries=new Rhino5RuntimeViewArray(runtime))using(var history=new Rhino5ViewProjectionHistory(entries,-1))
  {
   Edit("same");history.Push(current);history.Push(current);Check(history.Count==2&&history.Cursor==1&&history.Capacity==4&&history[0].Metadata.Name=="same"&&history[1].Metadata.Name=="same","Push appends owned values without duplicate suppression");
   Check(Copies(history[0])==2&&Copies(history[1])==2&&history[1].Details.OpaqueCopyByte==0,"History uses assignment/default slot semantics, not raw view Duplicate");
  }
  using(var entries=new Rhino5RuntimeViewArray(runtime))
  {
   for(int i=0;i<5;i++){Edit("old"+i);entries.Append(current);}var retained=entries[0];var replaced=entries[2];var pruned=entries[4];var oldData=replaced.Head!;
   replaced.SetConvertedValues(replaced.Viewport,replaced.TargetMirror,replaced.Details with {OpaqueCopyByte=0x5a},replaced.Metadata,[]);
   using var branch=new Rhino5ViewProjectionHistory(entries,1);Edit("branch");branch.Push(current);
   Check(branch.Count==3&&branch.Cursor==2&&ReferenceEquals(branch[0],retained)&&ReferenceEquals(branch[2],replaced)&&branch[2].Metadata.Name=="branch","Navigation branch overwrites next entry and prunes later entries");
   Check(oldData.IsDisposed&&pruned.IsDisposed&&branch[2].Details.OpaqueCopyByte==0x5a&&Copies(branch[2])==2,"Branch assignment purges old data and retains existing slot opaque byte");
  }
  using(var entries=new Rhino5RuntimeViewArray(runtime))using(var history=new Rhino5ViewProjectionHistory(entries,-1))
  {
   for(int i=0;i<270;i++){Edit(i.ToString());history.Push(current);Check(history.Count==Math.Min(i+1,255)&&history.Cursor==history.Count-1,"History cap/index after each actual push");}
   Check(history.Capacity==256&&history[0].Metadata.Name=="15"&&history[254].Metadata.Name=="269"&&Copies(history[0])==2&&Copies(history[254])==2,"255-entry sliding history preserves values and no-copy relocation counts");
   Check(ReferenceEquals(history[0].Head!.Owner,history[0]),"Managed relocation preserves safe logical ownership, without claiming native pointer-address identity");
  }
  using(var entries=new Rhino5RuntimeViewArray(runtime))
  {
   for(int i=0;i<300;i++)entries.Append(current);using var imported=new Rhino5ViewProjectionHistory(entries,299);imported.Push(current);
   Check(imported.Count==300&&imported.Cursor==299,"Oversized imported state removes oldest once, not an invented trim-to255 loop");
  }
  using(var entries=new Rhino5RuntimeViewArray(runtime))using(var stale=new Rhino5ViewProjectionHistory(entries,999))
  {stale.Push(current);stale.Push(current);Check(stale.Count==2&&stale.Cursor==1,"At/end or stale-high cursor appends and resets index");}
  using(var entries=new Rhino5RuntimeViewArray(runtime))
  {entries.Append(current);using var invalid=new Rhino5ViewProjectionHistory(entries,-2);Reject<ArgumentOutOfRangeException>(()=>invalid.Push(current),"Managed invalid-negative cursor rejects unsafe native memory access");Check(invalid.Count==1,"Unsafe-cursor rejection does not mutate data");}
  using(var observedCurrent=Rhino5ViewDefaults.Create(runtime))using(var entries=new Rhino5RuntimeViewArray(runtime))using(var history=new Rhino5ViewProjectionHistory(entries,-1))
  {
   observedCurrent.SetConvertedValues(observedCurrent.Viewport,observedCurrent.TargetMirror,observedCurrent.Details with {OpaqueCopyByte=0x99},observedCurrent.Metadata with {Name="source"},[]);
   var observer=new Observer(runtime){OnCopy=()=>{Check(history.Count==1&&history.Cursor== -1&&history[0].Metadata==m,"Append increments count before copying into genuine default fields, before final cursor update");var slot=history[0];slot.SetConvertedValues(slot.Viewport,slot.TargetMirror,slot.Details with {OpaqueCopyByte=0x33},slot.Metadata,[]);}};
   observedCurrent.AttachUserData(observer);history.Push(observedCurrent);
   Check(history[0].Details.OpaqueCopyByte==0x33&&history[0].Metadata.Name=="source"&&history.Cursor==0,"Copy callback opaque mutation survives assignment while other source fields and cursor complete");
  }
  using(var throwingCurrent=Rhino5ViewDefaults.Create(runtime))using(var entries=new Rhino5RuntimeViewArray(runtime))using(var history=new Rhino5ViewProjectionHistory(entries,-1))
  {
   throwingCurrent.AttachUserData(new Observer(runtime){Throw=true});
   Reject<InvalidOperationException>(()=>history.Push(throwingCurrent),"Managed copy failure propagates without invented push rollback");
   Check(history.Count==1&&history.Cursor== -1&&history[0].Metadata==m,"Failed append retains incremented count/default values and old cursor in source order");
  }
  Check(sourceData.CopyCount==1&&!sourceData.IsDisposed,"Stored history lifetime never consumes or increments caller's original data");
  return "PASS original-derived history push:"+checks+" native-default/array-lifetime/actual-codec/copy-count cases; navigation/Pop/document/original-pointer/full3dm/PC/physical parity separate.";
 }
 private sealed class Observer:Rhino5RuntimeUserData
 {
  public Action? OnCopy;public bool Throw;
  public override Guid RuntimeClassId=>Guid.Parse("00000001-0000-0000-0000-000000000001");
  public Observer(Rhino5UserDataRuntime runtime):base(runtime){UserDataId=RuntimeClassId;CopyCount=1;}
  private Observer(Observer source):base(source){}
  public override Rhino5UserDataOwner Duplicate(){OnCopy?.Invoke();if(Throw)throw new InvalidOperationException("history copy observer");return new Observer(this);}
 }
}
