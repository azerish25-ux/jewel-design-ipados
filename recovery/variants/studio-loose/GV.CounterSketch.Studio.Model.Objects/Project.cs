using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using Localization;

namespace GV.CounterSketch.Studio.Model.Objects;

public class Project : ViewModelBase, ICloneable
{
	private string _address1;

	private string _address2;

	private string _city;

	private string _comments;

	private string _country;

	private int _defaultRingSizeIndex = -1;

	private ObservableCollection<Design> _designs;

	private string _email;

	private string _firstName;

	private string _lastName;

	private string _name;

	private string _phoneNumber;

	private ProjectType _projectType;

	private string _state;

	private string _zip;

	[XmlIgnore]
	public ObservableCollection<Design> BaseDesigns
	{
		get
		{
			IEnumerable<Design> enumerable = Designs.Where((Design d) => d.IsBaseDesign);
			if (enumerable != null)
			{
				return new ObservableCollection<Design>(enumerable);
			}
			return new ObservableCollection<Design>();
		}
	}

	[XmlIgnore]
	public string CreationDate
	{
		get
		{
			if (Path != null)
			{
				return new FileInfo(Path).CreationTime.ToShortDateString();
			}
			return string.Empty;
		}
	}

	[XmlIgnore]
	public ObservableCollection<Design> FavoriteDesigns
	{
		get
		{
			List<Design> list = Designs.Where((Design design) => design.IsFavorite).ToList();
			if (!list.Any())
			{
				return new ObservableCollection<Design>();
			}
			return new ObservableCollection<Design>(list);
		}
	}

	public bool IsCollection
	{
		get
		{
			if (ProjectType == ProjectType.Collection)
			{
				return true;
			}
			return false;
		}
	}

	[XmlIgnore]
	public string ProjectInfoFilePath
	{
		get
		{
			if (Path != null)
			{
				return System.IO.Path.Combine(Path, $"{Name}.csp");
			}
			return string.Empty;
		}
	}

	[XmlIgnore]
	public ObservableCollection<Design> SearchFavoriteDesigns
	{
		get
		{
			List<Design> list = Designs.Where((Design design) => design.IsSearchFavorite).ToList();
			if (!list.Any())
			{
				return new ObservableCollection<Design>();
			}
			return new ObservableCollection<Design>(list);
		}
	}

	public static bool IsDeserializing { get; private set; }

	public string Address1
	{
		get
		{
			return _address1;
		}
		set
		{
			_address1 = value;
			((ObservableObject)this).RaisePropertyChanged("Address1");
		}
	}

	public string Address2
	{
		get
		{
			return _address2;
		}
		set
		{
			_address2 = value;
			((ObservableObject)this).RaisePropertyChanged("Address2");
		}
	}

	public string City
	{
		get
		{
			return _city;
		}
		set
		{
			_city = value;
			((ObservableObject)this).RaisePropertyChanged("City");
		}
	}

	public string Comments
	{
		get
		{
			return _comments;
		}
		set
		{
			_comments = value;
			((ObservableObject)this).RaisePropertyChanged("Comments");
		}
	}

	public string Country
	{
		get
		{
			return _country;
		}
		set
		{
			_country = value;
			((ObservableObject)this).RaisePropertyChanged("Country");
		}
	}

	public int DefaultRingSizeIndex
	{
		get
		{
			return _defaultRingSizeIndex;
		}
		set
		{
			if (_defaultRingSizeIndex != value)
			{
				_ = _defaultRingSizeIndex;
				_defaultRingSizeIndex = value;
				((ObservableObject)this).RaisePropertyChanged("DefaultRingSizeIndex");
			}
		}
	}

	public ObservableCollection<Design> Designs
	{
		get
		{
			return _designs;
		}
		set
		{
			_designs = value;
			((ObservableObject)this).RaisePropertyChanged<ObservableCollection<Design>>((Expression<Func<ObservableCollection<Design>>>)(() => Designs));
			((ObservableObject)this).RaisePropertyChanged<ObservableCollection<Design>>((Expression<Func<ObservableCollection<Design>>>)(() => FavoriteDesigns));
			((ObservableObject)this).RaisePropertyChanged<ObservableCollection<Design>>((Expression<Func<ObservableCollection<Design>>>)(() => BaseDesigns));
		}
	}

	public string Email
	{
		get
		{
			return _email;
		}
		set
		{
			_email = value;
			((ObservableObject)this).RaisePropertyChanged("Email");
		}
	}

	public string FirstName
	{
		get
		{
			return _firstName;
		}
		set
		{
			_firstName = value;
			((ObservableObject)this).RaisePropertyChanged("FirstName");
		}
	}

