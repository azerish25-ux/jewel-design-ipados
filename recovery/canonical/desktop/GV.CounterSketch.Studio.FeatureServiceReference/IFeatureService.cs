using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GV.CounterSketch.Studio.FeatureServiceReference;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "FeatureServiceReference.IFeatureService")]
public interface IFeatureService
{
	[OperationContract(Action = "http://tempuri.org/IFeatureService/AddFeature", ReplyAction = "http://tempuri.org/IFeatureService/AddFeatureResponse")]
	bool AddFeature(string name, string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/AddFeature", ReplyAction = "http://tempuri.org/IFeatureService/AddFeatureResponse")]
	Task<bool> AddFeatureAsync(string name, string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/AddFeatureToKey", ReplyAction = "http://tempuri.org/IFeatureService/AddFeatureToKeyResponse")]
	bool AddFeatureToKey(int featureId, int keyId, string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/AddFeatureToKey", ReplyAction = "http://tempuri.org/IFeatureService/AddFeatureToKeyResponse")]
	Task<bool> AddFeatureToKeyAsync(int featureId, int keyId, string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/DeleteFeature", ReplyAction = "http://tempuri.org/IFeatureService/DeleteFeatureResponse")]
	bool DeleteFeature(int featureId, string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/DeleteFeature", ReplyAction = "http://tempuri.org/IFeatureService/DeleteFeatureResponse")]
	Task<bool> DeleteFeatureAsync(int featureId, string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/GetFeature", ReplyAction = "http://tempuri.org/IFeatureService/GetFeatureResponse")]
	FeatureDTO GetFeature(int featureId, string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/GetFeature", ReplyAction = "http://tempuri.org/IFeatureService/GetFeatureResponse")]
	Task<FeatureDTO> GetFeatureAsync(int featureId, string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/GetFeatures", ReplyAction = "http://tempuri.org/IFeatureService/GetFeaturesResponse")]
	FeatureDTO[] GetFeatures(string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/GetFeatures", ReplyAction = "http://tempuri.org/IFeatureService/GetFeaturesResponse")]
	Task<FeatureDTO[]> GetFeaturesAsync(string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/GetFeaturesForKey", ReplyAction = "http://tempuri.org/IFeatureService/GetFeaturesForKeyResponse")]
	FeatureDTO[] GetFeaturesForKey(int keyId, string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/GetFeaturesForKey", ReplyAction = "http://tempuri.org/IFeatureService/GetFeaturesForKeyResponse")]
	Task<FeatureDTO[]> GetFeaturesForKeyAsync(int keyId, string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/RemoveFeatureFromAllKeys", ReplyAction = "http://tempuri.org/IFeatureService/RemoveFeatureFromAllKeysResponse")]
	bool RemoveFeatureFromAllKeys(int featureId, string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/RemoveFeatureFromAllKeys", ReplyAction = "http://tempuri.org/IFeatureService/RemoveFeatureFromAllKeysResponse")]
	Task<bool> RemoveFeatureFromAllKeysAsync(int featureId, string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/RemoveFeatureFromKey", ReplyAction = "http://tempuri.org/IFeatureService/RemoveFeatureFromKeyResponse")]
	bool RemoveFeatureFromKey(int featureId, int keyId, string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/IFeatureService/RemoveFeatureFromKey", ReplyAction = "http://tempuri.org/IFeatureService/RemoveFeatureFromKeyResponse")]
	Task<bool> RemoveFeatureFromKeyAsync(int featureId, int keyId, string securityKeyword);
}
