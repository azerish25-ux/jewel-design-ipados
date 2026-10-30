using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DevExpress.Xpf.Editors;

namespace GV.CounterSketch.Studio.View;

public class ColorPickerView : UserControl, IComponentConnector
{
	internal ColorPickerView userControl;

	internal ColorEdit colorEdit1;

	private bool _contentLoaded;

	public ColorPickerView()
	{
		InitializeComponent();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/colorpickerview.xaml", UriKind.Relative);
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
			userControl = (ColorPickerView)target;
			break;
		case 2:
			colorEdit1 = (ColorEdit)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
