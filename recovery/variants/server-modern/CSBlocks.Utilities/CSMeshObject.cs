using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Utilities;

[CSBlock("MeshObject", "{4961507D-883A-41BD-B9D5-3DA1D9C7C060}")]
public sealed class CSMeshObject : CSBlock
{
	public readonly CSInputChannel iObject;

	public readonly CSInputChannel iMeshingDensity;

	public readonly CSInputChannel iDisableMeshConversion;

	public readonly CSOutputChannel oObject;

	public CSMeshObject(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep | ChannelDataType.Mesh, ChannelArrayType.Array | ChannelArrayType.Individual);
		iMeshingDensity = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iDisableMeshConversion = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(this, ChannelDataType.Mesh, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		bool value = false;
		iDisableMeshConversion.GetBoolean(ref value);
		if (CSApplicationHelper.Application.DisableMeshObjectMeshing)
		{
			value = true;
		}
		if (CSApplicationHelper.Application.IsCamMode)
		{
			value = true;
		}
		MeshingParameters mp;
		if (iMeshingDensity != null)
		{
			double value2 = 0.0;
			mp = (MeshingParameters)((!iMeshingDensity.GetDouble(ref value2)) ? ((object)MeshingParameters.Coarse) : ((object)new MeshingParameters
			{
				ComputeCurvature = false,
				GridAmplification = 0.0,
				GridAngle = 0.0,
				GridAspectRatio = 0.0,
				GridMaxCount = 0,
				GridMinCount = 0,
				JaggedSeams = false,
				MaximumEdgeLength = 0.0,
				MinimumEdgeLength = 0.0,
				MinimumTolerance = 0.0,
				RefineAngle = 0.0,
				RefineGrid = true,
				RelativeTolerance = value2,
				SimplePlanes = true,
				Tolerance = 0.0
			}));
		}
		else
		{
			mp = MeshingParameters.Coarse;
		}
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray flattenArray = iObject.GetFlattenArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem == null)
			{
				continue;
			}
			if (value)
			{
				cSDataArray.Append(arrayItem.Duplicate());
				continue;
			}
			Mesh val = arrayItem.MeshObject(mp);
			if (val != null)
			{
				CSOnGeometryData data = new CSOnGeometryData((GeometryBase)(object)val)
				{
					DefaultDisplayMaterialClassification = arrayItem.DefaultDisplayMaterialClassification
				};
				cSDataArray.Append(data);
			}
		}
		if (cSDataArray.Array.Count <= 0)
		{
			oObject.SetData(null);
		}
		else if (cSDataArray.Array.Count == 1)
		{
			oObject.SetData(cSDataArray.Array[0]);
		}
		else
		{
			oObject.SetData(cSDataArray);
		}
		return base.Update();
	}
}
