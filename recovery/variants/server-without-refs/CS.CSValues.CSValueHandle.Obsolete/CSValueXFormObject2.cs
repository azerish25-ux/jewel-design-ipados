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
using Rhino.Runtime;
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
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			Vector3d result = default(Vector3d);
			((Vector3d)(ref result))._002Ector(((Transform)(ref _value))[0, 0], ((Transform)(ref _value))[1, 0], ((Transform)(ref _value))[2, 0]);
			((Vector3d)(ref result)).Unitize();
			return result;
		}
	}

	private Vector3d UDirY
	{
		get
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			Vector3d result = default(Vector3d);
			((Vector3d)(ref result))._002Ector(((Transform)(ref _value))[0, 1], ((Transform)(ref _value))[1, 1], ((Transform)(ref _value))[2, 1]);
			((Vector3d)(ref result)).Unitize();
			return result;
		}
	}

	private Vector3d UDirZ
	{
		get
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			Vector3d result = default(Vector3d);
			((Vector3d)(ref result))._002Ector(((Transform)(ref _value))[0, 2], ((Transform)(ref _value))[1, 2], ((Transform)(ref _value))[2, 2]);
			((Vector3d)(ref result)).Unitize();
			return result;
		}
	}

	private Vector3d UDirS
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(((BoundingBox)(ref _bb)).Max);
			((Point3d)(ref val)).Transform(_value);
			Vector3d result = default(Vector3d);
			((Vector3d)(ref result))._002Ector(val - BasePoint);
			((Vector3d)(ref result)).Unitize();
			return result;
		}
	}

	private Point3d BasePoint => new Point3d(((Transform)(ref _value))[0, 3], ((Transform)(ref _value))[1, 3], ((Transform)(ref _value))[2, 3]);

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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		if (!IsOn)
		{
			return false;
		}
		RhinoViewport activeViewport = e.View.ActiveViewport;
		Line frustrumLine = CSValueHandleHelper.GetFrustrumLine(activeViewport, e.ViewportPoint);
		Vector3d direction = ((Line)(ref frustrumLine)).Direction;
		((Vector3d)(ref direction)).Unitize();
		if (_bendMode)
		{
			if (!activeViewport.IsPerspectiveProjection)
			{
				return false;
			}
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(_currentBend.BasePoint);
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(_currentBend.EndPoint);
			Point3d bp = default(Point3d);
			((Point3d)(ref bp))._002Ector(_currentBend.BendPoint);
			double num = default(double);
			activeViewport.GetWorldToScreenScale(BasePoint, ref num);
			Vector3d val3 = val2 - val;
			Point3d bp2;
			if (_currentBend._bendMode > 0)
			{
				bp2 = val - val3;
			}
			else
			{
				((Vector3d)(ref val3)).Unitize();
				bp2 = val - val3 * 40.0 / num;
			}
			((Point3d)(ref val)).Transform(_value);
			((Point3d)(ref val2)).Transform(_value);
			((Point3d)(ref bp)).Transform(_value);
			((Point3d)(ref bp2)).Transform(_value);
			if (TestHitSphere(frustrumLine, activeViewport, val, WSelected.Dp0))
			{
				return true;
			}
			if (TestHitSphere(frustrumLine, activeViewport, val2, WSelected.Dp1))
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
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		if (!MouseOver)
		{
			return false;
		}
		Line frustrumLine = CSValueHandleHelper.GetFrustrumLine(e.View.ActiveViewport, e.ViewportPoint);
		double num = default(double);
		if (_whichSelected == WSelected.Dp0)
		{
			if (Intersection.LinePlane(frustrumLine, _pl0, ref num))
			{
				Point3d val = ((Line)(ref frustrumLine)).PointAt(num);
				Point3d newP = val;
				Transform val2 = default(Transform);
				((Transform)(ref _value)).TryGetInverse(ref val2);
				((Point3d)(ref newP)).Transform(val2);
				SetValueFromUI(Transform.Unset, newP, Point3d.Unset, Point3d.Unset, null, mouseDown: true);
				return true;
			}
			return false;
		}
		if (_whichSelected == WSelected.Dp1)
		{
			if (Intersection.LinePlane(frustrumLine, _pl1, ref num))
			{
				Point3d val = ((Line)(ref frustrumLine)).PointAt(num);
				Point3d newP2 = val;
				Transform val3 = default(Transform);
				((Transform)(ref _value)).TryGetInverse(ref val3);
				((Point3d)(ref newP2)).Transform(val3);
				SetValueFromUI(Transform.Unset, Point3d.Unset, newP2, Point3d.Unset, null, mouseDown: true);
				return true;
			}
			return false;
		}
		if (_whichSelected == WSelected.Dp2)
		{
			if (Intersection.LinePlane(frustrumLine, _pl2, ref num))
			{
				Point3d val = ((Line)(ref frustrumLine)).PointAt(num);
				Point3d newP3 = val;
				Transform val4 = default(Transform);
				((Transform)(ref _value)).TryGetInverse(ref val4);
				((Point3d)(ref newP3)).Transform(val4);
				SetValueFromUI(Transform.Unset, Point3d.Unset, Point3d.Unset, newP3, null, mouseDown: true);
				return true;
			}
			return false;
		}
		if (_whichSelected == WSelected.PCenter || _whichSelected == WSelected.DpMirror)
		{
			return false;
		}
		if (Intersection.LinePlane(frustrumLine, _hitPlane, ref num))
		{
			Point3d val = ((Line)(ref frustrumLine)).PointAt(num);
			double num2 = default(double);
			double x = default(double);
			((Plane)(ref _hitPlane)).ClosestParameter(val, ref num2, ref x);
			Transform val5 = Transform.Identity;
			double num3 = Math.PI / 36.0;
			double num4 = CSValueHandleHelper.AngleOfPointD(x, num2);
			if (CSValueHandleHelper.MouseButtonShift(e))
			{
				num4 = Math.Round(num4 / num3) * num3;
			}
			if (_whichSelected == WSelected.Rx)
			{
				val5 = Transform.Rotation(num4, -UDirZ, BasePoint);
			}
			if (_whichSelected == WSelected.Ry)
			{
				val5 = Transform.Rotation(num4, -UDirX, BasePoint);
			}
			if (_whichSelected == WSelected.Rz)
			{
				val5 = Transform.Rotation(num4, -UDirY, BasePoint);
			}
			double num5 = num2;
			if (_whichSelected == WSelected.Mx)
			{
				val5 = Transform.Translation(UDirX * num5);
			}
			if (_whichSelected == WSelected.My)
			{
				val5 = Transform.Translation(UDirY * num5);
			}
			if (_whichSelected == WSelected.Mz)
			{
				val5 = Transform.Translation(UDirZ * num5);
			}
			((Plane)(ref _hitPlane)).Transform(val5);
			val5 *= _value;
			if (num5 < 0.1)
			{
				num5 = 0.1;
			}
			if (_whichSelected == WSelected.Sx)
			{
				((Transform)(ref val5))[0, 0] = ((Transform)(ref val5))[0, 0] / _scaleHandleLengthX * num5;
				((Transform)(ref val5))[1, 0] = ((Transform)(ref val5))[1, 0] / _scaleHandleLengthX * num5;
				((Transform)(ref val5))[2, 0] = ((Transform)(ref val5))[2, 0] / _scaleHandleLengthX * num5;
				_scaleHandleLengthX = num5;
			}
			if (_whichSelected == WSelected.Sy)
			{
				((Transform)(ref val5))[0, 1] = ((Transform)(ref val5))[0, 1] / _scaleHandleLengthY * num5;
				((Transform)(ref val5))[1, 1] = ((Transform)(ref val5))[1, 1] / _scaleHandleLengthY * num5;
				((Transform)(ref val5))[2, 1] = ((Transform)(ref val5))[2, 1] / _scaleHandleLengthY * num5;
				_scaleHandleLengthY = num5;
			}
			if (_whichSelected == WSelected.Sz)
			{
				((Transform)(ref val5))[0, 2] = ((Transform)(ref val5))[0, 2] / _scaleHandleLengthZ * num5;
				((Transform)(ref val5))[1, 2] = ((Transform)(ref val5))[1, 2] / _scaleHandleLengthZ * num5;
				((Transform)(ref val5))[2, 2] = ((Transform)(ref val5))[2, 2] / _scaleHandleLengthZ * num5;
				_scaleHandleLengthZ = num5;
			}
			if (_whichSelected == WSelected.Ss)
			{
				double num6 = num5 / _scaleHandleLengthS;
				((Transform)(ref val5))[0, 0] = ((Transform)(ref val5))[0, 0] * num6;
				((Transform)(ref val5))[1, 0] = ((Transform)(ref val5))[1, 0] * num6;
				((Transform)(ref val5))[2, 0] = ((Transform)(ref val5))[2, 0] * num6;
				((Transform)(ref val5))[0, 1] = ((Transform)(ref val5))[0, 1] * num6;
				((Transform)(ref val5))[1, 1] = ((Transform)(ref val5))[1, 1] * num6;
				((Transform)(ref val5))[2, 1] = ((Transform)(ref val5))[2, 1] * num6;
				((Transform)(ref val5))[0, 2] = ((Transform)(ref val5))[0, 2] * num6;
				((Transform)(ref val5))[1, 2] = ((Transform)(ref val5))[1, 2] * num6;
				((Transform)(ref val5))[2, 2] = ((Transform)(ref val5))[2, 2] * num6;
				_scaleHandleLengthS = num5;
			}
			SetValueFromUI(val5, Point3d.Unset, Point3d.Unset, Point3d.Unset, null, mouseDown: true);
			return false;
		}
		return false;
	}

	public void MouseUp(MouseCallbackEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
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
		Mesh meshArrow = _meshArrow;
		if (meshArrow != null)
		{
			((CommonObject)meshArrow).Dispose();
		}
		Mesh meshCube = _meshCube;
		if (meshCube != null)
		{
			((CommonObject)meshCube).Dispose();
		}
		Mesh meshRotate = _meshRotate;
		if (meshRotate != null)
		{
			((CommonObject)meshRotate).Dispose();
		}
		Mesh meshSphere = _meshSphere;
		if (meshSphere != null)
		{
			((CommonObject)meshSphere).Dispose();
		}
		Mesh meshBendRotate = _meshBendRotate;
		if (meshBendRotate != null)
		{
			((CommonObject)meshBendRotate).Dispose();
		}
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
					double num2 = 0.0;
					archive.ReadDouble(ref num2);
					((Transform)(ref _value))[i, j] = num2;
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
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		base.SetFileValue(setObject);
		if (!base.NotCreatedYet && GetType().IsInstanceOfType(setObject))
		{
			_value = ((CSValueXFormObject2)setObject)._value;
		}
		return true;
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		bool flag = BeenUpdatedAfterLastUpdate(InObjects.OutputChannelBlock);
		if (flag)
		{
			_objOriginal = InObjects.Data.Duplicate();
			_objOriginal.GetBoundingBox(ref _bbOriginal);
			if (!((BoundingBox)(ref _bbOriginal)).IsValid)
			{
				return base.Update();
			}
			Point3d endPoint = _bendXy.EndPoint;
			Point3d max = ((BoundingBox)(ref _bbOriginal)).Max;
			((Point3d)(ref endPoint)).X = ((Point3d)(ref max)).X;
			_bendXy.EndPoint = endPoint;
			endPoint = _bendXz.EndPoint;
			max = ((BoundingBox)(ref _bbOriginal)).Max;
			((Point3d)(ref endPoint)).X = ((Point3d)(ref max)).X;
			_bendXz.EndPoint = endPoint;
			endPoint = _bendYz.EndPoint;
			max = ((BoundingBox)(ref _bbOriginal)).Max;
			((Point3d)(ref endPoint)).Y = ((Point3d)(ref max)).Y;
			_bendYz.EndPoint = endPoint;
			Point3d bendPoint = _bendXy.BendPoint;
			max = ((BoundingBox)(ref _bbOriginal)).Max;
			((Point3d)(ref bendPoint)).X = ((Point3d)(ref max)).X * 0.9;
			_bendXy.BendPoint = bendPoint;
			bendPoint = _bendXz.BendPoint;
			max = ((BoundingBox)(ref _bbOriginal)).Max;
			((Point3d)(ref bendPoint)).X = ((Point3d)(ref max)).X * 0.9;
			_bendXz.BendPoint = bendPoint;
			bendPoint = _bendYz.BendPoint;
			max = ((BoundingBox)(ref _bbOriginal)).Max;
			((Point3d)(ref bendPoint)).Y = ((Point3d)(ref max)).Y * 0.9;
			_bendYz.BendPoint = bendPoint;
			MeshingParameters coarse = MeshingParameters.Coarse;
			coarse.GridMinCount = 16;
			_objMeshOriginal = new CSOnGeometryData((GeometryBase)(object)_objOriginal.MeshObject(coarse));
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
				CSBendMorph2 cSBendMorph = new CSBendMorph2(_bendXz);
				((SpaceMorph)cSBendMorph).Tolerance = 0.05;
				CSBendMorph2 spaceMorph = cSBendMorph;
				_obj.Morph((SpaceMorph)(object)spaceMorph);
				_bendXz.Changed();
			}
			if (_bendXy.IsBending())
			{
				CSBendMorph2 cSBendMorph2 = new CSBendMorph2(_bendXy);
				((SpaceMorph)cSBendMorph2).Tolerance = 0.05;
				CSBendMorph2 spaceMorph2 = cSBendMorph2;
				_obj.Morph((SpaceMorph)(object)spaceMorph2);
				_bendXy.Changed();
			}
			if (_bendYz.IsBending())
			{
				CSBendMorph2 cSBendMorph3 = new CSBendMorph2(_bendYz);
				((SpaceMorph)cSBendMorph3).Tolerance = 0.05;
				CSBendMorph2 spaceMorph3 = cSBendMorph3;
				_obj.Morph((SpaceMorph)(object)spaceMorph3);
				_bendXy.Changed();
			}
			_obj.GetBoundingBox(ref _bb);
			if (!((BoundingBox)(ref _bb)).IsValid)
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
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
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
				archive.WriteDouble(((Transform)(ref _value))[i, j]);
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		Cone val = default(Cone);
		((Cone)(ref val))._002Ector(new Plane(new Point3d(1.3, 0.0, 0.0), Vector3d.YAxis, Vector3d.ZAxis), -2.5, 1.0);
		_meshArrow = Mesh.CreateFromCone(val, 1, 36);
	}

	private void BuildBendRotate()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Sphere val = default(Sphere);
		((Sphere)(ref val))._002Ector(Point3d.Origin, 2.0);
		_meshBendRotate = Mesh.CreateFromSphere(val, 20, 36);
	}

	private void BuildCube()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		List<Point3d> list = new List<Point3d>
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
		_meshCube = Mesh.CreateFromBox((IEnumerable<Point3d>)list, 1, 1, 1);
	}

	private void BuildRotate()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		_meshRotate = new Mesh();
		Mesh val = Mesh.CreateFromCone(new Cone(new Plane(new Point3d(0.5, 0.0, 5.0), Vector3d.XAxis, Vector3d.YAxis), -2.5, 1.0), 1, 36);
		Transform val2 = Transform.Rotation(-20.0 * CSBlockUtil.CSUtilMath.On_DEGREES_TO_RADIANS, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		((GeometryBase)val).Transform(val2);
		_meshRotate.Append(val);
		val = Mesh.CreateFromCone(new Cone(new Plane(new Point3d(0.5, 0.0, -5.0), Vector3d.XAxis, Vector3d.YAxis), 2.5, 1.0), 1, 36);
		val2 = Transform.Rotation(20.0 * CSBlockUtil.CSUtilMath.On_DEGREES_TO_RADIANS, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		((GeometryBase)val).Transform(val2);
		_meshRotate.Append(val);
		val = Mesh.CreateFromCylinder(new Cylinder(new Circle(new Plane(new Point3d(0.0, 0.0, -0.6), Vector3d.XAxis, Vector3d.YAxis), 0.5), 1.2), 1, 36);
		val2 = Transform.Rotation(-20.0 * CSBlockUtil.CSUtilMath.On_DEGREES_TO_RADIANS, new Vector3d(0.0, 1.0, 0.0), new Point3d(-6.0, 0.0, 0.0));
		((GeometryBase)val).Transform(val2);
		val2 = Transform.Rotation(10.0 * CSBlockUtil.CSUtilMath.On_DEGREES_TO_RADIANS, new Vector3d(0.0, 1.0, 0.0), new Point3d(-6.0, 0.0, 0.0));
		_meshRotate.Append(val);
		((GeometryBase)val).Transform(val2);
		_meshRotate.Append(val);
		((GeometryBase)val).Transform(val2);
		_meshRotate.Append(val);
		((GeometryBase)val).Transform(val2);
		_meshRotate.Append(val);
		((GeometryBase)val).Transform(val2);
		_meshRotate.Append(val);
	}

	private void BuildSphere()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Sphere val = default(Sphere);
		((Sphere)(ref val))._002Ector(Point3d.Origin, 1.0);
		_meshSphere = Mesh.CreateFromSphere(val, 20, 36);
	}

	private void CalculateScaleHandleSizes()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		if (!((BoundingBox)(ref _bb)).IsValid)
		{
			_scaleHandleLengthX = 5.0;
			_scaleHandleLengthY = 5.0;
			_scaleHandleLengthZ = 5.0;
			_scaleHandleLengthS = 10.0;
			return;
		}
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(((BoundingBox)(ref _bb)).Max);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(((Point3d)(ref val)).X, 0.0, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(0.0, ((Point3d)(ref val)).Y, 0.0);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(0.0, 0.0, ((Point3d)(ref val)).Z);
		((Point3d)(ref val2)).Transform(_value);
		((Point3d)(ref val3)).Transform(_value);
		((Point3d)(ref val4)).Transform(_value);
		((Point3d)(ref val)).Transform(_value);
		Vector3d val5 = val2 - BasePoint;
		_scaleHandleLengthX = ((Vector3d)(ref val5)).Length;
		val5 = val3 - BasePoint;
		_scaleHandleLengthY = ((Vector3d)(ref val5)).Length;
		val5 = val4 - BasePoint;
		_scaleHandleLengthZ = ((Vector3d)(ref val5)).Length;
		val5 = val - BasePoint;
		_scaleHandleLengthS = ((Vector3d)(ref val5)).Length;
	}

	private void DrawArrow(DisplayPipeline dp, Vector3d direction, Point3d baseLocation, Point3d location, double pixelPerUnit, Vector3d upVector, Color lineColor, WSelected ws, CSValueHandleHelper.WMode msrMode)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
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
		Vector3d val = default(Vector3d);
		if (((Vector3d)(ref upVector)).IsValid)
		{
			val = upVector;
		}
		else if (Math.Abs(direction * Vector3d.ZAxis) < 0.6)
		{
			val = Vector3d.CrossProduct(direction, Vector3d.ZAxis);
		}
		Vector3d val2 = Vector3d.CrossProduct(val, direction);
		val = Vector3d.CrossProduct(direction, val2);
		val *= 7.0;
		val2 *= 7.0;
		Vector3d val3 = direction * 7.0;
		val3 /= pixelPerUnit;
		val /= pixelPerUnit;
		val2 /= pixelPerUnit;
		DisplayMaterial val4 = new DisplayMaterial
		{
			Diffuse = lineColor,
			Transparency = (flag ? 0.2 : 0.9),
			Emission = Color.FromArgb(0, 0, 0),
			Ambient = Color.FromArgb(40, 40, 40)
		};
		Transform val5 = CSValueHandleHelper.ChangeBasis(location, val3, val2, val);
		Plane val6 = default(Plane);
		((Plane)(ref val6))._002Ector(baseLocation, location - baseLocation);
		Circle val7 = default(Circle);
		((Circle)(ref val7))._002Ector(val6, 2.0 / pixelPerUnit);
		Circle val8 = val7;
		Vector3d val9 = location - baseLocation;
		Mesh val10 = Mesh.CreateFromCylinder(new Cylinder(val8, ((Vector3d)(ref val9)).Length), 1, 36);
		dp.DrawMeshShaded(val10, val4);
		if (msrMode == CSValueHandleHelper.WMode.Rotate && flag)
		{
			Vector3d val11 = location - baseLocation;
			((Vector3d)(ref val11)).Unitize();
			Vector3d val12 = Vector3d.CrossProduct(val11, upVector);
			Plane val13 = default(Plane);
			((Plane)(ref val13))._002Ector(baseLocation, val12);
			Plane val14 = val13;
			val9 = location - baseLocation;
			Torus torus = default(Torus);
			((Torus)(ref torus))._002Ector(val14, ((Vector3d)(ref val9)).Length, 2.0 / pixelPerUnit);
			Mesh val15 = MeshTorus(torus, 90, 10);
			dp.DrawMeshShaded(val15, val4);
		}
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * val5;
		val4.Transparency = (flag ? 0.2 : 0.7);
		if (msrMode == CSValueHandleHelper.WMode.Rotate)
		{
			dp.DrawMeshShaded(_meshRotate, val4);
		}
		if (msrMode == CSValueHandleHelper.WMode.Move)
		{
			dp.DrawMeshShaded(_meshArrow, val4);
		}
		if (msrMode == CSValueHandleHelper.WMode.Scale)
		{
			dp.DrawMeshShaded(_meshSphere, val4);
		}
		dp.ModelTransform = modelTransform;
		val4.Dispose();
	}

	private void DrawBendBackGroundGrid(DisplayPipeline dp)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		if (((BoundingBox)(ref _bb)).IsValid && dp.Viewport.IsPerspectiveProjection)
		{
			Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(dp);
			((Vector3d)(ref cameraDirection)).Unitize();
			Point3d center = ((BoundingBox)(ref _bbOriginal)).Center;
			((Point3d)(ref center)).Transform(_value);
			Vector3d val = default(Vector3d);
			((Vector3d)(ref val))._002Ector(1.0, 0.0, 0.0);
			Vector3d val2 = default(Vector3d);
			((Vector3d)(ref val2))._002Ector(0.0, 1.0, 0.0);
			Vector3d val3 = default(Vector3d);
			((Vector3d)(ref val3))._002Ector(0.0, 0.0, 1.0);
			((Vector3d)(ref val)).Transform(_value);
			((Vector3d)(ref val)).Unitize();
			((Vector3d)(ref val2)).Transform(_value);
			((Vector3d)(ref val2)).Unitize();
			((Vector3d)(ref val3)).Transform(_value);
			((Vector3d)(ref val3)).Unitize();
			double num = Math.Abs(cameraDirection * val);
			double num2 = Math.Abs(cameraDirection * val2);
			double num3 = Math.Abs(cameraDirection * val3);
			Vector3d val4 = default(Vector3d);
			((Vector3d)(ref val4))._002Ector(1.0, 0.0, 0.0);
			Vector3d val5 = default(Vector3d);
			((Vector3d)(ref val5))._002Ector(1.0, 0.0, 0.0);
			double num4 = 0.0;
			if (num > num4)
			{
				val4 = val2;
				val5 = val3;
				num4 = num;
				_currentBend = _bendYz;
			}
			if (num2 > num4)
			{
				val4 = val;
				val5 = val3;
				num4 = num2;
				_currentBend = _bendXz;
			}
			if (num3 > num4)
			{
				val4 = val;
				val5 = val2;
				_currentBend = _bendXy;
			}
			Point3d val6 = default(Point3d);
			((Point3d)(ref val6))._002Ector(_currentBend.BasePoint);
			Point3d val7 = default(Point3d);
			((Point3d)(ref val7))._002Ector(_currentBend.EndPoint);
			Point3d val8 = default(Point3d);
			((Point3d)(ref val8))._002Ector(_currentBend.BendPoint);
			((Point3d)(ref val6)).Transform(_value);
			((Point3d)(ref val7)).Transform(_value);
			((Point3d)(ref val8)).Transform(_value);
			_pl0 = new Plane(val6, val4, val5);
			_pl1 = new Plane(val7, val4, val5);
			_pl2 = new Plane(val8, val4, val5);
		}
	}

	private void DrawBendControlPoints(DisplayPipeline dp)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		if (dp.Viewport.IsPerspectiveProjection)
		{
			double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(dp, BasePoint);
			Vector3d val = default(Vector3d);
			((Vector3d)(ref val))._002Ector(7.0 / worldToScreenScale, 0.0, 0.0);
			Vector3d initialBaseY = default(Vector3d);
			((Vector3d)(ref initialBaseY))._002Ector(0.0, 7.0 / worldToScreenScale, 0.0);
			Vector3d initialBaseZ = default(Vector3d);
			((Vector3d)(ref initialBaseZ))._002Ector(0.0, 0.0, 7.0 / worldToScreenScale);
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(_currentBend.BasePoint);
			Point3d val3 = default(Point3d);
			((Point3d)(ref val3))._002Ector(_currentBend.EndPoint);
			Point3d initialLocation = default(Point3d);
			((Point3d)(ref initialLocation))._002Ector(_currentBend.BendPoint);
			Vector3d val4 = val3 - val2;
			Point3d val5;
			if (_currentBend._bendMode > 0)
			{
				val5 = val2 - val4;
			}
			else
			{
				((Vector3d)(ref val4)).Unitize();
				val5 = val2 - val4 * 40.0 / worldToScreenScale;
			}
			((Point3d)(ref val2)).Transform(_value);
			((Point3d)(ref val3)).Transform(_value);
			((Point3d)(ref initialLocation)).Transform(_value);
			((Point3d)(ref val5)).Transform(_value);
			Arc arc = _currentBend.GetArc();
			NurbsCurve val6 = ((Arc)(ref arc)).ToNurbsCurve();
			((GeometryBase)val6).Transform(_value);
			Arc mirrorArc = _currentBend.GetMirrorArc();
			NurbsCurve val7 = ((Arc)(ref mirrorArc)).ToNurbsCurve();
			((GeometryBase)val7).Transform(_value);
			Transform val8 = CSValueHandleHelper.ChangeBasis(val2, val, initialBaseY, initialBaseZ);
			Transform val9 = CSValueHandleHelper.ChangeBasis(val3, val, initialBaseY, initialBaseZ);
			Transform val10 = CSValueHandleHelper.ChangeBasis(initialLocation, val, initialBaseY, initialBaseZ);
			Transform val11 = ((_currentBend._bendMode > 0) ? CSValueHandleHelper.ChangeBasis(val5, val, initialBaseY, initialBaseZ) : CSValueHandleHelper.ChangeBasis(val5, -val, initialBaseY, initialBaseZ));
			if (_meshSphere == null)
			{
				BuildSphere();
			}
			if (_meshBendRotate == null)
			{
				BuildBendRotate();
			}
			Transform modelTransform = dp.ModelTransform;
			dp.ModelTransform = modelTransform * val8;
			dp.DrawMeshShaded(_meshSphere, (MouseOver && _whichSelected == WSelected.Dp0) ? _matBendBasePointSelected : _matBendBasePoint);
			dp.ModelTransform = modelTransform * val9;
			dp.DrawMeshShaded(_meshSphere, (MouseOver && _whichSelected == WSelected.Dp1) ? _matBendEndPointSelected : _matBendEndPoint);
			dp.ModelTransform = modelTransform * val10;
			dp.DrawMeshShaded(_meshBendRotate, (MouseOver && _whichSelected == WSelected.Dp2) ? _matBendBendPointSelected : _matBendBendPoint);
			dp.ModelTransform = modelTransform * val11;
			dp.DrawMeshShaded(_meshArrow, (MouseOver && _whichSelected == WSelected.DpMirror) ? _matBendMirrorPointSelected : _matBendMirrorPoint);
			dp.ModelTransform = modelTransform;
			dp.DrawLine(val3, val5, Color.FromArgb(255, 160, 160, 160), 2);
			dp.DrawCurve((Curve)(object)val6, Color.FromArgb(255, 255, 144, 64));
			if (_currentBend._bendMode > 0)
			{
				dp.DrawCurve((Curve)(object)val7, Color.FromArgb(255, 255, 144, 64));
			}
		}
	}

	private void DrawModePicker(DisplayPipeline dp, Point3d location, double pixelPerUnit, Color pointColor)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		if (_meshSphere == null)
		{
			BuildSphere();
		}
		if (_meshCube == null)
		{
			BuildCube();
		}
		DisplayMaterial val = new DisplayMaterial
		{
			Diffuse = pointColor,
			Emission = Color.FromArgb(0, 0, 0),
			Ambient = Color.FromArgb(40, 40, 40)
		};
		Transform val2 = CSValueHandleHelper.ChangeBasis(location, UDirX / pixelPerUnit * 10.0, UDirY / pixelPerUnit * 10.0, UDirZ / pixelPerUnit * 10.0);
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * val2;
		dp.DrawMeshShaded((CSValueHandleHelper.CurrentHandle == this) ? _meshCube : _meshSphere, val);
		dp.ModelTransform = modelTransform;
		val.Dispose();
	}

	private Mesh MeshTorus(Torus torus, int majorAxis, int minorAxis)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		NurbsSurface val = ((Torus)(ref torus)).ToNurbsSurface();
		Mesh val2 = new Mesh();
		for (int i = 0; i <= majorAxis; i++)
		{
			for (int j = 0; j <= minorAxis; j++)
			{
				double num = (double)i / (double)majorAxis * 2.0 * Math.PI;
				double num2 = (double)j / (double)minorAxis * 2.0 * Math.PI;
				val2.Vertices.Add(((Surface)val).PointAt(num, num2));
				val2.Normals.Add(((Surface)val).NormalAt(num, num2));
			}
		}
		for (int k = 0; k <= majorAxis - 1; k++)
		{
			for (int l = 0; l <= minorAxis - 1; l++)
			{
				val2.Faces.AddFace(k * (minorAxis + 1) + l, (k + 1) * (minorAxis + 1) + l, (k + 1) * (minorAxis + 1) + l + 1, k * (minorAxis + 1) + l + 1);
			}
		}
		val2.Compact();
		return val2;
	}

	private static void SetDisplay()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Expected O, but got Unknown
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Expected O, but got Unknown
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Expected O, but got Unknown
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Expected O, but got Unknown
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Expected O, but got Unknown
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (!(newP0 == _currentBend.BasePoint))
		{
			PreChange("SetP0");
			_currentBend.BasePoint = newP0;
			Change();
		}
	}

	private void SetP1(Point3d newP1)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (!(newP1 == _currentBend.EndPoint))
		{
			PreChange("SetP1");
			_currentBend.EndPoint = new Point3d(newP1);
			Change();
		}
	}

	private void SetP2(Point3d newP2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (!(newValue == _value))
		{
			PreChange("SetValue");
			_value = newValue;
			Change();
		}
	}

	private void SetValueFromUI(Transform newValue, Point3d newP0, Point3d newP1, Point3d newP2, byte? bendMode, bool mouseDown)
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
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
				if (((Transform)(ref newValue)).IsValid)
				{
					SetValue(newValue);
				}
				if (((Point3d)(ref newP0)).IsValid)
				{
					SetP0(newP0);
				}
				if (((Point3d)(ref newP1)).IsValid)
				{
					SetP1(newP1);
				}
				if (((Point3d)(ref newP2)).IsValid)
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
				if (((Transform)(ref newValue)).IsValid)
				{
					SetValue(newValue);
				}
				if (((Point3d)(ref newP0)).IsValid)
				{
					SetP0(newP0);
				}
				if (((Point3d)(ref newP1)).IsValid)
				{
					SetP1(newP1);
				}
				if (((Point3d)(ref newP2)).IsValid)
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
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
		Point3d val = bp + d0 * num;
		if (!(Math.Abs(Math.Abs(d0 * worldLineDirection)) < 0.9))
		{
			return false;
		}
		Point3d val2 = ((Line)(ref mouseLine)).ClosestPoint(val, false);
		MouseOver = ((Point3d)(ref val2)).DistanceTo(val) < 10.0 / worldToScreenScale;
		if (!MouseOver)
		{
			return false;
		}
		((Vector3d)(ref d0)).Unitize();
		((Vector3d)(ref d1)).Unitize();
		_whichSelected = selId;
		if (msrMode == CSValueHandleHelper.WMode.Rotate)
		{
			_hitPlane = new Plane(bp, d0, d1);
		}
		if (msrMode == CSValueHandleHelper.WMode.Move)
		{
			Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(vp);
			Vector3d val3 = Vector3d.CrossProduct(d0, cameraDirection);
			((Vector3d)(ref val3)).Unitize();
			_hitPlane = new Plane(val, d0, val3);
		}
		if (msrMode == CSValueHandleHelper.WMode.Scale)
		{
			Vector3d cameraDirection2 = CSValueHandleHelper.GetCameraDirection(vp);
			Vector3d val4 = Vector3d.CrossProduct(d0, cameraDirection2);
			((Vector3d)(ref val4)).Unitize();
			_hitPlane = new Plane(bp, d0, val4);
		}
		return true;
	}

	private bool TestHitSphere(Line mouseLine, RhinoViewport vp, Point3d bp, WSelected selId)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(vp, bp);
		Point3d val = ((Line)(ref mouseLine)).ClosestPoint(bp, false);
		MouseOver = ((Point3d)(ref val)).DistanceTo(bp) < 10.0 / worldToScreenScale;
		if (!MouseOver)
		{
			return false;
		}
		_whichSelected = selId;
		return true;
	}
}
