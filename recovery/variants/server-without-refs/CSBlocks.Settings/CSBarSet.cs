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
using Rhino.Geometry;

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
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Expected O, but got Unknown
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Expected O, but got Unknown
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Expected O, but got Unknown
		//IL_05a0: Expected O, but got Unknown
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		if (!(InSortedGem.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		OnNurbsCurve val = InProfile.GetNurbsCurve();
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
		OnBoundingBox val3 = new OnBoundingBox();
		((OnGeometry)val).GetTightBoundingBox(ref val3);
		OnXform val4 = new OnXform();
		val4.Translation((0.0 - (val3.Max().x + val3.Min().x)) / 2.0, 0.0 - val3.Min().y, 0.0);
		OnXform val5 = new OnXform();
		val5.Scale(1.0 / (val3.Max().x - val3.Min().x), 1.0 / (val3.Max().y - val3.Min().y), 1.0);
		val.Transform((IOnXform)(object)(val5 * (IOnXform)(object)val4));
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
				OnLine val6 = new OnLine((IOn3dPoint)(object)cSChannelGemData2.Center.Convert(), (IOn3dPoint)(object)cSChannelGemData3.Center.Convert());
				OnLine val7 = new OnLine((IOn3dPoint)(object)cSChannelGemData2.TableCenterPoint.Convert(), (IOn3dPoint)(object)cSChannelGemData3.TableCenterPoint.Convert());
				On3dPoint centerLineCenter = val6.PointAt(0.5);
				On3dPoint closestPointToGemGirdle = GetClosestPointToGemGirdle(cSChannelGemData2, (IOn3dPoint)(object)centerLineCenter);
				On3dPoint closestPointToGemGirdle2 = GetClosestPointToGemGirdle(cSChannelGemData3, (IOn3dPoint)(object)centerLineCenter);
				OnLine val8 = new OnLine((IOn3dPoint)(object)closestPointToGemGirdle, (IOn3dPoint)(object)closestPointToGemGirdle2);
				On3dPoint val9 = val8.PointAt(0.5);
				On3dPoint val10 = val7.ClosestPointTo((IOn3dPoint)(object)val9);
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
				double height = num4 + num5 + val10.DistanceTo((IOn3dPoint)(object)val9);
				double num7 = Math.Min(value8, (val8.Length() + cSChannelGemData3.SizeForward * value7 * 2.0 + cSChannelGemData2.SizeForward * value7 * 2.0) / 2.0 - 0.01);
				double halfThickness = val8.Length() / 2.0 - num7;
				double gemNextNudge = cSChannelGemData3.SizeForward * value7 * 2.0;
				double gemCurrentNudge = cSChannelGemData2.SizeForward * value7 * 2.0;
				CSData data = CreateBarCached(val, num5, width, height, num7, halfThickness, gemNextNudge, gemCurrentNudge);
				On3dVector val11 = new On3dVector((IOn3dVector)(object)cSChannelGemData2.Up.Convert()) + (IOn3dVector)new On3dVector((IOn3dVector)(object)cSChannelGemData3.Up.Convert());
				val11.Unitize();
				On3dVector destination = val8.Tangent();
				OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)val9, (IOn3dVector)(object)val11, (IOn3dVector)(object)destination);
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
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		OnNurbsCurve val = vProfile.Duplicate();
		OnXform val2 = new OnXform();
		val2.Scale(width, height, 1.0);
		val.Transform((IOnXform)(object)val2);
		On3dVector val3 = new On3dVector((IOn3dVector)(object)(((OnGeometry)val).BoundingBox().Center() - OnUtil.On_origin));
		((OnGeometry)val).Translate((IOn3dVector)(object)(-val3));
		OnNurbsSurface obj = CSUtilRailRevolve.RailRevolve((IOnNurbsCurve)(object)CSUtilGeometry.CreateDome(domeAmountCurrent), (IOnNurbsCurve)(object)val, 0);
		((OnGeometry)obj).Translate((IOn3dVector)(object)val3);
		((OnGeometry)val).Translate((IOn3dVector)(object)val3);
		OnXform val4 = new OnXform();
		val4.Scale(1.0, 1.0, -1.0);
		CSOnGeometryData cSOnGeometryData = new CSOnGeometryData((GeometryBase)(object)obj.Convert());
		CSOnGeometryData cSOnGeometryData2 = new CSOnGeometryData((GeometryBase)(object)((OnSurface)obj).DuplicateSurface().Convert());
		OnXform val5 = new OnXform();
		val5.Translation(0.0, 0.0 - bottomDepth, halfThickness + gemNextNudge);
		val.Transform((IOnXform)(object)val5);
		cSOnGeometryData.ApplyTransform(val5.Convert());
		OnCurve val6 = (OnCurve)(object)vProfile.Duplicate();
		OnXform val7 = new OnXform();
		val7.Translation(0.0, 0.0 - bottomDepth, 0.0 - halfThickness - gemCurrentNudge);
		((OnGeometry)val6).Transform((IOnXform)(object)(val7 * (IOnXform)(object)val2));
		cSOnGeometryData2.ApplyTransform(val7.Convert() * val4.Convert());
		OnBrep brep = CSUtilSweep.Loft((IOnCurve[])(object)new IOnCurve[2]
		{
			(IOnCurve)val,
			(IOnCurve)val6
		}, (eLoftType)0, capped: false);
		CSDataArray cSDataArray = new CSDataArray();
		cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)brep.Convert()));
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
		double num = 0.0;
		OnNurbsCurve obj = gemCurrent.GetNurbsCurve().Convert();
		((OnCurve)obj).GetClosestPoint(centerLineCenter, ref num);
		return ((OnCurve)obj).PointAt(num);
	}
}
