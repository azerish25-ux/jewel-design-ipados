using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

try {
 if(args.Length!=3)throw new Exception("Expected input, output and test root.");
 var testRoot=Path.GetFullPath(args[2]);var input=Path.GetFullPath(args[0]);var output=Path.GetFullPath(args[1]);
 if(string.Equals(input,output,StringComparison.OrdinalIgnoreCase)||!output.EndsWith(".json",StringComparison.OrdinalIgnoreCase))throw new Exception("Verifier output must be a separate JSON report");
 foreach(var p in new[]{input,output})if(!p.StartsWith(testRoot+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase))throw new Exception("Path outside test root");
 if(new FileInfo(input).Length>64*1024*1024)throw new Exception("Input exceeds bounded verifier size");
 var result=IndependentCodec.Read(File.ReadAllBytes(input));var json=JsonSerializer.Serialize(result,new JsonSerializerOptions{WriteIndented=true});Directory.CreateDirectory(Path.GetDirectoryName(output)!);File.WriteAllText(output+".partial",json);File.Move(output+".partial",output,true);
 Console.WriteLine("PASS: independent C# reader, "+result.NodeCount+" records; "+result.NumericNodes.Count+" numeric nodes; all leaf CRCs and graph-directory CRC valid.");return 0;
}catch(Exception e){Console.Error.WriteLine("REJECTED: "+e.Message);return 1;}

