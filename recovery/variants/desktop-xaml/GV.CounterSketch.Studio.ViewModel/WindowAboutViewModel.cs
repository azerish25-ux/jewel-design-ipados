using System.Windows.Media;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.LibraryServices;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.ViewModel;

public class WindowAboutViewModel : ViewModelBase
{
	private string _counterSketchVersion;

	private int _customerId;

	private Color _foregroundColor;

	private Color _labelForegroundColor;

	private string _libraryVersion;

	private int _modelCount;

	private int _partsLibraryVersion;

	private string _rhinoVersion;

	public string CounterSketchVersion
	{
		get
		{
			return _counterSketchVersion;
		}
		set
		{
			_counterSketchVersion = value;
			RaisePropertyChanged("CounterSketchVersion");
		}
	}

	public int CustomerId
	{
		get
		{
			return _customerId;
		}
		set
		{
			_customerId = value;
			RaisePropertyChanged("CustomerId");
		}
	}

	public string LibraryVersion
	{
		get
		{
			return _libraryVersion;
		}
		set
		{
			_libraryVersion = value;
			RaisePropertyChanged("LibraryVersion");
		}
	}

	public int PartsLibraryVersion
	{
		get
		{
			return _partsLibraryVersion;
		}
		set
		{
			_partsLibraryVersion = value;
			RaisePropertyChanged("PartsLibraryVersion");
		}
	}

	public Color ForegroundColor
	{
		get
		{
			return _foregroundColor;
		}
		set
		{
			_foregroundColor = value;
			RaisePropertyChanged("ForegroundColor");
		}
	}

	public Color LabelForegroundColor
	{
		get
		{
			return _labelForegroundColor;
		}
		set
		{
			_labelForegroundColor = value;
			RaisePropertyChanged("LabelForegroundColor");
		}
	}

	public int ModelCount
	{
		get
		{
			return _modelCount;
		}
		set
		{
			_modelCount = value;
			RaisePropertyChanged("ModelCount");
		}
	}

	public string RhinoVersion
	{
		get
		{
			return _rhinoVersion;
		}
		set
		{
			_rhinoVersion = value;
			RaisePropertyChanged("RhinoVersion");
		}
	}

	public WindowAboutViewModel()
	{
		if (!base.IsInDesignMode)
		{
			try
			{
				UIHelper.CoreDataService.RhinoGetVersion(delegate(string version)
				{
					RhinoVersion = version;
				});
				CounterSketchVersion = UIHelper.VersionString;
				int result = 0;
				using (CSLibraryDAL cSLibraryDAL = new CSLibraryDAL())
				{
					int.TryParse(cSLibraryDAL.GetLibraryVersion(), out result);
					ModelCount = cSLibraryDAL.GetModelCount();
					CustomerId = UIHelper.CounterSketchOptions.GetKeyId();
				}
				using LibraryServiceClient libraryServiceClient = new LibraryServiceClient();
				LibraryVersion = $"{result} - {libraryServiceClient.GetDesignLibraryReleaseDate(result):MM/dd/yy}";
			}
			catch
			{
			}
		}
		else
		{
			CounterSketchVersion = "9.9.999.9";
			RhinoVersion = "5.0 SR 3";
			LibraryVersion = "99";
			ModelCount = 9999;
			CustomerId = 999;
		}
		ForegroundColor = UIHelper.KeyProfile.AboutForegroundColor;
		LabelForegroundColor = UIHelper.KeyProfile.AboutLabelForegroundColor;
	}
}
