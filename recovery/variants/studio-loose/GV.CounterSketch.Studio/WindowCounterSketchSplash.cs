using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight.Messaging;
using Localization;

namespace GV.CounterSketch.Studio;

public class WindowCounterSketchSplash : Window, IComponentConnector
{
	public delegate void HasShutDownHandler(object sender, EventArgs args);

	private static string _libraryText = string.Empty;

	public static readonly DependencyProperty NotificationMessageProperty = DependencyProperty.Register("NotificationMessage", typeof(string), typeof(WindowCounterSketchSplash));

	public static readonly DependencyProperty StartupParametersProperty = DependencyProperty.Register("StartupParameters", typeof(List<string>), typeof(WindowCounterSketchSplash));

	internal WindowCounterSketchSplash MainWindow;

	internal Grid Splash;

	internal Image Background_Image;

	internal ContentControl GVLOGO;

	internal StackPanel Copyright;

	internal StackPanel Progress_Bar;

	internal TextBlock Notification_Message;

	internal ProgressBar ProgressBar;

	internal ListBox DebugParametersList;

	private bool _contentLoaded;

	public string NotificationMessage
	{
		get
		{
			return (string)GetValue(NotificationMessageProperty);
		}
		set
		{
			SetValue(NotificationMessageProperty, value);
		}
	}

	public List<string> StartupParameters
	{
		get
		{
			return (List<string>)GetValue(StartupParametersProperty);
		}
		set
		{
			SetValue(StartupParametersProperty, value);
		}
	}

	public int KeyId => UIHelper.CounterSketchOptions.KeyId;

	public string Version => "Version " + UIHelper.VersionString;

	public string VersionServer => $"[{UIHelper.VersionRhinoServer.Build}]";

	public event EventHandler HasShutDown;

	public WindowCounterSketchSplash()
	{
		InitializeComponent();
		Messenger.Default.Register<string>((object)this, (Action<string>)ProcessGenericMessage, false);
		Messenger.Default.Register<NotificationMessage>((object)this, (Action<NotificationMessage>)ProcessLibraryNotificationMessage, false);
		base.Title = UIHelper.KeyProfile.ApplicationName;
		ProgressBar.Foreground = new SolidColorBrush(UIHelper.KeyProfile.HighlightAndAccent);
		if (UIHelper.ShowStartupParameters)
		{
			DebugParametersList.Visibility = Visibility.Visible;
			StartupParameters = UIHelper.StartupParameters;
		}
	}

	internal void ShutDown()
	{
		base.Dispatcher.ShutdownFinished += Dispatcher_ShutdownFinished;
		base.Dispatcher.InvokeShutdown();
	}

	private void Dispatcher_ShutdownFinished(object sender, EventArgs e)
	{
		if (this.HasShutDown != null)
		{
			this.HasShutDown(sender, e);
		}
	}

	private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
	{
	}

