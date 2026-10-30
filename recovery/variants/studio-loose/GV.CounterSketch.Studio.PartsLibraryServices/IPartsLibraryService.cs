using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GV.CounterSketch.Studio.PartsLibraryServices;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "PartsLibraryServices.IPartsLibraryService")]
public interface IPartsLibraryService
{
	[OperationContract(Action = "http://tempuri.org/IPartsLibraryService/GetLatestPartsLibraryVersionByKeyId", ReplyAction = "http://tempuri.org/IPartsLibraryService/GetLatestPartsLibraryVersionByKeyIdResponse")]
	int GetLatestPartsLibraryVersionByKeyId(int keyId, string counterSketchVersion);

	[OperationContract(Action = "http://tempuri.org/IPartsLibraryService/GetLatestPartsLibraryVersionByKeyId", ReplyAction = "http://tempuri.org/IPartsLibraryService/GetLatestPartsLibraryVersionByKeyIdResponse")]
	Task<int> GetLatestPartsLibraryVersionByKeyIdAsync(int keyId, string counterSketchVersion);
}
