using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.WinForms;

public class FrmPrintToProJet1200 : Form
{
	private readonly string _designId;

	private bool _initialDisplayConduitState;

	private int _orgHeight;

	private int _scaleSups;

	private string _strLastStatus = string.Empty;

	private IContainer components;

	private Button butPrint;

	private Button butCancel;

	private GroupBox gbRotateModel;

	private Label lblRotateZ;

	private Label lblRotateY;

	private Label lblRotateX;

	private TrackBar trkRotateAmountZ;

	private TrackBar trkRotateAmountY;

	private TrackBar trkRotateAmountX;

	private Button butTestConnection;

	private GroupBox groupBox1;

	private TrackBar trackBar1;

	private Button btnCreateSup;

	private StatusStrip statusStrip1;

	private ToolStripStatusLabel statusStatus;

	private ToolStripStatusLabel statusErr;

	private TextBox tbConnection;

	private Button btnReset;

	private CheckBox cbAccess;

	private Label lblPrintGems;

	private CheckBox cbSide;

	private CheckBox cbCenter;

	private Button btnAbortPrint;

	private Label lblZDeg;

	private Label lblYDeg;

	private Label lblXDeg;

	private Label lblZOff;

	private Label lblIPAddress;

	private GroupBox gbSavedJobs;

	private PictureBox picJob3;

	private Button btnSaveJob3;

	private CheckBox cbJobSaved3;

	private PictureBox picJob2;

	private Button btnSaveJob2;

	private CheckBox cbJobSaved2;

	private Button btnDeleteCheckedSavedJobs;

	private Button btnPrintCheckedSavedJobs;

	private PictureBox picJob1;

	private Button btnSaveJob1;

	private CheckBox cbJobSaved1;

	private TrackBar tbSupportDensity;

	private Label lblSupportDensity;

	private GroupBox gbActivation;

	private Label label1;

	private Button btnActivatePrinter;

	private TextBox tbActiveCompanyName;

	private TextBox tbActiveEmail;

	public FrmPrintToProJet1200(string designId = null)
	{
		InitializeComponent();
		UIHelper.CoreDataService.InitializeProjet();
		_designId = designId;
	}

