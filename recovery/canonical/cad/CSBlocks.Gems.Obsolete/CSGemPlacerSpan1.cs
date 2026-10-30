using System;
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
using Rhino.Geometry;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemPlacerSpan_V1", "{5807DFC4-8F07-441c-A793-48229FCC20D4}")]
public sealed class CSGemPlacerSpan1 : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemSpan;

	public readonly CSInputChannel InGemSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemYOffsetMirror;

	public readonly CSInputChannel InGemTilt;

	public readonly CSInputChannel InGemAngle;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemRotationMirror;

	public readonly CSInputChannel InGemPosition;

	public readonly CSInputChannel InGemPlacementMethod;

	public readonly CSOutputChannel OutGem;

	public CSGemPlacerSpan1(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemSpan = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffsetMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotationMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
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
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemPlacementMethod.Integer;
		double angleDegrees = InGemSpan.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double num = InGemSpacing.Double;
		double zOffset = InGemZOffset.Double;
		double num2 = InGemYOffset.Double;
		double num3 = InGemTilt.Double;
		num3 /= 100.0;
		double angleDegrees2 = InGemAngle.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		double angleDegrees3 = InGemRotation.Double;
		angleDegrees3 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees3);
		double num4 = InGemPosition.Double;
		bool boolean = InGemYOffsetMirror.Boolean;
		bool boolean2 = InGemRotationMirror.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataHelper.GetTransformNoArray(data, out var value);
		On3dVector on3dVector;
		On3dVector on3dVector2;
		if (!value.IsValid || data is CSOutsideRingRailData || data is CSRingRailData)
		{
			on3dVector = new On3dVector(OnUtil.On_zaxis);
			on3dVector2 = OnUtil.ON_CrossProduct(on3dVector, onCurve.TangentAt(onCurve.Domain().Min()));
			on3dVector2.Unitize();
		}
		else
		{
			on3dVector = new On3dVector(OnUtil.On_yaxis);
			on3dVector.Transform(value.Convert());
			on3dVector.Unitize();
			on3dVector2 = OnUtil.ON_CrossProduct(onCurve.TangentAt(onCurve.Domain().Min()), on3dVector);
			on3dVector2.Unitize();
		}
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer, ref zOffset);
		OnCurve onCurve2 = CSUtilGeometry.OriginOffset(onCurve, -on3dVector2, zOffset, closed: true);
		if (onCurve2 == null)
		{
			return UpdateError("Offset Curve");
		}
		double length = 0.0;
		if (!onCurve2.GetLength(ref length))
		{
			return UpdateError("Offset Curve Length");
		}
		OnXform onXform = new OnXform();
		onXform.Rotation(angleDegrees2, OnUtil.On_xaxis, OnUtil.On_origin);
		OnXform onXform2 = new OnXform();
		if (boolean)
		{
			onXform2.Translation(0.0, 0.0, 0.0);
		}
		else
		{
			onXform2.Translation(0.0, num2, 0.0);
		}
		OnXform onXform3 = new OnXform();
		if (boolean2)
		{
			onXform3.Rotation(0.0, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		else
		{
			onXform3.Rotation(angleDegrees3, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		OnXform xform = onXform * onXform3;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, xform.Convert()))
		{
			return UpdateError("Center Gem Bounding Box");
		}
		OnXform onXform4 = new OnXform();
		if (boolean)
		{
			onXform4.Translation(0.0, 0.0 - num2, 0.0);
		}
		else
		{
			onXform4.Translation(0.0, num2, 0.0);
		}
		OnXform onXform5 = new OnXform();
		if (boolean2)
		{
			onXform5.Rotation(0.0 - angleDegrees3, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		else
		{
			onXform5.Rotation(angleDegrees3, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		OnXform xform2 = onXform * onXform5;
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox2, doGrow: false, xform2.Convert()))
		{
			return UpdateError("RHS Gem Bounding Box");
		}
		OnXform onXform6 = new OnXform();
		onXform6.Translation(0.0, num2, 0.0);
		OnXform onXform7 = new OnXform();
		onXform7.Rotation(angleDegrees3, OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform xform3 = onXform * onXform7;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, xform3.Convert()))
		{
			return UpdateError("LHS Gem Bounding Box");
		}
		OnLineCurve onLineCurve = new OnLineCurve(OnUtil.On_origin, new On3dPoint(0.0, 0.0, length));
		onLineCurve.Rotate(angleDegrees / 2.0, OnUtil.On_yaxis, OnUtil.On_origin);
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		onCurve2.IntersectCurve(onLineCurve, ref x, CSApplicationHelper.Application.Tolerance);
		if (x.Count() < 1)
		{
			return UpdateError("Intersection");
		}
		double length2 = 0.0;
		if (!onCurve2.GetLength(ref length2, CSApplicationHelper.Application.Tolerance, new OnInterval(onCurve2.Domain().Min(), x[0].get_m_a(0))))
		{
			return UpdateError("Intersection Length");
		}
		int num5 = (int)(length2 / (Math.Abs(boundingBox2.Max.X) + Math.Abs(boundingBox2.Min.X) + num));
		double num6 = length2 % (Math.Abs(boundingBox2.Max.X) + Math.Abs(boundingBox2.Min.X) + num);
		int num7 = (int)(length2 / (Math.Abs(boundingBox3.Max.X) + Math.Abs(boundingBox3.Min.X) + num));
		double num8 = length2 % (Math.Abs(boundingBox3.Max.X) + Math.Abs(boundingBox3.Min.X) + num);
		int num9 = (int)((num6 + num8) / (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox.Min.X)));
		int num10 = (num5 + num7) / 2;
		num4 = ((num9 <= 0) ? (num4 - (Math.Abs(boundingBox.Max.X) - Math.Abs(boundingBox.Min.X)) / length) : (num4 - (Math.Abs(boundingBox.Max.X) - Math.Abs(boundingBox.Min.X)) / length / 2.0));
		double num11 = 0.0;
		num11 = ((num9 > 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + num) : (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox2.Max.X) + num / 2.0) : (Math.Abs(boundingBox2.Min.X) + num / 2.0)));
		for (int num12 = num10 - 1; num12 >= 0; num12--)
		{
			double num13 = num4 + (num11 + (double)num12 * (boundingBox2.Max.X - boundingBox2.Min.X + num)) / length;
			if (num13 > 1.0)
			{
				num13 -= 1.0;
			}
			if (num13 < 0.0)
			{
				num13 += 1.0;
			}
			double t = 0.0;
			if (onCurve2.GetNormalizedArcLengthPoint(num13, ref t))
			{
				On3dVector on3dVector3 = OnUtil.ON_CrossProduct(onCurve2.TangentAt(t), on3dVector2);
				on3dVector3.Unitize();
				On3dVector on3dVector4 = on3dVector3 * num3 + on3dVector * (1.0 - num3);
				if (on3dVector4.Length() < 0.001)
				{
					on3dVector4 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					on3dVector4.Unitize();
				}
				OnXform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, gem.North.Convert(), gem.Up.Convert(), onCurve2.PointAt(t), on3dVector2, on3dVector4);
				CSData data2 = gem.Duplicate();
				CSDataHelper.Transform(data2, xform4.Convert() * onXform4.Convert() * xform2.Convert());
				cSDataArray.Append(data2);
			}
		}
		if (num9 > 0)
		{
			double num14 = num4;
			if (num14 > 1.0)
			{
				num14 -= 1.0;
			}
			if (num14 < 0.0)
			{
				num14 += 1.0;
			}
			double t2 = 0.0;
			if (!onCurve2.GetNormalizedArcLengthPoint(num14, ref t2))
			{
				return UpdateError("Normalized Arc Length Point");
			}
			On3dVector on3dVector5 = OnUtil.ON_CrossProduct(onCurve2.TangentAt(t2), on3dVector2);
			on3dVector5.Unitize();
			On3dVector on3dVector6 = on3dVector5 * num3 + on3dVector * (1.0 - num3);
			if (on3dVector6.Length() < 0.001)
			{
				on3dVector6 = new On3dVector(OnUtil.On_zaxis);
			}
			else
			{
				on3dVector6.Unitize();
			}
			OnXform xform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, gem.North.Convert(), gem.Up.Convert(), onCurve2.PointAt(t2), on3dVector2, on3dVector6);
			CSData data3 = gem.Duplicate();
			CSDataHelper.Transform(data3, xform5.Convert() * onXform2.Convert() * xform.Convert());
			cSDataArray.Append(data3);
		}
		double num15 = 0.0;
		num15 = ((num9 > 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num) : (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox3.Min.X) + num / 2.0) : (Math.Abs(boundingBox3.Max.X) + num / 2.0)));
		for (int i = 0; i < num10; i++)
		{
			double num16 = num4 + (0.0 - (num15 + (double)i * (boundingBox3.Max.X - boundingBox3.Min.X + num)) / length);
			if (num16 > 1.0)
			{
				num16 -= 1.0;
			}
			if (num16 < 0.0)
			{
				num16 += 1.0;
			}
			double t3 = 0.0;
			if (onCurve2.GetNormalizedArcLengthPoint(num16, ref t3))
			{
				On3dVector on3dVector7 = OnUtil.ON_CrossProduct(onCurve2.TangentAt(t3), on3dVector2);
				on3dVector7.Unitize();
				On3dVector on3dVector8 = on3dVector7 * num3 + on3dVector * (1.0 - num3);
				if (on3dVector8.Length() < 0.001)
				{
					on3dVector8 = new On3dVector(OnUtil.On_zaxis);
				}
				else
				{
					on3dVector8.Unitize();
				}
				OnXform xform6 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, gem.North.Convert(), gem.Up.Convert(), onCurve2.PointAt(t3), on3dVector2, on3dVector8);
				CSData data4 = gem.Duplicate();
				CSDataHelper.Transform(data4, xform6.Convert() * onXform6.Convert() * xform3.Convert());
				cSDataArray.Append(data4);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
