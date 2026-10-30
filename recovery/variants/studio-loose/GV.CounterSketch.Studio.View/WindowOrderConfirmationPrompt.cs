using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public class WindowOrderConfirmationPrompt : Window, IComponentConnector
{
	public bool ProceedWithOrder;

	internal Grid Root;

	internal StackPanel RealPricingEligibilityResultsPanel;

	internal DockPanel DockExtendedSliderRanges;

	internal TextBlock Expanded_Slider_Ranges;

	internal TextBlock Notes_On_Order;

	internal DockPanel DockFreehand;

	internal TextBlock Freehand_Parts_Used;

	internal DockPanel DockUserSuppliedStones;

	internal TextBlock User_Supplied_Stones;

	internal DockPanel DockUserManualSizeStones;

	internal TextBlock Manual_Sized_Stones;

	internal Viewbox QuoteRequired;

	internal Viewbox FinalPrice;

	internal Grid Pricing_Details;

	internal TextBlock LargeStonesCost;

	internal TextBlock SettingCost;

	internal TextBlock CaratWeightTotal;

	internal TextBlock TotalCost;

	internal Border Design_Details;

	internal Image CSS3_About_Screen_Images_Image_png;

	internal DockPanel BuyerInfo;

	internal StackPanel BuyerContactFields;

	internal Grid GridFirstname;

	internal Grid GridLastname;

	internal Grid GridEmail;

	internal TextBox txtMobilePhone;

	internal DockPanel ActionPanel;

	internal Viewbox Viewbox;

	internal TextBlock textBlock;

	internal TextBlock textBlock1;

	internal VisualStateGroup RealPricingEligibility;

	internal VisualState Default;

	internal VisualState TwentyFourHourQuoteRequired;

	internal VisualState RealPricingEligible;

	private bool _contentLoaded;

	public WindowOrderConfirmationPrompt()
	{
		InitializeComponent();
		base.IsVisibleChanged += WindowOrderConfirmationPrompt_IsVisibleChanged;
		base.Loaded += WindowOrderConfirmationPrompt_Loaded;
		Messenger.Default.Register<string>((object)this, (Action<string>)ProcessGenericMessage, false);
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

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/windoworderconfirmationprompt.xaml", UriKind.Relative);
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
			Root = (Grid)target;
			break;
		case 2:
			RealPricingEligibilityResultsPanel = (StackPanel)target;
			break;
		case 3:
			DockExtendedSliderRanges = (DockPanel)target;
			break;
		case 4:
			Expanded_Slider_Ranges = (TextBlock)target;
			break;
		case 5:
			Notes_On_Order = (TextBlock)target;
			break;
		case 6:
			DockFreehand = (DockPanel)target;
			break;
		case 7:
			Freehand_Parts_Used = (TextBlock)target;
			break;
		case 8:
			DockUserSuppliedStones = (DockPanel)target;
			break;
		case 9:
			User_Supplied_Stones = (TextBlock)target;
			break;
		case 10:
			DockUserManualSizeStones = (DockPanel)target;
			break;
		case 11:
			Manual_Sized_Stones = (TextBlock)target;
			break;
		case 12:
			QuoteRequired = (Viewbox)target;
			break;
		case 13:
			FinalPrice = (Viewbox)target;
			break;
		case 14:
			Pricing_Details = (Grid)target;
			break;
		case 15:
			LargeStonesCost = (TextBlock)target;
			break;
		case 16:
			SettingCost = (TextBlock)target;
			break;
		case 17:
			CaratWeightTotal = (TextBlock)target;
			break;
		case 18:
			TotalCost = (TextBlock)target;
			break;
		case 19:
			Design_Details = (Border)target;
			break;
		case 20:
			CSS3_About_Screen_Images_Image_png = (Image)target;
			break;
		case 21:
			BuyerInfo = (DockPanel)target;
			break;
		case 22:
			BuyerContactFields = (StackPanel)target;
			break;
		case 23:
			GridFirstname = (Grid)target;
			break;
		case 24:
			GridLastname = (Grid)target;
			break;
		case 25:
			GridEmail = (Grid)target;
			break;
		case 26:
			txtMobilePhone = (TextBox)target;
			break;
		case 27:
			ActionPanel = (DockPanel)target;
			break;
		case 28:
			Viewbox = (Viewbox)target;
			break;
		case 29:
			((Button)target).Click += Cancel_Click;
			break;
		case 30:
			((Button)target).Click += Order_Click;
			break;
		case 31:
			textBlock = (TextBlock)target;
			break;
		case 32:
			((Button)target).Click += Order_Click;
			break;
		case 33:
			textBlock1 = (TextBlock)target;
			break;
		case 34:
			RealPricingEligibility = (VisualStateGroup)target;
			break;
		case 35:
			Default = (VisualState)target;
			break;
		case 36:
			TwentyFourHourQuoteRequired = (VisualState)target;
			break;
		case 37:
			RealPricingEligible = (VisualState)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
