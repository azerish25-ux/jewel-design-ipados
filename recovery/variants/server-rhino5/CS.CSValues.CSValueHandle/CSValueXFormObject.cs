using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using CS.CSObjects;
using CS.CSValues.CSValueHandle.HandleSupportClasses;
using CS.CSValues.CSValueHandle.Interfaces;
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

namespace CS.CSValues.CSValueHandle;

[CSBlock("ValueXFormObject", "{D69D90FF-1911-4469-92BC-97495236C8DA}")]
public sealed class CSValueXFormObject : CSValueXFormBase, ICSVHandle
{
	private enum EditMode
	{
		None,
		MSR,
		Bend,
		Array
	}

	private enum ArrayShape
	{
		Round,
		Cushion,
		Oval,
		Emerald,
		Asscher,
		Heart,
		Marquise,
		Pear,
		Square
	}

	private sealed class TxtBox
	{
		private readonly Color _txtBoxColor;

		private readonly Point3d _txtBoxPoint;

		private readonly string _txtBoxText;

		public TxtBox(Point3d txtBoxPoint, string txtBoxText, Color txtBoxColor)
		{
			_txtBoxPoint = txtBoxPoint;
			_txtBoxText = txtBoxText;
			_txtBoxColor = txtBoxColor;
		}

		public void Drawtxt(DisplayPipeline displayPipeline)
		{
			Transform xform = CSValueHandleHelper.WorldToScreen(displayPipeline);
			Point3d point3d = new Point3d(_txtBoxPoint);
			point3d.Transform(xform);
			Rectangle rectangle = displayPipeline.Measure2dText(_txtBoxText, new Point2d(0.0, 0.0), middleJustified: false, 0.0, 12, "Arial");
			using (Bitmap bitmap = new Bitmap(Math.Abs(rectangle.Width) + 8, Math.Abs(rectangle.Height) + 4, PixelFormat.Format32bppArgb))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					using (Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 1)))
					{
						graphics.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);
					}
					using Pen pen = new Pen(_txtBoxColor, 2f);
					graphics.DrawRectangle(pen, 1, 1, bitmap.Width - 2, bitmap.Height - 2);
				}
				using DisplayBitmap bitmap2 = new DisplayBitmap(bitmap);
				displayPipeline.DrawBitmap(bitmap2, (int)(point3d.X - (double)bitmap.Width / 2.0), (int)(point3d.Y - (double)bitmap.Height / 2.0 + 4.0));
			}
			displayPipeline.Draw2dText(_txtBoxText, Color.White, new Point2d(point3d.X - (double)rectangle.Width / 2.0, point3d.Y + (double)rectangle.Height / 2.0 + 2.0), middleJustified: false, 12, "Arial");
		}
	}

	private const double _bottomAngle = Math.PI * 179.0 / 180.0;

	private const double _topAngle = 6.2482787221397;

	private CSValueHandleHelper.ArrayDirection _arrayDirection;

	private bool _arrayMirror;

	private ArrayShape _arrayShape;

	private double _arrayRepeatAngle = Math.PI / 8.0;

	private double _arrayTotalAngle = Math.PI / 2.0;

	private BoundingBox _bb;

	private BoundingBox _bbOriginal;

	private bool _bendNeedsRefresh;

	private CSBendPoints _bendXy;

	private CSBendPoints _bendXz;

	private CSBendPoints _bendYz;

	private EditMode _editMode = EditMode.MSR;

	private CSData _objMeshOriginal;

	private CSData _objOriginal;

	private CSMaterials.MaterialGroup _objOriginalMaterialGroup;

	private CSData _objOutput;

	private int _outputFlipMode;

	private int _outputMirrorMode;

	private Transform _arrayObjectTransform;

	private Transform _arrayObjectTransformInv;

	private CSValueHandleHelper.WSelected _whichSelected;

	private CSValueHandleHelper.WSelected _whichSelectedLast;

	private List<TxtBox> _lsttxtBox;

	private Plane _hitPlane;

	private double _rotateAngle;

	private bool _mouseDown;

	public CSInputChannel InObjects { get; }

	public CSInputChannel InOnOff { get; }

	public CSInputChannel InAutoCenter { get; }

	public CSOutputChannel OutTransformedObjects { get; }

	public CSOutputChannel OutTransformedObjects1 { get; }

	public CSOutputChannel OutTransformedObjects2 { get; }

	public CSOutputChannel OutTransformedObjects3 { get; }

	public CSOutputChannel OutValue { get; }

	public CSOutputChannel OutValue1 { get; }

	public int FlipMode => _outputFlipMode;

	public int MirrorMode => _outputMirrorMode;

	private bool IsOn
	{
		get
		{
			bool value = true;
			InOnOff.GetBoolean(ref value);
			if (value)
			{
				return CSApplicationHelper.Application.Conduit.IsViewportHandlesVisible;
			}
			return false;
		}
	}

	private Point3d EPoint => new Point3d(_arrayObjectTransform[0, 3], _arrayObjectTransform[1, 3], _arrayObjectTransform[2, 3]);

	private Vector3d EDirX
	{
		get
		{
			Vector3d result = new Vector3d(_arrayObjectTransform[0, 0], _arrayObjectTransform[1, 0], _arrayObjectTransform[2, 0]);
			result.Unitize();
			return result;
		}
	}

	private Vector3d EDirY
	{
		get
		{
			Vector3d result = new Vector3d(_arrayObjectTransform[0, 1], _arrayObjectTransform[1, 1], _arrayObjectTransform[2, 1]);
			result.Unitize();
			return result;
		}
	}

	private Vector3d EDirZ
	{
		get
		{
			Vector3d result = new Vector3d(_arrayObjectTransform[0, 2], _arrayObjectTransform[1, 2], _arrayObjectTransform[2, 2]);
			result.Unitize();
			return result;
		}
	}

	private Point3d BasePoint => new Point3d(ObjectTransform[0, 3], ObjectTransform[1, 3], ObjectTransform[2, 3]);

	private Vector3d UDirX
	{
		get
		{
			Vector3d result = new Vector3d(ObjectTransform[0, 0], ObjectTransform[1, 0], ObjectTransform[2, 0]);
			result.Unitize();
			return result;
		}
	}

	private Vector3d UDirY
	{
		get
		{
			Vector3d result = new Vector3d(ObjectTransform[0, 1], ObjectTransform[1, 1], ObjectTransform[2, 1]);
			result.Unitize();
			return result;
		}
	}

	private Vector3d UDirZ
	{
		get
		{
			Vector3d result = new Vector3d(ObjectTransform[0, 2], ObjectTransform[1, 2], ObjectTransform[2, 2]);
			result.Unitize();
			return result;
		}
	}

	private Vector3d UDirS
	{
		get
		{
			Point3d point3d = new Point3d(_bb.Max);
			point3d.Transform(ObjectTransform);
			Vector3d result = new Vector3d(point3d - BasePoint);
			result.Unitize();
			return result;
		}
	}

	private Point3d UDirP
	{
		get
		{
			if (!_bb.IsValid)
			{
				Point3d origin = Point3d.Origin;
				origin.Transform(ObjectTransform);
				return origin;
			}
			Point3d result = new Point3d(_bb.Min.X, _bb.Max.Y, _bb.Max.Z);
			result *= 0.5;
			if (result.X > -1.0)
			{
				result.X = -1.0;
			}
			if (result.Y < 1.0)
			{
				result.Y = 1.0;
			}
			if (result.Z < 1.0)
			{
				result.Z = 1.0;
			}
			result.Transform(ObjectTransform);
			return result;
		}
	}

	private double HandleLengthX
	{
		get
		{
			if (!_bb.IsValid)
			{
				return 5.0;
			}
			Point3d point3d = new Point3d(_bb.Max.X, 0.0, 0.0);
			point3d.Transform(ObjectTransform);
			return (point3d - BasePoint).Length;
		}
	}

	private double HandleLengthY
	{
		get
		{
			if (!_bb.IsValid)
			{
				return 5.0;
			}
			Point3d point3d = new Point3d(0.0, _bb.Max.Y, 0.0);
			point3d.Transform(ObjectTransform);
			return (point3d - BasePoint).Length;
		}
	}

	private double HandleLengthZ
	{
		get
		{
			if (!_bb.IsValid)
			{
				return 5.0;
			}
			Point3d point3d = new Point3d(0.0, 0.0, _bb.Max.Z);
			point3d.Transform(ObjectTransform);
			return (point3d - BasePoint).Length;
		}
	}

	private double HandleLengthS
	{
		get
		{
			if (!_bb.IsValid)
			{
				return 5.0;
			}
			Point3d point3d = new Point3d(_bb.Max);
			point3d.Transform(ObjectTransform);
			return (point3d - BasePoint).Length;
		}
	}

	private double HandleLengthXMin
	{
		get
		{
			if (!_bb.IsValid)
			{
				return 5.0;
			}
			Point3d point3d = new Point3d(_bb.Min.X, 0.0, 0.0);
			point3d.Transform(ObjectTransform);
			return (point3d - BasePoint).Length;
		}
	}

	private double HandleLengthYMin
	{
		get
		{
			if (!_bb.IsValid)
			{
				return 5.0;
			}
			Point3d point3d = new Point3d(0.0, _bb.Min.Y, 0.0);
			point3d.Transform(ObjectTransform);
			return (point3d - BasePoint).Length;
		}
	}

	private double HandleLengthZMin
	{
		get
		{
			if (!_bb.IsValid)
			{
				return 5.0;
			}
			Point3d point3d = new Point3d(0.0, 0.0, _bb.Min.Z);
			point3d.Transform(ObjectTransform);
			return (point3d - BasePoint).Length;
		}
	}

	public bool IsFreehandCommonCenterModeEnabled { get; set; }

	private Transform ObjectTransform
	{
		get
		{
			if (_arrayObjectTransform.IsValid)
			{
				return _arrayObjectTransform * _value;
			}
			return _value;
		}
		set
		{
			if (!_arrayObjectTransform.IsValid)
			{
				_value = value;
			}
			else
			{
				_value = _arrayObjectTransformInv * value;
			}
		}
	}

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

	public bool MouseDown
	{
		get
		{
			return _mouseDown;
		}
		set
		{
			if (_mouseDown != value)
			{
				_mouseDown = value;
				if (_mouseDown)
				{
					PreSet();
				}
				else
				{
					PostSet(isMoving: false, isQuickRegen: false);
				}
			}
		}
	}

	private int ObjSizeX => (int)(_bbOriginal.Diagonal.X * ObjectTransform[0, 0] / 2.0 + 3.0);

	private int ObjSizeY => (int)(_bbOriginal.Diagonal.Y * ObjectTransform[1, 1] / 2.0 + 3.0);

	private int ObjSizeZ => (int)(_bbOriginal.Diagonal.Z * ObjectTransform[2, 2] / 2.0 + 3.0);

	public CSValueXFormObject(string identifier)
		: base(identifier)
	{
		_value = Transform.Identity;
		_arrayObjectTransform = Transform.Identity;
		_arrayObjectTransformInv = Transform.Identity;
		_bb = new BoundingBox(new Point3d(-5.0, -5.0, -5.0), new Point3d(5.0, 5.0, 5.0));
		_bendNeedsRefresh = true;
		InObjects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InOnOff = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InAutoCenter = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Any, 0);
		OutTransformedObjects = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutTransformedObjects1 = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutTransformedObjects2 = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutTransformedObjects3 = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutValue = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutValue1 = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
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
		OutValue.SetData(new CSXformData(ObjectTransform));
		OutValue1.SetData(new CSXformData(_arrayObjectTransform));
		bool flag = BeenUpdatedAfterLastUpdate(InObjects.OutputChannelBlock);
		flag |= BeenUpdatedAfterLastUpdate(InAutoCenter.OutputChannelBlock);
		if (flag || _objOriginal == null)
		{
			_bendNeedsRefresh = true;
			_objOriginal = InObjects.Data;
			if (_objOriginal == null)
			{
				_objMeshOriginal = null;
				_bbOriginal = new BoundingBox(new Point3d(-5.0, -5.0, -5.0), new Point3d(5.0, 5.0, 5.0));
				return base.Update();
			}
			_objOriginal = _objOriginal.Duplicate();
			CSDataArray cSDataArray = _objOriginal as CSDataArray;
			_bbOriginal = BoundingBox.Unset;
			_objOriginalMaterialGroup = MaterialGroupHelper.GetMaterialGroupByClassification(MaterialClassification.Undefined).FirstOrDefault();
			if (cSDataArray != null)
			{
				cSDataArray.GetFlattenedList().ForEach(delegate(CSData _)
				{
					BoundingBox boundingBox = BoundingBox.Unset;
					if (_.GetBoundingBox(ref boundingBox))
					{
						_bbOriginal.Union(boundingBox);
					}
					if (_.CurrentMaterialGroup.Classification != MaterialClassification.Undefined)
					{
						_objOriginalMaterialGroup = _.CurrentMaterialGroup;
					}
				});
			}
			else
			{
				_objOriginal.GetBoundingBox(ref _bbOriginal);
				_objOriginalMaterialGroup = _objOriginal.CurrentMaterialGroup;
			}
			if (!_bbOriginal.IsValid)
			{
				_objOriginal = null;
				_objMeshOriginal = null;
				_bbOriginal = new BoundingBox(new Point3d(-5.0, -5.0, -5.0), new Point3d(5.0, 5.0, 5.0));
				return base.Update();
			}
			int value = 0;
			InAutoCenter.GetInteger(ref value);
			switch (value)
			{
			case 1:
				if (_objOriginal is ICSTransformable iCSTransformable2)
				{
					Transform transform2 = Transform.Translation(-new Vector3d(_bbOriginal.Center));
					iCSTransformable2.ApplyTransform(transform2);
					_bbOriginal.Transform(transform2);
				}
				break;
			case 2:
				if (_objOriginal is ICSTransformable iCSTransformable)
				{
					Vector3d vector3d = new Vector3d(_bbOriginal.Center);
					vector3d.Z = _bbOriginal.Min.Z;
					Transform transform = Transform.Translation(-vector3d);
					iCSTransformable.ApplyTransform(transform);
					_bbOriginal.Transform(transform);
				}
				break;
			}
			MeshingParameters coarse = MeshingParameters.Coarse;
			coarse.GridMinCount = 16;
			coarse.MaximumEdgeLength = 0.3;
			_objMeshOriginal?.Dispose();
			Mesh mesh = _objOriginal.MeshObject(coarse, productionMeshes: false, useDisplayMesh: false);
			if (mesh == null)
			{
				_objOriginal = null;
				_objMeshOriginal = null;
				_bbOriginal = new BoundingBox(new Point3d(-5.0, -5.0, -5.0), new Point3d(5.0, 5.0, 5.0));
				return base.Update();
			}
			_objMeshOriginal = new CSOnGeometryData(mesh);
			if (_bendXz == null)
			{
				_bendXz = new CSBendPoints(Point3d.Origin, new Point3d(10.0, 0.0, 0.0), new Point3d(9.0, 0.0, 0.0), 1);
				_bendXy = new CSBendPoints(Point3d.Origin, new Point3d(10.0, 0.0, 0.0), new Point3d(9.0, 0.0, 0.0), 1);
				_bendYz = new CSBendPoints(Point3d.Origin, new Point3d(0.0, 10.0, 0.0), new Point3d(0.0, 9.0, 0.0), 1);
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
			}
		}
		flag |= _objOutput == null;
		flag |= _bendXz.HasChanged();
		flag |= _bendXy.HasChanged();
		flag |= _bendYz.HasChanged();
		if ((flag | _bendNeedsRefresh) && _objOriginal != null)
		{
			_objOutput = (MouseDown ? _objMeshOriginal.Duplicate() : _objOriginal.Duplicate());
			_objOutput.CurrentMaterialGroup = _objOriginalMaterialGroup;
			_objOutput.DefaultDisplayMaterialClassification = _objOriginalMaterialGroup.Classification;
			if (_bendXz.IsBending())
			{
				CSBendMorph spaceMorph = new CSBendMorph(_bendXz)
				{
					Tolerance = 0.05
				};
				_objOutput.Morph(spaceMorph);
				_bendXz.Changed();
			}
			if (_bendXy.IsBending())
			{
				CSBendMorph spaceMorph2 = new CSBendMorph(_bendXy)
				{
					Tolerance = 0.05
				};
				_objOutput.Morph(spaceMorph2);
				_bendXy.Changed();
			}
			if (_bendYz.IsBending())
			{
				CSBendMorph spaceMorph3 = new CSBendMorph(_bendYz)
				{
					Tolerance = 0.05
				};
				_objOutput.Morph(spaceMorph3);
				_bendYz.Changed();
			}
			_objOutput.GetBoundingBox(ref _bb);
			if (!_bb.IsValid)
			{
				_bb = new BoundingBox(new Point3d(-5.0, -5.0, -5.0), new Point3d(5.0, 5.0, 5.0));
			}
			(_objOutput as CSOnGeometryData)?.PreProcessDisplayInfo();
			if (_objOutput is CSDataArray cSDataArray2)
			{
				CSDataArray flattenArray = cSDataArray2.GetFlattenArray();
				for (int num = 0; num < flattenArray.GetArrayCount(); num++)
				{
					(flattenArray.GetArrayItem(num) as CSOnGeometryData)?.PreProcessDisplayInfo();
				}
			}
			_bendNeedsRefresh = false;
		}
		if (_objOutput == null)
		{
			return base.Update();
		}
		CSData cSData = _objOutput.Duplicate();
		ICSTransformable iCSTransformable3 = cSData as ICSTransformable;
		if (_outputFlipMode == 1)
		{
			Transform transform3 = Transform.Mirror(Point3d.Origin, new Vector3d(1.0, 0.0, 0.0));
			iCSTransformable3?.ApplyTransform(transform3);
		}
		iCSTransformable3?.ApplyTransform(ObjectTransform);
		if (_arrayDirection != CSValueHandleHelper.ArrayDirection.Off)
		{
			CSDataArray cSDataArray3 = new CSDataArray();
			int num2 = (int)Math.Round(_arrayTotalAngle / _arrayRepeatAngle);
			int num3 = num2;
			if (!_arrayMirror)
			{
				if (_arrayTotalAngle > 6.2482787221397)
				{
					num2--;
				}
			}
			else if (_arrayTotalAngle > Math.PI * 179.0 / 180.0)
			{
				num2--;
			}
			GetFollowCurve(out var crvFollow, out var pStart, out var pEnd, out var up, _arrayTotalAngle);
			if (crvFollow != null)
			{
				crvFollow.ClosestPoint(pStart, out var t);
				crvFollow.ChangeClosedCurveSeam(t);
				Plane plane = new Plane(crvFollow.PointAtStart, GetAverageTangency(crvFollow, 0.0), up);
				double length = crvFollow.GetLength();
				double num4;
				if (_arrayTotalAngle <= 6.2482787221397)
				{
					crvFollow.ClosestPoint(pEnd, out var t2);
					num4 = crvFollow.GetLength(new Interval(crvFollow.Domain.Min, t2));
				}
				else
				{
					num4 = length;
				}
				double num5 = num4 / length / (double)num3;
				double t3;
				if (_arrayMirror)
				{
					for (int num6 = num3; num6 > 0; num6--)
					{
						double num7 = 1.0 - num5 * (double)num6;
						crvFollow.NormalizedLengthParameter(num7, out t3);
						Plane plane2 = new Plane(crvFollow.PointAt(t3), GetAverageTangency(crvFollow, num7), up);
						Transform transform4 = Transform.PlaneToPlane(plane, plane2);
						CSData cSData2 = cSData.Duplicate();
						(cSData2 as ICSTransformable)?.ApplyTransform(transform4);
						cSDataArray3.Append(cSData2);
					}
				}
				for (int num8 = 0; num8 <= num2; num8++)
				{
					double num9 = num5 * (double)num8;
					crvFollow.NormalizedLengthParameter(num9, out t3);
					Plane plane3 = new Plane(crvFollow.PointAt(t3), GetAverageTangency(crvFollow, num9), up);
					Transform transform5 = Transform.PlaneToPlane(plane, plane3);
					CSData cSData3 = cSData.Duplicate();
					(cSData3 as ICSTransformable)?.ApplyTransform(transform5);
					cSDataArray3.Append(cSData3);
				}
				cSData = cSDataArray3;
			}
		}
		OutTransformedObjects.SetData(cSData);
		if (_outputMirrorMode == 1 || _outputMirrorMode == 4)
		{
			CSData cSData4 = cSData.Duplicate();
			if (cSData4 is ICSTransformable iCSTransformable4)
			{
				Transform transform6 = Transform.Mirror(Point3d.Origin, new Vector3d(1.0, 0.0, 0.0));
				iCSTransformable4.ApplyTransform(transform6);
			}
			OutTransformedObjects1.SetData(cSData4);
		}
		if (_outputMirrorMode == 2 || _outputMirrorMode == 4)
		{
			CSData cSData5 = cSData.Duplicate();
			if (cSData5 is ICSTransformable iCSTransformable5)
			{
				Transform transform7 = Transform.Mirror(Point3d.Origin, new Vector3d(1.0, 0.0, 0.0));
				iCSTransformable5.ApplyTransform(transform7);
				Transform transform8 = Transform.Mirror(Point3d.Origin, new Vector3d(0.0, 1.0, 0.0));
				iCSTransformable5.ApplyTransform(transform8);
			}
			OutTransformedObjects2.SetData(cSData5);
		}
		if (_outputMirrorMode == 3 || _outputMirrorMode == 4)
		{
			CSData cSData6 = cSData.Duplicate();
			if (cSData6 is ICSTransformable iCSTransformable6)
			{
				Transform transform9 = Transform.Mirror(Point3d.Origin, new Vector3d(0.0, 1.0, 0.0));
				iCSTransformable6.ApplyTransform(transform9);
			}
			OutTransformedObjects3.SetData(cSData6);
		}
		return base.Update();
	}

	private static Vector3d GetAverageTangency(Curve crv, double normParam)
	{
		double num = normParam - 0.01;
		double num2 = normParam + 0.01;
		if (num < 0.0)
		{
			num += 1.0;
		}
		if (num2 >= 1.0)
		{
			num2 -= 1.0;
		}
		crv.NormalizedLengthParameter(num, out var t);
		crv.NormalizedLengthParameter(num2, out var t2);
		Vector3d vector3d = crv.TangentAt(t);
		Vector3d vector3d2 = crv.TangentAt(t2);
		Vector3d result = vector3d + vector3d2;
		result.Unitize();
		return result;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		if (base.NotCreatedYet)
		{
			return true;
		}
		if (!GetType().IsInstanceOfType(setObject))
		{
			return true;
		}
		CSValueXFormObject cSValueXFormObject = (CSValueXFormObject)setObject;
		_value = cSValueXFormObject._value;
		_arrayObjectTransform = cSValueXFormObject._arrayObjectTransform;
		_arrayObjectTransformInv = cSValueXFormObject._arrayObjectTransformInv;
		_bendXy = CSBendPoints.NewBp(cSValueXFormObject._bendXy);
		_bendXz = CSBendPoints.NewBp(cSValueXFormObject._bendXz);
		_bendYz = CSBendPoints.NewBp(cSValueXFormObject._bendYz);
		_outputMirrorMode = cSValueXFormObject._outputMirrorMode;
		_arrayDirection = cSValueXFormObject._arrayDirection;
		_arrayTotalAngle = cSValueXFormObject._arrayTotalAngle;
		_arrayRepeatAngle = cSValueXFormObject._arrayRepeatAngle;
		_arrayMirror = cSValueXFormObject._arrayMirror;
		_arrayShape = cSValueXFormObject._arrayShape;
		_outputFlipMode = cSValueXFormObject._outputFlipMode;
		_bendNeedsRefresh = true;
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
		CSValueXFormObject cSValueXFormObject = (CSValueXFormObject)setObject;
		_value = cSValueXFormObject._value;
		_arrayObjectTransform = cSValueXFormObject._arrayObjectTransform;
		_arrayObjectTransformInv = cSValueXFormObject._arrayObjectTransformInv;
		_bendXy = CSBendPoints.NewBp(cSValueXFormObject._bendXy);
		_bendXz = CSBendPoints.NewBp(cSValueXFormObject._bendXz);
		_bendYz = CSBendPoints.NewBp(cSValueXFormObject._bendYz);
		_outputFlipMode = cSValueXFormObject._outputFlipMode;
		_outputMirrorMode = cSValueXFormObject._outputMirrorMode;
		_arrayShape = cSValueXFormObject._arrayShape;
		_arrayDirection = cSValueXFormObject._arrayDirection;
		_arrayMirror = cSValueXFormObject._arrayMirror;
		_arrayRepeatAngle = cSValueXFormObject._arrayRepeatAngle;
		_arrayTotalAngle = cSValueXFormObject._arrayTotalAngle;
		_bendNeedsRefresh = true;
		return true;
	}

	public override bool SetFileValue(CSObject setObject)
	{
		base.SetFileValue(setObject);
		if (!base.NotCreatedYet && GetType().IsInstanceOfType(setObject))
		{
			CSValueXFormObject cSValueXFormObject = (CSValueXFormObject)setObject;
			_value = cSValueXFormObject._value;
			_arrayObjectTransform = cSValueXFormObject._arrayObjectTransform;
			_arrayObjectTransformInv = cSValueXFormObject._arrayObjectTransformInv;
			_bendXy = CSBendPoints.NewBp(cSValueXFormObject._bendXy);
			_bendXz = CSBendPoints.NewBp(cSValueXFormObject._bendXz);
			_bendYz = CSBendPoints.NewBp(cSValueXFormObject._bendYz);
			_outputFlipMode = cSValueXFormObject._outputFlipMode;
			_outputMirrorMode = cSValueXFormObject._outputMirrorMode;
			_arrayShape = cSValueXFormObject._arrayShape;
			_arrayDirection = cSValueXFormObject._arrayDirection;
			_arrayMirror = cSValueXFormObject._arrayMirror;
			_arrayRepeatAngle = cSValueXFormObject._arrayRepeatAngle;
			_arrayTotalAngle = cSValueXFormObject._arrayTotalAngle;
			_bendNeedsRefresh = true;
		}
		return true;
	}

	private static void Write(OnBinaryArchive archive, Transform t)
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				archive.WriteDouble(t[i, j]);
			}
		}
	}

	private static Transform Read(OnBinaryArchive archive)
	{
		Transform identity = Transform.Identity;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				double value = 0.0;
				archive.ReadDouble(ref value);
				identity[i, j] = value;
			}
		}
		return identity;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		archive.Add("Value", _value);
		archive.Add("Transform", _arrayObjectTransform);
		if (_bendXy != null)
		{
			archive.Add("BendXY", _bendXy.WriteJson());
		}
		if (_bendXz != null)
		{
			archive.Add("BendXZ", _bendXz.WriteJson());
		}
		if (_bendYz != null)
		{
			archive.Add("BendYZ", _bendYz.WriteJson());
		}
		archive.Add("MirrorMode", _outputMirrorMode);
		archive.Add("Direction", (int)_arrayDirection);
		archive.Add("TotalAngle", _arrayTotalAngle);
		archive.Add("RepeatAngle", _arrayRepeatAngle);
		archive.Add("Mirror", _arrayMirror);
		archive.Add("Shape", (int)_arrayShape);
		archive.Add("FlipMode", _outputFlipMode);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.WriteInt(5);
		Write(archive, _value);
		Write(archive, _arrayObjectTransform);
		if (_bendXy == null)
		{
			CSBendPoints cSBendPoints = new CSBendPoints();
			cSBendPoints.Write(ref archive);
			cSBendPoints.Write(ref archive);
			cSBendPoints.Write(ref archive);
		}
		else
		{
			_bendXy.Write(ref archive);
			_bendXz.Write(ref archive);
			_bendYz.Write(ref archive);
		}
		archive.WriteInt(_outputMirrorMode);
		archive.WriteInt((int)_arrayDirection);
		archive.WriteDouble(_arrayTotalAngle);
		archive.WriteDouble(_arrayRepeatAngle);
		archive.WriteBool(_arrayMirror);
		archive.WriteInt((int)_arrayShape);
		archive.WriteInt(_outputFlipMode);
		return true;
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		bool flag = base.ReadJson(jBlock, graph, major, minor);
		flag &= jBlock.GetMatrix("Value", out _value);
		flag &= jBlock.GetMatrix("Transform", out _arrayObjectTransform);
		if (jBlock.ContainsKey("BendXY"))
		{
			flag &= CSBendPoints.Read(jBlock["BendXY"], out _bendXy);
		}
		if (jBlock.ContainsKey("BendXY"))
		{
			flag &= CSBendPoints.Read(jBlock["BendXZ"], out _bendXz);
		}
		if (jBlock.ContainsKey("BendXY"))
		{
			flag &= CSBendPoints.Read(jBlock["BendYZ"], out _bendYz);
		}
		flag &= jBlock.ReadJsonValue<int>("Shape", out var value);
		_arrayShape = (ArrayShape)value;
		flag &= jBlock.ReadJsonValue<int>("MirrorMode", out _outputMirrorMode);
		flag &= jBlock.ReadJsonValue<int>("Direction", out var value2);
		_arrayDirection = (CSValueHandleHelper.ArrayDirection)value2;
		flag &= jBlock.ReadJsonValue<double>("TotalAngle", out _arrayTotalAngle);
		flag &= jBlock.ReadJsonValue<double>("RepeatAngle", out _arrayRepeatAngle);
		flag &= jBlock.ReadJsonValue<bool>("Mirror", out _arrayMirror);
		return flag & jBlock.ReadJsonValue<int>("FlipMode", out _outputFlipMode);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		_bendXy = new CSBendPoints();
		_bendXz = new CSBendPoints();
		_bendYz = new CSBendPoints();
		int num = 0;
		archive.ReadInt(ref num);
		switch (num)
		{
		case 1:
			_value = Read(archive);
			_bendXy.Read(ref archive, num);
			_bendXz.Read(ref archive, num);
			_bendYz.Read(ref archive, num);
			_outputMirrorMode = 0;
			_outputFlipMode = 0;
			break;
		case 2:
			_value = Read(archive);
			_bendXy.Read(ref archive, num);
			_bendXz.Read(ref archive, num);
			_bendYz.Read(ref archive, num);
			archive.ReadInt(ref _outputMirrorMode);
			_outputFlipMode = 0;
			break;
		case 3:
		{
			_value = Read(archive);
			_bendXy.Read(ref archive, num);
			_bendXz.Read(ref archive, num);
			_bendYz.Read(ref archive, num);
			archive.ReadInt(ref _outputMirrorMode);
			int num5 = 0;
			archive.ReadInt(ref num5);
			if (Enum.IsDefined(typeof(CSValueHandleHelper.ArrayDirection), num5))
			{
				_arrayDirection = (CSValueHandleHelper.ArrayDirection)num5;
			}
			else
			{
				_arrayDirection = CSValueHandleHelper.ArrayDirection.Off;
			}
			archive.ReadDouble(ref _arrayTotalAngle);
			archive.ReadDouble(ref _arrayRepeatAngle);
			archive.ReadBool(ref _arrayMirror);
			_outputFlipMode = 0;
			break;
		}
		case 4:
		{
			_value = Read(archive);
			_bendXy.Read(ref archive, num);
			_bendXz.Read(ref archive, num);
			_bendYz.Read(ref archive, num);
			archive.ReadInt(ref _outputMirrorMode);
			int num4 = 0;
			archive.ReadInt(ref num4);
			if (Enum.IsDefined(typeof(CSValueHandleHelper.ArrayDirection), num4))
			{
				_arrayDirection = (CSValueHandleHelper.ArrayDirection)num4;
			}
			else
			{
				_arrayDirection = CSValueHandleHelper.ArrayDirection.Off;
			}
			archive.ReadDouble(ref _arrayTotalAngle);
			archive.ReadDouble(ref _arrayRepeatAngle);
			archive.ReadBool(ref _arrayMirror);
			archive.ReadInt(ref _outputFlipMode);
			break;
		}
		case 5:
		{
			_value = Read(archive);
			_arrayObjectTransform = Read(archive);
			_arrayObjectTransform.TryGetInverse(out _arrayObjectTransformInv);
			_bendXy.Read(ref archive, num);
			_bendXz.Read(ref archive, num);
			_bendYz.Read(ref archive, num);
			archive.ReadInt(ref _outputMirrorMode);
			int num2 = 0;
			archive.ReadInt(ref num2);
			if (Enum.IsDefined(typeof(CSValueHandleHelper.ArrayDirection), num2))
			{
				_arrayDirection = (CSValueHandleHelper.ArrayDirection)num2;
			}
			else
			{
				_arrayDirection = CSValueHandleHelper.ArrayDirection.Off;
			}
			archive.ReadDouble(ref _arrayTotalAngle);
			archive.ReadDouble(ref _arrayRepeatAngle);
			archive.ReadBool(ref _arrayMirror);
			int num3 = 0;
			archive.ReadInt(ref num3);
			if (Enum.IsDefined(typeof(ArrayShape), num3))
			{
				_arrayShape = (ArrayShape)num3;
			}
			else
			{
				_arrayShape = ArrayShape.Round;
			}
			archive.ReadInt(ref _outputFlipMode);
			break;
		}
		}
		if (_bendXy.IsAllZero())
		{
			_bendXy = null;
		}
		if (_bendXz.IsAllZero())
		{
			_bendXz = null;
		}
		if (_bendYz.IsAllZero())
		{
			_bendYz = null;
		}
		_bendNeedsRefresh = true;
		return true;
	}

	public void SetModesFromUI(int flipMode, int mirrorMode, bool mouseDown)
	{
		try
		{
			if (mouseDown)
			{
				CSUtil.ToggleMouseDown(this);
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetModesFromUI");
				PreChange("SetModesFromUI");
				_outputFlipMode = flipMode;
				_outputMirrorMode = mirrorMode;
				Change();
				CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetModesFromUI");
				CSApplicationHelper.Application.Document.Graph.Generate();
				CSUtil.Redraw();
				CSUtil.RedrawUI();
			}
			else
			{
				CSUtil.RemoveMouseDown();
				CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
				CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("SetModesFromUI");
				PreChange("SetModesFromUI");
				_outputFlipMode = flipMode;
				_outputMirrorMode = mirrorMode;
				Change();
				CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("SetModesFromUI");
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

	private void PreSet(bool isMouseMoving = false)
	{
		CSApplicationHelper.Application.Document.Graph.IncrementUpdateCounter();
		if (!isMouseMoving)
		{
			CSUtil.ToggleMouseDown(this);
			CSApplicationHelper.Application.Document.UndoTable.BeginUndoRecord("PreSet");
			PreChange("PreSet");
		}
	}

	private void PostSet(bool isMoving = false, bool isQuickRegen = true)
	{
		Change();
		if (!isMoving)
		{
			CSApplicationHelper.Application.Document.UndoTable.EndUndoRecord("PostSet");
		}
		CSApplicationHelper.Application.Document.Graph.Generate(isQuickRegen);
		CSUtil.Redraw();
		CSUtil.RedrawUI();
	}

	private void SetArrayMode(CSValueHandleHelper.ArrayDirection arrayMode)
	{
		PreSet();
		_arrayDirection = arrayMode;
		PostSet();
	}

	private void SetArrayMirrorMode(bool arrayMirror)
	{
		PreSet();
		_arrayMirror = arrayMirror;
		PostSet();
	}

	private void SetArrayShapeMode()
	{
		PreSet();
		_arrayShape = (ArrayShape)((int)(_arrayShape + 1) % 9);
		PostSet();
	}

	private void MSetP0(CSBendPoints bendPoint, Point3d newP0)
	{
		if (!(newP0 == bendPoint.BasePoint))
		{
			PreSet();
			bendPoint.BasePoint = newP0;
			PostSet();
		}
	}

	private void MSetP1(CSBendPoints bendPoint, Point3d newP1)
	{
		if (!(newP1 == bendPoint.EndPoint))
		{
			PreSet();
			bendPoint.EndPoint = new Point3d(newP1);
			PostSet();
		}
	}

	private void MSetP2(CSBendPoints bendPoint, Point3d newP2)
	{
		if (!(newP2 == bendPoint.BendPoint))
		{
			PreSet();
			bendPoint.BendPoint = new Point3d(newP2);
			PostSet();
		}
	}

	private void MSetBendMode(CSBendPoints bendPoint, byte bendMode)
	{
		if (bendMode != bendPoint._bendMode)
		{
			PreSet();
			bendPoint._bendMode = bendMode;
			PostSet();
		}
	}

	private CSBendPoints GetCurrentBend(RhinoViewport vp, out int posX, out int posY)
	{
		int num = 0;
		int num2 = 0;
		Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(vp);
		cameraDirection.Unitize();
		Point3d center = _bbOriginal.Center;
		center.Transform(ObjectTransform);
		Vector3d vector3d = new Vector3d(1.0, 0.0, 0.0);
		Vector3d vector3d2 = new Vector3d(0.0, 1.0, 0.0);
		Vector3d vector3d3 = new Vector3d(0.0, 0.0, 1.0);
		vector3d.Transform(ObjectTransform);
		vector3d.Unitize();
		vector3d2.Transform(ObjectTransform);
		vector3d2.Unitize();
		vector3d3.Transform(ObjectTransform);
		vector3d3.Unitize();
		double num3 = Math.Abs(cameraDirection * vector3d);
		double num4 = Math.Abs(cameraDirection * vector3d2);
		double num5 = Math.Abs(cameraDirection * vector3d3);
		Vector3d vector3d4 = new Vector3d(1.0, 0.0, 0.0);
		Vector3d vector3d5 = new Vector3d(1.0, 0.0, 0.0);
		CSBendPoints result = null;
		double num6 = 0.0;
		if (num3 > num6)
		{
			vector3d4 = vector3d2;
			vector3d5 = vector3d3;
			num6 = num3;
			result = _bendYz;
			num = ObjSizeY;
			num2 = ObjSizeZ;
		}
		if (num4 > num6)
		{
			vector3d4 = vector3d;
			vector3d5 = vector3d3;
			num6 = num4;
			result = _bendXz;
			num = ObjSizeX;
			num2 = ObjSizeZ;
		}
		if (num5 > num6)
		{
			vector3d4 = vector3d;
			vector3d5 = vector3d2;
			result = _bendXy;
			num = ObjSizeX;
			num2 = ObjSizeY;
		}
		Point3d point3d = new Point3d(-(vector3d4 * (num - 1)) + vector3d5 * (num2 - 1) + center);
		Transform xform = CSValueHandleHelper.WorldToScreen(vp);
		point3d.Transform(xform);
		double num7 = 32.0 * CSValueHandleHelper.ScreenScaleOverride;
		posX = (int)(point3d.X - num7 / 2.0);
		posY = (int)(point3d.Y - num7 / 2.0);
		return result;
	}

	private void DrawBendBackGroundGrid(DisplayPipeline displayPipeline)
	{
		if (!_bb.IsValid)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(displayPipeline);
		Point3d center = _bbOriginal.Center;
		center.Transform(ObjectTransform);
		Vector3d vector3d = new Vector3d(1.0, 0.0, 0.0);
		Vector3d vector3d2 = new Vector3d(0.0, 1.0, 0.0);
		Vector3d vector3d3 = new Vector3d(0.0, 0.0, 1.0);
		vector3d.Transform(ObjectTransform);
		vector3d.Unitize();
		vector3d2.Transform(ObjectTransform);
		vector3d2.Unitize();
		vector3d3.Transform(ObjectTransform);
		vector3d3.Unitize();
		double num3 = Math.Abs(cameraDirection * vector3d);
		double num4 = Math.Abs(cameraDirection * vector3d2);
		double num5 = Math.Abs(cameraDirection * vector3d3);
		Vector3d vector3d4 = Vector3d.Unset;
		Vector3d vector3d5 = Vector3d.Unset;
		double num6 = 0.0;
		if (num3 > num6)
		{
			vector3d4 = vector3d2;
			vector3d5 = vector3d3;
			num6 = num3;
			num = ObjSizeY;
			num2 = ObjSizeZ;
		}
		if (num4 > num6)
		{
			vector3d4 = vector3d;
			vector3d5 = vector3d3;
			num6 = num4;
			num = ObjSizeX;
			num2 = ObjSizeZ;
		}
		if (num5 > num6)
		{
			vector3d4 = vector3d;
			vector3d5 = vector3d2;
			num = ObjSizeX;
			num2 = ObjSizeY;
		}
		Transform designTransform = CSApplicationHelper.Application.Conduit.GetDesignTransform();
		Transform modelTransform = displayPipeline.ModelTransform;
		displayPipeline.ModelTransform = modelTransform * designTransform;
		for (int i = 0; i <= num; i++)
		{
			Color color = Color.FromArgb(255, 208, 208, 208);
			if (i % 5 == 0 || i == num)
			{
				color = Color.FromArgb(255, 160, 160, 160);
			}
			displayPipeline.DrawLine(new Point3d(vector3d4 * i - vector3d5 * num2 + center), new Point3d(vector3d4 * i + vector3d5 * num2 + center), color, 1);
			displayPipeline.DrawLine(new Point3d(-(vector3d4 * i) - vector3d5 * num2 + center), new Point3d(-(vector3d4 * i) + vector3d5 * num2 + center), color, 1);
		}
		for (int j = 0; j <= num2; j++)
		{
			Color color2 = Color.FromArgb(255, 208, 208, 208);
			if (j % 5 == 0 || j == num2)
			{
				color2 = Color.FromArgb(255, 160, 160, 160);
			}
			displayPipeline.DrawLine(new Point3d(vector3d5 * j - vector3d4 * num + center), new Point3d(vector3d5 * j + vector3d4 * num + center), color2, 1);
			displayPipeline.DrawLine(new Point3d(-(vector3d5 * j) - vector3d4 * num + center), new Point3d(-(vector3d5 * j) + vector3d4 * num + center), color2, 1);
		}
		displayPipeline.ModelTransform = modelTransform;
	}

	private void DrawBendControlPoints(DisplayPipeline displayPipeline)
	{
		double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(displayPipeline, BasePoint);
		int posX;
		int posY;
		CSBendPoints currentBend = GetCurrentBend(displayPipeline.Viewport, out posX, out posY);
		Point3d point3d = new Point3d(currentBend.BasePoint);
		Point3d point3d2 = new Point3d(currentBend.EndPoint);
		Point3d p = new Point3d(currentBend.BendPoint);
		Point3d to = ((currentBend._bendMode > 0) ? new Point3d(point3d - (point3d2 - point3d)) : new Point3d(point3d));
		Transform transform = ObjectTransform;
		if (_outputFlipMode == 1)
		{
			Point3d origin = Point3d.Origin;
			origin.Transform(ObjectTransform);
			transform = Transform.Mirror(origin, Vector3d.XAxis) * transform;
		}
		point3d.Transform(transform);
		point3d2.Transform(transform);
		p.Transform(transform);
		to.Transform(transform);
		CSValueHandleHelper.DrawMeshSphere(point3d, 9.0, worldToScreenScale, displayPipeline, (MouseOver && _whichSelected == CSValueHandleHelper.WSelected.BendPoint0) ? CSValueHandleHelper.MatSelected : CSValueHandleHelper.MatBendBasePoint);
		CSValueHandleHelper.DrawMeshSphere(point3d2, 9.0, worldToScreenScale, displayPipeline, (MouseOver && _whichSelected == CSValueHandleHelper.WSelected.BendPoint1) ? CSValueHandleHelper.MatSelected : CSValueHandleHelper.MatBendEndPoint);
		CSValueHandleHelper.DrawMeshSphere(p, 15.0, worldToScreenScale, displayPipeline, (MouseOver && _whichSelected == CSValueHandleHelper.WSelected.BendPoint2) ? CSValueHandleHelper.MatSelected : CSValueHandleHelper.MatBendBendPoint);
		using (Mesh mesh = CSValueHandleHelper.MeshCurve(new LineCurve(point3d2, to), 1, 20, 2.0 / worldToScreenScale))
		{
			displayPipeline.DrawMeshShaded(mesh, (MouseOver && _whichSelected == CSValueHandleHelper.WSelected.BendPoint1) ? CSValueHandleHelper.MatSelected : CSValueHandleHelper.MatBendEndPoint);
		}
		NurbsCurve displayCurve = currentBend.GetDisplayCurve();
		displayCurve.Transform(transform);
		using (Mesh mesh2 = CSValueHandleHelper.MeshCurve(displayCurve, 20, 20, 2.0 / worldToScreenScale))
		{
			displayPipeline.DrawMeshShaded(mesh2, (MouseOver && _whichSelected == CSValueHandleHelper.WSelected.BendPoint2) ? CSValueHandleHelper.MatSelected : CSValueHandleHelper.MatBendBendPoint);
		}
		if (currentBend._bendMode <= 0)
		{
			return;
		}
		NurbsCurve displayMirrorCurve = currentBend.GetDisplayMirrorCurve();
		displayMirrorCurve.Transform(transform);
		using Mesh mesh3 = CSValueHandleHelper.MeshCurve(displayMirrorCurve, 20, 20, 2.0 / worldToScreenScale);
		displayPipeline.DrawMeshShaded(mesh3, (MouseOver && _whichSelected == CSValueHandleHelper.WSelected.BendPoint2) ? CSValueHandleHelper.MatSelected : CSValueHandleHelper.MatBendBendPoint);
	}

	private void DrawBendShadow(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions)
	{
		Transform modelTransform = displayPipeline.ModelTransform;
		displayPipeline.ModelTransform = modelTransform * ObjectTransform;
		if (_objOriginal is CSOnGeometryData cSOnGeometryData)
		{
			cSOnGeometryData.Display(displayPipeline, displayOptions, CSValueHandleHelper.MatBendShadow);
		}
		else if (_objOriginal is CSDataArray cSDataArray)
		{
			for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
			{
				if (cSDataArray.GetArrayItem(i) is CSOnGeometryData cSOnGeometryData2)
				{
					cSOnGeometryData2.Display(displayPipeline, displayOptions, CSValueHandleHelper.MatBendShadow);
				}
			}
		}
		displayPipeline.ModelTransform = modelTransform;
	}

	private void DrawBendIcon(DisplayPipeline displayPipeline)
	{
		if (!_bb.IsValid)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(displayPipeline);
		Point3d center = _bbOriginal.Center;
		center.Transform(ObjectTransform);
		Vector3d xAxis = Vector3d.XAxis;
		xAxis.Transform(ObjectTransform);
		xAxis.Unitize();
		Vector3d yAxis = Vector3d.YAxis;
		yAxis.Transform(ObjectTransform);
		yAxis.Unitize();
		Vector3d zAxis = Vector3d.ZAxis;
		zAxis.Transform(ObjectTransform);
		zAxis.Unitize();
		double num3 = Math.Abs(cameraDirection * xAxis);
		double num4 = Math.Abs(cameraDirection * yAxis);
		double num5 = Math.Abs(cameraDirection * zAxis);
		Vector3d vector3d = Vector3d.Unset;
		Vector3d vector3d2 = Vector3d.Unset;
		CSBendPoints cSBendPoints = null;
		double num6 = 0.0;
		if (num3 > num6)
		{
			vector3d = yAxis;
			vector3d2 = zAxis;
			num6 = num3;
			cSBendPoints = _bendYz;
			num = ObjSizeY;
			num2 = ObjSizeZ;
		}
		if (num4 > num6)
		{
			vector3d = xAxis;
			vector3d2 = zAxis;
			num6 = num4;
			cSBendPoints = _bendXz;
			num = ObjSizeX;
			num2 = ObjSizeZ;
		}
		if (num5 > num6)
		{
			vector3d = xAxis;
			vector3d2 = yAxis;
			cSBendPoints = _bendXy;
			num = ObjSizeX;
			num2 = ObjSizeY;
		}
		if (cSBendPoints != null)
		{
			Point3d point3d = new Point3d(-(vector3d * (num - 1)) + vector3d2 * (num2 - 1) + center);
			DisplayBitmap displayBitmap = null;
			switch (cSBendPoints._bendMode)
			{
			case 0:
				displayBitmap = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.MirrorModeBmp && cSBendPoints._iconMouseOver) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Bend0S) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Bend0));
				break;
			case 1:
				displayBitmap = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.MirrorModeBmp && cSBendPoints._iconMouseOver) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Bend1S) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Bend1));
				break;
			case 2:
				displayBitmap = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.MirrorModeBmp && cSBendPoints._iconMouseOver) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Bend2S) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Bend2));
				break;
			}
			if (displayBitmap != null)
			{
				Transform xform = CSValueHandleHelper.WorldToScreen(displayPipeline);
				point3d.Transform(xform);
				double num7 = 32.0 * CSValueHandleHelper.ScreenScaleOverride;
				int left = (int)(point3d.X - num7 / 2.0);
				int top = (int)(point3d.Y - num7 / 2.0);
				displayPipeline.DrawBitmap(displayBitmap, left, top);
			}
		}
	}

	private void CalculateBendPlanes(RhinoViewport vp, out Plane pl0, out Plane pl1, out Plane pl2, out CSBendPoints currentBend)
	{
		Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(vp);
		cameraDirection.Unitize();
		_bbOriginal.Center.Transform(ObjectTransform);
		Vector3d vector3d = new Vector3d(1.0, 0.0, 0.0);
		Vector3d vector3d2 = new Vector3d(0.0, 1.0, 0.0);
		Vector3d vector3d3 = new Vector3d(0.0, 0.0, 1.0);
		vector3d.Transform(ObjectTransform);
		vector3d.Unitize();
		vector3d2.Transform(ObjectTransform);
		vector3d2.Unitize();
		vector3d3.Transform(ObjectTransform);
		vector3d3.Unitize();
		double num = Math.Abs(cameraDirection * vector3d);
		double num2 = Math.Abs(cameraDirection * vector3d2);
		double num3 = Math.Abs(cameraDirection * vector3d3);
		Vector3d xDirection = new Vector3d(1.0, 0.0, 0.0);
		Vector3d yDirection = new Vector3d(1.0, 0.0, 0.0);
		currentBend = null;
		double num4 = 0.0;
		if (num > num4)
		{
			xDirection = vector3d2;
			yDirection = vector3d3;
			num4 = num;
			currentBend = _bendYz;
		}
		if (num2 > num4)
		{
			xDirection = vector3d;
			yDirection = vector3d3;
			num4 = num2;
			currentBend = _bendXz;
		}
		if (num3 > num4)
		{
			xDirection = vector3d;
			yDirection = vector3d2;
			currentBend = _bendXy;
		}
		if (currentBend == null)
		{
			pl0 = Plane.Unset;
			pl1 = Plane.Unset;
			pl2 = Plane.Unset;
			return;
		}
		Point3d origin = new Point3d(currentBend.BasePoint);
		Point3d origin2 = new Point3d(currentBend.EndPoint);
		Point3d origin3 = new Point3d(currentBend.BendPoint);
		origin.Transform(ObjectTransform);
		origin2.Transform(ObjectTransform);
		origin3.Transform(ObjectTransform);
		pl0 = new Plane(origin, xDirection, yDirection);
		pl1 = new Plane(origin2, xDirection, yDirection);
		pl2 = new Plane(origin3, xDirection, yDirection);
	}

	private void ArrayGetPoints(double l, out Point3d arrayCenter, out Point3d arrayStart, out Point3d arrayEnd, out Point3d arrayCount, out Plane hitPlane)
	{
		arrayCenter = ArrayCenter();
		Vector3d vector3d = BasePoint - arrayCenter;
		if (vector3d.Length < 0.01)
		{
			vector3d = ((_arrayDirection == CSValueHandleHelper.ArrayDirection.Yz) ? new Vector3d(0.0, 0.0, 1.0) : new Vector3d(1.0, 0.0, 0.0));
		}
		double length = vector3d.Length;
		vector3d.Unitize();
		Vector3d vector3d2 = VRot();
		vector3d2.Transform(_arrayObjectTransform);
		arrayStart = arrayCenter + vector3d * (length + 60.0 / l);
		arrayEnd = arrayCenter + vector3d * (length + 40.0 / l);
		arrayCount = arrayCenter + vector3d * (length + 20.0 / l);
		Transform xform = Transform.Rotation(_arrayTotalAngle, vector3d2, arrayCenter);
		arrayEnd.Transform(xform);
		xform = Transform.Rotation(_arrayRepeatAngle, vector3d2, arrayCenter);
		arrayCount.Transform(xform);
		Vector3d yDirection = Vector3d.CrossProduct(vector3d, vector3d2);
		hitPlane = new Plane(arrayCenter, vector3d, yDirection);
	}

	private void GetFollowCurve(out Curve crvFollow, out Point3d pStart, out Point3d pEnd, out Vector3d up, double secondAngle)
	{
		pStart = Point3d.Unset;
		pEnd = Point3d.Unset;
		Point3d point3d = ArrayCenter();
		Vector3d xDirection = Vector3d.Unset;
		Vector3d yDirection = Vector3d.Unset;
		switch (_arrayDirection)
		{
		case CSValueHandleHelper.ArrayDirection.Xy:
			xDirection = Vector3d.XAxis;
			yDirection = Vector3d.YAxis;
			break;
		case CSValueHandleHelper.ArrayDirection.Xz:
			xDirection = -Vector3d.XAxis;
			yDirection = -Vector3d.ZAxis;
			break;
		case CSValueHandleHelper.ArrayDirection.Yz:
			xDirection = -Vector3d.YAxis;
			yDirection = Vector3d.ZAxis;
			break;
		}
		xDirection.Transform(_arrayObjectTransform);
		yDirection.Transform(_arrayObjectTransform);
		Plane plane = new Plane(point3d, xDirection, yDirection);
		Transform transform = Transform.PlaneToPlane(Plane.WorldXY, plane);
		Transform transformation = Transform.PlaneToPlane(plane, Plane.WorldXY);
		up = Vector3d.ZAxis;
		up.Transform(transform);
		Vector3d vector = BasePoint - point3d;
		double length = vector.Length;
		vector.Transform(transformation);
		vector *= 10.0 / length;
		crvFollow = ArrayShapeCurve(_arrayShape);
		if (crvFollow == null)
		{
			return;
		}
		LineCurve lineCurve = new LineCurve(new Line(Point3d.Origin, new Point3d(vector)));
		CurveIntersections curveIntersections = Intersection.CurveCurve(lineCurve, crvFollow, 0.001, 0.001);
		if (curveIntersections.Count == 1)
		{
			pStart = curveIntersections[0].PointA;
			double length2 = new Vector3d(pStart).Length;
			Transform xform = Transform.Scale(Point3d.Origin, length / length2);
			crvFollow.Transform(xform);
			lineCurve.Transform(xform);
			pStart.Transform(xform);
			Transform xform2 = Transform.Rotation(secondAngle, -Vector3d.ZAxis, Point3d.Origin);
			lineCurve.Transform(xform2);
			CurveIntersections curveIntersections2 = Intersection.CurveCurve(lineCurve, crvFollow, 0.001, 0.001);
			if (curveIntersections2.Count == 1)
			{
				pEnd = curveIntersections2[0].PointA;
				crvFollow.Transform(transform);
				pStart.Transform(transform);
				pEnd.Transform(transform);
			}
		}
	}

	private static Curve ArrayShapeCurve(ArrayShape arrayShape)
	{
		NurbsCurve nurbsCurve = null;
		List<ControlPoint> list = null;
		List<double> list2 = null;
		switch (arrayShape)
		{
		case ArrayShape.Round:
			nurbsCurve = new NurbsCurve(3, rational: true, 3, 9);
			list = new List<ControlPoint>
			{
				new ControlPoint(new Point3d(0.0, 5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(5.0, 5.0, 0.0), 0.707106781186548),
				new ControlPoint(new Point3d(5.0, 0.0, 0.0), 1.0),
				new ControlPoint(new Point3d(5.0, -5.0, 0.0), 0.707106781186548),
				new ControlPoint(new Point3d(0.0, -5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(-5.0, -5.0, 0.0), 0.707106781186548),
				new ControlPoint(new Point3d(-5.0, 0.0, 0.0), 1.0),
				new ControlPoint(new Point3d(-5.0, 5.0, 0.0), 0.707106781186548),
				new ControlPoint(new Point3d(0.0, 5.0, 0.0), 1.0)
			};
			list2 = new List<double> { -54.9778714378214, -54.9778714378214, -47.1238898038469, -47.1238898038469, -39.2699081698724, -39.2699081698724, -31.4159265358979, -31.4159265358979, -23.5619449019234, -23.5619449019234 };
			break;
		case ArrayShape.Cushion:
			nurbsCurve = new NurbsCurve(3, rational: true, 4, 21);
			list = new List<ControlPoint>
			{
				new ControlPoint(new Point3d(0.0, 5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(0.727, 5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(1.453, 4.84845483359746, 0.0), 1.0),
				new ControlPoint(new Point3d(2.914, 4.24029318541997, 0.0), 1.0),
				new ControlPoint(new Point3d(3.396, 3.6311410459588, 0.0), 1.0),
				new ControlPoint(new Point3d(4.121, 0.906299524564184, 0.0), 1.0),
				new ControlPoint(new Point3d(4.121, -0.912242472266244, 0.0), 1.0),
				new ControlPoint(new Point3d(3.395, -3.63213153724247, 0.0), 1.0),
				new ControlPoint(new Point3d(2.913, -4.24128367670364, 0.0), 1.0),
				new ControlPoint(new Point3d(0.728, -5.15154516640253, 0.0), 1.0),
				new ControlPoint(new Point3d(-0.729, -5.15154516640253, 0.0), 1.0),
				new ControlPoint(new Point3d(-2.914, -4.24029318541997, 0.0), 1.0),
				new ControlPoint(new Point3d(-3.396, -3.6311410459588, 0.0), 1.0),
				new ControlPoint(new Point3d(-4.121, -0.908280507131537, 0.0), 1.0),
				new ControlPoint(new Point3d(-4.121, 0.909270998415214, 0.0), 1.0),
				new ControlPoint(new Point3d(-3.395, 3.6311410459588, 0.0), 1.0),
				new ControlPoint(new Point3d(-2.90966877389025, 4.23365431555561, 0.0), 1.0),
				new ControlPoint(new Point3d(-2.182, 4.54536450079239, 0.0), 1.0),
				new ControlPoint(new Point3d(-1.49054618248125, 4.84156134924195, 0.0), 0.986597556634823),
				new ControlPoint(new Point3d(-0.753332382951691, 4.99516518231797, 0.0), 0.986597556634823),
				new ControlPoint(new Point3d(0.0, 5.0, 0.0), 1.0)
			};
			list2 = new List<double>
			{
				-63.4929997716415, -63.4929997716415, -63.4929997716415, -60.7506519513663, -60.7506519513663, -57.9790598946706, -57.9790598946706, -52.4361582004184, -52.4361582004184, -49.6771818937776,
				-49.6771818937776, -44.1864598649196, -44.1864598649196, -41.4270519987023, -41.4270519987023, -35.8799399106294, -35.8799399106294, -33.1191210507496, -33.1191210507496, -33.1191210507496,
				-30.3738787208919, -30.3738787208919, -30.3738787208919
			};
			break;
		case ArrayShape.Oval:
			nurbsCurve = new NurbsCurve(3, rational: true, 3, 9);
			list = new List<ControlPoint>
			{
				new ControlPoint(new Point3d(0.0, 5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(3.85095239299544, 5.0, 0.0), 0.707106781186548),
				new ControlPoint(new Point3d(3.85095239299544, 0.0, 0.0), 1.0),
				new ControlPoint(new Point3d(3.85095239299544, -5.0, 0.0), 0.707106781186548),
				new ControlPoint(new Point3d(0.0, -5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(-3.85095239299544, -5.0, 0.0), 0.707106781186548),
				new ControlPoint(new Point3d(-3.85095239299544, 0.0, 0.0), 1.0),
				new ControlPoint(new Point3d(-3.85095239299544, 5.0, 0.0), 0.707106781186548),
				new ControlPoint(new Point3d(0.0, 5.0, 0.0), 1.0)
			};
			list2 = new List<double> { -54.9778714378214, -54.9778714378214, -47.1238898038469, -47.1238898038469, -39.2699081698724, -39.2699081698724, -31.4159265358979, -31.4159265358979, -23.5619449019234, -23.5619449019234 };
			break;
		case ArrayShape.Emerald:
			nurbsCurve = new NurbsCurve(3, rational: false, 2, 10);
			list = new List<ControlPoint>
			{
				new ControlPoint(new Point3d(0.0, 5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(2.51275737490217, 5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(3.62486808335934, 3.90530266909049, 0.0), 1.0),
				new ControlPoint(new Point3d(3.62486808335934, -3.90530266909049, 0.0), 1.0),
				new ControlPoint(new Point3d(2.51275737490217, -5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(-2.51275737490217, -5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(-3.62486808335934, -3.90530266909049, 0.0), 1.0),
				new ControlPoint(new Point3d(-3.62486808335934, 3.90530266909049, 0.0), 1.0),
				new ControlPoint(new Point3d(-2.51275737490217, 5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(0.0, 5.0, 0.0), 1.0)
			};
			list2 = new List<double> { 0.0, 2.51275737490217, 4.07325482592397, 11.8838601641049, 13.4443576151267, 18.4698723649311, 20.0303698159529, 27.8409751541339, 29.4014726051557, 31.9142299800578 };
			break;
		case ArrayShape.Asscher:
			nurbsCurve = new NurbsCurve(3, rational: false, 2, 10);
			list = new List<ControlPoint>
			{
				new ControlPoint(new Point3d(0.0, 5.0, -1.47451495458029E-16), 1.0),
				new ControlPoint(new Point3d(3.49187732779735, 5.0, -1.47451495458029E-16), 1.0),
				new ControlPoint(new Point3d(5.0, 3.49187732779735, -1.33573707650214E-16), 1.0),
				new ControlPoint(new Point3d(5.0, -3.49187732779735, -1.47451495458029E-16), 1.0),
				new ControlPoint(new Point3d(3.49187732779735, -5.0, -1.47451495458029E-16), 1.0),
				new ControlPoint(new Point3d(-3.49187732779735, -5.0, -1.47451495458029E-16), 1.0),
				new ControlPoint(new Point3d(-5.0, -3.49187732779735, -1.47451495458029E-16), 1.0),
				new ControlPoint(new Point3d(-5.0, 3.49187732779735, -1.47451495458029E-16), 1.0),
				new ControlPoint(new Point3d(-3.49187732779735, 5.0, -1.47451495458029E-16), 1.0),
				new ControlPoint(new Point3d(0.0, 5.0, -1.47451495458029E-16), 1.0)
			};
			list2 = new List<double> { 0.0, 3.49187732779735, 5.62468486454869, 12.6084395201434, 14.7412470568947, 21.7250017124894, 23.8578092492408, 30.8415639048355, 32.9743714415868, 36.4662487693841 };
			break;
		case ArrayShape.Heart:
			nurbsCurve = new NurbsCurve(3, rational: false, 4, 21);
			list = new List<ControlPoint>
			{
				new ControlPoint(new Point3d(0.0, 3.05309571595388, 0.0), 1.0),
				new ControlPoint(new Point3d(0.0203651415156476, 3.05309571595388, 0.0), 1.0),
				new ControlPoint(new Point3d(0.264993331832227, 3.97836659795814, 0.0), 1.0),
				new ControlPoint(new Point3d(1.94194794608987, 4.73484796685005, 0.0), 1.0),
				new ControlPoint(new Point3d(4.29807695006768, 4.13446893966394, 0.0), 1.0),
				new ControlPoint(new Point3d(5.23398464757578, 1.81603754069441, 0.0), 1.0),
				new ControlPoint(new Point3d(4.71437052885891, -0.497952311032498, 0.0), 1.0),
				new ControlPoint(new Point3d(3.39470117219139, -2.39780259462241, 0.0), 1.0),
				new ControlPoint(new Point3d(1.87028154322722, -4.13547158073963, 0.0), 1.0),
				new ControlPoint(new Point3d(0.611295954179884, -5.03015291833902, 0.0), 1.0),
				new ControlPoint(new Point3d(1.56319401867222E-13, -5.49199999999987, 0.0), 1.0),
				new ControlPoint(new Point3d(-0.611295954179884, -5.03015291833902, 0.0), 1.0),
				new ControlPoint(new Point3d(-1.87028154322722, -4.13547158073963, 0.0), 1.0),
				new ControlPoint(new Point3d(-3.39470117219139, -2.39780259462241, 0.0), 1.0),
				new ControlPoint(new Point3d(-4.71437052885891, -0.497952311032498, 0.0), 1.0),
				new ControlPoint(new Point3d(-5.23398464757578, 1.81603754069441, 0.0), 1.0),
				new ControlPoint(new Point3d(-4.29807695006768, 4.13446893966394, 0.0), 1.0),
				new ControlPoint(new Point3d(-1.94194794608987, 4.73484796685005, 0.0), 1.0),
				new ControlPoint(new Point3d(-0.264993331832227, 3.97836659795814, 0.0), 1.0),
				new ControlPoint(new Point3d(-0.0203651415156476, 3.05309571595388, 0.0), 1.0),
				new ControlPoint(new Point3d(0.0, 3.05309571595388, 0.0), 1.0)
			};
			list2 = new List<double>
			{
				-24.0, -24.0, -24.0, -23.0, -22.0, -21.0, -20.0, -19.0, -18.0, -17.0,
				-16.0, -16.0, -16.0, -15.0, -14.0, -13.0, -12.0, -11.0, -10.0, -9.0,
				-8.0, -8.0, -8.0
			};
			break;
		case ArrayShape.Marquise:
			nurbsCurve = new NurbsCurve(3, rational: false, 4, 17);
			list = new List<ControlPoint>
			{
				new ControlPoint(new Point3d(0.0, 5.00019627843123, 0.0), 1.0),
				new ControlPoint(new Point3d(0.712007932535551, 4.53136035896029, 0.0), 1.0),
				new ControlPoint(new Point3d(1.80744945476397, 3.46956249884035, 0.0), 1.0),
				new ControlPoint(new Point3d(2.63224041736151, 1.63445204245651, 0.0), 1.0),
				new ControlPoint(new Point3d(2.84455188187878, -0.000329137596437408, 0.0), 1.0),
				new ControlPoint(new Point3d(2.63198455449184, -1.63500343297349, 0.0), 1.0),
				new ControlPoint(new Point3d(1.80745943468324, -3.46955066864161, 0.0), 1.0),
				new ControlPoint(new Point3d(0.71202037019818, -4.53135135126492, 0.0), 1.0),
				new ControlPoint(new Point3d(0.0, -5.00019627843123, 0.0), 1.0),
				new ControlPoint(new Point3d(-0.71202037019818, -4.53135135126492, 0.0), 1.0),
				new ControlPoint(new Point3d(-1.80745943468324, -3.46955066864161, 0.0), 1.0),
				new ControlPoint(new Point3d(-2.63198455449184, -1.63500343297349, 0.0), 1.0),
				new ControlPoint(new Point3d(-2.84455188187878, -0.000329137596437408, 0.0), 1.0),
				new ControlPoint(new Point3d(-2.63224041736151, 1.63445204245651, 0.0), 1.0),
				new ControlPoint(new Point3d(-1.80744945476397, 3.46956249884035, 0.0), 1.0),
				new ControlPoint(new Point3d(-0.712007932535551, 4.53136035896029, 0.0), 1.0),
				new ControlPoint(new Point3d(0.0, 5.00019627843123, 0.0), 1.0)
			};
			list2 = new List<double>
			{
				11.8712117080269, 11.8712117080269, 11.8712117080269, 14.4287225561793, 16.2945084340148, 17.8077445137763, 19.3191273125358, 21.1848665428327, 23.7424234160538, 23.7424234160538,
				23.7424234160538, 26.2999802892749, 28.1657195195717, 29.6771023183313, 31.1903383980928, 33.0561242759282, 35.6136351240807, 35.6136351240807, 35.6136351240807
			};
			break;
		case ArrayShape.Pear:
			nurbsCurve = new NurbsCurve(3, rational: true, 4, 33);
			list = new List<ControlPoint>
			{
				new ControlPoint(new Point3d(0.0, 3.535, 0.0), 1.0),
				new ControlPoint(new Point3d(0.124, 3.531, 0.0), 1.0),
				new ControlPoint(new Point3d(0.565, 3.497, 0.0), 1.0),
				new ControlPoint(new Point3d(1.503, 3.256, 0.0), 1.0),
				new ControlPoint(new Point3d(2.283, 2.671, 0.0), 1.0),
				new ControlPoint(new Point3d(2.888, 1.959, 0.0), 1.0),
				new ControlPoint(new Point3d(3.377, 1.031, 0.0), 1.0),
				new ControlPoint(new Point3d(3.527, -0.038, 0.0), 1.0),
				new ControlPoint(new Point3d(3.501, -0.883, 0.0), 1.0),
				new ControlPoint(new Point3d(3.308, -1.907, 0.0), 1.0),
				new ControlPoint(new Point3d(2.98, -2.805, 0.0), 1.0),
				new ControlPoint(new Point3d(2.495, -3.774, 0.0), 1.0),
				new ControlPoint(new Point3d(1.994, -4.533, 0.0), 1.0),
				new ControlPoint(new Point3d(1.27, -5.416, 0.0), 1.0),
				new ControlPoint(new Point3d(0.587, -6.028, 0.0), 1.0),
				new ControlPoint(new Point3d(0.22, -6.31, 0.0), 1.0),
				new ControlPoint(new Point3d(0.0, -6.465, 0.0), 1.0),
				new ControlPoint(new Point3d(-0.22, -6.31, 0.0), 1.0),
				new ControlPoint(new Point3d(-0.587, -6.028, 0.0), 1.0),
				new ControlPoint(new Point3d(-1.27, -5.416, 0.0), 1.0),
				new ControlPoint(new Point3d(-1.994, -4.533, 0.0), 1.0),
				new ControlPoint(new Point3d(-2.495, -3.774, 0.0), 1.0),
				new ControlPoint(new Point3d(-2.98, -2.805, 0.0), 1.0),
				new ControlPoint(new Point3d(-3.308, -1.907, 0.0), 1.0),
				new ControlPoint(new Point3d(-3.501, -0.883, 0.0), 1.0),
				new ControlPoint(new Point3d(-3.527, -0.038, 0.0), 1.0),
				new ControlPoint(new Point3d(-3.377, 1.031, 0.0), 1.0),
				new ControlPoint(new Point3d(-2.888, 1.959, 0.0), 1.0),
				new ControlPoint(new Point3d(-2.283, 2.671, 0.0), 1.0),
				new ControlPoint(new Point3d(-1.503, 3.256, 0.0), 1.0),
				new ControlPoint(new Point3d(-0.565, 3.497, 0.0), 1.0),
				new ControlPoint(new Point3d(-0.124, 3.531, 0.0), 1.0),
				new ControlPoint(new Point3d(0.0, 3.535, 0.0), 1.0)
			};
			list2 = new List<double>
			{
				-58.4025871190687, -58.4025871190687, -58.4025871190687, -58.0, -57.0, -56.0, -55.0, -54.0, -53.0, -52.0,
				-51.0, -50.0, -49.0, -48.0, -47.0, -46.0, -45.1501151119657, -45.1501151119657, -45.1501151119657, -44.3002302239314,
				-43.3002302239314, -42.3002302239314, -41.3002302239314, -40.3002302239314, -39.3002302239314, -38.3002302239314, -37.3002302239314, -36.3002302239314, -35.3002302239314, -34.3002302239314,
				-33.3002302239314, -32.3002302239314, -31.8976431048627, -31.8976431048627, -31.8976431048627
			};
			break;
		case ArrayShape.Square:
			nurbsCurve = new NurbsCurve(3, rational: false, 2, 6);
			list = new List<ControlPoint>
			{
				new ControlPoint(new Point3d(0.0, 5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(5.0, 5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(5.0, -5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(-5.0, -5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(-5.0, 5.0, 0.0), 1.0),
				new ControlPoint(new Point3d(0.0, 5.0, 0.0), 1.0)
			};
			list2 = new List<double> { -65.0, -60.0, -50.0, -40.0, -30.0, -25.0 };
			break;
		}
		if (nurbsCurve == null)
		{
			return null;
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			nurbsCurve.Points[i] = list[i];
		}
		int count2 = list2.Count;
		for (int j = 0; j < count2; j++)
		{
			nurbsCurve.Knots[j] = list2[j];
		}
		return nurbsCurve;
	}

	private void DrawArrayArrows(Point3d pointStart, Point3d pointEnd, DisplayPipeline displayPipeline, double pixelsPerUnit, Color lineColor, CSValueHandleHelper.WSelected selId, double rotateAngle)
	{
		Vector3d vector3d = pointEnd - pointStart;
		vector3d.Unitize();
		Point3d txtBoxPoint = pointEnd + vector3d * 50.0 / pixelsPerUnit;
		bool flag = MouseOver && _whichSelected == selId;
		DisplayMaterial displayMaterial = new DisplayMaterial
		{
			Diffuse = lineColor,
			Transparency = 0.5,
			Emission = Color.Black,
			Ambient = Color.FromArgb(40, 40, 40)
		};
		DisplayMaterial displayMaterial2 = (flag ? CSValueHandleHelper.MatSelected : displayMaterial);
		CSValueHandleHelper.DrawMeshCylinder(pointStart, pointEnd, pixelsPerUnit, displayPipeline, displayMaterial2);
		Vector3d vector3d2 = pointEnd - pointStart;
		vector3d2.Unitize();
		Vector3d vector3d3 = Vector3d.XAxis;
		switch (_arrayDirection)
		{
		case CSValueHandleHelper.ArrayDirection.Xy:
			vector3d3 = new Vector3d(0.0, 0.0, -1.0);
			break;
		case CSValueHandleHelper.ArrayDirection.Xz:
			vector3d3 = new Vector3d(0.0, 1.0, 0.0);
			break;
		case CSValueHandleHelper.ArrayDirection.Yz:
			vector3d3 = new Vector3d(1.0, 0.0, 0.0);
			break;
		}
		vector3d3.Transform(_arrayObjectTransform);
		Vector3d vector3d4 = Vector3d.CrossProduct(vector3d3, vector3d2);
		if (flag)
		{
			lineColor = Color.FromArgb(255, 247, 19, 255);
		}
		if (_lsttxtBox == null)
		{
			_lsttxtBox = new List<TxtBox>();
		}
		switch (selId)
		{
		case CSValueHandleHelper.WSelected.ArrayCount:
		{
			int num2 = (int)Math.Round(_arrayTotalAngle / _arrayRepeatAngle) + 1;
			if (_arrayMirror)
			{
				num2 += num2 - 1;
			}
			if (!_arrayMirror)
			{
				if (_arrayTotalAngle > 6.2482787221397)
				{
					num2--;
				}
			}
			else if (_arrayTotalAngle > Math.PI * 179.0 / 180.0)
			{
				num2--;
			}
			_lsttxtBox.Add(new TxtBox(txtBoxPoint, "Count: " + num2.ToString(CultureInfo.InvariantCulture), Color.FromArgb(128, lineColor.ToArgb() & 0xFF, (lineColor.ToArgb() >> 8) & 0xFF, (lineColor.ToArgb() >> 16) & 0xFF)));
			break;
		}
		case CSValueHandleHelper.WSelected.ArrayStart:
			_lsttxtBox.Add(new TxtBox(txtBoxPoint, "Position", Color.FromArgb(128, lineColor.ToArgb() & 0xFF, (lineColor.ToArgb() >> 8) & 0xFF, (lineColor.ToArgb() >> 16) & 0xFF)));
			break;
		case CSValueHandleHelper.WSelected.ArrayEnd:
		{
			double num = Math.Round(Math.Abs(rotateAngle * CSBlockUtil.CSUtilMath.On_RADIANS_TO_DEGREES));
			_lsttxtBox.Add(new TxtBox(txtBoxPoint, "Angle: " + num.ToString(CultureInfo.InvariantCulture) + " °", Color.FromArgb(128, lineColor.ToArgb() & 0xFF, (lineColor.ToArgb() >> 8) & 0xFF, (lineColor.ToArgb() >> 16) & 0xFF)));
			break;
		}
		}
		if (selId != CSValueHandleHelper.WSelected.ArrayStart && flag)
		{
			Plane plane = new Plane(pointStart, vector3d2, vector3d4);
			double length = (pointEnd - pointStart).Length;
			length *= 0.6;
			ArcCurve ac = new ArcCurve(new Arc(new Circle(plane, length), 0.0 - rotateAngle));
			int majorAxis = (int)Math.Abs(rotateAngle * 180.0 / Math.PI);
			using Mesh mesh = CSValueHandleHelper.MeshCurve(ac, majorAxis, 20, 2.1 / pixelsPerUnit);
			displayPipeline.DrawMeshShaded(mesh, displayMaterial2);
		}
		vector3d2 *= 7.0 / pixelsPerUnit;
		vector3d3 *= 7.0 / pixelsPerUnit;
		vector3d4 *= 7.0 / pixelsPerUnit;
		Transform transform = CSValueHandleHelper.ChangeBasis(pointEnd, vector3d2, vector3d3, vector3d4);
		Transform modelTransform = displayPipeline.ModelTransform;
		displayPipeline.ModelTransform = modelTransform * transform;
		displayMaterial2.Transparency = 0.7;
		displayPipeline.DrawMeshShaded(CSValueHandleHelper.MeshRotate, displayMaterial2);
		displayPipeline.ModelTransform = modelTransform;
		displayMaterial.Dispose();
	}

	private void DrawArrayIcon(DisplayPipeline displayPipeline)
	{
		if (!_bb.IsValid)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(displayPipeline);
		Point3d center = _bbOriginal.Center;
		center.Transform(ObjectTransform);
		Vector3d vector3d = new Vector3d(1.0, 0.0, 0.0);
		Vector3d vector3d2 = new Vector3d(0.0, 1.0, 0.0);
		Vector3d vector3d3 = new Vector3d(0.0, 0.0, 1.0);
		vector3d.Transform(ObjectTransform);
		vector3d.Unitize();
		vector3d2.Transform(ObjectTransform);
		vector3d2.Unitize();
		vector3d3.Transform(ObjectTransform);
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
			num = ObjSizeY;
			num2 = ObjSizeZ;
		}
		if (num4 > num6)
		{
			vector3d4 = vector3d;
			vector3d5 = vector3d3;
			num6 = num4;
			num = ObjSizeX;
			num2 = ObjSizeZ;
		}
		if (num5 > num6)
		{
			vector3d4 = vector3d;
			vector3d5 = vector3d2;
			num = ObjSizeX;
			num2 = ObjSizeY;
		}
		Point3d point3d = new Point3d(-(vector3d4 * (num - 1)) + vector3d5 * (num2 - 1) + center);
		DisplayBitmap displayBitmap = null;
		switch (_arrayDirection)
		{
		case CSValueHandleHelper.ArrayDirection.Off:
			displayBitmap = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.ArrayTypeBmp) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array0S) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array0));
			break;
		case CSValueHandleHelper.ArrayDirection.Xy:
			displayBitmap = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.ArrayTypeBmp) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array1S) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array1));
			break;
		case CSValueHandleHelper.ArrayDirection.Xz:
			displayBitmap = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.ArrayTypeBmp) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array2S) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array2));
			break;
		case CSValueHandleHelper.ArrayDirection.Yz:
			displayBitmap = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.ArrayTypeBmp) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array3S) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array3));
			break;
		}
		if (displayBitmap == null)
		{
			return;
		}
		Transform xform = CSValueHandleHelper.WorldToScreen(displayPipeline);
		point3d.Transform(xform);
		double num7 = 32.0 * CSValueHandleHelper.ScreenScaleOverride;
		int left = (int)(point3d.X - num7 / 2.0);
		int num8 = (int)(point3d.Y - num7 / 2.0);
		displayPipeline.DrawBitmap(displayBitmap, left, num8);
		num8 += (int)num7;
		if (_arrayDirection > CSValueHandleHelper.ArrayDirection.Off)
		{
			displayBitmap = ((!_arrayMirror) ? ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.ArrayMirrorBmp) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayMirrorOffS) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayMirrorOff)) : ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.ArrayMirrorBmp) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayMirrorOnS) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayMirrorOn)));
			displayPipeline.DrawBitmap(displayBitmap, left, num8);
			num8 += (int)num7;
			switch (_arrayShape)
			{
			case ArrayShape.Round:
				displayBitmap = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayRound);
				break;
			case ArrayShape.Cushion:
				displayBitmap = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayCushion);
				break;
			case ArrayShape.Oval:
				displayBitmap = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayOval);
				break;
			case ArrayShape.Emerald:
				displayBitmap = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayEmerald);
				break;
			case ArrayShape.Asscher:
				displayBitmap = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayAsscher);
				break;
			case ArrayShape.Heart:
				displayBitmap = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayHeart);
				break;
			case ArrayShape.Marquise:
				displayBitmap = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayMarquise);
				break;
			case ArrayShape.Pear:
				displayBitmap = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayPear);
				break;
			case ArrayShape.Square:
				displayBitmap = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArraySquare);
				break;
			}
			displayPipeline.DrawBitmap(displayBitmap, left, num8);
		}
	}

	private void GetIconLocation(RhinoViewport vp, out int posX, out int posY)
	{
		int num = 0;
		int num2 = 0;
		Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(vp);
		cameraDirection.Unitize();
		Point3d center = _bbOriginal.Center;
		center.Transform(ObjectTransform);
		Vector3d vector3d = new Vector3d(1.0, 0.0, 0.0);
		Vector3d vector3d2 = new Vector3d(0.0, 1.0, 0.0);
		Vector3d vector3d3 = new Vector3d(0.0, 0.0, 1.0);
		vector3d.Transform(ObjectTransform);
		vector3d.Unitize();
		vector3d2.Transform(ObjectTransform);
		vector3d2.Unitize();
		vector3d3.Transform(ObjectTransform);
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
			num = ObjSizeY;
			num2 = ObjSizeZ;
		}
		if (num4 > num6)
		{
			vector3d4 = vector3d;
			vector3d5 = vector3d3;
			num6 = num4;
			num = ObjSizeX;
			num2 = ObjSizeZ;
		}
		if (num5 > num6)
		{
			vector3d4 = vector3d;
			vector3d5 = vector3d2;
			num = ObjSizeX;
			num2 = ObjSizeY;
		}
		Point3d point3d = new Point3d(-(vector3d4 * (num - 1)) + vector3d5 * (num2 - 1) + center);
		Transform xform = CSValueHandleHelper.WorldToScreen(vp);
		point3d.Transform(xform);
		double num7 = 32.0 * CSValueHandleHelper.ScreenScaleOverride;
		posX = (int)(point3d.X - num7 / 2.0);
		posY = (int)(point3d.Y - num7 / 2.0);
	}

	public void DrawBackGround(DisplayPipeline displayPipeline)
	{
		if (IsOn && CSValueHandleHelper.CurrentHandle == this && _editMode == EditMode.Bend)
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
		DrawModePicker(displayPipeline, worldToScreenScale);
		if (CSValueHandleHelper.SelectedHandles.Contains(this))
		{
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 255, 0, 0), CSValueHandleHelper.WSelected.LinkedMx);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 0, 0, 255), CSValueHandleHelper.WSelected.LinkedMy);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 0, 255, 0), CSValueHandleHelper.WSelected.LinkedMz);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 255, 0, 0), CSValueHandleHelper.WSelected.LinkedMmx);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 0, 0, 255), CSValueHandleHelper.WSelected.LinkedMmy);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 0, 255, 0), CSValueHandleHelper.WSelected.LinkedMmz);
		}
		if (CSValueHandleHelper.CurrentHandle != this)
		{
			return;
		}
		if (_editMode == EditMode.Array && _arrayDirection != CSValueHandleHelper.ArrayDirection.Off && _arrayShape != ArrayShape.Round)
		{
			GetFollowCurve(out var crvFollow, out var _, out var _, out var _, _arrayTotalAngle);
			if (crvFollow != null)
			{
				displayPipeline.DrawCurve(crvFollow, Color.Pink, 1);
			}
		}
		if (_editMode == EditMode.Bend)
		{
			DrawBendControlPoints(displayPipeline);
			if (MouseOver && (_whichSelected == CSValueHandleHelper.WSelected.BendPoint0 || _whichSelected == CSValueHandleHelper.WSelected.BendPoint1))
			{
				DrawBendShadow(displayPipeline, displayOptions);
			}
			return;
		}
		if (_editMode == EditMode.Array)
		{
			if (_arrayDirection > CSValueHandleHelper.ArrayDirection.Off)
			{
				ArrayGetPoints(worldToScreenScale, out var arrayCenter, out var arrayStart, out var arrayEnd, out var arrayCount, out var _);
				DrawArrayArrows(arrayCenter, arrayStart, displayPipeline, worldToScreenScale, Color.FromArgb(255, 255, 0, 0), CSValueHandleHelper.WSelected.ArrayStart, 0.0);
				DrawArrayArrows(arrayCenter, arrayCount, displayPipeline, worldToScreenScale, Color.FromArgb(255, 0, 255, 0), CSValueHandleHelper.WSelected.ArrayCount, _arrayRepeatAngle);
				DrawArrayArrows(arrayCenter, arrayEnd, displayPipeline, worldToScreenScale, Color.FromArgb(255, 0, 0, 255), CSValueHandleHelper.WSelected.ArrayEnd, _arrayTotalAngle);
				double worldToScreenScale2 = CSValueHandleHelper.GetWorldToScreenScale(displayPipeline, arrayCenter);
				DrawArrow(displayPipeline, worldToScreenScale2, Color.FromArgb(255, 255, 0, 0), CSValueHandleHelper.WSelected.ArrayMx);
				DrawArrow(displayPipeline, worldToScreenScale2, Color.FromArgb(255, 0, 0, 255), CSValueHandleHelper.WSelected.ArrayMy);
				DrawArrow(displayPipeline, worldToScreenScale2, Color.FromArgb(255, 0, 255, 0), CSValueHandleHelper.WSelected.ArrayMz);
				DrawArrow(displayPipeline, worldToScreenScale2, Color.FromArgb(255, 255, 0, 0), CSValueHandleHelper.WSelected.ArrayRx);
				DrawArrow(displayPipeline, worldToScreenScale2, Color.FromArgb(255, 0, 0, 255), CSValueHandleHelper.WSelected.ArrayRy);
				DrawArrow(displayPipeline, worldToScreenScale2, Color.FromArgb(255, 0, 255, 0), CSValueHandleHelper.WSelected.ArrayRz);
			}
			return;
		}
		if (ShowScale)
		{
			if (MouseOver && (_whichSelected == CSValueHandleHelper.WSelected.ScaleX || _whichSelected == CSValueHandleHelper.WSelected.ScaleY || _whichSelected == CSValueHandleHelper.WSelected.ScaleZ || _whichSelected == CSValueHandleHelper.WSelected.Scale3D))
			{
				DrawBoundingBox(displayPipeline);
			}
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 255, 0, 0), CSValueHandleHelper.WSelected.ScaleX);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 0, 0, 255), CSValueHandleHelper.WSelected.ScaleY);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 0, 255, 0), CSValueHandleHelper.WSelected.ScaleZ);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 255, 255, 0), CSValueHandleHelper.WSelected.Scale3D);
		}
		if (ShowRotate)
		{
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 255, 0, 0), CSValueHandleHelper.WSelected.RotateX);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 0, 0, 255), CSValueHandleHelper.WSelected.RotateY);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 0, 255, 0), CSValueHandleHelper.WSelected.RotateZ);
		}
		if (ShowMove)
		{
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 255, 0, 0), CSValueHandleHelper.WSelected.MoveX);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 0, 0, 255), CSValueHandleHelper.WSelected.MoveY);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 0, 255, 0), CSValueHandleHelper.WSelected.MoveZ);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 255, 0, 0), CSValueHandleHelper.WSelected.MoveXNegative);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 0, 0, 255), CSValueHandleHelper.WSelected.MoveYNegative);
			DrawArrow(displayPipeline, worldToScreenScale, Color.FromArgb(255, 0, 255, 0), CSValueHandleHelper.WSelected.MoveZNegative);
		}
	}

	private Vector3d VRot()
	{
		return _arrayDirection switch
		{
			CSValueHandleHelper.ArrayDirection.Xy => new Vector3d(0.0, 0.0, -1.0), 
			CSValueHandleHelper.ArrayDirection.Xz => new Vector3d(0.0, 1.0, 0.0), 
			CSValueHandleHelper.ArrayDirection.Yz => new Vector3d(1.0, 0.0, 0.0), 
			_ => new Vector3d(0.0, 0.0, 0.0), 
		};
	}

	private Point3d ArrayCenter()
	{
		Point3d point3d = new Point3d(_value[0, 3], _value[1, 3], _value[2, 3]);
		point3d = _arrayDirection switch
		{
			CSValueHandleHelper.ArrayDirection.Xy => new Point3d(0.0, 0.0, point3d.Z), 
			CSValueHandleHelper.ArrayDirection.Xz => new Point3d(0.0, point3d.Y, 0.0), 
			CSValueHandleHelper.ArrayDirection.Yz => new Point3d(point3d.X, 0.0, 0.0), 
			_ => new Point3d(0.0, 0.0, 0.0), 
		};
		point3d.Transform(_arrayObjectTransform);
		return point3d;
	}

	public void DrawOverLay(DisplayPipeline displayPipeline)
	{
		if (!IsOn || CSValueHandleHelper.CurrentHandle != this)
		{
			return;
		}
		if (_editMode == EditMode.Bend)
		{
			DrawBendIcon(displayPipeline);
		}
		if (_editMode == EditMode.Array)
		{
			DrawArrayIcon(displayPipeline);
		}
		if (_lsttxtBox == null)
		{
			return;
		}
		foreach (TxtBox item in _lsttxtBox)
		{
			item.Drawtxt(displayPipeline);
		}
		_lsttxtBox.Clear();
	}

	private Transform ArrowTrans(double pixelPerUnit, CSValueHandleHelper.WSelected selId, out Point3d location, out Vector3d upDirection)
	{
		location = MsrMovePoint(pixelPerUnit, selId);
		MsrMoveDirection(selId, out var moveDirection, out upDirection, out var _);
		Vector3d a = default(Vector3d);
		if (upDirection.IsValid)
		{
			a = upDirection;
		}
		else if (Math.Abs(moveDirection * Vector3d.ZAxis) < 0.6)
		{
			a = Vector3d.CrossProduct(moveDirection, Vector3d.ZAxis);
		}
		Vector3d vector3d = Vector3d.CrossProduct(a, moveDirection);
		a = Vector3d.CrossProduct(moveDirection, vector3d);
		a *= 7.0 / pixelPerUnit;
		vector3d *= 7.0 / pixelPerUnit;
		Vector3d initialBaseX = moveDirection * 7.0 / pixelPerUnit;
		return CSValueHandleHelper.ChangeBasis(location, initialBaseX, vector3d, a);
	}

	private void DrawArrow(DisplayPipeline dp, double pixelPerUnit, Color lineColor, CSValueHandleHelper.WSelected selId)
	{
		bool flag = MouseOver && _whichSelected == selId;
		DisplayMaterial displayMaterial = null;
		DisplayMaterial displayMaterial2;
		if (CSValueHandleHelper.IsLinkedMove(selId))
		{
			displayMaterial2 = (IsFreehandCommonCenterModeEnabled ? CSValueHandleHelper.MatSecondType1 : CSValueHandleHelper.MatSecondType2);
		}
		else
		{
			displayMaterial = new DisplayMaterial
			{
				Diffuse = lineColor,
				Transparency = 0.5,
				Emission = Color.Black,
				Ambient = Color.FromArgb(40, 40, 40)
			};
			displayMaterial2 = (flag ? CSValueHandleHelper.MatSelected : displayMaterial);
		}
		Point3d location;
		Vector3d upDirection;
		Transform transform = ArrowTrans(pixelPerUnit, selId, out location, out upDirection);
		Point3d point3d = (CSValueHandleHelper.IsArrayMove(selId) ? ArrayCenter() : ((!CSValueHandleHelper.IsLinkedMove(selId)) ? BasePoint : UDirP));
		Mesh mesh = CSValueHandleHelper.MeshCylinder(point3d, location, pixelPerUnit);
		dp.DrawMeshShaded(mesh, displayMaterial2);
		if (CSValueHandleHelper.MsrMode(selId) == CSValueHandleHelper.WMode.Rotate && flag)
		{
			DrawRotateHandle(point3d, location, upDirection, pixelPerUnit, dp, displayMaterial2, displayMaterial, mesh);
		}
		mesh.Dispose();
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * transform;
		if (displayMaterial != null)
		{
			displayMaterial.Transparency = 0.7;
			displayMaterial.BackTransparency = 0.7;
		}
		switch (CSValueHandleHelper.MsrMode(selId))
		{
		case CSValueHandleHelper.WMode.Rotate:
			dp.DrawMeshShaded(CSValueHandleHelper.MeshRotate, displayMaterial2);
			break;
		case CSValueHandleHelper.WMode.Move:
			dp.DrawMeshShaded(CSValueHandleHelper.MeshArrow, displayMaterial2);
			break;
		case CSValueHandleHelper.WMode.Scale:
			dp.DrawMeshShaded(CSValueHandleHelper.MeshScale, displayMaterial2);
			break;
		}
		displayMaterial?.Dispose();
		dp.ModelTransform = modelTransform;
	}

	private void DrawRotateHandle(Point3d basePoint, Point3d location, Vector3d upDirection, double pixelPerUnit, DisplayPipeline dp, DisplayMaterial mat, DisplayMaterial mat1, Mesh cMesh)
	{
		Vector3d vector3d = location - basePoint;
		vector3d.Unitize();
		Vector3d rotationAxis = Vector3d.CrossProduct(vector3d, upDirection);
		Plane plane = new Plane(basePoint, vector3d, upDirection);
		double length = (location - basePoint).Length;
		length -= 20.0 / pixelPerUnit;
		Circle circle = new Circle(plane, length);
		using (Mesh mesh = CSValueHandleHelper.MeshTorus(new Torus(plane, length, 2.0 / pixelPerUnit), 90, 16))
		{
			dp.DrawMeshShaded(mesh, mat1);
		}
		ArcCurve arcCurve = new ArcCurve(new Arc(circle, 0.0 - _rotateAngle));
		double num = Math.Round(Math.Abs(_rotateAngle * CSBlockUtil.CSUtilMath.On_RADIANS_TO_DEGREES));
		if (num > 0.0)
		{
			if (_lsttxtBox == null)
			{
				_lsttxtBox = new List<TxtBox>();
			}
			_lsttxtBox.Add(new TxtBox(arcCurve.PointAt(arcCurve.Domain.Mid), num.ToString(CultureInfo.InvariantCulture) + " °", Color.FromArgb(255, 255, 19, 247)));
		}
		int majorAxis = (int)Math.Abs(_rotateAngle * 180.0 / Math.PI);
		using (Mesh mesh2 = CSValueHandleHelper.MeshCurve(arcCurve, majorAxis, 20, 2.1 / pixelPerUnit))
		{
			dp.DrawMeshShaded(mesh2, mat);
		}
		Transform xform = Transform.Rotation(0.0 - _rotateAngle, rotationAxis, basePoint);
		cMesh.Transform(xform);
		dp.DrawMeshShaded(cMesh, mat);
	}

	private void DrawModePicker(DisplayPipeline dp, double pixelPerUnit)
	{
		DisplayMaterial displayMaterial = new DisplayMaterial
		{
			Diffuse = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.TogglePoint) ? Color.FromArgb(255, 247, 19, 255) : Color.FromArgb(255, 61, 125, 255)),
			Emission = Color.FromArgb(0, 0, 0),
			Ambient = Color.FromArgb(40, 40, 40)
		};
		Transform transform = CSValueHandleHelper.ChangeBasis(UDirP, UDirX / pixelPerUnit * 10.0, UDirY / pixelPerUnit * 10.0, UDirZ / pixelPerUnit * 10.0);
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * transform;
		dp.DrawMeshShaded((CSValueHandleHelper.CurrentHandle == this) ? CSValueHandleHelper.MeshCube : CSValueHandleHelper.MeshSphere, displayMaterial);
		dp.ModelTransform = modelTransform;
		displayMaterial.Dispose();
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
			point3d.Transform(ObjectTransform);
			point3d2.Transform(ObjectTransform);
			point3d3.Transform(ObjectTransform);
			point3d4.Transform(ObjectTransform);
			point3d5.Transform(ObjectTransform);
			point3d6.Transform(ObjectTransform);
			to.Transform(ObjectTransform);
			point3d7.Transform(ObjectTransform);
			Color color = Color.FromArgb(255, 224, 224, 224);
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
			double value = 0.0;
			switch (_whichSelected)
			{
			case CSValueHandleHelper.WSelected.ScaleX:
				value = (point3d - point3d4).Length;
				break;
			case CSValueHandleHelper.WSelected.ScaleY:
				value = (point3d - point3d2).Length;
				break;
			case CSValueHandleHelper.WSelected.ScaleZ:
				value = (point3d - point3d5).Length;
				break;
			case CSValueHandleHelper.WSelected.Scale3D:
				return;
			}
			if (_lsttxtBox == null)
			{
				_lsttxtBox = new List<TxtBox>();
			}
			_lsttxtBox.Add(new TxtBox(new Point3d(BasePoint), Math.Round(value, 2) + " mm", Color.FromArgb(255, 255, 19, 247)));
		}
	}

	public bool MouseMoveHitTest(MouseCallbackEventArgs e)
	{
		_whichSelected = CSValueHandleHelper.WSelected.PNone;
		if (!IsOn)
		{
			return false;
		}
		RhinoViewport activeViewport = e.View.ActiveViewport;
		Line frustrumLine = CSValueHandleHelper.GetFrustrumLine(activeViewport, e.ViewportPoint);
		frustrumLine.Direction.Unitize();
		double distance = 9999.0;
		if (_editMode == EditMode.Bend)
		{
			if (CSValueHandleHelper.CurrentHandle == this)
			{
				int posX;
				int posY;
				CSBendPoints currentBend = GetCurrentBend(activeViewport, out posX, out posY);
				if (currentBend == null)
				{
					TestHitSphere(frustrumLine, activeViewport, UDirP, CSValueHandleHelper.WSelected.TogglePoint, ref distance);
					return _whichSelected != CSValueHandleHelper.WSelected.PNone;
				}
				_bendXy._iconMouseOver = false;
				_bendXz._iconMouseOver = false;
				_bendYz._iconMouseOver = false;
				double num = 32.0 * CSValueHandleHelper.ScreenScaleOverride;
				if (e.ViewportPoint.X > posX && (double)e.ViewportPoint.X < (double)posX + num && e.ViewportPoint.Y > posY && (double)e.ViewportPoint.Y < (double)posY + num)
				{
					MouseOver = true;
					currentBend._iconMouseOver = true;
					_whichSelected = CSValueHandleHelper.WSelected.MirrorModeBmp;
					return true;
				}
				Point3d bp = new Point3d(currentBend.BasePoint);
				Point3d bp2 = new Point3d(currentBend.EndPoint);
				Point3d bp3 = new Point3d(currentBend.BendPoint);
				Transform transform = ObjectTransform;
				if (_outputFlipMode == 1)
				{
					Point3d origin = Point3d.Origin;
					origin.Transform(ObjectTransform);
					transform = Transform.Mirror(origin, Vector3d.XAxis) * transform;
				}
				bp.Transform(transform);
				bp2.Transform(transform);
				bp3.Transform(transform);
				TestHitSphere(frustrumLine, activeViewport, bp, CSValueHandleHelper.WSelected.BendPoint0, ref distance);
				TestHitSphere(frustrumLine, activeViewport, bp2, CSValueHandleHelper.WSelected.BendPoint1, ref distance);
				TestHitSphere(frustrumLine, activeViewport, bp3, CSValueHandleHelper.WSelected.BendPoint2, ref distance);
			}
			TestHitSphere(frustrumLine, activeViewport, UDirP, CSValueHandleHelper.WSelected.TogglePoint, ref distance);
			return _whichSelected != CSValueHandleHelper.WSelected.PNone;
		}
		if (_editMode == EditMode.Array)
		{
			GetIconLocation(activeViewport, out var posX2, out var posY2);
			double num2 = 32.0 * CSValueHandleHelper.ScreenScaleOverride;
			if (e.ViewportPoint.X > posX2 && (double)e.ViewportPoint.X < (double)posX2 + num2 && e.ViewportPoint.Y > posY2 && (double)e.ViewportPoint.Y < (double)posY2 + num2)
			{
				MouseOver = true;
				_whichSelected = CSValueHandleHelper.WSelected.ArrayTypeBmp;
				return true;
			}
			if (_arrayDirection > CSValueHandleHelper.ArrayDirection.Off && e.ViewportPoint.X > posX2 && (double)e.ViewportPoint.X < (double)posX2 + num2 && (double)e.ViewportPoint.Y > (double)posY2 + num2 && (double)e.ViewportPoint.Y < (double)posY2 + num2 * 2.0)
			{
				MouseOver = true;
				_whichSelected = CSValueHandleHelper.WSelected.ArrayMirrorBmp;
				return true;
			}
			if (_arrayDirection > CSValueHandleHelper.ArrayDirection.Off && e.ViewportPoint.X > posX2 && (double)e.ViewportPoint.X < (double)posX2 + num2 && (double)e.ViewportPoint.Y > (double)posY2 + num2 * 2.0 && (double)e.ViewportPoint.Y < (double)posY2 + num2 * 3.0)
			{
				MouseOver = true;
				_whichSelected = CSValueHandleHelper.WSelected.ArrayShapeBmp;
				return true;
			}
			if (CSValueHandleHelper.CurrentHandle == this && _arrayDirection > CSValueHandleHelper.ArrayDirection.Off)
			{
				double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(activeViewport, BasePoint);
				ArrayGetPoints(worldToScreenScale, out var _, out var arrayStart, out var arrayEnd, out var arrayCount, out _hitPlane);
				TestHitSphere(frustrumLine, activeViewport, arrayEnd, CSValueHandleHelper.WSelected.ArrayEnd, ref distance);
				TestHitSphere(frustrumLine, activeViewport, arrayCount, CSValueHandleHelper.WSelected.ArrayCount, ref distance);
				TestHitSphere(frustrumLine, activeViewport, arrayStart, CSValueHandleHelper.WSelected.ArrayStart, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.ArrayMx, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.ArrayMy, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.ArrayMz, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.ArrayRx, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.ArrayRy, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.ArrayRz, ref distance);
			}
			TestHitSphere(frustrumLine, activeViewport, UDirP, CSValueHandleHelper.WSelected.TogglePoint, ref distance);
			return _whichSelected != CSValueHandleHelper.WSelected.PNone;
		}
		if (CSValueHandleHelper.CurrentHandle == this)
		{
			if (ShowMove)
			{
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.MoveX, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.MoveY, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.MoveZ, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.MoveXNegative, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.MoveYNegative, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.MoveZNegative, ref distance);
			}
			if (ShowRotate)
			{
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.RotateX, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.RotateY, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.RotateZ, ref distance);
			}
			if (ShowScale)
			{
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.ScaleX, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.ScaleY, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.ScaleZ, ref distance);
				TestHitPoint(frustrumLine, activeViewport, CSValueHandleHelper.WSelected.Scale3D, ref distance);
			}
		}
		TestHitSphere(frustrumLine, activeViewport, UDirP, CSValueHandleHelper.WSelected.TogglePoint, ref distance);
		return _whichSelected != CSValueHandleHelper.WSelected.PNone;
	}

	public bool SelectionChanged()
	{
		bool result = _whichSelected != _whichSelectedLast;
		_whichSelectedLast = _whichSelected;
		return result;
	}

	public bool MouseMove(MouseCallbackEventArgs e)
	{
		if (!MouseOver)
		{
			return false;
		}
		RhinoViewport activeViewport = e.View.ActiveViewport;
		if (_whichSelected == CSValueHandleHelper.WSelected.TogglePoint)
		{
			return true;
		}
		Line frustrumLine = CSValueHandleHelper.GetFrustrumLine(activeViewport, e.ViewportPoint);
		if (_whichSelected == CSValueHandleHelper.WSelected.BendPoint0 || _whichSelected == CSValueHandleHelper.WSelected.BendPoint1 || _whichSelected == CSValueHandleHelper.WSelected.BendPoint2)
		{
			CalculateBendPlanes(activeViewport, out var pl, out var pl2, out var pl3, out var currentBend);
			Transform transform = ObjectTransform;
			if (_outputFlipMode == 1)
			{
				Point3d origin = Point3d.Origin;
				origin.Transform(ObjectTransform);
				transform = Transform.Mirror(origin, Vector3d.XAxis) * transform;
			}
			switch (_whichSelected)
			{
			case CSValueHandleHelper.WSelected.BendPoint0:
			{
				if (Intersection.LinePlane(frustrumLine, pl, out var lineParameter3))
				{
					Point3d point3d4 = frustrumLine.PointAt(lineParameter3);
					Point3d newP2 = point3d4;
					transform.TryGetInverse(out var inverseTransform3);
					newP2.Transform(inverseTransform3);
					MSetP0(currentBend, newP2);
					return true;
				}
				return false;
			}
			case CSValueHandleHelper.WSelected.BendPoint1:
			{
				if (Intersection.LinePlane(frustrumLine, pl2, out var lineParameter2))
				{
					Point3d point3d2 = frustrumLine.PointAt(lineParameter2);
					Point3d point3d3 = point3d2;
					if (CSValueHandleHelper.MouseButtonShift(e))
					{
						pl.ClosestParameter(point3d3, out var s, out var t);
						double num = CSValueHandleHelper.AngleOfPointD(s, t);
						double num2 = Math.Sqrt(s * s + t * t);
						double num3 = Math.Round(num / (Math.PI / 4.0)) * (Math.PI / 4.0);
						s = Math.Sin(num3) * num2;
						t = Math.Cos(num3) * num2;
						point3d3 = pl.PointAt(s, t);
					}
					transform.TryGetInverse(out var inverseTransform2);
					point3d3.Transform(inverseTransform2);
					MSetP1(currentBend, point3d3);
					return true;
				}
				return false;
			}
			case CSValueHandleHelper.WSelected.BendPoint2:
			{
				if (Intersection.LinePlane(frustrumLine, pl3, out var lineParameter))
				{
					Point3d point3d = frustrumLine.PointAt(lineParameter);
					Point3d newP = point3d;
					transform.TryGetInverse(out var inverseTransform);
					newP.Transform(inverseTransform);
					MSetP2(currentBend, newP);
					return true;
				}
				return false;
			}
			}
		}
		if (Intersection.LinePlane(frustrumLine, _hitPlane, out var lineParameter4))
		{
			Point3d testPoint = frustrumLine.PointAt(lineParameter4);
			_hitPlane.ClosestParameter(testPoint, out var s2, out var t2);
			double num4 = s2;
			if (CSValueHandleHelper.MouseButtonShift(e))
			{
				num4 = Math.Round(num4 * 4.0) / 4.0;
			}
			if (num4 < 0.1)
			{
				num4 = 0.1;
			}
			Transform transform2 = Transform.Identity;
			Transform xTrans;
			double value;
			switch (_whichSelected)
			{
			case CSValueHandleHelper.WSelected.ArrayEnd:
			{
				PreSet(isMouseMoving: true);
				double num17 = CSValueHandleHelper.AngleOfPointD(t2, s2);
				_arrayRepeatAngle /= _arrayTotalAngle;
				_arrayTotalAngle = Math.PI * 2.0 - num17;
				if (!_arrayMirror)
				{
					if (_arrayTotalAngle > 6.2482787221397)
					{
						_arrayTotalAngle = Math.PI * 2.0;
					}
				}
				else if (_arrayTotalAngle > 3.1066860685499065)
				{
					_arrayTotalAngle = Math.PI;
				}
				_arrayRepeatAngle *= _arrayTotalAngle;
				PostSet(isMoving: true);
				return true;
			}
			case CSValueHandleHelper.WSelected.ArrayCount:
			{
				PreSet(isMouseMoving: true);
				double num11 = CSValueHandleHelper.AngleOfPointD(t2, s2);
				_arrayRepeatAngle = Math.PI * 2.0 - num11;
				double num12 = (_arrayTotalAngle + Math.PI * 2.0) / 2.0;
				if (_arrayRepeatAngle > num12)
				{
					_arrayRepeatAngle = 0.0;
				}
				if (_arrayRepeatAngle > _arrayTotalAngle)
				{
					_arrayRepeatAngle = _arrayTotalAngle;
				}
				int num13 = (int)(_arrayTotalAngle * CSBlockUtil.CSUtilMath.On_RADIANS_TO_DEGREES * 2.0);
				int num14 = num13;
				if (_arrayRepeatAngle >= _arrayTotalAngle / (double)num13)
				{
					num14 = (int)Math.Round(_arrayTotalAngle / _arrayRepeatAngle);
				}
				_arrayRepeatAngle = _arrayTotalAngle / (double)num14;
				PostSet(isMoving: true);
				return true;
			}
			case CSValueHandleHelper.WSelected.ArrayStart:
			{
				double num10 = CSValueHandleHelper.AngleOfPointD(t2, s2);
				if (num10 > Math.PI)
				{
					num10 = 0.0 - (Math.PI * 2.0 - num10);
				}
				double rotateAngle = _rotateAngle;
				_rotateAngle += num10;
				if (CSValueHandleHelper.MouseButtonShift(e))
				{
					_rotateAngle = Math.Round(_rotateAngle / (Math.PI / 4.0)) * (Math.PI / 4.0);
				}
				num10 = _rotateAngle - rotateAngle;
				if (_rotateAngle > Math.PI)
				{
					_rotateAngle = _rotateAngle - Math.PI - Math.PI;
				}
				if (_rotateAngle < -Math.PI)
				{
					_rotateAngle = Math.PI + (_rotateAngle + Math.PI);
				}
				GetFollowCurve(out var crvFollow, out var pStart, out var pEnd, out var up, 0.0 - num10);
				crvFollow.ClosestPoint(pStart, out var t3);
				crvFollow.ClosestPoint(pEnd, out var t4);
				Plane plane = new Plane(crvFollow.PointAt(t3), crvFollow.TangentAt(t3), up);
				Plane plane2 = new Plane(crvFollow.PointAt(t4), crvFollow.TangentAt(t4), up);
				transform2 = Transform.Rotation(num10, _hitPlane.Normal, EPoint);
				_hitPlane.Transform(transform2);
				transform2 = Transform.PlaneToPlane(plane, plane2);
				transform2 *= ObjectTransform;
				PreSet(isMouseMoving: true);
				ObjectTransform = transform2;
				PostSet(isMoving: true);
				return true;
			}
			case CSValueHandleHelper.WSelected.RotateX:
			case CSValueHandleHelper.WSelected.RotateY:
			case CSValueHandleHelper.WSelected.RotateZ:
			{
				double num16 = CSValueHandleHelper.AngleOfPointD(t2, s2);
				if (num16 > Math.PI)
				{
					num16 = 0.0 - (Math.PI * 2.0 - num16);
				}
				double rotateAngle2 = _rotateAngle;
				_rotateAngle += num16;
				if (CSValueHandleHelper.MouseButtonShift(e))
				{
					_rotateAngle = Math.Round(_rotateAngle / (Math.PI / 4.0)) * (Math.PI / 4.0);
				}
				num16 = _rotateAngle - rotateAngle2;
				if (_rotateAngle > Math.PI)
				{
					_rotateAngle = _rotateAngle - Math.PI - Math.PI;
				}
				if (_rotateAngle < -Math.PI)
				{
					_rotateAngle = Math.PI + (_rotateAngle + Math.PI);
				}
				if (_whichSelected == CSValueHandleHelper.WSelected.RotateX)
				{
					transform2 = Transform.Rotation(num16, -UDirZ, BasePoint);
				}
				if (_whichSelected == CSValueHandleHelper.WSelected.RotateY)
				{
					transform2 = Transform.Rotation(num16, -UDirX, BasePoint);
				}
				if (_whichSelected == CSValueHandleHelper.WSelected.RotateZ)
				{
					transform2 = Transform.Rotation(num16, -UDirY, BasePoint);
				}
				_hitPlane.Transform(transform2);
				xTrans = transform2;
				transform2 *= ObjectTransform;
				value = num16;
				break;
			}
			case CSValueHandleHelper.WSelected.ArrayRx:
			case CSValueHandleHelper.WSelected.ArrayRy:
			case CSValueHandleHelper.WSelected.ArrayRz:
			{
				double num18 = CSValueHandleHelper.AngleOfPointD(t2, s2);
				if (num18 > Math.PI)
				{
					num18 = 0.0 - (Math.PI * 2.0 - num18);
				}
				double rotateAngle3 = _rotateAngle;
				_rotateAngle += num18;
				if (CSValueHandleHelper.MouseButtonShift(e))
				{
					_rotateAngle = Math.Round(_rotateAngle / (Math.PI / 4.0)) * (Math.PI / 4.0);
				}
				num18 = _rotateAngle - rotateAngle3;
				if (_rotateAngle > Math.PI)
				{
					_rotateAngle = _rotateAngle - Math.PI - Math.PI;
				}
				if (_rotateAngle < -Math.PI)
				{
					_rotateAngle = Math.PI + (_rotateAngle + Math.PI);
				}
				if (_whichSelected == CSValueHandleHelper.WSelected.ArrayRx)
				{
					transform2 = Transform.Rotation(num18, -EDirZ, _hitPlane.Origin);
				}
				if (_whichSelected == CSValueHandleHelper.WSelected.ArrayRy)
				{
					transform2 = Transform.Rotation(num18, -EDirX, _hitPlane.Origin);
				}
				if (_whichSelected == CSValueHandleHelper.WSelected.ArrayRz)
				{
					transform2 = Transform.Rotation(num18, -EDirY, _hitPlane.Origin);
				}
				_hitPlane.Transform(transform2);
				PreSet(isMouseMoving: true);
				_arrayObjectTransform = transform2 * _arrayObjectTransform;
				_arrayObjectTransform.TryGetInverse(out _arrayObjectTransformInv);
				PostSet(isMoving: true);
				return true;
			}
			case CSValueHandleHelper.WSelected.MoveX:
			case CSValueHandleHelper.WSelected.MoveY:
			case CSValueHandleHelper.WSelected.MoveZ:
			case CSValueHandleHelper.WSelected.MoveXNegative:
			case CSValueHandleHelper.WSelected.MoveYNegative:
			case CSValueHandleHelper.WSelected.MoveZNegative:
			{
				double num15 = 0.0;
				if (CSValueHandleHelper.MouseButtonShift(e))
				{
					num15 = t2;
				}
				transform2 = Transform.Translation(_hitPlane.XAxis * s2 + _hitPlane.YAxis * num15);
				_hitPlane.Transform(transform2);
				xTrans = transform2;
				transform2 *= ObjectTransform;
				value = s2;
				break;
			}
			case CSValueHandleHelper.WSelected.ArrayMx:
			case CSValueHandleHelper.WSelected.ArrayMy:
			case CSValueHandleHelper.WSelected.ArrayMz:
			{
				double num9 = 0.0;
				if (CSValueHandleHelper.MouseButtonShift(e))
				{
					num9 = t2;
				}
				transform2 = Transform.Translation(_hitPlane.XAxis * s2 + _hitPlane.YAxis * num9);
				_hitPlane.Transform(transform2);
				PreSet(isMouseMoving: true);
				_arrayObjectTransform = transform2 * _arrayObjectTransform;
				_arrayObjectTransform.TryGetInverse(out _arrayObjectTransformInv);
				PostSet(isMoving: true);
				return true;
			}
			case CSValueHandleHelper.WSelected.ScaleX:
			{
				transform2 *= ObjectTransform;
				double num8 = num4 / HandleLengthX;
				value = num8;
				transform2[0, 0] *= num8;
				transform2[1, 0] *= num8;
				transform2[2, 0] *= num8;
				xTrans = Transform.Scale(new Plane(BasePoint, UDirZ), num8, 1.0, 1.0);
				break;
			}
			case CSValueHandleHelper.WSelected.ScaleY:
			{
				transform2 *= ObjectTransform;
				double num7 = num4 / HandleLengthY;
				value = num7;
				transform2[0, 1] *= num7;
				transform2[1, 1] *= num7;
				transform2[2, 1] *= num7;
				xTrans = Transform.Scale(new Plane(BasePoint, UDirZ), 1.0, num7, 1.0);
				break;
			}
			case CSValueHandleHelper.WSelected.ScaleZ:
			{
				transform2 *= ObjectTransform;
				double num6 = num4 / HandleLengthZ;
				value = num6;
				transform2[0, 2] *= num6;
				transform2[1, 2] *= num6;
				transform2[2, 2] *= num6;
				xTrans = Transform.Scale(new Plane(BasePoint, UDirZ), 1.0, 1.0, num6);
				break;
			}
			case CSValueHandleHelper.WSelected.Scale3D:
			{
				transform2 *= ObjectTransform;
				double num5 = num4 / HandleLengthS;
				value = num5;
				transform2[0, 0] *= num5;
				transform2[1, 0] *= num5;
				transform2[2, 0] *= num5;
				transform2[0, 1] *= num5;
				transform2[1, 1] *= num5;
				transform2[2, 1] *= num5;
				transform2[0, 2] *= num5;
				transform2[1, 2] *= num5;
				transform2[2, 2] *= num5;
				xTrans = Transform.Scale(new Plane(BasePoint, UDirZ), num5, num5, num5);
				break;
			}
			default:
				return true;
			}
			if (transform2 == ObjectTransform)
			{
				return true;
			}
			PreSet(isMouseMoving: true);
			ObjectTransform = transform2;
			CSValueHandleHelper.SetLinked(_whichSelected, value, xTrans);
			PostSet(isMoving: true);
			return true;
		}
		return false;
	}

	public void SetLinked(CSValueHandleHelper.WSelected type, double value, Transform xTrans)
	{
		if ((!ShowRotate || (type != CSValueHandleHelper.WSelected.RotateX && type != CSValueHandleHelper.WSelected.RotateY && type != CSValueHandleHelper.WSelected.RotateZ)) && (!ShowMove || (type != CSValueHandleHelper.WSelected.MoveX && type != CSValueHandleHelper.WSelected.MoveY && type != CSValueHandleHelper.WSelected.MoveZ)) && (!ShowMove || (type != CSValueHandleHelper.WSelected.MoveXNegative && type != CSValueHandleHelper.WSelected.MoveYNegative && type != CSValueHandleHelper.WSelected.MoveZNegative)) && (!ShowScale || (type != CSValueHandleHelper.WSelected.ScaleX && type != CSValueHandleHelper.WSelected.ScaleY && type != CSValueHandleHelper.WSelected.ScaleZ && type != CSValueHandleHelper.WSelected.Scale3D)))
		{
			return;
		}
		PreChange("SetLinked");
		if (IsFreehandCommonCenterModeEnabled)
		{
			ObjectTransform = xTrans * ObjectTransform;
		}
		else
		{
			Transform objectTransform = Transform.Identity;
			switch (type)
			{
			case CSValueHandleHelper.WSelected.RotateX:
				objectTransform = Transform.Rotation(value, -UDirZ, BasePoint);
				goto case CSValueHandleHelper.WSelected.RotateY;
			case CSValueHandleHelper.WSelected.RotateY:
			case CSValueHandleHelper.WSelected.RotateZ:
				if (type == CSValueHandleHelper.WSelected.RotateY)
				{
					objectTransform = Transform.Rotation(value, -UDirX, BasePoint);
				}
				if (type == CSValueHandleHelper.WSelected.RotateZ)
				{
					objectTransform = Transform.Rotation(value, -UDirY, BasePoint);
				}
				objectTransform *= ObjectTransform;
				break;
			case CSValueHandleHelper.WSelected.MoveX:
				objectTransform = Transform.Translation(UDirX * value);
				goto case CSValueHandleHelper.WSelected.MoveY;
			case CSValueHandleHelper.WSelected.MoveY:
			case CSValueHandleHelper.WSelected.MoveZ:
			case CSValueHandleHelper.WSelected.MoveXNegative:
			case CSValueHandleHelper.WSelected.MoveYNegative:
			case CSValueHandleHelper.WSelected.MoveZNegative:
				if (type == CSValueHandleHelper.WSelected.MoveY)
				{
					objectTransform = Transform.Translation(UDirY * value);
				}
				if (type == CSValueHandleHelper.WSelected.MoveZ)
				{
					objectTransform = Transform.Translation(UDirZ * value);
				}
				if (type == CSValueHandleHelper.WSelected.MoveXNegative)
				{
					objectTransform = Transform.Translation(UDirX * (0.0 - value));
				}
				if (type == CSValueHandleHelper.WSelected.MoveYNegative)
				{
					objectTransform = Transform.Translation(UDirY * (0.0 - value));
				}
				if (type == CSValueHandleHelper.WSelected.MoveZNegative)
				{
					objectTransform = Transform.Translation(UDirZ * (0.0 - value));
				}
				objectTransform *= ObjectTransform;
				break;
			case CSValueHandleHelper.WSelected.ScaleX:
				objectTransform *= ObjectTransform;
				objectTransform[0, 0] *= value;
				objectTransform[1, 0] *= value;
				objectTransform[2, 0] *= value;
				break;
			case CSValueHandleHelper.WSelected.ScaleY:
				objectTransform *= ObjectTransform;
				objectTransform[0, 1] *= value;
				objectTransform[1, 1] *= value;
				objectTransform[2, 1] *= value;
				break;
			case CSValueHandleHelper.WSelected.ScaleZ:
				objectTransform *= ObjectTransform;
				objectTransform[0, 2] *= value;
				objectTransform[1, 2] *= value;
				objectTransform[2, 2] *= value;
				break;
			case CSValueHandleHelper.WSelected.Scale3D:
				objectTransform *= ObjectTransform;
				objectTransform[0, 0] *= value;
				objectTransform[1, 0] *= value;
				objectTransform[2, 0] *= value;
				objectTransform[0, 1] *= value;
				objectTransform[1, 1] *= value;
				objectTransform[2, 1] *= value;
				objectTransform[0, 2] *= value;
				objectTransform[1, 2] *= value;
				objectTransform[2, 2] *= value;
				break;
			}
			ObjectTransform = objectTransform;
		}
		Change();
	}

	public void MouseUp(MouseCallbackEventArgs e)
	{
		if (!MouseOver)
		{
			return;
		}
		RhinoViewport activeViewport = e.View.ActiveViewport;
		try
		{
			switch (_whichSelected)
			{
			case CSValueHandleHelper.WSelected.ArrayTypeBmp:
				SetArrayMode((CSValueHandleHelper.ArrayDirection)((int)(_arrayDirection + 1) % 4));
				break;
			case CSValueHandleHelper.WSelected.ArrayMirrorBmp:
				SetArrayMirrorMode(!_arrayMirror);
				break;
			case CSValueHandleHelper.WSelected.ArrayShapeBmp:
				SetArrayShapeMode();
				break;
			case CSValueHandleHelper.WSelected.MirrorModeBmp:
			{
				int posX;
				int posY;
				CSBendPoints currentBend = GetCurrentBend(activeViewport, out posX, out posY);
				MSetBendMode(currentBend, (byte)((currentBend._bendMode + 1) % 3));
				_bendNeedsRefresh = true;
				break;
			}
			case CSValueHandleHelper.WSelected.TogglePoint:
				if (CSValueHandleHelper.CurrentHandle != this && CSValueHandleHelper.CurrentHandle != null && (CSValueHandleHelper.MouseButtonControl(e) || CSValueHandleHelper.MouseButtonShift(e)))
				{
					CSValueHandleHelper.MouseButtonShift(e);
					int num = CSValueHandleHelper.SelectedHandles.IndexOf(this);
					if (num >= 0)
					{
						CSValueHandleHelper.SelectedHandles.RemoveAt(num);
					}
					else
					{
						CSValueHandleHelper.SelectedHandles.Add(this);
					}
					CSUtil.Redraw();
					break;
				}
				CSValueHandleHelper.SelectedHandles.Clear();
				if (CSValueHandleHelper.MouseButtonLeft(e))
				{
					if (CSValueHandleHelper.CurrentHandle == this)
					{
						_editMode = (EditMode)((int)(_editMode + 1) % 4);
						if (_editMode == EditMode.Bend && !ShowBend)
						{
							_editMode = (EditMode)((int)(_editMode + 1) % 4);
						}
						if (_editMode == EditMode.None)
						{
							CSValueHandleHelper.CurrentHandle = null;
						}
					}
					else
					{
						CSValueHandleHelper.CurrentHandle = this;
						if (_editMode == EditMode.None)
						{
							_editMode = EditMode.MSR;
						}
					}
					CSUtil.Redraw();
				}
				else
				{
					if (!CSValueHandleHelper.MouseButtonRight(e))
					{
						break;
					}
					if (CSValueHandleHelper.CurrentHandle == this)
					{
						_editMode = (EditMode)((int)(_editMode + 3) % 4);
						if (_editMode == EditMode.Bend && !ShowBend)
						{
							_editMode = (EditMode)((int)(_editMode + 3) % 4);
						}
						if (_editMode == EditMode.None)
						{
							CSValueHandleHelper.CurrentHandle = null;
							if (_editMode == EditMode.None)
							{
								_editMode = EditMode.Array;
							}
						}
					}
					else
					{
						CSValueHandleHelper.CurrentHandle = this;
					}
					CSUtil.Redraw();
				}
				break;
			case CSValueHandleHelper.WSelected.BendPoint0:
			case CSValueHandleHelper.WSelected.BendPoint1:
			case CSValueHandleHelper.WSelected.BendPoint2:
				_bendNeedsRefresh = true;
				break;
			case CSValueHandleHelper.WSelected.ArrayEnd:
			case CSValueHandleHelper.WSelected.ArrayCount:
			case CSValueHandleHelper.WSelected.ArrayStart:
				break;
			}
		}
		finally
		{
			CSUtil.RemoveMouseDown();
			PostSet(isMoving: false, isQuickRegen: false);
		}
	}

	private Point3d MsrMovePoint(double l, CSValueHandleHelper.WSelected selId)
	{
		double num = 80.0 / l;
		double num2 = 160.0 / l;
		double num3 = 120.0 / l;
		double num4 = 30.0 / l;
		double num5 = 30.0 / l;
		double num6 = 23.0 / l;
		return selId switch
		{
			CSValueHandleHelper.WSelected.MoveX => BasePoint + UDirX * (HandleLengthX + num), 
			CSValueHandleHelper.WSelected.MoveY => BasePoint + UDirY * (HandleLengthY + num), 
			CSValueHandleHelper.WSelected.MoveZ => BasePoint + UDirZ * (HandleLengthZ + num), 
			CSValueHandleHelper.WSelected.MoveXNegative => BasePoint - UDirX * (HandleLengthXMin + num), 
			CSValueHandleHelper.WSelected.MoveYNegative => BasePoint - UDirY * (HandleLengthYMin + num), 
			CSValueHandleHelper.WSelected.MoveZNegative => BasePoint - UDirZ * (HandleLengthZMin + num), 
			CSValueHandleHelper.WSelected.RotateX => BasePoint - UDirX * (HandleLengthXMin + num5), 
			CSValueHandleHelper.WSelected.RotateY => BasePoint - UDirY * (HandleLengthYMin + num5), 
			CSValueHandleHelper.WSelected.RotateZ => BasePoint - UDirZ * (HandleLengthZMin + num5), 
			CSValueHandleHelper.WSelected.ScaleX => BasePoint + UDirX * (HandleLengthX + num4), 
			CSValueHandleHelper.WSelected.ScaleY => BasePoint + UDirY * (HandleLengthY + num4), 
			CSValueHandleHelper.WSelected.ScaleZ => BasePoint + UDirZ * (HandleLengthZ + num4), 
			CSValueHandleHelper.WSelected.Scale3D => BasePoint + UDirS * (HandleLengthS + num4), 
			CSValueHandleHelper.WSelected.ArrayMx => ArrayCenter() - EDirX * num2, 
			CSValueHandleHelper.WSelected.ArrayMy => ArrayCenter() - EDirY * num2, 
			CSValueHandleHelper.WSelected.ArrayMz => ArrayCenter() + EDirZ * num2, 
			CSValueHandleHelper.WSelected.ArrayRx => ArrayCenter() - EDirX * num3, 
			CSValueHandleHelper.WSelected.ArrayRy => ArrayCenter() - EDirY * num3, 
			CSValueHandleHelper.WSelected.ArrayRz => ArrayCenter() + EDirZ * num3, 
			CSValueHandleHelper.WSelected.LinkedMx => UDirP + UDirX * num6, 
			CSValueHandleHelper.WSelected.LinkedMy => UDirP + UDirY * num6, 
			CSValueHandleHelper.WSelected.LinkedMz => UDirP + UDirZ * num6, 
			CSValueHandleHelper.WSelected.LinkedMmx => UDirP - UDirX * num6, 
			CSValueHandleHelper.WSelected.LinkedMmy => UDirP - UDirY * num6, 
			CSValueHandleHelper.WSelected.LinkedMmz => UDirP - UDirZ * num6, 
			_ => Point3d.Unset, 
		};
	}

	private void MsrMoveDirection(CSValueHandleHelper.WSelected selId, out Vector3d moveDirection, out Vector3d upDirection, out Vector3d outDirection)
	{
		moveDirection = Vector3d.Unset;
		upDirection = Vector3d.Unset;
		outDirection = Vector3d.Unset;
		switch (selId)
		{
		case CSValueHandleHelper.WSelected.MoveX:
		case CSValueHandleHelper.WSelected.LinkedMx:
			moveDirection = UDirX;
			upDirection = UDirY;
			outDirection = UDirZ;
			break;
		case CSValueHandleHelper.WSelected.MoveY:
		case CSValueHandleHelper.WSelected.LinkedMy:
			moveDirection = UDirY;
			upDirection = UDirZ;
			outDirection = UDirX;
			break;
		case CSValueHandleHelper.WSelected.MoveZ:
		case CSValueHandleHelper.WSelected.LinkedMz:
			moveDirection = UDirZ;
			upDirection = UDirX;
			outDirection = UDirY;
			break;
		case CSValueHandleHelper.WSelected.MoveXNegative:
		case CSValueHandleHelper.WSelected.LinkedMmx:
			moveDirection = -UDirX;
			upDirection = UDirY;
			outDirection = UDirZ;
			break;
		case CSValueHandleHelper.WSelected.MoveYNegative:
		case CSValueHandleHelper.WSelected.LinkedMmy:
			moveDirection = -UDirY;
			upDirection = UDirZ;
			outDirection = UDirX;
			break;
		case CSValueHandleHelper.WSelected.MoveZNegative:
		case CSValueHandleHelper.WSelected.LinkedMmz:
			moveDirection = -UDirZ;
			upDirection = UDirX;
			outDirection = UDirY;
			break;
		case CSValueHandleHelper.WSelected.RotateX:
			moveDirection = -UDirX;
			upDirection = UDirY;
			break;
		case CSValueHandleHelper.WSelected.RotateY:
			moveDirection = -UDirY;
			upDirection = UDirZ;
			break;
		case CSValueHandleHelper.WSelected.RotateZ:
			moveDirection = -UDirZ;
			upDirection = UDirX;
			break;
		case CSValueHandleHelper.WSelected.ScaleX:
			moveDirection = UDirX;
			upDirection = UDirY;
			break;
		case CSValueHandleHelper.WSelected.ScaleY:
			moveDirection = UDirY;
			upDirection = UDirZ;
			break;
		case CSValueHandleHelper.WSelected.ScaleZ:
			moveDirection = UDirZ;
			upDirection = UDirX;
			break;
		case CSValueHandleHelper.WSelected.Scale3D:
			moveDirection = UDirS;
			upDirection = UDirZ;
			break;
		case CSValueHandleHelper.WSelected.ArrayMx:
			moveDirection = -EDirX;
			upDirection = EDirY;
			outDirection = EDirZ;
			break;
		case CSValueHandleHelper.WSelected.ArrayMy:
			moveDirection = -EDirY;
			upDirection = EDirZ;
			outDirection = EDirX;
			break;
		case CSValueHandleHelper.WSelected.ArrayMz:
			moveDirection = EDirZ;
			upDirection = EDirX;
			outDirection = EDirY;
			break;
		case CSValueHandleHelper.WSelected.ArrayRx:
			moveDirection = -EDirX;
			upDirection = EDirY;
			break;
		case CSValueHandleHelper.WSelected.ArrayRy:
			moveDirection = -EDirY;
			upDirection = EDirZ;
			break;
		case CSValueHandleHelper.WSelected.ArrayRz:
			moveDirection = EDirZ;
			upDirection = -EDirX;
			break;
		case CSValueHandleHelper.WSelected.TogglePoint:
		case CSValueHandleHelper.WSelected.BendPoint0:
		case CSValueHandleHelper.WSelected.BendPoint1:
		case CSValueHandleHelper.WSelected.BendPoint2:
		case CSValueHandleHelper.WSelected.MirrorModeBmp:
		case CSValueHandleHelper.WSelected.ArrayEnd:
		case CSValueHandleHelper.WSelected.ArrayCount:
		case CSValueHandleHelper.WSelected.ArrayStart:
		case CSValueHandleHelper.WSelected.ArrayTypeBmp:
		case CSValueHandleHelper.WSelected.ArrayMirrorBmp:
		case CSValueHandleHelper.WSelected.ArrayShapeBmp:
			break;
		}
	}

	private void TestHitPoint(Line mouseLine, RhinoViewport vp, CSValueHandleHelper.WSelected selId, ref double distance)
	{
		_rotateAngle = 0.0;
		Vector3d direction = mouseLine.Direction;
		direction.Unitize();
		MsrMoveDirection(selId, out var moveDirection, out var upDirection, out var outDirection);
		if (!(Math.Abs(Math.Abs(moveDirection * direction)) < 0.99))
		{
			return;
		}
		Point3d point3d = (CSValueHandleHelper.IsArrayMove(selId) ? ArrayCenter() : ((!CSValueHandleHelper.IsLinkedMove(selId)) ? BasePoint : UDirP));
		double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(vp, point3d);
		Point3d point3d2 = MsrMovePoint(worldToScreenScale, selId);
		double num = mouseLine.ClosestPoint(point3d2, limitToFiniteSegment: false).DistanceTo(point3d2);
		if (!(num < CSValueHandleHelper.SelectionSizePixels / worldToScreenScale) || !(num < distance))
		{
			return;
		}
		MouseOver = true;
		distance = num;
		_whichSelected = selId;
		moveDirection.Unitize();
		upDirection.Unitize();
		switch (CSValueHandleHelper.MsrMode(selId))
		{
		case CSValueHandleHelper.WMode.Rotate:
		{
			_hitPlane = new Plane(point3d, moveDirection, upDirection);
			if (Intersection.LinePlane(mouseLine, _hitPlane, out var lineParameter2))
			{
				Point3d testPoint2 = mouseLine.PointAt(lineParameter2);
				_hitPlane.ClosestParameter(testPoint2, out var s2, out var t2);
				Transform xform2 = Transform.Rotation(CSValueHandleHelper.AngleOfPointD(t2, s2), _hitPlane.Normal, _hitPlane.Origin);
				_hitPlane.Transform(xform2);
			}
			break;
		}
		case CSValueHandleHelper.WMode.Move:
		{
			Vector3d cameraDirection2 = CSValueHandleHelper.GetCameraDirection(vp);
			double value = cameraDirection2 * upDirection;
			double value2 = cameraDirection2 * outDirection;
			Vector3d yDirection2 = ((Math.Abs(value) < Math.Abs(value2)) ? upDirection : outDirection);
			yDirection2.Unitize();
			_hitPlane = new Plane(point3d2, moveDirection, yDirection2);
			if (Intersection.LinePlane(mouseLine, _hitPlane, out var lineParameter))
			{
				Point3d testPoint = mouseLine.PointAt(lineParameter);
				_hitPlane.ClosestParameter(testPoint, out var s, out var t);
				Transform xform = Transform.Translation(_hitPlane.XAxis * s + _hitPlane.YAxis * t);
				_hitPlane.Transform(xform);
			}
			break;
		}
		case CSValueHandleHelper.WMode.Scale:
		{
			Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(vp);
			Vector3d yDirection = Vector3d.CrossProduct(moveDirection, cameraDirection);
			yDirection.Unitize();
			double num2 = 20.0 / worldToScreenScale;
			_hitPlane = new Plane(point3d + moveDirection * num2, moveDirection, yDirection);
			break;
		}
		}
	}

	private void TestHitSphere(Line mouseLine, RhinoViewport vp, Point3d bp, CSValueHandleHelper.WSelected p, ref double distance)
	{
		double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(vp, bp);
		double num = mouseLine.ClosestPoint(bp, limitToFiniteSegment: false).DistanceTo(bp);
		if (num < CSValueHandleHelper.SelectionSizePixels / worldToScreenScale && num < distance)
		{
			MouseOver = true;
			distance = num;
			_whichSelected = p;
		}
	}
}
