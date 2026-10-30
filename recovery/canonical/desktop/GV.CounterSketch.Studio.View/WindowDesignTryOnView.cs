using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public class WindowDesignTryOnView : Window, IComponentConnector
{
	internal WindowDesignTryOnView TheWindow;

	internal Canvas CsHands;

	internal Path FingerL4;

	internal Path FingerL3;

	internal Path FingerL2;

	internal Path FingerL1;

	internal Path FingerL0;

	internal Path FingerR0;

	internal Path FingerR1;

	internal Path FingerR2;

	internal Path FingerR3;

	internal Path FingerR4;

	internal Path HandLeft;

	internal Path HandRight;

	internal Path HandPathRight;

	internal Path HandPathLeft;

	internal Path R4;

	internal Path R3;

	internal Path R2;

	internal Path R1;

	internal Path R0;

	internal Path L0;

	internal Path L1;

	internal Path L2;

	internal Path L3;

	internal Path L4;

	internal Path R4Selected;

	internal Path R3Selected;

	internal Path R2Selected;

	internal Path R1Selected;

	internal Path R0Selected;

	internal Path L0Selected;

	internal Path L1Selected;

	internal Path L2Selected;

	internal Path L3Selected;

	internal Path L4Selected;

	internal Slider Slider;

	private bool _contentLoaded;

	public WindowDesignTryOnView()
	{
		InitializeComponent();
		Messenger.Default.Register<string>(this, ProcessGenericMessage);
	}

	private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
	{
		Hide();
	}

	private void ProcessGenericMessage(string message)
	{
		if (message != null && message == "CloseWindow")
		{
			Hide();
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/windowdesigntryonview.xaml", UriKind.Relative);
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
			TheWindow = (WindowDesignTryOnView)target;
			break;
		case 2:
			CsHands = (Canvas)target;
			break;
		case 3:
			FingerL4 = (Path)target;
			break;
		case 4:
			FingerL3 = (Path)target;
			break;
		case 5:
			FingerL2 = (Path)target;
			break;
		case 6:
			FingerL1 = (Path)target;
			break;
		case 7:
			FingerL0 = (Path)target;
			break;
		case 8:
			FingerR0 = (Path)target;
			break;
		case 9:
			FingerR1 = (Path)target;
			break;
		case 10:
			FingerR2 = (Path)target;
			break;
		case 11:
			FingerR3 = (Path)target;
			break;
		case 12:
			FingerR4 = (Path)target;
			break;
		case 13:
			HandLeft = (Path)target;
			break;
		case 14:
			HandRight = (Path)target;
			break;
		case 15:
			HandPathRight = (Path)target;
			break;
		case 16:
			HandPathLeft = (Path)target;
			break;
		case 17:
			R4 = (Path)target;
			break;
		case 18:
			R3 = (Path)target;
			break;
		case 19:
			R2 = (Path)target;
			break;
		case 20:
			R1 = (Path)target;
			break;
		case 21:
			R0 = (Path)target;
			break;
		case 22:
			L0 = (Path)target;
			break;
		case 23:
			L1 = (Path)target;
			break;
		case 24:
			L2 = (Path)target;
			break;
		case 25:
			L3 = (Path)target;
			break;
		case 26:
			L4 = (Path)target;
			break;
		case 27:
			R4Selected = (Path)target;
			break;
		case 28:
			R3Selected = (Path)target;
			break;
		case 29:
			R2Selected = (Path)target;
			break;
		case 30:
			R1Selected = (Path)target;
			break;
		case 31:
			R0Selected = (Path)target;
			break;
		case 32:
			L0Selected = (Path)target;
			break;
		case 33:
			L1Selected = (Path)target;
			break;
		case 34:
			L2Selected = (Path)target;
			break;
		case 35:
			L3Selected = (Path)target;
			break;
		case 36:
			L4Selected = (Path)target;
			break;
		case 37:
			Slider = (Slider)target;
			break;
		case 38:
			((Button)target).Click += ButtonBase_OnClick;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
