using Metal;
namespace JewelDesign.Rendering.Candidate.iOS;

/// <summary>One immutable visible scene owns references to independently retained mesh/image uploads.</summary>
internal sealed class CadMetalGpuScene(CadMetalGpuMesh? mesh,CadMetalGpuWallpaper? wallpaper,CadMetalGpuTrace? trace=null,JewelDesign.Rendering.Candidate.Rhino5TraceImage? traceInfo=null):IDisposable
{
 internal readonly CadMetalGpuMesh? Mesh=mesh;
 internal readonly CadMetalGpuWallpaper? Wallpaper=wallpaper;
 internal readonly CadMetalGpuTrace? Trace=trace;
 internal readonly JewelDesign.Rendering.Candidate.Rhino5TraceImage? TraceInfo=traceInfo??trace?.Info;
 private int disposed;
 public void Dispose(){if(Interlocked.Exchange(ref disposed,1)!=0)return;Mesh?.Dispose();Wallpaper?.Dispose();Trace?.Dispose();}
}
internal sealed class CadMetalGpuWallpaper(long uploadId,IMTLTexture texture,int width,int height,int scale,Action<Exception?> released):IDisposable
{
 internal readonly long UploadId=uploadId;internal readonly IMTLTexture Texture=texture;
 internal readonly int Width=width,Height=height,Scale=scale;
 private int references=1;
 internal CadMetalGpuWallpaper Retain(){int value;do{value=Volatile.Read(ref references);ObjectDisposedException.ThrowIf(value==0,this);}while(Interlocked.CompareExchange(ref references,checked(value+1),value)!=value);return this;}
 public void Dispose(){
  int left=Interlocked.Decrement(ref references);if(left>0)return;if(left<0)throw new InvalidOperationException("Wallpaper reference released twice.");
  Exception? error=null;try{Texture.Dispose();}catch(Exception failure){error=failure;}released(error);
 }
}

internal sealed class CadMetalGpuTrace(long uploadId,IMTLTexture texture,int width,int height,JewelDesign.Rendering.Candidate.Rhino5TraceImage info,int dibDepth,Action<Exception?> released):IDisposable
{
 internal readonly long UploadId=uploadId;internal readonly IMTLTexture Texture=texture;internal readonly int Width=width,Height=height,DibDepth=dibDepth;internal readonly JewelDesign.Rendering.Candidate.Rhino5TraceImage Info=info;private int references=1;
 internal CadMetalGpuTrace Retain(){int value;do{value=Volatile.Read(ref references);ObjectDisposedException.ThrowIf(value==0,this);}while(Interlocked.CompareExchange(ref references,checked(value+1),value)!=value);return this;}
 public void Dispose(){int left=Interlocked.Decrement(ref references);if(left>0)return;if(left<0)throw new InvalidOperationException("Trace reference released twice.");Exception? error=null;try{Texture.Dispose();}catch(Exception failure){error=failure;}released(error);}
}
