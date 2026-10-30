using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Bails;

[CSBlock("VolumeCentroid", "{4EFC3D07-7125-4f5d-BD9C-CFE0415EC6A2}")]
public sealed class CSVolumeCentroid : CSBlock
{
	public CSInputChannel InObject { get; }

	public CSOutputChannel OutPoint { get; }

	public CSVolumeCentroid(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep | ChannelDataType.Mesh, ChannelArrayType.Array);
		OutPoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InObject.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Input object missing");
		}
		List<Brep> list = new List<Brep>();
		List<Mesh> list2 = new List<Mesh>();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem == null)
			{
				continue;
			}
			Brep brep = arrayItem.GetBrep();
			if (brep != null)
			{
				if (brep.IsSolid)
				{
					list.Add(brep);
					continue;
				}
				Brep brep2 = CSUtilRhino.CapPlanarHoles(brep, CSApplicationHelper.Application.Tolerance);
				if (brep2 != null && brep2.IsSolid)
				{
					list.Add(brep2);
				}
			}
			else
			{
				Mesh mesh = arrayItem.GetMesh();
				if (mesh != null && mesh.IsClosed)
				{
					list2.Add(mesh);
				}
			}
		}
		List<VolumeMassProperties> list3 = new List<VolumeMassProperties>();
		for (int j = 0; j < list.Count; j++)
		{
			VolumeMassProperties item = VolumeMassProperties.Compute(list[j]);
			list3.Add(item);
		}
		for (int k = 0; k < list2.Count; k++)
		{
			VolumeMassProperties item2 = VolumeMassProperties.Compute(list2[k]);
			list3.Add(item2);
		}
		VolumeMassProperties volumeMassProperties = list3[0];
		foreach (VolumeMassProperties item3 in list3)
		{
			volumeMassProperties.Sum(item3);
		}
		OutPoint.SetData(new CSPointData(volumeMassProperties.Centroid));
		return base.Update();
	}
}
