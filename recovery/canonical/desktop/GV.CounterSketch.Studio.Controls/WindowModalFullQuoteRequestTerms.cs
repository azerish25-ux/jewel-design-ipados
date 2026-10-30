using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Controls.BLL;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Helpers;
using Localization;

namespace GV.CounterSketch.Studio.Controls;

public class WindowModalFullQuoteRequestTerms : Window, IComponentConnector
{
	public static readonly DependencyProperty UsersProperty = DependencyProperty.Register("Users", typeof(ObservableCollection<CSUser>), typeof(WindowModalFullQuoteRequestTerms), new UIPropertyMetadata(new ObservableCollection<CSUser>()));

	internal WindowModalFullQuoteRequestTerms Window;

	internal ComboBox cboUsers;

	internal CheckBox checkAgree;

	internal Grid gridSubmit;

	internal Button btnCancel;

	internal Button buttonRequestQuote;

	internal DockPanel EmailPhoneDetails;

	internal TextBox textQuoteEmail;

	internal TextBox textPhone;

	private bool _contentLoaded;

	public ObservableCollection<CSUser> Users
	{
		get
		{
			return (ObservableCollection<CSUser>)GetValue(UsersProperty);
		}
		set
		{
			SetValue(UsersProperty, value);
		}
	}

	public bool IsAgreementAccepted { get; set; }

	public string QuoteEmail { get; set; }

	public string QuotePhone { get; set; }

	public WindowModalFullQuoteRequestTerms(string email, string phone)
	{
		InitializeComponent();
		textQuoteEmail.Text = email;
		textPhone.Text = phone;
		using (CSApplicationDAL cSApplicationDAL = new CSApplicationDAL())
		{
			Users = cSApplicationDAL.GetUsers();
		}
		if (UIHelper.CounterSketchOptions != null && !UIHelper.CounterSketchOptions.ShowQuoteEmailWhileOrdering)
		{
			EmailPhoneDetails.Visibility = Visibility.Hidden;
		}
	}

	private void ButtonCancelClick(object sender, RoutedEventArgs e)
	{
		IsAgreementAccepted = false;
		Close();
	}

	private void ButtonRequestQuoteClick(object sender, RoutedEventArgs e)
	{
		IsAgreementAccepted = true;
		if (UIHelper.TextValidation("^[A-Za-z0-9_\\-\\.]+@(([A-Za-z0-9\\-])+\\.)+([A-Za-z\\-])+$", textQuoteEmail.Text))
		{
			QuoteEmail = textQuoteEmail.Text;
			if (UIHelper.TextValidation("^.{7,}$", textPhone.Text))
			{
				QuotePhone = textPhone.Text;
				Close();
			}
			else
			{
				UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.SingleLineAlert, Strings.PhoneInvalid, messageIsModal: true);
				textPhone.Focus();
			}
		}
		else
		{
			UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.SingleLineAlert, Strings.Email_address_invalid, messageIsModal: true);
			textQuoteEmail.Focus();
		}
	}

	private void CboUsersSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (cboUsers.SelectedItem is CSUser cSUser && UIHelper.CounterSketchOptions.ShowQuoteEmailWhileOrdering)
		{
			textPhone.Text = cSUser.Phone;
			textQuoteEmail.Text = cSUser.EMail;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/windowmodalfullquoterequestterms.xaml", UriKind.Relative);
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
			Window = (WindowModalFullQuoteRequestTerms)target;
			break;
		case 2:
			cboUsers = (ComboBox)target;
			cboUsers.SelectionChanged += CboUsersSelectionChanged;
			break;
		case 3:
			checkAgree = (CheckBox)target;
			break;
		case 4:
			gridSubmit = (Grid)target;
			break;
		case 5:
			btnCancel = (Button)target;
			btnCancel.Click += ButtonCancelClick;
			break;
		case 6:
			buttonRequestQuote = (Button)target;
			buttonRequestQuote.Click += ButtonRequestQuoteClick;
			break;
		case 7:
			EmailPhoneDetails = (DockPanel)target;
			break;
		case 8:
			textQuoteEmail = (TextBox)target;
			break;
		case 9:
			textPhone = (TextBox)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
