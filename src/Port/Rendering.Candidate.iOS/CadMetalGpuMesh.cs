using Metal;
namespace JewelDesign.Rendering.Candidate.iOS;

internal sealed class CadMetalGpuMesh : IDisposable
{
    internal readonly long UploadId;
    internal readonly IMTLBuffer? Vertices,Indices;
    internal readonly int TriangleCount,BufferCount;
    private readonly Action<int,Exception?> released;
    private int disposed;
    internal CadMetalGpuMesh(long uploadId,IMTLBuffer? vertices,IMTLBuffer? indices,int triangles,Action<int,Exception?> released)
    {UploadId=uploadId;Vertices=vertices;Indices=indices;TriangleCount=triangles;BufferCount=(vertices is null?0:1)+(indices is null?0:1);this.released=released;}
    public void Dispose()
    {
        if(Interlocked.Exchange(ref disposed,1)!=0)return;
        Exception? failure=null;
        try{Vertices?.Dispose();}catch(Exception error){failure=error;}
        try{Indices?.Dispose();}catch(Exception error){failure??=error;}
        released(BufferCount,failure);
    }
}
