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
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value);
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(value, out var xUp, out var xDown);
			Brep brep = arrayItem.GetBrep();
			brep.Transform(xDown);
			BoundingBox boundingBox = brep.GetBoundingBox(accurate: true);
			Circle circle = new Circle(Plane.WorldXY, new Point3d(0.0, 0.0, boundingBox.Max.Z), boundingBox.Max.X);
			circle.Reverse();
			NurbsCurve nurbsCurve = circle.ToNurbsCurve();
			BoundingBox boundingBox2 = nurbsCurve.GetBoundingBox(accurate: false);
			double num5 = 0.0 - boundingBox2.Max.X;
			double num6 = 0.0 - boundingBox2.Max.Y;
			if (num < num5)
			{
				num = num5 + 0.01;
			}
			else if (num < num6)
			{
				num = num6 + 0.01;
			}
			if (!OffsetZ(nurbsCurve, num, placement, out var crvOffset))
			{
				return UpdateError("OffsetZ Failed");
			}
			BoundingBox boundingBox3 = crvOffset.GetBoundingBox(accurate: false);
			num5 = 0.0 - boundingBox3.Max.X;
			num6 = 0.0 - boundingBox3.Max.Y;
			if (num2 < num5)
			{
				num2 = num5 + 0.01;
			}
			else if (num2 < num6)
			{
				num2 = num6 + 0.01;
			}
			if (!OffsetZ(crvOffset, num2, placement2, out var crvOffset2))
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
			if (!OffsetZ(crvOffset, num3, placement3, out var crvOffset3))
			{
				return UpdateError("OffsetZ Failed");
			}
			BoundingBox boundingBox4 = crvOffset3.GetBoundingBox(accurate: false);
			num5 = 0.0 - boundingBox4.Max.X;
			num6 = 0.0 - boundingBox4.Max.Y;
			if (num4 < num5)
			{
				num4 = num5 + 0.01;
			}
			else if (num4 < num6)
			{
				num4 = num6 + 0.01;
			}
			if (!OffsetZ(crvOffset3, num4, placement4, out var crvOffset4))
			{
				return UpdateError("OffsetZ Failed");
			}
			Brep brep2 = CSUtilSweepRC.Loft(new Curve[4] { crvOffset2, crvOffset, crvOffset3, crvOffset4 }, LoftType.Straight, doCapped: true);
			brep2.Transform(xUp);
			cSDataArray.Append(new CSOnGeometryData(brep2, xUp, applyTransform: false));
		}
		OutBrep.SetData(cSDataArray);
		return base.Update();
	}

	private static bool OffsetZ(Curve crv, double centerSpacing, double placement, out NurbsCurve crvOffset)
	{
		crvOffset = null;
		Curve curve = CSUtilGeometry.OriginOffsetXY(crv, centerSpacing);
		if (curve == null)
		{
			curve = CSUtilGeometry.OriginOffsetXY(crv, 0.0);
			if (curve == null)
			{
				CSUtil.PrintLine("Bezel Cutter Offset Curve Unable to Create Output");
				return false;
			}
		}
		Transform xform = Transform.Translation(0.0, 0.0, placement);
		crvOffset = curve.ToNurbsCurve();
		crvOffset.Transform(xform);
		return true;
	}
}
