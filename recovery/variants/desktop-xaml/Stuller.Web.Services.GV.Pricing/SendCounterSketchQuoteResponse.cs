using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[MessageContract(WrapperName = "SendCounterSketchQuoteResponse", WrapperNamespace = "http://services.stuller.com", IsWrapped = true)]
public class SendCounterSketchQuoteResponse
{
	[MessageBodyMember(Namespace = "http://services.stuller.com", Order = 0)]
	public CounterSketchQuoteResponse SendCounterSketchQuoteResult;

	public SendCounterSketchQuoteResponse()
	{
	}

	public SendCounterSketchQuoteResponse(CounterSketchQuoteResponse SendCounterSketchQuoteResult)
	{
		this.SendCounterSketchQuoteResult = SendCounterSketchQuoteResult;
	}
}
