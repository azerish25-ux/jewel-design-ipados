using System.Drawing;
using CSDatas.Interfaces;
using CSMaterials;
using Rhino.Display;
using Rhino.Geometry;

namespace CSDatas;

public sealed class CSBrepEdgeData : CSData, ICSDisplayObject, ICSBrepEdge, ICSCurve
{
	private BrepEdge _brepEdge;

	public CSBrepEdgeData(BrepEdge brepEdge)
	{
		Brep brep = brepEdge.Brep.DuplicateBrep();
		int edgeIndex = brepEdge.EdgeIndex;
		_brepEdge = brep.Edges[edgeIndex];
	}

	private CSBrepEdgeData(CSBrepEdgeData copy)
	{
		Brep brep = copy._brepEdge.Brep.DuplicateBrep();
		int edgeIndex = copy._brepEdge.EdgeIndex;
		_brepEdge = brep.Edges[edgeIndex];
	}

	public BrepEdge GetBrepEdge()
	{
		Brep brep = _brepEdge.Brep.DuplicateBrep();
		int edgeIndex = _brepEdge.EdgeIndex;
		return brep.Edges[edgeIndex];
	}

	public bool HasBrepEdge()
	{
		return _brepEdge != null;
	}

	public override Curve GetCurve()
	{
		return _brepEdge?.DuplicateCurve();
	}

	public override NurbsCurve GetNurbsCurve()
	{
		return _brepEdge?.ToNurbsCurve();
	}

	public override bool HasNurbsCurve()
	{
		BrepEdge brepEdge = _brepEdge;
		if (brepEdge == null)
		{
			return false;
		}
		return brepEdge.HasNurbsForm() > 0;
	}

	public bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial csDisplayMaterial = null)
	{
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
		displayPipeline.DrawCurve(_brepEdge, diffuse);
		if (!displayOptions.DoShowStarts)
		{
			return true;
		}
		Color white = Color.White;
		displayPipeline.DrawLine(_brepEdge.PointAtStart, _brepEdge.PointAtStart + _brepEdge.TangentAt(_brepEdge.Domain.Min), white, 1);
		displayPipeline.DrawLine(_brepEdge.PointAtStart, _brepEdge.PointAtStart + Vector3d.ZAxis, white, 1);
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
		if (!_isDisposed)
		{
			try
			{
				_brepEdge?.Dispose();
				_brepEdge = null;
			}
			finally
			{
				base.Dispose(disposing);
			}
			_isDisposed = true;
		}
	}

	~CSBrepEdgeData()
	{
		Dispose(disposing: false);
	}
}
