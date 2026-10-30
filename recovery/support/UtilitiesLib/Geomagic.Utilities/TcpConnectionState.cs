using System.Net.Sockets;

namespace Geomagic.Utilities;

public class TcpConnectionState
{
	private TcpClient _Connection;

	private TcpServer _Server;

	private TcpServiceProvider _Provider;

	private byte[] _Buffer;

	public TcpClient Connection
	{
		get
		{
			return _Connection;
		}
		private set
		{
			_Connection = value;
		}
	}

	public Socket Client => Connection.Client;

	public TcpServer Server
	{
		get
		{
			return _Server;
		}
		private set
		{
			_Server = value;
		}
	}

	public TcpServiceProvider Provider
	{
		get
		{
			return _Provider;
		}
		private set
		{
			_Provider = value;
		}
	}

	public byte[] Buffer
	{
		get
		{
			return _Buffer;
		}
		private set
		{
			_Buffer = value;
		}
	}

	public int AvailableData => _Connection.Available;

	public bool Connected => _Connection.Connected;

	public TcpConnectionState(TcpClient Connection, TcpServer Server, TcpServiceProvider Provider, byte[] Buffer)
	{
		this.Connection = Connection;
		this.Server = Server;
		this.Provider = Provider;
		this.Buffer = Buffer;
	}

	public int Read(byte[] Buffer, int Offset, int Count)
	{
		try
		{
			if (Connection.Available > 0)
			{
				return Client.Receive(Buffer, Offset, Count, SocketFlags.None);
			}
			return 0;
		}
		catch
		{
			return 0;
		}
	}

	public bool Write(byte[] Buffer, int Offset, int Count)
	{
		try
		{
			Client.Send(Buffer, Offset, Count, SocketFlags.None);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public void EndConnection()
	{
		if (Connection != null && Connection.Connected)
		{
			Client.Shutdown(SocketShutdown.Both);
			Connection.Close();
		}
		Server.DropConnection(this);
	}
}
