using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

namespace Stuller.Web.Services.GV.Pricing;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[MessageContract(WrapperName = "GetCounterSketchPrototype", WrapperNamespace = "http://services.stuller.com", IsWrapped = true)]
public class GetCounterSketchPrototypeRequest
{
	[MessageBodyMember(Namespace = "http://services.stuller.com", Order = 0)]
	public string DeveloperToken;

	[MessageBodyMember(Namespace = "http://services.stuller.com", Order = 1)]
	public string shipToAccountNumber;

	public GetCounterSketchPrototypeRequest()
	{
	}

	public GetCounterSketchPrototypeRequest(string DeveloperToken, string shipToAccountNumber)
	{
		this.DeveloperToken = DeveloperToken;
		this.shipToAccountNumber = shipToAccountNumber;
	}
}
