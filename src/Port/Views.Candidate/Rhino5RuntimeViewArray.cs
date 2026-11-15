using JewelDesign.Rendering.Candidate;
namespace JewelDesign.Views.Candidate;

/// <summary>Original-derived view-array lifetime/copy operations for history. Managed references
/// retain logical object ownership across raw native relocations; native addresses/dangling aliases
/// are not exposed or emulated. Borrowed elements must not be disposed or kept across mutation.</summary>
public sealed class Rhino5RuntimeViewArray:IDisposable
{
    private readonly Rhino5UserDataRuntime runtime;
    private readonly int maxCapacity;
    private readonly List<Rhino5RuntimeView> slots=new();
    public int Count {get;private set;}
    public int Capacity=>slots.Count;
    public bool IsDisposed {get;private set;}
    public Rhino5RuntimeViewArray(Rhino5UserDataRuntime runtime,int maxCapacity=4096)
    {ArgumentNullException.ThrowIfNull(runtime);if(maxCapacity<0)throw new ArgumentOutOfRangeException(nameof(maxCapacity));this.runtime=runtime;this.maxCapacity=maxCapacity;}
    private void Alive()=>ObjectDisposedException.ThrowIf(IsDisposed,this);
    public Rhino5RuntimeView this[int index]
    {get{Alive();if(index<0||index>=Count)throw new ArgumentOutOfRangeException(nameof(index));return slots[index];}}

    public static int NativeNextCapacity(int count)
    {
        if(count<0)throw new ArgumentOutOfRangeException(nameof(count));
        return checked((long)count*1120>0x10000000&&count>7?count+Math.Min(count,0x3a842):count<3?4:count*2);
    }
    public void Reserve(int capacity){Alive();if(Capacity<capacity)SetCapacity(capacity);}
    public void SetCapacity(int capacity)
    {
        Alive();if(capacity>maxCapacity)throw new InvalidDataException("View-array managed capacity budget exceeded.");
        if(capacity<=0){DestroyStorage();return;}
        int old=Capacity;if(capacity==old)return;
        if(capacity<old)
        {
            for(int i=old-1;i>=capacity;i--)slots[i].Dispose();
            slots.RemoveRange(capacity,old-capacity);Count=Math.Min(Count,capacity);return;
        }
        // Native growth preserves existing values without copy callbacks and default-constructs
        // every new capacity slot. Stage empty defaults so managed allocation failure loses no data.
        var added=new List<Rhino5RuntimeView>(capacity-old);
        try{for(int i=old;i<capacity;i++)added.Add(Rhino5ViewDefaults.Create(runtime));slots.AddRange(added);}
        catch{foreach(var view in added)view.Dispose();throw;}
    }
    public void Append(Rhino5RuntimeView source)
    {
        Alive();ArgumentNullException.ThrowIfNull(source);ObjectDisposedException.ThrowIf(source.IsDisposed,source);
        if(Count==Capacity)
        {
            int next=NativeNextCapacity(Count);
            if(next>maxCapacity)throw new InvalidDataException("View-array managed capacity budget exceeded.");
            if(slots.Any(view=>ReferenceEquals(view,source)))
            {
                // Original self-alias/full-capacity path uses a default temporary + assignment,
                // then assigns that temporary to the new slot (two virtual copy operations).
                using var temporary=Rhino5ViewDefaults.Create(runtime);temporary.AssignFrom(source);
                Reserve(next);int index=Count++;slots[index].AssignFrom(temporary);return;
            }
            Reserve(next);
        }
        int target=Count++;slots[target].AssignFrom(source);
    }
    public Rhino5RuntimeView AppendNew()
    {
        Alive();if(Count==Capacity)Reserve(NativeNextCapacity(Count));
        else{slots[Count].Dispose();slots[Count]=Rhino5ViewDefaults.Create(runtime);}
        return slots[Count++];
    }
    public void RemoveLast()=>RemoveAt(Count-1);
    public void RemoveAt(int index)
    {
        Alive();if(index<0||index>=Count)return;
        slots[index].Dispose();
        // Original Move is memmove, not element assignment: no clone/counter increments.
        for(int i=index;i<Count-1;i++)slots[i]=slots[i+1];
        slots[Count-1]=Rhino5ViewDefaults.Create(runtime);Count--;
    }
    public void Empty()
    {
        Alive();for(int i=Count-1;i>=0;i--){slots[i].Dispose();slots[i]=Rhino5ViewDefaults.Create(runtime);}Count=0;
    }
    private void DestroyStorage()
    {for(int i=Capacity-1;i>=0;i--)slots[i].Dispose();slots.Clear();Count=0;}
    public void Dispose(){if(IsDisposed)return;DestroyStorage();IsDisposed=true;}
}
