using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class TileSearchCategoryViewModel : ViewModelBase
{
	private Visibility _freehandTileVisibility;

	private bool _hasEverAndEver;

	private List<string> _packageCollection;

	private int _selectedPackageIndex = -1;

	private string _selectedPackageName;

	private TileSearchCategory _selectedTileSearchCategory;

	private List<TileSearchCategory> _tileSearchCategories;

	public RelayCommand CommandLaunchStyleQuiz { get; protected set; }

	public List<string> PackagesCollection
	{
		get
		{
			return _packageCollection;
		}
		set
		{
			if (_packageCollection != value)
			{
				_packageCollection = value;
				((ObservableObject)this).RaisePropertyChanged("PackagesCollection");
			}
		}
	}

	public int SelectedPackageIndex
	{
		get
		{
			return _selectedPackageIndex;
		}
		set
		{
			if (_selectedPackageIndex != value)
			{
				_selectedPackageIndex = value;
				((ObservableObject)this).RaisePropertyChanged("SelectedPackageIndex");
			}
		}
	}

	public string SelectedPackageName
	{
		get
		{
			return _selectedPackageName;
		}
		set
		{
			if (value == "Ever & Ever Bridal Collection")
			{
				ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.IsEverAndEverPackage = true;
			}
			else
			{
				ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.IsEverAndEverPackage = false;
			}
			if (!(_selectedPackageName == value))
			{
				_selectedPackageName = value;
				((ObservableObject)this).RaisePropertyChanged("SelectedPackageName");
			}
		}
	}

	public bool HasEverAndEver
	{
		get
		{
			return _hasEverAndEver;
		}
		set
		{
			_hasEverAndEver = value;
			((ObservableObject)this).RaisePropertyChanged("HasEverAndEver");
		}
	}

	public TileSearchCategory SelectedTileSearchCategory
	{
		get
		{
			return _selectedTileSearchCategory;
		}
		set
		{
			if (_selectedTileSearchCategory != value)
			{
				_selectedTileSearchCategory = value;
				((ObservableObject)this).RaisePropertyChanged("SelectedTileSearchCategory");
			}
		}
	}

	public List<TileSearchCategory> TileSearchCategories
	{
		get
		{
			return _tileSearchCategories;
		}
		set
		{
			if (_tileSearchCategories != value)
			{
				_tileSearchCategories = value;
				((ObservableObject)this).RaisePropertyChanged("TileSearchCategories");
			}
		}
	}

	public Visibility FreehandTileVisibility
	{
		get
		{
			return _freehandTileVisibility;
		}
		set
		{
			_freehandTileVisibility = value;
			((ObservableObject)this).RaisePropertyChanged("FreehandTileVisibility");
		}
	}

	public TileSearchCategoryViewModel()
	{
		if (((ViewModelBase)this).IsInDesignMode)
		{
			PackagesCollection = new List<string> { "One", "Two", "Three", "Four", "Five" };
			HasEverAndEver = true;
		}
		else
		{
			HasEverAndEver = UIHelper.CounterSketchOptions.HasEverAndEver;
		}
		PopulateCategories();
		SetFreehandTileState(UIHelper.CounterSketchOptions.HideFreehandMode == true);
	}

	public void PopulateCategories()
	{
		if (UIHelper.KeyProfile.KeyProfileId == 2)
		{
			TileSearchCategories = new List<TileSearchCategory>
			{
				new TileSearchCategory
				{
					Id = 0,
					Title = Strings.FindyourSetting
				},
				new TileSearchCategory
				{
					Id = 14,
					Title = Strings.Mens
				},
				new TileSearchCategory
				{
					Id = 3,
					Title = Strings.Pendants
				},
				new TileSearchCategory
				{
					Id = 8,
					Title = Strings.Anniversary
				},
				new TileSearchCategory
				{
					TileImagePath = UIHelper.CSFeaturedDesign,
					Id = 17,
					Title = Strings.NewDesigns
				}
			};
			return;
		}
		TileSearchCategories = new List<TileSearchCategory>
		{
			new TileSearchCategory
			{
				Description = Strings.TileDescriptionAllRingDesigns,
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/All Rings.png",
				Id = 0,
				Title = Strings.AllRingDesigns
			},
			new TileSearchCategory
			{
				Description = Strings.TileDescriptionEngagement,
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Engagement.png",
				Id = 2,
				Title = Strings.Engagement
			},
			new TileSearchCategory
			{
				Description = Strings.TileDescriptionPendants,
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Pendants.png",
				Id = 3,
				Title = Strings.Pendants
			},
			new TileSearchCategory
			{
				Description = Strings.TileDescriptionGemstone,
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Gemstones.png",
				Id = 4,
				Title = Strings.Gemstone
			},
			new TileSearchCategory
			{
				Description = Strings.TileDescriptionFashion,
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Fashion.png",
				Id = 5,
				Title = Strings.Fashion
			},
			new TileSearchCategory
			{
				Description = Strings.TileDescriptionPromiseRings,
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Promise.png",
				Id = 6,
				Title = Strings.PromiseRings
			},
			new TileSearchCategory
			{
				Description = Strings.TileDescriptionFamily,
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Family.png",
				Id = 7,
				Title = Strings.Family
			},
			new TileSearchCategory
			{
				Description = Strings.TileDescriptionAnniversary,
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Anniversary.png",
				Id = 8,
				Title = Strings.Anniversary
			},
			new TileSearchCategory
			{
				Description = Strings.TileDescriptionSolitaires,
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Solitaires.png",
				Id = 9,
				Title = Strings.Solitaires
			},
			new TileSearchCategory
			{
				Description = Strings.TileDescriptionBridalSets,
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Bridal.png",
				Id = 10,
				Title = Strings.BridalSets
			},
			new TileSearchCategory
			{
				Description = Strings.TileDescriptionWeddingBands,
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Wedding Bands.png",
				Id = 11,
				Title = Strings.WeddingBands
			},
			new TileSearchCategory
			{
				Description = Strings.TileDescriptionThreeCDesigns,
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/3C.png",
				Id = 12,
				Title = Strings.ThreeCDesigns
			},
			new TileSearchCategory
			{
				Description = Strings.TileDescriptionThreeStone,
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Three Stone.png",
				Id = 13,
				Title = Strings.ThreeStone
			},
			new TileSearchCategory
			{
				Description = Strings.TileDescriptionMens,
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Mens.png",
				Id = 14,
				Title = Strings.Mens
			},
			new TileSearchCategory
			{
				TileImagePath = UIHelper.CSFeaturedDesign,
				Id = 17,
				Title = Strings.NewDesigns
			}
		};
		if (UIHelper.CounterSketchOptions.HasEverAndEver)
		{
			TileSearchCategories.Add(new TileSearchCategory
			{
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Ever and Ever.png",
				Id = 19,
				Title = Strings.EverEverBridalCollection
			});
		}
		else if (UIHelper.CounterSketchOptions.PrivateLabel)
		{
			TileSearchCategories.Add(new TileSearchCategory
			{
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Private Label.png",
				Id = 19,
				Title = Strings.PrivateLabelCollection
			});
		}
		else
		{
			TileSearchCategories.Add(new TileSearchCategory
			{
				TileImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/Tile/1240x1240/Curated Collection.png",
				Id = 19,
				Title = Strings.CuratedCollection
			});
		}
	}

	public void SetFreehandTileState(bool hideFreehandTile)
	{
		if (hideFreehandTile)
		{
			FreehandTileVisibility = Visibility.Collapsed;
		}
		else
		{
			FreehandTileVisibility = Visibility.Visible;
		}
	}

	public void ShowNewDesigns()
	{
		ViewModelLocator.Current.TileSearchMainVM.SetSelectedSearchTile(TileSearchCategories.First((TileSearchCategory _) => _.Id == 17));
	}

	public void ShowStoneShopper()
	{
		if (UIHelper.CounterSketchOptions.StullerHasAuthenticated)
		{
			PunchoutHelper.ShowDiamondShopper();
			return;
		}
		SecurityHelper.AuthenticationMode = SecurityHelper.LoginMode.NormalMode;
		SecurityHelper.PromptForAuthentication();
		if (SecurityHelper.IsAuthenticated)
		{
			PunchoutHelper.ShowDiamondShopper();
		}
	}

	public void ShowGemShopper()
	{
		if (UIHelper.CounterSketchOptions.StullerHasAuthenticated)
		{
			PunchoutHelper.ShowGemstoneShopper();
			return;
		}
		SecurityHelper.AuthenticationMode = SecurityHelper.LoginMode.NormalMode;
		SecurityHelper.PromptForAuthentication();
		if (SecurityHelper.IsAuthenticated)
		{
			PunchoutHelper.ShowGemstoneShopper();
		}
	}
}
