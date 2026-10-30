using System;
using System.Net.Configuration;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using GV.CounterSketchServices.WebServices;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class ConnectivityHelper
{
	public enum NetProtocol
	{
		FTP = 21,
		HTTP = 80,
		HTTPS = 443,
		BLOCKED = 0
	}

	public static string LastError { get; set; }

	public static bool ConnectivityValid()
	{
		try
		{
			LastError = string.Empty;
			using CounterSketchService counterSketchService = new CounterSketchService();
			string latestPublicCounterSketchVersionString = counterSketchService.GetLatestPublicCounterSketchVersionString();
			GlobalErrorHandler.WriteInfoToEventLog("Test Connectivity Result: " + latestPublicCounterSketchVersionString, 2);
			return true;
		}
		catch (Exception ex)
		{
			LastError = "Error Connecting to Internet Servers:\n" + ex.Message;
			GlobalErrorHandler.WriteErrorToEventLog("Error Connecting to Internet Servers:\n" + ex);
			return false;
		}
	}

	public static bool IsConnectedToInternet()
	{
		return NetworkInterface.GetIsNetworkAvailable();
	}

	public static bool IsStullerServicesAvailable()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		GlobalErrorHandler.WriteMethodInfo("IsStullerServicesAvailable");
		try
		{
			StullerAPIClient val = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)), "https://api.stuller.com/v2/", (Action<string>)null);
			try
			{
				val.Available();
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog(ex.ToString());
			return false;
		}
		return true;
	}

	public static bool IsStullerServicesAvailable(string username, byte[] password)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		GlobalErrorHandler.WriteMethodInfo("IsStullerServicesAvailable");
		try
		{
			StullerAPIClient val = new StullerAPIClient(username, Encoding.Default.GetString(DataProtectionHelper.Unprotect(password)), "https://api.stuller.com/v2/", (Action<string>)null);
			try
			{
				val.Available();
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog(ex.ToString());
			return false;
		}
		return true;
	}

	public static bool SetUseUnsafeHeaderParsing(bool allowUnsafeHeaderParsing)
	{
		Assembly assembly = Assembly.GetAssembly(typeof(SettingsSection));
		if (assembly == null)
		{
			return false;
		}
		Type type = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
		if (type == null)
		{
			return false;
		}
		object obj = type.InvokeMember("Section", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetProperty, null, null, new object[0]);
		if (obj == null)
		{
			return false;
		}
		FieldInfo field = type.GetField("useUnsafeHeaderParsing", BindingFlags.Instance | BindingFlags.NonPublic);
		if (field == null)
		{
			return false;
		}
		field.SetValue(obj, allowUnsafeHeaderParsing);
		return true;
	}

	public static NetProtocol TestProtocolToSite(string webURL)
	{
		LastError = "";
		try
		{
			new TcpClient(webURL, 80).Close();
			return NetProtocol.HTTP;
		}
		catch (Exception ex)
		{
			LastError = LastError + ex.Message + "\n\r";
			try
			{
				new TcpClient(webURL, 443).Close();
				return NetProtocol.HTTPS;
			}
			catch (Exception ex2)
			{
				LastError = LastError + ex2.Message + "\n\r";
				return NetProtocol.BLOCKED;
			}
		}
	}

	public static NetProtocol TestProtocolToSite(string webURL, int port)
	{
		LastError = "";
		try
		{
			new TcpClient(webURL, port).Close();
			return NetProtocol.HTTP;
		}
		catch (Exception ex)
		{
			LastError = LastError + ex.Message + "\n\r";
			return NetProtocol.BLOCKED;
		}
	}
}
