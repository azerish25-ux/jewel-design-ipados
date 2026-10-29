using System;
using GemvisionCommon.Utilities.Networking.Enums;

namespace GemvisionCommon.Utilities.Networking.Structs;

[Serializable]
public struct NetworkConnectionConfig
{
	public NetworkConnectionType ConnectionType { get; set; }

	public string Path { get; set; }

	public string Domain { get; set; }

	public string UserName { get; set; }

	public string Password { get; set; }

	public override string ToString()
	{
		return Path;
	}
}
