using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GV.CounterSketch.Studio.KeyCodeServiceReference;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "KeyCodeServiceReference.IKeyCodeService")]
public interface IKeyCodeService
{
	[OperationContract(Action = "http://tempuri.org/IKeyCodeService/UpdateKeyInformation", ReplyAction = "http://tempuri.org/IKeyCodeService/UpdateKeyInformationResponse")]
	void UpdateKeyInformation(int keyId, string name, string company, string accountNo, string address, string city, string state, string zip, string country, double latitude, double longitude);

	[OperationContract(Action = "http://tempuri.org/IKeyCodeService/UpdateKeyInformation", ReplyAction = "http://tempuri.org/IKeyCodeService/UpdateKeyInformationResponse")]
	Task UpdateKeyInformationAsync(int keyId, string name, string company, string accountNo, string address, string city, string state, string zip, string country, double latitude, double longitude);

	[OperationContract(Action = "http://tempuri.org/IKeyCodeService/GetSupportExpirationDate", ReplyAction = "http://tempuri.org/IKeyCodeService/GetSupportExpirationDateResponse")]
	DateTime? GetSupportExpirationDate(int keyId);

	[OperationContract(Action = "http://tempuri.org/IKeyCodeService/GetSupportExpirationDate", ReplyAction = "http://tempuri.org/IKeyCodeService/GetSupportExpirationDateResponse")]
	Task<DateTime?> GetSupportExpirationDateAsync(int keyId);

	[OperationContract(Action = "http://tempuri.org/IKeyCodeService/SetCSVersionInfo", ReplyAction = "http://tempuri.org/IKeyCodeService/SetCSVersionInfoResponse")]
	void SetCSVersionInfo(int keyId, string csVersion, string rhinoExeVersion, string rhinoExeServiceRelease, string rhinoBuildDate, string libraryVersion, string primaryEmail);

	[OperationContract(Action = "http://tempuri.org/IKeyCodeService/SetCSVersionInfo", ReplyAction = "http://tempuri.org/IKeyCodeService/SetCSVersionInfoResponse")]
	Task SetCSVersionInfoAsync(int keyId, string csVersion, string rhinoExeVersion, string rhinoExeServiceRelease, string rhinoBuildDate, string libraryVersion, string primaryEmail);

	[OperationContract(Action = "http://tempuri.org/IKeyCodeService/GetRhinoBuildDateNeeded", ReplyAction = "http://tempuri.org/IKeyCodeService/GetRhinoBuildDateNeededResponse")]
	string GetRhinoBuildDateNeeded(string csVersionString);

	[OperationContract(Action = "http://tempuri.org/IKeyCodeService/GetRhinoBuildDateNeeded", ReplyAction = "http://tempuri.org/IKeyCodeService/GetRhinoBuildDateNeededResponse")]
	Task<string> GetRhinoBuildDateNeededAsync(string csVersionString);

	[OperationContract(Action = "http://tempuri.org/IKeyCodeService/GetCSVersionNeeded", ReplyAction = "http://tempuri.org/IKeyCodeService/GetCSVersionNeededResponse")]
	string GetCSVersionNeeded(int keyId);

	[OperationContract(Action = "http://tempuri.org/IKeyCodeService/GetCSVersionNeeded", ReplyAction = "http://tempuri.org/IKeyCodeService/GetCSVersionNeededResponse")]
	Task<string> GetCSVersionNeededAsync(int keyId);
}
