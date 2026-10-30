using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using GV.CounterSketch.Shared;
using Newtonsoft.Json;

namespace GemvisionApplicationManager.ApplicationService;

public class RemoteMessageApplicationServiceEvent : IApplicationServiceEvent
{
	private static readonly HttpClient _httpClient;

	private static readonly string _loggingPrefix;

	private readonly string _identifier = string.Empty;

	private readonly List<ApplicationServiceEventResult> _applicationServiceEventResultsList = new List<ApplicationServiceEventResult>();

	public ApplicationServiceEventType ApplicationServiceEventType { get; set; }

	public bool IsExecuting { get; set; }

	public RemoteMessageApplicationServiceEvent(string identifier)
	{
		_identifier = identifier;
		LogHelper.Log.Setup(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Gemvision\\CounterSketch Studio"), "GemvisionApplicationManager.log");
	}

	public void Execute()
	{
		LogHelper.Log.WriteLog(_loggingPrefix + " Exectuing for identifier " + _identifier + ".");
		if (IsExecuting)
		{
			LogHelper.Log.WriteLog(_loggingPrefix + " Already Executing, terminating request.");
			return;
		}
		IsExecuting = true;
		ApplicationServiceEventResult remoteMessages = GetRemoteMessages();
		if (remoteMessages.Success)
		{
			_applicationServiceEventResultsList.Add(remoteMessages);
		}
		ProcessRemoteMessages();
		IsExecuting = false;
	}

	public virtual void Report()
	{
		throw new NotImplementedException();
	}

	private ApplicationServiceEventResult GetRemoteMessages()
	{
		LogHelper.Log.WriteLog(_loggingPrefix + " Getting Remote Messages.");
		ApplicationServiceEventResult applicationServiceEventResult = new ApplicationServiceEventResult();
		try
		{
			HttpResponseMessage result = _httpClient.GetAsync("api/message/unread/" + _identifier).Result;
			result.EnsureSuccessStatusCode();
			ApplicationMessage applicationMessage = JsonConvert.DeserializeObject<ApplicationMessage>(result.Content.ReadAsStringAsync().Result);
			if (applicationMessage != null)
			{
				LogHelper.Log.WriteLog($"{_loggingPrefix} Message Received - {applicationMessage}");
				applicationServiceEventResult.Success = true;
				applicationServiceEventResult.Value = applicationMessage;
				_httpClient.PostAsync($"api/message/markread/{applicationMessage.MessageId}", null);
			}
			else
			{
				applicationServiceEventResult.Success = false;
			}
		}
		catch (Exception ex)
		{
			LogHelper.Log.WriteLog(_loggingPrefix + " Error Getting Remote Messages.", ex);
			applicationServiceEventResult.Success = false;
		}
		return applicationServiceEventResult;
	}

	private void ProcessRemoteMessages()
	{
		LogHelper.Log.WriteLog(_loggingPrefix + " Processing Remote Messages.");
		_applicationServiceEventResultsList.Where((ApplicationServiceEventResult _) => !_.IsProcessed).ToList().ForEach(delegate(ApplicationServiceEventResult applicationServiceEventResult)
		{
			GemvisionApplicationService.OnProcessedServiceEventNotification(applicationServiceEventResult.Value.ApplicationType.ToString() ?? "", applicationServiceEventResult.Value.ApplicationActionType.ToString() ?? "", ServiceEventStatus.Information);
			applicationServiceEventResult.IsProcessed = ApplicationInteropHelper.ProcessApplicationAction(applicationServiceEventResult.Value);
		});
	}

	static RemoteMessageApplicationServiceEvent()
	{
		HttpClient httpClient = new HttpClient();
		httpClient.BaseAddress = new Uri("https://gemvisionapplicationmessageapi.azurewebsites.net/");
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "Z2VtdmlzaW9ubWVzc2FnZWNvbm5lY3RvcjptM3NzNGdlYzBubjNjdDBy");
		_httpClient = httpClient;
		_loggingPrefix = "\t\tRemoteMessageAppServiceEvents :";
	}
}
