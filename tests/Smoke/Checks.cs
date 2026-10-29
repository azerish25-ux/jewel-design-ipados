using GemvisionGeometry.Helpers;
using System.Globalization;
namespace ReuseSmoke;
public static class Checks {
public static string Run(){ int cases=0;using var sr=new StreamReader(typeof(Checks).Assembly.GetManifestResourceStream("golden.txt")!); foreach(var line in sr.ReadToEnd().Split('\n')){if(!line.Contains('|'))continue;var f=line.Trim().Split('|');int n=int.Parse(f[0]);double u=double.Parse(f[1],CultureInfo.InvariantCulture);var expected=f[2].Split(',').Select(v=>double.Parse(v,CultureInfo.InvariantCulture)).ToArray();GNurbsMath.AllBernstein(n,u,out var actual);if(actual.Length!=expected.Length)throw new Exception("Length mismatch");for(int i=0;i<actual.Length;i++)if(Math.Abs(actual[i]-expected[i])>1e-14)throw new Exception("Original binary golden mismatch");if(Math.Abs(actual.Sum()-1)>1e-12)throw new Exception("Basis sum");cases++;}if(cases!=16)throw new Exception("Missing golden cases");return $"PASS {cases} original-binary golden Bernstein cases";}
}