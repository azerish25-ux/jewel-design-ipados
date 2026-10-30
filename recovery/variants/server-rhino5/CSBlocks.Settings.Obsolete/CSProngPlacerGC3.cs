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
using RMA.Rhino;

namespace CSBlocks.Settings.Obsolete;

[Obsolete]
[CSBlock("ProngPlacerGC_V3", "{5081B43E-6AE3-4107-9D38-8D37B9837204}")]
public sealed class CSProngPlacerGC3 : CSBlock
{
	private CSData _prongCachedData;

	private double _prongCachedDiameter = -1.0;

	public CSInputChannel iSortedGem { get; }

	public CSInputChannel iGemNudge { get; }

	public CSInputChannel iEdgeNudge { get; }

	public CSInputChannel iProngSizeMin { get; }

	public CSInputChannel iProngSizeMax { get; }

	public CSInputChannel iHeightOffset { get; }

	public CSInputChannel iPlacementOffset { get; }

	public CSInputChannel iTaper { get; }

	public CSInputChannel iFillet { get; }

	public CSInputChannel iProfile { get; }

	public CSInputChannel iMeshedProngs { get; }

	public CSOutputChannel oSurface { get; }

	public CSOutputChannel oAvgProngDiameter { get; }

	public CSProngPlacerGC3(string identifier)
		: base(identifier)
	{
		iSortedGem = new CSInputChannel(GetPropertyName(() => iSortedGem), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iGemNudge = new CSInputChannel(GetPropertyName(() => iGemNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEdgeNudge = new CSInputChannel(GetPropertyName(() => iEdgeNudge), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngSizeMin = new CSInputChannel(GetPropertyName(() => iProngSizeMin), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngSizeMax = new CSInputChannel(GetPropertyName(() => iProngSizeMax), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iHeightOffset = new CSInputChannel(GetPropertyName(() => iHeightOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacementOffset = new CSInputChannel(GetPropertyName(() => iPlacementOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTaper = new CSInputChannel(GetPropertyName(() => iTaper), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFillet = new CSInputChannel(GetPropertyName(() => iFillet), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iMeshedProngs = new CSInputChannel(GetPropertyName(() => iMeshedProngs), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(GetPropertyName(() => oSurface), this, ChannelDataType.Brep, ChannelArrayType.Array);
		oAvgProngDiameter = new CSOutputChannel(GetPropertyName(() => oAvgProngDiameter), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Expected O, but got Unknown
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Expected O, but got Unknown
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Expected O, but got Unknown
		double num = 0.0;
		double num2 = 0.0;
		if (!(iSortedGem.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		OnNurbsCurve val = iProfile.GetNurbsCurve();
		if (val == null)
		{
			OnCircle val2 = new OnCircle(OnUtil.On_xy_plane, OnUtil.On_origin, 1.0);
			val = new OnNurbsCurve();
			val2.GetNurbForm(ref val);
			if (val == null)
			{
				return UpdateMissing("Profile");
			}
		}
		double value = 0.0;
		iGemNudge.GetDouble(ref value);
		double value2 = 0.0;
		iEdgeNudge.GetDouble(ref value2);
		double value3 = 0.0;
		iHeightOffset.GetDouble(ref value3);
		double value4 = 0.0;
		iFillet.GetDoublePercentage(ref value4);
		double value5 = 0.0;
		iTaper.GetDoublePercentage(ref value5);
		double value6 = 0.0;
		iPlacementOffset.GetDouble(ref value6);
		double value7 = 0.0;
		iProngSizeMin.GetDouble(ref value7);
		double value8 = 100.0;
		iProngSizeMax.GetDouble(ref value8);
		bool value9 = false;
		iMeshedProngs.GetBoolean(ref value9);
		CSDataArray cSDataArray2 = new CSDataArray();
		int arrayCount = cSDataArray.GetArrayCount();
		ClearProngCache();
		bool flag = CSUtilGemRails.IsClosed(cSDataArray.Array);
		for (int i = 0; i <= arrayCount + 1; i++)
		{
			CSChannelGemData cSChannelGemData = null;
			if (flag)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, (i - 2 + arrayCount) % arrayCount);
			}
			else if (i - 2 >= 0)
			{
				cSChannelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, i - 2);
			}
			CSChannelGemData cSChannelGemData2 = null;
			if (flag)
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, (i - 1 + arrayCount) % arrayCount);
			}
			else if (i - 1 >= 0)
			{
				cSChannelGemData2 = CSUtilGemRails.GetChannelGemData(cSDataArray, i - 1);
			}
			CSChannelGemData cSChannelGemData3 = null;
			if (flag)
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, i % arrayCount);
			}
			else if (i < cSDataArray.GetArrayCount())
			{
				cSChannelGemData3 = CSUtilGemRails.GetChannelGemData(cSDataArray, i);
			}
			CSChannelGemData cSChannelGemData4 = null;
			if (flag)
			{
				cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, (i + 1) % arrayCount);
			}
			else if (i + 1 < cSDataArray.GetArrayCount())
			{
				cSChannelGemData4 = CSUtilGemRails.GetChannelGemData(cSDataArray, i + 1);
			}
			if (cSChannelGemData2 == null && cSChannelGemData4 != null && cSChannelGemData3 != null)
			{
				cSChannelGemData2 = new CSChannelGemData(cSChannelGemData4);
				cSChannelGemData2.ApplyTransform(cSChannelGemData3.GetMirrorForwardBack());
				cSChannelGemData2.UnitForwardVector = -cSChannelGemData2.UnitForwardVector;
			}
			if (cSChannelGemData3 == null && cSChannelGemData2 != null && cSChannelGemData != null)
			{
				cSChannelGemData3 = new CSChannelGemData(cSChannelGemData);
				cSChannelGemData3.ApplyTransform(cSChannelGemData2.GetMirrorForwardBack());
				cSChannelGemData3.UnitForwardVector = -cSChannelGemData3.UnitForwardVector;
			}
			if (cSChannelGemData3 == null || cSChannelGemData2 == null)
			{
				continue;
			}
			CSDataHelper.SetSettingType(cSChannelGemData3, GemSettingType.PR);
			OnCurve rail = CSUtilGemRails.Rail(cSDataArray);
			OnCurve val3 = CSUtilGemRails.Left(cSDataArray, rail).DuplicateCurve();
			double num3 = -1.23432101234321E+308;
			if (!val3.GetLength(ref num3))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref val3, (Type)2, 0, num3 * 0.5);
			RhUtil.RhinoExtendCurve(ref val3, (Type)2, 1, num3 * 0.5);
			OnCurve val4 = CSUtilGemRails.Right(cSDataArray, rail).DuplicateCurve();
			double num4 = -1.23432101234321E+308;
			if (!val4.GetLength(ref num4))
			{
				return UpdateError("GetLength failed");
			}
			RhUtil.RhinoExtendCurve(ref val4, (Type)2, 0, num4 * 0.5);
			RhUtil.RhinoExtendCurve(ref val4, (Type)2, 1, num4 * 0.5);
			On3dPoint val5 = (new On3dPoint((IOn3dPoint)(object)cSChannelGemData3.Center.Convert()) + (IOn3dPoint)(object)cSChannelGemData2.Center.Convert()) / 2.0;
			if (!CalculateCircleEdgeLeft(cSChannelGemData2, cSChannelGemData3, (IOnCurve)(object)val3, value2, out var circleEdgeLeft))
			{
				return UpdateError("GetCircleEdgeLeft failed");
			}
			if (!CalculateCircleEdgeRight(cSChannelGemData2, cSChannelGemData3, (IOnCurve)(object)val4, value2, out var circleEdgeRight))
			{
				return UpdateError("GetCircleEdgeRight failed");
			}
			OnCircle val6 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dVector)(object)cSChannelGemData2.Normal.Convert()), cSChannelGemData2.SizeAverage - value);
			OnCircle val7 = new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)cSChannelGemData3.Center.Convert(), (IOn3dVector)(object)cSChannelGemData3.Normal.Convert()), cSChannelGemData3.SizeAverage - value);
			On3dPoint obj = val6.ClosestPointTo(val7.Center());
			On3dPoint val8 = val7.ClosestPointTo(val6.Center());
			OnLineCurve val9 = new OnLineCurve((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val8);
			OnLineCurve val10 = new OnLineCurve(val6.Center(), val7.Center());
			On3dPoint val11 = ((OnCurve)val9).PointAt(((OnCurve)val9).Domain().Mid());
			double num5 = -1.23432101234321E+308;
			if (!((OnCurve)val10).GetClosestPoint((IOn3dPoint)(object)val11, ref num5))
			{
				return UpdateError("GetClosestPoint failed");
			}
			On3dPoint val12 = ((OnCurve)val10).PointAt(num5);
			double num6 = val11.DistanceTo((IOn3dPoint)(object)val12);
			double num7 = (cSChannelGemData2.GemZMin + cSChannelGemData3.GemZMin) / 2.0;
			double num8 = (cSChannelGemData2.GemZSize + cSChannelGemData3.GemZSize) / 2.0;
			On3dVector val13 = new On3dVector((IOn3dVector)(object)cSChannelGemData3.Normal.Convert()) + (IOn3dVector)(object)cSChannelGemData2.Normal.Convert();
			val13.Unitize();
			double num9 = 0.0;
			OnCircle val14 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)(object)val6, (IOnCircle)(object)val7, (IOnCircle)(object)circleEdgeLeft);
			val14.ClosestPointTo((IOn3dPoint)(object)val5, ref num9);
			val14.Rotate(num9 + OnUtil.On_PI / 2.0, val14.Normal());
			val14.Translate((IOn3dVector)(object)(val13 * (value6 - num7)));
			double num10 = 0.0;
			OnCircle val15 = CSUtilGeometry.CalculateApolloniusCircle((IOnCircle)(object)val6, (IOnCircle)(object)val7, (IOnCircle)(object)circleEdgeRight);
			val15.ClosestPointTo((IOn3dPoint)(object)val5, ref num10);
			val15.Rotate(num10 + OnUtil.On_PI / 2.0, val15.Normal());
			val15.Translate((IOn3dVector)(object)(val13 * (value6 - num7)));
			double height = num8 + num6 + value3 - value6;
			foreach (OnCircle item in new List<OnCircle> { val14, val15 })
			{
				if (item.radius * 2.0 > value7 && item.radius * 2.0 < value8)
				{
					OnXform val16 = new OnXform();
					val16.ChangeBasis((IOnPlane)(object)item.plane, OnUtil.On_xy_plane);
					CSData data = CreateProngCached((IOnNurbsCurve)(object)val, height, value4, value5, item.radius * 2.0, value9);
					CSDataHelper.Transform(data, val16.Convert());
					cSDataArray2.Append(data);
					num += item.radius * 2.0;
					num2 += 1.0;
				}
			}
		}
		oSurface.SetData(cSDataArray2);
		if (num2 > 0.0)
		{
			oAvgProngDiameter.SetData(new CSNumberData(num / num2));
		}
		return base.Update();
	}

