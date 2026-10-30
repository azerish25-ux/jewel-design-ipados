using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("SetCurveDirection", "{D9C61D6E-E7FE-4E3A-8A87-4A1F38E1C7C3}")]
public sealed class CSSetCurveDirection : CSBlock
{
	public readonly CSInputChannel InUndirectedCurves;

	public readonly CSInputChannel InDirection;

	public readonly CSInputChannel InDoOutputAsArray;

	public readonly CSOutputChannel OutDirectedCurves;

	public CSSetCurveDirection(string identifier)
		: base(identifier)
	{
		InUndirectedCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InDirection = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InDoOutputAsArray = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutDirectedCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InUndirectedCurves.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing curves");
		}
		int value = 0;
		if (InDirection != null)
		{
			InDirection.GetInteger(ref value);
		}
		bool value2 = true;
		if (InDoOutputAsArray != null)
		{
			InDoOutputAsArray.GetBoolean(ref value2);
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (!CSDataHelper.GetNurbsCurve(arrayItem, out var value3))
			{
				cSDataArray.Append(arrayItem);
				continue;
			}
			Vector3d vector3d = value3.PointAtStart - value3.PointAtEnd;
			bool flag = false;
			switch (value)
			{
			case 0:
				if (vector3d.X < 0.0)
				{
					flag = true;
				}
				break;
			case 1:
				if (vector3d.X > 0.0)
				{
					flag = true;
				}
				break;
			case 2:
				if (vector3d.Y < 0.0)
				{
					flag = true;
				}
				break;
			case 3:
				if (vector3d.Y > 0.0)
				{
					flag = true;
				}
				break;
			case 4:
				if (vector3d.Z < 0.0)
				{
					flag = true;
				}
				break;
			case 5:
				if (vector3d.Z > 0.0)
				{
					flag = true;
				}
				break;
			}
			NurbsCurve nurbsCurve = value3.DuplicateCurve().ToNurbsCurve();
			if (flag)
			{
				nurbsCurve.Reverse();
			}
			cSDataArray.Append(new CSOnGeometryData(nurbsCurve));
		}
		if (value2)
		{
			OutDirectedCurves.SetData(cSDataArray);
		}
		else
		{
			base.OutputChannels.Clear();
			for (int j = 0; j < cSDataArray.GetArrayCount(); j++)
			{
				new CSOutputChannel("OutDirectedCurves_" + j, this, ChannelDataType.Any, ChannelArrayType.Any).SetData(cSDataArray.GetArrayItem(j));
			}
		}
		return base.Update();
	}
}
