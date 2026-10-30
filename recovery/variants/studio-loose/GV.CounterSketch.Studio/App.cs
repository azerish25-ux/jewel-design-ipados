using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio;

public class App : Application, IComponentConnector
{
	private bool _contentLoaded;

	public App()
	{
		InitializeComponent();
		if (!ViewModelBase.IsInDesignModeStatic)
		{
			InitializeCultures();
			base.DispatcherUnhandledException += Application_DispatcherUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
		}
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		PresentationTraceSources.Refresh();
		PresentationTraceSources.DataBindingSource.Listeners.Add(new DebugTraceListener());
		PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning;
		base.OnStartup(e);
	}

	private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		if (e.Exception is FaultException)
		{
			GlobalErrorHandler.WriteWarningToEventLog("Server Side Fault Exception: \n\n" + e.Exception);
			e.Handled = true;
		}
		else if (e.Exception is CommunicationObjectFaultedException)
		{
			GlobalErrorHandler.WriteWarningToEventLog("Server Side Fault Exception: \n\n" + e.Exception);
			e.Handled = true;
		}
		else
		{
			GlobalErrorHandler.HandleException(sender, e.Exception);
			e.Handled = true;
		}
	}

	private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		GlobalErrorHandler.HandleException(sender, (Exception)e.ExceptionObject);
	}

	private static void InitializeCultures()
	{
		if (!string.IsNullOrEmpty(UIHelper.CounterSketchOptions.Culture))
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo(UIHelper.CounterSketchOptions.Culture);
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(UIHelper.CounterSketchOptions.Culture);
		}
		FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag)));
	}

	private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
	{
		Application.Current?.Dispatcher?.InvokeAsync(delegate
		{
			GlobalErrorHandler.WriteErrorToEventLog(e.Exception.ToString());
			EventHelper.RecordImmediateEvent(EventHelper.EventTypes.Error, e.Exception.ToString(), sender.ToString());
		});
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/app.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		_contentLoaded = true;
	}
}
