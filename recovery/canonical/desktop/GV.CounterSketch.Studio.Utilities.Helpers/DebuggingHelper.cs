using System;
using System.ComponentModel;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketchServices.WebServices;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

internal class DebuggingHelper
{
	public static string GetCurrentUserLicenseInfo()
	{
		return string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat($"Key Id:{UIHelper.CounterSketchOptions.KeyId}" + $"|Username:{UIHelper.CounterSketchOptions.Username}", $"|Email:{UIHelper.CounterSketchOptions.Email}"), $"|Phone:{UIHelper.CounterSketchOptions.Phone}"), $"|Company Name:{UIHelper.CounterSketchOptions.Company}"), $"|Stuller BT Account Number:{UIHelper.CounterSketchOptions.BillToAccountNumber}"), $"|IP Address:{GetCurrentUsersIPAddress()}"), $"|CS Version:{UIHelper.CounterSketchOptions.ApplicationCurrentVersion}"), $"|Rhino Version:{UIHelper.CounterSketchOptions.ApplicationCurrentRhinoBuildDate}");
	}

	public static string GetCurrentUsersIPAddress()
	{
		try
		{
			IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
			foreach (IPAddress iPAddress in addressList)
			{
				if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					return iPAddress.ToString();
				}
			}
		}
		catch
		{
		}
		return "UNKNOWN";
	}

	public static string GetSystemInformation()
	{
		string text = "";
		try
		{
			text += "-----------------------------------<br />";
			text += "<b>CS LICENSE INFORMATION</b>";
			text += "-----------------------------------<br />";
			text += $"Key Id: <b>{UIHelper.CounterSketchOptions.KeyId}</b><br />";
			text += $"Username: <b>{UIHelper.CounterSketchOptions.Username}</b><br />";
			text += $"Email: <b>{UIHelper.CounterSketchOptions.Email}</b><br />";
			text += $"Phone: <b>{UIHelper.CounterSketchOptions.Phone}</b><br />";
			text += $"Company Name: <b>{UIHelper.CounterSketchOptions.Company}</b><br />";
			text += $"Stuller BT Account Number: <b>{UIHelper.CounterSketchOptions.BillToAccountNumber}</b><br />";
			text += $"IP Address: <b>{GetCurrentUsersIPAddress()}</b><br />";
			foreach (ManagementObject item in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem").Get())
			{
				text += "-----------------------------------<br />";
				text += "<b>OPERATING SYSTEM</b>";
				text += "-----------------------------------<br />";
				text += string.Format("OS Name: <b>{0}</b><br />", item["Name"]);
				text += string.Format("OS Version: <b>{0}</b><br />", item["Version"]);
				text += string.Format("OS Service Pack: <b>{0}.{1}</b><br />", item["ServicePackMajorVersion"], item["ServicePackMinorVersion"]);
				text += string.Format("OS Locale: <b>{0}</b><br />", item["Locale"]);
			}
			foreach (ManagementObject item2 in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem").Get())
			{
				text += "-----------------------------------<br />";
				text += "<b>PC INFORMATION</b>";
				text += "-----------------------------------<br />";
				text += string.Format("Bootup State: <b>{0}</b><br />", item2["BootupState"]);
				text += string.Format("DNS HostName: <b>{0}</b><br />", item2["DNSHostName"]);
				text += string.Format("Domain: <b>{0}</b><br />", item2["Domain"]);
				text += string.Format("Number Of Logical Processors: <b>{0}</b><br />", item2["NumberOfLogicalProcessors"]);
				text += string.Format("Number Of Processors: <b>{0}</b><br />", item2["NumberOfProcessors"]);
				text += string.Format("Part Of Domain: <b>{0}</b><br />", item2["PartOfDomain"]);
				text += string.Format("Primary Owner Contact: <b>{0}</b><br />", item2["PrimaryOwnerContact"]);
				text += string.Format("Primary Owner Name: <b>{0}</b><br />", item2["PrimaryOwnerName"]);
				text += string.Format("System Type: <b>{0}</b><br />", item2["SystemType"]);
				text += string.Format("Total Physical Memory: <b>{0}</b> bytes<br />", item2["TotalPhysicalMemory"]);
				text += string.Format("UserName: <b>{0}</b><br />", item2["UserName"]);
				text += string.Format("Workgroup: <b>{0}</b><br />", item2["Workgroup"]);
			}
			text += "-----------------------------------<br />";
			text += "<b>NETWORK INFORMATION</b>";
			text += "-----------------------------------<br />";
			text += $"Internet Accessible: <b>{InternetUtility.IsConnectedToInternet()}</b><br />";
			text += string.Format("Stuller Accessible via: <b>{0}</b><br />", InternetUtility.TestProtocolToSite("www.stuller.com"));
			text += string.Format("CS Serice Accessible via: <b>{0}</b><br />", InternetUtility.TestProtocolToSite("CounterSketch.biz"));
			text += "-----------------------------------<br />";
			text += "<b>MONITOR INFORMATION</b>";
			text += "-----------------------------------<br />";
			foreach (ManagementObject item3 in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DesktopMonitor").Get())
			{
				text += string.Format("DeviceID: <b>{0}</b><br />", item3["DeviceID"]);
				text += string.Format("Monitor Manufacturer: <b>{0}</b><br />", item3["MonitorManufacturer"]);
				text += string.Format("Name: <b>{0}</b><br />", item3["Name"]);
				text += string.Format("Pixels Per X Logical Inch: <b>{0}</b><br />", item3["PixelsPerXLogicalInch"]);
				text += string.Format("Pixels Per Y Logical Inch: <b>{0}</b><br />", item3["PixelsPerYLogicalInch"]);
				text += string.Format("Screen Height: <b>{0}</b><br />", item3["ScreenHeight"]);
				text += string.Format("Screen Width: <b>{0}</b><br />", item3["ScreenWidth"]);
				text += "-----------------------------------<br />";
			}
			text += "<b>DISPLAY ADAPTER INFORMATION</b>";
			text += "-----------------------------------<br />";
			foreach (ManagementObject item4 in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController").Get())
			{
				text += string.Format("Mfr: <b>{0}</b><br />", item4["AdapterCompatibility"]);
				text += string.Format("Name: <b>{0}</b><br />", item4["Name"]);
				text += string.Format("Description: <b>{0}</b><br />", item4["Description"]);
				text += string.Format("Adapter DAC Type: <b>{0}</b><br />", item4["AdapterDACType"]);
				text += string.Format("Adapter RAM: <b>{0}</b><br />", item4["AdapterRAM"]);
				text += string.Format("Driver Date: <b>{0}</b><br />", item4["DriverDate"]);
				text += string.Format("Driver Version: <b>{0}</b><br />", item4["DriverVersion"]);
				text += string.Format("Current Bits Per Pixel: <b>{0}</b><br />", item4["CurrentBitsPerPixel"]);
				text += string.Format("Current Horizontal Resolution: <b>{0}</b><br />", item4["CurrentHorizontalResolution"]);
				text += string.Format("Current Number Of Colors: <b>{0}</b><br />", item4["CurrentNumberOfColors"]);
				text += string.Format("Current Number Of Columns: <b>{0}</b><br />", item4["CurrentNumberOfColumns"]);
				text += string.Format("Current Number Of Rows: <b>{0}</b><br />", item4["CurrentNumberOfRows"]);
				text += string.Format("Current Refresh Rate: <b>{0}</b><br />", item4["CurrentRefreshRate"]);
				text += string.Format("Current Scan Mode: <b>{0}</b><br />", item4["CurrentScanMode"]);
				text += string.Format("Current VerticalResolution: <b>{0}</b><br />", item4["CurrentVerticalResolution"]);
				text += string.Format("Device ID: <b>{0}</b><br />", item4["DeviceID"]);
				text += string.Format("Installed Display Drivers: <b>{0}</b><br />", item4["InstalledDisplayDrivers"]);
				text += string.Format("Max Refresh Rate: <b>{0}</b><br />", item4["MaxRefreshRate"]);
				text += string.Format("Min Refresh Rate: <b>{0}</b><br />", item4["MinRefreshRate"]);
				text += string.Format("PNP Device ID: <b>{0}</b><br />", item4["PNPDeviceID"]);
				text += string.Format("Video Mode: <b>{0}</b><br />", item4["VideoMode"]);
				text += string.Format("Video Mode Description: <b>{0}</b><br />", item4["VideoModeDescription"]);
				text += string.Format("Video Processor: <b>{0}</b><br />", item4["VideoProcessor"]);
				text += "-----------------------------------<br />";
			}
			text += "<b>DRIVE INFORMATION</b>";
			text += "-----------------------------------<br />";
			foreach (ManagementObject item5 in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_LogicalDisk").Get())
			{
				text += string.Format("Name: <b>{0}</b><br />", item5["Name"]);
				text += string.Format("Source: <b>{0}</b><br />", item5["ProviderName"]);
				text += string.Format("Size: <b>{0}</b><br />", item5["Size"]);
				text += string.Format("FreeSpace: <b>{0}</b><br />", item5["FreeSpace"]);
				text += "-----------------------------------<br />";
			}
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog($"Error getting PC Info: {ex.Message}\n\n{ex.StackTrace}");
		}
		return text ?? "";
	}

	public static string GetTimeStamp()
	{
		return $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} : ";
	}

	public static string GetUserComments()
	{
		WindowMessage windowMessage = new WindowMessage(WindowMessage.WindowType.QuestionAnswer);
		windowMessage.Title = "Send Report";
		windowMessage.Question = "Describe the problem you are having.";
		windowMessage.Owner = Application.Current.MainWindow;
		windowMessage.ShowDialog();
		windowMessage.Focus();
		return windowMessage.Answer;
	}

	public static void PrintTimedOutput(string message)
	{
	}

	public static void SendDebugReport(string userComments)
	{
		if (!UIHelper.IsInternetAccessible())
		{
			return;
		}
		using CounterSketchService counterSketchService = new CounterSketchService();
		counterSketchService.TransmitDetailedDebuggingReportCompleted += CssTransmitDebuggingReportCompleted;
		counterSketchService.TransmitDetailedDebuggingReportAsync(UIHelper.CounterSketchOptions.Username, UIHelper.CounterSketchOptions.GetKeyId(), UIHelper.CounterSketchOptions.Email, UIHelper.CounterSketchOptions.Phone, userComments, UIHelper.VersionString, string.Empty, "<PRE>" + GlobalErrorHandler.GetLogEntriesAsString() + "</PRE>", GetSystemInformation(), debugMode: false);
	}

	public static void SendQuickDebugReport()
	{
		SendDebugReport(string.Empty);
	}

	private static void CssTransmitDebuggingReportCompleted(object sender, AsyncCompletedEventArgs e)
	{
		UIHelper.RaiseApplicationMessageEvent("DebuggingHelper", ApplicationMessageType.MultiLineInfo, "Your debugging report has been transmitted.  Thank you.", messageIsModal: true);
	}
}
