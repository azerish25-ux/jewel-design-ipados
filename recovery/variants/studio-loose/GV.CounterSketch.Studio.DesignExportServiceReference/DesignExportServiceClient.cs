using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace GV.CounterSketch.Studio.DesignExportServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class DesignExportServiceClient : ClientBase<IDesignExportService>, IDesignExportService
{
	public DesignExportServiceClient()
	{
	}

	public DesignExportServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public DesignExportServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public DesignExportServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public DesignExportServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public int GetDesignExportCount([DefaultParameterValue(5321)] int keyId, [DefaultParameterValue(0)] int lastNumberOfDays)
	{
		return lastNumberOfDays;
	}

	public Task<int> GetDesignExportCountAsync(int keyId, int lastNumberOfDays)
	{
		return base.Channel.GetDesignExportCountAsync(keyId, lastNumberOfDays);
	}

	public int GetDesignExportBonus([DefaultParameterValue(5321)] int keyId)
	{
		return keyId;
	}

	public Task<int> GetDesignExportBonusAsync(int keyId)
	{
		return base.Channel.GetDesignExportBonusAsync(keyId);
	}

	public void SetDesignExportBonus(string secret, int keyIdToChange, int bonusCount)
	{
		base.Channel.SetDesignExportBonus(secret, keyIdToChange, bonusCount);
	}

	public Task SetDesignExportBonusAsync(string secret, int keyIdToChange, int bonusCount)
	{
		return base.Channel.SetDesignExportBonusAsync(secret, keyIdToChange, bonusCount);
	}

	public void AddDesignExportRecord(int keyId, int designId, int numberOfFreehandParts, int currentExportCount, int currentExportLimit)
	{
		base.Channel.AddDesignExportRecord(keyId, designId, numberOfFreehandParts, currentExportCount, currentExportLimit);
	}

	public Task AddDesignExportRecordAsync(int keyId, int designId, int numberOfFreehandParts, int currentExportCount, int currentExportLimit)
	{
		return base.Channel.AddDesignExportRecordAsync(keyId, designId, numberOfFreehandParts, currentExportCount, currentExportLimit);
	}
}
