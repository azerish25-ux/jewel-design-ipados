using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class DesignStackViewModel : ViewModelBase
{
	private IEnumerable<UIControl> _cachedUiControlCollection;

	private ObservableCollection<DesignComponentDisplay> _designComponents;

	private ObservableCollection<UIControl> _designOptions;

	private ObservableCollection<UIElement> _designOptionsUiCollection;

	private ObservableCollection<DesignStackItemContainer> _designStackItemContainers;

	private bool _isDesignComponentsSelectorVisible;

	private bool _isDocumentLoaded;

	private bool _isFirstControlGroupExpanded;

	private bool _isLoading;

	private DesignComponentDisplay _selectedDesignComponentDisplay;

	private Dictionary<string, string> _standardControlNameReplacements = new Dictionary<string, string>();

	private List<string> _simpleStackControlListPriority = new List<string>();

	private bool _isDisplayingSimpleStack;

	private List<Tuple<ComponentsEnum, Guid>> _simpleStackDisplayedControlGuids;

	private Dictionary<Guid, int> _originalSortIndexLookup;

	private Collection<UIControl> _expandedControls;

	private bool _hasExpandedSliders;

	public RelayCommand<UIControl> CommandResetUIControl { get; set; }

	public RelayCommand<SelectionChangedEventArgs> CommandSelectedFeaturedIterationChanged { get; private set; }

	public RelayCommand CommandUpdateDesignStackCategoryFromComponents { get; set; }

	public RelayCommand<DesignComponentDisplay> CommandUpdateVisibleDisplayComponents { get; protected set; }

	public ObservableCollection<DesignComponentDisplay> DesignComponents
	{
		get
		{
			return _designComponents ?? new ObservableCollection<DesignComponentDisplay>();
		}
		set
		{
			_designComponents = value;
			RaisePropertyChanged(() => DesignComponents);
		}
	}

	public ObservableCollection<UIControl> DesignOptions
	{
		get
		{
			return _designOptions;
		}
		set
		{
			_designOptions = value;
			RaisePropertyChanged(() => DesignOptions);
		}
	}

	public ObservableCollection<UIElement> DesignOptionsUICollection
	{
		get
		{
			return _designOptionsUiCollection;
		}
		set
		{
			_designOptionsUiCollection = value;
			RaisePropertyChanged(() => DesignOptionsUICollection);
		}
	}

	public ObservableCollection<DesignStackItemContainer> DesignStackItemContainers
	{
		get
		{
			return _designStackItemContainers;
		}
		set
		{
			_designStackItemContainers = value;
			RaisePropertyChanged(() => DesignStackItemContainers);
		}
	}

	public bool IsDesignComponentsSelectorVisible
	{
		get
		{
			return _isDesignComponentsSelectorVisible;
		}
		set
		{
			_isDesignComponentsSelectorVisible = value;
			RaisePropertyChanged(() => IsDesignComponentsSelectorVisible);
		}
	}

	public bool IsDocumentLoaded
	{
		get
		{
			return _isDocumentLoaded;
		}
		set
		{
			if (_isDocumentLoaded != value)
			{
				_isDocumentLoaded = value;
				RaisePropertyChanged(() => IsDocumentLoaded);
			}
		}
	}

	public bool IsFirstGroupExpanded
	{
		get
		{
			return _isFirstControlGroupExpanded;
		}
		set
		{
			if (_isFirstControlGroupExpanded != value)
			{
				_isFirstControlGroupExpanded = value;
				RaisePropertyChanged(() => IsFirstGroupExpanded);
			}
		}
	}

	public bool IsLoading
	{
		get
		{
			return _isLoading;
		}
		set
		{
			if (_isLoading != value)
			{
				_isLoading = value;
				RaisePropertyChanged(() => IsLoading);
			}
		}
	}

	public DesignComponentDisplay SelectedDesignComponentDisplay
	{
		get
		{
			return _selectedDesignComponentDisplay;
		}
		set
		{
			_selectedDesignComponentDisplay = value;
			RaisePropertyChanged(() => SelectedDesignComponentDisplay);
		}
	}

	public bool IsDisplayingSimpleStack
	{
		get
		{
			return _isDisplayingSimpleStack;
		}
		set
		{
			_isDisplayingSimpleStack = value;
			RaisePropertyChanged(() => IsDisplayingSimpleStack);
			Application.Current.Dispatcher.InvokeAsync(delegate
			{
				InitializeStack(_cachedUiControlCollection, isUsingCachedControls: true);
			}, DispatcherPriority.ContextIdle);
		}
	}

	public Collection<UIControl> ExpandedControls
	{
		get
		{
			return _expandedControls;
		}
		set
		{
			_expandedControls = value;
			RaisePropertyChanged("ExpandedControls");
			HasExpandedSliders = value.Any();
		}
	}

	public bool HasExpandedSliders
	{
		get
		{
			if (UIHelper.CounterSketchOptions.UserTypeId < 8)
			{
				return false;
			}
			return _hasExpandedSliders;
		}
		set
		{
			_hasExpandedSliders = value;
			RaisePropertyChanged("HasExpandedSliders");
		}
	}

	public DesignStackViewModel()
	{
		if (base.IsInDesignMode)
		{
			DesignComponents = new ObservableCollection<DesignComponentDisplay>
			{
				new DesignComponentDisplay
				{
					IsEnabled = false,
					Name = Enum.GetName(typeof(ComponentsEnum), ComponentsEnum.Ring)
				},
				new DesignComponentDisplay
				{
					IsEnabled = true,
					Name = Enum.GetName(typeof(ComponentsEnum), ComponentsEnum.Band1)
				},
				new DesignComponentDisplay
				{
					IsEnabled = false,
					Name = Enum.GetName(typeof(ComponentsEnum), ComponentsEnum.Band2)
				}
			};
		}
		else
		{
			InitializeCommands();
			Messenger.Default.Register<string>(this, ProcessGenericMessage);
			Messenger.Default.Register<NotificationMessage<UIControl>>(this, ProcessUIControlNotificationMessage);
		}
	}

	public void ResetUIControl(UIControl uIControl)
	{
		CSUIControl cSUIControl = DesignStackItemContainers.SelectMany((DesignStackItemContainer container) => container.CategoryControls).SelectMany((CSCategoryControl ui) => from CSUIControl _ in ui.Children
			select (_)).ToList()
			.OfType<CSUIControl>()
			.FirstOrDefault((CSUIControl csuiControl) => csuiControl != null && csuiControl.LocalUIControl.EnglishControlName == uIControl.EnglishControlName && csuiControl.LocalUIControl.BlockInstanceGuid == uIControl.BlockInstanceGuid);
		if (cSUIControl != null && cSUIControl is CSValueRangeControl cSValueRangeControl)
		{
			cSValueRangeControl.Reset();
		}
	}

	public void ExpandFirstControlGroup()
	{
		DesignStackItemContainer designStackItemContainer = DesignStackItemContainers.FirstOrDefault();
		if (designStackItemContainer == null || designStackItemContainer.CategoryControls == null)
		{
			return;
		}
		CSCategoryControl firstControl = designStackItemContainer.CategoryControls.FirstOrDefault();
		if (firstControl != null)
		{
			firstControl.Collapsed = false;
			CollapseAllButOne(firstControl);
			Application.Current.Dispatcher.BeginInvoke((Action)delegate
			{
				firstControl.TryFindParent<ScrollViewer>()?.ScrollToTop();
			}, DispatcherPriority.Background);
		}
	}

	public void ExpandLastControlGroup()
	{
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			if (DesignStackItemContainers != null)
			{
				DesignStackItemContainer designStackItemContainer = DesignStackItemContainers.LastOrDefault((DesignStackItemContainer container) => container.IsVisible);
				if (designStackItemContainer != null && designStackItemContainer.CategoryControls != null)
				{
					CSCategoryControl cSCategoryControl = designStackItemContainer.CategoryControls.LastOrDefault((CSCategoryControl categoryControl) => categoryControl.Visibility == Visibility.Visible);
					if (cSCategoryControl != null)
					{
						cSCategoryControl.Collapsed = false;
						cSCategoryControl.TryFindParent<ListBox>()?.ScrollIntoView(cSCategoryControl);
					}
				}
			}
		}, DispatcherPriority.Background);
	}

	public void InitializeStack(IEnumerable<UIControl> uiControls, bool isUsingCachedControls = false)
	{
		FileLoadSimpleStackControlList();
		FileLoadStandardControlNameList();
		if (isUsingCachedControls)
		{
			Reset();
		}
		else
		{
			_isDisplayingSimpleStack = UIHelper.CounterSketchOptions.IsHideAdvancedStackDefault;
			RaisePropertyChanged(() => IsDisplayingSimpleStack);
			_originalSortIndexLookup = new Dictionary<Guid, int>(uiControls.ToDictionary((UIControl control) => control.BlockInstanceGuid, (UIControl control) => control.SortIndex));
		}
		PopulateDesignStackUIControls(uiControls);
		UpdateDesignStackDisplayComponents(!isUsingCachedControls);
		UpdateDesignStackCategoryFromComponents();
	}

	public void PopulateDesignStackUIControls(IEnumerable<UIControl> uiControls)
	{
		UnregisterControlsFromMessengerAndEventHandlers();
		ClearComponentContainersAndCategories();
		List<string> distinctCategories = new List<string>(uiControls.Select((UIControl uiControl) => uiControl.EnglishCategoryName).Distinct()).ToList();
		uiControls = uiControls.Where((UIControl uIControl) => uIControl.ImportGroupIdentifier == Guid.Empty);
		uiControls.ToList().ForEach(UpdateControlNamesAndTranslate);
		ExpandedControls = new Collection<UIControl>(uiControls.Where((UIControl _) => _.HasMissingObject).ToList());
		IList<UIControl> list = (uiControls as IList<UIControl>) ?? uiControls.ToList();
		if (!list.Any())
		{
			return;
		}
		foreach (UIControl item in from uiControl in list
			where uiControl.CurrentUIControlType == UIControlType.CSValueRingSize || uiControl.CurrentUIControlType == UIControlType.CSValueOnOff
			orderby distinctCategories.IndexOf(uiControl.EnglishCategoryName), uiControl.SortIndex
			select uiControl)
		{
			DesignOptionsUICollection.Add(CSCategoryControl.CreateControlFromUIControlObject(item));
		}
		foreach (UIControl item2 in list.Where((UIControl uiControl) => uiControl.CurrentUIControlType != UIControlType.CSValueRingSize && uiControl.CurrentUIControlType != UIControlType.CSValueOnOff))
		{
			ComponentsEnum controlComponent = item2.AssignedDisplayComponents.FirstOrDefault();
			if (IsDisplayingSimpleStack && _simpleStackControlListPriority.Any())
			{
				if (_simpleStackDisplayedControlGuids.Count((Tuple<ComponentsEnum, Guid> _) => _.Item1 == controlComponent) >= 10 || !_simpleStackControlListPriority.Contains(item2.EnglishControlName))
				{
					continue;
				}
				item2.SortIndex = _simpleStackControlListPriority.IndexOf(item2.EnglishControlName);
				_simpleStackDisplayedControlGuids.Add(new Tuple<ComponentsEnum, Guid>(controlComponent, item2.BlockInstanceGuid));
			}
			else
			{
				int value = 0;
				if (_originalSortIndexLookup.TryGetValue(item2.BlockInstanceGuid, out value))
				{
					item2.SortIndex = value;
				}
			}
			UIControl control = item2;
			CSCategoryControl cSCategoryControl = DesignStackItemContainers.SelectMany((DesignStackItemContainer container) => container.CategoryControls).FirstOrDefault((CSCategoryControl category) => category.EnglishCategoryName == (IsDisplayingSimpleStack ? (DesignStrings.DesignOptions + " - " + controlComponent) : control.EnglishCategoryName));
			if (cSCategoryControl == null)
			{
				cSCategoryControl = new CSCategoryControl
				{
					CategoryName = (IsDisplayingSimpleStack ? (DesignStrings.DesignOptions + " - " + controlComponent) : item2.CategoryName),
					EnglishCategoryName = (IsDisplayingSimpleStack ? (DesignStrings.DesignOptions + " - " + controlComponent) : item2.EnglishCategoryName),
					Title = (IsDisplayingSimpleStack ? (DesignStrings.DesignOptions + " - " + controlComponent) : item2.CategoryName),
					DeleteVisibility = ((!item2.CategoryDeletable) ? Visibility.Collapsed : Visibility.Visible)
				};
				cSCategoryControl.TitleClicked += SectionControlTitleClicked;
				ComponentsEnum assignedComponent = item2.AssignedDisplayComponents.FirstOrDefault();
				if (assignedComponent != ComponentsEnum.None)
				{
					DesignStackItemContainer designStackItemContainer = DesignStackItemContainers.FirstOrDefault((DesignStackItemContainer container) => container.AssociatedComponent == assignedComponent);
					if (designStackItemContainer == null)
					{
						designStackItemContainer = new DesignStackItemContainer
						{
							AssociatedComponent = assignedComponent,
							CategoryControls = new ObservableCollection<CSCategoryControl>()
						};
						designStackItemContainer.ContainerName = designStackItemContainer.ContainerName.ToUpper();
						DesignStackItemContainers.Add(designStackItemContainer);
					}
					designStackItemContainer.CategoryControls.Add(cSCategoryControl);
				}
			}
			cSCategoryControl.AddChild(CSCategoryControl.CreateControlFromUIControlObject(item2));
			cSCategoryControl.Sort();
		}
		if (UIHelper.CounterSketchOptions.ExpandStackControl || IsDisplayingSimpleStack)
		{
			ExpandFirstControlGroup();
		}
		else
		{
			CollapseAllButOne();
		}
		_cachedUiControlCollection = list;
		IsDocumentLoaded = true;
		ProjectManagementHelper.IsFileModified = false;
	}

	public void Reset()
	{
		UIHelper.CoreDataService.DocumentSetCurvesAndPointsVisibility(visible: false);
		UnregisterControlsFromMessengerAndEventHandlers();
		ClearComponentContainersAndCategories();
		ProjectManagementHelper.IsFileModified = false;
		IsDocumentLoaded = false;
	}

	public void UpdateDesignStackCategoryFromComponents()
	{
		if (SelectedDesignComponentDisplay != null && !SelectedDesignComponentDisplay.IsEnabled)
		{
			DesignComponentDisplay designComponentDisplay = DesignComponents.FirstOrDefault((DesignComponentDisplay designComponent) => designComponent.IsEnabled) ?? DesignComponents.FirstOrDefault();
			UpdateVisibleDisplayComponents(designComponentDisplay);
		}
		foreach (DesignStackItemContainer designStackItemContainer in DesignStackItemContainers)
		{
			designStackItemContainer.IsVisible = SelectedDesignComponentDisplay != null && Enumerable.Contains(UIHelper.VisibleComponents, designStackItemContainer.AssociatedComponent) && Enum.GetName(typeof(ComponentsEnum), designStackItemContainer.AssociatedComponent) == SelectedDesignComponentDisplay.Name && designStackItemContainer.CategoryControls.Any();
		}
		if (DesignOptionsUICollection != null)
		{
			(from control in DesignOptionsUICollection.OfType<CSUIControl>()
				where !(control is CSValueRingSizeControl)
				select control).ToList().ForEach(delegate(CSUIControl control)
			{
				control.Visibility = ((SelectedDesignComponentDisplay == null || !UIHelper.VisibleComponents.Intersect(control.AssignedDisplayComponents).Any() || !control.AssignedDisplayComponents.Contains((ComponentsEnum)Enum.Parse(typeof(ComponentsEnum), SelectedDesignComponentDisplay.Name))) ? Visibility.Collapsed : Visibility.Visible);
			});
		}
	}

	public void UpdateDesignStackDisplayComponents(bool isResetingComponents = false)
	{
		DesignComponentDisplay selectedDesignComponentDisplay = null;
		if (isResetingComponents)
		{
			DesignComponents = new ObservableCollection<DesignComponentDisplay>();
			IsDesignComponentsSelectorVisible = DesignComponents.Count > 1;
		}
		else
		{
			selectedDesignComponentDisplay = SelectedDesignComponentDisplay;
		}
		UIHelper.CoreDataService.GetComponents(delegate(IEnumerable<ComponentsEnum> availableComponents)
		{
			if (availableComponents != null)
			{
				IList<ComponentsEnum> source = (availableComponents as IList<ComponentsEnum>) ?? availableComponents.ToList();
				List<int> componentIds = new List<int>(source.Select((ComponentsEnum x) => (int)x));
				UIHelper.LoadedDesign.ComponentIds = componentIds;
				DesignComponents = new ObservableCollection<DesignComponentDisplay>(source.Select((ComponentsEnum component) => new DesignComponentDisplay
				{
					IsEnabled = false,
					Name = Enum.GetName(typeof(ComponentsEnum), component)
				}));
				IsDesignComponentsSelectorVisible = DesignComponents.Count > 1;
			}
		});
		UIHelper.CoreDataService.GetVisibleComponents(delegate(IEnumerable<ComponentsEnum> visibleComponents)
		{
			IList<ComponentsEnum> obj = (visibleComponents as IList<ComponentsEnum>) ?? visibleComponents.ToList();
			UIHelper.VisibleComponents = new List<ComponentsEnum>(obj);
			obj.ToList().ForEach(delegate(ComponentsEnum visibleComponent)
			{
				DesignComponentDisplay designComponentDisplay = DesignComponents.FirstOrDefault((DesignComponentDisplay component) => component.Name == Enum.GetName(typeof(ComponentsEnum), visibleComponent));
				if (designComponentDisplay != null)
				{
					designComponentDisplay.IsEnabled = true;
				}
			});
		});
		DesignStackItemContainers = new ObservableCollection<DesignStackItemContainer>(_designStackItemContainers ?? new ObservableCollection<DesignStackItemContainer>());
		if (isResetingComponents)
		{
			SelectedDesignComponentDisplay = DesignComponents.FirstOrDefault();
		}
		else
		{
			SelectedDesignComponentDisplay = selectedDesignComponentDisplay;
		}
	}

	public void UpdateDesignStackUIControls(IEnumerable<UIControl> uiControls, Exception exception)
	{
		if (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
		{
			return;
		}
		if (exception != null)
		{
			throw exception;
		}
		if (uiControls == null)
		{
			return;
		}
		ExpandedControls = new Collection<UIControl>(uiControls.Where((UIControl _) => _.HasMissingObject).ToList());
		uiControls = uiControls.Where((UIControl uIControl) => uIControl.ImportGroupIdentifier == Guid.Empty);
		uiControls.ToList().ForEach(UpdateControlNamesAndTranslate);
		IList<UIControl> cachedUIControlCollection = (uiControls as IList<UIControl>) ?? uiControls.ToList();
		foreach (UIControl item in cachedUIControlCollection.Where((UIControl uIControl) => uIControl.CurrentUIControlStatus == UIControlStatus.Changed))
		{
			UIControl control = item;
			if (control.CurrentUIControlType == UIControlType.CSValueRingSize || control.CurrentUIControlType == UIControlType.CSValueOnOff)
			{
				if (DesignOptionsUICollection != null)
				{
					CSUIControl cSUIControl = DesignOptionsUICollection.OfType<CSUIControl>().FirstOrDefault((CSUIControl csuiControl) => csuiControl != null && csuiControl.LocalUIControl.EnglishControlName == control.EnglishControlName && csuiControl.LocalUIControl.BlockInstanceGuid == control.BlockInstanceGuid);
					if (cSUIControl != null)
					{
						cSUIControl.UpdateUIControlSource(item);
					}
					else
					{
						DesignOptionsUICollection.Add(CSCategoryControl.CreateControlFromUIControlObject(item));
					}
				}
			}
			else
			{
				if (DesignStackItemContainers == null)
				{
					continue;
				}
				ComponentsEnum controlComponent = item.AssignedDisplayComponents.FirstOrDefault();
				if (IsDisplayingSimpleStack && !_simpleStackDisplayedControlGuids.Select((Tuple<ComponentsEnum, Guid> _) => _.Item2).Contains(item.BlockInstanceGuid))
				{
					continue;
				}
				CSCategoryControl cSCategoryControl = DesignStackItemContainers.SelectMany((DesignStackItemContainer container) => container.CategoryControls).FirstOrDefault((CSCategoryControl category) => category.EnglishCategoryName == (IsDisplayingSimpleStack ? (DesignStrings.DesignOptions + " - " + controlComponent) : control.EnglishCategoryName));
				if (cSCategoryControl != null)
				{
					cSCategoryControl.UpdateCSUIControlSource(item);
					continue;
				}
				cSCategoryControl = new CSCategoryControl
				{
					CategoryName = (IsDisplayingSimpleStack ? (DesignStrings.DesignOptions + " - " + controlComponent) : item.CategoryName),
					EnglishCategoryName = (IsDisplayingSimpleStack ? (DesignStrings.DesignOptions + " - " + controlComponent) : item.EnglishCategoryName),
					Title = (IsDisplayingSimpleStack ? (DesignStrings.DesignOptions + " - " + controlComponent) : item.CategoryName),
					DeleteVisibility = ((!item.CategoryDeletable) ? Visibility.Collapsed : Visibility.Visible)
				};
				cSCategoryControl.TitleClicked += SectionControlTitleClicked;
				ComponentsEnum assignedComponent = item.AssignedDisplayComponents.FirstOrDefault();
				if (assignedComponent != ComponentsEnum.None)
				{
					DesignStackItemContainer designStackItemContainer = DesignStackItemContainers.FirstOrDefault((DesignStackItemContainer container) => container.AssociatedComponent == assignedComponent);
					if (designStackItemContainer == null)
					{
						designStackItemContainer = new DesignStackItemContainer
						{
							AssociatedComponent = assignedComponent,
							CategoryControls = new ObservableCollection<CSCategoryControl>()
						};
						DesignStackItemContainers.Add(designStackItemContainer);
					}
					designStackItemContainer.CategoryControls.Add(cSCategoryControl);
				}
			}
		}
		if (_cachedUiControlCollection != null)
		{
			foreach (UIControl item2 in _cachedUiControlCollection.Where((UIControl uIControl) => cachedUIControlCollection.All((UIControl uiControl) => uiControl.EnglishControlName != uIControl.EnglishControlName)))
			{
				UIControl control2 = item2;
				if (control2.CurrentUIControlType == UIControlType.CSValueRingSize || control2.CurrentUIControlType == UIControlType.CSValueOnOff)
				{
					if (DesignOptionsUICollection != null)
					{
						CSUIControl cSUIControl2 = DesignOptionsUICollection.OfType<CSUIControl>().FirstOrDefault((CSUIControl csuiControl) => csuiControl != null && csuiControl.LocalUIControl.EnglishControlName == control2.EnglishControlName && csuiControl.LocalUIControl.BlockInstanceGuid == control2.BlockInstanceGuid);
						if (cSUIControl2 != null)
						{
							DesignOptionsUICollection.Remove(cSUIControl2);
						}
					}
				}
				else
				{
					if (DesignStackItemContainers == null)
					{
						continue;
					}
					CSCategoryControl cSCategoryControl2 = DesignStackItemContainers.SelectMany((DesignStackItemContainer container) => container.CategoryControls).FirstOrDefault((CSCategoryControl category) => category.EnglishCategoryName == control2.EnglishCategoryName);
					if (cSCategoryControl2 == null)
					{
						continue;
					}
					cSCategoryControl2.RemoveChildContaining(item2);
					if (cSCategoryControl2.Children.Count != 0 || DesignStackItemContainers == null)
					{
						continue;
					}
					DesignStackItemContainers.FirstOrDefault((DesignStackItemContainer container) => container.CategoryControls.Any((CSCategoryControl category) => category.EnglishCategoryName == control2.EnglishCategoryName))?.CategoryControls.Remove(cSCategoryControl2);
				}
			}
		}
		_cachedUiControlCollection = cachedUIControlCollection;
	}

	public void UpdateStackFromCurrentIterationId()
	{
		UIHelper.CoreDataService.SetDesignIteration(UIHelper.LoadedDesign.SelectedIterationId, UpdateDesignStackUIControls);
	}

	private void ClearComponentContainersAndCategories()
	{
		if (DesignStackItemContainers == null)
		{
			DesignStackItemContainers = new ObservableCollection<DesignStackItemContainer>();
		}
		if (DesignOptionsUICollection == null)
		{
			DesignOptionsUICollection = new ObservableCollection<UIElement>();
		}
		DesignStackItemContainers.ToList().ForEach(delegate(DesignStackItemContainer container)
		{
			if (container.CategoryControls != null)
			{
				container.CategoryControls.Clear();
			}
		});
		DesignStackItemContainers.Clear();
		DesignOptionsUICollection.Clear();
		_simpleStackDisplayedControlGuids = new List<Tuple<ComponentsEnum, Guid>>();
	}

	private void CollapseAllButOne(object expandedControl = null)
	{
		foreach (CSCategoryControl item in from categoryControl in DesignStackItemContainers.SelectMany((DesignStackItemContainer container) => container.CategoryControls)
			where categoryControl != null && !object.Equals(categoryControl, expandedControl)
			select categoryControl)
		{
			item.Collapsed = true;
		}
	}

	private void FileLoadSimpleStackControlList()
	{
		string path = Path.Combine(UIHelper.CSRootPath, "SimpleStackControls.txt");
		if (File.Exists(path))
		{
			string[] collection = File.ReadAllLines(path);
			_simpleStackControlListPriority = new List<string>(collection);
		}
	}

	private void FileLoadStandardControlNameList()
	{
		string path = Path.Combine(UIHelper.CSRootPath, "StandardControlNames.txt");
		if (!File.Exists(path))
		{
			return;
		}
		var list = (from _ in File.ReadAllLines(path)
			select new
			{
				ControlKey = _.Split('|').FirstOrDefault()?.Replace(" ", string.Empty).Replace("-", string.Empty).Replace(":", string.Empty)
					.Trim()
					.ToLowerInvariant(),
				ControlValue = _.Split('|').LastOrDefault()?.Trim()
			}).ToList();
		_standardControlNameReplacements = new Dictionary<string, string>();
		list.ForEach(_ =>
		{
			if (!_standardControlNameReplacements.ContainsKey(_.ControlKey))
			{
				_standardControlNameReplacements.Add(_.ControlKey, _.ControlValue);
			}
		});
	}

	private void InitializeCommands()
	{
		CommandSelectedFeaturedIterationChanged = new RelayCommand<SelectionChangedEventArgs>(SelectedFeaturedIterationChanged);
		CommandUpdateVisibleDisplayComponents = new RelayCommand<DesignComponentDisplay>(UpdateVisibleDisplayComponents);
		CommandUpdateDesignStackCategoryFromComponents = new RelayCommand(UpdateDesignStackCategoryFromComponents);
		CommandResetUIControl = new RelayCommand<UIControl>(ResetUIControl);
	}

	private void ProcessGenericMessage(string message)
	{
	}

	private void ProcessUIControlNotificationMessage(NotificationMessage<UIControl> notificationMessage)
	{
		switch (notificationMessage.Notification)
		{
		case "SetUIControlFull":
			UIHelper.CoreDataService.SetUIControl(notificationMessage.Content, quickSet: false, UpdateDesignStackUIControls);
			ProjectManagementHelper.IsFileModified = true;
			break;
		case "SetUIControlQuick":
			UIHelper.CoreDataService.SetUIControl(notificationMessage.Content, quickSet: true, UpdateDesignStackUIControls);
			ProjectManagementHelper.IsFileModified = true;
			break;
		case "AccessCurveEditor":
			UIHelper.CoreDataService.CurveEditorAccessCurveEditor(notificationMessage.Content, createAsCopy: false, UpdateDesignStackUIControls);
			break;
		case "AccessCurveEditorCopy":
			UIHelper.CoreDataService.CurveEditorAccessCurveEditor(notificationMessage.Content, createAsCopy: true, UpdateDesignStackUIControls);
			break;
		}
	}

	private void SectionControlTitleClicked(object sender, RoutedEventArgs e)
	{
		CollapseAllButOne(sender);
	}

	private void SelectedFeaturedIterationChanged(SelectionChangedEventArgs obj)
	{
		if (obj.AddedItems.Count > 0 && obj.AddedItems[0] is DesignIteration designIteration)
		{
			UIHelper.LoadedDesign.SelectedIterationId = designIteration.IterationId;
			UpdateStackFromCurrentIterationId();
		}
	}

	private void SetHandEnabled(bool isHandEnabled)
	{
		if (!isHandEnabled)
		{
			ViewModelLocator.Current.WindowDesignTryOnVM.TurnHandOff();
		}
		ViewModelLocator.Current.WindowDesignTryOnVM.IsHandEnabled = isHandEnabled;
	}

	private void UnregisterControlsFromMessengerAndEventHandlers()
	{
		if (DesignStackItemContainers == null)
		{
			return;
		}
		foreach (CSCategoryControl item in DesignStackItemContainers.SelectMany((DesignStackItemContainer container) => container.CategoryControls))
		{
			item.UnregisterControlsFromMessenger();
			item.UnwireControlsFromStoneTrayViewModel();
		}
	}

	private void UpdateControlNamesAndTranslate(UIControl uiControl)
	{
		if (_standardControlNameReplacements.Any() && _standardControlNameReplacements.TryGetValue(uiControl.EnglishControlName.Replace(" ", string.Empty).Replace("-", string.Empty).Replace(":", string.Empty)
			.Trim()
			.ToLowerInvariant(), out var value))
		{
			uiControl.EnglishControlName = value;
		}
		uiControl.ControlName = TranslationHelper.GetTranslatedDesignOrPartString(uiControl.EnglishControlName);
		uiControl.CategoryName = TranslationHelper.GetTranslatedDesignOrPartString(uiControl.EnglishCategoryName);
	}

	private void UpdateVisibleDisplayComponents(DesignComponentDisplay designComponentDisplay)
	{
		Enum.GetNames(typeof(ComponentsEnum)).ToList().ForEach(delegate(string componentName)
		{
			if (Enum.TryParse<ComponentsEnum>(componentName, ignoreCase: true, out var result) && designComponentDisplay.Name == componentName)
			{
				SetHandEnabled(componentName != "Pendant");
				DesignComponentDisplay designComponentDisplay2 = DesignComponents.FirstOrDefault((DesignComponentDisplay component) => component.Name == componentName);
				if (designComponentDisplay2 != null)
				{
					designComponentDisplay2.IsEnabled = !designComponentDisplay2.IsEnabled;
					UIHelper.CoreDataService.SetComponentVisibility(result, designComponentDisplay2.IsEnabled);
				}
			}
		});
		SelectedDesignComponentDisplay = DesignComponents.FirstOrDefault((DesignComponentDisplay component) => component.IsEnabled);
		DispatcherHelper.WaitForPriority();
		ViewModelLocator.Current.GemReportVM.Populate();
	}
}
