using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Collections;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("Curve", "{C2E9CEF8-5336-415c-92FF-E391D11E59FC}")]
public sealed class CSCurve : CSBlock
{
	public readonly CSInputChannel iPointArray;

	public readonly CSInputChannel iDegree;

	public readonly CSInputChannel iClosed;

	public readonly CSOutputChannel oCurve;

	public CSCurve(string identifier)
		: base(identifier)
	{
		iPointArray = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Array);
		iDegree = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iClosed = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		double value = 3.0;
		if (iDegree != null)
		{
			iDegree.GetDouble(ref value);
		}
		if (!(iPointArray.Data is ICSArray iCSArray))
		{
			return UpdateMissing("Missing Points");
		}
		Point3dList point3dList = new Point3dList();
		for (int i = 0; i < iCSArray.GetArrayCount(); i++)
		{
			if (iCSArray.GetArrayItem(i) is ICSPoint iCSPoint && iCSPoint.GetPoint(out var value2) && value2 != Point3d.Unset)
			{
				point3dList.Add(value2);
			}
		}
		if ((double)point3dList.Count < value + 1.0)
		{
			return UpdateMissing("Point Array is not large enough");
		}
		double value3 = 0.0;
		if (point3dList[0].CompareTo(point3dList[point3dList.Count - 1]) == 0)
		{
			value3 = 1.0;
		}
		if (iClosed != null)
		{
			iClosed.GetDouble(ref value3);
		}
		NurbsCurve nurbsCurve = NurbsCurve.Create(value3 > 0.5, (int)value - 1, point3dList.ToArray());
		if (!nurbsCurve.IsValid)
		{
			return UpdateError("Invalid Curve");
		}
		oCurve.SetData(new CSOnGeometryData(nurbsCurve));
		return base.Update();
	}
}
