using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Collections;
using Rhino.Geometry;
using Rhino.Runtime;

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
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		double value = 3.0;
		if (iDegree != null)
		{
			iDegree.GetDouble(ref value);
		}
		if (!(iPointArray.Data is ICSArray iCSArray))
		{
			return UpdateMissing("Missing Points");
		}
		Point3dList val = new Point3dList();
		for (int i = 0; i < iCSArray.GetArrayCount(); i++)
		{
			if (iCSArray.GetArrayItem(i) is ICSPoint iCSPoint && iCSPoint.GetPoint(out var value2) && value2 != Point3d.Unset)
			{
				((RhinoList<Point3d>)(object)val).Add(value2);
			}
		}
		if ((double)((RhinoList<Point3d>)(object)val).Count < value + 1.0)
		{
			return UpdateMissing("Point Array is not large enough");
		}
		double value3 = 0.0;
		Point3d val2 = ((RhinoList<Point3d>)(object)val)[0];
		if (((Point3d)(ref val2)).CompareTo(((RhinoList<Point3d>)(object)val)[((RhinoList<Point3d>)(object)val).Count - 1]) == 0)
		{
			value3 = 1.0;
		}
		if (iClosed != null)
		{
			iClosed.GetDouble(ref value3);
		}
		NurbsCurve val3 = NurbsCurve.Create(value3 > 0.5, (int)value - 1, (IEnumerable<Point3d>)((RhinoList<Point3d>)(object)val).ToArray());
		if (!((CommonObject)val3).IsValid)
		{
			return UpdateError("Invalid Curve");
		}
		oCurve.SetData(new CSOnGeometryData((GeometryBase)(object)val3));
		return base.Update();
	}
}
