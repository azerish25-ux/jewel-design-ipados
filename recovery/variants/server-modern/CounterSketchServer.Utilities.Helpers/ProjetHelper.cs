using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using CS.Utilities.Helpers;
using GemvisionGeometry.Geometry;
using Geomagic;
using Geomagic.Print.Headless;
using Geomagic.Utilities;
using OctTree;
using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CounterSketchServer.Utilities.Helpers;

public class ProjetHelper : IDisposable
{
	public class ProjetConduit : DisplayConduit
	{
		public class DrawRotation
		{
			public bool bEnabled;

			public double RotAngle;

			public Vector3d RotAxis;

			public Point3d RotPoint;
		}

		private readonly DisplayMaterial dmBuildMeshes = new DisplayMaterial
		{
			Diffuse = Color.Gray,
			BackDiffuse = Color.DarkGray,
			BackAmbient = Color.Gray,
			Transparency = 0.0,
			Specular = Color.DarkGray,
			Ambient = Color.DarkGray,
			Emission = Color.Black,
			IsTwoSided = true
		};

		private readonly DisplayMaterial dmBuildPlatform = new DisplayMaterial
		{
			Diffuse = Color.DarkGray,
			BackDiffuse = Color.DarkRed,
			Ambient = Color.Black,
			BackTransparency = 0.8,
			Transparency = 0.5,
			IsTwoSided = true
		};

		public bool bDrawGeo = true;

		public bool bDrawSupports = true;

		public List<Mesh> BuildMeshes = new List<Mesh>();

		public DisplayMaterial dmSupports = new DisplayMaterial
		{
			Diffuse = Color.Orange,
			Ambient = Color.DarkOrange,
			Specular = Color.White,
			Emission = Color.Black,
			Transparency = 0.0,
			IsTwoSided = false
		};

		public DrawRotation ModelRotationX = new DrawRotation();

		public DrawRotation ModelRotationY = new DrawRotation();

		public DrawRotation ModelRotationZ = new DrawRotation();

		public double ModelTranslation;

		public List<Mesh> RaftSupportMeshes = new List<Mesh>();

		public List<Mesh> SupportMeshes = new List<Mesh>();

		public double SupportScale = 1.0;

		public Vector3d VecMeshMoveBack;

		public Vector3d VecPlatMoveBack;

		public Mesh BuildPlatformMesh { get; set; }

		public Point3d[] FramePoints { get; set; }

		public BoundingBox BuildMeshBoundingBox { get; set; }

		public BoundingBox SupportMeshBoundingBox { get; set; }

		public BoundingBox PlatformBoundingBox { get; set; }

		protected override void CalculateBoundingBox(CalculateBoundingBoxEventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Mesh val = BuildPlatformMesh.DuplicateMesh();
				e.IncludeBoundingBox(((GeometryBase)val).GetBoundingBox(true));
				((GeometryBase)val).Translate(VecPlatMoveBack);
				e.IncludeBoundingBox(((GeometryBase)val).GetBoundingBox(true));
				if (BuildMeshes.Count > 0)
				{
					foreach (Mesh buildMesh in BuildMeshes)
					{
						val = buildMesh.DuplicateMesh();
						((GeometryBase)val).Translate(VecMeshMoveBack);
						e.IncludeBoundingBox(((GeometryBase)buildMesh).GetBoundingBox(true));
					}
				}
				((DisplayConduit)this).CalculateBoundingBox(e);
			}
			catch (Exception)
			{
			}
		}

		protected override void DrawOverlay(DrawEventArgs e)
		{
		}

