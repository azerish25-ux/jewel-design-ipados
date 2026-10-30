using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.View;

public class BackupRestoreView : UserControl, IComponentConnector
{
	internal BackupRestoreView BackupRestore;

	internal Grid LayoutRoot;

	internal TextBox txtRecoveryFile1;

	internal Button btnResetRecoveryFileDirectory1;

	internal Button btnRecoveryLocation1;

	internal Button btnRestoreBackup1;

	internal TextBox txtBackupDirectory;

	internal Button btnResetBackupDirectory;

	internal Button btnBrowseBackupDirectory;

	internal Button btnCreateBackup;

	internal TextBlock txtStatistics;

	internal TextBlock txtBackupLog;

	internal TextBox txtRecoveryFile;

	internal Button btnResetRecoveryFileDirectory;

	internal Button btnRecoveryLocation;

	internal Button btnRestoreBackup;

	private bool _contentLoaded;

	public BackupRestoreView()
	{
		InitializeComponent();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/backuprestoreview.xaml", UriKind.Relative);
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
			BackupRestore = (BackupRestoreView)target;
			break;
		case 2:
			LayoutRoot = (Grid)target;
			break;
		case 3:
			txtRecoveryFile1 = (TextBox)target;
			break;
		case 4:
			btnResetRecoveryFileDirectory1 = (Button)target;
			break;
		case 5:
			btnRecoveryLocation1 = (Button)target;
			break;
		case 6:
			btnRestoreBackup1 = (Button)target;
			break;
		case 7:
			txtBackupDirectory = (TextBox)target;
			break;
		case 8:
			btnResetBackupDirectory = (Button)target;
			break;
		case 9:
			btnBrowseBackupDirectory = (Button)target;
			break;
		case 10:
			btnCreateBackup = (Button)target;
			break;
		case 11:
			txtStatistics = (TextBlock)target;
			break;
		case 12:
			txtBackupLog = (TextBlock)target;
			break;
		case 13:
			txtRecoveryFile = (TextBox)target;
			break;
		case 14:
			btnResetRecoveryFileDirectory = (Button)target;
			break;
		case 15:
			btnRecoveryLocation = (Button)target;
			break;
		case 16:
			btnRestoreBackup = (Button)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
