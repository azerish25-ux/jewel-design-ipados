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
using Rhino.Collections;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Runtime;

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

	private Transform _xform;

	public CSOnGeometryData(GeometryBase geometry)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		ConstructorAux(geometry, Transform.Unset, applyTransform: false);
	}

	public CSOnGeometryData(GeometryBase geometry, Transform transform, bool applyTransform = true)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		ConstructorAux(geometry, transform, applyTransform);
	}

	private CSOnGeometryData(CSOnGeometryData copy)
		: base(copy)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		if (copy._isDisposed)
		{
			_xform = Transform.Unset;
		}
		if (copy._geometry != null)
		{
			_geometry = copy._geometry.Duplicate();
			string[] allKeys = copy._geometry.GetUserStrings().AllKeys;
			foreach (string text in allKeys)
			{
				string userString = copy._geometry.GetUserString(text);
				if (!string.IsNullOrEmpty(userString))
				{
					_geometry.SetUserString(text, userString);
				}
			}
		}
		if (copy._displayMesh != null)
		{
			_displayMesh = copy._displayMesh.DuplicateMesh();
		}
		_whichDisplayType = copy._whichDisplayType;
		_xform = ((copy._xform == Transform.Unset) ? Transform.Identity : copy._xform);
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
		GeometryBase geometry = _geometry;
		Brep val = (Brep)(object)((geometry is Brep) ? geometry : null);
		if (val != null)
		{
			return val.DuplicateBrep();
		}
		GeometryBase geometry2 = _geometry;
		GeometryBase obj = ((geometry2 is Surface) ? geometry2 : null);
		if (obj == null)
		{
			return null;
		}
		return ((Surface)obj).ToBrep();
	}

	public override Curve GetCurve()
	{
		GeometryBase geometry = _geometry;
		GeometryBase obj = ((geometry is Curve) ? geometry : null);
		if (obj == null)
		{
			return null;
		}
		return ((Curve)obj).DuplicateCurve();
	}

	public override NurbsCurve GetNurbsCurve()
	{
		GeometryBase geometry = _geometry;
		GeometryBase obj = ((geometry is Curve) ? geometry : null);
		if (obj == null)
		{
			return null;
		}
		return ((Curve)obj).ToNurbsCurve();
	}

	public override bool HasNurbsCurve()
	{
		GeometryBase geometry = _geometry;
		GeometryBase obj = ((geometry is Curve) ? geometry : null);
		if (obj == null)
		{
			return false;
		}
		return ((Curve)obj).HasNurbsForm() > 0;
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
		System.Drawing.Color color = ((csDisplayMaterial != null) ? csDisplayMaterial.Mat.Diffuse : System.Drawing.Color.Aqua);
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
		GeometryBase geometry = _geometry;
		Curve val = (Curve)(object)((geometry is Curve) ? geometry : null);
		if (val != null)
		{
			RhinoDoc.ActiveDoc.Objects.AddCurve(val);
			return;
		}
		GeometryBase geometry2 = _geometry;
		Brep val2 = (Brep)(object)((geometry2 is Brep) ? geometry2 : null);
		if (val2 != null)
		{
			Brep val3 = val2.Repair(0.01);
			CheckSolidOrientation(val3);
			if (!(RhinoDoc.ActiveDoc.Objects.AddBrep(val3) != Guid.Empty) && (_displayMesh != null || CalculateDisplayMesh()) && _displayMesh != null)
			{
				RhinoDoc.ActiveDoc.Objects.AddMesh(_displayMesh);
			}
			return;
		}
		GeometryBase geometry3 = _geometry;
		Surface val4 = (Surface)(object)((geometry3 is Surface) ? geometry3 : null);
		if (val4 != null)
		{
			RhinoDoc.ActiveDoc.Objects.AddSurface(val4);
			return;
		}
		GeometryBase geometry4 = _geometry;
		Mesh val5 = (Mesh)(object)((geometry4 is Mesh) ? geometry4 : null);
		if (val5 != null)
		{
			CheckSolidOrientation(val5);
			RhinoDoc.ActiveDoc.Objects.AddMesh(val5);
		}
	}

	public bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Transform transform, CSOptionsExport exportOptions)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (transform != Transform.Unset && transform != Transform.Identity)
		{
			CSData cSData = Duplicate();
			(cSData as ICSTransformable)?.ApplyTransform(transform);
			if (cSData is ICSDocumentObject iCSDocumentObject)
			{
				return iCSDocumentObject.AddToDocumentExport(ref rhinoRenderObjects, Transform.Unset, exportOptions);
			}
		}
		if (_geometry == null)
		{
			return false;
		}
		ObjectAttributes val = new ObjectAttributes
		{
			MaterialSource = (ObjectMaterialSource)1
		};
		GeometryBase geometry = _geometry;
		Curve val2 = (Curve)(object)((geometry is Curve) ? geometry : null);
		if (val2 != null)
		{
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddCurve(val2, val);
			return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
		}
		GeometryBase geometry2 = _geometry;
		Brep val3 = (Brep)(object)((geometry2 is Brep) ? geometry2 : null);
		if (val3 != null)
		{
			Brep val4 = val3.Repair(0.01);
			CheckSolidOrientation(val4);
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddBrep(val4, val);
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
				guid = RhinoDoc.ActiveDoc.Objects.AddMesh(_displayMesh, val);
				return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
			}
		}
		GeometryBase geometry3 = _geometry;
		NurbsSurface val5 = (NurbsSurface)(object)((geometry3 is NurbsSurface) ? geometry3 : null);
		if (val5 != null)
		{
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddSurface((Surface)(object)val5, val);
			return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
		}
		GeometryBase geometry4 = _geometry;
		Surface val6 = (Surface)(object)((geometry4 is Surface) ? geometry4 : null);
		if (val6 != null)
		{
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddSurface(val6, val);
			return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
		}
		GeometryBase geometry5 = _geometry;
		Mesh val7 = (Mesh)(object)((geometry5 is Mesh) ? geometry5 : null);
		if (val7 != null)
		{
			CheckSolidOrientation(val7);
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddMesh(val7, val);
			return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
		}
		return false;
	}

	public void Flip()
	{
		GeometryBase geometry = _geometry;
		Mesh val = (Mesh)(object)((geometry is Mesh) ? geometry : null);
		if (val != null)
		{
			val.Flip(true, true, true);
			return;
		}
		GeometryBase geometry2 = _geometry;
		GeometryBase obj = ((geometry2 is Brep) ? geometry2 : null);
		if (obj != null)
		{
			((Brep)obj).Flip();
		}
	}

	public override Mesh GetMesh()
	{
		GeometryBase geometry = _geometry;
		Mesh val = (Mesh)(object)((geometry is Mesh) ? geometry : null);
		if (val != null)
		{
			return val.DuplicateMesh();
		}
		return base.GetMesh();
	}

	public bool GetMeshableObjects(ref ConcurrentBag<GeometryBase> meshableObjects, CSOptionsExport exportOptions)
	{
		if (_geometry == null)
		{
			return false;
		}
		GeometryBase geometry = _geometry;
		Curve val = (Curve)(object)((geometry is Curve) ? geometry : null);
		if (val != null)
		{
			meshableObjects.Add((GeometryBase)(object)val);
			return true;
		}
		GeometryBase geometry2 = _geometry;
		Brep val2 = (Brep)(object)((geometry2 is Brep) ? geometry2 : null);
		if (val2 != null)
		{
			meshableObjects.Add((GeometryBase)(object)val2);
			return true;
		}
		GeometryBase geometry3 = _geometry;
		Surface val3 = (Surface)(object)((geometry3 is Surface) ? geometry3 : null);
		if (val3 != null)
		{
			meshableObjects.Add((GeometryBase)(object)val3);
			return true;
		}
		GeometryBase geometry4 = _geometry;
		Mesh val4 = (Mesh)(object)((geometry4 is Mesh) ? geometry4 : null);
		if (val4 != null)
		{
			meshableObjects.Add((GeometryBase)(object)val4);
			return true;
		}
		return false;
	}

	public override bool GetPoint(out Point3d point)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		point = Point3d.Unset;
		GeometryBase geometry = _geometry;
		Curve val = (Curve)(object)((geometry is Curve) ? geometry : null);
		if (val == null)
		{
			return false;
		}
		point = val.PointAtStart;
		return true;
	}

	public override bool HasPoint()
	{
		return _geometry is Curve;
	}

	public bool AddToDocumentRender(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Transform transform)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (transform != Transform.Unset && transform != Transform.Identity)
		{
			CSData cSData = Duplicate();
			(cSData as ICSTransformable)?.ApplyTransform(transform);
			cSData.CurrentMaterialGroup = base.CurrentMaterialGroup;
			if (cSData is ICSRenderObject iCSRenderObject)
			{
				return iCSRenderObject.AddToDocumentRender(ref rhinoRenderObjects, Transform.Unset);
			}
		}
		if (_geometry == null)
		{
			return false;
		}
		ObjectAttributes val = new ObjectAttributes
		{
			MaterialSource = (ObjectMaterialSource)1
		};
		GeometryBase geometry = _geometry;
		Brep val2 = (Brep)(object)((geometry is Brep) ? geometry : null);
		if (val2 != null)
		{
			Brep val3 = val2.Repair(0.01);
			CheckSolidOrientation(val3);
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddBrep(val3, val);
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
				guid = RhinoDoc.ActiveDoc.Objects.AddMesh(_displayMesh, val);
			}
			return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
		}
		GeometryBase geometry2 = _geometry;
		Surface val4 = (Surface)(object)((geometry2 is Surface) ? geometry2 : null);
		if (val4 != null)
		{
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddSurface(val4, val);
			return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
		}
		GeometryBase geometry3 = _geometry;
		Mesh val5 = (Mesh)(object)((geometry3 is Mesh) ? geometry3 : null);
		if (val5 != null)
		{
			CheckSolidOrientation(val5);
			Guid guid = RhinoDoc.ActiveDoc.Objects.AddMesh(val5, val);
			return TryAddRhinoRenderObject(ref rhinoRenderObjects, guid);
		}
		return false;
	}

	public override Surface GetSurface()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		if (_geometry == null)
		{
			return null;
		}
		GeometryBase geometry = _geometry;
		NurbsSurface val = (NurbsSurface)(object)((geometry is NurbsSurface) ? geometry : null);
		if (val != null)
		{
			return (Surface)new NurbsSurface(val);
		}
		GeometryBase geometry2 = _geometry;
		Surface val2 = (Surface)(object)((geometry2 is Surface) ? geometry2 : null);
		if (val2 != null)
		{
			return (Surface)(object)val2.ToNurbsSurface();
		}
		GeometryBase geometry3 = _geometry;
		Brep val3 = (Brep)(object)((geometry3 is Brep) ? geometry3 : null);
		if (val3 == null || val3.Faces.Count <= 0)
		{
			return null;
		}
		return val3.Faces[0].DuplicateSurface();
	}

	public override NurbsSurface GetNurbsSurface()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		if (_geometry == null)
		{
			return null;
		}
		GeometryBase geometry = _geometry;
		NurbsSurface val = (NurbsSurface)(object)((geometry is NurbsSurface) ? geometry : null);
		if (val != null)
		{
			return new NurbsSurface(val);
		}
		GeometryBase geometry2 = _geometry;
		Surface val2 = (Surface)(object)((geometry2 is Surface) ? geometry2 : null);
		if (val2 != null)
		{
			return val2.ToNurbsSurface();
		}
		GeometryBase geometry3 = _geometry;
		Brep val3 = (Brep)(object)((geometry3 is Brep) ? geometry3 : null);
		if (val3 == null || val3.Faces.Count <= 0)
		{
			return null;
		}
		return ((Surface)val3.Faces[0]).ToNurbsSurface();
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
		GeometryBase geometry = _geometry;
		GeometryBase obj = ((geometry is Surface) ? geometry : null);
		if (obj == null)
		{
			return false;
		}
		return ((Surface)obj).HasNurbsForm() > 0;
	}

	public bool GetTransform(out Transform value)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		value = _xform;
		return ((Transform)(ref _xform)).IsValid;
	}

	public bool ApplyTransform(Transform transform)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (_geometry == null)
		{
			return false;
		}
		TransformOnGeometry(transform);
		Mesh displayMesh = _displayMesh;
		if (displayMesh != null)
		{
			((GeometryBase)displayMesh).Transform(transform);
		}
		_xform = (((Transform)(ref _xform)).IsValid ? (_xform * transform) : transform);
		return true;
	}

	public static void CheckSolidOrientation(Brep brep)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		if (brep == null)
		{
			return;
		}
		if (brep.IsSolid)
		{
			if ((int)brep.SolidOrientation == -1)
			{
				brep.Flip();
			}
			return;
		}
		Brep val = brep.CapPlanarHoles(CSUtil.Tolerance);
		if (val != null && val.IsSolid)
		{
			if ((int)val.SolidOrientation == -1)
			{
				val.Flip();
			}
			BrepFace obj = ((IEnumerable<BrepFace>)brep.Faces).First();
			Interval val2 = ((Surface)obj).Domain(0);
			double mid = ((Interval)(ref val2)).Mid;
			val2 = ((Surface)obj).Domain(1);
			double mid2 = ((Interval)(ref val2)).Mid;
			Point3d val3 = ((Surface)obj).PointAt(mid, mid2);
			Vector3d val4 = ((Surface)obj).NormalAt(mid, mid2);
			Point3d val5 = default(Point3d);
			ComponentIndex val6 = default(ComponentIndex);
			double num = default(double);
			double num2 = default(double);
			Vector3d val7 = default(Vector3d);
			if (val.ClosestPoint(val3, ref val5, ref val6, ref num, ref num2, CSUtil.Tolerance, ref val7) && val4 * val7 < 0.0)
			{
				brep.Flip();
			}
		}
	}

	public static void CheckSolidOrientation(Mesh mesh)
	{
		if (mesh != null && mesh.SolidOrientation() == -1)
		{
			mesh.Flip(true, true, true);
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
			return new CSOnGeometryData((GeometryBase)(object)brep);
		}
		return null;
	}

	public static CSOnGeometryData FromCurve(Curve curve)
	{
		if (curve != null)
		{
			return new CSOnGeometryData((GeometryBase)(object)curve);
		}
		return null;
	}

	public override bool GetAllMaterial(List<CSData> metal, Transform transform, MaterialClassification whicMaterialClassification, MaterialClassification searchMaterialClassification)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
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
		Curve val = ((Curve)nurbsCurve).DuplicateCurve();
		if (!fromStart && ((Curve)nurbsCurve).IsClosed)
		{
			double num = ((double)segmentCount * 2.0 - 1.0) / ((double)segmentCount * 2.0);
			double num2 = default(double);
			if (!val.NormalizedLengthParameter(num, ref num2))
			{
				return null;
			}
			Interval domain = val.Domain;
			val.ChangeClosedCurveSeam(((Interval)(ref domain)).NormalizedParameterAt(num2));
		}
		double num3 = (double)segmentNumber / (double)segmentCount;
		double num4 = ((double)segmentNumber + 1.0) / (double)segmentCount;
		double num5 = default(double);
		if (!val.NormalizedLengthParameter(num3, ref num5))
		{
			return null;
		}
		double num6 = default(double);
		if (!val.NormalizedLengthParameter(num4, ref num6))
		{
			return null;
		}
		Curve obj = val.Trim(new Interval(num5, num6));
		if (obj == null)
		{
			return null;
		}
		return obj.ToNurbsCurve();
	}

	public override GeometryBase GetGeometry()
	{
		return _geometry;
	}

	public override PointCollection GetPolyline(double precision)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		PointCollection pointCollection = null;
		NurbsCurve nurbsCurve = GetNurbsCurve();
		if (nurbsCurve != null)
		{
			BoundingBox boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
			Transform objectDisplayXform = CSUtilMath.GetObjectDisplayXform(boundingBox);
			((GeometryBase)nurbsCurve).Transform(objectDisplayXform);
			Point3d val = ((BoundingBox)(ref boundingBox)).Max;
			double x = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num = x - ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Max;
			double y = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num2 = y - ((Point3d)(ref val)).Y;
			double num3 = 0.0;
			double num4 = 0.0;
			val = ((BoundingBox)(ref boundingBox)).Min;
			if (((Point3d)(ref val)).X < 0.0)
			{
				double num5 = num3;
				val = ((BoundingBox)(ref boundingBox)).Min;
				num3 = num5 - ((Point3d)(ref val)).X;
			}
			val = ((BoundingBox)(ref boundingBox)).Min;
			if (((Point3d)(ref val)).Y < 0.0)
			{
				double num6 = num4;
				val = ((BoundingBox)(ref boundingBox)).Min;
				num4 = num6 - ((Point3d)(ref val)).Y;
			}
			double num7 = ((!(num > num2)) ? (20.0 / num2) : (20.0 / num));
			double num8 = (20.0 - num * num7) / 2.0;
			double num9 = (20.0 - num2 * num7) / 2.0;
			Transform val2 = Transform.Translation(num3, num4, 0.0);
			Transform val3 = Transform.Scale(Plane.WorldXY, num7, num7, num7);
			Transform val4 = Transform.Translation(num8 + 2.5, num9 + 2.5, 0.0);
			((GeometryBase)nurbsCurve).Transform(val4 * val3 * val2);
			Polyline val5 = new Polyline();
			if (precision >= 1.0)
			{
				Interval domain = ((Curve)nurbsCurve).Domain;
				for (double num10 = 0.0; num10 <= 50.0; num10 += 1.0)
				{
					((RhinoList<Point3d>)(object)val5).Add(((Curve)nurbsCurve).PointAt(((Interval)(ref domain)).ParameterAt(num10 / 50.0)));
				}
			}
			else
			{
				PolylineCurve val6 = ((Curve)nurbsCurve).ToPolyline(5, 5, 3.14, 1.0, 1.0, precision, precision, precision * 20.0, false);
				if (val6 != null)
				{
					for (int i = 0; i < val6.PointCount; i++)
					{
						Point3d val7 = val6.Point(i);
						((RhinoList<Point3d>)(object)val5).Add(val7);
					}
				}
			}
			pointCollection = new PointCollection(((RhinoList<Point3d>)(object)val5).Count);
			foreach (Point3d item in (RhinoList<Point3d>)(object)val5)
			{
				Point3d current = item;
				pointCollection.Add(new System.Windows.Point(((Point3d)(ref current)).X, 25.0 - ((Point3d)(ref current)).Y));
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
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		if (_geometry == null)
		{
			return null;
		}
		if (productionMeshes)
		{
			mp = RhinoDoc.ActiveDoc.GetMeshingParameters((MeshingParameterStyle)2);
		}
		else if (_displayMesh != null && useDisplayMesh)
		{
			return _displayMesh.DuplicateMesh();
		}
		GeometryBase geometry = _geometry;
		Brep val = (Brep)(object)((geometry is Brep) ? geometry : null);
		if (val != null)
		{
			if (!((CommonObject)val).IsValid)
			{
				return null;
			}
			Mesh[] array = Mesh.CreateFromBrep(val, mp);
			if (array == null)
			{
				return null;
			}
			if (array.Length == 0)
			{
				return null;
			}
			Mesh val2 = new Mesh();
			Mesh[] array2 = array;
			foreach (Mesh val3 in array2)
			{
				if (val3 != null)
				{
					val2.Append(val3);
					((CommonObject)val3).Dispose();
				}
			}
			return val2;
		}
		GeometryBase geometry2 = _geometry;
		Surface val4 = (Surface)(object)((geometry2 is Surface) ? geometry2 : null);
		if (val4 != null)
		{
			if (!((CommonObject)val4).IsValid)
			{
				return null;
			}
			Brep val5 = val4.ToBrep();
			if (!((CommonObject)val5).IsValid)
			{
				((CommonObject)val5).Dispose();
				return null;
			}
			Mesh[] array3 = Mesh.CreateFromBrep(val5, mp);
			((CommonObject)val5).Dispose();
			if (array3 == null)
			{
				return null;
			}
			if (array3.Length == 0)
			{
				return null;
			}
			Mesh val6 = new Mesh();
			Mesh[] array2 = array3;
			foreach (Mesh val7 in array2)
			{
				if (val7 != null)
				{
					val6.Append(val7);
				}
			}
			array2 = array3;
			for (int i = 0; i < array2.Length; i++)
			{
				((CommonObject)array2[i]).Dispose();
			}
			return val6;
		}
		GeometryBase geometry3 = _geometry;
		GeometryBase obj = ((geometry3 is Mesh) ? geometry3 : null);
		if (obj == null)
		{
			return null;
		}
		return ((Mesh)obj).DuplicateMesh();
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
		foreach (string text in array)
		{
			string userString = _geometry.GetUserString(text);
			if (!string.IsNullOrWhiteSpace(userString) && !dictionary.ContainsKey(text))
			{
				dictionary.Add(text, userString);
			}
		}
		_geometry.MakeDeformable();
		bool flag;
		lock (_parallelLockMorph)
		{
			flag = spaceMorph.Morph(_geometry);
			if (!flag)
			{
				Brep val = null;
				GeometryBase geometry = _geometry;
				Extrusion val2 = (Extrusion)(object)((geometry is Extrusion) ? geometry : null);
				if (val2 != null)
				{
					val = ((Surface)val2).ToBrep();
				}
				if (val != null)
				{
					flag = spaceMorph.Morph((GeometryBase)(object)val);
					if (flag)
					{
						_geometry = (GeometryBase)(object)val;
					}
				}
			}
		}
		GeometryBase geometry2 = _geometry;
		GeometryBase obj = ((geometry2 is Mesh) ? geometry2 : null);
		if (obj != null)
		{
			((Mesh)obj).Normals.ComputeNormals();
		}
		_geometry.GetUserStrings().Clear();
		foreach (KeyValuePair<string, string> item in dictionary)
		{
			_geometry.SetUserString(item.Key, item.Value);
		}
		return flag;
	}

	public void PreProcessDisplayInfo()
	{
		GeometryBase geometry = _geometry;
		Mesh val = (Mesh)(object)((geometry is Mesh) ? geometry : null);
		if (val != null)
		{
			_whichDisplayType = EDisplayType.Mesh;
			_displayMesh = val;
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
		if (_isDisposed)
		{
			return;
		}
		try
		{
			Mesh displayMesh = _displayMesh;
			if (displayMesh != null)
			{
				((CommonObject)displayMesh).Dispose();
			}
			GeometryBase geometry = _geometry;
			if (geometry != null)
			{
				((CommonObject)geometry).Dispose();
			}
			_displayMesh = null;
			_geometry = null;
		}
		finally
		{
			base.Dispose(disposing);
		}
		_isDisposed = true;
	}

	protected override Image GetImage(int sizeX, int sizeY, System.Drawing.Color color, System.Drawing.Color background)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
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
		BoundingBox boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(false);
		Transform objectDisplayXform = CSUtilMath.GetObjectDisplayXform(boundingBox);
		((GeometryBase)nurbsCurve).Transform(objectDisplayXform);
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num = x - ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num2 = y - ((Point3d)(ref val)).Y;
		double num3 = 0.0;
		double num4 = 0.0;
		val = ((BoundingBox)(ref boundingBox)).Min;
		if (((Point3d)(ref val)).X < 0.0)
		{
			double num5 = num3;
			val = ((BoundingBox)(ref boundingBox)).Min;
			num3 = num5 - ((Point3d)(ref val)).X;
		}
		val = ((BoundingBox)(ref boundingBox)).Min;
		if (((Point3d)(ref val)).Y < 0.0)
		{
			double num6 = num4;
			val = ((BoundingBox)(ref boundingBox)).Min;
			num4 = num6 - ((Point3d)(ref val)).Y;
		}
		double num7 = (double)sizeX * 0.8;
		double num8 = (double)sizeY * 0.8;
		double num9 = ((!(num7 < num8)) ? (((double)sizeY - num8) / 2.0) : (((double)sizeX - num7) / 2.0));
		double num10 = ((num > num2) ? ((!(num7 > num8)) ? (num8 / num) : (num7 / num)) : ((!(num7 > num8)) ? (num8 / num2) : (num7 / num2)));
		double num11 = (num7 - num * num10) / 2.0;
		double num12 = (num8 - num2 * num10) / 2.0;
		graphics.TranslateTransform(0f, sizeY);
		graphics.ScaleTransform(1f, -1f);
		Polyline val2 = new Polyline();
		if (sizeX < 100 && sizeY < 100)
		{
			Interval domain = ((Curve)nurbsCurve).Domain;
			for (double num13 = 0.0; num13 <= 50.0; num13 += 1.0)
			{
				((RhinoList<Point3d>)(object)val2).Add(((Curve)nurbsCurve).PointAt(((Interval)(ref domain)).ParameterAt(num13 / 50.0)));
			}
		}
		else
		{
			PolylineCurve val3 = ((Curve)nurbsCurve).ToPolyline(20, 5, 5.0, 1.0, 1.0, 0.1, 0.1, 1.0, false);
			if (val3 != null)
			{
				for (int i = 0; i < val3.PointCount; i++)
				{
					Point3d val4 = val3.Point(i);
					((RhinoList<Point3d>)(object)val2).Add(val4);
				}
			}
		}
		System.Drawing.Pen pen = new System.Drawing.Pen(color, (float)(1.0 * (0.0 - num10)));
		for (int j = 1; j < ((RhinoList<Point3d>)(object)val2).Count; j++)
		{
			Point3d val5 = ((RhinoList<Point3d>)(object)val2)[j];
			Point3d val6 = ((RhinoList<Point3d>)(object)val2)[j - 1];
			float x2 = (float)((((Point3d)(ref val5)).X + num3) * num10 + num11 + num9);
			float y2 = (float)((((Point3d)(ref val5)).Y + num4) * num10 + num12 + num9);
			float x3 = (float)((((Point3d)(ref val6)).X + num3) * num10 + num11 + num9);
			float y3 = (float)((((Point3d)(ref val6)).Y + num4) * num10 + num12 + num9);
			graphics.DrawLine(pen, x2, y2, x3, y3);
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
		Mesh val = MeshObject(coarse);
		if (val == null)
		{
			return false;
		}
		if (!((CommonObject)val).IsValid)
		{
			return false;
		}
		_displayMesh = val;
		return true;
	}

	private void ConstructorAux(GeometryBase geometry, Transform transform, bool applyTransform)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if (geometry != null)
		{
			_geometry = geometry.Duplicate();
			string[] allKeys = geometry.GetUserStrings().AllKeys;
			foreach (string text in allKeys)
			{
				string userString = geometry.GetUserString(text);
				if (!string.IsNullOrEmpty(userString))
				{
					_geometry.SetUserString(text, userString);
				}
			}
		}
		if (transform != Transform.Unset)
		{
			_xform = transform;
			if (applyTransform)
			{
				TransformOnGeometry(transform);
			}
		}
		else
		{
			_xform = Transform.Identity;
		}
	}

	private void DrawNormal(DisplayPipeline dp, DisplayMaterial mat, System.Drawing.Color color, CSOptionsDisplay displayOptions)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		bool doShowCurves = displayOptions.DoShowCurves;
		bool isMouseDown = displayOptions.IsMouseDown;
		bool isSelectionDisplayModeEnabled = displayOptions.IsSelectionDisplayModeEnabled;
		switch (_whichDisplayType)
		{
		case EDisplayType.DisplayMesh:
			dp.DrawMeshShaded(_displayMesh, mat);
			break;
		case EDisplayType.Mesh:
		{
			GeometryBase geometry2 = _geometry;
			Mesh val2 = (Mesh)(object)((geometry2 is Mesh) ? geometry2 : null);
			if (val2 != null)
			{
				dp.DrawMeshShaded(val2, mat);
			}
			break;
		}
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
					Point3d pointAtStart = curve.PointAtStart;
					Point3d pointAtStart2 = curve.PointAtStart;
					Interval domain = curve.Domain;
					dp.DrawLine(pointAtStart, pointAtStart2 + curve.TangentAt(((Interval)(ref domain)).Min), System.Drawing.Color.White);
					dp.DrawLine(curve.PointAtStart, curve.PointAtStart + Vector3d.ZAxis, System.Drawing.Color.White);
				}
			}
			break;
		}
		case EDisplayType.Brep:
		{
			GeometryBase geometry = _geometry;
			Brep val = (Brep)(object)((geometry is Brep) ? geometry : null);
			if (val != null)
			{
				dp.DrawBrepShaded(val, mat);
			}
			break;
		}
		}
	}

	private void DrawWireframe(DisplayPipeline dp, System.Drawing.Color color, CSOptionsDisplay displayOptions)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		bool doShowCurves = displayOptions.DoShowCurves;
		bool isMouseDown = displayOptions.IsMouseDown;
		bool isSelectionDisplayModeEnabled = displayOptions.IsSelectionDisplayModeEnabled;
		switch (_whichDisplayType)
		{
		case EDisplayType.DisplayMesh:
			dp.DrawMeshWires(_displayMesh, color);
			break;
		case EDisplayType.Mesh:
		{
			GeometryBase geometry2 = _geometry;
			Mesh val2 = (Mesh)(object)((geometry2 is Mesh) ? geometry2 : null);
			if (val2 != null)
			{
				dp.DrawMeshWires(val2, color);
			}
			break;
		}
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
					Point3d pointAtStart = curve.PointAtStart;
					Point3d pointAtStart2 = curve.PointAtStart;
					Interval domain = curve.Domain;
					dp.DrawLine(pointAtStart, pointAtStart2 + curve.TangentAt(((Interval)(ref domain)).Min), color);
					dp.DrawLine(curve.PointAtStart, curve.PointAtStart + Vector3d.ZAxis, color);
				}
			}
			break;
		}
		case EDisplayType.Brep:
		{
			GeometryBase geometry = _geometry;
			Brep val = (Brep)(object)((geometry is Brep) ? geometry : null);
			if (val != null)
			{
				dp.DrawBrepWires(val, color);
			}
			break;
		}
		}
	}

	private void TransformOnGeometry(Transform transform)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (_geometry == null)
		{
			return;
		}
		if (!_geometry.MakeDeformable())
		{
			GeometryBase geometry = _geometry;
			Curve val = (Curve)(object)((geometry is Curve) ? geometry : null);
			if (val != null && HasNurbsCurve())
			{
				_geometry = (GeometryBase)(object)val.ToNurbsCurve();
			}
		}
		_geometry.Transform(transform);
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
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		if (_geometry == null)
		{
			return false;
		}
		if (!(_geometry is Brep))
		{
			return TryGetBoundingBox(doAccurate: false, doGrow: false, Transform.Unset, ref boundingBox);
		}
		CalculateDisplayMesh();
		if (_displayMesh != null)
		{
			boundingBox = ((GeometryBase)_displayMesh).GetBoundingBox(true);
			return true;
		}
		return TryGetBoundingBox(doAccurate: false, doGrow: false, Transform.Unset, ref boundingBox);
	}

	[Obsolete]
	public override bool GetTightBoundingBox(ref BoundingBox boundingBox)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return GetTightBoundingBox(ref boundingBox, doGrow: false, Transform.Unset);
	}

	[Obsolete]
	public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return GetTightBoundingBox(ref boundingBox, doGrow, Transform.Unset);
	}

	[Obsolete]
	public override bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow, Transform transform)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return GetTightBoundingBoxNew(ref boundingBox, doGrow: false, Transform.Unset);
	}

	public override bool GetTightBoundingBoxNew(ref BoundingBox boundingBox, bool doGrow)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return GetTightBoundingBoxNew(ref boundingBox, doGrow, Transform.Unset);
	}

	public override bool GetTightBoundingBoxNew(ref BoundingBox boundingBox, bool doGrow, Transform transform)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (_geometry == null)
		{
			return false;
		}
		return TryGetBoundingBox(doAccurate: true, doGrow, transform, ref boundingBox);
	}

	private bool TryGetBoundingBox(bool doAccurate, bool doGrow, Transform transform, ref BoundingBox boundingBox)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if (_geometry == null)
		{
			return false;
		}
		BoundingBox boundingBox2;
		if (((Transform)(ref transform)).IsValid && !((Transform)(ref transform)).Equals(Transform.Identity))
		{
			GeometryBase val = _geometry.Duplicate();
			try
			{
				val.Transform(transform);
				boundingBox2 = val.GetBoundingBox(doAccurate);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			boundingBox2 = _geometry.GetBoundingBox(doAccurate);
		}
		if (!((BoundingBox)(ref boundingBox2)).IsValid)
		{
			return false;
		}
		if (doGrow)
		{
			((BoundingBox)(ref boundingBox)).Union(boundingBox2);
		}
		else
		{
			boundingBox = boundingBox2;
		}
		return true;
	}
}
