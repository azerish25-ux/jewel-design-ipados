using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlocks.Curves;

[CSBlock("ChannelBorder", "{3FC8E180-E3EB-416C-AC3C-B959B8BE8097}")]
public sealed class CSChannelBorder : CSBlock
{
	public readonly CSInputChannel InSortedGems;

	public readonly CSInputChannel InChannelSideSpacing;

	public readonly CSInputChannel InChannelEndProfile;

	public readonly CSInputChannel InChannelEndSpacingBoth;

	public readonly CSInputChannel InChannelEndSpacingStart;

	public readonly CSInputChannel InChannelEndSpacingEnd;

	public readonly CSInputChannel InChannelEndExtendType;

	public readonly CSOutputChannel OutCurveStart;

	public readonly CSOutputChannel OutCurveEnd;

	public readonly CSOutputChannel OutCurveLeft;

	public readonly CSOutputChannel OutCurveRight;

	public CSChannelBorder(string identifier)
		: base(identifier)
	{
		InSortedGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InChannelSideSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEndProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InChannelEndSpacingBoth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEndSpacingStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEndSpacingEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEndExtendType = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutCurveStart = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutCurveEnd = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutCurveLeft = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutCurveRight = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!(InSortedGems.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Missing Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Gems");
		}
		CSDataHelper.GetNurbsCurve(InChannelEndProfile.Data, out var value);
		if (value == null)
		{
			Point3d point3d = new Point3d(-5.0, 0.0, 0.0);
			Point3d to = new Point3d(5.0, 0.0, 0.0);
			value = new LineCurve(point3d, to).ToNurbsCurve();
		}
		double value2 = 0.0;
		InChannelSideSpacing.GetDouble(ref value2);
		double value3 = 0.0;
		InChannelEndSpacingBoth.GetDouble(ref value3);
		double value4 = 0.0;
		InChannelEndSpacingStart.GetDouble(ref value4);
		value4 += value3;
		double value5 = 0.0;
		InChannelEndSpacingEnd.GetDouble(ref value5);
		value5 += value3;
		ValueListItem whichItem = ValueListItem.ExtendCurveTypeSmooth;
		InChannelEndExtendType.GetValueListItem(ref whichItem);
		if (!CalculateChannelBorderCurves(cSDataArray, value, value2, value4, value5, whichItem, out var crvStart, out var crvEnd, out var crvLeft, out var crvRight, out var msg))
		{
			return UpdateError(msg);
		}
		OutCurveStart.SetData((crvStart == null) ? null : new CSOnGeometryData(crvStart));
		OutCurveEnd.SetData((crvEnd == null) ? null : new CSOnGeometryData(crvEnd));
		OutCurveLeft.SetData(new CSOnGeometryData(crvLeft));
		OutCurveRight.SetData(new CSOnGeometryData(crvRight));
		return base.Update();
	}

