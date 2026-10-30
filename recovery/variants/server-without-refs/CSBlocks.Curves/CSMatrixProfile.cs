using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Runtime;

namespace CSBlocks.Curves;

[CSBlock("MatrixProfile", "{9C7AFFD9-4922-4b7d-BD35-BF24F0FB5AED}")]
public sealed class CSMatrixProfile : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iPoints;

	public readonly CSOutputChannel oCurve;

	public CSMatrixProfile(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iPoints = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(iCurve.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Profile: Missing Curve");
		}
		if (!((CommonObject)value).IsValid)
		{
			return UpdateMissing("Profile: Nurbs Curve is not valid");
		}
		if (!((Curve)value).IsClosed)
		{
			return UpdateMissing("Profile: Nurbs Curve is not closed");
		}
		NurbsCurve val = new NurbsCurve(value);
		Transform val2 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, 0.0);
		((GeometryBase)val).Transform(val2);
		LineCurve val3 = new LineCurve(new Point3d(0.0, -100.0, 0.0), new Point3d(0.0, 100.0, 0.0));
		CurveIntersections val4 = Intersection.CurveCurve((Curve)(object)val, (Curve)(object)val3, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val4.Count > 0)
		{
			int num = 0;
			for (int i = 1; i < val4.Count; i++)
			{
				Point3d pointA = val4[num].PointA;
				double y = ((Point3d)(ref pointA)).Y;
				pointA = val4[i].PointA;
				if (y > ((Point3d)(ref pointA)).Y)
				{
					num = i;
				}
			}
			((Curve)val).ChangeClosedCurveSeam(val4[num].ParameterA);
			Interval domain = ((Curve)val).Domain;
			if (((Curve)val).TangentAt(((Interval)(ref domain)).Min) * new Vector3d(1.0, 0.0, 0.0) < 0.0)
			{
				((Curve)val).Reverse();
			}
		}
		List<double> list = new List<double>();
		if (InputChannelCount > 1)
		{
			for (int j = 1; j < InputChannelCount; j++)
			{
				if (base.InputChannels[j] != null)
				{
					double value2 = 0.0;
					base.InputChannels[j].GetDouble(ref value2);
					if (value2 < 0.0)
					{
						value2 = 1.0 - value2;
					}
					double item = 0.0;
					if (((Curve)val).NormalizedLengthParameter(value2, ref item))
					{
						list.Add(item);
					}
				}
			}
		}
		else
		{
			for (int k = 1; k < 4; k++)
			{
				double item2 = 0.0;
				if (((Curve)val).NormalizedLengthParameter((double)k / 4.0, ref item2))
				{
					list.Add(item2);
				}
			}
		}
		oCurve.SetData(new CSProfileData((Curve)(object)val, list, Transform.Unset, null));
		return base.Update();
	}
}
