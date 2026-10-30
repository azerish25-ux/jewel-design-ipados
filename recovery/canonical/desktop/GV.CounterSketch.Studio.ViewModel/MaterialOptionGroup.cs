using GalaSoft.MvvmLight;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class MaterialOptionGroup : ViewModelBase
{
	private bool _isMaterialSelectorAvailable;

	private bool _isSelectorBannerVisible;

	private MaterialOptionSelectorViewModel _materialOptionSelectorVm;

	private string _selectorBannerTitle;

	public string TitleTranslated => TranslationHelper.GetTranslatedString(Title);

	public bool IsMaterialSelectorAvailable
	{
		get
		{
			return _isMaterialSelectorAvailable;
		}
		set
		{
			_isMaterialSelectorAvailable = value;
			RaisePropertyChanged("IsMaterialSelectorAvailable");
		}
	}

	public bool IsSelectorBannerVisible
	{
		get
		{
			return _isSelectorBannerVisible;
		}
		set
		{
			_isSelectorBannerVisible = value;
			RaisePropertyChanged("IsSelectorBannerVisible");
		}
	}

	public MaterialOptionSelectorViewModel MaterialOptionSelectorVM
	{
		get
		{
			return _materialOptionSelectorVm;
		}
		set
		{
			_materialOptionSelectorVm = value;
			RaisePropertyChanged(() => MaterialOptionSelectorVM);
		}
	}

	public string SelectorBannerTitle
	{
		get
		{
			return _selectorBannerTitle;
		}
		set
		{
			_selectorBannerTitle = value;
			RaisePropertyChanged("SelectorBannerTitle");
		}
	}

	public string Title { get; set; }
}
