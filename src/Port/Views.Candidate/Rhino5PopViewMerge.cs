namespace JewelDesign.Views.Candidate;

/// <summary>Actual original pre-Pop merge only; caller still requires concrete SetView, screen,
/// removal/cursor and document effects before full Pop restoration is implemented.</summary>
public static class Rhino5PopViewMerge
{
    private static bool TrimCharacter(char c)=>c is ' ' or '\t' or '\r' or '\n';
    /// <summary>Native TrimRight then TrimLeft, length-based IsEmpty and NUL-terminated Compare
    /// equality predicate. This intentionally is not Unicode string.Trim/managed full-string equality.</summary>
    public static bool IsEligibleSavedName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        if(name.Length==0||name[0]=='\0'||TrimCharacter(name[0]))return false;
        // Trimming after the first embedded NUL cannot change the compared C-string prefix.
        return name.IndexOf('\0')>=0||!TrimCharacter(name[^1]);
    }
    public static void Apply(Rhino5RuntimeView? saved,Rhino5RuntimeView current,Rhino5RuntimeView result)
    {
        ArgumentNullException.ThrowIfNull(current);ArgumentNullException.ThrowIfNull(result);
        ObjectDisposedException.ThrowIf(current.IsDisposed,current);ObjectDisposedException.ThrowIf(result.IsDisposed,result);
        if(!ReferenceEquals(current,result))result.AssignFrom(current);
        if(saved is null||ReferenceEquals(result,current)||ReferenceEquals(current,saved)||ReferenceEquals(result,saved))return;
        ObjectDisposedException.ThrowIf(saved.IsDisposed,saved);
        result.AssignFrom(current);
        result.AssignViewportFrom(saved);
        result.TrySetTargetPoint(saved.ReadTargetPoint());
        string name=saved.Metadata.Name;
        if(IsEligibleSavedName(name)&&result.Metadata.ViewType!=1)
            result.SetConvertedValues(result.Viewport,result.TargetMirror,result.Details,result.Metadata with {Name=name},result.CopyClippingPlanes());
        // Equal bounds reject before mutation, retaining the saved viewport port as original.
        result.TrySetScreenPort(0,0,0,0,0,0);
    }
}
