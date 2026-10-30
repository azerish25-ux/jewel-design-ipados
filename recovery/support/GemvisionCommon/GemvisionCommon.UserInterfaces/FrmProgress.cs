using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace GemvisionCommon.UserInterfaces;

public class FrmProgress : Form
{
	private const int _formBorderMarginBottom = 39;

	private readonly string _caption = "Progress";

	private readonly bool _doAllowCancellation;

	private readonly CancellationTokenSource _cancellationTokenSource;

	private IContainer components = null;

	private ProgressBar prgProgressBar;

	private Button butCancel;

	private Label lblMessage;

	public CancellationToken CancellationToken => _cancellationTokenSource?.Token ?? CancellationToken.None;

	public FrmProgress(string caption, bool doAllowCancel)
	{
		InitializeComponent();
		_caption = caption;
		_doAllowCancellation = doAllowCancel;
		if (doAllowCancel)
		{
			_cancellationTokenSource = new CancellationTokenSource();
		}
	}

	private void FrmProgress_Load(object sender, EventArgs e)
	{
		Text = _caption;
		UpdateLayout();
	}

	private void butCancel_Click(object sender, EventArgs e)
	{
		_cancellationTokenSource?.Cancel();
	}

	private void UpdateLayout()
	{
		bool flag = !string.IsNullOrWhiteSpace(lblMessage.Text);
		lblMessage.Visible = flag;
		int num = prgProgressBar.Location.X;
		int num2 = ((!flag) ? (base.Padding.Top + lblMessage.Margin.Top) : (base.Padding.Top + lblMessage.Margin.Top + lblMessage.Height + lblMessage.Margin.Bottom + prgProgressBar.Margin.Top));
		prgProgressBar.Location = new Point(num, num2);
		butCancel.Visible = _doAllowCancellation;
		int num3 = butCancel.Location.X;
		int num4 = num2 + prgProgressBar.Height + prgProgressBar.Margin.Bottom + butCancel.Margin.Top;
		butCancel.Location = new Point(num3, num4);
		int num5 = base.Size.Width;
		int num6 = base.Padding.Top + prgProgressBar.Margin.Top + prgProgressBar.Height + prgProgressBar.Margin.Bottom + base.Padding.Bottom + 39;
		if (flag)
		{
			num6 += lblMessage.Margin.Top + lblMessage.Height + lblMessage.Margin.Bottom;
		}
		if (_doAllowCancellation)
		{
			num6 += butCancel.Margin.Top + butCancel.Height + butCancel.Margin.Bottom;
		}
		MaximumSize = new Size(num5, num6);
		MinimumSize = new Size(num5, num6);
		base.Size = new Size(num5, num6);
	}

	public void UpdateProgress(string message, int value)
	{
		BeginInvoke((Action)delegate
		{
			lblMessage.Text = message;
			prgProgressBar.Value = value;
			UpdateLayout();
		});
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
		this.prgProgressBar = new System.Windows.Forms.ProgressBar();
		this.butCancel = new System.Windows.Forms.Button();
		this.lblMessage = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.prgProgressBar.Location = new System.Drawing.Point(6, 25);
		this.prgProgressBar.Name = "prgProgressBar";
		this.prgProgressBar.Size = new System.Drawing.Size(272, 27);
		this.prgProgressBar.TabIndex = 0;
		this.butCancel.Location = new System.Drawing.Point(108, 58);
		this.butCancel.Name = "butCancel";
		this.butCancel.Size = new System.Drawing.Size(68, 23);
		this.butCancel.TabIndex = 2;
		this.butCancel.Text = "Cancel";
		this.butCancel.UseVisualStyleBackColor = true;
		this.butCancel.Click += new System.EventHandler(butCancel_Click);
		this.lblMessage.AutoEllipsis = true;
		this.lblMessage.Location = new System.Drawing.Point(6, 6);
		this.lblMessage.Margin = new System.Windows.Forms.Padding(3);
		this.lblMessage.Name = "lblMessage";
		this.lblMessage.Size = new System.Drawing.Size(272, 13);
		this.lblMessage.TabIndex = 3;
		this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(284, 87);
		base.ControlBox = false;
		base.Controls.Add(this.lblMessage);
		base.Controls.Add(this.butCancel);
		base.Controls.Add(this.prgProgressBar);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		this.MaximumSize = new System.Drawing.Size(300, 126);
		this.MinimumSize = new System.Drawing.Size(300, 126);
		base.Name = "FrmProgress";
		base.Padding = new System.Windows.Forms.Padding(3);
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		this.Text = "Progress";
		base.TopMost = true;
		base.Load += new System.EventHandler(FrmProgress_Load);
		base.ResumeLayout(false);
	}
}
