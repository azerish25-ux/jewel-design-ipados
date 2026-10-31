using System.Runtime.InteropServices;
using Foundation;
using Metal;
using JewelDesign.Rendering.Candidate;
namespace JewelDesign.Rendering.Candidate.iOS;
/// <summary>Actual GPU offscreen CAD proof. Neutral test lighting, not original CounterSketch material parity.</summary>
public sealed class CadMetalRenderer:IDisposable
{
 private readonly IMTLDevice device;private IMTLCommandQueue? queue;private IMTLRenderPipelineState? pipeline;private IMTLDepthStencilState? depth;private bool disposed;
 public string DeviceName=>device.Name;
 public CadMetalRenderer(){device=MTLDevice.SystemDefault??throw new NotSupportedException("Metal device is unavailable; no CPU fallback.");try{
  queue=device.CreateCommandQueue()??throw new InvalidOperationException("Metal command queue creation failed.");
  using var stream=typeof(CadMetalRenderer).Assembly.GetManifestResourceStream("CadViewportMetalShader")??throw new InvalidOperationException("Metal shader resource missing.");using var reader=new StreamReader(stream);using var options=new MTLCompileOptions();using var library=device.CreateLibrary(reader.ReadToEnd(),options,out var compileError)??throw new InvalidOperationException("Metal shader compilation failed: "+compileError);
  using var vertex=library.CreateFunction("cad_vertex")??throw new InvalidOperationException("Metal vertex function missing.");using var fragment=library.CreateFunction("cad_fragment")??throw new InvalidOperationException("Metal fragment function missing.");using var description=new MTLRenderPipelineDescriptor();description.VertexFunction=vertex;description.FragmentFunction=fragment;description.ColorAttachments[0].PixelFormat=MTLPixelFormat.BGRA8Unorm;description.DepthAttachmentPixelFormat=MTLPixelFormat.Depth32Float;
  pipeline=device.CreateRenderPipelineState(description,out var pipelineError)??throw new InvalidOperationException("Metal pipeline compilation failed: "+pipelineError);using var depthDescription=new MTLDepthStencilDescriptor{DepthCompareFunction=MTLCompareFunction.Less,DepthWriteEnabled=true};depth=device.CreateDepthStencilState(depthDescription)??throw new InvalidOperationException("Metal depth state creation failed.");
 }catch{Dispose();throw;}}
 public byte[] RenderBgra(CadViewportMesh mesh,int width,int height,float yaw=.6f,float pitch=.35f,bool reverseSubmission=false){
  ObjectDisposedException.ThrowIf(disposed,this);ArgumentNullException.ThrowIfNull(mesh);var matrix=CadViewportCamera.Fit(width,height,yaw,pitch);
  using var targetDescription=MTLTextureDescriptor.CreateTexture2DDescriptor(MTLPixelFormat.BGRA8Unorm,(nuint)width,(nuint)height,false);targetDescription.StorageMode=MTLStorageMode.Shared;targetDescription.Usage=MTLTextureUsage.RenderTarget;
  using var target=device.CreateTexture(targetDescription)??throw new InvalidOperationException("Metal color target creation failed.");using var depthDescription=MTLTextureDescriptor.CreateTexture2DDescriptor(MTLPixelFormat.Depth32Float,(nuint)width,(nuint)height,false);depthDescription.StorageMode=MTLStorageMode.Private;depthDescription.Usage=MTLTextureUsage.RenderTarget;using var depthTarget=device.CreateTexture(depthDescription)??throw new InvalidOperationException("Metal depth target creation failed.");
  using var pass=MTLRenderPassDescriptor.CreateRenderPassDescriptor();pass.ColorAttachments[0].Texture=target;pass.ColorAttachments[0].LoadAction=MTLLoadAction.Clear;pass.ColorAttachments[0].StoreAction=MTLStoreAction.Store;pass.ColorAttachments[0].ClearColor=new MTLClearColor(.035,.045,.06,1);var depthAttachment=pass.DepthAttachment??throw new InvalidOperationException("Metal render pass depth attachment unavailable.");depthAttachment.Texture=depthTarget;depthAttachment.LoadAction=MTLLoadAction.Clear;depthAttachment.StoreAction=MTLStoreAction.DontCare;depthAttachment.ClearDepth=1;
  using var command=queue!.CommandBuffer()??throw new InvalidOperationException("Metal command buffer unavailable.");using var encoder=command.CreateRenderCommandEncoder(pass)??throw new InvalidOperationException("Metal render encoder unavailable.");
  using var vertices=mesh.VertexCount==0?null:device.CreateBuffer(mesh.CopyInterleavedVertices(),MTLResourceOptions.StorageModeShared);using var indices=mesh.TriangleCount==0?null:device.CreateBuffer(OrderedIndices(mesh,reverseSubmission),MTLResourceOptions.StorageModeShared);using var camera=device.CreateBuffer(CadViewportCamera.MatrixScalars(matrix),MTLResourceOptions.StorageModeShared)??throw new InvalidOperationException("Metal camera upload failed.");
  if(mesh.TriangleCount>0){if(vertices is null||indices is null)throw new InvalidOperationException("Metal CAD upload failed.");encoder.SetRenderPipelineState(pipeline!);encoder.SetDepthStencilState(depth!);encoder.SetCullMode(MTLCullMode.Back);encoder.SetFrontFacingWinding(MTLWinding.CounterClockwise);encoder.SetVertexBuffer(vertices,0,0);encoder.SetVertexBuffer(camera,0,1);encoder.DrawIndexedPrimitives(MTLPrimitiveType.Triangle,(nuint)(mesh.TriangleCount*3),MTLIndexType.UInt32,indices,0);}
  encoder.EndEncoding();command.Commit();command.WaitUntilCompleted();if(command.Status!=MTLCommandBufferStatus.Completed)throw new InvalidOperationException("Metal draw did not complete: "+command.Status+" "+command.Error);
  var bytes=new byte[checked(width*height*4)];var handle=GCHandle.Alloc(bytes,GCHandleType.Pinned);try{target.GetBytes(handle.AddrOfPinnedObject(),(nuint)(width*4),MTLRegion.Create2D(0,0,(nuint)width,(nuint)height),0);}finally{handle.Free();}return bytes;
 }
 private static uint[] OrderedIndices(CadViewportMesh mesh,bool reverse){var data=mesh.CopyIndices();if(reverse)for(int a=0,b=data.Length-3;a<b;a+=3,b-=3)for(int i=0;i<3;i++)(data[a+i],data[b+i])=(data[b+i],data[a+i]);return data;}
 public void Dispose(){if(disposed)return;disposed=true;depth?.Dispose();pipeline?.Dispose();queue?.Dispose();}
}
