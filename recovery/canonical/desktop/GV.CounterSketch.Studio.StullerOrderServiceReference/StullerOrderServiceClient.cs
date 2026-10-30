using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace GV.CounterSketch.Studio.StullerOrderServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class StullerOrderServiceClient : ClientBase<IStullerOrderService>, IStullerOrderService
{
	public StullerOrderServiceClient()
	{
	}

	public StullerOrderServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public StullerOrderServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public StullerOrderServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public StullerOrderServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public void SendCSGFileForOrder(byte[] designFile, string camJobNumber)
	{
		base.Channel.SendCSGFileForOrder(designFile, camJobNumber);
	}

	public Task SendCSGFileForOrderAsync(byte[] designFile, string camJobNumber)
	{
		return base.Channel.SendCSGFileForOrderAsync(designFile, camJobNumber);
	}

	public bool Ping()
	{
		return base.Channel.Ping();
	}

	public Task<bool> PingAsync()
	{
		return base.Channel.PingAsync();
	}
}
