using System.Runtime.InteropServices;
using Foundation;
using Metal;
using JewelDesign.Rendering.Candidate;
namespace JewelDesign.Rendering.Candidate.iOS;
public readonly record struct MetalImageQuadStats(int InFlight,int LiveTextures,int LiveTargets,int LiveVertexBuffers,bool IsDisposed,bool PipelineReleased,string? LastError);

/// <summary>Real offscreen image-quad GPU component with three retained frame leases.
/// Input MUST be straight RGBA8/sRGB values, top row first. Does not unpremultiply decoder output,
/// implement source image caches/SetView, or claim visible/legacy codec parity.</summary>
public sealed class MetalImageQuadRenderer:IDisposable
{
 private readonly object sync=new();
 private readonly IMTLDevice device;
 private readonly CadFrameGate<ImageTexture> frames=new();
 private IMTLCommandQueue? queue;private IMTLRenderPipelineState? opaque,blended;private IMTLSamplerState? nearest,linear;
 private bool disposed,released;private int targets,buffers;private Exception? lastError;
 public MetalImageQuadStats Stats{get{lock(sync){var s=frames.State;return new(s.InFlight,s.LiveResources,targets,buffers,disposed,released,lastError?.ToString());}}}
 public MetalImageQuadRenderer()
 {
  device=MTLDevice.SystemDefault??throw new NotSupportedException("Metal device unavailable.");
  try{
   queue=device.CreateCommandQueue()??throw new InvalidOperationException("Image command queue unavailable.");
   using var stream=typeof(MetalImageQuadRenderer).Assembly.GetManifestResourceStream("ImageQuadMetalShader")??throw new InvalidOperationException("Image shader missing.");
   using var reader=new StreamReader(stream);using var options=new MTLCompileOptions();
   using var library=device.CreateLibrary(reader.ReadToEnd(),options,out var error)??throw new InvalidOperationException("Image shader compile failed: "+error);
   using var vertex=library.CreateFunction("image_quad_vertex")??throw new InvalidOperationException("Image vertex shader missing.");
   using var fragment=library.CreateFunction("image_quad_fragment")??throw new InvalidOperationException("Image fragment shader missing.");
   IMTLRenderPipelineState Pipeline(bool blend){
    using var d=new MTLRenderPipelineDescriptor();d.VertexFunction=vertex;d.FragmentFunction=fragment;
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
  }catch{Dispose();throw;}
 }
 public void SetImage(int width,int height,ReadOnlySpan<byte> straightRgba)
 {
  lock(sync){
   ObjectDisposedException.ThrowIf(disposed,this);
   int size=new LocalImageLimits().ValidateDimensions(width,height);if(straightRgba.Length!=size)throw new ArgumentException("Straight RGBA storage length differs from image dimensions.",nameof(straightRgba));
   using var d=MTLTextureDescriptor.CreateTexture2DDescriptor(MTLPixelFormat.RGBA8Unorm,(nuint)width,(nuint)height,false);
   d.StorageMode=MTLStorageMode.Shared;d.Usage=MTLTextureUsage.ShaderRead;
   IMTLTexture? texture=device.CreateTexture(d)??throw new InvalidOperationException("Image texture allocation failed.");
   try{
    byte[] bytes=straightRgba.ToArray();var pin=GCHandle.Alloc(bytes,GCHandleType.Pinned);
    try{texture.ReplaceRegion(MTLRegion.Create2D(0,0,(nuint)width,(nuint)height),0,pin.AddrOfPinnedObject(),(nuint)(width*4));}finally{pin.Free();}
    var resource=new ImageTexture(texture);texture=null;
    try{frames.ReplaceResource(resource);}catch{resource.Dispose();throw;}
   }finally{texture?.Dispose();}
  }
 }
 /// <summary>Records an actual GPU command without committing it. Quad corners use source order:
 /// (width,0),(width,height),(0,height),(0,0), each supplied as homogeneous clip xyzw.
 /// Empty quad covers the target. Prepared frames own resource leases; dispose uncommitted frames.</summary>
 public ImageFrame PrepareFrame(int width,int height,bool filtered=false,bool blend=false,MTLClearColor? clear=null,ReadOnlySpan<float> quad=default)
 {
  lock(sync){
   ObjectDisposedException.ThrowIf(disposed,this);ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
   new LocalImageLimits().ValidateDimensions(width,height);
   if(!quad.IsEmpty&&quad.Length!=16)throw new ArgumentException("Quad requires16 finite clip coordinates.",nameof(quad));
   float[] points=quad.IsEmpty?[1,-1,0,1,1,1,0,1,-1,1,0,1,-1,-1,0,1]:quad.ToArray();
   if(points.Any(x=>!float.IsFinite(x)))throw new ArgumentException("Quad coordinates must be finite.",nameof(quad));
   frames.Invalidate();var lease=frames.TryBeginFrame()??throw new InvalidOperationException("No image available or three image frames are already pending.");
   IMTLTexture? target=null;IMTLBuffer? vertices=null;IMTLCommandBuffer? command=null;ImageFrame? frame=null;
   try{
    using var description=MTLTextureDescriptor.CreateTexture2DDescriptor(MTLPixelFormat.BGRA8Unorm,(nuint)width,(nuint)height,false);description.StorageMode=MTLStorageMode.Shared;description.Usage=MTLTextureUsage.RenderTarget;
    target=device.CreateTexture(description)??throw new InvalidOperationException("Image render target allocation failed.");
    vertices=device.CreateBuffer(points,MTLResourceOptions.StorageModeShared)??throw new InvalidOperationException("Image quad upload failed.");
    command=queue!.CommandBuffer()??throw new InvalidOperationException("Image command buffer unavailable.");
    if(!command.RetainedReferences)throw new InvalidOperationException("Image commands must retain native references.");
    using var pass=MTLRenderPassDescriptor.CreateRenderPassDescriptor();pass.ColorAttachments[0].Texture=target;pass.ColorAttachments[0].LoadAction=MTLLoadAction.Clear;pass.ColorAttachments[0].StoreAction=MTLStoreAction.Store;pass.ColorAttachments[0].ClearColor=clear??new MTLClearColor(0,0,0,0);
    using(var encoder=command.CreateRenderCommandEncoder(pass)??throw new InvalidOperationException("Image encoder unavailable.")){
     encoder.SetRenderPipelineState(blend?blended!:opaque!);encoder.SetCullMode(MTLCullMode.None);
     encoder.SetVertexBuffer(vertices,0,0);encoder.SetFragmentTexture(lease.Resource.Texture,0);encoder.SetFragmentSamplerState(filtered?linear!:nearest!,0);
     encoder.DrawPrimitives(MTLPrimitiveType.Triangle,0,6);encoder.EndEncoding();
    }
    frame=new ImageFrame(this,lease,target,vertices,command,width,height);targets++;buffers++;
    var submitted=frame;command.AddCompletedHandler(value=>Complete(submitted,value));return frame;
   }catch{if(frame is not null)lastError??=frame.Release(null);else{command?.Dispose();vertices?.Dispose();target?.Dispose();lease.Dispose();}throw;}
  }
 }
 private void Complete(ImageFrame frame,IMTLCommandBuffer command)
 {
  byte[]? bytes=null;Exception? error=null;
  lock(sync){
   try{
    if(command.Status!=MTLCommandBufferStatus.Completed)throw new InvalidOperationException("Image GPU command failed: "+command.Status+" "+command.Error);
    bytes=new byte[checked(frame.Width*frame.Height*4)];var pin=GCHandle.Alloc(bytes,GCHandleType.Pinned);
    try{frame.Target.GetBytes(pin.AddrOfPinnedObject(),(nuint)(frame.Width*4),MTLRegion.Create2D(0,0,(nuint)frame.Width,(nuint)frame.Height),0);}finally{pin.Free();}
   }catch(Exception failure){error=failure;}
   error=frame.Release(error);lastError??=error;ReleaseIfFinished();error??=lastError;
  }
  if(error is null)frame.Completion.TrySetResult(bytes!);else frame.Completion.TrySetException(error);
 }
 private void ReleaseIfFinished(){if(disposed&&frames.State.InFlight==0&&!released){released=true;foreach(var value in new IDisposable?[]{linear,nearest,blended,opaque,queue})try{value?.Dispose();}catch(Exception failure){lastError??=failure;}}}
 public void Dispose(){lock(sync){if(disposed)return;disposed=true;frames.Dispose();ReleaseIfFinished();}}
 internal sealed class ImageTexture(IMTLTexture texture):IDisposable
 {internal readonly IMTLTexture Texture=texture;public void Dispose()=>Texture.Dispose();}
 public sealed class ImageFrame:IDisposable
 {
  private readonly MetalImageQuadRenderer owner;private readonly CadFrameLease<ImageTexture> lease;private readonly IMTLBuffer vertices;
  private readonly IMTLCommandBuffer command;private int state;private bool freed;
  internal readonly IMTLTexture Target;internal readonly int Width,Height;
  internal readonly TaskCompletionSource<byte[]> Completion=new(TaskCreationOptions.RunContinuationsAsynchronously);
  internal ImageFrame(MetalImageQuadRenderer owner,CadFrameLease<ImageTexture> lease,IMTLTexture target,IMTLBuffer vertices,IMTLCommandBuffer command,int width,int height)
  {this.owner=owner;this.lease=lease;Target=target;this.vertices=vertices;this.command=command;Width=width;Height=height;}
  public Task<byte[]> CommitAsync()
  {
   lock(owner.sync){if(state!=0)throw new InvalidOperationException("Image frame has already committed or been disposed.");state=1;try{command.Commit();}catch(Exception failure){var error=Release(failure);owner.lastError??=error;owner.ReleaseIfFinished();Completion.TrySetException(error!);}return Completion.Task;}
  }
  internal Exception? Release(Exception? error)
  {
   if(freed)return error;freed=true;state=2;
   foreach(var value in new IDisposable[]{command,vertices,Target,lease})try{value.Dispose();}catch(Exception failure){error??=failure;}
   owner.targets--;owner.buffers--;return error;
  }
  public void Dispose(){lock(owner.sync){if(state!=0)return;var error=Release(null);owner.lastError??=error;owner.ReleaseIfFinished();Completion.TrySetCanceled();}}
 }
}
