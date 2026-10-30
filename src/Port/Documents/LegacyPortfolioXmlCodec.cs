using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace JewelDesign.Documents;

public sealed record LegacyPortfolioXmlLimits(int MaxBytes=16*1024*1024, long MaxCharacters=16*1024*1024, int MaxDepth=64, int MaxElements=100000)
{
    internal void Validate() { if(MaxBytes<=0||MaxCharacters<=0||MaxDepth<=0||MaxElements<=0)throw new ArgumentOutOfRangeException(nameof(LegacyPortfolioXmlLimits)); }
}

public static class LegacyPortfolioXmlCodec
{
    public static LegacyPortfolioXmlDocument Read(ReadOnlySpan<byte> input, LegacyPortfolioXmlLimits? limits=null)
    {
        limits??=new LegacyPortfolioXmlLimits();limits.Validate();
        if(input.Length>limits.MaxBytes)throw new InvalidDataException("Portfolio XML exceeds byte limit");
        var bytes=input.ToArray();
        Validate(bytes,limits);var settings=Settings(limits);
        XDocument tree;using(var reader=XmlReader.Create(new MemoryStream(bytes),settings))tree=XDocument.Load(reader,LoadOptions.PreserveWhitespace);
        if(tree.Root?.Name!=XName.Get("Project"))throw new NotSupportedException("Expected unqualified Project XML root");
        return new LegacyPortfolioXmlDocument(tree,bytes,DetectEncoding(bytes,tree.Declaration?.Encoding),limits,false);
    }

    private static XmlReaderSettings Settings(LegacyPortfolioXmlLimits limits)=>new(){DtdProcessing=DtdProcessing.Prohibit,XmlResolver=null,MaxCharactersInDocument=limits.MaxCharacters,IgnoreWhitespace=false,IgnoreComments=false};

    internal static void Validate(byte[] bytes,LegacyPortfolioXmlLimits limits)
    {
        if(bytes.Length>limits.MaxBytes)throw new InvalidDataException("Portfolio XML exceeds byte limit");
        using var scan=XmlReader.Create(new MemoryStream(bytes),Settings(limits));int elements=0;
        while(scan.Read())
            if(scan.Depth>limits.MaxDepth||(scan.NodeType==XmlNodeType.Element&&++elements>limits.MaxElements))
                throw new InvalidDataException("Portfolio XML depth/element limit exceeded");
    }

    public static byte[] Write(LegacyPortfolioXmlDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);return document.Encode();
    }

    private static Encoding DetectEncoding(byte[] bytes,string? declared)
    {
        if((bytes.Length>=4&&((bytes[0]==0xff&&bytes[1]==0xfe&&bytes[2]==0&&bytes[3]==0)||(bytes[0]==0&&bytes[1]==0&&bytes[2]==0xfe&&bytes[3]==0xff)))
            ||(declared is not null&&!declared.Equals("utf-8",StringComparison.OrdinalIgnoreCase)&&!declared.Equals("utf-16",StringComparison.OrdinalIgnoreCase)&&!declared.Equals("utf-16le",StringComparison.OrdinalIgnoreCase)&&!declared.Equals("utf-16be",StringComparison.OrdinalIgnoreCase)))
            throw new NotSupportedException("Only UTF8 and UTF16 portfolio XML are supported for edits");
        if(bytes.AsSpan().StartsWith(new byte[]{0xef,0xbb,0xbf}))return new UTF8Encoding(true,true);
        if(bytes.AsSpan().StartsWith(new byte[]{0xff,0xfe}))return new UnicodeEncoding(false,true,true);
        if(bytes.AsSpan().StartsWith(new byte[]{0xfe,0xff}))return new UnicodeEncoding(true,true,true);
        if(bytes.Length>=4&&bytes[0]==0&&bytes[1]==60)return new UnicodeEncoding(true,false,true);
        if(bytes.Length>=4&&bytes[0]==60&&bytes[1]==0)return new UnicodeEncoding(false,false,true);
        if(string.IsNullOrEmpty(declared)||declared.Equals("utf-8",StringComparison.OrdinalIgnoreCase))return new UTF8Encoding(false,true);
        throw new NotSupportedException("Portfolio XML encoding is not supported for loss-preserving edits: "+declared);
    }
}

/// <summary>Private immutable XML data; no desktop path access, load mutation or model setter execution.</summary>
public sealed class LegacyPortfolioXmlDocument
{
    private static readonly XNamespace xsi="http://www.w3.org/2001/XMLSchema-instance";
    private static readonly HashSet<string> projectText=new(StringComparer.Ordinal){"Address1","Address2","City","Comments","Country","Email","FirstName","LastName","Name","Path","PhoneNumber","State","Zip"};
    private static readonly HashSet<string> designText=new(StringComparer.Ordinal){"CSUserDesignName","Designer","DesignType","Name","Path","SeriesNumber","SPINumber"};
    private readonly XDocument tree;
    private readonly byte[] original;
    private readonly Encoding encoding;
    private readonly LegacyPortfolioXmlLimits limits;
    private readonly bool modified;
    public int DesignCount=>DesignElements(tree).Length;

