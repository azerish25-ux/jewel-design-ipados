using System;

namespace Geomagic.Utilities;

public abstract class TcpServiceProvider : ICloneable
{
	public virtual object Clone()
	{
		return null;
	}

	public abstract void OnAcceptConnection(TcpConnectionState state);

	public abstract void OnReceiveData(TcpConnectionState state);

	public abstract void OnDropConnection(TcpConnectionState state);
}
