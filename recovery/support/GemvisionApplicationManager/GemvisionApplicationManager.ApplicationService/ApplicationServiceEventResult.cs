namespace GemvisionApplicationManager.ApplicationService;

public class ApplicationServiceEventResult
{
	public bool Success { get; set; }

	public bool IsProcessed { get; set; }

	public ApplicationMessage Value { get; set; }
}
