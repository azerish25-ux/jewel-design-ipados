using Metal;
namespace JewelDesign.Rendering.Candidate.iOS;

internal sealed class CadMetalGpuMesh : IDisposable
{
    internal readonly long UploadId;
    internal readonly IMTLBuffer? Vertices,Indices;
    internal readonly int TriangleCount,BufferCount;
    private readonly Action<int,Exception?> released;
    private int references=1;
    internal CadMetalGpuMesh Retain(){int value;do{value=Volatile.Read(ref references);ObjectDisposedException.ThrowIf(value==0,this);}while(Interlocked.CompareExchange(ref references,checked(value+1),value)!=value);return this;}
    internal CadMetalGpuMesh(long uploadId,IMTLBuffer? vertices,IMTLBuffer? indices,int triangles,Action<int,Exception?> released)
    {UploadId=uploadId;Vertices=vertices;Indices=indices;TriangleCount=triangles;BufferCount=(vertices is null?0:1)+(indices is null?0:1);this.released=released;}
    public void Dispose()
    {
        int left=Interlocked.Decrement(ref references);if(left>0)return;if(left<0)throw new InvalidOperationException("Mesh reference released twice.");
        Exception? failure=null;
        try{Vertices?.Dispose();}catch(Exception error){failure=error;}
        try{Indices?.Dispose();}catch(Exception error){failure??=error;}
        released(BufferCount,failure);
    }
}
