using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemPlacerSide_V2", "{27B9B79D-F165-4e90-BB2E-8308960B25FA}")]
public sealed class CSGemPlacerSide2 : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGem;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemPairCount;

	public readonly CSInputChannel InGemSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemYOffsetMirror;

	public readonly CSInputChannel InGemTilt;

	public readonly CSInputChannel InGemAngle;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemRotationMirror;

	public readonly CSOutputChannel OutGem;

	public CSGemPlacerSide2(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemPairCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffsetMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotationMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSData data = InCurve.Data;
		if (data == null)
		{
			return UpdateMissing("Curve");
		}
		OnCurve onCurve = InCurve.NurbsCurve.Convert();
		if (onCurve == null)
		{
			return UpdateMissing("Curve");
		}
		List<CSGemData> gemList = InGem.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Gems");
		}
		CSGemData cSGemData = gemList[0];
		CSGemData cSGemData2 = gemList[gemList.Count - 1];
		CSGemData gem = InGemDefinition.Gem;
		double num = InGemSpacing.Double;
		double num2 = InGemZOffset.Double;
		double num3 = InGemYOffset.Double;
		double num4 = InGemTilt.Double;
		num4 /= 100.0;
		double angleDegrees = InGemAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double angleDegrees2 = InGemRotation.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		int integer = InGemPairCount.Integer;
		bool boolean = InGemYOffsetMirror.Boolean;
		bool boolean2 = InGemRotationMirror.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataHelper.GetTransformNoArray(data, out var value);
		if (!value.IsValid)
		{
			value = Transform.Identity;
		}
		On3dPoint on3dPoint = new On3dPoint(OnUtil.On_origin);
		on3dPoint.Transform(value.Convert());
		On3dVector on3dVector = onCurve.TangentAt(onCurve.Domain().Min());
		On3dVector on3dVector2;
		On3dVector on3dVector3;
		if (data is CSOutsideRingRailData || data is CSRingRailData)
		{
			on3dVector2 = new On3dVector(OnUtil.On_zaxis);
			on3dVector2.Transform(value.Convert());
			on3dVector2.Unitize();
			on3dVector3 = OnUtil.ON_CrossProduct(on3dVector2, on3dVector);
			on3dVector3.Unitize();
		}
		else
		{
			on3dVector2 = new On3dVector(OnUtil.On_yaxis);
			on3dVector2.Transform(value.Convert());
			on3dVector2.Unitize();
			on3dVector3 = OnUtil.ON_CrossProduct(on3dVector, on3dVector2);
			on3dVector3.Unitize();
		}
		OnXform onXform = new OnXform();
		onXform.Rotation(angleDegrees, OnUtil.On_xaxis, OnUtil.On_origin);
		double angle_radians = (boolean2 ? (0.0 - angleDegrees2) : angleDegrees2);
		OnXform onXform2 = new OnXform();
		onXform2.Rotation(angle_radians, OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform onXform3 = new OnXform();
		if (boolean)
		{
			onXform3.Translation(0.0, 0.0 - num3, 0.0);
		}
		else
		{
			onXform3.Translation(0.0, num3, 0.0);
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, onXform.Convert() * onXform2.Convert()))
		{
			return UpdateError("Rhs Gem Bounding Box");
		}
		double t = 0.0;
		if (!onCurve.GetClosestPoint(cSGemData.Center.Convert(), ref t))
		{
			return UpdateError("Rhs Closest Point");
		}
		On3dPoint on3dPoint2 = onCurve.PointAt(t);
		On3dVector on3dVector4 = new On3dPoint(cSGemData.Center.Convert()) - on3dPoint2;
		On3dVector on3dVector5 = on3dPoint2 - new On3dPoint(OnUtil.On_origin);
		on3dVector5.Unitize();
		OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(cSGemData.Center.Convert(), on3dVector3, on3dVector5, OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!cSGemData.GetTightBoundingBox(ref boundingBox2, doGrow: false, xform.Convert()))
		{
			return UpdateError("Rhs Test Gem Bounding Box");
		}
		OnXform onXform4 = new OnXform();
		onXform4.Translation(0.0, num3, 0.0);
		OnXform onXform5 = new OnXform();
		onXform5.Rotation(angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform xform2 = onXform * onXform5;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, xform2.Convert()))
		{
			return UpdateError("Lhs Gem Bounding Box");
		}
		double t2 = 0.0;
		if (!onCurve.GetClosestPoint(cSGemData2.Center.Convert(), ref t2))
		{
			return UpdateError("Lhs Closest Point");
		}
		On3dPoint on3dPoint3 = onCurve.PointAt(t2);
		_ = new On3dPoint(cSGemData2.Center.Convert()) - on3dPoint3;
		On3dVector on3dVector6 = on3dPoint3 - new On3dPoint(OnUtil.On_origin);
		on3dVector6.Unitize();
		OnXform xform3 = CSBlockUtil.CSUtilMath.Orient3Point(cSGemData2.Center.Convert(), on3dVector3, on3dVector6, OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis);
		BoundingBox boundingBox4 = BoundingBox.Unset;
		if (!cSGemData2.GetTightBoundingBox(ref boundingBox4, doGrow: false, xform3.Convert()))
		{
			return UpdateError("Lhs Test Gem Bounding Box");
		}
		double num5 = on3dVector4.Length();
		if (OnUtil.ON_DotProduct(on3dVector4, on3dVector5) < 0.0)
		{
			num5 = 0.0 - num5;
		}
		num5 += num2;
		OnCurve onCurve2 = CSUtilGeometry.OffsetOld(onCurve, -on3dVector3, on3dPoint, num5, closed: true);
		if (onCurve2 == null)
		{
			return UpdateError("Offset Curve");
		}
		double length = 0.0;
		if (!onCurve2.GetLength(ref length))
		{
			return UpdateError("Offset Curve Length");
		}
		double length2 = 0.0;
		if (!onCurve2.GetClosestPoint(cSGemData.Center.Convert(), ref t))
		{
			return UpdateError("Rhs Closest Point");
		}
		if (!onCurve2.GetLength(ref length2, 0.01, new OnInterval(onCurve2.Domain().Min(), t)))
		{
			return UpdateError("Rhs Closest Point Length");
		}
		double num6 = Math.Abs(boundingBox2.Max.X) + Math.Abs(boundingBox.Min.X) + num;
		for (int num7 = integer - 1; num7 >= 0; num7--)
		{
			double num8 = (length2 + (num6 + (double)num7 * (boundingBox.Max.X - boundingBox.Min.X + num))) / length;
			double t3 = 0.0;
			if (num8 > 1.0)
			{
				num8 -= 1.0;
			}
			if (num8 < 0.0)
			{
				num8 += 1.0;
			}
			if (onCurve2.GetNormalizedArcLengthPoint(num8, ref t3))
			{
				On3dVector on3dVector7 = OnUtil.ON_CrossProduct(onCurve2.TangentAt(t3), on3dVector3);
				on3dVector7.Unitize();
				On3dVector on3dVector8 = on3dVector7 * num4 + on3dVector2 * (1.0 - num4);
				if (on3dVector8.Length() < 0.001)
				{
					on3dVector8 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					on3dVector8.Unitize();
				}
				OnXform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, gem.North.Convert(), gem.Up.Convert(), onCurve2.PointAt(t3), on3dVector3, on3dVector8);
				CSData data2 = gem.Duplicate();
				CSDataHelper.Transform(data2, xform4.Convert() * onXform3.Convert() * onXform.Convert() * onXform2.Convert());
				cSDataArray.Append(data2);
			}
		}
		double length3 = 0.0;
		if (!onCurve2.GetClosestPoint(cSGemData2.Center.Convert(), ref t2))
		{
			return UpdateError("Lhs Closest Point");
		}
		if (!onCurve2.GetLength(ref length3, 0.01, new OnInterval(onCurve2.Domain().Min(), t2)))
		{
			return UpdateError("Lhs Closest Point Length");
		}
		double num9 = Math.Abs(boundingBox4.Min.X) + Math.Abs(boundingBox3.Max.X) + num;
		for (int i = 0; i < integer; i++)
		{
			double num10 = (length3 - (num9 + (double)i * (boundingBox3.Max.X - boundingBox3.Min.X + num))) / length;
			double t4 = 0.0;
			if (num10 > 1.0)
			{
				num10 -= 1.0;
			}
			if (num10 < 0.0)
			{
				num10 += 1.0;
			}
			if (onCurve2.GetNormalizedArcLengthPoint(num10, ref t4))
			{
				On3dVector on3dVector9 = OnUtil.ON_CrossProduct(onCurve2.TangentAt(t4), on3dVector3);
				on3dVector9.Unitize();
				On3dVector on3dVector10 = on3dVector9 * num4 + on3dVector2 * (1.0 - num4);
				if (on3dVector10.Length() < 0.001)
				{
					on3dVector10 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					on3dVector10.Unitize();
				}
				OnXform xform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, gem.North.Convert(), gem.Up.Convert(), onCurve2.PointAt(t4), on3dVector3, on3dVector10);
				CSData data3 = gem.Duplicate();
				CSDataHelper.Transform(data3, xform5.Convert() * onXform4.Convert() * xform2.Convert());
				cSDataArray.Append(data3);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
