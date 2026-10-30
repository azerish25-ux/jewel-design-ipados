using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.View;

public partial class WindowAnyGemAnyColorView : Window, IComponentConnector
{
	public WindowAnyGemAnyColorView(MaterialOptionSelectorViewModel materialOptionSelectorVM)
	{
		if (materialOptionSelectorVM != null)
		{
			base.DataContext = materialOptionSelectorVM;
		}
		InitializeComponent();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
