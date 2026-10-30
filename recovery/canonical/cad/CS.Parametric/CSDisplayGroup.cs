using RMA.OpenNURBS;

namespace CS.Parametric;

public sealed class CSDisplayGroup
{
	public bool Display = true;

	public string Name = "Unknown";

	public CSDisplayGroup(string name)
	{
		Name = name;
	}

	public bool Read(ref OnBinaryArchive archive)
	{
		int i = 0;
		int num = 1 & (archive.ReadInt(ref i) ? 1 : 0);
		if (i == 1)
		{
			Display = true;
		}
		else
		{
			Display = false;
		}
		return (byte)((uint)num & (archive.ReadString(ref Name) ? 1u : 0u)) != 0;
	}

	public bool Write(ref OnBinaryArchive archive)
	{
		return (byte)(1u & (archive.WriteInt(Display ? 1 : 0) ? 1u : 0u) & (archive.WriteString(Name) ? 1u : 0u)) != 0;
	}
}
