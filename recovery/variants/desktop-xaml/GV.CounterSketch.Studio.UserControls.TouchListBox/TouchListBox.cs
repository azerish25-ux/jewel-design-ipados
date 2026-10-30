using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using Microsoft.Surface.Presentation.Controls;

namespace GV.CounterSketch.Studio.UserControls.TouchListBox;

public partial class TouchListBox : SurfaceListBox, IComponentConnector
{
	public static readonly DependencyProperty HorizontalScrollOffsetProperty = DependencyProperty.Register("HorizontalScrollOffset", typeof(double), typeof(TouchListBox), new UIPropertyMetadata(0.0, OnHorizontalScrollOffsetChanged));

	public static readonly DependencyProperty IsResultSetEmptyProperty = DependencyProperty.Register("IsResultSetEmptyVisibility", typeof(Visibility), typeof(TouchListBox), new UIPropertyMetadata(Visibility.Collapsed));

	public static readonly DependencyProperty PagingIndicatorVisibilityProperty = DependencyProperty.Register("PagingIndicatorVisibility", typeof(Visibility), typeof(TouchListBox), new UIPropertyMetadata(Visibility.Visible));

	public static readonly RoutedEvent ItemClickedEvent = EventManager.RegisterRoutedEvent("ItemClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TouchListBox));

	private ScrollViewer _scrollViewer;

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

	public ICommand MoveCollectionLeft { get; private set; }

	public ICommand MoveCollectionRight { get; private set; }

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

	private static void OnHorizontalScrollOffsetChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
	{
		if (dependencyObject is TouchListBox touchListBox && touchListBox.Template.FindName("pagingScrollViewer", touchListBox) is ScrollViewer scrollViewer)
		{
			scrollViewer.ScrollToHorizontalOffset(touchListBox.HorizontalScrollOffset);
		}
	}

	public TouchListBox()
	{
		InitializeComponent();
		if (!DesignerProperties.GetIsInDesignMode(this))
		{
			InitializeCommands();
			base.Loaded += TouchListBoxLoaded;
			base.PreviewMouseUp += TouchListBoxPreviewMouseUp;
		}
	}

	private void AnimateScrollContent(double currentOffset, double scrollToOffset)
	{
		DoubleAnimation animation = new DoubleAnimation
		{
			EasingFunction = new ExponentialEase
			{
				EasingMode = EasingMode.EaseInOut,
				Exponent = 10.0
			},
			From = currentOffset,
			To = scrollToOffset,
			Duration = TimeSpan.FromSeconds(0.1)
		};
		BeginAnimation(HorizontalScrollOffsetProperty, animation);
	}

	private void InitializeCommands()
	{
		MoveCollectionLeft = new RoutedCommand();
		CommandBinding commandBinding = new CommandBinding(MoveCollectionLeft);
		commandBinding.Executed += delegate(object s, ExecutedRoutedEventArgs e)
		{
			if (e.Parameter is ScrollViewer scrollViewer)
			{
				AnimateScrollContent(scrollViewer.HorizontalOffset, scrollViewer.HorizontalOffset - 1.0);
			}
		};
		base.CommandBindings.Add(commandBinding);
		MoveCollectionRight = new RoutedCommand();
		CommandBinding commandBinding2 = new CommandBinding(MoveCollectionRight);
		commandBinding2.Executed += delegate(object s, ExecutedRoutedEventArgs e)
		{
			if (e.Parameter is ScrollViewer scrollViewer)
			{
				AnimateScrollContent(scrollViewer.HorizontalOffset, scrollViewer.HorizontalOffset + 1.0);
			}
		};
		base.CommandBindings.Add(commandBinding2);
	}

	private void ItemContainerGeneratorItemsChanged(object sender, ItemsChangedEventArgs e)
	{
		if (e.Action == NotifyCollectionChangedAction.Reset)
		{
			AnimateScrollContent(_scrollViewer.HorizontalOffset, 0.0);
		}
		IsResultSetEmptyVisibility = ((base.Items.Count != 0) ? Visibility.Collapsed : Visibility.Visible);
	}

	private void TouchListBoxLoaded(object sender, RoutedEventArgs routedEventArgs)
	{
		_scrollViewer = base.Template.FindName("pagingScrollViewer", this) as ScrollViewer;
		if (_scrollViewer != null)
		{
			base.ItemContainerGenerator.ItemsChanged += ItemContainerGeneratorItemsChanged;
		}
	}

	private void TouchListBoxPreviewMouseUp(object sender, MouseButtonEventArgs e)
	{
		foreach (object item in (IEnumerable)base.Items)
		{
			if (base.ItemContainerGenerator.ContainerFromItem(item) is ListBoxItem { IsMouseDirectlyOver: not false })
			{
				RaiseEvent(new RoutedEventArgs
				{
					RoutedEvent = ItemClickedEvent,
					Source = this
				});
			}
		}
	}
}
