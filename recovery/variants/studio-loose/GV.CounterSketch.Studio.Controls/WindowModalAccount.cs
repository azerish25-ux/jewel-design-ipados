using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using GV.CounterSketch.Common;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Localization;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.Controls;

public class WindowModalAccount : Window, IComponentConnector
{
	public static readonly DependencyProperty HasStullerShowcaseMarkupsAvailableProperty = DependencyProperty.Register("HasStullerShowcaseMarkupsAvailable", typeof(bool), typeof(WindowModalAccount), new PropertyMetadata(false));

	public static readonly DependencyProperty UseStullerShowcaseMarkupsProperty = DependencyProperty.Register("UseStullerShowcaseMarkups", typeof(bool), typeof(WindowModalAccount), new PropertyMetadata(false));

	private const string HiddenPassword = "************";

	private readonly CounterSketchOptions _resetOptionValues;

	internal WindowModalAccount _theWindow;

	internal StackPanel stackPanel;

	internal StackPanel panelOptions;

	internal StackPanel AccountSettings;

	internal Grid BillToShipTo;

	internal ComboBox cbxShipToAccountNumber;

	internal TextBlock txtBillToAccountNumber;

	internal TextBlock txtBillToInfo;

	internal TextBlock txtShipToInfo;

	internal StackPanel QuoteContactSettings;

	internal StackPanel EmailQuoteSettings;

	internal TextBox txtEmailQuote;

	internal TextBox txtPhone;

	internal CheckBox chkSendSMSNotifications;

	internal TextBlock txtDollars;

	internal TextBlock txtExchangedDollars;

	internal TextBlock txtCurrencyEnum;

	internal StackPanel ApplicationPermissions;

	internal CheckBox chkRequirePreferencesPassword;

	internal DockPanel ActionButtons;

	internal Button btnCancel;

	internal Button btnClose;

	internal TextBlock txtInfoMessage;

	private bool _contentLoaded;

	public bool HasStullerShowcaseMarkupsAvailable
	{
		get
		{
			return (bool)GetValue(HasStullerShowcaseMarkupsAvailableProperty);
		}
		set
		{
			SetValue(HasStullerShowcaseMarkupsAvailableProperty, value);
		}
	}

	public bool UseStullerShowcaseMarkups
	{
		get
		{
			return (bool)GetValue(UseStullerShowcaseMarkupsProperty);
		}
		set
		{
			SetValue(UseStullerShowcaseMarkupsProperty, value);
		}
	}

	public WindowModalAccount()
	{
		InitializeComponent();
		base.Closing += WindowModalAccount_Closing;
		if (!ViewModelBase.IsInDesignModeStatic)
		{
			_resetOptionValues = UIHelper.CounterSketchOptions.Clone() as CounterSketchOptions;
		}
		Messenger.Default.Register<string>((object)this, (Action<string>)ProcessGenericMessage, false);
	}

	private void BtnCancelClick(object sender, RoutedEventArgs e)
	{
		if (_resetOptionValues != null)
		{
			UIHelper.CounterSketchOptions = _resetOptionValues;
			UIHelper.CounterSketchOptions.WriteSettings();
			LoadOptions();
			if (ValidateData())
			{
				Close();
			}
		}
	}

	private void BtnCloseClick(object sender, RoutedEventArgs e)
	{
		if (ValidateData())
		{
			SaveOptions();
			Close();
		}
	}

