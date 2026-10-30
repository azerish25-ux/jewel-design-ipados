using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using CSCompatibility;
using CSGemInstance.GemTableData;
using CSMaterials;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSGemInstance;

public class CSGemInstanceDefinition : IDisposable
{
	public struct ProngPosition(int prongLayer, double prongParam)
	{
		public int ProngLayer { get; } = prongLayer;

		public double ProngParam { get; } = prongParam;
	}

	public delegate void LogPrintLine(string line);

	protected double _bezelParameter;

	protected NurbsCurve _curve;

	protected double _depthPercentage;

	protected Mesh _mesh;

	protected double _fudgefactor;

	protected int _gemTableVersion;

	protected bool _isGeneratedGem;

	protected string _layer = "Unknown";

	protected double _maxX;

	protected double _maxY;

	protected double _maxZ;

	protected double _minX;

	protected double _minY;

	protected double _minZ;

	protected string _shape;

	protected string _type;

	protected int _versionMajor;

	protected int _versionMinor;

	protected GemTableType _gemTableType;

	protected GemTableShape _gemTableShape;

	protected GemTableLayer _gemTableLayer;

	public double BezelParam => _bezelParameter;

	public virtual string BlockName => _shape;

	public double FudgeFactor => _fudgefactor;

	public int GemTableVersion => _gemTableVersion;

	public bool HasNurbsCurve => _curve != null;

	public bool IsGeneratedGem => _isGeneratedGem;

	public bool IsValid
	{
		get
		{
			if (_curve == null)
			{
				return false;
			}
			if (!((CommonObject)_curve).IsValid)
			{
				return false;
			}
			if (_mesh == null)
			{
				return false;
			}
			if (!((CommonObject)_mesh).IsValid)
			{
				return false;
			}
			return true;
		}
	}

	public string Layer => _layer;

	public double MaxX => _maxX;

	public double MaxY => _maxY;

	public double MaxZ => _maxZ;

	public Mesh Mesh
	{
		get
		{
			Mesh mesh = _mesh;
			if (mesh == null)
			{
				return null;
			}
			return mesh.DuplicateMesh();
		}
	}

	public double MinX => _minX;

	public double MinY => _minY;

	public double MinZ => _minZ;

	public NurbsCurve NurbsCurve
	{
		get
		{
			NurbsCurve curve = _curve;
			if (curve == null)
			{
				return null;
			}
			return ((Curve)curve).ToNurbsCurve();
		}
	}

	public List<ProngPosition> ProngPositions { get; } = new List<ProngPosition>();

	public string Shape => _shape;

	public GemTableShape GemTableShape => _gemTableShape;

	public List<GemSize> Sizes { get; } = new List<GemSize>();

	public string Type => _type;

	public GemTableType GemTableType => _gemTableType;

	public int VersionMajor => _versionMajor;

	public int VersionMinor => _versionMinor;

	public static string GemDir { get; set; }

	public CSGemInstanceDefinition()
	{
	}

	public CSGemInstanceDefinition(GemTableType gemTableType, GemTableShape gemTableShape)
		: this()
	{
		_gemTableType = gemTableType;
		_gemTableShape = gemTableShape;
		_gemTableLayer = GemTableHelper.GetLayer(_gemTableType, _gemTableShape);
		_gemTableVersion = GemTableHelper.GetCurrentGemTableVersion(gemTableType, gemTableShape);
		_type = GemTableHelper.GetDescriptionFromValue(_gemTableType);
		_shape = GemTableHelper.GetDescriptionFromValue(_gemTableShape);
		_layer = GemTableHelper.GetDescriptionFromValue(_gemTableLayer);
		_versionMajor = CSGemInstanceDefinitionHelper.CurrentVersionMajor;
		_versionMinor = CSGemInstanceDefinitionHelper.CurrentVersionMinor;
		if (_gemTableVersion > 0)
		{
			Sizes = GemTableHelper.GetGemSizes(_type, _shape, _gemTableVersion);
		}
		_isGeneratedGem = true;
	}

