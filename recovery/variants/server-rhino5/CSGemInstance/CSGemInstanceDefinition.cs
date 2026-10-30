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
			if (!_curve.IsValid)
			{
				return false;
			}
			if (_mesh == null)
			{
				return false;
			}
			if (!_mesh.IsValid)
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

	public Mesh Mesh => _mesh?.DuplicateMesh();

	public double MinX => _minX;

	public double MinY => _minY;

	public double MinZ => _minZ;

	public NurbsCurve NurbsCurve => _curve?.ToNurbsCurve();

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
		ObjectEnumeratorSettings settings = new ObjectEnumeratorSettings
		{
			DeletedObjects = false,
			ActiveObjects = true,
			ReferenceObjects = true,
			IncludeGrips = false,
			IncludeLights = false,
			ObjectTypeFilter = ObjectType.Brep
		};
		foreach (RhinoObject @object in RhinoDoc.ActiveDoc.Objects.GetObjectList(settings))
		{
			_layer = RhinoDoc.ActiveDoc.Layers[@object.Attributes.LayerIndex].Name;
			if (@object.Geometry != null)
			{
				@object.CreateMeshes(MeshType.Preview, MeshingParameters.Smooth, ignoreCustomParameters: true);
				Mesh[] meshes = @object.GetMeshes(MeshType.Preview);
				_mesh = new Mesh();
				Mesh[] array = meshes;
				foreach (Mesh other in array)
				{
					_mesh.Append(other);
				}
				BoundingBox boundingBox = _mesh.GetBoundingBox(accurate: true);
				_maxX = boundingBox.Max.X;
				_maxY = boundingBox.Max.Y;
				_maxZ = boundingBox.Max.Z;
				_minX = boundingBox.Min.X;
				_minY = boundingBox.Min.Y;
				_minZ = boundingBox.Min.Z;
			}
		}
		if (_mesh == null)
		{
			return false;
		}
		ObjectEnumeratorSettings settings2 = new ObjectEnumeratorSettings
		{
			DeletedObjects = false,
			ActiveObjects = true,
			ReferenceObjects = true,
			IncludeGrips = false,
			IncludeLights = false,
			ObjectTypeFilter = ObjectType.Curve
		};
		foreach (RhinoObject object2 in RhinoDoc.ActiveDoc.Objects.GetObjectList(settings2))
		{
			if (object2.Geometry != null && object2.Geometry is Curve curve && curve.HasNurbsForm() > 0)
			{
				_curve = curve.ToNurbsCurve();
			}
		}
		if (_curve == null)
		{
			return false;
		}
		ObjectEnumeratorSettings settings3 = new ObjectEnumeratorSettings
		{
			DeletedObjects = false,
			ActiveObjects = true,
			ReferenceObjects = true,
			IncludeGrips = false,
			IncludeLights = false,
			ObjectTypeFilter = ObjectType.Point
		};
		ProngPosition[] array2 = new ProngPosition[RhinoDoc.ActiveDoc.Objects.GetObjectList(settings3).Count() - 1];
		foreach (RhinoObject object3 in RhinoDoc.ActiveDoc.Objects.GetObjectList(settings3))
		{
			Match match = new Regex("^P(\\d*$)").Match(object3.Attributes.Name);
			if (!match.Success || match.Groups.Count < 2)
			{
				continue;
			}
			int num = Convert.ToInt32(match.Groups[match.Groups.Count - 1].Value);
			if (object3 is PointObject pointObject)
			{
				Point3d location = pointObject.PointGeometry.Location;
				if (location.IsValid && _curve.ClosestPoint(location, out var t))
				{
					array2[num] = new ProngPosition(pointObject.Attributes.LayerIndex, t);
				}
			}
		}
		ProngPositions.Clear();
		ProngPositions.AddRange(new List<ProngPosition>(array2));
		foreach (RhinoObject object4 in RhinoDoc.ActiveDoc.Objects.GetObjectList(settings3))
		{
			if (!(object4.Attributes.Name != "BP") && object4 is PointObject pointObject2)
			{
				Point3d location2 = pointObject2.PointGeometry.Location;
				if (location2.IsValid && _curve.ClosestPoint(location2, out var t2))
				{
					_bezelParameter = t2;
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
		if (_mesh.IsValid)
		{
			if (form.IsValid)
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
		if (_mesh == null)
		{
			return false;
		}
		BoundingBox boundingBox2 = _mesh.GetBoundingBox(accurate: false);
		if (!boundingBox2.MakeValid())
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
			_mesh.Dispose();
		}
		if (_curve != null)
		{
			_curve.Dispose();
		}
		_mesh = null;
		_curve = null;
	}

	public bool ReadJson(JObject jObj, LogPrintLine csPrintLine, bool isReloading)
	{
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
			Curve curve = val.Convert();
			if (curve != null)
			{
				if (curve.HasNurbsForm() > 0)
				{
					_curve = curve.ToNurbsCurve();
					if (!_curve.IsValid)
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
		OnMesh val2 = new OnMesh();
		if (((OnObject)val2).Read(ref archive))
		{
			_mesh = val2.Convert();
			if (_mesh != null)
			{
				if (!_mesh.IsValid)
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
		jObject.Add("curve", GeometryBaseToString(_curve));
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
			jObject.Add("gemMesh", GeometryBaseToString(_mesh));
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
			return obj as GeometryBase;
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
		if (!GetGemInstanceDefinition(out var gemInstanceDefinition))
		{
			return false;
		}
		Transform transform = Transform.Rotation(CSUtilGem.ToRadians(GemTableHelper.GetRotationAngle(_type, _shape)), Vector3d.ZAxis, Point3d.Origin);
		BoundingBox unset = BoundingBox.Unset;
		RhinoObject[] objects = gemInstanceDefinition.GetObjects();
		for (int i = 0; i < objects.Length; i++)
		{
			BoundingBox boundingBox = objects[i].Geometry.GetBoundingBox(accurate: true);
			boundingBox.Transform(transform);
			unset.Union(boundingBox);
		}
		Transform transform2 = Transform.Scale(Plane.WorldXY, xSize / (unset.Max.X - unset.Min.X), ySize / (unset.Max.Y - unset.Min.Y), zSize / (unset.Max.Z - unset.Min.Z));
		Transform instanceXform = xUp * transform2 * transform;
		RhinoDoc.ActiveDoc.Objects.AddInstanceObject(gemInstanceDefinition.Index, instanceXform);
		return true;
	}

	public virtual bool AddGemToDocumentExport(double xSize, double ySize, double zSize, Transform xUp, ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, MaterialGroup currentMaterialGroup, MaterialGroup defaultMaterialGroup)
	{
		if (!GetGemInstanceDefinition(out var gemInstanceDefinition))
		{
			return false;
		}
		Transform transform = Transform.Rotation(CSUtilGem.ToRadians(GemTableHelper.GetRotationAngle(_type, _shape)), Vector3d.ZAxis, Point3d.Origin);
		BoundingBox unset = BoundingBox.Unset;
		RhinoObject[] objects = gemInstanceDefinition.GetObjects();
		for (int i = 0; i < objects.Length; i++)
		{
			BoundingBox boundingBox = objects[i].Geometry.GetBoundingBox(accurate: true);
			boundingBox.Transform(transform);
			unset.Union(boundingBox);
		}
		Transform transform2 = Transform.Scale(Plane.WorldXY, xSize / (unset.Max.X - unset.Min.X), ySize / (unset.Max.Y - unset.Min.Y), zSize / (unset.Max.Z - unset.Min.Z));
		Transform instanceXform = xUp * transform2 * transform;
		ObjectAttributes objectAttributes = new ObjectAttributes
		{
			Name = "Gem",
			MaterialSource = ObjectMaterialSource.MaterialFromObject
		};
		if (currentMaterialGroup.IsDesignerMaterialGroup)
		{
			objectAttributes.AddToGroup(RhinoDoc.ActiveDoc.Groups.Find(currentMaterialGroup.Name.Trim(), ignoreDeletedGroups: true));
		}
		Guid rhinoObjectGuid = RhinoDoc.ActiveDoc.Objects.AddInstanceObject(gemInstanceDefinition.Index, instanceXform, objectAttributes);
		rhinoRenderObjects.Add(new RhinoRenderObject(rhinoObjectGuid, defaultMaterialGroup));
		return true;
	}

	public virtual bool AddGemToDocumentRender(double xSize, double ySize, double zSize, Transform xUp, ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, MaterialGroup currentMaterialGroup)
	{
		if (!GetGemInstanceDefinition(out var gemInstanceDefinition))
		{
			return false;
		}
		Transform transform = Transform.Rotation(CSUtilGem.ToRadians(GemTableHelper.GetRotationAngle(_type, _shape)), Vector3d.ZAxis, Point3d.Origin);
		BoundingBox unset = BoundingBox.Unset;
		RhinoObject[] objects = gemInstanceDefinition.GetObjects();
		for (int i = 0; i < objects.Length; i++)
		{
			BoundingBox boundingBox = objects[i].Geometry.GetBoundingBox(accurate: true);
			boundingBox.Transform(transform);
			unset.Union(boundingBox);
		}
		Transform transform2 = Transform.Scale(Plane.WorldXY, xSize / (unset.Max.X - unset.Min.X), ySize / (unset.Max.Y - unset.Min.Y), zSize / (unset.Max.Z - unset.Min.Z));
		Transform instanceXform = xUp * transform2 * transform;
		ObjectAttributes attributes = new ObjectAttributes
		{
			MaterialSource = ObjectMaterialSource.MaterialFromObject
		};
		Guid rhinoObjectGuid = RhinoDoc.ActiveDoc.Objects.AddInstanceObject(gemInstanceDefinition.Index, instanceXform, attributes);
		rhinoRenderObjects.Add(new RhinoRenderObject(rhinoObjectGuid, currentMaterialGroup));
		return true;
	}

	protected virtual bool GetGemInstanceDefinition(out InstanceDefinition gemInstanceDefinition)
	{
		gemInstanceDefinition = CSUtilGem.GetGemInstanceDefinition(_type, _shape);
		return gemInstanceDefinition != null;
	}
}
