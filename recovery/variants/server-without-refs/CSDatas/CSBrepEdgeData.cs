using System.Drawing;
using CSDatas.Interfaces;
using CSMaterials;
using Rhino.Display;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSDatas;

public sealed class CSBrepEdgeData : CSData, ICSDisplayObject, ICSBrepEdge, ICSCurve
{
	private BrepEdge _brepEdge;

	public CSBrepEdgeData(BrepEdge brepEdge)
	{
		Brep val = brepEdge.Brep.DuplicateBrep();
		int edgeIndex = brepEdge.EdgeIndex;
		_brepEdge = val.Edges[edgeIndex];
	}

	private CSBrepEdgeData(CSBrepEdgeData copy)
	{
		Brep val = copy._brepEdge.Brep.DuplicateBrep();
		int edgeIndex = copy._brepEdge.EdgeIndex;
		_brepEdge = val.Edges[edgeIndex];
	}

	public BrepEdge GetBrepEdge()
	{
		Brep obj = _brepEdge.Brep.DuplicateBrep();
		int edgeIndex = _brepEdge.EdgeIndex;
		return obj.Edges[edgeIndex];
	}

	public bool HasBrepEdge()
	{
		return _brepEdge != null;
	}

	public override Curve GetCurve()
	{
		BrepEdge brepEdge = _brepEdge;
		if (brepEdge == null)
		{
			return null;
		}
		return ((Curve)brepEdge).DuplicateCurve();
	}

	public override NurbsCurve GetNurbsCurve()
	{
		BrepEdge brepEdge = _brepEdge;
		if (brepEdge == null)
		{
			return null;
		}
		return ((Curve)brepEdge).ToNurbsCurve();
	}

	public override bool HasNurbsCurve()
	{
		BrepEdge brepEdge = _brepEdge;
		if (brepEdge == null)
		{
			return false;
		}
		return ((Curve)brepEdge).HasNurbsForm() > 0;
	}

	public bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial csDisplayMaterial = null)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		if (!displayOptions.DoShowCurves && !displayOptions.IsMouseDown && !displayOptions.IsSelectionDisplayModeEnabled)
		{
			return false;
		}
		if (_brepEdge == null)
		{
			return false;
		}
		if (csDisplayMaterial == null)
		{
			csDisplayMaterial = displayOptions.GetCSDisplayMaterialFromName(base.CurrentMaterialGroup.MaterialName);
		}
		Color diffuse = csDisplayMaterial.Mat.Diffuse;
		displayPipeline.DrawCurve((Curve)(object)_brepEdge, diffuse);
		if (!displayOptions.DoShowStarts)
		{
			return true;
		}
		Color white = Color.White;
		Point3d pointAtStart = ((Curve)_brepEdge).PointAtStart;
		Point3d pointAtStart2 = ((Curve)_brepEdge).PointAtStart;
		BrepEdge brepEdge = _brepEdge;
		Interval domain = ((Curve)_brepEdge).Domain;
		displayPipeline.DrawLine(pointAtStart, pointAtStart2 + ((Curve)brepEdge).TangentAt(((Interval)(ref domain)).Min), white, 1);
		displayPipeline.DrawLine(((Curve)_brepEdge).PointAtStart, ((Curve)_brepEdge).PointAtStart + Vector3d.ZAxis, white, 1);
		return true;
	}

	public bool AssignCurrentMaterialGroup(MaterialGroup materialGroup)
	{
		base.CurrentMaterialGroup = materialGroup;
		return true;
	}

	public override CSData Duplicate()
	{
		return new CSBrepEdgeData(this);
	}

	protected override void Dispose(bool disposing)
	{
		if (_isDisposed)
		{
			return;
		}
		try
		{
			BrepEdge brepEdge = _brepEdge;
			if (brepEdge != null)
			{
				((CommonObject)brepEdge).Dispose();
			}
			_brepEdge = null;
		}
		finally
		{
			base.Dispose(disposing);
		}
		_isDisposed = true;
	}

	~CSBrepEdgeData()
	{
		Dispose(disposing: false);
	}
}
