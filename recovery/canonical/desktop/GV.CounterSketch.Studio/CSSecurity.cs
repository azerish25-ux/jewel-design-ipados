using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using GV.CounterSketch.Studio.SecurityServices;
using GV.CounterSketch.Studio.Utilities.Helpers;
using Microsoft.Win32;

namespace GV.CounterSketch.Studio;

public class CSSecurity
{
	public enum SecurityResult
	{
		ValidKey,
		NewComputerRegistered,
		ErrorInDataSentToService,
		IncorrectComputer,
		InvalidKey,
		InvalidServer,
		KeyDisabled
	}

	public void ByteToString(ref byte[] arrIn, ref string strOut)
	{
		byte[] arrOut = null;
		ByteToBase24(ref arrIn, ref arrOut);
		Base24ToString(ref arrOut, ref strOut);
	}

	public static byte[] ConvertSerialNumberToBytes(string code)
	{
		byte[] arrOut = null;
		if (!StringToBase24(ref code, ref arrOut))
		{
			return null;
		}
		byte[] arrOut2 = null;
		Base24ToByte(ref arrOut, ref arrOut2);
		if (CheckBValue(ref arrOut2))
		{
			return null;
		}
		return arrOut2;
	}

	public static string GetDescriptionFromSecurityResult(SecurityResult securityResult)
	{
		return securityResult switch
		{
			SecurityResult.ValidKey => "Valid Serial Code.", 
			SecurityResult.NewComputerRegistered => "The Serial Code entered is now registered to this Computer.", 
			SecurityResult.ErrorInDataSentToService => "Error in the data sent to the validation service.", 
			SecurityResult.IncorrectComputer => "The Serial Code entered is registered to another computer.", 
			SecurityResult.InvalidKey => "There was an error validating your Serial Code.", 
			SecurityResult.KeyDisabled => "This Serial Code is disabled, please contact CounterSketch@gemvision.com.", 
			SecurityResult.InvalidServer => "Invalid Server.  Contact Support to verify your license.", 
			_ => "There was an error validating your Serial Code.", 
		};
	}

