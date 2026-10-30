using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[MessageContract(WrapperName = "GetCounterSketchQuoteStatusResponse", WrapperNamespace = "http://services.stuller.com", IsWrapped = true)]
public class GetCounterSketchQuoteStatusResponse
{
	[MessageBodyMember(Namespace = "http://services.stuller.com", Order = 0)]
	public CounterSketchQuoteStatusResponse GetCounterSketchQuoteStatusResult;

	public GetCounterSketchQuoteStatusResponse()
	{
	}

	public GetCounterSketchQuoteStatusResponse(CounterSketchQuoteStatusResponse GetCounterSketchQuoteStatusResult)
	{
		this.GetCounterSketchQuoteStatusResult = GetCounterSketchQuoteStatusResult;
	}
}
