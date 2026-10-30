using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[MessageContract(WrapperName = "GetCounterSketchCost", WrapperNamespace = "http://services.stuller.com", IsWrapped = true)]
public class GetCounterSketchCostRequest
{
	[MessageBodyMember(Namespace = "http://services.stuller.com", Order = 0)]
	public string DeveloperToken;

	[MessageBodyMember(Namespace = "http://services.stuller.com", Order = 1)]
	public CounterSketchCost CounterSketchCost;

	public GetCounterSketchCostRequest()
	{
	}

	public GetCounterSketchCostRequest(string DeveloperToken, CounterSketchCost CounterSketchCost)
	{
		this.DeveloperToken = DeveloperToken;
		this.CounterSketchCost = CounterSketchCost;
	}
}
