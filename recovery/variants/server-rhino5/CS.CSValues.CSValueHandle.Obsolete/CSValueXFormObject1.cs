using System;
using System.Collections.Generic;
using System.Drawing;
using CS.CSObjects;
using CS.CSValues.CSValueHandle.HandleSupportClasses;
using CS.CSValues.CSValueHandle.Interfaces;
using CS.CSValues.CSValueHandle.Obsolete.HandleSupportClasses;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino.Display;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.UI;

namespace CS.CSValues.CSValueHandle.Obsolete;

[Obsolete]
[CSBlock("ValueXFormObject1", "{76856B86-ABEB-4156-AB1A-7319B6B86C45}")]
public sealed class CSValueXFormObject1 : CSValueXFormBase, ICSVHandle
{
	private enum WSelected
	{
		PNone,
		Mx,
		My,
		Mz,
		Rx,
		Ry,
		Rz,
		Sx,
		Sy,
		Sz,
		Ss,
		PCenter,
		Dp0,
		Dp1,
		Dp2
	}

	private const double _handleLengthExtraPixels = 60.0;

	private const double _handleLengthPixels = 140.0;

	private const double _selectionSizePixels = 10.0;

	private Arc _arc;

	private BoundingBox _bb;

	private bool _bendMode;

	private bool _hasCreatedUndoTransaction;

	private Plane _hitPlane;

	private Mesh _meshArrow;

	private Mesh _meshCube;

	private Mesh _meshRotate;

	private Mesh _meshSphere;

	private CSData _obj;

	private CSData _objOriginal;

	private Point3d _p0 = new Point3d(0.0, 0.0, -1.0);

	private Point3d _p0A = new Point3d(0.0, 0.0, 0.0);

	private Point3d _p1 = new Point3d(0.0, 10.0, -1.0);

	private Point3d _p1A = new Point3d(0.0, 0.0, 0.0);

	private Point3d _p2 = new Point3d(0.0, 9.0, -1.0);

	private Point3d _p2A = new Point3d(0.0, 0.0, 0.0);

	private Plane _pl0;

	private Plane _pl1;

	private Plane _pl2;

	private double _scaleHandleLengthS;

	private double _scaleHandleLengthX;

	private double _scaleHandleLengthY;

	private double _scaleHandleLengthZ;

	private WSelected _whichSelected;

	public CSInputChannel InObjects { get; }

	public CSInputChannel InOnOff { get; }

	public CSOutputChannel OutTransformedObjects { get; }

	public CSOutputChannel OutValue { get; }

	private bool IsOn
	{
		get
		{
			bool value = true;
			InOnOff.GetBoolean(ref value);
			return value;
		}
	}

	private Vector3d UDirX
	{
		get
		{
			Vector3d result = new Vector3d(_value[0, 0], _value[1, 0], _value[2, 0]);
			result.Unitize();
			return result;
		}
	}

	private Vector3d UDirY
	{
		get
		{
			Vector3d result = new Vector3d(_value[0, 1], _value[1, 1], _value[2, 1]);
			result.Unitize();
			return result;
		}
	}

	private Vector3d UDirZ
	{
		get
		{
			Vector3d result = new Vector3d(_value[0, 2], _value[1, 2], _value[2, 2]);
			result.Unitize();
			return result;
		}
	}

	private Vector3d UDirS
	{
		get
		{
			Point3d point3d = new Point3d(_bb.Max);
			point3d.Transform(_value);
			Vector3d result = new Vector3d(point3d - BasePoint);
			result.Unitize();
			return result;
		}
	}

	private Point3d BasePoint => new Point3d(_value[0, 3], _value[1, 3], _value[2, 3]);

	private bool ShowMove
	{
		get
		{
			return base.UserData.FindBool("ValueXForm", "ShowMove", defaultValue: true);
		}
		set
		{
			base.UserData.AddKey("ValueXForm", "ShowMove", value.ToString());
		}
	}

	private bool ShowRotate
	{
		get
		{
			return base.UserData.FindBool("ValueXForm", "ShowRotate", defaultValue: true);
		}
		set
		{
			base.UserData.AddKey("ValueXForm", "ShowRotate", value.ToString());
		}
	}

	private bool ShowScale
	{
		get
		{
			return base.UserData.FindBool("ValueXForm", "ShowScale", defaultValue: true);
		}
		set
		{
			base.UserData.AddKey("ValueXForm", "ShowScale", value.ToString());
		}
	}

	public bool MouseOver { get; set; }

	public bool MouseDown { get; set; }