	public static SecurityResult SecurityCodeValidate(ref byte[] bytecode)
	{
		GlobalErrorHandler.WriteMethodInfo("SecurityCodeValidate");
		Guid guid = new Guid("{9F0D30F0-6671-4503-8E27-8A5E8EDB9368}");
		string username = UIHelper.CounterSketchOptions.Username;
		string company = UIHelper.CounterSketchOptions.Company;
		byte[] volumeSerial = GetVolumeSerial("C");
		byte[] array = new byte[15];
		byte[] array2 = bytecode;
		byte[] array3 = new byte[6];
		byte[] array4 = Guid.NewGuid().ToByteArray();
		byte[] array5 = guid.ToByteArray();
		byte[] array6 = new byte[67];
		int num = 0;
		for (int i = 0; i < volumeSerial.Length; i++)
		{
			array6[num] = volumeSerial[i];
			num++;
		}
		for (int j = 0; j < array.Length; j++)
		{
			array6[num] = array[j];
			num++;
		}
		for (int k = 0; k < array3.Length; k++)
		{
			array6[num] = array3[k];
			num++;
		}
		for (int l = 0; l < array2.Length; l++)
		{
			array6[num] = array2[l];
			num++;
		}
		for (int m = 0; m < array4.Length; m++)
		{
			array6[num] = array4[m];
			num++;
		}
		for (int n = 0; n < array5.Length; n++)
		{
			array6[num] = array5[n];
			num++;
		}
		SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
		sHA1CryptoServiceProvider.ComputeHash(array6);
		byte[] hash = sHA1CryptoServiceProvider.Hash;
		byte[] array7 = new byte[71];
		num = 0;
		for (int num2 = 0; num2 < array6.Length - 16; num2++)
		{
			array7[num] = array6[num2];
			num++;
		}
		for (int num3 = 0; num3 < hash.Length; num3++)
		{
			array7[num] = hash[num3];
			num++;
		}
		byte[] array8 = null;
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		try
		{
			using SecurityServiceClient securityServiceClient = new SecurityServiceClient("BasicHttpsBinding_ISecurityService");
			array8 = securityServiceClient.VerifyKey(array7, username, company);
		}
		catch (Exception arg)
		{
			GlobalErrorHandler.WriteErrorToEventLog($"Unabled to validate security key. {arg}");
			return SecurityResult.InvalidKey;
		}
		stopwatch.Stop();
		GlobalErrorHandler.WriteInfoToEventLog("VerifyKey() Service Call Time: " + stopwatch.ElapsedMilliseconds, 2);
		byte[] array9 = new Guid("{9F0D30F0-6671-4503-8E27-8A5E8EDB9368}").ToByteArray();
		num = 0;
		byte[] array10 = new byte[39];
		for (int num4 = 0; num4 < array10.Length - 16; num4++)
		{
			array10[num] = array8[num4];
			num++;
		}
		for (int num5 = 0; num5 < array9.Length; num5++)
		{
			array10[num] = array9[num5];
			num++;
		}
		byte[] array11 = new byte[20];
		num = 23;
		for (int num6 = 0; num6 < 20; num6++)
		{
			array11[num6] = array8[num];
			num++;
		}
		sHA1CryptoServiceProvider.ComputeHash(array10);
		byte[] hash2 = sHA1CryptoServiceProvider.Hash;
		SecurityResult securityResult = (SecurityResult)array8[0];
		if (securityResult == SecurityResult.ValidKey)
		{
			for (int num7 = 0; num7 < 20; num7++)
			{
				if (array11[num7] != hash2[num7])
				{
					securityResult = SecurityResult.InvalidKey;
				}
			}
		}
		if (securityResult == SecurityResult.ValidKey)
		{
			for (int num8 = 0; num8 < 16; num8++)
			{
				if (array8[num8 + 1 + 4 + 2] != array4[num8])
				{
					securityResult = SecurityResult.InvalidServer;
				}
			}
		}
		if (securityResult == SecurityResult.ValidKey || securityResult == SecurityResult.NewComputerRegistered)
		{
			int keyId = array8[1] | (array8[2] << 8) | (array8[3] << 16) | (array8[4] << 24);
			string countryCode = Convert.ToChar(array8[5]).ToString() + Convert.ToChar(array8[6]);
			UIHelper.CounterSketchOptions.SetKeyId(keyId);
			UIHelper.CounterSketchOptions.CountryCode = countryCode;
			UIHelper.CounterSketchOptions.WriteSettings();
		}
		return securityResult;
	}

	private static void Base24ToByte(ref byte[] arrIn, ref byte[] arrOut)
	{
		arrOut = new byte[10];
		for (int i = 0; i < 10; i++)
		{
			arrOut[i] = 0;
		}
		for (int num = 15; num >= 0; num--)
		{
			int num2 = arrIn[num];
			for (int j = 0; j < 10; j++)
			{
				num2 += arrOut[j] * 24;
				arrOut[j] = (byte)(num2 % 256);
				num2 /= 256;
			}
		}
	}

	private void Base24ToString(ref byte[] arrIn, ref string strOut)
	{
		string text = "BCDFGHJKMPQRTVWXY2346789";
		strOut = "";
		for (int i = 0; i < 16; i++)
		{
			if (i % 4 == 0 && i != 0)
			{
				strOut += "-";
			}
			strOut += text.Substring(arrIn[i], 1);
		}
	}

	private void ByteToBase24(ref byte[] arrIn, ref byte[] arrOut)
	{
		byte[] array = new byte[10];
		for (int i = 0; i < 10; i++)
		{
			array[i] = arrIn[i];
		}
		arrOut = new byte[16];
		for (int j = 0; j < 16; j++)
		{
			int num = 0;
			for (int num2 = 9; num2 >= 0; num2--)
			{
				num = num * 256 + array[num2];
				array[num2] = (byte)(num / 24);
				num %= 24;
			}
			arrOut[j] = (byte)num;
		}
	}

