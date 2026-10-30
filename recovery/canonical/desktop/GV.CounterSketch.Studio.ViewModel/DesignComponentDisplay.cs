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
			RaisePropertyChanged(() => IsEnabled);
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
			RaisePropertyChanged(() => Name);
		}
	}
}
