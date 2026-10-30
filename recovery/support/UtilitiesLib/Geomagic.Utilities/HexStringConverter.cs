using System;

namespace Geomagic.Utilities;

public class HexStringConverter
{
	public static byte[] ToByteArray(string HexString)
	{
		int length = HexString.Length;
		byte[] array = new byte[length / 2];
		for (int i = 0; i < length; i += 2)
		{
			array[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
		}
		return array;
	}
}
