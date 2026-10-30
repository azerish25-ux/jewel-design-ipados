using System.Diagnostics;
using System.IO;
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
			RaisePropertyChanged(() => QuoteId);
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
			RaisePropertyChanged(() => ShowSubmissionForm);
		}
	}

	public WindowOrderResultViewModel()
	{
		if (base.IsInDesignMode)
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
		CommandOpenSubmissionForm = new RelayCommand(OpenSubmissionForm);
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
