using Metal;
namespace JewelDesign.Rendering.Candidate.iOS;

// Shared encoder for offscreen proofs and visible CAD/image composition; no readback in live use.
internal sealed class MetalImageQuadPipeline:IDisposable
{
 private readonly IMTLDevice device;
 private IMTLRenderPipelineState? opaque,blended;private IMTLDepthStencilState? noDepth;private bool disposed;
 private readonly Dictionary<(bool Filtered,int Anisotropy),IMTLSamplerState> samplers=new();
 internal int SamplerCount=>samplers.Count;
 internal MetalImageQuadPipeline(IMTLDevice device,MTLPixelFormat depthFormat,bool worldPlane=false)
 {this.device=device;try{
   using var stream=typeof(MetalImageQuadRenderer).Assembly.GetManifestResourceStream("ImageQuadMetalShader")??throw new InvalidOperationException("Image shader missing.");
   using var reader=new StreamReader(stream);using var options=new MTLCompileOptions();
   using var library=device.CreateLibrary(reader.ReadToEnd(),options,out var error)??throw new InvalidOperationException("Image shader compile failed: "+error);
   using var vertex=library.CreateFunction(worldPlane?"image_world_vertex":"image_quad_vertex")??throw new InvalidOperationException("Image vertex shader missing.");
   using var fragment=library.CreateFunction("image_quad_fragment")??throw new InvalidOperationException("Image fragment shader missing.");
   IMTLRenderPipelineState Pipeline(bool blend){
    using var d=new MTLRenderPipelineDescriptor();d.VertexFunction=vertex;d.FragmentFunction=fragment;d.DepthAttachmentPixelFormat=depthFormat;
    var a=d.ColorAttachments[0];a.PixelFormat=MTLPixelFormat.BGRA8Unorm;a.BlendingEnabled=blend;
    a.SourceRgbBlendFactor=MTLBlendFactor.SourceAlpha;a.DestinationRgbBlendFactor=MTLBlendFactor.OneMinusSourceAlpha;
    a.SourceAlphaBlendFactor=MTLBlendFactor.SourceAlpha;a.DestinationAlphaBlendFactor=MTLBlendFactor.OneMinusSourceAlpha;
    return device.CreateRenderPipelineState(d,out var failure)??throw new InvalidOperationException("Image pipeline failed: "+failure);
   }
   opaque=Pipeline(false);blended=Pipeline(true);EnsureAnisotropy(1);
   using var depth=new MTLDepthStencilDescriptor{DepthCompareFunction=MTLCompareFunction.Always,DepthWriteEnabled=false};noDepth=device.CreateDepthStencilState(depth)??throw new InvalidOperationException("Image depth state unavailable.");
 }catch{Dispose();throw;}}
 // Callers serialize mutation/encoding and defer this pipeline's disposal until all frame leases finish.
 internal void EnsureAnisotropy(int maximum)
 {
  ObjectDisposedException.ThrowIf(disposed,this);
  if(maximum<1||maximum>16)throw new ArgumentOutOfRangeException(nameof(maximum));
  if(samplers.ContainsKey((false,maximum)))return;
  IMTLSamplerState? nearest=null,linear=null;
  IMTLSamplerState Create(bool filtered){using var d=new MTLSamplerDescriptor{MinFilter=filtered?MTLSamplerMinMagFilter.Linear:MTLSamplerMinMagFilter.Nearest,MagFilter=filtered?MTLSamplerMinMagFilter.Linear:MTLSamplerMinMagFilter.Nearest,SAddressMode=MTLSamplerAddressMode.ClampToEdge,TAddressMode=MTLSamplerAddressMode.ClampToEdge,NormalizedCoordinates=true,MaxAnisotropy=(nuint)maximum};return device.CreateSamplerState(d)??throw new InvalidOperationException("Image sampler unavailable.");}
  try{nearest=Create(false);linear=Create(true);samplers.Add((false,maximum),nearest);samplers.Add((true,maximum),linear);}catch{nearest?.Dispose();linear?.Dispose();throw;}
 }
 internal void Encode(IMTLRenderCommandEncoder encoder,IMTLTexture image,IMTLBuffer quad,bool filtered,bool blend,int maximumAnisotropy=1)
 {ObjectDisposedException.ThrowIf(disposed,this);EnsureAnisotropy(maximumAnisotropy);encoder.SetRenderPipelineState(blend?blended!:opaque!);encoder.SetDepthStencilState(noDepth!);encoder.SetCullMode(MTLCullMode.None);encoder.SetVertexBuffer(quad,0,0);encoder.SetFragmentTexture(image,0);encoder.SetFragmentSamplerState(samplers[(filtered,maximumAnisotropy)],0);encoder.DrawPrimitives(MTLPrimitiveType.Triangle,0,6);}
 public void Dispose(){if(disposed)return;disposed=true;noDepth?.Dispose();foreach(var sampler in samplers.Values)sampler.Dispose();samplers.Clear();blended?.Dispose();opaque?.Dispose();}
}
