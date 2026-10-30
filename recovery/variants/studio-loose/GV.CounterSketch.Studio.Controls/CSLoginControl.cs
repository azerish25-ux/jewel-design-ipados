using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using Localization;

namespace GV.CounterSketch.Studio.Controls;

public class CSLoginControl : UserControl, INotifyPropertyChanged, IComponentConnector
{
	private string _actionButtonName;

	private string _instruction;

	private SecurityHelper.LoginMode _loginControlMode;

	private string _response;

	private bool _showCancel;

	private bool _showNeedLogin;

	internal CSLoginControl CSLoginUserControl;

	internal Grid LayoutRoot;

	internal StackPanel panelLogin;

	internal StackPanel stackPanel;

	internal TextBlock txtInstruction;

	internal DockPanel panelUsername;

	internal TextBox txtUsername;

	internal DockPanel panelPassword;

	internal PasswordBox txtPassword;

	internal Popup popupCapsWarning;

	internal DockPanel panelSerialCode;

	internal TextBox txtSerialCode;

	internal TextBlock txtResponse;

	internal StackPanel panelButtons;

	internal TextBlock txtNeedLogin;

	internal Button btnCancel;

	internal Button btnLogin;

	internal VisualStateGroup LoginModeStates;

	internal VisualState Default;

	internal VisualState AlternatePasswordMode;

	internal VisualState SerialCodeMode;

	private bool _contentLoaded;

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

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/cslogincontrol.xaml", UriKind.Relative);
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
			CSLoginUserControl = (CSLoginControl)target;
			CSLoginUserControl.Loaded += CSLoginUserControl_Loaded;
			break;
		case 2:
			LayoutRoot = (Grid)target;
			break;
		case 3:
			panelLogin = (StackPanel)target;
			break;
		case 4:
			stackPanel = (StackPanel)target;
			break;
		case 5:
			txtInstruction = (TextBlock)target;
			break;
		case 6:
			panelUsername = (DockPanel)target;
			break;
		case 7:
			txtUsername = (TextBox)target;
			break;
		case 8:
			panelPassword = (DockPanel)target;
			break;
		case 9:
			txtPassword = (PasswordBox)target;
			txtPassword.GotKeyboardFocus += txtPassword_GotKeyboardFocus;
			txtPassword.KeyDown += txtPassword_KeyDown;
			break;
		case 10:
			popupCapsWarning = (Popup)target;
			break;
		case 11:
			panelSerialCode = (DockPanel)target;
			break;
		case 12:
			txtSerialCode = (TextBox)target;
			break;
		case 13:
			txtResponse = (TextBlock)target;
			break;
		case 14:
			panelButtons = (StackPanel)target;
			break;
		case 15:
			txtNeedLogin = (TextBlock)target;
			txtNeedLogin.MouseLeftButtonDown += txtNeedLogin_MouseLeftButtonDown;
			break;
		case 16:
			btnCancel = (Button)target;
			btnCancel.Click += btnCancel_Click;
			break;
		case 17:
			btnLogin = (Button)target;
			btnLogin.Click += btnLogin_Click;
			break;
		case 18:
			LoginModeStates = (VisualStateGroup)target;
			break;
		case 19:
			Default = (VisualState)target;
			break;
		case 20:
			AlternatePasswordMode = (VisualState)target;
			break;
		case 21:
			SerialCodeMode = (VisualState)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
