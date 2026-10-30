using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class WindowRenderOptionsViewModel : ViewModelBase
{
	private bool _isRenderDesignLayFlat;

	private bool _isRenderGroundPlaneAlphaed;

	private bool _isRenderPropsAlphaed;

	private bool _isSaveRenderEnabled;

	private bool _primaryPropMaterialOptionsEnabled;

	private List<RenderObjectPagedListBoxItem> _renderEnvironmentOptions;

	private List<RenderObjectPagedListBoxItem> _renderGroundplaneMaterialOptions;

	private List<RenderObjectPagedListBoxItem> _renderPropMaterialOptions;

	private List<RenderObjectPagedListBoxItem> _renderPropOptions;

	private bool _secondaryPropMaterialOptionsEnabled;

	private RenderObjectPagedListBoxItem _selectedRenderEnvironment;

	private RenderObjectPagedListBoxItem _selectedRenderGroundPlane;

	private RenderObjectPagedListBoxItem _selectedRenderProp;

	private RenderObjectPagedListBoxItem _selectedRenderPropPrimaryMaterial;

	private RenderObjectPagedListBoxItem _selectedRenderPropSecondaryMaterial;

	private CSEnumRenderResolutions _selectedRenderResolution;

	private bool _isRenderPreviewToggled;

	public IEnumerable<CSEnumRenderResolutions> SelectedRenderResolutionOptions => Enum.GetValues(typeof(CSEnumRenderResolutions)).Cast<CSEnumRenderResolutions>();

	public RelayCommand CommandRender { get; private set; }

	public RelayCommand CommandSaveDefaults { get; private set; }

	public RelayCommand CommandResetOptions { get; private set; }

	public RelayCommand CommandSetRenderSaveLocation { get; private set; }

	public bool IsSaveRenderEnabled
	{
		get
		{
			return _isSaveRenderEnabled;
		}
		set
		{
			_isSaveRenderEnabled = value;
			if (value)
			{
				EventHelper.RecordEvent(EventHelper.EventTypes.Render_SaveToFolder);
			}
			if (!value)
			{
				UIHelper.CounterSketchOptions.RenderSaveLocation = string.Empty;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			RaisePropertyChanged(() => IsSaveRenderEnabled);
		}
	}

	public bool IsRenderPropsAlphaed
	{
		get
		{
			return _isRenderPropsAlphaed;
		}
		set
		{
			_isRenderPropsAlphaed = value;
			if (value)
			{
				EventHelper.RecordEvent(EventHelper.EventTypes.Render_PropsOnAlpha);
			}
			UIHelper.CounterSketchOptions.RenderPropsAlphaed = value;
			UIHelper.CounterSketchOptions.WriteSettings();
			RaisePropertyChanged(() => IsRenderPropsAlphaed);
		}
	}

	public bool IsRenderGroundPlaneAlphaed
	{
		get
		{
			return _isRenderGroundPlaneAlphaed;
		}
		set
		{
			_isRenderGroundPlaneAlphaed = value;
			if (value)
			{
				EventHelper.RecordEvent(EventHelper.EventTypes.Render_GroundPlaneOnAlpha);
			}
			UIHelper.CounterSketchOptions.RenderGroundPlaneAlphaed = value;
			UIHelper.CounterSketchOptions.WriteSettings();
			RaisePropertyChanged(() => IsRenderGroundPlaneAlphaed);
		}
	}

	public bool PrimaryPropMaterialOptionsEnabled
	{
		get
		{
			return _primaryPropMaterialOptionsEnabled;
		}
		set
		{
			_primaryPropMaterialOptionsEnabled = value;
			RaisePropertyChanged(() => PrimaryPropMaterialOptionsEnabled);
		}
	}

	public bool SecondaryPropMaterialOptionsEnabled
	{
		get
		{
			return _secondaryPropMaterialOptionsEnabled;
		}
		set
		{
			_secondaryPropMaterialOptionsEnabled = value;
			RaisePropertyChanged(() => SecondaryPropMaterialOptionsEnabled);
		}
	}

	public List<RenderObjectPagedListBoxItem> RenderGroundplaneMaterialOptions
	{
		get
		{
			return _renderGroundplaneMaterialOptions ?? (_renderGroundplaneMaterialOptions = new List<RenderObjectPagedListBoxItem>());
		}
		set
		{
			_renderGroundplaneMaterialOptions = value;
			RaisePropertyChanged(() => RenderGroundplaneMaterialOptions);
		}
	}

	public List<RenderObjectPagedListBoxItem> RenderPropMaterialOptions
	{
		get
		{
			return _renderPropMaterialOptions ?? (_renderPropMaterialOptions = new List<RenderObjectPagedListBoxItem>());
		}
		set
		{
			_renderPropMaterialOptions = value;
			RaisePropertyChanged(() => RenderPropMaterialOptions);
		}
	}

	public List<RenderObjectPagedListBoxItem> RenderEnvironmentOptions
	{
		get
		{
			return _renderEnvironmentOptions ?? (_renderEnvironmentOptions = new List<RenderObjectPagedListBoxItem>());
		}
		set
		{
			_renderEnvironmentOptions = value;
			RaisePropertyChanged(() => RenderEnvironmentOptions);
		}
	}

	public List<RenderObjectPagedListBoxItem> RenderPropOptions
	{
		get
		{
			return _renderPropOptions ?? (_renderPropOptions = new List<RenderObjectPagedListBoxItem>());
		}
		set
		{
			_renderPropOptions = value;
			RaisePropertyChanged(() => RenderPropOptions);
		}
	}

	public RenderObjectPagedListBoxItem SelectedRenderEnvironment
	{
		get
		{
			return _selectedRenderEnvironment;
		}
		set
		{
			_selectedRenderEnvironment = value;
			RaisePropertyChanged(() => SelectedRenderEnvironment);
		}
	}

	public RenderObjectPagedListBoxItem SelectedRenderGroundPlane
	{
		get
		{
			return _selectedRenderGroundPlane;
		}
		set
		{
			_selectedRenderGroundPlane = value;
			RaisePropertyChanged(() => SelectedRenderGroundPlane);
		}
	}

	public RenderObjectPagedListBoxItem SelectedRenderPropPrimaryMaterial
	{
		get
		{
			return _selectedRenderPropPrimaryMaterial;
		}
		set
		{
			_selectedRenderPropPrimaryMaterial = value;
			RaisePropertyChanged(() => SelectedRenderPropPrimaryMaterial);
		}
	}

	public RenderObjectPagedListBoxItem SelectedRenderPropSecondaryMaterial
	{
		get
		{
			return _selectedRenderPropSecondaryMaterial;
		}
		set
		{
			_selectedRenderPropSecondaryMaterial = value;
			RaisePropertyChanged(() => SelectedRenderPropSecondaryMaterial);
		}
	}

	public RenderObjectPagedListBoxItem SelectedRenderProp
	{
		get
		{
			return _selectedRenderProp;
		}
		set
		{
			_selectedRenderProp = value;
			if (_selectedRenderProp == null)
			{
				SelectedRenderProp = RenderPropOptions.FirstOrDefault();
				return;
			}
			if (_selectedRenderProp.Name != "None")
			{
				EventHelper.RecordEvent(EventHelper.EventTypes.Render_Props);
				PrimaryPropMaterialOptionsEnabled = true;
				SecondaryPropMaterialOptionsEnabled = true;
			}
			else
			{
				PrimaryPropMaterialOptionsEnabled = false;
				SecondaryPropMaterialOptionsEnabled = false;
			}
			RaisePropertyChanged(() => SelectedRenderProp);
		}
	}

	public CSEnumRenderResolutions SelectedRenderResolution
	{
		get
		{
			return _selectedRenderResolution;
		}
		set
		{
			_selectedRenderResolution = value;
			UpdateRenderPreviewSquareInViewport();
			RaisePropertyChanged(() => SelectedRenderResolution);
		}
	}

	public bool IsRenderDesignLayFlat
	{
		get
		{
			return _isRenderDesignLayFlat;
		}
		set
		{
			_isRenderDesignLayFlat = value;
			RaisePropertyChanged(() => IsRenderDesignLayFlat);
			UpdateRhinoViewport();
		}
	}

	public bool IsRenderPreviewToggled
	{
		get
		{
			return _isRenderPreviewToggled;
		}
		set
		{
			if (_isRenderPreviewToggled != value)
			{
				_isRenderPreviewToggled = value;
				UpdateRenderPreviewSquareInViewport();
				RaisePropertyChanged(() => IsRenderPreviewToggled);
			}
		}
	}

	public bool DesignIsPendant { get; set; }

	public WindowRenderOptionsViewModel()
	{
		if (!base.IsInDesignMode)
		{
			PopulateRenderOptions();
			InitializeCommands();
		}
	}

	public void PopulateRenderOptions()
	{
		string text = Path.Combine(UIHelper.CSRootPath, "System\\RenderMaterials\\Groundplanes\\icons\\");
		string text2 = Path.Combine(UIHelper.CSRootPath, "Library\\Props\\Icons\\");
		string text3 = Path.Combine(UIHelper.CSRootPath, "Images\\Render\\Environment\\");
		List<RenderObjectPagedListBoxItem> list = new List<RenderObjectPagedListBoxItem>();
		list.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "None",
			ImagePath = text2 + "none.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("None")
		});
		if (!DesignIsPendant)
		{
			list.Add(new RenderObjectPagedListBoxItem
			{
				Description = "Description",
				Name = "STU_31058RW",
				ImagePath = text2 + "RingStandB.jpg",
				PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Ring"),
				SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Stand")
			});
		}
		list.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "block",
			ImagePath = text2 + "block.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Block")
		});
		if (!DesignIsPendant)
		{
			list.Add(new RenderObjectPagedListBoxItem
			{
				Description = "Description",
				Name = "fingerclothtwisted",
				ImagePath = text2 + "fingerclothtwisted.jpg",
				PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Finger Cloth"),
				SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Twisted")
			});
			list.Add(new RenderObjectPagedListBoxItem
			{
				Description = "Description",
				Name = "Mandrel",
				ImagePath = text2 + "mandrel.jpg",
				PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Mandrel")
			});
		}
		list.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "Pedestal",
			ImagePath = text2 + "pedestal.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Pedestal")
		});
		if (!DesignIsPendant)
		{
			list.Add(new RenderObjectPagedListBoxItem
			{
				Description = "Description",
				Name = "ringbox",
				ImagePath = text2 + "ringbox.jpg",
				PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Ring Box")
			});
			list.Add(new RenderObjectPagedListBoxItem
			{
				Description = "Description",
				Name = "STU_37A01UR",
				ImagePath = text2 + "RingTray.jpg",
				PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Ring Tray")
			});
		}
		List<RenderObjectPagedListBoxItem> list2 = new List<RenderObjectPagedListBoxItem>();
		list2.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "Studio",
			ImagePath = text3 + "Studio.bmp",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Studio")
		});
		list2.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "LightTent",
			ImagePath = text3 + "LightTent.bmp",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Light Tent")
		});
		list2.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "BlurStudio",
			ImagePath = text3 + "BlurStudio.bmp",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Blur Studio")
		});
		list2.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "BlurStudio2",
			ImagePath = text3 + "BlurStudio2.bmp",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Blur"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Studio 2")
		});
		list2.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "BoxStudio",
			ImagePath = text3 + "BoxStudio.bmp",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Box Studio")
		});
		list2.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "LightBox",
			ImagePath = text3 + "LightBox.bmp",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Light Box")
		});
		list2.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "WhiteBlur",
			ImagePath = text3 + "WhiteBlur.bmp",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("White Blur")
		});
		list2.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "System6",
			ImagePath = text3 + "System6.bmp",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("System 6")
		});
		List<RenderObjectPagedListBoxItem> list3 = new List<RenderObjectPagedListBoxItem>();
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "Bamboo",
			ImagePath = text + "Bamboo.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Bamboo")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "BlackPlastic",
			ImagePath = text + "BlackPlastic.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Black"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Plastic")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "BlackReflective",
			ImagePath = text + "BlackReflective.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Black"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Reflective")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "BlueFabric",
			ImagePath = text + "BlueFabric.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Blue Fabric")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "BluePlastic",
			ImagePath = text + "BluePlastic.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Blue"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Plastic")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "BlueToBlackReflective",
			ImagePath = text + "BlueToBlackReflective.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Blue To"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Black Reflective")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "BronzeTera",
			ImagePath = text + "BronzeTera.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Bronze Tera")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "BuffedNickel",
			ImagePath = text + "BuffedNickel.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Buffed"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Nickel")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "Canvas",
			ImagePath = text + "Canvas.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Canvas")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "Crayon",
			ImagePath = text + "Crayon.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Crayon")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "EarthWash",
			ImagePath = text + "EarthWash.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Earth Wash")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "EliteCream",
			ImagePath = text + "EliteCream.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Elite Cream")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "EliteGraphite",
			ImagePath = text + "EliteGraphite.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Elite"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Graphite")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "EliteLatte",
			ImagePath = text + "EliteLatte.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Elite"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Latte")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "EliteOnyx",
			ImagePath = text + "EliteOnyx.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Elite"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Onyx")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "GraniteButterum",
			ImagePath = text + "GraniteButterum.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Granite"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Butterum")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "Granite",
			ImagePath = text + "Granite.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Granite")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "GraniteBlack",
			ImagePath = text + "GraniteBlack.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Granite"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Black")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "LightMarble",
			ImagePath = text + "LightMarble.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Light"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Marble")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "MarbleVeined1",
			ImagePath = text + "MarbleVeined1.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Marble"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Veined 1")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "MarbleVeined6",
			ImagePath = text + "MarbleVeined6.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Marble"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Veined 2")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "NovasuedeOlive",
			ImagePath = text + "NovasuedeOlive.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Novasuede"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Olive")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "NovasuedePeriwinkle",
			ImagePath = text + "NovasuedePeriwinkle.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Novasuede"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Periwinkle")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "NovasuedeStraw",
			ImagePath = text + "NovasuedeStraw.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Novasuede"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Straw")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "RedFabric",
			ImagePath = text + "RedFabric.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Red"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Fabric")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "RedPlastic",
			ImagePath = text + "RedPlastic.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Red"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Plastic")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "RedToBlackReflective",
			ImagePath = text + "RedToBlackReflective.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Red To"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Black Reflective")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "SilkBone",
			ImagePath = text + "SilkBone.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Silk Bone")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "SilkDriftwood",
			ImagePath = text + "SilkDriftwood.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Silk"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Driftwood")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "SilkPlatinum",
			ImagePath = text + "SilkPlatinum.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Silk"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Platinum")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "SilkSerpentine",
			ImagePath = text + "SilkSerpentine.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Silk Serpentine")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "TanFelt",
			ImagePath = text + "TanFelt.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Tan Felt")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "WhiteFelt",
			ImagePath = text + "WhiteFelt.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("White Felt")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "WhiteMatte",
			ImagePath = text + "WhiteMatte.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("White Matte")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "WhiteReflective",
			ImagePath = text + "WhiteReflective.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("White"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Reflective")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "WhiteToBlackReflective",
			ImagePath = text + "WhiteToBlackReflective.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("White To"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Black Reflective")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "WinterRedelLeatheretteBordeaux",
			ImagePath = text + "WinterRedelLeatheretteBordeaux.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Winter Redel"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Leatherette Bordeaux")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "WinterRedelLeatheretteMarron",
			ImagePath = text + "WinterRedelLeatheretteMarron.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Winter Redel"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Leatherette Marron")
		});
		list3.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "WoodCherry",
			ImagePath = text + "WoodCherry.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Wood"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Cherry")
		});
		List<RenderObjectPagedListBoxItem> list4 = new List<RenderObjectPagedListBoxItem>();
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "Bamboo",
			ImagePath = text + "Bamboo.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Bamboo")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "BlackPlastic",
			ImagePath = text + "BlackPlastic.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Black Plastic")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "BluePlastic",
			ImagePath = text + "BluePlastic.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Blue Plastic")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "EliteCream",
			ImagePath = text + "EliteCream.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Elite Cream")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "EliteGraphite",
			ImagePath = text + "EliteGraphite.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Elite"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Graphite")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "EliteLatte",
			ImagePath = text + "EliteLatte.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Elite Latte")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "EliteOnyx",
			ImagePath = text + "EliteOnyx.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Elite Onyx")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "NovasuedeOlive",
			ImagePath = text + "NovasuedeOlive.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Novasuede"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Olive")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "NovasuedePeriwinkle",
			ImagePath = text + "NovasuedePeriwinkle.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Novasuede"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Periwinkle")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "NovasuedeStraw",
			ImagePath = text + "NovasuedeStraw.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Novasuede"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Straw")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "RedFabric",
			ImagePath = text + "RedFabric.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Red Fabric")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "RedPlastic",
			ImagePath = text + "RedPlastic.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Red Plastic")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "SilkBone",
			ImagePath = text + "SilkBone.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Silk Bone")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "SilkDriftwood",
			ImagePath = text + "SilkDriftwood.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Silk"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Driftwood")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "SilkPlatinum",
			ImagePath = text + "SilkPlatinum.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Silk"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Platinum")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "SilkSerpentine",
			ImagePath = text + "SilkSerpentine.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Silk Blue")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "TanFelt",
			ImagePath = text + "TanFelt.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Tan Felt")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "WhiteFelt",
			ImagePath = text + "WhiteFelt.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("White Felt")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "WinterRedelLeatheretteBordeaux",
			ImagePath = text + "WinterRedelLeatheretteBordeaux.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Winter Redel"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Leatherette Bordeaux")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "WinterRedelLeatheretteMarron",
			ImagePath = text + "WinterRedelLeatheretteMarron.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Winter Redel"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Leatherette Marron")
		});
		list4.Add(new RenderObjectPagedListBoxItem
		{
			Description = "Description",
			Name = "WoodCherry",
			ImagePath = text + "WoodCherry.jpg",
			PrimaryText = TranslationHelper.GetTranslatedRenderMaterialString("Wood"),
			SecondaryText = TranslationHelper.GetTranslatedRenderMaterialString("Cherry")
		});
		RenderGroundplaneMaterialOptions = list3;
		RenderPropMaterialOptions = list4;
		RenderEnvironmentOptions = list2;
		RenderPropOptions = list;
		ReadRenderOptionsFromCSOptions();
		if (!string.IsNullOrEmpty(UIHelper.CounterSketchOptions.RenderSaveLocation))
		{
			IsSaveRenderEnabled = true;
		}
	}

	public void ReadRenderOptionsFromCSOptions()
	{
		string primaryMaterial = UIHelper.CounterSketchOptions.RenderPropPrimaryMaterialName ?? "EliteCream";
		string secondaryMaterial = UIHelper.CounterSketchOptions.RenderPropSecondaryMaterialName ?? "EliteCream";
		string groundPlane = UIHelper.CounterSketchOptions.RenderGroundPlaneMaterialName ?? "WhiteReflective";
		string prop = UIHelper.CounterSketchOptions.RenderPropName ?? "None";
		string environment = UIHelper.CounterSketchOptions.RenderEnvironmentName ?? "Studio";
		string value = UIHelper.CounterSketchOptions.RenderResolutionSize ?? "W600H600";
		SelectedRenderPropPrimaryMaterial = RenderGroundplaneMaterialOptions.FirstOrDefault((RenderObjectPagedListBoxItem n) => n.Name == primaryMaterial);
		SelectedRenderPropSecondaryMaterial = RenderGroundplaneMaterialOptions.FirstOrDefault((RenderObjectPagedListBoxItem n) => n.Name == secondaryMaterial);
		SelectedRenderGroundPlane = RenderGroundplaneMaterialOptions.FirstOrDefault((RenderObjectPagedListBoxItem n) => n.Name == groundPlane);
		SelectedRenderProp = RenderPropOptions.FirstOrDefault((RenderObjectPagedListBoxItem n) => n.Name == prop);
		SelectedRenderEnvironment = RenderEnvironmentOptions.FirstOrDefault((RenderObjectPagedListBoxItem n) => n.Name == environment);
		SelectedRenderResolution = (CSEnumRenderResolutions)Enum.Parse(typeof(CSEnumRenderResolutions), value);
	}

	public void Render()
	{
		try
		{
			Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
			bool isRenderUsePropsSelected = SelectedRenderProp.Name != "None";
			bool saveRender = !string.IsNullOrEmpty(UIHelper.CounterSketchOptions.RenderSaveLocation);
			string renderSaveLocation = UIHelper.CounterSketchOptions.RenderSaveLocation;
			UIHelper.CoreDataService.RenderGenerateRender(IsRenderDesignLayFlat, isRenderUsePropsSelected, SelectedRenderProp.Name, SelectedRenderPropPrimaryMaterial.Name, SelectedRenderPropSecondaryMaterial.Name, SelectedRenderGroundPlane.Name, SelectedRenderResolution.ToString(), SelectedRenderEnvironment.Name, saveRender, renderSaveLocation, IsRenderPropsAlphaed, IsRenderGroundPlaneAlphaed);
		}
		finally
		{
			Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
			EventHelper.RecordEvent(EventHelper.EventTypes.Design_Rendered, $"IsNew={DesignHelper.IsNewDesign(UIHelper.LoadedDesign.DesignId)}", UIHelper.LoadedDesign.DesignId.ToString(CultureInfo.InvariantCulture));
		}
	}

	public void RenderAndClose()
	{
		Messenger.Default.Send<string, WindowRenderOptionsView>("CloseWindow");
		Render();
	}

	public void Reset()
	{
		ReadRenderOptionsFromCSOptions();
	}

	public void UpdateRenderPreviewSquareInViewport()
	{
		if (!_isRenderPreviewToggled)
		{
			UIHelper.CoreDataService.RhinoRunScript("!-_RenderSafeFrameOptions _Visible=Off _ActionArea=Off _TitleArea=Off _4x3FieldGrid=Off _Enter _Enter");
			return;
		}
		UIHelper.CoreDataService.RhinoRunScript("!-_RenderSafeFrameOptions _Visible=On _ActionArea=Off _TitleArea=Off _4x3FieldGrid=Off _Enter _Enter");
		string text = _selectedRenderResolution.ToString().Split('H')[0].Substring(1);
		string text2 = _selectedRenderResolution.ToString().Split('H')[1];
		if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
		{
			UIHelper.CoreDataService.RhinoRunScript("!-_DocumentProperties _RhinoRender _Resolution _Type=_Custom _Width=" + text + " _Height=" + text2 + " _Enter _Enter _Enter");
		}
	}

	public void UpdateRhinoViewport()
	{
		UIHelper.CoreDataService.UpdateRhinoAppearanceMaterials(string.Empty, string.Empty, IsRenderDesignLayFlat);
	}

	private void InitializeCommands()
	{
		CommandRender = new RelayCommand(RenderAndClose);
		CommandSaveDefaults = new RelayCommand(SaveDefaults);
		CommandResetOptions = new RelayCommand(Reset);
		CommandSetRenderSaveLocation = new RelayCommand(SetRenderSaveLocation);
	}

	private void SaveDefaults()
	{
		SaveRenderOptionsToCSOptions();
	}

	private void SaveRenderOptionsToCSOptions()
	{
		UIHelper.CounterSketchOptions.RenderPropName = SelectedRenderProp.Name;
		UIHelper.CounterSketchOptions.RenderPropPrimaryMaterialName = SelectedRenderPropPrimaryMaterial.Name;
		UIHelper.CounterSketchOptions.RenderPropSecondaryMaterialName = SelectedRenderPropSecondaryMaterial.Name;
		UIHelper.CounterSketchOptions.RenderGroundPlaneMaterialName = SelectedRenderGroundPlane.Name;
		UIHelper.CounterSketchOptions.RenderEnvironmentName = SelectedRenderEnvironment.Name;
		UIHelper.CounterSketchOptions.RenderResolutionSize = SelectedRenderResolution.ToString();
		UIHelper.CounterSketchOptions.WriteSettings();
	}

	private void SetRenderSaveLocation()
	{
		FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
		if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
		{
			UIHelper.CounterSketchOptions.RenderSaveLocation = folderBrowserDialog.SelectedPath;
			UIHelper.CounterSketchOptions.WriteSettings();
		}
	}
}
