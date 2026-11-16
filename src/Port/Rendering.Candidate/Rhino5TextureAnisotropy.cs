namespace JewelDesign.Rendering.Candidate;

/// <summary>Original texture-create setting normalization and bind-time single-float policy.
/// Source extension absence is distinct from an explicit parameter of1. Default is a fresh sampler.</summary>
public readonly record struct Rhino5TextureAnisotropy
{
 public bool ParameterSet{get;}
 public bool QueriesMaximum{get;}
 private readonly float value;
 public float Value=>ParameterSet?value:1f;
 private Rhino5TextureAnisotropy(bool parameterSet,bool queriesMaximum,float value){ParameterSet=parameterSet;QueriesMaximum=queriesMaximum;this.value=value;}
 public static Rhino5TextureAnisotropy ForApplicationSetting(int setting,bool extensionSupported,float sourceMaximum)
  =>ForTextureMode(setting is 1 or 2 or 3?setting:0,extensionSupported,sourceMaximum);
 public static Rhino5TextureAnisotropy ForTextureMode(int mode,bool extensionSupported,float sourceMaximum)
 {
  if(!extensionSupported)return default;
  if(mode==0)return new(true,false,1f);
  return new(true,true,sourceMaximum/(mode==1?4f:mode==2?2f:1f));
 }
 /// <summary>Exact descriptor representability only; no claim that Metal and a legacy GL driver filter identically.</summary>
 public int ToMetalMaximum()
 {
  float v=Value;
  if(!float.IsFinite(v)||v<1f||v>16f||v!=MathF.Truncate(v))throw new NotSupportedException("Source anisotropy cannot be represented exactly by Metal's integer1..16 sampler limit.");
  return (int)v;
 }
}
