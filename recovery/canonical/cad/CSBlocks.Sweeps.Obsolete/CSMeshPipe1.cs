using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Sweeps.Obsolete;

[Obsolete]
[CSBlock("MeshPipe_V1", "{519702C2-3DD6-4dfc-A760-97E4CFCC300D}")]
public sealed class CSMeshPipe1 : CSBlock
{
	public CSInputChannel iCurves { get; }

	public CSInputChannel iRadiusStart { get; }

	public CSInputChannel iRadiusEnd { get; }

	public CSOutputChannel oPipes { get; }

	public CSMeshPipe1(string identifier)
		: base(identifier)
	{
		iCurves = new CSInputChannel(GetPropertyName(() => iCurves), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iRadiusStart = new CSInputChannel(GetPropertyName(() => iRadiusStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRadiusEnd = new CSInputChannel(GetPropertyName(() => iRadiusEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oPipes = new CSOutputChannel(GetPropertyName(() => oPipes), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurves.GetFlattenArray();
		double value = 1.0;
		double num = 1.0;
		iRadiusStart.GetDouble(ref value);
		num = value;
		iRadiusEnd.GetDouble(ref num);
		CSDataArray cSDataArray = new CSDataArray();
		bool flag = false;
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (CSApplicationHelper.Application.Document.InterruptUpdate())
			{
				flag = true;
				break;
			}
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem == null)
			{
				continue;
			}
			OnNurbsCurve onNurbsCurve = arrayItem.GetNurbsCurve().Convert();
			if (onNurbsCurve != null)
			{
				OnMesh onMesh = CSUtilSweep.MeshPipe(onNurbsCurve, value, num, 0.1);
				if (onMesh != null)
				{
					cSDataArray.Append(new CSOnGeometryData(onMesh.Convert()));
				}
			}
		}
		oPipes.SetData(cSDataArray);
		if (!flag)
		{
			return base.Update();
		}
		return UpdateCancel();
	}
}