	private static bool CalculateChannelBorderCurves(CSDataArray channelGemDatas, Curve channelEndProfile, double channelSideSpacing, double channelEndSpacingStart, double channelEndSpacingEnd, ValueListItem channelEndExtendType, out Curve crvStart, out Curve crvEnd, out Curve crvLeft, out Curve crvRight, out string msg)
	{
		crvStart = null;
		crvEnd = null;
		crvLeft = null;
		crvRight = null;
		msg = string.Empty;
		OnCurve onCurve = CSUtilGemRails.Rail(channelGemDatas);
		if (!CSUtilGemRails.CalculateLeft(channelGemDatas.Array, onCurve, channelSideSpacing, 0.0, out crvLeft))
		{
			msg = "CalculateLeft failed";
			return false;
		}
		if (!CSUtilGemRails.CalculateRight(channelGemDatas.Array, onCurve, channelSideSpacing, 0.0, out crvRight))
		{
			msg = "CalculateRight failed";
			return false;
		}
		if (CSUtilGemRails.IsClosed(channelGemDatas.Array))
		{
			return true;
		}
		CurveExtensionStyle style;
		switch (channelEndExtendType)
		{
		case ValueListItem.ExtendCurveTypeLine:
			style = CurveExtensionStyle.Line;
			break;
		case ValueListItem.ExtendCurveTypeArc:
			style = CurveExtensionStyle.Arc;
			break;
		case ValueListItem.ExtendCurveTypeSmooth:
			style = CurveExtensionStyle.Smooth;
			break;
		default:
			msg = "Invalid extend curve type";
			return false;
		}
		double length = Interop.FromOnCurve(onCurve.DuplicateCurve()).GetLength();
		double num = double.NaN;
		num = crvLeft.GetLength();
		double length2 = crvRight.GetLength();
		double num2 = num / length;
		_ = length2 / length;
		crvLeft = crvLeft.Extend(CurveEnd.Start, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart) * num2, style);
		if (!crvLeft.IsValid)
		{
			msg = "RhinoExtendCurve failed (Left Start)";
			return false;
		}
		crvLeft = crvLeft.Extend(CurveEnd.End, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart) * num2, style);
		if (!crvLeft.IsValid)
		{
			msg = "RhinoExtendCurve failed (Left End)";
			return false;
		}
		crvRight = crvRight.Extend(CurveEnd.Start, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart) * num2, style);
		if (!crvRight.IsValid)
		{
			msg = "RhinoExtendCurve failed (Right Start)";
			return false;
		}
		crvRight = crvRight.Extend(CurveEnd.End, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart) * num2, style);
		if (!crvRight.IsValid)
		{
			msg = "RhinoExtendCurve failed (Right End)";
			return false;
		}
		BoundingBox boundingBox = channelEndProfile.GetBoundingBox(accurate: true);
		if (!boundingBox.IsValid)
		{
			msg = "GetTightBoundingBox failed";
			return false;
		}
		double num3 = boundingBox.Max.X - boundingBox.Min.X;
		double num4 = crvLeft.PointAtStart.DistanceTo(crvRight.PointAtStart);
		double num5 = num4 / num3;
		Transform transform = Transform.Scale(Plane.WorldXY, num5, num5, num5);
		double num6 = crvLeft.PointAtEnd.DistanceTo(crvRight.PointAtEnd);
		double num7 = num6 / num3;
		Transform transform2 = Transform.Scale(Plane.WorldXY, num7, num7, num7);
		Point3d source = new Point3d(0.0 - num4 / 2.0, 0.0, 0.0);
		Vector3d xAxis = Vector3d.XAxis;
		Vector3d yAxis = Vector3d.YAxis;
		Point3d destination = new Point3d(crvRight.PointAtStart);
		Vector3d vector3d = new Vector3d(crvLeft.PointAtStart - crvRight.PointAtStart);
		vector3d.Unitize();
		Vector3d vector3d2 = new Vector3d(crvLeft.TangentAt(crvLeft.Domain.Min) + crvRight.TangentAt(crvRight.Domain.Min));
		vector3d2.Reverse();
		vector3d2.Unitize();
		Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(source, xAxis, yAxis, destination, vector3d, vector3d2);
		Point3d source2 = new Point3d(0.0 - num6 / 2.0, 0.0, 0.0);
		Vector3d xAxis2 = Vector3d.XAxis;
		Vector3d yAxis2 = Vector3d.YAxis;
		Point3d destination2 = new Point3d(crvRight.PointAtEnd);
		Vector3d vector3d3 = new Vector3d(crvLeft.PointAtEnd - crvRight.PointAtEnd);
		vector3d3.Unitize();
		Vector3d vector3d4 = new Vector3d(crvLeft.TangentAt(crvLeft.Domain.Max) + crvRight.TangentAt(crvRight.Domain.Max));
		vector3d4.Unitize();
		Transform transform4 = CSBlockUtil.CSUtilMath.Orient3Point(source2, xAxis2, yAxis2, destination2, vector3d3, vector3d4);
		Curve curve = channelEndProfile.DuplicateCurve();
		curve.Transform(transform3 * transform);
		Curve curve2 = channelEndProfile.DuplicateCurve();
		curve2.Transform(transform4 * transform2);
		if (!CSUtilGemRails.CalculateLeft(channelGemDatas.Array, onCurve, channelSideSpacing + 1.0, 0.0, out Curve outRail))
		{
			msg = "CalculateLeft failed";
			return false;
		}
		if (!CSUtilGemRails.CalculateRight(channelGemDatas.Array, onCurve, channelSideSpacing + 1.0, 0.0, out Curve outRail2))
		{
			msg = "CalculateRight failed";
			return false;
		}
		double num8 = double.NaN;
		num8 = outRail.GetLength();
		double length3 = outRail2.GetLength();
		double num9 = num8 / length;
		_ = length3 / length;
		BoundingBox boundingBox2 = boundingBox;
		boundingBox2.Transform(transform);
		_ = boundingBox2.Max.Y;
		_ = boundingBox2.Min.Y;
		BoundingBox boundingBox3 = boundingBox;
		boundingBox3.Transform(transform2);
		double num10 = boundingBox3.Max.Y - boundingBox3.Min.Y;
		outRail = outRail.Extend(CurveEnd.Start, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd + num10 + 1.0) * num9, style);
		if (!outRail.IsValid)
		{
			msg = "RhinoExtendCurve failed (Left Extra Start)";
			return false;
		}
		outRail = outRail.Extend(CurveEnd.End, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd + num10 + 1.0) * num9, style);
		if (!outRail.IsValid)
		{
			msg = "RhinoExtendCurve failed (Left Extra End)";
			return false;
		}
		outRail2 = outRail2.Extend(CurveEnd.Start, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd + num10 + 1.0) * num9, style);
		if (!outRail2.IsValid)
		{
			msg = "RhinoExtendCurve failed (Right Extra Start)";
			return false;
		}
		outRail2 = outRail2.Extend(CurveEnd.End, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd + num10 + 1.0) * num9, style);
		if (!outRail2.IsValid)
		{
			msg = "RhinoExtendCurve failed (Right Extra End)";
			return false;
		}
		NurbsSurface nurbsSurface = NurbsSurface.CreateRuledSurface(outRail, outRail2);
		if (!nurbsSurface.IsValid)
		{
			msg = "CreateRuledSurface failed";
			return false;
		}
		Vector3d profileNormalRC = Vector3d.CrossProduct(vector3d, vector3d2);
		if (!CSUtilCurves.CurveFrom2ViewSimple(curve, profileNormalRC, nurbsSurface, out crvStart, out var msg2))
		{
			msg = msg2 + " (Start)";
			return false;
		}
		Vector3d profileNormalRC2 = Vector3d.CrossProduct(vector3d3, vector3d4);
		if (!CSUtilCurves.CurveFrom2ViewSimple(curve2, profileNormalRC2, nurbsSurface, out crvEnd, out var msg3))
		{
			msg = msg3 + " (End)";
			return false;
		}
		return true;
	}
}
