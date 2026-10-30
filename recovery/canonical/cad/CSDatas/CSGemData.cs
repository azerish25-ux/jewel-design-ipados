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
			Point3d origin = Point3d.Origin;
			origin.Transform(Xform);
			return origin;
		}
	}

	public virtual Vector3d Normal
	{
		get
		{
			Vector3d zAxis = Vector3d.ZAxis;
			zAxis.Transform(Xform);
			zAxis.Unitize();
			return zAxis;
		}
	}

	public virtual Vector3d Up
	{
		get
		{
			Vector3d zAxis = Vector3d.ZAxis;
			zAxis.Transform(Xform);
			zAxis.Unitize();
			return zAxis;
		}
	}

	public virtual Vector3d Down
	{
		get
		{
			Vector3d result = -Vector3d.ZAxis;
			result.Transform(Xform);
			result.Unitize();
			return result;
		}
	}

	public virtual Vector3d North
	{
		get
		{
			Vector3d yAxis = Vector3d.YAxis;
			yAxis.Transform(Xform);
			yAxis.Unitize();
			return yAxis;
		}
	}

	public virtual Vector3d South
	{
		get
		{
			Vector3d result = -Vector3d.YAxis;
			result.Transform(Xform);
			result.Unitize();
			return result;
		}
	}

	public virtual Vector3d East
	{
		get
		{
			Vector3d xAxis = Vector3d.XAxis;
			xAxis.Transform(Xform);
			xAxis.Unitize();
			return xAxis;
		}
	}

	public virtual Vector3d West
	{
		get
		{
			Vector3d result = -new Vector3d(Vector3d.XAxis);
			result.Transform(Xform);
			result.Unitize();
			return result;
		}
	}

	public string GemType { get; }

	public string GemShape { get; }

	public double GemXSize
	{
		get
		{
			Point3d point3d = new Point3d(Def.MinX, 0.0, 0.0);
			point3d.Transform(Xform);
			Point3d point3d2 = new Point3d(Def.MaxX, 0.0, 0.0);
			point3d2.Transform(Xform);
			return new Vector3d(point3d - point3d2).Length;
		}
	}

	public double GemYSize
	{
		get
		{
			Point3d point3d = new Point3d(0.0, Def.MinY, 0.0);
			point3d.Transform(Xform);
			Point3d point3d2 = new Point3d(0.0, Def.MaxY, 0.0);
			point3d2.Transform(Xform);
			return new Vector3d(point3d - point3d2).Length;
		}
	}

	public double GemZSize
	{
		get
		{
			Point3d point3d = new Point3d(0.0, 0.0, Def.MinZ);
			point3d.Transform(Xform);
			Point3d point3d2 = new Point3d(0.0, 0.0, Def.MaxZ);
			point3d2.Transform(Xform);
			return new Vector3d(point3d - point3d2).Length;
		}
	}

	public double GemXMin
	{
		get
		{
			Vector3d vector3d = new Vector3d(Def.MinX, 0.0, 0.0);
			vector3d.Transform(Xform);
			return vector3d.Length;
		}
	}

	public double GemYMin
	{
		get
		{
			Vector3d vector3d = new Vector3d(0.0, Def.MinY, 0.0);
			vector3d.Transform(Xform);
			return vector3d.Length;
		}
	}

	public double GemZMin
	{
		get
		{
			Vector3d vector3d = new Vector3d(0.0, 0.0, Def.MinZ);
			vector3d.Transform(Xform);
			return vector3d.Length;
		}
	}

	public double GemXMax
	{
		get
		{
			Vector3d vector3d = new Vector3d(Def.MaxX, 0.0, 0.0);
			vector3d.Transform(Xform);
			return vector3d.Length;
		}
	}

	public double GemYMax
	{
		get
		{
			Vector3d vector3d = new Vector3d(0.0, Def.MaxY, 0.0);
			vector3d.Transform(Xform);
			return vector3d.Length;
		}
	}

	public double GemZMax
	{
		get
		{
			Vector3d vector3d = new Vector3d(0.0, 0.0, Def.MaxZ);
			vector3d.Transform(Xform);
			return vector3d.Length;
		}
	}

	public Point3d XMaxPoint
	{
		get
		{
			Point3d result = new Point3d(Def.MaxX, 0.0, 0.0);
			result.Transform(Xform);
			return result;
		}
	}

	public Point3d YMaxPoint
	{
		get
		{
			Point3d result = new Point3d(0.0, Def.MaxY, 0.0);
			result.Transform(Xform);
			return result;
		}
	}

	public Point3d ZMaxPoint
	{
		get
		{
			Point3d result = new Point3d(0.0, 0.0, Def.MaxZ);
			result.Transform(Xform);
			return result;
		}
	}

	public Point3d XMinPoint
	{
		get
		{
			Point3d result = new Point3d(Def.MinX, 0.0, 0.0);
			result.Transform(Xform);
			return result;
		}
	}

	public Point3d YMinPoint
	{
		get
		{
			Point3d result = new Point3d(0.0, Def.MinY, 0.0);
			result.Transform(Xform);
			return result;
		}
	}

	public Point3d ZMinPoint
	{
		get
		{
			Point3d result = new Point3d(0.0, 0.0, Def.MinZ);
			result.Transform(Xform);
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

	public Rhino.Geometry.Transform Xform { get; set; }

	public CSGemData(CSGemInstanceDefinition definition)
	{
		Xform = Rhino.Geometry.Transform.Identity;
		Def = definition;
		GemType = definition.Type;
		GemShape = definition.Shape;
	}

	public CSGemData(string type, string shape, CSGemInstanceDefinition definition)
	{
		GemType = type;
		GemShape = shape;
		Def = definition;
		Xform = Rhino.Geometry.Transform.Identity;
	}

	public CSGemData(string type, string shape, CSGemInstanceDefinition definition, IOnXform transform)
	{
		GemType = type;
		GemShape = shape;
		Def = definition;
		Xform = ((transform != null) ? CSConvert.XformCopyToTransform(transform) : Rhino.Geometry.Transform.Identity);
	}

	public CSGemData(string type, string shape, CSGemInstanceDefinition definition, Rhino.Geometry.Transform transform)
	{
		GemType = type;
		GemShape = shape;
		Def = definition;
		Xform = ((transform != Rhino.Geometry.Transform.Unset) ? transform : Rhino.Geometry.Transform.Identity);
	}

	public CSGemData(CSGemData copy)
		: base(copy)
	{
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
		Curve curve = Def?.NurbsCurve;
		if (curve == null)
		{
			return null;
		}
		curve.MakeDeformable();
		curve.Transform(Xform);
		return curve;
	}

	public override NurbsCurve GetNurbsCurve()
	{
		NurbsCurve nurbsCurve = Def?.NurbsCurve;
		if (nurbsCurve == null)
		{
			return null;
		}
		nurbsCurve.MakeDeformable();
		nurbsCurve.Transform(Xform);
		return nurbsCurve;
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
			boundingBox.Transform(displayPipeline.ModelTransform);
			Point2d screenCoordinate = displayPipeline.Viewport.WorldToClient(boundingBox.Center);
			displayPipeline.Draw2dText(GemNumber.ToString(CultureInfo.InvariantCulture), System.Drawing.Color.Black, screenCoordinate, middleJustified: true, 20, "Consolas Bold");
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
		double gemXSize = GemXSize;
		double gemYSize = GemYSize;
		double gemZSize = GemZSize;
		CSUtilMath.GetUnscaledXform(Xform, out var xUp);
		if (Def.AddGemToDocument(gemXSize, gemYSize, gemZSize, xUp))
		{
			return;
		}
		using Mesh mesh = Def.Mesh;
		mesh.Transform(Xform);
		RhinoDoc.ActiveDoc.Objects.AddMesh(Interop.FromOnMesh(mesh));
	}

	public bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Rhino.Geometry.Transform transform, CSOptionsExport exportOptions)
	{
		if (exportOptions.WhenExportingHonorGemPrintingFlags && !IsGemGroupIncludedInPrintMesh(exportOptions.IsCenterGemAllowedForPrinting, exportOptions.IsSideGemAllowedForPrinting, exportOptions.IsAccentGemAllowedForPrinting))
		{
			return false;
		}
		if (transform != Rhino.Geometry.Transform.Unset && transform != Rhino.Geometry.Transform.Identity)
		{
			CSData cSData = Duplicate();
			(cSData as ICSTransformable)?.ApplyTransform(transform);
			if (cSData is ICSDocumentObject iCSDocumentObject)
			{
				return iCSDocumentObject.AddToDocumentExport(ref rhinoRenderObjects, Rhino.Geometry.Transform.Unset, exportOptions);
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
		ObjectAttributes objectAttributes = new ObjectAttributes
		{
			Name = "Gem",
			MaterialSource = ObjectMaterialSource.MaterialFromObject
		};
		if (base.CurrentMaterialGroup.IsDesignerMaterialGroup)
		{
			objectAttributes.AddToGroup(RhinoDoc.ActiveDoc.Groups.Find(base.CurrentMaterialGroup.Name, ignoreDeletedGroups: true));
		}
		Point3d origin = Point3d.Origin;
		Point3d point3d = new Point3d(1.0, 0.0, 0.0);
		Point3d point3d2 = new Point3d(0.0, 1.0, 0.0);
		Point3d point3d3 = new Point3d(0.0, 0.0, 1.0);
		origin.Transform(Xform);
		point3d.Transform(Xform);
		point3d2.Transform(Xform);
		point3d3.Transform(Xform);
		Vector3d vector3d = Vector3d.CrossProduct(point3d - origin, point3d2 - origin);
		using (Mesh mesh = Def.Mesh)
		{
			mesh.Transform(Xform);
			if (vector3d * (point3d3 - origin) < 0.0)
			{
				mesh.Flip(vertexNormals: true, faceNormals: true, faceOrientation: false);
			}
			rhinoRenderObjects.Add(new RhinoRenderObject(RhinoDoc.ActiveDoc.Objects.AddMesh(mesh, objectAttributes), materialGroup));
		}
		return true;
	}

	public bool GetMeshableObjects(ref ConcurrentBag<GeometryBase> meshableObjects, CSOptionsExport exportOptions)
	{
		if (!IsGemGroupIncludedInPrintMesh(exportOptions.IsCenterGemAllowedForPrinting, exportOptions.IsSideGemAllowedForPrinting, exportOptions.IsAccentGemAllowedForPrinting))
		{
			return true;
		}
		CSUtilMath.GetUnscaledXform(Xform, out var _);
		MRhinoObjectAttributes mRhinoObjectAttributes = new MRhinoObjectAttributes
		{
			m_name = "Gem"
		};
		mRhinoObjectAttributes.SetMaterialSource(IOn.object_material_source.material_from_object);
		if (base.CurrentMaterialGroup.IsDesignerMaterialGroup)
		{
			mRhinoObjectAttributes.AddToGroup(RhinoDoc.ActiveDoc.Groups.Find(base.CurrentMaterialGroup.Name, ignoreDeletedGroups: true));
		}
		Point3d point3d = new Point3d(0.0, 0.0, 0.0);
		Point3d point3d2 = new Point3d(1.0, 0.0, 0.0);
		Point3d point3d3 = new Point3d(0.0, 1.0, 0.0);
		Point3d point3d4 = new Point3d(0.0, 0.0, 1.0);
		point3d.Transform(Xform);
		point3d2.Transform(Xform);
		point3d3.Transform(Xform);
		point3d4.Transform(Xform);
		Vector3d vector3d = Vector3d.CrossProduct(point3d2 - point3d, point3d3 - point3d);
		using (Mesh mesh = Def.Mesh)
		{
			mesh.Transform(Xform);
			if (vector3d * (point3d4 - point3d) < 0.0)
			{
				mesh.Flip(vertexNormals: true, faceNormals: true, faceOrientation: false);
			}
			meshableObjects.Add(mesh.DuplicateMesh());
		}
		return true;
	}

	public override bool GetPoint(out Point3d value)
	{
		Point3d origin = Point3d.Origin;
		if (Xform.IsValid)
		{
			origin.Transform(Xform);
		}
		value = origin;
		return true;
	}

	public override bool HasPoint()
	{
		return true;
	}

	public bool AddToDocumentRender(ref ConcurrentBag<RhinoRenderObject> renderObjects, Rhino.Geometry.Transform transform)
	{
		if (transform != Rhino.Geometry.Transform.Unset && transform != Rhino.Geometry.Transform.Identity)
		{
			CSData cSData = Duplicate();
			(cSData as ICSTransformable)?.ApplyTransform(transform);
			cSData.CurrentMaterialGroup = base.CurrentMaterialGroup;
			if (cSData is ICSRenderObject iCSRenderObject)
			{
				return iCSRenderObject.AddToDocumentRender(ref renderObjects, Rhino.Geometry.Transform.Unset);
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
		ObjectAttributes attributes = new ObjectAttributes
		{
			MaterialSource = ObjectMaterialSource.MaterialFromObject
		};
		Point3d point3d = new Point3d(0.0, 0.0, 0.0);
		Point3d point3d2 = new Point3d(1.0, 0.0, 0.0);
		Point3d point3d3 = new Point3d(0.0, 1.0, 0.0);
		Point3d point3d4 = new Point3d(0.0, 0.0, 1.0);
		point3d.Transform(Xform);
		point3d2.Transform(Xform);
		point3d3.Transform(Xform);
		point3d4.Transform(Xform);
		Vector3d vector3d = Vector3d.CrossProduct(point3d2 - point3d, point3d3 - point3d);
		using (Mesh mesh = Def.Mesh)
		{
			mesh.Transform(Xform);
			if (vector3d * (point3d4 - point3d) < 0.0)
			{
				mesh.Flip(vertexNormals: true, faceNormals: true, faceOrientation: false);
			}
			renderObjects.Add(new RhinoRenderObject(RhinoDoc.ActiveDoc.Objects.AddMesh(mesh, attributes), base.CurrentMaterialGroup));
		}
		return true;
	}

	public bool GetTransform(out Rhino.Geometry.Transform value)
	{
		value = Xform;
		return Xform != Rhino.Geometry.Transform.Unset;
	}

	public virtual bool ApplyTransform(Rhino.Geometry.Transform transform)
	{
		if (!transform.IsValid)
		{
			return false;
		}
		Xform = (Xform.IsValid ? (transform * Xform) : transform);
		return true;
	}

	public override CSData Duplicate()
	{
		return new CSGemData(this);
	}

	public override bool GetBoundingBox(ref BoundingBox boundingBox)
	{
		if (Def == null)
		{
			return false;
		}
		BoundingBox boundingBox2 = BoundingBox.Unset;
		Def.GetBoundingBox(ref boundingBox2);
		if (!boundingBox2.IsValid)
		{
			return false;
		}
		boundingBox2.Transform(Xform);
		boundingBox = boundingBox2;
		return true;
	}

	public bool GetGemXformRC(out Rhino.Geometry.Transform xDown, out Rhino.Geometry.Transform xUp)
	{
		xUp = Rhino.Geometry.Transform.Identity;
		xDown = Rhino.Geometry.Transform.Identity;
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
		Mesh mesh = Def.Mesh;
		mesh.Transform(Xform);
		return mesh;
	}

	public Mesh GetMeshFromDefinition(Rhino.Geometry.Transform transform)
	{
		Mesh mesh = Def.Mesh;
		mesh.Transform(transform * Xform);
		return mesh;
	}

	public override PointCollection GetPolyline(double precision)
	{
		NurbsCurve nurbsCurve = GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return null;
		}
		BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: true);
		Rhino.Geometry.Transform xform = Rhino.Geometry.Transform.Rotation(RhinoMath.ToRadians(-90.0), Vector3d.XAxis, boundingBox.Center);
		if (boundingBox.Max.Z - boundingBox.Min.Z > 0.1)
		{
			nurbsCurve.Transform(xform);
		}
		double num = boundingBox.Max.X - boundingBox.Min.X;
		double num2 = boundingBox.Max.Y - boundingBox.Min.Y;
		double num3 = 0.0;
		double num4 = 0.0;
		if (boundingBox.Min.X < 0.0)
		{
			num3 -= boundingBox.Min.X;
		}
		if (boundingBox.Min.Y < 0.0)
		{
			num4 -= boundingBox.Min.Y;
		}
		double num5 = ((!(num > num2)) ? (20.0 / num2) : (20.0 / num));
		double num6 = (20.0 - num * num5) / 2.0;
		double num7 = (20.0 - num2 * num5) / 2.0;
		Rhino.Geometry.Transform transform = Rhino.Geometry.Transform.Translation(num3, num4, 0.0);
		Rhino.Geometry.Transform transform2 = Rhino.Geometry.Transform.Scale(Plane.WorldXY, num5, num5, num5);
		Rhino.Geometry.Transform transform3 = Rhino.Geometry.Transform.Translation(num6 + 2.5, num7 + 2.5, 0.0);
		nurbsCurve.Transform(transform3 * transform2 * transform);
		Polyline polyline = new Polyline();
		if (precision >= 1.0)
		{
			Interval domain = nurbsCurve.Domain;
			for (double num8 = 0.0; num8 <= 50.0; num8 += 1.0)
			{
				polyline.Add(nurbsCurve.PointAt(domain.ParameterAt(num8 / 50.0)));
			}
		}
		else
		{
			PolylineCurve polylineCurve = nurbsCurve.ToPolyline(5, 5, 3.14, 1.0, 1.0, precision, precision, precision * 20.0, keepStartPoint: false);
			if (polylineCurve != null)
			{
				for (int i = 0; i < polylineCurve.PointCount; i++)
				{
					Point3d item = polylineCurve.Point(i);
					polyline.Add(item);
				}
			}
		}
		PointCollection pointCollection = new PointCollection(polyline.Count);
		foreach (Point3d item2 in polyline)
		{
			pointCollection.Add(new System.Windows.Point(item2.X, 25.0 - item2.Y));
		}
		return pointCollection;
	}

	public override bool GetTightBoundingBox(ref BoundingBox boundingBox)
	{
		return TryGetBoundingBox(doAccurate: true, doGrow: false, Rhino.Geometry.Transform.Unset, ref boundingBox);
	}

	public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow)
	{
		return TryGetBoundingBox(doAccurate: true, doGrow, Rhino.Geometry.Transform.Unset, ref boundingBox);
	}

	public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow, Rhino.Geometry.Transform transform)
	{
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
		Mesh mesh = Def.Mesh;
		mesh.Transform(Xform);
		return mesh;
	}

	public override bool Morph(SpaceMorph spaceMorph)
	{
		if (spaceMorph == null)
		{
			return false;
		}
		return Xform.IsValid;
	}

	public override string Report()
	{
		return GemType + "," + GemShape;
	}

	public void Scale(double scaleFactorX, double scaleFactorY, double scaleFactorZ)
	{
		Rhino.Geometry.Transform transform = Rhino.Geometry.Transform.Scale(Plane.WorldXY, scaleFactorX, scaleFactorY, scaleFactorZ);
		Xform = transform * Xform;
	}

	public void Scale(double scaleFactorX, double scaleFactorY)
	{
		Rhino.Geometry.Transform transform = Rhino.Geometry.Transform.Scale(Plane.WorldXY, scaleFactorX, scaleFactorY, (scaleFactorX + scaleFactorY) / 2.0);
		Xform = transform * Xform;
	}

	public void Scale(double scaleFactorX)
	{
		Rhino.Geometry.Transform transform = Rhino.Geometry.Transform.Scale(Plane.WorldXY, scaleFactorX, scaleFactorX, (scaleFactorX + scaleFactorX) / 2.0);
		Xform = transform * Xform;
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
		if (Def != null)
		{
			double num = Math.Max(Math.Max(Def.MaxY, 0.0 - Def.MinY), Math.Max(Def.MaxX, 0.0 - Def.MinX));
			Rhino.Geometry.Transform transform = Rhino.Geometry.Transform.Scale(Point3d.Origin, size / 2.0 / num);
			Rhino.Geometry.Transform transform2 = Rhino.Geometry.Transform.ChangeBasis(new Plane(point, normal), new Plane(Point3d.Origin, Vector3d.ZAxis));
			Xform = transform2 * transform;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
			try
			{
				Xform = Rhino.Geometry.Transform.Unset;
			}
			finally
			{
				base.Dispose(disposing);
			}
			_isDisposed = true;
		}
	}

	private bool TryGetBoundingBox(bool doAccurate, bool doGrow, Rhino.Geometry.Transform transform, ref BoundingBox boundingBox)
	{
		Mesh mesh = Def?.Mesh;
		if (mesh == null)
		{
			return false;
		}
		Rhino.Geometry.Transform xform = ((!Xform.IsValid) ? (transform.IsValid ? transform : Rhino.Geometry.Transform.Unset) : (transform.IsValid ? (transform * Xform) : Xform));
		BoundingBox boundingBox2;
		if (xform.IsValid && !xform.Equals(Rhino.Geometry.Transform.Identity))
		{
			Mesh mesh2 = mesh.DuplicateMesh();
			mesh2.Transform(xform);
			boundingBox2 = mesh2.GetBoundingBox(accurate: true);
			mesh2.Dispose();
		}
		else
		{
			boundingBox2 = mesh.GetBoundingBox(doAccurate);
		}
		if (!boundingBox2.MakeValid())
		{
			return false;
		}
		if (boundingBox.IsValid && doGrow)
		{
			boundingBox.Union(boundingBox2);
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
