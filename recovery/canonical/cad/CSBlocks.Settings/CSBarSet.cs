using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Settings;

[CSBlock("BarSet", "{88E78390-368C-48A8-A1C9-2AA2E4A1048D}")]
public sealed class CSBarSet : CSBlock
{
	private CSDataArray _barCached;

	private double _cachedBottomDepth;

	private double _cachedWidth;

	private double _cachedHeight;

	private double _cachedDomeAmount;

	private double _cachedHalfThickness;

	private double _cachedGemNextNudge;

	private double _cachedGemCurrentNudge;

	public CSInputChannel InSortedGem { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InLargeWidthAdjustment { get; }

	public CSInputChannel InLargeTopHeight { get; }

	public CSInputChannel InLargeBottomDepth { get; }

	public CSInputChannel InSmallWidthAdjustment { get; }

	public CSInputChannel InSmallTopHeight { get; }

	public CSInputChannel InSmallBottomDepth { get; }

	public CSInputChannel InNudge { get; }

	public CSInputChannel InDomeAmount { get; }

	public CSOutputChannel OutSurface { get; }

	public CSBarSet(string identifier)
		: base(identifier)
	{
		InSortedGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InLargeWidthAdjustment = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InLargeTopHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InLargeBottomDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSmallWidthAdjustment = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSmallTopHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSmallBottomDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InDomeAmount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!(InSortedGem.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		OnNurbsCurve nurbs_curve = InProfile.GetNurbsCurve();
		if (nurbs_curve == null)
		{
			OnCircle onCircle = new OnCircle(OnUtil.On_xy_plane, OnUtil.On_origin, 1.0);
			nurbs_curve = new OnNurbsCurve();
			onCircle.GetNurbForm(ref nurbs_curve);
			if (nurbs_curve == null)
			{
				return UpdateMissing("Profile");
			}
		}
		OnBoundingBox tight_bbox = new OnBoundingBox();
		nurbs_curve.GetTightBoundingBox(ref tight_bbox);
		OnXform onXform = new OnXform();
		onXform.Translation((0.0 - (tight_bbox.Max().x + tight_bbox.Min().x)) / 2.0, 0.0 - tight_bbox.Min().y, 0.0);
		OnXform onXform2 = new OnXform();
		onXform2.Scale(1.0 / (tight_bbox.Max().x - tight_bbox.Min().x), 1.0 / (tight_bbox.Max().y - tight_bbox.Min().y), 1.0);
		nurbs_curve.Transform(onXform2 * onXform);
		double value = 0.0;
		InLargeWidthAdjustment.GetDouble(ref value);
		double value2 = 0.0;
		InLargeTopHeight.GetDouble(ref value2);
		double value3 = 0.0;
		InLargeBottomDepth.GetDouble(ref value3);
		double value4 = 0.0;
		InSmallWidthAdjustment.GetDouble(ref value4);
		double value5 = 0.0;
		InSmallTopHeight.GetDouble(ref value5);
		double value6 = 0.0;
		InSmallBottomDepth.GetDouble(ref value6);
		double value7 = 0.0;
		InNudge.GetDoublePercentage(ref value7);
		double value8 = 0.9;
		InDomeAmount.GetDouble(ref value8);
		bool flag = value4 != 0.0 || value5 != 0.0;
		CSDataArray cSDataArray2 = new CSDataArray();
		double num = double.MaxValue;
		double num2 = double.MinValue;
		if (flag)
		{
			for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
			{
				CSChannelGemData channelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, i);
				if (channelGemData != null)
				{
					if (channelGemData.SizeAverage < num)
					{
						num = channelGemData.SizeAverage;
					}
					if (channelGemData.SizeAverage > num2)
					{
						num2 = channelGemData.SizeAverage;
					}
				}
			}
			if (Math.Abs(num2 - num) < 0.001)
			{
				flag = false;
			}
		}
		int arrayCount = cSDataArray.GetArrayCount();
		ClearCache();
		bool flag2 = CSUtilGemRails.IsClosed(cSDataArray.Array);
		for (int j = -1; j <= arrayCount; j++)
		{
			CSChannelGemData cSChannelGemData = null;
			if (flag2)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, (j - 1 + arrayCount) % arrayCount);
			}
			else if (j - 1 >= 0)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, j - 1);
			}
			CSChannelGemData cSChannelGemData2 = null;
			if (flag2)
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, j % arrayCount);
			}
			else if (j < cSDataArray.GetArrayCount())
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, j);
			}
			CSChannelGemData cSChannelGemData3 = null;
			if (flag2)
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, (j + 1) % arrayCount);
			}
			else if (j + 1 < cSDataArray.GetArrayCount())
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, j + 1);
			}
			CSChannelGemData cSChannelGemData4 = null;
			if (flag2)
			{
				cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, (j + 2) % arrayCount);
			}
			else if (j + 2 < cSDataArray.GetArrayCount())
			{
				cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, j + 2);
			}
			if (cSChannelGemData2 == null && cSChannelGemData3 != null && cSChannelGemData4 != null)
			{
				cSChannelGemData2 = new CSChannelGemData(cSChannelGemData4);
				cSChannelGemData2.ApplyTransform(cSChannelGemData3.GetMirrorForwardBack());
			}
			if (cSChannelGemData3 == null && cSChannelGemData != null && cSChannelGemData2 != null)
			{
				cSChannelGemData3 = new CSChannelGemData(cSChannelGemData);
				cSChannelGemData3.ApplyTransform(cSChannelGemData2.GetMirrorForwardBack());
			}
			if (cSChannelGemData2 != null && cSChannelGemData3 != null)
			{
				OnLine onLine = new OnLine(cSChannelGemData2.Center.Convert(), cSChannelGemData3.Center.Convert());
				OnLine onLine2 = new OnLine(cSChannelGemData2.TableCenterPoint.Convert(), cSChannelGemData3.TableCenterPoint.Convert());
				On3dPoint centerLineCenter = onLine.PointAt(0.5);
				On3dPoint closestPointToGemGirdle = GetClosestPointToGemGirdle(cSChannelGemData2, centerLineCenter);
				On3dPoint closestPointToGemGirdle2 = GetClosestPointToGemGirdle(cSChannelGemData3, centerLineCenter);
				OnLine onLine3 = new OnLine(closestPointToGemGirdle, closestPointToGemGirdle2);
				On3dPoint on3dPoint = onLine3.PointAt(0.5);
				On3dPoint on3dPoint2 = onLine2.ClosestPointTo(on3dPoint);
				double num3 = value;
				double num4 = value2;
				double num5 = value3;
				if (flag)
				{
					double num6 = (cSChannelGemData2.SizeAverage - num) / (num2 - num);
					num3 = num6 * (value - value4) + value4;
					num4 = num6 * (value2 - value5) + value5;
				}
				double width = (cSChannelGemData3.Size * 2.0 + cSChannelGemData2.Size * 2.0) / 2.0 + num3;
				double height = num4 + num5 + on3dPoint2.DistanceTo(on3dPoint);
				double num7 = Math.Min(value8, (onLine3.Length() + cSChannelGemData3.SizeForward * value7 * 2.0 + cSChannelGemData2.SizeForward * value7 * 2.0) / 2.0 - 0.01);
				double halfThickness = onLine3.Length() / 2.0 - num7;
				double gemNextNudge = cSChannelGemData3.SizeForward * value7 * 2.0;
				double gemCurrentNudge = cSChannelGemData2.SizeForward * value7 * 2.0;
				CSData data = CreateBarCached(nurbs_curve, num5, width, height, num7, halfThickness, gemNextNudge, gemCurrentNudge);
				On3dVector on3dVector = new On3dVector(cSChannelGemData2.Up.Convert()) + new On3dVector(cSChannelGemData3.Up.Convert());
				on3dVector.Unitize();
				On3dVector destination = onLine3.Tangent();
				OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, on3dPoint, on3dVector, destination);
				CSDataHelper.Transform(data, xform.Convert());
				cSDataArray2.Append(data);
			}
		}
		OutSurface.SetData(cSDataArray2);
		return base.Update();
	}

	private void ClearCache()
	{
		_barCached = null;
	}

	private static CSDataArray CreateBar(OnNurbsCurve vProfile, double bottomDepth, double width, double height, double domeAmountCurrent, double halfThickness, double gemNextNudge, double gemCurrentNudge)
	{
		OnNurbsCurve onNurbsCurve = vProfile.Duplicate();
		OnXform onXform = new OnXform();
		onXform.Scale(width, height, 1.0);
		onNurbsCurve.Transform(onXform);
		On3dVector on3dVector = new On3dVector(onNurbsCurve.BoundingBox().Center() - OnUtil.On_origin);
		onNurbsCurve.Translate(-on3dVector);
		OnNurbsSurface onNurbsSurface = CSUtilRailRevolve.RailRevolve(CSUtilGeometry.CreateDome(domeAmountCurrent), onNurbsCurve, 0);
		onNurbsSurface.Translate(on3dVector);
		onNurbsCurve.Translate(on3dVector);
		OnXform onXform2 = new OnXform();
		onXform2.Scale(1.0, 1.0, -1.0);
		CSOnGeometryData cSOnGeometryData = new CSOnGeometryData(onNurbsSurface.Convert());
		CSOnGeometryData cSOnGeometryData2 = new CSOnGeometryData(onNurbsSurface.DuplicateSurface().Convert());
		OnXform onXform3 = new OnXform();
		onXform3.Translation(0.0, 0.0 - bottomDepth, halfThickness + gemNextNudge);
		onNurbsCurve.Transform(onXform3);
		cSOnGeometryData.ApplyTransform(onXform3.Convert());
		OnCurve onCurve = vProfile.Duplicate();
		OnXform onXform4 = new OnXform();
		onXform4.Translation(0.0, 0.0 - bottomDepth, 0.0 - halfThickness - gemCurrentNudge);
		onCurve.Transform(onXform4 * onXform);
		cSOnGeometryData2.ApplyTransform(onXform4.Convert() * onXform2.Convert());
		OnBrep brep = CSUtilSweep.Loft(new IOnCurve[2] { onNurbsCurve, onCurve }, IArgsRhinoLoft.eLoftType.ltNormal, capped: false);
		CSDataArray cSDataArray = new CSDataArray();
		cSDataArray.Append(new CSOnGeometryData(brep.Convert()));
		cSDataArray.Append(cSOnGeometryData2);
		cSDataArray.Append(cSOnGeometryData);
		return cSDataArray;
	}

	private CSData CreateBarCached(OnNurbsCurve vProfile, double bottomDepth, double width, double height, double domeAmount, double halfThickness, double gemNextNudge, double gemCurrentNudge)
	{
		if (_barCached != null && Math.Abs(_cachedBottomDepth - bottomDepth) < 0.01 && Math.Abs(_cachedWidth - width) < 0.01 && Math.Abs(_cachedHeight - height) < 0.01 && Math.Abs(_cachedDomeAmount - domeAmount) < 0.01 && Math.Abs(_cachedHalfThickness - halfThickness) < 0.01 && Math.Abs(_cachedGemNextNudge - gemNextNudge) < 0.01 && Math.Abs(_cachedGemCurrentNudge - gemCurrentNudge) < 0.01)
		{
			return _barCached.Duplicate();
		}
		_barCached = CreateBar(vProfile, bottomDepth, width, height, domeAmount, halfThickness, gemCurrentNudge, gemCurrentNudge);
		_cachedBottomDepth = bottomDepth;
		_cachedWidth = width;
		_cachedHeight = height;
		_cachedDomeAmount = domeAmount;
		_cachedHalfThickness = halfThickness;
		_cachedGemNextNudge = gemNextNudge;
		_cachedGemCurrentNudge = gemCurrentNudge;
		return _barCached.Duplicate();
	}

	private static On3dPoint GetClosestPointToGemGirdle(ICSCurve gemCurrent, IOn3dPoint centerLineCenter)
	{
		double t = 0.0;
		OnNurbsCurve onNurbsCurve = gemCurrent.GetNurbsCurve().Convert();
		onNurbsCurve.GetClosestPoint(centerLineCenter, ref t);
		return onNurbsCurve.PointAt(t);
	}
}
