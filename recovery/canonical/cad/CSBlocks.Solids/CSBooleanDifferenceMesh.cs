using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Solids;

[CSBlock("BooleanDifferenceMesh", "{9D063204-68D7-4da7-B08B-9401E23222E5}")]
public sealed class CSBooleanDifferenceMesh : CSBlock
{
	public CSInputChannel iSurfaceLhs { get; }

	public CSInputChannel iSurfaceRhs { get; }

	public CSOutputChannel oSurface { get; }

	public CSBooleanDifferenceMesh(string identifier)
		: base(identifier)
	{
		iSurfaceLhs = new CSInputChannel(GetPropertyName(() => iSurfaceLhs), this, ChannelDataType.Brep, ChannelArrayType.Array);
		iSurfaceRhs = new CSInputChannel(GetPropertyName(() => iSurfaceRhs), this, ChannelDataType.Brep, ChannelArrayType.Array);
		oSurface = new CSOutputChannel(GetPropertyName(() => oSurface), this, ChannelDataType.Mesh, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iSurfaceLhs.GetFlattenArray();
		CSDataArray flattenArray2 = iSurfaceRhs.GetFlattenArray();
		List<OnMesh> input = GetInput(flattenArray);
		List<OnMesh> input2 = GetInput(flattenArray2);
		OnMesh[] array = input.ToArray();
		OnMesh[] array2 = input2.ToArray();
		bool something_happened = false;
		IOnMesh[] array3 = array;
		IOnMesh[] inMeshes = array3;
		array3 = array2;
		RhUtil.RhinoMeshBooleanDifference(inMeshes, array3, 0.0, 0.0, ref something_happened, out var OutMeshes);
		if (!something_happened || OutMeshes == null)
		{
			return UpdateError("Failed Boolean");
		}
		CSDataArray cSDataArray = new CSDataArray();
		OnMesh[] array4 = OutMeshes;
		foreach (OnMesh mesh in array4)
		{
			cSDataArray.Append(new CSOnGeometryData(mesh.Convert()));
		}
		oSurface.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}

	private static List<OnMesh> GetInput(CSDataArray datas)
	{
		List<OnMesh> list = new List<OnMesh>();
		for (int i = 0; i < datas.GetArrayCount(); i++)
		{
			CSData arrayItem = datas.GetArrayItem(i);
			if (arrayItem != null)
			{
				Mesh mesh = arrayItem.MeshObject(MeshingParameters.Coarse);
				if (mesh != null)
				{
					list.Add(mesh.Convert());
				}
			}
		}
		return list;
	}
}
