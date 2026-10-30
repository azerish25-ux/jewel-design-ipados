using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using DesignLibrary;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Controls.BLL;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.ViewModel;
using Localization;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class ProjectManagementHelper
{
	public static string TempProjectName = "{" + Strings.NewPortfolio + "}";

	private static Project _activeProject;

	private static List<Project> _globalProjectCollection;

	private static bool _isFileModified;

	public static List<Project> CustomerProjectCollection => GlobalProjectCollection.Where((Project project) => project.ProjectType == ProjectType.Portfolio).ToList();

	public static Project ActiveProject
	{
		get
		{
			return _activeProject;
		}
		set
		{
			_activeProject = value;
			if (_activeProject != null)
			{
				_activeProject.PopulateProjectDesignsCollection();
				switch (_activeProject.ProjectType)
				{
				case ProjectType.Portfolio:
					UIHelper.CounterSketchOptions.LastOpenedPortfolio = _activeProject.Path;
					break;
				case ProjectType.Collection:
					UIHelper.CounterSketchOptions.LastOpenedInventoryPortfolio = _activeProject.Path;
					break;
				}
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			BroadcastActiveProjectChange();
		}
	}

	public static List<Project> GlobalProjectCollection
	{
		get
		{
			return _globalProjectCollection ?? (_globalProjectCollection = new List<Project>());
		}
		set
		{
			_globalProjectCollection = value;
			BroadcastGlobalCollectionChange();
		}
	}

	public static bool IsFileBackedUp { get; set; }

	public static bool IsFileModified
	{
		get
		{
			return _isFileModified;
		}
		set
		{
			if (value != _isFileModified)
			{
				_isFileModified = value;
			}
			if (value)
			{
				IsFileBackedUp = false;
			}
		}
	}

	public static void AddDesignFromLibraryToActiveProject(GV.CounterSketch.Studio.Model.Objects.Design design)
	{
		if (design == null)
		{
			return;
		}
		if (ActiveProject == null)
		{
			UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.SingleLineAlert, Strings.NoPortfolioCanStoreDesign, messageIsModal: true);
			return;
		}
		Project activeProject = ActiveProject;
		if (design.BaseDesignInstance == Guid.Empty)
		{
			design.Designer = UIHelper.CounterSketchOptions.Username;
			design.BaseDesignInstance = Guid.NewGuid();
			design.SPINumber = "Freehand Design";
			design.DesignIdSet(0, 0);
		}
		string path = $"{design.BaseDesignInstance}_{design.BaseDesignInstance}.csg";
		design.Path = Path.Combine(activeProject.Path, path);
		design = ExtractDesignFromLibrary(design);
		if (File.Exists(design.BaseDesignImagePath))
		{
			File.SetAttributes(design.BaseDesignImagePath, FileAttributes.Normal);
			File.Delete(design.BaseDesignImagePath);
		}
		if (design.DesignImageSource == null)
		{
			design.UpdateDesignImageSource();
		}
		ImageHelper.ExportBitmapImageAsPNG(design.DesignImageSource, design.BaseDesignImagePath);
		design.NotifyImagesUpdated();
		design.IsFavorite = false;
		design.CSUserDesignName = null;
		if (design.IsSearchFavorite)
		{
			activeProject.Designs.Where((GV.CounterSketch.Studio.Model.Objects.Design d) => d.IsSearchFavorite && d.DesignId == design.DesignId && d.DesignHistories.Count == 0).ToList().ForEach(delegate(GV.CounterSketch.Studio.Model.Objects.Design d)
			{
				DeleteDesignFromProject(d);
			});
		}
		activeProject.Designs.Add(design);
		activeProject.SaveProjectData();
		BroadcastActiveProjectChange();
	}

	public static void AddDesignOnCanvasToHistoryOfActiveProject(GV.CounterSketch.Studio.Model.Objects.Design design)
	{
		if (IsDocumentCanvasEmpty())
		{
			UIHelper.RaiseApplicationMessageEvent("Empty Design", ApplicationMessageType.MultiLineInfo, Strings.DesignEmptyOpenBuild, messageIsModal: true);
			return;
		}
		if (ActiveProject == null)
		{
			UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.SingleLineAlert, Strings.NoPortfolioCanStoreDesign, messageIsModal: true);
			return;
		}
		Project activeProject = ActiveProject;
		GV.CounterSketch.Studio.Model.Objects.Design designClone = design.Clone() as GV.CounterSketch.Studio.Model.Objects.Design;
		designClone.IsLoadedFromLibrary = false;
		bool flag = false;
		if (ViewModelLocator.Current.WindowDesignTryOnVM.IsHandOn)
		{
			ViewModelLocator.Current.WindowDesignTryOnVM.TurnHandOff();
			flag = true;
		}
		Stopwatch stopwatch = Stopwatch.StartNew();
		Stopwatch stopwatch2 = Stopwatch.StartNew();
		GlobalErrorHandler.WriteInfoToEventLog("Add to Design History Started.", 2);
		if (designClone != null)
		{
			if (activeProject.BaseDesigns.All((GV.CounterSketch.Studio.Model.Objects.Design d) => d.BaseDesignInstance != designClone.BaseDesignInstance))
			{
				designClone.IsBaseDesign = true;
				designClone.BaseDesignInstance = Guid.NewGuid();
				designClone.DesignInstance = designClone.BaseDesignInstance;
				string path = $"{designClone.BaseDesignInstance}_{designClone.BaseDesignInstance}.csg";
				designClone.Path = Path.Combine(activeProject.Path, path);
				if (File.Exists(designClone.BaseDesignImagePath))
				{
					File.SetAttributes(designClone.BaseDesignImagePath, FileAttributes.Normal);
					File.Delete(designClone.BaseDesignImagePath);
				}
				if (designClone.RenderFileCenterOffset == 0L)
				{
					UIHelper.CoreDataService.RequestScreenCaptureDefault(new System.Drawing.Size(400, 400), zoom: true, 3.0).Save(designClone.BaseDesignImagePath, ImageFormat.Png);
				}
				else
				{
					if (designClone.DesignImageSource == null)
					{
						designClone.UpdateDesignImageSource();
					}
					ImageHelper.ExportBitmapImageAsPNG(designClone.DesignImageSource, designClone.BaseDesignImagePath);
				}
			}
			else
			{
				string path = $"{designClone.BaseDesignInstance}_{designClone.DesignInstance}.csg";
				designClone.Path = Path.Combine(activeProject.Path, path);
			}
			string[] array = designClone.Path.Split('\\');
			designClone.Name = array[array.Length - 1];
			stopwatch2.Reset();
			stopwatch2.Start();
			OrderHelper.ClearOrderInformationFromDesignUserData();
			UIHelper.CoreDataService.DocumentWrite(designClone.Path);
			GlobalErrorHandler.WriteInfoToEventLog($"\t DocumentWrite: {stopwatch2.ElapsedMilliseconds}ms", 2);
			stopwatch2.Reset();
			stopwatch2.Start();
			if (!designClone.IsBaseDesign)
			{
				SaveDesignHistoryImagesToDisk(designClone);
				GlobalErrorHandler.WriteInfoToEventLog($"\t SaveDesignImagesToDisk: {stopwatch2.ElapsedMilliseconds}ms");
			}
			activeProject.Designs.Add(designClone);
			activeProject.SaveProjectData();
			UIHelper.LoadedDesign = designClone;
			BroadcastActiveProjectChange();
			IsFileModified = false;
		}
		else
		{
			UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.SingleLineAlert, "No design is available to store in Job Bag.", messageIsModal: true);
		}
		stopwatch2.Stop();
		if (flag)
		{
			ViewModelLocator.Current.WindowDesignTryOnVM.TurnHandOn();
		}
		GlobalErrorHandler.WriteInfoToEventLog($"Total time to add Design to JobBag: {stopwatch.ElapsedMilliseconds}ms");
	}

	public static void AddDesignOnCanvasToTimedBackupProject(GV.CounterSketch.Studio.Model.Objects.Design currentDesign)
	{
		string backupFolder = Path.Combine(UIHelper.CSProjectPath, "~ Auto Save");
		if (!Directory.Exists(backupFolder))
		{
			Project item = new Project(backupFolder)
			{
				ProjectType = ProjectType.Portfolio
			};
			GlobalProjectCollection.Add(item);
			Directory.CreateDirectory(backupFolder);
			ViewModelLocator.Current.PortfolioManagerVM.NotifyPortfoliosUpdated();
		}
		Project project = GlobalProjectCollection.First((Project _) => _.Path == backupFolder);
		if (currentDesign.Clone() is GV.CounterSketch.Studio.Model.Objects.Design design)
		{
			design.IsLoadedFromLibrary = false;
			design.IsFavorite = false;
			design.CSUserDesignName = null;
			if (design.DesignId == 0 && currentDesign.BaseDesignInstance == Guid.Empty)
			{
				design.Designer = UIHelper.CounterSketchOptions.Username;
				design.SPINumber = "Freehand Design";
				design.DesignIdSet(0, 0);
				design.BaseDesignInstance = Guid.NewGuid();
				currentDesign.BaseDesignInstance = design.BaseDesignInstance;
			}
			if (project.BaseDesigns.All((GV.CounterSketch.Studio.Model.Objects.Design d) => d.BaseDesignInstance != currentDesign.BaseDesignInstance))
			{
				design.IsBaseDesign = true;
				design.BaseDesignInstance = currentDesign.BaseDesignInstance;
				design.DesignInstance = design.BaseDesignInstance;
				string path = $"{design.BaseDesignInstance}_{design.BaseDesignInstance}.csg";
				design.Path = Path.Combine(project.Path, path);
				if (File.Exists(design.BaseDesignImagePath))
				{
					File.SetAttributes(design.BaseDesignImagePath, FileAttributes.Normal);
					File.Delete(design.BaseDesignImagePath);
				}
				if (design.RenderFileCenterOffset == 0L)
				{
					UIHelper.CoreDataService.RequestScreenCaptureDefault(new System.Drawing.Size(400, 400), zoom: true, 3.0).Save(design.BaseDesignImagePath, ImageFormat.Png);
				}
				else
				{
					if (File.Exists(design.BaseDesignImagePath))
					{
						File.SetAttributes(design.BaseDesignImagePath, FileAttributes.Normal);
						File.Delete(design.BaseDesignImagePath);
					}
					if (design.DesignImageSource == null)
					{
						design.UpdateDesignImageSource();
					}
					ImageHelper.ExportBitmapImageAsPNG(design.DesignImageSource, design.BaseDesignImagePath);
				}
			}
			else
			{
				string path = $"{design.BaseDesignInstance}_{design.DesignInstance}.csg";
				design.Path = Path.Combine(project.Path, path);
				SaveDesignHistoryImagesToDisk(design);
			}
			string[] array = design.Path.Split('\\');
			design.Name = array[array.Length - 1];
			OrderHelper.ClearOrderInformationFromDesignUserData();
			UIHelper.CoreDataService.DocumentWrite(design.Path);
			project.Designs.Add(design);
		}
		IsFileBackedUp = true;
		List<GV.CounterSketch.Studio.Model.Objects.Design> first = (from _ in project.Designs
			where _.IsBaseDesign
			orderby _.ModifiedDate descending
			select _).Skip(10).ToList();
		List<GV.CounterSketch.Studio.Model.Objects.Design> second = (from _ in project.Designs
			where !_.IsBaseDesign
			orderby _.ModifiedDate descending
			select _).Skip(100).ToList();
		first.Union(second).ForEach(delegate(GV.CounterSketch.Studio.Model.Objects.Design _)
		{
			if (File.Exists(_.Path))
			{
				File.SetAttributes(_.Path, FileAttributes.Normal);
				File.Delete(_.Path);
			}
			if (File.Exists(_.FavoriteImagePath))
			{
				File.SetAttributes(_.FavoriteImagePath, FileAttributes.Normal);
				File.Delete(_.FavoriteImagePath);
			}
			if (File.Exists(_.FavoriteImagePathSearchOrientation))
			{
				File.SetAttributes(_.FavoriteImagePathSearchOrientation, FileAttributes.Normal);
				File.Delete(_.FavoriteImagePathSearchOrientation);
			}
		});
		project.SaveProjectData();
	}

	public static void AddDesignSearchFromLibraryToActiveProjectAsSearchFavorite(DesignSearch designSearch)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		if (designSearch == null)
		{
			return;
		}
		if (ActiveProject == null)
		{
			UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.SingleLineAlert, Strings.NoPortfolioCanStoreDesign, messageIsModal: true);
			return;
		}
		Project activeProject = ActiveProject;
		GV.CounterSketch.Studio.Model.Objects.Design design = GetDesignFromDesignSearch(designSearch);
		design.IsSearchFavorite = true;
		string path = $"{design.BaseDesignInstance}_{design.BaseDesignInstance}.csg";
		design.Path = Path.Combine(activeProject.Path, path);
		design = ExtractDesignFromLibrary(design);
		if (File.Exists(design.BaseDesignImagePath))
		{
			File.SetAttributes(design.BaseDesignImagePath, FileAttributes.Normal);
			File.Delete(design.BaseDesignImagePath);
		}
		if (design.DesignImageSource == null)
		{
			design.UpdateDesignImageSource();
		}
		ImageHelper.ExportBitmapImageAsPNG(design.DesignImageSource, design.BaseDesignImagePath);
		UIHelper.CoreDataService.MaterialsGetGem(delegate(GemMaterial[] gemMaterials, Exception exception)
		{
			if (exception != null)
			{
				throw exception;
			}
			design.MaterialGem1 = (int)gemMaterials[0];
			design.MaterialGem2 = (int)gemMaterials[1];
		});
		UIHelper.CoreDataService.MaterialsGetMetal(delegate(MetalMaterial[] metalMaterials, Exception exception)
		{
			if (exception != null)
			{
				throw exception;
			}
			design.MaterialMetal1 = (int)metalMaterials[0];
			design.MaterialMetal2 = (int)metalMaterials[1];
		});
		activeProject.Designs.Add(design);
		activeProject.SaveProjectData();
		stopwatch.Stop();
		DebuggingHelper.PrintTimedOutput($"Time to Save Favorite to Portfolio: {stopwatch.ElapsedMilliseconds}");
		BroadcastActiveProjectChange();
	}

	public static void BroadcastGlobalCollectionChange()
	{
		ViewModelLocator.Current.PortfolioManagerVM.NotifyPortfoliosUpdated();
	}

	public static void ClearTemporaryPortfolio()
	{
		if (IsTempProjectDirty())
		{
			DeleteProject(GlobalProjectCollection.FirstOrDefault((Project _) => _.Name == TempProjectName));
		}
	}

	public static bool ContinueAfterSaveDesignPrompt()
	{
		if (!IsDocumentCanvasEmpty() && IsFileModified)
		{
			ViewModelLocator.Current.MainVM.NavigateToSection(Sections.Customize);
			if (ViewModelLocator.Current.CustomizePartsListVM.ImportGroupsCollection.Any())
			{
				if (!UIHelper.IsApplicationClosing)
				{
					ViewModelLocator.Current.CustomizeVM.IsInDesignStackMode = false;
				}
			}
			else if (!UIHelper.IsApplicationClosing)
			{
				ViewModelLocator.Current.CustomizeVM.IsInDesignStackMode = true;
			}
			DispatcherHelper.WaitForPriority();
			UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.AcceptPrompt, string.Format(UntranslatedStrings.DoYouWantToSaveTheCurrentDesign, ActiveProject.Name), messageIsModal: true);
			if (UIHelper.LastMessageBoxCancelled == true)
			{
				return false;
			}
			if (UIHelper.LastMessageBoxAccepted == true)
			{
				AddDesignOnCanvasToHistoryOfActiveProject(UIHelper.LoadedDesign);
			}
			_isFileModified = false;
		}
		return true;
	}

	public static bool ContinueAfterSaveNewPortfolioPrompt()
	{
		if (!IsDocumentCanvasEmpty() && IsTempProjectDirty())
		{
			ViewModelLocator.Current.MainVM.SaveProjectString = string.Empty;
			ViewModelLocator.Current.MainVM.NavigateToSection(Sections.Customize);
			if (ViewModelLocator.Current.CustomizePartsListVM.ImportGroupsCollection.Any())
			{
				if (!UIHelper.IsApplicationClosing)
				{
					ViewModelLocator.Current.CustomizeVM.IsInDesignStackMode = false;
				}
			}
			else if (!UIHelper.IsApplicationClosing)
			{
				ViewModelLocator.Current.CustomizeVM.IsInDesignStackMode = true;
			}
			DispatcherHelper.WaitForPriority();
			UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.AcceptPrompt, Strings.UnsavedPortfolioPrompt, messageIsModal: true);
			if (UIHelper.LastMessageBoxAccepted == true)
			{
				AddDesignOnCanvasToHistoryOfActiveProject(UIHelper.LoadedDesign);
				_isFileModified = false;
				string question = $"Please provide a name for your portfolio. \n Cannot contain ({Path.GetInvalidFileNameChars()})";
				while (true)
				{
					WindowMessage windowMessage = new WindowMessage(WindowMessage.WindowType.QuestionAnswer)
					{
						Title = "Portfolio Name",
						Question = question,
						Topmost = true,
						Owner = Application.Current.MainWindow
					};
					windowMessage.ShowDialog();
					if (windowMessage.IsAccepted != true)
					{
						break;
					}
					if (RenameActiveProject(windowMessage.Answer))
					{
						return true;
					}
				}
				return false;
			}
			if (UIHelper.LastMessageBoxCancelled == true)
			{
				return false;
			}
		}
		return true;
	}

	public static void CopyActiveProject(string newProjectName)
	{
		Project project = CopyProject(ActiveProject, newProjectName);
		if (project != null)
		{
			SetActiveProject(project.Name);
		}
	}

	public static Project CopyProject(Project projectSource, string newProjectName)
	{
		string path = projectSource.Path;
		Project project = (Project)projectSource.Clone();
		string path2 = Path.Combine(UIHelper.CSProjectPath, newProjectName);
		if (Directory.Exists(path2))
		{
			UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.SingleLineAlert, Strings.CantSaveAsProjectAlreadyExists, messageIsModal: true);
			return null;
		}
		Directory.CreateDirectory(path2);
		project.Path = path2;
		project.Name = newProjectName;
		string destFileName = Path.Combine(project.Path, Path.GetFileName(path2) + ".csp");
		FileInfo fileInfo = new FileInfo(projectSource.ProjectInfoFilePath);
		if (fileInfo.Exists)
		{
			fileInfo.CopyTo(destFileName);
		}
		FileInfo[] files = new DirectoryInfo(path).GetFiles();
		foreach (FileInfo fileInfo2 in files)
		{
			if (fileInfo2.Extension != ".csp")
			{
				fileInfo2.CopyTo(Path.Combine(project.Path, fileInfo2.Name));
			}
		}
		foreach (GV.CounterSketch.Studio.Model.Objects.Design design in project.Designs)
		{
			string fileName = Path.GetFileName(design.Path);
			if (fileName != null)
			{
				design.Path = Path.Combine(project.Path, fileName);
			}
		}
		project.FirstName = string.Empty;
		project.LastName = string.Empty;
		project.PhoneNumber = string.Empty;
		project.Email = string.Empty;
		project.Address1 = string.Empty;
		project.Address2 = string.Empty;
		project.City = string.Empty;
		project.State = string.Empty;
		project.Zip = string.Empty;
		project.Comments = string.Empty;
		project.Country = string.Empty;
		project.SaveProjectData();
		GlobalProjectCollection.Add(project);
		GlobalProjectCollection.Sort(new ProjectComparer());
		BroadcastGlobalCollectionChange();
		BroadcastActiveProjectChange();
		return project;
	}

	public static void CreateNewProject(string projectName, CustomerInfo customerInfo)
	{
		CreateNewProject(projectName, customerInfo, ProjectType.Portfolio);
	}

	public static void CreateNewProject(string projectName, CustomerInfo customerInfo, ProjectType projectType)
	{
		string text = Path.Combine(UIHelper.CSProjectPath, projectName);
		if (Directory.Exists(text))
		{
			UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.SingleLineAlert, "Cannot create - Project already Exists!", messageIsModal: true);
			return;
		}
		Directory.CreateDirectory(text);
		Project project = new Project(text)
		{
			ProjectType = projectType
		};
		GlobalProjectCollection.Add(project);
		GlobalProjectCollection.Sort(new ProjectComparer());
		project.FirstName = customerInfo.FirstName;
		project.LastName = customerInfo.LastName;
		project.PhoneNumber = customerInfo.PhoneNumber;
		project.Email = customerInfo.Email;
		project.Address1 = customerInfo.Address1;
		project.Address2 = customerInfo.Address2;
		project.City = customerInfo.City;
		project.State = customerInfo.State;
		project.Zip = customerInfo.Zip;
		project.DefaultRingSizeIndex = customerInfo.DefaultRingSizeIndex;
		project.Comments = customerInfo.Comments;
		project.Country = customerInfo.Country;
		project.SaveProjectData();
		SetActiveProject(project.Name);
		BroadcastGlobalCollectionChange();
		BroadcastActiveProjectChange();
	}

	public static void DeleteDesignFromProject(GV.CounterSketch.Studio.Model.Objects.Design design)
	{
		Project project = GlobalProjectCollection.FirstOrDefault((Project projectParent) => projectParent.Designs.Contains(design));
		if (project == null)
		{
			return;
		}
		string[] array = Directory.GetFiles(searchPattern: (!design.IsBaseDesign) ? string.Concat(design.BaseDesignInstance, "_", design.DesignInstance, "*.*") : string.Concat(design.BaseDesignInstance, "*.*"), path: project.Path);
		foreach (string path in array)
		{
			File.SetAttributes(path, FileAttributes.Normal);
			File.Delete(path);
		}
		if (project != null)
		{
			project.Designs.Remove(design);
			if (project == ActiveProject)
			{
				BroadcastActiveProjectChange();
			}
		}
	}

	public static void DeleteProject(Project project)
	{
		GlobalProjectCollection.Remove(project);
		if (PermissionsHelper.VerifyFolderAccess(project.Path))
		{
			Directory.Delete(project.Path, recursive: true);
			if (project.Name == TempProjectName)
			{
				CreateTempProject();
			}
			if (project == ActiveProject || ActiveProject == null)
			{
				SetActiveProject(string.Empty);
			}
			BroadcastGlobalCollectionChange();
		}
	}

	public static void DeleteSearchFavoriteFromActiveProject(DesignSearch designSearch)
	{
		Project activeProject = ActiveProject;
		activeProject.Designs.Where((GV.CounterSketch.Studio.Model.Objects.Design d) => d.IsSearchFavorite && d.DesignId == designSearch.DesignId && d.DesignHistories.Count == 0).ToList().ForEach(delegate(GV.CounterSketch.Studio.Model.Objects.Design d)
		{
			DeleteDesignFromProject(d);
		});
		activeProject.Designs.Where((GV.CounterSketch.Studio.Model.Objects.Design d) => d.IsSearchFavorite && d.DesignId == designSearch.DesignId && d.DesignHistories.Count > 0).ForEach(delegate(GV.CounterSketch.Studio.Model.Objects.Design d)
		{
			d.IsSearchFavorite = false;
		});
	}

	public static GV.CounterSketch.Studio.Model.Objects.Design ExtractDesignFromLibrary(GV.CounterSketch.Studio.Model.Objects.Design design)
	{
		GlobalErrorHandler.WriteInfoToEventLog($"\t Extract from library: {design.DesignId}", 2);
		CSGLLibrary.GetCSGToFile(design.DesignId, design.Version, design.Path);
		return design;
	}

	public static GV.CounterSketch.Studio.Model.Objects.Design GetBaseDesignFromFilePath(string path)
	{
		GV.CounterSketch.Studio.Model.Objects.Design obj = new GV.CounterSketch.Studio.Model.Objects.Design(path)
		{
			SelectedIterationId = null,
			BaseDesignInstance = Guid.NewGuid()
		};
		obj.DesignInstance = obj.BaseDesignInstance;
		obj.IsBaseDesign = true;
		obj.IsLoadedFromLibrary = false;
		return obj;
	}

	public static GV.CounterSketch.Studio.Model.Objects.Design GetDesignFromDesignSearch(DesignSearch designSearch)
	{
		GV.CounterSketch.Studio.Model.Objects.Design design = new GV.CounterSketch.Studio.Model.Objects.Design();
		design.DesignIdSet((int)designSearch.DesignId, (int)designSearch.Version);
		design.Designer = designSearch.Designer;
		design.DesignImageSource = designSearch.DesignImageSource;
		design.MaterialMetal1 = designSearch.MaterialMetal1;
		design.MaterialMetal2 = designSearch.MaterialMetal2;
		design.MaterialGem1 = designSearch.MaterialGem1;
		design.MaterialGem2 = designSearch.MaterialGem2;
		design.MaterialGem3 = designSearch.MaterialGem3;
		design.SPINumber = designSearch.SPINumber;
		design.SelectedIterationId = null;
		design.BaseDesignInstance = Guid.NewGuid();
		design.DesignInstance = design.BaseDesignInstance;
		design.IsBaseDesign = true;
		design.IsLoadedFromLibrary = true;
		design.IsSearchFavorite = designSearch.IsSearchFavorite;
		return design;
	}

	public static Project GetProjectForDesign(GV.CounterSketch.Studio.Model.Objects.Design design)
	{
		Project result = null;
		if (design != null && design.Path != null)
		{
			FileInfo designPath = new FileInfo(design.Path);
			result = GlobalProjectCollection.Where((Project parent) => parent.Name == designPath.Directory.Name).FirstOrDefault();
		}
		return result;
	}

	public static void ImportBaseDesignFromDiskToActiveProjectAndOpenDesign(string pathToDesign)
	{
		if (pathToDesign != null)
		{
			if (ActiveProject == null)
			{
				UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.SingleLineAlert, Strings.NoPortfolioAvaliabeToSave, messageIsModal: true);
				return;
			}
			_ = ActiveProject;
			LoadHelper.LoadDesign(GetBaseDesignFromFilePath(pathToDesign));
		}
	}

	public static bool IsDocumentCanvasEmpty()
	{
		return UIHelper.CoreDataService.IsDocumentCanvasEmpty();
	}

	public static bool IsTempProjectDirty()
	{
		Project project = GlobalProjectCollection.FirstOrDefault((Project project2) => project2.Name == TempProjectName);
		if (project != null && project.Designs?.Count > 0)
		{
			return true;
		}
		return false;
	}

	public static void PopulateGlobalProjectCollection()
	{
		string[] array;
		if (Directory.Exists(UIHelper.CSProjectPath))
		{
			array = Directory.GetDirectories(UIHelper.CSProjectPath);
		}
		else
		{
			Directory.CreateDirectory(UIHelper.CSProjectPath);
			array = new string[0];
		}
		List<Project> list = GlobalProjectCollection.ToList();
		list.Clear();
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Project project = new Project(array2[i]);
			project.PopulateProjectDesignsCollection();
			list.Add(project);
		}
		list.Sort(new ProjectComparer());
		GlobalProjectCollection = list;
		BroadcastGlobalCollectionChange();
	}

	public static bool PromptForPortfolioSave()
	{
		if (ContinueAfterSaveNewPortfolioPrompt())
		{
			if (_isFileModified && ActiveProject.Name == TempProjectName && UIHelper.LastMessageBoxAccepted == false)
			{
				return true;
			}
			return ContinueAfterSaveDesignPrompt();
		}
		return false;
	}

	public static bool RenameActiveProject(string newProjectName)
	{
		return RenameProject(ActiveProject, newProjectName);
	}

	public static bool RenameProject(Project project, string newProjectName)
	{
		if (project.Name == newProjectName)
		{
			return false;
		}
		string text = VerifyProjectName(newProjectName);
		if (!string.IsNullOrEmpty(text))
		{
			UndoRenameProject(project);
			UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.SingleLineAlert, Strings.CannotPerformRename + " - " + text, messageIsModal: true);
			return false;
		}
		if (newProjectName != project.Name)
		{
			string path = project.Path;
			string text2 = Path.Combine(UIHelper.CSProjectPath, newProjectName);
			if (Directory.Exists(text2))
			{
				UndoRenameProject(project);
				UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.SingleLineAlert, Strings.CannotPerformRename + " - " + Strings.ProjectAlreadyExists + "!", messageIsModal: true);
				return false;
			}
			Directory.Move(path, text2);
			string path2 = Path.Combine(text2, project.Name + ".csp");
			if (File.Exists(path2))
			{
				File.Delete(path2);
			}
			project.Path = text2;
			project.Name = newProjectName;
			foreach (GV.CounterSketch.Studio.Model.Objects.Design design in project.Designs)
			{
				string fileName = Path.GetFileName(design.Path);
				if (fileName != null)
				{
					design.Path = Path.Combine(project.Path, fileName);
				}
			}
			project.SaveProjectData();
			CreateTempProject();
			BroadcastGlobalCollectionChange();
			SetActiveProject(newProjectName);
		}
		return true;
	}

	public static void ResetProjectManagement()
	{
		PopulateGlobalProjectCollection();
		SetActiveProject(string.Empty);
	}

	public static string ResetTempPortfolio(string projectName)
	{
		Project project = GlobalProjectCollection.FirstOrDefault((Project project2) => project2.Name == TempProjectName);
		if (project != null)
		{
			ActiveProject = project;
			if (!string.IsNullOrEmpty(projectName))
			{
				string text = VerifyProjectName(projectName);
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
				RenameActiveProject(projectName);
			}
			else
			{
				DeleteProject(ActiveProject);
			}
			BroadcastGlobalCollectionChange();
			BroadcastActiveProjectChange();
		}
		return string.Empty;
	}

	public static void UndoRenameProject(Project project)
	{
		string[] array = project.Path.Split('\\');
		project.Name = array[array.Length - 1];
	}

	public static string VerifyProjectName(string projectName)
	{
		if (Path.GetInvalidFileNameChars().Any((char invalidCharacter) => projectName.Contains(invalidCharacter)))
		{
			return Strings.InvalidCharacterInName + "!";
		}
		if (!string.IsNullOrEmpty(projectName))
		{
			if (Directory.Exists(Path.Combine(UIHelper.CSProjectPath, projectName)))
			{
				return Strings.EnteredNameAlreadyExists + "!";
			}
			return string.Empty;
		}
		return Strings.NameCannotBeBlank + "!";
	}

	private static void BroadcastActiveProjectChange()
	{
		if (ActiveProject != null)
		{
			ActiveProject.SaveProjectData();
		}
		ViewModelLocator.Current.PortfolioManagerVM.NotifyActiveProjectUpdated();
	}

	private static void CreateTempProject()
	{
		string path = Path.Combine(UIHelper.CSProjectPath, TempProjectName);
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
			GlobalProjectCollection.Add(new Project(Path.Combine(UIHelper.CSProjectPath, TempProjectName))
			{
				ProjectType = ProjectType.Portfolio
			});
			GlobalProjectCollection.Sort(new ProjectComparer());
			BroadcastGlobalCollectionChange();
		}
	}

	private static void OpenInitialCustomerProject()
	{
		CreateTempProject();
		string text = Path.Combine(UIHelper.CSProjectPath, TempProjectName);
		string projectToOpen = text;
		if (UIHelper.CounterSketchOptions.OpenLastOpenedPortfolio && !string.IsNullOrEmpty(UIHelper.CounterSketchOptions.LastOpenedPortfolio) && Directory.Exists(UIHelper.CounterSketchOptions.LastOpenedPortfolio))
		{
			projectToOpen = UIHelper.CounterSketchOptions.LastOpenedPortfolio;
		}
		Project project = (from Project project2 in CustomerProjectCollection
			where project2.Path == projectToOpen
			select project2).FirstOrDefault();
		if (project != null)
		{
			ActiveProject = project;
		}
	}

	private static void SaveDesignHistoryImagesToDisk(GV.CounterSketch.Studio.Model.Objects.Design design)
	{
		if (design != null)
		{
			Bitmap bitmap = UIHelper.CoreDataService.RequestScreenCapture(new System.Drawing.Size(180, 128));
			string favoriteImagePath = design.FavoriteImagePath;
			if (File.Exists(favoriteImagePath))
			{
				File.SetAttributes(favoriteImagePath, FileAttributes.Normal);
				File.Delete(favoriteImagePath);
			}
			bitmap.Save(favoriteImagePath, ImageFormat.Png);
			favoriteImagePath = design.FavoriteImagePathSearchOrientation;
			if (File.Exists(favoriteImagePath))
			{
				File.SetAttributes(favoriteImagePath, FileAttributes.Normal);
				File.Delete(favoriteImagePath);
			}
			Bitmap bitmap2 = UIHelper.CoreDataService.RequestScreenCaptureDefault(new System.Drawing.Size(400, 400), zoom: true, 3.0);
			bitmap2.Save(favoriteImagePath, ImageFormat.Png);
			bitmap2.Dispose();
			design.NotifyImagesUpdated();
		}
	}

	private static void SetActiveProject(string projectName)
	{
		ActiveProject = CustomerProjectCollection.FirstOrDefault((Project project) => project.Name == projectName);
		if (ActiveProject == null)
		{
			OpenInitialCustomerProject();
		}
	}
}
