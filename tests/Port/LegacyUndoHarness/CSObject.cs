// Test-only target boundary. The original scheduler and CUndoItem are compiled unchanged.
// This contract does not implement CounterSketch objects, Rhino, UI or geometry.
namespace CS.CSObjects;
public abstract class CSObject
{
    public abstract CSObject CreateTempObject();
    public abstract bool SetUndo(CSObject source);
}
