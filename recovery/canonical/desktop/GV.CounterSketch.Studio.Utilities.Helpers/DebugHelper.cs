using System.Linq;
using System.Runtime.InteropServices;
using EnvDTE;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class DebugHelper
{
	public static void AttachDebuggerToProcess(int processIdToAttachTo)
	{
		DTE obj = (DTE)Marshal.GetActiveObject("VisualStudio.DTE.15.0");
		MessageFilter.Register();
		obj.Debugger.LocalProcesses.OfType<Process>().SingleOrDefault((Process x) => x.ProcessID == processIdToAttachTo)?.Attach();
		GlobalErrorHandler.WriteInfoToEventLog($"Attached VS Debugger to Process Id: {processIdToAttachTo}");
	}
}
