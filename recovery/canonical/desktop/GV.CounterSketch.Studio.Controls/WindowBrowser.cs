using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CefSharp;
using CefSharp.Wpf;
using GV.CounterSketch.Studio.Utilities.Behaviors;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.Controls;

public class WindowBrowser : Window, IComponentConnector
{
	private Uri _gotoUri;

	internal WindowBrowser theWindow;

	internal StackPanel ShopperButtonsPanel;

	internal Button ButtonSearchMountings;

	internal ChromiumWebBrowser Browser;

	internal HoverLinkBehaviour HoverLinkBehaviour;

	private bool _contentLoaded;

	public WindowBrowser(Uri uri, bool showShopperButtons)
		: this(uri)
	{
		ShopperButtonsPanel.Visibility = ((!showShopperButtons) ? Visibility.Collapsed : Visibility.Visible);
	}

	public WindowBrowser(Uri uri)
		: this()
	{
		_gotoUri = uri;
		Browser.LoadingStateChanged += Browser_LoadingStateChanged;
		base.Closing += WindowBrowser_Closing;
	}

	private void WindowBrowser_Closing(object sender, CancelEventArgs e)
	{
		Browser.LoadingStateChanged -= Browser_LoadingStateChanged;
	}

	private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
	{
		if (!e.Browser.FocusedFrame.Url.Contains("result#browse") || e.IsLoading)
		{
			return;
		}
		Browser.EvaluateScriptAsync("document.getElementById('result').innerText;").ContinueWith(delegate(Task<JavascriptResponse> thing)
		{
			if (thing.IsCompleted && thing.Result != null && !string.IsNullOrWhiteSpace(thing.Result.ToString()))
			{
				string[] array = thing.Result.Result.ToString().Split(',');
				string primarySearchElement = array[0].ToLower().Trim();
				string secondarySearchElement = array[1].ToLower().Trim();
				if (primarySearchElement != null && secondarySearchElement != null)
				{
					Application.Current.Dispatcher.Invoke(delegate
					{
						Close();
						GlobalErrorHandler.WriteInfoToEventLog($"Style Quiz Results: {primarySearchElement} : {secondarySearchElement}", 2);
						ViewModelLocator.Current.TileSearchMainVM.Reset();
						ViewModelLocator.Current.TileSearchMainVM.SetSelectedSearchTile(ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.TileSearchCategories.FirstOrDefault(), skipPopulate: true);
						Thread.Sleep(500);
						ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.GetDesignSearchResultsByDesignElement(primarySearchElement, secondarySearchElement);
					});
				}
			}
		});
	}

	public WindowBrowser()
	{
		if (ViewModelBase.IsInDesignModeStatic)
		{
			ShopperButtonsPanel.Visibility = Visibility.Visible;
		}
		InitializeComponent();
		Messenger.Default.Register<string>(this, ProcessMessage);
	}

	private void ProcessMessage(string message)
	{
		if (message != null && message == "Close")
		{
			Close();
		}
	}

	private void ButtonSearchMountings_Click(object sender, RoutedEventArgs e)
	{
		TileSearchCategory searchCategory = ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.TileSearchCategories.FirstOrDefault((TileSearchCategory _) => _.Id == 0);
		ViewModelLocator.Current.MainVM.SetSearchModeActive();
		ViewModelLocator.Current.TileSearchMainVM.SetSelectedSearchTile(searchCategory);
		Close();
	}

	private void TheWindowLoaded(object sender, RoutedEventArgs e)
	{
		Browser.Address = _gotoUri.AbsoluteUri;
	}

	private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
	{
		GlobalErrorHandler.WriteInfoToEventLog("Navigating To: " + e.Url, 2);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/windowbrowser.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
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
		switch (connectionId)
		{
		case 1:
			theWindow = (WindowBrowser)target;
			theWindow.Loaded += TheWindowLoaded;
			break;
		case 2:
			ShopperButtonsPanel = (StackPanel)target;
			break;
		case 3:
			ButtonSearchMountings = (Button)target;
			ButtonSearchMountings.Click += ButtonSearchMountings_Click;
			break;
		case 4:
			Browser = (ChromiumWebBrowser)target;
			Browser.FrameLoadEnd += Browser_FrameLoadEnd;
			break;
		case 5:
			HoverLinkBehaviour = (HoverLinkBehaviour)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
