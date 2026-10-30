using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.View;

public class ThemePickerView : UserControl, IComponentConnector
{
	internal ThemePickerView UserControl;

	internal Button BtnResetHighlightColor;

	internal Popup PopupViewPortBackground1;

	internal TextBlock TextBlock1;

	internal Button BtnResetViewportColor;

	internal Popup PopupViewPortBackground;

	internal Button BtnResetViewportGraphic;

	internal TextBlock TxtViewportGraphicPath;

	private bool _contentLoaded;

	public ThemePickerView()
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
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/themepickerview.xaml", UriKind.Relative);
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
			UserControl = (ThemePickerView)target;
			break;
		case 2:
			BtnResetHighlightColor = (Button)target;
			break;
		case 3:
			PopupViewPortBackground1 = (Popup)target;
			break;
		case 4:
			TextBlock1 = (TextBlock)target;
			break;
		case 5:
			BtnResetViewportColor = (Button)target;
			break;
		case 6:
			PopupViewPortBackground = (Popup)target;
			break;
		case 7:
			BtnResetViewportGraphic = (Button)target;
			break;
		case 8:
			TxtViewportGraphicPath = (TextBlock)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
