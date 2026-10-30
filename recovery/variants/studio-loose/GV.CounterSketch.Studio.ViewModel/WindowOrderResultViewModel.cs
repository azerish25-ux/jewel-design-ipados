using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace GV.CounterSketch.Studio.ViewModel;

public class WindowOrderResultViewModel : ViewModelBase
{
	private int _quoteId;

	private bool _showSubmissionForm;

	public string OrderConfirmationPromptMessage => UIHelper.KeyProfile.OrderConfirmationPromptMessage;

	public RelayCommand CommandOpenSubmissionForm { get; set; }

	public int QuoteId
	{
		get
		{
			return _quoteId;
		}
		set
		{
			_quoteId = value;
			((ObservableObject)this).RaisePropertyChanged<int>((Expression<Func<int>>)(() => QuoteId));
		}
	}

	public bool ShowSubmissionForm
	{
		get
		{
			return _showSubmissionForm;
		}
		set
		{
			_showSubmissionForm = value;
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => ShowSubmissionForm));
		}
	}

	public WindowOrderResultViewModel()
	{
		if (((ViewModelBase)this).IsInDesignMode)
		{
			QuoteId = 999;
			UIHelper.KeyProfile.OrderConfirmationPromptMessage = "This is a sample order confirmation prompt message";
			ShowSubmissionForm = true;
		}
		else
		{
			InitializeCommands();
		}
	}

	public void Populate(int quoteId, bool showSubmissionForm)
	{
		QuoteId = quoteId;
		ShowSubmissionForm = showSubmissionForm;
	}

	private void InitializeCommands()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		CommandOpenSubmissionForm = new RelayCommand((Action)OpenSubmissionForm, false);
	}

	private void OpenSubmissionForm()
	{
		try
		{
			Process.Start(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "StoneSubmissionFormTermsofAgreement.pdf"));
		}
		catch
		{
		}
	}
}
