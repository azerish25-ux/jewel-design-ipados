using System.Collections;

namespace GV.CounterSketch.Studio.Controls;

public sealed class StringLogicalComparer : IComparer
{
	private readonly bool _zeroesFirst;

	public static IComparer Default { get; } = new StringLogicalComparer();

	public static IComparer DefaultZeroesFirst { get; } = new StringLogicalComparer(zeroesFirst: true);

	private StringLogicalComparer(bool zeroesFirst)
	{
		_zeroesFirst = zeroesFirst;
	}

	private StringLogicalComparer()
	{
	}

	public int Compare(object x, object y)
	{
		if (x == null && y == null)
		{
			return 0;
		}
		if (x == null)
		{
			return -1;
		}
		if (y == null)
		{
			return 1;
		}
		if (x is string && y is string)
		{
			return Compare((string)x, (string)y, _zeroesFirst);
		}
		return Comparer.Default.Compare(x, y);
	}

	public static int Compare(string s1, string s2)
	{
		return Compare(s1, s2, zeroesFirst: false);
	}

	public static int Compare(string s1, string s2, bool zeroesFirst)
	{
		if (s1 == null && s2 == null)
		{
			return 0;
		}
		if (s1 == null)
		{
			return -1;
		}
		if (s2 == null)
		{
			return 1;
		}
		if (s1.Length <= 0 && s2.Length <= 0)
		{
			return 0;
		}
		if (s1.Length <= 0)
		{
			return -1;
		}
		if (s2.Length <= 0)
		{
			return 1;
		}
		bool flag = char.IsLetterOrDigit(s1[0]);
		bool flag2 = char.IsLetterOrDigit(s2[0]);
		if (flag && !flag2)
		{
			return 1;
		}
		if (!flag && flag2)
		{
			return -1;
		}
		int i = 0;
		int i2 = 0;
		do
		{
			char c = s1[i];
			char c2 = s2[i2];
			flag = char.IsDigit(c);
			flag2 = char.IsDigit(c2);
			if (!flag && !flag2)
			{
				bool flag3 = char.IsLetter(c);
				bool flag4 = char.IsLetter(c2);
				if (flag3 && flag4)
				{
					int num = char.ToUpper(c).ToString().CompareTo(char.ToUpper(c2).ToString());
					if (num != 0)
					{
						return num;
					}
				}
				else
				{
					if (flag3 || flag4)
					{
						if (!flag3)
						{
							return -1;
						}
						return 1;
					}
					int num = c.CompareTo(c2);
					if (num != 0)
					{
						return num;
					}
				}
			}
			else
			{
				if (!(flag && flag2))
				{
					if (flag)
					{
						return -1;
					}
					return 1;
				}
				int num = CompareNum(s1, ref i, s2, ref i2, zeroesFirst);
				if (num != 0)
				{
					return num;
				}
			}
			i++;
			i2++;
			if (i >= s1.Length && i2 >= s2.Length)
			{
				return 0;
			}
			if (i >= s1.Length)
			{
				return -1;
			}
		}
		while (i2 < s2.Length);
		return 1;
	}

	private static int CompareNum(string s1, ref int i1, string s2, ref int i2, bool zeroesFirst)
	{
		int nzStart = i1;
		int nzStart2 = i2;
		int end = i1;
		int end2 = i2;
		ScanNumEnd(s1, i1, ref end, ref nzStart);
		ScanNumEnd(s2, i2, ref end2, ref nzStart2);
		int num = i1;
		i1 = end - 1;
		int num2 = i2;
		i2 = end2 - 1;
		if (zeroesFirst)
		{
			int num3 = nzStart - num;
			int num4 = nzStart2 - num2;
			if (num3 > num4)
			{
				return -1;
			}
			if (num3 < num4)
			{
				return 1;
			}
		}
		int num5 = end - nzStart;
		int num6 = end2 - nzStart2;
		if (num5 < num6)
		{
			return -1;
		}
		if (num5 > num6)
		{
			return 1;
		}
		int num7 = nzStart;
		int num8 = nzStart2;
		while (num7 <= i1)
		{
			int num9 = s1[num7].CompareTo(s2[num8]);
			if (num9 != 0)
			{
				return num9;
			}
			num7++;
			num8++;
		}
		int num10 = end - num;
		int num11 = end2 - num2;
		if (num10 == num11)
		{
			return 0;
		}
		if (num10 > num11)
		{
			return -1;
		}
		return 1;
	}

	private static void ScanNumEnd(string s, int start, ref int end, ref int nzStart)
	{
		nzStart = start;
		end = start;
		bool flag = true;
		while (char.IsDigit(s, end))
		{
			if (flag && s[end].Equals('0'))
			{
				nzStart++;
			}
			else
			{
				flag = false;
			}
			end++;
			if (end >= s.Length)
			{
				break;
			}
		}
	}
}
