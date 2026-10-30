using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.Xpf.LayoutControl;
using GV.CounterSketch.Studio.Model;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class TileViewDesignsViewModel : TileViewViewModelBase
{
	private int _index;

	public override string PathToLayoutFile
	{
		get
		{
			string text = "TileLayoutDesigns";
			switch (UIHelper.KeyProfile.KeyProfileId)
			{
			case 4:
				text = "Bridal_" + text;
				break;
			case 5:
				text = "Plus_" + text;
				break;
			case 8:
				text = "Trial_" + text;
				break;
			}
			string path = text + "1.json";
			if (base.IsTemplateTwo)
			{
				path = text + "2.json";
			}
			if (base.IsTemplateThree)
			{
				path = text + "3.json";
			}
			if (((ViewModelBase)this).IsInDesignMode)
			{
				return Path.Combine(UIHelper.CSDataRootPath, "DesignTimeDesignsLayout.json");
			}
			return Path.Combine(UIHelper.CSDataRootPath, path);
		}
	}

	public TileViewDesignsViewModel()
	{
		if (UIHelper.KeyProfile.IsDesignLibraryDisabled)
		{
			ViewModelLocator.Current.CustomizePartsListVM.AddPart();
			ViewModelLocator.Current.BreadCrumbVM.LabelEditing = "Customize";
		}
	}

	public override List<TileItem> GetDefaultTiles()
	{
		if (((ViewModelBase)this).IsInDesignMode)
		{
			return GetDesignTimeTiles();
		}
		List<TileItem> list = ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.TileSearchCategories.Select((TileSearchCategory _) => new TileItem
		{
			Id = _.Id,
			Title = _.Title,
			Size = (TileSize)2,
			ImagePath = _.TileImagePath,
			TileCommandType = TileCommandType.SearchByTileSearchCategory,
			TileCommandObjectInput = _,
			ParentViewModel = this
		}).ToList();
		TileItem item = new TileItem
		{
			Id = 18,
			Title = Strings.FindYourStone,
			Size = (TileSize)2,
			ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Find Your Stone.png",
			TileCommandType = TileCommandType.LaunchStoneShopper,
			ParentViewModel = this
		};
		list.Add(item);
		TileItem item2 = new TileItem
		{
			Id = 20,
			Title = UntranslatedStrings.FindYourGem,
			Size = (TileSize)2,
			ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Find Your Gem.png",
			TileCommandType = TileCommandType.LaunchGemShopper,
			ParentViewModel = this
		};
		list.Add(item2);
		TileItem item3 = new TileItem
		{
			Id = 100,
			Title = Strings.FindyourStyle,
			Size = (TileSize)2,
			ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Style Quiz.png",
			TileCommandType = TileCommandType.LaunchStyleQuiz,
			ParentViewModel = this
		};
		list.Add(item3);
		if (!UIHelper.KeyProfile.HideFreehandMode)
		{
			TileItem item4 = new TileItem
			{
				Id = 1,
				Title = Strings.Freehand,
				Size = (TileSize)2,
				ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Freehand.png",
				TileCommandType = TileCommandType.LaunchFreehand,
				ParentViewModel = this
			};
			list.Add(item4);
		}
		list.Where((TileItem x, int i) => i % 4 == 0).ForEach(delegate(TileItem t)
		{
			t.IsGroupStart = true;
		});
		return list;
	}

	private List<TileItem> GetDesignTimeTiles()
	{
		List<TileItem> list = new List<TileItem>();
		List<TileItem> list2 = new List<TileItem>();
		List<TileItem> list3 = new List<TileItem>();
		List<TileItem> list4 = new List<TileItem>();
		List<TileItem> list5 = new List<TileItem>();
		for (int i = 1; i < 20; i++)
		{
			TileItem tileItem = new TileItem
			{
				Id = i,
				Title = "Category" + i,
				ImagePath = GetRandomImagePath(),
				TileCommandType = TileCommandType.SearchByTileSearchCategory,
				TileCommandObjectInput = 1
			};
			if (i < 5)
			{
				tileItem.Size = (TileSize)0;
			}
			else if (i < 6)
			{
				tileItem.Size = (TileSize)1;
			}
			else if (i < 11)
			{
				tileItem.Size = (TileSize)2;
			}
			else
			{
				tileItem.Size = (TileSize)3;
			}
			if (i < 11)
			{
				list2.Add(tileItem);
			}
			else if (i < 14)
			{
				list3.Add(tileItem);
			}
			else if (i < 17)
			{
				list4.Add(tileItem);
			}
			else
			{
				list5.Add(tileItem);
			}
		}
		list2.First().IsGroupStart = true;
		list3.First().IsGroupStart = true;
		list4.First().IsGroupStart = true;
		list5.First().IsGroupStart = true;
		list3.First().TileCommandType = TileCommandType.CustomTile;
		list.AddRange(list2);
		list.AddRange(list3);
		list.AddRange(list4);
		list.AddRange(list5);
		return list;
	}

	private string GetRandomImagePath()
	{
		_index++;
		if (_index == 12)
		{
			_index = 0;
		}
		return "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x600/" + _index switch
		{
			0 => "3C.png", 
			1 => "All Rings.png", 
			2 => "Anniversary.png", 
			3 => "Bridal.png", 
			4 => "Engagement.png", 
			5 => "Family.png", 
			6 => "Fashion.png", 
			7 => "Freehand.png", 
			8 => "Gemstones.png", 
			9 => "Mens.png", 
			10 => "3C.png", 
			11 => "Pendants.png", 
			_ => "Find Your Stone Red Box.png", 
		};
	}
}
