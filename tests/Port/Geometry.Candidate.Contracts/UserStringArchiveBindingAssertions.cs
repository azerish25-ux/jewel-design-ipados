using System.Buffers.Binary;
using JewelDesign.Documents;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Views.Candidate;
namespace CandidateGeometryContractChecks;
public static class UserStringArchiveBindingAssertions
{
 public static string Run()
 {
  int checks=0;void Check(bool ok,string message){checks++;if(!ok)throw new Exception(message);}
  var runtime=new Rhino5UserDataRuntime();var binding=Rhino5UserStringArchiveBinding.Register(runtime);
  var pairs=new[]{new KeyValuePair<string,string>("key","value"),new("key","値 💎")};var body=LegacyUserStringListCodec.Write(pairs,50);
  using var owner=new Rhino5UserDataOwner(runtime);var missing=new Rhino5UnknownUserData(runtime,Guid.Parse("00000001-0000-0000-0000-000000000001"),Guid.Empty,7,Rhino5Matrix4.Identity,Rhino5UserStringList.ClassId,body,body.Length,50,201508103);
  Check(owner.AttachUserData(missing),"Real body attached as unknown");var converted=owner.GetUserData(missing.UserDataId) as Rhino5UserStringList;
  Check(converted is not null&&converted.CopyPairs().SequenceEqual(pairs)&&converted.CopyCount==7&&missing.IsDisposed,"Actual registered native body codec materializes string-list values");
  Check(binding.TryGetReadResult(converted!,out var provenance)&&provenance!.NativeReadSucceeded&&provenance.CopyOriginal().SequenceEqual(body),"Owned original body retained for format provenance/no-op policy");
  Check(binding.WriteCanonical(converted!,50).SequenceEqual(body),"Actual registered body write reproduces canonical native bytes");
  var existing=new Rhino5UserStringList(runtime);existing.ReplacePairs(new[]{new KeyValuePair<string,string>("keep","state")});var result=binding.ReadInto(existing,body,50);
  Check(result.NativeReadSucceeded&&existing.CopyPairs().Length==3&&existing.CopyPairs()[0].Key=="keep","Native Read appends rather than replacing existing pairs");
  var bad=(byte[])body.Clone();bad[^1]^=1;var checksum=new Rhino5UserStringList(runtime);result=binding.ReadInto(checksum,bad,50);
  Check(result.NativeReadSucceeded&&result.CrcErrorCount==1&&runtime.ArchiveCrcErrorCount==1&&checksum.CopyPairs().SequenceEqual(pairs),"Native bool remains true while CRC diagnostics are surfaced");
  int first=12+12;int second=checked(first+12+(int)BinaryPrimitives.ReadInt64LittleEndian(body.AsSpan(first+4)));var partial=(byte[])body.Clone();BinaryPrimitives.WriteInt32LittleEndian(partial.AsSpan(second+12),2);
  var partialUnknown=new Rhino5UnknownUserData(runtime,Guid.Parse("00000002-0000-0000-0000-000000000002"),Guid.Empty,8,Rhino5Matrix4.Identity,Rhino5UserStringList.ClassId,partial,partial.Length,50,201508103);using var partialOwner=new Rhino5UserDataOwner(runtime);partialOwner.AttachUserData(partialUnknown);var partialConverted=partialOwner.GetUserData(partialUnknown.UserDataId) as Rhino5UserStringList;
  Check(partialConverted is not null&&partialConverted.CopyPairs().Length==1&&partialUnknown.IsDisposed,"Ignored reader false retains original native partial conversion behavior");
  Check(binding.TryGetReadResult(partialConverted!,out var partialResult)&&!partialResult!.NativeReadSucceeded&&partialResult.CopyOriginal().SequenceEqual(partial),"Partial failure remains explicit with original bytes preserved");
  var boundedRuntime=new Rhino5UserDataRuntime();Rhino5UserStringArchiveBinding.Register(boundedRuntime,new(MaxPairs:0));using var boundedOwner=new Rhino5UserDataOwner(boundedRuntime);var bounded=new Rhino5UnknownUserData(boundedRuntime,Guid.Parse("00000003-0000-0000-0000-000000000003"),Guid.Empty,1,Rhino5Matrix4.Identity,Rhino5UserStringList.ClassId,body,body.Length,50,201508103);boundedOwner.AttachUserData(bounded);bool rejected=false;try{boundedOwner.GetUserData(bounded.UserDataId);}catch(InvalidDataException){rejected=true;}Check(rejected&&!bounded.IsDisposed&&ReferenceEquals(boundedOwner.Head,bounded),"Explicit managed capability limit preserves unsupported opaque source instead of silently converting partial data");
  return "PASS actual user-string archive binding:"+checks+" decode/append/write/CRC/partial/limit checks; one native class codec, complete catalog/history/PC parity separate.";
 }
}
