using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using GemvisionApplicationManager.ApplicationService;
using Hardcodet.Wpf.TaskbarNotification;

namespace GemvisionApplicationManager;

public class App : Application
{
	private static TaskbarIcon _notifyIcon;

	private GemvisionApplicationService _gemvisionApplicationService;

	private bool _contentLoaded;

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);
		_notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
		_gemvisionApplicationService = new GemvisionApplicationService();
		GemvisionApplicationService.ProcessedServiceEventNotification += GemvisionApplicationServiceOnProcessedServiceEventNotification;
		_gemvisionApplicationService.Start();
	}

	private void GemvisionApplicationServiceOnProcessedServiceEventNotification(ServiceEventNotificationArgs eventArgs)
	{
		string title = "Event Notification Processed";
		string message = "Event XXXXX Information...";
		BalloonIcon symbol = BalloonIcon.None;
		if (eventArgs != null)
		{
			title = eventArgs.EventName;
			message = eventArgs.EventInformation;
			switch (eventArgs.EventStatus)
			{
			case ServiceEventStatus.Information:
				symbol = BalloonIcon.Info;
				break;
			case ServiceEventStatus.Error:
				symbol = BalloonIcon.Error;
				break;
			}
		}
		_notifyIcon.ResetBalloonCloseTimer();
		_notifyIcon.ShowBalloonTip(title, message, symbol);
	}

	protected override void OnExit(ExitEventArgs e)
	{
		_gemvisionApplicationService.Stop();
		_notifyIcon.Dispose();
		GemvisionApplicationService.ProcessedServiceEventNotification -= GemvisionApplicationServiceOnProcessedServiceEventNotification;
		base.OnExit(e);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/GemvisionApplicationManager;component/app.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[STAThread]
	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public static void Main()
	{
		App app = new App();
		app.InitializeComponent();
		app.Run();
	}
}
