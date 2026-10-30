using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GV.CounterSketch.Common.Annotations;
using Localization;

namespace GV.CounterSketch.Common;

[Serializable]
public class OrderSummaryPrefItemModel : INotifyPropertyChanged
{
	private bool _isDefault;

	private bool _isDisabled;

	private bool _isRadioButtonVisible;

	private int _meleeQuality;

	public string TranslatedTitle => TranslationHelper.GetTranslatedString(Title);

	public string TranslatedSection => TranslationHelper.GetTranslatedString(Section);

	public bool IsRadioButtonVisible
	{
		get
		{
			return _isRadioButtonVisible;
		}
		set
		{
			_isRadioButtonVisible = value;
			RaisePropertyChanged("IsRadioButtonVisible");
		}
	}

	public bool IsCheckBoxVisible { get; set; }

	public bool IsDisabled
	{
		get
		{
			return _isDisabled;
		}
		set
		{
			_isDisabled = value;
			RaisePropertyChanged("IsDisabled");
		}
	}

	public string Title { get; set; }

	public bool IsDefault
	{
		get
		{
			return _isDefault;
		}
		set
		{
			_isDefault = value;
			RaisePropertyChanged("IsDefault");
		}
	}

	public string Section { get; set; }

	public int MeleeQuality
	{
		get
		{
			return _meleeQuality;
		}
		set
		{
			_meleeQuality = value;
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public OrderSummaryPrefItemModel(string title, string section, bool isRadioButtonVisible = true, bool isCheckBoxVisible = true, int meleeQuality = 0)
	{
		Title = title;
		Section = section;
		IsRadioButtonVisible = isRadioButtonVisible;
		IsCheckBoxVisible = isCheckBoxVisible;
		MeleeQuality = meleeQuality;
	}

	public OrderSummaryPrefItemModel()
	{
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
