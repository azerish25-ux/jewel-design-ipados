using System.Runtime.InteropServices;
using Foundation;
using Metal;
using JewelDesign.Rendering.Candidate;
namespace JewelDesign.Rendering.Candidate.iOS;
public readonly record struct MetalImageQuadStats(int InFlight,int LiveTextures,int LiveTargets,int LiveVertexBuffers,bool IsDisposed,bool PipelineReleased,string? LastError){public int LiveSamplers{get;init;}public long TextureUploads{get;init;}}

/// <summary>Real offscreen image-quad GPU component with three retained frame leases.
/// Input MUST be straight RGBA8/sRGB values, top row first. Does not unpremultiply decoder output,
/// implement full document/SetView lifecycle, or claim visible/legacy codec parity.</summary>
public sealed class MetalImageQuadRenderer:IDisposable
{
 private readonly object sync=new();
 private readonly IMTLDevice device;
 private readonly CadFrameGate<ImageTexture> frames=new();
 private IMTLCommandQueue? queue;private MetalImageQuadPipeline? imagePipeline;
 private bool disposed,released;private long textureUploads;private int targets,buffers,textures;private Rhino5ContextImageCache<CachedSource>? sourceCache;private ulong sourceContext;private static long nextSourceContext;private Exception? lastError;
 public MetalImageQuadStats Stats{get{lock(sync){var s=frames.State;return new(s.InFlight,textures,targets,buffers,disposed,released,lastError?.ToString()){LiveSamplers=imagePipeline?.SamplerCount??0,TextureUploads=textureUploads};}}}
 public MetalImageQuadRenderer()
 {
  device=MTLDevice.SystemDefault??throw new NotSupportedException("Metal device unavailable.");
  try{
   queue=device.CreateCommandQueue()??throw new InvalidOperationException("Image command queue unavailable.");
   imagePipeline=new MetalImageQuadPipeline(device,MTLPixelFormat.Invalid);
  }catch{Dispose();throw;}
 }
 /// <summary>Prepares actual decoded source pixels in original normal texture order before GPU replacement.</summary>
 public Rhino5TextureRasterReceipt SetTextureSource(DecodedLocalImage source,int sourceDibDepth,Rhino5TextureRasterSettings settings,bool gray=false,Rhino5TextureAnisotropy anisotropy=default,LocalImageLimits? limits=null)
 {
  using var prepared=Rhino5TextureRasterPreparation.Prepare(source,sourceDibDepth,settings,gray,limits);
  using var image=prepared.CreateDecodedImage(source);SetImage(image,anisotropy);return prepared.Receipt;
 }
 public Rhino5WallpaperRasterReceipt SetWallpaperSource(DecodedLocalImage source,int sourceDibDepth,int requestedWidth,int requestedHeight,bool gray=false,LocalImageLimits? limits=null,long maxWorkingBytes=512L*1024*1024)
 {
  using var prepared=Rhino5WallpaperPreparation.Prepare(source,sourceDibDepth,requestedWidth,requestedHeight,gray,limits,maxWorkingBytes);
  using var image=prepared.CreateDecodedImage(source);SetImage(image);return prepared.Receipt;
 }
 /// <summary>Enables original source-cache behavior with an explicit, independently resolved filename profile.</summary>
 public void ConfigureSourceCache(Rhino5FilenameSemantics filenames,int maximumEntries=32)
 {ArgumentNullException.ThrowIfNull(filenames);lock(sync){ObjectDisposedException.ThrowIf(disposed,this);var replacement=new Rhino5ContextImageCache<CachedSource>(filenames,maximumEntries);sourceCache?.Dispose();sourceCache=replacement;sourceContext=checked((ulong)Interlocked.Increment(ref nextSourceContext));}}
 public int SourceCacheEntries{get{lock(sync)return sourceCache?.Count??0;}}
 public void SetSourceCacheUsage(uint serial){lock(sync){ObjectDisposedException.ThrowIf(disposed,this);(sourceCache??throw new InvalidOperationException("Source filename profile is not configured.")).UsageSerial=serial;}}
 public Rhino5TextureRasterReceipt SetCachedTextureSource(string filename,Func<DecodedLocalImage> resolve,int sourceDibDepth,Rhino5TextureRasterSettings settings,bool gray=false,Rhino5TextureAnisotropy anisotropy=default,LocalImageLimits? limits=null)
 {
  ArgumentNullException.ThrowIfNull(filename);ArgumentNullException.ThrowIfNull(resolve);lock(sync){ObjectDisposedException.ThrowIf(disposed,this);var cache=sourceCache??throw new InvalidOperationException("Explicit legacy filename profile is required before cached source upload.");
   using var lease=cache.Lookup(sourceContext,filename,gray,()=>{using var source=resolve();using var prepared=Rhino5TextureRasterPreparation.Prepare(source,sourceDibDepth,settings,gray,limits);using var image=prepared.CreateDecodedImage(source);return new CachedSource(Upload(image.Width,image.Height,image.CopyRgba(),anisotropy),prepared.Receipt,source.Source);})??throw new InvalidDataException("Source texture creation failed.");
   var resource=lease.Resource.Texture.Retain();try{frames.ReplaceResource(resource);}catch{resource.Dispose();throw;}return lease.Resource.Receipt;
  }
 }
 public int InvalidateSource(string filename){lock(sync){ObjectDisposedException.ThrowIf(disposed,this);return(sourceCache??throw new InvalidOperationException("Source cache not configured.")).Invalidate(filename);}}
 public void ClearSourceCache(){lock(sync){ObjectDisposedException.ThrowIf(disposed,this);sourceCache?.ClearContext(sourceContext);}}
 private sealed record CachedSource(ImageTexture Texture,Rhino5TextureRasterReceipt Receipt,LocalImageAsset Source):IDisposable{public void Dispose()=>Texture.Dispose();}
 public void SetImage(DecodedLocalImage image,Rhino5TextureAnisotropy anisotropy=default)
 {
  ArgumentNullException.ThrowIfNull(image);lock(sync){ObjectDisposedException.ThrowIf(disposed,this);
   if(image.AlphaRepresentation!=RgbaAlphaRepresentation.Straight)throw new NotSupportedException("The typed image upload requires straight RGBA. Premultiplied pixels are not silently unpremultiplied.");
   SetImage(image.Width,image.Height,image.CopyRgba(),anisotropy);
  }
 }
 public void SetImage(int width,int height,ReadOnlySpan<byte> straightRgba,Rhino5TextureAnisotropy anisotropy=default)
 {lock(sync){ObjectDisposedException.ThrowIf(disposed,this);var resource=Upload(width,height,straightRgba,anisotropy);try{frames.ReplaceResource(resource);}catch{resource.Dispose();throw;}}}
 private ImageTexture Upload(int width,int height,ReadOnlySpan<byte> straightRgba,Rhino5TextureAnisotropy anisotropy)
 {
  int maximumAnisotropy=anisotropy.ToMetalMaximum();imagePipeline!.EnsureAnisotropy(maximumAnisotropy);int size=new LocalImageLimits().ValidateDimensions(width,height);if(straightRgba.Length!=size)throw new ArgumentException("Straight RGBA storage length differs from image dimensions.",nameof(straightRgba));
  using var d=MTLTextureDescriptor.CreateTexture2DDescriptor(MTLPixelFormat.RGBA8Unorm,(nuint)width,(nuint)height,false);d.StorageMode=MTLStorageMode.Shared;d.Usage=MTLTextureUsage.ShaderRead;IMTLTexture? texture=device.CreateTexture(d)??throw new InvalidOperationException("Image texture allocation failed.");
  try{byte[] bytes=straightRgba.ToArray();var pin=GCHandle.Alloc(bytes,GCHandleType.Pinned);try{texture.ReplaceRegion(MTLRegion.Create2D(0,0,(nuint)width,(nuint)height),0,pin.AddrOfPinnedObject(),(nuint)(width*4));}finally{pin.Free();}var resource=new ImageTexture(texture,maximumAnisotropy,()=>{lock(sync)textures--;});texture=null;textures++;textureUploads++;return resource;}finally{texture?.Dispose();}
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
     imagePipeline!.Encode(encoder,lease.Resource.Texture,vertices,filtered,blend,lease.Resource.MaximumAnisotropy);encoder.EndEncoding();
    }
    frame=new ImageFrame(this,lease,target,vertices,command,width,height);targets++;buffers++;
    return frame;
   }catch{if(frame is not null)lastError??=frame.Release(null);else{command?.Dispose();vertices?.Dispose();target?.Dispose();lease.Dispose();}throw;}
  }
 }
 private void Complete(ImageFrame frame,IMTLCommandBuffer command)
 {
  byte[]? bytes=null;Exception? error=null;
  lock(sync){
   if(frame.IsReleased)return;
   try{
    if(command.Status!=MTLCommandBufferStatus.Completed)throw new InvalidOperationException("Image GPU command failed: "+command.Status+" "+command.Error);
    bytes=new byte[checked(frame.Width*frame.Height*4)];var pin=GCHandle.Alloc(bytes,GCHandleType.Pinned);
    try{frame.Target.GetBytes(pin.AddrOfPinnedObject(),(nuint)(frame.Width*4),MTLRegion.Create2D(0,0,(nuint)frame.Width,(nuint)frame.Height),0);}finally{pin.Free();}
   }catch(Exception failure){error=failure;}
   error=frame.Release(error);lastError??=error;ReleaseIfFinished();error??=lastError;
  }
  if(error is null)frame.Completion.TrySetResult(bytes!);else frame.Completion.TrySetException(error);
 }
 private void ReleaseIfFinished(){if(disposed&&frames.State.InFlight==0&&!released){released=true;foreach(var value in new IDisposable?[]{imagePipeline,queue})try{value?.Dispose();}catch(Exception failure){lastError??=failure;}}}
 public void Dispose(){lock(sync){if(disposed)return;disposed=true;try{sourceCache?.Dispose();}catch(Exception failure){lastError??=failure;}sourceCache=null;frames.Dispose();ReleaseIfFinished();}}
 // Each frame-gate transfer receives a distinct ownership token for one shared physical texture.
 internal sealed class ImageTexture:IDisposable
 {
  private sealed class Shared(IMTLTexture texture,int maximumAnisotropy,Action released)
  {internal readonly IMTLTexture Texture=texture;internal readonly int MaximumAnisotropy=maximumAnisotropy;internal int References=1;internal readonly Action Released=released;}
  private readonly Shared shared;private int disposed;
  internal IMTLTexture Texture=>shared.Texture;internal int MaximumAnisotropy=>shared.MaximumAnisotropy;
  internal ImageTexture(IMTLTexture texture,int maximumAnisotropy,Action released)=>shared=new(texture,maximumAnisotropy,released);
  private ImageTexture(Shared shared)=>this.shared=shared;
  internal ImageTexture Retain(){ObjectDisposedException.ThrowIf(Volatile.Read(ref disposed)!=0,this);int n;do{n=Volatile.Read(ref shared.References);ObjectDisposedException.ThrowIf(n==0,this);}while(Interlocked.CompareExchange(ref shared.References,checked(n+1),n)!=n);return new(shared);}
  public void Dispose(){if(Interlocked.Exchange(ref disposed,1)!=0)return;int n=Interlocked.Decrement(ref shared.References);if(n>0)return;if(n<0)throw new InvalidOperationException("Image texture released twice.");try{shared.Texture.Dispose();}finally{shared.Released();}}
 }
 public sealed class ImageFrame:IDisposable
 {
  private readonly MetalImageQuadRenderer owner;private readonly CadFrameLease<ImageTexture> lease;private readonly IMTLBuffer vertices;
  private readonly IMTLCommandBuffer command;private int state;private bool freed;
  internal readonly IMTLTexture Target;internal readonly int Width,Height;internal bool IsReleased=>freed;
  internal readonly TaskCompletionSource<byte[]> Completion=new(TaskCreationOptions.RunContinuationsAsynchronously);
  internal ImageFrame(MetalImageQuadRenderer owner,CadFrameLease<ImageTexture> lease,IMTLTexture target,IMTLBuffer vertices,IMTLCommandBuffer command,int width,int height)
  {this.owner=owner;this.lease=lease;Target=target;this.vertices=vertices;this.command=command;Width=width;Height=height;MaximumAnisotropy=lease.Resource.MaximumAnisotropy;}
  public int MaximumAnisotropy{get;}
  public Task<byte[]> CommitAsync()
  {
   lock(owner.sync){if(state!=0)throw new InvalidOperationException("Image frame has already committed or been disposed.");state=1;try{command.AddCompletedHandler(value=>owner.Complete(this,value));command.Commit();}catch(Exception failure){var error=Release(failure);owner.lastError??=error;owner.ReleaseIfFinished();Completion.TrySetException(error!);}return Completion.Task;}
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
