namespace JewelDesign.Rendering.Candidate;

public readonly record struct CadFrameGateState(long Revision, long LastSubmittedRevision,
    int InFlight, int LiveResources, bool NeedsFrame, bool IsDisposed);

/// <summary>A stable resource/revision snapshot owned until the submitted frame completes.</summary>
public sealed class CadFrameLease<T> : IDisposable where T : class, IDisposable
{
    private readonly T resource;
    private readonly Action release;
    private int released;
    public long Revision { get; }
    public T Resource
    {
        get { ObjectDisposedException.ThrowIf(Volatile.Read(ref released) != 0, this); return resource; }
    }
    internal CadFrameLease(T resource, long revision, Action release)
    { this.resource = resource; Revision = revision; this.release = release; }
    public void Dispose() { if (Interlocked.Exchange(ref released, 1) == 0) release(); }
}

/// <summary>
/// Owns one current resource and at most three submitted frames. Replacement/disposal retires
/// resources only after their final lease completes. Successful replacement transfers ownership.
/// Resource disposal follows the nonthrowing GPU-resource cleanup contract.
/// </summary>
public sealed class CadFrameGate<T> : IDisposable where T : class, IDisposable
{
    public const int MaximumFramesInFlight = 3;
    private sealed class Entry(T resource)
    {
        public readonly T Resource = resource;
        public int References;
        public bool Retired;
    }
    private readonly object sync = new();
    private readonly HashSet<Entry> entries = new();
    private Entry? current;
    private long revision, lastSubmitted = -1;
    private int inFlight;
    private bool disposed;

    public CadFrameGateState State
    {
        get
        {
            lock (sync) return new(revision, lastSubmitted, inFlight, entries.Count,
                !disposed && current is not null && revision != lastSubmitted, disposed);
        }
    }

    public void ReplaceResource(T resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        lock (sync)
        {
            ObjectDisposedException.ThrowIf(disposed, this);
            if (entries.Any(entry => ReferenceEquals(entry.Resource, resource)))
                throw new InvalidOperationException("Resource ownership has already been transferred to this frame gate.");
            long next = checked(revision + 1);
            var old = current;
            var replacement = new Entry(resource);
            entries.Add(replacement);
            current = replacement;
            revision = next;
            if (old is not null) { old.Retired = true; ReleaseRetired(old); }
        }
    }

    public void Invalidate()
    {
        lock (sync) { ObjectDisposedException.ThrowIf(disposed, this); revision = checked(revision + 1); }
    }

    public CadFrameLease<T>? TryBeginFrame()
    {
        lock (sync)
        {
            ObjectDisposedException.ThrowIf(disposed, this);
            if (current is null || revision == lastSubmitted || inFlight == MaximumFramesInFlight) return null;
            var entry = current;
            var lease = new CadFrameLease<T>(entry.Resource, revision, () => Complete(entry));
            entry.References++; inFlight++; lastSubmitted = revision;
            return lease;
        }
    }

    private void Complete(Entry entry)
    {
        lock (sync)
        {
            if (entry.References <= 0 || inFlight <= 0) throw new InvalidOperationException("Unbalanced frame completion.");
            entry.References--; inFlight--; ReleaseRetired(entry);
        }
    }

    private void ReleaseRetired(Entry entry)
    {
        if (!entry.Retired || entry.References != 0) return;
        entries.Remove(entry);
        entry.Resource.Dispose();
    }

    public void Dispose()
    {
        lock (sync)
        {
            if (disposed) return;
            disposed = true;
            if (current is not null) { current.Retired = true; ReleaseRetired(current); current = null; }
        }
    }
}
