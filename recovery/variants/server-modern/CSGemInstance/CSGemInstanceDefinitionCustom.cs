using System.Collections.Generic;
using System.Linq;
using Rhino.Collections;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSGemInstance;

public abstract class CSGemInstanceDefinitionCustom : CSGemInstanceDefinition
{
	protected readonly Point3dList _vertices = new Point3dList();

	protected readonly List<int[]> _faceCornerSets = new List<int[]>();

	protected override bool GetGemInstanceDefinition(out InstanceDefinition gemInstanceDefinition)
	{
		gemInstanceDefinition = CSUtilGem.GetGemInstanceDefinition(_type, _shape, BlockName);
		if (gemInstanceDefinition != null)
		{
			return true;
		}
		if (!CreateGemInstanceDefinition(out gemInstanceDefinition))
		{
			return false;
		}
		return gemInstanceDefinition != null;
	}

	protected abstract bool CreateGemInstanceDefinition(out InstanceDefinition gemInstanceDefinition);

	private bool CreateFaceFromCorners(out Brep face, params int[] cornerIndices)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		face = null;
		if (cornerIndices.Any((int cornerIndex) => cornerIndex < 0 || cornerIndex >= ((RhinoList<Point3d>)(object)_vertices).Count))
		{
			return false;
		}
		switch (cornerIndices.Length)
		{
		case 3:
		{
			int num5 = cornerIndices[0];
			int num6 = cornerIndices[1];
			int num7 = cornerIndices[2];
			face = Brep.CreateFromCornerPoints(((RhinoList<Point3d>)(object)_vertices)[num5], ((RhinoList<Point3d>)(object)_vertices)[num6], ((RhinoList<Point3d>)(object)_vertices)[num7], CSUtil.Tolerance);
			break;
		}
		case 4:
		{
			int num = cornerIndices[0];
			int num2 = cornerIndices[1];
			int num3 = cornerIndices[2];
			int num4 = cornerIndices[3];
			face = Brep.CreateFromCornerPoints(((RhinoList<Point3d>)(object)_vertices)[num], ((RhinoList<Point3d>)(object)_vertices)[num2], ((RhinoList<Point3d>)(object)_vertices)[num3], ((RhinoList<Point3d>)(object)_vertices)[num4], CSUtil.Tolerance);
			break;
		}
		default:
			return false;
		}
		if (face != null)
		{
			return ((CommonObject)face).IsValid;
		}
		return false;
	}

	private bool CreateFaceFromCorners(out MeshFace face, params int[] cornerIndices)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		face = MeshFace.Unset;
		if (cornerIndices.Any((int cornerIndex) => cornerIndex < 0 || cornerIndex >= ((RhinoList<Point3d>)(object)_vertices).Count))
		{
			return false;
		}
		switch (cornerIndices.Length)
		{
		case 3:
		{
			int num5 = cornerIndices[0];
			int num6 = cornerIndices[1];
			int num7 = cornerIndices[2];
			face = new MeshFace(num5, num6, num7);
			break;
		}
		case 4:
		{
			int num = cornerIndices[0];
			int num2 = cornerIndices[1];
			int num3 = cornerIndices[2];
			int num4 = cornerIndices[3];
			face = new MeshFace(num, num2, num3, num4);
			break;
		}
		default:
			return false;
		}
		return ((MeshFace)(ref face)).IsValid();
	}

	protected Brep GetBrep()
	{
		List<Brep> list = new List<Brep>();
		foreach (int[] faceCornerSet in _faceCornerSets)
		{
			if (CreateFaceFromCorners(out Brep face, faceCornerSet))
			{
				list.Add(face);
			}
		}
		Brep obj = Brep.MergeBreps((IEnumerable<Brep>)list, CSUtil.Tolerance);
		obj.Compact();
		return obj;
	}

	protected Mesh GetMesh()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		Mesh val = new Mesh();
		val.Vertices.AddVertices((IEnumerable<Point3d>)_vertices);
		foreach (int[] faceCornerSet in _faceCornerSets)
		{
			if (CreateFaceFromCorners(out MeshFace face, faceCornerSet))
			{
				val.Faces.AddFace(face);
			}
		}
		val.Unweld(0.0, true);
		val.Normals.ComputeNormals();
		val.UnifyNormals();
		val.Compact();
		return val;
	}
}
