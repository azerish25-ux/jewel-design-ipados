using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.ViewModel;

public class WindowDesignTryOnViewModel : ViewModelBase
{
	private bool _isHandEnabled;

	private bool _isHandOn;

	private bool _isRingFlipped;

	private bool _isRingOnRightHand;

	private List<RenderHandOptionsPagedListBoxItem> _nailOptions;

	private ObservableCollection<string> _polishSwatchCollection;

	private double _ringPositionOffset;

	private string _selectedHandGender;

	private RenderHandOptionsPagedListBoxItem _selectedNailOption;

	private int _selectedPolishIndex;

	private string _selectedRing = "L3";

	private int _selectedSkinIndex;

	private RenderHandOptionsPagedListBoxItem _selectedSkinToneOption;

	private ObservableCollection<string> _skinSwatchCollection;

	private List<RenderHandOptionsPagedListBoxItem> _skinToneOptions;

	public List<string> HandGenderOptions { get; } = new List<string> { "Woman", "Man" };

	public bool IsL0Enabled
	{
		get
		{
			if (SelectedRing != null)
			{
				if (SelectedRing == "L0")
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}

	public bool IsL1Enabled
	{
		get
		{
			if (SelectedRing != null)
			{
				if (SelectedRing == "L1")
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}

	public bool IsL2Enabled
	{
		get
		{
			if (SelectedRing != null)
			{
				if (SelectedRing == "L2")
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}

	public bool IsL3Enabled
	{
		get
		{
			if (SelectedRing != null)
			{
				if (SelectedRing == "L3")
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}

	public bool IsL4Enabled
	{
		get
		{
			if (SelectedRing != null)
			{
				if (SelectedRing == "L4")
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}

	public bool IsR0Enabled
	{
		get
		{
			if (SelectedRing != null)
			{
				if (SelectedRing == "R0")
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}

	public bool IsR1Enabled
	{
		get
		{
			if (SelectedRing != null)
			{
				if (SelectedRing == "R1")
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}

	public bool IsR2Enabled
	{
		get
		{
			if (SelectedRing != null)
			{
				if (SelectedRing == "R2")
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}

	public bool IsR3Enabled
	{
		get
		{
			if (SelectedRing != null)
			{
				if (SelectedRing == "R3")
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}

	public bool IsR4Enabled
	{
		get
		{
			if (SelectedRing != null)
			{
				if (SelectedRing == "R4")
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}

	public string SelectedPolishName
	{
		get
		{
			if (_polishSwatchCollection.Count > _selectedPolishIndex && _selectedPolishIndex != -1)
			{
				return _polishSwatchCollection[_selectedPolishIndex];
			}
			return string.Empty;
		}
	}

	public string SelectedSkinName
	{
		get
		{
			if (_skinSwatchCollection.Count > _selectedSkinIndex && _selectedSkinIndex != -1)
			{
				return _skinSwatchCollection[_selectedSkinIndex];
			}
			return string.Empty;
		}
	}

	public RelayCommand CommandCloseWindow { get; set; }

	public RelayCommand CommandRemoveHand { get; set; }

	public RelayCommand CommandUpdateHandGender { get; set; }

	public RelayCommand CommandUpdateHandObjects { get; set; }

	public RelayCommand CommandUpdateRhinoHand { get; set; }

	public RelayCommand CommandUpdateRhinoViewportObjects { get; set; }

	public RelayCommand<string> CommandUpdateSelectedRing { get; set; }

	public bool IsHandEnabled
	{
		get
		{
			return _isHandEnabled;
		}
		set
		{
			_isHandEnabled = value;
			RaisePropertyChanged("IsHandEnabled");
		}
	}

	public bool IsHandOn
	{
		get
		{
			return _isHandOn;
		}
		set
		{
			_isHandOn = value;
			RaisePropertyChanged("IsHandOn");
		}
	}

	public bool IsRingFlipped
	{
		get
		{
			return _isRingFlipped;
		}
		set
		{
			if (_isRingFlipped != value)
			{
				_isRingFlipped = value;
				RaisePropertyChanged("IsRingFlipped");
				UpdateHandObjects();
			}
		}
	}

	public bool IsRingOnRightHand
	{
		get
		{
			return _isRingOnRightHand;
		}
		set
		{
			if (_isRingOnRightHand != value)
			{
				_isRingOnRightHand = value;
				RaisePropertyChanged("IsRingOnRightHand");
			}
		}
	}

	public List<RenderHandOptionsPagedListBoxItem> NailOptions
	{
		get
		{
			return _nailOptions ?? (_nailOptions = new List<RenderHandOptionsPagedListBoxItem>());
		}
		set
		{
			_nailOptions = value;
			RaisePropertyChanged(() => NailOptions);
		}
	}

	public ObservableCollection<string> PolishSwatchCollection
	{
		get
		{
			return _polishSwatchCollection;
		}
		private set
		{
			_polishSwatchCollection = value;
			RaisePropertyChanged("PolishSwatchCollection");
		}
	}

	public double RingPositionOffset
	{
		get
		{
			return _ringPositionOffset;
		}
		set
		{
			if (_ringPositionOffset != value)
			{
				_ringPositionOffset = value;
				RaisePropertyChanged("RingPositionOffset");
			}
		}
	}

	public string SelectedHandGender
	{
		get
		{
			return _selectedHandGender;
		}
		set
		{
			_selectedHandGender = value;
			RaisePropertyChanged(() => SelectedHandGender);
		}
	}

	public RenderHandOptionsPagedListBoxItem SelectedNailOption
	{
		get
		{
			return _selectedNailOption;
		}
		set
		{
			_selectedNailOption = value;
			RaisePropertyChanged(() => SelectedNailOption);
		}
	}

	public int SelectedPolishIndex
	{
		get
		{
			return _selectedPolishIndex;
		}
		set
		{
			_selectedPolishIndex = value;
			RaisePropertyChanged("SelectedPolishIndex");
		}
	}

	public string SelectedRing
	{
		get
		{
			return _selectedRing;
		}
		set
		{
			if (!(_selectedRing == value))
			{
				_selectedRing = value;
				RaisePropertyChanged("SelectedRing");
				RaisePropertyChanged("IsL4Enabled");
				RaisePropertyChanged("IsL3Enabled");
				RaisePropertyChanged("IsL2Enabled");
				RaisePropertyChanged("IsL1Enabled");
				RaisePropertyChanged("IsL0Enabled");
				RaisePropertyChanged("IsR0Enabled");
				RaisePropertyChanged("IsR1Enabled");
				RaisePropertyChanged("IsR2Enabled");
				RaisePropertyChanged("IsR3Enabled");
				RaisePropertyChanged("IsR4Enabled");
			}
		}
	}

	public int SelectedSkinIndex
	{
		get
		{
			return _selectedSkinIndex;
		}
		set
		{
			_selectedSkinIndex = value;
			RaisePropertyChanged("SelectedSkinIndex");
		}
	}

	public RenderHandOptionsPagedListBoxItem SelectedSkinToneOption
	{
		get
		{
			return _selectedSkinToneOption;
		}
		set
		{
			_selectedSkinToneOption = value;
			RaisePropertyChanged(() => SelectedSkinToneOption);
		}
	}

	public ObservableCollection<string> SkinSwatchCollection
	{
		get
		{
			return _skinSwatchCollection;
		}
		private set
		{
			_skinSwatchCollection = value;
			RaisePropertyChanged("SkinSwatchCollection");
		}
	}

	public List<RenderHandOptionsPagedListBoxItem> SkinToneOptions
	{
		get
		{
			return _skinToneOptions ?? (_skinToneOptions = new List<RenderHandOptionsPagedListBoxItem>());
		}
		set
		{
			_skinToneOptions = value;
			RaisePropertyChanged(() => SkinToneOptions);
		}
	}

	public WindowDesignTryOnViewModel()
	{
		if (!base.IsInDesignMode)
		{
			InitializeCommands();
			PopulateRenderOptions();
		}
	}

	public void DefaultAppearanceMenu()
	{
		SelectedRing = "L3";
		SelectedSkinIndex = -1;
		SelectedPolishIndex = -1;
	}

	public void Reset()
	{
		SelectedSkinIndex = 0;
		SelectedPolishIndex = 0;
		TurnHandOff();
		IsHandEnabled = false;
	}

	public void TurnHandOff()
	{
		IsHandOn = false;
		UIHelper.CoreDataService.TurnHandOff();
	}

	public void TurnHandOn()
	{
		EventHelper.RecordEvent(EventHelper.EventTypes.Customize_TryOn);
		_isHandOn = true;
		UpdateHandObjects();
		UpdateRhinoViewportObjects();
	}

	private void CloseWindow()
	{
		Messenger.Default.Send<string, WindowDesignTryOnView>("CloseWindow");
	}

	private void InitializeCommands()
	{
		CommandUpdateSelectedRing = new RelayCommand<string>(UpdateSelectedRing);
		CommandCloseWindow = new RelayCommand(CloseWindow);
		CommandRemoveHand = new RelayCommand(RemoveHand);
		CommandUpdateRhinoViewportObjects = new RelayCommand(UpdateRhinoViewportObjects);
		CommandUpdateHandObjects = new RelayCommand(UpdateHandObjects);
		CommandUpdateHandGender = new RelayCommand(UpdateHandGender);
	}

	private void PopulateRenderOptions()
	{
		string text = Path.Combine(UIHelper.CSRootPath, "Images\\TryOn\\");
		List<RenderHandOptionsPagedListBoxItem> list = new List<RenderHandOptionsPagedListBoxItem>();
		list.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "White_Light",
			ImagePath = text + "White_Light.png",
			PrimaryText = ""
		});
		list.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "White_Medium",
			ImagePath = text + "White_Medium.png",
			PrimaryText = ""
		});
		list.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "White_Dark",
			ImagePath = text + "White_Dark.png",
			PrimaryText = ""
		});
		list.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "Brown_Light",
			ImagePath = text + "Brown_Light.png",
			PrimaryText = ""
		});
		list.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "Brown_Medium",
			ImagePath = text + "Brown_Medium.png",
			PrimaryText = ""
		});
		list.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "Brown_Dark",
			ImagePath = text + "Brown_Dark.png",
			PrimaryText = ""
		});
		list.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "Porcelain_Black",
			ImagePath = text + "PorcelainBlack.jpg",
			PrimaryText = ""
		});
		list.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "Porcelain_Gray",
			ImagePath = text + "PorcelainGray.jpg",
			PrimaryText = ""
		});
		list.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "Porcelain_White",
			ImagePath = text + "PorcelainWhite.jpg",
			PrimaryText = ""
		});
		List<RenderHandOptionsPagedListBoxItem> list2 = new List<RenderHandOptionsPagedListBoxItem>();
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishBlack",
			ImagePath = text + "Polish_Black.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishBlueSlate",
			ImagePath = text + "Polish_Blue_Slate.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishCinnamon",
			ImagePath = text + "Polish_Cinnamon.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishGreen",
			ImagePath = text + "Polish_Green.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishLavender",
			ImagePath = text + "Polish_Lavender.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "None",
			ImagePath = text + "Polish_None.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishPeach",
			ImagePath = text + "Polish_Peach.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishPinkAgain",
			ImagePath = text + "Polish_Pink_Again.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishPinkDark",
			ImagePath = text + "Polish_Pink_Dark.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishPinkUltra",
			ImagePath = text + "Polish_Pink_Ultra.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishPurple",
			ImagePath = text + "Polish_Purple.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishPurpleDust",
			ImagePath = text + "Polish_Purple_Dust.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishRed",
			ImagePath = text + "Polish_Red.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishRedChalk",
			ImagePath = text + "Polish_Red_Chalk.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishRose",
			ImagePath = text + "Polish_Rose.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishRoseLight",
			ImagePath = text + "Polish_Rose_Light.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishSalmon",
			ImagePath = text + "Polish_Salmon.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishSand",
			ImagePath = text + "Polish_Sand.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishStone",
			ImagePath = text + "Polish_Stone.png",
			PrimaryText = ""
		});
		list2.Add(new RenderHandOptionsPagedListBoxItem
		{
			Description = "Description",
			Name = "PolishWhite",
			ImagePath = text + "Polish_White.png",
			PrimaryText = ""
		});
		SkinToneOptions = list;
		NailOptions = list2;
		SelectedSkinToneOption = SkinToneOptions.FirstOrDefault((RenderHandOptionsPagedListBoxItem n) => n.Name == "White_Light");
		SelectedNailOption = NailOptions.FirstOrDefault((RenderHandOptionsPagedListBoxItem n) => n.Name == "None");
		SelectedHandGender = "Woman";
	}

