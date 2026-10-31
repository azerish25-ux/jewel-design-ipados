using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
// These probes model resource ownership only; actual GPU behavior is tested in the visible iOS harness.
public static class CadFrameGateAssertions
{
    private sealed class Resource : IDisposable
    {
        public int Disposals { get; private set; }
        public void Dispose() => Disposals++;
    }
    private static void Check(bool value, string message) { if (!value) throw new Exception(message); }
    private static void Throws<T>(Action action) where T : Exception
    { try { action(); } catch (T) { return; } throw new Exception("Expected " + typeof(T).Name); }
    public static string Run()
    {
        int checks = 0;
        using (var gate = new CadFrameGate<Resource>())
        {
            Check(gate.TryBeginFrame() is null && !gate.State.NeedsFrame, "Unbound viewport submitted a frame"); checks++;
            var first = new Resource(); gate.ReplaceResource(first);
            var a = gate.TryBeginFrame()!; Check(ReferenceEquals(a.Resource, first), "Lease resource identity changed"); checks++;
            Check(gate.TryBeginFrame() is null, "Unchanged revision resubmitted"); checks++;
            long oldRevision = a.Revision; gate.Invalidate(); var b = gate.TryBeginFrame()!;
            gate.Invalidate(); var c = gate.TryBeginFrame()!;
            gate.Invalidate(); long latest = gate.State.Revision;
            Check(gate.TryBeginFrame() is null && gate.State.InFlight == 3 && gate.State.NeedsFrame, "Three-frame bound lost pending work"); checks++;
            Check(a.Revision == oldRevision, "Input mutation changed an in-flight revision"); checks++;
            var second = new Resource(); gate.ReplaceResource(second);
            Throws<InvalidOperationException>(() => gate.ReplaceResource(first));
            Check(first.Disposals == 0 && gate.State.LiveResources == 2, "Replacement freed submitted resources"); checks++;
            b.Dispose(); var d = gate.TryBeginFrame()!;
            Check(ReferenceEquals(d.Resource, second) && d.Revision > latest && gate.State.InFlight == 3, "Latest revision not resumed after out-of-order completion"); checks++;
            a.Dispose(); a.Dispose(); Check(first.Disposals == 0, "Duplicate/early completion retired the old mesh"); checks++;
            c.Dispose(); Check(first.Disposals == 1 && gate.State.LiveResources == 1, "Final old frame did not retire its mesh exactly once"); checks++;
            gate.Dispose(); Check(second.Disposals == 0 && ReferenceEquals(d.Resource, second), "Owner disposal freed a pending frame"); checks++;
            Throws<ObjectDisposedException>(() => gate.TryBeginFrame());
            Throws<ObjectDisposedException>(() => gate.Invalidate());
            var rejected = new Resource(); Throws<ObjectDisposedException>(() => gate.ReplaceResource(rejected));
            Check(rejected.Disposals == 0, "Rejected ownership transfer disposed caller resource"); rejected.Dispose(); checks++;
            d.Dispose(); d.Dispose(); gate.Dispose();
            Check(second.Disposals == 1 && gate.State.InFlight == 0 && gate.State.LiveResources == 0, "Late completion leaked resources"); checks++;
            Throws<ObjectDisposedException>(() => _ = d.Resource);
        }
        using (var gate = new CadFrameGate<Resource>())
        {
            var held = new Resource(); gate.ReplaceResource(held); var frame = gate.TryBeginFrame()!;
            var resources = new List<Resource>();
            for (int i = 0; i < 100; i++) { var resource = new Resource(); resources.Add(resource); gate.ReplaceResource(resource); Check(gate.State.LiveResources == 2, "Rapid replacement grew resource queue"); }
            Check(resources.Take(99).All(x => x.Disposals == 1) && held.Disposals == 0, "Unsubmitted/held ownership differs"); checks += 101;
            Throws<InvalidOperationException>(() => gate.ReplaceResource(resources[^1]));
            Throws<ArgumentNullException>(() => gate.ReplaceResource(null!));
            Check(resources[^1].Disposals == 0, "Invalid replacement changed current ownership"); checks++;
            frame.Dispose(); Check(held.Disposals == 1, "Held generation survived its final lease"); checks++;
        }
        using (var gate = new CadFrameGate<Resource>())
        {
            var resource = new Resource(); gate.ReplaceResource(resource);
            var frames = new List<CadFrameLease<Resource>>();
            for (int i = 0; i < 3; i++) { gate.Invalidate(); frames.Add(gate.TryBeginFrame()!); }
            gate.Dispose(); Parallel.For(0, 30, i => frames[i % 3].Dispose());
            Check(resource.Disposals == 1 && gate.State.LiveResources == 0 && gate.State.InFlight == 0, "Concurrent duplicate completion leaked/double-freed resources"); checks++;
        }
        return "PASS viewport frame ownership:" + checks + " resource-only checks; three-frame bound/latest revision, retained replacement/disposal and concurrent idempotent completion; actual GPU presentation tested separately.";
    }
}
