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
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
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
			Point3d val = new Point3d(-5.0, 0.0, 0.0);
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(5.0, 0.0, 0.0);
			value = ((Curve)new LineCurve(val, val2)).ToNurbsCurve();
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
		if (!CalculateChannelBorderCurves(cSDataArray, (Curve)(object)value, value2, value4, value5, whichItem, out var crvStart, out var crvEnd, out var crvLeft, out var crvRight, out var msg))
		{
			return UpdateError(msg);
		}
		OutCurveStart.SetData((crvStart == null) ? null : new CSOnGeometryData((GeometryBase)(object)crvStart));
		OutCurveEnd.SetData((crvEnd == null) ? null : new CSOnGeometryData((GeometryBase)(object)crvEnd));
		OutCurveLeft.SetData(new CSOnGeometryData((GeometryBase)(object)crvLeft));
		OutCurveRight.SetData(new CSOnGeometryData((GeometryBase)(object)crvRight));
		return base.Update();
	}

	private static bool CalculateChannelBorderCurves(CSDataArray channelGemDatas, Curve channelEndProfile, double channelSideSpacing, double channelEndSpacingStart, double channelEndSpacingEnd, ValueListItem channelEndExtendType, out Curve crvStart, out Curve crvEnd, out Curve crvLeft, out Curve crvRight, out string msg)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		crvStart = null;
		crvEnd = null;
		crvLeft = null;
		crvRight = null;
		msg = string.Empty;
		OnCurve val = CSUtilGemRails.Rail(channelGemDatas);
		if (!CSUtilGemRails.CalculateLeft(channelGemDatas.Array, val, channelSideSpacing, 0.0, out crvLeft))
		{
			msg = "CalculateLeft failed";
			return false;
		}
		if (!CSUtilGemRails.CalculateRight(channelGemDatas.Array, val, channelSideSpacing, 0.0, out crvRight))
		{
			msg = "CalculateRight failed";
			return false;
		}
		if (CSUtilGemRails.IsClosed(channelGemDatas.Array))
		{
			return true;
		}
		CurveExtensionStyle val2;
		switch (channelEndExtendType)
		{
		case ValueListItem.ExtendCurveTypeLine:
			val2 = (CurveExtensionStyle)0;
			break;
		case ValueListItem.ExtendCurveTypeArc:
			val2 = (CurveExtensionStyle)1;
			break;
		case ValueListItem.ExtendCurveTypeSmooth:
			val2 = (CurveExtensionStyle)2;
			break;
		default:
			msg = "Invalid extend curve type";
			return false;
		}
		double length = Interop.FromOnCurve((object)val.DuplicateCurve()).GetLength();
		double num = double.NaN;
		num = crvLeft.GetLength();
		double length2 = crvRight.GetLength();
		double num2 = num / length;
		_ = length2 / length;
		crvLeft = crvLeft.Extend((CurveEnd)1, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart) * num2, val2);
		if (!((CommonObject)crvLeft).IsValid)
		{
			msg = "RhinoExtendCurve failed (Left Start)";
			return false;
		}
		crvLeft = crvLeft.Extend((CurveEnd)2, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart) * num2, val2);
		if (!((CommonObject)crvLeft).IsValid)
		{
			msg = "RhinoExtendCurve failed (Left End)";
			return false;
		}
		crvRight = crvRight.Extend((CurveEnd)1, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart) * num2, val2);
		if (!((CommonObject)crvRight).IsValid)
		{
			msg = "RhinoExtendCurve failed (Right Start)";
			return false;
		}
		crvRight = crvRight.Extend((CurveEnd)2, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart) * num2, val2);
		if (!((CommonObject)crvRight).IsValid)
		{
			msg = "RhinoExtendCurve failed (Right End)";
			return false;
		}
		BoundingBox boundingBox = ((GeometryBase)channelEndProfile).GetBoundingBox(true);
		if (!((BoundingBox)(ref boundingBox)).IsValid)
		{
			msg = "GetTightBoundingBox failed";
			return false;
		}
		Point3d val3 = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val3)).X;
		val3 = ((BoundingBox)(ref boundingBox)).Min;
		double num3 = x - ((Point3d)(ref val3)).X;
		val3 = crvLeft.PointAtStart;
		double num4 = ((Point3d)(ref val3)).DistanceTo(crvRight.PointAtStart);
		double num5 = num4 / num3;
		Transform val4 = Transform.Scale(Plane.WorldXY, num5, num5, num5);
		val3 = crvLeft.PointAtEnd;
		double num6 = ((Point3d)(ref val3)).DistanceTo(crvRight.PointAtEnd);
		double num7 = num6 / num3;
		Transform val5 = Transform.Scale(Plane.WorldXY, num7, num7, num7);
		Point3d source = new Point3d(0.0 - num4 / 2.0, 0.0, 0.0);
		Vector3d xAxis = Vector3d.XAxis;
		Vector3d yAxis = Vector3d.YAxis;
		Point3d destination = default(Point3d);
		((Point3d)(ref destination))._002Ector(crvRight.PointAtStart);
		Vector3d val6 = default(Vector3d);
		((Vector3d)(ref val6))._002Ector(crvLeft.PointAtStart - crvRight.PointAtStart);
		((Vector3d)(ref val6)).Unitize();
		Curve obj = crvLeft;
		Interval domain = crvLeft.Domain;
		Vector3d val7 = obj.TangentAt(((Interval)(ref domain)).Min);
		Curve obj2 = crvRight;
		domain = crvRight.Domain;
		Vector3d val8 = default(Vector3d);
		((Vector3d)(ref val8))._002Ector(val7 + obj2.TangentAt(((Interval)(ref domain)).Min));
		((Vector3d)(ref val8)).Reverse();
		((Vector3d)(ref val8)).Unitize();
		Transform val9 = CSBlockUtil.CSUtilMath.Orient3Point(source, xAxis, yAxis, destination, val6, val8);
		Point3d source2 = new Point3d(0.0 - num6 / 2.0, 0.0, 0.0);
		Vector3d xAxis2 = Vector3d.XAxis;
		Vector3d yAxis2 = Vector3d.YAxis;
		Point3d destination2 = default(Point3d);
		((Point3d)(ref destination2))._002Ector(crvRight.PointAtEnd);
		Vector3d val10 = default(Vector3d);
		((Vector3d)(ref val10))._002Ector(crvLeft.PointAtEnd - crvRight.PointAtEnd);
		((Vector3d)(ref val10)).Unitize();
		Curve obj3 = crvLeft;
		domain = crvLeft.Domain;
		Vector3d val11 = obj3.TangentAt(((Interval)(ref domain)).Max);
		Curve obj4 = crvRight;
		domain = crvRight.Domain;
		Vector3d val12 = default(Vector3d);
		((Vector3d)(ref val12))._002Ector(val11 + obj4.TangentAt(((Interval)(ref domain)).Max));
		((Vector3d)(ref val12)).Unitize();
		Transform val13 = CSBlockUtil.CSUtilMath.Orient3Point(source2, xAxis2, yAxis2, destination2, val10, val12);
		Curve val14 = channelEndProfile.DuplicateCurve();
		((GeometryBase)val14).Transform(val9 * val4);
		Curve val15 = channelEndProfile.DuplicateCurve();
		((GeometryBase)val15).Transform(val13 * val5);
		if (!CSUtilGemRails.CalculateLeft(channelGemDatas.Array, val, channelSideSpacing + 1.0, 0.0, out Curve outRail))
		{
			msg = "CalculateLeft failed";
			return false;
		}
		if (!CSUtilGemRails.CalculateRight(channelGemDatas.Array, val, channelSideSpacing + 1.0, 0.0, out Curve outRail2))
		{
			msg = "CalculateRight failed";
			return false;
		}
		double num8 = double.NaN;
		num8 = outRail.GetLength();
		double length3 = outRail2.GetLength();
		double num9 = num8 / length;
		_ = length3 / length;
		BoundingBox val16 = boundingBox;
		((BoundingBox)(ref val16)).Transform(val4);
		val3 = ((BoundingBox)(ref val16)).Max;
		_ = ((Point3d)(ref val3)).Y;
		val3 = ((BoundingBox)(ref val16)).Min;
		_ = ((Point3d)(ref val3)).Y;
		BoundingBox val17 = boundingBox;
		((BoundingBox)(ref val17)).Transform(val5);
		val3 = ((BoundingBox)(ref val17)).Max;
		double y = ((Point3d)(ref val3)).Y;
		val3 = ((BoundingBox)(ref val17)).Min;
		double num10 = y - ((Point3d)(ref val3)).Y;
		outRail = outRail.Extend((CurveEnd)1, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd + num10 + 1.0) * num9, val2);
		if (!((CommonObject)outRail).IsValid)
		{
			msg = "RhinoExtendCurve failed (Left Extra Start)";
			return false;
		}
		outRail = outRail.Extend((CurveEnd)2, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd + num10 + 1.0) * num9, val2);
		if (!((CommonObject)outRail).IsValid)
		{
			msg = "RhinoExtendCurve failed (Left Extra End)";
			return false;
		}
		outRail2 = outRail2.Extend((CurveEnd)1, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd + num10 + 1.0) * num9, val2);
		if (!((CommonObject)outRail2).IsValid)
		{
			msg = "RhinoExtendCurve failed (Right Extra Start)";
			return false;
		}
		outRail2 = outRail2.Extend((CurveEnd)2, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd + num10 + 1.0) * num9, val2);
		if (!((CommonObject)outRail2).IsValid)
		{
			msg = "RhinoExtendCurve failed (Right Extra End)";
			return false;
		}
		NurbsSurface val18 = NurbsSurface.CreateRuledSurface(outRail, outRail2);
		if (!((CommonObject)val18).IsValid)
		{
			msg = "CreateRuledSurface failed";
			return false;
		}
		Vector3d profileNormalRC = Vector3d.CrossProduct(val6, val8);
		if (!CSUtilCurves.CurveFrom2ViewSimple(val14, profileNormalRC, (Surface)(object)val18, out crvStart, out var msg2))
		{
			msg = msg2 + " (Start)";
			return false;
		}
		Vector3d profileNormalRC2 = Vector3d.CrossProduct(val10, val12);
		if (!CSUtilCurves.CurveFrom2ViewSimple(val15, profileNormalRC2, (Surface)(object)val18, out crvEnd, out var msg3))
		{
			msg = msg3 + " (End)";
			return false;
		}
		return true;
	}
}
