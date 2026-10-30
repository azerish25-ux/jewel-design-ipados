using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using GV.CounterSketch.Common;
using GV.CounterSketch.Common.Utilities.Helpers;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.SecurityServices;
using GV.CounterSketch.Studio.Utilities;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.View;
using GV.CounterSketchServices.WebServices;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Localization;
using Microsoft.Win32;

namespace GV.CounterSketch.Studio;

public class Program
{
	private const string Identifier = "{A8891938-3019-4120-A38E-3417D2013134}";

	private const string SecurityKeyword = "G3mv1s10nK3yPr0f1l3Upd4t3r!";

	private static readonly Dictionary<string, string> DisabledPluginsOriginalLoadModes = new Dictionary<string, string>();

	private static Thread _activationThread;

	private static string _designId;

	private static string _designVersion;

	private static string _filePath;

	private static string _fingerSizeUS;

	private static string _gem1;

	private static string _gem2;

	private static string _gem3;

	private static string _iterationId;

	private static string _iterationIndex;

	private static MainWindow _mainWindow;

	private static string _metal1;

	private static string _metal2;

	private static bool _noServer;

	private static string _rhinoInstallPath;

	private static string _rhinoPluginPath;

	private static bool _sendDebugReport;

	private static Thread _splashThread;

	private static bool _startInReducedFunctionalityMode;

	private static WindowCounterSketchSplash _windowCounterSketchSplash;

	private static int KeyProfileIdOverride { get; set; }

