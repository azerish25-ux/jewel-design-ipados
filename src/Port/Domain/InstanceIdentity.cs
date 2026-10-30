namespace JewelDesign.Domain;

/// <summary>Raw stored text plus the effective runtime GUID from the recovered object-read rule.</summary>
public sealed class InstanceIdentity
{
    public string? StoredText { get; }
    public Guid EffectiveGuid { get; }
    public bool WasGenerated { get; }

    private InstanceIdentity(string? storedText, Guid effectiveGuid, bool wasGenerated)
    {
        StoredText = storedText;
        EffectiveGuid = effectiveGuid;
        WasGenerated = wasGenerated;
    }

    public static InstanceIdentity ResolveStored(string? storedText)
    {
        bool parsed = Guid.TryParse(storedText, out var effective);
        if (!parsed) effective = Guid.NewGuid();
        return new InstanceIdentity(storedText, effective, !parsed);
    }
}
