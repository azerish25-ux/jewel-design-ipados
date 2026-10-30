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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		MeshingParameters mp;
		if (iMeshingDensity != null)
		{
			double value = 0.0;
			mp = (MeshingParameters)((!iMeshingDensity.GetDouble(ref value)) ? ((object)MeshingParameters.Coarse) : ((object)new MeshingParameters
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
			}));
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
				Mesh val = new Mesh();
				if (cSInputChannel.OutputChannelIndex == -1)
				{
					if (cSInputChannel.OutputChannelBlock != null)
					{
						for (int j = 0; j < cSInputChannel.OutputChannelBlock.OutputChannelCount; j++)
						{
							CSData outputChannelData = cSInputChannel.OutputChannelBlock.GetOutputChannelData(j);
							Mesh val2 = null;
							if (outputChannelData != null)
							{
								val2 = cSInputChannel.OutputChannelBlock.GetOutputChannelData(j).MeshObject(mp);
								defaultDisplayMaterialClassification = outputChannelData.DefaultDisplayMaterialClassification;
							}
							if (val2 != null)
							{
								val.Append(val2);
							}
						}
					}
				}
				else
				{
					CSData data = cSInputChannel.Data;
					Mesh val3 = null;
					if (data != null)
					{
						val3 = data.MeshObject(mp);
						defaultDisplayMaterialClassification = data.DefaultDisplayMaterialClassification;
					}
					if (val3 != null)
					{
						val.Append(val3);
					}
				}
				CSOnGeometryData data2 = new CSOnGeometryData((GeometryBase)(object)val)
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
