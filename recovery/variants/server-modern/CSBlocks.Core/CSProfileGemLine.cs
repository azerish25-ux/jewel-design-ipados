using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("ProfileGemLine", "{F85CAE09-4B93-40ba-B296-E736936F3741}")]
public sealed class CSProfileGemLine : CSBlock
{
	public readonly CSInputChannel iCurve1;

	public readonly CSInputChannel iCurve2;

	public readonly CSInputChannel iSurface;

	public readonly CSInputChannel iPoint1;

	public readonly CSInputChannel iPoint2;

	public readonly CSOutputChannel oLine;

	public CSProfileGemLine(string identifier)
		: base(identifier)
	{
		iCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		iPoint1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPoint2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oLine = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		CSDataHelper.GetNurbsCurve(iCurve1.GetFlattenArray().GetArrayItem(0), out var value);
		CSDataHelper.GetNurbsCurve(iCurve2.GetFlattenArray().GetArrayItem(0), out var value2);
		CSData data = iSurface.Data;
		if (data == null)
		{
			return UpdateMissing("Surface missing");
		}
		CSDataHelper.GetBrep(data, out var value3);
		BrepFace obj = value3.Faces[0];
		double value4 = 0.0;
		double value5 = 0.0;
		if (iPoint1 != null)
		{
			iPoint1.GetDouble(ref value4);
		}
		if (iPoint2 != null)
		{
			iPoint2.GetDouble(ref value5);
		}
		double num = 0.0;
		double num2 = 0.0;
		((Curve)value).NormalizedLengthParameter(value4, ref num);
		((Curve)value2).NormalizedLengthParameter(value5, ref num2);
		Point3d unset = Point3d.Unset;
		Point3d unset2 = Point3d.Unset;
		unset = ((Curve)value).PointAt(num);
		unset2 = ((Curve)value2).PointAt(num2);
		double num3 = 0.0;
		double num4 = 0.0;
		double num5 = 0.0;
		double num6 = 0.0;
		((Surface)obj).ClosestPoint(unset, ref num3, ref num4);
		((Surface)obj).ClosestPoint(unset2, ref num5, ref num6);
		LineCurve val = new LineCurve(new Line(new Point3d(num3, num4, 0.0), new Point3d(num5, num6, 0.0)));
		Curve geometry = ((Surface)obj).Pushup((Curve)(object)val, 0.01);
		oLine.SetData(new CSOnGeometryData((GeometryBase)(object)geometry));
		return base.Update();
	}
}
