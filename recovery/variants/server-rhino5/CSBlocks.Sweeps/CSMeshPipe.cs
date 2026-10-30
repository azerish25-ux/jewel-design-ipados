using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Sweeps;

[CSBlock("MeshPipe", "{3CBAFB89-7C60-4b5a-8C5F-BD7D9F32632D}")]
public sealed class CSMeshPipe : CSBlock
{
	public CSInputChannel iCurves { get; }

	public CSInputChannel iWidthStart { get; }

	public CSInputChannel iHeightStart { get; }

	public CSInputChannel iWidthEnd { get; }

	public CSInputChannel iHeightEnd { get; }

	public CSOutputChannel oPipes { get; }

	public CSMeshPipe(string identifier)
		: base(identifier)
	{
		iCurves = new CSInputChannel(GetPropertyName(() => iCurves), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iWidthStart = new CSInputChannel(GetPropertyName(() => iWidthStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iHeightStart = new CSInputChannel(GetPropertyName(() => iHeightStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iWidthEnd = new CSInputChannel(GetPropertyName(() => iWidthEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iHeightEnd = new CSInputChannel(GetPropertyName(() => iHeightEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oPipes = new CSOutputChannel(GetPropertyName(() => oPipes), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurves.GetFlattenArray();
		double num = 1.0;
		double value = 1.0;
		double num2 = 1.0;
		double num3 = 1.0;
		iWidthStart.GetDouble(ref value);
		num = value;
		num3 = value;
		iHeightStart.GetDouble(ref num);
		iWidthEnd.GetDouble(ref num3);
		num2 = num3;
		iHeightEnd.GetDouble(ref num2);
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
			OnNurbsCurve val = arrayItem.GetNurbsCurve().Convert();
			if (val != null)
			{
				OnMesh val2 = ((value != num || num3 != num2) ? CSUtilSweep.MeshPipe((IOnCurve)(object)val, value, num, num3, num2, 0.1) : CSUtilSweep.MeshPipe((IOnCurve)(object)val, value / 2.0, num3 / 2.0, 0.1));
				if (val2 != null)
				{
					cSDataArray.Append(new CSOnGeometryData(val2.Convert()));
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
