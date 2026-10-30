using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Geomagic.Utilities;

public class TcpServer
{
	private int _MaxConnections = 100;

	private ArrayList _Connections;

	private int _Port;

	private TcpListener _Listener;

	private TcpServiceProvider _Provider;

	private AsyncCallback _ConnectionReady;

	private WaitCallback _AcceptConnection;

	private AsyncCallback _ReceivedDataReady;

	public int MaxConnections
	{
		get
		{
			return _MaxConnections;
		}
		set
		{
			_MaxConnections = value;
		}
	}

	public int CurrentConnections
	{
		get
		{
			lock (this)
			{
				return _Connections.Count;
			}
		}
	}

	public TcpServer(TcpServiceProvider Provider, IPAddress Address, int Port)
	{
		_Provider = Provider;
		_Port = Port;
		_Listener = new TcpListener(Address, Port);
		_Connections = new ArrayList();
		_ConnectionReady = OnConnectionReady;
		_AcceptConnection = OnAcceptConnection;
		_ReceivedDataReady = OnReceivedDataReady;
	}

	private void OnConnectionReady(IAsyncResult ar)
	{
		lock (this)
		{
			if (_Listener != null)
			{
				TcpClient tcpClient = _Listener.EndAcceptTcpClient(ar);
				if (_Connections.Count >= _MaxConnections)
				{
					string text = "SE001: Server busy";
					tcpClient.Client.Send(Encoding.UTF8.GetBytes(text), 0, text.Length, SocketFlags.None);
					tcpClient.Client.Shutdown(SocketShutdown.Both);
					tcpClient.Close();
				}
				else
				{
					TcpConnectionState tcpConnectionState = new TcpConnectionState(tcpClient, this, (TcpServiceProvider)_Provider.Clone(), new byte[4]);
					_Connections.Add(tcpConnectionState);
					ThreadPool.QueueUserWorkItem(_AcceptConnection, tcpConnectionState);
				}
				_Listener.BeginAcceptTcpClient(_ConnectionReady, null);
			}
		}
	}

	private void OnAcceptConnection(object Parameter)
	{
		TcpConnectionState tcpConnectionState = Parameter as TcpConnectionState;
		try
		{
			tcpConnectionState.Provider.OnAcceptConnection(tcpConnectionState);
		}
		catch
		{
		}
		if (tcpConnectionState.Connection.Connected)
		{
			tcpConnectionState.Client.BeginReceive(tcpConnectionState.Buffer, 0, 0, SocketFlags.None, _ReceivedDataReady, tcpConnectionState);
		}
	}

	private void OnReceivedDataReady(IAsyncResult Result)
	{
		TcpConnectionState tcpConnectionState = Result.AsyncState as TcpConnectionState;
		if (!tcpConnectionState.Connected)
		{
			RemoveConnection(tcpConnectionState);
			return;
		}
		if (tcpConnectionState.Connection.Available == 0)
		{
			DropConnection(tcpConnectionState);
		}
		tcpConnectionState.Client.EndReceive(Result);
		try
		{
			tcpConnectionState.Provider.OnReceiveData(tcpConnectionState);
		}
		catch
		{
		}
		if (tcpConnectionState.Connection.Connected)
		{
			tcpConnectionState.Client.BeginReceive(tcpConnectionState.Buffer, 0, 0, SocketFlags.None, _ReceivedDataReady, tcpConnectionState);
		}
	}

	private void RemoveConnection(TcpConnectionState State)
	{
		if (_Connections.Contains(State))
		{
			_Connections.Remove(State);
		}
	}

	internal void DropConnection(TcpConnectionState State)
	{
		lock (this)
		{
			State.Client.Shutdown(SocketShutdown.Both);
			State.Connection.Close();
			RemoveConnection(State);
		}
	}

	public bool Start()
	{
		try
		{
			_Listener.Start();
			_Listener.BeginAcceptTcpClient(_ConnectionReady, null);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public void Stop()
	{
		lock (this)
		{
			_Listener.Stop();
			_Listener = null;
			foreach (object connection in _Connections)
			{
				TcpConnectionState tcpConnectionState = connection as TcpConnectionState;
				try
				{
					tcpConnectionState.Provider.OnDropConnection(tcpConnectionState);
				}
				catch
				{
				}
				tcpConnectionState.Client.Shutdown(SocketShutdown.Both);
				tcpConnectionState.Connection.Close();
			}
			_Connections.Clear();
		}
	}
}
