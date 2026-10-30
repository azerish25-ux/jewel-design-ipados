using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GV.CounterSketch.Shared.CAD;
using GV.CounterSketch.Studio.Utilities.Helpers;
using Localization;

namespace GV.CounterSketch.Studio.LegacyUIControl;

public class CurveEditorForm : Form
{
	private readonly List<CurvePoint> _curvePoints = new List<CurvePoint>();

	private readonly List<double> _editPointParams;

	private readonly List<Point2D> _pE2;

	private readonly List<double> _storedPointParams;

	public Guid BlockInstanceGuid;

	public bool SweepPointsShow;

	private bool _closed;

	private int _cphit = -1;

	private double _frmSizeX;

	private double _frmSizeY;

	private bool _lDown;

	private bool _mirror;

	private bool _mirrorpoints;

	private NurbsCurve _nurbsCurve;

	private double _originX;

	private double _originY;

	private List<Point2D> _polyline;

	private bool _rDown;

	private bool _region;

	private int _rotationtype;

	private double _scaleX;

	private double _scaleY;

	private int _xd;

	private int _xDr;

	private int _yd;

	private int _yDr;

	private IContainer components;

	private PictureBox pictureBox1;

	private CheckBox chkMirror;

	private CheckBox chkMirrorPoints;

	private Button btnRebuild;

	private Label label1;

	private CheckBox chkAntiAlias;

	public int CurveValue { get; set; }

	public CurveEditorForm()
	{
		InitializeComponent();
		InitializeLanguage();
		_editPointParams = new List<double>();
		_storedPointParams = new List<double>();
		_pE2 = new List<Point2D>();
		base.StartPosition = FormStartPosition.CenterScreen;
		base.TopMost = true;
		BackColor = Color.White;
		pictureBox1.BackColor = Color.White;
		label1.ForeColor = Color.Black;
		chkMirror.ForeColor = Color.Black;
		chkMirrorPoints.ForeColor = Color.Black;
	}

