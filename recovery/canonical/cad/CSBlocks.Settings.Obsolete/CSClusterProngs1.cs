using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Settings.Obsolete;

[Obsolete]
[CSBlock("ClusterProngs_V1", "{526287C1-6B13-474A-9907-88785326C9F3}")]
public sealed class CSClusterProngs1 : CSBlock
{
	private sealed class CacheProng
	{
		public readonly OnCurve Curve;

		private readonly CSData _data;

		private readonly double _height;

		private readonly double _filletPer;

		private readonly double _prongTaperPer;

		private readonly double _diameter;

		public CacheProng(CSData data, double height, double filletPer, double prongTaperPer, double diameter, OnCurve crv)
		{
			Curve = crv;
			_data = data;
			_height = height;
			_filletPer = filletPer;
			_prongTaperPer = prongTaperPer;
			_diameter = diameter;
		}

		public bool Compare(double cheight, double cfilletPer, double cProngTaperPer, double cDiameter, double tolerance)
		{
			if (Math.Abs(_height - cheight) > tolerance)
			{
				return false;
			}
			if (Math.Abs(_filletPer - cfilletPer) > tolerance)
			{
				return false;
			}
			if (Math.Abs(_prongTaperPer - cProngTaperPer) > tolerance)
			{
				return false;
			}
			if (Math.Abs(_diameter - cDiameter) > tolerance)
			{
				return false;
			}
			return true;
		}

		public CSData Duplicate()
		{
			return _data.Duplicate();
		}
	}

	private readonly List<CacheProng> _prongCache = new List<CacheProng>();

	public CSInputChannel iCenterGem { get; }

	public CSInputChannel iClusterGems { get; }

	public CSInputChannel iProngProfile { get; }

	public CSInputChannel iCenterShape { get; }

	public CSInputChannel iGemNudge { get; }

	public CSInputChannel iEdgeNudgeOutisde { get; }

	public CSInputChannel iEdgeNudgeInside { get; }

	public CSInputChannel iMinProngSize { get; }

	public CSInputChannel iMaxProngSize { get; }

	public CSInputChannel iProngHeightInside { get; }

	public CSInputChannel iProngHeightOutside { get; }

	public CSInputChannel iProngVOffsetInside { get; }

	public CSInputChannel iProngVOffsetOutside { get; }

	public CSInputChannel iTaper { get; }

	public CSInputChannel iFillet { get; }

	public CSInputChannel iStyle { get; }

	public CSInputChannel iFarInsideProngsSize { get; }

	public CSInputChannel iFarOutsideProngsSize { get; }

	public CSInputChannel iMeshProngs { get; }

	public CSInputChannel iBlendToCurve { get; }

	public CSInputChannel iBlendInside { get; }

	public CSInputChannel iBlendOutside { get; }

	public CSInputChannel iBlendAmount { get; }

	public CSInputChannel iBlendAmountTop { get; }

	public CSInputChannel iBlendAmountBottom { get; }

	public CSInputChannel iBlendTilt { get; }

	public CSInputChannel iBlendAdjustment { get; }

	public CSOutputChannel oProngs { get; }

	public CSOutputChannel oProfiles { get; }

	public CSOutputChannel oBlends { get; }

