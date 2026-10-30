using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using Localization;

namespace GV.CounterSketch.Studio.Utilities.USB;

internal class CSKey
{
	public static List<int> DriveIndex1;

	public static List<string> DriveIndex2;

	public static List<string> DriveIndex3;

	public static byte[] securityfile;

	public static List<string> strMasterSerialNumber;

	public static List<string> strMasterUSBID;

	public static List<string> strProductID;

	public static List<string> strSerialNo;

	public static List<string> strVendorID;

	public static string[,] ValidIDs;

	public static void FindKey()
	{
		bool flag = false;
		strSerialNo = new List<string>();
		strMasterUSBID = new List<string>();
		strMasterSerialNumber = new List<string>();
		strProductID = new List<string>();
		strVendorID = new List<string>();
		foreach (ManagementObject item in new ManagementObjectSearcher(new ObjectQuery("select * from Win32_USBHub")).Get())
		{
			string text = item["PNPDeviceID"].ToString();
			if (text.Length <= 23 || !(text.Substring(0, 8) == "USB\\VID_"))
			{
				continue;
			}
			string text2 = text.Substring(8, 4);
			if (!(text.Substring(12, 5) == "&PID_"))
			{
				continue;
			}
			string text3 = text.Substring(17, 4);
			if (!(text.Substring(21, 1) == "\\"))
			{
				continue;
			}
			string text4 = text.Substring(22);
			for (int i = 0; i < 7; i++)
			{
				if (ValidIDs[i, 0] == text2 && ValidIDs[i, 1] == text3)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				if (text4.Length > 20)
				{
					text4 = text4.Substring(0, 20);
				}
				string text5 = "00000000000000000000" + text4;
				text5 = text5.Substring(text5.Length - 20);
				text5 = text5.Replace("&", "0");
				strSerialNo.Add(text4);
				strMasterUSBID.Add(text);
				strMasterSerialNumber.Add(text5);
				strProductID.Add(text3);
				strVendorID.Add(text2);
			}
		}
	}

