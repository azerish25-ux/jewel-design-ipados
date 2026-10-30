using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Json;
using System.Reflection.Metadata.Ecma335;
using System.Buffers.Binary;

static IEnumerable<string> Files(string p) => Directory.EnumerateFiles(p, "*", SearchOption.AllDirectories).Where(p => new[] { ".exe", ".dll", ".rhp", ".pyd" }.Contains(Path.GetExtension(p).ToLowerInvariant()));
var mode = args[0]; var target = Path.GetFullPath(args[1]); var output = Path.GetFullPath(args[2]);
if (mode == "restore-sealed")
{
    using var originalStream = File.OpenRead(target); using var originalPe = new PEReader(originalStream); var omd = originalPe.GetMetadataReader();
    var sealedTypes = omd.TypeDefinitions.Select(h => omd.GetTypeDefinition(h)).Where(t => (t.Attributes & TypeAttributes.Sealed) != 0).Select(t => omd.GetString(t.Namespace) + "." + omd.GetString(t.Name)).ToHashSet();
    var bytes = File.ReadAllBytes(output); using var candidateStream = new MemoryStream(bytes, false); using var candidatePe = new PEReader(candidateStream); var md = candidatePe.GetMetadataReader(); var rows = new List<object>();
    foreach (var h in md.TypeDefinitions) { var t = md.GetTypeDefinition(h); var name = md.GetString(t.Namespace) + "." + md.GetString(t.Name); if (!sealedTypes.Contains(name) || (t.Attributes & TypeAttributes.Sealed) != 0) continue; int offset = candidatePe.PEHeaders.MetadataStartOffset + md.GetTableMetadataOffset(TableIndex.TypeDef) + (MetadataTokens.GetRowNumber(h) - 1) * md.GetTableRowSize(TableIndex.TypeDef); var before = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(offset, 4)); if (before != (uint)t.Attributes) throw new Exception("Metadata offset check failed"); var after = before | (uint)TypeAttributes.Sealed; BinaryPrimitives.WriteUInt32LittleEndian(bytes.AsSpan(offset, 4), after); rows.Add(new { name, offset, before, after }); }
    File.WriteAllBytes(output, bytes); File.WriteAllText(output + ".sealed-restoration.json", JsonSerializer.Serialize(rows, new JsonSerializerOptions { WriteIndented = true })); Console.WriteLine($"Restored original sealed metadata for {rows.Count} types; method bodies unchanged by this step.");
}
else if (mode == "msi")
{
    var data = MsiAudit.Read(target); File.WriteAllText(output, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true })); Console.WriteLine("Read installer tables without executing installation: " + output);
}
else if (mode == "xaml")
{
    var rows = new List<object>(); int errors = 0; foreach (var f in Directory.EnumerateFiles(target, "*.xaml", SearchOption.AllDirectories)) try { var x = System.Xml.Linq.XDocument.Load(f); rows.Add(new { path = Path.GetRelativePath(target, f), root = x.Root!.Name.ToString(), validXml = true }); } catch (Exception e) { errors++; rows.Add(new { path = f, error = e.Message, validXml = false }); }
    File.WriteAllText(output, JsonSerializer.Serialize(rows, new JsonSerializerOptions { WriteIndented = true })); Console.WriteLine($"XAML XML validation: {rows.Count} files; {errors} parse failures. Not a WPF runtime validation."); Environment.ExitCode = errors == 0 ? 0 : 1;
}
else if (mode == "inventory")
{
    var records = new List<object>(); foreach (var f in Files(target)) try
        {
            using var stream = File.OpenRead(f); using var pe = new PEReader(stream); if (!pe.HasMetadata) continue; var md = pe.GetMetadataReader(); if (!md.IsAssembly) continue; var asm = md.GetAssemblyDefinition();
            var refs = md.AssemblyReferences.Select(h => { var r = md.GetAssemblyReference(h); return new { name = md.GetString(r.Name), version = r.Version.ToString(), token = Convert.ToHexString(md.GetBlobBytes(r.PublicKeyOrToken)) }; }).ToArray();
            var imports = new List<object>(); int importIssues = 0; foreach (var h in md.MethodDefinitions) { var m = md.GetMethodDefinition(h); if ((m.Attributes & MethodAttributes.PinvokeImpl) == 0) continue; try { var i = m.GetImport(); if (i.Module.IsNil) { importIssues++; continue; } imports.Add(new { method = md.GetString(m.Name), module = md.GetString(md.GetModuleReference(i.Module).Name), entry = md.GetString(i.Name) }); } catch (BadImageFormatException) { importIssues++; } }
            var res = md.ManifestResources.Select(h => { var r = md.GetManifestResource(h); return new { name = md.GetString(r.Name), offset = r.Offset, embedded = r.Implementation.IsNil }; }).ToArray();
            records.Add(new { path = Path.GetRelativePath(target, f), name = md.GetString(asm.Name), version = asm.Version.ToString(), publicKey = Convert.ToHexString(md.GetBlobBytes(asm.PublicKey)), corFlags = pe.PEHeaders.CorHeader!.Flags.ToString(), types = md.TypeDefinitions.Count, methods = md.MethodDefinitions.Count, refs, imports, importIssues, resources = res });
        }
        catch (Exception e) { Console.Error.WriteLine(Path.GetFileName(f) + ": " + e.GetType().Name); }
    File.WriteAllText(output, JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true })); Console.WriteLine($"Inventoried {records.Count} managed assemblies -> {output}");
}
else if (mode == "resources")
{
    Directory.CreateDirectory(output); using var stream = File.OpenRead(target); using var pe = new PEReader(stream); var md = pe.GetMetadataReader(); var data = pe.GetSectionData(pe.PEHeaders.CorHeader!.ResourcesDirectory.RelativeVirtualAddress); var records = new List<object>();
    foreach (var h in md.ManifestResources) { var r = md.GetManifestResource(h); if (!r.Implementation.IsNil) continue; var name = md.GetString(r.Name); var reader = data.GetReader((int)r.Offset, data.Length - (int)r.Offset); var len = reader.ReadInt32(); var bytes = reader.ReadBytes(len); var file = $"resource-{records.Count:D3}.bin"; File.WriteAllBytes(Path.Combine(output, file), bytes); records.Add(new { name, file, bytes = len, sha256 = Convert.ToHexString(SHA256.HashData(bytes)) }); }
    File.WriteAllText(Path.Combine(output, "manifest.json"), JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true })); Console.WriteLine($"Extracted {records.Count} manifest resources without loading target code.");
}
