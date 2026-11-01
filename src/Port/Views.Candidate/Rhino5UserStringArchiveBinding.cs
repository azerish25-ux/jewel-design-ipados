using System.Runtime.CompilerServices;
using JewelDesign.Documents;
using JewelDesign.Rendering.Candidate;
namespace JewelDesign.Views.Candidate;

/// <summary>Actual native string-list body binding; this registers one codec, not the complete original class catalog.</summary>
public sealed class Rhino5UserStringArchiveBinding
{
 private readonly Rhino5UserDataRuntime runtime;
 private readonly LegacyUserStringListLimits limits;
 private readonly ConditionalWeakTable<Rhino5UserStringList,LegacyUserStringListReadResult> reads=new();
 private Rhino5UserStringArchiveBinding(Rhino5UserDataRuntime runtime,LegacyUserStringListLimits? limits)
 {this.runtime=runtime;this.limits=limits??new();}
 public static Rhino5UserStringArchiveBinding Register(Rhino5UserDataRuntime runtime,LegacyUserStringListLimits? limits=null)
 {
  ArgumentNullException.ThrowIfNull(runtime);var binding=new Rhino5UserStringArchiveBinding(runtime,limits);
  runtime.RegisterClass(Rhino5UserStringList.ClassId,r=>new Rhino5UserStringList(r),(data,archive)=>binding.ReadInto((Rhino5UserStringList)data,archive).NativeReadSucceeded);return binding;
 }
 public LegacyUserStringListReadResult ReadInto(Rhino5UserStringList target,Rhino5UserDataArchive archive)
 {
  ArgumentNullException.ThrowIfNull(target);ArgumentNullException.ThrowIfNull(archive);
  return ReadInto(target,archive.CopyBytes(),archive.ArchiveVersion);
 }
 public LegacyUserStringListReadResult ReadInto(Rhino5UserStringList target,ReadOnlySpan<byte> body,int archiveVersion)
 {
  ArgumentNullException.ThrowIfNull(target);var result=LegacyUserStringListCodec.Read(body,archiveVersion,limits);
  if(result.ManagedLimitHit)throw new InvalidDataException("Native user-string conversion exceeded an explicit managed bound; retain original opaque data.");
  var original=target.CopyPairs();var appended=result.CopyPairs();
  if((long)original.Length+appended.Length>limits.MaxPairs)throw new InvalidDataException("Appended user-string list exceeds managed bound");
  var combined=new KeyValuePair<string,string>[original.Length+appended.Length];original.CopyTo(combined,0);appended.CopyTo(combined,original.Length);target.ReplacePairs(combined);
  runtime.ReportArchiveCrcErrors(result.CrcErrorCount);reads.Remove(target);reads.Add(target,result);return result;
 }
 public bool TryGetReadResult(Rhino5UserStringList target,out LegacyUserStringListReadResult? result)=>reads.TryGetValue(target,out result);
 public byte[] WriteCanonical(Rhino5UserStringList target,int archiveVersion)=>LegacyUserStringListCodec.Write(target.CopyPairs(),archiveVersion,limits);
}
