using System.Buffers.Binary;
using JewelDesign.Documents;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Views.Candidate;
namespace CandidateGeometryContractChecks;
public static class UserStringProvenanceAssertions
{
    public static string Run()
    {
        int checks=0;
        void Check(bool ok,string message){checks++;if(!ok)throw new Exception(message);}
        void Reject(Action action,string message){bool rejected=false;try{action();}catch(InvalidDataException){rejected=true;}Check(rejected,message);}
        var runtime=new Rhino5UserDataRuntime();var binding=Rhino5UserStringArchiveBinding.Register(runtime);
        var pairs=new[]{new KeyValuePair<string,string>("key","value"),new("key","値 💎")};
        var edited=new[]{new KeyValuePair<string,string>("key","edited"),pairs[1]};
        foreach(int version in new[]{4,5,50})
        {
            var body=LegacyUserStringListCodec.Write(pairs,version);
            using var source=new Rhino5UserStringList(runtime);
            var read=binding.ReadInto(source,body,version,201508103);
            source.CopyCount=uint.MaxValue;
            using var clone=(Rhino5UserStringList)source.Duplicate();
            Check(binding.TryGetReadResult(clone,out var clonedRead)&&ReferenceEquals(read,clonedRead),"Native-style Duplicate loses actual archive provenance");
            Check(clone.CopyCount==1&&clone.Owner is null&&clone.CopyPairs().SequenceEqual(pairs),"Provenance cannot change original copy-count wrap/owner/value policy");
            Check(binding.WriteForArchive(clone,version).SequenceEqual(body),"Duplicate preserves exact source body");
            var origins=binding.CopyReadOrigins(clone);
            Check(origins.Length==1&&origins[0].ArchiveVersion==version&&origins[0].OpenNurbsVersion==201508103,"Archive context survives copying");
            origins[0]=null!;var exposed=read.CopyOriginal();exposed[0]^=1;
            Check(binding.CopyReadOrigins(clone)[0].Result.CopyOriginal().SequenceEqual(body),"Returned provenance arrays/bytes cannot mutate stored origin");
            clone.ReplacePairs(edited);
            Check(source.CopyPairs().SequenceEqual(pairs),"Copied values are independently editable");
            var written=LegacyUserStringListCodec.Read(binding.WriteForArchive(clone,version),version);
            Check(written.NativeReadSucceeded&&written.CrcErrorCount==0&&written.CopyPairs().SequenceEqual(edited),"Known edited schema regenerates actual canonical body");
            clone.ReplacePairs(pairs);
            Check(binding.WriteForArchive(clone,version).SequenceEqual(body),"Edit/revert restores byte-exact no-op output");
            int other=version==50?4:50;
            Check(binding.WriteForArchive(clone,other).SequenceEqual(LegacyUserStringListCodec.Write(pairs,other)),"Known complete body converts archive-length context through real writer");

            var otherRuntime=new Rhino5UserDataRuntime();var otherBinding=Rhino5UserStringArchiveBinding.Register(otherRuntime);
            using var assigned=new Rhino5UserStringList(otherRuntime);assigned.UserDataId=Guid.Parse("00000001-0000-0000-0000-000000000001");assigned.ApplicationId=Guid.Parse("00000002-0000-0000-0000-000000000002");
            using var owner=new Rhino5UserDataOwner(otherRuntime);owner.AttachUserData(assigned);
            var id=assigned.UserDataId;var app=assigned.ApplicationId;
            assigned.AssignFrom(source);
            Check(otherBinding.TryGetReadResult(assigned,out var assignedRead)&&ReferenceEquals(read,assignedRead),"Assignment across runtimes must retain immutable source provenance");
            Check(assigned.UserDataId==id&&assigned.ApplicationId==app&&ReferenceEquals(assigned.Owner,owner)&&assigned.CopyCount==1,"Original assignment identities/owner/count remain unchanged");
            assigned.AssignFrom(assigned);
            Check(otherBinding.WriteForArchive(assigned,version).SequenceEqual(body),"Self-assignment retains exact origin");
            source.Dispose();
            Check(!binding.TryGetReadResult(source,out _)&&binding.CopyReadOrigins(source).Length==0,"Disposal releases original object's provenance");
            Check(otherBinding.WriteForArchive(assigned,version).SequenceEqual(body),"Copy remains readable after original source disposal");

            int header=version<50?8:12;
            var future=(byte[])body.Clone();BinaryPrimitives.WriteInt32LittleEndian(future.AsSpan(header+4),1);
            using var futureList=new Rhino5UserStringList(runtime);binding.ReadInto(futureList,future,version);
            using var futureClone=(Rhino5UserStringList)futureList.Duplicate();
            Check(binding.WriteForArchive(futureClone,version).SequenceEqual(future),"Unchanged future schema retains every original byte");
            futureClone.ReplacePairs(edited);
            Reject(()=>binding.WriteForArchive(futureClone,version),"Unknown-schema edits cannot silently drop source fields");
            Check(LegacyUserStringListCodec.Read(binding.WriteCanonical(futureClone,version),version).CopyPairs().SequenceEqual(edited),"Explicit canonical rewrite is separate from preservation policy");
            futureClone.ReplacePairs(pairs);
            Check(binding.WriteForArchive(futureClone,version).SequenceEqual(future),"Unknown-schema edit/revert keeps original bytes");
            Reject(()=>binding.WriteForArchive(futureClone,other),"Unknown schema cannot silently change archive context");

            var crc=(byte[])body.Clone();crc[^1]^=1;
            using var crcList=new Rhino5UserStringList(runtime);binding.ReadInto(crcList,crc,version);
            using var crcClone=(Rhino5UserStringList)crcList.Duplicate();
            Check(binding.TryGetReadResult(crcClone,out var crcRead)&&crcRead!.CrcErrorCount==1&&binding.WriteForArchive(crcClone,version).SequenceEqual(crc),"CRC diagnostic and exact original survive duplicate");
            crcClone.ReplacePairs(edited);
            Check(LegacyUserStringListCodec.Read(binding.WriteForArchive(crcClone,version),version).CrcErrorCount==0,"Known edited body receives actual fresh CRCs");

            int first=header+12;long firstLength=version<50?BinaryPrimitives.ReadInt32LittleEndian(body.AsSpan(first+4)):BinaryPrimitives.ReadInt64LittleEndian(body.AsSpan(first+4));
            int second=checked(first+header+(int)firstLength);var partial=(byte[])body.Clone();BinaryPrimitives.WriteInt32LittleEndian(partial.AsSpan(second+header),2);
            using var partialList=new Rhino5UserStringList(runtime);binding.ReadInto(partialList,partial,version);
            using var partialClone=(Rhino5UserStringList)partialList.Duplicate();
            Check(binding.TryGetReadResult(partialClone,out var partialRead)&&!partialRead!.NativeReadSucceeded&&partialClone.CopyPairs().Length==1&&binding.WriteForArchive(partialClone,version).SequenceEqual(partial),"Partial native read retains both prefix values and full original across copy");
            partialClone.ReplacePairs(edited);
            Reject(()=>binding.WriteForArchive(partialClone,version),"Partial data cannot be silently rewritten after an edit");

            var trailing=body.Concat(new byte[]{12,34}).ToArray();using var tailList=new Rhino5UserStringList(runtime);binding.ReadInto(tailList,trailing,version);
            using var tailClone=(Rhino5UserStringList)tailList.Duplicate();
            Check(binding.WriteForArchive(tailClone,version).SequenceEqual(trailing),"Trailing opaque bytes survive unchanged copy/write");
            tailClone.ReplacePairs(edited);Reject(()=>binding.WriteForArchive(tailClone,version),"Trailing opaque bytes cannot silently disappear after an edit");
        }

        var bytes=LegacyUserStringListCodec.Write(pairs,50);
        using var accumulated=new Rhino5UserStringList(runtime);binding.ReadInto(accumulated,bytes,50,123);binding.ReadInto(accumulated,bytes,50,456);
        using var accumulatedClone=(Rhino5UserStringList)accumulated.Duplicate();
        var history=binding.CopyReadOrigins(accumulatedClone);
        Check(history.Length==2&&history[0].OpenNurbsVersion==123&&history[1].OpenNurbsVersion==456&&accumulatedClone.CopyPairs().Length==4,"Every appended read origin survives native-style copying in source order");
        Reject(()=>binding.WriteForArchive(accumulatedClone,50),"Accumulated reads cannot impersonate one source body");
        Check(LegacyUserStringListCodec.Read(binding.WriteCanonical(accumulatedClone,50),50).CopyPairs().Length==4,"Explicit canonical accumulated write includes all values");
        using var mixed=new Rhino5UserStringList(runtime);mixed.ReplacePairs(new[]{new KeyValuePair<string,string>("manual","value")});binding.ReadInto(mixed,bytes,50);
        Reject(()=>binding.WriteForArchive(mixed,50),"Pre-existing values cannot be dropped by returning only last-read bytes");
        using var fresh=new Rhino5UserStringList(runtime);fresh.ReplacePairs(pairs);
        Check(binding.CopyReadOrigins(fresh).Length==0&&binding.WriteForArchive(fresh,50).SequenceEqual(bytes),"New list with no origin uses actual canonical writer");
        accumulatedClone.AssignFrom(fresh);
        Check(!binding.TryGetReadResult(accumulatedClone,out _)&&binding.WriteForArchive(accumulatedClone,50).SequenceEqual(bytes),"Assignment from fresh source clears stale destination provenance");

        using var parent=new Rhino5UserStringList(runtime);binding.ReadInto(parent,bytes,50);
        var child=new Rhino5UserStringList(runtime);binding.ReadInto(child,bytes,50);child.CopyCount=1;parent.AttachUserData(child);
        var skipped=new Rhino5UserStringList(runtime);skipped.UserDataId=Guid.NewGuid();skipped.CopyCount=0;parent.AttachUserData(skipped);
        using var parentClone=(Rhino5UserStringList)parent.Duplicate();
        Check(parentClone.Head is Rhino5UserStringList nested&&nested.Next is null&&nested.CopyCount==2&&ReferenceEquals(nested.Owner,parentClone)&&binding.WriteForArchive(nested,50).SequenceEqual(bytes),"Nested native selective copy carries actual provenance and skips count0");
        parent.Dispose();
        Check(binding.WriteForArchive((Rhino5UserStringList)parentClone.Head!,50).SequenceEqual(bytes),"Nested copy remains readable after original parent purge");

        using var convertedOwner=new Rhino5UserDataOwner(runtime);var unknown=new Rhino5UnknownUserData(runtime,Guid.NewGuid(),Guid.Empty,1,Rhino5Matrix4.Identity,Rhino5UserStringList.ClassId,bytes,bytes.Length,50,201508103);
        convertedOwner.AttachUserData(unknown);var converted=(Rhino5UserStringList)convertedOwner.GetUserData(unknown.UserDataId)!;
        using var convertedClone=(Rhino5UserStringList)converted.Duplicate();
        Check(unknown.IsDisposed&&binding.CopyReadOrigins(convertedClone)[0].OpenNurbsVersion==201508103&&binding.WriteForArchive(convertedClone,50).SequenceEqual(bytes),"Real registered unknown conversion preserves archive context through duplicate");
        var limitedRuntime=new Rhino5UserDataRuntime();var limited=Rhino5UserStringArchiveBinding.Register(limitedRuntime,new(MaxPairs:0));
        Reject(()=>limited.ReadInto(convertedClone,bytes,50),"Managed read limit must reject before changing copied provenance");
        Check(binding.WriteForArchive(convertedClone,50).SequenceEqual(bytes)&&binding.CopyReadOrigins(convertedClone).Length==1,"Failed bounded read retains prior values and origin");
        using var byteBoundList=new Rhino5UserStringList(runtime);
        var byteBound=Rhino5UserStringArchiveBinding.Register(new Rhino5UserDataRuntime(),new(MaxPayloadBytes:32));
        var emptyBody=LegacyUserStringListCodec.Write([],50);byteBound.ReadInto(byteBoundList,emptyBody,50);
        Reject(()=>byteBound.ReadInto(byteBoundList,emptyBody,50),"Cumulative retained source bytes obey the managed archive budget");
        Check(byteBound.CopyReadOrigins(byteBoundList).Length==1,"Origin budget failure leaves prior origin unchanged");
        using var emptyReads=new Rhino5UserStringList(runtime);
        for(int i=0;i<1024;i++)binding.ReadInto(emptyReads,[],50);
        Reject(()=>binding.ReadInto(emptyReads,[],50),"Zero-byte reads cannot create unbounded provenance chains");
        Check(binding.CopyReadOrigins(emptyReads).Length==1024,"Origin-count bound retains existing history");
        Reject(()=>byteBound.WriteForArchive(convertedClone,50),"Raw no-op output respects the receiving binding byte budget");
        return "PASS copied user-string provenance:"+checks+" actual-codec copy/assignment/write checks; whole-view history/full3dm/original-PC/physical parity separate.";
    }
}
