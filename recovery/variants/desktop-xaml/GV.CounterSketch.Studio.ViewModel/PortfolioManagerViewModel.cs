using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public sealed class PortfolioManagerViewModel : ViewModelBase
{
	private string _address1;

	private string _address2;

	private string _city;

	private string _comments;

	private string _confirmStateMessage;

	private string _country;

	private int _defaultRingSizeIndex;

	private int _designImageWidth;

	private string _email;

	private List<string> _favoritesShowOptions = new List<string>
	{
		Strings.DesignHistory,
		Strings.Favorites
	};

	private string _firstName;

	private string _lastName;

	private string _name;

	private string _phoneNumber;

	private string _selectedManagerMode;

	private bool _spiNumberVisibility = true;

	private string _state;

	private string _userCreatedPortfolioName;

	private string _zip;

	private bool _isPortfolioEditingAllowed;

	private string _selectedProjectName;

	public ObservableCollection<Design> Designs
	{
		get
		{
			if (ProjectManagementHelper.ActiveProject == null)
			{
				return new ObservableCollection<Design>();
			}
			if (_favoritesShowOptions[0] == SelectedManagerMode)
			{
				return new ObservableCollection<Design>(ProjectManagementHelper.ActiveProject.BaseDesigns.OrderByDescending((Design d) => d.ModifiedDate));
			}
			if (_favoritesShowOptions[1] == SelectedManagerMode)
			{
				return ProjectManagementHelper.ActiveProject.FavoriteDesigns;
			}
			return null;
		}
	}

	public List<Project> ProjectCollection => ProjectManagementHelper.CustomerProjectCollection;

	public RelayCommand CommandCreateNewProject { get; private set; }

	public RelayCommand<Design> CommandDeleteDesignFromPortfolio { get; private set; }

	public RelayCommand CommandDeleteSelectedProject { get; private set; }

	public RelayCommand<Design> CommandOpenDesign { get; private set; }

	public RelayCommand CommandRenameSelectedProject { get; private set; }

	public RelayCommand CommandSaveProjectData { get; private set; }

	public RelayCommand CommandSaveUserInfo { get; private set; }

	public RelayCommand CommandSelectedProjectSaveAs { get; private set; }

	public RelayCommand CommandShowCreatePortfolio { get; private set; }

	public RelayCommand CommandShowDeletePortfolioDialog { get; private set; }

	public RelayCommand CommandShowDuplicatePortfolioDialog { get; private set; }

	public RelayCommand CommandShowRenamePortfolioDialog { get; private set; }

	public RelayCommand<Design> CommandUpdateDesignUserLabel { get; private set; }

	public string ConfirmStateMessage
	{
		get
		{
			return _confirmStateMessage;
		}
		set
		{
			_confirmStateMessage = value;
			RaisePropertyChanged(() => ConfirmStateMessage);
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
			RaisePropertyChanged("Country");
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
			_defaultRingSizeIndex = value;
			RaisePropertyChanged("DefaultRingSizeIndex");
		}
	}

	public int DesignImageWidth
	{
		get
		{
			return _designImageWidth;
		}
		set
		{
			_designImageWidth = value;
			RaisePropertyChanged(() => DesignImageWidth);
			if (UIHelper.CounterSketchOptions != null)
			{
				UIHelper.CounterSketchOptions.PortfolioManagerDesignWidthSliderValue = _designImageWidth;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
		}
	}

	public List<string> FavoritesShowOptions
	{
		get
		{
			return _favoritesShowOptions;
		}
		set
		{
			_favoritesShowOptions = value;
			RaisePropertyChanged(() => FavoritesShowOptions);
		}
	}

	public string SelectedManagerMode
	{
		get
		{
			return _selectedManagerMode;
		}
		set
		{
			if (ProjectManagementHelper.ActiveProject != null)
			{
				_selectedManagerMode = value;
				if (SelectedManagerMode == Strings.DesignHistory)
				{
					Messenger.Default.Send<string, PortfolioManagerView>("ShowDesignHistoryView");
				}
				else
				{
					Messenger.Default.Send<string, PortfolioManagerView>("ShowFavoritesView");
				}
				RaisePropertyChanged(() => SelectedManagerMode);
				RaisePropertyChanged(() => Designs);
			}
		}
	}

	public bool SPINumberVisibility
	{
		get
		{
			return _spiNumberVisibility;
		}
		set
		{
			_spiNumberVisibility = value;
			RaisePropertyChanged(() => SPINumberVisibility);
		}
	}

	public RelayCommand CommandEnableProjectRename { get; set; }

	public bool IsPortfolioEditingAllowed
	{
		get
		{
			return _isPortfolioEditingAllowed;
		}
		set
		{
			_isPortfolioEditingAllowed = value;
			RaisePropertyChanged(() => IsPortfolioEditingAllowed);
			RaisePropertyChanged(() => SelectedProjectName);
		}
	}

	public Project SelectedPortfolio
	{
		get
		{
			return ProjectManagementHelper.ActiveProject;
		}
		set
		{
			if (value != null)
			{
				ProjectManagementHelper.ActiveProject = value;
				IsPortfolioEditingAllowed = false;
				_selectedProjectName = ProjectManagementHelper.ActiveProject.Name;
				RaisePropertyChanged(() => SelectedProjectName);
				RaisePropertyChanged(() => SelectedPortfolio);
				PopulateUserInfo();
				ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.SyncSearchResultsWithPortfolioSearchFavorites();
			}
		}
	}

	public string SelectedProjectName
	{
		get
		{
			if (SelectedPortfolio == null)
			{
				return string.Empty;
			}
			return SelectedPortfolio.Name;
		}
		set
		{
			IsPortfolioEditingAllowed = false;
			if (!value.Contains("GV.CounterSketch.Studio.Model.Objects.Project"))
			{
				if (value != null)
				{
					value = value.Trim();
				}
				string name = ProjectManagementHelper.ActiveProject.Name;
				_selectedProjectName = value;
				if (ProjectManagementHelper.ActiveProject != null && ProjectManagementHelper.ActiveProject.Name != _selectedProjectName && ProjectManagementHelper.RenameActiveProject(_selectedProjectName))
				{
					string text = Strings.PortfolioHasBeenRenamed + " " + Strings.From + " " + name + " " + Strings.WordTo + " " + value + ".";
					GlobalErrorHandler.WriteInfoToEventLog(text);
					UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.SingleLineAlert, text, messageIsModal: true);
				}
				RaisePropertyChanged("SelectedProjectName");
			}
		}
	}

	public string Address1
	{
		get
		{
			return _address1;
		}
		set
		{
			_address1 = value;
			RaisePropertyChanged(() => Address1);
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
			RaisePropertyChanged(() => Address2);
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
			RaisePropertyChanged(() => City);
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
			RaisePropertyChanged(() => Comments);
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
			RaisePropertyChanged(() => Email);
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
			RaisePropertyChanged(() => FirstName);
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
			RaisePropertyChanged(() => LastName);
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
			RaisePropertyChanged(() => Name);
		}
	}

	public string PhoneNumber
	{
		get
		{
			return _phoneNumber;
		}
		set
		{
			_phoneNumber = value;
			RaisePropertyChanged(() => PhoneNumber);
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
			RaisePropertyChanged(() => State);
		}
	}

	public string UserCreatedPortfolioName
	{
		get
		{
			return _userCreatedPortfolioName ?? (_userCreatedPortfolioName = string.Empty);
		}
		set
		{
			_userCreatedPortfolioName = value;
			RaisePropertyChanged(() => UserCreatedPortfolioName);
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
			RaisePropertyChanged(() => Zip);
		}
	}

	public PortfolioManagerViewModel()
	{
		if (base.IsInDesignMode)
		{
			SelectedManagerMode = Strings.DesignHistory;
			RaisePropertyChanged(() => SelectedPortfolio);
			DesignImageWidth = 200;
		}
		else
		{
			InitializeCommands();
			SelectedManagerMode = Strings.DesignHistory;
			Messenger.Default.Send<string, PortfolioManagerView>("ShowDesignHistoryView");
			Messenger.Default.Register<NotificationMessage>(this, ProcessNotificationMessage);
			DesignImageWidth = UIHelper.CounterSketchOptions.PortfolioManagerDesignWidthSliderValue;
		}
	}

	public void NotifyPortfoliosUpdated()
	{
		RaisePropertyChanged(() => ProjectCollection);
	}

	private void ClearUserInfo()
	{
		FirstName = string.Empty;
		LastName = string.Empty;
		PhoneNumber = string.Empty;
		Email = string.Empty;
		Address1 = string.Empty;
		Address2 = string.Empty;
		City = string.Empty;
		State = string.Empty;
		Zip = string.Empty;
		Country = string.Empty;
		Comments = string.Empty;
	}

	private void CreateNewProject()
	{
		CustomerInfo customerInfo = new CustomerInfo
		{
			FirstName = FirstName,
			LastName = LastName,
			PhoneNumber = PhoneNumber,
			Email = Email,
			Address1 = Address1,
			Address2 = Address2,
			City = City,
			State = State,
			Zip = Zip,
			DefaultRingSizeIndex = DefaultRingSizeIndex,
			Comments = Comments
		};
		ProjectManagementHelper.CreateNewProject(UserCreatedPortfolioName, customerInfo);
		UserCreatedPortfolioName = string.Empty;
	}

	private void DeleteDesignFromPortfolio(Design selectedDesign)
	{
		ProjectManagementHelper.DeleteDesignFromProject(selectedDesign);
	}

	private void DeleteSelectedProject()
	{
		ProjectManagementHelper.DeleteProject(SelectedPortfolio);
		RaisePropertyChanged("ProjectCollection");
		SelectedPortfolio = ProjectCollection.FirstOrDefault();
		Messenger.Default.Send<string, PortfolioManagerView>("ShowPortfolioDefaultView");
	}

	private void InitializeCommands()
	{
		CommandSaveProjectData = new RelayCommand(SaveProjectData);
		CommandOpenDesign = new RelayCommand<Design>(OpenDesign);
		CommandDeleteDesignFromPortfolio = new RelayCommand<Design>(DeleteDesignFromPortfolio);
		CommandUpdateDesignUserLabel = new RelayCommand<Design>(UpdateDesignUserLabel);
		CommandCreateNewProject = new RelayCommand(CreateNewProject);
		CommandSelectedProjectSaveAs = new RelayCommand(SelectedProjectSaveAs);
		CommandDeleteSelectedProject = new RelayCommand(DeleteSelectedProject);
		CommandShowDeletePortfolioDialog = new RelayCommand(ShowDeletePortfolioDialog);
		CommandShowDuplicatePortfolioDialog = new RelayCommand(ShowDuplicatePortfolioDialog);
		CommandShowRenamePortfolioDialog = new RelayCommand(ShowRenamePortfolioDialog);
		CommandRenameSelectedProject = new RelayCommand(RenameSelectedProject);
		CommandSaveUserInfo = new RelayCommand(SaveUserInfo);
		CommandShowCreatePortfolio = new RelayCommand(ShowCreatePortfolio);
		CommandEnableProjectRename = new RelayCommand(EnableProjectRename);
	}

	private void OpenDesign(Design design)
	{
		if (design != null && File.Exists(design.Path))
		{
			LoadHelper.LoadDesign(design);
			Messenger.Default.Send<string, PortfolioManagerView>("CloseWindow");
		}
	}

	private void PopulateUserInfo()
	{
		Project selectedPortfolio = SelectedPortfolio;
		if (selectedPortfolio != null)
		{
			FirstName = selectedPortfolio.FirstName;
			LastName = selectedPortfolio.LastName;
			PhoneNumber = selectedPortfolio.PhoneNumber;
			Email = selectedPortfolio.Email;
			Address1 = selectedPortfolio.Address1;
			Address2 = selectedPortfolio.Address2;
			City = selectedPortfolio.City;
			State = selectedPortfolio.State;
			Zip = selectedPortfolio.Zip;
			Country = selectedPortfolio.Country;
			Comments = selectedPortfolio.Comments;
		}
	}

	private void ProcessNotificationMessage(NotificationMessage notificationMessage)
	{
		RaisePropertyChanged(() => SelectedPortfolio);
	}

	private void RenameSelectedProject()
	{
		ProjectManagementHelper.RenameProject(SelectedPortfolio, UserCreatedPortfolioName);
		Messenger.Default.Send<string, PortfolioManagerView>("ShowPortfolioDefaultView");
	}

	private void SaveProjectData()
	{
		ProjectManagementHelper.ActiveProject.SaveProjectData();
	}

	private void SaveUserInfo()
	{
		Project activeProject = ProjectManagementHelper.ActiveProject;
		activeProject.FirstName = FirstName;
		activeProject.LastName = LastName;
		activeProject.PhoneNumber = PhoneNumber;
		activeProject.Email = Email;
		activeProject.Address1 = Address1;
		activeProject.Address2 = Address2;
		activeProject.City = City;
		activeProject.State = State;
		activeProject.Zip = Zip;
		activeProject.Comments = Comments;
		activeProject.Country = Country;
		SaveProjectData();
	}

	private void SelectedProjectSaveAs()
	{
		ProjectManagementHelper.CopyActiveProject(UserCreatedPortfolioName);
		UserCreatedPortfolioName = string.Empty;
		Messenger.Default.Send<string, PortfolioManagerView>("ShowPortfolioDefaultView");
		PopulateUserInfo();
	}

	private void ShowCreatePortfolio()
	{
		ClearUserInfo();
	}

	private void ShowDeletePortfolioDialog()
	{
		Messenger.Default.Send<string, PortfolioManagerView>("ShowDeletePortfolioDialog");
	}

	private void ShowDuplicatePortfolioDialog()
	{
		Messenger.Default.Send<string, PortfolioManagerView>("ShowDuplicatePortfolioDialog");
	}

	private void ShowRenamePortfolioDialog()
	{
		Messenger.Default.Send<string, PortfolioManagerView>("ShowRenamePortfolioDialog");
	}

	private void UpdateDesignUserLabel(Design design)
	{
		design.Project.SaveProjectData();
	}

	public void NotifyActiveProjectUpdated()
	{
		RaisePropertyChanged(() => SelectedPortfolio);
		RaisePropertyChanged(() => SelectedProjectName);
		PopulateUserInfo();
		RaisePropertyChanged("Designs");
	}

	private void EnableProjectRename()
	{
		IsPortfolioEditingAllowed = true;
	}
}
