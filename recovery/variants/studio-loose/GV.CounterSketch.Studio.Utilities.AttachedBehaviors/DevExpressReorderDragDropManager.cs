using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.DragDrop;

namespace GV.CounterSketch.Studio.Utilities.AttachedBehaviors;

public class DevExpressReorderDragDropManager : ListBoxDragDropManager
{
	private class DragDropHitTestResult
	{
		public ListBoxEditItem Element { get; private set; }

		public HitTestResultBehavior CallBack(HitTestResult result)
		{
			ListBoxEditItem val = LayoutHelper.FindParentObject<ListBoxEditItem>(result.VisualHit);
			if (val != null)
			{
				Element = val;
				return HitTestResultBehavior.Stop;
			}
			return HitTestResultBehavior.Continue;
		}
	}

	private ListBoxEditItem TargetItem;

	protected override IList ItemsSource
	{
		get
		{
			if (((ListBoxDragDropManager)this).ListBox.ItemsSource is ICollectionView)
			{
				return ((ICollectionView)((ListBoxDragDropManager)this).ListBox.ItemsSource).SourceCollection as IList;
			}
			return ((ListBoxDragDropManager)this).ListBox.ItemsSource as IList;
		}
	}

	public override void OnDragOver(DragDropManagerBase sourceManager, UIElement source, Point pt)
	{
		ListBoxDragOverEventArgs e = ((ListBoxDragDropManager)this).RaiseDragOverEvent(sourceManager, pt, (DropTargetType)0);
		((DragDropManagerBase)this).DropEventIsLocked = (((DragDropDragOverEventArgs)e).Handled ? (!((DragDropDragOverEventArgs)e).AllowDrop) : (!((DragDropManagerBase)this).AllowDrop || sourceManager.DraggingRows == null || sourceManager.DraggingRows.Count < 1));
		if (!((DragDropManagerBase)this).DropEventIsLocked)
		{
			ProcessTargetItem(sourceManager, pt);
		}
	}

	public override void ShowDropMarker(UIElement element, TableDragIndicatorPosition dragIndicatorPosition)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		((DragDropManagerBase)this).ShowDropMarker(element, dragIndicatorPosition);
	}

	protected override IDragElement CreateDragElement(Point offset, FrameworkElement owner)
	{
		offset.Y -= 50.0;
		return ((DragDropManagerBase)this).CreateDragElement(offset, owner);
	}

	protected override TableDropMarkerControl CreateDropMarkerControl()
	{
		return ((DragDropManagerBase)this).CreateDropMarkerControl();
	}

	protected virtual ListBoxEditItem GetVisibleHitTestElement(Point pt)
	{
		DragDropHitTestResult dragDropHitTestResult = new DragDropHitTestResult();
		VisualTreeHelper.HitTest((Visual)(object)((ListBoxDragDropManager)this).ListBox, null, dragDropHitTestResult.CallBack, new PointHitTestParameters(pt));
		return dragDropHitTestResult.Element;
	}

	protected override void OnDrop(DragDropManagerBase sourceManager, UIElement source, Point pt)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected I4, but got Unknown
		if (((DragDropManagerBase)this).DropEventIsLocked)
		{
			return;
		}
		ListBoxDropEventArgs e = ((ListBoxDragDropManager)this).RaiseDropEvent(sourceManager);
		if (!((DropEventArgs)e).Handled && sourceManager.DraggingRows.Count > 0 && ((DragDropManagerBase)this).AllowDrop)
		{
			ProcessTargetItem(((DragDropEventArgs)e).SourceManager, pt);
			IList itemsSource = ((DragDropManagerBase)this).ItemsSource;
			IList source2 = ((DragDropEventArgs)e).SourceManager.GetSource((object)null);
			foreach (object draggedRow in ((DragDropEventArgs)e).DraggedRows)
			{
				if (TargetItem == null || draggedRow != ((ContentControl)(object)TargetItem).Content)
				{
					source2.Remove(draggedRow);
					DropTargetType dropTargetType = ((DragDropEventArgs)e).SourceManager.ViewInfo.DropTargetType;
					switch (dropTargetType - 1)
					{
					case 4:
						itemsSource.Add(draggedRow);
						break;
					case 0:
						itemsSource.Insert(itemsSource.IndexOf(((ContentControl)(object)TargetItem).Content) + 1, draggedRow);
						break;
					case 1:
						itemsSource.Insert(Math.Max(itemsSource.IndexOf(((ContentControl)(object)TargetItem).Content), 0), draggedRow);
						break;
					}
				}
			}
			((ListBoxDragDropManager)this).RaiseDroppedEvent(sourceManager, ((DragDropEventArgs)e).DraggedRows);
		}
		((DragDropManagerBase)this).HideDropMarker();
	}

	protected virtual void ProcessTargetItem(DragDropManagerBase sourceManager, Point pt)
	{
		TargetItem = GetVisibleHitTestElement(pt);
		if (TargetItem == null)
		{
			sourceManager.ViewInfo.DropTargetRow = null;
			sourceManager.SetDropTargetType((DropTargetType)5);
			((DragDropManagerBase)this).ShowDropMarker((UIElement)(object)((ListBoxDragDropManager)this).ListBox, (TableDragIndicatorPosition)0);
			return;
		}
		Point position = Mouse.GetPosition((IInputElement)TargetItem);
		double actualHeight = ((FrameworkElement)(object)TargetItem).ActualHeight;
		if (position.Y < actualHeight / 2.0)
		{
			sourceManager.SetDropTargetType((DropTargetType)2);
			((DragDropManagerBase)this).ShowDropMarker((UIElement)(object)TargetItem, (TableDragIndicatorPosition)1);
		}
		else
		{
			sourceManager.SetDropTargetType((DropTargetType)1);
			((DragDropManagerBase)this).ShowDropMarker((UIElement)(object)TargetItem, (TableDragIndicatorPosition)2);
		}
	}
}
