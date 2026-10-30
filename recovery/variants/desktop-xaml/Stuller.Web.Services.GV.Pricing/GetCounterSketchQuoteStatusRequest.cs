using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[MessageContract(WrapperName = "GetCounterSketchQuoteStatus", WrapperNamespace = "http://services.stuller.com", IsWrapped = true)]
public class GetCounterSketchQuoteStatusRequest
{
	[MessageBodyMember(Namespace = "http://services.stuller.com", Order = 0)]
	public string DeveloperToken;

	[MessageBodyMember(Namespace = "http://services.stuller.com", Order = 1)]
	public int[] RequestIDs;

	public GetCounterSketchQuoteStatusRequest()
	{
	}

	public GetCounterSketchQuoteStatusRequest(string DeveloperToken, int[] RequestIDs)
	{
		this.DeveloperToken = DeveloperToken;
		this.RequestIDs = RequestIDs;
	}
}
