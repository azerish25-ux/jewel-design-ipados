using System;
using System.CodeDom.Compiler;
using System.Collections;
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

namespace GV.CounterSketch.Studio.UserControls.TouchListBox;

public class TouchListBox : SurfaceListBox, IComponentConnector
{
	public static readonly DependencyProperty HorizontalScrollOffsetProperty = DependencyProperty.Register("HorizontalScrollOffset", typeof(double), typeof(TouchListBox), new UIPropertyMetadata(0.0, OnHorizontalScrollOffsetChanged));

	public static readonly DependencyProperty IsResultSetEmptyProperty = DependencyProperty.Register("IsResultSetEmptyVisibility", typeof(Visibility), typeof(TouchListBox), new UIPropertyMetadata(Visibility.Collapsed));

	public static readonly DependencyProperty PagingIndicatorVisibilityProperty = DependencyProperty.Register("PagingIndicatorVisibility", typeof(Visibility), typeof(TouchListBox), new UIPropertyMetadata(Visibility.Visible));

	public static readonly RoutedEvent ItemClickedEvent = EventManager.RegisterRoutedEvent("ItemClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TouchListBox));

	private ScrollViewer _scrollViewer;

	private bool _contentLoaded;

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

	public ICommand MoveCollectionLeft { get; private set; }

	public ICommand MoveCollectionRight { get; private set; }

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

	private static void OnHorizontalScrollOffsetChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
	{
		if (dependencyObject is TouchListBox touchListBox && ((Control)(object)touchListBox).Template.FindName("pagingScrollViewer", (FrameworkElement)(object)touchListBox) is ScrollViewer scrollViewer)
		{
			scrollViewer.ScrollToHorizontalOffset(touchListBox.HorizontalScrollOffset);
		}
	}

	public TouchListBox()
	{
		InitializeComponent();
		if (!DesignerProperties.GetIsInDesignMode((DependencyObject)(object)this))
		{
			InitializeCommands();
			((FrameworkElement)this).Loaded += TouchListBoxLoaded;
			((UIElement)this).PreviewMouseUp += TouchListBoxPreviewMouseUp;
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
		((UIElement)this).BeginAnimation(HorizontalScrollOffsetProperty, (AnimationTimeline)animation);
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
		((UIElement)this).CommandBindings.Add(commandBinding);
		MoveCollectionRight = new RoutedCommand();
		CommandBinding commandBinding2 = new CommandBinding(MoveCollectionRight);
		commandBinding2.Executed += delegate(object s, ExecutedRoutedEventArgs e)
		{
			if (e.Parameter is ScrollViewer scrollViewer)
			{
				AnimateScrollContent(scrollViewer.HorizontalOffset, scrollViewer.HorizontalOffset + 1.0);
			}
		};
		((UIElement)this).CommandBindings.Add(commandBinding2);
	}

	private void ItemContainerGeneratorItemsChanged(object sender, ItemsChangedEventArgs e)
	{
		if (e.Action == NotifyCollectionChangedAction.Reset)
		{
			AnimateScrollContent(_scrollViewer.HorizontalOffset, 0.0);
		}
		IsResultSetEmptyVisibility = ((((ItemsControl)this).Items.Count != 0) ? Visibility.Collapsed : Visibility.Visible);
	}

	private void TouchListBoxLoaded(object sender, RoutedEventArgs routedEventArgs)
	{
		_scrollViewer = ((Control)this).Template.FindName("pagingScrollViewer", (FrameworkElement)(object)this) as ScrollViewer;
		if (_scrollViewer != null)
		{
			((ItemsControl)this).ItemContainerGenerator.ItemsChanged += ItemContainerGeneratorItemsChanged;
		}
	}

	private void TouchListBoxPreviewMouseUp(object sender, MouseButtonEventArgs e)
	{
		foreach (object item in (IEnumerable)((ItemsControl)this).Items)
		{
			if (((ItemsControl)this).ItemContainerGenerator.ContainerFromItem(item) is ListBoxItem { IsMouseDirectlyOver: not false })
			{
				((UIElement)this).RaiseEvent(new RoutedEventArgs
				{
					RoutedEvent = ItemClickedEvent,
					Source = this
				});
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
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/usercontrols/touchlistbox/touchlistbox.xaml", UriKind.Relative);
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
