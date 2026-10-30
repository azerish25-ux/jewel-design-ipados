using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.Controls;

public class StoneTray : UserControl, IComponentConnector
{
	public static readonly DependencyProperty GemProperty = DependencyProperty.Register("Gem", typeof(Gem), typeof(StoneTray));

	public static readonly DependencyProperty MaxCaratProperty = DependencyProperty.Register("MaxCarat", typeof(double?), typeof(StoneTray));

	public static readonly DependencyProperty MinCaratProperty = DependencyProperty.Register("MinCarat", typeof(double?), typeof(StoneTray));

	public static readonly DependencyProperty ShowStoneTextProperty = DependencyProperty.Register("ShowStoneText", typeof(bool), typeof(StoneTray), new PropertyMetadata(false));

	public static readonly DependencyProperty ShowStoneTrayGraphicProperty = DependencyProperty.Register("ShowStoneTrayGraphic", typeof(bool), typeof(StoneTray), new PropertyMetadata(false));

	public static readonly DependencyProperty StoneShapeProperty = DependencyProperty.Register("StoneShape", typeof(string), typeof(StoneTray));

	internal StoneTray userControl;

	internal ToggleButton _stoneTrayToggle;

	internal Popup _stoneTrayPopup;

	private bool _contentLoaded;

	public Gem Gem
	{
		get
		{
			return (Gem)GetValue(GemProperty);
		}
		set
		{
			SetValue(GemProperty, value);
		}
	}

	public double? MaxCarat
	{
		get
		{
			return (double?)GetValue(MaxCaratProperty);
		}
		set
		{
			SetValue(MaxCaratProperty, value);
		}
	}

	public double? MinCarat
	{
		get
		{
			return (double?)GetValue(MinCaratProperty);
		}
		set
		{
			SetValue(MinCaratProperty, value);
		}
	}

	public bool ShowStoneText
	{
		get
		{
			return (bool)GetValue(ShowStoneTextProperty);
		}
		set
		{
			SetValue(ShowStoneTextProperty, value);
		}
	}

	public bool ShowStoneTrayGraphic
	{
		get
		{
			return (bool)GetValue(ShowStoneTrayGraphicProperty);
		}
		set
		{
			SetValue(ShowStoneTrayGraphicProperty, value);
		}
	}

	public string StoneShape
	{
		get
		{
			return (string)GetValue(StoneShapeProperty);
		}
		set
		{
			SetValue(StoneShapeProperty, value);
		}
	}

	public StoneTray()
	{
		InitializeComponent();
		Messenger.Default.Register<string>((object)this, (Action<string>)ProcessMessage, false);
	}

	private void _stoneTrayPopup_OnClosed(object sender, EventArgs e)
	{
		_stoneTrayToggle.IsChecked = false;
	}

	private void ProcessMessage(string message)
	{
		Application.Current.Dispatcher.Invoke(delegate
		{
			if (message != null && message == "CloseStoneTray")
			{
				_stoneTrayPopup.IsOpen = false;
				_stoneTrayToggle.IsChecked = false;
			}
		});
	}

	private void ToggleButton_Click(object sender, RoutedEventArgs e)
	{
		if (sender is ToggleButton toggleButton)
		{
			ViewModelLocator.Current.StoneTrayVM.OpenStoneTray(Gem, MinCarat, MaxCarat, StoneShape);
			if (toggleButton.IsChecked == true)
			{
				_stoneTrayPopup.IsOpen = true;
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/stonetray.xaml", UriKind.Relative);
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
			userControl = (StoneTray)target;
			break;
		case 2:
			_stoneTrayToggle = (ToggleButton)target;
			_stoneTrayToggle.Click += ToggleButton_Click;
			break;
		case 3:
			_stoneTrayPopup = (Popup)target;
			_stoneTrayPopup.Closed += _stoneTrayPopup_OnClosed;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
