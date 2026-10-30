using System;
using System.Linq.Expressions;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.ViewModel;

public class TileSearchCategory : ViewModelBase
{
	private string _tileImagePath;

	private long _packageAttributeId;

	public string Description { get; set; }

	public int Id { get; set; }

	public string TileImagePath
	{
		get
		{
			return _tileImagePath;
		}
		set
		{
			_tileImagePath = value;
			((ObservableObject)this).RaisePropertyChanged("TileImagePath");
		}
	}

	public long PackageAttributeId
	{
		get
		{
			return _packageAttributeId;
		}
		set
		{
			_packageAttributeId = value;
			((ObservableObject)this).RaisePropertyChanged("PackageAttributeId");
		}
	}

	public string Title { get; set; }

	public void RaiseImageChangedNotification()
	{
		((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => TileImagePath));
	}
}
