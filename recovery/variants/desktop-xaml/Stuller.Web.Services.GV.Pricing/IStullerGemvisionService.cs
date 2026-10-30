using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Stuller.Web.Services.GV.Pricing;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://services.stuller.com", ConfigurationName = "Stuller.Web.Services.GV.Pricing.IStullerGemvisionService")]
public interface IStullerGemvisionService
{
	[OperationContract(Action = "http://services.stuller.com/IStullerGemvisionService/GetCounterSketchCost", ReplyAction = "http://services.stuller.com/IStullerGemvisionService/GetCounterSketchCostResponse")]
	GetCounterSketchCostResponse GetCounterSketchCost(GetCounterSketchCostRequest request);

	[OperationContract(Action = "http://services.stuller.com/IStullerGemvisionService/GetCounterSketchCost", ReplyAction = "http://services.stuller.com/IStullerGemvisionService/GetCounterSketchCostResponse")]
	Task<GetCounterSketchCostResponse> GetCounterSketchCostAsync(GetCounterSketchCostRequest request);

	[OperationContract(Action = "http://services.stuller.com/IStullerGemvisionService/SendCounterSketchQuote", ReplyAction = "http://services.stuller.com/IStullerGemvisionService/SendCounterSketchQuoteResponse")]
	SendCounterSketchQuoteResponse SendCounterSketchQuote(SendCounterSketchQuoteRequest request);

	[OperationContract(Action = "http://services.stuller.com/IStullerGemvisionService/SendCounterSketchQuote", ReplyAction = "http://services.stuller.com/IStullerGemvisionService/SendCounterSketchQuoteResponse")]
	Task<SendCounterSketchQuoteResponse> SendCounterSketchQuoteAsync(SendCounterSketchQuoteRequest request);

	[OperationContract(Action = "http://services.stuller.com/IStullerGemvisionService/GetCounterSketchQuoteStatus", ReplyAction = "http://services.stuller.com/IStullerGemvisionService/GetCounterSketchQuoteStatusResponse")]
	GetCounterSketchQuoteStatusResponse GetCounterSketchQuoteStatus(GetCounterSketchQuoteStatusRequest request);

	[OperationContract(Action = "http://services.stuller.com/IStullerGemvisionService/GetCounterSketchQuoteStatus", ReplyAction = "http://services.stuller.com/IStullerGemvisionService/GetCounterSketchQuoteStatusResponse")]
	Task<GetCounterSketchQuoteStatusResponse> GetCounterSketchQuoteStatusAsync(GetCounterSketchQuoteStatusRequest request);

	[OperationContract(Action = "http://services.stuller.com/IStullerGemvisionService/GetCounterSketchPrototype", ReplyAction = "http://services.stuller.com/IStullerGemvisionService/GetCounterSketchPrototypeResponse")]
	GetCounterSketchPrototypeResponse GetCounterSketchPrototype(GetCounterSketchPrototypeRequest request);

	[OperationContract(Action = "http://services.stuller.com/IStullerGemvisionService/GetCounterSketchPrototype", ReplyAction = "http://services.stuller.com/IStullerGemvisionService/GetCounterSketchPrototypeResponse")]
	Task<GetCounterSketchPrototypeResponse> GetCounterSketchPrototypeAsync(GetCounterSketchPrototypeRequest request);
}
