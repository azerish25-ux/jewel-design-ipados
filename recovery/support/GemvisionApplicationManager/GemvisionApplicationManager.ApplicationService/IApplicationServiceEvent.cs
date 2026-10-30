namespace GemvisionApplicationManager.ApplicationService;

public interface IApplicationServiceEvent
{
	ApplicationServiceEventType ApplicationServiceEventType { get; set; }

	bool IsExecuting { get; set; }

	void Execute();

	void Report();
}