	public CSClusterProngs1(string identifier)
		: base(identifier)
	{
		iCenterGem = new CSInputChannel(GetPropertyName(() => iCenterGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iClusterGems = new CSInputChannel(GetPropertyName(() => iClusterGems), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iProngProfile = new CSInputChannel(GetPropertyName(() => iProngProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iCenterShape = new CSInputChannel(GetPropertyName(() => iCenterShape), this, ChannelDataType.Any, ChannelArrayType.Any);
		iGemNudge = new CSInputChannel(GetPropertyName(() => iGemNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEdgeNudgeOutisde = new CSInputChannel(GetPropertyName(() => iEdgeNudgeOutisde), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEdgeNudgeInside = new CSInputChannel(GetPropertyName(() => iEdgeNudgeInside), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMinProngSize = new CSInputChannel(GetPropertyName(() => iMinProngSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMaxProngSize = new CSInputChannel(GetPropertyName(() => iMaxProngSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngHeightInside = new CSInputChannel(GetPropertyName(() => iProngHeightInside), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngHeightOutside = new CSInputChannel(GetPropertyName(() => iProngHeightOutside), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngVOffsetInside = new CSInputChannel(GetPropertyName(() => iProngVOffsetInside), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngVOffsetOutside = new CSInputChannel(GetPropertyName(() => iProngVOffsetOutside), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTaper = new CSInputChannel(GetPropertyName(() => iTaper), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFillet = new CSInputChannel(GetPropertyName(() => iFillet), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iStyle = new CSInputChannel(GetPropertyName(() => iStyle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFarInsideProngsSize = new CSInputChannel(GetPropertyName(() => iFarInsideProngsSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFarOutsideProngsSize = new CSInputChannel(GetPropertyName(() => iFarOutsideProngsSize), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMeshProngs = new CSInputChannel(GetPropertyName(() => iMeshProngs), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iBlendToCurve = new CSInputChannel(GetPropertyName(() => iBlendToCurve), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iBlendInside = new CSInputChannel(GetPropertyName(() => iBlendInside), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iBlendOutside = new CSInputChannel(GetPropertyName(() => iBlendOutside), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iBlendAmount = new CSInputChannel(GetPropertyName(() => iBlendAmount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAmountBottom = new CSInputChannel(GetPropertyName(() => iBlendAmountBottom), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAmountTop = new CSInputChannel(GetPropertyName(() => iBlendAmountTop), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendTilt = new CSInputChannel(GetPropertyName(() => iBlendTilt), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBlendAdjustment = new CSInputChannel(GetPropertyName(() => iBlendAdjustment), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProngs = new CSOutputChannel(GetPropertyName(() => oProngs), this, ChannelDataType.Surface, ChannelArrayType.Array);
		oProfiles = new CSOutputChannel(GetPropertyName(() => oProfiles), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oBlends = new CSOutputChannel(GetPropertyName(() => oBlends), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCenterGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gem");
		}
		if (!(flattenArray.GetArrayItem(0) is CSGemData cSGemData))
		{
			return UpdateMissing("Missing gem");
		}
		CSDataArray cSDataArray = iClusterGems.Data as CSDataArray;
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Cluster Gems");
		}
		OnNurbsCurve nurbs_curve = iProngProfile.GetNurbsCurve();
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
		double value = 0.0;
		iGemNudge.GetDouble(ref value);
		double value2 = 0.0;
		iEdgeNudgeInside.GetDouble(ref value2);
		double value3 = 0.0;
		iEdgeNudgeOutisde.GetDouble(ref value3);
		double value4 = 0.0;
		iProngHeightInside.GetDouble(ref value4);
		double value5 = 0.0;
		iProngHeightOutside.GetDouble(ref value5);
		double value6 = 0.0;
		iFillet.GetDoublePercentage(ref value6);
		double value7 = 0.0;
		iTaper.GetDoublePercentage(ref value7);
		double value8 = 0.0;
		iProngVOffsetInside.GetDouble(ref value8);
		double value9 = 0.0;
		iProngVOffsetOutside.GetDouble(ref value9);
		double value10 = 0.0;
		iMinProngSize.GetDouble(ref value10);
		double value11 = 100.0;
		iMaxProngSize.GetDouble(ref value11);
		bool value12 = false;
		iMeshProngs.GetBoolean(ref value12);
		double value13 = 0.5;
		iFarOutsideProngsSize.GetDouble(ref value13);
		double value14 = 0.5;
		iFarInsideProngsSize.GetDouble(ref value14);
		bool value15 = false;
		iBlendInside.GetBoolean(ref value15);
		bool value16 = false;
		iBlendOutside.GetBoolean(ref value16);
		int value17 = 0;
		iStyle.GetInteger(ref value17);
		double value18 = 0.5;
		iBlendAmount.GetDouble(ref value18);
		double value19 = 1.0;
		iBlendAmountTop.GetDouble(ref value19);
		double value20 = 1.0;
		iBlendAmountBottom.GetDouble(ref value20);
		double value21 = 0.0;
		iBlendTilt.GetDouble(ref value21);
		double value22 = 0.2;
		iBlendAdjustment.GetDouble(ref value22);
		CSDataArray flattenArray2 = iBlendToCurve.GetFlattenArray();
		OnNurbsCurve onNurbsCurve = null;
		if (flattenArray2.GetArrayCount() > 0)
		{
			onNurbsCurve = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		CSDataArray cSDataArray4 = new CSDataArray();
		int arrayCount = cSDataArray.GetArrayCount();
		_prongCache.Clear();
		double edgeNudge = value2;
		double edgeNudge2 = value3;
		double num = value4;
		double num2 = value5;
		double num3 = value8;
		double num4 = value9;
		CSChannelGemData channelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, 0);
		double num5 = channelGemData.Left.DistanceTo(cSGemData.Center);
		double num6 = channelGemData.Right.DistanceTo(cSGemData.Center);
		bool flag = false;
		if (num5 > num6)
		{
			flag = true;
			edgeNudge = value3;
			edgeNudge2 = value2;
			num = value5;
			num2 = value4;
			num3 = value9;
			num4 = value8;
		}
		bool flag2 = !flag;
		for (int i = 0; i <= arrayCount + 1; i++)
		{
			CSChannelGemData cSChannelGemData = null;
			bool num7 = CSUtilGemRails.IsClosed(cSDataArray.Array);
			if (num7)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, (i - 1 + arrayCount) % arrayCount);
			}
			else if (i - 1 >= 0)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1);
			}
			CSChannelGemData cSChannelGemData2 = null;
			if (num7)
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i % arrayCount);
			}
			else if (i < flattenArray.GetArrayCount())
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i);
			}
			CSChannelGemData cSChannelGemData3 = null;
			if (num7)
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, (i + 1) % arrayCount);
			}
			else if (i + 1 < flattenArray.GetArrayCount())
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, i + 1);
			}
			if (cSChannelGemData == null && cSChannelGemData3 != null && cSChannelGemData2 != null)
			{
				cSChannelGemData = new CSChannelGemData(cSChannelGemData3);
				cSChannelGemData.ApplyTransform(cSChannelGemData2.GetMirrorForwardBack());
			}
			CSChannelGemData cSChannelGemData4 = null;
			if (num7)
			{
				cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, (i - 2 + arrayCount) % arrayCount);
			}
			else if (i - 2 >= 0)
			{
				cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, i - 2);
			}
			if (cSChannelGemData2 == null && cSChannelGemData != null && cSChannelGemData4 != null)
			{
				cSChannelGemData2 = new CSChannelGemData(cSChannelGemData4);
				cSChannelGemData2.ApplyTransform(cSChannelGemData.GetMirrorForwardBack());
			}
			if (cSChannelGemData2 == null || cSChannelGemData == null)
			{
				continue;
			}
			CSDataHelper.SetSettingType(cSChannelGemData2, GemSettingType.PR);
			On3dPoint on3dPoint = (cSChannelGemData2.Center.Convert() + cSChannelGemData.Center.Convert()) / 2.0;
			On3dVector on3dVector = cSChannelGemData2.Normal.Convert() + cSChannelGemData.Normal.Convert();
			on3dVector.Unitize();
			OnCircle edgeCircleRight = GetEdgeCircleRight(edgeNudge2, cSChannelGemData, cSChannelGemData2);
			OnCircle edgeCircleLeft = GetEdgeCircleLeft(edgeNudge, cSChannelGemData, cSChannelGemData2);
			OnCircle c = new OnCircle(new OnPlane(cSChannelGemData.Center.Convert(), cSChannelGemData.Normal.Convert()), cSChannelGemData.SizeAverage - value);
			OnCircle c2 = new OnCircle(new OnPlane(cSChannelGemData2.Center.Convert(), cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - value);
			OnCircle onCircle2 = CSUtilGeometry.CalculateApolloniusCircle(c, c2, edgeCircleLeft);
			OnCircle onCircle3 = CSUtilGeometry.CalculateApolloniusCircle(c, c2, edgeCircleRight);
			On3dVector on3dVector2 = cSChannelGemData2.Normal.Convert() + cSChannelGemData.Normal.Convert();
			on3dVector2.Unitize();
			double t = 0.0;
			onCircle2.ClosestPointTo(on3dPoint, ref t);
			onCircle2.Rotate(t + OnUtil.On_PI / 2.0, onCircle2.Normal());
			onCircle3.ClosestPointTo(on3dPoint, ref t);
			onCircle3.Rotate(t + OnUtil.On_PI / 2.0, onCircle3.Normal());
			onCircle2.Translate(on3dVector2 * (num3 - num));
			onCircle3.Translate(on3dVector2 * (num4 - num2));
			On3dPoint on3dPoint2 = null;
			On3dVector axis = null;
			if (onNurbsCurve != null)
			{
				double t2 = 0.0;
				onNurbsCurve.GetClosestPoint(on3dPoint, ref t2);
				double num8 = onNurbsCurve.PointAt(t2).DistanceTo(on3dPoint);
				onNurbsCurve.GetClosestPoint(on3dPoint - on3dVector * num8 * value22, ref t2);
				on3dPoint2 = onNurbsCurve.PointAt(t2);
				axis = onNurbsCurve.TangentAt(t2);
			}
			On3dPoint origin;
			On3dPoint origin2;
			if (flag)
			{
				origin = cSChannelGemData2.Center.Convert() + cSChannelGemData2.UnitLeftVector.Convert() * (value13 / 2.0 + cSChannelGemData2.Size - value);
				origin2 = cSChannelGemData2.Center.Convert() + cSChannelGemData2.UnitRightVector.Convert() * (value14 / 2.0 + cSChannelGemData2.Size - value);
			}
			else
			{
				origin = cSChannelGemData2.Center.Convert() + cSChannelGemData2.UnitRightVector.Convert() * (value13 / 2.0 + cSChannelGemData2.Size - value);
				origin2 = cSChannelGemData2.Center.Convert() + cSChannelGemData2.UnitLeftVector.Convert() * (value14 / 2.0 + cSChannelGemData2.Size - value);
			}
			OnCircle onCircle4;
			OnCircle onCircle5;
			if (OnUtil.ON_CrossProduct(cSChannelGemData2.ForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()) * cSChannelGemData2.Normal.Convert() > 0.0)
			{
				onCircle4 = new OnCircle(new OnPlane(origin, cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()), value13 / 2.0);
				onCircle5 = new OnCircle(new OnPlane(origin2, -cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitRightVector.Convert()), value14 / 2.0);
			}
			else
			{
				onCircle4 = new OnCircle(new OnPlane(origin, cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitRightVector.Convert()), value13 / 2.0);
				onCircle5 = new OnCircle(new OnPlane(origin2, -cSChannelGemData2.UnitForwardVector.Convert(), cSChannelGemData2.UnitLeftVector.Convert()), value14 / 2.0);
			}
			onCircle4.Translate(cSChannelGemData2.Normal.Convert() * (value9 - value5));
			onCircle5.Translate(cSChannelGemData2.Normal.Convert() * (value8 - value4));
			if (value17 == 0 || value17 == 1 || (flag2 && value17 == 2) || (flag && value17 == 3) || value17 == 6 || value17 == 7)
			{
				OnCurve curve;
				CSData data = CreateProngCached(nurbs_curve, num, value6, value7, onCircle2.radius * 2.0, value12, out curve);
				OnXform onXform = new OnXform();
				onXform.ChangeBasis(onCircle2.plane, OnUtil.On_xy_plane);
				curve.Transform(onXform);
				if (onCircle2.radius * 2.0 > value10 && onCircle2.radius * 2.0 < value11)
				{
					cSDataArray3.Append(new CSOnGeometryData(curve.Convert(), onXform.Convert(), applyTransform: false));
					CSDataHelper.Transform(data, onXform.Convert());
					cSDataArray2.Append(data);
					if (on3dPoint2 != null && ((!flag && value15) || (flag && value16)))
					{
						On3dVector on3dVector3 = new On3dVector(onCircle2.Normal());
						on3dVector3.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value21), axis);
						OnBrep brep = CSUtilSweep.Sweep1(CSUtilCurves.BlendCurve(onCircle2.Center(), on3dPoint2, -new On3dVector(onCircle2.Normal()), on3dVector3, value18, value19, value20), curve);
						cSDataArray4.Append(new CSOnGeometryData(brep.Convert()));
					}
				}
			}
			if (value17 == 0 || value17 == 1 || (flag && value17 == 2) || (flag2 && value17 == 3) || value17 == 6 || value17 == 7)
			{
				OnCurve curve2;
				CSData data2 = CreateProngCached(nurbs_curve, num2, value6, value7, onCircle3.radius * 2.0, value12, out curve2);
				OnXform onXform2 = new OnXform();
				onXform2.ChangeBasis(onCircle3.plane, OnUtil.On_xy_plane);
				curve2.Transform(onXform2);
				if (onCircle3.radius * 2.0 > value10 && onCircle3.radius * 2.0 < value11)
				{
					cSDataArray3.Append(new CSOnGeometryData(curve2.Convert(), onXform2.Convert(), applyTransform: false));
					CSDataHelper.Transform(data2, onXform2.Convert());
					cSDataArray2.Append(data2);
					if (on3dPoint2 != null && ((!flag && value16) || (flag && value15)))
					{
						On3dVector on3dVector4 = new On3dVector(onCircle3.Normal());
						on3dVector4.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value21), axis);
						OnBrep brep2 = CSUtilSweep.Sweep1(CSUtilCurves.BlendCurve(onCircle3.Center(), on3dPoint2, -new On3dVector(onCircle3.Normal()), on3dVector4, value18, value19, value20), curve2);
						cSDataArray4.Append(new CSOnGeometryData(brep2.Convert()));
					}
				}
			}
			if (value17 == 0 || value17 == 1 || value17 == 3 || value17 == 4 || value17 == 5)
			{
				OnCurve curve3;
				CSData data3 = CreateProngCached(nurbs_curve, value5, value6, value7, onCircle4.radius * 2.0, value12, out curve3);
				OnXform onXform3 = new OnXform();
				onXform3.ChangeBasis(onCircle4.plane, OnUtil.On_xy_plane);
				CSDataHelper.Transform(data3, onXform3.Convert());
				cSDataArray2.Append(data3);
				if (curve3 != null)
				{
					curve3.Transform(onXform3);
					cSDataArray3.Append(new CSOnGeometryData(curve3.Convert(), onXform3.Convert(), applyTransform: false));
					if (on3dPoint2 != null)
					{
						On3dPoint pointEnd = null;
						On3dVector axis2 = null;
						if (onNurbsCurve != null)
						{
							double t3 = 0.0;
							onNurbsCurve.GetClosestPoint(onCircle4.Center(), ref t3);
							double num9 = onNurbsCurve.PointAt(t3).DistanceTo(onCircle4.Center());
							onNurbsCurve.GetClosestPoint(new On3dPoint(onCircle4.Center()) - new On3dVector(onCircle4.Normal()) * num9 * value22, ref t3);
							pointEnd = onNurbsCurve.PointAt(t3);
							axis2 = onNurbsCurve.TangentAt(t3);
						}
						if (value16)
						{
							On3dVector on3dVector5 = new On3dVector(onCircle4.Normal());
							on3dVector5.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value21), axis2);
							OnBrep brep3 = CSUtilSweep.Sweep1(CSUtilCurves.BlendCurve(onCircle4.Center(), pointEnd, -new On3dVector(onCircle4.Normal()), on3dVector5, value18, value19, value20), curve3);
							cSDataArray4.Append(new CSOnGeometryData(brep3.Convert()));
						}
					}
				}
			}
			if (value17 != 0 && value17 != 7)
			{
				continue;
			}
			OnCurve curve4;
			CSData data4 = CreateProngCached(nurbs_curve, value4, value6, value7, onCircle5.radius * 2.0, value12, out curve4);
			OnXform onXform4 = new OnXform();
			onXform4.ChangeBasis(onCircle5.plane, OnUtil.On_xy_plane);
			CSDataHelper.Transform(data4, onXform4.Convert());
			cSDataArray2.Append(data4);
			if (curve4 == null)
			{
				continue;
			}
			curve4.Transform(onXform4);
			cSDataArray3.Append(new CSOnGeometryData(curve4.Convert()));
			if (on3dPoint2 != null)
			{
				On3dPoint pointEnd2 = null;
				On3dVector axis3 = null;
				if (onNurbsCurve != null)
				{
					double t4 = 0.0;
					onNurbsCurve.GetClosestPoint(onCircle5.Center(), ref t4);
					double num10 = onNurbsCurve.PointAt(t4).DistanceTo(onCircle5.Center());
					onNurbsCurve.GetClosestPoint(new On3dPoint(onCircle5.Center()) - new On3dVector(onCircle5.Normal()) * num10 * value22, ref t4);
					pointEnd2 = onNurbsCurve.PointAt(t4);
					axis3 = onNurbsCurve.TangentAt(t4);
				}
				if (value16)
				{
					On3dVector on3dVector6 = new On3dVector(onCircle5.Normal());
					on3dVector6.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value21), axis3);
					OnBrep brep4 = CSUtilSweep.Sweep1(CSUtilCurves.BlendCurve(onCircle5.Center(), pointEnd2, -new On3dVector(onCircle5.Normal()), on3dVector6, value18, value19, value20), curve4);
					cSDataArray4.Append(new CSOnGeometryData(brep4.Convert()));
				}
			}
		}
		oProngs.SetData(cSDataArray2);
		oProfiles.SetData(cSDataArray3);
		oBlends.SetData(cSDataArray4);
		return base.Update();
	}

	private CSData CreateProngCached(IOnNurbsCurve vProfile, double height, double filletPer, double prongTaperPer, double diameter, bool meshedProngs, out OnCurve curve)
	{
		curve = null;
		foreach (CacheProng item in _prongCache)
		{
			if (item.Compare(height, filletPer, prongTaperPer, diameter, 0.01))
			{
				if (item.Curve != null)
				{
					curve = item.Curve.Duplicate();
				}
				return item.Duplicate();
			}
		}
		if (CSApplicationHelper.Application.IsCamMode)
		{
			height += CSApplicationHelper.Application.CamProngExtensionHeight;
		}
		OnNurbsSurface nurbsSurface;
		OnNurbsCurve outputRail;
		if (meshedProngs)
		{
			if (CSUtilGeometry.CreateMeshProng(vProfile, diameter, height, filletPer, prongTaperPer, out var meshSurface, out var outRailLine))
			{
				CSOnGeometryData data = new CSOnGeometryData(meshSurface.Convert());
				OnPolylineCurve crv = new OnPolylineCurve(outRailLine);
				CacheProng cacheProng = new CacheProng(data, height, filletPer, prongTaperPer, diameter, crv);
				_prongCache.Add(cacheProng);
				if (cacheProng.Curve != null)
				{
					curve = cacheProng.Curve.Duplicate();
				}
				return cacheProng.Duplicate();
			}
		}
		else if (CSUtilGeometry.CreateProng(vProfile, diameter, height, filletPer, prongTaperPer, out nurbsSurface, out outputRail))
		{
			CacheProng cacheProng2 = new CacheProng(new CSOnGeometryData(nurbsSurface.Convert()), height, filletPer, prongTaperPer, diameter, outputRail);
			_prongCache.Add(cacheProng2);
			if (cacheProng2.Curve != null)
			{
				curve = cacheProng2.Curve.Duplicate();
			}
			return cacheProng2.Duplicate();
		}
		return new CSData();
	}

	private static OnCircle GetEdgeCircleLeft(double edgeNudge, CSChannelGemData gemPrevious, CSChannelGemData gemCurrent)
	{
		On3dPoint q = new OnLine(gemPrevious.Left.Convert(), gemCurrent.Left.Convert()).PointAt(0.5);
		On3dPoint p = gemPrevious.Left.Convert() + gemPrevious.LeftVector.Convert() * 0.1;
		On3dPoint r = gemCurrent.Left.Convert() + gemCurrent.LeftVector.Convert() * 0.1;
		OnCircle onCircle = new OnCircle(p, q, r);
		onCircle.radius -= edgeNudge;
		return onCircle;
	}

	private static OnCircle GetEdgeCircleRight(double edgeNudge, CSChannelGemData gemPrevious, CSChannelGemData gemCurrent)
	{
		On3dPoint q = new OnLine(gemPrevious.Right.Convert(), gemCurrent.Right.Convert()).PointAt(0.5);
		On3dPoint p = gemPrevious.Right.Convert() + gemPrevious.RightVector.Convert() * 0.1;
		On3dPoint r = gemCurrent.Right.Convert() + gemCurrent.RightVector.Convert() * 0.1;
		OnCircle onCircle = new OnCircle(p, q, r);
		onCircle.radius -= edgeNudge;
		return onCircle;
	}
}