	private static bool CalculateCircleEdgeLeft(CSChannelGemData gemPrevious, CSChannelGemData gemCurrent, IOnCurve rail, double edgeNudge, out OnCircle circleEdgeLeft)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_0098: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		circleEdgeLeft = new OnCircle();
		OnCurve val = rail.DuplicateCurve();
		double num = -1.23432101234321E+308;
		if (!val.GetClosestPoint((IOn3dPoint)(object)gemPrevious.Left.Convert(), ref num))
		{
			return false;
		}
		double num2 = -1.23432101234321E+308;
		if (!val.GetClosestPoint((IOn3dPoint)(object)gemCurrent.Left.Convert(), ref num2))
		{
			return false;
		}
		if (!val.Trim((IOnInterval)new OnInterval(num, num2)))
		{
			return false;
		}
		On3dPoint obj = val.PointAt(val.Domain().Mid());
		On3dVector val2 = new On3dVector((IOn3dVector)(object)gemPrevious.LeftVector.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)gemCurrent.LeftVector.Convert());
		val2.Unitize();
		On3dPoint val3 = obj + (IOn3dVector)(object)(val2 * 10.0);
		On3dVector val4 = new On3dVector((IOn3dVector)(object)gemPrevious.Normal.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)gemCurrent.Normal.Convert());
		val4.Unitize();
		OnPlane val5 = new OnPlane((IOn3dPoint)(object)val3, (IOn3dVector)(object)val4);
		double num3 = obj.DistanceTo((IOn3dPoint)(object)val3);
		circleEdgeLeft = new OnCircle((IOnPlane)(object)val5, (IOn3dPoint)(object)val3, num3);
		circleEdgeLeft.radius -= edgeNudge;
		return true;
	}

	private static bool CalculateCircleEdgeRight(CSChannelGemData gemPrevious, CSChannelGemData gemCurrent, IOnCurve rail, double edgeNudge, out OnCircle circleEdgeRight)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_0098: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		circleEdgeRight = new OnCircle();
		OnCurve val = rail.DuplicateCurve();
		double num = -1.23432101234321E+308;
		if (!val.GetClosestPoint((IOn3dPoint)(object)gemPrevious.Right.Convert(), ref num))
		{
			return false;
		}
		double num2 = -1.23432101234321E+308;
		if (!val.GetClosestPoint((IOn3dPoint)(object)gemCurrent.Right.Convert(), ref num2))
		{
			return false;
		}
		if (!val.Trim((IOnInterval)new OnInterval(num, num2)))
		{
			return false;
		}
		On3dPoint obj = val.PointAt(val.Domain().Mid());
		On3dVector val2 = new On3dVector((IOn3dVector)(object)gemPrevious.RightVector.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)gemCurrent.RightVector.Convert());
		val2.Unitize();
		On3dPoint val3 = obj + (IOn3dVector)(object)(val2 * 10.0);
		On3dVector val4 = new On3dVector((IOn3dVector)(object)gemPrevious.Normal.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)gemCurrent.Normal.Convert());
		val4.Unitize();
		OnPlane val5 = new OnPlane((IOn3dPoint)(object)val3, (IOn3dVector)(object)val4);
		double num3 = obj.DistanceTo((IOn3dPoint)(object)val3);
		circleEdgeRight = new OnCircle((IOnPlane)(object)val5, (IOn3dPoint)(object)val3, num3);
		circleEdgeRight.radius -= edgeNudge;
		return true;
	}

	private void ClearProngCache()
	{
		_prongCachedDiameter = -1.0;
		_prongCachedData = null;
	}

	private CSData CreateProngCached(IOnNurbsCurve vProfile, double height, double filletPer, double taperPer, double diameter, bool doMeshedProngs)
	{
		if (_prongCachedData != null && Math.Abs(diameter - _prongCachedDiameter) < 0.01)
		{
			return _prongCachedData.Duplicate();
		}
		if (CSApplicationHelper.Application.IsCamMode)
		{
			height += CSApplicationHelper.Application.CamProngExtensionHeight;
		}
		OnNurbsSurface nurbsSurface;
		if (doMeshedProngs)
		{
			if (CSUtilGeometry.CreateMeshProng(vProfile, diameter, height, filletPer, taperPer, out var meshSurface))
			{
				_prongCachedDiameter = diameter;
				_prongCachedData = new CSOnGeometryData(meshSurface.Convert());
				return _prongCachedData.Duplicate();
			}
		}
		else if (CSUtilGeometry.CreateProng(vProfile, diameter, height, filletPer, taperPer, out nurbsSurface))
		{
			_prongCachedDiameter = diameter;
			_prongCachedData = new CSOnGeometryData(nurbsSurface.Convert());
			return _prongCachedData.Duplicate();
		}
		return new CSData();
	}
}