	public CSValueXFormObject1(string identifier)
		: base(identifier)
	{
		InObjects = new CSInputChannel(GetPropertyName(() => InObjects), this, ChannelDataType.Any, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InOnOff = new CSInputChannel(GetPropertyName(() => InOnOff), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutTransformedObjects = new CSOutputChannel(GetPropertyName(() => OutTransformedObjects), this, ChannelDataType.Any, ChannelArrayType.Any);
		OutValue = new CSOutputChannel(GetPropertyName(() => OutValue), this, ChannelDataType.Transform, ChannelArrayType.Individual);
	}

	public CSValueXFormObject1(string identifier, Transform value)
		: base(identifier, value)
	{
		InObjects = new CSInputChannel(GetPropertyName(() => InObjects), this, ChannelDataType.Any, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InOnOff = new CSInputChannel(GetPropertyName(() => InOnOff), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutTransformedObjects = new CSOutputChannel(GetPropertyName(() => OutTransformedObjects), this, ChannelDataType.Any, ChannelArrayType.Any);
		OutValue = new CSOutputChannel(GetPropertyName(() => OutValue), this, ChannelDataType.Transform, ChannelArrayType.Individual);
	}

	public void DrawBackGround(DisplayPipeline displayPipeline)
	{
		if (IsOn && _bendMode)
		{
			DrawBendBackGroundGrid(displayPipeline);
		}
	}

	public void DrawHandle(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions)
	{
		if (!IsOn)
		{
			return;
		}
		double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(displayPipeline, BasePoint);
		if (_bendMode)
		{
			DrawBendControlPoints(displayPipeline);
			Point3d basePoint = BasePoint;
			Color pointColor = ((MouseOver && _whichSelected == WSelected.PCenter) ? Color.FromArgb(255, 192, 255, 255) : Color.FromArgb(255, 0, 255, 255));
			DrawModePicker(displayPipeline, basePoint, worldToScreenScale, pointColor);
		}
		else if (CSValueHandleHelper.CurrentHandle == this)
		{
			DrawBoundingBox(displayPipeline);
			if (ShowScale)
			{
				Point3d location = BasePoint + UDirX * _scaleHandleLengthX;
				Point3d location2 = BasePoint + UDirY * _scaleHandleLengthY;
				Point3d location3 = BasePoint + UDirZ * _scaleHandleLengthZ;
				Point3d location4 = BasePoint + UDirS * _scaleHandleLengthS;
				DrawArrow(displayPipeline, UDirX, BasePoint, location, worldToScreenScale, UDirY, Color.FromArgb(255, 255, 0, 0), WSelected.Sx, CSValueHandleHelper.WMode.Scale);
				DrawArrow(displayPipeline, UDirY, BasePoint, location2, worldToScreenScale, UDirZ, Color.FromArgb(255, 0, 255, 0), WSelected.Sy, CSValueHandleHelper.WMode.Scale);
				DrawArrow(displayPipeline, UDirZ, BasePoint, location3, worldToScreenScale, UDirX, Color.FromArgb(255, 0, 0, 255), WSelected.Sz, CSValueHandleHelper.WMode.Scale);
				DrawArrow(displayPipeline, UDirS, BasePoint, location4, worldToScreenScale, UDirZ, Color.FromArgb(255, 128, 128, 128), WSelected.Ss, CSValueHandleHelper.WMode.Scale);
			}
			if (ShowRotate)
			{
				Point3d location5 = BasePoint - UDirX * (_scaleHandleLengthX + 60.0 / worldToScreenScale);
				Point3d location6 = BasePoint - UDirY * (_scaleHandleLengthY + 60.0 / worldToScreenScale);
				Point3d location7 = BasePoint - UDirZ * (_scaleHandleLengthZ + 60.0 / worldToScreenScale);
				DrawArrow(displayPipeline, -UDirX, BasePoint, location5, worldToScreenScale, UDirY, Color.FromArgb(255, 255, 0, 0), WSelected.Rx, CSValueHandleHelper.WMode.Rotate);
				DrawArrow(displayPipeline, -UDirY, BasePoint, location6, worldToScreenScale, UDirZ, Color.FromArgb(255, 0, 255, 0), WSelected.Ry, CSValueHandleHelper.WMode.Rotate);
				DrawArrow(displayPipeline, -UDirZ, BasePoint, location7, worldToScreenScale, UDirX, Color.FromArgb(255, 0, 0, 255), WSelected.Rz, CSValueHandleHelper.WMode.Rotate);
			}
			if (ShowMove)
			{
				Point3d location8 = BasePoint + UDirX * (_scaleHandleLengthX + 60.0 / worldToScreenScale);
				Point3d location9 = BasePoint + UDirY * (_scaleHandleLengthY + 60.0 / worldToScreenScale);
				Point3d location10 = BasePoint + UDirZ * (_scaleHandleLengthZ + 60.0 / worldToScreenScale);
				DrawArrow(displayPipeline, UDirX, BasePoint, location8, worldToScreenScale, UDirY, Color.FromArgb(255, 255, 0, 0), WSelected.Mx, CSValueHandleHelper.WMode.Move);
				DrawArrow(displayPipeline, UDirY, BasePoint, location9, worldToScreenScale, UDirZ, Color.FromArgb(255, 0, 255, 0), WSelected.My, CSValueHandleHelper.WMode.Move);
				DrawArrow(displayPipeline, UDirZ, BasePoint, location10, worldToScreenScale, UDirX, Color.FromArgb(255, 0, 0, 255), WSelected.Mz, CSValueHandleHelper.WMode.Move);
			}
			Point3d basePoint2 = BasePoint;
			Color pointColor2 = ((MouseOver && _whichSelected == WSelected.PCenter) ? Color.FromArgb(255, 192, 255, 255) : Color.FromArgb(255, 0, 255, 255));
			DrawModePicker(displayPipeline, basePoint2, worldToScreenScale, pointColor2);
		}
		else
		{
			Point3d basePoint3 = BasePoint;
			Color pointColor3 = ((MouseOver && _whichSelected == WSelected.PCenter) ? Color.FromArgb(255, 128, 128, 255) : Color.FromArgb(255, 255, 255, 0));
			DrawModePicker(displayPipeline, basePoint3, worldToScreenScale, pointColor3);
		}
	}

	public void DrawOverLay(DisplayPipeline displayPipeline)
	{
	}

	public bool MouseMoveHitTest(MouseCallbackEventArgs e)
	{
		if (!IsOn)
		{
			return false;
		}
		RhinoViewport activeViewport = e.View.ActiveViewport;
		Line frustrumLine = CSValueHandleHelper.GetFrustrumLine(activeViewport, e.ViewportPoint);
		Vector3d direction = frustrumLine.Direction;
		direction.Unitize();
		if (_bendMode)
		{
			Point3d bp = new Point3d(_p0);
			Point3d bp2 = new Point3d(_p1);
			Point3d bp3 = new Point3d(_p2);
			bp.Transform(_value);
			bp2.Transform(_value);
			bp3.Transform(_value);
			if (TestHitSphere(frustrumLine, activeViewport, bp, WSelected.Dp0))
			{
				return true;
			}
			if (TestHitSphere(frustrumLine, activeViewport, bp2, WSelected.Dp1))
			{
				return true;
			}
			if (TestHitSphere(frustrumLine, activeViewport, bp3, WSelected.Dp2))
			{
				return true;
			}
			return TestHitSphere(frustrumLine, activeViewport, BasePoint, WSelected.PCenter);
		}
		if (CSValueHandleHelper.CurrentHandle == this)
		{
			if (ShowMove)
			{
				if (TestHitPoint(frustrumLine, direction, activeViewport, WSelected.Mx, BasePoint, UDirX, Vector3d.Unset, CSValueHandleHelper.WMode.Move))
				{
					return true;
				}
				if (TestHitPoint(frustrumLine, direction, activeViewport, WSelected.My, BasePoint, UDirY, Vector3d.Unset, CSValueHandleHelper.WMode.Move))
				{
					return true;
				}
				if (TestHitPoint(frustrumLine, direction, activeViewport, WSelected.Mz, BasePoint, UDirZ, Vector3d.Unset, CSValueHandleHelper.WMode.Move))
				{
					return true;
				}
			}
			if (ShowRotate)
			{
				if (TestHitPoint(frustrumLine, direction, activeViewport, WSelected.Rx, BasePoint, -UDirX, UDirY, CSValueHandleHelper.WMode.Rotate))
				{
					return true;
				}
				if (TestHitPoint(frustrumLine, direction, activeViewport, WSelected.Ry, BasePoint, -UDirY, UDirZ, CSValueHandleHelper.WMode.Rotate))
				{
					return true;
				}
				if (TestHitPoint(frustrumLine, direction, activeViewport, WSelected.Rz, BasePoint, -UDirZ, UDirX, CSValueHandleHelper.WMode.Rotate))
				{
					return true;
				}
			}
			if (ShowScale)
			{
				if (TestHitPoint(frustrumLine, direction, activeViewport, WSelected.Sx, BasePoint, UDirX, Vector3d.Unset, CSValueHandleHelper.WMode.Scale))
				{
					return true;
				}
				if (TestHitPoint(frustrumLine, direction, activeViewport, WSelected.Sy, BasePoint, UDirY, Vector3d.Unset, CSValueHandleHelper.WMode.Scale))
				{
					return true;
				}
				if (TestHitPoint(frustrumLine, direction, activeViewport, WSelected.Sz, BasePoint, UDirZ, Vector3d.Unset, CSValueHandleHelper.WMode.Scale))
				{
					return true;
				}
				if (TestHitPoint(frustrumLine, direction, activeViewport, WSelected.Ss, BasePoint, UDirS, Vector3d.Unset, CSValueHandleHelper.WMode.Scale))
				{
					return true;
				}
			}
		}
		return TestHitSphere(frustrumLine, activeViewport, BasePoint, WSelected.PCenter);
	}

	public bool MouseMove(MouseCallbackEventArgs e)
	{
		if (!MouseOver)
		{
			return false;
		}
		Line frustrumLine = CSValueHandleHelper.GetFrustrumLine(e.View.ActiveViewport, e.ViewportPoint);
		double lineParameter;
		if (_whichSelected == WSelected.Dp0)
		{
			if (Intersection.LinePlane(frustrumLine, _pl0, out lineParameter))
			{
				Point3d point3d = frustrumLine.PointAt(lineParameter);
				Point3d newP = point3d;
				_value.TryGetInverse(out var inverseTransform);
				newP.Transform(inverseTransform);
				SetValueFromUI(Transform.Unset, newP, Point3d.Unset, Point3d.Unset, mouseDown: true);
				return true;
			}
			return false;
		}
		if (_whichSelected == WSelected.Dp1)
		{
			if (Intersection.LinePlane(frustrumLine, _pl1, out lineParameter))
			{
				Point3d point3d = frustrumLine.PointAt(lineParameter);
				Point3d newP2 = point3d;
				_value.TryGetInverse(out var inverseTransform2);
				newP2.Transform(inverseTransform2);
				SetValueFromUI(Transform.Unset, Point3d.Unset, newP2, Point3d.Unset, mouseDown: true);
				return true;
			}
			return false;
		}
		if (_whichSelected == WSelected.Dp2)
		{
			if (Intersection.LinePlane(frustrumLine, _pl2, out lineParameter))
			{
				Point3d point3d = frustrumLine.PointAt(lineParameter);
				Point3d newP3 = point3d;
				_value.TryGetInverse(out var inverseTransform3);
				newP3.Transform(inverseTransform3);
				SetValueFromUI(Transform.Unset, Point3d.Unset, Point3d.Unset, newP3, mouseDown: true);
				return true;
			}
			return false;
		}
		if (_whichSelected == WSelected.PCenter)
		{
			return false;
		}
		if (Intersection.LinePlane(frustrumLine, _hitPlane, out lineParameter))
		{
			Point3d point3d = frustrumLine.PointAt(lineParameter);
			_hitPlane.ClosestParameter(point3d, out var s, out var t);
			Transform transform = Transform.Identity;
			double angleRadians = CSValueHandleHelper.AngleOfPointD(t, s);
			if (_whichSelected == WSelected.Rx)
			{
				transform = Transform.Rotation(angleRadians, -UDirZ, BasePoint);
			}
			if (_whichSelected == WSelected.Ry)
			{
				transform = Transform.Rotation(angleRadians, -UDirX, BasePoint);
			}
			if (_whichSelected == WSelected.Rz)
			{
				transform = Transform.Rotation(angleRadians, -UDirY, BasePoint);
			}
			double num = s;
			if (_whichSelected == WSelected.Mx)
			{
				transform = Transform.Translation(UDirX * num);
			}
			if (_whichSelected == WSelected.My)
			{
				transform = Transform.Translation(UDirY * num);
			}
			if (_whichSelected == WSelected.Mz)
			{
				transform = Transform.Translation(UDirZ * num);
			}
			_hitPlane.Transform(transform);
			transform *= _value;
			if (num < 0.1)
			{
				num = 0.1;
			}
			if (_whichSelected == WSelected.Sx)
			{
				transform[0, 0] = transform[0, 0] / _scaleHandleLengthX * num;
				transform[1, 0] = transform[1, 0] / _scaleHandleLengthX * num;
				transform[2, 0] = transform[2, 0] / _scaleHandleLengthX * num;
				_scaleHandleLengthX = num;
			}
			if (_whichSelected == WSelected.Sy)
			{
				transform[0, 1] = transform[0, 1] / _scaleHandleLengthY * num;
				transform[1, 1] = transform[1, 1] / _scaleHandleLengthY * num;
				transform[2, 1] = transform[2, 1] / _scaleHandleLengthY * num;
				_scaleHandleLengthY = num;
			}
			if (_whichSelected == WSelected.Sz)
			{
				transform[0, 2] = transform[0, 2] / _scaleHandleLengthZ * num;
				transform[1, 2] = transform[1, 2] / _scaleHandleLengthZ * num;
				transform[2, 2] = transform[2, 2] / _scaleHandleLengthZ * num;
				_scaleHandleLengthZ = num;
			}
			if (_whichSelected == WSelected.Ss)
			{
				double num2 = num / _scaleHandleLengthS;
				transform[0, 0] *= num2;
				transform[1, 0] *= num2;
				transform[2, 0] *= num2;
				transform[0, 1] *= num2;
				transform[1, 1] *= num2;
				transform[2, 1] *= num2;
				transform[0, 2] *= num2;
				transform[1, 2] *= num2;
				transform[2, 2] *= num2;
				_scaleHandleLengthS = num;
			}
			SetValueFromUI(transform, Point3d.Unset, Point3d.Unset, Point3d.Unset, mouseDown: true);
			return false;
		}
		return false;
	}

	public void MouseUp(MouseCallbackEventArgs e)
	{
		if (!MouseOver)
		{
			return;
		}
		if (_whichSelected == WSelected.PCenter)
		{
			if (CSValueHandleHelper.CurrentHandle == this && !_bendMode)
			{
				_bendMode = true;
				CSUtil.Redraw();
			}
			else
			{
				CSValueHandleHelper.CurrentHandle = ((CSValueHandleHelper.CurrentHandle == this) ? null : this);
				_bendMode = false;
				CSUtil.Redraw();
			}
		}
		else
		{
			SetValueFromUI(Transform.Unset, Point3d.Unset, Point3d.Unset, Point3d.Unset, mouseDown: false);
			CSApplicationHelper.Application.Document.Graph.Generate();
			CSUtil.Redraw();
			CSUtil.RedrawUI();
		}
	}

	public void SetLinked(CSValueHandleHelper.WSelected type, double value, Transform xTrans)
	{
	}

	public bool SelectionChanged()
	{
		return false;
	}

	public override void Dispose()
	{
		_meshArrow?.Dispose();
		_meshCube?.Dispose();
		_meshRotate?.Dispose();
		_meshSphere?.Dispose();
		base.Dispose();
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		if (base.ReadJson(jBlock, graph, major, minor))
		{
			return jBlock.GetMatrix("Value", out _value);
		}
		return false;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				double value = 0.0;
				archive.ReadDouble(ref value);
				_value[i, j] = value;
			}
		}
		return true;
	}

	public override bool SetFileValue(CSObject setObject)
	{
		base.SetFileValue(setObject);
		if (!base.NotCreatedYet && GetType().IsInstanceOfType(setObject))
		{
			_value = ((CSValueXFormObject1)setObject)._value;
		}
		return true;
	}

	public void SetLocation(double x, double y, double z)
	{
		_value = Transform.Translation(x, y, z);
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
		_value = ((CSValueXFormObject1)setObject)._value;
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
			_value = ((CSValueXFormObject1)setObject)._value;
		}
		return true;
	}

	public void SetVisible(bool showMove, bool showRotate, bool showScale)
	{
		ShowMove = showMove;
		ShowRotate = showRotate;
		ShowScale = showScale;
	}

	public override bool Update()
	{
		if (BeenUpdatedAfterLastUpdate(InObjects.OutputChannelBlock) | (_obj == null) | ((_p0 - _p0A).Length > 0.001) | ((_p1 - _p1A).Length > 0.001) | ((_p2 - _p2A).Length > 0.001))
		{
			_objOriginal = InObjects.Data.Duplicate();
			_obj = _objOriginal.Duplicate();
			Vector3d inputVector = _p1 - _p0;
			Vector3d inputVector2 = _p2 - _p0;
			if (CSValueHandleHelper.AngleBetweenVectors(inputVector, inputVector2) > Math.PI / 180.0)
			{
				Point3d p = new Point3d(_p0);
				Point3d p2 = new Point3d(_p1);
				Arc arc = _arc;
				CSBendMorph1 spaceMorph = new CSBendMorph1(p, p2, arc)
				{
					Tolerance = 0.05
				};
				_obj.Morph(spaceMorph);
			}
			_obj.GetBoundingBox(ref _bb);
			if (!_bb.IsValid)
			{
				_bb = new BoundingBox(new Point3d(-5.0, -5.0, -2.0), new Point3d(5.0, 5.0, 2.0));
			}
			_p0A = new Point3d(_p0);
			_p1A = new Point3d(_p1);
			_p2A = new Point3d(_p2);
		}
		CSData cSData = _obj.Duplicate();
		(cSData as ICSTransformable)?.ApplyTransform(_value);
		CalculateScaleHandleSizes();
		OutTransformedObjects.SetData(cSData);
		OutValue.SetData(new CSXformData(_value));
		return base.Update();
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("Value", _value);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				archive.WriteDouble(_value[i, j]);
			}
		}
		return true;
	}

	private void BuildArrow()
	{
		Cone cone = new Cone(new Plane(new Point3d(1.3, 0.0, 0.0), Vector3d.YAxis, Vector3d.ZAxis), -2.5, 1.0);
		_meshArrow = Mesh.CreateFromCone(cone, 1, 36);
	}

	private void BuildCube()
	{
		List<Point3d> corners = new List<Point3d>
		{
			new Point3d(-1.0, -1.0, -1.0),
			new Point3d(1.0, -1.0, -1.0),
			new Point3d(1.0, 1.0, -1.0),
			new Point3d(-1.0, 1.0, -1.0),
			new Point3d(-1.0, -1.0, 1.0),
			new Point3d(1.0, -1.0, 1.0),
			new Point3d(1.0, 1.0, 1.0),
			new Point3d(-1.0, 1.0, 1.0)
		};
		_meshCube = Mesh.CreateFromBox(corners, 1, 1, 1);
	}

	private void BuildRotate()
	{
		_meshRotate = new Mesh();
		Mesh mesh = Mesh.CreateFromCone(new Cone(new Plane(new Point3d(0.5, 0.0, 5.0), Vector3d.XAxis, Vector3d.YAxis), -2.5, 1.0), 1, 36);
		Transform xform = Transform.Rotation(-20.0 * CSBlockUtil.CSUtilMath.On_DEGREES_TO_RADIANS, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		mesh.Transform(xform);
		_meshRotate.Append(mesh);
		mesh = Mesh.CreateFromCone(new Cone(new Plane(new Point3d(0.5, 0.0, -5.0), Vector3d.XAxis, Vector3d.YAxis), 2.5, 1.0), 1, 36);
		xform = Transform.Rotation(20.0 * CSBlockUtil.CSUtilMath.On_DEGREES_TO_RADIANS, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		mesh.Transform(xform);
		_meshRotate.Append(mesh);
		mesh = Mesh.CreateFromCylinder(new Cylinder(new Circle(new Plane(new Point3d(0.0, 0.0, -0.6), Vector3d.XAxis, Vector3d.YAxis), 0.5), 1.2), 1, 36);
		xform = Transform.Rotation(-20.0 * CSBlockUtil.CSUtilMath.On_DEGREES_TO_RADIANS, new Vector3d(0.0, 1.0, 0.0), new Point3d(-6.0, 0.0, 0.0));
		mesh.Transform(xform);
		xform = Transform.Rotation(10.0 * CSBlockUtil.CSUtilMath.On_DEGREES_TO_RADIANS, new Vector3d(0.0, 1.0, 0.0), new Point3d(-6.0, 0.0, 0.0));
		_meshRotate.Append(mesh);
		mesh.Transform(xform);
		_meshRotate.Append(mesh);
		mesh.Transform(xform);
		_meshRotate.Append(mesh);
		mesh.Transform(xform);
		_meshRotate.Append(mesh);
		mesh.Transform(xform);
		_meshRotate.Append(mesh);
	}

	private void BuildSphere()
	{
		Sphere sphere = new Sphere(Point3d.Origin, 1.0);
		_meshSphere = Mesh.CreateFromSphere(sphere, 20, 36);
	}

	private void CalculateScaleHandleSizes()
	{
		Point3d point3d = new Point3d(_bb.Max);
		Point3d point3d2 = new Point3d(point3d.X, 0.0, 0.0);
		Point3d point3d3 = new Point3d(0.0, point3d.Y, 0.0);
		Point3d point3d4 = new Point3d(0.0, 0.0, point3d.Z);
		point3d2.Transform(_value);
		point3d3.Transform(_value);
		point3d4.Transform(_value);
		point3d.Transform(_value);
		_scaleHandleLengthX = (point3d2 - BasePoint).Length;
		_scaleHandleLengthY = (point3d3 - BasePoint).Length;
		_scaleHandleLengthZ = (point3d4 - BasePoint).Length;
		_scaleHandleLengthS = (point3d - BasePoint).Length;
	}

	private void DrawArrow(DisplayPipeline dp, Vector3d direction, Point3d baseLocation, Point3d location, double pixelPerUnit, Vector3d upVector, Color lineColor, WSelected ws, CSValueHandleHelper.WMode msrMode)
	{
		if (_meshArrow == null)
		{
			BuildArrow();
		}
		if (_meshRotate == null)
		{
			BuildRotate();
		}
		bool flag = MouseOver && _whichSelected == ws;
		Vector3d a = default(Vector3d);
		if (upVector.IsValid)
		{
			a = upVector;
		}
		else if (Math.Abs(direction * Vector3d.ZAxis) < 0.6)
		{
			a = Vector3d.CrossProduct(direction, Vector3d.ZAxis);
		}
		Vector3d vector3d = Vector3d.CrossProduct(a, direction);
		a = Vector3d.CrossProduct(direction, vector3d);
		a *= 7.0;
		vector3d *= 7.0;
		Vector3d initialBaseX = direction * 7.0;
		initialBaseX /= pixelPerUnit;
		a /= pixelPerUnit;
		vector3d /= pixelPerUnit;
		DisplayMaterial displayMaterial = new DisplayMaterial
		{
			Diffuse = lineColor,
			Transparency = (flag ? 0.2 : 0.9),
			Emission = Color.FromArgb(0, 0, 0),
			Ambient = Color.FromArgb(40, 40, 40)
		};
		Transform transform = CSValueHandleHelper.ChangeBasis(location, initialBaseX, vector3d, a);
		Plane plane = new Plane(baseLocation, location - baseLocation);
		Circle baseCircle = new Circle(plane, 2.0 / pixelPerUnit);
		Mesh mesh = Mesh.CreateFromCylinder(new Cylinder(baseCircle, (location - baseLocation).Length), 1, 36);
		dp.DrawMeshShaded(mesh, displayMaterial);
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * transform;
		displayMaterial.Transparency = (flag ? 0.0 : 0.4);
		if (msrMode == CSValueHandleHelper.WMode.Rotate)
		{
			dp.DrawMeshShaded(_meshRotate, displayMaterial);
		}
		if (msrMode == CSValueHandleHelper.WMode.Move)
		{
			dp.DrawMeshShaded(_meshArrow, displayMaterial);
		}
		if (msrMode == CSValueHandleHelper.WMode.Scale)
		{
			dp.DrawMeshShaded(_meshSphere, displayMaterial);
		}
		dp.ModelTransform = modelTransform;
	}

	private void DrawBendBackGroundGrid(DisplayPipeline dp)
	{
		if (!_bb.IsValid)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(dp);
		cameraDirection.Unitize();
		Point3d center = _bb.Center;
		center.Transform(_value);
		Vector3d vector3d = new Vector3d(1.0, 0.0, 0.0);
		Vector3d vector3d2 = new Vector3d(0.0, 1.0, 0.0);
		Vector3d vector3d3 = new Vector3d(0.0, 0.0, 1.0);
		vector3d.Transform(_value);
		vector3d.Unitize();
		vector3d2.Transform(_value);
		vector3d2.Unitize();
		vector3d3.Transform(_value);
		vector3d3.Unitize();
		double num3 = Math.Abs(cameraDirection * vector3d);
		double num4 = Math.Abs(cameraDirection * vector3d2);
		double num5 = Math.Abs(cameraDirection * vector3d3);
		Vector3d vector3d4 = new Vector3d(1.0, 0.0, 0.0);
		Vector3d vector3d5 = new Vector3d(1.0, 0.0, 0.0);
		double num6 = 0.0;
		if (num3 > num6)
		{
			vector3d4 = vector3d2;
			vector3d5 = vector3d3;
			num6 = num3;
			num = (int)(_bb.Diagonal.Y / 2.0 + 3.0);
			num2 = (int)(_bb.Diagonal.Z / 2.0 + 3.0);
		}
		if (num4 > num6)
		{
			vector3d4 = vector3d;
			vector3d5 = vector3d3;
			num6 = num4;
			num = (int)(_bb.Diagonal.X / 2.0 + 3.0);
			num2 = (int)(_bb.Diagonal.Z / 2.0 + 3.0);
		}
		if (num5 > num6)
		{
			vector3d4 = vector3d;
			vector3d5 = vector3d2;
			num = (int)(_bb.Diagonal.X / 2.0 + 3.0);
			num2 = (int)(_bb.Diagonal.Y / 2.0 + 3.0);
		}
		Point3d origin = new Point3d(_p0);
		Point3d origin2 = new Point3d(_p1);
		Point3d origin3 = new Point3d(_p2);
		origin.Transform(_value);
		origin2.Transform(_value);
		origin3.Transform(_value);
		_pl0 = new Plane(origin, vector3d4, vector3d5);
		_pl1 = new Plane(origin2, vector3d4, vector3d5);
		_pl2 = new Plane(origin3, vector3d4, vector3d5);
		if (_objOriginal is CSOnGeometryData cSOnGeometryData)
		{
			DisplayMaterial displayMaterial = new DisplayMaterial
			{
				Diffuse = Color.FromArgb(255, 64, 128, 128),
				Transparency = 0.8,
				Emission = Color.FromArgb(0, 0, 0),
				Ambient = Color.FromArgb(40, 40, 40)
			};
			Mesh mesh = cSOnGeometryData.GetMesh();
			Transform modelTransform = dp.ModelTransform;
			dp.ModelTransform = modelTransform * _value;
			dp.DrawMeshShaded(mesh, displayMaterial);
			displayMaterial.Dispose();
			dp.ModelTransform = modelTransform;
		}
		for (int i = 0; i <= num; i++)
		{
			Color color = Color.FromArgb(255, 208, 208, 208);
			if (i % 5 == 0 || i == num)
			{
				color = Color.FromArgb(255, 160, 160, 160);
			}
			dp.DrawLine(new Point3d(vector3d4 * i - vector3d5 * num2 + center), new Point3d(vector3d4 * i + vector3d5 * num2 + center), color, 1);
			dp.DrawLine(new Point3d(-(vector3d4 * i) - vector3d5 * num2 + center), new Point3d(-(vector3d4 * i) + vector3d5 * num2 + center), color, 1);
		}
		for (int j = 0; j <= num2; j++)
		{
			Color color2 = Color.FromArgb(255, 208, 208, 208);
			if (j % 5 == 0 || j == num2)
			{
				color2 = Color.FromArgb(255, 160, 160, 160);
			}
			dp.DrawLine(new Point3d(vector3d5 * j - vector3d4 * num + center), new Point3d(vector3d5 * j + vector3d4 * num + center), color2, 1);
			dp.DrawLine(new Point3d(-(vector3d5 * j) - vector3d4 * num + center), new Point3d(-(vector3d5 * j) + vector3d4 * num + center), color2, 1);
		}
		dp.DrawLine(center, center + vector3d4 * num, Color.FromArgb(255, 128, 255, 128), 2);
		dp.DrawLine(center, center + vector3d5 * num2, Color.FromArgb(255, 255, 128, 128), 2);
	}

	private void DrawBendControlPoints(DisplayPipeline dp)
	{
		DisplayMaterial displayMaterial = new DisplayMaterial
		{
			Diffuse = Color.FromArgb(255, 128, 128, 255),
			Transparency = 0.6,
			Emission = Color.FromArgb(0, 0, 0),
			Ambient = Color.FromArgb(40, 40, 40)
		};
		DisplayMaterial displayMaterial2 = new DisplayMaterial
		{
			Diffuse = Color.FromArgb(255, 128, 128, 255),
			Transparency = 0.2,
			Emission = Color.FromArgb(0, 0, 0),
			Ambient = Color.FromArgb(40, 40, 40)
		};
		double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(dp, BasePoint);
		Vector3d initialBaseX = new Vector3d(7.0 / worldToScreenScale, 0.0, 0.0);
		Vector3d initialBaseY = new Vector3d(0.0, 7.0 / worldToScreenScale, 0.0);
		Vector3d initialBaseZ = new Vector3d(0.0, 0.0, 7.0 / worldToScreenScale);
		Point3d point3d = new Point3d(_p0);
		Point3d point3d2 = new Point3d(_p1);
		Point3d point3d3 = new Point3d(_p2);
		point3d.Transform(_value);
		point3d2.Transform(_value);
		point3d3.Transform(_value);
		Transform transform = CSValueHandleHelper.ChangeBasis(point3d, initialBaseX, initialBaseY, initialBaseZ);
		Transform transform2 = CSValueHandleHelper.ChangeBasis(point3d2, initialBaseX, initialBaseY, initialBaseZ);
		Transform transform3 = CSValueHandleHelper.ChangeBasis(point3d3, initialBaseX, initialBaseY, initialBaseZ);
		if (_meshSphere == null)
		{
			BuildSphere();
		}
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * transform;
		dp.DrawMeshShaded(_meshSphere, (MouseOver && _whichSelected == WSelected.Dp0) ? displayMaterial2 : displayMaterial);
		dp.ModelTransform = modelTransform * transform2;
		dp.DrawMeshShaded(_meshSphere, (MouseOver && _whichSelected == WSelected.Dp1) ? displayMaterial2 : displayMaterial);
		dp.ModelTransform = modelTransform * transform3;
		dp.DrawMeshShaded(_meshSphere, (MouseOver && _whichSelected == WSelected.Dp2) ? displayMaterial2 : displayMaterial);
		dp.ModelTransform = modelTransform;
		dp.DrawLine(point3d, point3d2, Color.FromArgb(255, 0, 0, 160), 2);
		Vector3d vector3d = new Vector3d(point3d2 - point3d);
		vector3d.Unitize();
		Vector3d b = new Vector3d(point3d3 - point3d);
		b.Unitize();
		Vector3d a = Vector3d.CrossProduct(vector3d, b);
		a.Unitize();
		Vector3d vector3d2 = Vector3d.CrossProduct(a, vector3d);
		vector3d2.Unitize();
		Vector3d b2 = new Vector3d(point3d3 - point3d);
		b2.Unitize();
		Vector3d vector3d3 = Vector3d.CrossProduct(a, b2);
		vector3d3.Unitize();
		double a2 = CSValueHandleHelper.AngleBetweenVectors(vector3d3, vector3d2);
		double num = (point3d3 - point3d).Length / 2.0 / Math.Sin(a2);
		Point3d pointOnInterior = point3d + vector3d2 * num - vector3d3 * num;
		_arc = new Arc(point3d, pointOnInterior, point3d3);
		dp.DrawArc(_arc, Color.FromArgb(255));
		displayMaterial.Dispose();
		displayMaterial2.Dispose();
	}

	private void DrawBoundingBox(DisplayPipeline displayPipeline)
	{
		if (_bb.IsValid)
		{
			Point3d min = _bb.Min;
			Point3d max = _bb.Max;
			Point3d point3d = new Point3d(min.X, min.Y, min.Z);
			Point3d point3d2 = new Point3d(min.X, max.Y, min.Z);
			Point3d point3d3 = new Point3d(max.X, max.Y, min.Z);
			Point3d point3d4 = new Point3d(max.X, min.Y, min.Z);
			Point3d point3d5 = new Point3d(min.X, min.Y, max.Z);
			Point3d point3d6 = new Point3d(min.X, max.Y, max.Z);
			Point3d to = new Point3d(max.X, max.Y, max.Z);
			Point3d point3d7 = new Point3d(max.X, min.Y, max.Z);
			point3d.Transform(_value);
			point3d2.Transform(_value);
			point3d3.Transform(_value);
			point3d4.Transform(_value);
			point3d5.Transform(_value);
			point3d6.Transform(_value);
			to.Transform(_value);
			point3d7.Transform(_value);
			Color color = Color.FromArgb(255, 192, 192, 192);
			displayPipeline.DrawLine(point3d, point3d4, color, 1);
			displayPipeline.DrawLine(point3d2, point3d3, color, 1);
			displayPipeline.DrawLine(point3d5, point3d7, color, 1);
			displayPipeline.DrawLine(point3d6, to, color, 1);
			displayPipeline.DrawLine(point3d, point3d2, color, 1);
			displayPipeline.DrawLine(point3d4, point3d3, color, 1);
			displayPipeline.DrawLine(point3d5, point3d6, color, 1);
			displayPipeline.DrawLine(point3d7, to, color, 1);
			displayPipeline.DrawLine(point3d, point3d5, color, 1);
			displayPipeline.DrawLine(point3d2, point3d6, color, 1);
			displayPipeline.DrawLine(point3d3, to, color, 1);
			displayPipeline.DrawLine(point3d4, point3d7, color, 1);
		}
	}

	private void DrawModePicker(DisplayPipeline dp, Point3d location, double pixelPerUnit, Color pointColor)
	{
		if (_meshSphere == null)
		{
			BuildSphere();
		}
		if (_meshCube == null)
		{
			BuildCube();
		}
		DisplayMaterial material = new DisplayMaterial
		{
			Diffuse = pointColor,
			Emission = Color.FromArgb(0, 0, 0),
			Ambient = Color.FromArgb(40, 40, 40)
		};
		Transform transform = CSValueHandleHelper.ChangeBasis(location, UDirX / pixelPerUnit * 10.0, UDirY / pixelPerUnit * 10.0, UDirZ / pixelPerUnit * 10.0);
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * transform;
		dp.DrawMeshShaded((CSValueHandleHelper.CurrentHandle == this) ? _meshCube : _meshSphere, material);
		dp.ModelTransform = modelTransform;
	}

	private void SetP0(Point3d newP0)
	{
		if (!(newP0 == _p0))
		{
			PreChange("SetP0");
			_p0 = new Point3d(newP0);
			Change();
		}
	}

	private void SetP1(Point3d newP1)
	{
		if (!(newP1 == _p1))
		{
			PreChange("SetP1");
			_p1 = new Point3d(newP1);
			Change();
		}
	}

	private void SetP2(Point3d newP2)
	{
		if (!(newP2 == _p2))
		{
			PreChange("SetP2");
			_p2 = new Point3d(newP2);
			Change();
		}
	}

	private void SetValue(Transform newValue)
	{
		if (!(newValue == _value))
		{
			PreChange("SetValue");
			_value = newValue;
			Change();
		}
	}

	private void SetValueFromUI(Transform newValue, Point3d newP0, Point3d newP1, Point3d newP2, bool mouseDown)
	{
		if ((mouseDown && newValue.IsValid && _value == newValue) || (mouseDown && newP0.IsValid && _p0 == newP0) || (mouseDown && newP1.IsValid && _p1 == newP1) || (mouseDown && newP2.IsValid && _p2 == newP2))
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
				if (newValue.IsValid)
				{
					SetValue(newValue);
				}
				if (newP0.IsValid)
				{
					SetP0(newP0);
				}
				if (newP1.IsValid)
				{
					SetP1(newP1);
				}
				if (newP2.IsValid)
				{
					SetP2(newP2);
				}
			}
			else
			{
				CSUtil.RemoveMouseDown();
				if (newValue.IsValid)
				{
					SetValue(newValue);
				}
				if (newP0.IsValid)
				{
					SetP0(newP0);
				}
				if (newP1.IsValid)
				{
					SetP1(newP1);
				}
				if (newP2.IsValid)
				{
					SetP2(newP2);
				}
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
			CSUtil.PrintLine(ex.Message);
			CSUtil.PrintLine(ex.StackTrace);
		}
	}

	private bool TestHitPoint(Line mouseLine, Vector3d worldLineDirection, RhinoViewport vp, WSelected selId, Point3d bp, Vector3d d0, Vector3d d1, CSValueHandleHelper.WMode msrMode)
	{
		double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(vp, bp);
		double num = 140.0 / worldToScreenScale;
		if (selId == WSelected.Sx)
		{
			num = _scaleHandleLengthX;
		}
		if (selId == WSelected.Sy)
		{
			num = _scaleHandleLengthY;
		}
		if (selId == WSelected.Sz)
		{
			num = _scaleHandleLengthZ;
		}
		if (selId == WSelected.Ss)
		{
			num = _scaleHandleLengthS;
		}
		if (selId == WSelected.Rx)
		{
			num = _scaleHandleLengthX + 60.0 / worldToScreenScale;
		}
		if (selId == WSelected.Ry)
		{
			num = _scaleHandleLengthY + 60.0 / worldToScreenScale;
		}
		if (selId == WSelected.Rz)
		{
			num = _scaleHandleLengthZ + 60.0 / worldToScreenScale;
		}
		if (selId == WSelected.Mx)
		{
			num = _scaleHandleLengthX + 60.0 / worldToScreenScale;
		}
		if (selId == WSelected.My)
		{
			num = _scaleHandleLengthY + 60.0 / worldToScreenScale;
		}
		if (selId == WSelected.Mz)
		{
			num = _scaleHandleLengthZ + 60.0 / worldToScreenScale;
		}
		Point3d point3d = bp + d0 * num;
		if (!(Math.Abs(Math.Abs(d0 * worldLineDirection)) < 0.9))
		{
			return false;
		}
		MouseOver = mouseLine.ClosestPoint(point3d, limitToFiniteSegment: false).DistanceTo(point3d) < 10.0 / worldToScreenScale;
		if (!MouseOver)
		{
			return false;
		}
		d0.Unitize();
		d1.Unitize();
		_whichSelected = selId;
		if (msrMode == CSValueHandleHelper.WMode.Rotate)
		{
			_hitPlane = new Plane(bp, d0, d1);
		}
		if (msrMode == CSValueHandleHelper.WMode.Move)
		{
			Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(vp);
			Vector3d yDirection = Vector3d.CrossProduct(d0, cameraDirection);
			yDirection.Unitize();
			_hitPlane = new Plane(point3d, d0, yDirection);
		}
		if (msrMode == CSValueHandleHelper.WMode.Scale)
		{
			Vector3d cameraDirection2 = CSValueHandleHelper.GetCameraDirection(vp);
			Vector3d yDirection2 = Vector3d.CrossProduct(d0, cameraDirection2);
			yDirection2.Unitize();
			_hitPlane = new Plane(bp, d0, yDirection2);
		}
		return true;
	}

	private bool TestHitSphere(Line mouseLine, RhinoViewport vp, Point3d bp, WSelected selId)
	{
		double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(vp, bp);
		MouseOver = mouseLine.ClosestPoint(bp, limitToFiniteSegment: false).DistanceTo(bp) < 10.0 / worldToScreenScale;
		if (!MouseOver)
		{
			return false;
		}
		_whichSelected = selId;
		return true;
	}
}