	public void DrawCurve()
	{
		if (_nurbsCurve == null || pictureBox1.Width < 1 || pictureBox1.Height < 1)
		{
			return;
		}
		_polyline = GetPolyline(_nurbsCurve);
		Bitmap image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
		Graphics graphics = Graphics.FromImage(image);
		if (chkAntiAlias.Checked)
		{
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
		}
		Pen pen = new Pen(Color.FromArgb(230, 230, 230), 1f);
		Pen pen2 = new Pen(Color.FromArgb(60, 60, 180), 1f);
		Pen pen3 = new Pen(Color.FromArgb(228, 64, 64), 3f);
		Pen pen4 = new Pen(Color.FromArgb(64, 228, 64), 3f);
		Pen pen5 = new Pen(Color.FromArgb(120, 120, 120), 3f);
		Pen pen6 = new Pen(Color.FromArgb(205, 100, 0), 1f);
		pen6.DashPattern = new float[2] { 4f, 4f };
		Pen pen7 = new Pen(Color.FromArgb(205, 100, 0), 1f);
		Pen pen8 = new Pen(Color.FromArgb(155, 55, 255), 1f);
		Pen pen9 = new Pen(Color.FromArgb(0, 155, 200), 1f);
		for (int i = -20; i <= 20; i++)
		{
			if (i % 5 == 0)
			{
				graphics.DrawLine(pen2, fx(-20.0), fy(i), fx(20.0), fy(i));
				graphics.DrawLine(pen2, fx(i), fy(-20.0), fx(i), fy(20.0));
			}
			else
			{
				graphics.DrawLine(pen, fx(-20.0), fy(i), fx(20.0), fy(i));
				graphics.DrawLine(pen, fx(i), fy(-20.0), fx(i), fy(20.0));
			}
		}
		graphics.DrawLine(pen3, fx(0.0), fy(0.0), fx(0.0), fy(20.0));
		graphics.DrawLine(pen4, fx(0.0), fy(0.0), fx(20.0), fy(0.0));
		for (int j = 0; j < _curvePoints.Count; j++)
		{
			int num = (j + 1) % _curvePoints.Count;
			if ((_closed || num != 0) && (!_mirror || (_curvePoints[j].MirrorPoint >= 0 && _curvePoints[num].MirrorPoint >= 0)))
			{
				Point2D p = _curvePoints[j].P2;
				Point2D p2 = _curvePoints[num].P2;
				Pen pen10 = ((!_curvePoints[j].Selected && !_curvePoints[num].Selected) ? pen6 : pen8);
				graphics.DrawLine(pen10, fx(p.X), fy(p.Y), fx(p2.X), fy(p2.Y));
			}
		}
		for (int k = 0; k < _polyline.Count - 1; k++)
		{
			Point2D p = _polyline[k];
			Point2D p2 = _polyline[k + 1];
			graphics.DrawLine(pen5, fx(p.X), fy(p.Y), fx(p2.X), fy(p2.Y));
		}
		for (int l = 0; l < _curvePoints.Count; l++)
		{
			if (!_mirror || _curvePoints[l].MirrorPoint >= 0)
			{
				int num2 = fx(_curvePoints[l].P2.X);
				int num3 = fy(_curvePoints[l].P2.Y);
				Pen pen11 = ((!_curvePoints[l].Selected) ? pen7 : pen8);
				graphics.DrawLine(pen11, num2 + 3, num3 + 3, num2 + 3, num3 - 3);
				graphics.DrawLine(pen11, num2 + 3, num3 - 3, num2 - 3, num3 - 3);
				graphics.DrawLine(pen11, num2 - 3, num3 - 3, num2 - 3, num3 + 3);
				graphics.DrawLine(pen11, num2 - 3, num3 + 3, num2 + 3, num3 + 3);
				graphics.DrawLine(pen11, num2 + 2, num3 + 2, num2 + 2, num3 - 2);
				graphics.DrawLine(pen11, num2 + 2, num3 - 2, num2 - 2, num3 - 2);
				graphics.DrawLine(pen11, num2 - 2, num3 - 2, num2 - 2, num3 + 2);
				graphics.DrawLine(pen11, num2 - 2, num3 + 2, num2 + 2, num3 + 2);
				pen11 = pen8;
				graphics.DrawLine(pen11, num2 + 1, num3 + 1, num2 + 1, num3 - 1);
				graphics.DrawLine(pen11, num2 + 1, num3 - 1, num2 - 1, num3 - 1);
				graphics.DrawLine(pen11, num2 - 1, num3 - 1, num2 - 1, num3 + 1);
				graphics.DrawLine(pen11, num2 - 1, num3 + 1, num2 + 1, num3 + 1);
			}
		}
		_pE2.Clear();
		for (int m = 0; m < _editPointParams.Count; m++)
		{
			Point3D normalizedPointAt = GetNormalizedPointAt(_nurbsCurve, _editPointParams[m]);
			int num4 = fx(normalizedPointAt.X);
			int num5 = fy(normalizedPointAt.Y);
			_pE2.Add(new Point2D(num4, num5));
			graphics.DrawLine(pen9, num4 + 4, num5 + 4, num4 - 4, num5 - 4);
			graphics.DrawLine(pen9, num4 + 4, num5 - 4, num4 - 4, num5 + 4);
			graphics.DrawLine(pen9, num4 + 4, num5 + 4, num4 + 4, num5 - 4);
			graphics.DrawLine(pen9, num4 + 4, num5 - 4, num4 - 4, num5 - 4);
			graphics.DrawLine(pen9, num4 - 4, num5 - 4, num4 - 4, num5 + 4);
			graphics.DrawLine(pen9, num4 - 4, num5 + 4, num4 + 4, num5 + 4);
		}
		if (_region)
		{
			Pen pen12 = pen8;
			graphics.DrawLine(pen12, _xDr, _yDr, _xDr, _yd);
			graphics.DrawLine(pen12, _xDr, _yd, _xd, _yd);
			graphics.DrawLine(pen12, _xd, _yd, _xd, _yDr);
			graphics.DrawLine(pen12, _xd, _yDr, _xDr, _yDr);
		}
		pictureBox1.Image = image;
		pictureBox1.Refresh();
	}

