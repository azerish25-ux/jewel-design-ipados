using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GV.CounterSketch.Studio.SecurityServices;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "SecurityServices.ISecurityService")]
public interface ISecurityService
{
	[OperationContract(Action = "http://tempuri.org/ISecurityService/IsKeySet", ReplyAction = "http://tempuri.org/ISecurityService/IsKeySetResponse")]
	bool IsKeySet(int keyId);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/IsKeySet", ReplyAction = "http://tempuri.org/ISecurityService/IsKeySetResponse")]
	Task<bool> IsKeySetAsync(int keyId);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/PingServiceTest", ReplyAction = "http://tempuri.org/ISecurityService/PingServiceTestResponse")]
	bool PingServiceTest();

	[OperationContract(Action = "http://tempuri.org/ISecurityService/PingServiceTest", ReplyAction = "http://tempuri.org/ISecurityService/PingServiceTestResponse")]
	Task<bool> PingServiceTestAsync();

	[OperationContract(Action = "http://tempuri.org/ISecurityService/ResetKey", ReplyAction = "http://tempuri.org/ISecurityService/ResetKeyResponse")]
	void ResetKey(int keyId);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/ResetKey", ReplyAction = "http://tempuri.org/ISecurityService/ResetKeyResponse")]
	Task ResetKeyAsync(int keyId);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/VerifyKey", ReplyAction = "http://tempuri.org/ISecurityService/VerifyKeyResponse")]
	byte[] VerifyKey(byte[] sendB, string name, string companyName);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/VerifyKey", ReplyAction = "http://tempuri.org/ISecurityService/VerifyKeyResponse")]
	Task<byte[]> VerifyKeyAsync(byte[] sendB, string name, string companyName);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/VerifyKeyTest", ReplyAction = "http://tempuri.org/ISecurityService/VerifyKeyTestResponse")]
	byte[] VerifyKeyTest();

	[OperationContract(Action = "http://tempuri.org/ISecurityService/VerifyKeyTest", ReplyAction = "http://tempuri.org/ISecurityService/VerifyKeyTestResponse")]
	Task<byte[]> VerifyKeyTestAsync();

	[OperationContract(Action = "http://tempuri.org/ISecurityService/SyncKeyCodeDataToCloud", ReplyAction = "http://tempuri.org/ISecurityService/SyncKeyCodeDataToCloudResponse")]
	void SyncKeyCodeDataToCloud(byte[] codesBytes, byte[] computerBytes, int keyId);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/SyncKeyCodeDataToCloud", ReplyAction = "http://tempuri.org/ISecurityService/SyncKeyCodeDataToCloudResponse")]
	Task SyncKeyCodeDataToCloudAsync(byte[] codesBytes, byte[] computerBytes, int keyId);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/UpdateSupportExpirateDateForKey", ReplyAction = "http://tempuri.org/ISecurityService/UpdateSupportExpirateDateForKeyResponse")]
	void UpdateSupportExpirateDateForKey(int keyId, DateTime applicationUpdateExpireDate, DateTime libraryUpdateExpireDate);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/UpdateSupportExpirateDateForKey", ReplyAction = "http://tempuri.org/ISecurityService/UpdateSupportExpirateDateForKeyResponse")]
	Task UpdateSupportExpirateDateForKeyAsync(int keyId, DateTime applicationUpdateExpireDate, DateTime libraryUpdateExpireDate);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/GetKeyProfileByKey", ReplyAction = "http://tempuri.org/ISecurityService/GetKeyProfileByKeyResponse")]
	string GetKeyProfileByKey(Guid key);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/GetKeyProfileByKey", ReplyAction = "http://tempuri.org/ISecurityService/GetKeyProfileByKeyResponse")]
	Task<string> GetKeyProfileByKeyAsync(Guid key);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/GetKeyProfileByUserKeyId", ReplyAction = "http://tempuri.org/ISecurityService/GetKeyProfileByUserKeyIdResponse")]
	string GetKeyProfileByUserKeyId(int keyId);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/GetKeyProfileByUserKeyId", ReplyAction = "http://tempuri.org/ISecurityService/GetKeyProfileByUserKeyIdResponse")]
	Task<string> GetKeyProfileByUserKeyIdAsync(int keyId);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/WriteKeyProfileDataToProfile", ReplyAction = "http://tempuri.org/ISecurityService/WriteKeyProfileDataToProfileResponse")]
	void WriteKeyProfileDataToProfile(Guid key, string securityKeyword, string profileData);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/WriteKeyProfileDataToProfile", ReplyAction = "http://tempuri.org/ISecurityService/WriteKeyProfileDataToProfileResponse")]
	Task WriteKeyProfileDataToProfileAsync(Guid key, string securityKeyword, string profileData);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/GetKeyProfileById", ReplyAction = "http://tempuri.org/ISecurityService/GetKeyProfileByIdResponse")]
	string GetKeyProfileById(int keyProfileId);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/GetKeyProfileById", ReplyAction = "http://tempuri.org/ISecurityService/GetKeyProfileByIdResponse")]
	Task<string> GetKeyProfileByIdAsync(int keyProfileId);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/WriteKeyProfileDataToProfileById", ReplyAction = "http://tempuri.org/ISecurityService/WriteKeyProfileDataToProfileByIdResponse")]
	void WriteKeyProfileDataToProfileById(int keyProfileId, string securityKeyword, string profileData);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/WriteKeyProfileDataToProfileById", ReplyAction = "http://tempuri.org/ISecurityService/WriteKeyProfileDataToProfileByIdResponse")]
	Task WriteKeyProfileDataToProfileByIdAsync(int keyProfileId, string securityKeyword, string profileData);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/AddKeyProfile", ReplyAction = "http://tempuri.org/ISecurityService/AddKeyProfileResponse")]
	int AddKeyProfile(string profileName, string securityKeyword, Guid uniqueKey, string profileData);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/AddKeyProfile", ReplyAction = "http://tempuri.org/ISecurityService/AddKeyProfileResponse")]
	Task<int> AddKeyProfileAsync(string profileName, string securityKeyword, Guid uniqueKey, string profileData);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/UpdateKeyFromAdminTool", ReplyAction = "http://tempuri.org/ISecurityService/UpdateKeyFromAdminToolResponse")]
	void UpdateKeyFromAdminTool(int keyId, DateTime applicationUpdateExpireDate, DateTime libraryUpdateExpireDate, int keyProfileId, int keyEnabled, int userTypeId);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/UpdateKeyFromAdminTool", ReplyAction = "http://tempuri.org/ISecurityService/UpdateKeyFromAdminToolResponse")]
	Task UpdateKeyFromAdminToolAsync(int keyId, DateTime applicationUpdateExpireDate, DateTime libraryUpdateExpireDate, int keyProfileId, int keyEnabled, int userTypeId);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/UpdateKeyProfileId", ReplyAction = "http://tempuri.org/ISecurityService/UpdateKeyProfileIdResponse")]
	void UpdateKeyProfileId(int keyProfileId, int keyId, string securityKeyword);

	[OperationContract(Action = "http://tempuri.org/ISecurityService/UpdateKeyProfileId", ReplyAction = "http://tempuri.org/ISecurityService/UpdateKeyProfileIdResponse")]
	Task UpdateKeyProfileIdAsync(int keyProfileId, int keyId, string securityKeyword);
}
