using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CS.CSObjects.CSBlocks;
using CS.CSValues.CSValueHandle.Interfaces;
using CS.Selection;
using GV.CounterSketch.Shared.Model.Objects;
using Rhino.Display;
using Rhino.UI;

namespace CS.CSValues.CSValueHandle.HandleSupportClasses;

public sealed class CSValueHandleMouseWatcher : MouseCallback
{
	private bool _down;

	private ICSVHandle _selected;

	protected override void OnMouseDown(MouseCallbackEventArgs e)
	{
		CSValueHandleHelper.IsMouseDown = true;
		if (!CSValueHandleHelper.MouseButtonLeft(e))
		{
			((MouseCallback)this).OnMouseDown(e);
			return;
		}
		CSApplicationHelper.Application.RaiseRhinoWindowClickedEvent();
		if (_selected == null)
		{
			((MouseCallback)this).OnMouseDown(e);
			return;
		}
		_down = true;
		_selected.MouseDown = true;
		((CancelEventArgs)(object)e).Cancel = true;
	}

	protected override void OnMouseMove(MouseCallbackEventArgs e)
	{
		if (_down)
		{
			_selected.MouseMove(e);
			((CancelEventArgs)(object)e).Cancel = true;
			((MouseCallback)this).OnMouseMove(e);
			return;
		}
		IEnumerable<ICSVHandle> enumerable = CSApplicationHelper.Application.Document.Graph.ActiveObjects().OfType<ICSVHandle>();
		ICSVHandle iCSVHandle = null;
		foreach (ICSVHandle item in enumerable)
		{
			if (iCSVHandle == null)
			{
				if (item.MouseMoveHitTest(e))
				{
					iCSVHandle = item;
				}
			}
			else
			{
				item.MouseOver = false;
			}
		}
		if (_selected != iCSVHandle || (iCSVHandle != null && iCSVHandle.SelectionChanged()))
		{
			CSUtil.Redraw();
		}
		_selected = iCSVHandle;
		((MouseCallback)this).OnMouseMove(e);
	}

	protected override void OnMouseUp(MouseCallbackEventArgs e)
	{
		CSValueHandleHelper.IsMouseDown = false;
		if (CSValueHandleHelper.MouseButtonNone(e))
		{
			((MouseCallback)this).OnMouseUp(e);
			return;
		}
		if (e.Button == MouseButtons.Left)
		{
			if (_selected != null)
			{
				_selected.MouseDown = false;
				_selected.MouseUp(e);
				_down = false;
				CSApplicationHelper.Application.RaiseFreehandObjectSelectionChangedEvent(CSValueHandleHelper.CurrentHandle, CSValueHandleHelper.SelectedHandles);
				((CancelEventArgs)(object)e).Cancel = true;
				return;
			}
			ICSVHandle iCSVHandle = CheckGeometryForHitTestSelection(e.View, e.ViewportPoint);
			if (iCSVHandle != null)
			{
				CSValueHandleHelper.CurrentHandle = iCSVHandle;
			}
			else
			{
				CSValueHandleHelper.CurrentHandle = null;
			}
			CSValueHandleHelper.SelectedHandles.Clear();
			CSApplicationHelper.Application.RaiseFreehandObjectSelectionChangedEvent(CSValueHandleHelper.CurrentHandle, CSValueHandleHelper.SelectedHandles);
		}
		CSUtil.Redraw();
		((MouseCallback)this).OnMouseUp(e);
	}

	private static ICSVHandle CheckGeometryForHitTestSelection(RhinoView view, Point point)
	{
		SelectionHelper.CurrentSelectionMode = CS.Selection.SelectionMode.Block;
		SelectionHelper.IsReturningDirectHits = true;
		SelectionHelper.IncludeAllMeshes = true;
		ICSVHandle hitTestHandle = null;
		List<CSBlock> list = CSApplicationHelper.Application.Document.Graph.ActiveObjects().OfType<ICSVHandle>().ToList()
			.OfType<CSBlock>()
			.ToList();
		List<string> itemsUnderMouse = SelectionHelper.GetItemsUnderMouse(view, point, list);
		list.ForEach(delegate(CSBlock handle)
		{
			ImportGroup importGroup = CSApplicationHelper.Application.Document.Graph.ImportedCSGParts.FirstOrDefault((ImportGroup _) => _.IsViewportVisible && _.Identifier == handle.ImportGroup.Identifier);
			if (importGroup != null && importGroup.BlockIdentifiers.Select((Guid _) => _.ToString()).Intersect(itemsUnderMouse).Any())
			{
				hitTestHandle = handle as ICSVHandle;
			}
		});
		return hitTestHandle;
	}
}
