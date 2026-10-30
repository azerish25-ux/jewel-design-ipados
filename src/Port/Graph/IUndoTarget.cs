namespace JewelDesign.Graph;

/// <summary>Adapter boundary for complete isolated undo state, independent of native object factories.</summary>
public interface IUndoTarget<TState>
{
    TState CaptureUndoState();
    // The recovered scheduler ignores the boolean, including false; exceptions propagate.
    bool RestoreUndoState(TState state);
}
