using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
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
			ListBoxEditItem listBoxEditItem = LayoutHelper.FindParentObject<ListBoxEditItem>(result.VisualHit);
			if (listBoxEditItem != null)
			{
				Element = listBoxEditItem;
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
			if (base.ListBox.ItemsSource is ICollectionView)
			{
				return ((ICollectionView)base.ListBox.ItemsSource).SourceCollection as IList;
			}
			return base.ListBox.ItemsSource as IList;
		}
	}

	public override void OnDragOver(DragDropManagerBase sourceManager, UIElement source, Point pt)
	{
		ListBoxDragOverEventArgs e = RaiseDragOverEvent(sourceManager, pt, DropTargetType.None);
		base.DropEventIsLocked = (e.Handled ? (!e.AllowDrop) : (!base.AllowDrop || sourceManager.DraggingRows == null || sourceManager.DraggingRows.Count < 1));
		if (!base.DropEventIsLocked)
		{
			ProcessTargetItem(sourceManager, pt);
		}
	}

	public override void ShowDropMarker(UIElement element, TableDragIndicatorPosition dragIndicatorPosition)
	{
		base.ShowDropMarker(element, dragIndicatorPosition);
	}

	protected override IDragElement CreateDragElement(Point offset, FrameworkElement owner)
	{
		offset.Y -= 50.0;
		return base.CreateDragElement(offset, owner);
	}

	protected override TableDropMarkerControl CreateDropMarkerControl()
	{
		return base.CreateDropMarkerControl();
	}

	protected virtual ListBoxEditItem GetVisibleHitTestElement(Point pt)
	{
		DragDropHitTestResult dragDropHitTestResult = new DragDropHitTestResult();
		VisualTreeHelper.HitTest(base.ListBox, null, dragDropHitTestResult.CallBack, new PointHitTestParameters(pt));
		return dragDropHitTestResult.Element;
	}

	protected override void OnDrop(DragDropManagerBase sourceManager, UIElement source, Point pt)
	{
		if (base.DropEventIsLocked)
		{
			return;
		}
		ListBoxDropEventArgs e = RaiseDropEvent(sourceManager);
		if (!e.Handled && sourceManager.DraggingRows.Count > 0 && base.AllowDrop)
		{
			ProcessTargetItem(e.SourceManager, pt);
			IList itemsSource = ItemsSource;
			IList source2 = e.SourceManager.GetSource(null);
			foreach (object draggedRow in e.DraggedRows)
			{
				if (TargetItem == null || draggedRow != TargetItem.Content)
				{
					source2.Remove(draggedRow);
					switch (e.SourceManager.ViewInfo.DropTargetType)
					{
					case DropTargetType.DataArea:
						itemsSource.Add(draggedRow);
						break;
					case DropTargetType.InsertRowsAfter:
						itemsSource.Insert(itemsSource.IndexOf(TargetItem.Content) + 1, draggedRow);
						break;
					case DropTargetType.InsertRowsBefore:
						itemsSource.Insert(Math.Max(itemsSource.IndexOf(TargetItem.Content), 0), draggedRow);
						break;
					}
				}
			}
			RaiseDroppedEvent(sourceManager, e.DraggedRows);
		}
		HideDropMarker();
	}

	protected virtual void ProcessTargetItem(DragDropManagerBase sourceManager, Point pt)
	{
		TargetItem = GetVisibleHitTestElement(pt);
		if (TargetItem == null)
		{
			sourceManager.ViewInfo.DropTargetRow = null;
			sourceManager.SetDropTargetType(DropTargetType.DataArea);
			ShowDropMarker(base.ListBox, TableDragIndicatorPosition.None);
			return;
		}
		Point position = Mouse.GetPosition(TargetItem);
		double actualHeight = TargetItem.ActualHeight;
		if (position.Y < actualHeight / 2.0)
		{
			sourceManager.SetDropTargetType(DropTargetType.InsertRowsBefore);
			ShowDropMarker(TargetItem, TableDragIndicatorPosition.Top);
		}
		else
		{
			sourceManager.SetDropTargetType(DropTargetType.InsertRowsAfter);
			ShowDropMarker(TargetItem, TableDragIndicatorPosition.Bottom);
		}
	}
}
