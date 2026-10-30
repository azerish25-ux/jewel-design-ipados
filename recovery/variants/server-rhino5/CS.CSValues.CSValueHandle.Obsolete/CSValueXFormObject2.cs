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
using CSMaterials;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino.Display;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.UI;

namespace CS.CSValues.CSValueHandle.Obsolete;

[Obsolete]
[CSBlock("ValueXFormObject2", "{C0962ACD-1E65-4069-B666-BE83C59A98DF}")]
public sealed class CSValueXFormObject2 : CSValueXFormBase, ICSVHandle
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
		Dp2,
		DpMirror
	}

	private const double _handleLengthExtraPixels = 60.0;

	private const double _handleLengthPixels = 140.0;

	private const double _selectionSizePixels = 10.0;

	private static DisplayMaterial _matBendBasePoint;

	private static DisplayMaterial _matBendBasePointSelected;

	private static DisplayMaterial _matBendBendPoint;

	private static DisplayMaterial _matBendBendPointSelected;

	private static DisplayMaterial _matBendEndPoint;

	private static DisplayMaterial _matBendEndPointSelected;

	private static DisplayMaterial _matBendMirrorPoint;

	private static DisplayMaterial _matBendMirrorPointSelected;

	private static CSDisplayMaterial _matBendShadow;

	private readonly CS.CSValues.CSValueHandle.Obsolete.HandleSupportClasses.CSBendPoints _bendXy = new CS.CSValues.CSValueHandle.Obsolete.HandleSupportClasses.CSBendPoints(new Point3d(0.0, -0.2, 0.0), new Point3d(10.0, -0.2, 0.0), new Point3d(9.0, -0.2, 0.0), 1);

	private readonly CS.CSValues.CSValueHandle.Obsolete.HandleSupportClasses.CSBendPoints _bendXz = new CS.CSValues.CSValueHandle.Obsolete.HandleSupportClasses.CSBendPoints(new Point3d(0.0, 0.0, -0.2), new Point3d(10.0, 0.0, -0.2), new Point3d(9.0, 0.0, -0.2), 1);

	private readonly CS.CSValues.CSValueHandle.Obsolete.HandleSupportClasses.CSBendPoints _bendYz = new CS.CSValues.CSValueHandle.Obsolete.HandleSupportClasses.CSBendPoints(new Point3d(0.0, 0.0, -0.2), new Point3d(0.0, 10.0, -0.2), new Point3d(0.0, 9.0, -0.2), 1);

	private BoundingBox _bb;

	private BoundingBox _bbOriginal;

	private bool _bendMode;

	private CS.CSValues.CSValueHandle.Obsolete.HandleSupportClasses.CSBendPoints _currentBend;

	private bool _hasCreatedUndoTransaction;

	private Plane _hitPlane;

	private Mesh _meshArrow;

	private Mesh _meshBendRotate;

	private Mesh _meshCube;

	private Mesh _meshRotate;

	private Mesh _meshSphere;

	private CSData _obj;

	private CSData _objMeshOriginal;

	private CSData _objOriginal;

	private int _outputMirrorMode;

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

	public CSOutputChannel OutTransformedObjects1 { get; }

	public CSOutputChannel OutTransformedObjects2 { get; }

	public CSOutputChannel OutTransformedObjects3 { get; }

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

	private bool ShowBend
	{
		get
		{
			return base.UserData.FindBool("ValueXForm", "ShowBend", defaultValue: true);
		}
		set
		{
			base.UserData.AddKey("ValueXForm", "ShowBend", value.ToString());
		}
	}

	public bool MouseOver { get; set; }

	public bool MouseDown { get; set; }

	public CSValueXFormObject2(string identifier)
		: base(identifier)
	{
		_currentBend = _bendYz;
		SetDisplay();
		InObjects = new CSInputChannel(GetPropertyName(() => InObjects), this, ChannelDataType.Any, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InOnOff = new CSInputChannel(GetPropertyName(() => InOnOff), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutTransformedObjects = new CSOutputChannel(GetPropertyName(() => OutTransformedObjects), this, ChannelDataType.Any, ChannelArrayType.Any);
		OutTransformedObjects1 = new CSOutputChannel(GetPropertyName(() => OutTransformedObjects1), this, ChannelDataType.Any, ChannelArrayType.Any);
		OutTransformedObjects2 = new CSOutputChannel(GetPropertyName(() => OutTransformedObjects2), this, ChannelDataType.Any, ChannelArrayType.Any);
		OutTransformedObjects3 = new CSOutputChannel(GetPropertyName(() => OutTransformedObjects3), this, ChannelDataType.Any, ChannelArrayType.Any);
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
			CSOnGeometryData cSOnGeometryData = _objOriginal as CSOnGeometryData;
			if (cSOnGeometryData != null)
			{
				Transform modelTransform = displayPipeline.ModelTransform;
				displayPipeline.ModelTransform = modelTransform * _value;
				cSOnGeometryData.Display(displayPipeline, displayOptions, _matBendShadow);
				displayPipeline.ModelTransform = modelTransform;
			}
			else
			{
				if (!(_objOriginal is CSDataArray cSDataArray))
				{
					return;
				}
				Transform modelTransform2 = displayPipeline.ModelTransform;
				displayPipeline.ModelTransform = modelTransform2 * _value;
				for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
				{
					if (cSDataArray.GetArrayItem(i) is CSOnGeometryData cSOnGeometryData2)
					{
						cSOnGeometryData2.Display(displayPipeline, displayOptions, _matBendShadow);
					}
				}
				displayPipeline.ModelTransform = modelTransform2;
			}
		}
		else if (CSValueHandleHelper.CurrentHandle == this)
		{
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
			if (!activeViewport.IsPerspectiveProjection)
			{
				return false;
			}
			Point3d point3d = new Point3d(_currentBend.BasePoint);
			Point3d point3d2 = new Point3d(_currentBend.EndPoint);
			Point3d bp = new Point3d(_currentBend.BendPoint);
			activeViewport.GetWorldToScreenScale(BasePoint, out var pixelsPerUnit);
			Vector3d vector3d = point3d2 - point3d;
			Point3d bp2;
			if (_currentBend._bendMode > 0)
			{
				bp2 = point3d - vector3d;
			}
			else
			{
				vector3d.Unitize();
				bp2 = point3d - vector3d * 40.0 / pixelsPerUnit;
			}
			point3d.Transform(_value);
			point3d2.Transform(_value);
			bp.Transform(_value);
			bp2.Transform(_value);
			if (TestHitSphere(frustrumLine, activeViewport, point3d, WSelected.Dp0))
			{
				return true;
			}
			if (TestHitSphere(frustrumLine, activeViewport, point3d2, WSelected.Dp1))
			{
				return true;
			}
			if (TestHitSphere(frustrumLine, activeViewport, bp, WSelected.Dp2))
			{
				return true;
			}
			if (TestHitSphere(frustrumLine, activeViewport, bp2, WSelected.DpMirror))
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
				SetValueFromUI(Transform.Unset, newP, Point3d.Unset, Point3d.Unset, null, mouseDown: true);
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
				SetValueFromUI(Transform.Unset, Point3d.Unset, newP2, Point3d.Unset, null, mouseDown: true);
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
				SetValueFromUI(Transform.Unset, Point3d.Unset, Point3d.Unset, newP3, null, mouseDown: true);
				return true;
			}
			return false;
		}
		if (_whichSelected == WSelected.PCenter || _whichSelected == WSelected.DpMirror)
		{
			return false;
		}
		if (Intersection.LinePlane(frustrumLine, _hitPlane, out lineParameter))
		{
			Point3d point3d = frustrumLine.PointAt(lineParameter);
			_hitPlane.ClosestParameter(point3d, out var s, out var t);
			Transform transform = Transform.Identity;
			double num = Math.PI / 36.0;
			double num2 = CSValueHandleHelper.AngleOfPointD(t, s);
			if (CSValueHandleHelper.MouseButtonShift(e))
			{
				num2 = Math.Round(num2 / num) * num;
			}
			if (_whichSelected == WSelected.Rx)
			{
				transform = Transform.Rotation(num2, -UDirZ, BasePoint);
			}
			if (_whichSelected == WSelected.Ry)
			{
				transform = Transform.Rotation(num2, -UDirX, BasePoint);
			}
			if (_whichSelected == WSelected.Rz)
			{
				transform = Transform.Rotation(num2, -UDirY, BasePoint);
			}
			double num3 = s;
			if (_whichSelected == WSelected.Mx)
			{
				transform = Transform.Translation(UDirX * num3);
			}
			if (_whichSelected == WSelected.My)
			{
				transform = Transform.Translation(UDirY * num3);
			}
			if (_whichSelected == WSelected.Mz)
			{
				transform = Transform.Translation(UDirZ * num3);
			}
			_hitPlane.Transform(transform);
			transform *= _value;
			if (num3 < 0.1)
			{
				num3 = 0.1;
			}
			if (_whichSelected == WSelected.Sx)
			{
				transform[0, 0] = transform[0, 0] / _scaleHandleLengthX * num3;
				transform[1, 0] = transform[1, 0] / _scaleHandleLengthX * num3;
				transform[2, 0] = transform[2, 0] / _scaleHandleLengthX * num3;
				_scaleHandleLengthX = num3;
			}
			if (_whichSelected == WSelected.Sy)
			{
				transform[0, 1] = transform[0, 1] / _scaleHandleLengthY * num3;
				transform[1, 1] = transform[1, 1] / _scaleHandleLengthY * num3;
				transform[2, 1] = transform[2, 1] / _scaleHandleLengthY * num3;
				_scaleHandleLengthY = num3;
			}
			if (_whichSelected == WSelected.Sz)
			{
				transform[0, 2] = transform[0, 2] / _scaleHandleLengthZ * num3;
				transform[1, 2] = transform[1, 2] / _scaleHandleLengthZ * num3;
				transform[2, 2] = transform[2, 2] / _scaleHandleLengthZ * num3;
				_scaleHandleLengthZ = num3;
			}
			if (_whichSelected == WSelected.Ss)
			{
				double num4 = num3 / _scaleHandleLengthS;
				transform[0, 0] *= num4;
				transform[1, 0] *= num4;
				transform[2, 0] *= num4;
				transform[0, 1] *= num4;
				transform[1, 1] *= num4;
				transform[2, 1] *= num4;
				transform[0, 2] *= num4;
				transform[1, 2] *= num4;
				transform[2, 2] *= num4;
				_scaleHandleLengthS = num3;
			}
			SetValueFromUI(transform, Point3d.Unset, Point3d.Unset, Point3d.Unset, null, mouseDown: true);
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
		if (_whichSelected == WSelected.DpMirror)
		{
			SetValueFromUI(Transform.Unset, Point3d.Unset, Point3d.Unset, Point3d.Unset, (byte)((_currentBend._bendMode + 1) % 3), mouseDown: true);
		}
		else if (_whichSelected == WSelected.PCenter)
		{
			if (CSValueHandleHelper.CurrentHandle == this && !_bendMode && ShowBend)
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
			return;
		}
		SetValueFromUI(Transform.Unset, Point3d.Unset, Point3d.Unset, Point3d.Unset, null, mouseDown: false);
		CSApplicationHelper.Application.Document.Graph.Generate();
		CSUtil.Redraw();
		CSUtil.RedrawUI();
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
		_meshBendRotate?.Dispose();
		base.Dispose();
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		return base.ReadJson(jBlock, graph, major, minor) & jBlock.GetMatrix("Value", out _value) & _bendXy.Read("BendXY") & _bendXz.Read("BendXZ") & _bendYz.Read("BendYZ") & jBlock.ReadJsonValue<int>("MirrorMode", out _outputMirrorMode);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		int num = 0;
		archive.ReadInt(ref num);
		if (num == 1)
		{
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					double value = 0.0;
					archive.ReadDouble(ref value);
					_value[i, j] = value;
				}
			}
			_bendXy.Read(ref archive);
			_bendXz.Read(ref archive);
			_bendYz.Read(ref archive);
			archive.ReadInt(ref _outputMirrorMode);
		}
		return true;
	}

	public override bool SetFileValue(CSObject setObject)
	{
		base.SetFileValue(setObject);
		if (!base.NotCreatedYet && GetType().IsInstanceOfType(setObject))
		{
			_value = ((CSValueXFormObject2)setObject)._value;
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
		_value = ((CSValueXFormObject2)setObject)._value;
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (!base.NotCreatedYet && GetType().IsInstanceOfType(setObject))
		{
			_value = ((CSValueXFormObject2)setObject)._value;
		}
		return true;
	}

	public void SetVisible(bool showMove, bool showRotate, bool showScale, bool showBend)
	{
		ShowMove = showMove;
		ShowRotate = showRotate;
		ShowScale = showScale;
		ShowBend = showBend;
	}

	public override bool Update()
	{
		bool flag = BeenUpdatedAfterLastUpdate(InObjects.OutputChannelBlock);
		if (flag)
		{
			_objOriginal = InObjects.Data.Duplicate();
			_objOriginal.GetBoundingBox(ref _bbOriginal);
			if (!_bbOriginal.IsValid)
			{
				return base.Update();
			}
			Point3d endPoint = _bendXy.EndPoint;
			endPoint.X = _bbOriginal.Max.X;
			_bendXy.EndPoint = endPoint;
			endPoint = _bendXz.EndPoint;
			endPoint.X = _bbOriginal.Max.X;
			_bendXz.EndPoint = endPoint;
			endPoint = _bendYz.EndPoint;
			endPoint.Y = _bbOriginal.Max.Y;
			_bendYz.EndPoint = endPoint;
			Point3d bendPoint = _bendXy.BendPoint;
			bendPoint.X = _bbOriginal.Max.X * 0.9;
			_bendXy.BendPoint = bendPoint;
			bendPoint = _bendXz.BendPoint;
			bendPoint.X = _bbOriginal.Max.X * 0.9;
			_bendXz.BendPoint = bendPoint;
			bendPoint = _bendYz.BendPoint;
			bendPoint.Y = _bbOriginal.Max.Y * 0.9;
			_bendYz.BendPoint = bendPoint;
			MeshingParameters coarse = MeshingParameters.Coarse;
			coarse.GridMinCount = 16;
			_objMeshOriginal = new CSOnGeometryData(_objOriginal.MeshObject(coarse));
		}
		if (_objOriginal == null)
		{
			CalculateScaleHandleSizes();
			return base.Update();
		}
		flag |= _obj == null;
		flag |= _bendXz.HasChanged();
		flag |= _bendXy.HasChanged();
		if ((flag | _bendYz.HasChanged()) && _objOriginal != null)
		{
			_obj = (CSApplicationHelper.Application.Document.Graph.MouseDown ? _objMeshOriginal.Duplicate() : _objOriginal.Duplicate());
			if (_bendXz.IsBending())
			{
				CSBendMorph2 spaceMorph = new CSBendMorph2(_bendXz)
				{
					Tolerance = 0.05
				};
				_obj.Morph(spaceMorph);
				_bendXz.Changed();
			}
			if (_bendXy.IsBending())
			{
				CSBendMorph2 spaceMorph2 = new CSBendMorph2(_bendXy)
				{
					Tolerance = 0.05
				};
				_obj.Morph(spaceMorph2);
				_bendXy.Changed();
			}
			if (_bendYz.IsBending())
			{
				CSBendMorph2 spaceMorph3 = new CSBendMorph2(_bendYz)
				{
					Tolerance = 0.05
				};
				_obj.Morph(spaceMorph3);
				_bendXy.Changed();
			}
			_obj.GetBoundingBox(ref _bb);
			if (!_bb.IsValid)
			{
				_bb = new BoundingBox(new Point3d(-5.0, -5.0, -2.0), new Point3d(5.0, 5.0, 2.0));
			}
		}
		CSData cSData = _obj.Duplicate();
		(cSData as ICSTransformable)?.ApplyTransform(_value);
		CalculateScaleHandleSizes();
		OutTransformedObjects.SetData(cSData);
		if (_outputMirrorMode == 1 || _outputMirrorMode == 4)
		{
			CSData cSData2 = cSData.Duplicate();
			if (cSData2 is ICSTransformable iCSTransformable)
			{
				Transform transform = Transform.Mirror(new Point3d(0.0, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0));
				iCSTransformable.ApplyTransform(transform);
			}
			OutTransformedObjects1.SetData(cSData2);
		}
		if (_outputMirrorMode == 2 || _outputMirrorMode == 4)
		{
			CSData cSData3 = cSData.Duplicate();
			if (cSData3 is ICSTransformable iCSTransformable2)
			{
				Transform transform2 = Transform.Mirror(new Point3d(0.0, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0));
				iCSTransformable2.ApplyTransform(transform2);
				Transform transform3 = Transform.Mirror(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 1.0, 0.0));
				iCSTransformable2.ApplyTransform(transform3);
			}
			OutTransformedObjects2.SetData(cSData3);
		}
		if (_outputMirrorMode == 3 || _outputMirrorMode == 4)
		{
			CSData cSData4 = cSData.Duplicate();
			if (cSData4 is ICSTransformable iCSTransformable3)
			{
				Transform transform4 = Transform.Mirror(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 1.0, 0.0));
				iCSTransformable3.ApplyTransform(transform4);
			}
			OutTransformedObjects1.SetData(cSData4);
		}
		OutValue.SetData(new CSXformData(_value));
		return base.Update();
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("Value", _value);
		archive.Add("BendXY", _bendXy.WriteJson());
		archive.Add("BendXZ", _bendXz.WriteJson());
		archive.Add("BendYZ", _bendYz.WriteJson());
		archive.Add("MirrorMode", _outputMirrorMode);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.WriteInt(1);
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				archive.WriteDouble(_value[i, j]);
			}
		}
		_bendXy.Write(ref archive);
		_bendXz.Write(ref archive);
		_bendYz.Write(ref archive);
		archive.WriteInt(_outputMirrorMode);
		return true;
	}

	private void BuildArrow()
	{
		Cone cone = new Cone(new Plane(new Point3d(1.3, 0.0, 0.0), Vector3d.YAxis, Vector3d.ZAxis), -2.5, 1.0);
		_meshArrow = Mesh.CreateFromCone(cone, 1, 36);
	}

	private void BuildBendRotate()
	{
		Sphere sphere = new Sphere(Point3d.Origin, 2.0);
		_meshBendRotate = Mesh.CreateFromSphere(sphere, 20, 36);
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
		if (!_bb.IsValid)
		{
			_scaleHandleLengthX = 5.0;
			_scaleHandleLengthY = 5.0;
			_scaleHandleLengthZ = 5.0;
			_scaleHandleLengthS = 10.0;
			return;
		}
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
		if (_meshSphere == null)
		{
			BuildSphere();
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
		if (msrMode == CSValueHandleHelper.WMode.Rotate && flag)
		{
			Vector3d a2 = location - baseLocation;
			a2.Unitize();
			Vector3d normal = Vector3d.CrossProduct(a2, upVector);
			Plane basePlane = new Plane(baseLocation, normal);
			Torus torus = new Torus(basePlane, (location - baseLocation).Length, 2.0 / pixelPerUnit);
			Mesh mesh2 = MeshTorus(torus, 90, 10);
			dp.DrawMeshShaded(mesh2, displayMaterial);
		}
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * transform;
		displayMaterial.Transparency = (flag ? 0.2 : 0.7);
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
		displayMaterial.Dispose();
	}

	private void DrawBendBackGroundGrid(DisplayPipeline dp)
	{
		if (_bb.IsValid && dp.Viewport.IsPerspectiveProjection)
		{
			Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(dp);
			cameraDirection.Unitize();
			_bbOriginal.Center.Transform(_value);
			Vector3d vector3d = new Vector3d(1.0, 0.0, 0.0);
			Vector3d vector3d2 = new Vector3d(0.0, 1.0, 0.0);
			Vector3d vector3d3 = new Vector3d(0.0, 0.0, 1.0);
			vector3d.Transform(_value);
			vector3d.Unitize();
			vector3d2.Transform(_value);
			vector3d2.Unitize();
			vector3d3.Transform(_value);
			vector3d3.Unitize();
			double num = Math.Abs(cameraDirection * vector3d);
			double num2 = Math.Abs(cameraDirection * vector3d2);
			double num3 = Math.Abs(cameraDirection * vector3d3);
			Vector3d xDirection = new Vector3d(1.0, 0.0, 0.0);
			Vector3d yDirection = new Vector3d(1.0, 0.0, 0.0);
			double num4 = 0.0;
			if (num > num4)
			{
				xDirection = vector3d2;
				yDirection = vector3d3;
				num4 = num;
				_currentBend = _bendYz;
			}
			if (num2 > num4)
			{
				xDirection = vector3d;
				yDirection = vector3d3;
				num4 = num2;
				_currentBend = _bendXz;
			}
			if (num3 > num4)
			{
				xDirection = vector3d;
				yDirection = vector3d2;
				_currentBend = _bendXy;
			}
			Point3d origin = new Point3d(_currentBend.BasePoint);
			Point3d origin2 = new Point3d(_currentBend.EndPoint);
			Point3d origin3 = new Point3d(_currentBend.BendPoint);
			origin.Transform(_value);
			origin2.Transform(_value);
			origin3.Transform(_value);
			_pl0 = new Plane(origin, xDirection, yDirection);
			_pl1 = new Plane(origin2, xDirection, yDirection);
			_pl2 = new Plane(origin3, xDirection, yDirection);
		}
	}

	private void DrawBendControlPoints(DisplayPipeline dp)
	{
		if (dp.Viewport.IsPerspectiveProjection)
		{
			double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(dp, BasePoint);
			Vector3d vector3d = new Vector3d(7.0 / worldToScreenScale, 0.0, 0.0);
			Vector3d initialBaseY = new Vector3d(0.0, 7.0 / worldToScreenScale, 0.0);
			Vector3d initialBaseZ = new Vector3d(0.0, 0.0, 7.0 / worldToScreenScale);
			Point3d point3d = new Point3d(_currentBend.BasePoint);
			Point3d point3d2 = new Point3d(_currentBend.EndPoint);
			Point3d initialLocation = new Point3d(_currentBend.BendPoint);
			Vector3d vector3d2 = point3d2 - point3d;
			Point3d point3d3;
			if (_currentBend._bendMode > 0)
			{
				point3d3 = point3d - vector3d2;
			}
			else
			{
				vector3d2.Unitize();
				point3d3 = point3d - vector3d2 * 40.0 / worldToScreenScale;
			}
			point3d.Transform(_value);
			point3d2.Transform(_value);
			initialLocation.Transform(_value);
			point3d3.Transform(_value);
			NurbsCurve nurbsCurve = _currentBend.GetArc().ToNurbsCurve();
			nurbsCurve.Transform(_value);
			NurbsCurve nurbsCurve2 = _currentBend.GetMirrorArc().ToNurbsCurve();
			nurbsCurve2.Transform(_value);
			Transform transform = CSValueHandleHelper.ChangeBasis(point3d, vector3d, initialBaseY, initialBaseZ);
			Transform transform2 = CSValueHandleHelper.ChangeBasis(point3d2, vector3d, initialBaseY, initialBaseZ);
			Transform transform3 = CSValueHandleHelper.ChangeBasis(initialLocation, vector3d, initialBaseY, initialBaseZ);
			Transform transform4 = ((_currentBend._bendMode > 0) ? CSValueHandleHelper.ChangeBasis(point3d3, vector3d, initialBaseY, initialBaseZ) : CSValueHandleHelper.ChangeBasis(point3d3, -vector3d, initialBaseY, initialBaseZ));
			if (_meshSphere == null)
			{
				BuildSphere();
			}
			if (_meshBendRotate == null)
			{
				BuildBendRotate();
			}
			Transform modelTransform = dp.ModelTransform;
			dp.ModelTransform = modelTransform * transform;
			dp.DrawMeshShaded(_meshSphere, (MouseOver && _whichSelected == WSelected.Dp0) ? _matBendBasePointSelected : _matBendBasePoint);
			dp.ModelTransform = modelTransform * transform2;
			dp.DrawMeshShaded(_meshSphere, (MouseOver && _whichSelected == WSelected.Dp1) ? _matBendEndPointSelected : _matBendEndPoint);
			dp.ModelTransform = modelTransform * transform3;
			dp.DrawMeshShaded(_meshBendRotate, (MouseOver && _whichSelected == WSelected.Dp2) ? _matBendBendPointSelected : _matBendBendPoint);
			dp.ModelTransform = modelTransform * transform4;
			dp.DrawMeshShaded(_meshArrow, (MouseOver && _whichSelected == WSelected.DpMirror) ? _matBendMirrorPointSelected : _matBendMirrorPoint);
			dp.ModelTransform = modelTransform;
			dp.DrawLine(point3d2, point3d3, Color.FromArgb(255, 160, 160, 160), 2);
			dp.DrawCurve(nurbsCurve, Color.FromArgb(255, 255, 144, 64));
			if (_currentBend._bendMode > 0)
			{
				dp.DrawCurve(nurbsCurve2, Color.FromArgb(255, 255, 144, 64));
			}
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
		DisplayMaterial displayMaterial = new DisplayMaterial
		{
			Diffuse = pointColor,
			Emission = Color.FromArgb(0, 0, 0),
			Ambient = Color.FromArgb(40, 40, 40)
		};
		Transform transform = CSValueHandleHelper.ChangeBasis(location, UDirX / pixelPerUnit * 10.0, UDirY / pixelPerUnit * 10.0, UDirZ / pixelPerUnit * 10.0);
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * transform;
		dp.DrawMeshShaded((CSValueHandleHelper.CurrentHandle == this) ? _meshCube : _meshSphere, displayMaterial);
		dp.ModelTransform = modelTransform;
		displayMaterial.Dispose();
	}

	private Mesh MeshTorus(Torus torus, int majorAxis, int minorAxis)
	{
		NurbsSurface nurbsSurface = torus.ToNurbsSurface();
		Mesh mesh = new Mesh();
		for (int i = 0; i <= majorAxis; i++)
		{
			for (int j = 0; j <= minorAxis; j++)
			{
				double u = (double)i / (double)majorAxis * 2.0 * Math.PI;
				double v = (double)j / (double)minorAxis * 2.0 * Math.PI;
				mesh.Vertices.Add(nurbsSurface.PointAt(u, v));
				mesh.Normals.Add(nurbsSurface.NormalAt(u, v));
			}
		}
		for (int k = 0; k <= majorAxis - 1; k++)
		{
			for (int l = 0; l <= minorAxis - 1; l++)
			{
				mesh.Faces.AddFace(k * (minorAxis + 1) + l, (k + 1) * (minorAxis + 1) + l, (k + 1) * (minorAxis + 1) + l + 1, k * (minorAxis + 1) + l + 1);
			}
		}
		mesh.Compact();
		return mesh;
	}

	private static void SetDisplay()
	{
		if (_matBendShadow == null)
		{
			_matBendShadow = new CSDisplayMaterial(Color.FromArgb(255, 64, 128, 128), Color.FromArgb(40, 40, 40), 0.8, "Shadow");
		}
		if (_matBendBasePoint == null)
		{
			_matBendBasePoint = new DisplayMaterial
			{
				Diffuse = Color.FromArgb(255, 96, 96, 255),
				Transparency = 0.6,
				Emission = Color.FromArgb(0, 0, 0),
				Ambient = Color.FromArgb(40, 40, 40)
			};
		}
		if (_matBendBasePointSelected == null)
		{
			_matBendBasePointSelected = new DisplayMaterial
			{
				Diffuse = Color.FromArgb(255, 96, 96, 255),
				Transparency = 0.2,
				Emission = Color.FromArgb(0, 0, 0),
				Ambient = Color.FromArgb(40, 40, 40)
			};
		}
		if (_matBendEndPoint == null)
		{
			_matBendEndPoint = new DisplayMaterial
			{
				Diffuse = Color.FromArgb(255, 255, 255, 96),
				Transparency = 0.6,
				Emission = Color.FromArgb(0, 0, 0),
				Ambient = Color.FromArgb(40, 40, 40)
			};
		}
		if (_matBendEndPointSelected == null)
		{
			_matBendEndPointSelected = new DisplayMaterial
			{
				Diffuse = Color.FromArgb(255, 255, 255, 96),
				Transparency = 0.2,
				Emission = Color.FromArgb(0, 0, 0),
				Ambient = Color.FromArgb(40, 40, 40)
			};
		}
		if (_matBendBendPoint == null)
		{
			_matBendBendPoint = new DisplayMaterial
			{
				Diffuse = Color.FromArgb(255, 255, 144, 64),
				Transparency = 0.6,
				Emission = Color.FromArgb(0, 0, 0),
				Ambient = Color.FromArgb(40, 40, 40)
			};
		}
		if (_matBendBendPointSelected == null)
		{
			_matBendBendPointSelected = new DisplayMaterial
			{
				Diffuse = Color.FromArgb(255, 255, 144, 64),
				Transparency = 0.2,
				Emission = Color.FromArgb(0, 0, 0),
				Ambient = Color.FromArgb(40, 40, 40)
			};
		}
		if (_matBendMirrorPoint == null)
		{
			_matBendMirrorPoint = new DisplayMaterial
			{
				Diffuse = Color.FromArgb(255, 255, 255, 96),
				Transparency = 0.6,
				Emission = Color.FromArgb(0, 0, 0),
				Ambient = Color.FromArgb(40, 40, 40)
			};
		}
		if (_matBendMirrorPointSelected == null)
		{
			_matBendMirrorPointSelected = new DisplayMaterial
			{
				Diffuse = Color.FromArgb(255, 255, 255, 96),
				Transparency = 0.2,
				Emission = Color.FromArgb(0, 0, 0),
				Ambient = Color.FromArgb(40, 40, 40)
			};
		}
	}

	private void SetMirrorModeFromUI(int mirrorMode, bool mouseDown)
	{
		try
		{
			if (mouseDown)
			{
				CSUtil.ToggleMouseDown(this);
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetMirrorModeFromUI");
				PreChange("SetMirrorModeFromUI");
				_outputMirrorMode = mirrorMode;
				Change();
				CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetMirrorModeFromUI");
				CSApplicationHelper.Application.Document.Graph.Generate(isQuick: true);
				CSUtil.Redraw();
				CSUtil.RedrawUI();
			}
			else
			{
				CSUtil.RemoveMouseDown();
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetMirrorModeFromUI");
				PreChange("SetMirrorModeFromUI");
				_outputMirrorMode = mirrorMode;
				Change();
				CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetMirrorModeFromUI");
				CSApplicationHelper.Application.Document.Graph.Generate();
				CSUtil.Redraw();
				CSUtil.RedrawUI();
			}
		}
		catch (Exception ex)
		{
			CSUtil.PrintLine(ex.Message);
			CSUtil.PrintLine(ex.StackTrace);
		}
	}

	private void SetP0(Point3d newP0)
	{
		if (!(newP0 == _currentBend.BasePoint))
		{
			PreChange("SetP0");
			_currentBend.BasePoint = newP0;
			Change();
		}
	}

	private void SetP1(Point3d newP1)
	{
		if (!(newP1 == _currentBend.EndPoint))
		{
			PreChange("SetP1");
			_currentBend.EndPoint = new Point3d(newP1);
			Change();
		}
	}

	private void SetP2(Point3d newP2)
	{
		if (!(newP2 == _currentBend.BendPoint))
		{
			PreChange("SetP2");
			_currentBend.BendPoint = new Point3d(newP2);
			Change();
		}
	}

	private void SetSymetrical(byte symetrical)
	{
		if (symetrical != _currentBend._bendMode)
		{
			PreChange("SetSymetrical");
			_currentBend._bendMode = symetrical;
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

	private void SetValueFromUI(Transform newValue, Point3d newP0, Point3d newP1, Point3d newP2, byte? bendMode, bool mouseDown)
	{
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
				if (bendMode.HasValue)
				{
					SetSymetrical(bendMode.Value);
				}
			}
			else
			{
				CSUtil.RemoveMouseDown();
				PreChange("SetValueFromUI");
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
				if (bendMode.HasValue)
				{
					SetSymetrical(bendMode.Value);
				}
				Change();
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
