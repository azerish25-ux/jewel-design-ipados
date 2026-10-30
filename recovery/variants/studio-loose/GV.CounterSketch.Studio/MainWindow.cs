using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using GV.CounterSketch.Common;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.View;
using GV.CounterSketch.Studio.ViewModel;
using GV.CounterSketchServices.WebServices;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Localization;
using Microsoft.Expression.Interactivity.Media;
using Telerik.Windows.Controls;

namespace GV.CounterSketch.Studio;

public class MainWindow : Window, IComponentConnector
{
	private static readonly DispatcherTimer _newDesignImageTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);

	private static DispatcherTimer _timedBackupTimer;

	private static readonly DispatcherTimer _updateViewportBackgroundImageTimer = new DispatcherTimer(TimeSpan.FromSeconds(1.0), DispatcherPriority.Background, UpdateViewportBackgroundImage, Dispatcher.CurrentDispatcher);

	private DispatcherTimer _noticeTimer;

	private WindowMessage _windowAlert;

	private WindowMessage _windowDbUpdate;

	private WindowMessage _windowInfo;

	private OrderEstimateDebugWindow _windowOrderEstimateDebug;

	private QuickEstimateDebugWindow _windowQuickEstimateDebug;

	internal Grid GridMain;

	internal ScaleTransform EntireLayoutScaleTransform;

	internal ControlStoryboardAction ScrollSectionAnimation;

	internal ControlStoryboardAction StartSaveAnimation;

	internal ControlStoryboardAction StopSaveAnimation;

	internal Border DragBorder;

	internal ProgramMenuPopup MainMenuPopup;

	internal StackPanel TItleBarControls;

	internal StackPanel AreaSearchBox;

	internal System.Windows.Controls.TextBox RadWatermarkTextBox;

	internal StackPanel AreaWindowControls;

	internal System.Windows.Controls.Button ButtonMinimize;

	internal System.Windows.Controls.Button ButtonRestore;

	internal System.Windows.Controls.Button ButtonMaximize;

	internal System.Windows.Controls.Button ButtonClose;

	internal DockPanel PortfolioControls;

	internal System.Windows.Controls.Button buttonSave;

	internal DockPanel PanelPortfolio;

	internal TextBlock TextBlock;

	internal System.Windows.Controls.ComboBox ComboBoxPortfolio;

	internal StackPanel StackPanel;

	internal RadSlider SldUserInterfaceScaleFactor;

	internal Grid GridBackup;

	internal TextBlock TxtBackupProgress;

	internal System.Windows.Controls.ProgressBar ProgressBackup;

	internal Grid GrdUpdating;

	internal Viewbox ViewBoxIsKeyInUse;

	internal Grid DebugInfoText;

	private bool _contentLoaded;

	public MainWindow()
	{
		InitializeComponent();
		Hide();
		Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata
		{
			DefaultValue = 120
		});
		if (UIHelper.CounterSketchOptions.UserTypeId > 7)
		{
			DebugInfoText.Visibility = Visibility.Visible;
		}
		if (!ViewModelBase.IsInDesignModeStatic)
		{
			base.Loaded += MainWindow_Loaded;
			base.ContentRendered += MainWindow_ContentRendered;
			base.Closing += MainWindow_Closing;
			base.Closed += MainWindow_Closed;
			base.StateChanged += MainWindow_StateChanged;
			base.Activated += MainWindow_Activated;
			base.Deactivated += MainWindow_Deactivated;
		}
		if (SecurityHelper.IsStreaming)
		{
			ButtonClose.Visibility = Visibility.Collapsed;
			ButtonMinimize.Visibility = Visibility.Collapsed;
			ButtonMaximize.Visibility = Visibility.Collapsed;
			ButtonRestore.Visibility = Visibility.Collapsed;
		}
	}

	public void UpdateTimedBackupTimer(int seconds)
	{
		if (!SecurityHelper.IsStreaming && _timedBackupTimer != null)
		{
			_timedBackupTimer.Stop();
			_timedBackupTimer.Interval = new TimeSpan(0, 0, 0, seconds);
			_timedBackupTimer.Start();
		}
	}

	private void MaximizeWindowFixGlitch()
	{
		base.WindowStyle = WindowStyle.SingleBorderWindow;
		base.WindowState = WindowState.Maximized;
		base.WindowStyle = WindowStyle.None;
	}

	private void ButtonCloseClick(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void ButtonMaximize_OnClick(object sender, RoutedEventArgs e)
	{
		MaximizeWindowFixGlitch();
	}

	private void ButtonMinimize_OnClick(object sender, RoutedEventArgs e)
	{
		base.WindowState = WindowState.Minimized;
	}

	private void ButtonRestore_OnClick(object sender, RoutedEventArgs e)
	{
		base.WindowState = WindowState.Normal;
	}

	private void ComboBoxPortfolio_OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
	{
		if (e.Key == Key.Return)
		{
			TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
			if (Keyboard.FocusedElement is UIElement uIElement)
			{
				uIElement.MoveFocus(request);
			}
			e.Handled = true;
		}
	}

	private void ComboBoxPortfolio_OnMouseUp(object sender, MouseButtonEventArgs e)
	{
		if (ComboBoxPortfolio.Template.FindName("PART_EditableTextBox", ComboBoxPortfolio) is System.Windows.Controls.TextBox textBox && !ComboBoxPortfolio.IsDropDownOpen)
		{
			textBox.SelectAll();
			textBox.Focus();
		}
	}

	private void DragBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (!SecurityHelper.IsStreaming)
		{
			DragMove();
		}
	}

	private void DragBorder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (!SecurityHelper.IsStreaming && e.ClickCount == 2)
		{
			if (base.WindowState != WindowState.Maximized)
			{
				base.WindowState = WindowState.Maximized;
			}
			else
			{
				base.WindowState = WindowState.Normal;
			}
		}
	}

	[DebuggerStepThrough]
	private void MainWindow_Activated(object sender, EventArgs e)
	{
		_newDesignImageTimer?.Start();
		_noticeTimer?.Start();
	}

	private void MainWindow_Closed(object sender, EventArgs e)
	{
		UIHelper.CoreDataService.Dispose();
		GC.Collect(0);
	}

	private void MainWindow_Closing(object sender, CancelEventArgs e)
	{
		EventHelper.FlushEvents();
		EventHelper.RecordImmediateEvent(EventHelper.EventTypes.Power_Down);
		UIHelper.AbortPortfolioSetup();
		if (SecurityHelper.IsStreaming)
		{
			e.Cancel = true;
		}
		else if (UIHelper.AllowApplicationClose)
		{
			if (base.WindowState == WindowState.Minimized)
			{
				base.WindowState = WindowState.Maximized;
			}
			UIHelper.IsApplicationClosing = true;
			if (ProjectManagementHelper.PromptForPortfolioSave())
			{
				if (ProjectManagementHelper.IsTempProjectDirty())
				{
					ProjectManagementHelper.DeleteProject(ProjectManagementHelper.GlobalProjectCollection.FirstOrDefault((Project _) => _.Name == ProjectManagementHelper.TempProjectName));
				}
				try
				{
					ViewModelLocator.Cleanup();
					Dispatcher.ExitAllFrames();
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
			e.Cancel = true;
		}
		else
		{
			e.Cancel = true;
			if (BackupHelper.IsBackingUp)
			{
				UIHelper.RaiseApplicationMessageEvent("BackupHelper", ApplicationMessageType.MultiLineAlert, Strings.UnableToCloseBecauseBackupRunning, messageIsModal: false);
			}
		}
	}

	private void MainWindow_ContentRendered(object sender, EventArgs e)
	{
		Task.Run(delegate
		{
			StullerProductAPIHelper.InitializeGlobalFilterCollection();
		});
		UIHelper.CoreDataService.SetKeepAliveProcessIds(Process.GetCurrentProcess().Id);
		Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Merging_Rhino);
		GlobalErrorHandler.WriteInfoToEventLog(string.Format(Strings.Merging_Rhino), 2);
		if (!RhinoWindowHelper.ReparentRhino(this))
		{
			Messenger.Default.Send<string, WindowCounterSketchSplash>("Kill");
			System.Windows.MessageBox.Show(Strings.UnableToReparentRhino);
			Close();
			return;
		}
		RhinoWindowHelper.RhinoPerspectiveViewWindow = UIHelper.CoreDataService.RhinoGetActiveViewWindowHandle();
		UIHelper.CoreDataService.InitializeRhinoProperties(UIHelper.CSLibraryPath, UIHelper.CSRootPath);
		UIHelper.CoreDataService.ToggleQuarterCaratGemRounding(UIHelper.CounterSketchOptions.IsQuarterCaratLargeStoneRoundingEnabled);
		Win32Utility.AllowSetForegroungWindowSetProcessId(-1);
		Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Begin_Update_Checks);
		GlobalErrorHandler.WriteInfoToEventLog("Building the UI - Begin Update Checks.", 2);
		if (SecurityHelper.IsStreaming)
		{
			UpdateHelper.MinimumUpdate();
		}
		else if (!UpdateHelper.UpdateAll())
		{
			Messenger.Default.Send<string, WindowCounterSketchSplash>("Kill");
			Close();
			return;
		}
		ViewModelLocator.Current.TileViewDesignsVM.Populate();
		ViewModelLocator.Current.TileViewPartsVM.Populate();
		Task.Factory.StartNew(delegate
		{
			ViewModelLocator.Current.TileSearchMainVM.TileSearchResultPartsVM.ResetFilters();
		});
		Task.Factory.StartNew(delegate
		{
			ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.ResetFilters();
		});
		UIHelper.SkipInternetWarning = false;
		if (UIHelper.CounterSketchOptions.UserTypeId > 7)
		{
			ViewModelLocator.Current.MainVM.ShowAdminMenu = true;
		}
		ViewModelLocator.Current.MainVM.IsStreaming = SecurityHelper.IsStreaming;
		if (UIHelper.CounterSketchOptions.RingSizeSetting != null && UIHelper.CounterSketchOptions.RingSizeSetting.Length > 1)
		{
			UIHelper.CoreDataService.DocumentSetRingSizeSetting(UIHelper.CounterSketchOptions.RingSizeSetting);
		}
		if (!UIHelper.CounterSketchOptions.IsFirstTimeRun && UIHelper.ReducedFunctionalityModeActive)
		{
			if (!SecurityHelper.ValidateForReducedFunctionalityMode())
			{
				Close();
			}
		}
		else if (!UIHelper.CounterSketchOptions.IsFirstTimeRun && !SecurityHelper.ValidateStoredCredentials())
		{
			Close();
		}
		UIHelper.ShowWaitWindow(waitVisible: false);
		UIHelper.IsApplicationInitialized = true;
		base.WindowState = WindowState.Maximized;
		Show();
		Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Ready);
		GlobalErrorHandler.WriteInfoToEventLog(string.Format(Strings.Ready), 2);
		if (UIHelper.CounterSketchOptions.IsFirstTimeRun || !UIHelper.CounterSketchOptions.HasStullerShowcaseMarkups)
		{
			UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.SingleLineAlert, Strings.YourSerialCodeHasBeenAssigned, messageIsModal: true);
			UIHelper.OpenAccountWindow(promptAuthentication: false);
		}
		if (UIHelper.CounterSketchOptions.ApplicationCurrentVersion != UIHelper.VersionString && UIHelper.KeyProfile.KeyProfileId != 2)
		{
			EventHelper.RecordEvent(EventHelper.EventTypes.Application_VersionChanged, "Old:New", $"{UIHelper.CounterSketchOptions.ApplicationCurrentVersion}:{UIHelper.VersionString}");
			GlobalErrorHandler.WriteInfoToEventLog($"Version Changed - Old:{UIHelper.CounterSketchOptions.ApplicationCurrentVersion},New:{UIHelper.VersionString}");
			UIHelper.CounterSketchOptions.ApplicationCurrentVersion = UIHelper.VersionString;
			UIHelper.CounterSketchOptions.WriteSettings();
			ViewModelLocator.Current.MainVM.OpenReleaseNotesWindow();
		}
		_noticeTimer = new DispatcherTimer(new TimeSpan(0, 0, 10, 0), DispatcherPriority.Background, NoticeTimerElapsed, base.Dispatcher);
		_noticeTimer.Start();
		if (LoadHelper.UrlProtocolRequestedDesign != null)
		{
			LoadHelper.ExtractAndLoadDesignFromURL();
			return;
		}
		if (!string.IsNullOrWhiteSpace(LoadHelper.DoubleClickFileOpenRequest))
		{
			if (ProjectManagementHelper.ContinueAfterSaveDesignPrompt())
			{
				LoadHelper.LoadDesign(LoadHelper.DoubleClickFileOpenRequest);
			}
			LoadHelper.DoubleClickFileOpenRequest = null;
		}
		ProjectManagementHelper.IsFileModified = false;
		if (!string.IsNullOrWhiteSpace(UIHelper.CounterSketchOptions.ViewportGraphicPath) && File.Exists(UIHelper.CounterSketchOptions.ViewportGraphicPath))
		{
			ViewModelLocator.Current.ThemePickerVM.ViewportBitmapStretchedChanged();
		}
	}

	[DebuggerStepThrough]
	private void MainWindow_Deactivated(object sender, EventArgs e)
	{
		_newDesignImageTimer?.Stop();
		_noticeTimer?.Stop();
	}

	[DebuggerStepThrough]
	private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
	{
		if (e.Key == Key.Escape)
		{
			ViewModelLocator.Current.MainVM.NavigateToSection(Sections.Tiles);
		}
		if (Keyboard.Modifiers.HasFlag(ModifierKeys.Windows) && SecurityHelper.IsStreaming)
		{
			e.Handled = true;
		}
		else
		{
			if (!Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
			{
				return;
			}
			Key key = e.Key;
			if (key <= Key.S)
			{
				switch (key)
				{
				case Key.I:
					if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && FeatureHelper.HasFeature(Features.ShowDebugFeatures.ToString()))
					{
						ViewModelLocator.Current.DebugDesignInfoVM.ShowDesignInfo();
					}
					break;
				case Key.D:
					if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
					{
						if (UIHelper.CounterSketchOptions.UserTypeId > 6)
						{
							if (_windowQuickEstimateDebug == null)
							{
								_windowQuickEstimateDebug = new QuickEstimateDebugWindow
								{
									Topmost = true
								};
								_windowQuickEstimateDebug.Show();
							}
							else if (!_windowQuickEstimateDebug.IsVisible)
							{
								_windowQuickEstimateDebug.Visibility = Visibility.Visible;
							}
						}
					}
					else if (UIHelper.CounterSketchOptions.UserTypeId > 6)
					{
						if (_windowOrderEstimateDebug == null)
						{
							_windowOrderEstimateDebug = new OrderEstimateDebugWindow
							{
								Topmost = true
							};
							_windowOrderEstimateDebug.Show();
						}
						else if (!_windowOrderEstimateDebug.IsVisible)
						{
							_windowOrderEstimateDebug.Visibility = Visibility.Visible;
						}
					}
					break;
				case Key.S:
					ViewModelLocator.Current.MainVM.SaveCurrentDesign();
					break;
				}
			}
			else if (key <= Key.Z)
			{
				switch (key)
				{
				case Key.Z:
					ViewModelLocator.Current.MainVM.Undo();
					break;
				case Key.Y:
					ViewModelLocator.Current.MainVM.Redo();
					break;
				}
			}
			else if (key != Key.F1)
			{
				_ = 99;
			}
		}
	}

	private void MainWindow_Loaded(object sender, RoutedEventArgs e)
	{
		base.WindowStyle = WindowStyle.SingleBorderWindow;
		base.WindowState = WindowState.Maximized;
		base.WindowStyle = WindowStyle.None;
		UIHelper.ShowWaitWindow(waitVisible: true);
		if (!SecurityHelper.IsStreaming)
		{
			SecurityHelper.Initialize();
		}
		Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Wiring_Events);
		GlobalErrorHandler.WriteInfoToEventLog("Building the UI - Wiring Events.", 2);
		if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
		{
			UIHelper.CounterSketchOptions.DebugMode = true;
		}
		UIHelper.ToggleHardwareRenderingForWindow(this, UIHelper.CounterSketchOptions.DisableHardwareRendering);
		UIHelper.ToggleExpect100Continue(UIHelper.CounterSketchOptions.DisableExpect100Continue);
		UIHelper.EnableBypassSecurityCertificateValidation();
		if (!BitsHelper.TestBitsFunctionality())
		{
			UIHelper.RaiseApplicationMessageEvent("CompleteTransferredJobs", ApplicationMessageType.MultiLineAlert, Strings.YourBITSServiceIsNotRunning, messageIsModal: true);
			Close();
			return;
		}
		BitsHelper.Initialize();
		UIHelper.ApplicationMessageEventOccurred += OnApplicationMessageEventOccurredEvent;
		base.MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
		base.MouseLeave += MainWindow_MouseLeave;
		base.KeyDown += MainWindow_KeyDown;
		base.MouseDown += MainWindow_MouseDown;
		ViewModelLocator.Current.MainVM.IsSearchModeActive = true;
		Messenger.Default.Send<CounterSketchOptions>(UIHelper.CounterSketchOptions);
		SetUIScaleFactor();
		Messenger.Default.Send<string, WindowCounterSketchSplash>(UntranslatedStrings.MountingPortfolioStorage);
		UIHelper.ValidateProjectPath().GetAwaiter().GetResult();
		ProjectManagementHelper.ResetProjectManagement();
		ViewModelLocator.Current.UserPartsVM.Populate();
		int num = 10;
		_newDesignImageTimer.Interval = TimeSpan.FromSeconds(num);
		_newDesignImageTimer.Tick += NewDesignImageTimer_Tick;
		_newDesignImageTimer.Start();
		if (!SecurityHelper.IsStreaming)
		{
			_timedBackupTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, UIHelper.CounterSketchOptions.TimedBackupSeconds), DispatcherPriority.Background, TimedBackupTimerOnTick, base.Dispatcher);
			_timedBackupTimer.Start();
		}
		Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Window_is_Loaded);
		GlobalErrorHandler.WriteInfoToEventLog("Building the UI - Window is Loaded.", 2);
	}

	[DebuggerStepThrough]
	private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if (e.ChangedButton.Equals(MouseButton.XButton1))
		{
			ViewModelLocator.Current.TileSearchMainVM.Reset();
		}
		e.ChangedButton.Equals(MouseButton.XButton2);
	}

	[DebuggerStepThrough]
	private void MainWindow_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
	{
		if (UIHelper.IsRhinoVisible && !this.FindChildren<Popup>().Any((Popup findChild) => findChild.IsOpen) && !(Keyboard.FocusedElement is System.Windows.Controls.TextBox))
		{
			if (Win32Utility.WindowFromPoint(new Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y)) == RhinoWindowHelper.RhinoPerspectiveViewWindow)
			{
				UIHelper.FocusRequestDisabled = false;
				Win32Utility.SetWindowToForeground(RhinoWindowHelper.RhinoWindowHandle);
			}
			else
			{
				UIHelper.FocusRequestDisabled = true;
			}
		}
	}

	[DebuggerStepThrough]
	private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (ViewModelLocator.Current.MainVM.IsMenuToggleButtonChecked && !MainMenuPopup.IsMouseOver)
		{
			ViewModelLocator.Current.MainVM.IsMenuToggleButtonChecked = false;
			Messenger.Default.Send<string, ProgramMenuPopup>("ResetMenuViewState");
		}
	}

	private void MainWindow_StateChanged(object sender, EventArgs e)
	{
		ViewModelLocator.Current.ThemePickerVM.ViewportBitmapStretchedChanged();
		if (!SecurityHelper.IsStreaming)
		{
			if (base.WindowState == WindowState.Maximized)
			{
				ButtonRestore.Visibility = Visibility.Visible;
				ButtonMaximize.Visibility = Visibility.Collapsed;
				base.WindowStyle = WindowStyle.SingleBorderWindow;
				base.WindowStyle = WindowStyle.None;
			}
			if (base.WindowState == WindowState.Normal)
			{
				ButtonRestore.Visibility = Visibility.Collapsed;
				ButtonMaximize.Visibility = Visibility.Visible;
			}
		}
	}

	[DebuggerStepThrough]
	private void NewDesignImageTimer_Tick(object sender, EventArgs e)
	{
		if (UIHelper.KeyProfile.KeyProfileId == 2)
		{
			_newDesignImageTimer.Stop();
		}
		else if (CSLibraryHelper.IsInitialized && ViewModelLocator.Current.MainVM.SectionPageNumber == 0)
		{
			UIHelper.GenerateNewDesignTileImages();
		}
	}

	[DebuggerStepThrough]
	private void NoticeTimerElapsed(object sender, EventArgs e)
	{
		if (SecurityHelper.IsStreaming || !UIHelper.IsInternetAccessible(runCheckSilent: true))
		{
			return;
		}
		try
		{
			using CounterSketchService counterSketchService = new CounterSketchService();
			counterSketchService.GetLatestNoticeByKeyIdCompleted += svc_GetLastNoticeIdCompleted;
			counterSketchService.GetLatestNoticeByKeyIdAsync(UIHelper.CounterSketchOptions.KeyId);
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Error Occurred Checking for Notices: " + ex);
		}
	}

	[DebuggerStepThrough]
	private void OnApplicationMessageEventOccurredEvent(object sender, ApplicationMessageEventArgs e)
	{
		Messenger.Default.Send<string, WindowCounterSketchSplash>("Hide");
		UIHelper.ShowWaitWindow(waitVisible: false);
		base.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
		{
			switch (e.AlertMessageType)
			{
			case ApplicationMessageType.SingleLineInfo:
				GlobalErrorHandler.WriteInfoToEventLog(e.AlertMessageText, 2);
				_windowInfo = new WindowMessage
				{
					Message = e.AlertMessageText,
					Owner = Window.GetWindow(this),
					Icon = base.Icon
				};
				UIHelper.ToggleHardwareRenderingForWindow(_windowInfo, UIHelper.CounterSketchOptions.DisableHardwareRendering);
				if (e.ModalMessage)
				{
					_windowInfo.ShowDialog();
				}
				else
				{
					_windowInfo.Show();
				}
				_windowInfo.BringWindowToTop();
				_windowInfo.Focus();
				break;
			case ApplicationMessageType.MultiLineInfo:
				GlobalErrorHandler.WriteInfoToEventLog(e.AlertMessageText, 2);
				_windowInfo = new WindowMessage
				{
					Message = e.AlertMessageText,
					Owner = Window.GetWindow(this),
					Icon = base.Icon
				};
				UIHelper.ToggleHardwareRenderingForWindow(_windowInfo, UIHelper.CounterSketchOptions.DisableHardwareRendering);
				if (e.ModalMessage)
				{
					_windowInfo.ShowDialog();
				}
				else
				{
					_windowInfo.Show();
				}
				_windowInfo.BringWindowToTop();
				_windowInfo.Focus();
				break;
			case ApplicationMessageType.SingleLineAlert:
				GlobalErrorHandler.WriteInfoToEventLog(e.AlertMessageText, 2);
				_windowAlert = new WindowMessage
				{
					Message = e.AlertMessageText,
					Owner = Window.GetWindow(this),
					Icon = base.Icon
				};
				UIHelper.ToggleHardwareRenderingForWindow(_windowInfo, UIHelper.CounterSketchOptions.DisableHardwareRendering);
				if (e.ModalMessage)
				{
					_windowAlert.ShowDialog();
				}
				else
				{
					_windowAlert.Show();
				}
				_windowAlert.BringWindowToTop();
				_windowAlert.Focus();
				break;
			case ApplicationMessageType.MultiLineAlert:
				GlobalErrorHandler.WriteInfoToEventLog(e.AlertMessageText, 2);
				_windowAlert = new WindowMessage
				{
					Message = e.AlertMessageText,
					Owner = Window.GetWindow(this),
					Icon = base.Icon
				};
				UIHelper.ToggleHardwareRenderingForWindow(_windowInfo, UIHelper.CounterSketchOptions.DisableHardwareRendering);
				if (e.ModalMessage)
				{
					_windowAlert.ShowDialog();
				}
				else
				{
					_windowAlert.Show();
				}
				_windowAlert.BringWindowToTop();
				_windowAlert.Focus();
				break;
			case ApplicationMessageType.AcceptPrompt:
				GlobalErrorHandler.WriteInfoToEventLog(e.AlertMessageText, 2);
				_windowAlert = new WindowMessage(WindowMessage.WindowType.Confirmation)
				{
					Message = e.AlertMessageText,
					Owner = Window.GetWindow(this),
					Icon = base.Icon
				};
				UIHelper.ToggleHardwareRenderingForWindow(_windowInfo, UIHelper.CounterSketchOptions.DisableHardwareRendering);
				if (e.ModalMessage)
				{
					_windowAlert.ShowDialog();
				}
				else
				{
					_windowAlert.Show();
				}
				_windowAlert.BringWindowToTop();
				_windowAlert.Focus();
				UIHelper.LastMessageBoxAccepted = _windowAlert.IsAccepted;
				UIHelper.LastMessageBoxCancelled = _windowAlert.IsCancelled;
				break;
			case ApplicationMessageType.Debug:
				GlobalErrorHandler.WriteInfoToEventLog(e.AlertMessageText, 2);
				break;
			case ApplicationMessageType.Database:
				GlobalErrorHandler.WriteInfoToEventLog(e.AlertMessageText, 2);
				if (_windowDbUpdate != null && _windowDbUpdate.IsVisible && _windowDbUpdate.IsLoaded)
				{
					if (e.AlertMessageText == "FINISHED")
					{
						UIHelper.ShowWaitWindow(waitVisible: true);
						base.Cursor = System.Windows.Input.Cursors.Arrow;
						_windowDbUpdate.Close();
					}
					else
					{
						_windowDbUpdate.Message += e.AlertMessageText;
						_windowDbUpdate.Show();
						_windowDbUpdate.Focus();
					}
				}
				else
				{
					_windowDbUpdate = new WindowMessage
					{
						Title = "DATABASE UPDATE",
						Message = e.AlertMessageText,
						ShowInTaskbar = true,
						Owner = Window.GetWindow(this),
						Icon = base.Icon
					};
					UIHelper.ToggleHardwareRenderingForWindow(_windowInfo, UIHelper.CounterSketchOptions.DisableHardwareRendering);
					UIHelper.ShowWaitWindow(waitVisible: false);
					base.Cursor = System.Windows.Input.Cursors.Wait;
					_windowDbUpdate.OKButtonEnabled = false;
					_windowDbUpdate.Show();
					_windowDbUpdate.BringWindowToTop();
					_windowDbUpdate.Focus();
				}
				break;
			case ApplicationMessageType.Exception:
				break;
			}
		});
	}

	private void SetUIScaleFactor()
	{
		EntireLayoutScaleTransform.ScaleX = UIHelper.CounterSketchOptions.UIScaleFactor;
		EntireLayoutScaleTransform.ScaleY = UIHelper.CounterSketchOptions.UIScaleFactor;
		((RangeBase)(object)SldUserInterfaceScaleFactor).Value = UIHelper.CounterSketchOptions.UIScaleFactor;
		ViewModelLocator.Current.MainVM.UIScaleInverseFactor = 1.0 / UIHelper.CounterSketchOptions.UIScaleFactor;
	}

	private void SldUserInterfaceScaleFactorValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		RadSlider val = (RadSlider)((sender is RadSlider) ? sender : null);
		if (val != null && EntireLayoutScaleTransform != null)
		{
			UIHelper.CounterSketchOptions.UIScaleFactor = ((RangeBase)(object)val).Value;
			UIHelper.CounterSketchOptions.WriteSettings();
			SetUIScaleFactor();
		}
	}

	private void Storyboard_Completed(object sender, EventArgs e)
	{
		ViewModelLocator.Current.TileSearchMainVM.IsSearchingDesigns = true;
		if (ViewModelLocator.Current.MainVM.SectionPageNumber == 1 && !ViewModelLocator.Current.TileSearchMainVM.SkipPopulateAfterStorboardCompletes)
		{
			ViewModelLocator.Current.TileSearchMainVM.PopulateSearchResults();
		}
		ViewModelLocator.Current.MainVM.SetSectionTransitionTimeToNormal();
		ViewModelLocator.Current.TileSearchMainVM.SkipPopulateAfterStorboardCompletes = false;
	}

	[DebuggerStepThrough]
	private void svc_GetLastNoticeIdCompleted(object sender, GetLatestNoticeByKeyIdCompletedEventArgs e)
	{
		try
		{
			int result = e.Result;
			if (result > 0)
			{
				UIHelper.OpenApplicationNoticeWindow(result);
			}
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteWarningToEventLog("Error Occurred Checking for Notices: " + ex);
		}
	}

	private void TimedBackupTimerOnTick(object sender, EventArgs eventArgs)
	{
		try
		{
			if (UIHelper.CounterSketchOptions.IsTimedBackupEnabled && !ProjectManagementHelper.IsDocumentCanvasEmpty() && !ViewModelLocator.Current.MainVM.IsSaving && !ViewModelLocator.Current.MainVM.IsRhinoBusy && !UIHelper.IsWaitVisible && !ProjectManagementHelper.IsFileBackedUp)
			{
				GlobalErrorHandler.WriteInfoToEventLog("Performing Timed Backup...", 2);
				ViewModelLocator.Current.MainVM.IsSaving = true;
				Task.Run(delegate
				{
					ProjectManagementHelper.AddDesignOnCanvasToTimedBackupProject(UIHelper.LoadedDesign);
				}).ContinueWith(delegate
				{
					ViewModelLocator.Current.MainVM.IsSaving = false;
				});
			}
		}
		catch (Exception arg)
		{
			GlobalErrorHandler.WriteErrorToEventLog($"Timed Backup Exception: {arg}");
		}
	}

	private static void UpdateViewportBackgroundImage(object sender, EventArgs e)
	{
		ViewModelLocator.Current.ThemePickerVM.ViewportBitmapStretchedChanged();
		_updateViewportBackgroundImageTimer?.Stop();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/mainwindow.xaml", UriKind.Relative);
			System.Windows.Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		switch (connectionId)
		{
		case 1:
			GridMain = (Grid)target;
			break;
		case 2:
			EntireLayoutScaleTransform = (ScaleTransform)target;
			break;
		case 3:
			ScrollSectionAnimation = (ControlStoryboardAction)target;
			break;
		case 4:
			((Storyboard)target).Completed += Storyboard_Completed;
			break;
		case 5:
			StartSaveAnimation = (ControlStoryboardAction)target;
			break;
		case 6:
			StopSaveAnimation = (ControlStoryboardAction)target;
			break;
		case 7:
			DragBorder = (Border)target;
			DragBorder.MouseLeftButtonDown += DragBorder_MouseLeftButtonDown;
			DragBorder.PreviewMouseLeftButtonDown += DragBorder_PreviewMouseLeftButtonDown;
			break;
		case 8:
			MainMenuPopup = (ProgramMenuPopup)target;
			break;
		case 9:
			TItleBarControls = (StackPanel)target;
			break;
		case 10:
			AreaSearchBox = (StackPanel)target;
			break;
		case 11:
			RadWatermarkTextBox = (System.Windows.Controls.TextBox)target;
			break;
		case 12:
			AreaWindowControls = (StackPanel)target;
			break;
		case 13:
			ButtonMinimize = (System.Windows.Controls.Button)target;
			ButtonMinimize.Click += ButtonMinimize_OnClick;
			break;
		case 14:
			ButtonRestore = (System.Windows.Controls.Button)target;
			ButtonRestore.Click += ButtonRestore_OnClick;
			break;
		case 15:
			ButtonMaximize = (System.Windows.Controls.Button)target;
			ButtonMaximize.Click += ButtonMaximize_OnClick;
			break;
		case 16:
			ButtonClose = (System.Windows.Controls.Button)target;
			ButtonClose.Click += ButtonCloseClick;
			break;
		case 17:
			PortfolioControls = (DockPanel)target;
			break;
		case 18:
			buttonSave = (System.Windows.Controls.Button)target;
			break;
		case 19:
			PanelPortfolio = (DockPanel)target;
			break;
		case 20:
			TextBlock = (TextBlock)target;
			break;
		case 21:
			ComboBoxPortfolio = (System.Windows.Controls.ComboBox)target;
			ComboBoxPortfolio.KeyUp += ComboBoxPortfolio_OnKeyUp;
			ComboBoxPortfolio.MouseUp += ComboBoxPortfolio_OnMouseUp;
			break;
		case 22:
			StackPanel = (StackPanel)target;
			break;
		case 23:
			SldUserInterfaceScaleFactor = (RadSlider)target;
			((RangeBase)(object)SldUserInterfaceScaleFactor).ValueChanged += SldUserInterfaceScaleFactorValueChanged;
			break;
		case 24:
			GridBackup = (Grid)target;
			break;
		case 25:
			TxtBackupProgress = (TextBlock)target;
			break;
		case 26:
			ProgressBackup = (System.Windows.Controls.ProgressBar)target;
			break;
		case 27:
			GrdUpdating = (Grid)target;
			break;
		case 28:
			ViewBoxIsKeyInUse = (Viewbox)target;
			break;
		case 29:
			DebugInfoText = (Grid)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