	public void SetCurve(NurbsCurve newCrv, List<double> parameters)
	{
		_nurbsCurve = newCrv;
		if (_nurbsCurve == null)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		Point3D point = new Point3D();
		for (int i = 0; i < _nurbsCurve.CVCount; i++)
		{
			_nurbsCurve.GetCV(i, ref point);
			if (Math.Abs(point.X) > 0.01)
			{
				flag = true;
			}
			if (Math.Abs(point.Y) > 0.01)
			{
				flag2 = true;
			}
			if (Math.Abs(point.Z) > 0.01)
			{
				flag3 = true;
			}
		}
		_rotationtype = 0;
		if (flag && !flag2 && flag3)
		{
			_rotationtype = 1;
		}
		if (!flag && flag2 && flag3)
		{
			_rotationtype = 2;
		}
		rotatecurveUp(ref _nurbsCurve);
		_editPointParams.Clear();
		_storedPointParams.Clear();
		for (int j = 0; j < parameters.Count; j++)
		{
			if (SweepPointsShow)
			{
				_editPointParams.Add(parameters[j]);
			}
			_storedPointParams.Add(parameters[j]);
		}
		int num = pictureBox1.Width;
		int num2 = pictureBox1.Height;
		BoundingBox boundingBox = GetBoundingBox(_nurbsCurve);
		double num3 = boundingBox.Max.X - boundingBox.Min.X;
		double num4 = boundingBox.Max.Y - boundingBox.Min.Y;
		double num5 = (boundingBox.Max.X + boundingBox.Min.X) / 2.0;
		double num6 = (boundingBox.Max.Y + boundingBox.Min.Y) / 2.0;
		_scaleX = (double)num / (num3 + 6.0);
		_scaleY = (double)num2 / (num4 + 6.0);
		if (_scaleX > _scaleY)
		{
			_scaleX = _scaleY;
		}
		else
		{
			_scaleY = _scaleX;
		}
		_originX = (double)(-(num / 2)) / _scaleX + num5;
		_originY = (double)(num2 / 2) / _scaleY + num6;
		UpdateCurve();
	}

	private void CheckMirror(ref bool mirror1, ref bool mirror2)
	{
		mirror1 = true;
		for (int i = 0; i < _nurbsCurve.CVCount / 2; i++)
		{
			int i2 = _nurbsCurve.CVCount - 1 - i;
			Point4D point = new Point4D();
			Point4D point2 = new Point4D();
			_nurbsCurve.GetCV(i, ref point);
			_nurbsCurve.GetCV(i2, ref point2);
			if (Math.Abs(point.X + point2.X) > 0.0001)
			{
				mirror1 = false;
			}
			if (Math.Abs(point.Y - point2.Y) > 0.0001)
			{
				mirror1 = false;
			}
			if (Math.Abs(point.Z - point2.Z) > 0.0001)
			{
				mirror1 = false;
			}
			if (Math.Abs(point.W - point2.W) > 0.0001)
			{
				mirror1 = false;
			}
		}
		mirror2 = true;
		for (int j = 0; j < _editPointParams.Count / 2; j++)
		{
			int i2 = _editPointParams.Count - 1 - j;
			Point3D normalizedPointAt = GetNormalizedPointAt(_nurbsCurve, _editPointParams[j]);
			Point3D normalizedPointAt2 = GetNormalizedPointAt(_nurbsCurve, _editPointParams[i2]);
			if (Math.Abs(normalizedPointAt.X + normalizedPointAt2.X) > 0.0001)
			{
				mirror2 = false;
			}
			if (Math.Abs(normalizedPointAt.Y - normalizedPointAt2.Y) > 0.0001)
			{
				mirror2 = false;
			}
			if (Math.Abs(normalizedPointAt.Z - normalizedPointAt2.Z) > 0.0001)
			{
				mirror2 = false;
			}
		}
	}

	private void chkMirror_CheckedChanged(object sender, EventArgs e)
	{
		if (!chkMirror.Checked)
		{
			_mirror = false;
			_mirrorpoints = false;
			chkMirrorPoints.Checked = false;
			DrawCurve();
			return;
		}
		bool mirror = false;
		bool mirror2 = false;
		CheckMirror(ref mirror, ref mirror2);
		if (mirror)
		{
			_mirror = true;
			DrawCurve();
		}
		else
		{
			chkMirror.Checked = false;
			chkMirrorPoints.Checked = false;
		}
	}