	private static bool CheckBValue(ref byte[] b)
	{
		if (b == null)
		{
			return false;
		}
		if (b.Length != 10)
		{
			return false;
		}
		int num = 0;
		for (int num2 = 9; num2 > 0; num2--)
		{
			num += b[num2];
			num %= 256;
		}
		return num != b[0];
	}

	private static byte[] GetKeyB()
	{
		if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major == 10)
		{
			return new byte[15];
		}
		string subKey = "Software\\Microsoft\\Windows NT\\CurrentVersion";
		int phkResult = 0;
		byte[] array = null;
		if (RegOpenKeyEx(RegistryHive.LocalMachine, ref subKey, 0, 131353, ref phkResult) == 0)
		{
			string lpValueName = "DigitalProductId";
			int dataSize = 0;
			int lpType = 3;
			byte[] data = new byte[300];
			RegQueryValueEx(phkResult, ref lpValueName, 0, ref lpType, data, ref dataSize);
			data = new byte[dataSize];
			if (RegQueryValueEx(phkResult, ref lpValueName, 0, ref lpType, data, ref dataSize) == 0)
			{
				try
				{
					RegCloseKey(dataSize);
				}
				catch (Exception arg)
				{
					GlobalErrorHandler.WriteErrorToEventLog($"GetKeyB: Exception closing registry key - {arg}.");
				}
				array = data;
			}
		}
		byte[] array2 = new byte[15];
		for (int i = 0; i <= 14; i++)
		{
			array2[i] = array[52 + i];
		}
		return array2;
	}

	[DllImport("kernel32.dll")]
	private static extern long GetVolumeInformation(string PathName, StringBuilder VolumeNameBuffer, uint VolumeNameSize, ref uint VolumeSerialNumber, ref uint MaximumComponentLength, ref uint FileSystemFlags, StringBuilder FileSystemNameBuffer, uint FileSystemNameSize);

	private static byte[] GetVolumeSerial(string strDriveLetter)
	{
		uint VolumeSerialNumber = 0u;
		uint MaximumComponentLength = 0u;
		StringBuilder stringBuilder = new StringBuilder(256);
		uint FileSystemFlags = 0u;
		StringBuilder stringBuilder2 = new StringBuilder(256);
		strDriveLetter += ":\\";
		GetVolumeInformation(strDriveLetter, stringBuilder, (uint)stringBuilder.Capacity, ref VolumeSerialNumber, ref MaximumComponentLength, ref FileSystemFlags, stringBuilder2, (uint)stringBuilder2.Capacity);
		return new byte[4]
		{
			(byte)(VolumeSerialNumber % 256),
			(byte)((VolumeSerialNumber >> 8) % 256),
			(byte)((VolumeSerialNumber >> 16) % 256),
			(byte)((VolumeSerialNumber >> 24) % 256)
		};
	}

	[DllImport("advapi32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
	private static extern int RegCloseKey(int hKey);

	[DllImport("advapi32.dll")]
	private static extern int RegOpenKeyEx(RegistryHive hKey, [MarshalAs(UnmanagedType.VBByRefStr)] ref string subKey, int options, int sam, ref int phkResult);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegQueryValueEx(int hKey, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpValueName, int lpReserved, ref int lpType, [Out] byte[] data, ref int dataSize);

	private static bool StringToBase24(ref string strIn, ref byte[] arrOut)
	{
		string text = "BCDFGHJKMPQRTVWXY2346789";
		string text2 = strIn;
		text2 = text2.Replace("-", "");
		text2 = text2.ToUpper();
		if (text2.Length != 16)
		{
			return false;
		}
		arrOut = new byte[16];
		for (int i = 0; i < 16; i++)
		{
			int num = text.IndexOf(text2.Substring(i, 1));
			if (num == -1)
			{
				return false;
			}
			arrOut[i] = (byte)num;
		}
		return true;
	}
}
