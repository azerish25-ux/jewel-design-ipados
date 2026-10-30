using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Shapes;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public class ProgramMenuPopup : UserControl, IComponentConnector
{
	internal ProgramMenuPopup ProgramMenu;

	internal Grid Grid;

	internal ToggleButton ToggleButton;

	internal Viewbox MenuIconVector;

	internal Viewbox MenuIconVectorPoppedUp;

	internal Border Menu;

	internal Grid LayoutRoot;

	internal Border VerticalSeparator;

	internal Border HorizontalSeparator;

	internal StackPanel MainMenu;

	internal Button BtnNewSession;

	internal Button BtnAccountInfo;

	internal Button BtnPrefences;

	internal Border Border;

	internal Button BtnUnregister;

	internal Button BtnImportDesign;

	internal Button BtnExportDesign;

	internal Button BtnOrderHistory;

	internal Button BtnBackup;

	internal Border Border2;

	internal Button Btn3DPrint;

	internal Border Border5;

	internal Button BtnHelp;

	internal Border Border1;

	internal Button BtnTest;

	internal Border BorderTest;

	internal StackPanel HelpSubMenu;

	internal Button BtnUserGuide;

	internal Button BtnTroubleshooting;

	internal Button BtnProgramDataFolder;

	internal Button BtnOpenLogFile;

	internal Button BtnSupport;

	internal Button BtnCAMStandards;

	internal Button BtnCAMMountingSubmissionForm;

	internal Button BtnReleaseNotes;

	internal Button BtnKeywords;

	internal Button BtnAbout;

	internal TextBlock TxtAboutCounterSketch;

	internal StackPanel TestSubMenu;

	internal StackPanel Print3DSubMenu;

	internal Button Btn3DPrintProJet;

	internal Canvas Svg2;

	internal Canvas Layer1;

	internal Canvas G3347;

	internal Path Path3375;

	internal Path Path3373;

	internal Path Path3371;

	internal Path Path3369;

	internal Path Path3367;

	internal Path Path3365;

	internal Path Path3363;

	internal Path Path3361;

	internal Path Path3359;

	internal Path Path3357;

	internal Path Path3355;

	internal Path Path3353;

	internal Path Path3351;

	internal Path Path3349;

	internal Button BtnProjetInstructions;

	internal StackPanel BackupSubMenu;

	internal Button BtnBackup1;

	internal Button BtnBackupSettings;

	internal Button BtnRestore;

	internal StackPanel PreferencesSubMenu;

	internal Button BtnDisplayOptions;

	internal Button BtnMaterialOptions;

	internal Button BtnOrderOptions;

	internal Button BtnRegionSettings;

	internal Button BtnPortfolioSettings;

	internal Button BtnThemes;

	internal Button BtnUpdateSettings;

	internal Button BtnTileLayoutDesigns;

	internal Button BtnTileLayoutParts;

	internal Button BtnExit;

	internal VisualStateGroup ProgramMenuStates;

	internal VisualState Default;

	internal VisualState Preferences;

	internal VisualState Backup;

	internal VisualState Help;

	internal VisualState Test;

	internal VisualState Print3D;

	private bool _contentLoaded;

	public ProgramMenuPopup()
	{
		InitializeComponent();
		Messenger.Default.Register<string>((object)this, (Action<string>)ProcessGenericMessage, false);
		if (UIHelper.CounterSketchOptions.HideFreehandMode == true || UIHelper.KeyProfile.ForceHideFreehand)
		{
			BtnTileLayoutParts.Visibility = Visibility.Visible;
		}
		TxtAboutCounterSketch.Text = $"About {UIHelper.KeyProfile.ApplicationName}";
		if (SecurityHelper.IsStreaming)
		{
			BtnBackup.Visibility = Visibility.Visible;
			Btn3DPrint.Visibility = Visibility.Visible;
			BtnExit.Visibility = Visibility.Visible;
			BtnUpdateSettings.Visibility = Visibility.Visible;
			BtnTroubleshooting.Visibility = Visibility.Visible;
			BtnSupport.Visibility = Visibility.Visible;
			BtnOpenLogFile.Visibility = Visibility.Visible;
			BtnProgramDataFolder.Visibility = Visibility.Visible;
			BtnExportDesign.Visibility = Visibility.Visible;
			BtnImportDesign.Visibility = Visibility.Visible;
			BtnUnregister.Visibility = Visibility.Visible;
			BtnOrderOptions.Visibility = Visibility.Visible;
			BtnPortfolioSettings.Visibility = Visibility.Visible;
			BtnCAMStandards.Visibility = Visibility.Visible;
			BtnCAMMountingSubmissionForm.Visibility = Visibility.Visible;
			BtnUserGuide.Visibility = Visibility.Visible;
		}
	}

	private void ProcessGenericMessage(string message)
	{
		if (message != null && message == "ResetMenuViewState")
		{
			VisualStateManager.GoToElementState(Grid, "Default", useTransitions: false);
		}
	}

	private void Popup_MouseLeave(object sender, MouseEventArgs e)
	{
		((MainViewModel)base.DataContext).IsMenuToggleButtonChecked = false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/programmenupopup.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			ProgramMenu = (ProgramMenuPopup)target;
			break;
		case 2:
			Grid = (Grid)target;
			break;
		case 3:
			ToggleButton = (ToggleButton)target;
			break;
		case 4:
			MenuIconVector = (Viewbox)target;
			break;
		case 5:
			((Popup)target).MouseLeave += Popup_MouseLeave;
			break;
		case 6:
			MenuIconVectorPoppedUp = (Viewbox)target;
			break;
		case 7:
			Menu = (Border)target;
			break;
		case 8:
			LayoutRoot = (Grid)target;
			break;
		case 9:
			VerticalSeparator = (Border)target;
			break;
		case 10:
			HorizontalSeparator = (Border)target;
			break;
		case 11:
			MainMenu = (StackPanel)target;
			break;
		case 12:
			BtnNewSession = (Button)target;
			break;
		case 13:
			BtnAccountInfo = (Button)target;
			break;
		case 14:
			BtnPrefences = (Button)target;
			break;
		case 15:
			Border = (Border)target;
			break;
		case 16:
			BtnUnregister = (Button)target;
			break;
		case 17:
			BtnImportDesign = (Button)target;
			break;
		case 18:
			BtnExportDesign = (Button)target;
			break;
		case 19:
			BtnOrderHistory = (Button)target;
			break;
		case 20:
			BtnBackup = (Button)target;
			break;
		case 21:
			Border2 = (Border)target;
			break;
		case 22:
			Btn3DPrint = (Button)target;
			break;
		case 23:
			Border5 = (Border)target;
			break;
		case 24:
			BtnHelp = (Button)target;
			break;
		case 25:
			Border1 = (Border)target;
			break;
		case 26:
			BtnTest = (Button)target;
			break;
		case 27:
			BorderTest = (Border)target;
			break;
		case 28:
			HelpSubMenu = (StackPanel)target;
			break;
		case 29:
			BtnUserGuide = (Button)target;
			break;
		case 30:
			BtnTroubleshooting = (Button)target;
			break;
		case 31:
			BtnProgramDataFolder = (Button)target;
			break;
		case 32:
			BtnOpenLogFile = (Button)target;
			break;
		case 33:
			BtnSupport = (Button)target;
			break;
		case 34:
			BtnCAMStandards = (Button)target;
			break;
		case 35:
			BtnCAMMountingSubmissionForm = (Button)target;
			break;
		case 36:
			BtnReleaseNotes = (Button)target;
			break;
		case 37:
			BtnKeywords = (Button)target;
			break;
		case 38:
			BtnAbout = (Button)target;
			break;
		case 39:
			TxtAboutCounterSketch = (TextBlock)target;
			break;
		case 40:
			TestSubMenu = (StackPanel)target;
			break;
		case 41:
			Print3DSubMenu = (StackPanel)target;
			break;
		case 42:
			Btn3DPrintProJet = (Button)target;
			break;
		case 43:
			Svg2 = (Canvas)target;
			break;
		case 44:
			Layer1 = (Canvas)target;
			break;
		case 45:
			G3347 = (Canvas)target;
			break;
		case 46:
			Path3375 = (Path)target;
			break;
		case 47:
			Path3373 = (Path)target;
			break;
		case 48:
			Path3371 = (Path)target;
			break;
		case 49:
			Path3369 = (Path)target;
			break;
		case 50:
			Path3367 = (Path)target;
			break;
		case 51:
			Path3365 = (Path)target;
			break;
		case 52:
			Path3363 = (Path)target;
			break;
		case 53:
			Path3361 = (Path)target;
			break;
		case 54:
			Path3359 = (Path)target;
			break;
		case 55:
			Path3357 = (Path)target;
			break;
		case 56:
			Path3355 = (Path)target;
			break;
		case 57:
			Path3353 = (Path)target;
			break;
		case 58:
			Path3351 = (Path)target;
			break;
		case 59:
			Path3349 = (Path)target;
			break;
		case 60:
			BtnProjetInstructions = (Button)target;
			break;
		case 61:
			BackupSubMenu = (StackPanel)target;
			break;
		case 62:
			BtnBackup1 = (Button)target;
			break;
		case 63:
			BtnBackupSettings = (Button)target;
			break;
		case 64:
			BtnRestore = (Button)target;
			break;
		case 65:
			PreferencesSubMenu = (StackPanel)target;
			break;
		case 66:
			BtnDisplayOptions = (Button)target;
			break;
		case 67:
			BtnMaterialOptions = (Button)target;
			break;
		case 68:
			BtnOrderOptions = (Button)target;
			break;
		case 69:
			BtnRegionSettings = (Button)target;
			break;
		case 70:
			BtnPortfolioSettings = (Button)target;
			break;
		case 71:
			BtnThemes = (Button)target;
			break;
		case 72:
			BtnUpdateSettings = (Button)target;
			break;
		case 73:
			BtnTileLayoutDesigns = (Button)target;
			break;
		case 74:
			BtnTileLayoutParts = (Button)target;
			break;
		case 75:
			BtnExit = (Button)target;
			break;
		case 76:
			ProgramMenuStates = (VisualStateGroup)target;
			break;
		case 77:
			Default = (VisualState)target;
			break;
		case 78:
			Preferences = (VisualState)target;
			break;
		case 79:
			Backup = (VisualState)target;
			break;
		case 80:
			Help = (VisualState)target;
			break;
		case 81:
			Test = (VisualState)target;
			break;
		case 82:
			Print3D = (VisualState)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
