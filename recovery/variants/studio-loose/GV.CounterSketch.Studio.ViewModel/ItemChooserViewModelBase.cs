using System;
using System.Collections.ObjectModel;
using GV.CounterSketch.Studio.UserControls.PagedListBox;
using GV.CounterSketch.Studio.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class ItemChooserViewModelBase : ViewModelBase
{
	private Collection<IPagedListBoxItem> _items;

	private IPagedListBoxItem _selectedItem;

	private int _rows;

	private int _columns;

	private string _title;

	private string _mouseOverText;

	private string _mouseOverDescription;

	private string _titleTranslated;

	public RelayCommand<MaterialOptionItem> CommandUpdateMouseOverText { get; set; }

	public RelayCommand CommandResetMouseOverText { get; set; }

	public IPagedListBoxItem SelectedItem
	{
		get
		{
			return _selectedItem;
		}
		set
		{
			_selectedItem = value;
			((ObservableObject)this).RaisePropertyChanged("SelectedItem");
		}
	}

	public int Columns
	{
		get
		{
			return _columns;
		}
		set
		{
			_columns = value;
			((ObservableObject)this).RaisePropertyChanged("Columns");
		}
	}

	public int Rows
	{
		get
		{
			return _rows;
		}
		set
		{
			_rows = value;
			((ObservableObject)this).RaisePropertyChanged("Rows");
		}
	}

	public string MouseOverText
	{
		get
		{
			return _mouseOverText;
		}
		set
		{
			_mouseOverText = value;
			((ObservableObject)this).RaisePropertyChanged("MouseOverText");
		}
	}

	public string MouseOverDescription
	{
		get
		{
			return _mouseOverDescription;
		}
		set
		{
			_mouseOverDescription = value;
			((ObservableObject)this).RaisePropertyChanged("MouseOverDescription");
		}
	}

	public Collection<IPagedListBoxItem> Items
	{
		get
		{
			return _items;
		}
		set
		{
			_items = value;
			((ObservableObject)this).RaisePropertyChanged("Items");
		}
	}

	public string Title
	{
		get
		{
			return _title;
		}
		set
		{
			_title = value;
			TitleTranslated = TranslationHelper.GetTranslatedString(value);
			((ObservableObject)this).RaisePropertyChanged("Title");
		}
	}

	public string TitleTranslated
	{
		get
		{
			return _titleTranslated;
		}
		set
		{
			_titleTranslated = value;
			((ObservableObject)this).RaisePropertyChanged("TitleTranslated");
		}
	}

	public RelayCommand CommandOK { get; set; }

	public RelayCommand CommandCancel { get; set; }

	public RelayCommand CommandClear { get; set; }

	public ItemChooserViewModelBase()
	{
		if (!((ViewModelBase)this).IsInDesignMode)
		{
			InitializeCommands();
		}
	}

	public void Cancel()
	{
		Messenger.Default.Send<string, ItemChooserView>("Close");
	}

	private void Clear()
	{
		if (Items != null)
		{
			SelectedItem = Items[0];
		}
	}

	private void InitializeCommands()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		CommandCancel = new RelayCommand((Action)Cancel, false);
		CommandClear = new RelayCommand((Action)Clear, false);
		CommandOK = new RelayCommand((Action)OK, false);
	}

	private void OK()
	{
		Messenger.Default.Send<string, ItemChooserView>("Close");
	}
}
