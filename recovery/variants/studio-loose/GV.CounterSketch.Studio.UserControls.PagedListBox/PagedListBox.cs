using System;
using System.CodeDom.Compiler;
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

public class PagedListBox : SurfaceListBox, IComponentConnector
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

	private bool _contentLoaded;

	public IPagedListBoxItem FirstVisibleItem
	{
		get
		{
			return (IPagedListBoxItem)((DependencyObject)this).GetValue(FirstVisibleItemProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(FirstVisibleItemProperty, (object)value);
		}
	}

	public double HorizontalScrollOffset
	{
		get
		{
			return (double)((DependencyObject)this).GetValue(HorizontalScrollOffsetProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(HorizontalScrollOffsetProperty, (object)value);
		}
	}

	public bool IncludeLastItemInPageWhenCycling
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IncludeLastItemInPageWhenCyclingProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IncludeLastItemInPageWhenCyclingProperty, (object)value);
		}
	}

	public bool IsFieldModificationEnabled
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsFieldModificationEnabledProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsFieldModificationEnabledProperty, (object)value);
		}
	}

	public Visibility IsResultSetEmptyVisibility
	{
		get
		{
			return (Visibility)((DependencyObject)this).GetValue(IsResultSetEmptyProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsResultSetEmptyProperty, (object)value);
		}
	}

	public bool IsScrollToHomeOnResetEnabled
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsScrollToHomeOnResetEnabledProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsScrollToHomeOnResetEnabledProperty, (object)value);
		}
	}

	public int ItemsPerPage
	{
		get
		{
			return (int)((DependencyObject)this).GetValue(ItemsPerPageProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(ItemsPerPageProperty, (object)value);
		}
	}

	public IPagedListBoxItem MousedOverItem
	{
		get
		{
			return (IPagedListBoxItem)((DependencyObject)this).GetValue(MousedOverItemProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(MousedOverItemProperty, (object)value);
		}
	}

	public Visibility PagingIndicatorVisibility
	{
		get
		{
			return (Visibility)((DependencyObject)this).GetValue(PagingIndicatorVisibilityProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(PagingIndicatorVisibilityProperty, (object)value);
		}
	}

	public IPagedListBoxItem ScrollToItem
	{
		get
		{
			return (IPagedListBoxItem)((DependencyObject)this).GetValue(ScrollToItemProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(ScrollToItemProperty, (object)value);
		}
	}

	private double ItemWidth
	{
		get
		{
			double result = 0.0;
			DependencyObject dependencyObject = ((ItemsControl)this).ItemContainerGenerator.ContainerFromIndex(0);
			SurfaceListBoxItem val = (SurfaceListBoxItem)(object)((dependencyObject is SurfaceListBoxItem) ? dependencyObject : null);
			if (val != null)
			{
				result = ((FrameworkElement)(object)val).ActualWidth + ((FrameworkElement)(object)val).Margin.Left + ((FrameworkElement)(object)val).Margin.Right;
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
			((UIElement)this).AddHandler(ItemClickedEvent, (Delegate)value);
		}
		remove
		{
			((UIElement)this).RemoveHandler(ItemClickedEvent, (Delegate)value);
		}
	}

	[DebuggerStepThrough]
	private static void OnHorizontalScrollOffsetChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
	{
		if (dependencyObject is PagedListBox pagedListBox && ((Control)(object)pagedListBox).Template.FindName("pagingScrollViewer", (FrameworkElement)(object)pagedListBox) is ScrollViewer scrollViewer)
		{
			scrollViewer.ScrollToHorizontalOffset(pagedListBox.HorizontalScrollOffset);
		}
	}

	public PagedListBox()
	{
		InitializeComponent();
		if (!DesignerProperties.GetIsInDesignMode((DependencyObject)(object)this))
		{
			InitializeCommands();
			((FrameworkElement)this).Loaded += PagedListBoxLoaded;
			((Selector)this).SelectionChanged += PagedListBoxSelectionChanged;
			((UIElement)this).IsVisibleChanged += PagedListBoxIsVisibleChanged;
			((UIElement)this).PreviewMouseUp += PagedListBoxPreviewMouseUp;
		}
	}

	public void ScrollItemIntoView(IPagedListBoxItem pagedListBoxItem)
	{
		((UIElement)this).UpdateLayout();
		if (pagedListBoxItem != null && _scrollViewer != null)
		{
			AnimateScrollContent(_scrollViewer.HorizontalOffset, GetOffsetForPageItem(pagedListBoxItem));
			ScrollToItem = null;
		}
	}

	public void ScrollSelectedItemIntoView()
	{
		((UIElement)this).UpdateLayout();
		if (((Selector)this).SelectedItem != null && _scrollViewer != null)
		{
			_scrollViewer.ScrollToHorizontalOffset(GetOffsetForPage(GetPageForItem(((Selector)this).SelectedItem as IPagedListBoxItem)));
		}
	}

	private void AnimateScrollContent(double currentOffset, double scrollToOffset)
	{
		ScrollViewer scrollViewer = _scrollViewer;
		SurfaceScrollViewer val = (SurfaceScrollViewer)(object)((scrollViewer is SurfaceScrollViewer) ? scrollViewer : null);
		if (val != null && !((UIElement)(object)val).AreAnyTouchesCaptured)
		{
			val.StopFlick();
		}
		if (_isAnimating)
		{
			((UIElement)this).BeginAnimation(HorizontalScrollOffsetProperty, (AnimationTimeline)null);
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
		((UIElement)this).BeginAnimation(HorizontalScrollOffsetProperty, (AnimationTimeline)_animation);
	}

	private int GetItemsPerPage()
	{
		int num = 0;
		if (ItemsPerPage == 0)
		{
			if (((ItemsControl)this).Items.Count > 0 && _scrollViewer != null && ItemWidth > 0.0)
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
		if (((ItemsControl)this).Items.Count > 0)
		{
			int num = ((ItemsControl)this).Items.IndexOf(pagedListBoxItem);
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
		if (((ItemsControl)this).Items.Count > 0)
		{
			int itemsPerPage = GetItemsPerPage();
			int num = ((ItemsControl)this).Items.IndexOf(pagedListBoxItem);
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
		((UIElement)this).CommandBindings.Add(commandBinding);
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
		((UIElement)this).CommandBindings.Add(commandBinding2);
		RemoveItem = new RoutedCommand();
		CommandBinding commandBinding3 = new CommandBinding(RemoveItem);
		commandBinding3.Executed += delegate(object s, ExecutedRoutedEventArgs e)
		{
			if (e.Parameter is IPagedListBoxItem removeItem)
			{
				((ItemsControl)this).Items.Remove(removeItem);
				if (_scrollViewer != null)
				{
					PadScrollViewerExtentWidth();
					UpdatePagePlaceholderCollection();
					UpdatePageIndicatorInformation();
				}
			}
		};
		((UIElement)this).CommandBindings.Add(commandBinding3);
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
		((UIElement)this).CommandBindings.Add(commandBinding4);
	}

	private void ItemContainerGeneratorItemsChanged(object sender, ItemsChangedEventArgs e)
	{
		if (e.Action == NotifyCollectionChangedAction.Reset && IsScrollToHomeOnResetEnabled)
		{
			AnimateScrollContent(_scrollViewer.HorizontalOffset, 0.0);
			PadScrollViewerExtentWidth();
			UpdatePageIndicatorInformation();
		}
		IsResultSetEmptyVisibility = ((((ItemsControl)this).Items.Count != 0) ? Visibility.Collapsed : Visibility.Visible);
	}

	private static void OnScrollToItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (e.OldValue != e.NewValue)
		{
			GlobalErrorHandler.WriteMethodInfo("OnScrollToItemChanged");
			((PagedListBox)(object)d).ScrollItemIntoView(e.NewValue as IPagedListBoxItem);
		}
	}

	private void PadScrollViewerExtentWidth()
	{
		int itemsPerPage = GetItemsPerPage();
		if (itemsPerPage > 0)
		{
			int num = ((ItemsControl)this).Items.Count % itemsPerPage;
			if (_scrollViewer != null)
			{
				double right = ((num <= 0) ? (_scrollViewer.ViewportWidth - (double)itemsPerPage * ItemWidth) : (_scrollViewer.ViewportWidth - (double)num * ItemWidth));
				DependencyObject dependencyObject = ((ItemsControl)this).ItemContainerGenerator.ContainerFromIndex((((ItemsControl)this).Items.Count > 0) ? (((ItemsControl)this).Items.Count - 1) : 0);
				SurfaceListBoxItem val = (SurfaceListBoxItem)(object)((dependencyObject is SurfaceListBoxItem) ? dependencyObject : null);
				((DependencyObject)(object)val)?.SetValue(FrameworkElement.MarginProperty, (object)new Thickness(((FrameworkElement)(object)val).Margin.Left, ((FrameworkElement)(object)val).Margin.Top, right, ((FrameworkElement)(object)val).Margin.Bottom));
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
		_scrollViewer = ((Control)this).Template.FindName("pagingScrollViewer", (FrameworkElement)(object)this) as ScrollViewer;
		UpdatePagePlaceholderCollection();
		if (_scrollViewer != null)
		{
			_scrollViewer.SizeChanged += ScrollViewerSizeChanged;
			_scrollViewer.ScrollChanged += ScrollViewerScrollChanged;
			((ItemsControl)this).ItemContainerGenerator.ItemsChanged += ItemContainerGeneratorItemsChanged;
		}
		_listBoxPageIndicator = ((Control)this).Template.FindName("listBoxPageIndicator", (FrameworkElement)(object)this) as ListBox;
		if (_listBoxPageIndicator != null)
		{
			_listBoxPageIndicator.Visibility = PagingIndicatorVisibility;
			if (PagingIndicatorVisibility == Visibility.Collapsed && ((Control)this).Template.FindName("gridItemPresenter", (FrameworkElement)(object)this) is Grid element)
			{
				Grid.SetRowSpan(element, 2);
			}
			_listBoxPageIndicator.SelectionChanged += delegate(object s, SelectionChangedEventArgs e)
			{
				e.Handled = true;
			};
			_listBoxPageIndicator.ItemsSource = Pages;
		}
		ref SurfaceRepeatButton pageLeftButton = ref _pageLeftButton;
		object obj = ((Control)this).Template.FindName("pageLeftButton", (FrameworkElement)(object)this);
		pageLeftButton = (SurfaceRepeatButton)((obj is SurfaceRepeatButton) ? obj : null);
		ref SurfaceRepeatButton pageRightButton = ref _pageRightButton;
		object obj2 = ((Control)this).Template.FindName("pageRightButton", (FrameworkElement)(object)this);
		pageRightButton = (SurfaceRepeatButton)((obj2 is SurfaceRepeatButton) ? obj2 : null);
		UpdatePageIndicatorInformation();
		PadScrollViewerExtentWidth();
		if (_scrollViewer != null)
		{
			_scrollViewer.ScrollToHorizontalOffset(GetOffsetForPage(GetPageForItem(((Selector)this).SelectedItem as IPagedListBoxItem)));
		}
	}

	private void PagedListBoxPreviewMouseUp(object sender, MouseButtonEventArgs e)
	{
		foreach (object item in (IEnumerable)((ItemsControl)this).Items)
		{
			DependencyObject dependencyObject = ((ItemsControl)this).ItemContainerGenerator.ContainerFromItem(item);
			SurfaceListBoxItem val = (SurfaceListBoxItem)(object)((dependencyObject is SurfaceListBoxItem) ? dependencyObject : null);
			if (val != null && ((UIElement)(object)val).IsMouseDirectlyOver)
			{
				((UIElement)this).RaiseEvent(new RoutedEventArgs
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
				_scrollViewer.ScrollToHorizontalOffset(GetOffsetForPage(GetPageForItem(((Selector)this).SelectedItem as IPagedListBoxItem)));
			}
		}
	}

	private void ResetVisuals()
	{
		UpdatePagePlaceholderCollection();
		_currentPageIndex = ((((Selector)this).SelectedItem != null) ? GetPageForItem(((Selector)this).SelectedItem as IPagedListBoxItem) : 0);
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
		if (ItemsPerPage > 0 && !(((FrameworkElement)this).Width > 0.0) && ItemWidth > 0.0)
		{
			((FrameworkElement)this).Width = ItemWidth * (double)ItemsPerPage + 100.0;
		}
	}

	private void UpdateFirstVisibleItem()
	{
		if (!_isAnimating && _scrollViewer != null && ItemWidth != 0.0)
		{
			int num = (int)Math.Ceiling(_scrollViewer.HorizontalOffset / ItemWidth);
			if (((ItemsControl)this).Items != null && ((ItemsControl)this).Items.Count > 0 && ((ItemsControl)this).Items.Count > num)
			{
				object itemAt = ((ItemsControl)this).Items.GetItemAt(num);
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
				((UIElement)(object)_pageLeftButton).IsEnabled = false;
				((UIElement)(object)_pageRightButton).IsEnabled = false;
			}
			else if (_scrollViewer.HorizontalOffset > ItemWidth)
			{
				((UIElement)(object)_pageLeftButton).IsEnabled = true;
				((UIElement)(object)_pageRightButton).IsEnabled = true;
			}
			else
			{
				((UIElement)(object)_pageLeftButton).IsEnabled = false;
				((UIElement)(object)_pageRightButton).IsEnabled = true;
			}
		}
		else if (_currentPageIndex == Pages.Count - 1)
		{
			((UIElement)(object)_pageLeftButton).IsEnabled = true;
			((UIElement)(object)_pageRightButton).IsEnabled = false;
		}
		else
		{
			((UIElement)(object)_pageLeftButton).IsEnabled = true;
			((UIElement)(object)_pageRightButton).IsEnabled = true;
		}
	}

	private void UpdatePagePlaceholderCollection()
	{
		Pages.Clear();
		if (((ItemsControl)this).Items.Count <= 0)
		{
			return;
		}
		int itemsPerPage = GetItemsPerPage();
		if (itemsPerPage > 0)
		{
			int num = ((ItemsControl)this).Items.Count / itemsPerPage;
			if (((ItemsControl)this).Items.Count % GetItemsPerPage() == 0)
			{
				num--;
			}
			for (int i = 0; i <= num; i++)
			{
				Pages.Add(new object());
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/usercontrols/pagedlistbox/pagedlistbox.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		_contentLoaded = true;
	}
}
