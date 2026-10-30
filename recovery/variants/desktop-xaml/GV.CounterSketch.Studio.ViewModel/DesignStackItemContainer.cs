using System.Collections.ObjectModel;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class DesignStackItemContainer : ViewModelBase
{
	private ComponentsEnum _associatedComponent;

	private ObservableCollection<CSCategoryControl> _categoryControls = new ObservableCollection<CSCategoryControl>();

	private string _containerName;

	private bool _isVisible;

	public ComponentsEnum AssociatedComponent
	{
		get
		{
			return _associatedComponent;
		}
		set
		{
			_associatedComponent = value;
			ContainerName = GV.CounterSketch.Studio.Utilities.Helpers.EnumHelper.GetEnumDescriptionFromValue(value);
			RaisePropertyChanged(() => AssociatedComponent);
		}
	}

	public ObservableCollection<CSCategoryControl> CategoryControls
	{
		get
		{
			return _categoryControls;
		}
		set
		{
			if (_categoryControls != value)
			{
				_categoryControls = value;
				RaisePropertyChanged(() => CategoryControls);
			}
		}
	}

	public string ContainerName
	{
		get
		{
			return _containerName;
		}
		set
		{
			string translatedString = TranslationHelper.GetTranslatedString(value);
			_containerName = translatedString;
			RaisePropertyChanged(() => ContainerName);
		}
	}

	public bool IsVisible
	{
		get
		{
			return _isVisible;
		}
		set
		{
			_isVisible = value;
			RaisePropertyChanged(() => IsVisible);
		}
	}
}
