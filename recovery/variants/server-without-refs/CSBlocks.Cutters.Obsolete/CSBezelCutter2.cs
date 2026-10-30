using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Cutters.Obsolete;

[Obsolete]
[CSBlock("BezelCutter_V2", "{8CF04F0A-9297-4fe2-B036-E9A3BA200B93}")]
public sealed class CSBezelCutter2 : CSBlock
{
	public CSInputChannel InBezel { get; }

	public CSInputChannel InCenterSpacing { get; }

	public CSInputChannel InPlacement { get; }

	public CSInputChannel InTopX { get; }

	public CSInputChannel InTopZ { get; }

	public CSInputChannel InSeatX { get; }

	public CSInputChannel InSeatZ { get; }

	public CSInputChannel InBottomX { get; }

	public CSInputChannel InBottomZ { get; }

	public CSOutputChannel OutBrep { get; }

	public CSBezelCutter2(string identifier)
		: base(identifier)
	{
		InBezel = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		InCenterSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InTopX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InTopZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InSeatX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InSeatZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InBottomX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InBottomZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		OutBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InBezel.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Bezel Cutter: Missing Bezel");
		}
		double num = InCenterSpacing.Double;
		double placement = InPlacement.Double;
		double num2 = InTopX.Double;
		double placement2 = InTopZ.Double;
		double num3 = InSeatX.Double;
		double placement3 = InSeatZ.Double;
		double num4 = InBottomX.Double;
		double placement4 = InBottomZ.Double;
		CSDataArray cSDataArray = new CSDataArray();
		Circle val2 = default(Circle);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value);
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(value, out var xUp, out var xDown);
			Brep brep = arrayItem.GetBrep();
			((GeometryBase)brep).Transform(xDown);
			BoundingBox boundingBox = ((GeometryBase)brep).GetBoundingBox(false);
			Plane worldXY = Plane.WorldXY;
			Point3d max = ((BoundingBox)(ref boundingBox)).Max;
			Point3d val = new Point3d(0.0, 0.0, ((Point3d)(ref max)).Z);
			max = ((BoundingBox)(ref boundingBox)).Max;
			((Circle)(ref val2))._002Ector(worldXY, val, ((Point3d)(ref max)).X);
			((Circle)(ref val2)).Reverse();
			NurbsCurve obj = ((Circle)(ref val2)).ToNurbsCurve();
			BoundingBox boundingBox2 = ((GeometryBase)obj).GetBoundingBox(false);
			max = ((BoundingBox)(ref boundingBox2)).Max;
			double num5 = 0.0 - ((Point3d)(ref max)).X;
			max = ((BoundingBox)(ref boundingBox2)).Max;
			double num6 = 0.0 - ((Point3d)(ref max)).Y;
			if (num < num5)
			{
				num = num5 + 0.01;
			}
			else if (num < num6)
			{
				num = num6 + 0.01;
			}
			if (!OffsetZ((Curve)(object)obj, num, placement, out var outputCurve))
			{
				return UpdateError("OffsetZ Failed");
			}
			BoundingBox boundingBox3 = ((GeometryBase)outputCurve).GetBoundingBox(false);
			max = ((BoundingBox)(ref boundingBox3)).Max;
			num5 = 0.0 - ((Point3d)(ref max)).X;
			max = ((BoundingBox)(ref boundingBox3)).Max;
			num6 = 0.0 - ((Point3d)(ref max)).Y;
			if (num2 < num5)
			{
				num2 = num5 + 0.01;
			}
			else if (num2 < num6)
			{
				num2 = num6 + 0.01;
			}
			if (!OffsetZ((Curve)(object)outputCurve, num2, placement2, out var outputCurve2))
			{
				return UpdateError("OffsetZ Failed");
			}
			if (num3 < num5)
			{
				num3 = num5 + 0.01;
			}
			else if (num3 < num6)
			{
				num3 = num6 + 0.01;
			}
			if (!OffsetZ((Curve)(object)outputCurve, num3, placement3, out var outputCurve3))
			{
				return UpdateError("OffsetZ Failed");
			}
			BoundingBox boundingBox4 = ((GeometryBase)outputCurve3).GetBoundingBox(false);
			max = ((BoundingBox)(ref boundingBox4)).Max;
			num5 = 0.0 - ((Point3d)(ref max)).X;
			max = ((BoundingBox)(ref boundingBox4)).Max;
			num6 = 0.0 - ((Point3d)(ref max)).Y;
			if (num4 < num5)
			{
				num4 = num5 + 0.01;
			}
			else if (num4 < num6)
			{
				num4 = num6 + 0.01;
			}
			if (!OffsetZ((Curve)(object)outputCurve3, num4, placement4, out var outputCurve4))
			{
				return UpdateError("OffsetZ Failed");
			}
			Brep val3 = CSUtilSweepRC.Loft((IEnumerable<Curve>)(object)new Curve[4]
			{
				(Curve)outputCurve2,
				(Curve)outputCurve,
				(Curve)outputCurve3,
				(Curve)outputCurve4
			}, (LoftType)3, doCapped: true);
			((GeometryBase)val3).Transform(xUp);
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val3, xUp, applyTransform: false));
		}
		OutBrep.SetData(cSDataArray);
		return base.Update();
	}

	private static bool OffsetZ(Curve crv, double centerSpacing, double placement, out NurbsCurve outputCurve)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		outputCurve = null;
		Curve val = CSUtilGeometry.OriginOffsetOldXY(crv, centerSpacing, closed: true);
		if (val == null)
		{
			val = CSUtilGeometry.OriginOffsetOldXY(crv, 0.0, closed: true);
			if (val == null)
			{
				CSUtil.PrintLine("Bezel Cutter Offset Curve Unable to Create Output");
				return false;
			}
		}
		Transform val2 = Transform.Translation(0.0, 0.0, placement);
		outputCurve = val.ToNurbsCurve();
		((GeometryBase)outputCurve).Transform(val2);
		return true;
	}
}
