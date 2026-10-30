using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CSCompatibility;
using CSDatas.Interfaces;
using CSGemInstance;
using CSMaterials;
using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace CSDatas;

public sealed class CSOnGeometryData : CSData, ICSDisplayObject, ICSCurve, ICSDocumentObject, ICSMeshable, ICSPoint, ICSRenderObject, ICSTransform, ICSTransformable, ICSFlippable, ICSSurface, ICSBrep, ICSMesh
{
	private enum EDisplayType
	{
		Unknown,
		Nothing,
		Mesh,
		DisplayMesh,
		Curve,
		Brep
	}

	private static readonly object _parallelLockMorph = new object();

	private Mesh _displayMesh;

	private GeometryBase _geometry;

	private EDisplayType _whichDisplayType;

	private Rhino.Geometry.Transform _xform;

	public CSOnGeometryData(GeometryBase geometry)
	{
		ConstructorAux(geometry, Rhino.Geometry.Transform.Unset, applyTransform: false);
	}

	public CSOnGeometryData(GeometryBase geometry, Rhino.Geometry.Transform transform, bool applyTransform = true)
	{
		ConstructorAux(geometry, transform, applyTransform);
	}

	private CSOnGeometryData(CSOnGeometryData copy)
		: base(copy)
	{
		if (copy._isDisposed)
		{
			_xform = Rhino.Geometry.Transform.Unset;
		}
		if (copy._geometry != null)
		{
			_geometry = copy._geometry.Duplicate();
			string[] allKeys = copy._geometry.GetUserStrings().AllKeys;
			foreach (string key in allKeys)
			{
				string userString = copy._geometry.GetUserString(key);
				if (!string.IsNullOrEmpty(userString))
				{
					_geometry.SetUserString(key, userString);
				}
			}
		}
		if (copy._displayMesh != null)
		{
			_displayMesh = copy._displayMesh.DuplicateMesh();
		}
		_whichDisplayType = copy._whichDisplayType;
		_xform = ((copy._xform == Rhino.Geometry.Transform.Unset) ? Rhino.Geometry.Transform.Identity : copy._xform);
	}

	public override Brep GetBrep()
	{
		if (_geometry == null)
		{
			return null;
		}
		if (!_geometry.HasBrepForm)
		{
			return null;
		}
		if (_geometry is Brep brep)
		{
			return brep.DuplicateBrep();
		}
		return (_geometry as Surface)?.ToBrep();
	}

	public override Curve GetCurve()
	{
		return (_geometry as Curve)?.DuplicateCurve();
	}

	public override NurbsCurve GetNurbsCurve()
	{
		return (_geometry as Curve)?.ToNurbsCurve();
	}

	public override bool HasNurbsCurve()
	{
		Curve obj = _geometry as Curve;
		if (obj == null)
		{
			return false;
		}
		return obj.HasNurbsForm() > 0;
	}

	public bool AssignCurrentMaterialGroup(MaterialGroup materialGroup)
	{
		bool result = false;
		if (materialGroup.IsMetalMaterialGroup)
		{
			base.CurrentMaterialGroup = materialGroup;
			result = true;
		}
		if (!materialGroup.IsGemMaterialGroup)
		{
			base.CurrentMaterialGroup = materialGroup;
		}
		return result;
	}

