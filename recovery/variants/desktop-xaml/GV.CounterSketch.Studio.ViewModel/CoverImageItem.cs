using GV.CounterSketch.Studio.UserControls.PagedListBox;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.ViewModel;

public class CoverImageItem : ViewModelBase, IPagedListBoxItem
{
	private string _imagePath;

	public string Abbreviation { get; set; }

	public string Description { get; set; }

	public string ImagePath
	{
		get
		{
			return _imagePath;
		}
		set
		{
			_imagePath = value;
			RaisePropertyChanged(() => ImagePath);
		}
	}

	public bool IsFavorite { get; set; }

	public bool IsReadOnly { get; set; }

	public string PrimaryText { get; set; }

	public string SecondaryText { get; set; }
}
