using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace GV.CounterSketch.Studio.KeyCodeServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class KeyCodeServiceClient : ClientBase<IKeyCodeService>, IKeyCodeService
{
	public KeyCodeServiceClient()
	{
	}

	public KeyCodeServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public KeyCodeServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public KeyCodeServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public KeyCodeServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public void UpdateKeyInformation(int keyId, string name, string company, string accountNo, string address, string city, string state, string zip, string country, double latitude, double longitude)
	{
		base.Channel.UpdateKeyInformation(keyId, name, company, accountNo, address, city, state, zip, country, latitude, longitude);
	}

	public Task UpdateKeyInformationAsync(int keyId, string name, string company, string accountNo, string address, string city, string state, string zip, string country, double latitude, double longitude)
	{
		return base.Channel.UpdateKeyInformationAsync(keyId, name, company, accountNo, address, city, state, zip, country, latitude, longitude);
	}

	public DateTime? GetSupportExpirationDate(int keyId)
	{
		return base.Channel.GetSupportExpirationDate(keyId);
	}

	public Task<DateTime?> GetSupportExpirationDateAsync(int keyId)
	{
		return base.Channel.GetSupportExpirationDateAsync(keyId);
	}

	public void SetCSVersionInfo(int keyId, string csVersion, string rhinoExeVersion, string rhinoExeServiceRelease, string rhinoBuildDate, string libraryVersion, string primaryEmail)
	{
		base.Channel.SetCSVersionInfo(keyId, csVersion, rhinoExeVersion, rhinoExeServiceRelease, rhinoBuildDate, libraryVersion, primaryEmail);
	}

	public Task SetCSVersionInfoAsync(int keyId, string csVersion, string rhinoExeVersion, string rhinoExeServiceRelease, string rhinoBuildDate, string libraryVersion, string primaryEmail)
	{
		return base.Channel.SetCSVersionInfoAsync(keyId, csVersion, rhinoExeVersion, rhinoExeServiceRelease, rhinoBuildDate, libraryVersion, primaryEmail);
	}

	public string GetRhinoBuildDateNeeded(string csVersionString)
	{
		return base.Channel.GetRhinoBuildDateNeeded(csVersionString);
	}

	public Task<string> GetRhinoBuildDateNeededAsync(string csVersionString)
	{
		return base.Channel.GetRhinoBuildDateNeededAsync(csVersionString);
	}

	public string GetCSVersionNeeded(int keyId)
	{
		return base.Channel.GetCSVersionNeeded(keyId);
	}

	public Task<string> GetCSVersionNeededAsync(int keyId)
	{
		return base.Channel.GetCSVersionNeededAsync(keyId);
	}
}
