using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Model.Objects;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public partial class PortfolioManagerView : Window, IComponentConnector
{
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
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
