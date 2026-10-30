using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Cutters;

[CSBlock("BezelCutter", "{0C6E7B73-8365-4B34-ADD9-68604377C959}")]
public sealed class CSBezelCutter : CSBlock
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

	public CSBezelCutter(string identifier)
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
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InBezel.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Bezel Cutter: Missing Bezel");
		}
		double num = InCenterSpacing.Double * -1.0;
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
			BoundingBox boundingBox = ((GeometryBase)brep).GetBoundingBox(true);
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
			if (!OffsetZ((Curve)(object)obj, num, placement, out var crvOffset))
			{
				return UpdateError("OffsetZ Failed");
			}
			BoundingBox boundingBox3 = ((GeometryBase)crvOffset).GetBoundingBox(false);
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
			if (!OffsetZ((Curve)(object)crvOffset, num2, placement2, out var crvOffset2))
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
			if (!OffsetZ((Curve)(object)crvOffset, num3, placement3, out var crvOffset3))
			{
				return UpdateError("OffsetZ Failed");
			}
			BoundingBox boundingBox4 = ((GeometryBase)crvOffset3).GetBoundingBox(false);
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
			if (!OffsetZ((Curve)(object)crvOffset3, num4, placement4, out var crvOffset4))
			{
				return UpdateError("OffsetZ Failed");
			}
			Brep val3 = CSUtilSweepRC.Loft((IEnumerable<Curve>)(object)new Curve[4]
			{
				(Curve)crvOffset2,
				(Curve)crvOffset,
				(Curve)crvOffset3,
				(Curve)crvOffset4
			}, (LoftType)3, doCapped: true);
			((GeometryBase)val3).Transform(xUp);
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val3, xUp, applyTransform: false));
		}
		OutBrep.SetData(cSDataArray);
		return base.Update();
	}

	private static bool OffsetZ(Curve crv, double centerSpacing, double placement, out NurbsCurve crvOffset)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		crvOffset = null;
		Curve val = CSUtilGeometry.OriginOffsetXY(crv, centerSpacing);
		if (val == null)
		{
			val = CSUtilGeometry.OriginOffsetXY(crv, 0.0);
			if (val == null)
			{
				CSUtil.PrintLine("Bezel Cutter Offset Curve Unable to Create Output");
				return false;
			}
		}
		Transform val2 = Transform.Translation(0.0, 0.0, placement);
		crvOffset = val.ToNurbsCurve();
		((GeometryBase)crvOffset).Transform(val2);
		return true;
	}
}
