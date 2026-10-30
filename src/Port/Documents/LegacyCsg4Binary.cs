using System.Buffers.Binary;

namespace JewelDesign.Documents;

internal readonly record struct CsgChunk(int Offset, uint Code, int Length)
{
    public int Payload => Offset + 8;
    public int End => Payload + Length;
    public int CrcOffset => End - 4;
}

internal sealed class CsgCursor
{
    private readonly byte[] bytes;
    private readonly int limit;
    private readonly LegacyCsg4Limits limits;
    public int Position { get; set; }
    public CsgCursor(byte[] bytes, int start, int end, LegacyCsg4Limits limits) { this.bytes = bytes; Position = start; limit = end; this.limits = limits; }
    public void Need(int count) { if (count < 0 || Position < 0 || Position > limit - count) throw new InvalidDataException("Read outside bounded region at " + Position); }
    public int Int() { Need(4); int value = BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(Position, 4)); Position += 4; return value; }
    public uint UInt() => unchecked((uint)Int());
    public byte Byte(){Need(1);return bytes[Position++];}
    public double Double() { Need(8); long bits = BinaryPrimitives.ReadInt64LittleEndian(bytes.AsSpan(Position, 8)); Position += 8; return BitConverter.Int64BitsToDouble(bits); }
    public Guid Guid() { Need(16); var id = new Guid(bytes.AsSpan(Position, 16)); Position += 16; return id; }
    public int Count(int max) { int n = Int(); if (n < 0 || n > max) throw new InvalidDataException("Count outside configured bound"); return n; }
    public void NeedItems(int count, int width) { if (count < 0 || count > (limit - Position) / width) throw new InvalidDataException("Array exceeds bounded region"); }
    public int[] Ints() { int n = Count(limits.MaxArrayItems); NeedItems(n, 4); var values = new int[n]; for (int i = 0; i < n; i++) values[i] = Int(); return values; }
    public StoredText Text()
    {
        int n = Count(limits.MaxStringCodeUnits); NeedItems(n, 2);
        if (n == 0) return new StoredText(string.Empty, false);
        if (BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(Position + n * 2 - 2, 2)) != 0) throw new InvalidDataException("Unterminated UTF16 string");
        int start = Position; Position += n * 2;
        var value = string.Create(n - 1, (bytes, start), static (characters, state) =>
        {
            for (int i = 0; i < characters.Length; i++) characters[i] = (char)BinaryPrimitives.ReadUInt16LittleEndian(state.bytes.AsSpan(state.start + i * 2, 2));
        });
        return new StoredText(value, true);
    }
    public (StoredText Type, StoredText Key, StoredText Value)[] UserData()
    {
        int n = Count(limits.MaxArrayItems); NeedItems(n, 12);
        var items = new (StoredText, StoredText, StoredText)[n];
        for (int i = 0; i < n; i++) items[i] = (Text(), Text(), Text());
        return items;
    }
    public CsgChunk Chunk(uint expected)
    {
        int start = Position; uint code = UInt(); int size = Int();
        if (code != expected || (code & 0x80000000) != 0 || size < 4) throw new InvalidDataException("Unexpected long chunk at " + start);
        Need(size); return new CsgChunk(start, code, size);
    }
}

internal static class CsgCrc32
{
    private static readonly uint[] table = CreateTable();
    private static uint[] CreateTable()
    {
        var values = new uint[256];
        for (uint i = 0; i < values.Length; i++) { uint c = i; for (int b = 0; b < 8; b++) c = (c & 1) == 0 ? c >> 1 : (c >> 1) ^ 0xedb88320; values[i] = c; }
        return values;
    }
    public static uint Compute(ReadOnlySpan<byte> data) { uint crc = uint.MaxValue; foreach (byte b in data) crc = (crc >> 8) ^ table[(crc ^ b) & 255]; return ~crc; }
    public static void Check(byte[] bytes, CsgChunk chunk, int? end = null)
    {
        var actual = Compute(bytes.AsSpan(chunk.Payload, (end ?? chunk.CrcOffset) - chunk.Payload));
        if (BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(chunk.CrcOffset, 4)) != actual) throw new InvalidDataException("CRC mismatch at " + chunk.Offset);
    }
}
