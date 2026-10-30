using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public class ItemChooserView : ListBox, IComponentConnector
{
	public static readonly DependencyProperty IsFilterPickerProperty = DependencyProperty.Register("IsFilterPicker", typeof(bool), typeof(ItemChooserView), new PropertyMetadata(false));

	public static readonly DependencyProperty IsGemPickerProperty = DependencyProperty.Register("IsGemPicker", typeof(bool), typeof(ItemChooserView), new PropertyMetadata(false));

	public static readonly DependencyProperty IsMetalPickerProperty = DependencyProperty.Register("IsMetalPicker", typeof(bool), typeof(ItemChooserView), new PropertyMetadata(false));

	public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(ItemChooserView), new PropertyMetadata(OnIsOpenChanged));

	internal ItemChooserView ItemChooserViewControl;

	private bool _contentLoaded;

	public bool IsFilterPicker
	{
		get
		{
			return (bool)GetValue(IsFilterPickerProperty);
		}
		set
		{
			SetValue(IsFilterPickerProperty, value);
		}
	}

	public bool IsGemPicker
	{
		get
		{
			return (bool)GetValue(IsGemPickerProperty);
		}
		set
		{
			SetValue(IsGemPickerProperty, value);
		}
	}

	public bool IsMetalPicker
	{
		get
		{
			return (bool)GetValue(IsMetalPickerProperty);
		}
		set
		{
			SetValue(IsMetalPickerProperty, value);
		}
	}

	public bool IsOpen
	{
		get
		{
			return (bool)GetValue(IsOpenProperty);
		}
		set
		{
			SetValue(IsOpenProperty, value);
		}
	}

	private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
	}

	public ItemChooserView()
	{
		InitializeComponent();
		Messenger.Default.Register<string>((object)this, (Action<string>)ProcessMessage, false);
	}

	private void ProcessMessage(string message)
	{
		if (message != null && message == "Close")
		{
			IsOpen = false;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/itemchooserview.xaml", UriKind.Relative);
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
			ItemChooserViewControl = (ItemChooserView)target;
		}
		else
		{
			_contentLoaded = true;
		}
	}
}
