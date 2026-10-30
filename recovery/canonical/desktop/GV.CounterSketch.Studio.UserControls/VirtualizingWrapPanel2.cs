using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.UserControls;

public class VirtualizingWrapPanel2 : VirtualizingPanel, IScrollInfo
{
	internal class ExtentInfo
	{
		public int ItemsPerLine;

		public int TotalLines;

		public double ExtentHeight;

		public double MaxVerticalOffset;
	}

	public class ItemLayoutInfo
	{
		public int FirstRealizedItemIndex;

		public double FirstRealizedLineTop;

		public double FirstRealizedItemLeft;

		public int LastRealizedItemIndex;
	}

	private const double ScrollLineAmount = 16.0;

	private Size _extentSize;

	private Size _viewportSize;

	private Point _offset;

	private ItemsControl _itemsControl;

	private readonly Dictionary<UIElement, Rect> _childLayouts = new Dictionary<UIElement, Rect>();

	public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(VirtualizingWrapPanel2), new PropertyMetadata(1.0, HandleItemDimensionChanged));

	public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(VirtualizingWrapPanel2), new PropertyMetadata(1.0, HandleItemDimensionChanged));

	private static readonly DependencyProperty VirtualItemIndexProperty = DependencyProperty.RegisterAttached("VirtualItemIndex", typeof(int), typeof(VirtualizingWrapPanel2), new PropertyMetadata(-1));

	private IRecyclingItemContainerGenerator _itemsGenerator;

	private bool _isInMeasure;

	public double ItemHeight
	{
		get
		{
			return (double)GetValue(ItemHeightProperty);
		}
		set
		{
			SetValue(ItemHeightProperty, value);
		}
	}

	public double ItemWidth
	{
		get
		{
			return (double)GetValue(ItemWidthProperty);
		}
		set
		{
			SetValue(ItemWidthProperty, value);
		}
	}

	public bool CanVerticallyScroll { get; set; }

	public bool CanHorizontallyScroll { get; set; }

	public double ExtentWidth => _extentSize.Width;

	public double ExtentHeight => _extentSize.Height;

	public double ViewportWidth => _viewportSize.Width;

	public double ViewportHeight => _viewportSize.Height;

	public double HorizontalOffset => _offset.X;

	public double VerticalOffset => _offset.Y;

	public ScrollViewer ScrollOwner { get; set; }

	private static int GetVirtualItemIndex(DependencyObject obj)
	{
		return (int)obj.GetValue(VirtualItemIndexProperty);
	}

	private static void SetVirtualItemIndex(DependencyObject obj, int value)
	{
		obj.SetValue(VirtualItemIndexProperty, value);
	}

	public VirtualizingWrapPanel2()
	{
		if (!DesignerProperties.GetIsInDesignMode(this))
		{
			base.Dispatcher.BeginInvoke(new Action(Initialize));
		}
	}

	private void Initialize()
	{
		_itemsControl = ItemsControl.GetItemsOwner(this);
		_itemsGenerator = (IRecyclingItemContainerGenerator)base.ItemContainerGenerator;
		InvalidateMeasure();
	}

	protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
	{
		base.OnItemsChanged(sender, args);
		InvalidateMeasure();
	}

	protected override Size MeasureOverride(Size availableSize)
	{
		if (_itemsControl == null)
		{
			return availableSize;
		}
		_isInMeasure = true;
		_childLayouts.Clear();
		ExtentInfo extentInfo = GetExtentInfo(availableSize, ItemHeight);
		EnsureScrollOffsetIsWithinConstrains(extentInfo);
		ItemLayoutInfo layoutInfo = GetLayoutInfo(availableSize, ItemHeight, extentInfo);
		RecycleItems(layoutInfo);
		GeneratorPosition position = _itemsGenerator.GeneratorPositionFromIndex(layoutInfo.FirstRealizedItemIndex);
		int num = 0;
		double num2 = layoutInfo.FirstRealizedItemLeft;
		double num3 = layoutInfo.FirstRealizedLineTop;
		using (_itemsGenerator.StartAt(position, GeneratorDirection.Forward, allowStartAtRealizedItem: true))
		{
			int num4 = layoutInfo.FirstRealizedItemIndex;
			while (num4 <= layoutInfo.LastRealizedItemIndex)
			{
				bool isNewlyRealized;
				UIElement uIElement = (UIElement)_itemsGenerator.GenerateNext(out isNewlyRealized);
				SetVirtualItemIndex(uIElement, num4);
				if (isNewlyRealized)
				{
					InsertInternalChild(num, uIElement);
				}
				else if (num < base.Children.Count)
				{
					if (base.Children[num] != uIElement)
					{
						int num5 = base.Children.IndexOf(uIElement);
						if (num5 >= 0)
						{
							RemoveInternalChildRange(num5, 1);
						}
						InsertInternalChild(num, uIElement);
					}
				}
				else
				{
					AddInternalChild(uIElement);
				}
				_itemsGenerator.PrepareItemContainer(uIElement);
				uIElement.Measure(new Size(ItemWidth, ItemHeight));
				_childLayouts.Add(uIElement, new Rect(num2, num3, ItemWidth, ItemHeight));
				if (num2 + ItemWidth * 2.0 >= availableSize.Width)
				{
					num3 += ItemHeight;
					num2 = 0.0;
				}
				else
				{
					num2 += ItemWidth;
				}
				num4++;
				num++;
			}
		}
		RemoveRedundantChildren();
		UpdateScrollInfo(availableSize, extentInfo);
		Size result = new Size(double.IsInfinity(availableSize.Width) ? 0.0 : availableSize.Width, double.IsInfinity(availableSize.Height) ? 0.0 : availableSize.Height);
		_isInMeasure = false;
		return result;
	}

	private void EnsureScrollOffsetIsWithinConstrains(ExtentInfo extentInfo)
	{
		_offset.Y = Clamp(_offset.Y, 0.0, extentInfo.MaxVerticalOffset);
	}

	private void RecycleItems(ItemLayoutInfo layoutInfo)
	{
		foreach (UIElement child in base.Children)
		{
			int virtualItemIndex = GetVirtualItemIndex(child);
			if (virtualItemIndex < layoutInfo.FirstRealizedItemIndex || virtualItemIndex > layoutInfo.LastRealizedItemIndex)
			{
				GeneratorPosition position = _itemsGenerator.GeneratorPositionFromIndex(virtualItemIndex);
				if (position.Index >= 0)
				{
					_itemsGenerator.Recycle(position, 1);
				}
			}
			SetVirtualItemIndex(child, -1);
		}
	}

	protected override Size ArrangeOverride(Size finalSize)
	{
		foreach (UIElement child in base.Children)
		{
			child.Arrange(_childLayouts[child]);
		}
		return finalSize;
	}

	private void UpdateScrollInfo(Size availableSize, ExtentInfo extentInfo)
	{
		_viewportSize = availableSize;
		_extentSize = new Size(availableSize.Width, extentInfo.ExtentHeight);
		InvalidateScrollInfo();
	}

	private void RemoveRedundantChildren()
	{
		for (int num = base.Children.Count - 1; num >= 0; num--)
		{
			if (GetVirtualItemIndex(base.Children[num]) == -1)
			{
				RemoveInternalChildRange(num, 1);
			}
		}
	}

	private ItemLayoutInfo GetLayoutInfo(Size availableSize, double itemHeight, ExtentInfo extentInfo)
	{
		if (_itemsControl == null)
		{
			return new ItemLayoutInfo();
		}
		int num = (int)Math.Floor(VerticalOffset / itemHeight);
		int num2 = Math.Max(extentInfo.ItemsPerLine * num - 1, 0);
		double firstRealizedItemLeft = (double)(num2 % extentInfo.ItemsPerLine) * ItemWidth - HorizontalOffset;
		double num3 = (double)(num2 / extentInfo.ItemsPerLine) * itemHeight - VerticalOffset;
		double num4 = ((num == 0) ? num3 : (num3 + ItemHeight));
		int num5 = (int)Math.Ceiling((availableSize.Height - num4) / itemHeight);
		int lastRealizedItemIndex = Math.Min(num2 + num5 * extentInfo.ItemsPerLine + 2, _itemsControl.Items.Count - 1);
		return new ItemLayoutInfo
		{
			FirstRealizedItemIndex = num2,
			FirstRealizedItemLeft = firstRealizedItemLeft,
			FirstRealizedLineTop = num3,
			LastRealizedItemIndex = lastRealizedItemIndex
		};
	}

	private ExtentInfo GetExtentInfo(Size viewPortSize, double itemHeight)
	{
		if (_itemsControl == null)
		{
			return new ExtentInfo();
		}
		int num = Math.Max((int)Math.Floor(viewPortSize.Width / ItemWidth), 1);
		int num2 = (int)Math.Ceiling((double)_itemsControl.Items.Count / (double)num);
		double num3 = Math.Max((double)num2 * ItemHeight, viewPortSize.Height);
		return new ExtentInfo
		{
			ItemsPerLine = num,
			TotalLines = num2,
			ExtentHeight = num3,
			MaxVerticalOffset = num3 - viewPortSize.Height
		};
	}

	public void LineUp()
	{
		SetVerticalOffset(VerticalOffset - 16.0);
	}

	public void LineDown()
	{
		SetVerticalOffset(VerticalOffset + 16.0);
	}

	public void LineLeft()
	{
		SetHorizontalOffset(HorizontalOffset + 16.0);
	}

	public void LineRight()
	{
		SetHorizontalOffset(HorizontalOffset - 16.0);
	}

	public void PageUp()
	{
		SetVerticalOffset(VerticalOffset - ViewportHeight);
	}

	public void PageDown()
	{
		SetVerticalOffset(VerticalOffset + ViewportHeight);
	}

	public void PageLeft()
	{
		SetHorizontalOffset(HorizontalOffset + ItemWidth);
	}

	public void PageRight()
	{
		SetHorizontalOffset(HorizontalOffset - ItemWidth);
	}

	public void MouseWheelUp()
	{
		SetVerticalOffset(VerticalOffset - 16.0 * (double)SystemParameters.WheelScrollLines);
	}

	public void MouseWheelDown()
	{
		SetVerticalOffset(VerticalOffset + 16.0 * (double)SystemParameters.WheelScrollLines);
	}

	public void MouseWheelLeft()
	{
		SetHorizontalOffset(HorizontalOffset - 16.0 * (double)SystemParameters.WheelScrollLines);
	}

	public void MouseWheelRight()
	{
		SetHorizontalOffset(HorizontalOffset + 16.0 * (double)SystemParameters.WheelScrollLines);
	}

	public void SetHorizontalOffset(double offset)
	{
		if (!_isInMeasure)
		{
			offset = Clamp(offset, 0.0, ExtentWidth - ViewportWidth);
			_offset = new Point(offset, _offset.Y);
			InvalidateScrollInfo();
			InvalidateMeasure();
		}
	}

	public void SetVerticalOffset(double offset)
	{
		if (!_isInMeasure)
		{
			offset = Clamp(offset, 0.0, ExtentHeight - ViewportHeight);
			_offset = new Point(_offset.X, offset);
			InvalidateScrollInfo();
			InvalidateMeasure();
		}
	}

	public Rect MakeVisible(Visual visual, Rect rectangle)
	{
		if (rectangle.IsEmpty || visual == null || visual == this || !IsAncestorOf(visual))
		{
			return Rect.Empty;
		}
		rectangle = visual.TransformToAncestor(this).TransformBounds(rectangle);
		Rect rect = new Rect(HorizontalOffset, VerticalOffset, ViewportWidth, ViewportHeight);
		rectangle.X += rect.X;
		rectangle.Y += rect.Y;
		rect.X = CalculateNewScrollOffset(rect.Left, rect.Right, rectangle.Left, rectangle.Right);
		rect.Y = CalculateNewScrollOffset(rect.Top, rect.Bottom, rectangle.Top, rectangle.Bottom);
		SetHorizontalOffset(rect.X);
		SetVerticalOffset(rect.Y);
		rectangle.Intersect(rect);
		rectangle.X -= rect.X;
		rectangle.Y -= rect.Y;
		return rectangle;
	}

	private static double CalculateNewScrollOffset(double topView, double bottomView, double topChild, double bottomChild)
	{
		bool flag = topChild < topView && bottomChild < bottomView;
		bool flag2 = bottomChild > bottomView && topChild > topView;
		bool flag3 = bottomChild - topChild > bottomView - topView;
		if (!flag && !flag2)
		{
			return topView;
		}
		if ((flag && !flag3) || (flag2 && flag3))
		{
			return topChild;
		}
		return bottomChild - (bottomView - topView);
	}

	public ItemLayoutInfo GetVisibleItemsRange()
	{
		return GetLayoutInfo(_viewportSize, ItemHeight, GetExtentInfo(_viewportSize, ItemHeight));
	}

	private void InvalidateScrollInfo()
	{
		if (ScrollOwner != null)
		{
			ScrollOwner.InvalidateScrollInfo();
		}
	}

	private static void HandleItemDimensionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is VirtualizingWrapPanel2 virtualizingWrapPanel)
		{
			virtualizingWrapPanel.InvalidateMeasure();
		}
	}

	private double Clamp(double value, double min, double max)
	{
		return Math.Min(Math.Max(value, min), max);
	}
}