	private void chkMirrorPoints_CheckedChanged(object sender, EventArgs e)
	{
		if (!chkMirrorPoints.Checked)
		{
			_mirrorpoints = false;
			DrawCurve();
			return;
		}
		bool mirror = false;
		bool mirror2 = false;
		CheckMirror(ref mirror, ref mirror2);
		if (mirror)
		{
			_mirrorpoints = true;
			DrawCurve();
		}
		else
		{
			chkMirrorPoints.Checked = false;
		}
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		base.MouseWheel += frmCurveEditor_MouseWheel;
		_frmSizeX = base.Size.Width;
		_frmSizeY = base.Size.Height;
		base.Left = 350;
		DrawCurve();
	}

	private void Form1_Resize(object sender, EventArgs e)
	{
		_originX -= ((double)base.Size.Width - _frmSizeX) / 2.0 / _scaleX;
		_originY += ((double)base.Size.Height - _frmSizeY) / 2.0 / _scaleY;
		_frmSizeX = base.Size.Width;
		_frmSizeY = base.Size.Height;
		DrawCurve();
	}

	private void frmCurveEditor_MouseWheel(object sender, MouseEventArgs e)
	{
		double num = e.Delta;
		double num2 = fxr(e.X);
		double num3 = fyr(e.Y);
		if (num > 0.0)
		{
			_scaleX *= 1.1;
			_scaleY *= 1.1;
		}
		else
		{
			_scaleX /= 1.1;
			_scaleY /= 1.1;
		}
		double num4 = fxr(e.X);
		double num5 = fyr(e.Y);
		_originX += num2 - num4;
		_originY += num3 - num5;
		DrawCurve();
	}

	private int fx(double x)
	{
		return (int)((x - _originX) * _scaleX);
	}

	private double fxr(double x)
	{
		return x / _scaleX + _originX;
	}

	private int fy(double y)
	{
		return (int)((0.0 - (y - _originY)) * _scaleY);
	}

	private double fyr(double y)
	{
		return (0.0 - y) / _scaleY + _originY;
	}

	private BoundingBox GetBoundingBox(NurbsCurve nc)
	{
		return UIHelper.CoreDataService.CurveEditorNurbsGetBoundingBox(nc);
	}

	private double GetClosestPoint(NurbsCurve nc, Point3D testHit)
	{
		return UIHelper.CoreDataService.CurveEditorNurbsGetClosestPoint(nc, testHit);
	}

	private Point3D GetNormalizedPointAt(NurbsCurve nc, double editPointParam)
	{
		return UIHelper.CoreDataService.CurveEditorNurbsGetNormalizedPointAt(nc, editPointParam);
	}

	private List<Point2D> GetPolyline(NurbsCurve nc)
	{
		return new List<Point2D>(UIHelper.CoreDataService.CurveEditorNurbsGetPolyline(nc));
	}

	private void InitializeLanguage()
	{
		label1.Text = Strings.CurveEditorNote;
		chkMirror.Text = Strings.CurveEditorMirrorCurve;
		chkMirrorPoints.Text = Strings.CurveEditorMirrorSweepPoints;
		chkAntiAlias.Text = Strings.CurveEditorSmoothLines;
		base.Name = Strings.CurveProfileEditor;
		btnRebuild.Text = Strings.CurveEditorRebuild;
	}

