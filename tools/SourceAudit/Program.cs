using System.Text.Json;
using System.Security.Cryptography;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var workspace = Path.GetFullPath(args[0]); var output = Path.GetFullPath(args[1]); Directory.CreateDirectory(output);
var sources = new[] { ("desktop", "recovery/canonical/desktop"), ("cad", "recovery/canonical/cad") };
var fileRows = new List<object>(); var typeRows = new List<object>(); var memberRows = new List<object>();
foreach (var (component, relative) in sources) foreach (var file in Directory.EnumerateFiles(Path.Combine(workspace, relative), "*.cs", SearchOption.AllDirectories))
{
    var text = File.ReadAllText(file); var tree = CSharpSyntaxTree.ParseText(text); var root = tree.GetRoot(); var loc = Path.GetRelativePath(workspace, file).Replace('\\', '/');
    int Line(SyntaxNode n) => n.GetLocation().GetLineSpan().StartLinePosition.Line + 1; int End(SyntaxNode n) => n.GetLocation().GetLineSpan().EndLinePosition.Line + 1;
    string TypeName(BaseTypeDeclarationSyntax t) { var ns = t.Ancestors().OfType<BaseNamespaceDeclarationSyntax>().FirstOrDefault()?.Name.ToString() ?? ""; var names = t.Ancestors().OfType<BaseTypeDeclarationSyntax>().Reverse().Select(x => x.Identifier.Text).Append(t.Identifier.Text); return string.Join(".", new[] { ns }.Where(x => x.Length > 0).Concat(names)); }
    var errors = tree.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).Select(d => new { code = d.Id, line = d.Location.GetLineSpan().StartLinePosition.Line + 1, message = d.GetMessage() }).ToArray();
    fileRows.Add(new { component, path = loc, sha256 = Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(text))), lines = text.Split('\n').Length, nonblank = text.Split('\n').Count(l => !string.IsNullOrWhiteSpace(l)), syntaxErrors = errors, types = root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>().Count() });
    foreach (var t in root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
    {
        typeRows.Add(new { component, path = loc, line = Line(t), end = End(t), name = TypeName(t), kind = t.Kind().ToString(), bases = t.BaseList?.Types.Select(x => x.Type.ToString()).ToArray() ?? Array.Empty<string>(), attributes = t.AttributeLists.SelectMany(a => a.Attributes).Select(a => a.ToString()).ToArray(), legacy = loc.Contains("Obsolete"), guids = t.DescendantNodes().OfType<LiteralExpressionSyntax>().Select(l => l.Token.ValueText).Where(s => Guid.TryParse(s, out _)).Distinct().ToArray() });
    }
    foreach (var m in root.DescendantNodes().OfType<MemberDeclarationSyntax>().Where(m => m is BaseMethodDeclarationSyntax or PropertyDeclarationSyntax or FieldDeclarationSyntax or EventFieldDeclarationSyntax))
    {
        var owner = m.Ancestors().OfType<BaseTypeDeclarationSyntax>().FirstOrDefault(); if (owner == null) continue;
        string name = m switch { MethodDeclarationSyntax x => x.Identifier.Text, ConstructorDeclarationSyntax x => ".ctor", PropertyDeclarationSyntax x => x.Identifier.Text, FieldDeclarationSyntax x => string.Join(",", x.Declaration.Variables.Select(v => v.Identifier.Text)), EventFieldDeclarationSyntax x => string.Join(",", x.Declaration.Variables.Select(v => v.Identifier.Text)), _ => m.Kind().ToString() };
        var strings = m.DescendantNodes().OfType<LiteralExpressionSyntax>().Where(l => l.IsKind(SyntaxKind.StringLiteralExpression)).Select(l => l.Token.ValueText).Where(s => s.Length < 160).Distinct().ToArray();
        // Store only interface/format/control literals; never export credential-like field values.
        strings = strings.Where(s => s.StartsWith('.') || s.Contains(".csg") || s.Contains(".3dm") || s.Contains("Version") || s.Contains("Guid") || s.Contains("Graph") || s.Contains("Table") || s.Contains("Document") || s.Contains("Material") || s.Contains("Iteration") || s.Contains("Socket")).ToArray();
        var ioOperations = (name.StartsWith("Read") || name.StartsWith("Write") || name == "MajorMinorVersion") ? m.DescendantNodes().OfType<InvocationExpressionSyntax>().Where(i => i.Expression.ToString().Contains("Read") || i.Expression.ToString().Contains("Write")).Select(i => new { line = Line(i), call = i.ToString().Length < 400 ? i.ToString() : i.Expression.ToString() + "[arguments omitted]" }).ToArray() : Array.Empty<object>();
        var versionAssignments = name == "MajorMinorVersion" ? m.DescendantNodes().OfType<AssignmentExpressionSyntax>().Select(a => a.ToString()).ToArray() : Array.Empty<string>();
        memberRows.Add(new { component, path = loc, line = Line(m), end = End(m), type = TypeName(owner), name, kind = m.Kind().ToString(), signature = m switch { MethodDeclarationSyntax x => x.ReturnType + " " + name + x.ParameterList, ConstructorDeclarationSyntax x => name + x.ParameterList, PropertyDeclarationSyntax x => x.Type + " " + name, FieldDeclarationSyntax x => x.Declaration.Type + " " + name, EventFieldDeclarationSyntax x => x.Declaration.Type + " " + name, _ => name }, attributes = m.AttributeLists.SelectMany(a => a.Attributes).Select(a => a.ToString()).ToArray(), calls = m.DescendantNodes().OfType<InvocationExpressionSyntax>().Select(i => i.Expression.ToString()).Distinct().ToArray(), literals = strings, statements = m.DescendantNodes().OfType<StatementSyntax>().Count(s => s is not BlockSyntax), ioOperations, versionAssignments });
    }
}
var opts = new JsonSerializerOptions { WriteIndented = true }; File.WriteAllText(Path.Combine(output, "source-files.json"), JsonSerializer.Serialize(fileRows, opts)); File.WriteAllText(Path.Combine(output, "source-types.json"), JsonSerializer.Serialize(typeRows, opts)); File.WriteAllText(Path.Combine(output, "source-members.json"), JsonSerializer.Serialize(memberRows, opts)); Console.WriteLine($"Parsed {fileRows.Count} source files, {typeRows.Count} types, {memberRows.Count} members.");
