using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using DevExpress.Xpf.LayoutControl;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.Model;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight.Command;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class TileViewPartsViewModel : TileViewViewModelBase
{
	private KeyTime _sectionTransitionTime;

	private bool _isBusySearching;

	public override string PathToLayoutFile
	{
		get
		{
			string path = "TileLayoutParts1.json";
			if (UIHelper.CounterSketchOptions.IsPartsTemplateTwo)
			{
				path = "TileLayoutParts2.json";
			}
			if (UIHelper.CounterSketchOptions.IsPartsTemplateThree)
			{
				path = "TileLayoutParts3.json";
			}
			if (base.IsInDesignMode)
			{
				return Path.Combine(UIHelper.CSDataRootPath, "DesignTimePartsLayout.json");
			}
			return Path.Combine(UIHelper.CSDataRootPath, path);
		}
	}

	public bool IsBusySearching
	{
		get
		{
			return _isBusySearching;
		}
		set
		{
			Application.Current.Dispatcher.Invoke(delegate
			{
				_isBusySearching = value;
				RaisePropertyChanged("IsBusySearching");
				if (value)
				{
					Application.Current.MainWindow.Cursor = Cursors.Wait;
				}
				else
				{
					Application.Current.MainWindow.Cursor = Cursors.Arrow;
				}
			});
		}
	}

	public KeyTime SectionTransitionTime
	{
		get
		{
			return _sectionTransitionTime;
		}
		set
		{
			_sectionTransitionTime = value;
			RaisePropertyChanged("SectionTransitionTime");
		}
	}

	public RelayCommand CommandAddRingRailGuide { get; set; }

	public RelayCommand CommandAddAGem { get; set; }

	public RelayCommand CommandAddText { get; set; }

	public TileViewPartsViewModel()
	{
		CommandAddAGem = new RelayCommand(AddAGem);
		CommandAddRingRailGuide = new RelayCommand(AddRingRailGuid);
		CommandAddText = new RelayCommand(AddText);
	}

	public override List<TileItem> GetDefaultTiles()
	{
		if (base.IsInDesignMode)
		{
			return GetDesignTimeTiles();
		}
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		List<TileItem> list = (from _ in (from a in counterSketchLibraryDataEntities.Attributes
				where a.AttributeTypeId == 11 && a.AttributeName.StartsWith("FH-")
				select new TileSearchCategory
				{
					Id = (int)a.AttributeId,
					Title = a.AttributeName.Substring(3),
					TileImagePath = string.Concat("pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/" + a.AttributeName, ".png")
				}).ToList()
			select new TileItem
			{
				Id = _.Id,
				Title = _.Title,
				Size = TileSize.Large,
				ImagePath = _.TileImagePath,
				TileCommandType = TileCommandType.SearchByFreehandCategory,
				TileCommandObjectInput = _,
				ParentViewModel = this
			}).ToList();
		TileItem item = new TileItem
		{
			Id = 100,
			Title = Strings.AllParts,
			Size = TileSize.Large,
			ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Freehand.png",
			TileCommandType = TileCommandType.SearchByFreehandCategory,
			TileCommandObjectInput = new TileSearchCategory
			{
				Id = 100,
				Title = "All Parts",
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Freehand.png"
			},
			ParentViewModel = this
		};
		TileItem item2 = new TileItem
		{
			Id = 101,
			Title = Strings.NewParts,
			Size = TileSize.Large,
			ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/FH-NewParts.png",
			TileCommandType = TileCommandType.SearchByFreehandCategory,
			TileCommandObjectInput = new TileSearchCategory
			{
				Id = 101,
				Title = "New Parts",
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/FH-NewParts.png"
			},
			ParentViewModel = this
		};
		TileItem item3 = new TileItem
		{
			Id = 102,
			Title = Strings.UserParts,
			Size = TileSize.Large,
			ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/FH-UserParts.png",
			TileCommandType = TileCommandType.SearchByUserParts,
			TileCommandObjectInput = new TileSearchCategory
			{
				Id = 102,
				Title = "User Parts",
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/FH-UserParts.png"
			},
			ParentViewModel = this
		};
		list.Insert(0, item);
		list.Insert(1, item2);
		list.Add(item3);
		list.Where((TileItem x, int i) => i % 4 == 0).ForEach(delegate(TileItem t)
		{
			t.IsGroupStart = true;
		});
		return list;
	}

	public void SetSectionTransitionTimeToInstant()
	{
		SectionTransitionTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0));
	}

	public void SetSectionTransitionTimeToNormal()
	{
		SectionTransitionTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(ViewModelLocator.Current.MainVM.SelectedKeyTime));
	}

	private void AddAGem()
	{
		CustomizePartsListViewModel.AddAGem();
	}

	private void AddRingRailGuid()
	{
		CustomizePartsListViewModel.AddRingRailGuide();
	}

	private void AddText()
	{
		CustomizePartsListViewModel.AddTextTool();
	}

	private List<TileItem> GetDesignTimeTiles()
	{
		return new List<TileItem>();
	}
}
