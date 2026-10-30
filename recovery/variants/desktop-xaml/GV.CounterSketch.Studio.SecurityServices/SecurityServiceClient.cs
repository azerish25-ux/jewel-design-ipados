using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace GV.CounterSketch.Studio.SecurityServices;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class SecurityServiceClient : ClientBase<ISecurityService>, ISecurityService
{
	public SecurityServiceClient()
	{
	}

	public SecurityServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public SecurityServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public SecurityServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public SecurityServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public bool IsKeySet(int keyId)
	{
		return base.Channel.IsKeySet(keyId);
	}

	public Task<bool> IsKeySetAsync(int keyId)
	{
		return base.Channel.IsKeySetAsync(keyId);
	}

	public bool PingServiceTest()
	{
		return base.Channel.PingServiceTest();
	}

	public Task<bool> PingServiceTestAsync()
	{
		return base.Channel.PingServiceTestAsync();
	}

	public void ResetKey(int keyId)
	{
		base.Channel.ResetKey(keyId);
	}

	public Task ResetKeyAsync(int keyId)
	{
		return base.Channel.ResetKeyAsync(keyId);
	}

	public byte[] VerifyKey(byte[] sendB, string name, string companyName)
	{
		return base.Channel.VerifyKey(sendB, name, companyName);
	}

	public Task<byte[]> VerifyKeyAsync(byte[] sendB, string name, string companyName)
	{
		return base.Channel.VerifyKeyAsync(sendB, name, companyName);
	}

	public byte[] VerifyKeyTest()
	{
		return base.Channel.VerifyKeyTest();
	}

	public Task<byte[]> VerifyKeyTestAsync()
	{
		return base.Channel.VerifyKeyTestAsync();
	}

	public void SyncKeyCodeDataToCloud(byte[] codesBytes, byte[] computerBytes, int keyId)
	{
		base.Channel.SyncKeyCodeDataToCloud(codesBytes, computerBytes, keyId);
	}

	public Task SyncKeyCodeDataToCloudAsync(byte[] codesBytes, byte[] computerBytes, int keyId)
	{
		return base.Channel.SyncKeyCodeDataToCloudAsync(codesBytes, computerBytes, keyId);
	}

	public void UpdateSupportExpirateDateForKey(int keyId, DateTime applicationUpdateExpireDate, DateTime libraryUpdateExpireDate)
	{
		base.Channel.UpdateSupportExpirateDateForKey(keyId, applicationUpdateExpireDate, libraryUpdateExpireDate);
	}

	public Task UpdateSupportExpirateDateForKeyAsync(int keyId, DateTime applicationUpdateExpireDate, DateTime libraryUpdateExpireDate)
	{
		return base.Channel.UpdateSupportExpirateDateForKeyAsync(keyId, applicationUpdateExpireDate, libraryUpdateExpireDate);
	}

	public string GetKeyProfileByKey(Guid key)
	{
		return base.Channel.GetKeyProfileByKey(key);
	}

	public Task<string> GetKeyProfileByKeyAsync(Guid key)
	{
		return base.Channel.GetKeyProfileByKeyAsync(key);
	}

	public string GetKeyProfileByUserKeyId(int keyId)
	{
		return base.Channel.GetKeyProfileByUserKeyId(keyId);
	}

	public Task<string> GetKeyProfileByUserKeyIdAsync(int keyId)
	{
		return base.Channel.GetKeyProfileByUserKeyIdAsync(keyId);
	}

	public void WriteKeyProfileDataToProfile(Guid key, string securityKeyword, string profileData)
	{
		base.Channel.WriteKeyProfileDataToProfile(key, securityKeyword, profileData);
	}

	public Task WriteKeyProfileDataToProfileAsync(Guid key, string securityKeyword, string profileData)
	{
		return base.Channel.WriteKeyProfileDataToProfileAsync(key, securityKeyword, profileData);
	}

	public string GetKeyProfileById(int keyProfileId)
	{
		return base.Channel.GetKeyProfileById(keyProfileId);
	}

	public Task<string> GetKeyProfileByIdAsync(int keyProfileId)
	{
		return base.Channel.GetKeyProfileByIdAsync(keyProfileId);
	}

	public void WriteKeyProfileDataToProfileById(int keyProfileId, string securityKeyword, string profileData)
	{
		base.Channel.WriteKeyProfileDataToProfileById(keyProfileId, securityKeyword, profileData);
	}

	public Task WriteKeyProfileDataToProfileByIdAsync(int keyProfileId, string securityKeyword, string profileData)
	{
		return base.Channel.WriteKeyProfileDataToProfileByIdAsync(keyProfileId, securityKeyword, profileData);
	}

	public int AddKeyProfile(string profileName, string securityKeyword, Guid uniqueKey, string profileData)
	{
		return base.Channel.AddKeyProfile(profileName, securityKeyword, uniqueKey, profileData);
	}

	public Task<int> AddKeyProfileAsync(string profileName, string securityKeyword, Guid uniqueKey, string profileData)
	{
		return base.Channel.AddKeyProfileAsync(profileName, securityKeyword, uniqueKey, profileData);
	}

	public void UpdateKeyFromAdminTool(int keyId, DateTime applicationUpdateExpireDate, DateTime libraryUpdateExpireDate, int keyProfileId, int keyEnabled, int userTypeId)
	{
		base.Channel.UpdateKeyFromAdminTool(keyId, applicationUpdateExpireDate, libraryUpdateExpireDate, keyProfileId, keyEnabled, userTypeId);
	}

	public Task UpdateKeyFromAdminToolAsync(int keyId, DateTime applicationUpdateExpireDate, DateTime libraryUpdateExpireDate, int keyProfileId, int keyEnabled, int userTypeId)
	{
		return base.Channel.UpdateKeyFromAdminToolAsync(keyId, applicationUpdateExpireDate, libraryUpdateExpireDate, keyProfileId, keyEnabled, userTypeId);
	}

	public void UpdateKeyProfileId(int keyProfileId, int keyId, string securityKeyword)
	{
		base.Channel.UpdateKeyProfileId(keyProfileId, keyId, securityKeyword);
	}

	public Task UpdateKeyProfileIdAsync(int keyProfileId, int keyId, string securityKeyword)
	{
		return base.Channel.UpdateKeyProfileIdAsync(keyProfileId, keyId, securityKeyword);
	}
}
