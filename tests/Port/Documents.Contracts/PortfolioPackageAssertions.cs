using System.Text;
using JewelDesign.Documents;

namespace DocumentContractChecks;

internal static class PortfolioPackageAssertions
{
    public static string Run(byte[] csg)
    {
        var baseId=Guid.Parse("8c969c9a-a810-4fe2-8fc6-1d802d26bd55");
        var historyId=Guid.Parse("f11630c7-77d0-4269-a919-de50173a674c");
        string baseName=baseId+"_"+baseId+".csg",historyName=baseId+"_"+historyId+".csg";
        string Design(string file,Guid id)=>$@"<Design><BaseDesignInstance>{baseId}</BaseDesignInstance><DesignInstance>{id}</DesignInstance><Path>C:\Old\Fixture\{file}</Path><Name>Item</Name></Design>";
        byte[] Manifest(string designs)=>Encoding.UTF8.GetBytes(@"<Project><Name>Fixture</Name><Path>C:\Old\Fixture</Path><Comments>Keep</Comments><Designs>"+designs+"</Designs><Future retained='yes'/></Project>");
        var manifest=Manifest(Design(baseName,baseId)+Design(historyName,historyId));
        var preview=new byte[]{0,255,7};var opaque=new byte[]{4,0,9};
        var members=new[]{new LegacyPortfolioMember("Fixture.csp",manifest),new LegacyPortfolioMember(baseName,csg,DateTimeOffset.Parse("2020-01-01T00:00:00Z")),
            new LegacyPortfolioMember(historyName,csg,DateTimeOffset.Parse("2021-01-01T00:00:00Z")),new LegacyPortfolioMember(baseId+"_spi.png",preview),
            new LegacyPortfolioMember(baseId+"_"+baseId+"_fav.png",preview),new LegacyPortfolioMember("extras/custom.blob",opaque),new LegacyPortfolioMember("extras/nested.csg",opaque)};
        var package=LegacyPortfolioPackage.Create("fixture.CSP",members);
        if(package.Members.Count!=7||package.Designs.Count!=2||package.Diagnostics.Count!=0)throw new Exception("Portfolio member/design inventory");
        if(package.GetHistoryCandidates(baseId).Count!=1||package.Designs[0].MetadataOrdinals.Count!=1||package.Designs[0].BasePreviewMember!=baseId+"_spi.png")throw new Exception("History/metadata/preview candidate binding");
        preview[0]=3;opaque[0]=3;
        if(package.CopyMemberBytes(@"EXTRAS\custom.blob")[0]!=4||package.CopyMemberBytes(baseId+"_spi.png")[0]!=0)throw new Exception("Member input alias");
        var copy=package.CopyMemberBytes(baseName);copy[0]=0;Equal(csg,package.CopyMemberBytes(baseName),"Member output alias");
        var opened=package.OpenDesign(baseName.ToUpperInvariant());
        var value=opened.Session.GetParameter(8);opened.Session.BeginEditTransaction();opened.Session.SetValue(value,4.48);opened.Session.EndEditTransaction();
        var changed=package.WithDesignArchiveData(opened);
        var reopened=changed.OpenDesign(baseName).Session;
        if(reopened.ReadValue(reopened.GetParameter(8)).RealValue!=4.48)throw new Exception("Package edit/reopen");
        Equal(csg,package.CopyMemberBytes(baseName),"Original package changed");
        foreach(var member in package.Members)if(member.RelativePath!=baseName)Equal(package.CopyMemberBytes(member.RelativePath),changed.CopyMemberBytes(member.RelativePath),"Opaque or sidecar changed");
        var metadata=changed.WithProjectMetadata(changed.Manifest.WithProjectText("Comments","Edited"));
        if(metadata.Manifest.ReadProjectText("Comments")!="Edited"||metadata.Manifest.ReadProjectText("Path")!=@"C:\Old\Fixture")throw new Exception("Metadata edit/path rewrite");
        Equal(changed.CopyMemberBytes(baseName),metadata.CopyMemberBytes(baseName),"Metadata edit changed CSG");
        var foreignMetadata=LegacyPortfolioXmlCodec.Read(Encoding.UTF8.GetBytes("<Project><Name>Other</Name></Project>"));
        Throws<ArgumentException>(()=>package.WithProjectMetadata(foreignMetadata));
        Throws<ArgumentException>(()=>metadata.WithProjectMetadata(changed.Manifest.WithProjectText("Comments","Stale")));

        Throws<ArgumentException>(()=>changed.WithDesignArchiveData(opened));
        var foreign=LegacyPortfolioPackage.Create("Fixture.csp",members).OpenDesign(baseName);
        Throws<ArgumentException>(()=>package.WithDesignArchiveData(foreign));
        Throws<NotSupportedException>(()=>package.OpenDesign("extras/nested.csg"));
        Throws<KeyNotFoundException>(()=>package.CopyMemberBytes("missing.bin"));
        Throws<ArgumentException>(()=>new LegacyPortfolioMember("../escape",opaque));
        Throws<ArgumentException>(()=>new LegacyPortfolioMember(@"C:\escape",opaque));
        Throws<ArgumentException>(()=>new LegacyPortfolioMember("/escape",opaque));
        Throws<InvalidDataException>(()=>LegacyPortfolioPackage.Create("Fixture.csp",members.Append(new LegacyPortfolioMember("fixture.CSP",manifest))));
        Throws<InvalidDataException>(()=>LegacyPortfolioPackage.Create("Fixture.csp",members,new LegacyPortfolioPackageLimits(MaxTotalBytes:10)));
        Throws<InvalidDataException>(()=>LegacyPortfolioPackage.Create("Fixture.csp",members,new LegacyPortfolioPackageLimits(MaxMembers:2)));

        var missing=LegacyPortfolioPackage.Create("Fixture.csp",new[]{new LegacyPortfolioMember("Fixture.csp",manifest),members[1]});
        if(!missing.Diagnostics.Any(x=>x.Kind==LegacyPortfolioIssueKind.MissingDesignMember))throw new Exception("Missing metadata member not reported");
        Equal(manifest,missing.CopyMemberBytes("Fixture.csp"),"Missing entry was silently removed");
        var ambiguous=LegacyPortfolioPackage.Create("Fixture.csp",new[]{new LegacyPortfolioMember("Fixture.csp",Manifest(Design(baseName,baseId)+Design(baseName,baseId))),members[1]});
        if(!ambiguous.Diagnostics.Any(x=>x.Kind==LegacyPortfolioIssueKind.AmbiguousMetadata)||ambiguous.Designs[0].MetadataOrdinals.Count!=2)throw new Exception("Ambiguous metadata silently picked");
        var mismatch=LegacyPortfolioPackage.Create("Fixture.csp",new[]{new LegacyPortfolioMember("Fixture.csp",Manifest(Design(baseName,historyId))),members[1]});
        if(!mismatch.Diagnostics.Any(x=>x.Kind==LegacyPortfolioIssueKind.MetadataGuidMismatch))throw new Exception("GUID/file mismatch not reported");
        return "PASS portfolio package: immutable member/sidecar preservation, real CSG edit/reopen, metadata/history candidates, ownership/case/path/bounds guards and unresolved-entry diagnostics; synthetic inventory, no native folder/PC claim.";
    }
    private static void Equal(byte[] a,byte[] b,string name){if(!a.AsSpan().SequenceEqual(b))throw new Exception(name);}
    private static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
}
