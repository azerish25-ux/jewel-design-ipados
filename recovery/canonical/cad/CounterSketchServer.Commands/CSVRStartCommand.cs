using System;
using CounterSketchServer.Utilities.Helpers;
using RMA.Rhino;

namespace CounterSketchServer.Commands;

public class CSVRStartCommand : MRhinoTestCommand
{
	public override Guid CommandUUID()
	{
		return new Guid("{056D5020-C10B-4194-922D-4AC0E11C499E}");
	}

	public override string EnglishCommandName()
	{
		return "CSVRStart";
	}

	public override IRhinoCommand.result RunCommand(IRhinoCommandContext context)
	{
		if (CSVRHelper.StartServer() == 1)
		{
			return IRhinoCommand.result.success;
		}
		return IRhinoCommand.result.failure;
	}
}
