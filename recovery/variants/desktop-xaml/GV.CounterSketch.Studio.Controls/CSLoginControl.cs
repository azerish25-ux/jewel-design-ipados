using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using Localization;

namespace GV.CounterSketch.Studio.Controls;

public partial class CSLoginControl : UserControl, INotifyPropertyChanged, IComponentConnector
{
	private string _actionButtonName;

	private string _instruction;

	private SecurityHelper.LoginMode _loginControlMode;

	private string _response;

	private bool _showCancel;

	private bool _showNeedLogin;

	public string Instruction
	{
		get
		{
			return _instruction;
		}
		set
		{
			_instruction = value;
			RaisePropertyChanged("Instruction");
		}
	}

	public SecurityHelper.LoginMode LoginControlMode
	{
		get
		{
			return _loginControlMode;
		}
		set
		{
			_loginControlMode = value;
			SetControl();
			RaisePropertyChanged("LoginControlMode");
		}
	}

	public bool RequiresKeyValidation { get; set; }

	public string Response
	{
		get
		{
			return _response;
		}
		set
		{
			_response = value;
			RaisePropertyChanged("Response");
		}
	}

	public string UserName { get; set; }

	public bool ShowNeedLogin
	{
		get
		{
			return _showNeedLogin;
		}
		set
		{
			_showNeedLogin = value;
			RaisePropertyChanged("ShowNeedLogin");
		}
	}

	public bool ShowCancel
	{
		get
		{
			return _showCancel;
		}
		set
		{
			_showCancel = value;
			RaisePropertyChanged("ShowCancel");
		}
	}

	public string ActionButtonName
	{
		get
		{
			if (_actionButtonName == null)
			{
				_actionButtonName = Strings.Login;
			}
			return _actionButtonName;
		}
		set
		{
			_actionButtonName = value;
			RaisePropertyChanged("ActionButtonName");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public CSLoginControl()
	{
		InitializeComponent();
		if (!ViewModelBase.IsInDesignModeStatic)
		{
			SetControl();
			if (txtUsername != null)
			{
				txtUsername.Text = UIHelper.CounterSketchOptions.Username;
			}
			if (UIHelper.KeyProfile.KeyProfileId == 2)
			{
				txtNeedLogin.Visibility = Visibility.Hidden;
			}
		}
		else
		{
			Instruction = "Sample Instruction Text";
		}
	}

	private void Authenticate()
	{
		base.Dispatcher.Invoke(delegate
		{
			Response = "Checking Credentials...";
		});
		if (txtSerialCode.Text != null)
		{
			txtSerialCode.Text = txtSerialCode.Text.Trim();
		}
		SecurityHelper.Authenticate(txtUsername.Text, txtPassword.Password.GetBytes(), txtSerialCode.Text);
		Response = SecurityHelper.AuthenticationResponse;
	}

	private void btnCancel_Click(object sender, RoutedEventArgs e)
	{
		this.TryFindParent<Window>()?.Close();
	}

	private void btnLogin_Click(object sender, RoutedEventArgs e)
	{
		if (txtSerialCode.IsVisible)
		{
			try
			{
				string text = txtSerialCode.Text;
				if (!string.IsNullOrEmpty(text))
				{
					text = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant).Replace(text, string.Empty);
					text.Replace(" ", string.Empty);
					text = text.Insert(4, "-");
					text = text.Insert(9, "-");
					text = text.Insert(14, "-");
					txtSerialCode.Text = text;
				}
			}
			catch (Exception)
			{
				Response = Strings.SerialCodeIncorrectFormat;
				return;
			}
		}
		Authenticate();
		if (SecurityHelper.IsAuthenticated)
		{
			this.TryFindParent<Window>()?.Close();
		}
	}

	private void CSLoginUserControl_Loaded(object sender, RoutedEventArgs e)
	{
		if (UIHelper.CheckDesignTime())
		{
			return;
		}
		SecurityHelper.IsAuthenticated = false;
		if (!SecurityHelper.IsStreaming)
		{
			UIHelper.IsInternetAccessible();
		}
		if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
		{
			if (RequiresKeyValidation)
			{
				panelSerialCode.Visibility = Visibility.Visible;
			}
			if (UIHelper.CounterSketchOptions.Username != null && UIHelper.CounterSketchOptions.Username.Length > 1)
			{
				txtUsername.Text = UIHelper.CounterSketchOptions.Username;
				txtPassword.Focus();
			}
		}
	}

	private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void SetControl()
	{
		txtUsername.IsReadOnly = SecurityHelper.IsStreaming;
		SecurityHelper.AuthenticationMode = LoginControlMode;
		switch (LoginControlMode)
		{
		case SecurityHelper.LoginMode.NormalMode:
			VisualStateManager.GoToState(this, "Default", useTransitions: true);
			Instruction = ((UIHelper.KeyProfile.KeyProfileId == 2) ? (Strings.EnteryourCredentialsbelow + ":") : (Strings.EnterYourStullerCredentials + ":"));
			break;
		case SecurityHelper.LoginMode.SerialCodeMode:
			VisualStateManager.GoToState(this, "SerialCodeMode", useTransitions: true);
			Instruction = ((UIHelper.KeyProfile.KeyProfileId == 2) ? (Strings.EnterYourCredentialsAndSerialCode + ":") : (Strings.EnterYourStullerCredentialsAndSerialCode + ":"));
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void txtNeedLogin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		WindowBrowser windowBrowser = new WindowBrowser(new Uri("http://www.stuller.com/public/register.aspx"));
		if (Application.Current != null)
		{
			Window mainWindow = Application.Current.MainWindow;
			if (mainWindow != null)
			{
				windowBrowser.Owner = mainWindow;
			}
		}
		windowBrowser.ShowDialog();
	}

	private void txtPassword_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
	{
		if (Keyboard.IsKeyToggled(Key.Capital))
		{
			popupCapsWarning.IsOpen = true;
		}
		else
		{
			popupCapsWarning.IsOpen = false;
		}
	}

	private void txtPassword_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Capital)
		{
			if (Keyboard.IsKeyToggled(Key.Capital))
			{
				popupCapsWarning.IsOpen = true;
			}
			else
			{
				popupCapsWarning.IsOpen = false;
			}
		}
	}
}