record Block(int Offset,uint Code,int Length){public int Payload=>Offset+8;public int End=>Payload+Length;public int CrcOffset=>End-4;}
record NumericRecord(int Index,string TypeGuid,string Identifier,uint Flags,int Layer,int UserDataEntries,string UserDataHash,int[] Objects,int[] Channels,int[] Categories,int LegacyUiInteger,int ValueOffset,double Value,double Minimum,double Maximum,double? Step);
record RecordDigest(int Index,string TypeGuid,int Major,int Minor,string RawSha256,bool CrcValid);
record VerificationResult(string Scope,int FileBytes,string Sha256,string ContainerGuid,int[] FileInfoVersion,int[] Build,int ProvenanceCount,int DocumentUserDataEntries,string DocumentUserDataHash,int GemRecords,string GemTableHash,int CategoryRecords,string CategoryTableHash,int NodeCount,List<RecordDigest> Nodes,List<NumericRecord> NumericNodes,uint GraphDirectoryCrc,bool EndMarkerValid);
sealed class Bytes {
 public readonly byte[] Data;public int Position;readonly int limit;
 public Bytes(byte[] data,int start=0,int? end=null){Data=data;Position=start;limit=end??data.Length;}
 public void Need(int n){if(n<0||Position>limit-n)throw new Exception("Read outside bounded region at "+Position);}
 public int Int(){Need(4);int v=BinaryPrimitives.ReadInt32LittleEndian(Data.AsSpan(Position,4));Position+=4;return v;}
 public uint UInt()=>unchecked((uint)Int());
 public double Double(){Need(8);double v=BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64LittleEndian(Data.AsSpan(Position,8)));Position+=8;if(!double.IsFinite(v))throw new Exception("Nonfinite number");return v;}
 public int Count(int max=100000){int n=Int();if(n<0||n>max)throw new Exception("Count outside bound");return n;}
 public string Text(){int n=Count(1000000);int size=checked(n*2);Need(size);if(n>0&&BinaryPrimitives.ReadUInt16LittleEndian(Data.AsSpan(Position+size-2,2))!=0)throw new Exception("Unterminated UTF16");string s=Encoding.Unicode.GetString(Data,Position,size);Position+=size;return n>0?s[..^1]:s;}
 public string Guid(){Need(16);var g=new Guid(Data.AsSpan(Position,16));Position+=16;return g.ToString();}
 public int[] Ints(){int n=Count(10000);var a=new int[n];for(int i=0;i<n;i++)a[i]=Int();return a;}
 public int UserData(){int n=Count(10000);for(int i=0;i<n;i++){Text();Text();Text();}return n;}
 public Block Chunk(uint expected){int start=Position;uint code=UInt();int size=Int();if(code!=expected||size<4||(code&0x80000000)!=0)throw new Exception("Unexpected long chunk at "+start);Need(size);return new(start,code,size);}
}
static class IndependentCodec {
 const string Range="56b84606-7f1d-41fb-b81f-f521d9a9577d",Ring="dd5c2342-d89a-440e-a76e-624a1faf2b68";
 static string Hash(byte[] data,int start,int count)=>Convert.ToHexString(SHA256.HashData(data.AsSpan(start,count))).ToLowerInvariant();
 static uint Crc(byte[] data,int start,int count){uint c=0xffffffff;for(int n=start;n<start+count;n++){c^=data[n];for(int bit=0;bit<8;bit++)c=(c&1)!=0?(c>>1)^0xedb88320:c>>1;}return ~c;}
 static uint Check(byte[] b,Block c,int? end=null){uint stored=BinaryPrimitives.ReadUInt32LittleEndian(b.AsSpan(c.CrcOffset,4)),actual=Crc(b,c.Payload,(end??c.CrcOffset)-c.Payload);if(stored!=actual)throw new Exception("CRC mismatch at "+c.Offset);return stored;}
 public static VerificationResult Read(byte[] b){
 if(b.Length<32||Encoding.ASCII.GetString(b,0,32)!="3D Geometry File Format        4")throw new Exception("Unsupported archive header");var r=new Bytes(b,32);
 foreach(var type in new uint[]{1,0x10000014,0x10000015}){var c=r.Chunk(type);r.Position=c.End;}
 string marker=r.Guid();if(marker!="b237ee6c-ac7e-4725-86ee-54bccda34fea")throw new Exception("Unsupported CSG marker");
 var info=r.Chunk(0x40008000);int[] infoVersion={r.Int(),r.Int()};r.Text();r.Text();int[] build={r.Int(),r.Int(),r.Int(),r.Int()};int provenance=r.Count(10000);for(int i=0;i<provenance;i++)r.Guid();if(r.Position!=info.CrcOffset)throw new Exception("File-info trailing bytes");Check(b,info);r.Position=info.End;
 int userStart=r.Position,userCount=r.UserData();string userHash=Hash(b,userStart,r.Position-userStart);
 var gems=r.Chunk(0x40008000);if(r.Int()!=1||r.Int()!=0)throw new Exception("Unsupported gem table version");int gemCount=r.Count();string gemHash=Hash(b,gems.Offset,gems.End-gems.Offset);r.Position=gems.End;
 var cats=r.Chunk(0x40008000);if(r.Int()!=1||r.Int()!=0)throw new Exception("Unsupported category version");int categoryCount=r.Count(10000);for(int i=0;i<categoryCount;i++)r.Text();if(r.Position!=cats.CrcOffset)throw new Exception("Category trailing bytes");Check(b,cats);string categoryHash=Hash(b,cats.Offset,cats.End-cats.Offset);r.Position=cats.End;
 var graph=r.Chunk(0x40008000);if(r.Int()!=1||r.Int()!=0)throw new Exception("Unsupported graph version");int n=r.Count();var guids=new string[n];for(int i=0;i<n;i++)guids[i]=r.Guid();uint graphCrc=Check(b,graph,r.Position);var nodes=new List<RecordDigest>();var numbers=new List<NumericRecord>();
 for(int i=0;i<n;i++){var block=r.Chunk(0x40008000);int major=r.Int(),minor=r.Int();Check(b,block);nodes.Add(new(i,guids[i],major,minor,Hash(b,block.Offset,block.End-block.Offset),true));if(guids[i]==Range||guids[i]==Ring)numbers.Add(ReadNumber(b,block,i,guids[i],major,minor));r.Position=block.End;}
 if(r.Position!=graph.CrcOffset)throw new Exception("Graph trailing bytes");r.Position=graph.End;bool eof=r.UInt()==0x7fff&&r.Int()==4&&r.UInt()==b.Length&&r.Position==b.Length;if(!eof)throw new Exception("Bad EOF/archive length");
 return new("Independent bounded legacy CSG byte reader; no native target or geometry regeneration.",b.Length,Hash(b,0,b.Length),marker,infoVersion,build,provenance,userCount,userHash,gemCount,gemHash,categoryCount,categoryHash,n,nodes,numbers,graphCrc,eof);
 }
 static NumericRecord ReadNumber(byte[] b,Block block,int index,string guid,int major,int minor){
 if(major!=1||minor!=(guid==Range?1:2))throw new Exception("Unsupported numeric record version");var r=new Bytes(b,block.Payload+8,block.CrcOffset);string name=r.Text();uint flags=r.UInt();int layer=r.Int(),uStart=r.Position,uCount=r.UserData();string uHash=Hash(b,uStart,r.Position-uStart);int[] objects=r.Ints(),channels=r.Ints(),categories=r.Ints();if(objects.Length!=channels.Length)throw new Exception("Link count mismatch");int legacy=r.Int(),offset=r.Position;double value=r.Double(),min=r.Double(),max=r.Double();double? step=guid==Range?r.Double():null;if(r.Position!=block.CrcOffset)throw new Exception("Numeric payload incomplete");return new(index,guid,name,flags,layer,uCount,uHash,objects,channels,categories,legacy,offset,value,min,max,step);
 }
}
