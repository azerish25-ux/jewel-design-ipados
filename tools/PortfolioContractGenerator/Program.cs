using System.Text;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var root=Path.GetFullPath(args[0]);
var output=Path.Combine(root,"tests/Port/Documents.Contracts/Recovered/PortfolioSchema.cs");
var folder="recovery/canonical/desktop/GV.CounterSketch.Studio.Model.Objects/";
var code=new StringBuilder("// Generated declared XML property shapes only; no desktop runtime or inherited behavior.\n#nullable disable\nusing System;\nusing System.Collections.Generic;\nusing System.Collections.ObjectModel;\nnamespace LegacyPortfolioSchema;\n");
var counts=new Dictionary<string,int>();
var supported=new HashSet<string>{"string","int","long","bool","Guid","int?","ProjectType","ObservableCollection<Design>","Collection<Design>","List<int>"};
foreach(var name in new[]{"Project","Design"})
{
 var bytes=File.ReadAllBytes(Path.Combine(root,folder+name+".cs"));
 var type=CSharpSyntaxTree.ParseText(Encoding.UTF8.GetString(bytes)).GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single(c=>c.Identifier.Text==name);
 code.AppendLine("// Source SHA256 "+Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant());
 code.AppendLine("public class "+name+"\n{");int count=0;
 foreach(var p in type.Members.OfType<PropertyDeclarationSyntax>())
 {
  if(!p.Modifiers.Any(SyntaxKind.PublicKeyword)||p.Modifiers.Any(SyntaxKind.StaticKeyword))continue;
  var attrs=p.AttributeLists.SelectMany(a=>a.Attributes).Select(a=>a.Name.ToString()).ToArray();
  if(attrs.Any(a=>a.EndsWith("XmlIgnore")||a.EndsWith("XmlIgnoreAttribute")))continue;
  var accessors=p.AccessorList?.Accessors;
  if(accessors is null||!accessors.Value.Any(a=>a.IsKind(SyntaxKind.GetAccessorDeclaration))||!accessors.Value.Any(a=>a.IsKind(SyntaxKind.SetAccessorDeclaration)&&!a.Modifiers.Any(SyntaxKind.PrivateKeyword)&&!a.Modifiers.Any(SyntaxKind.ProtectedKeyword)))continue;
  if(attrs.Any(a=>a.Contains("Xml"))||!supported.Contains(p.Type.ToString()))throw new Exception("Unmapped XML property "+p.Identifier.Text);
  code.AppendLine(" public "+p.Type+" "+p.Identifier.Text+" { get; set; }");count++;
 }
 code.AppendLine("}");counts[name]=count;
}
var enumBytes=File.ReadAllBytes(Path.Combine(root,folder+"ProjectType.cs"));
var enumNode=CSharpSyntaxTree.ParseText(Encoding.UTF8.GetString(enumBytes)).GetRoot().DescendantNodes().OfType<EnumDeclarationSyntax>().Single();
code.AppendLine("// Source SHA256 "+Convert.ToHexString(SHA256.HashData(enumBytes)).ToLowerInvariant());code.AppendLine(enumNode.WithoutTrivia().ToString());
string text=code.ToString().Replace("\r\n","\n");
if(args.Contains("--verify")){if(!File.Exists(output)||File.ReadAllText(output).Replace("\r\n","\n")!=text)throw new Exception("Portfolio XML oracle differs from source; regenerate and review");}
else{Directory.CreateDirectory(Path.GetDirectoryName(output)!);File.WriteAllText(output,text);}
Console.WriteLine(JsonSerializer.Serialize(new{Counts=counts,Output=Path.GetRelativePath(root,output),Scope="Declared XML shapes; no original PC runtime behavior"}));
