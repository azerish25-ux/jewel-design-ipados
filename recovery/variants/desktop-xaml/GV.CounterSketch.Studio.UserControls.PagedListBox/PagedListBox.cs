using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Controls.Primitives;

namespace GV.CounterSketch.Studio.UserControls.PagedListBox;

public partial class PagedListBox : SurfaceListBox, IComponentConnector
{
	public static readonly DependencyProperty FirstVisibleItemProperty = DependencyProperty.Register("FirstVisibleItem", typeof(IPagedListBoxItem), typeof(PagedListBox));

	public static readonly DependencyProperty HorizontalScrollOffsetProperty = DependencyProperty.Register("HorizontalScrollOffset", typeof(double), typeof(PagedListBox), new UIPropertyMetadata(0.0, OnHorizontalScrollOffsetChanged));

	public static readonly DependencyProperty IncludeLastItemInPageWhenCyclingProperty = DependencyProperty.Register("IncludeLastItemInPageWhenCycling", typeof(bool), typeof(PagedListBox), new UIPropertyMetadata(false));

	public static readonly DependencyProperty IsFieldModificationEnabledProperty = DependencyProperty.Register("IsFieldModificationEnabled", typeof(bool), typeof(PagedListBox), new UIPropertyMetadata(false));

	public static readonly DependencyProperty IsResultSetEmptyProperty = DependencyProperty.Register("IsResultSetEmptyVisibility", typeof(Visibility), typeof(PagedListBox), new UIPropertyMetadata(Visibility.Collapsed));

	public static readonly DependencyProperty IsScrollToHomeOnResetEnabledProperty = DependencyProperty.Register("IsScrollToHomeOnResetEnabled", typeof(bool), typeof(PagedListBox), new PropertyMetadata(true));

	public static readonly DependencyProperty ItemsPerPageProperty = DependencyProperty.Register("ItemsPerPage", typeof(int), typeof(PagedListBox), new UIPropertyMetadata(0));

	public static readonly DependencyProperty MousedOverItemProperty = DependencyProperty.Register("MousedOverItem", typeof(IPagedListBoxItem), typeof(PagedListBox));

	public static readonly DependencyProperty PagingIndicatorVisibilityProperty = DependencyProperty.Register("PagingIndicatorVisibility", typeof(Visibility), typeof(PagedListBox), new UIPropertyMetadata(Visibility.Visible));

