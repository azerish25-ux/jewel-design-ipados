using System;

namespace GemvisionApplicationManager.ApplicationService;

public class ServiceEventNotificationArgs : EventArgs
{
	public string EventName { get; }

	public string EventInformation { get; }

	public ServiceEventStatus EventStatus { get; }

	public ServiceEventNotificationArgs(string eventName, string eventInformation, ServiceEventStatus status)
	{
		EventName = eventName;
		EventInformation = eventInformation;
		EventStatus = status;
	}
}
