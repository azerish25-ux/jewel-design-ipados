using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GV.CounterSketch.Studio.StullerOrderServiceReference;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "StullerOrderServiceReference.IStullerOrderService")]
public interface IStullerOrderService
{
	[OperationContract(Action = "http://tempuri.org/IStullerOrderService/SendCSGFileForOrder", ReplyAction = "http://tempuri.org/IStullerOrderService/SendCSGFileForOrderResponse")]
	void SendCSGFileForOrder(byte[] designFile, string camJobNumber);

	[OperationContract(Action = "http://tempuri.org/IStullerOrderService/SendCSGFileForOrder", ReplyAction = "http://tempuri.org/IStullerOrderService/SendCSGFileForOrderResponse")]
	Task SendCSGFileForOrderAsync(byte[] designFile, string camJobNumber);

	[OperationContract(Action = "http://tempuri.org/IStullerOrderService/Ping", ReplyAction = "http://tempuri.org/IStullerOrderService/PingResponse")]
	bool Ping();

	[OperationContract(Action = "http://tempuri.org/IStullerOrderService/Ping", ReplyAction = "http://tempuri.org/IStullerOrderService/PingResponse")]
	Task<bool> PingAsync();
}