	public static readonly RoutedEvent ItemClickedEvent = EventManager.RegisterRoutedEvent("ItemClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PagedListBox));

	public static readonly DependencyProperty ScrollToItemProperty = DependencyProperty.Register("ScrollToItem", typeof(IPagedListBoxItem), typeof(PagedListBox), new FrameworkPropertyMetadata
	{
		PropertyChangedCallback = OnScrollToItemChanged,
		BindsTwoWayByDefault = true
	});

	private DoubleAnimation _animation;

	private int _currentPageIndex;

	private bool _isAnimating;

	private ListBox _listBoxPageIndicator;

	private SurfaceRepeatButton _pageLeftButton;

	private SurfaceRepeatButton _pageRightButton;

	private ObservableCollection<object> _pages;

	private ScrollViewer _scrollViewer;

	public IPagedListBoxItem FirstVisibleItem
	{
		get
		{
			return (IPagedListBoxItem)GetValue(FirstVisibleItemProperty);
		}
		set
		{
			SetValue(FirstVisibleItemProperty, value);
		}
	}

	public double HorizontalScrollOffset
	{
		get
		{
			return (double)GetValue(HorizontalScrollOffsetProperty);
		}
		set
		{
			SetValue(HorizontalScrollOffsetProperty, value);
		}
	}

	public bool IncludeLastItemInPageWhenCycling
	{
		get
		{
			return (bool)GetValue(IncludeLastItemInPageWhenCyclingProperty);
		}
		set
		{
			SetValue(IncludeLastItemInPageWhenCyclingProperty, value);
		}
	}

	public bool IsFieldModificationEnabled
	{
		get
		{
			return (bool)GetValue(IsFieldModificationEnabledProperty);
		}
		set
		{
			SetValue(IsFieldModificationEnabledProperty, value);
		}
	}

	public Visibility IsResultSetEmptyVisibility
	{
		get
		{
			return (Visibility)GetValue(IsResultSetEmptyProperty);
		}
		set
		{
			SetValue(IsResultSetEmptyProperty, value);
		}
	}

	public bool IsScrollToHomeOnResetEnabled
	{
		get
		{
			return (bool)GetValue(IsScrollToHomeOnResetEnabledProperty);
		}
		set
		{
			SetValue(IsScrollToHomeOnResetEnabledProperty, value);
		}
	}

	public int ItemsPerPage
	{
		get
		{
			return (int)GetValue(ItemsPerPageProperty);
		}
		set
		{
			SetValue(ItemsPerPageProperty, value);
		}
	}

	public IPagedListBoxItem MousedOverItem
	{
		get
		{
			return (IPagedListBoxItem)GetValue(MousedOverItemProperty);
		}
		set
		{
			SetValue(MousedOverItemProperty, value);
		}
	}

	public Visibility PagingIndicatorVisibility
	{
		get
		{
			return (Visibility)GetValue(PagingIndicatorVisibilityProperty);
		}
		set
		{
			SetValue(PagingIndicatorVisibilityProperty, value);
		}
	}

	public IPagedListBoxItem ScrollToItem
	{
		get
		{
			return (IPagedListBoxItem)GetValue(ScrollToItemProperty);
		}
		set
		{
			SetValue(ScrollToItemProperty, value);
		}
	}

	private double ItemWidth
	{
		get
		{
			double result = 0.0;
			if (base.ItemContainerGenerator.ContainerFromIndex(0) is SurfaceListBoxItem { ActualWidth: var actualWidth, Margin: var margin } surfaceListBoxItem)
			{
				result = actualWidth + margin.Left + surfaceListBoxItem.Margin.Right;
			}
			return result;
		}
	}

	public ICommand GoToSelectedPage { get; private set; }

	public ICommand PageCollectionLeft { get; private set; }

	public ICommand PageCollectionRight { get; private set; }

	public ObservableCollection<object> Pages
	{
		get
		{
			return _pages ?? (_pages = new ObservableCollection<object>());
		}
		set
		{
			_pages = value;
		}
	}

	public ICommand RemoveItem { get; private set; }

	public event RoutedEventHandler ItemClicked
	{
		add
		{
			AddHandler(ItemClickedEvent, value);
		}
		remove
		{
			RemoveHandler(ItemClickedEvent, value);
		}
	}

	[DebuggerStepThrough]
	private static void OnHorizontalScrollOffsetChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
	{
		if (dependencyObject is PagedListBox pagedListBox && pagedListBox.Template.FindName("pagingScrollViewer", pagedListBox) is ScrollViewer scrollViewer)
		{
			scrollViewer.ScrollToHorizontalOffset(pagedListBox.HorizontalScrollOffset);
		}
	}

	public PagedListBox()
	{
		InitializeComponent();
		if (!DesignerProperties.GetIsInDesignMode(this))
		{
			InitializeCommands();
			base.Loaded += PagedListBoxLoaded;
			base.SelectionChanged += PagedListBoxSelectionChanged;
			base.IsVisibleChanged += PagedListBoxIsVisibleChanged;
			base.PreviewMouseUp += PagedListBoxPreviewMouseUp;
		}
	}

	public void ScrollItemIntoView(IPagedListBoxItem pagedListBoxItem)
	{
		UpdateLayout();
		if (pagedListBoxItem != null && _scrollViewer != null)
		{
			AnimateScrollContent(_scrollViewer.HorizontalOffset, GetOffsetForPageItem(pagedListBoxItem));
			ScrollToItem = null;
		}
	}

	public void ScrollSelectedItemIntoView()
	{
		UpdateLayout();
		if (base.SelectedItem != null && _scrollViewer != null)
		{
			_scrollViewer.ScrollToHorizontalOffset(GetOffsetForPage(GetPageForItem(base.SelectedItem as IPagedListBoxItem)));
		}
	}

	private void AnimateScrollContent(double currentOffset, double scrollToOffset)
	{
		if (_scrollViewer is SurfaceScrollViewer { AreAnyTouchesCaptured: false } surfaceScrollViewer)
		{
			surfaceScrollViewer.StopFlick();
		}
		if (_isAnimating)
		{
			BeginAnimation(HorizontalScrollOffsetProperty, null);
			_isAnimating = false;
		}
		_isAnimating = true;
		_animation = new DoubleAnimation
		{
			EasingFunction = new ExponentialEase
			{
				EasingMode = EasingMode.EaseInOut,
				Exponent = 10.0
			},
			From = currentOffset,
			To = scrollToOffset,
			Duration = TimeSpan.FromSeconds(0.5)
		};
		_animation.Completed += delegate
		{
			_isAnimating = false;
		};
		BeginAnimation(HorizontalScrollOffsetProperty, _animation);
	}

	private int GetItemsPerPage()
	{
		int num = 0;
		if (ItemsPerPage == 0)
		{
			if (base.Items.Count > 0 && _scrollViewer != null && ItemWidth > 0.0)
			{
				num = Convert.ToInt32(Math.Floor(_scrollViewer.ViewportWidth / ItemWidth));
			}
		}
		else
		{
			num = ItemsPerPage;
		}
		if (IncludeLastItemInPageWhenCycling && num > 0)
		{
			num--;
		}
		return num;
	}

	private double GetOffsetForPage(int pageIndex)
	{
		double result = 0.0;
		if (_scrollViewer != null)
		{
			int itemsPerPage = GetItemsPerPage();
			result = ItemWidth * (double)itemsPerPage * (double)pageIndex;
		}
		return result;
	}

	private double GetOffsetForPageItem(IPagedListBoxItem pagedListBoxItem)
	{
		double result = 0.0;
		if (base.Items.Count > 0)
		{
			int num = base.Items.IndexOf(pagedListBoxItem);
			if (_scrollViewer != null)
			{
				result = ItemWidth * (double)num;
			}
		}
		return result;
	}

	private int GetPageForItem(IPagedListBoxItem pagedListBoxItem)
	{
		int result = 0;
		if (base.Items.Count > 0)
		{
			int itemsPerPage = GetItemsPerPage();
			int num = base.Items.IndexOf(pagedListBoxItem);
			if (itemsPerPage != 0)
			{
				result = num / itemsPerPage;
			}
		}
		return result;
	}

	private int GetPageIndexFromOffset(double horizontalOffset)
	{
		int result = 0;
		if (ItemWidth > 0.0 && GetItemsPerPage() > 0)
		{
			result = Convert.ToInt32(horizontalOffset / ((double)GetItemsPerPage() * ItemWidth));
		}
		return result;
	}

	private void InitializeCommands()
	{
		PageCollectionLeft = new RoutedCommand();
		CommandBinding commandBinding = new CommandBinding(PageCollectionLeft);
		commandBinding.Executed += delegate(object s, ExecutedRoutedEventArgs e)
		{
			if (e.Parameter is ScrollViewer scrollViewer)
			{
				if (_currentPageIndex != 0)
				{
					_currentPageIndex--;
				}
				AnimateScrollContent(scrollViewer.HorizontalOffset, GetOffsetForPage(_currentPageIndex));
			}
		};
		base.CommandBindings.Add(commandBinding);
		PageCollectionRight = new RoutedCommand();
		CommandBinding commandBinding2 = new CommandBinding(PageCollectionRight);
		commandBinding2.Executed += delegate(object s, ExecutedRoutedEventArgs e)
		{
			if (e.Parameter is ScrollViewer scrollViewer)
			{
				if (_currentPageIndex != Pages.Count - 1)
				{
					_currentPageIndex++;
				}
				AnimateScrollContent(scrollViewer.HorizontalOffset, GetOffsetForPage(_currentPageIndex));
			}
		};
		base.CommandBindings.Add(commandBinding2);
		RemoveItem = new RoutedCommand();
		CommandBinding commandBinding3 = new CommandBinding(RemoveItem);
		commandBinding3.Executed += delegate(object s, ExecutedRoutedEventArgs e)
		{
			if (e.Parameter is IPagedListBoxItem removeItem)
			{
				base.Items.Remove(removeItem);
				if (_scrollViewer != null)
				{
					PadScrollViewerExtentWidth();
					UpdatePagePlaceholderCollection();
					UpdatePageIndicatorInformation();
				}
			}
		};
		base.CommandBindings.Add(commandBinding3);
		GoToSelectedPage = new RoutedCommand();
		CommandBinding commandBinding4 = new CommandBinding(GoToSelectedPage);
		commandBinding4.Executed += delegate(object s, ExecutedRoutedEventArgs e)
		{
			object parameter = e.Parameter;
			if (parameter != null)
			{
				AnimateScrollContent(_scrollViewer.HorizontalOffset, GetOffsetForPage(Pages.IndexOf(parameter)));
			}
		};
		base.CommandBindings.Add(commandBinding4);
	}

	private void ItemContainerGeneratorItemsChanged(object sender, ItemsChangedEventArgs e)
	{
		if (e.Action == NotifyCollectionChangedAction.Reset && IsScrollToHomeOnResetEnabled)
		{
			AnimateScrollContent(_scrollViewer.HorizontalOffset, 0.0);
			PadScrollViewerExtentWidth();
			UpdatePageIndicatorInformation();
		}
		IsResultSetEmptyVisibility = ((base.Items.Count != 0) ? Visibility.Collapsed : Visibility.Visible);
	}

	private static void OnScrollToItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (e.OldValue != e.NewValue)
		{
			GlobalErrorHandler.WriteMethodInfo("OnScrollToItemChanged");
			((PagedListBox)d).ScrollItemIntoView(e.NewValue as IPagedListBoxItem);
		}
	}

