using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.Model.Objects;

public class Design : ViewModelBase, ICloneable
{
	[XmlIgnore]
	private Guid _baseDesignInstance;

	private List<int> _componentIds;

	private string _csUserDesignName;

	private BitmapSource _designImageSource;

	private Guid _designInstance;

	private string _designer;

	private bool _isFavorite;

	private bool _isLoadedFromLibrary;

	private bool _isSearchFavorite;

	private string _path;

	private int? _selectedIterationId;

	private string _seriesNumber;

	private string _spiNumber;

	private bool _isLoadedFromServiceRequest;

	[XmlIgnore]
	public string BaseDesignImagePath => System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Path), string.Concat(BaseDesignInstance, "_spi.png"));

	[XmlIgnore]
	public List<string> ComponentNames => ComponentIds.Select(delegate(int id)
	{
		ComponentsEnum componentsEnum = (ComponentsEnum)id;
		return componentsEnum.ToString();
	}).ToList();

	[XmlIgnore]
	public string CreationDate => new FileInfo(Path).CreationTime.ToShortDateString();

	[XmlIgnore]
	public ObservableCollection<Design> DesignHistories
	{
		get
		{
			if (Project != null && IsBaseDesign)
			{
				List<Design> source = Project.Designs.Where((Design design) => !design.IsBaseDesign && design.BaseDesignInstance.Equals(BaseDesignInstance)).ToList();
				if (source.Any())
				{
					return new ObservableCollection<Design>(source.OrderByDescending((Design dh) => dh.ModifiedDate));
				}
			}
			return new ObservableCollection<Design>();
		}
	}

	[XmlIgnore]
	public string FavoriteImagePath
	{
		get
		{
			string directoryName = System.IO.Path.GetDirectoryName(Path);
			string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(Path);
			if (directoryName != null)
			{
				return System.IO.Path.Combine(directoryName, fileNameWithoutExtension + "_fav.png");
			}
			return string.Empty;
		}
	}

	[XmlIgnore]
	public string FavoriteImagePathSearchOrientation
	{
		get
		{
			string directoryName = System.IO.Path.GetDirectoryName(Path);
			string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(Path);
			if (directoryName != null)
			{
				return System.IO.Path.Combine(directoryName, fileNameWithoutExtension + "_fav_spi.png");
			}
			return string.Empty;
		}
	}

	[XmlIgnore]
	public DateTime ModifiedDate
	{
		get
		{
			if (File.Exists(_path))
			{
				return File.GetLastWriteTime(_path);
			}
			return new DateTime(2001, 1, 1);
		}
	}

	[XmlIgnore]
	public Project Project
	{
		get
		{
			Project projectForDesign = ProjectManagementHelper.GetProjectForDesign(this);
			if (projectForDesign != null)
			{
				return projectForDesign;
			}
			return new Project
			{
				Name = "- UNKNOWN - "
			};
		}
	}

	public Guid BaseDesignInstance
	{
		get
		{
			return _baseDesignInstance;
		}
		set
		{
			_baseDesignInstance = value;
			((ObservableObject)this).RaisePropertyChanged<Guid>((Expression<Func<Guid>>)(() => BaseDesignInstance));
		}
	}

	public List<int> ComponentIds
	{
		get
		{
			if (_componentIds != null)
			{
				return _componentIds;
			}
			_componentIds = new List<int>();
			return _componentIds;
		}
		set
		{
			_componentIds = value;
			((ObservableObject)this).RaisePropertyChanged("ComponentIds");
		}
	}

	public int ComponentTypeId { get; set; }

	public string CSUserDesignName
	{
		get
		{
			return _csUserDesignName ?? ModifiedDate.ToString();
		}
		set
		{
			_csUserDesignName = value;
			((ObservableObject)this).RaisePropertyChanged("CSUserDesignName");
		}
	}

	public string Designer
	{
		get
		{
			return _designer;
		}
		set
		{
			_designer = value;
			((ObservableObject)this).RaisePropertyChanged("Designer");
			((ObservableObject)this).RaisePropertyChanged("DesignerDisplayName");
		}
	}

	public int DesignId { get; set; }

	public int Version { get; set; }

	[XmlIgnore]
	public BitmapSource DesignImageSource
	{
		get
		{
			return _designImageSource;
		}
		set
		{
			if (value != null)
			{
				_designImageSource = value;
				((ObservableObject)this).RaisePropertyChanged("DesignImageSource");
			}
		}
	}

	public Guid DesignInstance
	{
		get
		{
			return _designInstance;
		}
		set
		{
			_designInstance = value;
			((ObservableObject)this).RaisePropertyChanged<Guid>((Expression<Func<Guid>>)(() => DesignInstance));
		}
	}

	public string DesignType { get; set; }

	public int DesignTypeId { get; set; }

	public bool IsBaseDesign { get; set; }

	public bool IsFavorite
	{
		get
		{
			return _isFavorite;
		}
		set
		{
			_isFavorite = value;
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsFavorite));
			if (Project != null && !Project.IsDeserializing)
			{
				Project.SaveProjectData();
			}
		}
	}

	public bool IsImageLoaded { get; set; }

	[XmlIgnore]
	public bool IsLoadedFromLibrary
	{
		get
		{
			return _isLoadedFromLibrary;
		}
		set
		{
			_isLoadedFromLibrary = value;
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsLoadedFromLibrary));
		}
	}

	[XmlIgnore]
	public bool IsLoadedFromServiceRequest
	{
		get
		{
			return _isLoadedFromServiceRequest;
		}
		set
		{
			_isLoadedFromServiceRequest = value;
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsLoadedFromServiceRequest));
		}
	}

	public bool IsSearchFavorite
	{
		get
		{
			return _isSearchFavorite;
		}
		set
		{
			_isSearchFavorite = value;
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsSearchFavorite));
			if (Project != null && !Project.IsDeserializing)
			{
				Project.SaveProjectData();
			}
		}
	}

	public int MaterialGem1 { get; set; }

	public int MaterialGem2 { get; set; }

	public int MaterialGem3 { get; set; }

	public int MaterialMetal1 { get; set; }

	public int MaterialMetal2 { get; set; }

	public string Name { get; set; }

	public string Path
	{
		get
		{
			return _path;
		}
		set
		{
			_path = value;
			((ObservableObject)this).RaisePropertyChanged("Path");
		}
	}

	public long RenderFileCenterOffset { get; set; }

	public int? SelectedIterationId
	{
		get
		{
			return _selectedIterationId;
		}
		set
		{
			if (value != _selectedIterationId)
			{
				_selectedIterationId = value;
				((ObservableObject)this).RaisePropertyChanged("SelectedIterationId");
			}
		}
	}

	public string SeriesNumber
	{
		get
		{
			if (!string.IsNullOrEmpty(_seriesNumber) && _seriesNumber[0] == ':')
			{
				_seriesNumber = _seriesNumber.Substring(1);
			}
			return _seriesNumber;
		}
		set
		{
			_seriesNumber = value;
			((ObservableObject)this).RaisePropertyChanged("SeriesNumber");
		}
	}

	public string SPINumber
	{
		get
		{
			return _spiNumber;
		}
		set
		{
			_spiNumber = value;
			((ObservableObject)this).RaisePropertyChanged("SPINumber");
		}
	}

	public int VariantParentDesignId { get; set; }

	public Collection<Design> Variants { get; set; }

	public Design(string pathToDesignFile)
		: this()
	{
		string[] array = pathToDesignFile.Split('\\');
		Path = pathToDesignFile;
		Name = array[array.Length - 1];
	}

	public Design()
	{
	}

	public object Clone()
	{
		try
		{
			using MemoryStream memoryStream = new MemoryStream();
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Design));
			xmlSerializer.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			Design design = xmlSerializer.Deserialize(memoryStream) as Design;
			if (design != null)
			{
				design.DesignInstance = Guid.NewGuid();
				design.IsImageLoaded = false;
				design.IsBaseDesign = false;
				design.IsSearchFavorite = false;
				design.IsFavorite = false;
				design.CSUserDesignName = null;
			}
			return design;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public void DesignIdSet(int designId, int version)
	{
		DesignId = designId;
		Version = version;
	}

	public void NotifyImagesUpdated()
	{
		((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => BaseDesignImagePath));
		((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => FavoriteImagePath));
		((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => FavoriteImagePathSearchOrientation));
	}

	public void UpdateDesignImageSource()
	{
		DesignImageSource = LoadGImageSource();
	}

	internal BitmapImage LoadGImageSource()
	{
		return LibraryGImageHelper.GetImageSource(UIHelper.CurrentSearchResultImageType, MaterialMetal1, MaterialMetal2, MaterialGem1, MaterialGem2, MaterialGem3, DesignId, Version);
	}
}