	public bool CreateGem(string filePath)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Expected O, but got Unknown
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		if (!File.Exists(filePath))
		{
			return false;
		}
		_versionMajor = CSGemInstanceDefinitionHelper.CurrentVersionMajor;
		_versionMinor = CSGemInstanceDefinitionHelper.CurrentVersionMinor;
		_type = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(filePath));
		_shape = Path.GetFileNameWithoutExtension(filePath);
		_gemTableVersion = GemTableHelper.GetCurrentGemTableVersion(_type, _shape);
		if (_gemTableVersion <= 0)
		{
			return false;
		}
		if (!CSUtilGem.Open3dm(filePath))
		{
			return false;
		}
		ObjectEnumeratorSettings val = new ObjectEnumeratorSettings
		{
			DeletedObjects = false,
			ActiveObjects = true,
			ReferenceObjects = true,
			IncludeGrips = false,
			IncludeLights = false,
			ObjectTypeFilter = (ObjectType)16
		};
		foreach (RhinoObject @object in RhinoDoc.ActiveDoc.Objects.GetObjectList(val))
		{
			_layer = RhinoDoc.ActiveDoc.Layers[@object.Attributes.LayerIndex].Name;
			if (@object.Geometry != null)
			{
				@object.CreateMeshes((MeshType)3, MeshingParameters.Smooth, true);
				Mesh[] meshes = @object.GetMeshes((MeshType)3);
				_mesh = new Mesh();
				Mesh[] array = meshes;
				foreach (Mesh val2 in array)
				{
					_mesh.Append(val2);
				}
				BoundingBox boundingBox = ((GeometryBase)_mesh).GetBoundingBox(true);
				Point3d val3 = ((BoundingBox)(ref boundingBox)).Max;
				_maxX = ((Point3d)(ref val3)).X;
				val3 = ((BoundingBox)(ref boundingBox)).Max;
				_maxY = ((Point3d)(ref val3)).Y;
				val3 = ((BoundingBox)(ref boundingBox)).Max;
				_maxZ = ((Point3d)(ref val3)).Z;
				val3 = ((BoundingBox)(ref boundingBox)).Min;
				_minX = ((Point3d)(ref val3)).X;
				val3 = ((BoundingBox)(ref boundingBox)).Min;
				_minY = ((Point3d)(ref val3)).Y;
				val3 = ((BoundingBox)(ref boundingBox)).Min;
				_minZ = ((Point3d)(ref val3)).Z;
			}
		}
		if (_mesh == null)
		{
			return false;
		}
		ObjectEnumeratorSettings val4 = new ObjectEnumeratorSettings
		{
			DeletedObjects = false,
			ActiveObjects = true,
			ReferenceObjects = true,
			IncludeGrips = false,
			IncludeLights = false,
			ObjectTypeFilter = (ObjectType)4
		};
		foreach (RhinoObject object2 in RhinoDoc.ActiveDoc.Objects.GetObjectList(val4))
		{
			if (object2.Geometry != null)
			{
				GeometryBase geometry = object2.Geometry;
				Curve val5 = (Curve)(object)((geometry is Curve) ? geometry : null);
				if (val5 != null && val5.HasNurbsForm() > 0)
				{
					_curve = val5.ToNurbsCurve();
				}
			}
		}
		if (_curve == null)
		{
			return false;
		}
		ObjectEnumeratorSettings val6 = new ObjectEnumeratorSettings
		{
			DeletedObjects = false,
			ActiveObjects = true,
			ReferenceObjects = true,
			IncludeGrips = false,
			IncludeLights = false,
			ObjectTypeFilter = (ObjectType)1
		};
		ProngPosition[] array2 = new ProngPosition[RhinoDoc.ActiveDoc.Objects.GetObjectList(val6).Count() - 1];
		double prongParam = default(double);
		foreach (RhinoObject object3 in RhinoDoc.ActiveDoc.Objects.GetObjectList(val6))
		{
			Match match = new Regex("^P(\\d*$)").Match(object3.Attributes.Name);
			if (!match.Success || match.Groups.Count < 2)
			{
				continue;
			}
			int num = Convert.ToInt32(match.Groups[match.Groups.Count - 1].Value);
			PointObject val7 = (PointObject)(object)((object3 is PointObject) ? object3 : null);
			if (val7 != null)
			{
				Point3d location = val7.PointGeometry.Location;
				if (((Point3d)(ref location)).IsValid && ((Curve)_curve).ClosestPoint(location, ref prongParam))
				{
					array2[num] = new ProngPosition(((RhinoObject)val7).Attributes.LayerIndex, prongParam);
				}
			}
		}
		ProngPositions.Clear();
		ProngPositions.AddRange(new List<ProngPosition>(array2));
		double bezelParameter = default(double);
		foreach (RhinoObject object4 in RhinoDoc.ActiveDoc.Objects.GetObjectList(val6))
		{
			if (object4.Attributes.Name != "BP")
			{
				continue;
			}
			PointObject val8 = (PointObject)(object)((object4 is PointObject) ? object4 : null);
			if (val8 != null)
			{
				Point3d location2 = val8.PointGeometry.Location;
				if (((Point3d)(ref location2)).IsValid && ((Curve)_curve).ClosestPoint(location2, ref bezelParameter))
				{
					_bezelParameter = bezelParameter;
				}
			}
		}
		Sizes.Clear();
		Sizes.AddRange(GemTableHelper.GetGemSizes(_type, _shape));
		_fudgefactor = GemTableHelper.GetFudgeFactor(_type, _shape);
		return true;
	}

	public void DrawMesh(DisplayPipeline dp, Transform form, DisplayMaterial mat)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (((CommonObject)_mesh).IsValid)
		{
			if (((Transform)(ref form)).IsValid)
			{
				dp.PushModelTransform(dp.ModelTransform * form);
				dp.DrawMeshShaded(_mesh, mat);
				dp.PopModelTransform();
			}
			else
			{
				dp.DrawMeshShaded(_mesh, mat);
			}
		}
	}

	public CSGemInstanceDefinition GenerateCSGemData(int gemTableSizeIndex)
	{
		if (!IsGeneratedGem)
		{
			return null;
		}
		GemTableType valueFromDescription = GemTableHelper.GetValueFromDescription(_type, GemTableType.Unknown);
		GemTableShape valueFromDescription2 = GemTableHelper.GetValueFromDescription(_shape, GemTableShape.Unknown);
		if (valueFromDescription == GemTableType.Unknown || valueFromDescription2 == GemTableShape.Unknown)
		{
			return null;
		}
		return CSUtilCustomGems.CreateCustomGemFromShapeAndType(valueFromDescription, valueFromDescription2, gemTableSizeIndex, GemTableVersion);
	}

	public bool GetBoundingBox(ref BoundingBox boundingBox)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if (_mesh == null)
		{
			return false;
		}
		BoundingBox boundingBox2 = ((GeometryBase)_mesh).GetBoundingBox(false);
		if (!((BoundingBox)(ref boundingBox2)).MakeValid())
		{
			return false;
		}
		boundingBox = boundingBox2;
		return true;
	}

	public bool IsGem(string checkType, string checkShape, int checkGemTableVersion = 0)
	{
		if (checkGemTableVersion > 0)
		{
			if (checkType == _type && checkShape == _shape)
			{
				return checkGemTableVersion == _gemTableVersion;
			}
			return false;
		}
		if (checkType == _type)
		{
			return checkShape == _shape;
		}
		return false;
	}

	public bool IsGem(string checkType, string checkShape, int checkVersionMajor, int checkVersionMinor, int checkGemTableVersion = 0)
	{
		if (checkGemTableVersion > 0)
		{
			if (checkType == _type && checkShape == _shape && checkVersionMajor == _versionMajor && checkVersionMinor == _versionMinor)
			{
				return checkGemTableVersion == _gemTableVersion;
			}
			return false;
		}
		if (checkType == _type && checkShape == _shape && checkVersionMajor == _versionMajor)
		{
			return checkVersionMinor == _versionMinor;
		}
		return false;
	}

	public bool LoadFromLibrary(string loadType, string loadShape, LogPrintLine csPrintLine, bool isReloading)
	{
		string gemPath = Path.Combine(GemDir, "Gems", loadType, $"{loadShape}.gem");
		return LoadFromPath(gemPath, csPrintLine, isReloading);
	}

	public bool LoadFromPath(string gemPath, LogPrintLine csPrintLine, bool isReloading)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		if (!File.Exists(gemPath))
		{
			return false;
		}
		OnFileHandle val = OnUtil.OpenFile(gemPath, "rb");
		if (val == null)
		{
			return false;
		}
		OnBinaryArchive archive = (OnBinaryArchive)new OnBinaryFile((archive_mode)5, val);
		bool result = Read(ref archive, csPrintLine, isReloading);
		OnUtil.CloseFile(val);
		return result;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("MatrixRhinoGemInstance:");
		stringBuilder.AppendLine("Type:" + _type + "  Shape:" + _shape + "  Layer:" + _layer);
		stringBuilder.AppendLine("Min: (" + _minX + "," + _minY + "," + _minZ + ")");
		stringBuilder.AppendLine("Max: (" + _maxX + "," + _maxY + "," + _maxZ + ")");
		stringBuilder.AppendLine("Bezel Parameter:" + _bezelParameter + " FFactor:" + _fudgefactor);
		return stringBuilder.ToString();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (_mesh != null)
		{
			((CommonObject)_mesh).Dispose();
		}
		if (_curve != null)
		{
			((CommonObject)_curve).Dispose();
		}
		_mesh = null;
		_curve = null;
	}

	public bool ReadJson(JObject jObj, LogPrintLine csPrintLine, bool isReloading)
	{
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Expected O, but got Unknown
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Expected O, but got Unknown
		_versionMajor = Convert.ToInt32(jObj["VersionMajor"]);
		_versionMinor = Convert.ToInt32(jObj["VersionMinor"]);
		_gemTableVersion = 0;
		if ((_versionMajor != 1 || _versionMinor != 0) && (_versionMajor != 1 || _versionMinor != 1) && (_versionMajor != 1 || _versionMinor != 2) && (_versionMajor != 1 || _versionMinor != 3) && (_versionMajor != 1 || _versionMinor != 4) && (_versionMajor != 1 || _versionMinor != 5) && (_versionMajor != 2 || _versionMinor != 0) && (_versionMajor != 2 || _versionMinor != 1) && (_versionMajor != 3 || _versionMinor != 0) && (_versionMajor != 3 || _versionMinor != 1) && (_versionMajor != 3 || _versionMinor != 2) && (_versionMajor != 3 || _versionMinor != 3) && (_versionMajor != 3 || _versionMinor != 4))
		{
			return false;
		}
		_type = jObj["Type"].ToString();
		_shape = jObj["Shape"].ToString();
		_layer = jObj["Layer"]?.ToString() ?? GemTableHelper.GetDescriptionFromValue(GemTableHelper.GetLayer(_type, _shape));
		_minX = Convert.ToDouble(jObj["minX"]);
		_minY = Convert.ToDouble(jObj["minY"]);
		_minZ = Convert.ToDouble(jObj["minZ"]);
		_maxX = Convert.ToDouble(jObj["maxX"]);
		_maxY = Convert.ToDouble(jObj["maxY"]);
		_maxZ = Convert.ToDouble(jObj["maxZ"]);
		_bezelParameter = Convert.ToDouble(jObj["bezelParameter"]);
		_fudgefactor = Convert.ToDouble(jObj["fudgeFactor"]);
		_depthPercentage = Convert.ToDouble(jObj["depthPerentage"]);
		_curve = (NurbsCurve)GeometryBaseFromString(jObj["curve"].ToString());
		JArray jArray = (JArray)jObj["sizes"];
		Sizes.Clear();
		if (jArray != null)
		{
			foreach (JObject item in jArray)
			{
				double width = Convert.ToDouble(item["sizeX"]);
				double num = Convert.ToDouble(item["sizeY"]);
				if (Math.Abs(_depthPercentage) < CSUtil.Tolerance)
				{
					double depth = num * ((_maxZ - _minZ) / (_maxY - _minY));
					Sizes.Add(new GemSize(num, width, depth));
				}
				else
				{
					Sizes.Add(new GemSizePercent(num, width, _depthPercentage));
				}
			}
		}
		else
		{
			JToken jToken = jObj["gemTableVersion"];
			if (jToken != null)
			{
				_gemTableVersion = Convert.ToInt32(jToken);
				if (_gemTableVersion == 0)
				{
					_gemTableVersion = GemTableHelper.GetCurrentGemTableVersion(_type, _shape);
				}
			}
			else if (isReloading)
			{
				_gemTableVersion = GemTableHelper.GetCurrentGemTableVersion(_type, _shape);
			}
			else
			{
				_gemTableVersion = ((!(_shape == "Oval")) ? 1 : 3);
			}
			Sizes.Clear();
			Sizes.AddRange(GemTableHelper.GetGemSizes(_type, _shape, _gemTableVersion));
			_fudgefactor = GemTableHelper.GetFudgeFactor(_type, _shape);
		}
		ProngPositions.Clear();
		JArray jArray2 = (JArray)jObj["prongPos"];
		if (jArray2 != null)
		{
			foreach (JObject item2 in jArray2)
			{
				double num2 = Convert.ToDouble(item2["prongParam"]);
				int num3 = Convert.ToInt32(item2["prongLayer"]);
				if (num2 != 0.0 || num3 != -1)
				{
					ProngPositions.Add(new ProngPosition(num3, num2));
				}
			}
		}
		_isGeneratedGem = Convert.ToBoolean(jObj["isGenerated"]);
		_mesh = (Mesh)GeometryBaseFromString(jObj["gemMesh"].ToString());
		return true;
	}

	public bool Read(ref OnBinaryArchive archive, LogPrintLine csPrintLine, bool isReloading)
	{
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Expected O, but got Unknown
		uint num = TypeCodes.TC_USER | TypeCodes.TC_CRC;
		_versionMajor = 0;
		_versionMinor = 0;
		_gemTableVersion = 0;
		archive.BeginRead3dmChunk(num, ref _versionMajor, ref _versionMinor);
		if ((_versionMajor != 1 || _versionMinor != 0) && (_versionMajor != 1 || _versionMinor != 1) && (_versionMajor != 1 || _versionMinor != 2) && (_versionMajor != 1 || _versionMinor != 3) && (_versionMajor != 1 || _versionMinor != 4) && (_versionMajor != 1 || _versionMinor != 5) && (_versionMajor != 2 || _versionMinor != 0) && (_versionMajor != 2 || _versionMinor != 1) && (_versionMajor != 3 || _versionMinor != 0) && (_versionMajor != 3 || _versionMinor != 1) && (_versionMajor != 3 || _versionMinor != 2) && (_versionMajor != 3 || _versionMinor != 3) && (_versionMajor != 3 || _versionMinor != 4))
		{
			archive.EndRead3dmChunk();
			return false;
		}
		if ((_versionMajor == 1 && _versionMinor == 1) || (_versionMajor == 1 && _versionMinor == 3) || (_versionMajor == 1 && _versionMinor == 5))
		{
			new OnWindowsBitmap().ReadCompressed(ref archive);
		}
		bool flag = true;
		if (_versionMajor == 1 || _versionMajor == 2 || (_versionMajor == 3 && _versionMinor == 0) || (_versionMajor == 3 && _versionMinor == 1))
		{
			flag &= archive.ReadString(ref _type);
			flag &= archive.ReadString(ref _shape);
			flag &= archive.ReadString(ref _layer);
		}
		else
		{
			flag &= archive.ReadString(ref _type);
			flag &= archive.ReadString(ref _shape);
			_layer = GemTableHelper.GetDescriptionFromValue(GemTableHelper.GetLayer(_type, _shape));
		}
		flag &= archive.ReadDouble(ref _minX);
		flag &= archive.ReadDouble(ref _minY);
		flag &= archive.ReadDouble(ref _minZ);
		flag &= archive.ReadDouble(ref _maxX);
		flag &= archive.ReadDouble(ref _maxY);
		flag &= archive.ReadDouble(ref _maxZ);
		flag &= archive.ReadDouble(ref _bezelParameter);
		if (_versionMajor < 3)
		{
			flag &= archive.ReadDouble(ref _fudgefactor);
		}
		OnNurbsCurve val = new OnNurbsCurve();
		if (((OnObject)val).Read(ref archive))
		{
			Curve val2 = (Curve)(object)val.Convert();
			if (val2 != null)
			{
				if (val2.HasNurbsForm() > 0)
				{
					_curve = val2.ToNurbsCurve();
					if (!((CommonObject)_curve).IsValid)
					{
						csPrintLine($"Error converted nurbscurve is invalid for {_type} {_shape} {_versionMajor}.{_versionMinor}:{_gemTableVersion}");
						flag = false;
					}
				}
				else
				{
					csPrintLine($"Error converted curve does not have nurbs form for {_type} {_shape} {_versionMajor}.{_versionMinor}:{_gemTableVersion}");
					flag = false;
				}
			}
			else
			{
				csPrintLine($"Error converting curve for {_type} {_shape} {_versionMajor}.{_versionMinor}:{_gemTableVersion}");
				flag = false;
			}
		}
		else
		{
			csPrintLine($"Error reading curve for {_type} {_shape} {_versionMajor}.{_versionMinor}:{_gemTableVersion}");
			flag = false;
		}
		if ((_versionMajor == 1 && _versionMinor > 1) || _versionMajor >= 2)
		{
			if (_versionMajor == 1)
			{
				archive.ReadDouble(ref _depthPercentage);
				bool flag2 = false;
				archive.ReadBool(ref flag2);
				bool flag3 = false;
				archive.ReadBool(ref flag3);
			}
			if (_versionMajor < 3)
			{
				string text = null;
				archive.ReadString(ref text);
				string text2 = null;
				archive.ReadString(ref text2);
				int num2 = 0;
				archive.ReadInt(ref num2);
				if (num2 > 0)
				{
					Sizes.Clear();
					Sizes.Capacity = num2;
					for (int i = 0; i < num2; i++)
					{
						double width = 0.0;
						archive.ReadDouble(ref width);
						double num3 = 0.0;
						archive.ReadDouble(ref num3);
						if (Math.Abs(_depthPercentage) < CSUtil.Tolerance)
						{
							double depth = num3 * ((_maxZ - _minZ) / (_maxY - _minY));
							Sizes.Add(new GemSize(num3, width, depth));
						}
						else
						{
							Sizes.Add(new GemSizePercent(num3, width, _depthPercentage));
						}
					}
				}
			}
			else
			{
				if (_versionMinor > 2)
				{
					archive.ReadInt(ref _gemTableVersion);
					if (_gemTableVersion == 0)
					{
						_gemTableVersion = GemTableHelper.GetCurrentGemTableVersion(_type, _shape);
					}
				}
				else if (isReloading)
				{
					_gemTableVersion = GemTableHelper.GetCurrentGemTableVersion(_type, _shape);
				}
				else
				{
					_gemTableVersion = ((!(_shape == "Oval")) ? 1 : 3);
				}
				Sizes.Clear();
				Sizes.AddRange(GemTableHelper.GetGemSizes(_type, _shape, _gemTableVersion));
				_fudgefactor = GemTableHelper.GetFudgeFactor(_type, _shape);
			}
			if ((_versionMajor == 1 && _versionMinor > 3) || _versionMajor >= 2)
			{
				int num4 = 0;
				archive.ReadInt(ref num4);
				if (num4 > 0)
				{
					ProngPositions.Clear();
					for (int j = 0; j < num4; j++)
					{
						double num5 = 0.0;
						archive.ReadDouble(ref num5);
						int num6 = -1;
						archive.ReadInt(ref num6);
						if (num5 != 0.0 || num6 != -1)
						{
							ProngPositions.Add(new ProngPosition(num6, num5));
						}
					}
				}
				else
				{
					ProngPositions.Clear();
				}
			}
		}
		if ((_versionMajor == 2 && _versionMinor == 1) || (_versionMajor == 3 && _versionMinor >= 1) || _versionMajor > 3)
		{
			flag &= archive.ReadBool(ref _isGeneratedGem);
		}
		OnMesh val3 = new OnMesh();
		if (((OnObject)val3).Read(ref archive))
		{
			_mesh = val3.Convert();
			if (_mesh != null)
			{
				if (!((CommonObject)_mesh).IsValid)
				{
					csPrintLine($"Error converted mesh is invalid for {_type} {_shape} {_versionMajor}.{_versionMinor}:{_gemTableVersion}");
					flag = false;
				}
			}
			else
			{
				csPrintLine($"Error converting mesh for {_type} {_shape} {_versionMajor}.{_versionMinor}:{_gemTableVersion}");
				flag = false;
			}
		}
		else
		{
			csPrintLine($"Error reading mesh for {_type} {_shape} {_versionMajor}.{_versionMinor}:{_gemTableVersion}");
			flag = false;
		}
		archive.EndRead3dmChunk();
		return flag;
	}

	public JObject WriteJson(LogPrintLine csPrintLine)
	{
		if (_versionMajor == 1)
		{
			switch (_versionMinor)
			{
			case 1:
			case 3:
			case 5:
				_versionMinor = 4;
				break;
			}
		}
		JObject jObject = new JObject();
		jObject.Add("VersionMajor", _versionMajor);
		jObject.Add("VersionMinor", _versionMinor);
		jObject.Add("Type", _type);
		jObject.Add("Shape", _shape);
		if (_versionMajor == 1 || _versionMajor == 2 || (_versionMajor == 3 && _versionMinor == 0) || (_versionMajor == 3 && _versionMinor == 1))
		{
			jObject.Add("Layer", _layer);
		}
		jObject.Add("minX", _minX);
		jObject.Add("minY", _minY);
		jObject.Add("minZ", _minZ);
		jObject.Add("maxX", _maxX);
		jObject.Add("maxY", _maxY);
		jObject.Add("maxZ", _maxZ);
		jObject.Add("bezelParameter", _bezelParameter);
		if (_versionMajor < 3)
		{
			jObject.Add("fudgeFactor", _fudgefactor);
		}
		jObject.Add("curve", GeometryBaseToString((GeometryBase)(object)_curve));
		if (_versionMajor == 1)
		{
			jObject.Add("depthPercentage", _depthPercentage);
		}
		if (_versionMajor < 3)
		{
			JArray jArray = new JArray();
			foreach (GemSize size in Sizes)
			{
				jArray.Add(new JObject
				{
					{ "sizeX", size.X },
					{ "sizeY", size.Y }
				});
			}
			jObject.Add("sizes", jArray);
		}
		else if (_versionMinor > 2)
		{
			jObject.Add("gemTableVersion", _gemTableVersion);
		}
		JArray jArray2 = new JArray();
		foreach (ProngPosition prongPosition in ProngPositions)
		{
			jArray2.Add(new JObject
			{
				{ "prongParam", prongPosition.ProngParam },
				{ "prongLayer", prongPosition.ProngLayer }
			});
		}
		jObject.Add("prongPos", jArray2);
		if ((_versionMajor == 2 && _versionMinor == 1) || (_versionMajor == 3 && _versionMinor >= 1) || _versionMajor > 3)
		{
			jObject.Add("isGenerated", _isGeneratedGem);
		}
		if (_mesh != null)
		{
			jObject.Add("gemMesh", GeometryBaseToString((GeometryBase)(object)_mesh));
		}
		else
		{
			csPrintLine($"Error converting mesh for {_type} {_shape} {_versionMajor}.{_versionMinor}:{_gemTableVersion}");
		}
		return jObject;
	}

	private static string GeometryBaseToString(GeometryBase gb)
	{
		try
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			using MemoryStream memoryStream = new MemoryStream();
			binaryFormatter.Serialize(memoryStream, gb);
			return Convert.ToBase64String(memoryStream.ToArray());
		}
		catch (Exception)
		{
			return null;
		}
	}

	private static GeometryBase GeometryBaseFromString(string str)
	{
		try
		{
			byte[] array = Convert.FromBase64String(str);
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(array, 0, array.Length);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			object obj = new BinaryFormatter().Deserialize(memoryStream);
			memoryStream.Dispose();
			return (GeometryBase)((obj is GeometryBase) ? obj : null);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public bool Write(ref OnBinaryArchive archive, LogPrintLine csPrintLine)
	{
		if (_versionMajor == 1)
		{
			switch (_versionMinor)
			{
			case 1:
			case 3:
			case 5:
				_versionMinor = 4;
				break;
			}
		}
		archive.BeginWrite3dmChunk(TypeCodes.TC_USER | TypeCodes.TC_CRC, _versionMajor, _versionMinor);
		archive.WriteString(_type);
		archive.WriteString(_shape);
		if (_versionMajor == 1 || _versionMajor == 2 || (_versionMajor == 3 && _versionMinor == 0) || (_versionMajor == 3 && _versionMinor == 1))
		{
			archive.WriteString(_layer);
		}
		archive.WriteDouble(_minX);
		archive.WriteDouble(_minY);
		archive.WriteDouble(_minZ);
		archive.WriteDouble(_maxX);
		archive.WriteDouble(_maxY);
		archive.WriteDouble(_maxZ);
		archive.WriteDouble(_bezelParameter);
		if (_versionMajor < 3)
		{
			archive.WriteDouble(_fudgefactor);
		}
		OnNurbsCurve val = _curve.Convert();
		if (val != null)
		{
			if (!((OnObject)val).Write(ref archive))
			{
				csPrintLine($"Error writing curve for {_type} {_shape} {_versionMajor}.{_versionMinor}:{_gemTableVersion}");
			}
		}
		else
		{
			csPrintLine($"Error converting curve for {_type} {_shape} {_versionMajor}.{_versionMinor}:{_gemTableVersion}");
		}
		if (_versionMajor == 1)
		{
			archive.WriteDouble(_depthPercentage);
			archive.WriteBool(false);
			archive.WriteBool(false);
		}
		if (_versionMajor < 3)
		{
			archive.WriteString(string.Empty);
			archive.WriteString(string.Empty);
			archive.WriteInt(Sizes.Count);
			foreach (GemSize size in Sizes)
			{
				archive.WriteDouble(size.X);
				archive.WriteDouble(size.Y);
			}
		}
		else if (_versionMinor > 2)
		{
			archive.WriteInt(_gemTableVersion);
		}
		archive.WriteInt(ProngPositions.Count);
		foreach (ProngPosition prongPosition in ProngPositions)
		{
			archive.WriteDouble(prongPosition.ProngParam);
			archive.WriteInt(prongPosition.ProngLayer);
		}
		if ((_versionMajor == 2 && _versionMinor == 1) || (_versionMajor == 3 && _versionMinor >= 1) || _versionMajor > 3)
		{
			archive.WriteBool(_isGeneratedGem);
		}
		OnMesh val2 = _mesh.Convert();
		if (val2 != null)
		{
			if (!((OnObject)val2).Write(ref archive))
			{
				csPrintLine($"Error writing mesh for {_type} {_shape} {_versionMajor}.{_versionMinor}:{_gemTableVersion}");
			}
		}
		else
		{
			csPrintLine($"Error converting mesh for {_type} {_shape} {_versionMajor}.{_versionMinor}:{_gemTableVersion}");
		}
		archive.EndWrite3dmChunk();
		return true;
	}

	public virtual bool AddGemToDocument(double xSize, double ySize, double zSize, Transform xUp)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		if (!GetGemInstanceDefinition(out var gemInstanceDefinition))
		{
			return false;
		}
		Transform val = Transform.Rotation(CSUtilGem.ToRadians(GemTableHelper.GetRotationAngle(_type, _shape)), Vector3d.ZAxis, Point3d.Origin);
		BoundingBox unset = BoundingBox.Unset;
		RhinoObject[] objects = gemInstanceDefinition.GetObjects();
		for (int i = 0; i < objects.Length; i++)
		{
			BoundingBox boundingBox = objects[i].Geometry.GetBoundingBox(true);
			((BoundingBox)(ref boundingBox)).Transform(val);
			((BoundingBox)(ref unset)).Union(boundingBox);
		}
		Plane worldXY = Plane.WorldXY;
		Point3d val2 = ((BoundingBox)(ref unset)).Max;
		double x = ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref unset)).Min;
		double num = xSize / (x - ((Point3d)(ref val2)).X);
		val2 = ((BoundingBox)(ref unset)).Max;
		double y = ((Point3d)(ref val2)).Y;
		val2 = ((BoundingBox)(ref unset)).Min;
		double num2 = ySize / (y - ((Point3d)(ref val2)).Y);
		val2 = ((BoundingBox)(ref unset)).Max;
		double z = ((Point3d)(ref val2)).Z;
		val2 = ((BoundingBox)(ref unset)).Min;
		Transform val3 = Transform.Scale(worldXY, num, num2, zSize / (z - ((Point3d)(ref val2)).Z));
		Transform val4 = xUp * val3 * val;
		RhinoDoc.ActiveDoc.Objects.AddInstanceObject(gemInstanceDefinition.Index, val4);
		return true;
	}

	public virtual bool AddGemToDocumentExport(double xSize, double ySize, double zSize, Transform xUp, ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, MaterialGroup currentMaterialGroup, MaterialGroup defaultMaterialGroup)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		if (!GetGemInstanceDefinition(out var gemInstanceDefinition))
		{
			return false;
		}
		Transform val = Transform.Rotation(CSUtilGem.ToRadians(GemTableHelper.GetRotationAngle(_type, _shape)), Vector3d.ZAxis, Point3d.Origin);
		BoundingBox unset = BoundingBox.Unset;
		RhinoObject[] objects = gemInstanceDefinition.GetObjects();
		for (int i = 0; i < objects.Length; i++)
		{
			BoundingBox boundingBox = objects[i].Geometry.GetBoundingBox(true);
			((BoundingBox)(ref boundingBox)).Transform(val);
			((BoundingBox)(ref unset)).Union(boundingBox);
		}
		Plane worldXY = Plane.WorldXY;
		Point3d val2 = ((BoundingBox)(ref unset)).Max;
		double x = ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref unset)).Min;
		double num = xSize / (x - ((Point3d)(ref val2)).X);
		val2 = ((BoundingBox)(ref unset)).Max;
		double y = ((Point3d)(ref val2)).Y;
		val2 = ((BoundingBox)(ref unset)).Min;
		double num2 = ySize / (y - ((Point3d)(ref val2)).Y);
		val2 = ((BoundingBox)(ref unset)).Max;
		double z = ((Point3d)(ref val2)).Z;
		val2 = ((BoundingBox)(ref unset)).Min;
		Transform val3 = Transform.Scale(worldXY, num, num2, zSize / (z - ((Point3d)(ref val2)).Z));
		Transform val4 = xUp * val3 * val;
		ObjectAttributes val5 = new ObjectAttributes
		{
			Name = "Gem",
			MaterialSource = (ObjectMaterialSource)1
		};
		if (currentMaterialGroup.IsDesignerMaterialGroup)
		{
			val5.AddToGroup(RhinoDoc.ActiveDoc.Groups.Find(currentMaterialGroup.Name.Trim(), true));
		}
		Guid rhinoObjectGuid = RhinoDoc.ActiveDoc.Objects.AddInstanceObject(gemInstanceDefinition.Index, val4, val5);
		rhinoRenderObjects.Add(new RhinoRenderObject(rhinoObjectGuid, defaultMaterialGroup));
		return true;
	}

	public virtual bool AddGemToDocumentRender(double xSize, double ySize, double zSize, Transform xUp, ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, MaterialGroup currentMaterialGroup)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		if (!GetGemInstanceDefinition(out var gemInstanceDefinition))
		{
			return false;
		}
		Transform val = Transform.Rotation(CSUtilGem.ToRadians(GemTableHelper.GetRotationAngle(_type, _shape)), Vector3d.ZAxis, Point3d.Origin);
		BoundingBox unset = BoundingBox.Unset;
		RhinoObject[] objects = gemInstanceDefinition.GetObjects();
		for (int i = 0; i < objects.Length; i++)
		{
			BoundingBox boundingBox = objects[i].Geometry.GetBoundingBox(true);
			((BoundingBox)(ref boundingBox)).Transform(val);
			((BoundingBox)(ref unset)).Union(boundingBox);
		}
		Plane worldXY = Plane.WorldXY;
		Point3d val2 = ((BoundingBox)(ref unset)).Max;
		double x = ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref unset)).Min;
		double num = xSize / (x - ((Point3d)(ref val2)).X);
		val2 = ((BoundingBox)(ref unset)).Max;
		double y = ((Point3d)(ref val2)).Y;
		val2 = ((BoundingBox)(ref unset)).Min;
		double num2 = ySize / (y - ((Point3d)(ref val2)).Y);
		val2 = ((BoundingBox)(ref unset)).Max;
		double z = ((Point3d)(ref val2)).Z;
		val2 = ((BoundingBox)(ref unset)).Min;
		Transform val3 = Transform.Scale(worldXY, num, num2, zSize / (z - ((Point3d)(ref val2)).Z));
		Transform val4 = xUp * val3 * val;
		ObjectAttributes val5 = new ObjectAttributes
		{
			MaterialSource = (ObjectMaterialSource)1
		};
		Guid rhinoObjectGuid = RhinoDoc.ActiveDoc.Objects.AddInstanceObject(gemInstanceDefinition.Index, val4, val5);
		rhinoRenderObjects.Add(new RhinoRenderObject(rhinoObjectGuid, currentMaterialGroup));
		return true;
	}

	protected virtual bool GetGemInstanceDefinition(out InstanceDefinition gemInstanceDefinition)
	{
		gemInstanceDefinition = CSUtilGem.GetGemInstanceDefinition(_type, _shape);
		return gemInstanceDefinition != null;
	}
}
