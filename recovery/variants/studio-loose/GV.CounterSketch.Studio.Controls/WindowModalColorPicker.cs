using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Controls;

public class WindowModalColorPicker : Window, IComponentConnector
{
	internal WindowModalColorPicker theWindow;

	internal StackPanel stackPanel;

	internal CSColorPicker csColorPicker;

	internal StackPanel panelOptions;

	internal DockPanel ActionButtons;

	internal Button btnCancel;

	internal Button btnApplyChanges;

	private bool _contentLoaded;

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
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/windowmodalcolorpicker.xaml", UriKind.Relative);
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
			theWindow = (WindowModalColorPicker)target;
			break;
		case 2:
			stackPanel = (StackPanel)target;
			break;
		case 3:
			csColorPicker = (CSColorPicker)target;
			break;
		case 4:
			panelOptions = (StackPanel)target;
			break;
		case 5:
			ActionButtons = (DockPanel)target;
			break;
		case 6:
			btnCancel = (Button)target;
			btnCancel.Click += btnCancel_Click;
			break;
		case 7:
			btnApplyChanges = (Button)target;
			btnApplyChanges.Click += btnApplyChanges_Click;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
