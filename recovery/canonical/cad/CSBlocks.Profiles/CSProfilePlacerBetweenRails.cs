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
		CSDataArray flattenArray = iRail.GetFlattenArray();
		CSDataArray flattenArray2 = iRail2.GetFlattenArray();
		if (flattenArray.GetArrayCount() < 1 || flattenArray2.GetArrayCount() < 1)
		{
			return UpdateMissing("CSProfilePlacerBetweenRails: Missing inputs\n");
		}
		OnNurbsCurve onNurbsCurve = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		OnNurbsCurve onNurbsCurve2 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		bool value = false;
		iFlipX.GetBoolean(ref value);
		if (value)
		{
			OnNurbsCurve onNurbsCurve3 = onNurbsCurve2;
			onNurbsCurve2 = onNurbsCurve;
			onNurbsCurve = onNurbsCurve3;
		}
		CSData cSData = iProfile.Data.Duplicate();
		if (onNurbsCurve == null || onNurbsCurve2 == null || cSData == null)
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
			double t = onNurbsCurve.Domain().Min();
			double t2 = onNurbsCurve2.Domain().Min();
			bool flag2 = false;
			bool flag3 = false;
			OnNurbsCurve nurbsCurve = iFromObject.GetNurbsCurve();
			OnPlane onPlane = new OnPlane();
			if (nurbsCurve != null && nurbsCurve.IsPlanar(onPlane))
			{
				OnPlaneSurface onPlaneSurface = new OnPlaneSurface(onPlane);
				On3dPoint fixed_point = onPlane.PointAt(0.5, 0.5);
				OnXform onXform = new OnXform();
				onXform.Scale(fixed_point, 1000.0);
				onPlaneSurface.Transform(onXform);
				ArrayOnX_EVENT x = new ArrayOnX_EVENT();
				onNurbsCurve.IntersectSurface(onPlaneSurface, ref x);
				if (x.Count() > 0)
				{
					int num = -1;
					double num2 = 1000.0;
					for (int i = 0; i < x.Count(); i++)
					{
						if (!x[i].IsOverlapEvent())
						{
							double num3 = x[i].get_m_pointA(0).DistanceTo(nurbsCurve.PointAtStart());
							if (num3 < num2)
							{
								num = i;
								num2 = num3;
							}
						}
					}
					if (num >= 0)
					{
						t = x[num].get_m_a(0);
						flag2 = true;
					}
				}
				x = new ArrayOnX_EVENT();
				onNurbsCurve2.IntersectSurface(onPlaneSurface, ref x);
				if (x.Count() > 0)
				{
					int num4 = -1;
					double num5 = 1000.0;
					for (int j = 0; j < x.Count(); j++)
					{
						if (!x[j].IsOverlapEvent())
						{
							double num6 = x[j].get_m_pointA(0).DistanceTo(nurbsCurve.PointAtStart());
							if (num6 < num5)
							{
								num4 = j;
								num5 = num6;
							}
						}
					}
					if (num4 >= 0)
					{
						t2 = x[num4].get_m_a(0);
						flag3 = true;
					}
				}
			}
			On3dPoint onPoint = new On3dPoint();
			if (!flag2 && iFromObject.GetPoint(ref onPoint))
			{
				onNurbsCurve.GetClosestPoint(onPoint, ref t);
			}
			if (!flag3 && iFromObject.GetPoint(ref onPoint))
			{
				onNurbsCurve2.GetClosestPoint(onPoint, ref t2);
			}
			if (onNurbsCurve.IsClosed() && onNurbsCurve2.IsClosed())
			{
				onNurbsCurve.ChangeClosedCurveSeam(t);
				onNurbsCurve2.ChangeClosedCurveSeam(t2);
				if (value2 < 0.0)
				{
					onNurbsCurve.Reverse();
					value2 = 0.0 - value2;
					flag = true;
				}
				if (value3 < 0.0)
				{
					onNurbsCurve2.Reverse();
					value3 = 0.0 - value3;
				}
			}
			else
			{
				double length = 1.0;
				onNurbsCurve.GetLength(ref length);
				double length2 = 1.0;
				if (value2 > 0.0)
				{
					if (onNurbsCurve.Domain().Max() != t)
					{
						onNurbsCurve.Trim(new OnInterval(t, onNurbsCurve.Domain().Max()));
					}
					else
					{
						value2 = 1.0;
					}
				}
				else if (onNurbsCurve.Domain().Min() != t)
				{
					onNurbsCurve.Trim(new OnInterval(onNurbsCurve.Domain().Min(), t));
					value2 = 0.0 - value2;
					onNurbsCurve.Reverse();
					flag = true;
				}
				else
				{
					value3 = 0.0;
				}
				onNurbsCurve.GetLength(ref length2);
				value2 /= length2 / length;
				length = 1.0;
				onNurbsCurve2.GetLength(ref length);
				if (value3 > 0.0)
				{
					if (onNurbsCurve2.Domain().Max() != t2)
					{
						onNurbsCurve2.Trim(new OnInterval(t2, onNurbsCurve2.Domain().Max()));
					}
					else
					{
						value3 = 1.0;
					}
				}
				else if (onNurbsCurve2.Domain().Min() != t2)
				{
					onNurbsCurve2.Trim(new OnInterval(onNurbsCurve2.Domain().Min(), t2));
					value3 = 0.0 - value3;
					onNurbsCurve2.Reverse();
				}
				else
				{
					value3 = 0.0;
				}
				onNurbsCurve2.GetLength(ref length2);
				value3 /= length2 / length;
			}
		}
		double t3 = 0.0;
		double t4 = 0.0;
		if (value2 >= 1.0)
		{
			t3 = onNurbsCurve.Domain().Max();
		}
		else if (value2 <= 0.0)
		{
			t3 = onNurbsCurve.Domain().Min();
		}
		else
		{
			onNurbsCurve.GetNormalizedArcLengthPoint(value2, ref t3);
		}
		if (value3 >= 1.0)
		{
			t4 = onNurbsCurve2.Domain().Max();
		}
		else if (value3 <= 0.0)
		{
			t4 = onNurbsCurve2.Domain().Min();
		}
		else
		{
			onNurbsCurve2.GetNormalizedArcLengthPoint(value3, ref t4);
		}
		On3dPoint on3dPoint = onNurbsCurve.PointAt(t3);
		On3dPoint on3dPoint2 = onNurbsCurve2.PointAt(t4);
		BoundingBox boundingBox = BoundingBox.Unset;
		cSData.GetTightBoundingBox(ref boundingBox);
		OnXform onXform2 = new OnXform();
		onXform2.Translation(new On3dPoint(OnUtil.On_origin) - boundingBox.Min.Convert());
		OnXform onXform3 = new OnXform();
		double value4 = 1.0;
		double y = boundingBox.Diagonal.Y;
		double x2 = boundingBox.Diagonal.X;
		y = ((y < 0.0001) ? 1.0 : y);
		x2 = ((x2 < 0.0001) ? 1.0 : x2);
		if (iSizeY != null)
		{
			if (iSizeY.GetDouble(ref value4))
			{
				onXform3.Scale(1.0 / x2, value4 / y, 1.0);
			}
			else
			{
				onXform3.Scale(OnUtil.On_origin, 1.0 / x2);
			}
		}
		else
		{
			onXform3.Scale(OnUtil.On_origin, 1.0 / x2);
		}
		OnXform onXform4 = new OnXform();
		On3dVector on3dVector = on3dPoint2 - on3dPoint;
		on3dVector.Unitize();
		On3dVector on3dVector2 = onNurbsCurve.TangentAt(t3);
		on3dVector2.Unitize();
		On3dVector on3dVector3 = OnUtil.ON_CrossProduct(on3dVector, on3dVector2);
		on3dVector3.Unitize();
		bool value5 = false;
		iFlipZ.GetBoolean(ref value5);
		if (value5)
		{
			on3dVector3.Reverse();
		}
		if (flag)
		{
			on3dVector3.Reverse();
		}
		if (Math.Abs(on3dVector3.Length() - 1.0) > 0.1)
		{
			on3dVector3 = (on3dPoint + on3dPoint2) / 2.0 - OnUtil.On_origin;
		}
		double value6 = 0.0;
		iOffsetZ.GetDouble(ref value6);
		On3dVector on3dVector4 = on3dPoint2 - on3dPoint;
		double value7 = 1.0;
		if (iSizeX != null && iSizeX.GetDouble(ref value7))
		{
			on3dVector4.Unitize();
			on3dVector4 *= value7;
		}
		OnXform onXform5 = new OnXform();
		onXform5.Identity();
		double value8 = 0.0;
		if (iTilt != null && iTilt.GetDouble(ref value8))
		{
			onXform5.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value8), on3dVector, on3dPoint);
		}
		OnXform onXform6 = new OnXform();
		onXform6.Identity();
		double value9 = 0.0;
		if (iRotate != null && iRotate.GetDouble(ref value9))
		{
			onXform6.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value9), on3dVector2, on3dPoint);
		}
		OnXform onXform7 = new OnXform();
		onXform7.Identity();
		double value10 = 0.0;
		if (iTwist != null && iTwist.GetDouble(ref value10))
		{
			onXform7.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value10), on3dVector3, on3dPoint);
		}
		onXform4.ChangeBasis(on3dPoint + on3dVector3 * value6, on3dVector4, on3dVector3, on3dVector2, OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_yaxis, OnUtil.On_zaxis);
		CSDataHelper.Transform(cSData, onXform7.Convert() * onXform6.Convert() * onXform5.Convert() * onXform4.Convert() * onXform3.Convert() * onXform2.Convert());
		oProfile.SetData(cSData);
		oParam1.SetData(new CSNumberData(t3));
		oParam2.SetData(new CSNumberData(t4));
		return base.Update();
	}
}
