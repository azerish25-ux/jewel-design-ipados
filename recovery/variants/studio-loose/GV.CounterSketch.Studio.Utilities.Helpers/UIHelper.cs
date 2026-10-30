using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Common;
using GV.CounterSketch.Common.Utilities.Helpers;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Model;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.View;
using GV.CounterSketch.Studio.ViewModel;
using GV.CounterSketchServices.WebServices;
using GalaSoft.MvvmLight;
using Localization;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class UIHelper
{
	public delegate void ApplicationMessageHandler(object sender, ApplicationMessageEventArgs e);

	public const double DwtToGramsFactor = 1.55517;

	public const string RegexEmail = "^[A-Za-z0-9_\\-\\.]+@(([A-Za-z0-9\\-])+\\.)+([A-Za-z\\-])+$";

	public const string RegexNumber = "^\\d*$";

	public const string RegexPhone = "^.{7,}$";

	public const string RegexZip = "^\\d{5}(-\\d{4})?$";

	public static Guid BlockInstanceGuidToHighlight;

	public static string StullerApiVersion;

	public static Timer HighlightTimer;

	private static HttpClient _client;

	private static CSCoreDataService _coreDataService;

	private static string _csRootPath;

	private static bool _isOrderedDesignLoaded;

	private static bool _lastInternetCheckResult;

	private static RhinoPosition _lastRhinoPosition;

	private static PortfolioManagerView _portfolioManagerView;

	private static BitmapImage _sampleDesignImage;

	private static BitmapImage _shadowImagePendant;

	private static BitmapImage _shadowImageRing;

	private static List<ComponentsEnum> _visibleComponents;

	private static WindowModalWait _windowModalWait;

	[CompilerGenerated]
	private static bool _003CReducedFunctionalityModeActive_003Ek__BackingField;

	[CompilerGenerated]
	private static bool _003CSkipInternetWarning_003Ek__BackingField;

	[CompilerGenerated]
	private static bool _003CAllowApplicationClose_003Ek__BackingField;

	private static CancellationTokenSource _cancelSetupTokenSource;

	public static int MaxNumberOfAvailableCores
	{
		get
		{
			if (Environment.ProcessorCount <= 1)
			{
				return 1;
			}
			return Environment.ProcessorCount - 1;
		}
	}

	public static CSCoreDataService CoreDataService
	{
		get
		{
			if (ViewModelBase.IsInDesignModeStatic)
			{
				return new CSCoreDataService();
			}
			if (_coreDataService == null)
			{
				_coreDataService = new CSCoreDataService();
			}
			return _coreDataService;
		}
	}

	public static string CSDataRootPath { get; set; }

	public static string CSLibraryPath => Path.Combine(CSRootPath, "Library");

	public static string CSRootPath
	{
		get
		{
			if (ViewModelBase.IsInDesignModeStatic)
			{
				_csRootPath = "D:\\tfs\\CounterSketchSystem\\CounterSketch\\Studio\\Studio\\bin\\debug";
			}
			else if (_csRootPath == null)
			{
				_csRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			}
			return _csRootPath;
		}
	}

	public static string LatestVersionString
	{
		get
		{
			if (LatestVersion != null)
			{
				return string.Format("{0}.{1}.{2}.{3}", LatestVersion.Major, LatestVersion.Minor, LatestVersion.Build, LatestVersion.Revision.ToString("0###"));
			}
			return "UNKNOWN";
		}
	}

	public static PortfolioManagerView PortfolioManagerView
	{
		get
		{
			if (_portfolioManagerView == null)
			{
				_portfolioManagerView = new PortfolioManagerView
				{
					DataContext = ViewModelLocator.Current.PortfolioManagerVM,
					Owner = Application.Current.MainWindow,
					WindowStartupLocation = WindowStartupLocation.CenterScreen,
					Icon = Application.Current.MainWindow.Icon,
					Width = 1300.0,
					Height = 740.0
				};
			}
			return _portfolioManagerView;
		}
	}

	public static string VersionString
	{
		get
		{
			if (Version != null)
			{
				return string.Format("{0}.{1}.{2}.{3}", Version.Major, Version.Minor, Version.Build, Version.Revision.ToString("0###"));
			}
			return "UNKNOWN";
		}
	}

	public static BitmapImage SampleDesignImage
	{
		get
		{
			if (_sampleDesignImage == null)
			{
				_sampleDesignImage = new BitmapImage(new Uri("pack://application:,,,/CounterSketch Studio;component/Images/SampleDesignImage.png"));
			}
			return _sampleDesignImage;
		}
	}

	public static GV.CounterSketch.Studio.Model.Objects.Design LoadedDesign { get; set; }

	public static int DefaultRingSizeIndex { get; set; }

	public static KeyProfile KeyProfile { get; set; }

	public static CounterSketchOptions CounterSketchOptions { get; set; }

	public static string CSApplicationDatabasePath { get; set; }

	public static string CSFeaturedDesign { get; set; }

	public static string CSLibraryDatabasePath { get; set; }

	public static string ZBinFileName { get; set; }

	public static string ZBinFileNameNew { get; set; }

	public static string CSModelLibraryCSGL { get; set; }

	public static string CSProjectPath { get; set; }

	public static string CSPrototypeOrdersPath { get; set; }

	public static string CSQuotesPath { get; set; }

	public static string CSUpdatesPath { get; set; }

	public static RhinoPosition LastRhinoPosition
	{
		get
		{
			if (_lastRhinoPosition == null)
			{
				_lastRhinoPosition = new RhinoPosition();
			}
			return _lastRhinoPosition;
		}
		set
		{
			_lastRhinoPosition = value;
		}
	}

	public static bool IsApplicationInitialized { get; set; }

	public static bool IsDesignSpinning { get; set; }

	public static bool IsGemReportActive { get; set; }

	public static bool IsRhinoVisible { get; set; }

	public static bool? LastMessageBoxAccepted { get; set; }

	public static bool? LastMessageBoxCancelled { get; set; }

	public static Version LatestVersion { get; set; }

	public static string RhinoBuildDate { get; set; }

	public static string LatestRhinoBuildDate { get; set; }

	public static IntPtr LivePreviewWindowHandle { get; set; }

	public static bool ReducedFunctionalityModeActive
	{
		get
		{
			return false;
		}
		[CompilerGenerated]
		set
		{
			_003CReducedFunctionalityModeActive_003Ek__BackingField = value;
		}
	}

	public static bool SkipInternetWarning
	{
		[CompilerGenerated]
		get
		{
			return true;
		}
		[CompilerGenerated]
		set
		{
			_003CSkipInternetWarning_003Ek__BackingField = value;
		}
	}

	public static Version Version { get; set; }

	public static Version VersionRhinoServer { get; set; }

	public static List<ComponentsEnum> VisibleComponents
	{
		get
		{
			if (ViewModelBase.IsInDesignModeStatic)
			{
				_visibleComponents = new List<ComponentsEnum>
				{
					ComponentsEnum.Band1,
					ComponentsEnum.Band2,
					ComponentsEnum.Pendant,
					ComponentsEnum.Ring
				};
				return _visibleComponents;
			}
			if (_visibleComponents == null)
			{
				_visibleComponents = new List<ComponentsEnum>();
			}
			return _visibleComponents;
		}
		set
		{
			_visibleComponents = value;
		}
	}

	public static string ProjetLastIPLocation { get; set; }

	public static double ProjetIncreaseSupportAmount { get; set; }

	public static bool IsApplicationClosing { get; set; }

	public static bool AllowTileNavigation { get; set; }

	public static bool AllowApplicationClose
	{
		get
		{
			return true;
		}
		[CompilerGenerated]
		set
		{
			_003CAllowApplicationClose_003Ek__BackingField = value;
		}
	}

	public static string LastDesignPathLoaded { get; set; }

	public static bool IsWaitVisible { get; set; }

	public static bool FocusRequestDisabled { get; set; }

	public static bool IsOrderDeserializing { get; set; }

	public static bool IsOrderedDesignLoaded
	{
		get
		{
			if (IsOrderDeserializing)
			{
				return true;
			}
			return _isOrderedDesignLoaded;
		}
		set
		{
			_isOrderedDesignLoaded = value;
			ViewModelLocator.Current.MainVM.IsOrderedDesignLoaded = value;
		}
	}

	public static bool IsResettingApplicationState { get; set; }

	public static string DefaultCSProjectPath { get; set; }

	public static ImageType CurrentSearchResultImageType { get; set; }

	public static bool ShowStartupParameters { get; internal set; }

	public static List<string> StartupParameters { get; internal set; }

	public static string CSThemePath { get; set; }

	public static event ApplicationMessageHandler ApplicationMessageEventOccurred;

	public static event EventHandler MaterialsUpdatedEvent;

	public static IEnumerable<DependencyObject> GetChildObjects(this DependencyObject parent)
	{
		if (parent == null)
		{
			yield break;
		}
		if (parent is ContentElement current)
		{
			foreach (object child in LogicalTreeHelper.GetChildren(current))
			{
				if (child is DependencyObject)
				{
					yield return (DependencyObject)child;
				}
			}
		}
		else
		{
			int count = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < count; i++)
			{
				yield return VisualTreeHelper.GetChild(parent, i);
			}
		}
	}

	public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
	{
		if (depObj == null)
		{
			yield break;
		}
		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
			if (child is T)
			{
				yield return (T)child;
			}
			foreach (T item in FindVisualChildren<T>(child))
			{
				yield return item;
			}
		}
	}

	public static DependencyObject GetParentObject(this DependencyObject child)
	{
		if (child == null)
		{
			return null;
		}
		if (child is ContentElement contentElement)
		{
			DependencyObject parent = ContentOperations.GetParent(contentElement);
			if (parent != null)
			{
				return parent;
			}
			if (!(contentElement is FrameworkContentElement frameworkContentElement))
			{
				return null;
			}
			return frameworkContentElement.Parent;
		}
		return VisualTreeHelper.GetParent(child);
	}

	static UIHelper()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		StullerApiVersion = "v2/";
		HighlightTimer = new Timer(HighlightGraphObject);
		_client = new HttpClient();
		LoadedDesign = new GV.CounterSketch.Studio.Model.Objects.Design();
		DefaultRingSizeIndex = -1;
		SetApplicationPaths();
		KeyProfile = new KeyProfile();
		if (ViewModelBase.IsInDesignModeStatic)
		{
			KeyProfile.SetValuesToDefaultCounterSketch();
		}
		CounterSketchOptions = CounterSketchOptions.ReadSettings(CSDataRootPath);
		if (!SecurityHelper.IsStreaming)
		{
			SetProjectPathFromUserSettings();
		}
		AllowTileNavigation = true;
		CurrentSearchResultImageType = ImageType.Perspective;
	}

	public static BitmapImage BitmapImageFromByteArray(byte[] bytes, int decodePixelWidth)
	{
		BitmapImage bitmapImage = new BitmapImage();
		if (bytes == null)
		{
			return GetBlankImage();
		}
		using WrappingStream streamSource = new WrappingStream(new MemoryStream(bytes, writable: false));
		bitmapImage.BeginInit();
		bitmapImage.StreamSource = streamSource;
		bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
		bitmapImage.DecodePixelWidth = decodePixelWidth;
		RenderOptions.SetBitmapScalingMode(bitmapImage, BitmapScalingMode.LowQuality);
		bitmapImage.EndInit();
		bitmapImage.Freeze();
		return bitmapImage;
	}

	public static bool CheckDesignTime()
	{
		if ((bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue)
		{
			return true;
		}
		return false;
	}

	public static void EnableBypassSecurityCertificateValidation()
	{
		ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
	}

	public static void GenerateNewDesignTileImages()
	{
		if (!CSLibraryHelper.IsInitialized)
		{
			return;
		}
		try
		{
			List<DesignSearch> newDesigns = SearchHelper.GetNewDesigns();
			int count = newDesigns.Count;
			DesignSearch designSearch = null;
			if (count > 0)
			{
				designSearch = newDesigns[RandomHelper.GetRandomInteger(0, count)];
			}
			TileItem tileItem = ViewModelLocator.Current.TileViewDesignsVM.Tiles?.FirstOrDefault((TileItem t) => t.Title == Strings.NewDesigns);
			if (designSearch == null || tileItem == null)
			{
				return;
			}
			if (File.Exists(CSFeaturedDesign))
			{
				File.SetAttributes(CSFeaturedDesign, FileAttributes.Normal);
				File.Delete(CSFeaturedDesign);
			}
			BitmapImage imageSource = LibraryGImageHelper.GetImageSource(ImageType.Perspective, 3, 3, 105, 105, 105, designSearch.DesignId, designSearch.Version);
			ScaleTransform newTransform = new ScaleTransform(-1.0, 1.0, 0.0, 0.0);
			if (imageSource != null)
			{
				TransformedBitmap source = new TransformedBitmap(imageSource, newTransform);
				PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
				pngBitmapEncoder.Frames.Add(BitmapFrame.Create(source));
				using FileStream stream = new FileStream(CSFeaturedDesign, FileMode.Create);
				pngBitmapEncoder.Save(stream);
			}
			tileItem.RefreshImagePath();
		}
		catch (Exception)
		{
		}
	}

	public static BitmapImage GetBlankImage()
	{
		BitmapImage bitmapImage = new BitmapImage();
		try
		{
			bitmapImage.BeginInit();
			bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
			bitmapImage.UriSource = new Uri("pack://application:,,,/CounterSketch Studio;component/Images/MissingImage.png");
			bitmapImage.DecodePixelWidth = 90;
			bitmapImage.EndInit();
			bitmapImage.Freeze();
		}
		catch (Exception)
		{
		}
		return bitmapImage;
	}

	public static BitmapImage GetShadowImage(int componentTypeId)
	{
		switch (componentTypeId)
		{
		case 0:
		case 1:
		case 2:
		case 3:
			if (_shadowImageRing == null)
			{
				_shadowImageRing = GetRenderShadowImage(componentTypeId);
			}
			return _shadowImageRing;
		case 4:
			if (_shadowImagePendant == null)
			{
				_shadowImagePendant = GetRenderShadowImage(componentTypeId);
			}
			return _shadowImagePendant;
		default:
			return GetBlankImage();
		}
	}

	[DllImport("user32.dll")]
	public static extern int GetSystemMetrics(int nIndex);

	public static void HighlightGraphObject(object state)
	{
		if (!CounterSketchOptions.DisableViewportHighlight && BlockInstanceGuidToHighlight != Guid.Empty)
		{
			CoreDataService.HighlightRhinoDocumentGraphItem(BlockInstanceGuidToHighlight, isHighlighted: true);
		}
	}

	public static bool IsInternetAccessible(bool runCheckSilent = false)
	{
		if (SecurityHelper.IsStreaming)
		{
			return true;
		}
		bool flag = false;
		if (_lastInternetCheckResult)
		{
			return _lastInternetCheckResult;
		}
		bool reducedFunctionalityModeActive = ReducedFunctionalityModeActive;
		ReducedFunctionalityModeActive = false;
		DebuggingHelper.PrintTimedOutput("IsInternetAccessible Start");
		if (SkipInternetWarning)
		{
			runCheckSilent = true;
		}
		if (InternetUtility.IsConnectedToInternet())
		{
			try
			{
				using CounterSketchService counterSketchService = new CounterSketchService();
				counterSketchService.GetLastNoticeId();
			}
			catch (Exception)
			{
			}
			if (!IsStullerProductServicesAvailable())
			{
				if (!runCheckSilent)
				{
					ShowReducedFunctionalityModeMessage();
				}
				ReducedFunctionalityModeActive = true;
			}
			if (reducedFunctionalityModeActive && !ReducedFunctionalityModeActive)
			{
				GlobalErrorHandler.WriteInfoToEventLog("Access to Stuller Services Restored.");
			}
			DebuggingHelper.PrintTimedOutput("IsInternetAccessible End");
			flag = !ReducedFunctionalityModeActive;
		}
		_lastInternetCheckResult = flag;
		if (flag)
		{
		}
		return true;
	}

	public static bool IsSecurityServicesAvailable()
	{
		//Discarded unreachable code: IL_000e, IL_0038
		return true;
	}

	public static bool IsTouchEnabled()
	{
		return GetSystemMetrics(149) > 0;
	}

	public static void OpenAccountWindow(bool promptAuthentication)
	{
		if (promptAuthentication)
		{
			SecurityHelper.ReasonForPrompt = "Authentication required for Account Info";
			SecurityHelper.AuthenticationMode = SecurityHelper.LoginMode.NormalMode;
			SecurityHelper.PromptForAuthentication();
		}
		else
		{
			SecurityHelper.IsAuthenticated = true;
		}
		if (SecurityHelper.IsAuthenticated)
		{
			WindowModalAccount windowModalAccount = new WindowModalAccount();
			windowModalAccount.Owner = Application.Current.MainWindow;
			windowModalAccount.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			windowModalAccount.ShowDialog();
		}
	}

	public static void OpenApplicationNoticeWindow(int noticeId)
	{
		new WindowBrowser(new Uri($"http://countersketch.biz/ApplicationNotices/{noticeId}/Default.aspx")).ShowDialog();
	}

	public static void PromptToForceCloseApplication()
	{
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			WindowMessage obj = new WindowMessage
			{
				Message = "The modeling engine has failed unexpectedly.  \n\nCounterSketch will be closed to prevent any further undesired behaviors. \nPlease submit a debug report if you continue to experience issues.",
				Owner = Application.Current.MainWindow,
				Icon = Application.Current.MainWindow.Icon
			};
			ToggleHardwareRenderingForWindow(obj, CounterSketchOptions.DisableHardwareRendering);
			obj.ShowDialog();
			obj.BringWindowToTop();
			obj.Focus();
			Application.Current.MainWindow.Close();
		});
	}

	public static void RaiseApplicationMessageEvent(object sender, ApplicationMessageType messageType, string message, bool messageIsModal)
	{
		if (UIHelper.ApplicationMessageEventOccurred != null)
		{
			UIHelper.ApplicationMessageEventOccurred(sender, new ApplicationMessageEventArgs(messageType, message, messageIsModal));
		}
	}

	public static void RaiseMaterialsUpdatedEvent()
	{
		if (UIHelper.MaterialsUpdatedEvent != null)
		{
			UIHelper.MaterialsUpdatedEvent(null, new EventArgs());
		}
	}

	public static void ResetInternetCheckFlag()
	{
		_lastInternetCheckResult = false;
	}

	public static void SetApplicationPaths()
	{
		GlobalErrorHandler.WriteMethodInfoOverride("SetApplicationPaths");
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Gemvision\\CounterSketch Studio");
		if (SecurityHelper.IsStreaming)
		{
			GlobalErrorHandler.WriteInfoToEventLog("%Username% = " + Environment.UserName + ", LoggedOnAs = " + WindowsIdentity.GetCurrent()?.Name);
			string userName = Environment.UserName;
			CSDataRootPath = "C:\\Users\\" + userName + "\\Gemvision\\CounterSketch Studio";
			DefaultCSProjectPath = "C:\\Users\\" + userName + "\\My Files\\Home Folder\\Projects";
			CSProjectPath = DefaultCSProjectPath;
		}
		else
		{
			CSDataRootPath = Path.Combine(text);
			DefaultCSProjectPath = Path.Combine(CSDataRootPath, "Projects");
		}
		CSModelLibraryCSGL = Path.Combine(text, "LibraryCSGL_V2");
		CSLibraryDatabasePath = Path.Combine(text, "CounterSketchLibraryData.s3db");
		ZBinFileName = Path.Combine(text, "Library.zbin");
		ZBinFileNameNew = Path.Combine(text, "LibraryNew.zbin");
		CSUpdatesPath = Path.Combine(CSDataRootPath, "Updates");
		CSQuotesPath = Path.Combine(CSDataRootPath, "Quotes");
		CSPrototypeOrdersPath = Path.Combine(CSDataRootPath, "PrototypeOrders");
		CSFeaturedDesign = Path.Combine(CSDataRootPath, "Featured_Design.png");
		CSThemePath = Path.Combine(CSDataRootPath, "CustomTheme_8.xaml");
		if (!Directory.Exists(CSDataRootPath))
		{
			Directory.CreateDirectory(CSDataRootPath);
		}
		if (!Directory.Exists(CSModelLibraryCSGL))
		{
			Directory.CreateDirectory(CSModelLibraryCSGL);
		}
		if (!Directory.Exists(CSUpdatesPath))
		{
			Directory.CreateDirectory(CSUpdatesPath);
		}
		if (!Directory.Exists(CSQuotesPath))
		{
			Directory.CreateDirectory(CSQuotesPath);
		}
		if (!Directory.Exists(CSPrototypeOrdersPath))
		{
			Directory.CreateDirectory(CSPrototypeOrdersPath);
		}
		CSApplicationDatabasePath = Path.Combine(CSDataRootPath, "CounterSketchApplicationData.s3db");
	}

	public static void AbortPortfolioSetup()
	{
		if (_cancelSetupTokenSource != null)
		{
			_cancelSetupTokenSource.Cancel();
			_cancelSetupTokenSource.Token.WaitHandle.WaitOne();
			_cancelSetupTokenSource = null;
		}
	}

	public static async Task ValidateProjectPath()
	{
		if (_cancelSetupTokenSource != null)
		{
			_cancelSetupTokenSource.Cancel();
			_cancelSetupTokenSource.Token.WaitHandle.WaitOne();
			_cancelSetupTokenSource = null;
		}
		_cancelSetupTokenSource = new CancellationTokenSource();
		await Task.Run(delegate
		{
			CreateProjectPathAsync(CSProjectPath, _cancelSetupTokenSource.Token).Wait();
		}, _cancelSetupTokenSource.Token).ConfigureAwait(continueOnCapturedContext: false);
	}

	private static async Task CreateProjectPathAsync(string path, CancellationToken cancellationToken)
	{
		int createProjectPathRetryCount = 0;
		while (!Directory.Exists(path))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				GlobalErrorHandler.WriteInfoToEventLog("Shutting Down, Aborting Project Directory Creation.");
				break;
			}
			if (createProjectPathRetryCount > 30)
			{
				GlobalErrorHandler.WriteInfoToEventLog("Unable to create Project Path after 30 seconds, Aborting.");
				break;
			}
			try
			{
				createProjectPathRetryCount++;
				Directory.CreateDirectory(path);
			}
			catch (Exception arg)
			{
				GlobalErrorHandler.WriteInfoToEventLog($"Exception Creating Project Folder: {arg}");
				GlobalErrorHandler.WriteInfoToEventLog("Project Path Not Yet Available, Waiting 2 seconds to retry.");
				await Task.Delay(2000);
			}
		}
	}

	public static void ShowReducedFunctionalityModeMessage()
	{
		WindowMessage windowMessage = new WindowMessage
		{
			Width = 550.0,
			Message = "CounterSketch cannot access the services of www.Stuller.com.  Please make certain you do not have a firewall blocking access to any of the following addresses: www.Stuller.com, countersketch.biz.\r\n\r\nWhile this connectivity is broken, CounterSketch will continue to run but with reduced functionality.\r\n\r\nIf you believe your connection is working please contact CounterSketch customer service for assistance in resolving your problem.\r\n\r\nTelephone Numbers:\r\nMonday - Friday, 8:30 a.m. - 5:30 p.m. CST.\r\nToll Free Support: 888.357.6272\r\nemail: countersketch@gemvision.com"
		};
		if (Application.Current != null && Application.Current.MainWindow != null)
		{
			windowMessage.Owner = Application.Current.MainWindow;
		}
		windowMessage.BringWindowToTop();
		windowMessage.ShowInTaskbar = true;
		windowMessage.WindowStartupLocation = WindowStartupLocation.CenterScreen;
		windowMessage.ShowDialog();
	}

	public static void ShowSecurityServicesUnavailableMessage()
	{
		WindowMessage windowMessage = new WindowMessage
		{
			Width = 550.0,
			Message = "CounterSketch cannot access the Security Services.  Please make certain you do not have a firewall blocking access to any of the following addresses: countersketch.azurewebsites.net.\r\n\r\nWhile this connectivity is broken CounterSketch will not continue.\r\n\r\nIf you believe your connection is working please contact CounterSketch customer service for assistance in resolving your problem.\r\n\r\nTelephone Numbers:\r\nMonday - Friday, 8:30 a.m. - 5:30 p.m. CST.\r\nToll Free Support: 888.357.6272\r\nemail: countersketch@gemvision.com"
		};
		if (Application.Current != null && Application.Current.MainWindow != null)
		{
			windowMessage.Owner = Application.Current.MainWindow;
		}
		windowMessage.BringWindowToTop();
		windowMessage.ShowInTaskbar = true;
		windowMessage.WindowStartupLocation = WindowStartupLocation.CenterScreen;
		windowMessage.ShowDialog();
	}

	public static void ShowStullerDownMessage()
	{
		WindowMessage windowMessage = new WindowMessage
		{
			Width = 550.0,
			Message = "Stuller.com is currently unavailable. Please try again later.\r\nIf you continue to receive this message after an extended period of time, please contact Stuller customer service for assistance.\r\n\r\nTelephone Numbers:\r\nMonday - Friday, 8:00 a.m. - 6:00 p.m. CST.\r\nToll Free Sales Line: 800.877.7777\r\nBusiness or International Line: 337.262.7700\r\nTechnical Assistance for Web Site: 877.619.2174"
		};
		if (Application.Current != null && Application.Current.MainWindow != null)
		{
			windowMessage.Owner = Application.Current.MainWindow;
		}
		windowMessage.BringWindowToTop();
		windowMessage.ShowInTaskbar = true;
		windowMessage.WindowStartupLocation = WindowStartupLocation.CenterScreen;
		windowMessage.ShowDialog();
	}

	public static void ShowWaitWindow(bool waitVisible, string message = "Please Wait")
	{
		if (message == "Please Wait")
		{
			message = TranslationHelper.GetTranslatedString("Please Wait");
		}
		Application.Current.Dispatcher.Invoke(delegate
		{
			if (waitVisible)
			{
				if (_windowModalWait != null)
				{
					_windowModalWait.SetMessage(message);
				}
				else
				{
					Application.Current.MainWindow.Cursor = Cursors.Wait;
					if (!IsWaitVisible)
					{
						IsWaitVisible = true;
						if (_windowModalWait == null)
						{
							_windowModalWait = new WindowModalWait(message);
							ToggleHardwareRenderingForWindow(_windowModalWait, CounterSketchOptions.DisableHardwareRendering);
							_windowModalWait.ParentWindowHandle = RhinoWindowHelper.RhinoWindowHandle;
							Rectangle windowRectangle = ScreenHelper.GetWindowRectangle(Application.Current.MainWindow);
							_windowModalWait.TopPosition = windowRectangle.Top;
							_windowModalWait.LeftPosition = windowRectangle.Left;
							_windowModalWait.MaxHeight = Application.Current.MainWindow.ActualHeight;
							_windowModalWait.MaxWidth = Application.Current.MainWindow.ActualWidth;
							_windowModalWait.Height = Application.Current.MainWindow.ActualHeight;
							_windowModalWait.Width = Application.Current.MainWindow.ActualWidth;
							_windowModalWait.Owner = Application.Current.MainWindow;
							_windowModalWait.Show();
							_windowModalWait.BringWindowToTop();
						}
					}
				}
			}
			else
			{
				IsWaitVisible = false;
				_windowModalWait = null;
				Application.Current.MainWindow.Cursor = Cursors.Arrow;
			}
		});
	}

	public static bool TextValidation(string regexString, string userString)
	{
		bool result = false;
		if (new Regex(regexString).Match(userString).Success)
		{
			result = true;
		}
		return result;
	}

	public static void ToggleExpect100Continue(bool isDisabled)
	{
		ServicePointManager.Expect100Continue = !isDisabled;
	}

	public static void ToggleHardwareRenderingForWindow(Window window, bool isDisabled)
	{
		if (window != null && PresentationSource.FromVisual(window) is HwndSource { CompositionTarget: { } compositionTarget })
		{
			compositionTarget.RenderMode = (isDisabled ? RenderMode.SoftwareOnly : RenderMode.Default);
		}
	}

	private static BitmapImage GetRenderShadowImage(int componentTypeId)
	{
		string text = null;
		switch (componentTypeId)
		{
		case 0:
		case 1:
		case 2:
		case 3:
			text = "pack://application:,,,/CounterSketch Studio;component/Images/RenderShadow.png";
			break;
		case 4:
			text = "pack://application:,,,/CounterSketch Studio;component/Images/RenderShadowPendant.png";
			break;
		}
		BitmapImage bitmapImage = new BitmapImage();
		try
		{
			bitmapImage.BeginInit();
			bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
			if (text != null)
			{
				bitmapImage.UriSource = new Uri(text);
			}
			bitmapImage.DecodePixelWidth = 240;
			bitmapImage.EndInit();
			bitmapImage.Freeze();
		}
		catch (Exception)
		{
		}
		return bitmapImage;
	}

	private static bool IsStullerProductServicesAvailable()
	{
		//Discarded unreachable code: IL_000c, IL_00b3
		return true;
	}

	private static void SetProjectPathFromUserSettings()
	{
		string userDefinedPortfolioPath = CounterSketchOptions.UserDefinedPortfolioPath;
		if (!Directory.Exists(userDefinedPortfolioPath))
		{
			CSProjectPath = DefaultCSProjectPath;
			CounterSketchOptions.UserDefinedPortfolioPath = DefaultCSProjectPath;
			CounterSketchOptions.WriteSettings();
			if (!string.IsNullOrWhiteSpace(CounterSketchOptions.UserDefinedPortfolioPath))
			{
				GlobalErrorHandler.WriteInfoToEventLog("User has changed Portfolio Location to: " + CounterSketchOptions.UserDefinedPortfolioPath);
			}
		}
		else
		{
			CSProjectPath = userDefinedPortfolioPath;
		}
	}
}
