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

namespace CSBlocks.Gems;

[CSBlock("GemPlacerSide", "{3D234E2A-CEC6-49A0-B49E-69317913DA52}")]
public sealed class CSGemPlacerSide : CSBlock
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

	public readonly CSInputChannel InGemOnOff;

	public readonly CSOutputChannel OutGem;

	public readonly CSOutputChannel OutDisplayGem;

	public CSGemPlacerSide(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
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
		InGemOnOff = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutDisplayGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		OnCurve onCurve = InCurve.NurbsCurve.Convert();
		List<CSGemData> gemList = InGem.GemList;
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
		CSDataHelper.GetTransformNoArray(InCurve.Data, out var value);
		if (!value.IsValid)
		{
			value = Transform.Identity;
		}
		On3dPoint on3dPoint = new On3dPoint(OnUtil.On_origin);
		on3dPoint.Transform(value.Convert());
		On3dVector on3dVector = onCurve.TangentAt(onCurve.Domain().Min());
		On3dVector on3dVector2;
		On3dVector on3dVector3;
		if (InCurve.Data is CSOutsideRingRailData || InCurve.Data is CSRingRailData)
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
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, onXform.Convert() * onXform2.Convert()))
		{
			return UpdateError("Rhs Gem Bounding Box");
		}
		double angle_radians2 = angleDegrees2;
		OnXform onXform3 = new OnXform();
		onXform3.Rotation(angle_radians2, OnUtil.On_zaxis, OnUtil.On_origin);
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox2, doGrow: false, onXform.Convert() * onXform3.Convert()))
		{
			return UpdateError("Lhs Gem Bounding Box");
		}
		for (int i = 0; i < integer; i++)
		{
			double t = 0.0;
			if (!onCurve.GetClosestPoint(cSGemData.Center.Convert(), ref t))
			{
				return UpdateError("Rhs Closest Point");
			}
			On3dPoint on3dPoint2 = onCurve.PointAt(t);
			On3dVector arg = new On3dPoint(cSGemData.Center.Convert()) - on3dPoint2;
			On3dVector on3dVector4 = on3dPoint2 - on3dPoint;
			OnXform onXform4 = CSBlockUtil.CSUtilMath.Orient3Point(cSGemData.Center.Convert(), on3dVector3, on3dVector4, OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis);
			on3dPoint2.Transform(onXform4);
			double num5 = 0.0 - on3dPoint2.y;
			double num6 = 0.0 - on3dPoint2.z;
			double dy = num5 + (boolean ? (0.0 - num3) : num3);
			OnXform onXform5 = new OnXform();
			onXform5.Translation(0.0, dy, 0.0);
			double num7 = num6;
			if (OnUtil.ON_DotProduct(arg, on3dVector4) < 0.0)
			{
				num7 = 0.0 - num7;
			}
			num7 += num2;
			OnCurve onCurve2 = CSUtilGeometry.OffsetOld(onCurve, -on3dVector3, on3dPoint, num7, closed: true);
			if (onCurve2 == null)
			{
				return UpdateError("Offset Curve");
			}
			double length = 0.0;
			if (!onCurve2.GetLength(ref length))
			{
				return UpdateError("Offset Curve Length");
			}
			BoundingBox boundingBox3 = BoundingBox.Unset;
			if (!cSGemData.GetTightBoundingBox(ref boundingBox3, doGrow: false, onXform4.Convert()))
			{
				return UpdateError("Rhs Test Gem Bounding Box");
			}
			double t2 = 0.0;
			if (!onCurve2.GetClosestPoint(cSGemData.Center.Convert(), ref t2))
			{
				return UpdateError("Rhs Closest Point");
			}
			double length2 = 0.0;
			if (!onCurve2.GetLength(ref length2, 0.01, new OnInterval(onCurve2.Domain().Min(), t2)))
			{
				return UpdateError("Rhs Closest Point Length");
			}
			double num8 = (length2 + (Math.Abs(boundingBox3.Max.X) + num + Math.Abs(boundingBox.Min.X))) / length;
			if (num8 > 1.0)
			{
				num8 -= 1.0;
			}
			if (num8 < 0.0)
			{
				num8 += 1.0;
			}
			double t3 = 0.0;
			if (!onCurve2.GetNormalizedArcLengthPoint(num8, ref t3))
			{
				return UpdateError("Rhs GetNormalizedArcLengthPoint");
			}
			On3dPoint on3dPoint3 = onCurve2.PointAt(t3);
			On3dVector on3dVector5 = onCurve2.TangentAt(t3);
			on3dVector5.Unitize();
			On3dVector inputVector = OnUtil.ON_CrossProduct(on3dVector5, on3dVector3);
			On3dPoint destination = on3dPoint3 + on3dVector5 * Math.Abs(boundingBox.Min.X) * -1.0;
			IOn3dPoint on3dPoint4 = new On3dPoint(boundingBox.Min.X, 0.0, 0.0);
			double angle_radians3 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(inputVector, on3dVector2, on3dVector3) * num4;
			OnXform onXform6 = new OnXform();
			onXform6.Rotation(angle_radians3, OnUtil.On_yaxis, on3dPoint4);
			OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(on3dPoint4, OnUtil.On_yaxis, OnUtil.On_zaxis, destination, on3dVector3, on3dVector2);
			CSGemData cSGemData3 = new CSGemData(gem);
			cSGemData3.ApplyTransform(xform.Convert() * onXform5.Convert() * onXform6.Convert() * onXform.Convert() * onXform2.Convert());
			cSDataArray.Append(cSGemData3);
			cSGemData = cSGemData3;
			double t4 = 0.0;
			if (!onCurve.GetClosestPoint(cSGemData2.Center.Convert(), ref t4))
			{
				return UpdateError("Lhs Closest Point");
			}
			On3dPoint on3dPoint5 = onCurve.PointAt(t4);
			On3dVector arg2 = new On3dPoint(cSGemData2.Center.Convert()) - on3dPoint5;
			On3dVector on3dVector6 = on3dPoint5 - on3dPoint;
			OnXform onXform7 = CSBlockUtil.CSUtilMath.Orient3Point(cSGemData2.Center.Convert(), on3dVector3, on3dVector6, OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis);
			on3dPoint5.Transform(onXform7);
			double num9 = 0.0 - on3dPoint5.y;
			double num10 = 0.0 - on3dPoint5.z;
			double dy2 = num9 + num3;
			OnXform onXform8 = new OnXform();
			onXform8.Translation(0.0, dy2, 0.0);
			double num11 = num10;
			if (OnUtil.ON_DotProduct(arg2, on3dVector6) < 0.0)
			{
				num11 = 0.0 - num11;
			}
			num11 += num2;
			OnCurve onCurve3 = CSUtilGeometry.OffsetOld(onCurve, -on3dVector3, on3dPoint, num11, closed: true);
			if (onCurve3 == null)
			{
				return UpdateError("Offset Curve");
			}
			double length3 = 0.0;
			if (!onCurve3.GetLength(ref length3))
			{
				return UpdateError("Offset Curve Length");
			}
			BoundingBox boundingBox4 = BoundingBox.Unset;
			if (!cSGemData2.GetTightBoundingBox(ref boundingBox4, doGrow: false, onXform7.Convert()))
			{
				return UpdateError("Lhs Test Gem Bounding Box");
			}
			double t5 = 0.0;
			if (!onCurve3.GetClosestPoint(cSGemData2.Center.Convert(), ref t5))
			{
				return UpdateError("Lhs Closest Point");
			}
			double length4 = 0.0;
			if (!onCurve3.GetLength(ref length4, 0.01, new OnInterval(onCurve3.Domain().Min(), t5)))
			{
				return UpdateError("Lhs Closest Point Length");
			}
			double num12 = (length4 - (Math.Abs(boundingBox4.Min.X) + num + Math.Abs(boundingBox2.Max.X))) / length3;
			if (num12 > 1.0)
			{
				num12 -= 1.0;
			}
			if (num12 < 0.0)
			{
				num12 += 1.0;
			}
			double t6 = 0.0;
			if (!onCurve3.GetNormalizedArcLengthPoint(num12, ref t6))
			{
				return UpdateError("Lhs GetNormalizedArcLengthPoint");
			}
			On3dPoint on3dPoint6 = onCurve3.PointAt(t6);
			On3dVector on3dVector7 = onCurve3.TangentAt(t6);
			on3dVector7.Unitize();
			On3dVector inputVector2 = OnUtil.ON_CrossProduct(on3dVector7, on3dVector3);
			On3dPoint destination2 = on3dPoint6 + on3dVector7 * Math.Abs(boundingBox2.Max.X);
			IOn3dPoint on3dPoint7 = new On3dPoint(boundingBox2.Max.X, 0.0, 0.0);
			double angle_radians4 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(inputVector2, on3dVector2, on3dVector3) * num4;
			OnXform onXform9 = new OnXform();
			onXform9.Rotation(angle_radians4, OnUtil.On_yaxis, on3dPoint7);
			OnXform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(on3dPoint7, OnUtil.On_yaxis, OnUtil.On_zaxis, destination2, on3dVector3, on3dVector2);
			CSGemData cSGemData4 = new CSGemData(gem);
			cSGemData4.ApplyTransform(xform2.Convert() * onXform8.Convert() * onXform9.Convert() * onXform.Convert() * onXform3.Convert());
			cSDataArray.Append(cSGemData4);
			cSGemData2 = cSGemData4;
		}
		OutGem.SetData(cSDataArray);
		InGemOnOff.GetValue(out bool value2);
		if (value2)
		{
			OutDisplayGem.SetData(cSDataArray);
		}
		return base.Update();
	}
}
