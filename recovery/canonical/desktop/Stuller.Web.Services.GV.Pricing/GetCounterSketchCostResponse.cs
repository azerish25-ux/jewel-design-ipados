using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[MessageContract(WrapperName = "GetCounterSketchCostResponse", WrapperNamespace = "http://services.stuller.com", IsWrapped = true)]
public class GetCounterSketchCostResponse
{
	[MessageBodyMember(Namespace = "http://services.stuller.com", Order = 0)]
	public CounterSketchCost GetCounterSketchCostResult;

	public GetCounterSketchCostResponse()
	{
	}

	public GetCounterSketchCostResponse(CounterSketchCost GetCounterSketchCostResult)
	{
		this.GetCounterSketchCostResult = GetCounterSketchCostResult;
	}
}
