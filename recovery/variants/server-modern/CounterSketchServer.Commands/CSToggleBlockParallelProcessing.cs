using System.Runtime.InteropServices;
using CS;
using Rhino;
using Rhino.Commands;

namespace CounterSketchServer.Commands;

[Guid("fa2becfd-cd55-4f92-8e57-a0c5b48e2fec")]
[CommandStyle(/*Could not decode attribute arguments.*/)]
public class CSToggleBlockParallelProcessing : Command
{
	public static CSToggleBlockParallelProcessing Instance { get; private set; }

	public override string EnglishName => "CSToggleBlockParallelProcessing";

	public CSToggleBlockParallelProcessing()
	{
		Instance = this;
	}

	protected override Result RunCommand(RhinoDoc doc, RunMode mode)
	{
		CSApplicationHelper.Application.IsBlockParallelProcessingEnabled = !CSApplicationHelper.Application.IsBlockParallelProcessingEnabled;
		CSUtil.PrintLine(CSApplicationHelper.Application.IsBlockParallelProcessingEnabled ? "IsBlockParallelProcessingEnabled is TRUE" : "IsBlockParallelProcessingEnabled is FALSE");
		CSUtil.ForceGraphToRedraw();
		return (Result)0;
	}
}