	private void CbxShipToAccountNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.AddedItems.Count > 0)
		{
			object obj = e.AddedItems[0];
			ShipToAccount val = (ShipToAccount)((obj is ShipToAccount) ? obj : null);
			if (val != null)
			{
				txtShipToInfo.Text = val.CompanyName + "\n" + val.AddressLine1 + "\n" + val.City + ", " + val.State + " " + val.Zip + "\n" + val.Country + "\n";
			}
			if (val != null)
			{
				UIHelper.CounterSketchOptions.CountryCode = val.Country;
				ViewModelLocator.Current.StudioMaterialSelectionVM.ReloadMaterialsAfterAccountChanged();
			}
		}
	}

	private bool CheckTextBoxValue(TextBox textBox, bool checkPercentageMinimum, string optionalFieldName = "")
	{
		if (!UIHelper.TextValidation("^\\d*$", textBox.Text))
		{
			UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.SingleLineAlert, optionalFieldName + " " + Strings.Invalid_Format, messageIsModal: true);
			textBox.Text = textBox.Tag as string;
			textBox.Focus();
			return false;
		}
		if (checkPercentageMinimum)
		{
			if (!double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
			{
				UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.SingleLineAlert, optionalFieldName + " " + Strings.Invalid_Format, messageIsModal: true);
				textBox.Text = textBox.Tag as string;
				textBox.Focus();
				return false;
			}
			if (result < 100.0)
			{
				UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.SingleLineAlert, optionalFieldName + " " + Strings.Value_must_be_greater_than_100_percent, messageIsModal: true);
				textBox.Text = textBox.Tag as string;
				return false;
			}
		}
		return true;
	}

	private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
	{
		ViewModelLocator.Current.MainVM.OpenURLInOurBrowserWindow(e.Uri.ToString());
	}

	private void LoadOptions()
	{
		if (SecurityHelper.CustomerObject != null && UIHelper.CounterSketchOptions.ShipToAccountNumber != null && SecurityHelper.CustomerObject.ShipToAccounts.Any((ShipToAccount _) => _.AccountNumber == UIHelper.CounterSketchOptions.ShipToAccountNumber))
		{
			if (!string.IsNullOrEmpty(UIHelper.CounterSketchOptions.ShipToAccountNumber))
			{
				cbxShipToAccountNumber.SelectedValue = UIHelper.CounterSketchOptions.ShipToAccountNumber;
			}
			if (string.IsNullOrWhiteSpace(UIHelper.CounterSketchOptions.BillToAccountNumber))
			{
				UIHelper.CounterSketchOptions.BillToAccountNumber = SecurityHelper.CustomerObject.BillToAccount.AccountNumber;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
		}
		HasStullerShowcaseMarkupsAvailable = UIHelper.CounterSketchOptions.HasStullerShowcaseMarkups;
		txtPhone.Text = UIHelper.CounterSketchOptions.Phone;
		chkSendSMSNotifications.IsChecked = UIHelper.CounterSketchOptions.SendSMSNotifications;
		txtEmailQuote.Text = ((!string.IsNullOrEmpty(UIHelper.CounterSketchOptions.Email)) ? UIHelper.CounterSketchOptions.Email : UIHelper.CounterSketchOptions.ShipToContactEMail);
		chkRequirePreferencesPassword.IsChecked = UIHelper.CounterSketchOptions.RequirePasswordForPreferences;
		UpdateExchange();
	}

	private void ProcessGenericMessage(string message)
	{
		if (message == "WindowModalAccount_ReloadCulture")
		{
			UpdateExchange();
		}
	}

	private void UpdateExchange()
	{
		txtCurrencyEnum.Text = EnumHelper.GetEnumDescriptionFromValue((CSEnumCurrencyTypeOption)UIHelper.CounterSketchOptions.CurrencyEnumValue) ?? "";
		txtExchangedDollars.Text = $"{UIHelper.CounterSketchOptions.CurrentExchangeRate:c}";
	}

	private void PopulateAccounts(Customer stullerCustomer)
	{
		if (stullerCustomer != null)
		{
			txtBillToAccountNumber.Text = stullerCustomer.BillToAccount.AccountNumber;
			cbxShipToAccountNumber.SelectedValuePath = "AccountNumber";
			cbxShipToAccountNumber.DisplayMemberPath = "AccountNumber";
			cbxShipToAccountNumber.ItemsSource = stullerCustomer.ShipToAccounts;
			ShipToAccount val = stullerCustomer.ShipToAccounts[0];
			cbxShipToAccountNumber.SelectedItem = val;
			cbxShipToAccountNumber.SelectedValue = val.AccountNumber;
			txtBillToInfo.Text = stullerCustomer.BillToAccount.CompanyName + "\n" + stullerCustomer.BillToAccount.AddressLine1 + "\n" + stullerCustomer.BillToAccount.City + ", " + stullerCustomer.BillToAccount.State + " " + stullerCustomer.BillToAccount.Zip + "\n" + stullerCustomer.BillToAccount.Country + "\n";
			txtShipToInfo.Text = val.CompanyName + "\n" + val.AddressLine1 + "\n" + val.City + ", " + val.State + " " + val.Zip + "\n" + val.Country + "\n";
			if (stullerCustomer.ShipToAccounts.Count == 1)
			{
				cbxShipToAccountNumber.SelectedIndex = 0;
			}
			cbxShipToAccountNumber.SelectionChanged += CbxShipToAccountNumber_SelectionChanged;
		}
	}

	private void SaveOptions()
	{
		UIHelper.CounterSketchOptions.Email = txtEmailQuote.Text;
		UIHelper.CounterSketchOptions.Phone = txtPhone.Text;
		UIHelper.CounterSketchOptions.SendSMSNotifications = chkSendSMSNotifications.IsChecked == true;
		UIHelper.CounterSketchOptions.RequirePasswordForPreferences = chkRequirePreferencesPassword.IsChecked == true;
		UIHelper.CounterSketchOptions.WriteSettings();
	}

	private void TxtCurrencyEnumMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		ViewModelLocator.Current.MainVM.OpenPreferencesWindow(CSPreferenceTabs.Region);
	}

	private bool ValidateData()
	{
		return true;
	}

	private void WindowLoaded(object sender, RoutedEventArgs e)
	{
		if (!ViewModelBase.IsInDesignModeStatic)
		{
			PopulateAccounts(SecurityHelper.CustomerObject);
			LoadOptions();
		}
	}

	private void WindowModalAccount_Closing(object sender, CancelEventArgs e)
	{
		if (!ValidateData())
		{
			e.Cancel = true;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/windowmodalaccount.xaml", UriKind.Relative);
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
			_theWindow = (WindowModalAccount)target;
			_theWindow.Loaded += WindowLoaded;
			break;
		case 2:
			stackPanel = (StackPanel)target;
			break;
		case 3:
			panelOptions = (StackPanel)target;
			break;
		case 4:
			AccountSettings = (StackPanel)target;
			break;
		case 5:
			BillToShipTo = (Grid)target;
			break;
		case 6:
			cbxShipToAccountNumber = (ComboBox)target;
			break;
		case 7:
			txtBillToAccountNumber = (TextBlock)target;
			break;
		case 8:
			txtBillToInfo = (TextBlock)target;
			break;
		case 9:
			txtShipToInfo = (TextBlock)target;
			break;
		case 10:
			QuoteContactSettings = (StackPanel)target;
			break;
		case 11:
			EmailQuoteSettings = (StackPanel)target;
			break;
		case 12:
			txtEmailQuote = (TextBox)target;
			break;
		case 13:
			txtPhone = (TextBox)target;
			break;
		case 14:
			chkSendSMSNotifications = (CheckBox)target;
			break;
		case 15:
			((Hyperlink)target).RequestNavigate += Hyperlink_RequestNavigate;
			break;
		case 16:
			((Hyperlink)target).RequestNavigate += Hyperlink_RequestNavigate;
			break;
		case 17:
			txtDollars = (TextBlock)target;
			break;
		case 18:
			txtExchangedDollars = (TextBlock)target;
			break;
		case 19:
			txtCurrencyEnum = (TextBlock)target;
			txtCurrencyEnum.MouseLeftButtonDown += TxtCurrencyEnumMouseLeftButtonDown;
			break;
		case 20:
			ApplicationPermissions = (StackPanel)target;
			break;
		case 21:
			chkRequirePreferencesPassword = (CheckBox)target;
			break;
		case 22:
			ActionButtons = (DockPanel)target;
			break;
		case 23:
			btnCancel = (Button)target;
			btnCancel.Click += BtnCancelClick;
			break;
		case 24:
			btnClose = (Button)target;
			btnClose.Click += BtnCloseClick;
			break;
		case 25:
			txtInfoMessage = (TextBlock)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
