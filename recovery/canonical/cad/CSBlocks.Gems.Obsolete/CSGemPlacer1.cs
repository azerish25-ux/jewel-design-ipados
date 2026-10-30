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

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemPlacer_V1", "{5B2B4460-789C-4139-8F87-9CC33AD10BBA}")]
public sealed class CSGemPlacer1 : CSBlock
{
	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemCount;

	public readonly CSInputChannel InGemSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemYOffsetMirror;

	public readonly CSInputChannel InGemTilt;

	public readonly CSInputChannel InGemAngle;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemRotationMirror;

	public readonly CSInputChannel InGemPosition;

	public readonly CSOutputChannel OutGem;

	public CSInputChannel iCurve;

	public CSGemPlacer1(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffsetMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotationMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray == null)
		{
			return UpdateMissing("Missing Curve Input");
		}
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemCount.Integer;
		double num = InGemSpacing.Double;
		double distance = InGemZOffset.Double;
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
			return UpdateError("Bounding Box Failed");
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
			return UpdateError("Bounding Box Failed");
		}
		OnXform onXform6 = new OnXform();
		onXform6.Translation(0.0, num2, 0.0);
		OnXform onXform7 = new OnXform();
		onXform7.Rotation(angleDegrees2, OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform xform3 = onXform * onXform7;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, xform3.Convert()))
		{
			return UpdateError("Bounding Box Failed");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem == null)
			{
				return UpdateError("Missing curve Data");
			}
			OnCurve onCurve = arrayItem.GetNurbsCurve().Convert();
			CSDataHelper.GetTransformNoArray(arrayItem, out var value);
			On3dVector on3dVector;
			On3dVector on3dVector2;
			if (!value.IsValid || arrayItem is CSOutsideRingRailData || arrayItem is CSRingRailData)
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
			OnCurve onCurve2 = CSUtilGeometry.OriginOffset(onCurve, -on3dVector2, distance, closed: true);
			double length = 0.0;
			if (!onCurve2.GetLength(ref length))
			{
				return UpdateError("offset failed");
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
				if (!onCurve2.GetNormalizedArcLengthPoint(num9, ref t))
				{
					return UpdateError("Arc length failed");
				}
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
				CSData data = gem.Duplicate();
				CSDataHelper.Transform(data, xform4.Convert() * onXform4.Convert() * xform2.Convert());
				cSDataArray.Append(data);
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
				if (!onCurve2.GetNormalizedArcLengthPoint(num10, ref t2))
				{
					return UpdateError("Arc length failed");
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
				CSData data2 = gem.Duplicate();
				CSDataHelper.Transform(data2, xform5.Convert() * onXform2.Convert() * xform.Convert());
				cSDataArray.Append(data2);
			}
			double num11 = 0.0;
			num11 = ((num5 > 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num) : (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox3.Min.X) + num / 2.0) : (Math.Abs(boundingBox3.Max.X) + num / 2.0)));
			for (int j = 0; j < num6; j++)
			{
				double num12 = num4 + (0.0 - (num11 + (double)j * (boundingBox3.Max.X - boundingBox3.Min.X + num)) / length);
				double t3 = 0.0;
				if (num12 > 1.0)
				{
					num12 -= 1.0;
				}
				if (num12 < 0.0)
				{
					num12 += 1.0;
				}
				if (!onCurve2.GetNormalizedArcLengthPoint(num12, ref t3))
				{
					return UpdateError("Arc Length Failed");
				}
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
				CSData data3 = gem.Duplicate();
				CSDataHelper.Transform(data3, xform6.Convert() * onXform6.Convert() * xform3.Convert());
				cSDataArray.Append(data3);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
