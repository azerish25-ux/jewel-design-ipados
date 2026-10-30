using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.UserControls.ScrollableListBox;
using GalaSoft.MvvmLight.Messaging;
using Telerik.Windows.Controls;

namespace GV.CounterSketch.Studio.View;

public class PortfolioManagerView : Window, IComponentConnector
{
	internal Grid Grid;

	internal DockPanel Controls;

	internal Separator Separator;

	internal RadSlider RadSlider;

	internal TextBox portfolioSearch;

	internal ListBox Portfolios;

	internal DockPanel DockPanelAddPortfolio;

	internal TextBox TextNewProjectName;

	internal Button ButtonPortfolioCreate;

	internal Button ButtonPortfolioCreateCancel;

	internal DockPanel DockPanelDeletePortfolio;

	internal TextBlock TextBlock;

	internal Button ButtonDeletePortfolio;

	internal Button ButtonCancelDeletePortfolio;

	internal DockPanel DockPanelCopyPortfolio;

	internal TextBox TextNewProjectName1;

	internal Button ButtonPortfolioCreate1;

	internal Button ButtonPortfolioCreateCancel1;

	internal DockPanel DockPanelRenamePortfolio;

	internal TextBox TextNewProjectName2;

	internal Button ButtonPortfolioRename;

	internal Button ButtonPortfolioRenameCancel;

	internal ScrollableListBox ListBoxSpi;

	internal Grid ListBoxDesignHistoryHeaders;

	internal ScrollableListBox ListBoxDesignHistory;

	internal ScrollableListBox ListBoxFavorites;

	internal VisualStateGroup VisualStateGroup;

	internal VisualState SPIView;

	internal VisualState DesignHistoryView;

	internal VisualState FavoritesView;

	internal VisualStateGroup PortfolioInfoStateGroup;

	internal VisualState Default;

	internal VisualState AddPortfolio;

	internal VisualState DeletePortfolio;

	internal VisualState RenamePortfolio;

	internal VisualState CopyPortfolio;

	private bool _contentLoaded;

	public PortfolioManagerView()
	{
		InitializeComponent();
		base.Closing += PortfolioManagerView_Closing;
		Messenger.Default.Register<string>(this, ProcessGenericMessage);
	}

	private void PortfolioManagerView_Closing(object sender, CancelEventArgs e)
	{
		e.Cancel = true;
		Hide();
	}

	private bool ProjectSearchFilter(object o)
	{
		if (((Project)o).Name.ToLower().Contains(portfolioSearch.Text.ToLower()))
		{
			return true;
		}
		return false;
	}

	private void UpdateProjectFilter()
	{
		if (Portfolios?.ItemsSource != null)
		{
			CollectionViewSource.GetDefaultView(Portfolios.ItemsSource).Filter = ProjectSearchFilter;
		}
	}

	private void ProcessGenericMessage(string message)
	{
		if (message != null)
		{
			switch (message)
			{
			case "CloseWindow":
				Hide();
				break;
			case "ShowSPIView":
				VisualStateManager.GoToElementState(Grid, "SPIView", useTransitions: false);
				break;
			case "ShowFavoritesView":
				VisualStateManager.GoToElementState(Grid, "FavoritesView", useTransitions: false);
				break;
			case "ShowDesignHistoryView":
				VisualStateManager.GoToElementState(Grid, "DesignHistoryView", useTransitions: false);
				break;
			case "ShowCompareDesignsView":
				VisualStateManager.GoToElementState(Grid, "CompareDesignsView", useTransitions: false);
				break;
			case "ShowDeletePortfolioDialog":
				VisualStateManager.GoToElementState(Grid, "DeletePortfolio", useTransitions: false);
				break;
			case "ShowDuplicatePortfolioDialog":
				VisualStateManager.GoToElementState(Grid, "CopyPortfolio", useTransitions: false);
				break;
			case "ShowPortfolioDefaultView":
				VisualStateManager.GoToElementState(Grid, "Default", useTransitions: false);
				break;
			case "ShowRenamePortfolioDialog":
				VisualStateManager.GoToElementState(Grid, "RenamePortfolio", useTransitions: false);
				break;
			}
		}
	}

	private void PortfolioSearch_OnTextChanged(object sender, TextChangedEventArgs e)
	{
		UpdateProjectFilter();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/portfoliomanagerview.xaml", UriKind.Relative);
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
			Grid = (Grid)target;
			break;
		case 2:
			Controls = (DockPanel)target;
			break;
		case 3:
			Separator = (Separator)target;
			break;
		case 4:
			RadSlider = (RadSlider)target;
			break;
		case 5:
			portfolioSearch = (TextBox)target;
			portfolioSearch.TextChanged += PortfolioSearch_OnTextChanged;
			break;
		case 6:
			Portfolios = (ListBox)target;
			break;
		case 7:
			DockPanelAddPortfolio = (DockPanel)target;
			break;
		case 8:
			TextNewProjectName = (TextBox)target;
			break;
		case 9:
			ButtonPortfolioCreate = (Button)target;
			break;
		case 10:
			ButtonPortfolioCreateCancel = (Button)target;
			break;
		case 11:
			DockPanelDeletePortfolio = (DockPanel)target;
			break;
		case 12:
			TextBlock = (TextBlock)target;
			break;
		case 13:
			ButtonDeletePortfolio = (Button)target;
			break;
		case 14:
			ButtonCancelDeletePortfolio = (Button)target;
			break;
		case 15:
			DockPanelCopyPortfolio = (DockPanel)target;
			break;
		case 16:
			TextNewProjectName1 = (TextBox)target;
			break;
		case 17:
			ButtonPortfolioCreate1 = (Button)target;
			break;
		case 18:
			ButtonPortfolioCreateCancel1 = (Button)target;
			break;
		case 19:
			DockPanelRenamePortfolio = (DockPanel)target;
			break;
		case 20:
			TextNewProjectName2 = (TextBox)target;
			break;
		case 21:
			ButtonPortfolioRename = (Button)target;
			break;
		case 22:
			ButtonPortfolioRenameCancel = (Button)target;
			break;
		case 23:
			ListBoxSpi = (ScrollableListBox)target;
			break;
		case 24:
			ListBoxDesignHistoryHeaders = (Grid)target;
			break;
		case 25:
			ListBoxDesignHistory = (ScrollableListBox)target;
			break;
		case 26:
			ListBoxFavorites = (ScrollableListBox)target;
			break;
		case 27:
			VisualStateGroup = (VisualStateGroup)target;
			break;
		case 28:
			SPIView = (VisualState)target;
			break;
		case 29:
			DesignHistoryView = (VisualState)target;
			break;
		case 30:
			FavoritesView = (VisualState)target;
			break;
		case 31:
			PortfolioInfoStateGroup = (VisualStateGroup)target;
			break;
		case 32:
			Default = (VisualState)target;
			break;
		case 33:
			AddPortfolio = (VisualState)target;
			break;
		case 34:
			DeletePortfolio = (VisualState)target;
			break;
		case 35:
			RenamePortfolio = (VisualState)target;
			break;
		case 36:
			CopyPortfolio = (VisualState)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
