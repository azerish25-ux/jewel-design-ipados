using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using GV.CounterSketch.Common;
using GV.CounterSketch.Common.Utilities.Helpers;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Utilities.USB;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class SecurityHelper
{
	public enum LoginMode
	{
		NormalMode,
		SerialCodeMode
	}

	private static readonly string _amazonSocketAddress;

	private static readonly CancellationToken _cancelationToken;

	[CompilerGenerated]
	private static bool _003CIsStreaming_003Ek__BackingField;

	[CompilerGenerated]
	private static string _003CAuthenticationResponse_003Ek__BackingField;

	[CompilerGenerated]
	private static bool _003CIsAuthenticated_003Ek__BackingField;

	[CompilerGenerated]
	private static string _003CUserName_003Ek__BackingField;

	[CompilerGenerated]
	private static byte[] _003CPassword_003Ek__BackingField;

	public static bool IsStreaming
	{
		[CompilerGenerated]
		get
		{
			return true;
		}
		[CompilerGenerated]
		set
		{
			_003CIsStreaming_003Ek__BackingField = value;
		}
	}

	public static LoginMode AuthenticationMode { get; set; }

	public static string AuthenticationResponse
	{
		[CompilerGenerated]
		get
		{
			return "Authenticated";
		}
		[CompilerGenerated]
		set
		{
			_003CAuthenticationResponse_003Ek__BackingField = value;
		}
	}

	public static Customer CustomerObject { get; set; }

	public static bool IsAuthenticated
	{
		[CompilerGenerated]
		get
		{
			return true;
		}
		[CompilerGenerated]
		set
		{
			_003CIsAuthenticated_003Ek__BackingField = value;
		}
	}

	public static string ReasonForPrompt { get; set; }

	public static string UserName
	{
		[CompilerGenerated]
		get
		{
			return "ATOM";
		}
		[CompilerGenerated]
		set
		{
			_003CUserName_003Ek__BackingField = value;
		}
	}

	public static byte[] Password
	{
		[CompilerGenerated]
		get
		{
			return null;
		}
		[CompilerGenerated]
		set
		{
			_003CPassword_003Ek__BackingField = value;
		}
	}

	public static string ApplicationId { get; set; }

	public static string SessionId { get; set; }

	public static bool UserAlternateUserPath { get; internal set; }

	public static string FingerPrint { get; set; }

	static SecurityHelper()
	{
		_amazonSocketAddress = "wss://9os6ttxcze.execute-api.us-east-1.amazonaws.com/dev";
		_cancelationToken = default(CancellationToken);
		if (ViewModelBase.IsInDesignModeStatic)
		{
			IsStreaming = true;
		}
	}

	public static void Authenticate(string username, byte[] password, string serialCode)
	{
		IsAuthenticated = false;
		if (IsStreaming)
		{
			AuthenticationMode = LoginMode.NormalMode;
		}
		switch (AuthenticationMode)
		{
		case LoginMode.NormalMode:
			IsAuthenticated = CheckStullerCredentials(username, DataProtectionHelper.Protect(password));
			break;
		case LoginMode.SerialCodeMode:
			PerformSerialAuthentication(username, DataProtectionHelper.Protect(password), serialCode);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public static bool CheckStullerCredentials(string username, byte[] password)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		GlobalErrorHandler.WriteMethodInfo("CheckStullerCredentials");
		if (UIHelper.IsInternetAccessible())
		{
			Dispatcher.CurrentDispatcher.Invoke(delegate
			{
				AuthenticationResponse = "Authenticating...";
			});
			if (!ConnectivityHelper.IsStullerServicesAvailable(username, password))
			{
				AuthenticationResponse = "Authentication Services Unavailable";
				IsAuthenticated = false;
				return false;
			}
			try
			{
				StullerAPIClient val = new StullerAPIClient(username, Encoding.Default.GetString(DataProtectionHelper.Unprotect(password)), "https://api.stuller.com/v2/", (Action<string>)null);
				try
				{
					CustomerResponse val2 = val.FetchCustomerResponse();
					if (val2 != null)
					{
						UIHelper.CounterSketchOptions.HasEverAndEver = false;
						if (val2.Customer.ShipToAccounts != null && val2.Customer.ShipToAccounts[0] != null)
						{
							List<string> memberships = val2.Customer.ShipToAccounts[0].Memberships;
							if (memberships != null && memberships.Contains("Ever&Ever"))
							{
								UIHelper.CounterSketchOptions.HasEverAndEver = true;
							}
						}
						UIHelper.CounterSketchOptions.MemberId = val2.Customer.MemberId;
						UIHelper.CounterSketchOptions.Company = val2.Customer.BillToAccount.CompanyName;
						UIHelper.CounterSketchOptions.BillToAccountNumber = val2.Customer.BillToAccount.AccountNumber;
						if (IsStreaming)
						{
							CounterSketchOptions counterSketchOptions = UIHelper.CounterSketchOptions;
							ShipToAccount obj = val2.Customer.ShipToAccounts.FirstOrDefault();
							counterSketchOptions.ShipToAccountNumber = ((obj != null) ? obj.AccountNumber : null);
						}
						UIHelper.CounterSketchOptions.WriteSettings();
						Customer customer = val2.Customer;
						if (customer != null)
						{
							CustomerObject = customer;
						}
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				GlobalErrorHandler.WriteErrorToEventLog(ex.ToString());
				EventHelper.RecordImmediateEvent(EventHelper.EventTypes.Security_StullerCredentialsInvalid, username);
				AuthenticationResponse = "Incorrect Username and/or Password";
				IsAuthenticated = false;
				return false;
			}
			UIHelper.CounterSketchOptions.StullerHasAuthenticated = true;
			UIHelper.CounterSketchOptions.Password = password;
			UIHelper.CounterSketchOptions.Username = username;
			UIHelper.CounterSketchOptions.WriteSettings();
			UserName = username;
			Password = password;
			IsAuthenticated = true;
			AuthenticationResponse = "Login Successful.";
			return true;
		}
		UserName = username;
		Password = password;
		AuthenticationResponse = "Not Connected to Internet.";
		return false;
	}

	public static string HashPassword(string plainPassword)
	{
		byte[] bytes = Encoding.UTF8.GetBytes("StullerIsGreat!2011-SoAreTheGemvisionDevelopers");
		byte[] bytes2 = Encoding.UTF8.GetBytes(plainPassword);
		return Convert.ToBase64String(new HMACSHA256(bytes).ComputeHash(bytes2));
	}

	public static void Initialize()
	{
		if (!UIHelper.CounterSketchOptions.StullerHasAuthenticated && UIHelper.CounterSketchOptions.SerialCode != null)
		{
			AuthenticationMode = LoginMode.NormalMode;
			PromptForAuthentication();
		}
		else
		{
			UserName = UIHelper.CounterSketchOptions.Username;
			Password = UIHelper.CounterSketchOptions.Password;
		}
	}

	public static void PromptForAuthentication()
	{
		IsAuthenticated = false;
		WindowModalLogin windowModalLogin = new WindowModalLogin();
		double num = (int)Application.Current.MainWindow.ActualHeight;
		double num2 = (int)Application.Current.MainWindow.ActualWidth;
		windowModalLogin.MaxHeight = num;
		windowModalLogin.MaxWidth = num2;
		windowModalLogin.Height = num;
		windowModalLogin.Width = num2;
		windowModalLogin.ParentWindowHandle = new WindowInteropHelper(Application.Current.MainWindow).Handle;
		Rectangle windowRectangle = ScreenHelper.GetWindowRectangle();
		windowModalLogin.TopPosition = windowRectangle.Top;
		windowModalLogin.LeftPosition = windowRectangle.Left;
		windowModalLogin.Owner = Application.Current.MainWindow;
		windowModalLogin.ReasonForPrompt = ReasonForPrompt;
		windowModalLogin.LoginControlMode = AuthenticationMode;
		Messenger.Default.Send<string, WindowCounterSketchSplash>("Hide");
		windowModalLogin.ShowDialog();
	}

	public static bool ValidateForReducedFunctionalityMode()
	{
		return true;
	}

	public static bool ValidateStoredCredentials()
	{
		return true;
	}

	internal static void AuthenticateWithAmazonAndStuller()
	{
	}

	private static bool CheckIfUSBKeyPresent()
	{
		bool flag = CSKey.FindKeyInitial();
		if (flag)
		{
			CSKey.SetIDs();
		}
		if (flag)
		{
			CSKey.FindKey();
		}
		if (flag)
		{
			CSKey.FindKey2();
		}
		if (flag)
		{
			flag = CSKey.FindKey3();
		}
		if (flag)
		{
			flag = CSKey.FindKey4();
		}
		if (flag)
		{
			flag = CSKey.KeyDecode();
		}
		if (flag)
		{
			flag = CSKey.GetCSNoKey();
		}
		return flag;
	}

	private static void PerformSerialAuthentication(string username, byte[] password, string serialCode)
	{
		bool num = CheckStullerCredentials(username, password);
		if (!num)
		{
			UIHelper.CounterSketchOptions.Username = null;
			UIHelper.CounterSketchOptions.Password = null;
			UIHelper.CounterSketchOptions.WriteSettings();
		}
		if (num)
		{
			if (!ProductSerialValidation(serialCode))
			{
				IsAuthenticated = false;
			}
			else
			{
				IsAuthenticated = true;
			}
		}
		else
		{
			AuthenticationResponse = "Invalid Credentials! Try Again.";
		}
	}

	private static bool ProductSerialValidation(string code)
	{
		if (!string.IsNullOrEmpty(code))
		{
			byte[] bytecode = CSSecurity.ConvertSerialNumberToBytes(code);
			if (bytecode != null)
			{
				CSSecurity.SecurityResult securityResult = CSSecurity.SecurityCodeValidate(ref bytecode);
				if (securityResult == CSSecurity.SecurityResult.ValidKey || securityResult == CSSecurity.SecurityResult.NewComputerRegistered)
				{
					UIHelper.CounterSketchOptions.SerialCode = bytecode;
					UIHelper.CounterSketchOptions.WriteSettings();
					_ = 1;
					AuthenticationResponse = "Login Successful.";
					return true;
				}
				AuthenticationResponse = CSSecurity.GetDescriptionFromSecurityResult(securityResult);
				EventHelper.RecordEvent(EventHelper.EventTypes.Security_InvalidSerialCode, null, code);
				return false;
			}
		}
		AuthenticationResponse = "Invalid Serial Code, please try again.";
		return false;
	}
}