	private void PadScrollViewerExtentWidth()
	{
		int itemsPerPage = GetItemsPerPage();
		if (itemsPerPage <= 0)
		{
			return;
		}
		int num = base.Items.Count % itemsPerPage;
		if (_scrollViewer != null)
		{
			double right = ((num <= 0) ? (_scrollViewer.ViewportWidth - (double)itemsPerPage * ItemWidth) : (_scrollViewer.ViewportWidth - (double)num * ItemWidth));
			if (base.ItemContainerGenerator.ContainerFromIndex((base.Items.Count > 0) ? (base.Items.Count - 1) : 0) is SurfaceListBoxItem surfaceListBoxItem)
			{
				surfaceListBoxItem.SetValue(FrameworkElement.MarginProperty, new Thickness(surfaceListBoxItem.Margin.Left, surfaceListBoxItem.Margin.Top, right, surfaceListBoxItem.Margin.Bottom));
			}
		}
	}

	private void PagedListBoxIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if ((bool)e.NewValue)
		{
			ScrollSelectedItemIntoView();
		}
	}

	private void PagedListBoxLoaded(object sender, RoutedEventArgs routedEventArgs)
	{
		if (ItemsPerPage > 0)
		{
			SetDesiredControlWidth();
		}
		_scrollViewer = base.Template.FindName("pagingScrollViewer", this) as ScrollViewer;
		UpdatePagePlaceholderCollection();
		if (_scrollViewer != null)
		{
			_scrollViewer.SizeChanged += ScrollViewerSizeChanged;
			_scrollViewer.ScrollChanged += ScrollViewerScrollChanged;
			base.ItemContainerGenerator.ItemsChanged += ItemContainerGeneratorItemsChanged;
		}
		_listBoxPageIndicator = base.Template.FindName("listBoxPageIndicator", this) as ListBox;
		if (_listBoxPageIndicator != null)
		{
			_listBoxPageIndicator.Visibility = PagingIndicatorVisibility;
			if (PagingIndicatorVisibility == Visibility.Collapsed && base.Template.FindName("gridItemPresenter", this) is Grid element)
			{
				Grid.SetRowSpan(element, 2);
			}
			_listBoxPageIndicator.SelectionChanged += delegate(object s, SelectionChangedEventArgs e)
			{
				e.Handled = true;
			};
			_listBoxPageIndicator.ItemsSource = Pages;
		}
		_pageLeftButton = base.Template.FindName("pageLeftButton", this) as SurfaceRepeatButton;
		_pageRightButton = base.Template.FindName("pageRightButton", this) as SurfaceRepeatButton;
		UpdatePageIndicatorInformation();
		PadScrollViewerExtentWidth();
		if (_scrollViewer != null)
		{
			_scrollViewer.ScrollToHorizontalOffset(GetOffsetForPage(GetPageForItem(base.SelectedItem as IPagedListBoxItem)));
		}
	}

	private void PagedListBoxPreviewMouseUp(object sender, MouseButtonEventArgs e)
	{
		foreach (object item in (IEnumerable)base.Items)
		{
			if (base.ItemContainerGenerator.ContainerFromItem(item) is SurfaceListBoxItem { IsMouseDirectlyOver: not false })
			{
				RaiseEvent(new RoutedEventArgs
				{
					RoutedEvent = ItemClickedEvent,
					Source = this
				});
			}
		}
	}

	private void PagedListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.AddedItems.Count > 0)
		{
			MousedOverItem = e.AddedItems[0] as IPagedListBoxItem;
			if (_scrollViewer != null)
			{
				_scrollViewer.ScrollToHorizontalOffset(GetOffsetForPage(GetPageForItem(base.SelectedItem as IPagedListBoxItem)));
			}
		}
	}

	private void ResetVisuals()
	{
		UpdatePagePlaceholderCollection();
		_currentPageIndex = ((base.SelectedItem != null) ? GetPageForItem(base.SelectedItem as IPagedListBoxItem) : 0);
		AnimateScrollContent(_scrollViewer.HorizontalOffset, GetOffsetForPage(_currentPageIndex));
		PadScrollViewerExtentWidth();
		UpdatePageIndicatorInformation();
	}

	private void ScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
	{
		_currentPageIndex = GetPageIndexFromOffset(e.HorizontalOffset);
		UpdateFirstVisibleItem();
		UpdatePagePlaceholderCollection();
		UpdatePageIndicatorInformation();
	}

	private void ScrollViewerSizeChanged(object sender, SizeChangedEventArgs e)
	{
		ResetVisuals();
	}

	private void SetDesiredControlWidth()
	{
		if (ItemsPerPage > 0 && !(base.Width > 0.0) && ItemWidth > 0.0)
		{
			base.Width = ItemWidth * (double)ItemsPerPage + 100.0;
		}
	}

	private void UpdateFirstVisibleItem()
	{
		if (!_isAnimating && _scrollViewer != null && ItemWidth != 0.0)
		{
			int num = (int)Math.Ceiling(_scrollViewer.HorizontalOffset / ItemWidth);
			if (base.Items != null && base.Items.Count > 0 && base.Items.Count > num)
			{
				object itemAt = base.Items.GetItemAt(num);
				FirstVisibleItem = itemAt as IPagedListBoxItem;
			}
		}
	}

	private void UpdatePageIndicatorInformation()
	{
		if (_listBoxPageIndicator == null)
		{
			return;
		}
		_listBoxPageIndicator.SelectedIndex = _currentPageIndex;
		if (_currentPageIndex == 0)
		{
			if (Pages.Count <= 1)
			{
				_pageLeftButton.IsEnabled = false;
				_pageRightButton.IsEnabled = false;
			}
			else if (_scrollViewer.HorizontalOffset > ItemWidth)
			{
				_pageLeftButton.IsEnabled = true;
				_pageRightButton.IsEnabled = true;
			}
			else
			{
				_pageLeftButton.IsEnabled = false;
				_pageRightButton.IsEnabled = true;
			}
		}
		else if (_currentPageIndex == Pages.Count - 1)
		{
			_pageLeftButton.IsEnabled = true;
			_pageRightButton.IsEnabled = false;
		}
		else
		{
			_pageLeftButton.IsEnabled = true;
			_pageRightButton.IsEnabled = true;
		}
	}

	private void UpdatePagePlaceholderCollection()
	{
		Pages.Clear();
		if (base.Items.Count <= 0)
		{
			return;
		}
		int itemsPerPage = GetItemsPerPage();
		if (itemsPerPage > 0)
		{
			int num = base.Items.Count / itemsPerPage;
			if (base.Items.Count % GetItemsPerPage() == 0)
			{
				num--;
			}
			for (int i = 0; i <= num; i++)
			{
				Pages.Add(new object());
			}
		}
	}
}
