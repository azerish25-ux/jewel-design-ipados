using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.View;

public partial class MaterialOptionSelectorView : UserControl, IComponentConnector
{
	public MaterialOptionSelectorView()
	{
		InitializeComponent();
	}

	private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
	{
		Window window = Window.GetWindow(this);
		if (window != null && !ComponentDispatcher.IsThreadModal)
		{
			window.Close();
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
