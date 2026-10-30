using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GV.CounterSketch.Studio.Utilities;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.View;

public class CustomizeView : UserControl, IComponentConnector
{
	internal Grid StackGrid;

	internal DesignStackView DesignStackView;

	internal FreehandStackView FreehandStackView;

	internal StackPanel Components;

	internal StackPanel DesignActionButtons;

	internal ToggleButton BtnToggleViewportControls;

	internal ToggleButton BtnEnableFreehandCommonCenterMode;

	internal ToggleButton BtnShowGrid;

	internal Button BtnCameraTop;

	internal Viewbox CsButtonViewTopDown_Default_Copy;

	internal Button BtnCameraFront;

	internal Viewbox CsButtonViewThroughFinger_Default_Copy;

	internal Button BtnCameraSide;

	internal Viewbox CsButtonViewSide_Default;

	internal Button BtnCameraPerspective;

	internal Viewbox CsButtonViewPerspective_Default_Copy;

	internal Button BtnDesignTryOn;

	internal Canvas Toolbar;

	internal Button BtnMeasure;

	internal Canvas Layer_2;

	internal Button BtnDesignReport;

	internal Path CsGraphicManageEditDesign;

	internal Path CsGraphicManageEditDesign_3;

	internal Path CsGraphicManageEditDesign_4;

	internal Path CsGraphicManageEditDesign_5;

	internal Path CsGraphicManageEditDesign_9;

	internal Button BtnRender;

	internal Button BtnRenderSettings;

	internal Button BtnQuickEstimate;

	internal OrderSummaryView OrderSummaryViewControl;

	internal Grid RhinoRootGrid;

	internal Grid GridPartsListView;

	private bool _contentLoaded;

	public CustomizeView()
	{
		InitializeComponent();
		RhinoRootGrid.LayoutUpdated += RhinoRootGrid_LayoutUpdated;
	}

	[DebuggerStepThrough]
	private void RhinoRootGrid_LayoutUpdated(object sender, EventArgs e)
	{
		RhinoWindowHelper.RepositionRhino(RhinoRootGrid);
	}

	private void ShowOrderTimeline_OnCompleted(object sender, EventArgs e)
	{
		Task.Run(delegate
		{
			ViewModelLocator.Current.OrderSummaryVM.Populate();
		});
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/customizeview.xaml", UriKind.Relative);
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
		switch (connectionId)
		{
		case 1:
			((Storyboard)target).Completed += ShowOrderTimeline_OnCompleted;
			break;
		case 2:
			StackGrid = (Grid)target;
			break;
		case 3:
			DesignStackView = (DesignStackView)target;
			break;
		case 4:
			FreehandStackView = (FreehandStackView)target;
			break;
		case 5:
			Components = (StackPanel)target;
			break;
		case 6:
			DesignActionButtons = (StackPanel)target;
			break;
		case 7:
			BtnToggleViewportControls = (ToggleButton)target;
			break;
		case 8:
			BtnEnableFreehandCommonCenterMode = (ToggleButton)target;
			break;
		case 9:
			BtnShowGrid = (ToggleButton)target;
			break;
		case 10:
			BtnCameraTop = (Button)target;
			break;
		case 11:
			CsButtonViewTopDown_Default_Copy = (Viewbox)target;
			break;
		case 12:
			BtnCameraFront = (Button)target;
			break;
		case 13:
			CsButtonViewThroughFinger_Default_Copy = (Viewbox)target;
			break;
		case 14:
			BtnCameraSide = (Button)target;
			break;
		case 15:
			CsButtonViewSide_Default = (Viewbox)target;
			break;
		case 16:
			BtnCameraPerspective = (Button)target;
			break;
		case 17:
			CsButtonViewPerspective_Default_Copy = (Viewbox)target;
			break;
		case 18:
			BtnDesignTryOn = (Button)target;
			break;
		case 19:
			Toolbar = (Canvas)target;
			break;
		case 20:
			BtnMeasure = (Button)target;
			break;
		case 21:
			Layer_2 = (Canvas)target;
			break;
		case 22:
			BtnDesignReport = (Button)target;
			break;
		case 23:
			CsGraphicManageEditDesign = (Path)target;
			break;
		case 24:
			CsGraphicManageEditDesign_3 = (Path)target;
			break;
		case 25:
			CsGraphicManageEditDesign_4 = (Path)target;
			break;
		case 26:
			CsGraphicManageEditDesign_5 = (Path)target;
			break;
		case 27:
			CsGraphicManageEditDesign_9 = (Path)target;
			break;
		case 28:
			BtnRender = (Button)target;
			break;
		case 29:
			BtnRenderSettings = (Button)target;
			break;
		case 30:
			BtnQuickEstimate = (Button)target;
			break;
		case 31:
			OrderSummaryViewControl = (OrderSummaryView)target;
			break;
		case 32:
			RhinoRootGrid = (Grid)target;
			break;
		case 33:
			GridPartsListView = (Grid)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
