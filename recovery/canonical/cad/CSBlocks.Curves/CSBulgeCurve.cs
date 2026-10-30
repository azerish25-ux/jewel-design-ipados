using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Collections;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("BulgeCurve", "{4606C1DC-C0C1-4C55-8D2D-9415ECDE9DBB}")]
public sealed class CSBulgeCurve : CSBlock
{
	public readonly CSInputChannel iWidth;

	public readonly CSInputChannel iBulge;

	public readonly CSInputChannel iResolution;

	public readonly CSInputChannel iDegree;

	public readonly CSInputChannel iClosed;

	public readonly CSOutputChannel oCurve;

	public CSBulgeCurve(string identifier)
		: base(identifier)
	{
		iWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBulge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iResolution = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iDegree = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iClosed = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		int value = 0;
		iResolution.GetInteger(ref value);
		double value2 = 0.0;
		iWidth.GetDouble(ref value2);
		double value3 = 0.0;
		iBulge.GetDouble(ref value3);
		if (value % 2 != 0)
		{
			return UpdateError("Resolution must be even");
		}
		int num = value / 2;
		double num2 = 0.0;
		Point3dList point3dList = new Point3dList();
		for (int i = 0; i < value; i++)
		{
			if (i == 0 || i == value - 1)
			{
				num2 = 0.0;
			}
			else if (i == 1)
			{
				num2 = value3;
				for (int num3 = num - 1; num3 > i; num3--)
				{
					num2 /= 2.0;
				}
			}
			else
			{
				num2 = ((i >= num) ? ((i != num) ? (num2 / 2.0) : value3) : (num2 * 2.0));
			}
			point3dList.Add(new Point3d((double)i / (double)(value - 1) * value2, 0.0, num2));
		}
		NurbsCurve nurbsCurve = NurbsCurve.Create(periodic: false, 2, point3dList.ToArray());
		if (!nurbsCurve.IsValid)
		{
			return UpdateError("Invalid Curve");
		}
		oCurve.SetData(new CSOnGeometryData(nurbsCurve));
		return base.Update();
	}
}