	private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
	{
		_xd = e.X;
		_yd = e.Y;
		if (e.Button == MouseButtons.Right)
		{
			_rDown = true;
		}
		if (e.Button != MouseButtons.Left)
		{
			return;
		}
		if (Control.ModifierKeys == Keys.Control)
		{
			for (int i = 0; i < _curvePoints.Count; i++)
			{
				_curvePoints[i].Selected = false;
			}
			_cphit = -1;
			for (int j = 0; j < _editPointParams.Count; j++)
			{
				if (Math.Abs(_pE2[j].X - (double)e.X) < 4.0 && Math.Abs(_pE2[j].Y - (double)e.Y) < 4.0)
				{
					_cphit = j;
				}
			}
			if (_cphit >= 0)
			{
				_lDown = true;
			}
		}
		else
		{
			int num = -1;
			for (int k = 0; k < _curvePoints.Count; k++)
			{
				if ((!_mirror || _curvePoints[k].MirrorPoint >= 0) && Math.Abs(fx(_curvePoints[k].P2.X) - e.X) < 4 && Math.Abs(fy(_curvePoints[k].P2.Y) - e.Y) < 4)
				{
					num = k;
				}
			}
			if (num >= 0)
			{
				if (!_curvePoints[num].Selected)
				{
					if (Control.ModifierKeys != Keys.Shift)
					{
						for (int l = 0; l < _curvePoints.Count; l++)
						{
							_curvePoints[l].Selected = false;
						}
					}
					_curvePoints[num].Selected = true;
				}
				_lDown = true;
			}
			else
			{
				if (Control.ModifierKeys != Keys.Shift)
				{
					for (int m = 0; m < _curvePoints.Count; m++)
					{
						_curvePoints[m].Selected = false;
					}
				}
				_xDr = e.X;
				_yDr = e.Y;
				_region = true;
			}
		}
		DrawCurve();
	}

	private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
	{
		if (_rDown)
		{
			int num = e.X;
			int num2 = e.Y;
			_originX += (double)(-(num - _xd)) / _scaleX;
			_originY += (double)(num2 - _yd) / _scaleY;
			_xd = num;
			_yd = num2;
			DrawCurve();
		}
		else if (_lDown)
		{
			int num = e.X;
			int num2 = e.Y;
			for (int i = 0; i < _curvePoints.Count; i++)
			{
				if ((_mirror && _curvePoints[i].MirrorPoint < 0) || !_curvePoints[i].Selected)
				{
					continue;
				}
				int num3 = num;
				if (_curvePoints[i].ClampXWhenMirror)
				{
					num3 = _xd;
				}
				_curvePoints[i].P2.X += (double)(num3 - _xd) / _scaleX;
				_curvePoints[i].P2.Y += (double)(-(num2 - _yd)) / _scaleY;
				_curvePoints[i].P4.X = _curvePoints[i].P2.X * _curvePoints[i].P4.W;
				_curvePoints[i].P4.Y = _curvePoints[i].P2.Y * _curvePoints[i].P4.W;
				if (_curvePoints[i].NcIndex >= 0)
				{
					_nurbsCurve.SetCV(_curvePoints[i].NcIndex, _curvePoints[i].P4);
				}
				if (_curvePoints[i].NcIndex2 >= 0)
				{
					_nurbsCurve.SetCV(_curvePoints[i].NcIndex2, _curvePoints[i].P4);
				}
				if (!_mirror || _curvePoints[i].MirrorPoint < 0)
				{
					continue;
				}
				int mirrorPoint = _curvePoints[i].MirrorPoint;
				if (mirrorPoint >= 0 && mirrorPoint < _curvePoints.Count && mirrorPoint != i)
				{
					_curvePoints[mirrorPoint].P2.X += (double)(-(num3 - _xd)) / _scaleX;
					_curvePoints[mirrorPoint].P2.Y += (double)(-(num2 - _yd)) / _scaleY;
					_curvePoints[mirrorPoint].P4.X = _curvePoints[mirrorPoint].P2.X * _curvePoints[mirrorPoint].P4.W;
					_curvePoints[mirrorPoint].P4.Y = _curvePoints[mirrorPoint].P2.Y * _curvePoints[mirrorPoint].P4.W;
					if (_curvePoints[mirrorPoint].NcIndex >= 0)
					{
						_nurbsCurve.SetCV(_curvePoints[mirrorPoint].NcIndex, _curvePoints[mirrorPoint].P4);
					}
					if (_curvePoints[mirrorPoint].NcIndex2 >= 0)
					{
						_nurbsCurve.SetCV(_curvePoints[mirrorPoint].NcIndex2, _curvePoints[mirrorPoint].P4);
					}
				}
			}
			if (_cphit >= 0)
			{
				Point3D testHit = new Point3D(fxr(num), fyr(num2), 0.0);
				_editPointParams[_cphit] = GetClosestPoint(_nurbsCurve, testHit);
				if (_mirrorpoints)
				{
					testHit = new Point3D(0.0 - fxr(num), fyr(num2), 0.0);
					_editPointParams[3 - _cphit] = GetClosestPoint(_nurbsCurve, testHit);
				}
			}
			_xd = num;
			_yd = num2;
			DrawCurve();
			List<double> list = new List<double>();
			for (int j = 0; j < _editPointParams.Count; j++)
			{
				list.Add(_editPointParams[j]);
			}
			list.Sort();
			NurbsCurve nc = _nurbsCurve.Duplicate();
			rotatecurveDown(ref nc);
			UIHelper.CoreDataService.CurveEditorUpdateCurveObjectFromUI(BlockInstanceGuid, CurveValue, nc, list.ToArray(), b: true);
		}
		else if (_region)
		{
			_xd = e.X;
			_yd = e.Y;
			DrawCurve();
		}
	}