    internal LegacyPortfolioXmlDocument(XDocument tree,byte[] original,Encoding encoding,LegacyPortfolioXmlLimits limits,bool modified)
    {this.tree=tree;this.original=original;this.encoding=encoding;this.limits=limits;this.modified=modified;}

    public string? ReadProjectText(string field){ValidateField(field,projectText);return Scalar(tree.Root!,field);}
    public string? ReadDesignText(int ordinal,string field){ValidateField(field,designText);return Scalar(Design(tree,ordinal),field);}
    public Guid? ReadDesignGuid(int ordinal,string field)
    {
        if(field is not ("BaseDesignInstance" or "DesignInstance"))throw new ArgumentException("Not a declared design GUID field",nameof(field));
        var text=Scalar(Design(tree,ordinal),field);if(text is null)return null;
        if(!Guid.TryParse(text,out var id))throw new InvalidDataException("Invalid design GUID field");return id;
    }
    public LegacyPortfolioXmlDocument WithProjectText(string field,string? value){ValidateField(field,projectText);return Change(null,field,value);}
    public LegacyPortfolioXmlDocument WithDesignText(int ordinal,string field,string? value){ValidateField(field,designText);return Change(ordinal,field,value);}
    public LegacyPortfolioXmlDocument WithFavorite(int ordinal,bool value)=>Change(ordinal,"IsFavorite",XmlConvert.ToString(value));
    public LegacyPortfolioXmlDocument WithSelectedIteration(int ordinal,int? value)=>Change(ordinal,"SelectedIterationId",value.HasValue?XmlConvert.ToString(value.Value):null,nullable:true);
    public LegacyPortfolioXmlDocument WithDefaultRingSizeIndex(int value)=>Change(null,"DefaultRingSizeIndex",XmlConvert.ToString(value));

    private LegacyPortfolioXmlDocument Change(int? ordinal,string field,string? value,bool nullable=false)
    {
        if(value is not null){if(value.Length>limits.MaxCharacters)throw new InvalidDataException("Metadata exceeds character limit");XmlConvert.VerifyXmlChars(value);}
        var parent=ordinal.HasValue?Design(tree,ordinal.Value):tree.Root!;
        if(Scalar(parent,field,nullable)==value)return this;
        var copy=new XDocument(tree);var target=ordinal.HasValue?Design(copy,ordinal.Value):copy.Root!;
        var element=Single(target,field);if(element is null){element=new XElement(field);target.Add(element);}
        if(value is null&&!nullable)
        {
            if(element.Attributes().Any(a=>!a.IsNamespaceDeclaration&&a.Name!=xsi+"nil")||element.Nodes().Any(n=>n is not XText))
                throw new NotSupportedException("Removing the string field would discard opaque metadata");
            element.Remove();
        }
        else
        {
            foreach(var text in element.Nodes().OfType<XText>().ToArray())text.Remove();
            if(value is null)element.SetAttributeValue(xsi+"nil","true");
            else{element.SetAttributeValue(xsi+"nil",null);element.AddFirst(new XText(value));}
        }
        return new LegacyPortfolioXmlDocument(copy,original,encoding,limits,true);
    }
    private static void ValidateField(string field,HashSet<string> allowed)
    {ArgumentNullException.ThrowIfNull(field);if(!allowed.Contains(field))throw new ArgumentException("Field is not declared editable text metadata",nameof(field));}
    private static XElement? Single(XElement parent,string name)
    {var matches=parent.Elements(name).ToArray();if(matches.Length>1)throw new InvalidDataException("Ambiguous duplicate XML field: "+name);return matches.SingleOrDefault();}
    private static string? Scalar(XElement parent,string field,bool nullable=false)
    {
        var element=Single(parent,field);if(element is null)return null;
        if(element.HasElements)throw new NotSupportedException("Scalar field contains unknown structured XML: "+field);
        if(nullable&&element.Attribute(xsi+"nil") is { } nil&&XmlConvert.ToBoolean(nil.Value))return null;
        return element.Value;
    }
    private static XElement[] DesignElements(XDocument document)=>Single(document.Root!,"Designs")?.Elements("Design").ToArray()??Array.Empty<XElement>();
    private static XElement Design(XDocument document,int ordinal)
    {var items=DesignElements(document);if((uint)ordinal>=(uint)items.Length)throw new ArgumentOutOfRangeException(nameof(ordinal));return items[ordinal];}

    internal byte[] Encode()
    {
        if(!modified)return (byte[])original.Clone();
        using var output=new MemoryStream();
        using(var writer=XmlWriter.Create(output,new XmlWriterSettings{Encoding=encoding,Indent=false,NewLineHandling=NewLineHandling.None,OmitXmlDeclaration=tree.Declaration is null}))tree.Save(writer);
        if(output.Length>limits.MaxBytes)throw new InvalidDataException("Edited XML exceeds byte limit");
        var encoded=output.ToArray();LegacyPortfolioXmlCodec.Validate(encoded,limits);return encoded;
    }
}
