using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.View;

public class WindowNewLibrary : Window, IComponentConnector
{
	private bool _contentLoaded;

	public WindowNewLibrary()
	{
		InitializeComponent();
	}

	private void WindowNewLibrary_OnClosing(object sender, CancelEventArgs e)
	{
		ViewModelLocator.Current.UpdateInfoVM.StopNewImageTimer();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/windownewlibrary.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		if (connectionId == 1)
		{
			((WindowNewLibrary)target).Closing += WindowNewLibrary_OnClosing;
		}
		else
		{
			_contentLoaded = true;
		}
	}
}
