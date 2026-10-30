using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CSCompatibility;
using CSDatas.Interfaces;
using CSGemInstance;
using CSGemInstance.GemTableData;
using CSMaterials;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;
using Rhino.Collections;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSDatas;

public class CSGemData : CSData, ICSDisplayObject, ICSCurve, ICSDocumentObject, ICSMeshable, ICSPoint, ICSRenderObject, ICSTransform, ICSTransformable
{
	private Dictionary<string, string> _attributes;

	public int VersionMajor => Def.VersionMajor;

	public int VersionMinor => Def.VersionMinor;

	public CSGemInstanceDefinition Def { get; }

	public virtual Point3d Center
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Point3d origin = Point3d.Origin;
			((Point3d)(ref origin)).Transform(Xform);
			return origin;
		}
	}

	public virtual Vector3d Normal
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			Vector3d zAxis = Vector3d.ZAxis;
			((Vector3d)(ref zAxis)).Transform(Xform);
			((Vector3d)(ref zAxis)).Unitize();
			return zAxis;
		}
	}

	public virtual Vector3d Up
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			Vector3d zAxis = Vector3d.ZAxis;
			((Vector3d)(ref zAxis)).Transform(Xform);
			((Vector3d)(ref zAxis)).Unitize();
			return zAxis;
		}
	}

	public virtual Vector3d Down
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			Vector3d result = -Vector3d.ZAxis;
			((Vector3d)(ref result)).Transform(Xform);
			((Vector3d)(ref result)).Unitize();
			return result;
		}
	}

	public virtual Vector3d North
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			Vector3d yAxis = Vector3d.YAxis;
			((Vector3d)(ref yAxis)).Transform(Xform);
			((Vector3d)(ref yAxis)).Unitize();
			return yAxis;
		}
	}

	public virtual Vector3d South
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			Vector3d result = -Vector3d.YAxis;
			((Vector3d)(ref result)).Transform(Xform);
			((Vector3d)(ref result)).Unitize();
			return result;
		}
	}

	public virtual Vector3d East
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			Vector3d xAxis = Vector3d.XAxis;
			((Vector3d)(ref xAxis)).Transform(Xform);
			((Vector3d)(ref xAxis)).Unitize();
			return xAxis;
		}
	}

	public virtual Vector3d West
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			Vector3d result = -new Vector3d(Vector3d.XAxis);
			((Vector3d)(ref result)).Transform(Xform);
			((Vector3d)(ref result)).Unitize();
			return result;
		}
	}

	public string GemType { get; }

	public string GemShape { get; }

	public double GemXSize
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(Def.MinX, 0.0, 0.0);
			((Point3d)(ref val)).Transform(Xform);
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(Def.MaxX, 0.0, 0.0);
			((Point3d)(ref val2)).Transform(Xform);
			Vector3d val3 = default(Vector3d);
			((Vector3d)(ref val3))._002Ector(val - val2);
			return ((Vector3d)(ref val3)).Length;
		}
	}

	public double GemYSize
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(0.0, Def.MinY, 0.0);
			((Point3d)(ref val)).Transform(Xform);
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(0.0, Def.MaxY, 0.0);
			((Point3d)(ref val2)).Transform(Xform);
			Vector3d val3 = default(Vector3d);
			((Vector3d)(ref val3))._002Ector(val - val2);
			return ((Vector3d)(ref val3)).Length;
		}
	}

	public double GemZSize
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			Point3d val = default(Point3d);
			((Point3d)(ref val))._002Ector(0.0, 0.0, Def.MinZ);
			((Point3d)(ref val)).Transform(Xform);
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(0.0, 0.0, Def.MaxZ);
			((Point3d)(ref val2)).Transform(Xform);
			Vector3d val3 = default(Vector3d);
			((Vector3d)(ref val3))._002Ector(val - val2);
			return ((Vector3d)(ref val3)).Length;
		}
	}

	public double GemXMin
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			Vector3d val = default(Vector3d);
			((Vector3d)(ref val))._002Ector(Def.MinX, 0.0, 0.0);
			((Vector3d)(ref val)).Transform(Xform);
			return ((Vector3d)(ref val)).Length;
		}
	}

	public double GemYMin
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			Vector3d val = default(Vector3d);
			((Vector3d)(ref val))._002Ector(0.0, Def.MinY, 0.0);
			((Vector3d)(ref val)).Transform(Xform);
			return ((Vector3d)(ref val)).Length;
		}
	}

	public double GemZMin
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			Vector3d val = default(Vector3d);
			((Vector3d)(ref val))._002Ector(0.0, 0.0, Def.MinZ);
			((Vector3d)(ref val)).Transform(Xform);
			return ((Vector3d)(ref val)).Length;
		}
	}

	public double GemXMax
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			Vector3d val = default(Vector3d);
			((Vector3d)(ref val))._002Ector(Def.MaxX, 0.0, 0.0);
			((Vector3d)(ref val)).Transform(Xform);
			return ((Vector3d)(ref val)).Length;
		}
	}

	public double GemYMax
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			Vector3d val = default(Vector3d);
			((Vector3d)(ref val))._002Ector(0.0, Def.MaxY, 0.0);
			((Vector3d)(ref val)).Transform(Xform);
			return ((Vector3d)(ref val)).Length;
		}
	}

	public double GemZMax
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			Vector3d val = default(Vector3d);
			((Vector3d)(ref val))._002Ector(0.0, 0.0, Def.MaxZ);
			((Vector3d)(ref val)).Transform(Xform);
			return ((Vector3d)(ref val)).Length;
		}
	}

	public Point3d XMaxPoint
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			Point3d result = default(Point3d);
			((Point3d)(ref result))._002Ector(Def.MaxX, 0.0, 0.0);
			((Point3d)(ref result)).Transform(Xform);
			return result;
		}
	}

	public Point3d YMaxPoint
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			Point3d result = default(Point3d);
			((Point3d)(ref result))._002Ector(0.0, Def.MaxY, 0.0);
			((Point3d)(ref result)).Transform(Xform);
			return result;
		}
	}

	public Point3d ZMaxPoint
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			Point3d result = default(Point3d);
			((Point3d)(ref result))._002Ector(0.0, 0.0, Def.MaxZ);
			((Point3d)(ref result)).Transform(Xform);
			return result;
		}
	}

	public Point3d XMinPoint
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			Point3d result = default(Point3d);
			((Point3d)(ref result))._002Ector(Def.MinX, 0.0, 0.0);
			((Point3d)(ref result)).Transform(Xform);
			return result;
		}
	}

	public Point3d YMinPoint
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			Point3d result = default(Point3d);
			((Point3d)(ref result))._002Ector(0.0, Def.MinY, 0.0);
			((Point3d)(ref result)).Transform(Xform);
			return result;
		}
	}

	public Point3d ZMinPoint
	{
		get
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			Point3d result = default(Point3d);
			((Point3d)(ref result))._002Ector(0.0, 0.0, Def.MinZ);
			((Point3d)(ref result)).Transform(Xform);
			return result;
		}
	}

	public double FFactor
	{
		get
		{
			if (Def == null)
			{
				return 0.0;
			}
			return Def.FudgeFactor;
		}
	}

	public double BezelParam
	{
		get
		{
			if (Def == null)
			{
				return 0.0;
			}
			return Def.BezelParam;
		}
	}

	public List<GemSize> Sizes
	{
		get
		{
			if (Def != null)
			{
				return Def.Sizes;
			}
			return new List<GemSize>();
		}
	}

	public int CurrentSizeTableIndex { get; set; }

	public string GemGroupName { get; set; }

	public string GemNumber { get; set; }

	public double TableMaxX { get; set; }

	public double TableMaxY { get; set; }

	public double TableMinX { get; set; }

	public double TableMinY { get; set; }

	public GemSettingType SettingType { get; set; }

	public Transform Xform { get; set; }

	public CSGemData(CSGemInstanceDefinition definition)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		Xform = Transform.Identity;
		Def = definition;
		GemType = definition.Type;
		GemShape = definition.Shape;
	}

	public CSGemData(string type, string shape, CSGemInstanceDefinition definition)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		GemType = type;
		GemShape = shape;
		Def = definition;
		Xform = Transform.Identity;
	}

	public CSGemData(string type, string shape, CSGemInstanceDefinition definition, IOnXform transform)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		GemType = type;
		GemShape = shape;
		Def = definition;
		Xform = ((transform != null) ? CSConvert.XformCopyToTransform(transform) : Transform.Identity);
	}

	public CSGemData(string type, string shape, CSGemInstanceDefinition definition, Transform transform)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		GemType = type;
		GemShape = shape;
		Def = definition;
		Xform = ((transform != Transform.Unset) ? transform : Transform.Identity);
	}

	public CSGemData(CSGemData copy)
		: base(copy)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		GemGroupName = copy.GemGroupName;
		GemNumber = copy.GemNumber;
		TableMinX = copy.TableMinX;
		TableMinY = copy.TableMinY;
		TableMaxX = copy.TableMaxX;
		TableMaxY = copy.TableMaxY;
		CurrentSizeTableIndex = copy.CurrentSizeTableIndex;
		GemType = copy.GemType;
		GemShape = copy.GemShape;
		if (copy.Def != null)
		{
			Def = copy.Def;
		}
		Xform = copy.Xform;
		SettingType = copy.SettingType;
		if (copy._attributes == null)
		{
			_attributes = null;
			return;
		}
		_attributes = new Dictionary<string, string>();
		foreach (KeyValuePair<string, string> attribute in copy._attributes)
		{
			_attributes.Add(attribute.Key, attribute.Value);
		}
	}

	public override Curve GetCurve()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Curve val = (Curve)(object)Def?.NurbsCurve;
		if (val == null)
		{
			return null;
		}
		((GeometryBase)val).MakeDeformable();
		((GeometryBase)val).Transform(Xform);
		return val;
	}

	public override NurbsCurve GetNurbsCurve()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve val = Def?.NurbsCurve;
		if (val == null)
		{
			return null;
		}
		((GeometryBase)val).MakeDeformable();
		((GeometryBase)val).Transform(Xform);
		return val;
	}

	public override bool HasNurbsCurve()
	{
		if (Def != null)
		{
			return Def.HasNurbsCurve;
		}
		return false;
	}

	public bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial csDisplayMaterial = null)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		if (Def == null)
		{
			return false;
		}
		if (csDisplayMaterial == null)
		{
			csDisplayMaterial = displayOptions.GetCSDisplayMaterialFromName(base.CurrentMaterialGroup.MaterialName);
		}
		Def.DrawMesh(displayPipeline, Xform, csDisplayMaterial?.Mat);
		if (!displayOptions.IsGemNumberDisplayEnabled)
		{
			return true;
		}
		if (GemNumber != null)
		{
			BoundingBox boundingBox = BoundingBox.Unset;
			GetTightBoundingBox(ref boundingBox);
			((BoundingBox)(ref boundingBox)).Transform(displayPipeline.ModelTransform);
			Point2d val = displayPipeline.Viewport.WorldToClient(((BoundingBox)(ref boundingBox)).Center);
			displayPipeline.Draw2dText(GemNumber.ToString(CultureInfo.InvariantCulture), System.Drawing.Color.Black, val, true, 20, "Consolas Bold");
		}
		return true;
	}

	public bool AssignCurrentMaterialGroup(MaterialGroup materialGroup)
	{
		bool result = false;
		if (materialGroup.IsGemMaterialGroup)
		{
			base.CurrentMaterialGroup = materialGroup;
			result = true;
		}
		if (!materialGroup.IsMetalMaterialGroup)
		{
			base.CurrentMaterialGroup = materialGroup;
		}
		return result;
	}

	public void AddToDocument()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		double gemXSize = GemXSize;
		double gemYSize = GemYSize;
		double gemZSize = GemZSize;
		CSUtilMath.GetUnscaledXform(Xform, out var xUp);
		if (Def.AddGemToDocument(gemXSize, gemYSize, gemZSize, xUp))
		{
			return;
		}
		Mesh mesh = Def.Mesh;
		try
		{
			((GeometryBase)mesh).Transform(Xform);
			RhinoDoc.ActiveDoc.Objects.AddMesh(Interop.FromOnMesh((object)mesh));
		}
		finally
		{
			((IDisposable)mesh)?.Dispose();
		}
	}

	public bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Transform transform, CSOptionsExport exportOptions)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		if (exportOptions.WhenExportingHonorGemPrintingFlags && !IsGemGroupIncludedInPrintMesh(exportOptions.IsCenterGemAllowedForPrinting, exportOptions.IsSideGemAllowedForPrinting, exportOptions.IsAccentGemAllowedForPrinting))
		{
			return false;
		}
		if (transform != Transform.Unset && transform != Transform.Identity)
		{
			CSData cSData = Duplicate();
			(cSData as ICSTransformable)?.ApplyTransform(transform);
			if (cSData is ICSDocumentObject iCSDocumentObject)
			{
				return iCSDocumentObject.AddToDocumentExport(ref rhinoRenderObjects, Transform.Unset, exportOptions);
			}
		}
		MaterialGroup materialGroup = MaterialGroupHelper.GetMaterialGroupByClassification(base.DefaultDisplayMaterialClassification).FirstOrDefault();
		if (materialGroup == null)
		{
			return false;
		}
		double gemXSize = GemXSize;
		double gemYSize = GemYSize;
		double gemZSize = GemZSize;
		CSUtilMath.GetUnscaledXform(Xform, out var xUp);
		if (exportOptions.WhenExportingHonorGemPrintingFlags && !exportOptions.IsExportingSTL && Def.AddGemToDocumentExport(gemXSize, gemYSize, gemZSize, xUp, ref rhinoRenderObjects, base.CurrentMaterialGroup, materialGroup))
		{
			return true;
		}
		if (Def.AddGemToDocumentExport(gemXSize, gemYSize, gemZSize, xUp, ref rhinoRenderObjects, base.CurrentMaterialGroup, materialGroup))
		{
			return true;
		}
		ObjectAttributes val = new ObjectAttributes
		{
			Name = "Gem",
			MaterialSource = (ObjectMaterialSource)1
		};
		if (base.CurrentMaterialGroup.IsDesignerMaterialGroup)
		{
			val.AddToGroup(RhinoDoc.ActiveDoc.Groups.Find(base.CurrentMaterialGroup.Name, true));
		}
		Point3d origin = Point3d.Origin;
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(1.0, 0.0, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(0.0, 1.0, 0.0);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(0.0, 0.0, 1.0);
		((Point3d)(ref origin)).Transform(Xform);
		((Point3d)(ref val2)).Transform(Xform);
		((Point3d)(ref val3)).Transform(Xform);
		((Point3d)(ref val4)).Transform(Xform);
		Vector3d val5 = Vector3d.CrossProduct(val2 - origin, val3 - origin);
		Mesh mesh = Def.Mesh;
		try
		{
			((GeometryBase)mesh).Transform(Xform);
			if (val5 * (val4 - origin) < 0.0)
			{
				mesh.Flip(true, true, false);
			}
			rhinoRenderObjects.Add(new RhinoRenderObject(RhinoDoc.ActiveDoc.Objects.AddMesh(mesh, val), materialGroup));
		}
		finally
		{
			((IDisposable)mesh)?.Dispose();
		}
		return true;
	}

	public bool GetMeshableObjects(ref ConcurrentBag<GeometryBase> meshableObjects, CSOptionsExport exportOptions)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		if (!IsGemGroupIncludedInPrintMesh(exportOptions.IsCenterGemAllowedForPrinting, exportOptions.IsSideGemAllowedForPrinting, exportOptions.IsAccentGemAllowedForPrinting))
		{
			return true;
		}
		CSUtilMath.GetUnscaledXform(Xform, out var _);
		MRhinoObjectAttributes val = new MRhinoObjectAttributes
		{
			m_name = "Gem"
		};
		((On3dmObjectAttributes)val).SetMaterialSource((object_material_source)1);
		if (base.CurrentMaterialGroup.IsDesignerMaterialGroup)
		{
			((On3dmObjectAttributes)val).AddToGroup(RhinoDoc.ActiveDoc.Groups.Find(base.CurrentMaterialGroup.Name, true));
		}
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(0.0, 0.0, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(1.0, 0.0, 0.0);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(0.0, 1.0, 0.0);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(0.0, 0.0, 1.0);
		((Point3d)(ref val2)).Transform(Xform);
		((Point3d)(ref val3)).Transform(Xform);
		((Point3d)(ref val4)).Transform(Xform);
		((Point3d)(ref val5)).Transform(Xform);
		Vector3d val6 = Vector3d.CrossProduct(val3 - val2, val4 - val2);
		Mesh mesh = Def.Mesh;
		try
		{
			((GeometryBase)mesh).Transform(Xform);
			if (val6 * (val5 - val2) < 0.0)
			{
				mesh.Flip(true, true, false);
			}
			meshableObjects.Add((GeometryBase)(object)mesh.DuplicateMesh());
		}
		finally
		{
			((IDisposable)mesh)?.Dispose();
		}
		return true;
	}

	public override bool GetPoint(out Point3d value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		Point3d origin = Point3d.Origin;
		Transform xform = Xform;
		if (((Transform)(ref xform)).IsValid)
		{
			((Point3d)(ref origin)).Transform(Xform);
		}
		value = origin;
		return true;
	}

	public override bool HasPoint()
	{
		return true;
	}

	public bool AddToDocumentRender(ref ConcurrentBag<RhinoRenderObject> renderObjects, Transform transform)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (transform != Transform.Unset && transform != Transform.Identity)
		{
			CSData cSData = Duplicate();
			(cSData as ICSTransformable)?.ApplyTransform(transform);
			cSData.CurrentMaterialGroup = base.CurrentMaterialGroup;
			if (cSData is ICSRenderObject iCSRenderObject)
			{
				return iCSRenderObject.AddToDocumentRender(ref renderObjects, Transform.Unset);
			}
		}
		double gemXSize = GemXSize;
		double gemYSize = GemYSize;
		double gemZSize = GemZSize;
		CSUtilMath.GetUnscaledXform(Xform, out var xUp);
		if (Def.AddGemToDocumentRender(gemXSize, gemYSize, gemZSize, xUp, ref renderObjects, base.CurrentMaterialGroup))
		{
			return true;
		}
		ObjectAttributes val = new ObjectAttributes
		{
			MaterialSource = (ObjectMaterialSource)1
		};
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(0.0, 0.0, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(1.0, 0.0, 0.0);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(0.0, 1.0, 0.0);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(0.0, 0.0, 1.0);
		((Point3d)(ref val2)).Transform(Xform);
		((Point3d)(ref val3)).Transform(Xform);
		((Point3d)(ref val4)).Transform(Xform);
		((Point3d)(ref val5)).Transform(Xform);
		Vector3d val6 = Vector3d.CrossProduct(val3 - val2, val4 - val2);
		Mesh mesh = Def.Mesh;
		try
		{
			((GeometryBase)mesh).Transform(Xform);
			if (val6 * (val5 - val2) < 0.0)
			{
				mesh.Flip(true, true, false);
			}
			renderObjects.Add(new RhinoRenderObject(RhinoDoc.ActiveDoc.Objects.AddMesh(mesh, val), base.CurrentMaterialGroup));
		}
		finally
		{
			((IDisposable)mesh)?.Dispose();
		}
		return true;
	}

	public bool GetTransform(out Transform value)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		value = Xform;
		return Xform != Transform.Unset;
	}

	public virtual bool ApplyTransform(Transform transform)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (!((Transform)(ref transform)).IsValid)
		{
			return false;
		}
		Transform xform = Xform;
		Xform = (((Transform)(ref xform)).IsValid ? (transform * Xform) : transform);
		return true;
	}

	public override CSData Duplicate()
	{
		return new CSGemData(this);
	}

	public override bool GetBoundingBox(ref BoundingBox boundingBox)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		if (Def == null)
		{
			return false;
		}
		BoundingBox boundingBox2 = BoundingBox.Unset;
		Def.GetBoundingBox(ref boundingBox2);
		if (!((BoundingBox)(ref boundingBox2)).IsValid)
		{
			return false;
		}
		((BoundingBox)(ref boundingBox2)).Transform(Xform);
		boundingBox = boundingBox2;
		return true;
	}

	public bool GetGemXformRC(out Transform xDown, out Transform xUp)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		xUp = Transform.Identity;
		xDown = Transform.Identity;
		if (!GetTransform(out var value))
		{
			return false;
		}
		if (!CSUtilMath.GetUnscaledXforms(value, out var xUp2, out var xDown2))
		{
			return false;
		}
		xUp = xUp2;
		xDown = xDown2;
		return true;
	}

	public override GeometryBase GetGeometry()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Mesh mesh = Def.Mesh;
		((GeometryBase)mesh).Transform(Xform);
		return (GeometryBase)(object)mesh;
	}

	public Mesh GetMeshFromDefinition(Transform transform)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Mesh mesh = Def.Mesh;
		((GeometryBase)mesh).Transform(transform * Xform);
		return mesh;
	}

	public override PointCollection GetPolyline(double precision)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve nurbsCurve = GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return null;
		}
		BoundingBox boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
		Transform val = Transform.Rotation(RhinoMath.ToRadians(-90.0), Vector3d.XAxis, ((BoundingBox)(ref boundingBox)).Center);
		Point3d val2 = ((BoundingBox)(ref boundingBox)).Max;
		double z = ((Point3d)(ref val2)).Z;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		if (z - ((Point3d)(ref val2)).Z > 0.1)
		{
			((GeometryBase)nurbsCurve).Transform(val);
		}
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		double num = x - ((Point3d)(ref val2)).X;
		val2 = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val2)).Y;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		double num2 = y - ((Point3d)(ref val2)).Y;
		double num3 = 0.0;
		double num4 = 0.0;
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		if (((Point3d)(ref val2)).X < 0.0)
		{
			double num5 = num3;
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			num3 = num5 - ((Point3d)(ref val2)).X;
		}
		val2 = ((BoundingBox)(ref boundingBox)).Min;
		if (((Point3d)(ref val2)).Y < 0.0)
		{
			double num6 = num4;
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			num4 = num6 - ((Point3d)(ref val2)).Y;
		}
		double num7 = ((!(num > num2)) ? (20.0 / num2) : (20.0 / num));
		double num8 = (20.0 - num * num7) / 2.0;
		double num9 = (20.0 - num2 * num7) / 2.0;
		Transform val3 = Transform.Translation(num3, num4, 0.0);
		Transform val4 = Transform.Scale(Plane.WorldXY, num7, num7, num7);
		Transform val5 = Transform.Translation(num8 + 2.5, num9 + 2.5, 0.0);
		((GeometryBase)nurbsCurve).Transform(val5 * val4 * val3);
		Polyline val6 = new Polyline();
		if (precision >= 1.0)
		{
			Interval domain = ((Curve)nurbsCurve).Domain;
			for (double num10 = 0.0; num10 <= 50.0; num10 += 1.0)
			{
				((RhinoList<Point3d>)(object)val6).Add(((Curve)nurbsCurve).PointAt(((Interval)(ref domain)).ParameterAt(num10 / 50.0)));
			}
		}
		else
		{
			PolylineCurve val7 = ((Curve)nurbsCurve).ToPolyline(5, 5, 3.14, 1.0, 1.0, precision, precision, precision * 20.0, false);
			if (val7 != null)
			{
				for (int i = 0; i < val7.PointCount; i++)
				{
					Point3d val8 = val7.Point(i);
					((RhinoList<Point3d>)(object)val6).Add(val8);
				}
			}
		}
		PointCollection pointCollection = new PointCollection(((RhinoList<Point3d>)(object)val6).Count);
		foreach (Point3d item in (RhinoList<Point3d>)(object)val6)
		{
			Point3d current = item;
			pointCollection.Add(new System.Windows.Point(((Point3d)(ref current)).X, 25.0 - ((Point3d)(ref current)).Y));
		}
		return pointCollection;
	}

	public override bool GetTightBoundingBox(ref BoundingBox boundingBox)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return TryGetBoundingBox(doAccurate: true, doGrow: false, Transform.Unset, ref boundingBox);
	}

	public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return TryGetBoundingBox(doAccurate: true, doGrow, Transform.Unset, ref boundingBox);
	}

	public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow, Transform transform)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return TryGetBoundingBox(doAccurate: true, doGrow, transform, ref boundingBox);
	}

	public bool IsGemGroupIncludedInPrintMesh(bool isCenterGemAllowed, bool isSideGemAllowed, bool isAccentGemAllowed)
	{
		MaterialGroup materialGroup = MaterialGroupHelper.GetMaterialGroupByClassification(base.DefaultDisplayMaterialClassification).FirstOrDefault();
		if (materialGroup == null)
		{
			return false;
		}
		if (isCenterGemAllowed && materialGroup.Classification == MaterialClassification.GemCenter)
		{
			return true;
		}
		if (isSideGemAllowed && materialGroup.Classification == MaterialClassification.GemSide)
		{
			return true;
		}
		if (isAccentGemAllowed && materialGroup.Classification == MaterialClassification.GemAccent)
		{
			return true;
		}
		return false;
	}

	public override Mesh MeshObject(MeshingParameters mp, bool productionMeshes = false, bool useDisplayMesh = true)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Mesh mesh = Def.Mesh;
		((GeometryBase)mesh).Transform(Xform);
		return mesh;
	}

	public override bool Morph(SpaceMorph spaceMorph)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		if (spaceMorph == null)
		{
			return false;
		}
		Transform xform = Xform;
		return ((Transform)(ref xform)).IsValid;
	}

	public override string Report()
	{
		return GemType + "," + GemShape;
	}

	public void Scale(double scaleFactorX, double scaleFactorY, double scaleFactorZ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Transform val = Transform.Scale(Plane.WorldXY, scaleFactorX, scaleFactorY, scaleFactorZ);
		Xform = val * Xform;
	}

	public void Scale(double scaleFactorX, double scaleFactorY)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		Transform val = Transform.Scale(Plane.WorldXY, scaleFactorX, scaleFactorY, (scaleFactorX + scaleFactorY) / 2.0);
		Xform = val * Xform;
	}

	public void Scale(double scaleFactorX)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		Transform val = Transform.Scale(Plane.WorldXY, scaleFactorX, scaleFactorX, (scaleFactorX + scaleFactorX) / 2.0);
		Xform = val * Xform;
	}

	public void ScaleToSize(double newSizeX, double newSizeY, double newSizeZ)
	{
		Scale(newSizeX / GemXSize, newSizeY / GemYSize, newSizeZ / GemZSize);
	}

	public void ScaleToSize(double newSizeX, double newSizeY)
	{
		Scale(newSizeX / GemXSize, newSizeY / GemYSize);
	}

	public void ScaleToSize(double newSizeX)
	{
		Scale(newSizeX / GemXSize);
	}

	public void SetXform(Point3d point, Vector3d normal, double size)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		if (Def != null)
		{
			double num = Math.Max(Math.Max(Def.MaxY, 0.0 - Def.MinY), Math.Max(Def.MaxX, 0.0 - Def.MinX));
			Transform val = Transform.Scale(Point3d.Origin, size / 2.0 / num);
			Transform val2 = Transform.ChangeBasis(new Plane(point, normal), new Plane(Point3d.Origin, Vector3d.ZAxis));
			Xform = val2 * val;
		}
	}

	protected override void Dispose(bool disposing)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (!_isDisposed)
		{
			try
			{
				Xform = Transform.Unset;
			}
			finally
			{
				base.Dispose(disposing);
			}
			_isDisposed = true;
		}
	}

	private bool TryGetBoundingBox(bool doAccurate, bool doGrow, Transform transform, ref BoundingBox boundingBox)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		Mesh val = Def?.Mesh;
		if (val == null)
		{
			return false;
		}
		Transform xform = Xform;
		Transform val2 = ((!((Transform)(ref xform)).IsValid) ? (((Transform)(ref transform)).IsValid ? transform : Transform.Unset) : (((Transform)(ref transform)).IsValid ? (transform * Xform) : Xform));
		BoundingBox boundingBox2;
		if (((Transform)(ref val2)).IsValid && !((Transform)(ref val2)).Equals(Transform.Identity))
		{
			Mesh obj = val.DuplicateMesh();
			((GeometryBase)obj).Transform(val2);
			boundingBox2 = ((GeometryBase)obj).GetBoundingBox(true);
			((CommonObject)obj).Dispose();
		}
		else
		{
			boundingBox2 = ((GeometryBase)val).GetBoundingBox(doAccurate);
		}
		if (!((BoundingBox)(ref boundingBox2)).MakeValid())
		{
			return false;
		}
		if (((BoundingBox)(ref boundingBox)).IsValid && doGrow)
		{
			((BoundingBox)(ref boundingBox)).Union(boundingBox2);
		}
		else
		{
			boundingBox = boundingBox2;
		}
		return true;
	}

	public override bool GetUserString(string key, ref string value)
	{
		if (_attributes == null)
		{
			return false;
		}
		if (!_attributes.ContainsKey(key))
		{
			return false;
		}
		value = _attributes[key];
		return true;
	}

	public void SetUserString(string key, string value)
	{
		if (_attributes == null)
		{
			_attributes = new Dictionary<string, string>();
		}
		_attributes.Remove(key);
		_attributes.Add(key, value);
	}

	~CSGemData()
	{
		Dispose(disposing: false);
	}
}
