using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Profiles;

[CSBlock("ProfilePlacerBetweenRails", "{CBDFDA63-1729-41ae-AB9C-DB1DF0AACC99}")]
public sealed class CSProfilePlacerBetweenRails : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iRail2 { get; }

	public CSInputChannel iProfile { get; }

	public CSInputChannel iParameter { get; }

	public CSInputChannel iParameter2 { get; }

	public CSInputChannel iSizeY { get; }

	public CSInputChannel iFromObject { get; }

	public CSInputChannel iFlipZ { get; }

	public CSInputChannel iOffsetZ { get; }

	public CSInputChannel iFlipX { get; }

	public CSInputChannel iTilt { get; }

	public CSInputChannel iRotate { get; }

	public CSInputChannel iTwist { get; }

	public CSInputChannel iSizeX { get; }

	public CSOutputChannel oProfile { get; }

	public CSOutputChannel oParam1 { get; }

	public CSOutputChannel oParam2 { get; }

	public CSProfilePlacerBetweenRails(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRail2 = new CSInputChannel(GetPropertyName(() => iRail2), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve | ChannelDataType.Profile, ChannelArrayType.Individual);
		iParameter = new CSInputChannel(GetPropertyName(() => iParameter), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iParameter2 = new CSInputChannel(GetPropertyName(() => iParameter2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSizeY = new CSInputChannel(GetPropertyName(() => iSizeY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFromObject = new CSInputChannel(GetPropertyName(() => iFromObject), this, ChannelDataType.Point | ChannelDataType.Curve, ChannelArrayType.Individual);
		iFlipZ = new CSInputChannel(GetPropertyName(() => iFlipZ), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iOffsetZ = new CSInputChannel(GetPropertyName(() => iOffsetZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iFlipX = new CSInputChannel(GetPropertyName(() => iFlipX), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iTilt = new CSInputChannel(GetPropertyName(() => iTilt), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRotate = new CSInputChannel(GetPropertyName(() => iRotate), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTwist = new CSInputChannel(GetPropertyName(() => iTwist), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSizeX = new CSInputChannel(GetPropertyName(() => iSizeX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfile = new CSOutputChannel(GetPropertyName(() => oProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oParam1 = new CSOutputChannel(GetPropertyName(() => oParam1), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParam2 = new CSOutputChannel(GetPropertyName(() => oParam2), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Expected O, but got Unknown
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Expected O, but got Unknown
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Expected O, but got Unknown
		//IL_0760: Unknown result type (might be due to invalid IL or missing references)
		//IL_0767: Expected O, but got Unknown
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a9: Expected O, but got Unknown
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07eb: Expected O, but got Unknown
		CSDataArray flattenArray = iRail.GetFlattenArray();
		CSDataArray flattenArray2 = iRail2.GetFlattenArray();
		if (flattenArray.GetArrayCount() < 1 || flattenArray2.GetArrayCount() < 1)
		{
			return UpdateMissing("CSProfilePlacerBetweenRails: Missing inputs\n");
		}
		OnNurbsCurve val = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		OnNurbsCurve val2 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		bool value = false;
		iFlipX.GetBoolean(ref value);
		if (value)
		{
			OnNurbsCurve obj = val2;
			val2 = val;
			val = obj;
		}
		CSData cSData = iProfile.Data.Duplicate();
		if (val == null || val2 == null || cSData == null)
		{
			return UpdateMissing("CSProfilePlacerBetweenRails: Missing rails\n");
		}
		double value2 = 0.0;
		iParameter.GetDouble(ref value2);
		double value3 = value2;
		iParameter2.GetDouble(ref value3);
		bool flag = false;
		if (iFromObject.OutputChannelBlock != null)
		{
			double num = ((OnCurve)val).Domain().Min();
			double num2 = ((OnCurve)val2).Domain().Min();
			bool flag2 = false;
			bool flag3 = false;
			OnNurbsCurve nurbsCurve = iFromObject.GetNurbsCurve();
			OnPlane val3 = new OnPlane();
			if (nurbsCurve != null && ((OnCurve)nurbsCurve).IsPlanar(val3))
			{
				OnPlaneSurface val4 = new OnPlaneSurface((IOnPlane)(object)val3);
				On3dPoint val5 = val3.PointAt(0.5, 0.5);
				OnXform val6 = new OnXform();
				val6.Scale((IOn3dPoint)(object)val5, 1000.0);
				val4.Transform((IOnXform)(object)val6);
				ArrayOnX_EVENT val7 = new ArrayOnX_EVENT();
				((OnCurve)val).IntersectSurface((IOnSurface)(object)val4, ref val7);
				if (val7.Count() > 0)
				{
					int num3 = -1;
					double num4 = 1000.0;
					for (int i = 0; i < val7.Count(); i++)
					{
						if (!val7[i].IsOverlapEvent())
						{
							double num5 = val7[i][0].DistanceTo((IOn3dPoint)(object)((OnCurve)nurbsCurve).PointAtStart());
							if (num5 < num4)
							{
								num3 = i;
								num4 = num5;
							}
						}
					}
					if (num3 >= 0)
					{
						num = val7[num3][0];
						flag2 = true;
					}
				}
				val7 = new ArrayOnX_EVENT();
				((OnCurve)val2).IntersectSurface((IOnSurface)(object)val4, ref val7);
				if (val7.Count() > 0)
				{
					int num6 = -1;
					double num7 = 1000.0;
					for (int j = 0; j < val7.Count(); j++)
					{
						if (!val7[j].IsOverlapEvent())
						{
							double num8 = val7[j][0].DistanceTo((IOn3dPoint)(object)((OnCurve)nurbsCurve).PointAtStart());
							if (num8 < num7)
							{
								num6 = j;
								num7 = num8;
							}
						}
					}
					if (num6 >= 0)
					{
						num2 = val7[num6][0];
						flag3 = true;
					}
				}
			}
			On3dPoint onPoint = new On3dPoint();
			if (!flag2 && iFromObject.GetPoint(ref onPoint))
			{
				((OnCurve)val).GetClosestPoint((IOn3dPoint)(object)onPoint, ref num);
			}
			if (!flag3 && iFromObject.GetPoint(ref onPoint))
			{
				((OnCurve)val2).GetClosestPoint((IOn3dPoint)(object)onPoint, ref num2);
			}
			if (((OnCurve)val).IsClosed() && ((OnCurve)val2).IsClosed())
			{
				((OnCurve)val).ChangeClosedCurveSeam(num);
				((OnCurve)val2).ChangeClosedCurveSeam(num2);
				if (value2 < 0.0)
				{
					((OnCurve)val).Reverse();
					value2 = 0.0 - value2;
					flag = true;
				}
				if (value3 < 0.0)
				{
					((OnCurve)val2).Reverse();
					value3 = 0.0 - value3;
				}
			}
			else
			{
				double num9 = 1.0;
				((OnCurve)val).GetLength(ref num9);
				double num10 = 1.0;
				if (value2 > 0.0)
				{
					if (((OnCurve)val).Domain().Max() != num)
					{
						((OnCurve)val).Trim((IOnInterval)new OnInterval(num, ((OnCurve)val).Domain().Max()));
					}
					else
					{
						value2 = 1.0;
					}
				}
				else if (((OnCurve)val).Domain().Min() != num)
				{
					((OnCurve)val).Trim((IOnInterval)new OnInterval(((OnCurve)val).Domain().Min(), num));
					value2 = 0.0 - value2;
					((OnCurve)val).Reverse();
					flag = true;
				}
				else
				{
					value3 = 0.0;
				}
				((OnCurve)val).GetLength(ref num10);
				value2 /= num10 / num9;
				num9 = 1.0;
				((OnCurve)val2).GetLength(ref num9);
				if (value3 > 0.0)
				{
					if (((OnCurve)val2).Domain().Max() != num2)
					{
						((OnCurve)val2).Trim((IOnInterval)new OnInterval(num2, ((OnCurve)val2).Domain().Max()));
					}
					else
					{
						value3 = 1.0;
					}
				}
				else if (((OnCurve)val2).Domain().Min() != num2)
				{
					((OnCurve)val2).Trim((IOnInterval)new OnInterval(((OnCurve)val2).Domain().Min(), num2));
					value3 = 0.0 - value3;
					((OnCurve)val2).Reverse();
				}
				else
				{
					value3 = 0.0;
				}
				((OnCurve)val2).GetLength(ref num10);
				value3 /= num10 / num9;
			}
		}
		double num11 = 0.0;
		double num12 = 0.0;
		if (value2 >= 1.0)
		{
			num11 = ((OnCurve)val).Domain().Max();
		}
		else if (value2 <= 0.0)
		{
			num11 = ((OnCurve)val).Domain().Min();
		}
		else
		{
			((OnCurve)val).GetNormalizedArcLengthPoint(value2, ref num11);
		}
		if (value3 >= 1.0)
		{
			num12 = ((OnCurve)val2).Domain().Max();
		}
		else if (value3 <= 0.0)
		{
			num12 = ((OnCurve)val2).Domain().Min();
		}
		else
		{
			((OnCurve)val2).GetNormalizedArcLengthPoint(value3, ref num12);
		}
		On3dPoint val8 = ((OnCurve)val).PointAt(num11);
		On3dPoint val9 = ((OnCurve)val2).PointAt(num12);
		BoundingBox boundingBox = BoundingBox.Unset;
		cSData.GetTightBoundingBox(ref boundingBox);
		OnXform val10 = new OnXform();
		val10.Translation((IOn3dVector)(object)(new On3dPoint(OnUtil.On_origin) - (IOn3dPoint)(object)boundingBox.Min.Convert()));
		OnXform val11 = new OnXform();
		double value4 = 1.0;
		double y = boundingBox.Diagonal.Y;
		double x = boundingBox.Diagonal.X;
		y = ((y < 0.0001) ? 1.0 : y);
		x = ((x < 0.0001) ? 1.0 : x);
		if (iSizeY != null)
		{
			if (iSizeY.GetDouble(ref value4))
			{
				val11.Scale(1.0 / x, value4 / y, 1.0);
			}
			else
			{
				val11.Scale(OnUtil.On_origin, 1.0 / x);
			}
		}
		else
		{
			val11.Scale(OnUtil.On_origin, 1.0 / x);
		}
		OnXform val12 = new OnXform();
		On3dVector val13 = val9 - (IOn3dPoint)(object)val8;
		val13.Unitize();
		On3dVector val14 = ((OnCurve)val).TangentAt(num11);
		val14.Unitize();
		On3dVector val15 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val13, (IOn3dVector)(object)val14);
		val15.Unitize();
		bool value5 = false;
		iFlipZ.GetBoolean(ref value5);
		if (value5)
		{
			val15.Reverse();
		}
		if (flag)
		{
			val15.Reverse();
		}
		if (Math.Abs(val15.Length() - 1.0) > 0.1)
		{
			val15 = (val8 + (IOn3dPoint)(object)val9) / 2.0 - OnUtil.On_origin;
		}
		double value6 = 0.0;
		iOffsetZ.GetDouble(ref value6);
		On3dVector val16 = val9 - (IOn3dPoint)(object)val8;
		double value7 = 1.0;
		if (iSizeX != null && iSizeX.GetDouble(ref value7))
		{
			val16.Unitize();
			val16 *= value7;
		}
		OnXform val17 = new OnXform();
		val17.Identity();
		double value8 = 0.0;
		if (iTilt != null && iTilt.GetDouble(ref value8))
		{
			val17.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value8), (IOn3dVector)(object)val13, (IOn3dPoint)(object)val8);
		}
		OnXform val18 = new OnXform();
		val18.Identity();
		double value9 = 0.0;
		if (iRotate != null && iRotate.GetDouble(ref value9))
		{
			val18.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value9), (IOn3dVector)(object)val14, (IOn3dPoint)(object)val8);
		}
		OnXform val19 = new OnXform();
		val19.Identity();
		double value10 = 0.0;
		if (iTwist != null && iTwist.GetDouble(ref value10))
		{
			val19.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value10), (IOn3dVector)(object)val15, (IOn3dPoint)(object)val8);
		}
		val12.ChangeBasis((IOn3dPoint)(object)(val8 + (IOn3dVector)(object)(val15 * value6)), (IOn3dVector)(object)val16, (IOn3dVector)(object)val15, (IOn3dVector)(object)val14, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
		CSDataHelper.Transform(cSData, val19.Convert() * val18.Convert() * val17.Convert() * val12.Convert() * val11.Convert() * val10.Convert());
		oProfile.SetData(cSData);
		oParam1.SetData(new CSNumberData(num11));
		oParam2.SetData(new CSNumberData(num12));
		return base.Update();
	}
}
