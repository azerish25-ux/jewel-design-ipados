using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class StullerGemvisionServiceClient : ClientBase<IStullerGemvisionService>, IStullerGemvisionService
{
	public StullerGemvisionServiceClient()
	{
	}

	public StullerGemvisionServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public StullerGemvisionServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public StullerGemvisionServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public StullerGemvisionServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public GetCounterSketchCostResponse GetCounterSketchCost(GetCounterSketchCostRequest request)
	{
		return base.Channel.GetCounterSketchCost(request);
	}

	public Task<GetCounterSketchCostResponse> GetCounterSketchCostAsync(GetCounterSketchCostRequest request)
	{
		return base.Channel.GetCounterSketchCostAsync(request);
	}

	public SendCounterSketchQuoteResponse SendCounterSketchQuote(SendCounterSketchQuoteRequest request)
	{
		return base.Channel.SendCounterSketchQuote(request);
	}

	public Task<SendCounterSketchQuoteResponse> SendCounterSketchQuoteAsync(SendCounterSketchQuoteRequest request)
	{
		return base.Channel.SendCounterSketchQuoteAsync(request);
	}

	public GetCounterSketchQuoteStatusResponse GetCounterSketchQuoteStatus(GetCounterSketchQuoteStatusRequest request)
	{
		return base.Channel.GetCounterSketchQuoteStatus(request);
	}

	public Task<GetCounterSketchQuoteStatusResponse> GetCounterSketchQuoteStatusAsync(GetCounterSketchQuoteStatusRequest request)
	{
		return base.Channel.GetCounterSketchQuoteStatusAsync(request);
	}

	public GetCounterSketchPrototypeResponse GetCounterSketchPrototype(GetCounterSketchPrototypeRequest request)
	{
		return base.Channel.GetCounterSketchPrototype(request);
	}

	public Task<GetCounterSketchPrototypeResponse> GetCounterSketchPrototypeAsync(GetCounterSketchPrototypeRequest request)
	{
		return base.Channel.GetCounterSketchPrototypeAsync(request);
	}
}
