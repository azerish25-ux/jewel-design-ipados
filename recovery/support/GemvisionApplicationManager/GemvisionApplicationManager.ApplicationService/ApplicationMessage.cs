using System;

namespace GemvisionApplicationManager.ApplicationService;

public class ApplicationMessage
{
	public int MessageId { get; set; }

	public int TargetId { get; set; }

	public DateTime CreationTimeStamp { get; set; }

	public string Message { get; set; }

	public ApplicationActionType ApplicationActionType { get; set; }

	public ApplicationType ApplicationType { get; set; }

	public bool IsRead { get; }

	public override string ToString()
	{
		return $"Application - {ApplicationType}, Action - {ApplicationActionType}, Message - {Message}";
	}
}