	private void btnAbortPrint_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("Are you sure you want to cancel this print job?", "Projet Printer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		{
			StopPrinting();
		}
	}

	private void btnActivatePrinter_MouseUp(object sender, MouseEventArgs e)
	{
		string sError = string.Empty;
		bool bDoFirmware = UIHelper.CoreDataService.CheckFirmwareVersion();
		bool activationState = UIHelper.CoreDataService.GetActivationState();
		if (!UIHelper.CoreDataService.UpdateFirmwareAndActivate(bDoFirmware, activationState, tbActiveEmail.Text, tbActiveCompanyName.Text, out sError))
		{
			statusStatus.Text = sError;
			return;
		}
		gbActivation.Visible = false;
		ConnectWithStatus(_strLastStatus);
	}

	private void btnCreateSup_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
		{
			EnableSaveJobui();
			base.Height = _orgHeight;
		}
	}

	private void btnDeleteCheckedSavedJobs_MouseUp(object sender, MouseEventArgs e)
	{
		if (cbJobSaved1.Checked)
		{
			UIHelper.CoreDataService.DeleteSavedJob(0);
		}
		if (cbJobSaved2.Checked)
		{
			UIHelper.CoreDataService.DeleteSavedJob(1);
		}
		if (cbJobSaved3.Checked)
		{
			UIHelper.CoreDataService.DeleteSavedJob(2);
		}
		LoadSavedJobs();
	}

	private void btnPrintCheckedSavedJobs_MouseUp(object sender, MouseEventArgs e)
	{
		List<int> list = new List<int>();
		if (cbJobSaved1.Checked)
		{
			list.Add(0);
		}
		if (cbJobSaved2.Checked)
		{
			list.Add(1);
		}
		if (cbJobSaved3.Checked)
		{
			list.Add(2);
		}
		string sStatus = string.Empty;
		if (list.Count == 0)
		{
			statusStatus.Text = "Nothing checked to print!";
			return;
		}
		try
		{
			if (MessageBox.Show("Please make sure material is loaded and locked down, and there is new clean platform in machine and locked down. Continue?", "Projet Printer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				statusErr.Text = "Prep models and sending to printer......";
				base.Visible = false;
				UIHelper.ShowWaitWindow(waitVisible: true, "Preparing Models to Print");
				bool num = UIHelper.CoreDataService.PrintMultipleJobs(out sStatus, list.ToArray());
				statusErr.Text = sStatus;
				Refresh();
				UIHelper.ShowWaitWindow(waitVisible: false, "");
				Thread.Sleep(3);
				UIHelper.ShowWaitWindow(waitVisible: true, "Sending Multiple Models to Printer");
				Thread.Sleep(6);
				Refresh();
				if (num)
				{
					UIHelper.ShowWaitWindow(waitVisible: false, "");
					Refresh();
					Thread.Sleep(6);
					UIHelper.ShowWaitWindow(waitVisible: true, "Models printing. Job should take about " + sStatus);
					Refresh();
					EventHelper.RecordImmediateEvent(EventHelper.EventTypes.Print3D_ProjetPrintModel, "DesignId", _designId);
					Thread.Sleep(15);
					Refresh();
					UIHelper.ShowWaitWindow(waitVisible: false, "");
					Thread.Sleep(5);
					Close();
				}
				UIHelper.ShowWaitWindow(waitVisible: false, "");
				base.Visible = true;
			}
		}
		catch (Exception)
		{
			UIHelper.ShowWaitWindow(waitVisible: false, "");
			Close();
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		trackBar1.Value = 0;
		trkRotateAmountX.Value = 1;
		trkRotateAmountY.Value = 1;
		trkRotateAmountZ.Value = 1;
		trkRotateAmountX.Value = 0;
		trkRotateAmountY.Value = 0;
		trkRotateAmountZ.Value = 0;
	}

	private void btnSaveJob1_Click(object sender, EventArgs e)
	{
	}

	private void btnSaveJob1_MouseUp(object sender, MouseEventArgs e)
	{
		int num = Convert.ToInt32((sender as Button).Tag);
		string sStatus = string.Empty;
		if (!UIHelper.CoreDataService.GetSavedJobs().Contains(num) || MessageBox.Show("Overwrite this saved Job?", "Projet Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
		{
			Cursor = Cursors.AppStarting;
			if (UIHelper.CoreDataService.SavePrintJob(out sStatus, num))
			{
				LoadSavedJobs();
			}
			Cursor = Cursors.Arrow;
		}
	}

	private void butCancel_Click(object sender, EventArgs e)
	{
		UIHelper.ShowWaitWindow(waitVisible: false, "");
		Close();
	}

	private void butPrint_Click(object sender, EventArgs e)
	{
		try
		{
			if (MessageBox.Show("Please make sure material is loaded and locked down, and there is new clean platform in machine and locked down. Continue?", "Projet Printer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				statusErr.Text = "Prep model and sending to printer......";
				base.Visible = false;
				UIHelper.ShowWaitWindow(waitVisible: true, "Preparing Model to Print");
				string sErr;
				bool num = UIHelper.CoreDataService.PrintUnifiedMeshToProJet1200(out sErr);
				statusErr.Text = sErr;
				Refresh();
				UIHelper.ShowWaitWindow(waitVisible: false, "");
				Thread.Sleep(3);
				UIHelper.ShowWaitWindow(waitVisible: true, "Sending Model to Printer");
				Thread.Sleep(6);
				Refresh();
				if (num)
				{
					UIHelper.ShowWaitWindow(waitVisible: false, "");
					Refresh();
					Thread.Sleep(6);
					UIHelper.ShowWaitWindow(waitVisible: true, "Model printing. Job should take about " + sErr);
					lblPrintGems.Text = "Model printing. Job should take about " + sErr;
					Refresh();
					EventHelper.RecordImmediateEvent(EventHelper.EventTypes.Print3D_ProjetPrintModel, "DesignId", _designId);
					Thread.Sleep(15);
					Refresh();
					UIHelper.ShowWaitWindow(waitVisible: false, "");
					Thread.Sleep(5);
					Close();
				}
				else
				{
					base.Visible = true;
				}
				UIHelper.ShowWaitWindow(waitVisible: false, "");
				if (sErr == "Failed Generating Build File")
				{
					EventHelper.RecordImmediateEvent(EventHelper.EventTypes.Print3D_ProjetBadModel, "DesignId", _designId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Projet Error");
			UIHelper.ShowWaitWindow(waitVisible: false, "");
			Close();
		}
	}

	private void butTestConnection_Click(object sender, EventArgs e)
	{
		string text = tbConnection.Text.Trim();
		if (!IPAddress.TryParse(tbConnection.Text, out var _))
		{
			statusErr.Text = "Invalid IP Format!";
			return;
		}
		statusErr.Text = "Connecting and getting item ready to print....";
		statusStatus.Text = "Not Connected";
		if (UIHelper.CoreDataService.CheckConnectionToProJet1200(text, out var sError, out var sStatus))
		{
			UIHelper.CounterSketchOptions.ProjetLastIPLocation = text;
			string message = "Green";
			UIHelper.CoreDataService.GetPrinterMaterial(out message);
			if (message == "Green")
			{
				statusStatus.ForeColor = Color.Green;
			}
			else
			{
				statusStatus.ForeColor = Color.Gray;
			}
			statusStatus.Text = message + " Material-" + sError;
			ConnectWithStatus(sStatus);
		}
		else
		{
			SupportPreviewDisplayConduitDisable();
			statusErr.Text = sError;
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		string empty = string.Empty;
		empty = "Creating Supports...";
		statusErr.Text = empty;
		Cursor = Cursors.AppStarting;
		bool flag = UIHelper.CoreDataService.CSCreateSupportsForCSItem(out empty);
		Cursor = Cursors.Arrow;
		butPrint.Enabled = flag;
		btnSaveJob1.Enabled = flag;
		btnSaveJob2.Enabled = flag;
		btnSaveJob3.Enabled = flag;
		btnCreateSup.Enabled = !flag;
		if (flag)
		{
			statusErr.Text = "Step 2: Click Print";
		}
		else
		{
			statusErr.Text = empty;
		}
	}

	private void cbAccess_CheckedChanged(object sender, EventArgs e)
	{
		Cursor = Cursors.AppStarting;
		statusErr.Text = "Updating Gems...";
		SetUnifiedMeshRotation();
		SetCSModelForPrint();
		UIHelper.CoreDataService.PrintingSupportDisplayConduitEnabled(isEnabled: true);
		Cursor = Cursors.Arrow;
		statusErr.Text = "Step 1: Create Supports";
	}

	private void ConnectWithStatus(string sStatus)
	{
		_strLastStatus = sStatus;
		lblPrintGems.Visible = true;
		cbCenter.Visible = true;
		cbSide.Visible = true;
		cbAccess.Visible = true;
		btnReset.Visible = true;
		butPrint.Visible = true;
		butTestConnection.Visible = false;
		tbConnection.Visible = false;
		lblIPAddress.Visible = false;
		gbActivation.Visible = false;
		if (sStatus.StartsWith("Printing"))
		{
			btnAbortPrint.Visible = true;
			statusErr.Text = sStatus;
		}
		else if (UIHelper.CoreDataService.GetActivationState() || UIHelper.CoreDataService.CheckFirmwareVersion())
		{
			gbActivation.Visible = true;
			lblPrintGems.Visible = false;
			cbCenter.Visible = false;
			cbSide.Visible = false;
			cbAccess.Visible = false;
			gbActivation.Left = 0;
			gbActivation.Top = 0;
			btnReset.Visible = false;
			butPrint.Visible = false;
			butCancel.Top = btnCreateSup.Top;
			base.Height = _orgHeight;
			statusErr.Text = "Activate Printer!!!";
			butPrint.Enabled = false;
		}
		else
		{
			groupBox1.Visible = true;
			gbRotateModel.Visible = true;
			btnCreateSup.Visible = true;
			UIHelper.CounterSketchOptions.WriteSettings();
			SupportPreviewDisplayConduitEnable();
			butCancel.Top = btnCreateSup.Top;
			base.Height = _orgHeight;
			statusErr.Text = "Step 1: Create Supports";
			butPrint.Enabled = false;
			LoadSavedJobs();
		}
	}

	private void EnableSaveJobui()
	{
		_orgHeight = 342;
		gbSavedJobs.Visible = true;
		UIHelper.CounterSketchOptions.ProjetShowSavedJobsUI = 1;
	}

	private void FrmPrintToProJet1200_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			UIHelper.CoreDataService.ConduitEnabled(isEnabled: true);
			UIHelper.CoreDataService.ClearUnifiedMesh();
			UIHelper.CoreDataService.PrintingSupportDisplayConduitEnabled(isEnabled: false);
			UIHelper.CoreDataService.SetLoadedDialogState(bState: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			throw;
		}
		Dispose();
	}

	private void FrmPrintToProJet1200_Load(object sender, EventArgs e)
	{
		try
		{
			gbRotateModel.Top = 0;
			butCancel.Top = butTestConnection.Top;
			UIHelper.CoreDataService.SetLoadedDialogState(bState: true);
			base.StartPosition = FormStartPosition.CenterScreen;
			btnAbortPrint.Width = butTestConnection.Width;
			btnAbortPrint.Location = butTestConnection.Location;
			UIHelper.CoreDataService.InitializeProjet();
			_orgHeight = 242;
			base.Height = 95;
			statusErr.Text = "Connect to Printer...";
			SetCSModelForPrint();
			tbConnection.Text = UIHelper.CounterSketchOptions.ProjetLastIPLocation ?? "192.168.1.201";
			if (UIHelper.CounterSketchOptions.ProjetShowSavedJobsUI == 1)
			{
				_orgHeight = 342;
				gbSavedJobs.Visible = true;
			}
			_scaleSups = UIHelper.CounterSketchOptions.ProjetIncreaseSupportType;
			if (_scaleSups < 1)
			{
				_scaleSups = 0;
			}
			UIHelper.CoreDataService.SetSupportScales(UIHelper.CounterSketchOptions.UserTypeId, _scaleSups);
			UIHelper.CoreDataService.ConduitEnabled(isEnabled: false);
			UIHelper.CoreDataService.PrintingSupportDisplayConduitEnabled(isEnabled: true);
			_ = string.Empty;
			string sError = string.Empty;
			string sStatus = string.Empty;
			if (UIHelper.CoreDataService.CheckConnectionToProJet1200("192.168.7.2", out sError, out sStatus))
			{
				ConnectWithStatus(sStatus);
			}
			else
			{
				SupportPreviewDisplayConduitDisable();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			throw;
		}
	}

	private Bitmap LoadImageFromFile(string fileName)
	{
		FileStream fileStream = null;
		Bitmap bitmap = null;
		try
		{
			fileStream = File.OpenRead(fileName);
			MemoryStream memoryStream = new MemoryStream();
			byte[] buffer = new byte[10000];
			while (true)
			{
				int num = fileStream.Read(buffer, 0, 10000);
				if (num == 0)
				{
					break;
				}
				memoryStream.Write(buffer, 0, num);
			}
			return (Bitmap)Image.FromStream(memoryStream);
		}
		finally
		{
			if (fileStream != null)
			{
				fileStream.Close();
				fileStream.Dispose();
			}
		}
	}

	private void LoadSavedJobs()
	{
		List<int> savedJobs = UIHelper.CoreDataService.GetSavedJobs();
		string empty = string.Empty;
		cbJobSaved1.Enabled = false;
		cbJobSaved2.Enabled = false;
		cbJobSaved3.Enabled = false;
		cbJobSaved1.Checked = false;
		cbJobSaved2.Checked = false;
		cbJobSaved3.Checked = false;
		picJob1.BackgroundImage = null;
		picJob2.BackgroundImage = null;
		picJob3.BackgroundImage = null;
		if (savedJobs.Contains(0))
		{
			empty = Path.Combine(Path.GetTempPath(), "CSSavedJob0.bmp");
			if (File.Exists(empty))
			{
				picJob1.BackgroundImage = LoadImageFromFile(empty);
			}
			cbJobSaved1.Enabled = true;
		}
		if (savedJobs.Contains(1))
		{
			empty = Path.Combine(Path.GetTempPath(), "CSSavedJob1.bmp");
			if (File.Exists(empty))
			{
				picJob2.BackgroundImage = LoadImageFromFile(empty);
			}
			cbJobSaved2.Enabled = true;
		}
		if (savedJobs.Contains(1))
		{
			empty = Path.Combine(Path.GetTempPath(), "CSSavedJob2.bmp");
			if (File.Exists(empty))
			{
				picJob3.BackgroundImage = LoadImageFromFile(empty);
			}
			cbJobSaved3.Enabled = true;
		}
	}

	private void SetCSModelForPrint()
	{
		if (ProjectManagementHelper.IsDocumentCanvasEmpty())
		{
			butTestConnection.Enabled = false;
			tbConnection.Enabled = false;
			statusErr.Text = "No Model to Print!";
		}
		else
		{
			string sError = string.Empty;
			UIHelper.CoreDataService.SetMeshFromComponent(UIHelper.VisibleComponents, cbCenter.Checked, cbSide.Checked, cbAccess.Checked, out sError);
		}
	}

	private void SetUnifiedMeshRotation()
	{
		butPrint.Enabled = false;
		btnSaveJob1.Enabled = false;
		btnSaveJob2.Enabled = false;
		btnSaveJob3.Enabled = false;
		btnCreateSup.Enabled = true;
		UIHelper.CoreDataService.SetUnifiedMeshRotation(trkRotateAmountX.Value, trkRotateAmountY.Value, trkRotateAmountZ.Value);
		lblXDeg.Text = trkRotateAmountX.Value + "°";
		lblYDeg.Text = trkRotateAmountY.Value + "°";
		lblZDeg.Text = trkRotateAmountZ.Value + "°";
		statusErr.Text = "Step 1: Create Supports";
	}

	private void statusStatus_DoubleClick(object sender, EventArgs e)
	{
	}

	private void StopPrinting()
	{
		UIHelper.CoreDataService.AbortPrintJob();
		Close();
	}

	private void SupportPreviewDisplayConduitDisable()
	{
		UIHelper.CoreDataService.ConduitEnabled(_initialDisplayConduitState);
		UIHelper.CoreDataService.PrintingSupportDisplayConduitEnabled(isEnabled: false);
	}

	private void SupportPreviewDisplayConduitEnable()
	{
		_initialDisplayConduitState = UIHelper.CoreDataService.IsDisplayConduitEnabled();
		if (_initialDisplayConduitState)
		{
			UIHelper.CoreDataService.ConduitEnabled(isEnabled: false);
		}
		UIHelper.CoreDataService.PrintingSupportDisplayConduitEnabled(isEnabled: true);
	}

	private void tbConnection_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
		{
			e.Handled = true;
		}
	}

	private void tbSupportDensity_ValueChanged(object sender, EventArgs e)
	{
		UIHelper.CoreDataService.SetSupportDensity(tbSupportDensity.Value);
	}

	private void trackBar1_ValueChanged(object sender, EventArgs e)
	{
		butPrint.Enabled = false;
		btnSaveJob1.Enabled = false;
		btnSaveJob2.Enabled = false;
		btnSaveJob3.Enabled = false;
		btnCreateSup.Enabled = true;
		UIHelper.CoreDataService.SetUnifiedMeshLocation(trackBar1.Value);
		lblZOff.Text = trackBar1.Value.ToString();
		statusErr.Text = "Step 1: Create Supports";
	}

	private void trkRotateAmountY_ValueChanged(object sender, EventArgs e)
	{
		SetUnifiedMeshRotation();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.butPrint = new System.Windows.Forms.Button();
		this.butCancel = new System.Windows.Forms.Button();
		this.gbRotateModel = new System.Windows.Forms.GroupBox();
		this.lblZDeg = new System.Windows.Forms.Label();
		this.lblYDeg = new System.Windows.Forms.Label();
		this.lblXDeg = new System.Windows.Forms.Label();
		this.lblRotateZ = new System.Windows.Forms.Label();
		this.lblRotateY = new System.Windows.Forms.Label();
		this.lblRotateX = new System.Windows.Forms.Label();
		this.trkRotateAmountX = new System.Windows.Forms.TrackBar();
		this.trkRotateAmountZ = new System.Windows.Forms.TrackBar();
		this.trkRotateAmountY = new System.Windows.Forms.TrackBar();
		this.butTestConnection = new System.Windows.Forms.Button();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.lblZOff = new System.Windows.Forms.Label();
		this.trackBar1 = new System.Windows.Forms.TrackBar();
		this.btnCreateSup = new System.Windows.Forms.Button();
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.statusStatus = new System.Windows.Forms.ToolStripStatusLabel();
		this.statusErr = new System.Windows.Forms.ToolStripStatusLabel();
		this.tbConnection = new System.Windows.Forms.TextBox();
		this.btnReset = new System.Windows.Forms.Button();
		this.cbAccess = new System.Windows.Forms.CheckBox();
		this.lblPrintGems = new System.Windows.Forms.Label();
		this.cbSide = new System.Windows.Forms.CheckBox();
		this.cbCenter = new System.Windows.Forms.CheckBox();
		this.btnAbortPrint = new System.Windows.Forms.Button();
		this.lblIPAddress = new System.Windows.Forms.Label();
		this.gbSavedJobs = new System.Windows.Forms.GroupBox();
		this.cbJobSaved2 = new System.Windows.Forms.CheckBox();
		this.cbJobSaved3 = new System.Windows.Forms.CheckBox();
		this.cbJobSaved1 = new System.Windows.Forms.CheckBox();
		this.picJob3 = new System.Windows.Forms.PictureBox();
		this.btnSaveJob3 = new System.Windows.Forms.Button();
		this.picJob2 = new System.Windows.Forms.PictureBox();
		this.btnSaveJob2 = new System.Windows.Forms.Button();
		this.btnDeleteCheckedSavedJobs = new System.Windows.Forms.Button();
		this.btnPrintCheckedSavedJobs = new System.Windows.Forms.Button();
		this.picJob1 = new System.Windows.Forms.PictureBox();
		this.btnSaveJob1 = new System.Windows.Forms.Button();
		this.tbSupportDensity = new System.Windows.Forms.TrackBar();
		this.lblSupportDensity = new System.Windows.Forms.Label();
		this.gbActivation = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.btnActivatePrinter = new System.Windows.Forms.Button();
		this.tbActiveCompanyName = new System.Windows.Forms.TextBox();
		this.tbActiveEmail = new System.Windows.Forms.TextBox();
		this.gbRotateModel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.trkRotateAmountX).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.trkRotateAmountZ).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.trkRotateAmountY).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.trackBar1).BeginInit();
		this.statusStrip1.SuspendLayout();
		this.gbSavedJobs.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picJob3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picJob2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picJob1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.tbSupportDensity).BeginInit();
		this.gbActivation.SuspendLayout();
		base.SuspendLayout();
		this.butPrint.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.butPrint.Enabled = false;
		this.butPrint.Location = new System.Drawing.Point(173, 156);
		this.butPrint.Name = "butPrint";
		this.butPrint.Size = new System.Drawing.Size(88, 23);
		this.butPrint.TabIndex = 10;
		this.butPrint.Text = "Print";
		this.butPrint.UseVisualStyleBackColor = true;
		this.butPrint.Click += new System.EventHandler(butPrint_Click);
		this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.butCancel.Location = new System.Drawing.Point(267, 156);
		this.butCancel.Name = "butCancel";
		this.butCancel.Size = new System.Drawing.Size(75, 23);
		this.butCancel.TabIndex = 11;
		this.butCancel.Text = "Cancel";
		this.butCancel.UseVisualStyleBackColor = true;
		this.butCancel.Click += new System.EventHandler(butCancel_Click);
		this.gbRotateModel.Controls.Add(this.lblZDeg);
		this.gbRotateModel.Controls.Add(this.lblYDeg);
		this.gbRotateModel.Controls.Add(this.lblXDeg);
		this.gbRotateModel.Controls.Add(this.lblRotateZ);
		this.gbRotateModel.Controls.Add(this.lblRotateY);
		this.gbRotateModel.Controls.Add(this.lblRotateX);
		this.gbRotateModel.Controls.Add(this.trkRotateAmountX);
		this.gbRotateModel.Controls.Add(this.trkRotateAmountZ);
		this.gbRotateModel.Controls.Add(this.trkRotateAmountY);
		this.gbRotateModel.Location = new System.Drawing.Point(0, 296);
		this.gbRotateModel.Name = "gbRotateModel";
		this.gbRotateModel.Size = new System.Drawing.Size(260, 107);
		this.gbRotateModel.TabIndex = 12;
		this.gbRotateModel.TabStop = false;
		this.gbRotateModel.Text = "Rotate Model";
		this.gbRotateModel.Visible = false;
		this.lblZDeg.AutoSize = true;
		this.lblZDeg.Location = new System.Drawing.Point(44, 23);
		this.lblZDeg.Name = "lblZDeg";
		this.lblZDeg.Size = new System.Drawing.Size(13, 13);
		this.lblZDeg.TabIndex = 18;
		this.lblZDeg.Text = "0";
		this.lblYDeg.AutoSize = true;
		this.lblYDeg.Location = new System.Drawing.Point(44, 51);
		this.lblYDeg.Name = "lblYDeg";
		this.lblYDeg.Size = new System.Drawing.Size(13, 13);
		this.lblYDeg.TabIndex = 17;
		this.lblYDeg.Text = "0";
		this.lblXDeg.AutoSize = true;
		this.lblXDeg.Location = new System.Drawing.Point(44, 77);
		this.lblXDeg.Name = "lblXDeg";
		this.lblXDeg.Size = new System.Drawing.Size(13, 13);
		this.lblXDeg.TabIndex = 16;
		this.lblXDeg.Text = "0";
		this.lblRotateZ.AutoSize = true;
		this.lblRotateZ.Location = new System.Drawing.Point(6, 77);
		this.lblRotateZ.Name = "lblRotateZ";
		this.lblRotateZ.Size = new System.Drawing.Size(14, 13);
		this.lblRotateZ.TabIndex = 15;
		this.lblRotateZ.Text = "Z";
		this.lblRotateY.AutoSize = true;
		this.lblRotateY.Location = new System.Drawing.Point(6, 51);
		this.lblRotateY.Name = "lblRotateY";
		this.lblRotateY.Size = new System.Drawing.Size(14, 13);
		this.lblRotateY.TabIndex = 14;
		this.lblRotateY.Text = "Y";
		this.lblRotateX.AutoSize = true;
		this.lblRotateX.Location = new System.Drawing.Point(6, 23);
		this.lblRotateX.Name = "lblRotateX";
		this.lblRotateX.Size = new System.Drawing.Size(14, 13);
		this.lblRotateX.TabIndex = 13;
		this.lblRotateX.Text = "X";
		this.trkRotateAmountX.AutoSize = false;
		this.trkRotateAmountX.LargeChange = 30;
		this.trkRotateAmountX.Location = new System.Drawing.Point(59, 73);
		this.trkRotateAmountX.Maximum = 90;
		this.trkRotateAmountX.Minimum = -90;
		this.trkRotateAmountX.Name = "trkRotateAmountX";
		this.trkRotateAmountX.Size = new System.Drawing.Size(195, 28);
		this.trkRotateAmountX.SmallChange = 10;
		this.trkRotateAmountX.TabIndex = 10;
		this.trkRotateAmountX.TickFrequency = 10;
		this.trkRotateAmountX.TickStyle = System.Windows.Forms.TickStyle.None;
		this.trkRotateAmountX.ValueChanged += new System.EventHandler(trkRotateAmountY_ValueChanged);
		this.trkRotateAmountZ.AutoSize = false;
		this.trkRotateAmountZ.LargeChange = 30;
		this.trkRotateAmountZ.Location = new System.Drawing.Point(59, 19);
		this.trkRotateAmountZ.Maximum = 90;
		this.trkRotateAmountZ.Minimum = -90;
		this.trkRotateAmountZ.Name = "trkRotateAmountZ";
		this.trkRotateAmountZ.Size = new System.Drawing.Size(195, 28);
		this.trkRotateAmountZ.SmallChange = 10;
		this.trkRotateAmountZ.TabIndex = 12;
		this.trkRotateAmountZ.TickFrequency = 10;
		this.trkRotateAmountZ.TickStyle = System.Windows.Forms.TickStyle.None;
		this.trkRotateAmountZ.ValueChanged += new System.EventHandler(trkRotateAmountY_ValueChanged);
		this.trkRotateAmountY.AutoSize = false;
		this.trkRotateAmountY.LargeChange = 30;
		this.trkRotateAmountY.Location = new System.Drawing.Point(59, 47);
		this.trkRotateAmountY.Maximum = 90;
		this.trkRotateAmountY.Minimum = -90;
		this.trkRotateAmountY.Name = "trkRotateAmountY";
		this.trkRotateAmountY.Size = new System.Drawing.Size(195, 28);
		this.trkRotateAmountY.SmallChange = 10;
		this.trkRotateAmountY.TabIndex = 11;
		this.trkRotateAmountY.TickFrequency = 10;
		this.trkRotateAmountY.TickStyle = System.Windows.Forms.TickStyle.None;
		this.trkRotateAmountY.ValueChanged += new System.EventHandler(trkRotateAmountY_ValueChanged);
		this.butTestConnection.Location = new System.Drawing.Point(190, 2);
		this.butTestConnection.Name = "butTestConnection";
		this.butTestConnection.Size = new System.Drawing.Size(75, 23);
		this.butTestConnection.TabIndex = 13;
		this.butTestConnection.Text = "Connect";
		this.butTestConnection.UseVisualStyleBackColor = true;
		this.butTestConnection.Click += new System.EventHandler(butTestConnection_Click);
		this.groupBox1.Controls.Add(this.lblZOff);
		this.groupBox1.Controls.Add(this.trackBar1);
		this.groupBox1.Location = new System.Drawing.Point(278, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(60, 148);
		this.groupBox1.TabIndex = 14;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Move Z";
		this.groupBox1.Visible = false;
		this.lblZOff.AutoSize = true;
		this.lblZOff.Location = new System.Drawing.Point(38, 125);
		this.lblZOff.Name = "lblZOff";
		this.lblZOff.Size = new System.Drawing.Size(13, 13);
		this.lblZOff.TabIndex = 18;
		this.lblZOff.Text = "0";
		this.trackBar1.AutoSize = false;
		this.trackBar1.Location = new System.Drawing.Point(9, 13);
		this.trackBar1.Maximum = 15;
		this.trackBar1.Minimum = -15;
		this.trackBar1.Name = "trackBar1";
		this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
		this.trackBar1.Size = new System.Drawing.Size(31, 132);
		this.trackBar1.TabIndex = 15;
		this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
		this.trackBar1.ValueChanged += new System.EventHandler(trackBar1_ValueChanged);
		this.btnCreateSup.Location = new System.Drawing.Point(69, 156);
		this.btnCreateSup.Name = "btnCreateSup";
		this.btnCreateSup.Size = new System.Drawing.Size(91, 23);
		this.btnCreateSup.TabIndex = 15;
		this.btnCreateSup.Text = "Create Supports";
		this.btnCreateSup.UseVisualStyleBackColor = true;
		this.btnCreateSup.Visible = false;
		this.btnCreateSup.Click += new System.EventHandler(button1_Click);
		this.btnCreateSup.MouseUp += new System.Windows.Forms.MouseEventHandler(btnCreateSup_MouseUp);
		this.statusStrip1.GripMargin = new System.Windows.Forms.Padding(0);
		this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.statusStatus, this.statusErr });
		this.statusStrip1.Location = new System.Drawing.Point(0, 182);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Size = new System.Drawing.Size(343, 22);
		this.statusStrip1.SizingGrip = false;
		this.statusStrip1.TabIndex = 16;
		this.statusStrip1.Text = "statusStrip1";
		this.statusStatus.Name = "statusStatus";
		this.statusStatus.Size = new System.Drawing.Size(88, 17);
		this.statusStatus.Text = "Not Connected";
		this.statusStatus.DoubleClick += new System.EventHandler(statusStatus_DoubleClick);
		this.statusErr.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.statusErr.Name = "statusErr";
		this.statusErr.Size = new System.Drawing.Size(240, 17);
		this.statusErr.Spring = true;
		this.statusErr.Text = "No Error..";
		this.tbConnection.Location = new System.Drawing.Point(98, 4);
		this.tbConnection.Name = "tbConnection";
		this.tbConnection.Size = new System.Drawing.Size(86, 20);
		this.tbConnection.TabIndex = 17;
		this.tbConnection.KeyPress += new System.Windows.Forms.KeyPressEventHandler(tbConnection_KeyPress);
		this.btnReset.Location = new System.Drawing.Point(2, 156);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(57, 22);
		this.btnReset.TabIndex = 18;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = true;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.cbAccess.AutoSize = true;
		this.cbAccess.Location = new System.Drawing.Point(127, 135);
		this.cbAccess.Name = "cbAccess";
		this.cbAccess.Size = new System.Drawing.Size(60, 17);
		this.cbAccess.TabIndex = 21;
		this.cbAccess.Text = "Accent";
		this.cbAccess.UseVisualStyleBackColor = true;
		this.cbAccess.CheckedChanged += new System.EventHandler(cbAccess_CheckedChanged);
		this.lblPrintGems.AutoSize = true;
		this.lblPrintGems.BackColor = System.Drawing.Color.Transparent;
		this.lblPrintGems.Location = new System.Drawing.Point(56, 116);
		this.lblPrintGems.Name = "lblPrintGems";
		this.lblPrintGems.Size = new System.Drawing.Size(61, 13);
		this.lblPrintGems.TabIndex = 22;
		this.lblPrintGems.Text = "Print Gems:";
		this.cbSide.AutoSize = true;
		this.cbSide.Location = new System.Drawing.Point(71, 135);
		this.cbSide.Name = "cbSide";
		this.cbSide.Size = new System.Drawing.Size(47, 17);
		this.cbSide.TabIndex = 23;
		this.cbSide.Text = "Side";
		this.cbSide.UseVisualStyleBackColor = true;
		this.cbSide.CheckedChanged += new System.EventHandler(cbAccess_CheckedChanged);
		this.cbCenter.AutoSize = true;
		this.cbCenter.Location = new System.Drawing.Point(4, 135);
		this.cbCenter.Name = "cbCenter";
		this.cbCenter.Size = new System.Drawing.Size(57, 17);
		this.cbCenter.TabIndex = 24;
		this.cbCenter.Text = "Center";
		this.cbCenter.UseVisualStyleBackColor = true;
		this.cbCenter.CheckedChanged += new System.EventHandler(cbAccess_CheckedChanged);
		this.btnAbortPrint.Location = new System.Drawing.Point(190, 156);
		this.btnAbortPrint.Name = "btnAbortPrint";
		this.btnAbortPrint.Size = new System.Drawing.Size(67, 23);
		this.btnAbortPrint.TabIndex = 25;
		this.btnAbortPrint.Text = "Abort Print";
		this.btnAbortPrint.UseVisualStyleBackColor = true;
		this.btnAbortPrint.Visible = false;
		this.btnAbortPrint.Click += new System.EventHandler(btnAbortPrint_Click);
		this.lblIPAddress.AutoSize = true;
		this.lblIPAddress.BackColor = System.Drawing.Color.Transparent;
		this.lblIPAddress.Location = new System.Drawing.Point(-1, 7);
		this.lblIPAddress.Name = "lblIPAddress";
		this.lblIPAddress.Size = new System.Drawing.Size(94, 13);
		this.lblIPAddress.TabIndex = 26;
		this.lblIPAddress.Text = "Printer IP Address:";
		this.gbSavedJobs.Controls.Add(this.cbJobSaved2);
		this.gbSavedJobs.Controls.Add(this.cbJobSaved3);
		this.gbSavedJobs.Controls.Add(this.cbJobSaved1);
		this.gbSavedJobs.Controls.Add(this.picJob3);
		this.gbSavedJobs.Controls.Add(this.btnSaveJob3);
		this.gbSavedJobs.Controls.Add(this.picJob2);
		this.gbSavedJobs.Controls.Add(this.btnSaveJob2);
		this.gbSavedJobs.Controls.Add(this.btnDeleteCheckedSavedJobs);
		this.gbSavedJobs.Controls.Add(this.btnPrintCheckedSavedJobs);
		this.gbSavedJobs.Controls.Add(this.picJob1);
		this.gbSavedJobs.Controls.Add(this.btnSaveJob1);
		this.gbSavedJobs.Location = new System.Drawing.Point(2, 185);
		this.gbSavedJobs.Name = "gbSavedJobs";
		this.gbSavedJobs.Size = new System.Drawing.Size(340, 94);
		this.gbSavedJobs.TabIndex = 27;
		this.gbSavedJobs.TabStop = false;
		this.gbSavedJobs.Text = "Saved Print Jobs";
		this.gbSavedJobs.Visible = false;
		this.cbJobSaved2.AutoSize = true;
		this.cbJobSaved2.Location = new System.Drawing.Point(144, 72);
		this.cbJobSaved2.Name = "cbJobSaved2";
		this.cbJobSaved2.Size = new System.Drawing.Size(15, 14);
		this.cbJobSaved2.TabIndex = 33;
		this.cbJobSaved2.Tag = "1";
		this.cbJobSaved2.UseVisualStyleBackColor = true;
		this.cbJobSaved3.AutoSize = true;
		this.cbJobSaved3.Location = new System.Drawing.Point(221, 72);
		this.cbJobSaved3.Name = "cbJobSaved3";
		this.cbJobSaved3.Size = new System.Drawing.Size(15, 14);
		this.cbJobSaved3.TabIndex = 36;
		this.cbJobSaved3.Tag = "2";
		this.cbJobSaved3.UseVisualStyleBackColor = true;
		this.cbJobSaved1.AutoSize = true;
		this.cbJobSaved1.Location = new System.Drawing.Point(65, 72);
		this.cbJobSaved1.Name = "cbJobSaved1";
		this.cbJobSaved1.Size = new System.Drawing.Size(15, 14);
		this.cbJobSaved1.TabIndex = 25;
		this.cbJobSaved1.Tag = "0";
		this.cbJobSaved1.UseVisualStyleBackColor = true;
		this.picJob3.BackColor = System.Drawing.Color.Gray;
		this.picJob3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.picJob3.Location = new System.Drawing.Point(164, 35);
		this.picJob3.Name = "picJob3";
		this.picJob3.Size = new System.Drawing.Size(73, 52);
		this.picJob3.TabIndex = 38;
		this.picJob3.TabStop = false;
		this.btnSaveJob3.Enabled = false;
		this.btnSaveJob3.Font = new System.Drawing.Font("Tahoma", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveJob3.Location = new System.Drawing.Point(165, 15);
		this.btnSaveJob3.Name = "btnSaveJob3";
		this.btnSaveJob3.Size = new System.Drawing.Size(46, 19);
		this.btnSaveJob3.TabIndex = 37;
		this.btnSaveJob3.Tag = "2";
		this.btnSaveJob3.Text = "Save";
		this.btnSaveJob3.UseVisualStyleBackColor = true;
		this.btnSaveJob3.MouseUp += new System.Windows.Forms.MouseEventHandler(btnSaveJob1_MouseUp);
		this.picJob2.BackColor = System.Drawing.Color.Gray;
		this.picJob2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.picJob2.Location = new System.Drawing.Point(86, 35);
		this.picJob2.Name = "picJob2";
		this.picJob2.Size = new System.Drawing.Size(73, 52);
		this.picJob2.TabIndex = 35;
		this.picJob2.TabStop = false;
		this.btnSaveJob2.Enabled = false;
		this.btnSaveJob2.Font = new System.Drawing.Font("Tahoma", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveJob2.Location = new System.Drawing.Point(87, 15);
		this.btnSaveJob2.Name = "btnSaveJob2";
		this.btnSaveJob2.Size = new System.Drawing.Size(46, 19);
		this.btnSaveJob2.TabIndex = 34;
		this.btnSaveJob2.Tag = "1";
		this.btnSaveJob2.Text = "Save";
		this.btnSaveJob2.UseVisualStyleBackColor = true;
		this.btnSaveJob2.MouseUp += new System.Windows.Forms.MouseEventHandler(btnSaveJob1_MouseUp);
		this.btnDeleteCheckedSavedJobs.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDeleteCheckedSavedJobs.Location = new System.Drawing.Point(243, 15);
		this.btnDeleteCheckedSavedJobs.Name = "btnDeleteCheckedSavedJobs";
		this.btnDeleteCheckedSavedJobs.Size = new System.Drawing.Size(91, 23);
		this.btnDeleteCheckedSavedJobs.TabIndex = 32;
		this.btnDeleteCheckedSavedJobs.Text = "Delete Checked";
		this.btnDeleteCheckedSavedJobs.UseVisualStyleBackColor = true;
		this.btnDeleteCheckedSavedJobs.MouseUp += new System.Windows.Forms.MouseEventHandler(btnDeleteCheckedSavedJobs_MouseUp);
		this.btnPrintCheckedSavedJobs.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintCheckedSavedJobs.Location = new System.Drawing.Point(243, 64);
		this.btnPrintCheckedSavedJobs.Name = "btnPrintCheckedSavedJobs";
		this.btnPrintCheckedSavedJobs.Size = new System.Drawing.Size(91, 23);
		this.btnPrintCheckedSavedJobs.TabIndex = 31;
		this.btnPrintCheckedSavedJobs.Text = "Print Checked";
		this.btnPrintCheckedSavedJobs.UseVisualStyleBackColor = true;
		this.btnPrintCheckedSavedJobs.MouseUp += new System.Windows.Forms.MouseEventHandler(btnPrintCheckedSavedJobs_MouseUp);
		this.picJob1.BackColor = System.Drawing.Color.Gray;
		this.picJob1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.picJob1.Location = new System.Drawing.Point(8, 35);
		this.picJob1.Name = "picJob1";
		this.picJob1.Size = new System.Drawing.Size(73, 52);
		this.picJob1.TabIndex = 30;
		this.picJob1.TabStop = false;
		this.btnSaveJob1.Enabled = false;
		this.btnSaveJob1.Font = new System.Drawing.Font("Tahoma", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveJob1.Location = new System.Drawing.Point(12, 15);
		this.btnSaveJob1.Name = "btnSaveJob1";
		this.btnSaveJob1.Size = new System.Drawing.Size(46, 19);
		this.btnSaveJob1.TabIndex = 28;
		this.btnSaveJob1.Tag = "0";
		this.btnSaveJob1.Text = "Save";
		this.btnSaveJob1.UseVisualStyleBackColor = true;
		this.btnSaveJob1.Click += new System.EventHandler(btnSaveJob1_Click);
		this.btnSaveJob1.MouseUp += new System.Windows.Forms.MouseEventHandler(btnSaveJob1_MouseUp);
		this.tbSupportDensity.AutoSize = false;
		this.tbSupportDensity.Location = new System.Drawing.Point(184, 128);
		this.tbSupportDensity.Maximum = 0;
		this.tbSupportDensity.Minimum = -10;
		this.tbSupportDensity.Name = "tbSupportDensity";
		this.tbSupportDensity.RightToLeftLayout = true;
		this.tbSupportDensity.Size = new System.Drawing.Size(89, 25);
		this.tbSupportDensity.TabIndex = 28;
		this.tbSupportDensity.TickStyle = System.Windows.Forms.TickStyle.None;
		this.tbSupportDensity.Visible = false;
		this.tbSupportDensity.ValueChanged += new System.EventHandler(tbSupportDensity_ValueChanged);
		this.lblSupportDensity.AutoSize = true;
		this.lblSupportDensity.BackColor = System.Drawing.Color.Transparent;
		this.lblSupportDensity.Location = new System.Drawing.Point(185, 115);
		this.lblSupportDensity.Name = "lblSupportDensity";
		this.lblSupportDensity.Size = new System.Drawing.Size(85, 13);
		this.lblSupportDensity.TabIndex = 29;
		this.lblSupportDensity.Text = "Support Density:";
		this.lblSupportDensity.Visible = false;
		this.gbActivation.Controls.Add(this.label1);
		this.gbActivation.Controls.Add(this.btnActivatePrinter);
		this.gbActivation.Controls.Add(this.tbActiveCompanyName);
		this.gbActivation.Controls.Add(this.tbActiveEmail);
		this.gbActivation.Location = new System.Drawing.Point(4, 409);
		this.gbActivation.Name = "gbActivation";
		this.gbActivation.Size = new System.Drawing.Size(338, 144);
		this.gbActivation.TabIndex = 30;
		this.gbActivation.TabStop = false;
		this.gbActivation.Text = "Activation";
		this.label1.BackColor = System.Drawing.Color.Transparent;
		this.label1.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 16);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(245, 32);
		this.label1.TabIndex = 36;
		this.label1.Text = "Projet has to be activated and firmware updated before first use!";
		this.btnActivatePrinter.Font = new System.Drawing.Font("Tahoma", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnActivatePrinter.Location = new System.Drawing.Point(150, 119);
		this.btnActivatePrinter.Name = "btnActivatePrinter";
		this.btnActivatePrinter.Size = new System.Drawing.Size(59, 19);
		this.btnActivatePrinter.TabIndex = 35;
		this.btnActivatePrinter.Tag = "1";
		this.btnActivatePrinter.Text = "Activate";
		this.btnActivatePrinter.UseVisualStyleBackColor = true;
		this.btnActivatePrinter.MouseUp += new System.Windows.Forms.MouseEventHandler(btnActivatePrinter_MouseUp);
		this.tbActiveCompanyName.Location = new System.Drawing.Point(4, 86);
		this.tbActiveCompanyName.Name = "tbActiveCompanyName";
		this.tbActiveCompanyName.Size = new System.Drawing.Size(328, 20);
		this.tbActiveCompanyName.TabIndex = 28;
		this.tbActiveCompanyName.Text = "Company Name";
		this.tbActiveEmail.Location = new System.Drawing.Point(4, 57);
		this.tbActiveEmail.Name = "tbActiveEmail";
		this.tbActiveEmail.Size = new System.Drawing.Size(328, 20);
		this.tbActiveEmail.TabIndex = 27;
		this.tbActiveEmail.Text = "Email Address";
		base.AcceptButton = this.butPrint;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.butCancel;
		base.ClientSize = new System.Drawing.Size(343, 204);
		base.ControlBox = false;
		base.Controls.Add(this.gbActivation);
		base.Controls.Add(this.lblSupportDensity);
		base.Controls.Add(this.tbSupportDensity);
		base.Controls.Add(this.gbSavedJobs);
		base.Controls.Add(this.lblIPAddress);
		base.Controls.Add(this.btnAbortPrint);
		base.Controls.Add(this.butCancel);
		base.Controls.Add(this.cbCenter);
		base.Controls.Add(this.cbSide);
		base.Controls.Add(this.lblPrintGems);
		base.Controls.Add(this.cbAccess);
		base.Controls.Add(this.gbRotateModel);
		base.Controls.Add(this.btnReset);
		base.Controls.Add(this.tbConnection);
		base.Controls.Add(this.statusStrip1);
		base.Controls.Add(this.btnCreateSup);
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.butTestConnection);
		base.Controls.Add(this.butPrint);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.Name = "FrmPrintToProJet1200";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Print to ProJet 1200";
		base.TopMost = true;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FrmPrintToProJet1200_FormClosed);
		base.Load += new System.EventHandler(FrmPrintToProJet1200_Load);
		this.gbRotateModel.ResumeLayout(false);
		this.gbRotateModel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.trkRotateAmountX).EndInit();
		((System.ComponentModel.ISupportInitialize)this.trkRotateAmountZ).EndInit();
		((System.ComponentModel.ISupportInitialize)this.trkRotateAmountY).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.trackBar1).EndInit();
		this.statusStrip1.ResumeLayout(false);
		this.statusStrip1.PerformLayout();
		this.gbSavedJobs.ResumeLayout(false);
		this.gbSavedJobs.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picJob3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picJob2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picJob1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.tbSupportDensity).EndInit();
		this.gbActivation.ResumeLayout(false);
		this.gbActivation.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
