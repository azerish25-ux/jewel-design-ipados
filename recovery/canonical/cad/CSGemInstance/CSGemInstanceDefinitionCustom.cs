using System.Collections.Generic;
using System.Linq;
using Rhino.Collections;
using Rhino.DocObjects;
using Rhino.Geometry;

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
		face = null;
		if (cornerIndices.Any((int cornerIndex) => cornerIndex < 0 || cornerIndex >= _vertices.Count))
		{
			return false;
		}
		switch (cornerIndices.Length)
		{
		case 3:
		{
			int index5 = cornerIndices[0];
			int index6 = cornerIndices[1];
			int index7 = cornerIndices[2];
			face = Brep.CreateFromCornerPoints(_vertices[index5], _vertices[index6], _vertices[index7], CSUtil.Tolerance);
			break;
		}
		case 4:
		{
			int index = cornerIndices[0];
			int index2 = cornerIndices[1];
			int index3 = cornerIndices[2];
			int index4 = cornerIndices[3];
			face = Brep.CreateFromCornerPoints(_vertices[index], _vertices[index2], _vertices[index3], _vertices[index4], CSUtil.Tolerance);
			break;
		}
		default:
			return false;
		}
		if (face != null)
		{
			return face.IsValid;
		}
		return false;
	}

	private bool CreateFaceFromCorners(out MeshFace face, params int[] cornerIndices)
	{
		face = MeshFace.Unset;
		if (cornerIndices.Any((int cornerIndex) => cornerIndex < 0 || cornerIndex >= _vertices.Count))
		{
			return false;
		}
		switch (cornerIndices.Length)
		{
		case 3:
		{
			int a2 = cornerIndices[0];
			int b2 = cornerIndices[1];
			int c2 = cornerIndices[2];
			face = new MeshFace(a2, b2, c2);
			break;
		}
		case 4:
		{
			int a = cornerIndices[0];
			int b = cornerIndices[1];
			int c = cornerIndices[2];
			int d = cornerIndices[3];
			face = new MeshFace(a, b, c, d);
			break;
		}
		default:
			return false;
		}
		return face.IsValid();
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
		Brep brep = Brep.MergeBreps(list, CSUtil.Tolerance);
		brep.Compact();
		return brep;
	}

	protected Mesh GetMesh()
	{
		Mesh mesh = new Mesh();
		mesh.Vertices.AddVertices(_vertices);
		foreach (int[] faceCornerSet in _faceCornerSets)
		{
			if (CreateFaceFromCorners(out MeshFace face, faceCornerSet))
			{
				mesh.Faces.AddFace(face);
			}
		}
		mesh.Unweld(0.0, modifyNormals: true);
		mesh.Normals.ComputeNormals();
		mesh.UnifyNormals();
		mesh.Compact();
		return mesh;
	}
}
