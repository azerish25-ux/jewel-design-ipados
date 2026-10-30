using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Collections;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("SineWave", "{B83C9B85-960F-4d5c-981F-CBB758D93DBF}")]
public sealed class CSSineWave : CSBlock
{
	public readonly CSInputChannel iRadius;

	public readonly CSInputChannel iRepeat;

	public readonly CSInputChannel iYOffset;

	public readonly CSInputChannel iWidth;

	public readonly CSOutputChannel oCurve;

	public CSSineWave(string identifier)
		: base(identifier)
	{
		iRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRepeat = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		double value = 0.0;
		iRadius.GetDouble(ref value);
		double value2 = 0.0;
		iRepeat.GetDouble(ref value2);
		double value3 = 0.0;
		iYOffset.GetDouble(ref value3);
		double value4 = 0.0;
		iWidth.GetDouble(ref value4);
		Point3dList val = new Point3dList();
		Point3d val2 = default(Point3d);
		for (double num = 0.0; num < value2; num += 1.0)
		{
			for (double num2 = 0.0; num2 < 360.0; num2 += 30.0)
			{
				double num3 = CSBlockUtil.CSUtilMath.ToRadians((num2 / 360.0 + num) * 360.0 / value2);
				double num4 = Math.Sin(CSBlockUtil.CSUtilMath.ToRadians(num2)) * (value4 / 2.0) + value3;
				double num5 = Math.Sin(num3) * value;
				double num6 = Math.Cos(num3) * value;
				((Point3d)(ref val2))._002Ector(num5, num4, num6);
				((RhinoList<Point3d>)(object)val).Add(val2);
			}
		}
		((RhinoList<Point3d>)(object)val).Add(((RhinoList<Point3d>)(object)val)[0]);
		Curve geometry = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)val, 3, (CurveKnotStyle)0);
		oCurve.SetData(new CSOnGeometryData((GeometryBase)(object)geometry));
		return base.Update();
	}
}
