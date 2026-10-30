using System;
using System.Linq.Expressions;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.ViewModel;

public class DesignComponentDisplay : ViewModelBase
{
	private bool _isEnabled;

	private string _name;

	public bool IsEnabled
	{
		get
		{
			return _isEnabled;
		}
		set
		{
			_isEnabled = value;
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsEnabled));
		}
	}

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Name));
		}
	}
}
