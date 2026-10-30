using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public partial class ItemChooserView : ListBox, IComponentConnector
{
	public static readonly DependencyProperty IsFilterPickerProperty = DependencyProperty.Register("IsFilterPicker", typeof(bool), typeof(ItemChooserView), new PropertyMetadata(false));

	public static readonly DependencyProperty IsGemPickerProperty = DependencyProperty.Register("IsGemPicker", typeof(bool), typeof(ItemChooserView), new PropertyMetadata(false));

	public static readonly DependencyProperty IsMetalPickerProperty = DependencyProperty.Register("IsMetalPicker", typeof(bool), typeof(ItemChooserView), new PropertyMetadata(false));

	public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(ItemChooserView), new PropertyMetadata(OnIsOpenChanged));

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
		Messenger.Default.Register<string>(this, ProcessMessage);
	}

	private void ProcessMessage(string message)
	{
		if (message != null && message == "Close")
		{
			IsOpen = false;
		}
	}
}
