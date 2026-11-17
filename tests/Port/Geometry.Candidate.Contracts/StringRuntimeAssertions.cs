using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class StringRuntimeAssertions
{
 private sealed class Resource:IDisposable {public bool Disposed;public void Dispose()=>Disposed=true;}
 public static string Run()
 {
  int checks=0,transitions=0;
  void Check(bool value,string message){checks++;if(!value)throw new Exception(message);}
  var c=Rhino5Crt100FilenameProfile.Get(Rhino5Crt100Locale.C);var english=Rhino5Crt100FilenameProfile.Get(Rhino5Crt100Locale.EnglishUnitedStates1252);var turkish=Rhino5Crt100FilenameProfile.Get(Rhino5Crt100Locale.Turkish1254);
  var runtime=new Rhino5StringRuntimeProfile(c);var liveNames=runtime.FilenameSemantics;
  uint Set(uint requested,uint ansi){transitions++;return runtime.SetWindowsLocaleId(requested,ansi);}
  Check(runtime.WindowsLocaleId==0&&runtime.State.UnicodeFlag==0&&runtime.Mode==Rhino5StringRuntimeMode.Crt,"Initial zero-filled flags changed");
  Check(runtime.Lower("Ì")=="Ì"&&runtime.CompareRaw("I","i")==0,"Initial explicit C fallback changed");
  foreach(var crt in new[]{c,english,turkish})
  {
   runtime.SetRecordedCrtFallback(crt);uint previous=runtime.WindowsLocaleId;
   foreach(uint request in new uint[]{0,1,1033,1055,uint.MaxValue})foreach(uint ansi in new uint[]{0,1,17,uint.MaxValue})
   {
    uint before=Set(request,ansi);
    Check(before==previous,"Setter returned requested LCID instead of previous0/1 flag");previous=request==0?0u:1u;
    var state=runtime.State;
    Check(runtime.WindowsLocaleId==previous&&state.LocaleFlag==previous&&state.UnicodeFlag==(ansi==0?1u:0u),"Original Boolean coercion/store values changed");
    Check(ReferenceEquals(runtime.RecordedCrtFallback,crt)&&ReferenceEquals(runtime.FilenameSemantics,liveNames),"Core locale setter changed CRT or replaced live policy");
    Rhino5StringRuntimeMode expected=request==0?Rhino5StringRuntimeMode.Crt:ansi==0?Rhino5StringRuntimeMode.UnicodeNls:Rhino5StringRuntimeMode.AnsiNls;
    Check(runtime.Mode==expected,"Runtime mode selection changed");
    string expectedNul=expected==Rhino5StringRuntimeMode.UnicodeNls?"abc\0def":expected==Rhino5StringRuntimeMode.AnsiNls?"abc":"abc\0DEF";
    string expectedAccent=expected==Rhino5StringRuntimeMode.AnsiNls?"Ü":expected==Rhino5StringRuntimeMode.UnicodeNls?"ì":crt==c?"Ì":"ì";
    Check(runtime.Lower("ABC\0DEF")==expectedNul&&liveNames.Lower("Ì")==expectedAccent,"Literal mode-specific lower behavior changed");
    int expectedEmptyAccent=expected==Rhino5StringRuntimeMode.UnicodeNls?-1:crt==english?-253:-221;
    string expectedY=expected==Rhino5StringRuntimeMode.AnsiNls?"Ý":expected==Rhino5StringRuntimeMode.UnicodeNls?"ý":crt==c?"Ý":"ý";
    Check(runtime.CompareRaw("I","i")==0&&runtime.Lower("I")=="i","Recorded native I/i control changed");
    Check(runtime.CompareRaw("","Ý")==expectedEmptyAccent&&runtime.Lower("Ý")==expectedY,"Current CRT mapping/fallback selection changed");
    Check(runtime.CompareRaw("A-B.png","AB.png")== (expected==Rhino5StringRuntimeMode.UnicodeNls?1:-53),"Native word-sort vs raw CRT/ANSI ASCII routing changed");
    Check(runtime.CompareRaw(null,null)==0&&runtime.CompareRaw(null,"")==-1&&runtime.CompareRaw("",null)==1,"Raw pointer guards changed");
    Check(liveNames.Compare(null,"")==0&&liveNames.Compare("\0tail","")==1&&liveNames.Compare("","\0tail")==0,"Stored-length filename guards changed");
    Check(runtime.Lower("")==""&&runtime.CompareRaw("A\0B","a\0C")==0,"Empty/C-string termination changed");
   }
  }
  Set(1033,1);Check(runtime.Mode==Rhino5StringRuntimeMode.AnsiNls,"ANSI setup failed");
  Check(Set(1033,0)==1&&runtime.Mode==Rhino5StringRuntimeMode.UnicodeNls&&runtime.WindowsLocaleId==1,"Same requested language did not reset the core flags");
  Set(0,0);runtime.SetRecordedCrtFallback(c);using var cache=new Rhino5ContextImageCache<Resource>(liveNames);
  using var initial=cache.Lookup(7,"ß.PNG",false,()=>new())!;Check(cache.Count==1&&cache.Snapshot().Single().Key.Filename=="ß.png","Initial real cache insert changed");
  int misses=0;Check(cache.Lookup(7,"ss.png",false,()=>{misses++;return null;}) is null&&misses==1&&cache.Count==1,"Initial CRT alias incorrectly hit");
  Set(1033,0);using var alias=cache.Lookup(7,"ss.png",false,()=>throw new Exception("Unicode transition cleared/froze existing cache"))!;
  Check(ReferenceEquals(initial.Resource,alias.Resource)&&cache.Count==1,"Live profile did not reach existing cache");
  Set(0,1);Check(cache.Lookup(7,"ss.png",false,()=>{misses++;return null;}) is null&&cache.Count==1&&!initial.Resource.Disposed,"Disabling NLS cleared cache or retained Unicode comparer");
  Set(1,0);Check(cache.Invalidate("SS.PNG")==1&&cache.Count==0&&!initial.Resource.Disposed,"Transition invalidation/lease retention changed");var held=initial.Resource;initial.Dispose();alias.Dispose();Check(held.Disposed,"Transition resource did not retire after last lease");
  var independent=new Rhino5StringRuntimeProfile(c);Check(independent.WindowsLocaleId==0&&independent.Mode==Rhino5StringRuntimeMode.Crt,"Explicit owners leaked flags across instances");
  Set(1033,0);Check(ReferenceEquals(runtime.SetRecordedCrtFallback(english),c)&&runtime.CompareRaw("","Ý")==-1&&runtime.WindowsLocaleId==1,"Recorded fallback selection changed active Unicode NLS state");Set(1033,1);Check(runtime.CompareRaw("","Ý")==-253&&runtime.Lower("Ý")=="Ý","Selected current CRT did not affect only ANSI ASCII/failure routes");
  foreach(Action invalid in new Action[]{()=>new Rhino5StringRuntimeProfile(null!),()=>runtime.SetRecordedCrtFallback(null!),()=>runtime.Lower(null!)}){try{invalid();throw new Exception("Null runtime dependency accepted");}catch(ArgumentNullException){checks++;}}
  foreach(Action invalid in new Action[]{()=>runtime.Lower("a",-1),()=>runtime.CompareRaw("a","b",-1)}){try{invalid();throw new Exception("Negative runtime budget accepted");}catch(ArgumentOutOfRangeException){checks++;}}
  foreach(Action invalid in new Action[]{()=>runtime.Lower("AB",1),()=>runtime.CompareRaw("A","BB",1),()=>runtime.CompareRaw("AA","B",1)}){try{invalid();throw new Exception("Runtime budget overflow accepted");}catch(InvalidDataException){checks++;}}
  Check(runtime.WindowsLocaleId==1&&runtime.Mode==Rhino5StringRuntimeMode.AnsiNls&&ReferenceEquals(runtime.RecordedCrtFallback,english),"Failed value operation mutated runtime state");
  Check(transitions==68,"Runtime transition coverage changed");
  return $"PASS mutable string runtime:{transitions} source flag transitions/{checks} literal/mode/cache/guard checks; explicit recorded CRT snapshots, full document/native-array/PC/physical remain open.";
 }
}
