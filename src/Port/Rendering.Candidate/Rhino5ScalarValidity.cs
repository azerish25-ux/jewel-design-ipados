namespace JewelDesign.Rendering.Candidate;

/// <summary>Exact scalar predicate recovered from the hash-locked Rhino5 SR12 core leaf routine.</summary>
/// <remarks>Unlike the published6 candidate, the positive unset counterpart remains valid.</remarks>
public static class Rhino5ScalarValidity
{
    public static bool IsValid(double value)
    {
        ulong bits=BitConverter.DoubleToUInt64Bits(value);
        return (bits&0x7ff0000000000000UL)!=0x7ff0000000000000UL && bits!=0xffe5f8bd9e331dd2UL;
    }
}
