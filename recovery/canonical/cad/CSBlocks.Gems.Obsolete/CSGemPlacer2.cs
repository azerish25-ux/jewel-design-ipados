using System;
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
[CSBlock("GemPlacer_V2", "{71A62BA3-7474-43f6-B69E-C33F76E240DC}")]
public sealed class CSGemPlacer2 : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemCount;

	public readonly CSInputChannel IGemSpacing;

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

	public CSGemPlacer2(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		IGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
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
		OnNurbsCurve onNurbsCurve = InCurve.NurbsCurve.Convert();
		if (onNurbsCurve == null)
		{
			return UpdateMissing("Curve");
		}
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemCount.Integer;
		int integer2 = InGemPlacementMethod.Integer;
		double num = IGemSpacing.Double;
		double zOffset = InGemZOffset.Double;
		double num2 = InGemYOffset.Double;
		double num3 = InGemTilt.Double;
		num3 /= 100.0;
		double angleDegrees = InGemAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double angleDegrees2 = InGemRotation.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
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
			on3dVector2 = OnUtil.ON_CrossProduct(on3dVector, onNurbsCurve.TangentAt(onNurbsCurve.Domain().Min()));
			on3dVector2.Unitize();
		}
		else
		{
			on3dVector = new On3dVector(OnUtil.On_yaxis);
			on3dVector.Transform(value.Convert());
			on3dVector.Unitize();
			on3dVector2 = OnUtil.ON_CrossProduct(onNurbsCurve.TangentAt(onNurbsCurve.Domain().Min()), on3dVector);
			on3dVector2.Unitize();
		}
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer2, ref zOffset);
		OnCurve onCurve = CSUtilGeometry.OriginOffset(onNurbsCurve, -on3dVector2, zOffset, closed: true);
		if (onCurve == null)
		{
			return UpdateError("Offset Curve");
		}
		double length = 0.0;
		if (!onCurve.GetLength(ref length))
		{
			return UpdateError("Offset Curve Length");
		}
		OnXform onXform = new OnXform();
		onXform.Rotation(angleDegrees, OnUtil.On_xaxis, OnUtil.On_origin);
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
			onXform3.Rotation(angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
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
			onXform5.Rotation(0.0 - angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
		}
		else
		{
			onXform5.Rotation(angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
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
		onXform7.Rotation(angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform xform3 = onXform * onXform7;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, xform3.Convert()))
		{
			return UpdateError("LHS Gem Bounding Box");
		}
		int num5 = integer % 2;
		int num6 = integer / 2;
		num4 = ((num5 <= 0) ? (num4 - (Math.Abs(boundingBox.Max.X) - Math.Abs(boundingBox.Min.X)) / length) : (num4 - (Math.Abs(boundingBox.Max.X) - Math.Abs(boundingBox.Min.X)) / length / 2.0));
		double num7 = 0.0;
		num7 = ((num5 > 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + num) : (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox2.Max.X) + num / 2.0) : (Math.Abs(boundingBox2.Min.X) + num / 2.0)));
		for (int num8 = num6 - 1; num8 >= 0; num8--)
		{
			double num9 = num4 + (num7 + (double)num8 * (boundingBox2.Max.X - boundingBox2.Min.X + num)) / length;
			double t = 0.0;
			if (num9 > 1.0)
			{
				num9 -= 1.0;
			}
			if (num9 < 0.0)
			{
				num9 += 1.0;
			}
			if (onCurve.GetNormalizedArcLengthPoint(num9, ref t))
			{
				On3dVector on3dVector3 = OnUtil.ON_CrossProduct(onCurve.TangentAt(t), on3dVector2);
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
				OnXform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, gem.North.Convert(), gem.Up.Convert(), onCurve.PointAt(t), on3dVector2, on3dVector4);
				CSData data2 = gem.Duplicate();
				CSDataHelper.Transform(data2, xform4.Convert() * onXform4.Convert() * xform2.Convert());
				cSDataArray.Append(data2);
			}
		}
		if (num5 > 0)
		{
			double num10 = num4;
			double t2 = 0.0;
			if (num10 > 1.0)
			{
				num10 -= 1.0;
			}
			if (num10 < 0.0)
			{
				num10 += 1.0;
			}
			if (!onCurve.GetNormalizedArcLengthPoint(num10, ref t2))
			{
				return UpdateError("Normalized Arc Length Point");
			}
			On3dVector on3dVector5 = OnUtil.ON_CrossProduct(onCurve.TangentAt(t2), on3dVector2);
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
			OnXform xform5 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, gem.North.Convert(), gem.Up.Convert(), onCurve.PointAt(t2), on3dVector2, on3dVector6);
			CSData data3 = gem.Duplicate();
			CSDataHelper.Transform(data3, xform5.Convert() * onXform2.Convert() * xform.Convert());
			cSDataArray.Append(data3);
		}
		double num11 = 0.0;
		num11 = ((num5 > 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num) : (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox3.Min.X) + num / 2.0) : (Math.Abs(boundingBox3.Max.X) + num / 2.0)));
		for (int i = 0; i < num6; i++)
		{
			double num12 = num4 + (0.0 - (num11 + (double)i * (boundingBox3.Max.X - boundingBox3.Min.X + num)) / length);
			double t3 = 0.0;
			if (num12 > 1.0)
			{
				num12 -= 1.0;
			}
			if (num12 < 0.0)
			{
				num12 += 1.0;
			}
			if (onCurve.GetNormalizedArcLengthPoint(num12, ref t3))
			{
				On3dVector on3dVector7 = OnUtil.ON_CrossProduct(onCurve.TangentAt(t3), on3dVector2);
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
				OnXform xform6 = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, gem.North.Convert(), gem.Up.Convert(), onCurve.PointAt(t3), on3dVector2, on3dVector8);
				CSData data4 = gem.Duplicate();
				CSDataHelper.Transform(data4, xform6.Convert() * onXform6.Convert() * xform3.Convert());
				cSDataArray.Append(data4);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
