using System;
using System.IO;
using System.Text;

namespace GV.CounterSketch.Shared.Utilities.Helpers;

public static class FileFormatHelper
{
	public static void BinObjToObj(Stream sIn, Stream sOut)
	{
		using BinaryReader binaryReader = new BinaryReader(sIn, Encoding.UTF8, leaveOpen: true);
		using TextWriter textWriter = new StreamWriter(sOut, Encoding.UTF8, 1, leaveOpen: true);
		if (binaryReader.ReadByte() != 1)
		{
			return;
		}
		int num = binaryReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			double num2 = Math.Round(binaryReader.ReadSingle(), 6);
			double num3 = Math.Round(binaryReader.ReadSingle(), 6);
			double num4 = Math.Round(binaryReader.ReadSingle(), 6);
			string value = $"v {num2:0.######} {num3:0.######} {num4:0.######}";
			textWriter.WriteLine(value);
		}
		int num5 = binaryReader.ReadInt32();
		int count = num5 + 7 >> 3;
		byte[] array = binaryReader.ReadBytes(count);
		int num6 = 0;
		byte b = 1;
		for (int j = 0; j < num5; j++)
		{
			bool num7 = (array[num6] & b) == b;
			b <<= 1;
			if (b == 0)
			{
				b = 1;
				num6++;
			}
			if (num7)
			{
				textWriter.WriteLine("f {0} {1} {2} {3}", binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32());
			}
			else
			{
				textWriter.WriteLine("f {0} {1} {2}", binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32());
			}
		}
	}
}
