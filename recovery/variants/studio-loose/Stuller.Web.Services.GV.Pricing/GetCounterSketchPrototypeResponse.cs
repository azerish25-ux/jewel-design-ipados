using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[MessageContract(WrapperName = "GetCounterSketchPrototypeResponse", WrapperNamespace = "http://services.stuller.com", IsWrapped = true)]
public class GetCounterSketchPrototypeResponse
{
	[MessageBodyMember(Namespace = "http://services.stuller.com", Order = 0)]
	public CounterSketchPrototypeResponse GetCounterSketchPrototypeResult;

	public GetCounterSketchPrototypeResponse()
	{
	}

	public GetCounterSketchPrototypeResponse(CounterSketchPrototypeResponse GetCounterSketchPrototypeResult)
	{
		this.GetCounterSketchPrototypeResult = GetCounterSketchPrototypeResult;
	}
}
