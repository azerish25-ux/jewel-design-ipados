using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Telerik.Windows.Controls;

namespace GV.CounterSketch.Studio.View;

public partial class TileSearchResultListView : UserControl, IComponentConnector
{
	public TileSearchResultListView()
	{
		InitializeComponent();
		Messenger.Default.Register<string>(this, ProcessMessage);
	}

	private static Visual GetDescendantByType(Visual element, Type type)
	{
		if (element == null)
		{
			return null;
		}
		if (element.GetType() == type)
		{
			return element;
		}
		Visual visual = null;
		if (element is FrameworkElement)
		{
			(element as FrameworkElement).ApplyTemplate();
		}
		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
		{
			visual = GetDescendantByType(VisualTreeHelper.GetChild(element, i) as Visual, type);
			if (visual != null)
			{
				break;
			}
		}
		return visual;
	}

	private void ProcessMessage(string obj)
	{
		if (obj?.ToLower() == "resetscroll" && GetDescendantByType(SearchResultsList, typeof(ScrollViewer)) is ScrollViewer scrollViewer)
		{
			scrollViewer.ScrollToTop();
			scrollViewer.ScrollToHome();
		}
	}

	private void RadSlider_OnDragCompleted(object sender, RadDragCompletedEventArgs e)
	{
		if (ViewModelLocator.Current.TileSearchMainVM != null)
		{
			if (base.DataContext is TileSearchResultDesignsViewModel)
			{
				ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.UpdateSearchResultImageSizes();
			}
			else
			{
				ViewModelLocator.Current.TileSearchMainVM.TileSearchResultPartsVM.UpdateSearchResultImageSizes();
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