	private void RemoveHand()
	{
		TurnHandOff();
	}

	private void UpdateHandGender()
	{
		UpdateHandObjects();
		UpdateRhinoViewportObjects();
	}

	private void UpdateHandObjects()
	{
		string text = "L1";
		if (SelectedRing != null)
		{
			text = SelectedRing;
		}
		text = ((!(SelectedHandGender == "Man")) ? ("Female_" + text) : ("Male_" + text));
		UIHelper.CoreDataService.UpdateRhinoHand(text, IsRingOnRightHand, IsRingFlipped, RingPositionOffset);
	}

	private void UpdateRhinoViewportObjects()
	{
		string skinName = "Skin_Light_Light";
		if (SelectedSkinToneOption != null)
		{
			skinName = ((!(SelectedHandGender == "Man")) ? ("Female_" + SelectedSkinToneOption.Name) : ("Male_" + SelectedSkinToneOption.Name));
		}
		string polishName = "Polish_Pink_Ultra";
		if (SelectedNailOption != null)
		{
			polishName = _selectedNailOption.Name;
		}
		UIHelper.CoreDataService.UpdateRhinoAppearanceMaterials(skinName, polishName, isRenderLayDesignFlat: false);
	}

	private void UpdateSelectedRing(string fingerPosition)
	{
		IsRingOnRightHand = fingerPosition[0] == 'R';
		SelectedRing = fingerPosition;
	}
}
