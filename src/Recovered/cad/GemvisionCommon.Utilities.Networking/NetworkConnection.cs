using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using GemvisionCommon.Helpers;
using GemvisionCommon.Utilities.Networking.Enums;
using GemvisionCommon.Utilities.Networking.Structs;

namespace GemvisionCommon.Utilities.Networking;

public sealed class NetworkConnection : IDisposable
{
	private const int CONNECT_TEMPORARY = 4;

	private const int CONNECT_UPDATE_PROFILE = 1;

	private const int ERROR_ALREADY_ASSIGNED = 85;

	private const int ERROR_INVALID_PARAMETER = 87;

	private const int ERROR_INVALID_PASSWORD = 86;

	private const int ERROR_LOGON_FAILURE = 1326;

	private const int ERROR_NO_MORE_ITEMS = 259;

	private const int ERROR_NOT_CONNECTED = 2250;

	private const int ERROR_SESSION_CREDENTIAL_CONFLICT = 1219;

	private const int NO_ERROR = 0;

	private const int RESOURCE_CONNECTED = 1;

	private const int RESOURCE_GLOBALNET = 2;

	private const int RESOURCEDISPLAYTYPE_SERVER = 2;

	private const int RESOURCETYPE_DISK = 1;

	private const int RESOURCEUSAGE_CONTAINER = 2;

	private readonly string _serverPath;

	public NetworkConnection(string serverPath, string username, string password, string domain = null)
	{
		_serverPath = serverPath;
		NetworkCredential networkCredential = GetNetworkCredential(username, password, domain);
		bool flag = Connect(_serverPath, networkCredential);
	}

	public NetworkConnection(string serverPath, NetworkCredential netCred)
	{
		_serverPath = serverPath;
		bool flag = Connect(_serverPath, netCred);
	}

	public static bool Connect(string serverPath, string username, string password, string domain = null)
	{
		NetworkCredential networkCredential = GetNetworkCredential(username, password, domain);
		return Connect(serverPath, networkCredential);
	}

	public static bool Connect(string serverPath, NetworkCredential netCred)
	{
		NetworkResource netResource = new NetworkResource
		{
			ResourceType = ResourceType.Disk,
			DisplayType = ResourceDisplayType.Share,
			RemoteName = serverPath
		};
		string username = (string.IsNullOrEmpty(netCred.Domain) ? netCred.UserName : $"{netCred.Domain}\\{netCred.UserName}");
		int num = WNetAddConnection2(ref netResource, netCred.Password, username, 4);
		string message = "Unknown Error";
		if (!Enum.IsDefined(typeof(NetworkConnectionResult), num))
		{
			throw new Win32Exception(num, message);
		}
		NetworkConnectionResult networkConnectionResult = (NetworkConnectionResult)num;
		if (networkConnectionResult == NetworkConnectionResult.NoError || networkConnectionResult == NetworkConnectionResult.ErrorSessionCredentialConflict)
		{
			return true;
		}
		message = GetErrorCodeDescription(networkConnectionResult);
		return false;
	}

	public static bool Disconnect(string serverPath)
	{
		int num = WNetCancelConnection2(serverPath, 1, force: true);
		string message = "Unknown Error";
		if (!Enum.IsDefined(typeof(NetworkConnectionResult), num))
		{
			throw new Win32Exception(num, message);
		}
		NetworkConnectionResult networkConnectionResult = (NetworkConnectionResult)num;
		if (networkConnectionResult == NetworkConnectionResult.NoError || networkConnectionResult == NetworkConnectionResult.ErrorNotConnected)
		{
			return true;
		}
		message = GetErrorCodeDescription(networkConnectionResult);
		throw new Win32Exception(num, message);
	}

	[DllImport("mpr.dll")]
	public static extern int WNetOpenEnum(int dwScope, int dwType, int dwUsage, NetworkResource lpNetResource, out IntPtr lphEnum);

	private static string GetErrorCodeDescription(NetworkConnectionResult networkConnectionResult)
	{
		string text = networkConnectionResult.GetDescription();
		if (string.IsNullOrWhiteSpace(text))
		{
			text = "Error description is  not available";
		}
		return text;
	}

	private static NetworkCredential GetNetworkCredential(string username, string password, string domain = null)
	{
		return string.IsNullOrWhiteSpace(domain) ? new NetworkCredential(username, password) : new NetworkCredential(username, password, domain);
	}

	[DllImport("mpr.dll")]
	private static extern int WNetAddConnection2(ref NetworkResource netResource, string password, string username, int flags);

	[DllImport("mpr.dll")]
	private static extern int WNetCancelConnection2(string name, int flags, bool force);

	[DllImport("Mpr.dll")]
	private static extern int WNetCloseEnum(IntPtr hEnum);

	[DllImport("mpr.dll")]
	private static extern int WNetEnumResource(IntPtr hEnum, ref uint lpcCount, IntPtr buffer, ref uint lpBufferSize);

	~NetworkConnection()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		Disconnect(_serverPath);
	}
}