	private void ProcessGenericMessage(string message)
	{
		if (message == "Kill")
		{
			base.Dispatcher.Invoke(base.Close);
			return;
		}
		if (message == "Hide")
		{
			base.Dispatcher.Invoke(base.Hide);
			return;
		}
		if (!base.IsVisible)
		{
			base.Dispatcher.Invoke(base.Show);
		}
		int progressValue;
		if (message == Strings.Transmitting_Debug_Report)
		{
			progressValue = 1;
		}
		else if (message == Strings.Debug_Report_Sent)
		{
			progressValue = 5;
		}
		else if (message == Strings.Testing_Connectivity)
		{
			progressValue = 10;
		}
		else if (message == Strings.Disabling_Trouble_Plugins)
		{
			progressValue = 12;
		}
		else if (message == Strings.Starting_Rhino)
		{
			progressValue = 15;
		}
		else if (message == Strings.Connecting_To_Rhino)
		{
			progressValue = 18;
		}
		else if (message == Strings.Building_the_UI)
		{
			progressValue = 20;
		}
		else if (message == Strings.Listening_for_Arguments)
		{
			progressValue = 23;
		}
		else if (message == Strings.Opening_Main_Window)
		{
			progressValue = 24;
		}
		else if (message == Strings.Setting_Theme)
		{
			progressValue = 25;
		}
		else if (message == Strings.Wiring_Events)
		{
			progressValue = 26;
		}
		else if (message == UntranslatedStrings.MountingPortfolioStorage)
		{
			progressValue = 27;
		}
		else if (message == Strings.Window_is_Loaded)
		{
			progressValue = 40;
		}
		else if (message == Strings.Merging_Rhino)
		{
			progressValue = 42;
		}
		else if (message == Strings.Begin_Update_Checks)
		{
			progressValue = 44;
		}
		else if (message == Strings.USB_Key_bypassing_security)
		{
			progressValue = 46;
		}
		else if (message == Strings.Validating_License)
		{
			progressValue = 48;
		}
		else if (message == Strings.Invalid_Serial_Code_detected)
		{
			progressValue = 50;
		}
		else if (message == Strings.Updating_Gem_Database)
		{
			progressValue = 52;
		}
		else if (message == Strings.Checking_for_Application_Updates)
		{
			progressValue = 54;
		}
		else if (message == Strings.Checking_for_Library_Updates)
		{
			progressValue = 56;
		}
		else if (message == Strings.Converting_to_new_Library_This_will_take_some_time)
		{
			progressValue = 58;
		}
		else if (message == Strings.Preparing_Design_Library)
		{
			progressValue = 60;
		}
		else if (message == Strings.Downloading_Updated_Design_Database)
		{
			progressValue = 65;
		}
		else if (message == Strings.Downloading_Missing_Design_Files)
		{
			progressValue = 70;
		}
		else if (message == Strings.Preparing_Part_Library)
		{
			progressValue = 75;
		}
		else if (message == Strings.Sending_Statistics)
		{
			progressValue = 95;
		}
		else if (message == Strings.Precaching_Render_Images)
		{
			progressValue = 96;
		}
		else if (message == Strings.Checking_for_Catalog_Updates)
		{
			progressValue = 97;
		}
		else if (message == Strings.Ready)
		{
			progressValue = 100;
		}
		else
		{
			progressValue = 100;
		}
		base.Dispatcher.Invoke(delegate
		{
			ProgressBar.Value = progressValue;
			NotificationMessage = message;
		});
		if (progressValue == 100)
		{
			Messenger.Default.Unregister<string>((object)this);
			Messenger.Default.Unregister<NotificationMessage>((object)this);
			Thread.Sleep(500);
			base.Dispatcher.InvokeAsync(base.Close);
		}
	}

	private void ProcessLibraryNotificationMessage(NotificationMessage message)
	{
		if (!base.IsVisible)
		{
			base.Dispatcher.Invoke(base.Show);
		}
		base.Dispatcher.Invoke(delegate
		{
			string notification = message.Notification;
			if (base.IsVisible)
			{
				if (notification.ToLower() == Strings.NewLibraryFilesFound.ToLower() || notification.ToLower() == Strings.AddingNewDesigns.ToLower() || notification.ToLower() == Strings.ClearingOldDesigns.ToLower())
				{
					ProgressBar.Value = 60.0;
					NotificationMessage = notification;
				}
				int num = 1;
				if (notification.ToLower().Contains(Strings.Processing_Library_Files.ToLower()))
				{
					int.TryParse(notification.Split('|')[1], out var result);
					num = (int)(95.0 - ProgressBar.Value) / result;
				}
				if (notification.ToLower().Contains(Strings.Processing_Library_Colon.ToLower()))
				{
					ProgressBar.Value += num;
					NotificationMessage = notification;
					_libraryText = notification;
				}
				if (notification.ToLower().Contains(Strings.Extracting_Image))
				{
					NotificationMessage = _libraryText + notification;
				}
			}
		});
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/windowcountersketchsplash.xaml", UriKind.Relative);
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
			MainWindow = (WindowCounterSketchSplash)target;
			MainWindow.MouseDown += Grid_MouseDown;
			break;
		case 2:
			Splash = (Grid)target;
			break;
		case 3:
			Background_Image = (Image)target;
			break;
		case 4:
			GVLOGO = (ContentControl)target;
			break;
		case 5:
			Copyright = (StackPanel)target;
			break;
		case 6:
			Progress_Bar = (StackPanel)target;
			break;
		case 7:
			Notification_Message = (TextBlock)target;
			break;
		case 8:
			ProgressBar = (ProgressBar)target;
			break;
		case 9:
			DebugParametersList = (ListBox)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
