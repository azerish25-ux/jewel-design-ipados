using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace GV.CounterSketch.Studio.PartsLibraryServices;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class PartsLibraryServiceClient : ClientBase<IPartsLibraryService>, IPartsLibraryService
{
	public PartsLibraryServiceClient()
	{
	}

	public PartsLibraryServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public PartsLibraryServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public PartsLibraryServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public PartsLibraryServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public int GetLatestPartsLibraryVersionByKeyId(int keyId, string counterSketchVersion)
	{
		return base.Channel.GetLatestPartsLibraryVersionByKeyId(keyId, counterSketchVersion);
	}

	public Task<int> GetLatestPartsLibraryVersionByKeyIdAsync(int keyId, string counterSketchVersion)
	{
		return base.Channel.GetLatestPartsLibraryVersionByKeyIdAsync(keyId, counterSketchVersion);
	}
}
