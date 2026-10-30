using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.View;

public class WindowQuickEstimateView : Window, IComponentConnector
{
	internal Grid RootGrid;

	internal Border Header;

	internal Button SaveEdit;

	internal Button button;

	internal Viewbox CSButtonPrint_Default;

	internal CheckBox CheckBoxIncludeScreenshotWhilePrinting;

	internal Button ToggleEdit;

	internal Border MetalsList;

	internal DataGrid dataGrid;

	internal Border GemCounts;

	internal Grid GridGemSummary;

	internal TextBlock TextBlock2;

	internal TextBlock textBlock3;

	internal TextBlock TextBlock4;

	internal TextBlock TextBlock5;

	internal TextBlock TextBlock6;

	internal TextBlock TextBlock7;

	internal TextBlock TextBlock8;

	internal TextBlock TextBlock9;

	internal Border MissingGemsList;

	internal ScrollViewer ScrollViewer;

	internal StackPanel SpGhsi1;

	internal TextBlock TbGhsi1;

	internal StackPanel SpGhsi23;

	internal TextBlock TbGhsi23;

	internal StackPanel SpIji1;

	internal TextBlock TbIji1;

	internal Border MissingGemsActions;

	internal Button ButtonClose;

	internal Button ButtonView;

	internal Image ImageCapture;

	internal VisualStateGroup VisualStateGroup;

	internal VisualState Default;

	internal VisualState MissingGems;

	internal VisualState MissingGemsDisplayed;

	private bool _contentLoaded;

	public WindowQuickEstimateView()
	{
		InitializeComponent();
		base.Loaded += WindowQuickEstimateView_Loaded;
		if (SecurityHelper.IsStreaming)
		{
			button.Visibility = Visibility.Collapsed;
			CheckBoxIncludeScreenshotWhilePrinting.Visibility = Visibility.Collapsed;
		}
	}

	private void WindowQuickEstimateView_Loaded(object sender, RoutedEventArgs e)
	{
		if (ViewModelLocator.Current.QuickEstimateVM.IsMissingGems)
		{
			GlobalErrorHandler.WriteInfoToEventLog(VisualStateManager.GoToElementState(RootGrid, "MissingGems", useTransitions: false).ToString(), 2);
		}
		else
		{
			GlobalErrorHandler.WriteInfoToEventLog(VisualStateManager.GoToElementState(RootGrid, "Default", useTransitions: false).ToString(), 2);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/windowquickestimateview.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			RootGrid = (Grid)target;
			break;
		case 2:
			Header = (Border)target;
			break;
		case 3:
			SaveEdit = (Button)target;
			break;
		case 4:
			button = (Button)target;
			break;
		case 5:
			CSButtonPrint_Default = (Viewbox)target;
			break;
		case 6:
			CheckBoxIncludeScreenshotWhilePrinting = (CheckBox)target;
			break;
		case 7:
			ToggleEdit = (Button)target;
			break;
		case 8:
			MetalsList = (Border)target;
			break;
		case 9:
			dataGrid = (DataGrid)target;
			break;
		case 10:
			GemCounts = (Border)target;
			break;
		case 11:
			GridGemSummary = (Grid)target;
			break;
		case 12:
			TextBlock2 = (TextBlock)target;
			break;
		case 13:
			textBlock3 = (TextBlock)target;
			break;
		case 14:
			TextBlock4 = (TextBlock)target;
			break;
		case 15:
			TextBlock5 = (TextBlock)target;
			break;
		case 16:
			TextBlock6 = (TextBlock)target;
			break;
		case 17:
			TextBlock7 = (TextBlock)target;
			break;
		case 18:
			TextBlock8 = (TextBlock)target;
			break;
		case 19:
			TextBlock9 = (TextBlock)target;
			break;
		case 20:
			MissingGemsList = (Border)target;
			break;
		case 21:
			ScrollViewer = (ScrollViewer)target;
			break;
		case 22:
			SpGhsi1 = (StackPanel)target;
			break;
		case 23:
			TbGhsi1 = (TextBlock)target;
			break;
		case 24:
			SpGhsi23 = (StackPanel)target;
			break;
		case 25:
			TbGhsi23 = (TextBlock)target;
			break;
		case 26:
			SpIji1 = (StackPanel)target;
			break;
		case 27:
			TbIji1 = (TextBlock)target;
			break;
		case 28:
			MissingGemsActions = (Border)target;
			break;
		case 29:
			ButtonClose = (Button)target;
			break;
		case 30:
			ButtonView = (Button)target;
			break;
		case 31:
			ImageCapture = (Image)target;
			break;
		case 32:
			VisualStateGroup = (VisualStateGroup)target;
			break;
		case 33:
			Default = (VisualState)target;
			break;
		case 34:
			MissingGems = (VisualState)target;
			break;
		case 35:
			MissingGemsDisplayed = (VisualState)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
