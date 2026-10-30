using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Cutters;

[CSBlock("ChannelFromGems", "{F3FA8F61-6A90-4AD4-8C42-9251007CBE92}")]
public sealed class CSChannelFromGems : CSBlock
{
	private enum EndStyle
	{
		Capped,
		Round,
		Flat,
		SharpCorner
	}

	public CSInputChannel InSortedGemArray { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InScaleXStart { get; }

	public CSInputChannel InScaleXEnd { get; }

	public CSInputChannel InScaleYStart { get; }

	public CSInputChannel InScaleYEnd { get; }

	public CSInputChannel InVOffsetStart { get; }

	public CSInputChannel InVOffsetEnd { get; }

	public CSInputChannel InStartSizeScalar { get; }

	public CSInputChannel InEndSizeScalar { get; }

	public CSInputChannel InStartTaper { get; }

	public CSInputChannel InEndTaper { get; }

	public CSInputChannel InStartEndType { get; }

	public CSInputChannel InEndEndType { get; }

	public CSInputChannel InMiddleProfileCount { get; }

	public CSInputChannel InMirror { get; }

	public CSOutputChannel OutBrep { get; }

	public CSChannelFromGems(string identifier)
		: base(identifier)
	{
		InSortedGemArray = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InScaleXStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InScaleXEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InScaleYStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InScaleYEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InVOffsetStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InVOffsetEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartSizeScalar = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndSizeScalar = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStartEndType = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndEndType = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMiddleProfileCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!(InSortedGemArray.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems Count 0");
		}
		OnNurbsCurve nurbsCurve = InProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Profile");
		}
		OnBoundingBox tight_bbox = new OnBoundingBox();
		nurbsCurve.GetTightBoundingBox(ref tight_bbox);
		OnCurve crv = CSUtilGemRails.Rail(cSDataArray.Array).DuplicateCurve();
		OnCurve crv2 = CSUtilGemRails.Right(cSDataArray, crv).DuplicateCurve();
		OnCurve crv3 = CSUtilGemRails.Left(cSDataArray, crv).DuplicateCurve();
		if (crv2 == null || crv3 == null)
		{
			return UpdateError("Failed Duplicate");
		}
		double length = 0.0;
		double length2 = 0.0;
		RhUtil.RhinoExtendCurve(ref crv2, IRhinoExtend.Type.Line, 0, length);
		RhUtil.RhinoExtendCurve(ref crv, IRhinoExtend.Type.Line, 0, length);
		RhUtil.RhinoExtendCurve(ref crv3, IRhinoExtend.Type.Line, 0, length);
		RhUtil.RhinoExtendCurve(ref crv2, IRhinoExtend.Type.Line, 1, length2);
		RhUtil.RhinoExtendCurve(ref crv, IRhinoExtend.Type.Line, 1, length2);
		RhUtil.RhinoExtendCurve(ref crv3, IRhinoExtend.Type.Line, 1, length2);
		OnXform onXform = new OnXform();
		OnXform onXform2 = new OnXform();
		double value = 1.0;
		double value2 = 1.0;
		InScaleXEnd.GetDouble(ref value2);
		InScaleXStart.GetDouble(ref value);
		double value3 = 1.0;
		double value4 = 1.0;
		InScaleYEnd.GetDouble(ref value4);
		InScaleYStart.GetDouble(ref value3);
		double value5 = 0.0;
		double value6 = 0.0;
		InVOffsetEnd.GetDouble(ref value6);
		InVOffsetStart.GetDouble(ref value5);
		double value7 = 1.0;
		InEndSizeScalar.GetDouble(ref value7);
		double value8 = 1.0;
		InStartSizeScalar.GetDouble(ref value8);
		double value9 = 0.0;
		InEndTaper.GetDouble(ref value9);
		double value10 = 0.0;
		InStartTaper.GetDouble(ref value10);
		int value11 = 0;
		InMiddleProfileCount.GetInteger(ref value11);
		int value12 = 0;
		InEndEndType.GetInteger(ref value12);
		int value13 = 0;
		InStartEndType.GetInteger(ref value13);
		bool value14 = false;
		InMirror.GetBoolean(ref value14);
		if (!value14)
		{
			value2 = value;
			value4 = value3;
			value6 = value5;
			value7 = value8;
			value9 = value10;
			value12 = value13;
		}
		onXform.Scale(CSUtilGemRails.SizeStart(cSDataArray) / tight_bbox.Max().x * value, value3 / tight_bbox.Max().y, 1.0);
		onXform2.Scale(CSUtilGemRails.SizeEnd(cSDataArray) / tight_bbox.Max().x * value2, value4 / tight_bbox.Max().y, 1.0);
		OnXform onXform3 = new OnXform();
		OnXform onXform4 = new OnXform();
		onXform3.Translation(0.0, value5, 0.0);
		onXform4.Translation(0.0, value6, 0.0);
		IOn3dVector arg = CSUtilGemRails.GetChannelGemData(cSDataArray, 0).Up.Convert();
		IOn3dVector on3dVector = crv.TangentAt(crv.Domain().Min());
		IOn3dVector on3dVector2 = OnUtil.ON_CrossProduct(on3dVector, arg);
		arg = OnUtil.ON_CrossProduct(on3dVector2, on3dVector);
		OnCurve onCurve = CSUtilGemRails.Rail(cSDataArray.Array).DuplicateCurve();
		OnCurve onCurve2 = CSUtilGemRails.Right(cSDataArray, onCurve).DuplicateCurve();
		OnCurve onCurve3 = CSUtilGemRails.Left(cSDataArray, onCurve).DuplicateCurve();
		OnXform onXform5 = new OnXform();
		onXform5.ChangeBasis(onCurve.PointAtStart(), on3dVector2, arg, on3dVector, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
		OnNurbsCurve onNurbsCurve = nurbsCurve.Duplicate();
		onNurbsCurve.Transform(onXform5 * onXform3 * onXform);
		int num = (cSDataArray.Array.Count - 1) / (value11 + 1);
		if (num < 1)
		{
			num = 1;
		}
		List<IOnCurve> list = new List<IOnCurve>();
		List<IOn3dPoint> list2 = new List<IOn3dPoint>();
		List<IOn3dPoint> list3 = new List<IOn3dPoint>();
		OnPolyline onPolyline = new OnPolyline();
		onPolyline.Append(new On3dPoint(value, value3, 0.0));
		onPolyline.Append(new On3dPoint(value2, value4, 0.0));
		OnPolyline onPolyline2 = new OnPolyline();
		onPolyline2.Append(new On3dPoint(0.0, value5, 0.0));
		onPolyline2.Append(new On3dPoint(0.0, value6, 0.0));
		for (int i = num; i < cSDataArray.Array.Count - 1; i += num)
		{
			OnXform onXform6 = new OnXform();
			OnXform onXform7 = new OnXform();
			double t = (double)i / (double)(cSDataArray.Array.Count - 1);
			double x = onPolyline.PointAt(t).x;
			double y = onPolyline.PointAt(t).y;
			double y2 = onPolyline2.PointAt(t).y;
			double size = CSUtilGemRails.GetChannelGemData(cSDataArray, i).Size;
			onXform7.Translation(0.0, y2, 0.0);
			onXform6.Scale(size / tight_bbox.Max().x * x, y / tight_bbox.Max().y, 0.0);
			IOn3dPoint p = CSUtilGemRails.GetChannelGemData(cSDataArray, i).Center.Convert();
			arg = CSUtilGemRails.GetChannelGemData(cSDataArray, i).Up.Convert();
			on3dVector2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i).UnitRightVector.Convert();
			on3dVector = OnUtil.ON_CrossProduct(on3dVector2, arg);
			OnXform onXform8 = new OnXform();
			onXform8.ChangeBasis(p, on3dVector2, arg, on3dVector, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
			OnNurbsCurve onNurbsCurve2 = nurbsCurve.Duplicate();
			onNurbsCurve2.Transform(onXform8 * onXform7 * onXform6);
			list.Add(onNurbsCurve2);
			list2.Add(CSUtilGemRails.GetChannelGemData(cSDataArray, i).Right.Convert());
			list3.Add(CSUtilGemRails.GetChannelGemData(cSDataArray, i).Left.Convert());
		}
		arg = CSUtilGemRails.GetChannelGemData(cSDataArray, cSDataArray.Array.Count - 1).Up.Convert();
		on3dVector = crv.TangentAt(crv.Domain().Max());
		on3dVector2 = OnUtil.ON_CrossProduct(on3dVector, arg);
		arg = OnUtil.ON_CrossProduct(on3dVector2, on3dVector);
		OnXform onXform9 = new OnXform();
		onXform9.ChangeBasis(onCurve.PointAtEnd(), on3dVector2, arg, on3dVector, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
		OnNurbsCurve onNurbsCurve3 = nurbsCurve.Duplicate();
		onNurbsCurve3.Transform(onXform9 * onXform4 * onXform2);
		List<double> list4 = new List<double>();
		List<double> list5 = new List<double>();
		List<IOnCurve> list6 = new List<IOnCurve>();
		list4.Add(onCurve2.Domain().Min());
		list5.Add(onCurve3.Domain().Min());
		list6.Add(onNurbsCurve);
		for (int j = 0; j < list.Count; j++)
		{
			double t2 = 0.0;
			onCurve2.GetClosestPoint(list2[j], ref t2);
			list4.Add(t2);
			onCurve3.GetClosestPoint(list3[j], ref t2);
			list5.Add(t2);
			list6.Add(list[j]);
		}
		bool num2 = CSUtilGemRails.IsClosed(cSDataArray.Array);
		if (!num2)
		{
			list6.Add(onNurbsCurve3);
			list4.Add(onCurve2.Domain().Max());
			list5.Add(onCurve3.Domain().Max());
		}
		CSUtilSweep.Sweep2(onCurve2, onCurve3, list6.ToArray(), list4.ToArray(), list5.ToArray(), out var breps);
		List<OnBrep> list7 = new List<OnBrep>(breps);
		if (!num2)
		{
			OnBrep onBrep = BuildEnd(nurbsCurve, onXform9 * onXform4, onXform2, value2, reverse: false, value7, value7 * CSUtilGemRails.SizeEnd(cSDataArray), value9, (EndStyle)value12);
			OnBrep onBrep2 = BuildEnd(nurbsCurve, onXform5 * onXform3, onXform, value, reverse: true, value8, value8 * CSUtilGemRails.SizeStart(cSDataArray), value10, (EndStyle)value13);
			if (onBrep != null)
			{
				list7.Add(onBrep);
			}
			if (onBrep2 != null)
			{
				list7.Add(onBrep2);
			}
		}
		OnBrep onBrep3 = CSUtilRhino.JoinAllBreps(list7.ToArray(), 0.01);
		if (onBrep3 != null)
		{
			OnBrep onBrep4 = CSUtilRhino.CapPlanarHoles(onBrep3, 0.01);
			if (onBrep4 != null)
			{
				onBrep3 = onBrep4;
			}
		}
		if (onBrep3 != null)
		{
			On3dPoint brepPoint = new On3dPoint();
			On3dVector brepNormal = new On3dVector();
			CSUtilGeometry.GetClosestPointToBrep(onBrep3, crv.PointAtStart(), ref brepPoint, ref brepNormal);
			if (new On3dVector(brepPoint - crv.PointAtStart()) * brepNormal < 0.0)
			{
				onBrep3.Flip();
			}
		}
		if (onBrep3 != null)
		{
			OutBrep.SetData(new CSOnGeometryData(onBrep3.Convert()));
		}
		return base.Update();
	}

	private static void AdjustEndSurface(double amount, OnNurbsSurface surface)
	{
		OnBoundingBox onBoundingBox = surface.BoundingBox();
		On4dPoint cv = new On4dPoint();
		for (int i = 0; i < surface.CVCount(0); i++)
		{
			for (int j = 0; j < surface.CVCount(1); j++)
			{
				surface.GetCV(i, j, ref cv);
				double w = cv.w;
				cv.y /= cv.w;
				cv.x /= cv.w;
				cv.z /= cv.w;
				cv.w = 1.0;
				OnXform onXform = new OnXform();
				double num = Math.Sin((cv.y - onBoundingBox.Min().y) / (onBoundingBox.Max().y - onBoundingBox.Min().y) * OnUtil.On_PI / 2.0);
				onXform.Scale(1.0, 1.0, 1.0 * (1.0 - amount) + num * amount);
				if (Math.Abs(cv.z - onBoundingBox.Min().z) > 0.1)
				{
					cv.Transform(onXform);
				}
				cv.w = w;
				cv.x *= cv.w;
				cv.y *= cv.w;
				cv.z *= cv.w;
				surface.SetCV(i, j, cv);
			}
		}
	}

	private static OnBrep BuildEnd(IOnCurve profile, OnXform xform, IOnXform scale, double scaleX, bool reverse, double endSizeScalar, double endSize, double endTaper, EndStyle endStyle)
	{
		OnXform xform2 = xform * scale;
		if (endSize < 0.01)
		{
			endStyle = EndStyle.Capped;
		}
		OnCurve leftSide = null;
		OnCurve rightSide = null;
		if (!CSUtilGeometry.SplitProfileInHalf(profile, ref leftSide, ref rightSide))
		{
			return null;
		}
		if (leftSide == null || rightSide == null)
		{
			return null;
		}
		OnNurbsCurve onNurbsCurve = CSUtilGeometry.ChannelAdjustEndProfile(leftSide, endTaper);
		OnNurbsCurve onNurbsCurve2 = CSUtilGeometry.ChannelAdjustEndProfile(rightSide, endTaper);
		if (endSize < 0.01)
		{
			endStyle = EndStyle.Flat;
		}
		switch (endStyle)
		{
		case EndStyle.Flat:
		{
			List<IOnCurve> list = new List<IOnCurve>();
			double num = Math.Atan(endSizeScalar / scaleX);
			OnXform onXform3 = new OnXform();
			onXform3.Scale(1.0 / Math.Cos(num), 1.0, 1.0);
			OnNurbsCurve onNurbsCurve3 = leftSide.NurbsCurve();
			OnNurbsCurve onNurbsCurve4 = onNurbsCurve.Duplicate();
			onNurbsCurve3.Transform(xform2);
			OnXform onXform4 = new OnXform();
			if (!reverse)
			{
				onXform4.Rotation(num, OnUtil.On_yaxis, OnUtil.On_origin);
			}
			else
			{
				onXform4.Rotation(0.0 - num, OnUtil.On_yaxis, OnUtil.On_origin);
			}
			OnXform xform3 = xform * onXform4 * scale * onXform3;
			onNurbsCurve4.Transform(xform3);
			onNurbsCurve4.SetStartPoint(onNurbsCurve3.PointAtStart());
			onNurbsCurve4.SetEndPoint(onNurbsCurve3.PointAtEnd());
			list.Add(onNurbsCurve3);
			list.Add(onNurbsCurve4);
			OnNurbsCurve onNurbsCurve5 = rightSide.NurbsCurve();
			OnNurbsCurve onNurbsCurve6 = onNurbsCurve2.Duplicate();
			onNurbsCurve5.Transform(xform2);
			OnXform onXform5 = new OnXform();
			if (!reverse)
			{
				onXform5.Rotation(0.0 - num, OnUtil.On_yaxis, OnUtil.On_origin);
			}
			else
			{
				onXform5.Rotation(num, OnUtil.On_yaxis, OnUtil.On_origin);
			}
			OnXform xform4 = xform * onXform5 * scale * onXform3;
			onNurbsCurve6.Transform(xform4);
			onNurbsCurve5.Reverse();
			onNurbsCurve6.Reverse();
			onNurbsCurve6.SetStartPoint(onNurbsCurve3.PointAtStart());
			onNurbsCurve5.SetStartPoint(onNurbsCurve3.PointAtStart());
			onNurbsCurve6.SetEndPoint(onNurbsCurve3.PointAtEnd());
			onNurbsCurve5.SetEndPoint(onNurbsCurve3.PointAtEnd());
			list.Add(onNurbsCurve6);
			list.Add(onNurbsCurve5);
			return CSUtilSweep.Loft(list.ToArray(), IArgsRhinoLoft.eLoftType.ltStraight);
		}
		case EndStyle.Round:
		{
			OnNurbsCurve pProfile = leftSide.NurbsCurve();
			OnXform onXform = new OnXform();
			if (!reverse)
			{
				onXform.Rotation(OnUtil.On_PI / 2.0, OnUtil.On_yaxis, OnUtil.On_origin);
			}
			else
			{
				onXform.Rotation((0.0 - OnUtil.On_PI) / 2.0, OnUtil.On_yaxis, OnUtil.On_origin);
			}
			new OnXform().Scale(endSize / 2.0, 1.0, 1.0);
			OnLine axis = new OnLine(OnUtil.On_origin, new On3dPoint(0.0, 1.0, 0.0));
			OnInterval domain = new OnInterval(0.0, OnUtil.On_PI);
			OnNurbsSurface onNurbsSurface = RhUtil.RhinoRevolve(pProfile, ref axis, ref domain).NurbsSurface();
			OnXform onXform2 = new OnXform();
			if (!reverse)
			{
				onXform2.Scale(1.0, 1.0, endSize / 4.0);
			}
			else
			{
				onXform2.Scale(1.0, 1.0, (0.0 - endSize) / 4.0);
			}
			if (onNurbsSurface != null)
			{
				AdjustEndSurface(endTaper, onNurbsSurface);
				onNurbsSurface.Transform(xform * scale * onXform2);
				return onNurbsSurface.BrepForm();
			}
			break;
		}
		}
		if (endStyle == EndStyle.SharpCorner)
		{
			OnNurbsCurve onNurbsCurve7 = leftSide.NurbsCurve();
			onNurbsCurve7.Transform(xform2);
			OnNurbsCurve onNurbsCurve8 = onNurbsCurve.Duplicate();
			double angle_radians = (reverse ? CSBlockUtil.CSUtilMath.ToRadians(-90.0) : CSBlockUtil.CSUtilMath.ToRadians(90.0));
			OnXform onXform6 = new OnXform();
			onXform6.Rotation(angle_radians, new On3dVector(0.0, 1.0, 0.0), new On3dPoint(0.0, 0.0, 0.0));
			double x_scale_factor = endSizeScalar / scaleX;
			OnXform onXform7 = new OnXform();
			onXform7.Scale(x_scale_factor, 1.0, 1.0);
			onNurbsCurve8.Transform(xform * onXform6 * scale * onXform7);
			List<IOnCurve> obj = new List<IOnCurve> { onNurbsCurve7, onNurbsCurve8 };
			OnNurbsCurve onNurbsCurve9 = rightSide.NurbsCurve();
			onNurbsCurve9.Transform(xform2);
			onNurbsCurve9.Reverse();
			if (onNurbsCurve9.PointAtStart().DistanceTo(onNurbsCurve7.PointAtStart()) > 0.01)
			{
				onNurbsCurve9.Reverse();
			}
			obj.Add(onNurbsCurve9);
			return CSUtilSweep.Loft(obj.ToArray(), IArgsRhinoLoft.eLoftType.ltStraight);
		}
		return null;
	}
}
