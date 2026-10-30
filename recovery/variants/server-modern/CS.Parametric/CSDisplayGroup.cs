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
		int num = 0;
		int num2 = 1 & (archive.ReadInt(ref num) ? 1 : 0);
		if (num == 1)
		{
			Display = true;
		}
		else
		{
			Display = false;
		}
		return (byte)((uint)num2 & (archive.ReadString(ref Name) ? 1u : 0u)) != 0;
	}

	public bool Write(ref OnBinaryArchive archive)
	{
		return (byte)(1u & (archive.WriteInt(Display ? 1 : 0) ? 1u : 0u) & (archive.WriteString(Name) ? 1u : 0u)) != 0;
	}
}
