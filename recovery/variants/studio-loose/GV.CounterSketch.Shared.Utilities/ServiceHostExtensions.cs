using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace GV.CounterSketch.Shared.Utilities;

public static class ServiceHostExtensions
{
	public const string EndpointBaseAddress = "net.tcp://localhost:8556/RhinoServices";

	public const string EndpointMexAddress = "http://localhost:8556/RhinoServices/mex";

	public static void AddHttpMexBindingAndEndpoint(this ServiceHost serviceHost)
	{
		Uri address = new Uri("http://localhost:8556/RhinoServices/mex");
		ServiceMetadataBehavior serviceMetadataBehavior = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
		if (serviceMetadataBehavior == null)
		{
			serviceMetadataBehavior = new ServiceMetadataBehavior();
			serviceHost.Description.Behaviors.Add(serviceMetadataBehavior);
		}
		Binding binding = MetadataExchangeBindings.CreateMexHttpBinding();
		serviceHost.AddServiceEndpoint("IMetadataExchange", binding, address);
	}

	public static void AddNetTcpBindingAndEndpoint<TInterface>(this ServiceHost serviceHost)
	{
		serviceHost.AddServiceEndpoint(typeof(TInterface), GetNetTcpBinding(), "");
	}

	public static void AddNetTcpMexBindingAndEndpoint(this ServiceHost serviceHost)
	{
		ServiceMetadataBehavior serviceMetadataBehavior = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
		if (serviceMetadataBehavior == null)
		{
			serviceMetadataBehavior = new ServiceMetadataBehavior();
			serviceHost.Description.Behaviors.Add(serviceMetadataBehavior);
		}
		Binding binding = MetadataExchangeBindings.CreateMexTcpBinding();
		serviceHost.AddServiceEndpoint("IMetadataExchange", binding, "mex");
	}

	public static ServiceHost CreateNewServiceHost<TService>()
	{
		Uri uri = new Uri("net.tcp://localhost:8556/RhinoServices");
		return new ServiceHost(typeof(TService), uri);
	}

	public static NetTcpBinding GetNetTcpBinding()
	{
		return new NetTcpBinding(SecurityMode.Transport, reliableSessionEnabled: true)
		{
			Name = "NetTcpBinding_IRhinoServices",
			SendTimeout = TimeSpan.FromHours(1.0),
			CloseTimeout = TimeSpan.FromMinutes(5.0),
			OpenTimeout = TimeSpan.FromMinutes(5.0),
			MaxBufferPoolSize = 2147483647L,
			MaxBufferSize = int.MaxValue,
			MaxReceivedMessageSize = 2147483647L,
			ReaderQuotas = 
			{
				MaxDepth = int.MaxValue,
				MaxStringContentLength = int.MaxValue,
				MaxArrayLength = int.MaxValue,
				MaxBytesPerRead = int.MaxValue,
				MaxNameTableCharCount = int.MaxValue
			},
			ReliableSession = 
			{
				Ordered = true,
				InactivityTimeout = TimeSpan.FromDays(7.0)
			},
			ReceiveTimeout = TimeSpan.MaxValue
		};
	}

	public static void IncreaseObjectGraphMaxValues(this ServiceHost serviceHost)
	{
		foreach (ServiceEndpoint endpoint in serviceHost.Description.Endpoints)
		{
			foreach (OperationDescription operation in endpoint.Contract.Operations)
			{
				DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
				if (dataContractSerializerOperationBehavior != null)
				{
					dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = int.MaxValue;
				}
			}
		}
	}

	public static void IncreaseThrottlingMaxValues(this ServiceHost serviceHost)
	{
		ServiceThrottlingBehavior serviceThrottlingBehavior = serviceHost.Description.Behaviors.Find<ServiceThrottlingBehavior>();
		if (serviceThrottlingBehavior == null)
		{
			ServiceThrottlingBehavior item = new ServiceThrottlingBehavior
			{
				MaxConcurrentCalls = 1000,
				MaxConcurrentSessions = 1000,
				MaxConcurrentInstances = 1000
			};
			serviceHost.Description.Behaviors.Add(item);
		}
		else
		{
			serviceThrottlingBehavior.MaxConcurrentCalls = 1000;
			serviceThrottlingBehavior.MaxConcurrentSessions = 1000;
			serviceThrottlingBehavior.MaxConcurrentInstances = 1000;
		}
	}
}
