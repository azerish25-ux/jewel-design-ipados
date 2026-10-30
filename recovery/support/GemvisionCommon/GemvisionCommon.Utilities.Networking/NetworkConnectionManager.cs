using System;
using System.Collections.Generic;
using GemvisionCommon.Utilities.Logging;
using GemvisionCommon.Utilities.Networking.Enums;
using GemvisionCommon.Utilities.Networking.Structs;

namespace GemvisionCommon.Utilities.Networking;

public class NetworkConnectionManager
{
	public delegate void NetworkConnectionManagerExceptionHandler(Exception ex);

	private NetworkConnectionManagerExceptionHandler _onNetworkConnectionManagerException;

	private readonly Dictionary<NetworkConnectionConfig, NetworkConnection> _networkConnections = new Dictionary<NetworkConnectionConfig, NetworkConnection>();

	private readonly Logger _logger;

	public NetworkConnectionManagerExceptionHandler OnNetworkConnectionManagerException
	{
		set
		{
			_onNetworkConnectionManagerException = value;
		}
	}

	public NetworkConnectionManager()
	{
	}

	public NetworkConnectionManager(Logger logger)
	{
		_logger = logger;
	}

	public NetworkConnectionManager(NetworkConnectionManagerExceptionHandler onNetworkConnectionManagerException)
	{
		_onNetworkConnectionManagerException = onNetworkConnectionManagerException;
	}

	public string GetNetworkConnectionPath(NetworkConnectionConfig netConnConfig)
	{
		if (_networkConnections.ContainsKey(netConnConfig))
		{
			return netConnConfig.Path;
		}
		if (netConnConfig.ConnectionType != NetworkConnectionType.Network)
		{
			return netConnConfig.Path;
		}
		try
		{
			NetworkConnection value = new NetworkConnection(netConnConfig.Path, netConnConfig.UserName, netConnConfig.Password, netConnConfig.Domain);
			_networkConnections.Add(netConnConfig, value);
		}
		catch (ArgumentException ex)
		{
			_logger?.WriteToLog(ex);
			_onNetworkConnectionManagerException?.Invoke(ex);
		}
		return netConnConfig.Path;
	}
}