	public static void FindKey2()
	{
		DriveIndex1 = new List<int>();
		DriveIndex2 = new List<string>();
		DriveIndex3 = new List<string>();
		ObjectQuery query = new ObjectQuery("select * from Win32_DiskDrive");
		if (strSerialNo == null)
		{
			MessageBox.Show(Strings.Error);
			return;
		}
		foreach (ManagementObject item in new ManagementObjectSearcher(query).Get())
		{
			if (item == null || item["PNPDeviceID"] == null)
			{
				continue;
			}
			string text = item["PNPDeviceID"].ToString();
			if (text == null)
			{
				continue;
			}
			string text2;
			if (text.Length > 50 && text.Substring(0, 50) == "USBSTOR\\DISK&VEN_USB007&PROD_MINI-USB2BU&REV_0.00\\")
			{
				text2 = text.Substring(50);
				if (text2.Length > 4)
				{
					text2 = text2.Substring(0, text2.Length - 2);
					for (int i = 0; i < strSerialNo.Count; i++)
					{
						if (text2 == strSerialNo[i])
						{
							DriveIndex1.Add(i);
							if (item["Index"] == null)
							{
								MessageBox.Show(Strings.Missing_Drive_Letter);
							}
							else
							{
								DriveIndex2.Add(item["Index"].ToString());
							}
							DriveIndex3.Add("");
						}
					}
				}
			}
			if (text.Length > 46 && text.Substring(0, 46) == "USBSTOR\\DISK&VEN_CBM&PROD_FLASH_DISK&REV_5.00\\")
			{
				text2 = text.Substring(46);
				if (text2.Length > 4)
				{
					text2 = text2.Substring(0, text2.Length - 2);
					for (int j = 0; j < strSerialNo.Count; j++)
					{
						if (text2 == strSerialNo[j])
						{
							DriveIndex1.Add(j);
							if (item["Index"] == null)
							{
								MessageBox.Show(Strings.Missing_Drive_Letter);
							}
							else
							{
								DriveIndex2.Add(item["Index"].ToString());
							}
							DriveIndex3.Add("");
						}
					}
				}
			}
			if (text.Length > 50 && text.Substring(0, 50) == "USBSTOR\\DISK&VEN_USB_2.0&PROD_FLASH_DISK&REV_5.00\\")
			{
				text2 = text.Substring(50);
				if (text2.Length > 4)
				{
					text2 = text2.Substring(0, text2.Length - 2);
					for (int k = 0; k < strSerialNo.Count; k++)
					{
						if (text2 == strSerialNo[k])
						{
							DriveIndex1.Add(k);
							if (item["Index"] == null)
							{
								MessageBox.Show(Strings.Missing_Drive_Letter);
							}
							else
							{
								DriveIndex2.Add(item["Index"].ToString());
							}
							DriveIndex3.Add("");
						}
					}
				}
			}
			if (text.Length > 56 && text.Substring(0, 56) == "USBSTOR\\DISK&VEN_USB_2.0&PROD_FLASH_DISK_IQC-5&REV_5.00\\")
			{
				text2 = text.Substring(56);
				if (text2.Length > 4)
				{
					text2 = text2.Substring(0, text2.Length - 2);
					for (int l = 0; l < strSerialNo.Count; l++)
					{
						if (text2 == strSerialNo[l])
						{
							DriveIndex1.Add(l);
							if (item["Index"] == null)
							{
								MessageBox.Show(Strings.Missing_Drive_Letter);
							}
							else
							{
								DriveIndex2.Add(item["Index"].ToString());
							}
							DriveIndex3.Add("");
						}
					}
				}
			}
			if (text.Length > 52 && text.Substring(0, 52) == "USBSTOR\\DISK&VEN_FLASH&PROD_DRIVE_AU_USB20&REV_8.07\\")
			{
				text2 = text.Substring(53);
				if (text2.Length > 4)
				{
					text2 = text2.Substring(0, text2.Length - 2);
					for (int m = 0; m < strSerialNo.Count; m++)
					{
						if (text2 == strSerialNo[m])
						{
							DriveIndex1.Add(m);
							if (item["Index"] == null)
							{
								MessageBox.Show(Strings.Missing_Drive_Letter);
							}
							else
							{
								DriveIndex2.Add(item["Index"].ToString());
							}
							DriveIndex3.Add("");
						}
					}
				}
			}
			if (text.Length > 49 && text.Substring(0, 49) == "USBSTOR\\DISK&VEN_USB2.0&PROD_FLASH_DISK&REV_2.20\\")
			{
				text2 = text.Substring(49);
				if (text2.Length > 4)
				{
					text2 = text2.Substring(0, text2.Length - 2);
					for (int n = 0; n < strSerialNo.Count; n++)
					{
						if (text2 == strSerialNo[n])
						{
							DriveIndex1.Add(n);
							if (item["Index"] == null)
							{
								MessageBox.Show(Strings.Missing_Drive_Letter);
							}
							else
							{
								DriveIndex2.Add(item["Index"].ToString());
							}
							DriveIndex3.Add("");
						}
					}
				}
			}
			if (text.Length <= 50 || !(text.Substring(0, 50) == "USBSTOR\\DISK&VEN_GENERIC&PROD_FLASH_DISK&REV_8.07\\"))
			{
				continue;
			}
			text2 = text.Substring(50);
			if (text2.Length <= 4)
			{
				continue;
			}
			text2 = text2.Substring(0, text2.Length - 2);
			for (int num = 0; num < strSerialNo.Count; num++)
			{
				if (text2 == strSerialNo[num])
				{
					DriveIndex1.Add(num);
					if (item["Index"] == null)
					{
						MessageBox.Show(Strings.Missing_Drive_Letter);
					}
					else
					{
						DriveIndex2.Add(item["Index"].ToString());
					}
					DriveIndex3.Add("");
				}
			}
		}
	}

	public static bool FindKey3()
	{
		if (DriveIndex1.Count == 0)
		{
			return false;
		}
		foreach (ManagementObject item in new ManagementObjectSearcher(new ObjectQuery("select * from Win32_LogicalDiskToPartition")).Get())
		{
			string text = item["Antecedent"].ToString();
			if (text.IndexOf("=") <= 0)
			{
				continue;
			}
			for (int i = 0; i < DriveIndex1.Count; i++)
			{
				text = text.Substring(text.IndexOf("=") + 1);
				string text2 = "\"Disk #" + DriveIndex2[i] + ", Partition #0\"";
				if (text == text2)
				{
					string text3 = item["Dependent"].ToString();
					if (text3.IndexOf("=") > 0)
					{
						DriveIndex3[i] = text3.Substring(text3.IndexOf("=") + 2, 2);
						break;
					}
				}
			}
		}
		return true;
	}