	private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
	{
		if (_region)
		{
			int num;
			int num2;
			if (_xd > _xDr)
			{
				num = _xDr;
				num2 = _xd;
			}
			else
			{
				num = _xd;
				num2 = _xDr;
			}
			int num3;
			int num4;
			if (_yd > _yDr)
			{
				num3 = _yDr;
				num4 = _yd;
			}
			else
			{
				num3 = _yd;
				num4 = _yDr;
			}
			for (int i = 0; i < _curvePoints.Count; i++)
			{
				if (!_mirror || _curvePoints[i].MirrorPoint >= 0)
				{
					int num5 = fx(_curvePoints[i].P2.X);
					int num6 = fy(_curvePoints[i].P2.Y);
					if (num5 >= num && num5 <= num2 && num6 >= num3 && num6 <= num4)
					{
						_curvePoints[i].Selected = true;
					}
				}
			}
			_region = false;
			DrawCurve();
			return;
		}
		if (_lDown)
		{
			List<double> list = new List<double>();
			if (_editPointParams.Count == _storedPointParams.Count)
			{
				for (int j = 0; j < _editPointParams.Count; j++)
				{
					list.Add(_editPointParams[j]);
				}
			}
			else
			{
				for (int k = 0; k < _storedPointParams.Count; k++)
				{
					list.Add(_storedPointParams[k]);
				}
			}
			list.Sort();
			NurbsCurve nc = _nurbsCurve.Duplicate();
			rotatecurveDown(ref nc);
			UIHelper.CoreDataService.CurveEditorUpdateCurveObjectFromUI(BlockInstanceGuid, CurveValue, nc, list.ToArray(), b: false);
		}
		_cphit = -1;
		_rDown = false;
		_lDown = false;
	}

	private NurbsCurve Rebuild(NurbsCurve nc, int i, int i1)
	{
		return UIHelper.CoreDataService.CurveEditorNurbsRebuild(nc, i, i1);
	}

	private void rebuild_Click(object sender, EventArgs e)
	{
		List<double> list = new List<double>();
		if (_editPointParams.Count == _storedPointParams.Count)
		{
			for (int i = 0; i < _editPointParams.Count; i++)
			{
				list.Add(_editPointParams[i]);
			}
		}
		else
		{
			for (int j = 0; j < _storedPointParams.Count; j++)
			{
				list.Add(_storedPointParams[j]);
			}
		}
		NurbsCurve nc = _nurbsCurve.Duplicate();
		rotatecurveDown(ref nc);
		NurbsCurve nc2 = Rebuild(nc, 3, nc.CVCount - 3);
		nc2 = SetDomain(nc2, 0, 1);
		UIHelper.CoreDataService.CurveEditorUpdateCurveObjectFromUI(BlockInstanceGuid, CurveValue, nc2, list.ToArray(), b: false);
		SetCurve(nc2, list);
		DrawCurve();
	}

