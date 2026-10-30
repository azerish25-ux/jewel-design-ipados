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
using Rhino.Runtime;
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

	public bool MouseOver { get; set; }

	public bool MouseDown { get; set; }

	public CSValueXFormObject1(string identifier)
		: base(identifier)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		InObjects = new CSInputChannel(GetPropertyName(() => InObjects), this, ChannelDataType.Any, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InOnOff = new CSInputChannel(GetPropertyName(() => InOnOff), this, ChannelDataType.Boolean, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		OutTransformedObjects = new CSOutputChannel(GetPropertyName(() => OutTransformedObjects), this, ChannelDataType.Any, ChannelArrayType.Any);
		OutValue = new CSOutputChannel(GetPropertyName(() => OutValue), this, ChannelDataType.Transform, ChannelArrayType.Individual);
	}

	public CSValueXFormObject1(string identifier, Transform value)
		: base(identifier, value)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
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
			Point3d bp = default(Point3d);
			((Point3d)(ref bp))._002Ector(_p0);
			Point3d bp2 = default(Point3d);
			((Point3d)(ref bp2))._002Ector(_p1);
			Point3d bp3 = default(Point3d);
			((Point3d)(ref bp3))._002Ector(_p2);
			((Point3d)(ref bp)).Transform(_value);
			((Point3d)(ref bp2)).Transform(_value);
			((Point3d)(ref bp3)).Transform(_value);
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
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
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
				SetValueFromUI(Transform.Unset, newP, Point3d.Unset, Point3d.Unset, mouseDown: true);
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
				SetValueFromUI(Transform.Unset, Point3d.Unset, newP2, Point3d.Unset, mouseDown: true);
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
				SetValueFromUI(Transform.Unset, Point3d.Unset, Point3d.Unset, newP3, mouseDown: true);
				return true;
			}
			return false;
		}
		if (_whichSelected == WSelected.PCenter)
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
			double num3 = CSValueHandleHelper.AngleOfPointD(x, num2);
			if (_whichSelected == WSelected.Rx)
			{
				val5 = Transform.Rotation(num3, -UDirZ, BasePoint);
			}
			if (_whichSelected == WSelected.Ry)
			{
				val5 = Transform.Rotation(num3, -UDirX, BasePoint);
			}
			if (_whichSelected == WSelected.Rz)
			{
				val5 = Transform.Rotation(num3, -UDirY, BasePoint);
			}
			double num4 = num2;
			if (_whichSelected == WSelected.Mx)
			{
				val5 = Transform.Translation(UDirX * num4);
			}
			if (_whichSelected == WSelected.My)
			{
				val5 = Transform.Translation(UDirY * num4);
			}
			if (_whichSelected == WSelected.Mz)
			{
				val5 = Transform.Translation(UDirZ * num4);
			}
			((Plane)(ref _hitPlane)).Transform(val5);
			val5 *= _value;
			if (num4 < 0.1)
			{
				num4 = 0.1;
			}
			if (_whichSelected == WSelected.Sx)
			{
				((Transform)(ref val5))[0, 0] = ((Transform)(ref val5))[0, 0] / _scaleHandleLengthX * num4;
				((Transform)(ref val5))[1, 0] = ((Transform)(ref val5))[1, 0] / _scaleHandleLengthX * num4;
				((Transform)(ref val5))[2, 0] = ((Transform)(ref val5))[2, 0] / _scaleHandleLengthX * num4;
				_scaleHandleLengthX = num4;
			}
			if (_whichSelected == WSelected.Sy)
			{
				((Transform)(ref val5))[0, 1] = ((Transform)(ref val5))[0, 1] / _scaleHandleLengthY * num4;
				((Transform)(ref val5))[1, 1] = ((Transform)(ref val5))[1, 1] / _scaleHandleLengthY * num4;
				((Transform)(ref val5))[2, 1] = ((Transform)(ref val5))[2, 1] / _scaleHandleLengthY * num4;
				_scaleHandleLengthY = num4;
			}
			if (_whichSelected == WSelected.Sz)
			{
				((Transform)(ref val5))[0, 2] = ((Transform)(ref val5))[0, 2] / _scaleHandleLengthZ * num4;
				((Transform)(ref val5))[1, 2] = ((Transform)(ref val5))[1, 2] / _scaleHandleLengthZ * num4;
				((Transform)(ref val5))[2, 2] = ((Transform)(ref val5))[2, 2] / _scaleHandleLengthZ * num4;
				_scaleHandleLengthZ = num4;
			}
			if (_whichSelected == WSelected.Ss)
			{
				double num5 = num4 / _scaleHandleLengthS;
				((Transform)(ref val5))[0, 0] = ((Transform)(ref val5))[0, 0] * num5;
				((Transform)(ref val5))[1, 0] = ((Transform)(ref val5))[1, 0] * num5;
				((Transform)(ref val5))[2, 0] = ((Transform)(ref val5))[2, 0] * num5;
				((Transform)(ref val5))[0, 1] = ((Transform)(ref val5))[0, 1] * num5;
				((Transform)(ref val5))[1, 1] = ((Transform)(ref val5))[1, 1] * num5;
				((Transform)(ref val5))[2, 1] = ((Transform)(ref val5))[2, 1] * num5;
				((Transform)(ref val5))[0, 2] = ((Transform)(ref val5))[0, 2] * num5;
				((Transform)(ref val5))[1, 2] = ((Transform)(ref val5))[1, 2] * num5;
				((Transform)(ref val5))[2, 2] = ((Transform)(ref val5))[2, 2] * num5;
				_scaleHandleLengthS = num4;
			}
			SetValueFromUI(val5, Point3d.Unset, Point3d.Unset, Point3d.Unset, mouseDown: true);
			return false;
		}
		return false;
	}

	public void MouseUp(MouseCallbackEventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
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
				double num = 0.0;
				archive.ReadDouble(ref num);
				((Transform)(ref _value))[i, j] = num;
			}
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
			_value = ((CSValueXFormObject1)setObject)._value;
		}
		return true;
	}

	public void SetLocation(double x, double y, double z)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		_value = Transform.Translation(x, y, z);
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
		_value = ((CSValueXFormObject1)setObject)._value;
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		bool num = BeenUpdatedAfterLastUpdate(InObjects.OutputChannelBlock) | (_obj == null);
		Vector3d val = _p0 - _p0A;
		bool num2 = num | (((Vector3d)(ref val)).Length > 0.001);
		val = _p1 - _p1A;
		bool num3 = num2 | (((Vector3d)(ref val)).Length > 0.001);
		val = _p2 - _p2A;
		if (num3 | (((Vector3d)(ref val)).Length > 0.001))
		{
			_objOriginal = InObjects.Data.Duplicate();
			_obj = _objOriginal.Duplicate();
			Vector3d inputVector = _p1 - _p0;
			Vector3d inputVector2 = _p2 - _p0;
			if (CSValueHandleHelper.AngleBetweenVectors(inputVector, inputVector2) > Math.PI / 180.0)
			{
				Point3d p = new Point3d(_p0);
				Point3d p2 = default(Point3d);
				((Point3d)(ref p2))._002Ector(_p1);
				CSBendMorph1 obj = new CSBendMorph1(arc: _arc, p0: p, p1: p2);
				((SpaceMorph)obj).Tolerance = 0.05;
				CSBendMorph1 spaceMorph = obj;
				_obj.Morph((SpaceMorph)(object)spaceMorph);
			}
			_obj.GetBoundingBox(ref _bb);
			if (!((BoundingBox)(ref _bb)).IsValid)
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
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
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
				archive.WriteDouble(((Transform)(ref _value))[i, j]);
			}
		}
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
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		if (_meshArrow == null)
		{
			BuildArrow();
		}
		if (_meshRotate == null)
		{
			BuildRotate();
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
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * val5;
		val4.Transparency = (flag ? 0.0 : 0.4);
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
	}

	private void DrawBendBackGroundGrid(DisplayPipeline dp)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		if (!((BoundingBox)(ref _bb)).IsValid)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(dp);
		((Vector3d)(ref cameraDirection)).Unitize();
		Point3d center = ((BoundingBox)(ref _bb)).Center;
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
		double num3 = Math.Abs(cameraDirection * val);
		double num4 = Math.Abs(cameraDirection * val2);
		double num5 = Math.Abs(cameraDirection * val3);
		Vector3d val4 = default(Vector3d);
		((Vector3d)(ref val4))._002Ector(1.0, 0.0, 0.0);
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(1.0, 0.0, 0.0);
		double num6 = 0.0;
		Vector3d diagonal;
		if (num3 > num6)
		{
			val4 = val2;
			val5 = val3;
			num6 = num3;
			diagonal = ((BoundingBox)(ref _bb)).Diagonal;
			num = (int)(((Vector3d)(ref diagonal)).Y / 2.0 + 3.0);
			diagonal = ((BoundingBox)(ref _bb)).Diagonal;
			num2 = (int)(((Vector3d)(ref diagonal)).Z / 2.0 + 3.0);
		}
		if (num4 > num6)
		{
			val4 = val;
			val5 = val3;
			num6 = num4;
			diagonal = ((BoundingBox)(ref _bb)).Diagonal;
			num = (int)(((Vector3d)(ref diagonal)).X / 2.0 + 3.0);
			diagonal = ((BoundingBox)(ref _bb)).Diagonal;
			num2 = (int)(((Vector3d)(ref diagonal)).Z / 2.0 + 3.0);
		}
		if (num5 > num6)
		{
			val4 = val;
			val5 = val2;
			diagonal = ((BoundingBox)(ref _bb)).Diagonal;
			num = (int)(((Vector3d)(ref diagonal)).X / 2.0 + 3.0);
			diagonal = ((BoundingBox)(ref _bb)).Diagonal;
			num2 = (int)(((Vector3d)(ref diagonal)).Y / 2.0 + 3.0);
		}
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector(_p0);
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector(_p1);
		Point3d val8 = default(Point3d);
		((Point3d)(ref val8))._002Ector(_p2);
		((Point3d)(ref val6)).Transform(_value);
		((Point3d)(ref val7)).Transform(_value);
		((Point3d)(ref val8)).Transform(_value);
		_pl0 = new Plane(val6, val4, val5);
		_pl1 = new Plane(val7, val4, val5);
		_pl2 = new Plane(val8, val4, val5);
		if (_objOriginal is CSOnGeometryData cSOnGeometryData)
		{
			DisplayMaterial val9 = new DisplayMaterial
			{
				Diffuse = Color.FromArgb(255, 64, 128, 128),
				Transparency = 0.8,
				Emission = Color.FromArgb(0, 0, 0),
				Ambient = Color.FromArgb(40, 40, 40)
			};
			Mesh mesh = cSOnGeometryData.GetMesh();
			Transform modelTransform = dp.ModelTransform;
			dp.ModelTransform = modelTransform * _value;
			dp.DrawMeshShaded(mesh, val9);
			val9.Dispose();
			dp.ModelTransform = modelTransform;
		}
		for (int i = 0; i <= num; i++)
		{
			Color color = Color.FromArgb(255, 208, 208, 208);
			if (i % 5 == 0 || i == num)
			{
				color = Color.FromArgb(255, 160, 160, 160);
			}
			dp.DrawLine(new Point3d(val4 * (double)i - val5 * (double)num2 + center), new Point3d(val4 * (double)i + val5 * (double)num2 + center), color, 1);
			dp.DrawLine(new Point3d(-(val4 * (double)i) - val5 * (double)num2 + center), new Point3d(-(val4 * (double)i) + val5 * (double)num2 + center), color, 1);
		}
		for (int j = 0; j <= num2; j++)
		{
			Color color2 = Color.FromArgb(255, 208, 208, 208);
			if (j % 5 == 0 || j == num2)
			{
				color2 = Color.FromArgb(255, 160, 160, 160);
			}
			dp.DrawLine(new Point3d(val5 * (double)j - val4 * (double)num + center), new Point3d(val5 * (double)j + val4 * (double)num + center), color2, 1);
			dp.DrawLine(new Point3d(-(val5 * (double)j) - val4 * (double)num + center), new Point3d(-(val5 * (double)j) + val4 * (double)num + center), color2, 1);
		}
		dp.DrawLine(center, center + val4 * (double)num, Color.FromArgb(255, 128, 255, 128), 2);
		dp.DrawLine(center, center + val5 * (double)num2, Color.FromArgb(255, 255, 128, 128), 2);
	}

	private void DrawBendControlPoints(DisplayPipeline dp)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		DisplayMaterial val = new DisplayMaterial
		{
			Diffuse = Color.FromArgb(255, 128, 128, 255),
			Transparency = 0.6,
			Emission = Color.FromArgb(0, 0, 0),
			Ambient = Color.FromArgb(40, 40, 40)
		};
		DisplayMaterial val2 = new DisplayMaterial
		{
			Diffuse = Color.FromArgb(255, 128, 128, 255),
			Transparency = 0.2,
			Emission = Color.FromArgb(0, 0, 0),
			Ambient = Color.FromArgb(40, 40, 40)
		};
		double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(dp, BasePoint);
		Vector3d initialBaseX = default(Vector3d);
		((Vector3d)(ref initialBaseX))._002Ector(7.0 / worldToScreenScale, 0.0, 0.0);
		Vector3d initialBaseY = default(Vector3d);
		((Vector3d)(ref initialBaseY))._002Ector(0.0, 7.0 / worldToScreenScale, 0.0);
		Vector3d initialBaseZ = default(Vector3d);
		((Vector3d)(ref initialBaseZ))._002Ector(0.0, 0.0, 7.0 / worldToScreenScale);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(_p0);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(_p1);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(_p2);
		((Point3d)(ref val3)).Transform(_value);
		((Point3d)(ref val4)).Transform(_value);
		((Point3d)(ref val5)).Transform(_value);
		Transform val6 = CSValueHandleHelper.ChangeBasis(val3, initialBaseX, initialBaseY, initialBaseZ);
		Transform val7 = CSValueHandleHelper.ChangeBasis(val4, initialBaseX, initialBaseY, initialBaseZ);
		Transform val8 = CSValueHandleHelper.ChangeBasis(val5, initialBaseX, initialBaseY, initialBaseZ);
		if (_meshSphere == null)
		{
			BuildSphere();
		}
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * val6;
		dp.DrawMeshShaded(_meshSphere, (MouseOver && _whichSelected == WSelected.Dp0) ? val2 : val);
		dp.ModelTransform = modelTransform * val7;
		dp.DrawMeshShaded(_meshSphere, (MouseOver && _whichSelected == WSelected.Dp1) ? val2 : val);
		dp.ModelTransform = modelTransform * val8;
		dp.DrawMeshShaded(_meshSphere, (MouseOver && _whichSelected == WSelected.Dp2) ? val2 : val);
		dp.ModelTransform = modelTransform;
		dp.DrawLine(val3, val4, Color.FromArgb(255, 0, 0, 160), 2);
		Vector3d val9 = default(Vector3d);
		((Vector3d)(ref val9))._002Ector(val4 - val3);
		((Vector3d)(ref val9)).Unitize();
		Vector3d val10 = default(Vector3d);
		((Vector3d)(ref val10))._002Ector(val5 - val3);
		((Vector3d)(ref val10)).Unitize();
		Vector3d val11 = Vector3d.CrossProduct(val9, val10);
		((Vector3d)(ref val11)).Unitize();
		Vector3d val12 = Vector3d.CrossProduct(val11, val9);
		((Vector3d)(ref val12)).Unitize();
		Vector3d val13 = default(Vector3d);
		((Vector3d)(ref val13))._002Ector(val5 - val3);
		((Vector3d)(ref val13)).Unitize();
		Vector3d val14 = Vector3d.CrossProduct(val11, val13);
		((Vector3d)(ref val14)).Unitize();
		double a = CSValueHandleHelper.AngleBetweenVectors(val14, val12);
		Vector3d val15 = val5 - val3;
		double num = ((Vector3d)(ref val15)).Length / 2.0 / Math.Sin(a);
		Point3d val16 = val3 + val12 * num - val14 * num;
		_arc = new Arc(val3, val16, val5);
		dp.DrawArc(_arc, Color.FromArgb(255));
		val.Dispose();
		val2.Dispose();
	}

	private void DrawBoundingBox(DisplayPipeline displayPipeline)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		if (((BoundingBox)(ref _bb)).IsValid)
		{
			Point3d min = ((BoundingBox)(ref _bb)).Min;
			Point3d max = ((BoundingBox)(ref _bb)).Max;
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(((Point3d)(ref min)).X, ((Point3d)(ref min)).Y, ((Point3d)(ref min)).Z);
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(((Point3d)(ref min)).X, ((Point3d)(ref max)).Y, ((Point3d)(ref min)).Z);
			Point3d val3 = default(Point3d);
			((Point3d)(ref val3))._002Ector(((Point3d)(ref max)).X, ((Point3d)(ref max)).Y, ((Point3d)(ref min)).Z);
			Point3d val4 = default(Point3d);
			((Point3d)(ref val4))._002Ector(((Point3d)(ref max)).X, ((Point3d)(ref min)).Y, ((Point3d)(ref min)).Z);
			Point3d val5 = default(Point3d);
			((Point3d)(ref val5))._002Ector(((Point3d)(ref min)).X, ((Point3d)(ref min)).Y, ((Point3d)(ref max)).Z);
			Point3d val6 = default(Point3d);
			((Point3d)(ref val6))._002Ector(((Point3d)(ref min)).X, ((Point3d)(ref max)).Y, ((Point3d)(ref max)).Z);
			Point3d val7 = default(Point3d);
			((Point3d)(ref val7))._002Ector(((Point3d)(ref max)).X, ((Point3d)(ref max)).Y, ((Point3d)(ref max)).Z);
			Point3d val8 = default(Point3d);
			((Point3d)(ref val8))._002Ector(((Point3d)(ref max)).X, ((Point3d)(ref min)).Y, ((Point3d)(ref max)).Z);
			((Point3d)(ref val)).Transform(_value);
			((Point3d)(ref val2)).Transform(_value);
			((Point3d)(ref val3)).Transform(_value);
			((Point3d)(ref val4)).Transform(_value);
			((Point3d)(ref val5)).Transform(_value);
			((Point3d)(ref val6)).Transform(_value);
			((Point3d)(ref val7)).Transform(_value);
			((Point3d)(ref val8)).Transform(_value);
			Color color = Color.FromArgb(255, 192, 192, 192);
			displayPipeline.DrawLine(val, val4, color, 1);
			displayPipeline.DrawLine(val2, val3, color, 1);
			displayPipeline.DrawLine(val5, val8, color, 1);
			displayPipeline.DrawLine(val6, val7, color, 1);
			displayPipeline.DrawLine(val, val2, color, 1);
			displayPipeline.DrawLine(val4, val3, color, 1);
			displayPipeline.DrawLine(val5, val6, color, 1);
			displayPipeline.DrawLine(val8, val7, color, 1);
			displayPipeline.DrawLine(val, val5, color, 1);
			displayPipeline.DrawLine(val2, val6, color, 1);
			displayPipeline.DrawLine(val3, val7, color, 1);
			displayPipeline.DrawLine(val4, val8, color, 1);
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
	}

	private void SetP0(Point3d newP0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (!(newP0 == _p0))
		{
			PreChange("SetP0");
			_p0 = new Point3d(newP0);
			Change();
		}
	}

	private void SetP1(Point3d newP1)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (!(newP1 == _p1))
		{
			PreChange("SetP1");
			_p1 = new Point3d(newP1);
			Change();
		}
	}

	private void SetP2(Point3d newP2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (!(newP2 == _p2))
		{
			PreChange("SetP2");
			_p2 = new Point3d(newP2);
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

	private void SetValueFromUI(Transform newValue, Point3d newP0, Point3d newP1, Point3d newP2, bool mouseDown)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		if ((mouseDown && ((Transform)(ref newValue)).IsValid && _value == newValue) || (mouseDown && ((Point3d)(ref newP0)).IsValid && _p0 == newP0) || (mouseDown && ((Point3d)(ref newP1)).IsValid && _p1 == newP1) || (mouseDown && ((Point3d)(ref newP2)).IsValid && _p2 == newP2))
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
			}
			else
			{
				CSUtil.RemoveMouseDown();
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
