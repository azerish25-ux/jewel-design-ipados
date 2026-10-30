using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using JewelDesign.Documents;
using Schema = LegacyPortfolioSchema;

namespace DocumentContractChecks;

public static class PortfolioAssertions
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Schema.Project))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Schema.Design))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Schema.ProjectType))]
    public static string Run()
    {
        var baseId = Guid.Parse("8c969c9a-a810-4fe2-8fc6-1d802d26bd55");
        var historyId = Guid.Parse("f11630c7-77d0-4269-a919-de50173a674c");
        Schema.Design Design(Guid id) => new() { BaseDesignInstance=baseId, DesignInstance=id, Path=@"C:\Portfolios\Fixture\"+baseId+"_"+id+".csg", Name="Design", CSUserDesignName="Original", ComponentIds=new List<int>{1,7,99}, SelectedIterationId=3, RenderFileCenterOffset=9007199254740993, Variants=new Collection<Schema.Design>() };
        var source = new Schema.Project { Name="Fixture", Comments="Source & notes", Path=@"C:\Portfolios\Fixture", FirstName="Fixture", Email="fixture@example.invalid", DefaultRingSizeIndex=-1, ProjectType=Schema.ProjectType.Collection, Designs=new ObservableCollection<Schema.Design>{Design(baseId),Design(historyId)} };
        source.Designs[0].IsBaseDesign=true; source.Designs[1].Variants.Add(Design(Guid.Empty));
        var serializer = new XmlSerializer(typeof(Schema.Project));
        var baseline = Serialize(serializer,source,new UTF8Encoding(false));
        var tree = XDocument.Parse(Encoding.UTF8.GetString(baseline),LoadOptions.PreserveWhitespace);
        XNamespace future="urn:future";
        tree.Root!.Add(new XAttribute("extension","kept"),new XComment("preserve comment"),new XElement(future+"Payload",new XAttribute("mode","opaque"),new XCData("opaque <&>")));
        tree.Root!.Element("Name")!.SetAttributeValue("futureFlag","keep");
        tree.Root!.Element("Name")!.Add(new XComment("scalar comment"));
        tree.Root!.Element("Designs")!.Elements("Design").First().Add(new XElement("FutureDesign", "retained"));
        var bytes=Encoding.UTF8.GetBytes(tree.ToString(SaveOptions.DisableFormatting));
        tree=XDocument.Load(new MemoryStream(bytes),LoadOptions.PreserveWhitespace); // compare the actual serialized input, including namespace declarations
        var document=LegacyPortfolioXmlCodec.Read(bytes);
        Equal(bytes,LegacyPortfolioXmlCodec.Write(document),"Exact XML no-op");
        if(document.DesignCount!=2||document.ReadProjectText("Path")!=source.Path||document.ReadDesignGuid(1,"DesignInstance")!=historyId)throw new Exception("Stored path/GUID changed");
        var edited=document.WithProjectText("Comments","Changed <&> Ω").WithProjectText("Email",null).WithDesignText(1,"CSUserDesignName","History renamed").WithFavorite(0,true).WithSelectedIteration(1,null);
        var result=LegacyPortfolioXmlCodec.Write(edited);
        var decoded=(Schema.Project)serializer.Deserialize(new MemoryStream(result))!;
        if(decoded.Comments!="Changed <&> Ω"||decoded.Email is not null||!decoded.Designs[0].IsFavorite||decoded.Designs[1].SelectedIterationId is not null||decoded.Designs[1].CSUserDesignName!="History renamed")throw new Exception($"Source-derived serializer mismatch: comments={decoded.Comments}, email-null={decoded.Email is null}, favorite={decoded.Designs[0].IsFavorite}, iteration={decoded.Designs[1].SelectedIterationId}, label={decoded.Designs[1].CSUserDesignName}");
        if(decoded.Path!=source.Path||decoded.ProjectType!=Schema.ProjectType.Collection||decoded.DefaultRingSizeIndex!=-1||decoded.Designs[1].Variants.Count!=1||decoded.Designs[0].RenderFileCenterOffset!=9007199254740993||!decoded.Designs[0].ComponentIds.SequenceEqual(new[]{1,7,99}))throw new Exception("Unedited schema data lost");
        var after=XDocument.Load(new MemoryStream(result),LoadOptions.PreserveWhitespace);
        if(!XNode.DeepEquals(tree.Root!.Element(future+"Payload"),after.Root!.Element(future+"Payload"))||after.Root.Attribute("extension")?.Value!="kept"||after.Root.Element("Name")?.Attribute("futureFlag")?.Value!="keep"||!after.DescendantNodes().OfType<XComment>().Any(c=>c.Value=="preserve comment")||after.Root.Element("Designs")!.Elements("Design").First().Element("FutureDesign")?.Value!="retained")throw new Exception($"Unknown extension mismatch: future={XNode.DeepEquals(tree.Root!.Element(future+"Payload"),after.Root.Element(future+"Payload"))}, root={after.Root.Attribute("extension")?.Value}, flag={after.Root.Element("Name")?.Attribute("futureFlag")?.Value}, comments={after.DescendantNodes().OfType<XComment>().Count()}, design={after.Root.Element("Designs")!.Elements("Design").First().Element("FutureDesign")?.Value}");
        var nameEdit=XDocument.Load(new MemoryStream(LegacyPortfolioXmlCodec.Write(document.WithProjectText("Name","Renamed"))));
        if(!nameEdit.Root!.Element("Name")!.Nodes().OfType<XComment>().Any(c=>c.Value=="scalar comment"))throw new Exception("Scalar edit discarded comment");
        Throws<NotSupportedException>(()=>document.WithProjectText("Name",null));
        Equal(bytes,LegacyPortfolioXmlCodec.Write(document),"Original mutated by edit");
        Equal(bytes,LegacyPortfolioXmlCodec.Write(document.WithProjectText("Name","Fixture")),"Same-value edit rewrote original");
        var alias=LegacyPortfolioXmlCodec.Write(document);alias[0]=0;Equal(bytes,LegacyPortfolioXmlCodec.Write(document),"Output alias");
        foreach(var encoding in new Encoding[]{new UTF8Encoding(false),new UTF8Encoding(true),new UnicodeEncoding(false,true),new UnicodeEncoding(true,true)})
        {
            var encoded=Serialize(serializer,source,encoding);var parsed=LegacyPortfolioXmlCodec.Read(encoded);
            Equal(encoded,LegacyPortfolioXmlCodec.Write(parsed),"Encoding/BOM no-op");
            var changed=LegacyPortfolioXmlCodec.Write(parsed.WithProjectText("Name","New Ω"));
            var reread=(Schema.Project)serializer.Deserialize(new MemoryStream(changed))!;
            if(reread.Name!="New Ω"||reread.Designs.Count!=2)throw new Exception("Encoding edit not readable");
            if(encoding.GetPreamble().Length>0&&!changed.AsSpan().StartsWith(encoding.GetPreamble()))throw new Exception("Encoding/BOM lost");
        }
        int elementBudget=XDocument.Load(new MemoryStream(baseline)).Descendants().Count();
        var exactLimit=LegacyPortfolioXmlCodec.Read(baseline,new LegacyPortfolioXmlLimits(MaxElements:elementBudget));
        Throws<InvalidDataException>(()=>LegacyPortfolioXmlCodec.Write(exactLimit.WithProjectText("City","Added field")));

        Throws<NotSupportedException>(()=>LegacyPortfolioXmlCodec.Read(Serialize(serializer,source,new UTF32Encoding(false,true))));
        var ringChanged=(Schema.Project)serializer.Deserialize(new MemoryStream(LegacyPortfolioXmlCodec.Write(document.WithDefaultRingSizeIndex(5))))!;
        if(ringChanged.DefaultRingSizeIndex!=5)throw new Exception("Typed project integer edit");
        Throws<XmlException>(()=>LegacyPortfolioXmlCodec.Read(Encoding.UTF8.GetBytes("<!DOCTYPE Project [<!ENTITY external SYSTEM 'file:///never-read'>]><Project><Name>&external;</Name></Project>")));
        Throws<InvalidDataException>(()=>LegacyPortfolioXmlCodec.Read(bytes,new LegacyPortfolioXmlLimits(MaxBytes:32)));
        Throws<InvalidDataException>(()=>LegacyPortfolioXmlCodec.Read(bytes,new LegacyPortfolioXmlLimits(MaxDepth:2)));
        Throws<InvalidDataException>(()=>LegacyPortfolioXmlCodec.Read(bytes,new LegacyPortfolioXmlLimits(MaxElements:2)));
        Throws<NotSupportedException>(()=>LegacyPortfolioXmlCodec.Read(Encoding.UTF8.GetBytes("<Project xmlns='urn:future'/>")));
        Throws<ArgumentException>(()=>document.WithProjectText("Unknown", "x"));
        Throws<ArgumentOutOfRangeException>(()=>document.WithFavorite(2,true));
        var ambiguous=LegacyPortfolioXmlCodec.Read(Encoding.UTF8.GetBytes("<Project><Name>A</Name><Name>B</Name></Project>"));
        Throws<InvalidDataException>(()=>ambiguous.WithProjectText("Name","C"));
        var complex=LegacyPortfolioXmlCodec.Read(Encoding.UTF8.GetBytes("<Project><Name><Future/>A</Name></Project>"));
        Throws<NotSupportedException>(()=>complex.WithProjectText("Name","C"));

        var leaf=baseId+"_"+historyId+".csg";var key=LegacyDesignFileKey.Parse(leaf);
        if(key.BaseDesignInstance!=baseId||key.DesignInstance!=historyId||key.IsBaseDesign||key.FavoritePreviewName!=baseId+"_"+historyId+"_fav.png"||key.SearchFavoritePreviewName!=baseId+"_"+historyId+"_fav_spi.png"||key.BasePreviewName!=baseId+"_spi.png")throw new Exception("Paired filenames/sidecars");
        if(!LegacyDesignFileKey.Parse(baseId+"_"+baseId+".CSG").IsBaseDesign)throw new Exception("Windows case-insensitive extension");
        if(LegacyDesignFileKey.Parse(baseId.ToString().ToUpperInvariant()+"_"+baseId+".csg").IsBaseDesign)throw new Exception("Native raw stem comparison was normalized");
        if(LegacyDesignFileKey.StoredFileName(@"C:\Portfolios\Fixture\"+leaf)!=leaf||LegacyDesignFileKey.StoredFileName("/import/"+leaf)!=leaf)throw new Exception("Stored Windows path basename");
        Throws<ArgumentException>(()=>LegacyDesignFileKey.Parse("../"+leaf));
        Throws<FormatException>(()=>LegacyDesignFileKey.Parse("bad_bad.csg"));
        return $"PASS portfolio XML: source-derived 16/27 property contracts, nested/unknown/nil/path and UTF8/UTF16 preservation, bounded DTD rejection and paired filenames; synthetic fixtures only; dynamic-code={RuntimeFeature.IsDynamicCodeSupported}; no PC portfolio/load-mutation claim.";
    }

    private static byte[] Serialize(XmlSerializer serializer,Schema.Project source,Encoding encoding)
    {
        using var stream=new MemoryStream();using(var writer=XmlWriter.Create(stream,new XmlWriterSettings{Encoding=encoding,Indent=true}))serializer.Serialize(writer,source);return stream.ToArray();
    }
    private static void Equal(byte[] a,byte[] b,string message){if(!a.AsSpan().SequenceEqual(b))throw new Exception(message);}
    private static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
}
