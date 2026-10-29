using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace GV.CounterSketch.Shared;

public class InternetUtility
{
	public enum NetProtocol
	{
		FTP = 21,
		HTTP = 80,
		HTTPS = 443,
		BLOCKED = 0
	}

	public static string LastError = "";

	[DllImport("wininet.dll")]
	private static extern bool InternetGetConnectedState(out int Description, int ReservedValue);

	public static bool IsConnectedToInternet()
	{
		return NetworkInterface.GetIsNetworkAvailable();
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
