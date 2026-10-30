using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using GV.CounterSketch.Common;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Localization;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.Controls;

public partial class WindowModalAccount : Window, IComponentConnector
{
	public static readonly DependencyProperty HasStullerShowcaseMarkupsAvailableProperty = DependencyProperty.Register("HasStullerShowcaseMarkupsAvailable", typeof(bool), typeof(WindowModalAccount), new PropertyMetadata(false));

	public static readonly DependencyProperty UseStullerShowcaseMarkupsProperty = DependencyProperty.Register("UseStullerShowcaseMarkups", typeof(bool), typeof(WindowModalAccount), new PropertyMetadata(false));

	private const string HiddenPassword = "************";

	private readonly CounterSketchOptions _resetOptionValues;

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
		Messenger.Default.Register<string>(this, ProcessGenericMessage);
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
			ShipToAccount shipToAccount = e.AddedItems[0] as ShipToAccount;
			if (shipToAccount != null)
			{
				txtShipToInfo.Text = shipToAccount.CompanyName + "\n" + shipToAccount.AddressLine1 + "\n" + shipToAccount.City + ", " + shipToAccount.State + " " + shipToAccount.Zip + "\n" + shipToAccount.Country + "\n";
			}
			if (shipToAccount != null)
			{
				UIHelper.CounterSketchOptions.CountryCode = shipToAccount.Country;
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
			ShipToAccount shipToAccount = stullerCustomer.ShipToAccounts[0];
			cbxShipToAccountNumber.SelectedItem = shipToAccount;
			cbxShipToAccountNumber.SelectedValue = shipToAccount.AccountNumber;
			txtBillToInfo.Text = stullerCustomer.BillToAccount.CompanyName + "\n" + stullerCustomer.BillToAccount.AddressLine1 + "\n" + stullerCustomer.BillToAccount.City + ", " + stullerCustomer.BillToAccount.State + " " + stullerCustomer.BillToAccount.Zip + "\n" + stullerCustomer.BillToAccount.Country + "\n";
			txtShipToInfo.Text = shipToAccount.CompanyName + "\n" + shipToAccount.AddressLine1 + "\n" + shipToAccount.City + ", " + shipToAccount.State + " " + shipToAccount.Zip + "\n" + shipToAccount.Country + "\n";
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
		bool flag = true;
		if (!string.IsNullOrEmpty(txtEmailQuote.Text))
		{
			if (UIHelper.TextValidation("^[A-Za-z0-9_\\-\\.]+@(([A-Za-z0-9\\-])+\\.)+([A-Za-z\\-])+$", txtEmailQuote.Text))
			{
				flag = true;
			}
			else
			{
				flag = false;
				UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.SingleLineAlert, Strings.Email_address_invalid + ", " + Strings.format_must_be + " name@address.suffix!", messageIsModal: true);
				txtEmailQuote.Focus();
			}
		}
		else
		{
			flag = false;
			UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.SingleLineAlert, Strings.Email_address_invalid + ", " + Strings.must_have_a_value + "!", messageIsModal: true);
			txtEmailQuote.Focus();
		}
		if (!string.IsNullOrEmpty(txtPhone.Text))
		{
			if (UIHelper.TextValidation("^.{7,}$", txtPhone.Text))
			{
				flag = flag;
			}
			else
			{
				flag = false;
				UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.SingleLineAlert, Strings.Phone + " " + Strings.invalid + ", " + Strings.format_must_be + " 888-8888!", messageIsModal: true);
				txtPhone.Focus();
			}
		}
		else
		{
			flag = false;
			UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.SingleLineAlert, Strings.Phone + " " + Strings.invalid + ", " + Strings.must_have_a_value + "!", messageIsModal: true);
			txtPhone.Focus();
		}
		UIHelper.CounterSketchOptions.ShipToAccountNumber = cbxShipToAccountNumber.SelectedValue.ToString();
		StullerAccountUtility.UpdateCustomerFromAccountNumber(cbxShipToAccountNumber.SelectedValue.ToString());
		HasStullerShowcaseMarkupsAvailable = UIHelper.CounterSketchOptions.HasStullerShowcaseMarkups;
		if (!HasStullerShowcaseMarkupsAvailable)
		{
			flag = false;
			UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.SingleLineAlert, UntranslatedStrings.StullerShowcaseNotSetup + "!", messageIsModal: true);
		}
		return flag;
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
}
