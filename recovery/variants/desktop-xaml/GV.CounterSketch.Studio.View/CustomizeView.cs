using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.View;

public partial class CustomizeView : UserControl, IComponentConnector
{
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
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
