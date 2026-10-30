using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace CS;

public static class CSUtilINI
{
	private const int MaxEntry = 32768;

	[DllImport("KERNEL32.DLL", CharSet = CharSet.Ansi, EntryPoint = "GetPrivateProfileStringA")]
	private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder returnValue, int size, string filePath);

	[DllImport("KERNEL32.DLL", CharSet = CharSet.Ansi, EntryPoint = "WritePrivateProfileStringA")]
	private static extern int WritePrivateProfileString(string section, string key, string value, string filePath);

	public static DateTime ReadDateTime(string filePath, string section, string key, DateTime defaultValue)
	{
		if (DateTime.TryParse(ReadString(filePath, section, key, defaultValue.ToString()), out var result))
		{
			return result;
		}
		return defaultValue;
	}

	public static DateTime ReadDateTime(string filePath, string section, string key)
	{
		return ReadDateTime(filePath, section, key, DateTime.Now);
	}

	public static string ReadString(string filePath, string section, string key, string defaultValue)
	{
		StringBuilder stringBuilder = new StringBuilder(32768);
		GetPrivateProfileString(section, key, defaultValue, stringBuilder, 32768, filePath);
		return stringBuilder.ToString();
	}

	public static string ReadString(string filePath, string section, string key)
	{
		return ReadString(filePath, section, key, string.Empty);
	}

	public static double ReadDouble(string filePath, string section, string key, double defaultValue)
	{
		if (double.TryParse(ReadString(filePath, section, key, defaultValue.ToString()), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
		{
			return result;
		}
		return defaultValue;
	}

	public static double ReadDouble(string filePath, string section, string key)
	{
		return ReadDouble(filePath, section, key, 0.0);
	}

	public static int ReadInt(string filePath, string section, string key, int defaultValue)
	{
		if (int.TryParse(ReadString(filePath, section, key, defaultValue.ToString()), out var result))
		{
			return result;
		}
		return defaultValue;
	}

	public static int ReadInt(string filePath, string section, string key)
	{
		return ReadInt(filePath, section, key, 0);
	}

	public static short ReadShort(string filePath, string section, string key, short defaultValue)
	{
		if (short.TryParse(ReadString(filePath, section, key, defaultValue.ToString()), out var result))
		{
			return result;
		}
		return defaultValue;
	}

	public static short ReadShort(string filePath, string section, string key)
	{
		return ReadShort(filePath, section, key, 0);
	}

	public static bool ReadBool(string filePath, string section, string key, bool defaultValue)
	{
		if (bool.TryParse(ReadString(filePath, section, key, defaultValue.ToString()), out var result))
		{
			return result;
		}
		return defaultValue;
	}

	public static bool ReadBool(string filePath, string section, string key)
	{
		return ReadBool(filePath, section, key, defaultValue: false);
	}

	public static bool WriteValue(string filePath, string section, string key, DateTime value)
	{
		return WritePrivateProfileString(section, key, value.ToString(), filePath) != 0;
	}

	public static bool WriteValue(string filePath, string section, string key, string value)
	{
		return WritePrivateProfileString(section, key, value, filePath) != 0;
	}

	public static bool WriteValue(string filePath, string section, string key, double value)
	{
		return WritePrivateProfileString(section, key, value.ToString(), filePath) != 0;
	}

	public static bool WriteValue(string filePath, string section, string key, int value)
	{
		return WritePrivateProfileString(section, key, value.ToString(), filePath) != 0;
	}

	public static bool WriteValue(string filePath, string section, string key, short value)
	{
		return WritePrivateProfileString(section, key, value.ToString(), filePath) != 0;
	}

	public static bool WriteValue(string filePath, string section, string key, bool value)
	{
		return WritePrivateProfileString(section, key, value.ToString(), filePath) != 0;
	}
}