	public static bool FindKey4()
	{
		bool flag = false;
		int index = 0;
		for (int i = 0; i < DriveIndex3.Count; i++)
		{
			if (File.Exists(DriveIndex3[i] + "\\DO_NOT_DELETE\\CounterSketch.lic"))
			{
				flag = true;
				index = i;
			}
		}
		if (!flag)
		{
			return false;
		}
		int item = DriveIndex1[index];
		string item2 = DriveIndex2[index];
		string item3 = DriveIndex3[index];
		DriveIndex3.Clear();
		DriveIndex1.Clear();
		DriveIndex2.Clear();
		DriveIndex1.Add(item);
		DriveIndex2.Add(item2);
		DriveIndex3.Add(item3);
		securityfile = new byte[1024];
		FileStream fileStream = new FileStream(DriveIndex3[0] + "\\DO_NOT_DELETE\\CounterSketch.lic", FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		securityfile = binaryReader.ReadBytes(1024);
		binaryReader.Close();
		fileStream.Close();
		return true;
	}

	public static bool FindKeyInitial()
	{
		bool result = false;
		foreach (string item in from driveInfo in DriveInfo.GetDrives()
			where driveInfo.DriveType == DriveType.Removable && driveInfo.IsReady
			select driveInfo.RootDirectory.FullName)
		{
			if (File.Exists(Path.Combine(item, "do_not_delete\\countersketch.lic")))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public static bool GetCSNoKey()
	{
		if (securityfile == null || securityfile.Length < 564)
		{
			return false;
		}
		return (securityfile[563] & 4) == 4;
	}

	public static bool GetDesigner()
	{
		if (securityfile == null || securityfile.Length < 564)
		{
			return false;
		}
		return (securityfile[563] & 1) == 1;
	}

	public static bool GetExport()
	{
		if (securityfile == null || securityfile.Length < 564)
		{
			return false;
		}
		return (securityfile[563] & 2) == 2;
	}

	public static bool GetKeyGuid(ref Guid CSID)
	{
		if (securityfile == null)
		{
			return false;
		}
		if (securityfile.Length < 43)
		{
			return false;
		}
		if (securityfile[28] == 0 && securityfile[29] == 0 && securityfile[30] == 0)
		{
			return false;
		}
		byte[] array = new byte[16];
		for (int i = 0; i < 16; i++)
		{
			array[i] = securityfile[28 + i];
		}
		CSID = new Guid(array);
		return true;
	}

	public static bool GetNameCompany(ref string Name, ref string Company)
	{
		if (securityfile != null)
		{
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			int count = 255;
			for (int i = 0; i < 256; i++)
			{
				if (securityfile[i + 44] == 0)
				{
					count = i;
					break;
				}
			}
			Name = aSCIIEncoding.GetString(securityfile, 44, count);
			count = 255;
			for (int j = 0; j < 256; j++)
			{
				if (securityfile[j + 300] == 0)
				{
					count = j;
					break;
				}
			}
			Company = aSCIIEncoding.GetString(securityfile, 300, count);
		}
		return true;
	}

	public static int KeyCheckSerialCount()
	{
		if (strMasterSerialNumber != null)
		{
			return strMasterSerialNumber.Count;
		}
		return 0;
	}

	public static bool KeyDecode()
	{
		int index = DriveIndex1[0];
		new youknow(StrToByteArray("keNPLReqT2ENOzQp9UnJ4tIuPkQkMl2FEMwdrDCiSCEsNUZOOydrFtl4Bjat2LaiLKxltke3LwhCChpu0dFMqokw2h3OyHKuXM5rKMDLW5KLlvKvOEHoouP5ORTDAqON")).Decipher(securityfile, 568);
		new youknow(StrToByteArray(strProductID[index] + strMasterSerialNumber[index] + strVendorID[index])).Decipher(securityfile, 568);
		new youknow(StrToByteArray("JjrcfZuANoOKFGzvSn8Wt23BvBAULDbqVF5vvBvXocUWBwc6slXMEYx3aUMWNeY7FNbXYvlk2NwtoiDbXHaVsMGzD9jGaoZUvv6TS7YKQtQT5ruyq6wykj6ZDwxhs1mU")).Decipher(securityfile, 568);
		uint num = 0u;
		for (int i = 0; i < 567; i++)
		{
			num ^= securityfile[i];
		}
		if (num != securityfile[567])
		{
			return false;
		}
		if (securityfile[557] != 2)
		{
			return false;
		}
		if (securityfile[559] < 100)
		{
			return false;
		}
		return true;
	}

	public static void SetIDs()
	{
		ValidIDs = new string[7, 2];
		ValidIDs[0, 0] = "0457";
		ValidIDs[0, 1] = "0151";
		ValidIDs[1, 0] = "1307";
		ValidIDs[1, 1] = "0163";
		ValidIDs[2, 0] = "0204";
		ValidIDs[2, 1] = "6025";
		ValidIDs[3, 0] = "058F";
		ValidIDs[3, 1] = "6387";
		ValidIDs[4, 0] = "1E3D";
		ValidIDs[4, 1] = "2092";
		ValidIDs[5, 0] = "1221";
		ValidIDs[5, 1] = "3234";
		ValidIDs[6, 0] = "0001";
		ValidIDs[6, 1] = "7778";
	}

	private static byte[] StrToByteArray(string str)
	{
		return new ASCIIEncoding().GetBytes(str);
	}
}
