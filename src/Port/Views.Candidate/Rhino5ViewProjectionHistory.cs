namespace JewelDesign.Views.Candidate;

/// <summary>Original PushViewProjection storage policy. Owns the supplied array and cursor state;
/// does not pretend to implement navigation, Pop/SetView or actual document notifications.</summary>
public sealed class Rhino5ViewProjectionHistory:IDisposable
{
    private readonly Rhino5RuntimeViewArray views;
    public int Cursor {get;private set;}
    public int Count=>views.Count;
    public int Capacity=>views.Capacity;
    public bool IsDisposed {get;private set;}
    public Rhino5ViewProjectionHistory(Rhino5RuntimeViewArray ownedViews,int importedCursor)
    {ArgumentNullException.ThrowIfNull(ownedViews);ObjectDisposedException.ThrowIf(ownedViews.IsDisposed,ownedViews);views=ownedViews;Cursor=importedCursor;}
    public Rhino5RuntimeView this[int index]=>views[index];
    public void Push(Rhino5RuntimeView current)
    {
        ObjectDisposedException.ThrowIf(IsDisposed,this);ArgumentNullException.ThrowIfNull(current);
        if(Count<1||Count-1<=Cursor)views.Append(current);
        else
        {
            if(Cursor< -1)throw new ArgumentOutOfRangeException(nameof(Cursor),"Imported cursor would address memory before the native array.");
            views[Cursor+1].AssignFrom(current);
            while(Cursor+2<Count)views.RemoveLast();
        }
        if(Count>255)views.RemoveAt(0);
        Cursor=Count-1;
    }
    public void Dispose(){if(IsDisposed)return;views.Dispose();IsDisposed=true;}
}