	[STAThread]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
	public static void Main(string[] parameters)
	{
		ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11;
		ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
		if (ViewModelBase.IsInDesignModeStatic)
		{
			App app = new App();
			app.InitializeComponent();
			app.Run();
			return;
		}
		try
		{
			if (parameters.Any())
			{
				CultureInfo culture = CultureInfo.InvariantCulture;
				parameters.ToList().ForEach(delegate(string p)
				{
					if (culture.CompareInfo.IndexOf(p, "streaming", CompareOptions.IgnoreCase) >= 0)
					{
						LogHelper.Log.Setup("C:\\Users\\" + Environment.UserName + "\\Gemvision\\CounterSketch Studio\\", "CounterSketch.log");
						LogHelper.Log.WriteLog("----------------------------------------------------------");
						LogHelper.Log.WriteLog("SessionContext = " + Environment.GetEnvironmentVariable("APPSTREAM_SESSION_CONTEXT"));
						SecurityHelper.IsStreaming = true;
					}
				});
			}
			if (!SecurityHelper.IsStreaming)
			{
				LogHelper.Log.Setup(UIHelper.CSDataRootPath, "CounterSketch.log");
			}
			GlobalErrorHandler.WriteInfoToEventLog("----------------------------------------------------------");
			GlobalErrorHandler.WriteInfoToEventLog(DateTime.Now.ToString());
			GlobalErrorHandler.WriteInfoToEventLog("----------------------------------------------------------");
			GlobalErrorHandler.WriteInfoToEventLog("CS Settings Path: " + UIHelper.CSDataRootPath);
			GlobalErrorHandler.WriteInfoToEventLog("CS Projects Path: " + UIHelper.CSProjectPath);
			GlobalErrorHandler.WriteInfoToEventLog("CS Startup Parameters: " + string.Join(", ", parameters));
			SetStartupParamters(parameters);
			if (!string.IsNullOrEmpty(UIHelper.CounterSketchOptions.Culture))
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo(UIHelper.CounterSketchOptions.Culture);
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(UIHelper.CounterSketchOptions.Culture);
			}
			if (SystemParameters.MenuDropAlignment)
			{
				FieldInfo field = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.Static | BindingFlags.NonPublic);
				if (field != null)
				{
					field.SetValue(null, false);
				}
			}
			if (!UIHelper.CounterSketchOptions.AllowMoreThanOneInstanceOfCounterSketch && Process.GetProcesses().FirstOrDefault((Process process2) => process2.ProcessName == "CounterSketch Studio" && process2.Id != Process.GetCurrentProcess().Id) != null)
			{
				PassRequestArgumentsToFirstInstance();
				GlobalErrorHandler.WriteInfoToEventLog("Found running instance, sending data via NamedPipeStream, and Shutdown.", 2);
				return;
			}
			if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
			{
				_sendDebugReport = true;
			}
			UIHelper.VersionRhinoServer = new Version(FileVersionInfo.GetVersionInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "CounterSketchServer\\CounterSketchServer.rhp")).FileVersion);
			UIHelper.Version = Assembly.GetExecutingAssembly().GetName().Version;
			if (UIHelper.Version.Build == 0)
			{
				UIHelper.Version = new Version("8.0.19270.3003");
			}
			GlobalErrorHandler.WriteInfoToEventLog($"CounterSketch v {UIHelper.VersionString} Starting up...\r\n");
			Version version = new Version((!string.IsNullOrWhiteSpace(UIHelper.CounterSketchOptions.ApplicationCurrentVersion)) ? UIHelper.CounterSketchOptions.ApplicationCurrentVersion : "1.0.0.0");
			Version version2 = UIHelper.Version;
			if (version.Major < 5 && version2.Major >= 5)
			{
				if (UIHelper.IsInternetAccessible())
				{
					using SecurityServiceClient securityServiceClient = new SecurityServiceClient("BasicHttpsBinding_ISecurityService");
					securityServiceClient.ResetKeyAsync(UIHelper.CounterSketchOptions.KeyId);
				}
				UIHelper.CounterSketchOptions = CounterSketchOptions.NewCounterSketchOptions();
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			EventHelper.Initialize();
			EventHelper.RecordImmediateEvent(EventHelper.EventTypes.Power_Up);
			if (SecurityHelper.IsStreaming)
			{
				using (SecurityServiceClient securityServiceClient2 = new SecurityServiceClient("BasicHttpsBinding_ISecurityService"))
				{
					KeyProfile keyProfileFromString = KeyProfile.GetKeyProfileFromString(securityServiceClient2.GetKeyProfileById(1));
					keyProfileFromString.HighlightAndAccent = (Color)ColorConverter.ConvertFromString("#FF3d6db5");
					UIHelper.KeyProfile = keyProfileFromString;
				}
				SecurityHelper.AuthenticateWithAmazonAndStuller();
				if (!SecurityHelper.IsAuthenticated)
				{
					MessageBox.Show(Strings.Key_Profile_Error_Contact_Support);
					return;
				}
				UIHelper.CounterSketchOptions.KeyId = 5321;
				UIHelper.CounterSketchOptions.UserTypeId = 1;
				UIHelper.CounterSketchOptions.HasStullerShowcaseMarkups = true;
				UIHelper.CounterSketchOptions.RequirePasswordForPreferences = false;
				UIHelper.CounterSketchOptions.WriteSettings();
				Task.Run(delegate
				{
					StullerAccountUtility.UpdateCustomerFromAccountNumber(UIHelper.CounterSketchOptions.ShipToAccountNumber);
				});
			}
			else if (UIHelper.IsInternetAccessible())
			{
				int keyId = UIHelper.CounterSketchOptions.KeyId;
				bool flag = true;
				CSSecurity.SecurityResult securityResult = CSSecurity.SecurityResult.InvalidKey;
				byte[] bytecode = UIHelper.CounterSketchOptions.SerialCode;
				if (bytecode != null && bytecode.Length >= 10 && !string.IsNullOrEmpty(UIHelper.CounterSketchOptions.Username) && !string.IsNullOrEmpty(UIHelper.CounterSketchOptions.Company))
				{
					securityResult = CSSecurity.SecurityCodeValidate(ref bytecode);
				}
				if (securityResult == CSSecurity.SecurityResult.ValidKey || securityResult == CSSecurity.SecurityResult.NewComputerRegistered)
				{
					SecurityHelper.IsAuthenticated = true;
					flag = false;
				}
				if (keyId != UIHelper.CounterSketchOptions.KeyId)
				{
					flag = true;
					EventHelper.RecordImmediateEvent(EventHelper.EventTypes.Security_KeyManipulated, "KeyIdAttempted:ActualKeyId", keyId + ":" + UIHelper.CounterSketchOptions.KeyId);
					GlobalErrorHandler.WriteInfoToEventLog($"Config File was Hand Edited.  Incorrect Key Attempted: {keyId}, Correct Key: {UIHelper.CounterSketchOptions.KeyId}");
				}
				if (securityResult == CSSecurity.SecurityResult.IncorrectComputer && flag && version.Major < 6 && version2.Major == 6)
				{
					using (SecurityServiceClient securityServiceClient3 = new SecurityServiceClient("BasicHttpsBinding_ISecurityService"))
					{
						securityServiceClient3.ResetKeyAsync(UIHelper.CounterSketchOptions.KeyId);
					}
					flag = false;
				}
				if (UIHelper.CounterSketchOptions.Username == null || UIHelper.CounterSketchOptions.Password == null || UIHelper.CounterSketchOptions.KeyId == 0 || flag)
				{
					if (!ShowActivationWindow())
					{
						return;
					}
					UIHelper.CounterSketchOptions.WriteSettings();
				}
				GlobalErrorHandler.WriteInfoToEventLog("Downloading Key Profile from Cloud.", 2);
				using SecurityServiceClient securityServiceClient4 = new SecurityServiceClient("BasicHttpsBinding_ISecurityService");
				try
				{
					if (KeyProfileIdOverride > 0)
					{
						int userTypeId;
						using (CounterSketchService counterSketchService = new CounterSketchService())
						{
							userTypeId = counterSketchService.GetUserTypeId(UIHelper.CounterSketchOptions.KeyId);
						}
						EventHelper.RecordImmediateEvent(EventHelper.EventTypes.Security_KeyProfileOverrideAttempted, "old:new", userTypeId + ":" + KeyProfileIdOverride);
						if (userTypeId > 2)
						{
							securityServiceClient4.UpdateKeyProfileId(KeyProfileIdOverride, UIHelper.CounterSketchOptions.KeyId, "G3mv1s10nK3yPr0f1l3Upd4t3r!");
							EventHelper.RecordImmediateEvent(EventHelper.EventTypes.Security_KeyProfileOverridden, "old:new", userTypeId + ":" + KeyProfileIdOverride);
						}
					}
					KeyProfile keyProfileFromString2 = KeyProfile.GetKeyProfileFromString(securityServiceClient4.GetKeyProfileByUserKeyId(UIHelper.CounterSketchOptions.KeyId));
					if (keyProfileFromString2.ProfileName == "Default")
					{
						GlobalErrorHandler.WriteErrorToEventLog("Profile Failed to Deserialize from Cloud, Default Profile Created.");
						MessageBox.Show(Strings.Key_Profile_Error_Contact_Support, Strings.Key_Profile_Error, MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK);
						return;
					}
					keyProfileFromString2.HighlightAndAccent = (Color)ColorConverter.ConvertFromString("#FF3d6db5");
					UIHelper.KeyProfile = keyProfileFromString2;
					UIHelper.KeyProfile.BackupSettings(UIHelper.CSDataRootPath);
				}
				catch (Exception ex)
				{
					GlobalErrorHandler.WriteErrorToEventLog(string.Format("Error Downloading Key Profile from Cloud. " + ex));
					if (!KeyProfile.GetBackupSettings(UIHelper.CSDataRootPath, out var profile))
					{
						MessageBox.Show(Strings.Key_Profile_Error_Missing_Backup, Strings.Key_Profile_Error, MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK);
						return;
					}
					UIHelper.KeyProfile = profile;
				}
			}
			else
			{
				if (!KeyProfile.GetBackupSettings(UIHelper.CSDataRootPath, out var profile2))
				{
					MessageBox.Show(Strings.Key_Profile_Error_No_Internet_Missing_Backup, Strings.Key_Profile_Error, MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK);
					return;
				}
				UIHelper.KeyProfile = profile2;
			}
			if (UIHelper.KeyProfile.KeyProfileId == 6 || UIHelper.KeyProfile.KeyProfileId == 7)
			{
				UIHelper.KeyProfile.IsDesignLibraryDisabled = true;
			}
			ThemeHelper.Initialize(UIHelper.KeyProfile, UIHelper.CSThemePath);
			CefSharpHelper.Initialize();
			if (UIHelper.CounterSketchOptions.HighlightAccentSavedColor != default(Color))
			{
				ThemeHelper.DefaultColorTheme.HighlightAndAccent = UIHelper.CounterSketchOptions.HighlightAccentSavedColor;
			}
			else
			{
				ThemeHelper.DefaultColorTheme.HighlightAndAccent = UIHelper.KeyProfile.HighlightAndAccent;
			}
			if (UIHelper.KeyProfile.ForceHideFreehand)
			{
				UIHelper.KeyProfile.HideFreehandMode = true;
				UIHelper.CounterSketchOptions.HideFreehandMode = true;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			if (SecurityHelper.IsStreaming)
			{
				UIHelper.CounterSketchOptions.CurrentEulaPath = Path.Combine(UIHelper.CSRootPath, "Eula\\CS_Streaming_EULA_05_2019.rtf");
			}
			else
			{
				UIHelper.CounterSketchOptions.CurrentEulaPath = Path.Combine(UIHelper.CSRootPath, (UIHelper.KeyProfile.KeyProfileId == 3) ? "Eula\\CS_International_EULA_02_2018.rtf" : "Eula\\CS_EULA_02_2018.rtf");
			}
			if (UIHelper.CounterSketchOptions.LastEulaPathAgreedTo != UIHelper.CounterSketchOptions.CurrentEulaPath)
			{
				UIHelper.CounterSketchOptions.HasAgreedToEula = false;
			}
			if (UIHelper.CounterSketchOptions.LastKeyProfileId != UIHelper.KeyProfile.KeyProfileId)
			{
				if (File.Exists(ThemeHelper.CustomThemePath))
				{
					File.SetAttributes(ThemeHelper.CustomThemePath, FileAttributes.Normal);
					File.Delete(ThemeHelper.CustomThemePath);
				}
				UIHelper.CounterSketchOptions.ThemingColorTheme = ThemeHelper.DefaultColorTheme;
				UIHelper.CounterSketchOptions.IsFirstTimeRun = true;
				UIHelper.CounterSketchOptions.HasAgreedToEula = false;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			UIHelper.CounterSketchOptions.LastKeyProfileId = UIHelper.KeyProfile.KeyProfileId;
			if (UIHelper.CounterSketchOptions.IsFirstTimeRun)
			{
				UIHelper.CounterSketchOptions.HideAdvancedStackOptions = UIHelper.KeyProfile.HideAdvancedStackOptions;
				UIHelper.CounterSketchOptions.HideFreehandMode = UIHelper.KeyProfile.HideFreehandMode;
				UIHelper.CounterSketchOptions.HideQuickEstimate = UIHelper.KeyProfile.HideQuickEstimate;
				UIHelper.CounterSketchOptions.ShowDesignerInSearchDetail = !UIHelper.KeyProfile.HideDesignerNameText;
				UIHelper.CounterSketchOptions.ShowPackagesInSearchDetail = !UIHelper.KeyProfile.HidePackageInfoText;
				UIHelper.CounterSketchOptions.ShowSeriesInSearchDetail = !UIHelper.KeyProfile.HideSeriesNumberText;
				UIHelper.CounterSketchOptions.EnableProjetPrinterSupport = !UIHelper.KeyProfile.HideProjetPrinterSupport;
				UIHelper.CounterSketchOptions.DisplayRedboxLogo = !UIHelper.KeyProfile.HideRedboxLogo;
				UIHelper.CounterSketchOptions.IsWhatsNewVisibleMainUI = !UIHelper.KeyProfile.HideIsWhatsNew;
				UIHelper.CounterSketchOptions.HideNEWRibbons = UIHelper.KeyProfile.HideNEWRibbons;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			if (!UIHelper.CounterSketchOptions.HideFreehandMode.HasValue)
			{
				UIHelper.CounterSketchOptions.HideFreehandMode = UIHelper.KeyProfile.HideFreehandMode;
			}
			if (!UIHelper.CounterSketchOptions.HideQuickEstimate.HasValue)
			{
				UIHelper.CounterSketchOptions.HideQuickEstimate = UIHelper.KeyProfile.HideQuickEstimate;
			}
			if (UIHelper.KeyProfile.RequireEULAAgreement && !UIHelper.CounterSketchOptions.HasAgreedToEula && new WindowEula().ShowDialog() != true)
			{
				return;
			}
			GlobalErrorHandler.WriteInfoToEventLog("Begin ShowSplash", 2);
			ShowSplash();
			GlobalErrorHandler.WriteInfoToEventLog("ShowSplash Complete", 2);
			if (_sendDebugReport)
			{
				Thread.Sleep(500);
				Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Transmitting_Debug_Report);
				GlobalErrorHandler.WriteInfoToEventLog("Transmitting Debug Report on Startup");
				TransmitDebugReport();
			}
			if (!CheckRhino())
			{
				return;
			}
			if (!SecurityHelper.IsStreaming)
			{
				string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TroubleShooter\\TroubleShooter.exe");
				if (File.Exists(text))
				{
					Thread.Sleep(500);
					Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Testing_Connectivity);
					GlobalErrorHandler.WriteInfoToEventLog("Testing Connectivity.", 2);
					ProcessStartInfo startInfo = new ProcessStartInfo(text, "silent countersketch");
					Process process = new Process
					{
						StartInfo = startInfo
					};
					process.Start();
					process.WaitForExit();
					if (process.ExitCode != 0)
					{
						if (process.ExitCode != -5)
						{
							ShowMessage(Strings.Problem_Preventing_Startup);
							return;
						}
						_startInReducedFunctionalityMode = true;
					}
				}
				EnableDisableRhinoPlugins(enabled: false);
				Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Disabling_Trouble_Plugins);
				GlobalErrorHandler.WriteInfoToEventLog("Disabling Trouble Plug-ins", 2);
				if (!CheckVRayRenderPlugin())
				{
					GlobalErrorHandler.WriteInfoToEventLog("Render Plugin is Missing from Filesystem.  Please run a repair your of CounterSketch install and restart.");
					ShowMessage("Render Plugin is Missing from Filesystem.  Please run a repair your of CounterSketch install and restart.");
					return;
				}
				ConnectivityHelper.SetUseUnsafeHeaderParsing(UIHelper.CounterSketchOptions.AllowUnsafeHTTPHeaders);
			}
			if (!_noServer)
			{
				Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Starting_Rhino);
				GlobalErrorHandler.WriteInfoToEventLog("Begin StartRhino", 2);
				if (!StartRhino())
				{
					return;
				}
				GlobalErrorHandler.WriteInfoToEventLog("StartRhino Complete", 2);
			}
			else
			{
				GlobalErrorHandler.WriteInfoToEventLog("/NoServer Parameter Set:  Skipping Server Creation.", 2);
			}
			Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Connecting_To_Rhino);
			UIHelper.CoreDataService.InitializeRhinoConnection();
			App app2 = new App();
			app2.InitializeComponent();
			try
			{
				Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Building_the_UI);
				GlobalErrorHandler.WriteInfoToEventLog("Building the UI.", 2);
				UIHelper.ReducedFunctionalityModeActive = _startInReducedFunctionalityMode;
				_mainWindow = new MainWindow
				{
					Topmost = false
				};
				Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Listening_for_Arguments);
				GlobalErrorHandler.WriteInfoToEventLog("Building the UI - Listending for Arguments.", 2);
				ListenForArgumentsFromSuccessiveInstances();
				if (!string.IsNullOrWhiteSpace(_designId))
				{
					GlobalErrorHandler.WriteInfoToEventLog($"Main: Starting window with Url Protocol Request, DesignId = {_designId}.", 2);
					LoadHelper.SetUrlProtocolRequestedDesign(_designId, _metal1, _metal2, _gem1, _gem2, _gem3, _iterationIndex, _iterationId, _designVersion);
				}
				if (!string.IsNullOrWhiteSpace(_filePath))
				{
					GlobalErrorHandler.WriteInfoToEventLog($"Main: Starting window with DoubleClick Request, Path = {_filePath}.", 2);
					LoadHelper.DoubleClickFileOpenRequest = _filePath;
				}
				Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Opening_Main_Window);
				GlobalErrorHandler.WriteInfoToEventLog("Building the UI - Opening Main Window.", 2);
				app2.Run(_mainWindow);
				GlobalErrorHandler.WriteInfoToEventLog("CounterSketch UI Closed.", 2);
			}
			finally
			{
				if (RhinoWindowHelper.RhinoCounterSketchServerProcess != null && !RhinoWindowHelper.RhinoCounterSketchServerProcess.HasExited)
				{
					GlobalErrorHandler.WriteInfoToEventLog("Program Finally Block: Killing Rhino. Die. Die.", 2);
					RhinoWindowHelper.RhinoCounterSketchServerProcess.Kill();
				}
				EnableDisableRhinoPlugins(enabled: true);
			}
		}
		catch (WaitHandleCannotBeOpenedException ex2)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Wait Handle Error in CS Program Main().\r\n" + ex2);
			if (ex2 != null)
			{
				GlobalErrorHandler.WriteErrorToEventLog("WaitHandle Exception: " + ex2);
			}
		}
		catch (Exception ex3)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Error in CS Program Main().\r\n" + ex3);
			GlobalErrorHandler.WriteErrorToEventLog("Exception: " + ex3);
			if (ex3.InnerException != null)
			{
				GlobalErrorHandler.WriteErrorToEventLog("Inner Exception: " + ex3.InnerException);
			}
		}
	}

	private static bool CheckRhino()
	{
		bool flag = true;
		using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
		{
			if (registryKey == null)
			{
				GlobalErrorHandler.WriteErrorToEventLog("Unable to Open Registry.");
				flag = false;
			}
			else
			{
				using RegistryKey registryKey2 = registryKey.OpenSubKey("Software\\McNeel\\Rhinoceros\\5.0x64\\Install", RegistryKeyPermissionCheck.ReadSubTree);
				string eventLogMessage = $"Rhino 5 Key not found in Registry.  Checked {registryKey2?.Name}";
				if (registryKey2 == null)
				{
					flag = false;
					GlobalErrorHandler.WriteErrorToEventLog(eventLogMessage);
				}
				else
				{
					object value = registryKey2.GetValue("Path");
					if (value == null)
					{
						flag = false;
						GlobalErrorHandler.WriteErrorToEventLog(eventLogMessage);
					}
					else
					{
						_rhinoInstallPath = value.ToString();
						_rhinoPluginPath = registryKey2.GetValue("Default Plug-ins Folder").ToString();
						object value2 = registryKey2.GetValue("BuildDate");
						if (value2 != null)
						{
							string[] array = value2.ToString().Split('-');
							DateTime dateTime = new DateTime(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
							if (dateTime < new DateTime(2015, 8, 10))
							{
								flag = false;
								GlobalErrorHandler.WriteErrorToEventLog("Rhino 5 is out of date: " + dateTime);
								ShowMessage(Strings.Rhino_Out_Of_Date);
							}
						}
					}
				}
			}
		}
		if (flag)
		{
			return true;
		}
		GlobalErrorHandler.WriteErrorToEventLog("Unable to find the Rhino 5 installation.");
		ShowMessage(Strings.Problem_Finding_Rhino);
		return false;
	}

	private static bool CheckVRayRenderPlugin()
	{
		string text = Path.Combine(_rhinoPluginPath, "VRayForRhino\\VRayForRhino.rhp");
		string text2 = null;
		bool flag = false;
		if (File.Exists("C:\\Program Files\\CounterSketch Studio\\VRay\\dlls\\VRayForMatrix.rhp"))
		{
			text2 = "C:\\Program Files\\CounterSketch Studio\\VRay\\dlls\\VRayForMatrix.rhp";
			flag = true;
		}
		if (File.Exists(text))
		{
			text2 = text;
			flag = true;
		}
		return true;
	}

	private static void ShowMessage(string message)
	{
		KillSplash();
		MessageBox.Show(message, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK);
	}

	public static void ListenForArgumentsFromSuccessiveInstances()
	{
		Task.Run(async delegate
		{
			await ListenForArguments();
		});
	}

	public static void PassRequestArgumentsToFirstInstance()
	{
		try
		{
			using NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream("{A8891938-3019-4120-A38E-3417D2013134}");
			using StreamWriter streamWriter = new StreamWriter(namedPipeClientStream);
			if (!string.IsNullOrWhiteSpace(_designId))
			{
				GlobalErrorHandler.WriteInfoToEventLog($"ArgumentsReceived: Opening design from Named Pipe Request - DesignId = {_designId}; Metal1 = {_metal1}; Metal2 = {_metal2}; Gem1 = {_gem1}; Gem2 = {_gem2}; Gem3 = {_gem3}; IterationIndex = {_iterationIndex}, IterationId = {_iterationId}, DesignVersion = {_designVersion}.", 2);
				namedPipeClientStream.Connect(1000);
				if (namedPipeClientStream.IsConnected)
				{
					streamWriter.WriteLine("URLPROTOCOLREQUEST");
					streamWriter.WriteLine("DesignId={0}", _designId);
					streamWriter.WriteLine("Metal1={0}", _metal1);
					streamWriter.WriteLine("Metal2={0}", _metal2);
					streamWriter.WriteLine("Gem1={0}", _gem1);
					streamWriter.WriteLine("Gem2={0}", _gem2);
					streamWriter.WriteLine("Gem3={0}", _gem3);
					streamWriter.Write("Iteration={0}", _iterationIndex);
					streamWriter.Write("IterationComponentId={0}", _iterationId);
					streamWriter.Write("DesignVersion={0}", _designVersion);
				}
			}
			else if (!string.IsNullOrWhiteSpace(_filePath))
			{
				GlobalErrorHandler.WriteInfoToEventLog($"PassRequestArgumentsToFirstInstance: Sending DoubleClick FilePath via Named Pipe Request - Path = {_filePath}.", 2);
				namedPipeClientStream.Connect(1000);
				if (namedPipeClientStream.IsConnected)
				{
					streamWriter.WriteLine("DOUBLECLICKREQUEST");
					streamWriter.Write("FilePath={0}", _filePath);
				}
			}
			else
			{
				GlobalErrorHandler.WriteInfoToEventLog("PassRequestArgumentsToFirstInstance: Bring existing instance to front.", 2);
				namedPipeClientStream.Connect(1000);
				if (namedPipeClientStream.IsConnected)
				{
					streamWriter.WriteLine("ITSSHOWTIME");
				}
			}
		}
		catch (TimeoutException)
		{
			GlobalErrorHandler.WriteInfoToEventLog("PassRequestArgumentsToFirstInstance: Exception while passing arguments to existing instance - Timeout.", 2);
		}
		catch (IOException)
		{
			GlobalErrorHandler.WriteInfoToEventLog("PassRequestArgumentsToFirstInstance: Exception while passing arguments to existing instance - Pipe broken.", 2);
		}
		catch (Exception arg)
		{
			GlobalErrorHandler.WriteInfoToEventLog($"PassRequestArgumentsToFirstInstance: Exception while passing arguments to existing instance - {arg}.", 2);
		}
	}

	private static void ArgumentsReceived(object state)
	{
		GlobalErrorHandler.WriteInfoToEventLog("InterProcess message received.", 2);
		IEnumerable<string> enumerable = state as IEnumerable<string>;
		try
		{
			if (enumerable != null && enumerable.FirstOrDefault() != null)
			{
				GlobalErrorHandler.WriteInfoToEventLog(string.Format("ArgumentsReceived: Arguments - {0}", enumerable.Aggregate((string existing, string current) => existing + " " + current)));
				if (enumerable.FirstOrDefault().ToUpper() == "DOUBLECLICKREQUEST")
				{
					ProcessDoubleClickInterProcRequest(enumerable);
					return;
				}
				if (enumerable.FirstOrDefault().ToUpper() == "URLPROTOCOLREQUEST")
				{
					ProcessUrlProtocolInterProcRequest(enumerable);
					return;
				}
				_mainWindow.Dispatcher.Invoke(delegate
				{
					_mainWindow.BringWindowToTop();
				});
			}
			else
			{
				GlobalErrorHandler.WriteInfoToEventLog("ArgumentsReceived: Empty message received from another instance of CounterSketch");
			}
		}
		catch (Exception arg)
		{
			GlobalErrorHandler.WriteInfoToEventLog($"ArgumentsReceived: Exception - {arg}");
		}
	}

	private static async Task ListenForArguments()
	{
		try
		{
			PipeSecurity pipeSecurity = new PipeSecurity();
			pipeSecurity.SetAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), PipeAccessRights.FullControl, AccessControlType.Allow));
			using NamedPipeServerStream server = new NamedPipeServerStream("{A8891938-3019-4120-A38E-3417D2013134}", PipeDirection.InOut, 2, PipeTransmissionMode.Message, PipeOptions.WriteThrough, 1024, 1024, pipeSecurity);
			using StreamReader reader = new StreamReader(server);
			GlobalErrorHandler.WriteInfoToEventLog("ListenForArguments: Listening for communication across named pipe.", 2);
			server.WaitForConnection();
			List<string> arguments = new List<string>();
			while (server.IsConnected)
			{
				string text = reader.ReadLine();
				GlobalErrorHandler.WriteInfoToEventLog($"ListenForArguments: Connection received across named pipe - {text}.", 2);
				arguments.Add(text);
			}
			GlobalErrorHandler.WriteInfoToEventLog("ListenForArguments: Processing message from named pipe.", 2);
			while (!UIHelper.IsApplicationInitialized)
			{
				await Task.Delay(1000);
			}
			ArgumentsReceived(arguments);
		}
		catch (IOException)
		{
			GlobalErrorHandler.WriteInfoToEventLog("ListenForArguments: Exception while listening for messages - broken pipe.", 2);
		}
		finally
		{
			GlobalErrorHandler.WriteInfoToEventLog("ListenForArguments: Respawn named pipe listener.", 2);
			ListenForArgumentsFromSuccessiveInstances();
		}
	}

	private static void ProcessDoubleClickInterProcRequest(IEnumerable<string> arguments)
	{
		_filePath = (from argument in arguments
			where argument.Split('=')[0].ToUpper() == "FILEPATH"
			select argument.Split('=')[1]).FirstOrDefault();
		if (!string.IsNullOrWhiteSpace(_filePath) && File.Exists(_filePath))
		{
			GlobalErrorHandler.WriteInfoToEventLog($"ProcessUrlProtocolInterProcRequest: Opening design from Named Pipe Request - FilePath = {_filePath}", 2);
			_mainWindow.Dispatcher.Invoke(delegate
			{
				_mainWindow.BringWindowToTop();
				ProjectManagementHelper.ImportBaseDesignFromDiskToActiveProjectAndOpenDesign(_filePath);
			});
		}
	}

	private static void ProcessDoubleClickRequest(string filePath)
	{
		GlobalErrorHandler.WriteInfoToEventLog($"ProcessDoubleClickRequest: Path - {filePath}.", 2);
		_filePath = filePath;
	}

	private static void ProcessUrlProtocolInterProcRequest(IEnumerable<string> arguments)
	{
		_designId = (from argument in arguments
			where argument.Split('=')[0].ToUpper() == "DESIGNID"
			select argument.Split('=')[1]).FirstOrDefault();
		_metal1 = (from argument in arguments
			where argument.Split('=')[0].ToUpper() == "METAL1"
			select argument.Split('=')[1]).FirstOrDefault() ?? "0";
		_metal2 = (from argument in arguments
			where argument.Split('=')[0].ToUpper() == "METAL2"
			select argument.Split('=')[1]).FirstOrDefault() ?? "0";
		_gem1 = (from argument in arguments
			where argument.Split('=')[0].ToUpper() == "GEM1"
			select argument.Split('=')[1]).FirstOrDefault() ?? "0";
		_gem2 = (from argument in arguments
			where argument.Split('=')[0].ToUpper() == "GEM2"
			select argument.Split('=')[1]).FirstOrDefault() ?? "0";
		_gem3 = (from argument in arguments
			where argument.Split('=')[0].ToUpper() == "GEM3"
			select argument.Split('=')[1]).FirstOrDefault() ?? "0";
		_iterationIndex = (from argument in arguments
			where argument.Split('=')[0].ToUpper() == "ITERATION"
			select argument.Split('=')[1]).FirstOrDefault();
		_iterationId = (from argument in arguments
			where argument.Split('=')[0].ToUpper() == "ITERATIONCOMPONENTID"
			select argument.Split('=')[1]).FirstOrDefault();
		_designVersion = (from argument in arguments
			where argument.Split('=')[0].ToUpper() == "DESIGNVERSION"
			select argument.Split('=')[1]).FirstOrDefault();
		_fingerSizeUS = (from argument in arguments
			where argument.Split('=')[0].ToUpper() == "FINGERSIZEUS"
			select argument.Split('=')[1]).FirstOrDefault();
		if (!string.IsNullOrWhiteSpace(_designId))
		{
			GlobalErrorHandler.WriteInfoToEventLog($"ArgumentsReceived: Opening design from Named Pipe Request - DesignId = {_designId}; Metal1 = {_metal1}; Metal2 = {_metal2}; Gem1 = {_gem1}; Gem2 = {_gem2}; Gem3 = {_gem3}; IterationIndex = {_iterationIndex}, IterationId = {_iterationId}, DesignVersion = {_designVersion}, FingerSizeUS = {_fingerSizeUS}.", 2);
			Application.Current.Dispatcher.Invoke(delegate
			{
				_mainWindow.BringWindowToTop();
				LoadHelper.SetUrlProtocolRequestedDesign(_designId, _metal1, _metal2, _gem1, _gem2, _gem3, _iterationIndex, _iterationId, _designVersion, _fingerSizeUS);
				EventHelper.RecordEvent(EventHelper.EventTypes.API_Design_Open, LoadHelper.UrlProtocolRequestedDesign.DesignId.ToString(), LoadHelper.UrlProtocolRequestedDesign.Version.ToString());
				LoadHelper.ExtractAndLoadDesignFromURL();
			});
		}
	}

	private static void SetStartupParamters(string[] parameters)
	{
		if (!parameters.Any())
		{
			return;
		}
		string completeParameterString = parameters.Aggregate((string existing, string current) => existing + " " + current);
		Regex cmdRegEx = new Regex("(?<name>.+?)=(?<val>.+)");
		Dictionary<string, string> cmdArgs = new Dictionary<string, string>();
		parameters.ToList().ForEach(delegate(string parameter)
		{
			Match match = cmdRegEx.Match(parameter);
			if (match.Success)
			{
				cmdArgs.Add(match.Groups[1].Value.ToUpper(), match.Groups[2].Value);
			}
		});
		parameters.ToList().ForEach(delegate(string parameter)
		{
			switch (parameter.ToUpper().Trim())
			{
			case "NOSERVER":
				_noServer = true;
				RhinoWindowHelper.IsNoServerSwitchEnabled = true;
				GlobalErrorHandler.WriteInfoToEventLog("SetStartupParamters: Setting NOSERVER switch.");
				break;
			case "KEYPROFILE=1":
				KeyProfileIdOverride = 1;
				break;
			case "KEYPROFILE=2":
				KeyProfileIdOverride = 2;
				break;
			case "KEYPROFILE=3":
				KeyProfileIdOverride = 3;
				break;
			case "KEYPROFILE=4":
				KeyProfileIdOverride = 4;
				break;
			case "KEYPROFILE=5":
				KeyProfileIdOverride = 5;
				break;
			case "ALTERNATEUSER":
				SecurityHelper.UserAlternateUserPath = true;
				break;
			case "STREAMING":
			{
				GlobalErrorHandler.WriteInfoToEventLog("+++> STREAMING MODE ENABLED!");
				SecurityHelper.IsStreaming = true;
				SecurityHelper.ApplicationId = "None";
				SecurityHelper.SessionId = "None";
				string environmentVariable = Environment.GetEnvironmentVariable("APPSTREAM_SESSION_CONTEXT");
				string value;
				if (!string.IsNullOrWhiteSpace(environmentVariable))
				{
					environmentVariable.Split(' ').ToList().ForEach(delegate(string _)
					{
						if (_.ToUpper().Contains("APPID"))
						{
							SecurityHelper.ApplicationId = _.Split('=')[1];
						}
						if (_.ToUpper().Contains("SESSIONID"))
						{
							SecurityHelper.SessionId = _.Split('=')[1];
						}
						if (_.ToUpper().Contains("USERNAME"))
						{
							SecurityHelper.UserName = _.Split('=')[1];
						}
						if (_.ToUpper().Contains("FINGERPRINT"))
						{
							SecurityHelper.FingerPrint = _.Split('=')[1];
						}
					});
				}
				else if (cmdArgs.TryGetValue("SESSIONID", out value))
				{
					SecurityHelper.SessionId = value;
					if (cmdArgs.TryGetValue("USERNAME", out var value2))
					{
						SecurityHelper.UserName = value2;
						if (cmdArgs.TryGetValue("FINGERPRINT", out var value3))
						{
							SecurityHelper.FingerPrint = value3;
						}
					}
				}
				break;
			}
			case "DEBUGPARAMS":
				GlobalErrorHandler.WriteInfoToEventLog("Showing Startup Parameters on Splash");
				UIHelper.ShowStartupParameters = true;
				UIHelper.StartupParameters = parameters.ToList();
				break;
			default:
				if (parameter.ToUpper().Trim().EndsWith(".CSG"))
				{
					ProcessDoubleClickRequest(completeParameterString);
				}
				break;
			}
		});
		GlobalErrorHandler.WriteInfoToEventLog($"SetStartupParamters: Provided parameters - {completeParameterString}.", 2);
	}

	private static bool StartRhino()
	{
		string text = Path.Combine(_rhinoInstallPath, "Rhino.exe");
		if (!File.Exists(text))
		{
			KillSplash();
			GlobalErrorHandler.WriteInfoToEventLog("Rhino Install Missing, aborting startup.", 2);
			MessageBox.Show(Strings.Rhino_64_Bit_Required);
			return false;
		}
		RegMergeFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "System\\Scheme_CounterSketch.reg"));
		GlobalErrorHandler.WriteInfoToEventLog("- Scheme Imported to Registry", 2);
		string arg = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "System\\DefaultViews\\CounterSketchTemplate.3dm");
		RegMergeSegment(string.Concat("[HKEY_CURRENT_USER\\Software\\McNeel\\Rhinoceros\\5.0x64\\Scheme: CounterSketch_Studio\\Options\\FileSettings]" + Environment.NewLine, $"\"Default Template File\"=\"{arg}\"{Environment.NewLine}"));
		GlobalErrorHandler.WriteInfoToEventLog($"- Starting Rhino from: {text}");
		string arg2 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "CounterSketchServer\\CounterSketchServer.rhp");
		GlobalErrorHandler.WriteInfoToEventLog($"- Server Plugin from: {arg2}");
		string text2 = $" \"{arg2}\" /nosplash /scheme=\"CounterSketch_Studio\" ";
		GlobalErrorHandler.WriteInfoToEventLog($"- Rhino Arguments: {text2}");
		ProcessStartInfo startInfo = new ProcessStartInfo
		{
			FileName = text,
			WindowStyle = ProcessWindowStyle.Hidden,
			Arguments = text2
		};
		RhinoWindowHelper.RhinoCounterSketchServerProcess = new Process
		{
			StartInfo = startInfo,
			EnableRaisingEvents = true
		};
		RhinoWindowHelper.RhinoCounterSketchServerProcess.ErrorDataReceived += RhinoErrorDataReceived;
		RhinoWindowHelper.RhinoCounterSketchServerProcess.Exited += RhinoCounterSketchServerExited;
		GlobalErrorHandler.WriteInfoToEventLog("- Starting Rhino...", 2);
		RhinoWindowHelper.RhinoCounterSketchServerProcess.Start();
		RhinoWindowHelper.RhinoCounterSketchServerProcess.WaitForInputIdle();
		AppDomain.CurrentDomain.ProcessExit += CurrentDomainProcessExitKillRhino;
		GlobalErrorHandler.WriteInfoToEventLog("- Rhino Started.", 2);
		return true;
	}

	private static void EnableDisableRhinoPlugins(bool enabled)
	{
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\McNeel\\Rhinoceros\\5.0x64");
			if (registryKey == null)
			{
				return;
			}
			string[] array = ConfigurationManager.AppSettings.Get("DisableRhinoPlugins").Split(',');
			foreach (string text in array)
			{
				using RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("Software\\McNeel\\Rhinoceros\\5.0x64\\Plug-ins\\" + text, RegistryKeyPermissionCheck.ReadWriteSubTree);
				if (registryKey2 == null)
				{
					continue;
				}
				if (enabled)
				{
					string text2 = DisabledPluginsOriginalLoadModes[text];
					if (text2 != null)
					{
						GlobalErrorHandler.WriteInfoToEventLog($"Setting plug-in {text} to previous state: {text2}", 2);
						registryKey2.SetValue("LoadMode", text2, RegistryValueKind.DWord);
						break;
					}
				}
				object value = registryKey2.GetValue("LoadMode");
				if (DisabledPluginsOriginalLoadModes.ContainsKey(text))
				{
					DisabledPluginsOriginalLoadModes[text] = value.ToString();
				}
				else if (value != null)
				{
					DisabledPluginsOriginalLoadModes.Add(text, value.ToString());
				}
				GlobalErrorHandler.WriteInfoToEventLog($"Disabling plugin {text}, Original Load Mode: {value}", 2);
				registryKey2.SetValue("LoadMode", 0, RegistryValueKind.DWord);
			}
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog(string.Format("Error attempting to Enable/Disable plugin\r\n" + ex));
		}
	}

	private static string GetSystemInformation()
	{
		string text = "";
		try
		{
			foreach (ManagementObject item in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem").Get())
			{
				text += "-----------------------------------<br />";
				text += "<h3>PC INFORMATION</h3>";
				text += "-----------------------------------<br />";
				text += string.Format("Bootup State: <b>{0}</b><br />", item["BootupState"]);
				text += string.Format("DNS HostName: <b>{0}</b><br />", item["DNSHostName"]);
				text += string.Format("Domain: <b>{0}</b><br />", item["Domain"]);
				text += string.Format("Number Of Logical Processors: <b>{0}</b><br />", item["NumberOfLogicalProcessors"]);
				text += string.Format("Number Of Processors: <b>{0}</b><br />", item["NumberOfProcessors"]);
				text += string.Format("Part Of Domain: <b>{0}</b><br />", item["PartOfDomain"]);
				text += string.Format("Primary Owner Contact: <b>{0}</b><br />", item["PrimaryOwnerContact"]);
				text += string.Format("Primary Owner Name: <b>{0}</b><br />", item["PrimaryOwnerName"]);
				text += string.Format("System Type: <b>{0}</b><br />", item["SystemType"]);
				text += string.Format("Total Physical Memory: <b>{0}</b> bytes<br />", item["TotalPhysicalMemory"]);
				text += string.Format("UserName: <b>{0}</b><br />", item["UserName"]);
				text += string.Format("Workgroup: <b>{0}</b><br />", item["Workgroup"]);
			}
			text += "-----------------------------------<br />";
			text += "<h3>MONITOR INFORMATION</h3>";
			text += "-----------------------------------<br />";
			foreach (ManagementObject item2 in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DesktopMonitor").Get())
			{
				text += string.Format("DeviceID: <b>{0}</b><br />", item2["DeviceID"]);
				text += string.Format("Monitor Manufacturer: <b>{0}</b><br />", item2["MonitorManufacturer"]);
				text += string.Format("Name: <b>{0}</b><br />", item2["Name"]);
				text += string.Format("Pixels Per X Logical Inch: <b>{0}</b><br />", item2["PixelsPerXLogicalInch"]);
				text += string.Format("Pixels Per Y Logical Inch: <b>{0}</b><br />", item2["PixelsPerYLogicalInch"]);
				text += string.Format("Screen Height: <b>{0}</b><br />", item2["ScreenHeight"]);
				text += string.Format("Screen Width: <b>{0}</b><br />", item2["ScreenWidth"]);
				text += "-----------------------------------<br />";
			}
			text += "<h3>DISPLAY ADAPTER INFORMATION</h3>";
			text += "-----------------------------------<br />";
			foreach (ManagementObject item3 in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController").Get())
			{
				text += string.Format("Mfr: <b>{0}</b><br />", item3["AdapterCompatibility"]);
				text += string.Format("Name: <b>{0}</b><br />", item3["Name"]);
				text += string.Format("Description: <b>{0}</b><br />", item3["Description"]);
				text += string.Format("Adapter DAC Type: <b>{0}</b><br />", item3["AdapterDACType"]);
				text += string.Format("Adapter RAM: <b>{0}</b><br />", item3["AdapterRAM"]);
				text += string.Format("Driver Date: <b>{0}</b><br />", item3["DriverDate"]);
				text += string.Format("Driver Version: <b>{0}</b><br />", item3["DriverVersion"]);
				text += string.Format("Current Bits Per Pixel: <b>{0}</b><br />", item3["CurrentBitsPerPixel"]);
				text += string.Format("Current Horizontal Resolution: <b>{0}</b><br />", item3["CurrentHorizontalResolution"]);
				text += string.Format("Current Number Of Colors: <b>{0}</b><br />", item3["CurrentNumberOfColors"]);
				text += string.Format("Current Number Of Columns: <b>{0}</b><br />", item3["CurrentNumberOfColumns"]);
				text += string.Format("Current Number Of Rows: <b>{0}</b><br />", item3["CurrentNumberOfRows"]);
				text += string.Format("Current Refresh Rate: <b>{0}</b><br />", item3["CurrentRefreshRate"]);
				text += string.Format("Current Scan Mode: <b>{0}</b><br />", item3["CurrentScanMode"]);
				text += string.Format("Current VerticalResolution: <b>{0}</b><br />", item3["CurrentVerticalResolution"]);
				text += string.Format("Device ID: <b>{0}</b><br />", item3["DeviceID"]);
				text += string.Format("Installed Display Drivers: <b>{0}</b><br />", item3["InstalledDisplayDrivers"]);
				text += string.Format("Max Refresh Rate: <b>{0}</b><br />", item3["MaxRefreshRate"]);
				text += string.Format("Min Refresh Rate: <b>{0}</b><br />", item3["MinRefreshRate"]);
				text += string.Format("PNP Device ID: <b>{0}</b><br />", item3["PNPDeviceID"]);
				text += string.Format("Video Mode: <b>{0}</b><br />", item3["VideoMode"]);
				text += string.Format("Video Mode Description: <b>{0}</b><br />", item3["VideoModeDescription"]);
				text += string.Format("Video Processor: <b>{0}</b><br />", item3["VideoProcessor"]);
				text += "-----------------------------------<br />";
			}
			text += "<h3>DRIVE INFORMATION</h3>";
			text += "-----------------------------------<br />";
			foreach (ManagementObject item4 in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_LogicalDisk").Get())
			{
				text += string.Format("Name: <b>{0}</b><br />", item4["Name"]);
				text += string.Format("Source: <b>{0}</b><br />", item4["ProviderName"]);
				text += string.Format("Size: <b>{0}</b><br />", item4["Size"]);
				text += string.Format("FreeSpace: <b>{0}</b><br />", item4["FreeSpace"]);
				text += "-----------------------------------<br />";
			}
		}
		catch (ManagementException)
		{
		}
		return text ?? "";
	}

	private static void RegMergeFile(string pathToRegFile)
	{
		if (!File.Exists(pathToRegFile))
		{
			return;
		}
		using StreamReader inputTextReader = new StreamReader(pathToRegFile);
		RegReadAndMerge(inputTextReader);
	}

	private static void RegMergeSegment(string regfileSegmentText)
	{
		using StringReader inputTextReader = new StringReader(regfileSegmentText);
		RegReadAndMerge(inputTextReader);
	}

	private static void RegReadAndMerge(TextReader inputTextReader)
	{
		string keyName = "";
		while (inputTextReader.Peek() >= 0)
		{
			string text = inputTextReader.ReadLine();
			if (text == null || string.IsNullOrWhiteSpace(text))
			{
				continue;
			}
			switch (text[0])
			{
			case '[':
				keyName = text.Trim('[', ']');
				break;
			case '"':
			{
				string text2 = text.Trim('"');
				char[] separator = new char[2] { '"', '=' };
				string[] array = text2.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length == 2)
				{
					if (array[0] == "DefCursorDumpFolder")
					{
						array[1] = array[1].Replace("\\\\", "\\");
					}
					Registry.SetValue(keyName, array[0], array[1]);
				}
				break;
			}
			}
		}
	}

	private static void TransmitDebugReport()
	{
		using (CounterSketchService counterSketchService = new CounterSketchService())
		{
			counterSketchService.TransmitDebuggingReport("Launch User", "Development@Gemvision.com", "123-123-1234", "User is having trouble launching CounterSketch.", "Unknown", "", "<PRE>" + GlobalErrorHandler.GetLogEntriesAsString() + "</PRE>", GetSystemInformation(), debugMode: false);
		}
		Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Debug_Report_Sent);
	}

	private static void KillSplash()
	{
		try
		{
			if (_windowCounterSketchSplash != null)
			{
				_windowCounterSketchSplash.ShutDown();
			}
		}
		catch (Exception)
		{
		}
	}

	private static bool ShowActivationWindow()
	{
		ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
		bool? result = false;
		SecurityHelper.IsAuthenticated = false;
		_activationThread = new Thread((ThreadStart)delegate
		{
			try
			{
				if (!string.IsNullOrEmpty(UIHelper.CounterSketchOptions.Culture))
				{
					Thread.CurrentThread.CurrentCulture = new CultureInfo(UIHelper.CounterSketchOptions.Culture);
					Thread.CurrentThread.CurrentUICulture = new CultureInfo(UIHelper.CounterSketchOptions.Culture);
				}
				WindowUnknownUserLogin windowUnknownUserLogin = new WindowUnknownUserLogin();
				windowUnknownUserLogin.Closing += delegate
				{
					if (!SecurityHelper.IsAuthenticated)
					{
						result = false;
					}
					else
					{
						result = true;
					}
					manualResetEvent.Set();
				};
				windowUnknownUserLogin.Show();
				Dispatcher.Run();
			}
			catch (Exception ex)
			{
				GlobalErrorHandler.WriteErrorToEventLog("Exception while showing Activation Window: \r\n" + ex);
				try
				{
					Dispatcher.CurrentDispatcher.InvokeShutdown();
				}
				catch
				{
				}
			}
		});
		_activationThread.SetApartmentState(ApartmentState.STA);
		_activationThread.Name = "Activation Screen";
		_activationThread.IsBackground = true;
		_activationThread.Start();
		manualResetEvent.WaitOne();
		return result.Value;
	}

	private static void ShowSplash()
	{
		ManualResetEvent resetSplashCreated = new ManualResetEvent(initialState: false);
		_splashThread = new Thread((ThreadStart)delegate
		{
			try
			{
				if (!string.IsNullOrEmpty(UIHelper.CounterSketchOptions.Culture))
				{
					Thread.CurrentThread.CurrentCulture = new CultureInfo(UIHelper.CounterSketchOptions.Culture);
					Thread.CurrentThread.CurrentUICulture = new CultureInfo(UIHelper.CounterSketchOptions.Culture);
				}
				_windowCounterSketchSplash = new WindowCounterSketchSplash();
				_windowCounterSketchSplash.Show();
				_windowCounterSketchSplash.Focus();
				resetSplashCreated.Set();
				Dispatcher.Run();
			}
			catch (Exception ex)
			{
				GlobalErrorHandler.WriteErrorToEventLog("Exception while showing Splash: \r\n" + ex);
				try
				{
					Dispatcher.CurrentDispatcher.InvokeShutdown();
					Thread.ResetAbort();
				}
				catch
				{
				}
			}
		})
		{
			IsBackground = true
		};
		_splashThread.SetApartmentState(ApartmentState.STA);
		_splashThread.IsBackground = true;
		_splashThread.Name = "Splash Screen";
		_splashThread.Start();
		resetSplashCreated.WaitOne();
	}

	private static void CurrentDomainProcessExitKillRhino(object sender, EventArgs e)
	{
		if (RhinoWindowHelper.RhinoCounterSketchServerProcess != null && !RhinoWindowHelper.RhinoCounterSketchServerProcess.HasExited)
		{
			GlobalErrorHandler.WriteInfoToEventLog("CurrentDomain Exited: Killing Rhino. Die. Die.");
			if (!RhinoWindowHelper.RhinoCounterSketchServerProcess.HasExited)
			{
				Thread.Sleep(100);
				RhinoWindowHelper.RhinoCounterSketchServerProcess.Kill();
			}
		}
	}

	private static void RhinoCounterSketchServerExited(object sender, EventArgs e)
	{
		GlobalErrorHandler.WriteInfoToEventLog($"** RHINO EXITED: {RhinoWindowHelper.RhinoCounterSketchServerProcess.ExitCode}");
	}

	private static void RhinoErrorDataReceived(object sender, DataReceivedEventArgs e)
	{
		GlobalErrorHandler.WriteErrorToEventLog($"- Rhino Error Data: {e.Data}");
	}
}