	public bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial csDisplayMaterial = null)
	{
		if (_geometry == null)
		{
			return false;
		}
		if (csDisplayMaterial == null)
		{
			csDisplayMaterial = displayOptions.GetCSDisplayMaterialFromName(base.CurrentMaterialGroup.MaterialName);
		}
		System.Drawing.Color color = csDisplayMaterial?.Mat.Diffuse ?? System.Drawing.Color.Aqua;
		if (_whichDisplayType == EDisplayType.Unknown)
		{
			PreProcessDisplayInfo();
		}
		if (displayOptions.IsWireframeModeEnabled)
		{
			DrawWireframe(displayPipeline, color, displayOptions);
		}
		else
		{
			DrawNormal(displayPipeline, csDisplayMaterial?.Mat, color, displayOptions);
		}
		return true;
	}

	public void AddToDocument()
	{
		if (_geometry == null)
		{
			return;
		}
		if (_geometry is Curve curve)
		{
			RhinoDoc.ActiveDoc.Objects.AddCurve(curve);
		}
		else if (_geometry is Brep brep)
		{
			Brep brep2 = brep.Repair(0.01);
			CheckSolidOrientation(brep2);
			if (!(RhinoDoc.ActiveDoc.Objects.AddBrep(brep2) != Guid.Empty) && (_displayMesh != null || CalculateDisplayMesh()) && _displayMesh != null)
			{
				RhinoDoc.ActiveDoc.Objects.AddMesh(_displayMesh);
			}
		}
		else if (_geometry is Surface surface)
		{
			RhinoDoc.ActiveDoc.Objects.AddSurface(surface);
		}
		else if (_geometry is Mesh mesh)
		{
			CheckSolidOrientation(mesh);
			RhinoDoc.ActiveDoc.Objects.AddMesh(mesh);
		}
	}

	public bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Rhino.Geometry.Transform transform, CSOptionsExport exportOptions)
	{
		if (transform != Rhino.Geometry.Transform.Unset && transform != Rhino.Geometry.Transform.Identity)
		{
			CSData cSData = Duplicate();
			(cSData as ICSTransformable)?.ApplyTransform(transform);
			if (cSData is ICSDocumentObject iCSDocumentObject)
			{
				return iCSDocumentObject.AddToDocumentExport(ref rhinoRenderObjects, Rhino.Geometry.Transform.Unset, exportOptions);
			}
		}
		if (_geometry == null)
		{
			return false;
		}
		ObjectAttributes attributes = new ObjectAttributes
		{
			MaterialSource = ObjectMaterialSource.MaterialFromObject
		};
		if (_geometry is Curve curve)
		{
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddCurve(curve, attributes);
			return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
		}
		if (_geometry is Brep brep)
		{
			Brep brep2 = brep.Repair(0.01);
			CheckSolidOrientation(brep2);
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddBrep(brep2, attributes);
			if (guid != Guid.Empty)
			{
				return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
			}
			if (_displayMesh == null)
			{
				CalculateDisplayMesh();
			}
			if (_displayMesh != null)
			{
				guid = RhinoDoc.ActiveDoc.Objects.AddMesh(_displayMesh, attributes);
				return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
			}
		}
		if (_geometry is NurbsSurface surface)
		{
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddSurface(surface, attributes);
			return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
		}
		if (_geometry is Surface surface2)
		{
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddSurface(surface2, attributes);
			return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
		}
		if (_geometry is Mesh mesh)
		{
			CheckSolidOrientation(mesh);
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddMesh(mesh, attributes);
			return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
		}
		return false;
	}

	public void Flip()
	{
		if (_geometry is Mesh mesh)
		{
			mesh.Flip(vertexNormals: true, faceNormals: true, faceOrientation: true);
		}
		else
		{
			(_geometry as Brep)?.Flip();
		}
	}

	public override Mesh GetMesh()
	{
		if (_geometry is Mesh mesh)
		{
			return mesh.DuplicateMesh();
		}
		return base.GetMesh();
	}

	public bool GetMeshableObjects(ref ConcurrentBag<GeometryBase> meshableObjects, CSOptionsExport exportOptions)
	{
		if (_geometry == null)
		{
			return false;
		}
		if (_geometry is Curve item)
		{
			meshableObjects.Add(item);
			return true;
		}
		if (_geometry is Brep item2)
		{
			meshableObjects.Add(item2);
			return true;
		}
		if (_geometry is Surface item3)
		{
			meshableObjects.Add(item3);
			return true;
		}
		if (_geometry is Mesh item4)
		{
			meshableObjects.Add(item4);
			return true;
		}
		return false;
	}

	public override bool GetPoint(out Point3d point)
	{
		point = Point3d.Unset;
		if (!(_geometry is Curve curve))
		{
			return false;
		}
		point = curve.PointAtStart;
		return true;
	}

	public override bool HasPoint()
	{
		return _geometry is Curve;
	}

	public bool AddToDocumentRender(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Rhino.Geometry.Transform transform)
	{
		if (transform != Rhino.Geometry.Transform.Unset && transform != Rhino.Geometry.Transform.Identity)
		{
			CSData cSData = Duplicate();
			(cSData as ICSTransformable)?.ApplyTransform(transform);
			cSData.CurrentMaterialGroup = base.CurrentMaterialGroup;
			if (cSData is ICSRenderObject iCSRenderObject)
			{
				return iCSRenderObject.AddToDocumentRender(ref rhinoRenderObjects, Rhino.Geometry.Transform.Unset);
			}
		}
		if (_geometry == null)
		{
			return false;
		}
		ObjectAttributes attributes = new ObjectAttributes
		{
			MaterialSource = ObjectMaterialSource.MaterialFromObject
		};
		if (_geometry is Brep brep)
		{
			Brep brep2 = brep.Repair(0.01);
			CheckSolidOrientation(brep2);
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddBrep(brep2, attributes);
			if (guid != Guid.Empty)
			{
				return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
			}
			if (_displayMesh == null && !CalculateDisplayMesh())
			{
				return false;
			}
			if (_displayMesh != null)
			{
				guid = RhinoDoc.ActiveDoc.Objects.AddMesh(_displayMesh, attributes);
			}
			return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
		}
		if (_geometry is Surface surface)
		{
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddSurface(surface, attributes);
			return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
		}
		if (_geometry is Mesh mesh)
		{
			CheckSolidOrientation(mesh);
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddMesh(mesh, attributes);
			return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
		}
		return false;
	}

	public override Surface GetSurface()
	{
		if (_geometry == null)
		{
			return null;
		}
		if (_geometry is NurbsSurface other)
		{
			return new NurbsSurface(other);
		}
		if (_geometry is Surface surface)
		{
			return surface.ToNurbsSurface();
		}
		if (!(_geometry is Brep brep) || brep.Faces.Count <= 0)
		{
			return null;
		}
		return brep.Faces[0].DuplicateSurface();
	}

	public override NurbsSurface GetNurbsSurface()
	{
		if (_geometry == null)
		{
			return null;
		}
		if (_geometry is NurbsSurface other)
		{
			return new NurbsSurface(other);
		}
		if (_geometry is Surface surface)
		{
			return surface.ToNurbsSurface();
		}
		if (!(_geometry is Brep brep) || brep.Faces.Count <= 0)
		{
			return null;
		}
		return brep.Faces[0].ToNurbsSurface();
	}

	public override bool HasNurbsSurface()
	{
		if (_geometry == null)
		{
			return false;
		}
		if (_geometry is NurbsSurface)
		{
			return true;
		}
		Surface obj = _geometry as Surface;
		if (obj == null)
		{
			return false;
		}
		return obj.HasNurbsForm() > 0;
	}

	public bool GetTransform(out Rhino.Geometry.Transform value)
	{
		value = _xform;
		return _xform.IsValid;
	}

	public bool ApplyTransform(Rhino.Geometry.Transform transform)
	{
		if (_geometry == null)
		{
			return false;
		}
		TransformOnGeometry(transform);
		_displayMesh?.Transform(transform);
		_xform = (_xform.IsValid ? (_xform * transform) : transform);
		return true;
	}

	public static void CheckSolidOrientation(Brep brep)
	{
		if (brep == null)
		{
			return;
		}
		if (brep.IsSolid)
		{
			if (brep.SolidOrientation == BrepSolidOrientation.Inward)
			{
				brep.Flip();
			}
			return;
		}
		Brep brep2 = brep.CapPlanarHoles(CSUtil.Tolerance);
		if (brep2 != null && brep2.IsSolid)
		{
			if (brep2.SolidOrientation == BrepSolidOrientation.Inward)
			{
				brep2.Flip();
			}
			BrepFace brepFace = brep.Faces.First();
			double mid = brepFace.Domain(0).Mid;
			double mid2 = brepFace.Domain(1).Mid;
			Point3d testPoint = brepFace.PointAt(mid, mid2);
			Vector3d vector3d = brepFace.NormalAt(mid, mid2);
			if (brep2.ClosestPoint(testPoint, out var _, out var _, out var _, out var _, CSUtil.Tolerance, out var normal) && vector3d * normal < 0.0)
			{
				brep.Flip();
			}
		}
	}

	public static void CheckSolidOrientation(Mesh mesh)
	{
		if (mesh != null && mesh.SolidOrientation() == -1)
		{
			mesh.Flip(vertexNormals: true, faceNormals: true, faceOrientation: true);
		}
	}

	public override CSData Duplicate()
	{
		return new CSOnGeometryData(this);
	}

	public static CSOnGeometryData FromBrep(Brep brep)
	{
		if (brep != null)
		{
			return new CSOnGeometryData(brep);
		}
		return null;
	}

	public static CSOnGeometryData FromCurve(Curve curve)
	{
		if (curve != null)
		{
			return new CSOnGeometryData(curve);
		}
		return null;
	}

	public override bool GetAllMaterial(List<CSData> metal, Rhino.Geometry.Transform transform, MaterialClassification whicMaterialClassification, MaterialClassification searchMaterialClassification)
	{
		if (whicMaterialClassification == MaterialClassification.Undefined || whicMaterialClassification == MaterialClassification.UseOriginal)
		{
			whicMaterialClassification = (base.CurrentMaterialGroup.IsDefaultMaterialGroup ? base.CurrentMaterialGroup.Classification : base.DefaultDisplayMaterialClassification);
		}
		if (whicMaterialClassification != searchMaterialClassification)
		{
			return false;
		}
		CSData cSData = Duplicate();
		(cSData as ICSTransformable)?.ApplyTransform(transform);
		metal.Add(cSData);
		return true;
	}

	public override NurbsCurve GetCurveSegment(int segmentNumber, int segmentCount, bool fromStart)
	{
		NurbsCurve nurbsCurve = GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return null;
		}
		if (segmentNumber >= segmentCount)
		{
			return null;
		}
		if (segmentNumber < 0)
		{
			return null;
		}
		Curve curve = nurbsCurve.DuplicateCurve();
		if (!fromStart && nurbsCurve.IsClosed)
		{
			double s = ((double)segmentCount * 2.0 - 1.0) / ((double)segmentCount * 2.0);
			if (!curve.NormalizedLengthParameter(s, out var t))
			{
				return null;
			}
			curve.ChangeClosedCurveSeam(curve.Domain.NormalizedParameterAt(t));
		}
		double s2 = (double)segmentNumber / (double)segmentCount;
		double s3 = ((double)segmentNumber + 1.0) / (double)segmentCount;
		if (!curve.NormalizedLengthParameter(s2, out var t2))
		{
			return null;
		}
		if (!curve.NormalizedLengthParameter(s3, out var t3))
		{
			return null;
		}
		return curve.Trim(new Interval(t2, t3))?.ToNurbsCurve();
	}

	public override GeometryBase GetGeometry()
	{
		return _geometry;
	}

	public override PointCollection GetPolyline(double precision)
	{
		PointCollection pointCollection = null;
		NurbsCurve nurbsCurve = GetNurbsCurve();
		if (nurbsCurve != null)
		{
			BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: true);
			Rhino.Geometry.Transform objectDisplayXform = CSUtilMath.GetObjectDisplayXform(boundingBox);
			nurbsCurve.Transform(objectDisplayXform);
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
			pointCollection = new PointCollection(polyline.Count);
			foreach (Point3d item2 in polyline)
			{
				pointCollection.Add(new System.Windows.Point(item2.X, 25.0 - item2.Y));
			}
		}
		return pointCollection;
	}

	public override bool GetUserString(string key, ref string value)
	{
		if (_geometry == null)
		{
			return false;
		}
		if (!_geometry.GetUserStrings().AllKeys.Contains(key))
		{
			return false;
		}
		value = _geometry.GetUserString(key);
		return true;
	}

	public override bool HasBrep()
	{
		return _geometry is Brep;
	}

	public override bool HasMesh()
	{
		return _geometry is Mesh;
	}

	public override Mesh MeshObject(MeshingParameters mp, bool productionMeshes = false, bool useDisplayMesh = true)
	{
		if (_geometry == null)
		{
			return null;
		}
		if (productionMeshes)
		{
			mp = RhinoDoc.ActiveDoc.GetMeshingParameters(MeshingParameterStyle.Quality);
		}
		else if (_displayMesh != null && useDisplayMesh)
		{
			return _displayMesh.DuplicateMesh();
		}
		if (_geometry is Brep brep)
		{
			if (!brep.IsValid)
			{
				return null;
			}
			Mesh[] array = Mesh.CreateFromBrep(brep, mp);
			if (array == null)
			{
				return null;
			}
			if (array.Length == 0)
			{
				return null;
			}
			Mesh mesh = new Mesh();
			Mesh[] array2 = array;
			foreach (Mesh mesh2 in array2)
			{
				if (mesh2 != null)
				{
					mesh.Append(mesh2);
					mesh2.Dispose();
				}
			}
			return mesh;
		}
		if (_geometry is Surface surface)
		{
			if (!surface.IsValid)
			{
				return null;
			}
			Brep brep2 = surface.ToBrep();
			if (!brep2.IsValid)
			{
				brep2.Dispose();
				return null;
			}
			Mesh[] array3 = Mesh.CreateFromBrep(brep2, mp);
			brep2.Dispose();
			if (array3 == null)
			{
				return null;
			}
			if (array3.Length == 0)
			{
				return null;
			}
			Mesh mesh3 = new Mesh();
			Mesh[] array2 = array3;
			foreach (Mesh mesh4 in array2)
			{
				if (mesh4 != null)
				{
					mesh3.Append(mesh4);
				}
			}
			array2 = array3;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Dispose();
			}
			return mesh3;
		}
		return (_geometry as Mesh)?.DuplicateMesh();
	}

	public override bool Morph(SpaceMorph spaceMorph)
	{
		if (_geometry == null)
		{
			return false;
		}
		_displayMesh = null;
		_whichDisplayType = EDisplayType.Unknown;
		string[] allKeys = _geometry.GetUserStrings().AllKeys;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string[] array = allKeys;
		foreach (string key in array)
		{
			string userString = _geometry.GetUserString(key);
			if (!string.IsNullOrWhiteSpace(userString) && !dictionary.ContainsKey(key))
			{
				dictionary.Add(key, userString);
			}
		}
		_geometry.MakeDeformable();
		bool flag;
		lock (_parallelLockMorph)
		{
			flag = spaceMorph.Morph(_geometry);
			if (!flag)
			{
				Brep brep = null;
				if (_geometry is Extrusion extrusion)
				{
					brep = extrusion.ToBrep();
				}
				if (brep != null)
				{
					flag = spaceMorph.Morph(brep);
					if (flag)
					{
						_geometry = brep;
					}
				}
			}
		}
		(_geometry as Mesh)?.Normals.ComputeNormals();
		_geometry.GetUserStrings().Clear();
		foreach (KeyValuePair<string, string> item in dictionary)
		{
			_geometry.SetUserString(item.Key, item.Value);
		}
		return flag;
	}

	public void PreProcessDisplayInfo()
	{
		if (_geometry is Mesh displayMesh)
		{
			_whichDisplayType = EDisplayType.Mesh;
			_displayMesh = displayMesh;
		}
		else if (CalculateDisplayMesh())
		{
			_whichDisplayType = EDisplayType.DisplayMesh;
		}
		else if (HasBrep())
		{
			_whichDisplayType = EDisplayType.Brep;
		}
		else if (HasNurbsCurve())
		{
			_whichDisplayType = EDisplayType.Curve;
		}
		else
		{
			_whichDisplayType = EDisplayType.Nothing;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
			try
			{
				_displayMesh?.Dispose();
				_geometry?.Dispose();
				_displayMesh = null;
				_geometry = null;
			}
			finally
			{
				base.Dispose(disposing);
			}
			_isDisposed = true;
		}
	}

	protected override Image GetImage(int sizeX, int sizeY, System.Drawing.Color color, System.Drawing.Color background)
	{
		Image image = base.GetImage(sizeX, sizeY, color, background);
		if (image != null)
		{
			return image;
		}
		NurbsCurve nurbsCurve = GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return null;
		}
		Bitmap bitmap = new Bitmap(sizeX, sizeY);
		using Graphics graphics = Graphics.FromImage(bitmap);
		graphics.Clear(background);
		BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: false);
		Rhino.Geometry.Transform objectDisplayXform = CSUtilMath.GetObjectDisplayXform(boundingBox);
		nurbsCurve.Transform(objectDisplayXform);
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
		double num5 = (double)sizeX * 0.8;
		double num6 = (double)sizeY * 0.8;
		double num7 = ((!(num5 < num6)) ? (((double)sizeY - num6) / 2.0) : (((double)sizeX - num5) / 2.0));
		double num8 = ((num > num2) ? ((!(num5 > num6)) ? (num6 / num) : (num5 / num)) : ((!(num5 > num6)) ? (num6 / num2) : (num5 / num2)));
		double num9 = (num5 - num * num8) / 2.0;
		double num10 = (num6 - num2 * num8) / 2.0;
		graphics.TranslateTransform(0f, sizeY);
		graphics.ScaleTransform(1f, -1f);
		Polyline polyline = new Polyline();
		if (sizeX < 100 && sizeY < 100)
		{
			Interval domain = nurbsCurve.Domain;
			for (double num11 = 0.0; num11 <= 50.0; num11 += 1.0)
			{
				polyline.Add(nurbsCurve.PointAt(domain.ParameterAt(num11 / 50.0)));
			}
		}
		else
		{
			PolylineCurve polylineCurve = nurbsCurve.ToPolyline(20, 5, 5.0, 1.0, 1.0, 0.1, 0.1, 1.0, keepStartPoint: false);
			if (polylineCurve != null)
			{
				for (int i = 0; i < polylineCurve.PointCount; i++)
				{
					Point3d item = polylineCurve.Point(i);
					polyline.Add(item);
				}
			}
		}
		System.Drawing.Pen pen = new System.Drawing.Pen(color, (float)(1.0 * (0.0 - num8)));
		for (int j = 1; j < polyline.Count; j++)
		{
			Point3d point3d = polyline[j];
			Point3d point3d2 = polyline[j - 1];
			float x = (float)((point3d.X + num3) * num8 + num9 + num7);
			float y = (float)((point3d.Y + num4) * num8 + num10 + num7);
			float x2 = (float)((point3d2.X + num3) * num8 + num9 + num7);
			float y2 = (float)((point3d2.Y + num4) * num8 + num10 + num7);
			graphics.DrawLine(pen, x, y, x2, y2);
		}
		graphics.Flush();
		return bitmap;
	}

	private bool CalculateDisplayMesh()
	{
		if (_displayMesh != null)
		{
			return true;
		}
		MeshingParameters coarse = MeshingParameters.Coarse;
		coarse.GridMinCount = 16;
		Mesh mesh = MeshObject(coarse);
		if (mesh == null)
		{
			return false;
		}
		if (!mesh.IsValid)
		{
			return false;
		}
		_displayMesh = mesh;
		return true;
	}

	private void ConstructorAux(GeometryBase geometry, Rhino.Geometry.Transform transform, bool applyTransform)
	{
		if (geometry != null)
		{
			_geometry = geometry.Duplicate();
			string[] allKeys = geometry.GetUserStrings().AllKeys;
			foreach (string key in allKeys)
			{
				string userString = geometry.GetUserString(key);
				if (!string.IsNullOrEmpty(userString))
				{
					_geometry.SetUserString(key, userString);
				}
			}
		}
		if (transform != Rhino.Geometry.Transform.Unset)
		{
			_xform = transform;
			if (applyTransform)
			{
				TransformOnGeometry(transform);
			}
		}
		else
		{
			_xform = Rhino.Geometry.Transform.Identity;
		}
	}

	private void DrawNormal(DisplayPipeline dp, DisplayMaterial mat, System.Drawing.Color color, CSOptionsDisplay displayOptions)
	{
		bool doShowCurves = displayOptions.DoShowCurves;
		bool isMouseDown = displayOptions.IsMouseDown;
		bool isSelectionDisplayModeEnabled = displayOptions.IsSelectionDisplayModeEnabled;
		switch (_whichDisplayType)
		{
		case EDisplayType.DisplayMesh:
			dp.DrawMeshShaded(_displayMesh, mat);
			break;
		case EDisplayType.Mesh:
			if (_geometry is Mesh mesh)
			{
				dp.DrawMeshShaded(mesh, mat);
			}
			break;
		case EDisplayType.Curve:
		{
			if (!(doShowCurves || isMouseDown || isSelectionDisplayModeEnabled))
			{
				break;
			}
			Curve curve = GetCurve();
			if (curve != null)
			{
				dp.DrawCurve(curve, color);
				if (displayOptions.DoShowStarts)
				{
					dp.DrawLine(curve.PointAtStart, curve.PointAtStart + curve.TangentAt(curve.Domain.Min), System.Drawing.Color.White);
					dp.DrawLine(curve.PointAtStart, curve.PointAtStart + Vector3d.ZAxis, System.Drawing.Color.White);
				}
			}
			break;
		}
		case EDisplayType.Brep:
			if (_geometry is Brep brep)
			{
				dp.DrawBrepShaded(brep, mat);
			}
			break;
		}
	}

	private void DrawWireframe(DisplayPipeline dp, System.Drawing.Color color, CSOptionsDisplay displayOptions)
	{
		bool doShowCurves = displayOptions.DoShowCurves;
		bool isMouseDown = displayOptions.IsMouseDown;
		bool isSelectionDisplayModeEnabled = displayOptions.IsSelectionDisplayModeEnabled;
		switch (_whichDisplayType)
		{
		case EDisplayType.DisplayMesh:
			dp.DrawMeshWires(_displayMesh, color);
			break;
		case EDisplayType.Mesh:
			if (_geometry is Mesh mesh)
			{
				dp.DrawMeshWires(mesh, color);
			}
			break;
		case EDisplayType.Curve:
		{
			if (!(doShowCurves || isMouseDown || isSelectionDisplayModeEnabled))
			{
				break;
			}
			Curve curve = GetCurve();
			if (curve != null)
			{
				dp.DrawCurve(curve, color);
				if (displayOptions.DoShowStarts)
				{
					dp.DrawLine(curve.PointAtStart, curve.PointAtStart + curve.TangentAt(curve.Domain.Min), color);
					dp.DrawLine(curve.PointAtStart, curve.PointAtStart + Vector3d.ZAxis, color);
				}
			}
			break;
		}
		case EDisplayType.Brep:
			if (_geometry is Brep brep)
			{
				dp.DrawBrepWires(brep, color);
			}
			break;
		}
	}

	private void TransformOnGeometry(Rhino.Geometry.Transform transform)
	{
		if (_geometry != null)
		{
			if (!_geometry.MakeDeformable() && _geometry is Curve curve && HasNurbsCurve())
			{
				_geometry = curve.ToNurbsCurve();
			}
			_geometry.Transform(transform);
		}
	}

	private bool TryAddRhinoRenderObject(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Guid guid)
	{
		if (guid == Guid.Empty)
		{
			return false;
		}
		rhinoRenderObjects.Add(new RhinoRenderObject(guid, base.CurrentMaterialGroup));
		return true;
	}

	~CSOnGeometryData()
	{
		Dispose(disposing: false);
	}

	public override bool GetBoundingBox(ref BoundingBox boundingBox)
	{
		if (_geometry == null)
		{
			return false;
		}
		if (!(_geometry is Brep))
		{
			return TryGetBoundingBox(doAccurate: false, doGrow: false, Rhino.Geometry.Transform.Unset, ref boundingBox);
		}
		CalculateDisplayMesh();
		if (_displayMesh != null)
		{
			boundingBox = _displayMesh.GetBoundingBox(accurate: true);
			return true;
		}
		return TryGetBoundingBox(doAccurate: false, doGrow: false, Rhino.Geometry.Transform.Unset, ref boundingBox);
	}

	[Obsolete]
	public override bool GetTightBoundingBox(ref BoundingBox boundingBox)
	{
		return GetTightBoundingBox(ref boundingBox, doGrow: false, Rhino.Geometry.Transform.Unset);
	}

	[Obsolete]
	public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow)
	{
		return GetTightBoundingBox(ref boundingBox, doGrow, Rhino.Geometry.Transform.Unset);
	}

	[Obsolete]
	public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow, Rhino.Geometry.Transform transform)
	{
		if (_geometry == null)
		{
			return false;
		}
		if (!(_geometry is Brep))
		{
			return TryGetBoundingBox(doAccurate: true, doGrow, transform, ref boundingBox);
		}
		return TryGetBoundingBox(doAccurate: false, doGrow, transform, ref boundingBox);
	}

	public override bool GetTightBoundingBoxNew(ref BoundingBox boundingBox)
	{
		return GetTightBoundingBoxNew(ref boundingBox, doGrow: false, Rhino.Geometry.Transform.Unset);
	}

	public override bool GetTightBoundingBoxNew(ref BoundingBox boundingBox, bool doGrow)
	{
		return GetTightBoundingBoxNew(ref boundingBox, doGrow, Rhino.Geometry.Transform.Unset);
	}

	public override bool GetTightBoundingBoxNew(ref BoundingBox boundingBox, bool doGrow, Rhino.Geometry.Transform transform)
	{
		if (_geometry == null)
		{
			return false;
		}
		return TryGetBoundingBox(doAccurate: true, doGrow, transform, ref boundingBox);
	}

	private bool TryGetBoundingBox(bool doAccurate, bool doGrow, Rhino.Geometry.Transform transform, ref BoundingBox boundingBox)
	{
		if (_geometry == null)
		{
			return false;
		}
		BoundingBox boundingBox2;
		if (transform.IsValid && !transform.Equals(Rhino.Geometry.Transform.Identity))
		{
			using GeometryBase geometryBase = _geometry.Duplicate();
			geometryBase.Transform(transform);
			boundingBox2 = geometryBase.GetBoundingBox(doAccurate);
		}
		else
		{
			boundingBox2 = _geometry.GetBoundingBox(doAccurate);
		}
		if (!boundingBox2.IsValid)
		{
			return false;
		}
		if (doGrow)
		{
			boundingBox.Union(boundingBox2);
		}
		else
		{
			boundingBox = boundingBox2;
		}
		return true;
	}
}
