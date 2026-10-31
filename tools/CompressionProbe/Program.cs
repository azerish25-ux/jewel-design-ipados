extern alias DesktopRecovered;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Reflection.Emit;
using RecoveredZlib=DesktopRecovered::GV.CounterSketch.Studio.ZipIt.ZlibStream;

string Hash(byte[] bytes)=>Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
if(args.Length==3&&args[0]=="--inspect-adler-il")
{
    var file=Path.GetFullPath(args[1]);using var stream=File.OpenRead(file);using var pe=new PEReader(stream);var metadata=pe.GetMetadataReader();
    var type=metadata.TypeDefinitions.Select(h=>metadata.GetTypeDefinition(h)).Single(t=>metadata.GetString(t.Namespace)=="GV.CounterSketch.Studio.ZipIt"&&metadata.GetString(t.Name)=="Adler");
    var methodHandle=type.GetMethods().Single(h=>metadata.GetString(metadata.GetMethodDefinition(h).Name)=="Adler32");
    var method=metadata.GetMethodDefinition(methodHandle);var body=pe.GetMethodBody(method.RelativeVirtualAddress);var il=body.GetILBytes()!;
    var codes=typeof(OpCodes).GetFields(System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.Public).Where(f=>f.FieldType==typeof(OpCode)).Select(f=>(OpCode)f.GetValue(null)!).ToDictionary(c=>(ushort)c.Value);
    var operations=new List<object>();int signedRemainders=0,unsignedRemainders=0;
    for(int offset=0;offset<il.Length;)
    {
        int start=offset;ushort value=il[offset++];if(value==0xfe)value=(ushort)(0xfe00|il[offset++]);var code=codes[value];
        int size=code.OperandType switch {OperandType.InlineNone=>0,OperandType.ShortInlineBrTarget or OperandType.ShortInlineI or OperandType.ShortInlineVar=>1,OperandType.InlineVar=>2,OperandType.InlineI8 or OperandType.InlineR=>8,OperandType.InlineSwitch=>4+4*BitConverter.ToInt32(il,offset),_=>4};
        if(offset+size>il.Length)throw new BadImageFormatException("Truncated IL operand");
        if(code==OpCodes.Rem)signedRemainders++;if(code==OpCodes.Rem_Un)unsignedRemainders++;
        operations.Add(new{offset=start,opcode=code.Name,operandHex=Convert.ToHexString(il.AsSpan(offset,size)).ToLowerInvariant()});offset+=size;
    }
    var locals=body.LocalSignature.IsNil?Array.Empty<byte>():metadata.GetBlobBytes(metadata.GetStandaloneSignature(body.LocalSignature).Signature);
    var proof=new{schemaVersion=1,scope="Static original PE method inspection only; original assembly not executed",assemblyFile=Path.GetFileName(file),assemblySha256=Hash(File.ReadAllBytes(file)),methodToken=MetadataTokens.GetToken(methodHandle).ToString("x8"),methodRva=method.RelativeVirtualAddress,ilSha256=Hash(il),ilBytes=Convert.ToHexString(il).ToLowerInvariant(),localSignatureHex=Convert.ToHexString(locals).ToLowerInvariant(),signedRemainders,unsignedRemainders,operations};
    var output=Path.GetFullPath(args[2]);Directory.CreateDirectory(Path.GetDirectoryName(output)!);File.WriteAllText(output,JsonSerializer.Serialize(proof,new JsonSerializerOptions{WriteIndented=true}));return;
}

var results=new List<object>();
string artifactFolder=args.Length==1?Path.Combine(Path.GetDirectoryName(Path.GetFullPath(args[0]))!,"packed"):null;
if(artifactFolder is not null)Directory.CreateDirectory(artifactFolder);
foreach(int length in new[]{0,1,32,1024,32768,65536,100000})
foreach(string pattern in new[]{"repeat","text","random"})
{
    var input=new byte[length];
    if(pattern=="repeat")Array.Fill(input,(byte)'x');
    else if(pattern=="text"){var phrase=Encoding.UTF8.GetBytes("CounterSketch portable compression ");for(int i=0;i<input.Length;i++)input[i]=phrase[i%phrase.Length];}
    else new Random(1701).NextBytes(input);
    byte[] packed=null;
    foreach(string stage in new[]{"recovered.compress","bcl.decode.recovered","recovered.decode.recovered","recovered.decode.bcl"})
    {
        try
        {
            byte[] actual;
            if(stage=="recovered.compress"){packed=RecoveredZlib.CompressBuffer(input);actual=packed;if(artifactFolder is not null)File.WriteAllBytes(Path.Combine(artifactFolder,length+"-"+pattern+".zlib"),packed);}
            else if(stage=="bcl.decode.recovered")
            {
                if(packed is null){results.Add(new{length,pattern,stage,status="not_run_no_recovered_output"});continue;}
                using var stream=new MemoryStream(packed);using var reader=new System.IO.Compression.ZLibStream(stream,System.IO.Compression.CompressionMode.Decompress);using var output=new MemoryStream();reader.CopyTo(output);actual=output.ToArray();
                if(!actual.SequenceEqual(input))throw new InvalidDataException("Recovered output differs after BCL decompression");
            }
            else if(stage=="recovered.decode.recovered")
            {
                if(packed is null){results.Add(new{length,pattern,stage,status="not_run_no_recovered_output"});continue;}
                actual=RecoveredZlib.UncompressBuffer(packed);if(!actual.SequenceEqual(input))throw new InvalidDataException("Recovered self round trip differs");
            }
            else
            {
                using var target=new MemoryStream();using(var writer=new System.IO.Compression.ZLibStream(target,System.IO.Compression.CompressionLevel.Optimal,true))writer.Write(input);
                actual=RecoveredZlib.UncompressBuffer(target.ToArray());if(!actual.SequenceEqual(input))throw new InvalidDataException("BCL output differs after recovered decompression");
            }
            results.Add(new{length,pattern,stage,status="observed_stage_success",inputSha256=Hash(input),outputBytes=actual.Length,outputSha256=Hash(actual),emptyRecoveredStream=stage=="recovered.compress"&&actual.Length==0});
        }
        catch(Exception error){results.Add(new{length,pattern,stage,status="failure",inputSha256=Hash(input),error=error.ToString()});}
    }
}
var report=new{schemaVersion=1,scope="Recovered managed compression versus BCL diagnostic only; no original-binary/PC/iOS compatibility claim",cases=results};
string json=JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true});
if(args.Length==1){Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(args[0]))!);File.WriteAllText(args[0],json);}else Console.WriteLine(json);
