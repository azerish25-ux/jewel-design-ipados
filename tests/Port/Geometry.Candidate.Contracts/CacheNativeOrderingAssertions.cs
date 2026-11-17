using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class CacheNativeOrderingAssertions
{
 private sealed class Resource(int id):IDisposable{public int Id=>id;public bool Disposed;public void Dispose()=>Disposed=true;}
 public static string Run()
 {
  int checks=0;void Check(bool b,string m){checks++;if(!b)throw new Exception(m);}
  var crt=Rhino5Crt100FilenameProfile.Get(Rhino5Crt100Locale.C);var names=Rhino5AnsiNlsProfile.Reference60502.CreateFilenameSemantics(crt);
  using var cache=new Rhino5ContextImageCache<Resource>(names,8);string first=char.ConvertFromUtf32(0x10c000)+".PNG",second=char.ConvertFromUtf32(0x10c001)+".PNG";
  using var a=cache.Lookup(1,first,false,()=>new(1))!;using var b=cache.Lookup(1,second,false,()=>new(2))!;
  Check(cache.Count==2&&cache.Snapshot().All(e=>e.Key.Filename=="\ufffd.png"),"Native alias-key setup failed");
  using var alias=cache.Lookup(1,"\ufffd.png",false,()=>throw new Exception("Stored-key hit created image"))!;
  Check(alias.Resource.Id==2,"Native CRT width8 equal-key order must select second image at source midpoint; actual image="+alias.Resource.Id);
  Check(ReferenceEquals(alias.Resource,b.Resource)&&!ReferenceEquals(alias.Resource,a.Resource),"Native selected identity lost");
  Check(cache.Invalidate("\ufffd.png")==2&&!a.Resource.Disposed&&!b.Resource.Disposed,"Equal-key invalidation lost lease ownership");var ra=a.Resource;var rb=b.Resource;a.Dispose();b.Dispose();Check(ra.Disposed&&!rb.Disposed,"Selected alias lease retired too early or other image leaked");alias.Dispose();Check(rb.Disposed,"Selected image not retired after last alias lease");
  bool fail=false;var controlled=new Rhino5FilenameSemantics("sort-failure-control",names.Lower,(x,y)=>{if(fail&&x=="a")throw new InvalidDataException("actual sort comparison failed");return names.Compare(x,y);});using var atomic=new Rhino5ContextImageCache<Resource>(controlled,8);using var retained=atomic.Lookup(2,"b",false,()=>new(3))!;Resource? rejected=null;fail=true;try{atomic.Lookup(2,"a",false,()=>rejected=new(4));throw new Exception("Expected sort failure");}catch(InvalidDataException){checks++;}Check(rejected is {Disposed:true}&&atomic.Count==1&&!retained.Resource.Disposed,"Failed native-order insertion changed old list/ownership");fail=false;using var hit=atomic.Lookup(2,"B",false,()=>throw new Exception("Existing image lost"))!;Check(ReferenceEquals(hit.Resource,retained.Resource),"Rollback changed retained identity");
  return $"PASS native cache ordering:{checks} actual alias-selection/lease/atomic-failure checks; source entry-copy/document/PC/physical remain open.";
 }
}