		protected override void PostDrawObjects(DrawEventArgs e)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				e.Display.EnableLighting(true);
				e.Display.DepthMode = (DepthMode)0;
				Mesh val = BuildPlatformMesh.DuplicateMesh();
				((GeometryBase)val).Translate(VecPlatMoveBack);
				if (BuildMeshes.Count > 0)
				{
					e.Display.DrawMeshShaded(val, dmBuildPlatform);
					e.Display.DrawBoxCorners(((GeometryBase)val).GetBoundingBox(true), Color.Red);
				}
				if (bDrawGeo && BuildMeshes.Count > 0)
				{
					foreach (Mesh buildMesh in BuildMeshes)
					{
						val = buildMesh.DuplicateMesh();
						((GeometryBase)val).Translate(VecMeshMoveBack);
						if (ModelRotationX.bEnabled)
						{
							((GeometryBase)val).Rotate(ModelRotationX.RotAngle, ModelRotationX.RotAxis, ModelRotationX.RotPoint);
						}
						if (ModelRotationY.bEnabled)
						{
							((GeometryBase)val).Rotate(ModelRotationY.RotAngle, ModelRotationY.RotAxis, ModelRotationY.RotPoint);
						}
						if (ModelRotationZ.bEnabled)
						{
							((GeometryBase)val).Rotate(ModelRotationZ.RotAngle, ModelRotationZ.RotAxis, ModelRotationZ.RotPoint);
						}
						if (ModelTranslation != 0.0)
						{
							((GeometryBase)val).Translate(0.0, 0.0, ModelTranslation);
						}
						e.Display.DrawMeshShaded(val, dmBuildMeshes);
					}
				}
				if (bDrawSupports && BuildMeshes.Count > 0)
				{
					if (SupportMeshes.Count > 0)
					{
						foreach (Mesh supportMesh in SupportMeshes)
						{
							val = supportMesh.DuplicateMesh();
							((GeometryBase)val).Translate(VecMeshMoveBack);
							e.Display.DrawMeshShaded(val, dmSupports);
						}
					}
					if (RaftSupportMeshes.Count > 0)
					{
						foreach (Mesh raftSupportMesh in RaftSupportMeshes)
						{
							val = raftSupportMesh.DuplicateMesh();
							((GeometryBase)val).Translate(VecMeshMoveBack);
							e.Display.DrawMeshShaded(val, dmSupports);
						}
					}
				}
				((CommonObject)val).Dispose();
			}
			catch (Exception)
			{
			}
			((DisplayConduit)this).PostDrawObjects(e);
		}
	}

	public class WatsonProgressListener : ProgressListener
	{
		public class ProgressEvent : EventArgs
		{
			public string Message;

			public float pValue;

			public ProgressEvent(string mess, float val)
			{
				Message = mess;
				pValue = val;
			}
		}

		private static string _message;

		private readonly float _resolution;

		private float _lastFraction;

		public event EventHandler<ProgressEvent> ProgressChange;

		public event EventHandler<ProgressEvent> ProgressEnded;

		public event EventHandler<ProgressEvent> ProgressStarted;

		public WatsonProgressListener()
		{
			_lastFraction = 0f;
			_resolution = 0.001f;
		}

		public void Cancel()
		{
			try
			{
				Progress.instance().interrupt();
				if (this.ProgressEnded != null)
				{
					this.ProgressEnded(null, new ProgressEvent(_message, 0f));
				}
			}
			catch (Exception)
			{
			}
		}

		public void Message(string commandlinemessage)
		{
			_message = commandlinemessage;
			if (this.ProgressChange != null)
			{
				this.ProgressChange(null, new ProgressEvent(_message, _lastFraction));
			}
		}

		public override bool progress(float fraction)
		{
			try
			{
				return progressCore(fraction);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public override void reset()
		{
			_lastFraction = 0f;
			if (this.ProgressEnded != null)
			{
				this.ProgressEnded(null, new ProgressEvent(_message, 0f));
			}
		}

		public void Start()
		{
			if (this.ProgressStarted != null)
			{
				this.ProgressStarted(null, new ProgressEvent(_message, _lastFraction));
			}
			Progress.instance().registerListener(this);
		}

		public void Stop()
		{
			if (this.ProgressEnded != null)
			{
				this.ProgressEnded(null, new ProgressEvent(_message, _lastFraction));
			}
			Progress.instance().unregisterListener(this);
			RhinoDoc.ActiveDoc.Views.Redraw();
		}

		public override void textChanged(string text)
		{
			_message = text;
			if (this.ProgressChange != null)
			{
				this.ProgressChange(null, new ProgressEvent(_message, _lastFraction));
			}
			base.textChanged(text);
		}

		private bool progressCore(float fraction)
		{
			try
			{
				if ((double)fraction == 0.0 || (double)fraction == 1.0 || fraction <= _lastFraction - _resolution || fraction >= _lastFraction + _resolution)
				{
					_lastFraction = fraction;
					int num = Convert.ToInt16(fraction * 100f);
					if (this.ProgressChange != null)
					{
						this.ProgressChange(null, new ProgressEvent(_message, num));
					}
				}
			}
			catch (Exception)
			{
			}
			return true;
		}
	}

	public enum ModelRotationType
	{
		X,
		Y,
		Z
	}

	private bool _bNeedsActivated;

	private bool _bNeedsFirmware;

	private static Printer _printer;

	private static bool _printerOnline;

	private static string _ipAddress;

	private ProjetConduit pConduit;

	private RectangleF BoundaryRectangle;

	private string _executePath;

	private Vector3d _vecMeshMove;

	private Vector3d _vecMeshMoveBack;

	private DataObjectArray _inputMeshes;

	private DataObjectArray _supportMeshes;

	private DataObjectArray _raftMeshes;

	private DataObjectArray _supportNativeData;

	private Mesh _csitem;

	private Mesh _tempMesh;

	private List<Mesh> _rhinoraftsupports;

	private List<Mesh> _rhinomainsupports;

	private List<Mesh> _rhinotempmainsupports;

	private List<Guid> _tmpRhinoDocSupports;

	private int _intsupportdensity;

	private bool _bSupOverride;

	private List<DataObject> _lstSavedJobs;

	private List<Bitmap> _lstSavedJobPreviews;

	private DataObjectArray _tmpSups;

	private int _curmatint = 5;

	private static bool _dialogloaded;

	private double _userscale = 1.0;

	private int _supporttype;

	private Point3d pCenter;

	private readonly ObjectAttributes oaDrawnSupports = new ObjectAttributes();

	private static double On_RADIANS_TO_DEGREES => 180.0 / Math.PI;

	private static double On_DEGREES_TO_RADIANS => Math.PI / 180.0;

	public bool DrawingConduitEnabled
	{
		get
		{
			return ((DisplayConduit)pConduit).Enabled;
		}
		set
		{
			((DisplayConduit)pConduit).Enabled = value;
			RhinoDoc.ActiveDoc.Views.Redraw();
		}
	}

	public bool IsPrinterOnline => _printerOnline;

	public ProjetHelper()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		Directory.SetCurrentDirectory(GetPathOfAssembly());
		_printerOnline = false;
		_ipAddress = string.Empty;
		pConduit = new ProjetConduit();
		((DisplayConduit)pConduit).Enabled = false;
		double num = 43.43;
		double num2 = 27.18;
		BoundaryRectangle = new RectangleF(0f, 0f, (float)num, (float)num2);
		pConduit.BuildPlatformMesh = Mesh.CreateFromPlane(Plane.WorldXY, new Interval(0.0, num), new Interval(0.0, num2), 1, 1);
		pConduit.PlatformBoundingBox = ((GeometryBase)pConduit.BuildPlatformMesh).GetBoundingBox(true);
		try
		{
			for (int i = 0; i < 3; i++)
			{
				string path = Path.Combine(Path.GetTempPath(), "CSSavedJob" + i + ".bmp");
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
		}
		catch
		{
		}
	}

	public void Dispose()
	{
		pConduit = null;
		_printer.Dispose();
	}

	public string AbortBuildJob()
	{
		AbortBuild abortBuild = new AbortBuild(_printer);
		_ = string.Empty;
		try
		{
			abortBuild.run();
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
		return ((GPEnum.ErrorCode)abortBuild.ErrorCode/*cast due to constrained. prefix*/).ToString();
	}

	public bool ConnectToPrinter(string ipAddress, out string sErr, out string sPrinterStatus)
	{
		sErr = string.Empty;
		sPrinterStatus = string.Empty;
		bool flag = InitializeOnlineProjetConnection(ipAddress, out sErr, out sPrinterStatus);
		if (flag)
		{
			_inputMeshes = new DataObjectArray();
			_supportMeshes = new DataObjectArray();
			_raftMeshes = new DataObjectArray();
			_supportNativeData = new DataObjectArray();
			if (sPrinterStatus == "DEVICE_STATE_PRINTING")
			{
				int buildTime = GetBuildTime();
				sPrinterStatus = "Printing:" + buildTime + " minutes left";
			}
			return SetCSItemToPrintPreview(_csitem, out sErr);
		}
		return flag;
	}

	public bool CSCreateSupportsForCSItem(out string errorMessage)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (_csitem == null)
			{
				errorMessage = "No Model has been set";
				return false;
			}
			GetDeviceStatus(_printer, _printer.IpAddress, out var state, verbose: true, out errorMessage);
			if (state != GPEnum.DeviceState.DEVICE_STATE_IDLE)
			{
				errorMessage = "Printer busy!";
				return false;
			}
			errorMessage = string.Empty;
			Mesh val = _csitem.DuplicateMesh();
			BoundingBox boundingBox = ((GeometryBase)val).GetBoundingBox(true);
			Point3d center = ((BoundingBox)(ref boundingBox)).Center;
			if (pConduit.ModelRotationX.bEnabled)
			{
				((GeometryBase)val).Rotate(pConduit.ModelRotationX.RotAngle, pConduit.ModelRotationX.RotAxis, center);
			}
			if (pConduit.ModelRotationY.bEnabled)
			{
				((GeometryBase)val).Rotate(pConduit.ModelRotationY.RotAngle, pConduit.ModelRotationY.RotAxis, center);
			}
			if (pConduit.ModelRotationZ.bEnabled)
			{
				((GeometryBase)val).Rotate(pConduit.ModelRotationZ.RotAngle, pConduit.ModelRotationZ.RotAxis, center);
			}
			if (pConduit.ModelTranslation != 0.0)
			{
				((GeometryBase)val).Translate(0.0, 0.0, pConduit.ModelTranslation);
			}
			_inputMeshes = new DataObjectArray();
			BoundingBox boundingBox2 = ((GeometryBase)val).GetBoundingBox(false);
			Point3d val2 = ((BoundingBox)(ref boundingBox2)).Min;
			if (((Point3d)(ref val2)).Z < 1.5)
			{
				errorMessage = "Model must be higher above platform!";
				return false;
			}
			double num = ToDegrees(pConduit.ModelRotationZ.RotAngle);
			if (num < -45.0 || num > 45.0)
			{
				_bSupOverride = true;
			}
			val2 = ((BoundingBox)(ref boundingBox2)).Min;
			if (((Point3d)(ref val2)).Z < 1.5)
			{
				Vector3d diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
				if (((Vector3d)(ref diagonal)).Z < 10.0 && num != 0.0)
				{
					errorMessage = "When flat must be higher above platform!";
					return false;
				}
			}
			val2 = ((BoundingBox)(ref boundingBox2)).Min;
			if (!(((Point3d)(ref val2)).X < 0.0))
			{
				val2 = ((BoundingBox)(ref boundingBox2)).Min;
				if (!(((Point3d)(ref val2)).Y < 0.0))
				{
					val2 = ((BoundingBox)(ref boundingBox2)).Max;
					if (!(((Point3d)(ref val2)).X > (double)BoundaryRectangle.Right))
					{
						val2 = ((BoundingBox)(ref boundingBox2)).Max;
						if (!(((Point3d)(ref val2)).Y > (double)BoundaryRectangle.Bottom))
						{
							if (!AddRhinoMesh(val, out errorMessage))
							{
								return false;
							}
							GPEnum.ErrorCode errorCode = GenerateSupport(_inputMeshes, out errorMessage, out _supportMeshes, out _supportNativeData, out _raftMeshes);
							_rhinomainsupports = new List<Mesh>();
							_rhinoraftsupports = new List<Mesh>();
							_rhinotempmainsupports = new List<Mesh>();
							DataObjectArray dataObjectArray = new DataObjectArray();
							List<Mesh> list = new List<Mesh>();
							double z = 8.0;
							foreach (Guid tmpRhinoDocSupport in _tmpRhinoDocSupports)
							{
								RhinoDoc.ActiveDoc.Objects.Delete(tmpRhinoDocSupport, true);
							}
							_tmpRhinoDocSupports = new List<Guid>();
							foreach (DataObject raftMesh in _raftMeshes)
							{
								if (_supporttype != 3)
								{
									(raftMesh as Mesh).transform(Transform3D.scale(1.0, 1.0, z));
								}
								dataObjectArray.Clear();
								dataObjectArray.Add(raftMesh);
								list = RhinoMeshFromGeoMesh(dataObjectArray);
								_rhinoraftsupports.Add(list[0]);
							}
							int num2 = 0;
							foreach (DataObject supportMesh in _supportMeshes)
							{
								num2++;
								dataObjectArray.Clear();
								dataObjectArray.Add(supportMesh);
								list = RhinoMeshFromGeoMesh(dataObjectArray);
								boundingBox = ((GeometryBase)list[0]).GetBoundingBox(false);
								val2 = ((BoundingBox)(ref boundingBox)).Max;
								double z2 = ((Point3d)(ref val2)).Z;
								boundingBox = ((GeometryBase)list[0]).GetBoundingBox(false);
								val2 = ((BoundingBox)(ref boundingBox)).Min;
								if (!(z2 - ((Point3d)(ref val2)).Z < 3.0))
								{
									_rhinotempmainsupports.Add(list[0]);
									_rhinomainsupports.Add(list[0]);
								}
							}
							switch (errorCode)
							{
							case GPEnum.ErrorCode.NO_ERROR_OCCURRED:
								foreach (Mesh rhinomainsupport in _rhinomainsupports)
								{
									((GeometryBase)rhinomainsupport).Translate(_vecMeshMoveBack);
									Guid item = RhinoDoc.ActiveDoc.Objects.Add((GeometryBase)(object)rhinomainsupport, oaDrawnSupports);
									_tmpRhinoDocSupports.Add(item);
								}
								_rhinoraftsupports.ForEach(delegate(Mesh _)
								{
									//IL_0002: Unknown result type (might be due to invalid IL or missing references)
									((GeometryBase)_).Translate(_vecMeshMoveBack);
									Guid item2 = RhinoDoc.ActiveDoc.Objects.Add((GeometryBase)(object)_, oaDrawnSupports);
									_tmpRhinoDocSupports.Add(item2);
								});
								((DisplayConduit)pConduit).Enabled = true;
								RhinoDoc.ActiveDoc.Views.Redraw();
								return true;
							case GPEnum.ErrorCode.FAILED_TO_MODEL_IS_OUT_OF_PLATFORM:
								errorMessage = "Model not inside platform area!";
								return false;
							default:
								errorMessage = errorCode.ToString();
								return false;
							}
						}
					}
				}
			}
			errorMessage = "Model outside platform area!";
			return false;
		}
		catch (Exception)
		{
			errorMessage = "Error generating supports!";
			return false;
		}
	}

	public bool DeleteSavedJob(int whatjob)
	{
		_lstSavedJobs[whatjob] = new DataObject();
		string path = Path.Combine(Path.GetTempPath(), "CSSavedJob" + whatjob + ".bmp");
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		return true;
	}

	public GPEnum.DeviceState GetPrinterStatus(out string sErr, out string sStatus)
	{
		sErr = string.Empty;
		sStatus = string.Empty;
		if (_printer == null || _ipAddress.IsNullOrEmpty())
		{
			sErr = "No printer connected yet!";
			return GPEnum.DeviceState.DEVICE_STATE_ERROR;
		}
		sErr = GetDeviceStatus(_printer, _ipAddress, out var state, verbose: false, out sStatus).ToString();
		return state;
	}

	public List<int> GetSavedJobs()
	{
		List<int> list = new List<int>();
		int num = 0;
		foreach (DataObject lstSavedJob in _lstSavedJobs)
		{
			if (lstSavedJob.Name != "")
			{
				list.Add(num);
			}
			num++;
		}
		return list;
	}

	public bool IsProjetDialogLoaded()
	{
		return _dialogloaded;
	}

	public GPEnum.ErrorCode PauseBuildJob(out string sErr)
	{
		PauseBuild pauseBuild = new PauseBuild(_printer);
		sErr = string.Empty;
		try
		{
			pauseBuild.run();
		}
		catch (Exception ex)
		{
			sErr = ex.Message;
		}
		return (GPEnum.ErrorCode)pauseBuild.ErrorCode;
	}

	public bool PrintCSModel(out string sStatus)
	{
		sStatus = string.Empty;
		if (!GetModelPrepped(out sStatus, out _tmpSups, _rhinomainsupports, _rhinoraftsupports, _tempMesh))
		{
			return false;
		}
		return SendToPrinter(out sStatus, _tmpSups);
	}

	public bool PrintMultipleJobs(List<int> lstJobsToPrint, out string sStatus)
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		string text = Path.Combine(Path.GetTempPath(), "CS Models");
		if (File.Exists(text))
		{
			File.Delete(text);
		}
		GetDeviceStatus(_printer, _printer.IpAddress, out var state, verbose: true, out sStatus);
		if (state != GPEnum.DeviceState.DEVICE_STATE_IDLE)
		{
			sStatus = "Build cancelled. " + state;
			return false;
		}
		int iPercentLeft = 0;
		if (GetCartridgeInfo(out var materialtype, out sStatus, out iPercentLeft) != GPEnum.ErrorCode.NO_ERROR_OCCURRED)
		{
			return false;
		}
		if (materialtype != "MATID_VISIJET_FTX_GRAY" && materialtype != "MATID_VISIJET_FTX_GREEN")
		{
			sStatus = "Material Cartridge Error";
			return false;
		}
		DataObjectArray dataObjectArray = new DataObjectArray();
		foreach (int item in lstJobsToPrint)
		{
			dataObjectArray.Add(_lstSavedJobs[item]);
		}
		Mesh val = new Mesh();
		List<Mesh> list = RhinoMeshFromGeoMesh(dataObjectArray);
		List<Mesh> list2 = new List<Mesh>();
		Point3d val2;
		foreach (Mesh item2 in list)
		{
			Mesh mFit = item2.DuplicateMesh();
			BoundingBox boundingBox = ((GeometryBase)mFit).GetBoundingBox(false);
			val2 = ((BoundingBox)(ref boundingBox)).Max;
			double x = ((Point3d)(ref val2)).X;
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			double num = x - ((Point3d)(ref val2)).X;
			val2 = ((BoundingBox)(ref boundingBox)).Max;
			double y = ((Point3d)(ref val2)).Y;
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			Plane worldXY;
			BoundingBox boundingBox2;
			if (num > y - ((Point3d)(ref val2)).Y)
			{
				Mesh obj = mFit;
				double num2 = ToRadians(90.0);
				worldXY = Plane.WorldXY;
				Vector3d normal = ((Plane)(ref worldXY)).Normal;
				boundingBox2 = ((GeometryBase)mFit).GetBoundingBox(false);
				((GeometryBase)obj).Rotate(num2, normal, ((BoundingBox)(ref boundingBox2)).Center);
			}
			if (!GetPointOutsideExistingMeshItems(list2, ref mFit))
			{
				Mesh obj2 = mFit;
				double num3 = ToRadians(90.0);
				worldXY = Plane.WorldXY;
				Vector3d normal2 = ((Plane)(ref worldXY)).Normal;
				boundingBox2 = ((GeometryBase)mFit).GetBoundingBox(false);
				((GeometryBase)obj2).Rotate(num3, normal2, ((BoundingBox)(ref boundingBox2)).Center);
				if (!GetPointOutsideExistingMeshItems(list2, ref mFit))
				{
					sStatus = "Multiple build too big to print";
					return false;
				}
			}
			val.Append(mFit);
			list2.Add(mFit.DuplicateMesh());
		}
		BoundingBox boundingBox3 = ((GeometryBase)val).GetBoundingBox(false);
		val2 = ((BoundingBox)(ref boundingBox3)).Min;
		if (!(((Point3d)(ref val2)).X < 0.0))
		{
			val2 = ((BoundingBox)(ref boundingBox3)).Min;
			if (!(((Point3d)(ref val2)).Y < 0.0))
			{
				val2 = ((BoundingBox)(ref boundingBox3)).Max;
				if (!(((Point3d)(ref val2)).X > (double)BoundaryRectangle.Width))
				{
					val2 = ((BoundingBox)(ref boundingBox3)).Max;
					if (!(((Point3d)(ref val2)).Y > (double)BoundaryRectangle.Height))
					{
						dataObjectArray = GeoMeshFromRhinoMesh(val, out sStatus);
						if (GenerateBuildFile(dataObjectArray, new DataObjectArray(), new DataObjectArray(), text, out sStatus) != GPEnum.ErrorCode.NO_ERROR_OCCURRED)
						{
							sStatus = "Failed Generating Build File";
							return false;
						}
						if (SendBuildJob(text, out sStatus) != GPEnum.ErrorCode.NO_ERROR_OCCURRED)
						{
							sStatus = "Failed Sending Build File to Projet";
							return false;
						}
						try
						{
							GetBuildStatusInfo getBuildStatusInfo = new GetBuildStatusInfo(_printer);
							getBuildStatusInfo.run();
							BuildStatusInfo info = getBuildStatusInfo.getInfo();
							sStatus = info.getTotalTime() + " minutes";
						}
						catch (Exception)
						{
							return false;
						}
						return true;
					}
				}
			}
		}
		sStatus = "Multiple build too big to print";
		return false;
	}

	public GPEnum.ErrorCode ResumeBuildJob(out string sErr)
	{
		ResumeBuild resumeBuild = new ResumeBuild(_printer);
		sErr = string.Empty;
		try
		{
			resumeBuild.run();
		}
		catch (Exception ex)
		{
			sErr = ex.Message;
		}
		return (GPEnum.ErrorCode)resumeBuild.ErrorCode;
	}

	public bool SaveCurrentJob(int whatslot, out string sStatus)
	{
		DataObjectArray dJob = new DataObjectArray();
		bool modelPrepped = GetModelPrepped(out sStatus, out dJob, _rhinomainsupports, _rhinoraftsupports, _tempMesh);
		if (modelPrepped)
		{
			dJob[0].Name = "SavedJob" + whatslot;
			string text = Path.Combine(Path.GetTempPath(), "CSSavedJob" + whatslot + ".bmp");
			RhinoDoc.ActiveDoc.Views.ActiveView.CreateShadedPreviewImage(text, new Size(80, 60), true, false, false);
			_lstSavedJobs[whatslot] = dJob[0];
			sStatus = "Job Saved";
		}
		return modelPrepped;
	}

	public bool SetCSItemToPrintPreview(Mesh m, out string sErr)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_bSupOverride = false;
			int iPercentLeft = 0;
			_ = string.Empty;
			sErr = string.Empty;
			if (_csitem == null)
			{
				_csitem = m.DuplicateMesh();
				return false;
			}
			((DisplayConduit)pConduit).Enabled = false;
			pConduit.SupportMeshes.Clear();
			pConduit.RaftSupportMeshes.Clear();
			_curmatint = 5;
			if (!ConnectGemvisionLicense())
			{
				sErr = "Can't load GV Lic";
				return false;
			}
			if (_printer == null)
			{
				sErr = "Printer not connected";
				return false;
			}
			if (m == null)
			{
				return false;
			}
			Mesh val = m.DuplicateMesh();
			BoundingBox boundingBox = ((GeometryBase)val).GetBoundingBox(false);
			pCenter = ((BoundingBox)(ref boundingBox)).Center;
			Point3d pointOnPlatform = GetPointOnPlatform(boundingBox);
			_vecMeshMove = Point3d.Subtract(pointOnPlatform, ((BoundingBox)(ref boundingBox)).Min);
			_vecMeshMoveBack = Point3d.Subtract(((BoundingBox)(ref boundingBox)).Min, pointOnPlatform);
			pConduit.VecMeshMoveBack = _vecMeshMoveBack;
			pConduit.VecPlatMoveBack = _vecMeshMoveBack;
			((GeometryBase)val).Translate(_vecMeshMove);
			_csitem = val.DuplicateMesh();
			_inputMeshes = new DataObjectArray();
			_tmpRhinoDocSupports = new List<Guid>();
			pConduit.BuildMeshes.Clear();
			pConduit.BuildMeshes = new List<Mesh>();
			pConduit.BuildMeshes.Add(val);
			pConduit.SupportMeshes.Clear();
			double num = 43.43;
			double num2 = 27.18;
			BoundaryRectangle = new RectangleF(0f, 0f, (float)num, (float)num2);
			pConduit.BuildPlatformMesh = Mesh.CreateFromPlane(Plane.WorldXY, new Interval(0.0, num), new Interval(0.0, num2), 1, 1);
			pConduit.PlatformBoundingBox = ((GeometryBase)pConduit.BuildPlatformMesh).GetBoundingBox(true);
			Mesh mesh = val.DuplicateMesh();
			Mesh val2 = new Mesh();
			GMesh item = MeshRepairHelper.OctTreeMeshFromMesh(mesh);
			GMesh gMeshOut = new GMesh();
			global::OctTree.OctTree.GetMesh(new List<GMesh> { item }, 11, out gMeshOut);
			Mesh val3 = MeshRepairHelper.MeshFromOctTreeMeshes(gMeshOut);
			val2.Append(val3);
			mesh = val2.DuplicateMesh();
			boundingBox = ((GeometryBase)mesh).GetBoundingBox(false);
			if (!AddRhinoMesh(mesh, out sErr))
			{
				return false;
			}
			pConduit.BuildMeshBoundingBox = ((GeometryBase)m).GetBoundingBox(true);
			try
			{
				string materialtype = string.Empty;
				string sStatus = string.Empty;
				GetCartridgeInfo(out materialtype, out sStatus, out iPercentLeft);
				if (materialtype == "MATID_VISIJET_FTX_GREEN")
				{
					_curmatint = 0;
				}
			}
			catch (Exception)
			{
				_curmatint = 5;
			}
			sErr = iPercentLeft + "%";
			return true;
		}
		catch (Exception)
		{
			sErr = "Error getting CS Model";
			return false;
		}
	}

	public void SetLoadedDialogState(bool bState)
	{
		_dialogloaded = bState;
	}

	public bool SetSupportScales(double UserScale, int SupType)
	{
		if (_supporttype < 3 && _supporttype >= 0)
		{
			_supporttype = SupType;
		}
		_userscale = UserScale;
		return true;
	}

	public bool UpdateMeshLocation(double dHeight)
	{
		pConduit.SupportMeshes.Clear();
		pConduit.RaftSupportMeshes.Clear();
		((DisplayConduit)pConduit).Enabled = false;
		pConduit.ModelTranslation = dHeight;
		foreach (Guid tmpRhinoDocSupport in _tmpRhinoDocSupports)
		{
			RhinoDoc.ActiveDoc.Objects.Delete(tmpRhinoDocSupport, true);
		}
		_tmpRhinoDocSupports = new List<Guid>();
		((DisplayConduit)pConduit).Enabled = true;
		RhinoDoc.ActiveDoc.Views.Redraw();
		return true;
	}

	public bool UpdateMeshRotation(double dDegreex, double dDegreey, double dDegreez, out string errorMessage)
	{
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		pConduit.SupportMeshes.Clear();
		pConduit.RaftSupportMeshes.Clear();
		pConduit.ModelRotationX.bEnabled = false;
		pConduit.ModelRotationX.bEnabled = false;
		pConduit.ModelRotationX.bEnabled = false;
		foreach (Guid tmpRhinoDocSupport in _tmpRhinoDocSupports)
		{
			RhinoDoc.ActiveDoc.Objects.Delete(tmpRhinoDocSupport, true);
		}
		_tmpRhinoDocSupports = new List<Guid>();
		Plane worldXY;
		if (dDegreex != 0.0)
		{
			pConduit.ModelRotationX.RotAngle = ToRadians(dDegreex);
			ProjetConduit.DrawRotation modelRotationX = pConduit.ModelRotationX;
			worldXY = Plane.WorldXY;
			modelRotationX.RotAxis = ((Plane)(ref worldXY)).ZAxis;
			pConduit.ModelRotationX.RotPoint = pCenter;
			pConduit.ModelRotationX.bEnabled = true;
		}
		if (dDegreey != 0.0)
		{
			pConduit.ModelRotationY.RotAngle = ToRadians(dDegreey);
			ProjetConduit.DrawRotation modelRotationY = pConduit.ModelRotationY;
			worldXY = Plane.WorldXY;
			modelRotationY.RotAxis = ((Plane)(ref worldXY)).YAxis;
			pConduit.ModelRotationY.RotPoint = pCenter;
			pConduit.ModelRotationY.bEnabled = true;
		}
		if (dDegreez != 0.0)
		{
			pConduit.ModelRotationZ.RotAngle = ToRadians(dDegreez);
			ProjetConduit.DrawRotation modelRotationZ = pConduit.ModelRotationZ;
			worldXY = Plane.WorldXY;
			modelRotationZ.RotAxis = ((Plane)(ref worldXY)).XAxis;
			pConduit.ModelRotationZ.RotPoint = pCenter;
			pConduit.ModelRotationZ.bEnabled = true;
		}
		errorMessage = "success";
		RhinoDoc.ActiveDoc.Views.Redraw();
		return true;
	}

	private BoundingBox GetBBSofItems(List<Mesh> lstgs)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox result = default(BoundingBox);
		for (int i = 0; i < lstgs.Count; i++)
		{
			GeometryBase val = (GeometryBase)(object)lstgs[i];
			((BoundingBox)(ref result)).Union(val.GetBoundingBox(true));
		}
		return result;
	}

	private int GetBuildTime()
	{
		try
		{
			GetBuildStatusInfo getBuildStatusInfo = new GetBuildStatusInfo(_printer);
			getBuildStatusInfo.run();
			return getBuildStatusInfo.getInfo().getTimeRemaining();
		}
		catch (Exception)
		{
			return 0;
		}
	}

	private Mesh GetDeformedCSModel(Mesh csMesh)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		Mesh val = new Mesh();
		new Mesh();
		GMesh item = MeshRepairHelper.OctTreeMeshFromMesh(_tempMesh);
		GMesh gMeshOut = new GMesh();
		global::OctTree.OctTree.GetMesh(new List<GMesh> { item }, 11, out gMeshOut);
		Mesh val2 = MeshRepairHelper.MeshFromOctTreeMeshes(gMeshOut);
		val.Append(val2);
		return val;
	}

	private bool GetModelPrepped(out string sStatus, out DataObjectArray dJob, List<Mesh> RhinoMainSupports, List<Mesh> RhinoRaftSupports, Mesh MainModel)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		dJob = new DataObjectArray();
		sStatus = string.Empty;
		if (pConduit.BuildMeshes.Count < 1)
		{
			sStatus = "No Model";
			return false;
		}
		if ((RhinoMainSupports.Count < 1 || RhinoRaftSupports.Count < 1) && _supporttype < 5)
		{
			sStatus = "No Supports";
			return false;
		}
		GetDeviceStatus(_printer, _printer.IpAddress, out var state, verbose: true, out sStatus);
		if (state != GPEnum.DeviceState.DEVICE_STATE_IDLE)
		{
			sStatus = "Build cancelled. " + state;
			return false;
		}
		int iPercentLeft = 0;
		if (GetCartridgeInfo(out var materialtype, out sStatus, out iPercentLeft) != GPEnum.ErrorCode.NO_ERROR_OCCURRED)
		{
			return false;
		}
		if (materialtype != "MATID_VISIJET_FTX_GRAY" && materialtype != "MATID_VISIJET_FTX_GREEN")
		{
			sStatus = "Material Cartridge Error";
			return false;
		}
		int supporttype = _supporttype;
		Mesh val = new Mesh();
		switch (supporttype)
		{
		case 1:
			foreach (Mesh RhinoRaftSupport in RhinoRaftSupports)
			{
				val.Append(RhinoRaftSupport);
			}
			val.Append(IncreaseReplaceRhinoMainSupports(1.5, bReplace: false));
			break;
		case 2:
			foreach (Mesh RhinoRaftSupport2 in RhinoRaftSupports)
			{
				val.Append(RhinoRaftSupport2);
			}
			val.Append(IncreaseReplaceRhinoMainSupports(1.5, bReplace: true));
			break;
		default:
		{
			Transform val2 = Transform.Translation(_vecMeshMoveBack);
			Transform identity = Transform.Identity;
			((Transform)(ref val2)).TryGetInverse(ref identity);
			foreach (Guid tmpRhinoDocSupport in _tmpRhinoDocSupports)
			{
				RhinoObject val3 = RhinoDoc.ActiveDoc.Objects.Find(tmpRhinoDocSupport);
				if (val3 != null)
				{
					GeometryBase geometry = val3.Geometry;
					Mesh val4 = (Mesh)(object)((geometry is Mesh) ? geometry : null);
					((GeometryBase)val4).Transform(identity);
					val.Append(val4);
				}
			}
			break;
		}
		case 3:
			break;
		}
		val.Append(GetDeformedCSModel(MainModel));
		dJob = GeoMeshFromRhinoMesh(val, out sStatus);
		return true;
	}

	private bool GetPointOutsideExistingMeshItems(List<Mesh> lstMeshtocheck, ref Mesh mFit)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0, 0.0, 0.0);
		BoundingBox boundingBox = ((GeometryBase)mFit).GetBoundingBox(false);
		BoundingBox bBSofItems = GetBBSofItems(lstMeshtocheck);
		double num = BoundaryRectangle.Width;
		Point3d val2 = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		int num2 = Convert.ToInt32(num - (x - ((Point3d)(ref val2)).X));
		double num3 = BoundaryRectangle.Height;
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val2)).Y;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		int num4 = Convert.ToInt32(num3 - (y - ((Point3d)(ref val2)).Y));
		if (lstMeshtocheck.Count == 0)
		{
			Vector3d val3 = Point3d.Subtract(val, ((BoundingBox)(ref boundingBox)).Min);
			((GeometryBase)mFit).Translate(val3);
			return true;
		}
		if (num2 < 0 || num4 < 0)
		{
			return true;
		}
		BoundingBox val4 = default(BoundingBox);
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j <= num4; j++)
			{
				((Point3d)(ref val))._002Ector((double)i, (double)j, 0.0);
				if (!((BoundingBox)(ref bBSofItems)).Contains(val))
				{
					((BoundingBox)(ref val4))._002Ector((IEnumerable<Point3d>)((BoundingBox)(ref boundingBox)).GetCorners());
					Vector3d val5 = Point3d.Subtract(val, ((BoundingBox)(ref boundingBox)).Min);
					((BoundingBox)(ref val4)).Transform(Transform.Translation(val5));
					BoundingBox val6 = BoundingBox.Intersection(val4, bBSofItems);
					if (!((BoundingBox)(ref val6)).IsValid)
					{
						((GeometryBase)mFit).Translate(val5);
						return true;
					}
				}
			}
		}
		return false;
	}

	private bool SendToPrinter(out string sStatus, DataObjectArray Model)
	{
		sStatus = string.Empty;
		try
		{
			string text = Path.Combine(Path.GetTempPath(), "CS Model");
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			AutoPlace(Model);
			DataObjectArray modelSupports = new DataObjectArray();
			DataObjectArray modelNative = new DataObjectArray();
			if (_supporttype == 3)
			{
				modelSupports = cloneDataObjectArray(_supportMeshes);
				modelNative = cloneDataObjectArray(_supportNativeData);
			}
			if (GenerateBuildFile(Model, modelSupports, modelNative, text, out sStatus) != GPEnum.ErrorCode.NO_ERROR_OCCURRED)
			{
				sStatus = "Failed Generating Build File";
				return false;
			}
			if (SendBuildJob(text, out sStatus) != GPEnum.ErrorCode.NO_ERROR_OCCURRED)
			{
				sStatus = "Failed Sending Build File to Projet";
				return false;
			}
			try
			{
				GetBuildStatusInfo getBuildStatusInfo = new GetBuildStatusInfo(_printer);
				getBuildStatusInfo.run();
				BuildStatusInfo info = getBuildStatusInfo.getInfo();
				sStatus = info.getTotalTime() + " minutes";
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
		catch (Exception)
		{
			return false;
		}
		finally
		{
			RhinoDoc.ActiveDoc.Objects.Delete(((IEnumerable<RhinoObject>)RhinoDoc.ActiveDoc.Objects).Select((RhinoObject _) => _.Id), true);
		}
	}

	private static double ToDegrees(double angleRadians)
	{
		return On_RADIANS_TO_DEGREES * angleRadians;
	}

	private static double ToRadians(double angleDegrees)
	{
		return On_DEGREES_TO_RADIANS * angleDegrees;
	}

	public string GetPrinterMaterial()
	{
		string result = "Green";
		if (_curmatint == 5)
		{
			result = "Gray";
		}
		return result;
	}

	private DataObjectArray cloneDataObjectArray(DataObjectArray doa)
	{
		doa.grab();
		return new DataObjectArray((IntPtr)DataObjectArray.getCPtr(doa), cMemoryOwn: true);
	}

	private List<string> GetMaterialList(Printer printer)
	{
		List<string> list = new List<string>();
		IntArray partMaterialList = new SetupPrinterParameters(printer).getPartMaterialList("");
		ConvertNameandIDUtil convertNameandIDUtil = new ConvertNameandIDUtil();
		foreach (int item2 in partMaterialList)
		{
			string item = convertNameandIDUtil.ConvertMaterialIDtoName((GPEnum.MaterialModelID)item2);
			list.Add(item);
		}
		return list;
	}

	private GPEnum.ErrorCode SendBuildJob(string Filename, out string sErr)
	{
		sErr = string.Empty;
		if (!File.Exists(Filename))
		{
			return GPEnum.ErrorCode.FAILED_TO_LOAD_BUILD;
		}
		SendBuildJob sendBuildJob = new SendBuildJob(_printer);
		try
		{
			sendBuildJob.Buildfilepath = Filename;
			sendBuildJob.UseQueueAgent = false;
			preprocess("Sending Build...");
			sendBuildJob.run();
			postprocess();
		}
		catch (Exception ex)
		{
			sErr = ex.Message;
		}
		return (GPEnum.ErrorCode)sendBuildJob.ErrorCode;
	}

	private void AutoPlace(DataObjectArray Meshes, bool z0 = false)
	{
		if (Meshes == null)
		{
			return;
		}
		AutoPlace autoPlace = new AutoPlace(_printer);
		double arg = 0.0;
		new ManagePreset(_printer);
		try
		{
			if (z0)
			{
				arg = 0.0;
			}
			Vector3D margin = new Vector3D(0.0, 0.0, arg);
			autoPlace.Meshes = Meshes;
			autoPlace.Margin = margin;
			autoPlace.run();
			int count = Meshes.Count;
			for (int i = 0; i < count; i++)
			{
				(Meshes[i] as Mesh).transform(autoPlace.getTransformAt(i));
			}
		}
		catch (Exception)
		{
		}
	}

	private DataObjectArray GeoMeshFromRhinoMesh(Mesh mesh, out string sErr)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		sErr = string.Empty;
		DataObjectArray dataObjectArray = new DataObjectArray();
		try
		{
			CreateMesh createMesh = new CreateMesh();
			Mesh mesh2 = null;
			int count = mesh.Faces.Count;
			for (int i = 0; i < count; i++)
			{
				MeshFace val = mesh.Faces[i];
				Point3f val2 = mesh.Vertices[((MeshFace)(ref val)).A];
				Point3f val3 = mesh.Vertices[((MeshFace)(ref val)).B];
				Point3f val4 = mesh.Vertices[((MeshFace)(ref val)).C];
				int a = createMesh.addPoint(new Vector3D(((Point3f)(ref val2)).X, ((Point3f)(ref val2)).Y, ((Point3f)(ref val2)).Z));
				int b = createMesh.addPoint(new Vector3D(((Point3f)(ref val3)).X, ((Point3f)(ref val3)).Y, ((Point3f)(ref val3)).Z));
				int c = createMesh.addPoint(new Vector3D(((Point3f)(ref val4)).X, ((Point3f)(ref val4)).Y, ((Point3f)(ref val4)).Z));
				createMesh.addTriangle(a, b, c);
			}
			createMesh.MergeVertices = 1;
			createMesh.run();
			mesh2 = createMesh.Msh;
			mesh2.transform(Transform3D.scale(0.001));
			dataObjectArray.Add(mesh2);
		}
		catch (Exception ex)
		{
			sErr = ex.Message;
		}
		return dataObjectArray;
	}

	private bool AddRhinoMesh(Mesh mesh, out string sErr)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		sErr = string.Empty;
		try
		{
			_tempMesh = mesh.DuplicateMesh();
			CreateMesh createMesh = new CreateMesh();
			Mesh mesh2 = null;
			int count = mesh.Faces.Count;
			for (int i = 0; i < count; i++)
			{
				MeshFace val = mesh.Faces[i];
				Point3f val2 = mesh.Vertices[((MeshFace)(ref val)).A];
				Point3f val3 = mesh.Vertices[((MeshFace)(ref val)).B];
				Point3f val4 = mesh.Vertices[((MeshFace)(ref val)).C];
				int a = createMesh.addPoint(new Vector3D(((Point3f)(ref val2)).X, ((Point3f)(ref val2)).Y, ((Point3f)(ref val2)).Z));
				int b = createMesh.addPoint(new Vector3D(((Point3f)(ref val3)).X, ((Point3f)(ref val3)).Y, ((Point3f)(ref val3)).Z));
				int c = createMesh.addPoint(new Vector3D(((Point3f)(ref val4)).X, ((Point3f)(ref val4)).Y, ((Point3f)(ref val4)).Z));
				createMesh.addTriangle(a, b, c);
			}
			createMesh.MergeVertices = 1;
			createMesh.run();
			mesh2 = createMesh.Msh;
			mesh2.transform(Transform3D.scale(0.001));
			_inputMeshes.Add(mesh2);
		}
		catch (Exception ex)
		{
			sErr = ex.Message;
			return false;
		}
		return true;
	}

	public void SetSupportDensity(int iSupportDensity)
	{
		_intsupportdensity = Math.Abs(iSupportDensity);
		if (iSupportDensity == 0)
		{
			pConduit.SupportMeshes = _rhinotempmainsupports;
			((DisplayConduit)pConduit).Enabled = true;
			RhinoDoc.ActiveDoc.Views.Redraw();
			return;
		}
		int num = 0;
		int num2 = Math.Abs(iSupportDensity) + 1;
		num2++;
		_rhinomainsupports = new List<Mesh>();
		foreach (Mesh rhinotempmainsupport in _rhinotempmainsupports)
		{
			if (num < num2 + 2)
			{
				_rhinomainsupports.Add(rhinotempmainsupport.DuplicateMesh());
				num++;
			}
			else if (num > _rhinotempmainsupports.Count - (num2 + 2))
			{
				_rhinomainsupports.Add(rhinotempmainsupport.DuplicateMesh());
				num++;
			}
			else if (num == _rhinotempmainsupports.Count / 2 - (num2 + 1))
			{
				_rhinomainsupports.Add(rhinotempmainsupport.DuplicateMesh());
				num++;
			}
			else if (num == _rhinotempmainsupports.Count / 2 - (num2 - 1))
			{
				_rhinomainsupports.Add(rhinotempmainsupport.DuplicateMesh());
				num++;
			}
			else
			{
				num++;
			}
		}
		pConduit.SupportMeshes = _rhinomainsupports;
		((DisplayConduit)pConduit).Enabled = true;
		RhinoDoc.ActiveDoc.Views.Redraw();
	}

	public bool FindProjetPrinter(out string FoundAddress)
	{
		Directory.SetCurrentDirectory(GetPathOfAssembly());
		FoundAddress = string.Empty;
		if (!ConnectGemvisionLicense())
		{
			return false;
		}
		FindPrintersOnNetwork findPrintersOnNetwork = new FindPrintersOnNetwork();
		try
		{
			findPrintersOnNetwork.run();
			if (findPrintersOnNetwork.FoundPrinters == null)
			{
				return false;
			}
			if (findPrintersOnNetwork.FoundPrinters.Count > 0)
			{
				IProperty property = findPrintersOnNetwork.FoundPrinters[0].GetProperty("ipAddress");
				FoundAddress = property.Value.ToString();
				return true;
			}
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	private DataObjectArray FindPrinters(string IPAddress, out string Err)
	{
		FindPrintersOnNetwork findPrintersOnNetwork = new FindPrintersOnNetwork();
		Err = string.Empty;
		if (!string.IsNullOrEmpty(IPAddress))
		{
			findPrintersOnNetwork.setTargetIpAddressStr(IPAddress);
		}
		try
		{
			findPrintersOnNetwork.run();
			if (findPrintersOnNetwork.FoundPrinters == null)
			{
				throw new Exception("No printer found.");
			}
		}
		catch (Exception ex)
		{
			Err = ex.Message;
		}
		return findPrintersOnNetwork.FoundPrinters;
	}

	private Point3d GetPointOnPlatform(BoundingBox bb)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = ((BoundingBox)(ref bb)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref bb)).Min;
		double num = x - ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref bb)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref bb)).Min;
		double num2 = y - ((Point3d)(ref val)).Y;
		return new Point3d(((double)BoundaryRectangle.Width - num) / 2.0, ((double)BoundaryRectangle.Height - num2) / 2.0, 3.0);
	}

	public void ResetProjet()
	{
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		if (_printer != null)
		{
			_printer.Dispose();
		}
		if (_inputMeshes != null)
		{
			_inputMeshes.Dispose();
		}
		if (_tmpRhinoDocSupports != null)
		{
			foreach (Guid tmpRhinoDocSupport in _tmpRhinoDocSupports)
			{
				RhinoDoc.ActiveDoc.Objects.Delete(tmpRhinoDocSupport, true);
			}
			_tmpRhinoDocSupports = new List<Guid>();
		}
		_printerOnline = false;
		if (_supportMeshes != null)
		{
			_supportMeshes.Dispose();
		}
		if (_raftMeshes != null)
		{
			_raftMeshes.Dispose();
		}
		((DisplayConduit)pConduit).Enabled = false;
		pConduit.BuildMeshes.Clear();
		pConduit.RaftSupportMeshes.Clear();
		pConduit.SupportMeshes.Clear();
		pCenter = new Point3d(0.0, 0.0, 0.0);
		pConduit.ModelRotationX.RotAngle = 0.0;
		pConduit.ModelRotationY.RotAngle = 0.0;
		pConduit.ModelRotationZ.RotAngle = 0.0;
		pConduit.ModelRotationX.bEnabled = false;
		pConduit.ModelRotationY.bEnabled = false;
		pConduit.ModelRotationZ.bEnabled = false;
		pConduit.ModelTranslation = 0.0;
		_csitem = null;
		if (_tempMesh != null)
		{
			((CommonObject)_tempMesh).Dispose();
		}
		if (_rhinomainsupports != null)
		{
			_rhinomainsupports.Clear();
		}
		if (_rhinotempmainsupports != null)
		{
			_rhinotempmainsupports.Clear();
		}
		if (_rhinoraftsupports != null)
		{
			_rhinoraftsupports.Clear();
		}
		if (_tmpSups != null)
		{
			_tmpSups.Dispose();
		}
	}

	private GPEnum.ErrorCode GenerateBuildFile(DataObjectArray ModelMeshes, DataObjectArray ModelSupports, DataObjectArray ModelNative, string Filename, out string sErr)
	{
		sErr = string.Empty;
		GPEnum.ErrorCode errorCode = GPEnum.ErrorCode.NO_ERROR_OCCURRED;
		try
		{
			int iPercentLeft = 0;
			string materialtype = string.Empty;
			if (GetCartridgeInfo(out materialtype, out sErr, out iPercentLeft) != GPEnum.ErrorCode.NO_ERROR_OCCURRED)
			{
				return GPEnum.ErrorCode.NO_REGISTERED_PRINTERS;
			}
			if (materialtype != "MATID_VISIJET_FTX_GRAY" && materialtype != "MATID_VISIJET_FTX_GREEN")
			{
				sErr = "Material Cartridge Error";
				return GPEnum.ErrorCode.FAILED_TO_BUILD_CARTRDIGE_IS_INVALID_OR_NOT_FOUND;
			}
			GenerateBuildFile generateBuildFile = new GenerateBuildFile(_printer);
			generateBuildFile.ModelMeshArray = ModelMeshes;
			generateBuildFile.BuildFilePath = Filename;
			generateBuildFile.ShrinkCompX = 0.0;
			generateBuildFile.ShrinkCompY = 0.0;
			generateBuildFile.SupportNativeDataArray = ModelNative;
			generateBuildFile.SupportMeshArray = ModelSupports;
			generateBuildFile.run();
			errorCode = (GPEnum.ErrorCode)generateBuildFile.ErrorCode;
		}
		catch (Exception ex)
		{
			sErr = ex.Message;
			return GPEnum.ErrorCode.FAILED_TO_PUT_FILE;
		}
		if (!File.Exists(Filename))
		{
			errorCode = GPEnum.ErrorCode.FAILED_TO_CREATE_BUILD_FILE;
		}
		return errorCode;
	}

	private GPEnum.ErrorCode GetCartridgeInfo(out string materialtype, out string sStatus, out int iPercentLeft)
	{
		materialtype = "";
		GetCartridgeInfo getCartridgeInfo = new GetCartridgeInfo(_printer);
		CartridgeInfo cartridgeInfo = null;
		sStatus = string.Empty;
		iPercentLeft = 0;
		try
		{
			getCartridgeInfo.run();
			cartridgeInfo = getCartridgeInfo.getInfo();
			if (cartridgeInfo == null || cartridgeInfo.emptyOfCartridgeHeadInfo())
			{
				sStatus = "Projet 1200 Print Cartridge was not detected.";
				return (GPEnum.ErrorCode)getCartridgeInfo.ErrorCode;
			}
			uint num = (uint)cartridgeInfo.sizeOfCartridgeHeadInfo();
			for (uint num2 = 0u; num2 < num; num2++)
			{
				CartridgeHeadInfo cartridgeHeadInfoAt = cartridgeInfo.getCartridgeHeadInfoAt(num2);
				if (cartridgeHeadInfoAt != null)
				{
					materialtype = cartridgeHeadInfoAt.getMaterial().ToString();
					ushort percentRemaining = cartridgeHeadInfoAt.getPercentRemaining();
					iPercentLeft = Convert.ToInt32(percentRemaining);
				}
			}
		}
		catch (Exception ex)
		{
			sStatus = ex.Message;
		}
		return (GPEnum.ErrorCode)getCartridgeInfo.ErrorCode;
	}

	private static string GetPathOfAssembly()
	{
		return Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
	}

	private bool PrintPrePassCheck(out string ErrorMessage)
	{
		Directory.SetCurrentDirectory(GetPathOfAssembly());
		ErrorMessage = string.Empty;
		string materialtype = string.Empty;
		int iPercentLeft = 0;
		GPEnum.ErrorCode cartridgeInfo = GetCartridgeInfo(out materialtype, out ErrorMessage, out iPercentLeft);
		if (cartridgeInfo != GPEnum.ErrorCode.NO_ERROR_OCCURRED)
		{
			ErrorMessage = cartridgeInfo.ToString();
			return false;
		}
		if (materialtype != "MATID_VISIJET_FTX_GRAY")
		{
			ErrorMessage = "FTX Gray not current loaded material";
			return false;
		}
		cartridgeInfo = GetDeviceStatus(_printer, _ipAddress, out var state, verbose: true, out ErrorMessage);
		if (state != GPEnum.DeviceState.DEVICE_STATE_IDLE)
		{
			ErrorMessage = cartridgeInfo.ToString();
			return false;
		}
		return true;
	}

	private bool ConnectGemvisionLicense()
	{
		Directory.SetCurrentDirectory(GetPathOfAssembly());
		oaDrawnSupports.SetDisplayModeOverride(DisplayModeDescription.FindByName("Shaded"));
		oaDrawnSupports.ObjectColor = Color.Red;
		byte[] clcBinary = new byte[32]
		{
			121, 241, 229, 235, 72, 219, 84, 83, 62, 148,
			9, 231, 75, 14, 76, 157, 232, 220, 93, 93,
			17, 146, 45, 30, 92, 119, 184, 220, 47, 81,
			200, 38
		};
		Bootstrapper bootstrapper = new Bootstrapper();
		int swapSizeMB = 0;
		bootstrapper.setMemoryManagerParameters(-1, swapSizeMB, -1);
		string folderPath = FileSystem.GetFolderPath(FileSystem.SpecialFolder.Executing);
		if (!bootstrapper.init(folderPath, folderPath, "WatsonHeadless", "1.0.0.0", clcBinary))
		{
			return false;
		}
		GeoPrint.Initialize();
		string masterLocalDBFolderPath = Path.Combine(folderPath, "PrinterDB");
		try
		{
			if (_lstSavedJobs == null)
			{
				_lstSavedJobs = new List<DataObject>(4);
				for (int i = 0; i < 3; i++)
				{
					_lstSavedJobs.Add(new DataObject());
				}
			}
			PrinterEnvironments printerEnvironments = new PrinterEnvironments();
			printerEnvironments.setMasterLocalDBFolderPath(masterLocalDBFolderPath);
			printerEnvironments.setTempFolderPath(Path.GetTempPath());
			printerEnvironments.initialize();
		}
		catch (Exception)
		{
			RhinoApp.WriteLine("Failed to initialize printer environments");
		}
		return true;
	}

	private void IncreaseRhinoRaftSupports(double scale)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		if (scale == 1.0)
		{
			return;
		}
		Plane worldXY = Plane.WorldXY;
		Transform identity = Transform.Identity;
		new DataObjectArray();
		_ = string.Empty;
		foreach (Mesh rhinoraftsupport in _rhinoraftsupports)
		{
			identity = Transform.Scale(worldXY, 1.0, 1.0, scale);
			((GeometryBase)rhinoraftsupport).Transform(identity);
		}
	}

	private Mesh ReplaceRaftMeshes()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		Mesh val = new Mesh();
		Mesh val2 = new Mesh();
		foreach (Mesh rhinoraftsupport in _rhinoraftsupports)
		{
			val.Append(rhinoraftsupport);
		}
		BoundingBox boundingBox = ((GeometryBase)val).GetBoundingBox(true);
		return Mesh.CreateFromBox((IEnumerable<Point3d>)((BoundingBox)(ref boundingBox)).GetCorners(), 20, 20, 20);
	}

	private Mesh IncreaseReplaceRhinoMainSupports(double scale, bool bReplace)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		Plane worldXY = Plane.WorldXY;
		Transform identity = Transform.Identity;
		_ = string.Empty;
		Mesh val = new Mesh();
		Plane val7 = default(Plane);
		Rectangle3d val8 = default(Rectangle3d);
		Point3d val9 = default(Point3d);
		Point3d val10 = default(Point3d);
		foreach (Mesh rhinomainsupport in _rhinomainsupports)
		{
			Mesh val2 = rhinomainsupport.DuplicateMesh();
			BoundingBox boundingBox = ((GeometryBase)rhinomainsupport).GetBoundingBox(true);
			((Plane)(ref worldXY)).Origin = ((BoundingBox)(ref boundingBox)).Center;
			if (scale != 1.0)
			{
				identity = Transform.Scale(worldXY, scale, scale, 1.0);
				((GeometryBase)val2).Transform(identity);
			}
			if (!bReplace)
			{
				val.Append(val2);
				continue;
			}
			BoundingBox boundingBox2 = ((GeometryBase)val2).GetBoundingBox(true);
			Point3d[] corners = ((BoundingBox)(ref boundingBox2)).GetCorners();
			_ = ((BoundingBox)(ref boundingBox2)).Center;
			double num = ((Point3d)(ref corners[0])).DistanceTo(corners[2]);
			num /= 2.0;
			Mesh val3 = Mesh.CreateFromBox((IEnumerable<Point3d>)((BoundingBox)(ref boundingBox2)).GetCorners(), 2, 2, 2);
			Plane worldXY2 = Plane.WorldXY;
			Point3d val4 = ((BoundingBox)(ref boundingBox2)).Max;
			((Plane)(ref worldXY2)).OriginZ = ((Point3d)(ref val4)).Z + 0.1;
			Polyline[] outlines = val3.GetOutlines(worldXY2);
			Curve val5 = (Curve)new PolyCurve();
			Polyline[] array = outlines;
			for (int i = 0; i < array.Length; i++)
			{
				val5 = (Curve)(object)array[i].ToNurbsCurve();
				Curve obj = val5;
				boundingBox = ((GeometryBase)val5).GetBoundingBox(false);
				((GeometryBase)obj).Transform(Transform.Scale(((BoundingBox)(ref boundingBox)).Center, 0.5));
			}
			worldXY2 = Plane.WorldXY;
			val4 = ((BoundingBox)(ref boundingBox2)).Min;
			((Plane)(ref worldXY2)).OriginZ = ((Point3d)(ref val4)).Z;
			Polyline[] outlines2 = val3.GetOutlines(worldXY2);
			Curve item = (Curve)new PolyCurve();
			array = outlines2;
			for (int i = 0; i < array.Length; i++)
			{
				item = (Curve)(object)array[i].ToNurbsCurve();
			}
			val4 = ((BoundingBox)(ref boundingBox2)).Min;
			double x = ((Point3d)(ref val4)).X;
			val4 = ((BoundingBox)(ref boundingBox2)).Min;
			double y = ((Point3d)(ref val4)).Y;
			val4 = ((BoundingBox)(ref boundingBox2)).Center;
			Point3d val6 = new Point3d(x, y, ((Point3d)(ref val4)).Z);
			Plane worldXY3 = Plane.WorldXY;
			((Plane)(ref val7))._002Ector(val6, ((Plane)(ref worldXY3)).Normal);
			((Rectangle3d)(ref val8))._002Ector(val7, num * 1.5, num * 1.5);
			val4 = ((BoundingBox)(ref boundingBox2)).Center;
			double x2 = ((Point3d)(ref val4)).X;
			val4 = ((BoundingBox)(ref boundingBox2)).Center;
			double y2 = ((Point3d)(ref val4)).Y;
			val4 = ((BoundingBox)(ref boundingBox2)).Min;
			((Point3d)(ref val9))._002Ector(x2, y2, ((Point3d)(ref val4)).Z);
			val4 = ((BoundingBox)(ref boundingBox2)).Center;
			double x3 = ((Point3d)(ref val4)).X;
			val4 = ((BoundingBox)(ref boundingBox2)).Center;
			double y3 = ((Point3d)(ref val4)).Y;
			val4 = ((BoundingBox)(ref boundingBox2)).Max;
			((Point3d)(ref val10))._002Ector(x3, y3, ((Point3d)(ref val4)).Z);
			Brep[] array2 = Brep.CreateFromLoft((IEnumerable<Curve>)new List<Curve>
			{
				item,
				(Curve)(object)((Rectangle3d)(ref val8)).ToNurbsCurve(),
				val5
			}, val9, val10, (LoftType)3, true);
			for (int i = 0; i < array2.Length; i++)
			{
				Mesh[] array3 = Mesh.CreateFromBrep(array2[i], MeshingParameters.Coarse);
				foreach (Mesh val11 in array3)
				{
					val11.Faces.ConvertQuadsToTriangles();
					val11.Normals.ComputeNormals();
					val.Append(val11);
				}
			}
		}
		return val;
	}

	private DataObjectArray IncreaseRaftSupports(DataObjectArray raftSupports, double scale)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (scale == 1.0)
		{
			return raftSupports;
		}
		List<Mesh> list = RhinoMeshFromGeoMesh(raftSupports);
		Plane worldXY = Plane.WorldXY;
		Transform identity = Transform.Identity;
		DataObjectArray dataObjectArray = new DataObjectArray();
		string sErr = string.Empty;
		foreach (Mesh item in list)
		{
			BoundingBox boundingBox = ((GeometryBase)item).GetBoundingBox(true);
			((Plane)(ref worldXY)).Origin = ((BoundingBox)(ref boundingBox)).Center;
			identity = Transform.Scale(worldXY, 1.0, 1.0, scale);
			((GeometryBase)item).Transform(identity);
			DataObjectArray dataObjectArray2 = GeoMeshFromRhinoMesh(item, out sErr);
			if (!(dataObjectArray2 == null) && dataObjectArray2.Count != 0)
			{
				dataObjectArray.Add(dataObjectArray2[0]);
			}
		}
		return dataObjectArray;
	}

	private DataObjectArray IncreaseMainSupports(DataObjectArray regSupports, double scale)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		if (scale == 1.0)
		{
			return regSupports;
		}
		List<Mesh> list = RhinoMeshFromGeoMesh(regSupports);
		Plane worldXY = Plane.WorldXY;
		Transform identity = Transform.Identity;
		DataObjectArray dataObjectArray = new DataObjectArray();
		string sErr = string.Empty;
		foreach (Mesh item in list)
		{
			BoundingBox boundingBox = ((GeometryBase)item).GetBoundingBox(true);
			((Plane)(ref worldXY)).Origin = ((BoundingBox)(ref boundingBox)).Center;
			identity = Transform.Scale(worldXY, scale, scale, 1.0);
			((GeometryBase)item).Transform(identity);
			DataObjectArray dataObjectArray2 = GeoMeshFromRhinoMesh(item, out sErr);
			if (!(dataObjectArray2 == null) && dataObjectArray2.Count != 0)
			{
				dataObjectArray.Add(dataObjectArray2[0]);
			}
		}
		return dataObjectArray;
	}

	private List<Mesh> IncreaseRhinoRaftSupports(DataObjectArray raftSupports)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		List<Mesh> list = RhinoMeshFromGeoMesh(raftSupports);
		if (_userscale == 1.0)
		{
			return list;
		}
		List<Mesh> list2 = new List<Mesh>();
		Plane worldXY = Plane.WorldXY;
		Transform identity = Transform.Identity;
		_ = string.Empty;
		foreach (Mesh item in list)
		{
			BoundingBox boundingBox = ((GeometryBase)item).GetBoundingBox(true);
			((Plane)(ref worldXY)).Origin = ((BoundingBox)(ref boundingBox)).Center;
			identity = Transform.Scale(worldXY, 1.0, 1.0, _userscale);
			((GeometryBase)item).Transform(identity);
			list2.Add(item);
		}
		return list2;
	}

	private List<Mesh> IncreaseRhinoMainSupports(DataObjectArray regSupports)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		List<Mesh> list = RhinoMeshFromGeoMesh(regSupports);
		if (_userscale == 1.0)
		{
			return list;
		}
		Plane worldXY = Plane.WorldXY;
		Transform identity = Transform.Identity;
		List<Mesh> list2 = new List<Mesh>();
		_ = string.Empty;
		foreach (Mesh item in list)
		{
			BoundingBox boundingBox = ((GeometryBase)item).GetBoundingBox(true);
			((Plane)(ref worldXY)).Origin = ((BoundingBox)(ref boundingBox)).Center;
			identity = Transform.Scale(worldXY, _userscale, _userscale, 1.0);
			((GeometryBase)item).Transform(identity);
			list2.Add(item);
		}
		return list2;
	}

	private GPEnum.ErrorCode GetDeviceStatus(Printer prn, string IpAddress, out GPEnum.DeviceState state, bool verbose, out string sStatus)
	{
		sStatus = string.Empty;
		if (prn == null)
		{
			state = GPEnum.DeviceState.DEVICE_STATE_ERROR;
			return GPEnum.ErrorCode.DEVICE_IS_NOT_CONNECTED;
		}
		GetPrinterStatus getPrinterStatus = new GetPrinterStatus(prn);
		if (verbose)
		{
			try
			{
				getPrinterStatus.run();
			}
			catch (Exception ex)
			{
				sStatus = ex.Message;
			}
		}
		state = (GPEnum.DeviceState)getPrinterStatus.DeviceStatus;
		GPEnum.ErrorCode errorCode = (GPEnum.ErrorCode)getPrinterStatus.ErrorCode;
		if (verbose)
		{
			sStatus = "Projet 1200 IPAddress: " + IpAddress + "Status: " + state;
		}
		return errorCode;
	}

	private List<Mesh> RhinoMeshFromGeoMesh(DataObjectArray meshes)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (meshes == null || !meshes.Any())
			{
				return new List<Mesh>();
			}
			List<Mesh> list = new List<Mesh>();
			int count = meshes.Count;
			for (int i = 0; i < count; i++)
			{
				if (!(meshes[i] is Mesh))
				{
					continue;
				}
				Mesh mesh = (Mesh)meshes[i];
				if (mesh != null)
				{
					int maxTriangle = mesh.MaxTriangle;
					_ = mesh.NumPoints;
					Mesh val = new Mesh();
					for (int j = 0; j < maxTriangle; j++)
					{
						int vertexIndex = mesh.getVertexIndex(j, 0, 0);
						int vertexIndex2 = mesh.getVertexIndex(j, 0, 1);
						int vertexIndex3 = mesh.getVertexIndex(j, 0, 2);
						Vector3D coordinate = mesh.getCoordinate(vertexIndex);
						Vector3D coordinate2 = mesh.getCoordinate(vertexIndex2);
						Vector3D coordinate3 = mesh.getCoordinate(vertexIndex3);
						val.Vertices.SetVertex(vertexIndex, coordinate.X, coordinate.Y, coordinate.Z);
						val.Vertices.SetVertex(vertexIndex2, coordinate2.X, coordinate2.Y, coordinate2.Z);
						val.Vertices.SetVertex(vertexIndex3, coordinate3.X, coordinate3.Y, coordinate3.Z);
						val.Faces.SetFace(j, new MeshFace(vertexIndex, vertexIndex2, vertexIndex3));
						Vector3D triangleNormal = mesh.getTriangleNormal(j);
						val.FaceNormals.AddFaceNormal(triangleNormal.X, triangleNormal.Y, triangleNormal.Z);
					}
					val.Normals.ComputeNormals();
					val.Compact();
					val.Unweld(0.0, true);
					((GeometryBase)val).Scale(1000.0);
					list.Add(val);
				}
			}
			return list;
		}
		catch (Exception)
		{
			return null;
		}
	}

	private void preprocess(string message)
	{
	}

	private void postprocess()
	{
	}

	private GPEnum.ErrorCode GenerateSupport(DataObjectArray Meshes, out string sErr, out DataObjectArray Supports, out DataObjectArray NativeData, out DataObjectArray RaftMeshes)
	{
		Supports = new DataObjectArray();
		RaftMeshes = new DataObjectArray();
		NativeData = new DataObjectArray();
		GenerateSupport generateSupport = new GenerateSupport(_printer);
		sErr = string.Empty;
		try
		{
			generateSupport.ModelMeshArray = Meshes;
			generateSupport.RaftMeshOnly = false;
			generateSupport.run();
			RaftMeshes = generateSupport.RaftMeshArray;
			NativeData = generateSupport.SupportNativeDataArray;
			Supports = generateSupport.SupportMeshArray;
		}
		catch (Exception)
		{
			sErr = "Support Generation Failed to Complete. Check that all parts fit within the build extents.";
			return GPEnum.ErrorCode.FAILED_TO_MODEL_IS_OUT_OF_PLATFORM;
		}
		return (GPEnum.ErrorCode)generateSupport.ErrorCode;
	}

	private void SetPrinterMaterial(int materialToUse)
	{
		SetupPrinterParameters setupPrinterParameters = new SetupPrinterParameters(_printer);
		IntArray partMaterialList = setupPrinterParameters.getPartMaterialList("");
		StringArray buildModeList = setupPrinterParameters.getBuildModeList(partMaterialList[materialToUse]);
		StringArray valueSetNameList = setupPrinterParameters.getValueSetNameList(buildModeList[0], partMaterialList[materialToUse]);
		setupPrinterParameters.PartMaterialList = new IntArray { partMaterialList[materialToUse] };
		setupPrinterParameters.BuildMode = buildModeList[0];
		setupPrinterParameters.ValueSetName = valueSetNameList[0];
		setupPrinterParameters.run();
	}

	private Bitmap LoadImageFromFile(string fileName)
	{
		FileStream fileStream = null;
		Bitmap bitmap = null;
		try
		{
			fileStream = File.OpenRead(fileName);
			MemoryStream memoryStream = new MemoryStream();
			byte[] buffer = new byte[10000];
			while (true)
			{
				int num = fileStream.Read(buffer, 0, 10000);
				if (num == 0)
				{
					break;
				}
				memoryStream.Write(buffer, 0, num);
			}
			return (Bitmap)System.Drawing.Image.FromStream(memoryStream);
		}
		finally
		{
			if (fileStream != null)
			{
				fileStream.Close();
				fileStream.Dispose();
			}
		}
	}

	public bool UpdateFirmwareAndActivate(bool bDoFirmware, bool bDoActivate, string sEmail, string sCompany, out string sError)
	{
		sError = string.Empty;
		string text = Path.Combine(GetPathOfAssembly(), "PrinterDB\\firmware_V1.0.53.0.zip");
		GPEnum.ErrorCode errorCode = GPEnum.ErrorCode.NO_ERROR_OCCURRED;
		bool result = true;
		if (bDoActivate)
		{
			GetPrinterActivationState getPrinterActivationState = new GetPrinterActivationState(_printer);
			try
			{
				getPrinterActivationState.run();
				if (getPrinterActivationState.Activated)
				{
					sError = "Already activated";
					result = false;
				}
				else
				{
					QueryActivationCode queryActivationCode = new QueryActivationCode(_printer);
					queryActivationCode.Email = sEmail;
					queryActivationCode.Company = sCompany;
					queryActivationCode.SerialNumber = getPrinterActivationState.SerialNumber;
					queryActivationCode.Address1 = "No Address Given";
					queryActivationCode.Address2 = "No Address Given";
					queryActivationCode.FirstName = "FirstName";
					queryActivationCode.LastName = "LastName";
					queryActivationCode.Country = "Country";
					queryActivationCode.Phone = "Phone";
					queryActivationCode.PostalCode = "PostalCode";
					queryActivationCode.run();
					ActivatePrinter activatePrinter = new ActivatePrinter(_printer);
					activatePrinter.ActivationCode = queryActivationCode.ActivationCode;
					activatePrinter.SerialNumber = getPrinterActivationState.SerialNumber;
					activatePrinter.run();
					errorCode = (GPEnum.ErrorCode)getPrinterActivationState.ErrorCode;
				}
			}
			catch (Exception ex)
			{
				sError = ex.Message;
				result = false;
			}
		}
		if (bDoFirmware)
		{
			SendFirmwareFile sendFirmwareFile = new SendFirmwareFile(_printer);
			try
			{
				if (!File.Exists(text))
				{
					sError += " Firmware file doesn't exist";
					result = false;
				}
				else
				{
					sendFirmwareFile.FirmwareFilePath = text;
					preprocess("Updating Firmware...");
					sendFirmwareFile.run();
					postprocess();
				}
			}
			catch (Exception ex2)
			{
				sError = sError + " " + ex2.Message;
				result = false;
			}
			errorCode = (GPEnum.ErrorCode)sendFirmwareFile.ErrorCode;
			if (errorCode != GPEnum.ErrorCode.NO_ERROR_OCCURRED)
			{
				sError = sError + " " + errorCode;
				return false;
			}
		}
		return result;
	}

	public bool CheckFirmwareVersion()
	{
		GetFirmwareFileVersion getFirmwareFileVersion = new GetFirmwareFileVersion(_printer);
		getFirmwareFileVersion.run();
		_ = getFirmwareFileVersion.FirmwareFileVersion;
		Version version = new Version("1.0.53.0");
		try
		{
			version = new Version(getFirmwareFileVersion.FirmwareFileVersion.Replace("v", ""));
		}
		catch (Exception)
		{
			version = new Version("1.0.53.0");
		}
		if (version.Major >= 1 && version.Build >= 53)
		{
			_bNeedsFirmware = false;
		}
		else
		{
			_bNeedsFirmware = true;
		}
		return _bNeedsFirmware;
	}

	public bool GetActivationState()
	{
		GetPrinterActivationState getPrinterActivationState = new GetPrinterActivationState(_printer);
		getPrinterActivationState.run();
		if (!getPrinterActivationState.Activated)
		{
			_bNeedsActivated = true;
		}
		else
		{
			_bNeedsActivated = false;
		}
		return _bNeedsActivated;
	}

	private bool InitializeOnlineProjetConnection(string IPAddress, out string ErrorMessage, out string strStatus)
	{
		ErrorMessage = string.Empty;
		strStatus = string.Empty;
		bool num = ConnectGemvisionLicense();
		string empty = string.Empty;
		if (!num)
		{
			ErrorMessage = "Cannot connect GV License";
			return false;
		}
		DataObjectArray dataObjectArray = FindPrinters(IPAddress, out ErrorMessage);
		if (dataObjectArray.Count == 0)
		{
			_printerOnline = false;
			ErrorMessage = "Cannot find printer";
			return false;
		}
		foreach (DataObject item in dataObjectArray)
		{
			Printer printer = item as Printer;
			if (!(printer != null))
			{
				continue;
			}
			_printer = printer;
			try
			{
				GPEnum.DeviceState state;
				GPEnum.ErrorCode deviceStatus = GetDeviceStatus(printer, _printer.IpAddress, out state, verbose: true, out ErrorMessage);
				if (state == GPEnum.DeviceState.DEVICE_STATE_IDLE || state == GPEnum.DeviceState.DEVICE_STATE_PRINTING)
				{
					strStatus = state.ToString();
					_ipAddress = IPAddress;
					if (deviceStatus != GPEnum.ErrorCode.NO_ERROR_OCCURRED)
					{
						ErrorMessage = deviceStatus.ToString();
					}
					else
					{
						ErrorMessage = empty;
					}
					if (state == GPEnum.DeviceState.DEVICE_STATE_IDLE)
					{
						_printerOnline = true;
					}
					_executePath = FileSystem.GetFolderPath(FileSystem.SpecialFolder.Executing);
					_supportMeshes = new DataObjectArray();
					_supportNativeData = new DataObjectArray();
					_raftMeshes = new DataObjectArray();
					GetPrinterActivationState getPrinterActivationState = new GetPrinterActivationState(_printer);
					getPrinterActivationState.run();
					if (!getPrinterActivationState.Activated)
					{
						_bNeedsActivated = true;
					}
					return true;
				}
				ErrorMessage = "Printer not ready to print!";
				return false;
			}
			catch (Exception)
			{
				ErrorMessage = "Printer not ready to print!";
				return false;
			}
		}
		_executePath = FileSystem.GetFolderPath(FileSystem.SpecialFolder.Executing);
		RhinoDoc.ActiveDoc.Views.Redraw();
		return true;
	}
}
