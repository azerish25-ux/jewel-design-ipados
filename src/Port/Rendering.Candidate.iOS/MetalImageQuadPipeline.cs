using Metal;
namespace JewelDesign.Rendering.Candidate.iOS;

// Shared encoder for offscreen proofs and visible CAD/image composition; no readback in live use.
internal sealed class MetalImageQuadPipeline:IDisposable
{
 private IMTLRenderPipelineState? opaque,blended;private IMTLSamplerState? nearest,linear;private IMTLDepthStencilState? noDepth;private bool disposed;
 internal MetalImageQuadPipeline(IMTLDevice device,MTLPixelFormat depthFormat)
 {try{
   using var stream=typeof(MetalImageQuadRenderer).Assembly.GetManifestResourceStream("ImageQuadMetalShader")??throw new InvalidOperationException("Image shader missing.");
   using var reader=new StreamReader(stream);using var options=new MTLCompileOptions();
   using var library=device.CreateLibrary(reader.ReadToEnd(),options,out var error)??throw new InvalidOperationException("Image shader compile failed: "+error);
   using var vertex=library.CreateFunction("image_quad_vertex")??throw new InvalidOperationException("Image vertex shader missing.");
   using var fragment=library.CreateFunction("image_quad_fragment")??throw new InvalidOperationException("Image fragment shader missing.");
   IMTLRenderPipelineState Pipeline(bool blend){
    using var d=new MTLRenderPipelineDescriptor();d.VertexFunction=vertex;d.FragmentFunction=fragment;d.DepthAttachmentPixelFormat=depthFormat;
    var a=d.ColorAttachments[0];a.PixelFormat=MTLPixelFormat.BGRA8Unorm;a.BlendingEnabled=blend;
    a.SourceRgbBlendFactor=MTLBlendFactor.SourceAlpha;a.DestinationRgbBlendFactor=MTLBlendFactor.OneMinusSourceAlpha;
    a.SourceAlphaBlendFactor=MTLBlendFactor.SourceAlpha;a.DestinationAlphaBlendFactor=MTLBlendFactor.OneMinusSourceAlpha;
    return device.CreateRenderPipelineState(d,out var failure)??throw new InvalidOperationException("Image pipeline failed: "+failure);
   }
   opaque=Pipeline(false);blended=Pipeline(true);
   IMTLSamplerState Sampler(MTLSamplerMinMagFilter filter){
    using var d=new MTLSamplerDescriptor{MinFilter=filter,MagFilter=filter,SAddressMode=MTLSamplerAddressMode.ClampToEdge,TAddressMode=MTLSamplerAddressMode.ClampToEdge,NormalizedCoordinates=true};
    return device.CreateSamplerState(d)??throw new InvalidOperationException("Image sampler unavailable.");
   }
   nearest=Sampler(MTLSamplerMinMagFilter.Nearest);linear=Sampler(MTLSamplerMinMagFilter.Linear);
 using var depth=new MTLDepthStencilDescriptor{DepthCompareFunction=MTLCompareFunction.Always,DepthWriteEnabled=false};noDepth=device.CreateDepthStencilState(depth)??throw new InvalidOperationException("Image depth state unavailable.");
 }catch{Dispose();throw;}}
 internal void Encode(IMTLRenderCommandEncoder encoder,IMTLTexture image,IMTLBuffer quad,bool filtered,bool blend)
 {ObjectDisposedException.ThrowIf(disposed,this);encoder.SetRenderPipelineState(blend?blended!:opaque!);encoder.SetDepthStencilState(noDepth!);encoder.SetCullMode(MTLCullMode.None);encoder.SetVertexBuffer(quad,0,0);encoder.SetFragmentTexture(image,0);encoder.SetFragmentSamplerState(filtered?linear!:nearest!,0);encoder.DrawPrimitives(MTLPrimitiveType.Triangle,0,6);}
 public void Dispose(){if(disposed)return;disposed=true;noDepth?.Dispose();linear?.Dispose();nearest?.Dispose();blended?.Dispose();opaque?.Dispose();}
}
