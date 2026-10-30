using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text.Json;
using System.Security.Cryptography;
var source = Path.GetFullPath(args[0]); var output = Path.GetFullPath(args[1]); Directory.CreateDirectory(output);
var refs = Directory.GetFiles(args[2], "*.dll").Select(f => MetadataReference.CreateFromFile(f)).ToArray();
var all = Directory.GetFiles(source, "*.cs", SearchOption.AllDirectories).Where(f => !f.EndsWith("AssemblyInfo.cs")).Order().ToArray();
var trees = all.Select(f => CSharpSyntaxTree.ParseText(File.ReadAllText(f), path: f)).ToList(); var rounds = new List<object>();
while (true)
{
    var c = CSharpCompilation.Create("Recovered", trees, refs, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true)); var errors = c.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToArray(); if (errors.Length == 0) { using var stream = File.Create(Path.Combine(output, "Recovered.dll")); var e = c.Emit(stream); if (!e.Success) throw new Exception(string.Join("\n", e.Diagnostics)); break; }
    var rejected = errors.Where(d => d.Location.SourceTree != null).Select(d => d.Location.SourceTree).ToHashSet(); if (rejected.Count == 0) throw new Exception(string.Join("\n", errors.Select(e => e.ToString()))); rounds.Add(new { round = rounds.Count + 1, filesRemoved = rejected.Count, errors = errors.Select(d => new { id = d.Id, file = Path.GetRelativePath(source, d.Location.SourceTree?.FilePath ?? source), line = d.Location.GetLineSpan().StartLinePosition.Line + 1, message = d.GetMessage() }).ToArray() }); trees.RemoveAll(t => rejected.Contains(t));
}
object Describe(string f) { var s = File.ReadAllText(f); return new { path = Path.GetRelativePath(source, f).Replace('\\', '/'), sha256 = Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(s))), lines = s.Split('\n').Length, nonblank = s.Split('\n').Count(l => !string.IsNullOrWhiteSpace(l)) }; }
File.WriteAllText(Path.Combine(output, "result.json"), JsonSerializer.Serialize(new { method = "Conservative whole-file closed set against .NET BCL only; no stubs or original Windows libraries", inputFiles = all.Length, retained = trees.Select(t => Describe(t.FilePath)), rounds }, new JsonSerializerOptions { WriteIndented = true })); Console.WriteLine($"{source}: {all.Length} files -> {trees.Count} retained, {rounds.Count} elimination rounds");
