using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace GV.CounterSketch.Studio.FeatureServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class FeatureServiceClient : ClientBase<IFeatureService>, IFeatureService
{
	public FeatureServiceClient()
	{
	}

	public FeatureServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public FeatureServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public FeatureServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public FeatureServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public bool AddFeature(string name, string securityKeyword)
	{
		return base.Channel.AddFeature(name, securityKeyword);
	}

	public Task<bool> AddFeatureAsync(string name, string securityKeyword)
	{
		return base.Channel.AddFeatureAsync(name, securityKeyword);
	}

	public bool AddFeatureToKey(int featureId, int keyId, string securityKeyword)
	{
		return base.Channel.AddFeatureToKey(featureId, keyId, securityKeyword);
	}

	public Task<bool> AddFeatureToKeyAsync(int featureId, int keyId, string securityKeyword)
	{
		return base.Channel.AddFeatureToKeyAsync(featureId, keyId, securityKeyword);
	}

	public bool DeleteFeature(int featureId, string securityKeyword)
	{
		return base.Channel.DeleteFeature(featureId, securityKeyword);
	}

	public Task<bool> DeleteFeatureAsync(int featureId, string securityKeyword)
	{
		return base.Channel.DeleteFeatureAsync(featureId, securityKeyword);
	}

	public FeatureDTO GetFeature(int featureId, string securityKeyword)
	{
		return base.Channel.GetFeature(featureId, securityKeyword);
	}

	public Task<FeatureDTO> GetFeatureAsync(int featureId, string securityKeyword)
	{
		return base.Channel.GetFeatureAsync(featureId, securityKeyword);
	}

	public FeatureDTO[] GetFeatures(string securityKeyword)
	{
		return base.Channel.GetFeatures(securityKeyword);
	}

	public Task<FeatureDTO[]> GetFeaturesAsync(string securityKeyword)
	{
		return base.Channel.GetFeaturesAsync(securityKeyword);
	}

	public FeatureDTO[] GetFeaturesForKey(int keyId, string securityKeyword)
	{
		return base.Channel.GetFeaturesForKey(keyId, securityKeyword);
	}

	public Task<FeatureDTO[]> GetFeaturesForKeyAsync(int keyId, string securityKeyword)
	{
		return base.Channel.GetFeaturesForKeyAsync(keyId, securityKeyword);
	}

	public bool RemoveFeatureFromAllKeys(int featureId, string securityKeyword)
	{
		return base.Channel.RemoveFeatureFromAllKeys(featureId, securityKeyword);
	}

	public Task<bool> RemoveFeatureFromAllKeysAsync(int featureId, string securityKeyword)
	{
		return base.Channel.RemoveFeatureFromAllKeysAsync(featureId, securityKeyword);
	}

	public bool RemoveFeatureFromKey(int featureId, int keyId, string securityKeyword)
	{
		return base.Channel.RemoveFeatureFromKey(featureId, keyId, securityKeyword);
	}

	public Task<bool> RemoveFeatureFromKeyAsync(int featureId, int keyId, string securityKeyword)
	{
		return base.Channel.RemoveFeatureFromKeyAsync(featureId, keyId, securityKeyword);
	}
}
