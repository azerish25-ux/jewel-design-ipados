using System.Windows;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public partial class WindowOrderConfirmationPrompt : Window, IComponentConnector
{
	public bool ProceedWithOrder;

	public WindowOrderConfirmationPrompt()
	{
		InitializeComponent();
		base.IsVisibleChanged += WindowOrderConfirmationPrompt_IsVisibleChanged;
		base.Loaded += WindowOrderConfirmationPrompt_Loaded;
		Messenger.Default.Register<string>(this, ProcessGenericMessage);
	}

	private void Cancel_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void CapturePricingImage()
	{
		byte[] pricingImageBytes = WpfHelper.SaveElementToPngByteArray(Root);
		ViewModelLocator.Current.OrderSummaryVM.PricingImageBytes = pricingImageBytes;
	}

	private void Order_Click(object sender, RoutedEventArgs e)
	{
		ProceedWithOrder = true;
		Close();
	}

	private void ProcessGenericMessage(string message)
	{
		if (message != null)
		{
			string text = message.ToLower();
			if (text != null && text == "capturepricingimage")
			{
				CapturePricingImage();
			}
		}
	}

	private void WindowOrderConfirmationPrompt_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (base.Visibility != Visibility.Visible)
		{
			Close();
		}
		else if (ViewModelLocator.Current.OrderSummaryVM.Requires24HourQuote)
		{
			VisualStateManager.GoToElementState(Root, "TwentyFourHourQuoteRequired", useTransitions: true);
		}
	}

	private void WindowOrderConfirmationPrompt_Loaded(object sender, RoutedEventArgs e)
	{
		if (!UIHelper.CounterSketchOptions.IsLargeStoneSettingPriceToggled)
		{
			LargeStonesCost.Visibility = Visibility.Collapsed;
			SettingCost.Visibility = Visibility.Collapsed;
		}
		else
		{
			LargeStonesCost.Visibility = Visibility.Visible;
			SettingCost.Visibility = Visibility.Visible;
		}
		if (UIHelper.KeyProfile.BuyerEMailHiddenOnPrompt)
		{
			GridEmail.Visibility = Visibility.Collapsed;
		}
		if (UIHelper.KeyProfile.ForceHideFreehand)
		{
			DockFreehand.Visibility = Visibility.Collapsed;
		}
	}
}
