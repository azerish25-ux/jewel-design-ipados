using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[MessageContract(WrapperName = "SendCounterSketchQuote", WrapperNamespace = "http://services.stuller.com", IsWrapped = true)]
public class SendCounterSketchQuoteRequest
{
	[MessageBodyMember(Namespace = "http://services.stuller.com", Order = 0)]
	public string DeveloperToken;

	[MessageBodyMember(Namespace = "http://services.stuller.com", Order = 1)]
	public CounterSketchQuote CounterSketchQuote;

	public SendCounterSketchQuoteRequest()
	{
	}

	public SendCounterSketchQuoteRequest(string DeveloperToken, CounterSketchQuote CounterSketchQuote)
	{
		this.DeveloperToken = DeveloperToken;
		this.CounterSketchQuote = CounterSketchQuote;
	}
}