	public string LastName
	{
		get
		{
			return _lastName;
		}
		set
		{
			_lastName = value;
			((ObservableObject)this).RaisePropertyChanged("LastName");
		}
	}

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
			((ObservableObject)this).RaisePropertyChanged("Name");
		}
	}

	public string Path { get; set; }

	public string PhoneNumber
	{
		get
		{
			return _phoneNumber;
		}
		set
		{
			_phoneNumber = value;
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => PhoneNumber));
		}
	}

	public ProjectType ProjectType
	{
		get
		{
			return _projectType;
		}
		set
		{
			_projectType = value;
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsCollection));
		}
	}

	public string State
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
			((ObservableObject)this).RaisePropertyChanged("State");
		}
	}

	public string Zip
	{
		get
		{
			return _zip;
		}
		set
		{
			_zip = value;
			((ObservableObject)this).RaisePropertyChanged("Zip");
		}
	}

	public Project(string pathToProject, ProjectType projectType)
		: this()
	{
		string[] array = pathToProject.Split('\\');
		Path = pathToProject;
		Name = array[array.Length - 1];
		ProjectType = projectType;
	}

	public Project(string pathToProject)
		: this()
	{
		string[] array = pathToProject.Split('\\');
		Path = pathToProject;
		Name = array[array.Length - 1];
	}

	public Project()
	{
		Designs = new ObservableCollection<Design>();
	}

	public object Clone()
	{
		try
		{
			using MemoryStream memoryStream = new MemoryStream();
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Project));
			xmlSerializer.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return xmlSerializer.Deserialize(memoryStream);
		}
		catch (Exception arg)
		{
			GlobalErrorHandler.WriteErrorToEventLog($"Error calling Clone(): {arg}");
		}
		return null;
	}

	public void PopulateProjectDesignsCollection()
	{
		ObservableCollection<Design> observableCollection = new ObservableCollection<Design>();
		if (Directory.Exists(UIHelper.CSProjectPath))
		{
			string[] files = Directory.GetFiles(Path, "*.csg");
			foreach (string obj in files)
			{
				Design design = new Design(obj);
				string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(obj);
				if (fileNameWithoutExtension != null)
				{
					string[] array = fileNameWithoutExtension.Split('_');
					if (array.Length == 2)
					{
						string text = array[0];
						string text2 = array[1];
						bool isBaseDesign = text == text2;
						design.IsBaseDesign = isBaseDesign;
						design.BaseDesignInstance = new Guid(text);
						design.DesignInstance = new Guid(text2);
					}
				}
				observableCollection.Add(design);
			}
		}
		else
		{
			Directory.CreateDirectory(UIHelper.CSProjectPath);
		}
		if (!DeserializeProjectInfo())
		{
			GlobalErrorHandler.WriteInfoToEventLog("Recreating missing CSP: " + Name);
		}
		foreach (Design item in observableCollection)
		{
			string[] array2 = item.Path.Split('\\');
			string relativeCSGPath = System.IO.Path.Combine(array2[array2.Length - 2], array2[array2.Length - 1]);
			Design design2 = Designs.Where((Design resultDesign) => resultDesign.Path.Contains(relativeCSGPath)).FirstOrDefault();
			if (design2 != null)
			{
				design2.Path = System.IO.Path.Combine(UIHelper.CSProjectPath, relativeCSGPath);
				design2.Name = array2[array2.Length - 1];
			}
			else
			{
				Designs.Add(item);
			}
		}
		List<Design> list = Designs.ToList();
		list.RemoveAll((Design x) => !File.Exists(x.Path));
		Designs = new ObservableCollection<Design>(list);
		SaveProjectData();
	}

	public bool SaveProjectData()
	{
		if (!string.IsNullOrEmpty(ProjectInfoFilePath) && !PermissionsHelper.VerifyFileAccess(ProjectInfoFilePath))
		{
			return false;
		}
		bool result = false;
		if (!IsDeserializing)
		{
			try
			{
				if (!string.IsNullOrEmpty(ProjectInfoFilePath))
				{
					using (FileStream stream = File.Create(ProjectInfoFilePath))
					{
						new XmlSerializer(typeof(Project)).Serialize(stream, this);
					}
					result = true;
				}
				else
				{
					((object)this).ToString();
				}
			}
			catch (Exception arg)
			{
				GlobalErrorHandler.WriteErrorToEventLog($"Error calling SerializeProjectInfo(): {arg}");
			}
		}
		return result;
	}

	private bool DeserializeProjectInfo()
	{
		bool result = false;
		if (File.Exists(ProjectInfoFilePath))
		{
			try
			{
				IsDeserializing = true;
				using (FileStream stream = new FileStream(ProjectInfoFilePath, FileMode.Open))
				{
					if (new XmlSerializer(typeof(Project)).Deserialize(stream) is Project project)
					{
						Designs = project.Designs;
						_firstName = project.FirstName;
						_lastName = project.LastName;
						_phoneNumber = project.PhoneNumber;
						_email = project.Email;
						_address1 = project.Address1;
						_address2 = project.Address2;
						_city = project.City;
						_state = project.State;
						_zip = project.Zip;
						_comments = project.Comments;
						_country = project.Country;
						_defaultRingSizeIndex = project.DefaultRingSizeIndex;
						ProjectType = ProjectType.Portfolio;
						Designs.ForEach(delegate(Design d)
						{
							d.SelectedIterationId = null;
						});
					}
				}
				result = true;
			}
			catch (Exception ex)
			{
				UIHelper.RaiseApplicationMessageEvent("Project", ApplicationMessageType.Exception, Strings.FailedToSerializeProjectCorrectly + ".\n" + ex.Message, messageIsModal: true);
			}
			finally
			{
				IsDeserializing = false;
			}
		}
		return result;
	}
}
