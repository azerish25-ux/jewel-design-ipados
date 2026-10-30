using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.View;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.ViewModel;

public class DebugDesignInfoViewmodel : ViewModelBase
{
	private List<Tuple<string, string, string>> _documentDataElements;

	public List<Tuple<string, string, string>> DocumentDataElements
	{
		get
		{
			return _documentDataElements;
		}
		set
		{
			_documentDataElements = value;
			RaisePropertyChanged("DocumentDataElements");
		}
	}

	public DebugDesignInfoViewmodel()
	{
		if (base.IsInDesignMode)
		{
			PopulateDesignTime();
		}
	}

	public void ShowDesignInfo()
	{
		DocumentDataElements = (from _ in UIHelper.CoreDataService.DocumentUserDataGetAllElements()
			orderby _.Item1, _.Item2, _.Item3
			select _).ToList();
		WindowDebugDesignInfo windowDebugDesignInfo = new WindowDebugDesignInfo();
		windowDebugDesignInfo.Owner = Application.Current.MainWindow;
		windowDebugDesignInfo.ShowDialog();
	}

	private void PopulateDesignTime()
	{
		DocumentDataElements = new List<Tuple<string, string, string>>
		{
			new Tuple<string, string, string>("LICENSE", string.Concat(DateTime.Now.ToUniversalTime(), 10.ToString(CultureInfo.InvariantCulture)), DebuggingHelper.GetCurrentUserLicenseInfo()),
			new Tuple<string, string, string>("LICENSE", string.Concat(DateTime.Now.ToUniversalTime(), 20.ToString(CultureInfo.InvariantCulture)), DebuggingHelper.GetCurrentUserLicenseInfo()),
			new Tuple<string, string, string>("LICENSE", string.Concat(DateTime.Now.ToUniversalTime(), 30.ToString(CultureInfo.InvariantCulture)), DebuggingHelper.GetCurrentUserLicenseInfo())
		};
	}
}
