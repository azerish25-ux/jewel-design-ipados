using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class CacheEntryNameAssertions
{
 private sealed class Resource:IDisposable{public bool Disposed;public void Dispose()=>Disposed=true;}
 public static string Run()
 {
  int checks=0,cases=0;void Check(bool b,string m){checks++;if(!b)throw new Exception(m);}
  (string Input,string Prefix)[] inputs=[("", ""),("ABC","ABC"),("ABC\0DEF","ABC"),("\0ABC",""),("\0",""),("A\0\ud800B","A"),("A\ud800B\0C","A\ud800B"),("\ud800A\0B","\ud800A"),("\udc00A\0B","\udc00A"),("\ufeffA\0B","\ufeffA"),("Ì\0A","Ì"),("Ý\0B","Ý"),("\ud801\udc00\0A","\ud801\udc00"),("\udbf0\udc00\0A","\udbf0\udc00"),("猫.PNG","猫.PNG"),("A\0B\0C","A")];
  foreach(var crtLocale in new[]{Rhino5Crt100Locale.C,Rhino5Crt100Locale.EnglishUnitedStates1252,Rhino5Crt100Locale.Turkish1254})
  {
   var runtime=new Rhino5StringRuntimeProfile(Rhino5Crt100FilenameProfile.Get(crtLocale));
   foreach(var mode in new[]{Rhino5StringRuntimeMode.Crt,Rhino5StringRuntimeMode.UnicodeNls,Rhino5StringRuntimeMode.AnsiNls})
   {
    runtime.SetWindowsLocaleId(mode==Rhino5StringRuntimeMode.Crt?0u:1033u,mode==Rhino5StringRuntimeMode.UnicodeNls?0u:1u);
    foreach(var (input,prefix) in inputs)
    {
     var loweredInputs=new List<string>();var policy=new Rhino5FilenameSemantics("entry-copy-control",name=>{loweredInputs.Add(name);return runtime.Lower(name);},runtime.FilenameSemantics.Compare);
     using var cache=new Rhino5ContextImageCache<Resource>(policy,4);int made=0;using var lease=cache.Lookup(1,input,false,()=>{made++;return new();})!;
     Check(cache.Snapshot().Single().Key.Filename==runtime.Lower(prefix),"Entry stored data after the source C-prefix boundary: "+mode+"/"+crtLocale+"/"+cases);
     Check(loweredInputs.Count==1&&loweredInputs[0]==prefix,"Entry lower saw stored suffix instead of the copied wchar_t C prefix");Check(made==1&&cache.Count==1&&!lease.Resource.Disposed,"Entry copy changed factory/ownership");cases++;
    }
   }
  }
  var c=Rhino5Crt100FilenameProfile.Get(Rhino5Crt100Locale.C);var unicode=new Rhino5StringRuntimeProfile(c);unicode.SetWindowsLocaleId(1033,0);
  Check(unicode.Lower("ABC\0DEF")=="abc\0def","Standalone stored-string lower was globally truncated");
  string? observedQuery=null;var live=new Rhino5FilenameSemantics("query-preservation",value=>unicode.Lower(value),(left,right)=>{observedQuery=right;return unicode.FilenameSemantics.Compare(left,right);});
  using var retained=new Rhino5ContextImageCache<Resource>(live,4);using var first=retained.Lookup(7,"ABC\0DEF",false,()=>new())!;using var alias=retained.Lookup(7,"ABC\0OTHER",false,()=>throw new Exception("C-prefix query alias missed"))!;
  Check(observedQuery=="ABC\0OTHER"&&ReferenceEquals(first.Resource,alias.Resource),"Lookup query was pre-truncated or identity changed");Check(retained.Snapshot().Single().Key.Filename=="abc","Unicode entry prefix not retained exactly");
  using var leading=new Rhino5ContextImageCache<Resource>(live,4);using var empty=leading.Lookup(8,"\0TAIL",false,()=>new())!;using var emptyHit=leading.Lookup(8,"",false,()=>throw new Exception("Stored empty prefix not found"))!;Check(leading.Snapshot().Single().Key.Filename==""&&ReferenceEquals(empty.Resource,emptyHit.Resource),"Leading-NUL stored-length semantics changed");
  Resource? rejected=null;bool reject=false;var failingLower=new Rhino5FilenameSemantics("copy-lower-failure",name=>reject&&name=="A"?throw new InvalidDataException("copied prefix lower rejected"):unicode.Lower(name),unicode.FilenameSemantics.Compare);using var atomic=new Rhino5ContextImageCache<Resource>(failingLower,4);using var old=atomic.Lookup(9,"b",false,()=>new())!;reject=true;try{atomic.Lookup(9,"A\0B",false,()=>rejected=new());throw new Exception("Expected copied-prefix lower failure");}catch(InvalidDataException){checks++;}Check(rejected is {Disposed:true}&&atomic.Count==1&&!old.Resource.Disposed&&atomic.Snapshot().Single().Key.Filename=="b","Failed entry construction changed previous cache/ownership");
  int lowers=0;var counting=new Rhino5FilenameSemantics("factory-order",name=>{lowers++;return name;},Rhino5FilenameSemantics.CrtCLocale.Compare);using var factory=new Rhino5ContextImageCache<Resource>(counting,4);Check(factory.Lookup(1,"A\0B",false,()=>null) is null&&lowers==0,"Null factory invoked entry copy/lower");try{factory.Lookup(1,"A\0B",false,()=>throw new InvalidDataException());throw new Exception("Expected factory error");}catch(InvalidDataException){checks++;}Check(lowers==0&&factory.Count==0,"Factory failure constructed entry metadata");
  var one=first.Resource;var two=empty.Resource;retained.Dispose();leading.Dispose();Check(!one.Disposed&&!two.Disposed,"Cache disposal killed copied-name leases");first.Dispose();alias.Dispose();empty.Dispose();emptyHit.Dispose();Check(one.Disposed&&two.Disposed,"Copied-name resources did not retire");
  Check(cases==144,"Entry-name source case coverage changed");
  return $"PASS source entry-name copy:{cases} mode/CRT/string cases/{checks} C-prefix/query/ownership/failure checks; factory path resolution/full document/PC/physical remain open.";
 }
}
