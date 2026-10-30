using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Controls;

public partial class WindowModalColorPicker : Window, IComponentConnector
{
	public Color SelectedColor => csColorPicker.SelectedColor;

	public ImageSource CustomLogoImageSource
	{
		get
		{
			return csColorPicker.CustomLogoImageSource;
		}
		set
		{
			csColorPicker.CustomLogoImageSource = value;
		}
	}

	public WindowModalColorPicker()
	{
		InitializeComponent();
	}

	private void btnApplyChanges_Click(object sender, RoutedEventArgs e)
	{
		base.DialogResult = true;
	}

	private void btnCancel_Click(object sender, RoutedEventArgs e)
	{
		base.DialogResult = false;
		Close();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
