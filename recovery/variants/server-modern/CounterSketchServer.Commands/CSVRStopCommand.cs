using System;
using CounterSketchServer.Utilities.Helpers;
using RMA.Rhino;

namespace CounterSketchServer.Commands;

public class CSVRStopCommand : MRhinoTestCommand
{
	public override Guid CommandUUID()
	{
		return new Guid("{ED840D84-B4C8-40c7-9AB9-574EC2459088}");
	}

	public override string EnglishCommandName()
	{
		return "CSVRStop";
	}

	public override result RunCommand(IRhinoCommandContext context)
	{
		CSVRHelper.StopServer();
		return (result)0;
	}
}
