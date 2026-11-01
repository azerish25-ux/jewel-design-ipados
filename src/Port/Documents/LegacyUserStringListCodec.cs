using System.Buffers.Binary;
namespace JewelDesign.Documents;
public sealed record LegacyUserStringListLimits(int MaxPayloadBytes=64*1024*1024,int MaxPairs=1_000_000,int MaxStringCodeUnits=4_000_000)
{internal void Validate(){if(MaxPayloadBytes<32||MaxPairs<0||MaxStringCodeUnits<1)throw new ArgumentOutOfRangeException(nameof(MaxPayloadBytes));}}
public sealed class LegacyUserStringListReadResult
{
 private readonly KeyValuePair<string,string>[] pairs;private readonly byte[] original;
 public bool NativeReadSucceeded {get;} public int CrcErrorCount {get;} public int SkippedCrcCount {get;} public long ConsumedBytes {get;} public bool ManagedLimitHit {get;} public bool HasUnknownSchema {get;}
 internal LegacyUserStringListReadResult(byte[] b,List<KeyValuePair<string,string>> p,bool ok,int errors,int skipped,long position,bool limited,bool unknown){original=b;pairs=p.ToArray();NativeReadSucceeded=ok;CrcErrorCount=errors;SkippedCrcCount=skipped;ConsumedBytes=position;ManagedLimitHit=limited;HasUnknownSchema=unknown;}
 public KeyValuePair<string,string>[] CopyPairs()=>(KeyValuePair<string,string>[])pairs.Clone();
 public byte[] CopyOriginal()=>(byte[])original.Clone();
}
/// <summary>Native user-string-list body1.0; bool, CRC diagnostics and retained partial entries are distinct.</summary>
public static class LegacyUserStringListCodec
{
 private const uint Code=0x40008000;
 private static void ValidateVersion(int version){if(version!=4&&version!=5&&(version<50||version%10!=0))throw new ArgumentOutOfRangeException(nameof(version),"Only archive4, early5 and50+ multiples of10 are supported by this body codec.");}
 private sealed class Frame {public long End,CrcOffset;public uint Crc;}
 private sealed class Reader
 {
  internal readonly byte[] Bytes;internal readonly int Header;internal readonly LegacyUserStringListLimits Limits;
  internal long Position;internal int Errors,Skipped;internal bool Limited,Unknown;
  internal Reader(byte[] b,int version,LegacyUserStringListLimits limits){Bytes=b;Header=version<50?8:12;Limits=limits;}
  private bool Available(int count,long bound)=>count>=0&&Position>=0&&Position<=Bytes.Length-count&&Position<=bound-count;
  private bool Readable(Frame f,int count){if(count<0||Position<0||Position>f.CrcOffset-count)return false;if(Position>Bytes.Length-count){Position+=Math.Max(0,Bytes.Length-Position);return false;}return true;}
  internal Frame? Open(Frame? parent)
  {
   if(!Available(Header,parent?.CrcOffset??Bytes.Length))return null;
   uint code=BinaryPrimitives.ReadUInt32LittleEndian(Bytes.AsSpan((int)Position));long length=Header==8?BinaryPrimitives.ReadInt32LittleEndian(Bytes.AsSpan((int)Position+4)):BinaryPrimitives.ReadInt64LittleEndian(Bytes.AsSpan((int)Position+4));
   if(code!=Code||length<8||length>long.MaxValue-Position-Header)return null;
   Position+=Header;return new(){End=Position+length,CrcOffset=Position+length-4};
  }
  internal bool Int(Frame f,out int value){value=0;if(!Readable(f,4))return false;value=BinaryPrimitives.ReadInt32LittleEndian(Bytes.AsSpan((int)Position));f.Crc=CsgCrc32.Append(f.Crc,Bytes.AsSpan((int)Position,4));Position+=4;return true;}
  internal bool Version(Frame f,out int major){major=0;if(!Int(f,out major)||major<1||!Int(f,out int minor)||minor<0)return false;if(minor!=0)Unknown=true;return true;}
  internal bool Text(Frame f,out string value)
  {
   value=string.Empty;if(!Int(f,out int n)||n<0)return false;if(n>Limits.MaxStringCodeUnits){Limited=true;return false;}if(n==0)return true;
   long size=2L*n;if(size>int.MaxValue||!Readable(f,(int)size))return false;
   int start=(int)Position;value=string.Create(n-1,(Bytes,start),static(chars,state)=>{for(int i=0;i<chars.Length;i++)chars[i]=(char)BinaryPrimitives.ReadUInt16LittleEndian(state.Bytes.AsSpan(state.start+2*i,2));});
   f.Crc=CsgCrc32.Append(f.Crc,Bytes.AsSpan((int)Position,(int)size));Position+=(int)size;return true;
  }
  internal bool End(Frame f)
  {
   bool ok=true;long local=Position;
   if(Position==f.CrcOffset)
   {
    int available=(int)Math.Max(0,Math.Min(4,(long)Bytes.Length-Position));
    if(available==4){uint expected=BinaryPrimitives.ReadUInt32LittleEndian(Bytes.AsSpan((int)Position));Position+=4;local+=4;if(expected!=f.Crc)Errors++;}
    else {Position+=available;ok=false;}
   }
   else {Skipped++;Unknown=true;}
   // Native buffer archives allow seeks beyond the buffer. Partial CRC reads advance the real
   // cursor while the local offset is unchanged, so its later relative seek can overshoot End.
   try{Position=checked(Position+(f.End-local));}catch(OverflowException){return false;}
   return ok;
  }
 }
 public static LegacyUserStringListReadResult Read(ReadOnlySpan<byte> payload,int archiveVersion,LegacyUserStringListLimits? limits=null)
 {
  limits??=new();limits.Validate();ValidateVersion(archiveVersion);if(payload.Length>limits.MaxPayloadBytes)throw new InvalidDataException("User-string payload exceeds managed bound");
  var bytes=payload.ToArray();var values=new List<KeyValuePair<string,string>>();var r=new Reader(bytes,archiveVersion,limits);bool success=false;var root=r.Open(null);
  if(root is not null)
  {
   int count=0;success=r.Version(root,out int major)&&major==1&&r.Int(root,out count);
   if(success)for(int i=0;i<count;i++)
   {
    if(i>=limits.MaxPairs){r.Limited=true;success=false;break;}var entry=r.Open(root);if(entry is null){success=false;break;}
    string key=string.Empty,value=string.Empty;bool valid=r.Version(entry,out int v)&&v==1&&r.Text(entry,out key)&&r.Text(entry,out value);bool ended=r.End(entry);
    if(!valid||!ended){success=false;break;}values.Add(new(key,value));
   }
   bool closed=r.End(root);success=success&&closed;
  }
  return new(bytes,values,success,r.Errors,r.Skipped,r.Position,r.Limited,r.Unknown);
 }
 /// <summary>Canonical native1.0 output; retained original bytes remain necessary for future/opaque-schema no-op preservation.</summary>
 public static byte[] Write(ReadOnlySpan<KeyValuePair<string,string>> pairs,int archiveVersion,LegacyUserStringListLimits? limits=null)
 {
  limits??=new();limits.Validate();ValidateVersion(archiveVersion);if(pairs.Length>limits.MaxPairs)throw new InvalidDataException("User-string pair limit");int header=archiveVersion<50?8:12;long total=header+16;
  foreach(var p in pairs){ArgumentNullException.ThrowIfNull(p.Key);ArgumentNullException.ThrowIfNull(p.Value);foreach(var s in new[]{p.Key,p.Value})if(s.Length+1L>limits.MaxStringCodeUnits)throw new InvalidDataException("User-string text limit");total+=header+20+2L*(p.Key.Length+(p.Key.Length==0?0:1)+p.Value.Length+(p.Value.Length==0?0:1));if(total>limits.MaxPayloadBytes)throw new InvalidDataException("User-string payload limit");}
  using var body=new MemoryStream();using var writer=new BinaryWriter(body);writer.Write(1);writer.Write(0);writer.Write(pairs.Length);uint rootCrc=CsgCrc32.Compute(body.ToArray());
  foreach(var p in pairs){using var entry=new MemoryStream();using var e=new BinaryWriter(entry);e.Write(1);e.Write(0);Text(e,p.Key);Text(e,p.Value);var data=entry.ToArray();Chunk(writer,data,header,CsgCrc32.Compute(data));}
  using var output=new MemoryStream();using var final=new BinaryWriter(output);Chunk(final,body.ToArray(),header,rootCrc);return output.ToArray();
 }
 private static void Text(BinaryWriter w,string value){if(value.Length==0){w.Write(0);return;}w.Write(checked(value.Length+1));foreach(char c in value)w.Write((ushort)c);w.Write((ushort)0);}
 private static void Chunk(BinaryWriter w,byte[] body,int header,uint crc){w.Write(Code);if(header==8)w.Write(checked(body.Length+4));else w.Write((long)body.Length+4);w.Write(body);w.Write(crc);}
}
