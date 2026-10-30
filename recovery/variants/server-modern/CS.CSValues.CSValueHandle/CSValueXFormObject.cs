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
using Rhino.Runtime;
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
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			_txtBoxPoint = txtBoxPoint;
			_txtBoxText = txtBoxText;
			_txtBoxColor = txtBoxColor;
		}

		public void Drawtxt(DisplayPipeline displayPipeline)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			Transform val = CSValueHandleHelper.WorldToScreen(displayPipeline);
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(_txtBoxPoint);
			((Point3d)(ref val2)).Transform(val);
			Rectangle rectangle = displayPipeline.Measure2dText(_txtBoxText, new Point2d(0.0, 0.0), false, 0.0, 12, "Arial");
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
				DisplayBitmap val3 = new DisplayBitmap(bitmap);
				try
				{
					displayPipeline.DrawBitmap(val3, (int)(((Point3d)(ref val2)).X - (double)bitmap.Width / 2.0), (int)(((Point3d)(ref val2)).Y - (double)bitmap.Height / 2.0 + 4.0));
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			displayPipeline.Draw2dText(_txtBoxText, Color.White, new Point2d(((Point3d)(ref val2)).X - (double)rectangle.Width / 2.0, ((Point3d)(ref val2)).Y + (double)rectangle.Height / 2.0 + 2.0), false, 12, "Arial");
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

	private Point3d EPoint => new Point3d(((Transform)(ref _arrayObjectTransform))[0, 3], ((Transform)(ref _arrayObjectTransform))[1, 3], ((Transform)(ref _arrayObjectTransform))[2, 3]);

	private Vector3d EDirX
	{
		get
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			Vector3d result = default(Vector3d);
			((Vector3d)(ref result))._002Ector(((Transform)(ref _arrayObjectTransform))[0, 0], ((Transform)(ref _arrayObjectTransform))[1, 0], ((Transform)(ref _arrayObjectTransform))[2, 0]);
			((Vector3d)(ref result)).Unitize();
			return result;
		}
	}

	private Vector3d EDirY
	{
		get
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			Vector3d result = default(Vector3d);
			((Vector3d)(ref result))._002Ector(((Transform)(ref _arrayObjectTransform))[0, 1], ((Transform)(ref _arrayObjectTransform))[1, 1], ((Transform)(ref _arrayObjectTransform))[2, 1]);
			((Vector3d)(ref result)).Unitize();
			return result;
		}
	}

	private Vector3d EDirZ
	{
		get
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			Vector3d result = default(Vector3d);
			((Vector3d)(ref result))._002Ector(((Transform)(ref _arrayObjectTransform))[0, 2], ((Transform)(ref _arrayObjectTransform))[1, 2], ((Transform)(ref _arrayObjectTransform))[2, 2]);
			((Vector3d)(ref result)).Unitize();
			return result;
		}
	}

	private Point3d BasePoint
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			Transform objectTransform = ObjectTransform;
			double num = ((Transform)(ref objectTransform))[0, 3];
			objectTransform = ObjectTransform;
			double num2 = ((Transform)(ref objectTransform))[1, 3];
			objectTransform = ObjectTransform;
			return new Point3d(num, num2, ((Transform)(ref objectTransform))[2, 3]);
		}
	}

	private Vector3d UDirX
	{
		get
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			Transform objectTransform = ObjectTransform;
			double num = ((Transform)(ref objectTransform))[0, 0];
			objectTransform = ObjectTransform;
			double num2 = ((Transform)(ref objectTransform))[1, 0];
			objectTransform = ObjectTransform;
			Vector3d result = default(Vector3d);
			((Vector3d)(ref result))._002Ector(num, num2, ((Transform)(ref objectTransform))[2, 0]);
			((Vector3d)(ref result)).Unitize();
			return result;
		}
	}

	private Vector3d UDirY
	{
		get
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			Transform objectTransform = ObjectTransform;
			double num = ((Transform)(ref objectTransform))[0, 1];
			objectTransform = ObjectTransform;
			double num2 = ((Transform)(ref objectTransform))[1, 1];
			objectTransform = ObjectTransform;
			Vector3d result = default(Vector3d);
			((Vector3d)(ref result))._002Ector(num, num2, ((Transform)(ref objectTransform))[2, 1]);
			((Vector3d)(ref result)).Unitize();
			return result;
		}
	}

	private Vector3d UDirZ
	{
		get
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			Transform objectTransform = ObjectTransform;
			double num = ((Transform)(ref objectTransform))[0, 2];
			objectTransform = ObjectTransform;
			double num2 = ((Transform)(ref objectTransform))[1, 2];
			objectTransform = ObjectTransform;
			Vector3d result = default(Vector3d);
			((Vector3d)(ref result))._002Ector(num, num2, ((Transform)(ref objectTransform))[2, 2]);
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
			((Point3d)(ref val)).Transform(ObjectTransform);
			Vector3d result = default(Vector3d);
			((Vector3d)(ref result))._002Ector(val - BasePoint);
			((Vector3d)(ref result)).Unitize();
			return result;
		}
	}

	private Point3d UDirP
	{
		get
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			if (!((BoundingBox)(ref _bb)).IsValid)
			{
				Point3d origin = Point3d.Origin;
				((Point3d)(ref origin)).Transform(ObjectTransform);
				return origin;
			}
			Point3d val = ((BoundingBox)(ref _bb)).Min;
			double x = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref _bb)).Max;
			double y = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref _bb)).Max;
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(x, y, ((Point3d)(ref val)).Z);
			val2 *= 0.5;
			if (((Point3d)(ref val2)).X > -1.0)
			{
				((Point3d)(ref val2)).X = -1.0;
			}
			if (((Point3d)(ref val2)).Y < 1.0)
			{
				((Point3d)(ref val2)).Y = 1.0;
			}
			if (((Point3d)(ref val2)).Z < 1.0)
			{
				((Point3d)(ref val2)).Z = 1.0;
			}
			((Point3d)(ref val2)).Transform(ObjectTransform);
			return val2;
		}
	}

	private double HandleLengthX
	{
		get
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (!((BoundingBox)(ref _bb)).IsValid)
			{
				return 5.0;
			}
			Point3d max = ((BoundingBox)(ref _bb)).Max;
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(((Point3d)(ref max)).X, 0.0, 0.0);
			((Point3d)(ref val)).Transform(ObjectTransform);
			Vector3d val2 = val - BasePoint;
			return ((Vector3d)(ref val2)).Length;
		}
	}

	private double HandleLengthY
	{
		get
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (!((BoundingBox)(ref _bb)).IsValid)
			{
				return 5.0;
			}
			Point3d max = ((BoundingBox)(ref _bb)).Max;
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(0.0, ((Point3d)(ref max)).Y, 0.0);
			((Point3d)(ref val)).Transform(ObjectTransform);
			Vector3d val2 = val - BasePoint;
			return ((Vector3d)(ref val2)).Length;
		}
	}

	private double HandleLengthZ
	{
		get
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (!((BoundingBox)(ref _bb)).IsValid)
			{
				return 5.0;
			}
			Point3d max = ((BoundingBox)(ref _bb)).Max;
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(0.0, 0.0, ((Point3d)(ref max)).Z);
			((Point3d)(ref val)).Transform(ObjectTransform);
			Vector3d val2 = val - BasePoint;
			return ((Vector3d)(ref val2)).Length;
		}
	}

	private double HandleLengthS
	{
		get
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			if (!((BoundingBox)(ref _bb)).IsValid)
			{
				return 5.0;
			}
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(((BoundingBox)(ref _bb)).Max);
			((Point3d)(ref val)).Transform(ObjectTransform);
			Vector3d val2 = val - BasePoint;
			return ((Vector3d)(ref val2)).Length;
		}
	}

	private double HandleLengthXMin
	{
		get
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (!((BoundingBox)(ref _bb)).IsValid)
			{
				return 5.0;
			}
			Point3d min = ((BoundingBox)(ref _bb)).Min;
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(((Point3d)(ref min)).X, 0.0, 0.0);
			((Point3d)(ref val)).Transform(ObjectTransform);
			Vector3d val2 = val - BasePoint;
			return ((Vector3d)(ref val2)).Length;
		}
	}

	private double HandleLengthYMin
	{
		get
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (!((BoundingBox)(ref _bb)).IsValid)
			{
				return 5.0;
			}
			Point3d min = ((BoundingBox)(ref _bb)).Min;
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(0.0, ((Point3d)(ref min)).Y, 0.0);
			((Point3d)(ref val)).Transform(ObjectTransform);
			Vector3d val2 = val - BasePoint;
			return ((Vector3d)(ref val2)).Length;
		}
	}

	private double HandleLengthZMin
	{
		get
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (!((BoundingBox)(ref _bb)).IsValid)
			{
				return 5.0;
			}
			Point3d min = ((BoundingBox)(ref _bb)).Min;
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(0.0, 0.0, ((Point3d)(ref min)).Z);
			((Point3d)(ref val)).Transform(ObjectTransform);
			Vector3d val2 = val - BasePoint;
			return ((Vector3d)(ref val2)).Length;
		}
	}

	public bool IsFreehandCommonCenterModeEnabled { get; set; }

	private Transform ObjectTransform
	{
		get
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (((Transform)(ref _arrayObjectTransform)).IsValid)
			{
				return _arrayObjectTransform * _value;
			}
			return _value;
		}
		set
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (!((Transform)(ref _arrayObjectTransform)).IsValid)
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

	private int ObjSizeX
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			Vector3d diagonal = ((BoundingBox)(ref _bbOriginal)).Diagonal;
			double x = ((Vector3d)(ref diagonal)).X;
			Transform objectTransform = ObjectTransform;
			return (int)(x * ((Transform)(ref objectTransform))[0, 0] / 2.0 + 3.0);
		}
	}

	private int ObjSizeY
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			Vector3d diagonal = ((BoundingBox)(ref _bbOriginal)).Diagonal;
			double y = ((Vector3d)(ref diagonal)).Y;
			Transform objectTransform = ObjectTransform;
			return (int)(y * ((Transform)(ref objectTransform))[1, 1] / 2.0 + 3.0);
		}
	}

	private int ObjSizeZ
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			Vector3d diagonal = ((BoundingBox)(ref _bbOriginal)).Diagonal;
			double z = ((Vector3d)(ref diagonal)).Z;
			Transform objectTransform = ObjectTransform;
			return (int)(z * ((Transform)(ref objectTransform))[2, 2] / 2.0 + 3.0);
		}
	}

	public CSValueXFormObject(string identifier)
		: base(identifier)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0800: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Unknown result type (might be due to invalid IL or missing references)
		//IL_0765: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_092b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0936: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0baa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0985: Unknown result type (might be due to invalid IL or missing references)
		//IL_098e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_099a: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
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
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					//IL_0005: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Unknown result type (might be due to invalid IL or missing references)
					BoundingBox boundingBox = BoundingBox.Unset;
					if (_.GetBoundingBox(ref boundingBox))
					{
						((BoundingBox)(ref _bbOriginal)).Union(boundingBox);
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
			if (!((BoundingBox)(ref _bbOriginal)).IsValid)
			{
				_objOriginal = null;
				_objMeshOriginal = null;
				_bbOriginal = new BoundingBox(new Point3d(-5.0, -5.0, -5.0), new Point3d(5.0, 5.0, 5.0));
				return base.Update();
			}
			int value = 0;
			InAutoCenter.GetInteger(ref value);
			Point3d val2;
			switch (value)
			{
			case 1:
				if (_objOriginal is ICSTransformable iCSTransformable2)
				{
					Transform val4 = Transform.Translation(-new Vector3d(((BoundingBox)(ref _bbOriginal)).Center));
					iCSTransformable2.ApplyTransform(val4);
					((BoundingBox)(ref _bbOriginal)).Transform(val4);
				}
				break;
			case 2:
				if (_objOriginal is ICSTransformable iCSTransformable)
				{
					Vector3d val = default(Vector3d);
					((Vector3d)(ref val))._002Ector(((BoundingBox)(ref _bbOriginal)).Center);
					val2 = ((BoundingBox)(ref _bbOriginal)).Min;
					((Vector3d)(ref val)).Z = ((Point3d)(ref val2)).Z;
					Transform val3 = Transform.Translation(-val);
					iCSTransformable.ApplyTransform(val3);
					((BoundingBox)(ref _bbOriginal)).Transform(val3);
				}
				break;
			}
			MeshingParameters coarse = MeshingParameters.Coarse;
			coarse.GridMinCount = 16;
			coarse.MaximumEdgeLength = 0.3;
			_objMeshOriginal?.Dispose();
			Mesh val5 = _objOriginal.MeshObject(coarse, productionMeshes: false, useDisplayMesh: false);
			if (val5 == null)
			{
				_objOriginal = null;
				_objMeshOriginal = null;
				_bbOriginal = new BoundingBox(new Point3d(-5.0, -5.0, -5.0), new Point3d(5.0, 5.0, 5.0));
				return base.Update();
			}
			_objMeshOriginal = new CSOnGeometryData((GeometryBase)(object)val5);
			if (_bendXz == null)
			{
				_bendXz = new CSBendPoints(Point3d.Origin, new Point3d(10.0, 0.0, 0.0), new Point3d(9.0, 0.0, 0.0), 1);
				_bendXy = new CSBendPoints(Point3d.Origin, new Point3d(10.0, 0.0, 0.0), new Point3d(9.0, 0.0, 0.0), 1);
				_bendYz = new CSBendPoints(Point3d.Origin, new Point3d(0.0, 10.0, 0.0), new Point3d(0.0, 9.0, 0.0), 1);
				Point3d endPoint = _bendXy.EndPoint;
				val2 = ((BoundingBox)(ref _bbOriginal)).Max;
				((Point3d)(ref endPoint)).X = ((Point3d)(ref val2)).X;
				_bendXy.EndPoint = endPoint;
				endPoint = _bendXz.EndPoint;
				val2 = ((BoundingBox)(ref _bbOriginal)).Max;
				((Point3d)(ref endPoint)).X = ((Point3d)(ref val2)).X;
				_bendXz.EndPoint = endPoint;
				endPoint = _bendYz.EndPoint;
				val2 = ((BoundingBox)(ref _bbOriginal)).Max;
				((Point3d)(ref endPoint)).Y = ((Point3d)(ref val2)).Y;
				_bendYz.EndPoint = endPoint;
				Point3d bendPoint = _bendXy.BendPoint;
				val2 = ((BoundingBox)(ref _bbOriginal)).Max;
				((Point3d)(ref bendPoint)).X = ((Point3d)(ref val2)).X * 0.9;
				_bendXy.BendPoint = bendPoint;
				bendPoint = _bendXz.BendPoint;
				val2 = ((BoundingBox)(ref _bbOriginal)).Max;
				((Point3d)(ref bendPoint)).X = ((Point3d)(ref val2)).X * 0.9;
				_bendXz.BendPoint = bendPoint;
				bendPoint = _bendYz.BendPoint;
				val2 = ((BoundingBox)(ref _bbOriginal)).Max;
				((Point3d)(ref bendPoint)).Y = ((Point3d)(ref val2)).Y * 0.9;
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
				CSBendMorph cSBendMorph = new CSBendMorph(_bendXz);
				((SpaceMorph)cSBendMorph).Tolerance = 0.05;
				CSBendMorph spaceMorph = cSBendMorph;
				_objOutput.Morph((SpaceMorph)(object)spaceMorph);
				_bendXz.Changed();
			}
			if (_bendXy.IsBending())
			{
				CSBendMorph cSBendMorph2 = new CSBendMorph(_bendXy);
				((SpaceMorph)cSBendMorph2).Tolerance = 0.05;
				CSBendMorph spaceMorph2 = cSBendMorph2;
				_objOutput.Morph((SpaceMorph)(object)spaceMorph2);
				_bendXy.Changed();
			}
			if (_bendYz.IsBending())
			{
				CSBendMorph cSBendMorph3 = new CSBendMorph(_bendYz);
				((SpaceMorph)cSBendMorph3).Tolerance = 0.05;
				CSBendMorph spaceMorph3 = cSBendMorph3;
				_objOutput.Morph((SpaceMorph)(object)spaceMorph3);
				_bendYz.Changed();
			}
			_objOutput.GetBoundingBox(ref _bb);
			if (!((BoundingBox)(ref _bb)).IsValid)
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
			Transform transform = Transform.Mirror(Point3d.Origin, new Vector3d(1.0, 0.0, 0.0));
			iCSTransformable3?.ApplyTransform(transform);
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
				double num4 = default(double);
				crvFollow.ClosestPoint(pStart, ref num4);
				crvFollow.ChangeClosedCurveSeam(num4);
				Plane val6 = default(Plane);
				((Plane)(ref val6))._002Ector(crvFollow.PointAtStart, GetAverageTangency(crvFollow, 0.0), up);
				double length = crvFollow.GetLength();
				double num6;
				if (_arrayTotalAngle <= 6.2482787221397)
				{
					double num5 = default(double);
					crvFollow.ClosestPoint(pEnd, ref num5);
					Curve obj = crvFollow;
					Interval domain = crvFollow.Domain;
					num6 = obj.GetLength(new Interval(((Interval)(ref domain)).Min, num5));
				}
				else
				{
					num6 = length;
				}
				double num7 = num6 / length / (double)num3;
				double num10 = default(double);
				if (_arrayMirror)
				{
					Plane val7 = default(Plane);
					for (int num8 = num3; num8 > 0; num8--)
					{
						double num9 = 1.0 - num7 * (double)num8;
						crvFollow.NormalizedLengthParameter(num9, ref num10);
						((Plane)(ref val7))._002Ector(crvFollow.PointAt(num10), GetAverageTangency(crvFollow, num9), up);
						Transform transform2 = Transform.PlaneToPlane(val6, val7);
						CSData cSData2 = cSData.Duplicate();
						(cSData2 as ICSTransformable)?.ApplyTransform(transform2);
						cSDataArray3.Append(cSData2);
					}
				}
				Plane val8 = default(Plane);
				for (int num11 = 0; num11 <= num2; num11++)
				{
					double num12 = num7 * (double)num11;
					crvFollow.NormalizedLengthParameter(num12, ref num10);
					((Plane)(ref val8))._002Ector(crvFollow.PointAt(num10), GetAverageTangency(crvFollow, num12), up);
					Transform transform3 = Transform.PlaneToPlane(val6, val8);
					CSData cSData3 = cSData.Duplicate();
					(cSData3 as ICSTransformable)?.ApplyTransform(transform3);
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
				Transform transform4 = Transform.Mirror(Point3d.Origin, new Vector3d(1.0, 0.0, 0.0));
				iCSTransformable4.ApplyTransform(transform4);
			}
			OutTransformedObjects1.SetData(cSData4);
		}
		if (_outputMirrorMode == 2 || _outputMirrorMode == 4)
		{
			CSData cSData5 = cSData.Duplicate();
			if (cSData5 is ICSTransformable iCSTransformable5)
			{
				Transform transform5 = Transform.Mirror(Point3d.Origin, new Vector3d(1.0, 0.0, 0.0));
				iCSTransformable5.ApplyTransform(transform5);
				Transform transform6 = Transform.Mirror(Point3d.Origin, new Vector3d(0.0, 1.0, 0.0));
				iCSTransformable5.ApplyTransform(transform6);
			}
			OutTransformedObjects2.SetData(cSData5);
		}
		if (_outputMirrorMode == 3 || _outputMirrorMode == 4)
		{
			CSData cSData6 = cSData.Duplicate();
			if (cSData6 is ICSTransformable iCSTransformable6)
			{
				Transform transform7 = Transform.Mirror(Point3d.Origin, new Vector3d(0.0, 1.0, 0.0));
				iCSTransformable6.ApplyTransform(transform7);
			}
			OutTransformedObjects3.SetData(cSData6);
		}
		return base.Update();
	}

	private static Vector3d GetAverageTangency(Curve crv, double normParam)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
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
		double num3 = default(double);
		crv.NormalizedLengthParameter(num, ref num3);
		double num4 = default(double);
		crv.NormalizedLengthParameter(num2, ref num4);
		Vector3d val = crv.TangentAt(num3);
		Vector3d val2 = crv.TangentAt(num4);
		Vector3d result = val + val2;
		((Vector3d)(ref result)).Unitize();
		return result;
	}

	public override bool SetUndo(CSObject setObject)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
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
				archive.WriteDouble(((Transform)(ref t))[i, j]);
			}
		}
	}

	private static Transform Read(OnBinaryArchive archive)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		Transform identity = Transform.Identity;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				double num = 0.0;
				archive.ReadDouble(ref num);
				((Transform)(ref identity))[i, j] = num;
			}
		}
		return identity;
	}

	public override void WriteJson(ref JObject archive)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
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
			((Transform)(ref _arrayObjectTransform)).TryGetInverse(ref _arrayObjectTransformInv);
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (!(newP0 == bendPoint.BasePoint))
		{
			PreSet();
			bendPoint.BasePoint = newP0;
			PostSet();
		}
	}

	private void MSetP1(CSBendPoints bendPoint, Point3d newP1)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (!(newP1 == bendPoint.EndPoint))
		{
			PreSet();
			bendPoint.EndPoint = new Point3d(newP1);
			PostSet();
		}
	}

	private void MSetP2(CSBendPoints bendPoint, Point3d newP2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int num2 = 0;
		Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(vp);
		((Vector3d)(ref cameraDirection)).Unitize();
		Point3d center = ((BoundingBox)(ref _bbOriginal)).Center;
		((Point3d)(ref center)).Transform(ObjectTransform);
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(1.0, 0.0, 0.0);
		Vector3d val2 = default(Vector3d);
		((Vector3d)(ref val2))._002Ector(0.0, 1.0, 0.0);
		Vector3d val3 = default(Vector3d);
		((Vector3d)(ref val3))._002Ector(0.0, 0.0, 1.0);
		((Vector3d)(ref val)).Transform(ObjectTransform);
		((Vector3d)(ref val)).Unitize();
		((Vector3d)(ref val2)).Transform(ObjectTransform);
		((Vector3d)(ref val2)).Unitize();
		((Vector3d)(ref val3)).Transform(ObjectTransform);
		((Vector3d)(ref val3)).Unitize();
		double num3 = Math.Abs(cameraDirection * val);
		double num4 = Math.Abs(cameraDirection * val2);
		double num5 = Math.Abs(cameraDirection * val3);
		Vector3d val4 = default(Vector3d);
		((Vector3d)(ref val4))._002Ector(1.0, 0.0, 0.0);
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(1.0, 0.0, 0.0);
		CSBendPoints result = null;
		double num6 = 0.0;
		if (num3 > num6)
		{
			val4 = val2;
			val5 = val3;
			num6 = num3;
			result = _bendYz;
			num = ObjSizeY;
			num2 = ObjSizeZ;
		}
		if (num4 > num6)
		{
			val4 = val;
			val5 = val3;
			num6 = num4;
			result = _bendXz;
			num = ObjSizeX;
			num2 = ObjSizeZ;
		}
		if (num5 > num6)
		{
			val4 = val;
			val5 = val2;
			result = _bendXy;
			num = ObjSizeX;
			num2 = ObjSizeY;
		}
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector(-(val4 * (double)(num - 1)) + val5 * (double)(num2 - 1) + center);
		Transform val7 = CSValueHandleHelper.WorldToScreen(vp);
		((Point3d)(ref val6)).Transform(val7);
		double num7 = 32.0 * CSValueHandleHelper.ScreenScaleOverride;
		posX = (int)(((Point3d)(ref val6)).X - num7 / 2.0);
		posY = (int)(((Point3d)(ref val6)).Y - num7 / 2.0);
		return result;
	}

	private void DrawBendBackGroundGrid(DisplayPipeline displayPipeline)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		if (!((BoundingBox)(ref _bb)).IsValid)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(displayPipeline);
		Point3d center = ((BoundingBox)(ref _bbOriginal)).Center;
		((Point3d)(ref center)).Transform(ObjectTransform);
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(1.0, 0.0, 0.0);
		Vector3d val2 = default(Vector3d);
		((Vector3d)(ref val2))._002Ector(0.0, 1.0, 0.0);
		Vector3d val3 = default(Vector3d);
		((Vector3d)(ref val3))._002Ector(0.0, 0.0, 1.0);
		((Vector3d)(ref val)).Transform(ObjectTransform);
		((Vector3d)(ref val)).Unitize();
		((Vector3d)(ref val2)).Transform(ObjectTransform);
		((Vector3d)(ref val2)).Unitize();
		((Vector3d)(ref val3)).Transform(ObjectTransform);
		((Vector3d)(ref val3)).Unitize();
		double num3 = Math.Abs(cameraDirection * val);
		double num4 = Math.Abs(cameraDirection * val2);
		double num5 = Math.Abs(cameraDirection * val3);
		Vector3d val4 = Vector3d.Unset;
		Vector3d val5 = Vector3d.Unset;
		double num6 = 0.0;
		if (num3 > num6)
		{
			val4 = val2;
			val5 = val3;
			num6 = num3;
			num = ObjSizeY;
			num2 = ObjSizeZ;
		}
		if (num4 > num6)
		{
			val4 = val;
			val5 = val3;
			num6 = num4;
			num = ObjSizeX;
			num2 = ObjSizeZ;
		}
		if (num5 > num6)
		{
			val4 = val;
			val5 = val2;
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
			displayPipeline.DrawLine(new Point3d(val4 * (double)i - val5 * (double)num2 + center), new Point3d(val4 * (double)i + val5 * (double)num2 + center), color, 1);
			displayPipeline.DrawLine(new Point3d(-(val4 * (double)i) - val5 * (double)num2 + center), new Point3d(-(val4 * (double)i) + val5 * (double)num2 + center), color, 1);
		}
		for (int j = 0; j <= num2; j++)
		{
			Color color2 = Color.FromArgb(255, 208, 208, 208);
			if (j % 5 == 0 || j == num2)
			{
				color2 = Color.FromArgb(255, 160, 160, 160);
			}
			displayPipeline.DrawLine(new Point3d(val5 * (double)j - val4 * (double)num + center), new Point3d(val5 * (double)j + val4 * (double)num + center), color2, 1);
			displayPipeline.DrawLine(new Point3d(-(val5 * (double)j) - val4 * (double)num + center), new Point3d(-(val5 * (double)j) + val4 * (double)num + center), color2, 1);
		}
		displayPipeline.ModelTransform = modelTransform;
	}

	private void DrawBendControlPoints(DisplayPipeline displayPipeline)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(displayPipeline, BasePoint);
		int posX;
		int posY;
		CSBendPoints currentBend = GetCurrentBend(displayPipeline.Viewport, out posX, out posY);
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(currentBend.BasePoint);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(currentBend.EndPoint);
		Point3d p = default(Point3d);
		((Point3d)(ref p))._002Ector(currentBend.BendPoint);
		Point3d val3 = ((currentBend._bendMode > 0) ? new Point3d(val - (val2 - val)) : new Point3d(val));
		Transform val4 = ObjectTransform;
		if (_outputFlipMode == 1)
		{
			Point3d origin = Point3d.Origin;
			((Point3d)(ref origin)).Transform(ObjectTransform);
			val4 = Transform.Mirror(origin, Vector3d.XAxis) * val4;
		}
		((Point3d)(ref val)).Transform(val4);
		((Point3d)(ref val2)).Transform(val4);
		((Point3d)(ref p)).Transform(val4);
		((Point3d)(ref val3)).Transform(val4);
		CSValueHandleHelper.DrawMeshSphere(val, 9.0, worldToScreenScale, displayPipeline, (MouseOver && _whichSelected == CSValueHandleHelper.WSelected.BendPoint0) ? CSValueHandleHelper.MatSelected : CSValueHandleHelper.MatBendBasePoint);
		CSValueHandleHelper.DrawMeshSphere(val2, 9.0, worldToScreenScale, displayPipeline, (MouseOver && _whichSelected == CSValueHandleHelper.WSelected.BendPoint1) ? CSValueHandleHelper.MatSelected : CSValueHandleHelper.MatBendEndPoint);
		CSValueHandleHelper.DrawMeshSphere(p, 15.0, worldToScreenScale, displayPipeline, (MouseOver && _whichSelected == CSValueHandleHelper.WSelected.BendPoint2) ? CSValueHandleHelper.MatSelected : CSValueHandleHelper.MatBendBendPoint);
		Mesh val5 = CSValueHandleHelper.MeshCurve((Curve)new LineCurve(val2, val3), 1, 20, 2.0 / worldToScreenScale);
		try
		{
			displayPipeline.DrawMeshShaded(val5, (MouseOver && _whichSelected == CSValueHandleHelper.WSelected.BendPoint1) ? CSValueHandleHelper.MatSelected : CSValueHandleHelper.MatBendEndPoint);
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
		NurbsCurve displayCurve = currentBend.GetDisplayCurve();
		((GeometryBase)displayCurve).Transform(val4);
		Mesh val6 = CSValueHandleHelper.MeshCurve((Curve)(object)displayCurve, 20, 20, 2.0 / worldToScreenScale);
		try
		{
			displayPipeline.DrawMeshShaded(val6, (MouseOver && _whichSelected == CSValueHandleHelper.WSelected.BendPoint2) ? CSValueHandleHelper.MatSelected : CSValueHandleHelper.MatBendBendPoint);
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		if (currentBend._bendMode <= 0)
		{
			return;
		}
		NurbsCurve displayMirrorCurve = currentBend.GetDisplayMirrorCurve();
		((GeometryBase)displayMirrorCurve).Transform(val4);
		Mesh val7 = CSValueHandleHelper.MeshCurve((Curve)(object)displayMirrorCurve, 20, 20, 2.0 / worldToScreenScale);
		try
		{
			displayPipeline.DrawMeshShaded(val7, (MouseOver && _whichSelected == CSValueHandleHelper.WSelected.BendPoint2) ? CSValueHandleHelper.MatSelected : CSValueHandleHelper.MatBendBendPoint);
		}
		finally
		{
			((IDisposable)val7)?.Dispose();
		}
	}

	private void DrawBendShadow(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		if (!((BoundingBox)(ref _bb)).IsValid)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(displayPipeline);
		Point3d center = ((BoundingBox)(ref _bbOriginal)).Center;
		((Point3d)(ref center)).Transform(ObjectTransform);
		Vector3d xAxis = Vector3d.XAxis;
		((Vector3d)(ref xAxis)).Transform(ObjectTransform);
		((Vector3d)(ref xAxis)).Unitize();
		Vector3d yAxis = Vector3d.YAxis;
		((Vector3d)(ref yAxis)).Transform(ObjectTransform);
		((Vector3d)(ref yAxis)).Unitize();
		Vector3d zAxis = Vector3d.ZAxis;
		((Vector3d)(ref zAxis)).Transform(ObjectTransform);
		((Vector3d)(ref zAxis)).Unitize();
		double num3 = Math.Abs(cameraDirection * xAxis);
		double num4 = Math.Abs(cameraDirection * yAxis);
		double num5 = Math.Abs(cameraDirection * zAxis);
		Vector3d val = Vector3d.Unset;
		Vector3d val2 = Vector3d.Unset;
		CSBendPoints cSBendPoints = null;
		double num6 = 0.0;
		if (num3 > num6)
		{
			val = yAxis;
			val2 = zAxis;
			num6 = num3;
			cSBendPoints = _bendYz;
			num = ObjSizeY;
			num2 = ObjSizeZ;
		}
		if (num4 > num6)
		{
			val = xAxis;
			val2 = zAxis;
			num6 = num4;
			cSBendPoints = _bendXz;
			num = ObjSizeX;
			num2 = ObjSizeZ;
		}
		if (num5 > num6)
		{
			val = xAxis;
			val2 = yAxis;
			cSBendPoints = _bendXy;
			num = ObjSizeX;
			num2 = ObjSizeY;
		}
		if (cSBendPoints != null)
		{
			Point3d val3 = default(Point3d);
			((Point3d)(ref val3))._002Ector(-(val * (double)(num - 1)) + val2 * (double)(num2 - 1) + center);
			DisplayBitmap val4 = null;
			switch (cSBendPoints._bendMode)
			{
			case 0:
				val4 = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.MirrorModeBmp && cSBendPoints._iconMouseOver) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Bend0S) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Bend0));
				break;
			case 1:
				val4 = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.MirrorModeBmp && cSBendPoints._iconMouseOver) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Bend1S) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Bend1));
				break;
			case 2:
				val4 = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.MirrorModeBmp && cSBendPoints._iconMouseOver) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Bend2S) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Bend2));
				break;
			}
			if (val4 != null)
			{
				Transform val5 = CSValueHandleHelper.WorldToScreen(displayPipeline);
				((Point3d)(ref val3)).Transform(val5);
				double num7 = 32.0 * CSValueHandleHelper.ScreenScaleOverride;
				int num8 = (int)(((Point3d)(ref val3)).X - num7 / 2.0);
				int num9 = (int)(((Point3d)(ref val3)).Y - num7 / 2.0);
				displayPipeline.DrawBitmap(val4, num8, num9);
			}
		}
	}

	private void CalculateBendPlanes(RhinoViewport vp, out Plane pl0, out Plane pl1, out Plane pl2, out CSBendPoints currentBend)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(vp);
		((Vector3d)(ref cameraDirection)).Unitize();
		Point3d center = ((BoundingBox)(ref _bbOriginal)).Center;
		((Point3d)(ref center)).Transform(ObjectTransform);
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(1.0, 0.0, 0.0);
		Vector3d val2 = default(Vector3d);
		((Vector3d)(ref val2))._002Ector(0.0, 1.0, 0.0);
		Vector3d val3 = default(Vector3d);
		((Vector3d)(ref val3))._002Ector(0.0, 0.0, 1.0);
		((Vector3d)(ref val)).Transform(ObjectTransform);
		((Vector3d)(ref val)).Unitize();
		((Vector3d)(ref val2)).Transform(ObjectTransform);
		((Vector3d)(ref val2)).Unitize();
		((Vector3d)(ref val3)).Transform(ObjectTransform);
		((Vector3d)(ref val3)).Unitize();
		double num = Math.Abs(cameraDirection * val);
		double num2 = Math.Abs(cameraDirection * val2);
		double num3 = Math.Abs(cameraDirection * val3);
		Vector3d val4 = default(Vector3d);
		((Vector3d)(ref val4))._002Ector(1.0, 0.0, 0.0);
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(1.0, 0.0, 0.0);
		currentBend = null;
		double num4 = 0.0;
		if (num > num4)
		{
			val4 = val2;
			val5 = val3;
			num4 = num;
			currentBend = _bendYz;
		}
		if (num2 > num4)
		{
			val4 = val;
			val5 = val3;
			num4 = num2;
			currentBend = _bendXz;
		}
		if (num3 > num4)
		{
			val4 = val;
			val5 = val2;
			currentBend = _bendXy;
		}
		if (currentBend == null)
		{
			pl0 = Plane.Unset;
			pl1 = Plane.Unset;
			pl2 = Plane.Unset;
			return;
		}
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector(currentBend.BasePoint);
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector(currentBend.EndPoint);
		Point3d val8 = default(Point3d);
		((Point3d)(ref val8))._002Ector(currentBend.BendPoint);
		((Point3d)(ref val6)).Transform(ObjectTransform);
		((Point3d)(ref val7)).Transform(ObjectTransform);
		((Point3d)(ref val8)).Transform(ObjectTransform);
		pl0 = new Plane(val6, val4, val5);
		pl1 = new Plane(val7, val4, val5);
		pl2 = new Plane(val8, val4, val5);
	}

	private void ArrayGetPoints(double l, out Point3d arrayCenter, out Point3d arrayStart, out Point3d arrayEnd, out Point3d arrayCount, out Plane hitPlane)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		arrayCenter = ArrayCenter();
		Vector3d val = BasePoint - arrayCenter;
		if (((Vector3d)(ref val)).Length < 0.01)
		{
			val = ((_arrayDirection == CSValueHandleHelper.ArrayDirection.Yz) ? new Vector3d(0.0, 0.0, 1.0) : new Vector3d(1.0, 0.0, 0.0));
		}
		double length = ((Vector3d)(ref val)).Length;
		((Vector3d)(ref val)).Unitize();
		Vector3d val2 = VRot();
		((Vector3d)(ref val2)).Transform(_arrayObjectTransform);
		arrayStart = arrayCenter + val * (length + 60.0 / l);
		arrayEnd = arrayCenter + val * (length + 40.0 / l);
		arrayCount = arrayCenter + val * (length + 20.0 / l);
		Transform val3 = Transform.Rotation(_arrayTotalAngle, val2, arrayCenter);
		((Point3d)(ref arrayEnd)).Transform(val3);
		val3 = Transform.Rotation(_arrayRepeatAngle, val2, arrayCenter);
		((Point3d)(ref arrayCount)).Transform(val3);
		Vector3d val4 = Vector3d.CrossProduct(val, val2);
		hitPlane = new Plane(arrayCenter, val, val4);
	}

	private void GetFollowCurve(out Curve crvFollow, out Point3d pStart, out Point3d pEnd, out Vector3d up, double secondAngle)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		pStart = Point3d.Unset;
		pEnd = Point3d.Unset;
		Point3d val = ArrayCenter();
		Vector3d val2 = Vector3d.Unset;
		Vector3d val3 = Vector3d.Unset;
		switch (_arrayDirection)
		{
		case CSValueHandleHelper.ArrayDirection.Xy:
			val2 = Vector3d.XAxis;
			val3 = Vector3d.YAxis;
			break;
		case CSValueHandleHelper.ArrayDirection.Xz:
			val2 = -Vector3d.XAxis;
			val3 = -Vector3d.ZAxis;
			break;
		case CSValueHandleHelper.ArrayDirection.Yz:
			val2 = -Vector3d.YAxis;
			val3 = Vector3d.ZAxis;
			break;
		}
		((Vector3d)(ref val2)).Transform(_arrayObjectTransform);
		((Vector3d)(ref val3)).Transform(_arrayObjectTransform);
		Plane val4 = default(Plane);
		((Plane)(ref val4))._002Ector(val, val2, val3);
		Transform val5 = Transform.PlaneToPlane(Plane.WorldXY, val4);
		Transform val6 = Transform.PlaneToPlane(val4, Plane.WorldXY);
		up = Vector3d.ZAxis;
		((Vector3d)(ref up)).Transform(val5);
		Vector3d val7 = BasePoint - val;
		double length = ((Vector3d)(ref val7)).Length;
		((Vector3d)(ref val7)).Transform(val6);
		val7 *= 10.0 / length;
		crvFollow = ArrayShapeCurve(_arrayShape);
		if (crvFollow == null)
		{
			return;
		}
		LineCurve val8 = new LineCurve(new Line(Point3d.Origin, new Point3d(val7)));
		CurveIntersections val9 = Intersection.CurveCurve((Curve)(object)val8, crvFollow, 0.001, 0.001);
		if (val9.Count == 1)
		{
			pStart = val9[0].PointA;
			Vector3d val10 = new Vector3d(pStart);
			double length2 = ((Vector3d)(ref val10)).Length;
			Transform val11 = Transform.Scale(Point3d.Origin, length / length2);
			((GeometryBase)crvFollow).Transform(val11);
			((GeometryBase)val8).Transform(val11);
			((Point3d)(ref pStart)).Transform(val11);
			Transform val12 = Transform.Rotation(secondAngle, -Vector3d.ZAxis, Point3d.Origin);
			((GeometryBase)val8).Transform(val12);
			CurveIntersections val13 = Intersection.CurveCurve((Curve)(object)val8, crvFollow, 0.001, 0.001);
			if (val13.Count == 1)
			{
				pEnd = val13[0].PointA;
				((GeometryBase)crvFollow).Transform(val5);
				((Point3d)(ref pStart)).Transform(val5);
				((Point3d)(ref pEnd)).Transform(val5);
			}
		}
	}

	private static Curve ArrayShapeCurve(ArrayShape arrayShape)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Unknown result type (might be due to invalid IL or missing references)
		//IL_0879: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0908: Unknown result type (might be due to invalid IL or missing references)
		//IL_0916: Unknown result type (might be due to invalid IL or missing references)
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_094a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b05: Expected O, but got Unknown
		//IL_0b26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbf: Expected O, but got Unknown
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1073: Unknown result type (might be due to invalid IL or missing references)
		//IL_1079: Expected O, but got Unknown
		//IL_109a: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_10dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1102: Unknown result type (might be due to invalid IL or missing references)
		//IL_1110: Unknown result type (might be due to invalid IL or missing references)
		//IL_1136: Unknown result type (might be due to invalid IL or missing references)
		//IL_1144: Unknown result type (might be due to invalid IL or missing references)
		//IL_116a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1178: Unknown result type (might be due to invalid IL or missing references)
		//IL_119e: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1206: Unknown result type (might be due to invalid IL or missing references)
		//IL_1214: Unknown result type (might be due to invalid IL or missing references)
		//IL_123a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1248: Unknown result type (might be due to invalid IL or missing references)
		//IL_126e: Unknown result type (might be due to invalid IL or missing references)
		//IL_127c: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_130a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1318: Unknown result type (might be due to invalid IL or missing references)
		//IL_133e: Unknown result type (might be due to invalid IL or missing references)
		//IL_134c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1372: Unknown result type (might be due to invalid IL or missing references)
		//IL_1380: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13da: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_140e: Unknown result type (might be due to invalid IL or missing references)
		//IL_141c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1442: Unknown result type (might be due to invalid IL or missing references)
		//IL_1450: Unknown result type (might be due to invalid IL or missing references)
		//IL_1476: Unknown result type (might be due to invalid IL or missing references)
		//IL_1484: Unknown result type (might be due to invalid IL or missing references)
		//IL_14aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_162c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1632: Expected O, but got Unknown
		//IL_1653: Unknown result type (might be due to invalid IL or missing references)
		//IL_1661: Unknown result type (might be due to invalid IL or missing references)
		//IL_1687: Unknown result type (might be due to invalid IL or missing references)
		//IL_1695: Unknown result type (might be due to invalid IL or missing references)
		//IL_16bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1723: Unknown result type (might be due to invalid IL or missing references)
		//IL_1731: Unknown result type (might be due to invalid IL or missing references)
		//IL_1757: Unknown result type (might be due to invalid IL or missing references)
		//IL_1765: Unknown result type (might be due to invalid IL or missing references)
		//IL_178b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1799: Unknown result type (might be due to invalid IL or missing references)
		//IL_17bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1801: Unknown result type (might be due to invalid IL or missing references)
		//IL_1827: Unknown result type (might be due to invalid IL or missing references)
		//IL_1835: Unknown result type (might be due to invalid IL or missing references)
		//IL_185b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1869: Unknown result type (might be due to invalid IL or missing references)
		//IL_188f: Unknown result type (might be due to invalid IL or missing references)
		//IL_189d: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1905: Unknown result type (might be due to invalid IL or missing references)
		//IL_192b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1939: Unknown result type (might be due to invalid IL or missing references)
		//IL_195f: Unknown result type (might be due to invalid IL or missing references)
		//IL_196d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1993: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1adf: Expected O, but got Unknown
		//IL_1b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b76: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1baa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bde: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c04: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c38: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e40: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e82: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1edc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eea: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f78: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fac: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fee: Unknown result type (might be due to invalid IL or missing references)
		//IL_2014: Unknown result type (might be due to invalid IL or missing references)
		//IL_2022: Unknown result type (might be due to invalid IL or missing references)
		//IL_2048: Unknown result type (might be due to invalid IL or missing references)
		//IL_2056: Unknown result type (might be due to invalid IL or missing references)
		//IL_207c: Unknown result type (might be due to invalid IL or missing references)
		//IL_208a: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_20be: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2118: Unknown result type (might be due to invalid IL or missing references)
		//IL_2126: Unknown result type (might be due to invalid IL or missing references)
		//IL_214c: Unknown result type (might be due to invalid IL or missing references)
		//IL_215a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2180: Unknown result type (might be due to invalid IL or missing references)
		//IL_218e: Unknown result type (might be due to invalid IL or missing references)
		//IL_23b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_23bb: Expected O, but got Unknown
		//IL_23dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_23ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_2410: Unknown result type (might be due to invalid IL or missing references)
		//IL_241e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2444: Unknown result type (might be due to invalid IL or missing references)
		//IL_2452: Unknown result type (might be due to invalid IL or missing references)
		//IL_2478: Unknown result type (might be due to invalid IL or missing references)
		//IL_2486: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_2575: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve val = null;
		List<ControlPoint> list = null;
		List<double> list2 = null;
		switch (arrayShape)
		{
		case ArrayShape.Round:
			val = new NurbsCurve(3, true, 3, 9);
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
			val = new NurbsCurve(3, true, 4, 21);
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
			val = new NurbsCurve(3, true, 3, 9);
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
			val = new NurbsCurve(3, false, 2, 10);
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
			val = new NurbsCurve(3, false, 2, 10);
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
			val = new NurbsCurve(3, false, 4, 21);
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
			val = new NurbsCurve(3, false, 4, 17);
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
			val = new NurbsCurve(3, true, 4, 33);
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
			val = new NurbsCurve(3, false, 2, 6);
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
		if (val == null)
		{
			return null;
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			val.Points[i] = list[i];
		}
		int count2 = list2.Count;
		for (int j = 0; j < count2; j++)
		{
			val.Knots[j] = list2[j];
		}
		return (Curve)(object)val;
	}

	private void DrawArrayArrows(Point3d pointStart, Point3d pointEnd, DisplayPipeline displayPipeline, double pixelsPerUnit, Color lineColor, CSValueHandleHelper.WSelected selId, double rotateAngle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = pointEnd - pointStart;
		((Vector3d)(ref val)).Unitize();
		Point3d txtBoxPoint = pointEnd + val * 50.0 / pixelsPerUnit;
		bool flag = MouseOver && _whichSelected == selId;
		DisplayMaterial val2 = new DisplayMaterial
		{
			Diffuse = lineColor,
			Transparency = 0.5,
			Emission = Color.Black,
			Ambient = Color.FromArgb(40, 40, 40)
		};
		DisplayMaterial val3 = (flag ? CSValueHandleHelper.MatSelected : val2);
		CSValueHandleHelper.DrawMeshCylinder(pointStart, pointEnd, pixelsPerUnit, displayPipeline, val3);
		Vector3d val4 = pointEnd - pointStart;
		((Vector3d)(ref val4)).Unitize();
		Vector3d val5 = Vector3d.XAxis;
		switch (_arrayDirection)
		{
		case CSValueHandleHelper.ArrayDirection.Xy:
			((Vector3d)(ref val5))._002Ector(0.0, 0.0, -1.0);
			break;
		case CSValueHandleHelper.ArrayDirection.Xz:
			((Vector3d)(ref val5))._002Ector(0.0, 1.0, 0.0);
			break;
		case CSValueHandleHelper.ArrayDirection.Yz:
			((Vector3d)(ref val5))._002Ector(1.0, 0.0, 0.0);
			break;
		}
		((Vector3d)(ref val5)).Transform(_arrayObjectTransform);
		Vector3d val6 = Vector3d.CrossProduct(val5, val4);
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
			Plane val7 = default(Plane);
			((Plane)(ref val7))._002Ector(pointStart, val4, val6);
			Vector3d val8 = pointEnd - pointStart;
			double length = ((Vector3d)(ref val8)).Length;
			length *= 0.6;
			ArcCurve val9 = new ArcCurve(new Arc(new Circle(val7, length), 0.0 - rotateAngle));
			int majorAxis = (int)Math.Abs(rotateAngle * 180.0 / Math.PI);
			Mesh val10 = CSValueHandleHelper.MeshCurve((Curve)val9, majorAxis, 20, 2.1 / pixelsPerUnit);
			try
			{
				displayPipeline.DrawMeshShaded(val10, val3);
			}
			finally
			{
				((IDisposable)val10)?.Dispose();
			}
		}
		val4 *= 7.0 / pixelsPerUnit;
		val5 *= 7.0 / pixelsPerUnit;
		val6 *= 7.0 / pixelsPerUnit;
		Transform val11 = CSValueHandleHelper.ChangeBasis(pointEnd, val4, val5, val6);
		Transform modelTransform = displayPipeline.ModelTransform;
		displayPipeline.ModelTransform = modelTransform * val11;
		val3.Transparency = 0.7;
		displayPipeline.DrawMeshShaded(CSValueHandleHelper.MeshRotate, val3);
		displayPipeline.ModelTransform = modelTransform;
		val2.Dispose();
	}

	private void DrawArrayIcon(DisplayPipeline displayPipeline)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		if (!((BoundingBox)(ref _bb)).IsValid)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(displayPipeline);
		Point3d center = ((BoundingBox)(ref _bbOriginal)).Center;
		((Point3d)(ref center)).Transform(ObjectTransform);
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(1.0, 0.0, 0.0);
		Vector3d val2 = default(Vector3d);
		((Vector3d)(ref val2))._002Ector(0.0, 1.0, 0.0);
		Vector3d val3 = default(Vector3d);
		((Vector3d)(ref val3))._002Ector(0.0, 0.0, 1.0);
		((Vector3d)(ref val)).Transform(ObjectTransform);
		((Vector3d)(ref val)).Unitize();
		((Vector3d)(ref val2)).Transform(ObjectTransform);
		((Vector3d)(ref val2)).Unitize();
		((Vector3d)(ref val3)).Transform(ObjectTransform);
		((Vector3d)(ref val3)).Unitize();
		double num3 = Math.Abs(cameraDirection * val);
		double num4 = Math.Abs(cameraDirection * val2);
		double num5 = Math.Abs(cameraDirection * val3);
		Vector3d val4 = default(Vector3d);
		((Vector3d)(ref val4))._002Ector(1.0, 0.0, 0.0);
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(1.0, 0.0, 0.0);
		double num6 = 0.0;
		if (num3 > num6)
		{
			val4 = val2;
			val5 = val3;
			num6 = num3;
			num = ObjSizeY;
			num2 = ObjSizeZ;
		}
		if (num4 > num6)
		{
			val4 = val;
			val5 = val3;
			num6 = num4;
			num = ObjSizeX;
			num2 = ObjSizeZ;
		}
		if (num5 > num6)
		{
			val4 = val;
			val5 = val2;
			num = ObjSizeX;
			num2 = ObjSizeY;
		}
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector(-(val4 * (double)(num - 1)) + val5 * (double)(num2 - 1) + center);
		DisplayBitmap val7 = null;
		switch (_arrayDirection)
		{
		case CSValueHandleHelper.ArrayDirection.Off:
			val7 = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.ArrayTypeBmp) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array0S) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array0));
			break;
		case CSValueHandleHelper.ArrayDirection.Xy:
			val7 = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.ArrayTypeBmp) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array1S) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array1));
			break;
		case CSValueHandleHelper.ArrayDirection.Xz:
			val7 = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.ArrayTypeBmp) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array2S) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array2));
			break;
		case CSValueHandleHelper.ArrayDirection.Yz:
			val7 = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.ArrayTypeBmp) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array3S) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.Array3));
			break;
		}
		if (val7 == null)
		{
			return;
		}
		Transform val8 = CSValueHandleHelper.WorldToScreen(displayPipeline);
		((Point3d)(ref val6)).Transform(val8);
		double num7 = 32.0 * CSValueHandleHelper.ScreenScaleOverride;
		int num8 = (int)(((Point3d)(ref val6)).X - num7 / 2.0);
		int num9 = (int)(((Point3d)(ref val6)).Y - num7 / 2.0);
		displayPipeline.DrawBitmap(val7, num8, num9);
		num9 += (int)num7;
		if (_arrayDirection > CSValueHandleHelper.ArrayDirection.Off)
		{
			val7 = ((!_arrayMirror) ? ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.ArrayMirrorBmp) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayMirrorOffS) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayMirrorOff)) : ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.ArrayMirrorBmp) ? CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayMirrorOnS) : CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayMirrorOn)));
			displayPipeline.DrawBitmap(val7, num8, num9);
			num9 += (int)num7;
			switch (_arrayShape)
			{
			case ArrayShape.Round:
				val7 = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayRound);
				break;
			case ArrayShape.Cushion:
				val7 = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayCushion);
				break;
			case ArrayShape.Oval:
				val7 = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayOval);
				break;
			case ArrayShape.Emerald:
				val7 = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayEmerald);
				break;
			case ArrayShape.Asscher:
				val7 = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayAsscher);
				break;
			case ArrayShape.Heart:
				val7 = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayHeart);
				break;
			case ArrayShape.Marquise:
				val7 = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayMarquise);
				break;
			case ArrayShape.Pear:
				val7 = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArrayPear);
				break;
			case ArrayShape.Square:
				val7 = CSValueHandleHelper.GetDispBm(CSValueHandleHelper.VPCHImages.ArraySquare);
				break;
			}
			displayPipeline.DrawBitmap(val7, num8, num9);
		}
	}

	private void GetIconLocation(RhinoViewport vp, out int posX, out int posY)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int num2 = 0;
		Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(vp);
		((Vector3d)(ref cameraDirection)).Unitize();
		Point3d center = ((BoundingBox)(ref _bbOriginal)).Center;
		((Point3d)(ref center)).Transform(ObjectTransform);
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(1.0, 0.0, 0.0);
		Vector3d val2 = default(Vector3d);
		((Vector3d)(ref val2))._002Ector(0.0, 1.0, 0.0);
		Vector3d val3 = default(Vector3d);
		((Vector3d)(ref val3))._002Ector(0.0, 0.0, 1.0);
		((Vector3d)(ref val)).Transform(ObjectTransform);
		((Vector3d)(ref val)).Unitize();
		((Vector3d)(ref val2)).Transform(ObjectTransform);
		((Vector3d)(ref val2)).Unitize();
		((Vector3d)(ref val3)).Transform(ObjectTransform);
		((Vector3d)(ref val3)).Unitize();
		double num3 = Math.Abs(cameraDirection * val);
		double num4 = Math.Abs(cameraDirection * val2);
		double num5 = Math.Abs(cameraDirection * val3);
		Vector3d val4 = default(Vector3d);
		((Vector3d)(ref val4))._002Ector(1.0, 0.0, 0.0);
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(1.0, 0.0, 0.0);
		double num6 = 0.0;
		if (num3 > num6)
		{
			val4 = val2;
			val5 = val3;
			num6 = num3;
			num = ObjSizeY;
			num2 = ObjSizeZ;
		}
		if (num4 > num6)
		{
			val4 = val;
			val5 = val3;
			num6 = num4;
			num = ObjSizeX;
			num2 = ObjSizeZ;
		}
		if (num5 > num6)
		{
			val4 = val;
			val5 = val2;
			num = ObjSizeX;
			num2 = ObjSizeY;
		}
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector(-(val4 * (double)(num - 1)) + val5 * (double)(num2 - 1) + center);
		Transform val7 = CSValueHandleHelper.WorldToScreen(vp);
		((Point3d)(ref val6)).Transform(val7);
		double num7 = 32.0 * CSValueHandleHelper.ScreenScaleOverride;
		posX = (int)(((Point3d)(ref val6)).X - num7 / 2.0);
		posY = (int)(((Point3d)(ref val6)).Y - num7 / 2.0);
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		Vector3d result = default(Vector3d);
		switch (_arrayDirection)
		{
		case CSValueHandleHelper.ArrayDirection.Xy:
			((Vector3d)(ref result))._002Ector(0.0, 0.0, -1.0);
			break;
		case CSValueHandleHelper.ArrayDirection.Xz:
			((Vector3d)(ref result))._002Ector(0.0, 1.0, 0.0);
			break;
		case CSValueHandleHelper.ArrayDirection.Yz:
			((Vector3d)(ref result))._002Ector(1.0, 0.0, 0.0);
			break;
		default:
			((Vector3d)(ref result))._002Ector(0.0, 0.0, 0.0);
			break;
		}
		return result;
	}

	private Point3d ArrayCenter()
	{
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		Point3d result = default(Point3d);
		((Point3d)(ref result))._002Ector(((Transform)(ref _value))[0, 3], ((Transform)(ref _value))[1, 3], ((Transform)(ref _value))[2, 3]);
		switch (_arrayDirection)
		{
		case CSValueHandleHelper.ArrayDirection.Xy:
			((Point3d)(ref result))._002Ector(0.0, 0.0, ((Point3d)(ref result)).Z);
			break;
		case CSValueHandleHelper.ArrayDirection.Xz:
			((Point3d)(ref result))._002Ector(0.0, ((Point3d)(ref result)).Y, 0.0);
			break;
		case CSValueHandleHelper.ArrayDirection.Yz:
			((Point3d)(ref result))._002Ector(((Point3d)(ref result)).X, 0.0, 0.0);
			break;
		default:
			((Point3d)(ref result))._002Ector(0.0, 0.0, 0.0);
			break;
		}
		((Point3d)(ref result)).Transform(_arrayObjectTransform);
		return result;
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
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		location = MsrMovePoint(pixelPerUnit, selId);
		MsrMoveDirection(selId, out var moveDirection, out upDirection, out var _);
		Vector3d val = default(Vector3d);
		if (((Vector3d)(ref upDirection)).IsValid)
		{
			val = upDirection;
		}
		else if (Math.Abs(moveDirection * Vector3d.ZAxis) < 0.6)
		{
			val = Vector3d.CrossProduct(moveDirection, Vector3d.ZAxis);
		}
		Vector3d val2 = Vector3d.CrossProduct(val, moveDirection);
		val = Vector3d.CrossProduct(moveDirection, val2);
		val *= 7.0 / pixelPerUnit;
		val2 *= 7.0 / pixelPerUnit;
		Vector3d initialBaseX = moveDirection * 7.0 / pixelPerUnit;
		return CSValueHandleHelper.ChangeBasis(location, initialBaseX, val2, val);
	}

	private void DrawArrow(DisplayPipeline dp, double pixelPerUnit, Color lineColor, CSValueHandleHelper.WSelected selId)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		bool flag = MouseOver && _whichSelected == selId;
		DisplayMaterial val = null;
		DisplayMaterial val2;
		if (CSValueHandleHelper.IsLinkedMove(selId))
		{
			val2 = (IsFreehandCommonCenterModeEnabled ? CSValueHandleHelper.MatSecondType1 : CSValueHandleHelper.MatSecondType2);
		}
		else
		{
			val = new DisplayMaterial
			{
				Diffuse = lineColor,
				Transparency = 0.5,
				Emission = Color.Black,
				Ambient = Color.FromArgb(40, 40, 40)
			};
			val2 = (flag ? CSValueHandleHelper.MatSelected : val);
		}
		Point3d location;
		Vector3d upDirection;
		Transform val3 = ArrowTrans(pixelPerUnit, selId, out location, out upDirection);
		Point3d val4 = (CSValueHandleHelper.IsArrayMove(selId) ? ArrayCenter() : ((!CSValueHandleHelper.IsLinkedMove(selId)) ? BasePoint : UDirP));
		Mesh val5 = CSValueHandleHelper.MeshCylinder(val4, location, pixelPerUnit);
		dp.DrawMeshShaded(val5, val2);
		if (CSValueHandleHelper.MsrMode(selId) == CSValueHandleHelper.WMode.Rotate && flag)
		{
			DrawRotateHandle(val4, location, upDirection, pixelPerUnit, dp, val2, val, val5);
		}
		((CommonObject)val5).Dispose();
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * val3;
		if (val != null)
		{
			val.Transparency = 0.7;
			val.BackTransparency = 0.7;
		}
		switch (CSValueHandleHelper.MsrMode(selId))
		{
		case CSValueHandleHelper.WMode.Rotate:
			dp.DrawMeshShaded(CSValueHandleHelper.MeshRotate, val2);
			break;
		case CSValueHandleHelper.WMode.Move:
			dp.DrawMeshShaded(CSValueHandleHelper.MeshArrow, val2);
			break;
		case CSValueHandleHelper.WMode.Scale:
			dp.DrawMeshShaded(CSValueHandleHelper.MeshScale, val2);
			break;
		}
		if (val != null)
		{
			val.Dispose();
		}
		dp.ModelTransform = modelTransform;
	}

	private void DrawRotateHandle(Point3d basePoint, Point3d location, Vector3d upDirection, double pixelPerUnit, DisplayPipeline dp, DisplayMaterial mat, DisplayMaterial mat1, Mesh cMesh)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = location - basePoint;
		((Vector3d)(ref val)).Unitize();
		Vector3d val2 = Vector3d.CrossProduct(val, upDirection);
		Plane val3 = default(Plane);
		((Plane)(ref val3))._002Ector(basePoint, val, upDirection);
		Vector3d val4 = location - basePoint;
		double length = ((Vector3d)(ref val4)).Length;
		length -= 20.0 / pixelPerUnit;
		Circle val5 = default(Circle);
		((Circle)(ref val5))._002Ector(val3, length);
		Mesh val6 = CSValueHandleHelper.MeshTorus(new Torus(val3, length, 2.0 / pixelPerUnit), 90, 16);
		try
		{
			dp.DrawMeshShaded(val6, mat1);
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		ArcCurve val7 = new ArcCurve(new Arc(val5, 0.0 - _rotateAngle));
		double num = Math.Round(Math.Abs(_rotateAngle * CSBlockUtil.CSUtilMath.On_RADIANS_TO_DEGREES));
		if (num > 0.0)
		{
			if (_lsttxtBox == null)
			{
				_lsttxtBox = new List<TxtBox>();
			}
			List<TxtBox> lsttxtBox = _lsttxtBox;
			Interval domain = ((Curve)val7).Domain;
			lsttxtBox.Add(new TxtBox(((Curve)val7).PointAt(((Interval)(ref domain)).Mid), num.ToString(CultureInfo.InvariantCulture) + " °", Color.FromArgb(255, 255, 19, 247)));
		}
		int majorAxis = (int)Math.Abs(_rotateAngle * 180.0 / Math.PI);
		Mesh val8 = CSValueHandleHelper.MeshCurve((Curve)(object)val7, majorAxis, 20, 2.1 / pixelPerUnit);
		try
		{
			dp.DrawMeshShaded(val8, mat);
		}
		finally
		{
			((IDisposable)val8)?.Dispose();
		}
		Transform val9 = Transform.Rotation(0.0 - _rotateAngle, val2, basePoint);
		((GeometryBase)cMesh).Transform(val9);
		dp.DrawMeshShaded(cMesh, mat);
	}

	private void DrawModePicker(DisplayPipeline dp, double pixelPerUnit)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		DisplayMaterial val = new DisplayMaterial
		{
			Diffuse = ((MouseOver && _whichSelected == CSValueHandleHelper.WSelected.TogglePoint) ? Color.FromArgb(255, 247, 19, 255) : Color.FromArgb(255, 61, 125, 255)),
			Emission = Color.FromArgb(0, 0, 0),
			Ambient = Color.FromArgb(40, 40, 40)
		};
		Transform val2 = CSValueHandleHelper.ChangeBasis(UDirP, UDirX / pixelPerUnit * 10.0, UDirY / pixelPerUnit * 10.0, UDirZ / pixelPerUnit * 10.0);
		Transform modelTransform = dp.ModelTransform;
		dp.ModelTransform = modelTransform * val2;
		dp.DrawMeshShaded((CSValueHandleHelper.CurrentHandle == this) ? CSValueHandleHelper.MeshCube : CSValueHandleHelper.MeshSphere, val);
		dp.ModelTransform = modelTransform;
		val.Dispose();
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
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
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
			((Point3d)(ref val)).Transform(ObjectTransform);
			((Point3d)(ref val2)).Transform(ObjectTransform);
			((Point3d)(ref val3)).Transform(ObjectTransform);
			((Point3d)(ref val4)).Transform(ObjectTransform);
			((Point3d)(ref val5)).Transform(ObjectTransform);
			((Point3d)(ref val6)).Transform(ObjectTransform);
			((Point3d)(ref val7)).Transform(ObjectTransform);
			((Point3d)(ref val8)).Transform(ObjectTransform);
			Color color = Color.FromArgb(255, 224, 224, 224);
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
			double value = 0.0;
			Vector3d val9;
			switch (_whichSelected)
			{
			case CSValueHandleHelper.WSelected.ScaleX:
				val9 = val - val4;
				value = ((Vector3d)(ref val9)).Length;
				break;
			case CSValueHandleHelper.WSelected.ScaleY:
				val9 = val - val2;
				value = ((Vector3d)(ref val9)).Length;
				break;
			case CSValueHandleHelper.WSelected.ScaleZ:
				val9 = val - val5;
				value = ((Vector3d)(ref val9)).Length;
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
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		_whichSelected = CSValueHandleHelper.WSelected.PNone;
		if (!IsOn)
		{
			return false;
		}
		RhinoViewport activeViewport = e.View.ActiveViewport;
		Line frustrumLine = CSValueHandleHelper.GetFrustrumLine(activeViewport, e.ViewportPoint);
		Vector3d direction = ((Line)(ref frustrumLine)).Direction;
		((Vector3d)(ref direction)).Unitize();
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
				Point3d bp = default(Point3d);
				((Point3d)(ref bp))._002Ector(currentBend.BasePoint);
				Point3d bp2 = default(Point3d);
				((Point3d)(ref bp2))._002Ector(currentBend.EndPoint);
				Point3d bp3 = default(Point3d);
				((Point3d)(ref bp3))._002Ector(currentBend.BendPoint);
				Transform val = ObjectTransform;
				if (_outputFlipMode == 1)
				{
					Point3d origin = Point3d.Origin;
					((Point3d)(ref origin)).Transform(ObjectTransform);
					val = Transform.Mirror(origin, Vector3d.XAxis) * val;
				}
				((Point3d)(ref bp)).Transform(val);
				((Point3d)(ref bp2)).Transform(val);
				((Point3d)(ref bp3)).Transform(val);
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
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ace: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Unknown result type (might be due to invalid IL or missing references)
		//IL_091c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0921: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_092b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0933: Unknown result type (might be due to invalid IL or missing references)
		//IL_093b: Unknown result type (might be due to invalid IL or missing references)
		//IL_093d: Unknown result type (might be due to invalid IL or missing references)
		//IL_093f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0974: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_098d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0992: Unknown result type (might be due to invalid IL or missing references)
		//IL_0997: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Unknown result type (might be due to invalid IL or missing references)
		//IL_0844: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_084e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0862: Unknown result type (might be due to invalid IL or missing references)
		//IL_086d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0872: Unknown result type (might be due to invalid IL or missing references)
		//IL_0877: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_089b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
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
			Transform val = ObjectTransform;
			if (_outputFlipMode == 1)
			{
				Point3d origin = Point3d.Origin;
				((Point3d)(ref origin)).Transform(ObjectTransform);
				val = Transform.Mirror(origin, Vector3d.XAxis) * val;
			}
			switch (_whichSelected)
			{
			case CSValueHandleHelper.WSelected.BendPoint0:
			{
				double num8 = default(double);
				if (Intersection.LinePlane(frustrumLine, pl, ref num8))
				{
					Point3d val7 = ((Line)(ref frustrumLine)).PointAt(num8);
					Point3d newP2 = val7;
					Transform val8 = default(Transform);
					((Transform)(ref val)).TryGetInverse(ref val8);
					((Point3d)(ref newP2)).Transform(val8);
					MSetP0(currentBend, newP2);
					return true;
				}
				return false;
			}
			case CSValueHandleHelper.WSelected.BendPoint1:
			{
				double num2 = default(double);
				if (Intersection.LinePlane(frustrumLine, pl2, ref num2))
				{
					Point3d val4 = ((Line)(ref frustrumLine)).PointAt(num2);
					Point3d val5 = val4;
					if (CSValueHandleHelper.MouseButtonShift(e))
					{
						double num3 = default(double);
						double num4 = default(double);
						((Plane)(ref pl)).ClosestParameter(val5, ref num3, ref num4);
						double num5 = CSValueHandleHelper.AngleOfPointD(num3, num4);
						double num6 = Math.Sqrt(num3 * num3 + num4 * num4);
						double num7 = Math.Round(num5 / (Math.PI / 4.0)) * (Math.PI / 4.0);
						num3 = Math.Sin(num7) * num6;
						num4 = Math.Cos(num7) * num6;
						val5 = ((Plane)(ref pl)).PointAt(num3, num4);
					}
					Transform val6 = default(Transform);
					((Transform)(ref val)).TryGetInverse(ref val6);
					((Point3d)(ref val5)).Transform(val6);
					MSetP1(currentBend, val5);
					return true;
				}
				return false;
			}
			case CSValueHandleHelper.WSelected.BendPoint2:
			{
				double num = default(double);
				if (Intersection.LinePlane(frustrumLine, pl3, ref num))
				{
					Point3d val2 = ((Line)(ref frustrumLine)).PointAt(num);
					Point3d newP = val2;
					Transform val3 = default(Transform);
					((Transform)(ref val)).TryGetInverse(ref val3);
					((Point3d)(ref newP)).Transform(val3);
					MSetP2(currentBend, newP);
					return true;
				}
				return false;
			}
			}
		}
		double num9 = default(double);
		if (Intersection.LinePlane(frustrumLine, _hitPlane, ref num9))
		{
			Point3d val9 = ((Line)(ref frustrumLine)).PointAt(num9);
			double num10 = default(double);
			double num11 = default(double);
			((Plane)(ref _hitPlane)).ClosestParameter(val9, ref num10, ref num11);
			double num12 = num10;
			if (CSValueHandleHelper.MouseButtonShift(e))
			{
				num12 = Math.Round(num12 * 4.0) / 4.0;
			}
			if (num12 < 0.1)
			{
				num12 = 0.1;
			}
			Transform val10 = Transform.Identity;
			Transform xTrans;
			double value;
			switch (_whichSelected)
			{
			case CSValueHandleHelper.WSelected.ArrayEnd:
			{
				PreSet(isMouseMoving: true);
				double num27 = CSValueHandleHelper.AngleOfPointD(num11, num10);
				_arrayRepeatAngle /= _arrayTotalAngle;
				_arrayTotalAngle = Math.PI * 2.0 - num27;
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
				double num21 = CSValueHandleHelper.AngleOfPointD(num11, num10);
				_arrayRepeatAngle = Math.PI * 2.0 - num21;
				double num22 = (_arrayTotalAngle + Math.PI * 2.0) / 2.0;
				if (_arrayRepeatAngle > num22)
				{
					_arrayRepeatAngle = 0.0;
				}
				if (_arrayRepeatAngle > _arrayTotalAngle)
				{
					_arrayRepeatAngle = _arrayTotalAngle;
				}
				int num23 = (int)(_arrayTotalAngle * CSBlockUtil.CSUtilMath.On_RADIANS_TO_DEGREES * 2.0);
				int num24 = num23;
				if (_arrayRepeatAngle >= _arrayTotalAngle / (double)num23)
				{
					num24 = (int)Math.Round(_arrayTotalAngle / _arrayRepeatAngle);
				}
				_arrayRepeatAngle = _arrayTotalAngle / (double)num24;
				PostSet(isMoving: true);
				return true;
			}
			case CSValueHandleHelper.WSelected.ArrayStart:
			{
				double num18 = CSValueHandleHelper.AngleOfPointD(num11, num10);
				if (num18 > Math.PI)
				{
					num18 = 0.0 - (Math.PI * 2.0 - num18);
				}
				double rotateAngle = _rotateAngle;
				_rotateAngle += num18;
				if (CSValueHandleHelper.MouseButtonShift(e))
				{
					_rotateAngle = Math.Round(_rotateAngle / (Math.PI / 4.0)) * (Math.PI / 4.0);
				}
				num18 = _rotateAngle - rotateAngle;
				if (_rotateAngle > Math.PI)
				{
					_rotateAngle = _rotateAngle - Math.PI - Math.PI;
				}
				if (_rotateAngle < -Math.PI)
				{
					_rotateAngle = Math.PI + (_rotateAngle + Math.PI);
				}
				GetFollowCurve(out var crvFollow, out var pStart, out var pEnd, out var up, 0.0 - num18);
				double num19 = default(double);
				crvFollow.ClosestPoint(pStart, ref num19);
				double num20 = default(double);
				crvFollow.ClosestPoint(pEnd, ref num20);
				Plane val11 = new Plane(crvFollow.PointAt(num19), crvFollow.TangentAt(num19), up);
				Plane val12 = default(Plane);
				((Plane)(ref val12))._002Ector(crvFollow.PointAt(num20), crvFollow.TangentAt(num20), up);
				val10 = Transform.Rotation(num18, ((Plane)(ref _hitPlane)).Normal, EPoint);
				((Plane)(ref _hitPlane)).Transform(val10);
				val10 = Transform.PlaneToPlane(val11, val12);
				val10 *= ObjectTransform;
				PreSet(isMouseMoving: true);
				ObjectTransform = val10;
				PostSet(isMoving: true);
				return true;
			}
			case CSValueHandleHelper.WSelected.RotateX:
			case CSValueHandleHelper.WSelected.RotateY:
			case CSValueHandleHelper.WSelected.RotateZ:
			{
				double num26 = CSValueHandleHelper.AngleOfPointD(num11, num10);
				if (num26 > Math.PI)
				{
					num26 = 0.0 - (Math.PI * 2.0 - num26);
				}
				double rotateAngle2 = _rotateAngle;
				_rotateAngle += num26;
				if (CSValueHandleHelper.MouseButtonShift(e))
				{
					_rotateAngle = Math.Round(_rotateAngle / (Math.PI / 4.0)) * (Math.PI / 4.0);
				}
				num26 = _rotateAngle - rotateAngle2;
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
					val10 = Transform.Rotation(num26, -UDirZ, BasePoint);
				}
				if (_whichSelected == CSValueHandleHelper.WSelected.RotateY)
				{
					val10 = Transform.Rotation(num26, -UDirX, BasePoint);
				}
				if (_whichSelected == CSValueHandleHelper.WSelected.RotateZ)
				{
					val10 = Transform.Rotation(num26, -UDirY, BasePoint);
				}
				((Plane)(ref _hitPlane)).Transform(val10);
				xTrans = val10;
				val10 *= ObjectTransform;
				value = num26;
				break;
			}
			case CSValueHandleHelper.WSelected.ArrayRx:
			case CSValueHandleHelper.WSelected.ArrayRy:
			case CSValueHandleHelper.WSelected.ArrayRz:
			{
				double num28 = CSValueHandleHelper.AngleOfPointD(num11, num10);
				if (num28 > Math.PI)
				{
					num28 = 0.0 - (Math.PI * 2.0 - num28);
				}
				double rotateAngle3 = _rotateAngle;
				_rotateAngle += num28;
				if (CSValueHandleHelper.MouseButtonShift(e))
				{
					_rotateAngle = Math.Round(_rotateAngle / (Math.PI / 4.0)) * (Math.PI / 4.0);
				}
				num28 = _rotateAngle - rotateAngle3;
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
					val10 = Transform.Rotation(num28, -EDirZ, ((Plane)(ref _hitPlane)).Origin);
				}
				if (_whichSelected == CSValueHandleHelper.WSelected.ArrayRy)
				{
					val10 = Transform.Rotation(num28, -EDirX, ((Plane)(ref _hitPlane)).Origin);
				}
				if (_whichSelected == CSValueHandleHelper.WSelected.ArrayRz)
				{
					val10 = Transform.Rotation(num28, -EDirY, ((Plane)(ref _hitPlane)).Origin);
				}
				((Plane)(ref _hitPlane)).Transform(val10);
				PreSet(isMouseMoving: true);
				_arrayObjectTransform = val10 * _arrayObjectTransform;
				((Transform)(ref _arrayObjectTransform)).TryGetInverse(ref _arrayObjectTransformInv);
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
				double num25 = 0.0;
				if (CSValueHandleHelper.MouseButtonShift(e))
				{
					num25 = num11;
				}
				val10 = Transform.Translation(((Plane)(ref _hitPlane)).XAxis * num10 + ((Plane)(ref _hitPlane)).YAxis * num25);
				((Plane)(ref _hitPlane)).Transform(val10);
				xTrans = val10;
				val10 *= ObjectTransform;
				value = num10;
				break;
			}
			case CSValueHandleHelper.WSelected.ArrayMx:
			case CSValueHandleHelper.WSelected.ArrayMy:
			case CSValueHandleHelper.WSelected.ArrayMz:
			{
				double num17 = 0.0;
				if (CSValueHandleHelper.MouseButtonShift(e))
				{
					num17 = num11;
				}
				val10 = Transform.Translation(((Plane)(ref _hitPlane)).XAxis * num10 + ((Plane)(ref _hitPlane)).YAxis * num17);
				((Plane)(ref _hitPlane)).Transform(val10);
				PreSet(isMouseMoving: true);
				_arrayObjectTransform = val10 * _arrayObjectTransform;
				((Transform)(ref _arrayObjectTransform)).TryGetInverse(ref _arrayObjectTransformInv);
				PostSet(isMoving: true);
				return true;
			}
			case CSValueHandleHelper.WSelected.ScaleX:
			{
				val10 *= ObjectTransform;
				double num16 = num12 / HandleLengthX;
				value = num16;
				((Transform)(ref val10))[0, 0] = ((Transform)(ref val10))[0, 0] * num16;
				((Transform)(ref val10))[1, 0] = ((Transform)(ref val10))[1, 0] * num16;
				((Transform)(ref val10))[2, 0] = ((Transform)(ref val10))[2, 0] * num16;
				xTrans = Transform.Scale(new Plane(BasePoint, UDirZ), num16, 1.0, 1.0);
				break;
			}
			case CSValueHandleHelper.WSelected.ScaleY:
			{
				val10 *= ObjectTransform;
				double num15 = num12 / HandleLengthY;
				value = num15;
				((Transform)(ref val10))[0, 1] = ((Transform)(ref val10))[0, 1] * num15;
				((Transform)(ref val10))[1, 1] = ((Transform)(ref val10))[1, 1] * num15;
				((Transform)(ref val10))[2, 1] = ((Transform)(ref val10))[2, 1] * num15;
				xTrans = Transform.Scale(new Plane(BasePoint, UDirZ), 1.0, num15, 1.0);
				break;
			}
			case CSValueHandleHelper.WSelected.ScaleZ:
			{
				val10 *= ObjectTransform;
				double num14 = num12 / HandleLengthZ;
				value = num14;
				((Transform)(ref val10))[0, 2] = ((Transform)(ref val10))[0, 2] * num14;
				((Transform)(ref val10))[1, 2] = ((Transform)(ref val10))[1, 2] * num14;
				((Transform)(ref val10))[2, 2] = ((Transform)(ref val10))[2, 2] * num14;
				xTrans = Transform.Scale(new Plane(BasePoint, UDirZ), 1.0, 1.0, num14);
				break;
			}
			case CSValueHandleHelper.WSelected.Scale3D:
			{
				val10 *= ObjectTransform;
				double num13 = num12 / HandleLengthS;
				value = num13;
				((Transform)(ref val10))[0, 0] = ((Transform)(ref val10))[0, 0] * num13;
				((Transform)(ref val10))[1, 0] = ((Transform)(ref val10))[1, 0] * num13;
				((Transform)(ref val10))[2, 0] = ((Transform)(ref val10))[2, 0] * num13;
				((Transform)(ref val10))[0, 1] = ((Transform)(ref val10))[0, 1] * num13;
				((Transform)(ref val10))[1, 1] = ((Transform)(ref val10))[1, 1] * num13;
				((Transform)(ref val10))[2, 1] = ((Transform)(ref val10))[2, 1] * num13;
				((Transform)(ref val10))[0, 2] = ((Transform)(ref val10))[0, 2] * num13;
				((Transform)(ref val10))[1, 2] = ((Transform)(ref val10))[1, 2] * num13;
				((Transform)(ref val10))[2, 2] = ((Transform)(ref val10))[2, 2] * num13;
				xTrans = Transform.Scale(new Plane(BasePoint, UDirZ), num13, num13, num13);
				break;
			}
			default:
				return true;
			}
			if (val10 == ObjectTransform)
			{
				return true;
			}
			PreSet(isMouseMoving: true);
			ObjectTransform = val10;
			CSValueHandleHelper.SetLinked(_whichSelected, value, xTrans);
			PostSet(isMoving: true);
			return true;
		}
		return false;
	}

	public void SetLinked(CSValueHandleHelper.WSelected type, double value, Transform xTrans)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
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
			Transform val = Transform.Identity;
			switch (type)
			{
			case CSValueHandleHelper.WSelected.RotateX:
				val = Transform.Rotation(value, -UDirZ, BasePoint);
				goto case CSValueHandleHelper.WSelected.RotateY;
			case CSValueHandleHelper.WSelected.RotateY:
			case CSValueHandleHelper.WSelected.RotateZ:
				if (type == CSValueHandleHelper.WSelected.RotateY)
				{
					val = Transform.Rotation(value, -UDirX, BasePoint);
				}
				if (type == CSValueHandleHelper.WSelected.RotateZ)
				{
					val = Transform.Rotation(value, -UDirY, BasePoint);
				}
				val *= ObjectTransform;
				break;
			case CSValueHandleHelper.WSelected.MoveX:
				val = Transform.Translation(UDirX * value);
				goto case CSValueHandleHelper.WSelected.MoveY;
			case CSValueHandleHelper.WSelected.MoveY:
			case CSValueHandleHelper.WSelected.MoveZ:
			case CSValueHandleHelper.WSelected.MoveXNegative:
			case CSValueHandleHelper.WSelected.MoveYNegative:
			case CSValueHandleHelper.WSelected.MoveZNegative:
				if (type == CSValueHandleHelper.WSelected.MoveY)
				{
					val = Transform.Translation(UDirY * value);
				}
				if (type == CSValueHandleHelper.WSelected.MoveZ)
				{
					val = Transform.Translation(UDirZ * value);
				}
				if (type == CSValueHandleHelper.WSelected.MoveXNegative)
				{
					val = Transform.Translation(UDirX * (0.0 - value));
				}
				if (type == CSValueHandleHelper.WSelected.MoveYNegative)
				{
					val = Transform.Translation(UDirY * (0.0 - value));
				}
				if (type == CSValueHandleHelper.WSelected.MoveZNegative)
				{
					val = Transform.Translation(UDirZ * (0.0 - value));
				}
				val *= ObjectTransform;
				break;
			case CSValueHandleHelper.WSelected.ScaleX:
				val *= ObjectTransform;
				((Transform)(ref val))[0, 0] = ((Transform)(ref val))[0, 0] * value;
				((Transform)(ref val))[1, 0] = ((Transform)(ref val))[1, 0] * value;
				((Transform)(ref val))[2, 0] = ((Transform)(ref val))[2, 0] * value;
				break;
			case CSValueHandleHelper.WSelected.ScaleY:
				val *= ObjectTransform;
				((Transform)(ref val))[0, 1] = ((Transform)(ref val))[0, 1] * value;
				((Transform)(ref val))[1, 1] = ((Transform)(ref val))[1, 1] * value;
				((Transform)(ref val))[2, 1] = ((Transform)(ref val))[2, 1] * value;
				break;
			case CSValueHandleHelper.WSelected.ScaleZ:
				val *= ObjectTransform;
				((Transform)(ref val))[0, 2] = ((Transform)(ref val))[0, 2] * value;
				((Transform)(ref val))[1, 2] = ((Transform)(ref val))[1, 2] * value;
				((Transform)(ref val))[2, 2] = ((Transform)(ref val))[2, 2] * value;
				break;
			case CSValueHandleHelper.WSelected.Scale3D:
				val *= ObjectTransform;
				((Transform)(ref val))[0, 0] = ((Transform)(ref val))[0, 0] * value;
				((Transform)(ref val))[1, 0] = ((Transform)(ref val))[1, 0] * value;
				((Transform)(ref val))[2, 0] = ((Transform)(ref val))[2, 0] * value;
				((Transform)(ref val))[0, 1] = ((Transform)(ref val))[0, 1] * value;
				((Transform)(ref val))[1, 1] = ((Transform)(ref val))[1, 1] * value;
				((Transform)(ref val))[2, 1] = ((Transform)(ref val))[2, 1] * value;
				((Transform)(ref val))[0, 2] = ((Transform)(ref val))[0, 2] * value;
				((Transform)(ref val))[1, 2] = ((Transform)(ref val))[1, 2] * value;
				((Transform)(ref val))[2, 2] = ((Transform)(ref val))[2, 2] * value;
				break;
			}
			ObjectTransform = val;
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
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		double num = 80.0 / l;
		double num2 = 160.0 / l;
		double num3 = 120.0 / l;
		double num4 = 30.0 / l;
		double num5 = 30.0 / l;
		double num6 = 23.0 / l;
		return (Point3d)(selId switch
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
		});
	}

	private void MsrMoveDirection(CSValueHandleHelper.WSelected selId, out Vector3d moveDirection, out Vector3d upDirection, out Vector3d outDirection)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		_rotateAngle = 0.0;
		Vector3d direction = ((Line)(ref mouseLine)).Direction;
		((Vector3d)(ref direction)).Unitize();
		MsrMoveDirection(selId, out var moveDirection, out var upDirection, out var outDirection);
		if (!(Math.Abs(Math.Abs(moveDirection * direction)) < 0.99))
		{
			return;
		}
		Point3d val = (CSValueHandleHelper.IsArrayMove(selId) ? ArrayCenter() : ((!CSValueHandleHelper.IsLinkedMove(selId)) ? BasePoint : UDirP));
		double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(vp, val);
		Point3d val2 = MsrMovePoint(worldToScreenScale, selId);
		Point3d val3 = ((Line)(ref mouseLine)).ClosestPoint(val2, false);
		double num = ((Point3d)(ref val3)).DistanceTo(val2);
		if (!(num < CSValueHandleHelper.SelectionSizePixels / worldToScreenScale) || !(num < distance))
		{
			return;
		}
		MouseOver = true;
		distance = num;
		_whichSelected = selId;
		((Vector3d)(ref moveDirection)).Unitize();
		((Vector3d)(ref upDirection)).Unitize();
		switch (CSValueHandleHelper.MsrMode(selId))
		{
		case CSValueHandleHelper.WMode.Rotate:
		{
			_hitPlane = new Plane(val, moveDirection, upDirection);
			double num6 = default(double);
			if (Intersection.LinePlane(mouseLine, _hitPlane, ref num6))
			{
				Point3d val8 = ((Line)(ref mouseLine)).PointAt(num6);
				double y = default(double);
				double x = default(double);
				((Plane)(ref _hitPlane)).ClosestParameter(val8, ref y, ref x);
				Transform val9 = Transform.Rotation(CSValueHandleHelper.AngleOfPointD(x, y), ((Plane)(ref _hitPlane)).Normal, ((Plane)(ref _hitPlane)).Origin);
				((Plane)(ref _hitPlane)).Transform(val9);
			}
			break;
		}
		case CSValueHandleHelper.WMode.Move:
		{
			Vector3d cameraDirection2 = CSValueHandleHelper.GetCameraDirection(vp);
			double value = cameraDirection2 * upDirection;
			double value2 = cameraDirection2 * outDirection;
			Vector3d val5 = ((Math.Abs(value) < Math.Abs(value2)) ? upDirection : outDirection);
			((Vector3d)(ref val5)).Unitize();
			_hitPlane = new Plane(val2, moveDirection, val5);
			double num3 = default(double);
			if (Intersection.LinePlane(mouseLine, _hitPlane, ref num3))
			{
				Point3d val6 = ((Line)(ref mouseLine)).PointAt(num3);
				double num4 = default(double);
				double num5 = default(double);
				((Plane)(ref _hitPlane)).ClosestParameter(val6, ref num4, ref num5);
				Transform val7 = Transform.Translation(((Plane)(ref _hitPlane)).XAxis * num4 + ((Plane)(ref _hitPlane)).YAxis * num5);
				((Plane)(ref _hitPlane)).Transform(val7);
			}
			break;
		}
		case CSValueHandleHelper.WMode.Scale:
		{
			Vector3d cameraDirection = CSValueHandleHelper.GetCameraDirection(vp);
			Vector3d val4 = Vector3d.CrossProduct(moveDirection, cameraDirection);
			((Vector3d)(ref val4)).Unitize();
			double num2 = 20.0 / worldToScreenScale;
			_hitPlane = new Plane(val + moveDirection * num2, moveDirection, val4);
			break;
		}
		}
	}

	private void TestHitSphere(Line mouseLine, RhinoViewport vp, Point3d bp, CSValueHandleHelper.WSelected p, ref double distance)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		double worldToScreenScale = CSValueHandleHelper.GetWorldToScreenScale(vp, bp);
		Point3d val = ((Line)(ref mouseLine)).ClosestPoint(bp, false);
		double num = ((Point3d)(ref val)).DistanceTo(bp);
		if (num < CSValueHandleHelper.SelectionSizePixels / worldToScreenScale && num < distance)
		{
			MouseOver = true;
			distance = num;
			_whichSelected = p;
		}
	}
}
