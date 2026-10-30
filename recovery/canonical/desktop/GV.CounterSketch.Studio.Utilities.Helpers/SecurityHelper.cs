using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
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

	public static bool IsStreaming { get; set; }

	public static LoginMode AuthenticationMode { get; set; }

	public static string AuthenticationResponse { get; set; }

	public static Customer CustomerObject { get; set; }

	public static bool IsAuthenticated { get; set; }

	public static string ReasonForPrompt { get; set; }

	public static string UserName { get; set; }

	public static byte[] Password { get; set; }

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
				using StullerAPIClient stullerAPIClient = new StullerAPIClient(username, Encoding.Default.GetString(DataProtectionHelper.Unprotect(password)));
				CustomerResponse customerResponse = stullerAPIClient.FetchCustomerResponse();
				if (customerResponse != null)
				{
					UIHelper.CounterSketchOptions.HasEverAndEver = false;
					if (customerResponse.Customer.ShipToAccounts != null && customerResponse.Customer.ShipToAccounts[0] != null)
					{
						List<string> memberships = customerResponse.Customer.ShipToAccounts[0].Memberships;
						if (memberships != null && memberships.Contains("Ever&Ever"))
						{
							UIHelper.CounterSketchOptions.HasEverAndEver = true;
						}
					}
					UIHelper.CounterSketchOptions.MemberId = customerResponse.Customer.MemberId;
					UIHelper.CounterSketchOptions.Company = customerResponse.Customer.BillToAccount.CompanyName;
					UIHelper.CounterSketchOptions.BillToAccountNumber = customerResponse.Customer.BillToAccount.AccountNumber;
					if (IsStreaming)
					{
						UIHelper.CounterSketchOptions.ShipToAccountNumber = customerResponse.Customer.ShipToAccounts.FirstOrDefault()?.AccountNumber;
					}
					UIHelper.CounterSketchOptions.WriteSettings();
					Customer customer = customerResponse.Customer;
					if (customer != null)
					{
						CustomerObject = customer;
					}
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
		byte[] bytecode = UIHelper.CounterSketchOptions.SerialCode;
		if (bytecode != null)
		{
			CSSecurity.SecurityResult securityResult = CSSecurity.SecurityCodeValidate(ref bytecode);
			if (securityResult == CSSecurity.SecurityResult.ValidKey || securityResult == CSSecurity.SecurityResult.NewComputerRegistered)
			{
				if (securityResult == CSSecurity.SecurityResult.NewComputerRegistered)
				{
					UIHelper.CounterSketchOptions.IsFirstTimeRun = true;
					UIHelper.CounterSketchOptions.WriteSettings();
				}
				AuthenticationResponse = "Login Successful.";
				return true;
			}
		}
		AuthenticationMode = LoginMode.NormalMode;
		PromptForAuthentication();
		return IsAuthenticated;
	}

	public static bool ValidateStoredCredentials()
	{
		if (string.IsNullOrWhiteSpace(UserName) || Password == null || !CheckStullerCredentials(UserName, Password))
		{
			AuthenticationMode = LoginMode.NormalMode;
			PromptForAuthentication();
		}
		return IsAuthenticated;
	}

	internal static void AuthenticateWithAmazonAndStuller()
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		SocketIOHelper.GetStullerCredentialsFromAmazon(UserName, SessionId, FingerPrint);
		while (stopwatch.ElapsedMilliseconds < UIHelper.CounterSketchOptions.StreamingAuthenticationTimeoutMilliseconds)
		{
			if (SocketIOHelper.IsAuthenticating)
			{
				GlobalErrorHandler.WriteInfoToEventLog(".", 2);
				Thread.Sleep(200);
				continue;
			}
			if (!string.IsNullOrWhiteSpace(SocketIOHelper.UnencryptedPassword))
			{
				byte[] password = DataProtectionHelper.Protect(SocketIOHelper.UnencryptedPassword.GetBytes());
				if (CheckStullerCredentials(UserName, password))
				{
					IsAuthenticated = true;
				}
			}
			break;
		}
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
					if (securityResult == CSSecurity.SecurityResult.NewComputerRegistered)
					{
						UIHelper.CounterSketchOptions.IsFirstTimeRun = true;
						UIHelper.CounterSketchOptions.WriteSettings();
					}
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
