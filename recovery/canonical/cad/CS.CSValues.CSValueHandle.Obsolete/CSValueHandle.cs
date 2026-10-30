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
		_basePoint = Point3d.Origin;
		_movePoint = new Point3d(_doubleValue, 0.0, 0.0);
		_movementDirection = Vector3d.XAxis;
		_upDirection = Vector3d.ZAxis;
		InXform.GetValue(out Transform value);
		if (value.IsValid)
		{
			_basePoint.Transform(value);
			_movePoint.Transform(value);
			_movementDirection.Transform(value);
			_upDirection.Transform(value);
			_movementDirection.Unitize();
			_upDirection.Unitize();
		}
		ConduitDrawWires(displayPipeline);
		ConduitDrawShaded(displayPipeline);
	}

	public void DrawOverLay(DisplayPipeline displayPipeline)
	{
	}

	public bool MouseMoveHitTest(MouseCallbackEventArgs e)
	{
		RhinoViewport activeViewport = e.View.ActiveViewport;
		activeViewport.GetWorldToScreenScale(_movePoint, out var pixelsPerUnit);
		Vector3d vector3d = _upDirection / pixelsPerUnit;
		Point3d point3d = _movePoint + vector3d * 40.0;
		activeViewport.GetFrustumLine(e.ViewportPoint.X, e.ViewportPoint.Y, out var worldLine);
		Vector3d direction = worldLine.Direction;
		direction.Unitize();
		double num = Math.Abs(_movementDirection * direction);
		double num2 = 0.9;
		if (num > num2)
		{
			return false;
		}
		MouseOver = worldLine.ClosestPoint(point3d, limitToFiniteSegment: false).DistanceTo(point3d) < 5.0 / pixelsPerUnit;
		if (MouseOver)
		{
			Vector3d direction2 = worldLine.Direction;
			direction2.Unitize();
			if (Math.Abs(direction2 * _upDirection) < 0.5)
			{
				_hitPlane = new Plane(point3d, _movementDirection, _upDirection);
			}
			else
			{
				Vector3d yDirection = Vector3d.CrossProduct(_upDirection, _movementDirection);
				_hitPlane = new Plane(point3d, _movementDirection, yDirection);
			}
			_localValue = Value;
		}
		_lastMousePointU = 0.0;
		return MouseOver;
	}

	public bool MouseMove(MouseCallbackEventArgs e)
	{
		e.View.ActiveViewport.GetFrustumLine(e.ViewportPoint.X, e.ViewportPoint.Y, out var worldLine);
		if (Intersection.LinePlane(worldLine, _hitPlane, out var lineParameter))
		{
			Point3d testPoint = worldLine.PointAt(lineParameter);
			_hitPlane.ClosestParameter(testPoint, out var s, out var _);
			double num = s - _lastMousePointU;
			_localValue += num * 1.0;
			_lastMousePointU = s;
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
		using Mesh mesh = Mesh.CreateFromCone(new Cone(new Plane(new Point3d(-3.0, 0.0, 0.0), Vector3d.YAxis, Vector3d.ZAxis), 2.5, 1.0), 1, 36);
		if (mesh != null)
		{
			_mesh = mesh.DuplicateMesh();
			Transform xform = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
			mesh.Transform(xform);
			mesh.Flip(vertexNormals: true, faceNormals: true, faceOrientation: true);
			_mesh.Append(mesh);
		}
	}

	private void ConduitDrawShaded(DisplayPipeline dp)
	{
		dp.Viewport.GetWorldToScreenScale(_movePoint, out var pixelsPerUnit);
		Vector3d vector3d = _upDirection / pixelsPerUnit;
		Point3d location = _movePoint + vector3d * 40.0;
		DrawArrow(dp, _movementDirection, location, pixelsPerUnit, _upDirection);
	}

	private void ConduitDrawWires(DisplayPipeline dp)
	{
		Color lineColor = _lineColor;
		dp.Viewport.GetWorldToScreenScale(_movePoint, out var pixelsPerUnit);
		Vector3d vector3d = _movementDirection / pixelsPerUnit;
		Vector3d vector3d2 = _upDirection / pixelsPerUnit;
		Point3d point3d = _movePoint + vector3d2 * 5.0;
		Point3d point3d2 = _movePoint + vector3d2 * 40.0;
		Point3d point3d3 = point3d2 + vector3d * 10.0;
		Point3d to = point3d2 - vector3d * 10.0;
		Point3d point3d4 = point3d2;
		dp.DrawLine(point3d, point3d2, lineColor);
		dp.DrawLine(point3d3, to, lineColor);
		dp.DrawPoint(point3d2, Rhino.Display.PointStyle.ControlPoint, 2, lineColor);
		if (MouseOver)
		{
			CSValueHandleHelper.LineInt(out var outpoint, _basePoint, _upDirection, _movePoint, _movementDirection);
			lineColor = _selectedColor;
			dp.DrawLine(_movePoint + vector3d2 * 40.0 / 2.0, outpoint + vector3d2 * 40.0 / 2.0, lineColor);
			dp.DrawLine(_basePoint + vector3d2 * 5.0, outpoint + vector3d2 * 40.0 / 2.0, lineColor);
			dp.DrawPoint(point3d2, Rhino.Display.PointStyle.ControlPoint, 2, lineColor);
			string text = string.Format(base.Identifier + " {0:0.00} mm", _doubleValue);
			Point3d worldCoordinate = dp.Viewport.GetTransform(CoordinateSystem.World, CoordinateSystem.Screen) * point3d4;
			worldCoordinate += new Vector3d(4.0, -4.0, 0.0);
			dp.Draw2dText(text, RhUtil.RhinoApp().AppSettings().TrackingColor(), worldCoordinate, middleJustified: false, 14, "System");
		}
	}

	private void DrawArrow(DisplayPipeline dp, Vector3d direction, Point3d location, double pixelPerUnit, Vector3d snapVector)
	{
		if (_mesh == null)
		{
			BuildArrow();
		}
		Vector3d a = default(Vector3d);
		if (snapVector.IsValid)
		{
			a = snapVector;
		}
		else if (Math.Abs(direction * Vector3d.ZAxis) < 0.6)
		{
			a = Vector3d.CrossProduct(direction, Vector3d.ZAxis);
		}
		Vector3d vector3d = Vector3d.CrossProduct(a, direction);
		vector3d *= 7.0;
		Vector3d initialBaseX = direction * 7.0;
		initialBaseX /= pixelPerUnit;
		vector3d /= pixelPerUnit;
		Color diffuse = Color.FromArgb((!MouseOver) ? 255 : 240, 0, 0);
		DisplayMaterial material = new DisplayMaterial
		{
			Diffuse = diffuse,
			Emission = Color.FromArgb(0, 0, 0),
			Ambient = Color.FromArgb(40, 40, 40)
		};
		Transform transform = CSValueHandleHelper.ChangeBasis(location, initialBaseX, vector3d, vector3d);
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * transform;
		dp.DrawMeshShaded(_mesh, material);
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
