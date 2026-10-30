using System;
using System.Security.Cryptography;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class DataProtectionHelper
{
	private static readonly byte[] AdditionalEntropy = new byte[5] { 12, 8, 6, 4, 7 };

	public static byte[] GetBytes(this string source)
	{
		byte[] array = new byte[source.Length * 2];
		Buffer.BlockCopy(source.ToCharArray(), 0, array, 0, array.Length);
		return array;
	}

	public static string GetString(this byte[] source)
	{
		char[] array = new char[source.Length / 2];
		Buffer.BlockCopy(source, 0, array, 0, source.Length);
		return new string(array);
	}

	public static byte[] Protect(byte[] data)
	{
		try
		{
			return ProtectedData.Protect(data, AdditionalEntropy, DataProtectionScope.CurrentUser);
		}
		catch (CryptographicException ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog(ex.ToString());
			return null;
		}
	}

	public static byte[] Unprotect(byte[] data)
	{
		try
		{
			return ProtectedData.Unprotect(data, AdditionalEntropy, DataProtectionScope.CurrentUser);
		}
		catch (CryptographicException ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog(ex.ToString());
			return null;
		}
	}
}
