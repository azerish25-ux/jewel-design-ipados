using System;
using System.Drawing;
using CS.CSObjects;
using CS.CSValues.CSValueHandle.HandleSupportClasses;
using CS.CSValues.CSValueHandle.Interfaces;
using CS.CSValues.Interfaces;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.UI;

namespace CS.CSValues.CSValueHandle.Obsolete;

[Obsolete]
[CSBlock("ValueHandle", "{D479F134-AA11-468F-85D2-96FF7A9D4851}")]
public sealed class CSValueHandle : CSValueBase, ICSValue<double>, ICSVHandle
{
	private const double _handleSizePixels = 40.0;

	private const double _selectionSizePixels = 5.0;

	private const double _valueScale = 1.0;

	private const double _valueShiftStep = 0.5;

	private const double _valueStep = 0.01;

	private readonly Color _lineColor = Color.FromArgb(255, 0, 0, 255);

	private readonly Color _selectedColor = Color.FromArgb(255, 0, 0, 255);

	private Point3d _basePoint;

	private double _doubleValue;

	private bool _hasCreatedUndoTransaction;

	private Plane _hitPlane;

	private double _lastMousePointU;

	private double _localValue;

	private Mesh _mesh;

	private Vector3d _movementDirection;

	private Point3d _movePoint;

	private Vector3d _upDirection;

	public CSInputChannel InXform { get; }

	public CSOutputChannel OutValue { get; }

	public double Value
	{
		get
		{
			return _doubleValue;
		}
		set
		{
			SetValue(value);
		}
	}

	public bool MouseOver { get; set; }

	public bool MouseDown { get; set; }

	public CSValueHandle(string identifier)
		: base(identifier)
	{
		InXform = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual, ChannelFlag.NotLoopChecked);
		OutValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public CSValueHandle(string identifier, double value)
		: base(identifier)
	{
		_doubleValue = value;
		InXform = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual, ChannelFlag.NotLoopChecked);
		OutValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public bool SetValue(double newValue)
	{
		if (newValue != _doubleValue)
		{
			PreChange("SetValue");
			_doubleValue = newValue;
			Change();
		}
		return true;
	}

	public void DrawBackGround(DisplayPipeline displayPipeline)
	{
	}

	public void DrawHandle(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		_basePoint = Point3d.Origin;
		_movePoint = new Point3d(_doubleValue, 0.0, 0.0);
		_movementDirection = Vector3d.XAxis;
		_upDirection = Vector3d.ZAxis;
		InXform.GetValue(out Transform value);
		if (((Transform)(ref value)).IsValid)
		{
			((Point3d)(ref _basePoint)).Transform(value);
			((Point3d)(ref _movePoint)).Transform(value);
			((Vector3d)(ref _movementDirection)).Transform(value);
			((Vector3d)(ref _upDirection)).Transform(value);
			((Vector3d)(ref _movementDirection)).Unitize();
			((Vector3d)(ref _upDirection)).Unitize();
		}
		ConduitDrawWires(displayPipeline);
		ConduitDrawShaded(displayPipeline);
	}

	public void DrawOverLay(DisplayPipeline displayPipeline)
	{
	}

	public bool MouseMoveHitTest(MouseCallbackEventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		RhinoViewport activeViewport = e.View.ActiveViewport;
		double num = default(double);
		activeViewport.GetWorldToScreenScale(_movePoint, ref num);
		Vector3d val = _upDirection / num;
		Point3d val2 = _movePoint + val * 40.0;
		Line val3 = default(Line);
		activeViewport.GetFrustumLine((double)e.ViewportPoint.X, (double)e.ViewportPoint.Y, ref val3);
		Vector3d direction = ((Line)(ref val3)).Direction;
		((Vector3d)(ref direction)).Unitize();
		double num2 = Math.Abs(_movementDirection * direction);
		double num3 = 0.9;
		if (num2 > num3)
		{
			return false;
		}
		Point3d val4 = ((Line)(ref val3)).ClosestPoint(val2, false);
		MouseOver = ((Point3d)(ref val4)).DistanceTo(val2) < 5.0 / num;
		if (MouseOver)
		{
			Vector3d direction2 = ((Line)(ref val3)).Direction;
			((Vector3d)(ref direction2)).Unitize();
			if (Math.Abs(direction2 * _upDirection) < 0.5)
			{
				_hitPlane = new Plane(val2, _movementDirection, _upDirection);
			}
			else
			{
				Vector3d val5 = Vector3d.CrossProduct(_upDirection, _movementDirection);
				_hitPlane = new Plane(val2, _movementDirection, val5);
			}
			_localValue = Value;
		}
		_lastMousePointU = 0.0;
		return MouseOver;
	}

