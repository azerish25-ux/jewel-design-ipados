using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.View;

public partial class ProgramMenuPopup : UserControl, IComponentConnector
{
	public ProgramMenuPopup()
	{
		InitializeComponent();
		Messenger.Default.Register<string>(this, ProcessGenericMessage);
		if (UIHelper.CounterSketchOptions.HideFreehandMode == true || UIHelper.KeyProfile.ForceHideFreehand)
		{
			BtnTileLayoutParts.Visibility = Visibility.Collapsed;
		}
		TxtAboutCounterSketch.Text = $"About {UIHelper.KeyProfile.ApplicationName}";
		if (SecurityHelper.IsStreaming)
		{
			BtnBackup.Visibility = Visibility.Collapsed;
			Btn3DPrint.Visibility = Visibility.Collapsed;
			BtnExit.Visibility = Visibility.Collapsed;
			BtnUpdateSettings.Visibility = Visibility.Collapsed;
			BtnTroubleshooting.Visibility = Visibility.Collapsed;
			BtnSupport.Visibility = Visibility.Collapsed;
			BtnOpenLogFile.Visibility = Visibility.Collapsed;
			BtnProgramDataFolder.Visibility = Visibility.Collapsed;
			BtnExportDesign.Visibility = Visibility.Collapsed;
			BtnImportDesign.Visibility = Visibility.Collapsed;
			BtnUnregister.Visibility = Visibility.Collapsed;
			BtnOrderOptions.Visibility = Visibility.Collapsed;
			BtnPortfolioSettings.Visibility = Visibility.Collapsed;
			BtnCAMStandards.Visibility = Visibility.Collapsed;
			BtnCAMMountingSubmissionForm.Visibility = Visibility.Collapsed;
			BtnUserGuide.Visibility = Visibility.Collapsed;
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
}