	private void rotatecurveDown(ref NurbsCurve nc)
	{
		switch (_rotationtype)
		{
		case 0:
			break;
		case 1:
		{
			for (int j = 0; j < nc.CVCount; j++)
			{
				nc.Points[j].Z = nc.Points[j].Y;
				nc.Points[j].Y = 0.0;
			}
			break;
		}
		case 2:
		{
			for (int i = 0; i < nc.CVCount; i++)
			{
				nc.Points[i].Z = nc.Points[i].Y;
				nc.Points[i].Y = 0.0;
				nc.Points[i].Y = nc.Points[i].X;
				nc.Points[i].X = 0.0;
			}
			break;
		}
		}
	}

	private void rotatecurveUp(ref NurbsCurve nc)
	{
		switch (_rotationtype)
		{
		case 0:
			break;
		case 1:
		{
			for (int j = 0; j < nc.CVCount; j++)
			{
				nc.Points[j].Y = nc.Points[j].Z;
				nc.Points[j].Z = 0.0;
			}
			break;
		}
		case 2:
		{
			for (int i = 0; i < nc.CVCount; i++)
			{
				nc.Points[i].X = nc.Points[i].Y;
				nc.Points[i].Y = 0.0;
				nc.Points[i].Y = nc.Points[i].Z;
				nc.Points[i].Z = 0.0;
			}
			break;
		}
		}
	}

	private NurbsCurve SetDomain(NurbsCurve nc, int i, int i1)
	{
		return UIHelper.CoreDataService.CurveEditorNurbsSetDomain(nc, i, i1);
	}

