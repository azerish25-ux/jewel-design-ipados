using System.Buffers.Binary;
using System.IO.Compression;
namespace JewelDesign.Rendering.Candidate;

public sealed record PngSampleImage(int Width,int Height,int BitDepth,int ColorType,int Interlace,byte[] Rgba);

/// <summary>PNG integer sample expansion to unassociated RGBA8. CRC, dimensions, row sizes, filters,
/// Adam7 and zlib/Adler are checked. No color-profile transform or alpha multiplication.
/// Original encoded metadata remains in LocalImageAsset;16-bit reduction/APNG are explicit gaps.</summary>
public static class PngStraightSamples
{
 private static readonly byte[] Signature=[137,80,78,71,13,10,26,10];
 private static readonly uint[] CrcTable=CreateCrcTable();
 private static uint[] CreateCrcTable(){var t=new uint[256];for(uint i=0;i<256;i++){uint c=i;for(int b=0;b<8;b++)c=(c&1)!=0?0xedb88320u^(c>>1):c>>1;t[i]=c;}return t;}
 private static uint Crc(ReadOnlySpan<byte> bytes){uint c=uint.MaxValue;foreach(byte b in bytes)c=CrcTable[(c^b)&255]^(c>>8);return ~c;}
 private static InvalidDataException Bad(string message)=>new("PNG: "+message);
 public static bool IsPng(ReadOnlySpan<byte> bytes)=>bytes.Length>=8&&bytes[..8].SequenceEqual(Signature);
 public static PngSampleImage Decode(ReadOnlySpan<byte> encoded,LocalImageLimits? limits=null)
 {
  limits??=new();limits.Validate();
  if(!IsPng(encoded)||encoded.Length>limits.MaxEncodedBytes)throw Bad("signature or encoded budget");
  int width=0,height=0,depth=0,type=0,interlace=0,channels=0,pos=8;
  bool header=false,dataSeen=false,dataEnded=false,end=false;byte[]? palette=null,transparency=null;
  using var compressed=new MemoryStream();
  while(pos<encoded.Length){
   if(encoded.Length-pos<12)throw Bad("truncated chunk");
   uint size=BinaryPrimitives.ReadUInt32BigEndian(encoded.Slice(pos,4));
   if(size>int.MaxValue||size>(uint)(encoded.Length-pos-12))throw Bad("chunk length");
   int length=(int)size;var name=encoded.Slice(pos+4,4);var data=encoded.Slice(pos+8,length);
   foreach(byte b in name)if(b is not (>=65 and <=90) and not (>=97 and <=122))throw Bad("chunk name");
   if((name[2]&32)!=0)throw Bad("reserved chunk bit");
   uint expected=BinaryPrimitives.ReadUInt32BigEndian(encoded.Slice(pos+8+length,4));
   if(Crc(encoded.Slice(pos+4,4+length))!=expected)throw Bad("chunk CRC");
   string kind=System.Text.Encoding.ASCII.GetString(name);pos+=12+length;
   if(!header&&kind!="IHDR")throw Bad("IHDR must be first");
   if(dataSeen&&kind!="IDAT")dataEnded=true;
   switch(kind){
    case "IHDR":
     if(header||length!=13)throw Bad("IHDR duplicate/length");header=true;
     uint w=BinaryPrimitives.ReadUInt32BigEndian(data),h=BinaryPrimitives.ReadUInt32BigEndian(data[4..]);
     if(w>int.MaxValue||h>int.MaxValue)throw Bad("dimension range");width=(int)w;height=(int)h;limits.ValidateDimensions(width,height);
     depth=data[8];type=data[9];interlace=data[12];
     if(data[10]!=0||data[11]!=0||interlace>1)throw Bad("compression/filter/interlace method");
     bool valid=type switch{0=>depth is 1 or 2 or 4 or 8 or 16,2 or 4 or 6=>depth is 8 or 16,3=>depth is 1 or 2 or 4 or 8,_=>false};
     if(!valid)throw Bad("color type/bit depth");
     if(depth==16)throw new NotSupportedException("PNG16 samples require an explicit precision-preserving/source-compatible output adapter.");
     channels=type switch{0 or 3=>1,2=>3,4=>2,_=>4};break;
    case "PLTE":
     if(dataSeen||palette is not null||transparency is not null||type is 0 or 4||length<3||length>768||length%3!=0)throw Bad("palette order/size");
     if(type==3&&length/3>1<<depth)throw Bad("palette exceeds index depth");palette=data.ToArray();break;
    case "tRNS":
     if(dataSeen||transparency is not null)throw Bad("transparency order/duplicate");
     if(type==0&&length!=2||type==2&&length!=6||type==3&&(palette is null||length<1||length>palette.Length/3)||type is 4 or 6)throw Bad("transparency size/type");
     transparency=data.ToArray();break;
    case "IDAT":
     if(dataEnded||type==3&&palette is null)throw Bad("image data order/palette");
     dataSeen=true;compressed.Write(data);break;
    case "IEND":
     if(length!=0||!dataSeen||compressed.Length<6)throw Bad("end/data");
     end=true;if(pos!=encoded.Length)throw Bad("data after IEND");break;
    case "acTL":case "fcTL":case "fdAT":throw new NotSupportedException("Animated PNG needs explicit frame/default-image selection.");
    default:if((name[0]&32)==0)throw new NotSupportedException("Unknown critical PNG chunk "+kind);break;
   }
   if(end)break;
  }
  if(!header||!end)throw Bad("missing IHDR/IEND");
  var buffer=compressed.GetBuffer();int compressedSize=checked((int)compressed.Length);
  if((buffer[0]&15)!=8||(buffer[0]>>4)>7||((buffer[0]<<8)|buffer[1])%31!=0||(buffer[1]&32)!=0)throw Bad("zlib header/dictionary");
  uint adlerExpected=BinaryPrimitives.ReadUInt32BigEndian(buffer.AsSpan(compressedSize-4,4));
  compressed.Position=0;using var z=new ZLibStream(compressed,CompressionMode.Decompress,leaveOpen:true);
  byte[] pixels=new byte[limits.ValidateDimensions(width,height)];var adler=new Adler();
  (int X,int Y,int Dx,int Dy)[] passes=interlace==0?[(0,0,1,1)]:[(0,0,8,8),(4,0,8,8),(0,4,4,8),(2,0,4,4),(0,2,2,4),(1,0,2,2),(0,1,1,2)];
  foreach(var pass in passes){
   int pw=width<=pass.X?0:(width-pass.X+pass.Dx-1)/pass.Dx,ph=height<=pass.Y?0:(height-pass.Y+pass.Dy-1)/pass.Dy;
   if(pw==0||ph==0)continue;
   int rowBytes=checked((int)(((long)pw*channels*depth+7)/8)),bpp=Math.Max(1,(channels*depth+7)/8);
   byte[] row=new byte[rowBytes],previous=new byte[rowBytes];
   for(int y=0;y<ph;y++){
    int filter=z.ReadByte();if(filter<0||filter>4)throw Bad("row filter/truncation");
    try{z.ReadExactly(row);}catch(EndOfStreamException e){throw new InvalidDataException("PNG truncated row",e);}
    adler.Byte((byte)filter);adler.Update(row);
    for(int i=0;i<row.Length;i++){
     int left=i>=bpp?row[i-bpp]:0,up=previous[i],ul=i>=bpp?previous[i-bpp]:0;
     int predict=filter switch{0=>0,1=>left,2=>up,3=>(left+up)/2,_=>Paeth(left,up,ul)};
     row[i]=unchecked((byte)(row[i]+predict));
    }
    int mask=(1<<depth)-1;
    for(int x=0;x<pw;x++){
     int dest=((pass.Y+y*pass.Dy)*width+pass.X+x*pass.Dx)*4;
     int Sample(int component){long bit=((long)x*channels+component)*depth;return (row[checked((int)(bit/8))]>>(8-depth-(int)(bit%8)))&mask;}
     int r,g,b,a=255;
     if(type==3){int index=Sample(0);if(palette is null||index>=palette.Length/3)throw Bad("palette index");r=palette[index*3];g=palette[index*3+1];b=palette[index*3+2];if(transparency is not null&&index<transparency.Length)a=transparency[index];}
     else if(type is 0 or 4){int sample=Sample(0);r=g=b=sample*255/mask;if(type==4)a=Sample(1);else if(transparency is not null&&(BinaryPrimitives.ReadUInt16BigEndian(transparency)&mask)==sample)a=0;}
     else{r=Sample(0);g=Sample(1);b=Sample(2);if(type==6)a=Sample(3);else if(transparency is not null&&(BinaryPrimitives.ReadUInt16BigEndian(transparency)&mask)==r&&(BinaryPrimitives.ReadUInt16BigEndian(transparency.AsSpan(2))&mask)==g&&(BinaryPrimitives.ReadUInt16BigEndian(transparency.AsSpan(4))&mask)==b)a=0;}
     pixels[dest]=(byte)r;pixels[dest+1]=(byte)g;pixels[dest+2]=(byte)b;pixels[dest+3]=(byte)a;
    }
    (row,previous)=(previous,row);
   }
  }
  if(z.ReadByte()!=-1)throw Bad("extra inflated image data");
  if(adler.Value!=adlerExpected)throw Bad("Adler checksum");
  return new(width,height,depth,type,interlace,pixels);
 }
 private static int Paeth(int a,int b,int c){int p=a+b-c,x=Math.Abs(p-a),y=Math.Abs(p-b),z=Math.Abs(p-c);return x<=y&&x<=z?a:y<=z?b:c;}
 private sealed class Adler
 {
  private uint a=1,b;
  internal uint Value=>(b<<16)|a;
  internal void Byte(byte value){a=(a+value)%65521;b=(b+a)%65521;}
  internal void Update(ReadOnlySpan<byte> bytes){while(!bytes.IsEmpty){int size=Math.Min(bytes.Length,5552);foreach(byte value in bytes[..size]){a+=value;b+=a;}a%=65521;b%=65521;bytes=bytes[size..];}}
 }
}
