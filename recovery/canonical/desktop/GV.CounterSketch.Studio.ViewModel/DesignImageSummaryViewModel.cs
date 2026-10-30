using System.ComponentModel;
using System.IO;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.ViewModel;

public class DesignImageSummaryViewModel : ViewModelBase
{
	private readonly string _frontImage;

	private string _frontImageUri;

	private readonly string _perspectiveImage;

	private string _perspectiveImageUri;

	private readonly string _rightImage;

	private string _rightImageUri;

	private readonly string _rootPath;

	private readonly string _topImage;

	private string _topImageUri;

	public RelayCommand CommandPopulateImages { get; private set; }

	public string TopImageUri
	{
		get
		{
			return _topImageUri;
		}
		set
		{
			_topImageUri = value;
			RaisePropertyChanged("TopImageUri");
		}
	}

	public string RightImageUri
	{
		get
		{
			return _rightImageUri;
		}
		set
		{
			_rightImageUri = value;
			RaisePropertyChanged("RightImageUri");
		}
	}

	public string FrontImageUri
	{
		get
		{
			return _frontImageUri;
		}
		set
		{
			_frontImageUri = value;
			RaisePropertyChanged("FrontImageUri");
		}
	}

	public string PerspectiveImageUri
	{
		get
		{
			return _perspectiveImageUri;
		}
		set
		{
			_perspectiveImageUri = value;
			RaisePropertyChanged("PerspectiveImageUri");
		}
	}

	public DesignImageSummaryViewModel()
	{
		_rootPath = UIHelper.CSDataRootPath + "\\temp_";
		_topImage = _rootPath + "Viewport_Top.png";
		_rightImage = _rootPath + "Viewport_Right.png";
		_frontImage = _rootPath + "Viewport_Front.png";
		_perspectiveImage = _rootPath + "Viewport_Perspective.png";
		if (base.IsInDesignMode)
		{
			PopulateTestImages();
		}
		else
		{
			CommandPopulateImages = new RelayCommand(PopulateImages);
		}
	}

	private void PopulateImages()
	{
		if (File.Exists(_topImage))
		{
			File.Delete(_topImage);
		}
		if (File.Exists(_rightImage))
		{
			File.Delete(_rightImage);
		}
		if (File.Exists(_frontImage))
		{
			File.Delete(_frontImage);
		}
		if (File.Exists(_perspectiveImage))
		{
			File.Delete(_perspectiveImage);
		}
		BackgroundWorker backgroundWorker = new BackgroundWorker();
		backgroundWorker.RunWorkerCompleted += workerThread_RunWorkerCompleted;
		backgroundWorker.DoWork += workerThread_DoWork;
		backgroundWorker.RunWorkerAsync();
	}

	private void PopulateTestImages()
	{
		TopImageUri = null;
		RightImageUri = null;
		FrontImageUri = null;
		PerspectiveImageUri = null;
	}

	private void workerThread_DoWork(object sender, DoWorkEventArgs e)
	{
	}

	private void workerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		TopImageUri = _topImage;
		RightImageUri = _rightImage;
		FrontImageUri = _frontImage;
		PerspectiveImageUri = _perspectiveImage;
		Messenger.Default.Send<string, DesignImageSummaryView>("loaded");
	}
}