	private void UpdateCurve()
	{
		_curvePoints.Clear();
		if (_nurbsCurve.IsClosed && !_nurbsCurve.IsClamped)
		{
			for (int i = 0; i < _nurbsCurve.CVCount - _nurbsCurve.Degree; i++)
			{
				CurvePoint curvePoint = new CurvePoint();
				Point4D point = curvePoint.P4;
				_nurbsCurve.GetCV(i, ref point);
				curvePoint.NcIndex = i;
				if (i < _nurbsCurve.Degree)
				{
					curvePoint.NcIndex2 = _nurbsCurve.CVCount - _nurbsCurve.Degree + i;
				}
				int degree = _nurbsCurve.Degree;
				int num = _nurbsCurve.Degree / 2;
				int num2 = _nurbsCurve.CVCount - _nurbsCurve.Degree;
				int num3 = (num2 - num) / 2;
				if (num2 % 2 == 0)
				{
					if (i >= num && i - num - 1 <= num3)
					{
						curvePoint.MirrorPoint = (num2 + degree - i - 1) % num2;
					}
				}
				else if (i >= num && i - num - num / 2 <= num3)
				{
					curvePoint.MirrorPoint = (num2 + degree - i - 1) % num2;
				}
				if (i == curvePoint.MirrorPoint)
				{
					curvePoint.ClampXWhenMirror = true;
				}
				curvePoint.P2.X = curvePoint.P4.X / curvePoint.P4.W;
				curvePoint.P2.Y = curvePoint.P4.Y / curvePoint.P4.W;
				_curvePoints.Add(curvePoint);
			}
		}
		else if (_nurbsCurve.IsClosed)
		{
			for (int j = 0; j < _nurbsCurve.CVCount - 1; j++)
			{
				CurvePoint curvePoint2 = new CurvePoint();
				Point4D point2 = curvePoint2.P4;
				_nurbsCurve.GetCV(j, ref point2);
				curvePoint2.NcIndex = j;
				if (j == 0)
				{
					curvePoint2.NcIndex2 = _nurbsCurve.CVCount - 1;
				}
				int num4 = _nurbsCurve.CVCount - 1;
				int num5 = (num4 + 1) / 2;
				if (j <= num5)
				{
					curvePoint2.MirrorPoint = (num4 - j) % num4;
				}
				if (j == curvePoint2.MirrorPoint)
				{
					curvePoint2.ClampXWhenMirror = true;
				}
				curvePoint2.P2.X = curvePoint2.P4.X / curvePoint2.P4.W;
				curvePoint2.P2.Y = curvePoint2.P4.Y / curvePoint2.P4.W;
				_curvePoints.Add(curvePoint2);
			}
		}
		else
		{
			for (int k = 0; k < _nurbsCurve.CVCount; k++)
			{
				CurvePoint curvePoint3 = new CurvePoint();
				Point4D point3 = curvePoint3.P4;
				_nurbsCurve.GetCV(k, ref point3);
				int cVCount = _nurbsCurve.CVCount;
				int num6 = (cVCount + 1) / 2;
				if (k < num6)
				{
					curvePoint3.MirrorPoint = (cVCount - k) % cVCount;
				}
				if (k == curvePoint3.MirrorPoint)
				{
					curvePoint3.ClampXWhenMirror = true;
				}
				curvePoint3.NcIndex = k;
				curvePoint3.P2.X = curvePoint3.P4.X / curvePoint3.P4.W;
				curvePoint3.P2.Y = curvePoint3.P4.Y / curvePoint3.P4.W;
				_curvePoints.Add(curvePoint3);
			}
		}
		_closed = false;
		if (_nurbsCurve.IsClosed)
		{
			_closed = true;
		}
		CheckMirror(ref _mirror, ref _mirrorpoints);
		if (_mirror)
		{
			chkMirror.Checked = true;
			chkMirror.Enabled = true;
		}
		else
		{
			chkMirror.Checked = false;
			chkMirror.Enabled = false;
		}
		if (_mirrorpoints)
		{
			chkMirrorPoints.Checked = true;
			chkMirrorPoints.Enabled = true;
		}
		else
		{
			chkMirrorPoints.Checked = false;
			chkMirrorPoints.Enabled = false;
		}
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(GV.CounterSketch.Studio.LegacyUIControl.CurveEditorForm));
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.chkMirror = new System.Windows.Forms.CheckBox();
		this.chkMirrorPoints = new System.Windows.Forms.CheckBox();
		this.btnRebuild = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.chkAntiAlias = new System.Windows.Forms.CheckBox();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.pictureBox1.BackColor = System.Drawing.Color.White;
		componentResourceManager.ApplyResources(this.pictureBox1, "pictureBox1");
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.TabStop = false;
		this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseDown);
		this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseMove);
		this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseUp);
		componentResourceManager.ApplyResources(this.chkMirror, "chkMirror");
		this.chkMirror.BackColor = System.Drawing.Color.White;
		this.chkMirror.ForeColor = System.Drawing.Color.Black;
		this.chkMirror.Name = "chkMirror";
		this.chkMirror.UseVisualStyleBackColor = false;
		this.chkMirror.CheckedChanged += new System.EventHandler(chkMirror_CheckedChanged);
		componentResourceManager.ApplyResources(this.chkMirrorPoints, "chkMirrorPoints");
		this.chkMirrorPoints.BackColor = System.Drawing.Color.White;
		this.chkMirrorPoints.ForeColor = System.Drawing.Color.Black;
		this.chkMirrorPoints.Name = "chkMirrorPoints";
		this.chkMirrorPoints.UseVisualStyleBackColor = false;
		this.chkMirrorPoints.CheckedChanged += new System.EventHandler(chkMirrorPoints_CheckedChanged);
		componentResourceManager.ApplyResources(this.btnRebuild, "btnRebuild");
		this.btnRebuild.BackColor = System.Drawing.Color.LightGray;
		this.btnRebuild.Name = "btnRebuild";
		this.btnRebuild.UseVisualStyleBackColor = false;
		this.btnRebuild.Click += new System.EventHandler(rebuild_Click);
		componentResourceManager.ApplyResources(this.label1, "label1");
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Name = "label1";
		componentResourceManager.ApplyResources(this.chkAntiAlias, "chkAntiAlias");
		this.chkAntiAlias.BackColor = System.Drawing.Color.White;
		this.chkAntiAlias.ForeColor = System.Drawing.Color.Black;
		this.chkAntiAlias.Name = "chkAntiAlias";
		this.chkAntiAlias.UseVisualStyleBackColor = false;
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.Black;
		base.Controls.Add(this.chkAntiAlias);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.btnRebuild);
		base.Controls.Add(this.chkMirrorPoints);
		base.Controls.Add(this.chkMirror);
		base.Controls.Add(this.pictureBox1);
		this.DoubleBuffered = true;
		base.Name = "CurveEditorForm";
		base.Load += new System.EventHandler(Form1_Load);
		base.Resize += new System.EventHandler(Form1_Resize);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