	public bool MouseMove(MouseCallbackEventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		Line val = default(Line);
		e.View.ActiveViewport.GetFrustumLine((double)e.ViewportPoint.X, (double)e.ViewportPoint.Y, ref val);
		double num = default(double);
		if (Intersection.LinePlane(val, _hitPlane, ref num))
		{
			Point3d val2 = ((Line)(ref val)).PointAt(num);
			double num2 = default(double);
			double num3 = default(double);
			((Plane)(ref _hitPlane)).ClosestParameter(val2, ref num2, ref num3);
			double num4 = num2 - _lastMousePointU;
			_localValue += num4 * 1.0;
			_lastMousePointU = num2;
			double value = ((!CSValueHandleHelper.MouseButtonShift(e)) ? (Math.Round(_localValue / 0.01) * 0.01) : (Math.Round(_localValue / 0.5) * 0.5));
			SetValueFromUI(value, mouseDown: true);
			return true;
		}
		return false;
	}

	public void MouseUp(MouseCallbackEventArgs e)
	{
		SetValueFromUI(_doubleValue, mouseDown: false);
	}

	public void SetLinked(CSValueHandleHelper.WSelected type, double value, Transform xTrans)
	{
	}

	public bool SelectionChanged()
	{
		return false;
	}

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		OutValue.SetData(new CSNumberData(outputChannelIndex));
		return OutValue;
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		return new CSNumberData(_doubleValue);
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		if (base.ReadJson(jBlock, graph, major, minor))
		{
			return jBlock.ReadJsonValue<double>("Value", out _doubleValue);
		}
		return false;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		archive.ReadDouble(ref _doubleValue);
		return true;
	}

	public override bool SetFileValue(CSObject setObject)
	{
		base.SetFileValue(setObject);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (GetType().IsInstanceOfType(setObject))
		{
			_doubleValue = ((CSValueHandle)setObject)._doubleValue;
		}
		return true;
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (!GetType().IsInstanceOfType(setObject))
		{
			return true;
		}
		string identifier = setObject.Identifier;
		if (identifier != null && identifier.Length > 0)
		{
			SetIdentifier(setObject.Identifier);
		}
		_doubleValue = ((CSValueHandle)setObject)._doubleValue;
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (GetType().IsInstanceOfType(setObject))
		{
			_doubleValue = ((CSValueHandle)setObject)._doubleValue;
		}
		return true;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("Value", _doubleValue);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.WriteDouble(_doubleValue);
		return true;
	}

	private void BuildArrow()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		Mesh val = Mesh.CreateFromCone(new Cone(new Plane(new Point3d(-3.0, 0.0, 0.0), Vector3d.YAxis, Vector3d.ZAxis), 2.5, 1.0), 1, 36);
		try
		{
			if (val != null)
			{
				_mesh = val.DuplicateMesh();
				Transform val2 = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
				((GeometryBase)val).Transform(val2);
				val.Flip(true, true, true);
				_mesh.Append(val);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ConduitDrawShaded(DisplayPipeline dp)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		double num = default(double);
		dp.Viewport.GetWorldToScreenScale(_movePoint, ref num);
		Vector3d val = _upDirection / num;
		Point3d location = _movePoint + val * 40.0;
		DrawArrow(dp, _movementDirection, location, num, _upDirection);
	}

	private void ConduitDrawWires(DisplayPipeline dp)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		Color lineColor = _lineColor;
		double num = default(double);
		dp.Viewport.GetWorldToScreenScale(_movePoint, ref num);
		Vector3d val = _movementDirection / num;
		Vector3d val2 = _upDirection / num;
		Point3d val3 = _movePoint + val2 * 5.0;
		Point3d val4 = _movePoint + val2 * 40.0;
		Point3d val5 = val4 + val * 10.0;
		Point3d val6 = val4 - val * 10.0;
		Point3d val7 = val4;
		dp.DrawLine(val3, val4, lineColor);
		dp.DrawLine(val5, val6, lineColor);
		dp.DrawPoint(val4, (PointStyle)1, 2, lineColor);
		if (MouseOver)
		{
			CSValueHandleHelper.LineInt(out var outpoint, _basePoint, _upDirection, _movePoint, _movementDirection);
			lineColor = _selectedColor;
			dp.DrawLine(_movePoint + val2 * 40.0 / 2.0, outpoint + val2 * 40.0 / 2.0, lineColor);
			dp.DrawLine(_basePoint + val2 * 5.0, outpoint + val2 * 40.0 / 2.0, lineColor);
			dp.DrawPoint(val4, (PointStyle)1, 2, lineColor);
			string text = string.Format(base.Identifier + " {0:0.00} mm", _doubleValue);
			Point3d val8 = dp.Viewport.GetTransform((CoordinateSystem)0, (CoordinateSystem)3) * val7;
			val8 += new Vector3d(4.0, -4.0, 0.0);
			dp.Draw2dText(text, OnColor.op_Implicit(RhUtil.RhinoApp().AppSettings().TrackingColor()), val8, false, 14, "System");
		}
	}

	private void DrawArrow(DisplayPipeline dp, Vector3d direction, Point3d location, double pixelPerUnit, Vector3d snapVector)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		if (_mesh == null)
		{
			BuildArrow();
		}
		Vector3d val = default(Vector3d);
		if (((Vector3d)(ref snapVector)).IsValid)
		{
			val = snapVector;
		}
		else if (Math.Abs(direction * Vector3d.ZAxis) < 0.6)
		{
			val = Vector3d.CrossProduct(direction, Vector3d.ZAxis);
		}
		Vector3d val2 = Vector3d.CrossProduct(val, direction);
		val2 *= 7.0;
		Vector3d val3 = direction * 7.0;
		val3 /= pixelPerUnit;
		val2 /= pixelPerUnit;
		Color diffuse = Color.FromArgb((!MouseOver) ? 255 : 240, 0, 0);
		DisplayMaterial val4 = new DisplayMaterial
		{
			Diffuse = diffuse,
			Emission = Color.FromArgb(0, 0, 0),
			Ambient = Color.FromArgb(40, 40, 40)
		};
		Transform val5 = CSValueHandleHelper.ChangeBasis(location, val3, val2, val2);
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * val5;
		dp.DrawMeshShaded(_mesh, val4);
		dp.ModelTransform = modelTransform;
	}

	private void SetValueFromUI(double value, bool mouseDown)
	{
		if (mouseDown && Value == value)
		{
			return;
		}
		try
		{
			CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
			if (!_hasCreatedUndoTransaction)
			{
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetValueFromUI");
				PreChange("SetValueFromUI");
				_hasCreatedUndoTransaction = true;
			}
			if (mouseDown)
			{
				CSUtil.ToggleMouseDown(this);
				SetValue(value);
			}
			else
			{
				CSUtil.RemoveMouseDown();
				SetValue(value);
				CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetValueFromUI");
				_hasCreatedUndoTransaction = false;
			}
			CSApplicationHelper.Application.Document.Graph.Generate(mouseDown);
			CSUtil.Redraw();
			if (!mouseDown)
			{
				CSUtil.RedrawUI();
			}
		}
		catch (Exception ex)
		{
			CSUtil.Print(ex.Message + "\n");
			CSUtil.Print(ex.StackTrace + "\n");
		}
	}
}
