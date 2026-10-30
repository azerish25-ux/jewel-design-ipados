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

		public List<Rhino.Geometry.Mesh> BuildMeshes = new List<Rhino.Geometry.Mesh>();

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

		public List<Rhino.Geometry.Mesh> RaftSupportMeshes = new List<Rhino.Geometry.Mesh>();

		public List<Rhino.Geometry.Mesh> SupportMeshes = new List<Rhino.Geometry.Mesh>();

		public double SupportScale = 1.0;

		public Vector3d VecMeshMoveBack;

		public Vector3d VecPlatMoveBack;

		public Rhino.Geometry.Mesh BuildPlatformMesh { get; set; }

		public Point3d[] FramePoints { get; set; }

		public BoundingBox BuildMeshBoundingBox { get; set; }

		public BoundingBox SupportMeshBoundingBox { get; set; }

		public BoundingBox PlatformBoundingBox { get; set; }

		protected override void CalculateBoundingBox(CalculateBoundingBoxEventArgs e)
		{
			try
			{
				Rhino.Geometry.Mesh mesh = BuildPlatformMesh.DuplicateMesh();
				e.IncludeBoundingBox(mesh.GetBoundingBox(accurate: true));
				mesh.Translate(VecPlatMoveBack);
				e.IncludeBoundingBox(mesh.GetBoundingBox(accurate: true));
				if (BuildMeshes.Count > 0)
				{
					foreach (Rhino.Geometry.Mesh buildMesh in BuildMeshes)
					{
						mesh = buildMesh.DuplicateMesh();
						mesh.Translate(VecMeshMoveBack);
						e.IncludeBoundingBox(buildMesh.GetBoundingBox(accurate: true));
					}
				}
				base.CalculateBoundingBox(e);
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
			try
			{
				e.Display.EnableLighting(enable: true);
				e.Display.DepthMode = DepthMode.Neutral;
				Rhino.Geometry.Mesh mesh = BuildPlatformMesh.DuplicateMesh();
				mesh.Translate(VecPlatMoveBack);
				if (BuildMeshes.Count > 0)
				{
					e.Display.DrawMeshShaded(mesh, dmBuildPlatform);
					e.Display.DrawBoxCorners(mesh.GetBoundingBox(accurate: true), Color.Red);
				}
				if (bDrawGeo && BuildMeshes.Count > 0)
				{
					foreach (Rhino.Geometry.Mesh buildMesh in BuildMeshes)
					{
						mesh = buildMesh.DuplicateMesh();
						mesh.Translate(VecMeshMoveBack);
						if (ModelRotationX.bEnabled)
						{
							mesh.Rotate(ModelRotationX.RotAngle, ModelRotationX.RotAxis, ModelRotationX.RotPoint);
						}
						if (ModelRotationY.bEnabled)
						{
							mesh.Rotate(ModelRotationY.RotAngle, ModelRotationY.RotAxis, ModelRotationY.RotPoint);
						}
						if (ModelRotationZ.bEnabled)
						{
							mesh.Rotate(ModelRotationZ.RotAngle, ModelRotationZ.RotAxis, ModelRotationZ.RotPoint);
						}
						if (ModelTranslation != 0.0)
						{
							mesh.Translate(0.0, 0.0, ModelTranslation);
						}
						e.Display.DrawMeshShaded(mesh, dmBuildMeshes);
					}
				}
				if (bDrawSupports && BuildMeshes.Count > 0)
				{
					if (SupportMeshes.Count > 0)
					{
						foreach (Rhino.Geometry.Mesh supportMesh in SupportMeshes)
						{
							mesh = supportMesh.DuplicateMesh();
							mesh.Translate(VecMeshMoveBack);
							e.Display.DrawMeshShaded(mesh, dmSupports);
						}
					}
					if (RaftSupportMeshes.Count > 0)
					{
						foreach (Rhino.Geometry.Mesh raftSupportMesh in RaftSupportMeshes)
						{
							mesh = raftSupportMesh.DuplicateMesh();
							mesh.Translate(VecMeshMoveBack);
							e.Display.DrawMeshShaded(mesh, dmSupports);
						}
					}
				}
				mesh.Dispose();
			}
			catch (Exception)
			{
			}
			base.PostDrawObjects(e);
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

	private Rhino.Geometry.Mesh _csitem;

	private Rhino.Geometry.Mesh _tempMesh;

	private List<Rhino.Geometry.Mesh> _rhinoraftsupports;

	private List<Rhino.Geometry.Mesh> _rhinomainsupports;

	private List<Rhino.Geometry.Mesh> _rhinotempmainsupports;

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
			return pConduit.Enabled;
		}
		set
		{
			pConduit.Enabled = value;
			RhinoDoc.ActiveDoc.Views.Redraw();
		}
	}

	public bool IsPrinterOnline => _printerOnline;

	public ProjetHelper()
	{
		Directory.SetCurrentDirectory(GetPathOfAssembly());
		_printerOnline = false;
		_ipAddress = string.Empty;
		pConduit = new ProjetConduit();
		pConduit.Enabled = false;
		double num = 43.43;
		double num2 = 27.18;
		BoundaryRectangle = new RectangleF(0f, 0f, (float)num, (float)num2);
		pConduit.BuildPlatformMesh = Rhino.Geometry.Mesh.CreateFromPlane(Plane.WorldXY, new Interval(0.0, num), new Interval(0.0, num2), 1, 1);
		pConduit.PlatformBoundingBox = pConduit.BuildPlatformMesh.GetBoundingBox(accurate: true);
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
			Rhino.Geometry.Mesh mesh = _csitem.DuplicateMesh();
			Point3d center = mesh.GetBoundingBox(accurate: true).Center;
			if (pConduit.ModelRotationX.bEnabled)
			{
				mesh.Rotate(pConduit.ModelRotationX.RotAngle, pConduit.ModelRotationX.RotAxis, center);
			}
			if (pConduit.ModelRotationY.bEnabled)
			{
				mesh.Rotate(pConduit.ModelRotationY.RotAngle, pConduit.ModelRotationY.RotAxis, center);
			}
			if (pConduit.ModelRotationZ.bEnabled)
			{
				mesh.Rotate(pConduit.ModelRotationZ.RotAngle, pConduit.ModelRotationZ.RotAxis, center);
			}
			if (pConduit.ModelTranslation != 0.0)
			{
				mesh.Translate(0.0, 0.0, pConduit.ModelTranslation);
			}
			_inputMeshes = new DataObjectArray();
			BoundingBox boundingBox = mesh.GetBoundingBox(accurate: false);
			if (boundingBox.Min.Z < 1.5)
			{
				errorMessage = "Model must be higher above platform!";
				return false;
			}
			double num = ToDegrees(pConduit.ModelRotationZ.RotAngle);
			if (num < -45.0 || num > 45.0)
			{
				_bSupOverride = true;
			}
			if (boundingBox.Min.Z < 1.5 && boundingBox.Diagonal.Z < 10.0 && num != 0.0)
			{
				errorMessage = "When flat must be higher above platform!";
				return false;
			}
			if (boundingBox.Min.X < 0.0 || boundingBox.Min.Y < 0.0 || boundingBox.Max.X > (double)BoundaryRectangle.Right || boundingBox.Max.Y > (double)BoundaryRectangle.Bottom)
			{
				errorMessage = "Model outside platform area!";
				return false;
			}
			if (!AddRhinoMesh(mesh, out errorMessage))
			{
				return false;
			}
			GPEnum.ErrorCode errorCode = GenerateSupport(_inputMeshes, out errorMessage, out _supportMeshes, out _supportNativeData, out _raftMeshes);
			_rhinomainsupports = new List<Rhino.Geometry.Mesh>();
			_rhinoraftsupports = new List<Rhino.Geometry.Mesh>();
			_rhinotempmainsupports = new List<Rhino.Geometry.Mesh>();
			DataObjectArray dataObjectArray = new DataObjectArray();
			List<Rhino.Geometry.Mesh> list = new List<Rhino.Geometry.Mesh>();
			double z = 8.0;
			foreach (Guid tmpRhinoDocSupport in _tmpRhinoDocSupports)
			{
				RhinoDoc.ActiveDoc.Objects.Delete(tmpRhinoDocSupport, quiet: true);
			}
			_tmpRhinoDocSupports = new List<Guid>();
			foreach (DataObject raftMesh in _raftMeshes)
			{
				if (_supporttype != 3)
				{
					(raftMesh as Geomagic.Print.Headless.Mesh).transform(Transform3D.scale(1.0, 1.0, z));
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
				if (!(list[0].GetBoundingBox(accurate: false).Max.Z - list[0].GetBoundingBox(accurate: false).Min.Z < 3.0))
				{
					_rhinotempmainsupports.Add(list[0]);
					_rhinomainsupports.Add(list[0]);
				}
			}
			switch (errorCode)
			{
			case GPEnum.ErrorCode.NO_ERROR_OCCURRED:
				foreach (Rhino.Geometry.Mesh rhinomainsupport in _rhinomainsupports)
				{
					rhinomainsupport.Translate(_vecMeshMoveBack);
					Guid item = RhinoDoc.ActiveDoc.Objects.Add(rhinomainsupport, oaDrawnSupports);
					_tmpRhinoDocSupports.Add(item);
				}
				_rhinoraftsupports.ForEach(delegate(Rhino.Geometry.Mesh _)
				{
					_.Translate(_vecMeshMoveBack);
					Guid item2 = RhinoDoc.ActiveDoc.Objects.Add(_, oaDrawnSupports);
					_tmpRhinoDocSupports.Add(item2);
				});
				pConduit.Enabled = true;
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
		Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();
		List<Rhino.Geometry.Mesh> list = RhinoMeshFromGeoMesh(dataObjectArray);
		List<Rhino.Geometry.Mesh> list2 = new List<Rhino.Geometry.Mesh>();
		foreach (Rhino.Geometry.Mesh item2 in list)
		{
			Rhino.Geometry.Mesh mFit = item2.DuplicateMesh();
			BoundingBox boundingBox = mFit.GetBoundingBox(accurate: false);
			if (boundingBox.Max.X - boundingBox.Min.X > boundingBox.Max.Y - boundingBox.Min.Y)
			{
				mFit.Rotate(ToRadians(90.0), Plane.WorldXY.Normal, mFit.GetBoundingBox(accurate: false).Center);
			}
			if (!GetPointOutsideExistingMeshItems(list2, ref mFit))
			{
				mFit.Rotate(ToRadians(90.0), Plane.WorldXY.Normal, mFit.GetBoundingBox(accurate: false).Center);
				if (!GetPointOutsideExistingMeshItems(list2, ref mFit))
				{
					sStatus = "Multiple build too big to print";
					return false;
				}
			}
			mesh.Append(mFit);
			list2.Add(mFit.DuplicateMesh());
		}
		BoundingBox boundingBox2 = mesh.GetBoundingBox(accurate: false);
		if (boundingBox2.Min.X < 0.0 || boundingBox2.Min.Y < 0.0 || boundingBox2.Max.X > (double)BoundaryRectangle.Width || boundingBox2.Max.Y > (double)BoundaryRectangle.Height)
		{
			sStatus = "Multiple build too big to print";
			return false;
		}
		dataObjectArray = GeoMeshFromRhinoMesh(mesh, out sStatus);
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
			string imagePath = Path.Combine(Path.GetTempPath(), "CSSavedJob" + whatslot + ".bmp");
			RhinoDoc.ActiveDoc.Views.ActiveView.CreateShadedPreviewImage(imagePath, new Size(80, 60), ignoreHighlights: true, drawConstructionPlane: false, useGhostedShading: false);
			_lstSavedJobs[whatslot] = dJob[0];
			sStatus = "Job Saved";
		}
		return modelPrepped;
	}

	public bool SetCSItemToPrintPreview(Rhino.Geometry.Mesh m, out string sErr)
	{
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
			pConduit.Enabled = false;
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
			Rhino.Geometry.Mesh mesh = m.DuplicateMesh();
			BoundingBox boundingBox = mesh.GetBoundingBox(accurate: false);
			pCenter = boundingBox.Center;
			Point3d pointOnPlatform = GetPointOnPlatform(boundingBox);
			_vecMeshMove = Point3d.Subtract(pointOnPlatform, boundingBox.Min);
			_vecMeshMoveBack = Point3d.Subtract(boundingBox.Min, pointOnPlatform);
			pConduit.VecMeshMoveBack = _vecMeshMoveBack;
			pConduit.VecPlatMoveBack = _vecMeshMoveBack;
			mesh.Translate(_vecMeshMove);
			_csitem = mesh.DuplicateMesh();
			_inputMeshes = new DataObjectArray();
			_tmpRhinoDocSupports = new List<Guid>();
			pConduit.BuildMeshes.Clear();
			pConduit.BuildMeshes = new List<Rhino.Geometry.Mesh>();
			pConduit.BuildMeshes.Add(mesh);
			pConduit.SupportMeshes.Clear();
			double num = 43.43;
			double num2 = 27.18;
			BoundaryRectangle = new RectangleF(0f, 0f, (float)num, (float)num2);
			pConduit.BuildPlatformMesh = Rhino.Geometry.Mesh.CreateFromPlane(Plane.WorldXY, new Interval(0.0, num), new Interval(0.0, num2), 1, 1);
			pConduit.PlatformBoundingBox = pConduit.BuildPlatformMesh.GetBoundingBox(accurate: true);
			Rhino.Geometry.Mesh mesh2 = mesh.DuplicateMesh();
			Rhino.Geometry.Mesh mesh3 = new Rhino.Geometry.Mesh();
			GMesh item = MeshRepairHelper.OctTreeMeshFromMesh(mesh2);
			GMesh gMeshOut = new GMesh();
			global::OctTree.OctTree.GetMesh(new List<GMesh> { item }, 11, out gMeshOut);
			Rhino.Geometry.Mesh other = MeshRepairHelper.MeshFromOctTreeMeshes(gMeshOut);
			mesh3.Append(other);
			mesh2 = mesh3.DuplicateMesh();
			boundingBox = mesh2.GetBoundingBox(accurate: false);
			if (!AddRhinoMesh(mesh2, out sErr))
			{
				return false;
			}
			pConduit.BuildMeshBoundingBox = m.GetBoundingBox(accurate: true);
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
		pConduit.Enabled = false;
		pConduit.ModelTranslation = dHeight;
		foreach (Guid tmpRhinoDocSupport in _tmpRhinoDocSupports)
		{
			RhinoDoc.ActiveDoc.Objects.Delete(tmpRhinoDocSupport, quiet: true);
		}
		_tmpRhinoDocSupports = new List<Guid>();
		pConduit.Enabled = true;
		RhinoDoc.ActiveDoc.Views.Redraw();
		return true;
	}

	public bool UpdateMeshRotation(double dDegreex, double dDegreey, double dDegreez, out string errorMessage)
	{
		pConduit.SupportMeshes.Clear();
		pConduit.RaftSupportMeshes.Clear();
		pConduit.ModelRotationX.bEnabled = false;
		pConduit.ModelRotationX.bEnabled = false;
		pConduit.ModelRotationX.bEnabled = false;
		foreach (Guid tmpRhinoDocSupport in _tmpRhinoDocSupports)
		{
			RhinoDoc.ActiveDoc.Objects.Delete(tmpRhinoDocSupport, quiet: true);
		}
		_tmpRhinoDocSupports = new List<Guid>();
		if (dDegreex != 0.0)
		{
			pConduit.ModelRotationX.RotAngle = ToRadians(dDegreex);
			pConduit.ModelRotationX.RotAxis = Plane.WorldXY.ZAxis;
			pConduit.ModelRotationX.RotPoint = pCenter;
			pConduit.ModelRotationX.bEnabled = true;
		}
		if (dDegreey != 0.0)
		{
			pConduit.ModelRotationY.RotAngle = ToRadians(dDegreey);
			pConduit.ModelRotationY.RotAxis = Plane.WorldXY.YAxis;
			pConduit.ModelRotationY.RotPoint = pCenter;
			pConduit.ModelRotationY.bEnabled = true;
		}
		if (dDegreez != 0.0)
		{
			pConduit.ModelRotationZ.RotAngle = ToRadians(dDegreez);
			pConduit.ModelRotationZ.RotAxis = Plane.WorldXY.XAxis;
			pConduit.ModelRotationZ.RotPoint = pCenter;
			pConduit.ModelRotationZ.bEnabled = true;
		}
		errorMessage = "success";
		RhinoDoc.ActiveDoc.Views.Redraw();
		return true;
	}

	private BoundingBox GetBBSofItems(List<Rhino.Geometry.Mesh> lstgs)
	{
		BoundingBox result = default(BoundingBox);
		for (int i = 0; i < lstgs.Count; i++)
		{
			GeometryBase geometryBase = lstgs[i];
			result.Union(geometryBase.GetBoundingBox(accurate: true));
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

	private Rhino.Geometry.Mesh GetDeformedCSModel(Rhino.Geometry.Mesh csMesh)
	{
		Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();
		new Rhino.Geometry.Mesh();
		GMesh item = MeshRepairHelper.OctTreeMeshFromMesh(_tempMesh);
		GMesh gMeshOut = new GMesh();
		global::OctTree.OctTree.GetMesh(new List<GMesh> { item }, 11, out gMeshOut);
		Rhino.Geometry.Mesh other = MeshRepairHelper.MeshFromOctTreeMeshes(gMeshOut);
		mesh.Append(other);
		return mesh;
	}

	private bool GetModelPrepped(out string sStatus, out DataObjectArray dJob, List<Rhino.Geometry.Mesh> RhinoMainSupports, List<Rhino.Geometry.Mesh> RhinoRaftSupports, Rhino.Geometry.Mesh MainModel)
	{
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
		Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();
		switch (supporttype)
		{
		case 1:
			foreach (Rhino.Geometry.Mesh RhinoRaftSupport in RhinoRaftSupports)
			{
				mesh.Append(RhinoRaftSupport);
			}
			mesh.Append(IncreaseReplaceRhinoMainSupports(1.5, bReplace: false));
			break;
		case 2:
			foreach (Rhino.Geometry.Mesh RhinoRaftSupport2 in RhinoRaftSupports)
			{
				mesh.Append(RhinoRaftSupport2);
			}
			mesh.Append(IncreaseReplaceRhinoMainSupports(1.5, bReplace: true));
			break;
		default:
		{
			Transform transform = Transform.Translation(_vecMeshMoveBack);
			Transform inverseTransform = Transform.Identity;
			transform.TryGetInverse(out inverseTransform);
			foreach (Guid tmpRhinoDocSupport in _tmpRhinoDocSupports)
			{
				RhinoObject rhinoObject = RhinoDoc.ActiveDoc.Objects.Find(tmpRhinoDocSupport);
				if (rhinoObject != null)
				{
					Rhino.Geometry.Mesh mesh2 = rhinoObject.Geometry as Rhino.Geometry.Mesh;
					mesh2.Transform(inverseTransform);
					mesh.Append(mesh2);
				}
			}
			break;
		}
		case 3:
			break;
		}
		mesh.Append(GetDeformedCSModel(MainModel));
		dJob = GeoMeshFromRhinoMesh(mesh, out sStatus);
		return true;
	}

	private bool GetPointOutsideExistingMeshItems(List<Rhino.Geometry.Mesh> lstMeshtocheck, ref Rhino.Geometry.Mesh mFit)
	{
		Point3d point = new Point3d(0.0, 0.0, 0.0);
		BoundingBox boundingBox = mFit.GetBoundingBox(accurate: false);
		BoundingBox bBSofItems = GetBBSofItems(lstMeshtocheck);
		int num = Convert.ToInt32((double)BoundaryRectangle.Width - (boundingBox.Max.X - boundingBox.Min.X));
		int num2 = Convert.ToInt32((double)BoundaryRectangle.Height - (boundingBox.Max.Y - boundingBox.Min.Y));
		if (lstMeshtocheck.Count == 0)
		{
			Vector3d translationVector = Point3d.Subtract(point, boundingBox.Min);
			mFit.Translate(translationVector);
			return true;
		}
		if (num < 0 || num2 < 0)
		{
			return true;
		}
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j <= num2; j++)
			{
				point = new Point3d(i, j, 0.0);
				if (!bBSofItems.Contains(point))
				{
					BoundingBox a = new BoundingBox(boundingBox.GetCorners());
					Vector3d vector3d = Point3d.Subtract(point, boundingBox.Min);
					a.Transform(Transform.Translation(vector3d));
					if (!BoundingBox.Intersection(a, bBSofItems).IsValid)
					{
						mFit.Translate(vector3d);
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
			RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Select((RhinoObject _) => _.Id), quiet: true);
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
				(Meshes[i] as Geomagic.Print.Headless.Mesh).transform(autoPlace.getTransformAt(i));
			}
		}
		catch (Exception)
		{
		}
	}

	private DataObjectArray GeoMeshFromRhinoMesh(Rhino.Geometry.Mesh mesh, out string sErr)
	{
		sErr = string.Empty;
		DataObjectArray dataObjectArray = new DataObjectArray();
		try
		{
			CreateMesh createMesh = new CreateMesh();
			Geomagic.Print.Headless.Mesh mesh2 = null;
			int count = mesh.Faces.Count;
			for (int i = 0; i < count; i++)
			{
				MeshFace meshFace = mesh.Faces[i];
				Point3f point3f = mesh.Vertices[meshFace.A];
				Point3f point3f2 = mesh.Vertices[meshFace.B];
				Point3f point3f3 = mesh.Vertices[meshFace.C];
				int a = createMesh.addPoint(new Vector3D(point3f.X, point3f.Y, point3f.Z));
				int b = createMesh.addPoint(new Vector3D(point3f2.X, point3f2.Y, point3f2.Z));
				int c = createMesh.addPoint(new Vector3D(point3f3.X, point3f3.Y, point3f3.Z));
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

	private bool AddRhinoMesh(Rhino.Geometry.Mesh mesh, out string sErr)
	{
		sErr = string.Empty;
		try
		{
			_tempMesh = mesh.DuplicateMesh();
			CreateMesh createMesh = new CreateMesh();
			Geomagic.Print.Headless.Mesh mesh2 = null;
			int count = mesh.Faces.Count;
			for (int i = 0; i < count; i++)
			{
				MeshFace meshFace = mesh.Faces[i];
				Point3f point3f = mesh.Vertices[meshFace.A];
				Point3f point3f2 = mesh.Vertices[meshFace.B];
				Point3f point3f3 = mesh.Vertices[meshFace.C];
				int a = createMesh.addPoint(new Vector3D(point3f.X, point3f.Y, point3f.Z));
				int b = createMesh.addPoint(new Vector3D(point3f2.X, point3f2.Y, point3f2.Z));
				int c = createMesh.addPoint(new Vector3D(point3f3.X, point3f3.Y, point3f3.Z));
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
			pConduit.Enabled = true;
			RhinoDoc.ActiveDoc.Views.Redraw();
			return;
		}
		int num = 0;
		int num2 = Math.Abs(iSupportDensity) + 1;
		num2++;
		_rhinomainsupports = new List<Rhino.Geometry.Mesh>();
		foreach (Rhino.Geometry.Mesh rhinotempmainsupport in _rhinotempmainsupports)
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
		pConduit.Enabled = true;
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
		double num = bb.Max.X - bb.Min.X;
		double num2 = bb.Max.Y - bb.Min.Y;
		return new Point3d(((double)BoundaryRectangle.Width - num) / 2.0, ((double)BoundaryRectangle.Height - num2) / 2.0, 3.0);
	}

	public void ResetProjet()
	{
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
				RhinoDoc.ActiveDoc.Objects.Delete(tmpRhinoDocSupport, quiet: true);
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
		pConduit.Enabled = false;
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
			_tempMesh.Dispose();
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
		if (scale == 1.0)
		{
			return;
		}
		Plane worldXY = Plane.WorldXY;
		Transform identity = Transform.Identity;
		new DataObjectArray();
		_ = string.Empty;
		foreach (Rhino.Geometry.Mesh rhinoraftsupport in _rhinoraftsupports)
		{
			identity = Transform.Scale(worldXY, 1.0, 1.0, scale);
			rhinoraftsupport.Transform(identity);
		}
	}

	private Rhino.Geometry.Mesh ReplaceRaftMeshes()
	{
		Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();
		Rhino.Geometry.Mesh mesh2 = new Rhino.Geometry.Mesh();
		foreach (Rhino.Geometry.Mesh rhinoraftsupport in _rhinoraftsupports)
		{
			mesh.Append(rhinoraftsupport);
		}
		return Rhino.Geometry.Mesh.CreateFromBox(mesh.GetBoundingBox(accurate: true).GetCorners(), 20, 20, 20);
	}

	private Rhino.Geometry.Mesh IncreaseReplaceRhinoMainSupports(double scale, bool bReplace)
	{
		Plane worldXY = Plane.WorldXY;
		Transform identity = Transform.Identity;
		_ = string.Empty;
		Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();
		foreach (Rhino.Geometry.Mesh rhinomainsupport in _rhinomainsupports)
		{
			Rhino.Geometry.Mesh mesh2 = rhinomainsupport.DuplicateMesh();
			worldXY.Origin = rhinomainsupport.GetBoundingBox(accurate: true).Center;
			if (scale != 1.0)
			{
				identity = Transform.Scale(worldXY, scale, scale, 1.0);
				mesh2.Transform(identity);
			}
			if (!bReplace)
			{
				mesh.Append(mesh2);
				continue;
			}
			BoundingBox boundingBox = mesh2.GetBoundingBox(accurate: true);
			Point3d[] corners = boundingBox.GetCorners();
			_ = boundingBox.Center;
			double num = corners[0].DistanceTo(corners[2]);
			num /= 2.0;
			Rhino.Geometry.Mesh mesh3 = Rhino.Geometry.Mesh.CreateFromBox(boundingBox.GetCorners(), 2, 2, 2);
			Plane worldXY2 = Plane.WorldXY;
			worldXY2.OriginZ = boundingBox.Max.Z + 0.1;
			Polyline[] outlines = mesh3.GetOutlines(worldXY2);
			Curve curve = new PolyCurve();
			Polyline[] array = outlines;
			for (int i = 0; i < array.Length; i++)
			{
				curve = array[i].ToNurbsCurve();
				curve.Transform(Transform.Scale(curve.GetBoundingBox(accurate: false).Center, 0.5));
			}
			worldXY2 = Plane.WorldXY;
			worldXY2.OriginZ = boundingBox.Min.Z;
			Polyline[] outlines2 = mesh3.GetOutlines(worldXY2);
			Curve item = new PolyCurve();
			array = outlines2;
			for (int i = 0; i < array.Length; i++)
			{
				item = array[i].ToNurbsCurve();
			}
			Plane plane = new Plane(new Point3d(boundingBox.Min.X, boundingBox.Min.Y, boundingBox.Center.Z), Plane.WorldXY.Normal);
			Rectangle3d rectangle3d = new Rectangle3d(plane, num * 1.5, num * 1.5);
			Point3d start = new Point3d(boundingBox.Center.X, boundingBox.Center.Y, boundingBox.Min.Z);
			Point3d end = new Point3d(boundingBox.Center.X, boundingBox.Center.Y, boundingBox.Max.Z);
			Brep[] array2 = Brep.CreateFromLoft(new List<Curve>
			{
				item,
				rectangle3d.ToNurbsCurve(),
				curve
			}, start, end, LoftType.Straight, closed: true);
			for (int i = 0; i < array2.Length; i++)
			{
				Rhino.Geometry.Mesh[] array3 = Rhino.Geometry.Mesh.CreateFromBrep(array2[i], MeshingParameters.Coarse);
				foreach (Rhino.Geometry.Mesh mesh4 in array3)
				{
					mesh4.Faces.ConvertQuadsToTriangles();
					mesh4.Normals.ComputeNormals();
					mesh.Append(mesh4);
				}
			}
		}
		return mesh;
	}

	private DataObjectArray IncreaseRaftSupports(DataObjectArray raftSupports, double scale)
	{
		if (scale == 1.0)
		{
			return raftSupports;
		}
		List<Rhino.Geometry.Mesh> list = RhinoMeshFromGeoMesh(raftSupports);
		Plane worldXY = Plane.WorldXY;
		Transform identity = Transform.Identity;
		DataObjectArray dataObjectArray = new DataObjectArray();
		string sErr = string.Empty;
		foreach (Rhino.Geometry.Mesh item in list)
		{
			worldXY.Origin = item.GetBoundingBox(accurate: true).Center;
			identity = Transform.Scale(worldXY, 1.0, 1.0, scale);
			item.Transform(identity);
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
		if (scale == 1.0)
		{
			return regSupports;
		}
		List<Rhino.Geometry.Mesh> list = RhinoMeshFromGeoMesh(regSupports);
		Plane worldXY = Plane.WorldXY;
		Transform identity = Transform.Identity;
		DataObjectArray dataObjectArray = new DataObjectArray();
		string sErr = string.Empty;
		foreach (Rhino.Geometry.Mesh item in list)
		{
			worldXY.Origin = item.GetBoundingBox(accurate: true).Center;
			identity = Transform.Scale(worldXY, scale, scale, 1.0);
			item.Transform(identity);
			DataObjectArray dataObjectArray2 = GeoMeshFromRhinoMesh(item, out sErr);
			if (!(dataObjectArray2 == null) && dataObjectArray2.Count != 0)
			{
				dataObjectArray.Add(dataObjectArray2[0]);
			}
		}
		return dataObjectArray;
	}

	private List<Rhino.Geometry.Mesh> IncreaseRhinoRaftSupports(DataObjectArray raftSupports)
	{
		List<Rhino.Geometry.Mesh> list = RhinoMeshFromGeoMesh(raftSupports);
		if (_userscale == 1.0)
		{
			return list;
		}
		List<Rhino.Geometry.Mesh> list2 = new List<Rhino.Geometry.Mesh>();
		Plane worldXY = Plane.WorldXY;
		Transform identity = Transform.Identity;
		_ = string.Empty;
		foreach (Rhino.Geometry.Mesh item in list)
		{
			worldXY.Origin = item.GetBoundingBox(accurate: true).Center;
			identity = Transform.Scale(worldXY, 1.0, 1.0, _userscale);
			item.Transform(identity);
			list2.Add(item);
		}
		return list2;
	}

	private List<Rhino.Geometry.Mesh> IncreaseRhinoMainSupports(DataObjectArray regSupports)
	{
		List<Rhino.Geometry.Mesh> list = RhinoMeshFromGeoMesh(regSupports);
		if (_userscale == 1.0)
		{
			return list;
		}
		Plane worldXY = Plane.WorldXY;
		Transform identity = Transform.Identity;
		List<Rhino.Geometry.Mesh> list2 = new List<Rhino.Geometry.Mesh>();
		_ = string.Empty;
		foreach (Rhino.Geometry.Mesh item in list)
		{
			worldXY.Origin = item.GetBoundingBox(accurate: true).Center;
			identity = Transform.Scale(worldXY, _userscale, _userscale, 1.0);
			item.Transform(identity);
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

	private List<Rhino.Geometry.Mesh> RhinoMeshFromGeoMesh(DataObjectArray meshes)
	{
		try
		{
			if (meshes == null || !meshes.Any())
			{
				return new List<Rhino.Geometry.Mesh>();
			}
			List<Rhino.Geometry.Mesh> list = new List<Rhino.Geometry.Mesh>();
			int count = meshes.Count;
			for (int i = 0; i < count; i++)
			{
				if (!(meshes[i] is Geomagic.Print.Headless.Mesh))
				{
					continue;
				}
				Geomagic.Print.Headless.Mesh mesh = (Geomagic.Print.Headless.Mesh)meshes[i];
				if (mesh != null)
				{
					int maxTriangle = mesh.MaxTriangle;
					_ = mesh.NumPoints;
					Rhino.Geometry.Mesh mesh2 = new Rhino.Geometry.Mesh();
					for (int j = 0; j < maxTriangle; j++)
					{
						int vertexIndex = mesh.getVertexIndex(j, 0, 0);
						int vertexIndex2 = mesh.getVertexIndex(j, 0, 1);
						int vertexIndex3 = mesh.getVertexIndex(j, 0, 2);
						Vector3D coordinate = mesh.getCoordinate(vertexIndex);
						Vector3D coordinate2 = mesh.getCoordinate(vertexIndex2);
						Vector3D coordinate3 = mesh.getCoordinate(vertexIndex3);
						mesh2.Vertices.SetVertex(vertexIndex, coordinate.X, coordinate.Y, coordinate.Z);
						mesh2.Vertices.SetVertex(vertexIndex2, coordinate2.X, coordinate2.Y, coordinate2.Z);
						mesh2.Vertices.SetVertex(vertexIndex3, coordinate3.X, coordinate3.Y, coordinate3.Z);
						mesh2.Faces.SetFace(j, new MeshFace(vertexIndex, vertexIndex2, vertexIndex3));
						Vector3D triangleNormal = mesh.getTriangleNormal(j);
						mesh2.FaceNormals.AddFaceNormal(triangleNormal.X, triangleNormal.Y, triangleNormal.Z);
					}
					mesh2.Normals.ComputeNormals();
					mesh2.Compact();
					mesh2.Unweld(0.0, modifyNormals: true);
					mesh2.Scale(1000.0);
					list.Add(mesh2);
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
