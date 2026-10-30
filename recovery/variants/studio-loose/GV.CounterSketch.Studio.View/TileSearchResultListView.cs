using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Telerik.Windows.Controls;

namespace GV.CounterSketch.Studio.View;

public class TileSearchResultListView : UserControl, IComponentConnector
{
	internal RowDefinition SessionPicksRow;

	internal CheckBox CheckBoxNewFirst;

	internal RadSlider RadSlider;

	internal Button BtnExportImages;

	internal FilterResultsView FilterResultsView;

	internal Border VirtualWrapPanelSizer;

	internal ListBox SearchResultsList;

	internal StackPanel UpdateStatuses;

	internal Canvas add__to__session__picks;

	internal Canvas Layer_1;

	internal Path Path;

	internal Path Path_0;

	internal ScrollViewer SessionPicksScrollViewer;

	private bool _contentLoaded;

	public TileSearchResultListView()
	{
		InitializeComponent();
		Messenger.Default.Register<string>((object)this, (Action<string>)ProcessMessage, false);
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
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/tilesearchresultlistview.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		switch (connectionId)
		{
		case 1:
			SessionPicksRow = (RowDefinition)target;
			break;
		case 2:
			CheckBoxNewFirst = (CheckBox)target;
			break;
		case 3:
			RadSlider = (RadSlider)target;
			RadSlider.DragCompleted += RadSlider_OnDragCompleted;
			break;
		case 4:
			BtnExportImages = (Button)target;
			break;
		case 5:
			FilterResultsView = (FilterResultsView)target;
			break;
		case 6:
			VirtualWrapPanelSizer = (Border)target;
			break;
		case 7:
			SearchResultsList = (ListBox)target;
			break;
		case 8:
			UpdateStatuses = (StackPanel)target;
			break;
		case 9:
			add__to__session__picks = (Canvas)target;
			break;
		case 10:
			Layer_1 = (Canvas)target;
			break;
		case 11:
			Path = (Path)target;
			break;
		case 12:
			Path_0 = (Path)target;
			break;
		case 13:
			SessionPicksScrollViewer = (ScrollViewer)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
