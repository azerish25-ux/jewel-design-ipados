using System;
using CS.CSObjects.CSBlocks;
using RMA.Rhino;

namespace CS;

public sealed class CSEventWatcher : MRhinoEventWatcher
{
	public override void OnAddObject(ref MRhinoDoc doc, ref MRhinoObject @object)
	{
		if (CSApplicationHelper.Application.InRunScriptBlock)
		{
			return;
		}
		foreach (CSBlock item in CSApplicationHelper.Application.Document.Graph.ActiveOutputObjects())
		{
			if (item is ICSBlockObjectWatcher iCSBlockObjectWatcher)
			{
				iCSBlockObjectWatcher.CheckForChangeOnAdd(@object);
			}
		}
	}

	public override void OnBeginCommand(IRhinoCommand command, IRhinoCommandContext context)
	{
		if (!CSApplicationHelper.Application.InRunScriptBlock)
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		}
	}

	public override void OnDeleteObject(ref MRhinoDoc doc, ref MRhinoObject @object)
	{
		if (CSApplicationHelper.Application.InRunScriptBlock)
		{
			return;
		}
		foreach (CSBlock item in CSApplicationHelper.Application.Document.Graph.ActiveOutputObjects())
		{
			if (item is ICSBlockObjectWatcher iCSBlockObjectWatcher)
			{
				iCSBlockObjectWatcher.CheckForChangeOnDelete(@object);
			}
		}
	}

	public override void OnEndCommand(IRhinoCommand command, IRhinoCommandContext context, IRhinoCommand.result rc)
	{
		if ((command == null || !command.IsTransparent()) && !CSApplicationHelper.Application.InRunScriptBlock && !CSApplicationHelper.Application.InRenderCode)
		{
			try
			{
				CSApplicationHelper.Application.Document.Graph.Generate();
				CSUtil.Redraw();
			}
			catch (Exception)
			{
				CSUtil.Print("CS Error in Graph on run command\n");
			}
		}
	}

	public override void OnModifyObjectAttributes(ref MRhinoDoc doc, ref MRhinoObject rhinoObject, IRhinoObjectAttributes oldAttributes)
	{
		if (CSApplicationHelper.Application.InRunScriptBlock)
		{
			return;
		}
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		bool flag = false;
		foreach (CSBlock item in CSApplicationHelper.Application.Document.Graph.ActiveOutputObjects())
		{
			if (item is ICSBlockObjectWatcher iCSBlockObjectWatcher)
			{
				flag |= iCSBlockObjectWatcher.CheckForChangeOnReplace(rhinoObject, rhinoObject);
			}
		}
		try
		{
			if (flag)
			{
				CSApplicationHelper.Application.Document.Graph.Generate();
				CSUtil.Redraw();
			}
		}
		catch (Exception)
		{
			CSUtil.Print("CS Error in Graph on run command\n");
		}
	}

	public override void OnReplaceObject(ref MRhinoDoc doc, ref MRhinoObject oldObject, ref MRhinoObject newObject)
	{
		if (CSApplicationHelper.Application.InRunScriptBlock)
		{
			return;
		}
		foreach (CSBlock item in CSApplicationHelper.Application.Document.Graph.ActiveOutputObjects())
		{
			if (item is ICSBlockObjectWatcher iCSBlockObjectWatcher)
			{
				iCSBlockObjectWatcher.CheckForChangeOnReplace(oldObject, newObject);
			}
		}
	}
}
