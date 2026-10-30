using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace GV.CounterSketch.Shared.Utilities;

public static class InProcFactory
{
	private struct HostRecord
	{
		public readonly string Address;

		public readonly ServiceHost Host;

		public HostRecord(ServiceHost host, string address)
		{
			Host = host;
			Address = address;
		}
	}

	private static readonly Uri BaseAddress;

	private static readonly Dictionary<Type, HostRecord> m_Hosts;

	private static readonly Binding NamedPipeBinding;

	static InProcFactory()
	{
		BaseAddress = new Uri("net.pipe://localhost/");
		m_Hosts = new Dictionary<Type, HostRecord>();
		NamedPipeBinding = new NetNamedPipeBinding
		{
			TransactionFlow = true
		};
		AppDomain.CurrentDomain.ProcessExit += delegate
		{
			foreach (KeyValuePair<Type, HostRecord> host in m_Hosts)
			{
				host.Value.Host.Close();
			}
		};
	}

	public static void CloseProxy<I>(I instance) where I : class
	{
		(instance as ICommunicationObject).Close();
	}

	public static I CreateInstance<S, I>() where S : I where I : class
	{
		HostRecord hostRecord = GetHostRecord<S, I>();
		return ChannelFactory<I>.CreateChannel(NamedPipeBinding, new EndpointAddress(hostRecord.Address));
	}

	private static HostRecord GetHostRecord<S, I>() where S : I where I : class
	{
		if (m_Hosts.ContainsKey(typeof(S)))
		{
			return m_Hosts[typeof(S)];
		}
		ServiceHost serviceHost = new ServiceHost(typeof(S), BaseAddress);
		string address = string.Concat(BaseAddress, Guid.NewGuid().ToString());
		HostRecord hostRecord = new HostRecord(serviceHost, address);
		m_Hosts.Add(typeof(S), hostRecord);
		serviceHost.AddServiceEndpoint(typeof(I), NamedPipeBinding, address);
		serviceHost.Open();
		return hostRecord;
	}
}
