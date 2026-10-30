using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSMaterials;
using Rhino.Geometry;

namespace CSBlocks.Utilities.Obsolete;

[Obsolete]
[CSBlock("MeshObject_V1", "{7A81A9AF-A84F-4ba1-8D3D-9FCD30001342}")]
public sealed class CSMeshObject1 : CSBlock
{
	public readonly CSInputChannel iObject;

	public readonly CSInputChannel iMeshingDensity;

	public readonly CSOutputChannel oObject;

	public CSMeshObject1(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iMeshingDensity = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		MeshingParameters mp;
		if (iMeshingDensity != null)
		{
			double value = 0.0;
			mp = ((!iMeshingDensity.GetDouble(ref value)) ? MeshingParameters.Coarse : new MeshingParameters
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
				RelativeTolerance = value,
				SimplePlanes = true,
				Tolerance = 0.0
			});
		}
		else
		{
			mp = MeshingParameters.Coarse;
		}
		base.OutputChannels.Clear();
		for (int i = 0; i < InputChannelCount; i++)
		{
			CSOutputChannel cSOutputChannel = new CSOutputChannel($"{base.Identifier}_{i}", this, ChannelDataType.Any, ChannelArrayType.Any);
			CSInputChannel cSInputChannel = base.InputChannels[i];
			if (cSInputChannel != null)
			{
				MaterialClassification defaultDisplayMaterialClassification = MaterialClassification.Undefined;
				Mesh mesh = new Mesh();
				if (cSInputChannel.OutputChannelIndex == -1)
				{
					if (cSInputChannel.OutputChannelBlock != null)
					{
						for (int j = 0; j < cSInputChannel.OutputChannelBlock.OutputChannelCount; j++)
						{
							CSData outputChannelData = cSInputChannel.OutputChannelBlock.GetOutputChannelData(j);
							Mesh mesh2 = null;
							if (outputChannelData != null)
							{
								mesh2 = cSInputChannel.OutputChannelBlock.GetOutputChannelData(j).MeshObject(mp);
								defaultDisplayMaterialClassification = outputChannelData.DefaultDisplayMaterialClassification;
							}
							if (mesh2 != null)
							{
								mesh.Append(mesh2);
							}
						}
					}
				}
				else
				{
					CSData data = cSInputChannel.Data;
					Mesh mesh3 = null;
					if (data != null)
					{
						mesh3 = data.MeshObject(mp);
						defaultDisplayMaterialClassification = data.DefaultDisplayMaterialClassification;
					}
					if (mesh3 != null)
					{
						mesh.Append(mesh3);
					}
				}
				CSOnGeometryData data2 = new CSOnGeometryData(mesh)
				{
					DefaultDisplayMaterialClassification = defaultDisplayMaterialClassification
				};
				cSOutputChannel.SetData(data2);
			}
			else
			{
				cSOutputChannel.SetData(new CSData());
			}
		}
		return base.Update();
	}
}
