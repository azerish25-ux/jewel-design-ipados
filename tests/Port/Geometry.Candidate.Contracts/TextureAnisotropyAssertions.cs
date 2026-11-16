using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class TextureAnisotropyAssertions
{
 public static string Run(){int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}void Reject(float value){checks++;try{Rhino5TextureAnisotropy.ForTextureMode(3,true,value).ToMetalMaximum();}catch(NotSupportedException){return;}throw new Exception("Unrepresentable anisotropy accepted");}
  foreach(int mode in new[]{int.MinValue,-1,0,1,2,3,4,int.MaxValue}){var off=Rhino5TextureAnisotropy.ForTextureMode(mode,false,float.NaN);Check(!off.ParameterSet&&!off.QueriesMaximum&&off.Value==1&&off.ToMetalMaximum()==1,"Extension absent ignores mode/maximum");}
  Check(default(Rhino5TextureAnisotropy).ToMetalMaximum()==1,"Default sampler has effective maximum1");
  var zero=Rhino5TextureAnisotropy.ForTextureMode(0,true,float.NaN);Check(zero.ParameterSet&&!zero.QueriesMaximum&&zero.Value==1,"Mode0 does not query maximum");
  foreach(var x in new[]{(1,4f),(2,8f),(3,16f),(-1,16f),(4,16f)}){var p=Rhino5TextureAnisotropy.ForTextureMode(x.Item1,true,16);Check(p.ParameterSet&&p.QueriesMaximum&&p.Value==x.Item2&&p.ToMetalMaximum()==(int)x.Item2,"Original divss policy");}
  foreach(int setting in new[]{int.MinValue,-1,0,4,int.MaxValue})Check(Rhino5TextureAnisotropy.ForApplicationSetting(setting,true,16).Value==1,"Application unknown setting coerces0; raw texture mode differs");
  for(int i=1;i<=16;i++)Check(Rhino5TextureAnisotropy.ForTextureMode(3,true,i).ToMetalMaximum()==i,"Representable Metal maximum");
  Check(Rhino5TextureAnisotropy.ForTextureMode(1,true,6).Value==1.5f,"Fraction retained without clamp/round");
  foreach(float value in new[]{0f,-1f,.5f,1.5f,16.5f,17f,float.NaN,float.PositiveInfinity,float.NegativeInfinity,float.MaxValue})Reject(value);
  Check(float.IsNaN(Rhino5TextureAnisotropy.ForTextureMode(1,true,float.NaN).Value),"Raw NaN source result retained; backend rejects separately");
  return "PASS original texture anisotropy:"+checks+" source mode/extension/division and explicit Metal representability cases; original-driver filtering/cache/PC/physical parity separate.";
 }
}
