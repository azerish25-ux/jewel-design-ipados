using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemOnProfile_V1", "{EDDBC082-E81F-4bf6-A971-46C6E75E5770}")]
public sealed class CSGemOnProfile1 : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemCount;

	public readonly CSInputChannel InGemSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemYOffsetMirror;

	public readonly CSInputChannel InGemAngle;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemRotationMirror;

	public readonly CSInputChannel InGemPosition;

	public readonly CSOutputChannel OutGem;

	public CSGemOnProfile1(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffsetMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotationMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		List<Curve> curveList = InCurve.CurveList;
		if (curveList == null)
		{
			return UpdateMissing("Curve Input");
		}
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemCount.Integer;
		double num = InGemSpacing.Double;
		double dz = InGemZOffset.Double;
		double num2 = InGemYOffset.Double;
		double angleDegrees = InGemAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double angleDegrees2 = InGemRotation.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		double num3 = InGemPosition.Double;
		bool boolean = InGemYOffsetMirror.Boolean;
		bool boolean2 = InGemRotationMirror.Boolean;
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(angleDegrees, Vector3d.XAxis, Point3d.Origin);
		Transform transform2 = default(Transform);
		transform2 = ((!boolean) ? Transform.Translation(0.0, num2, dz) : Transform.Translation(0.0, 0.0, dz));
		Transform transform3 = default(Transform);
		transform3 = ((!boolean2) ? CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(0.0, Vector3d.ZAxis, Point3d.Origin));
		Transform transform4 = transform * transform3;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, transform4))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform transform5 = default(Transform);
		transform5 = ((!boolean) ? Transform.Translation(0.0, num2, dz) : Transform.Translation(0.0, 0.0 - num2, dz));
		Transform transform6 = default(Transform);
		transform6 = ((!boolean2) ? CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(0.0 - angleDegrees2, Vector3d.ZAxis, Point3d.Origin));
		Transform transform7 = transform * transform6;
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox2, doGrow: false, transform7))
		{
			return UpdateError("Bounding Box Failed");
		}
		Transform transform8 = Transform.Translation(0.0, num2, dz);
		Transform transform9 = CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.ZAxis, Point3d.Origin);
		Transform transform10 = transform * transform9;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, transform10))
		{
			return UpdateError("Bounding Box Failed");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < curveList.Count; i++)
		{
			NurbsCurve nurbsCurve = curveList[i].ToNurbsCurve();
			Point3d point3d = nurbsCurve.PointAt(nurbsCurve.Domain.Min);
			Point3d point3d2 = nurbsCurve.PointAt(nurbsCurve.Domain.Mid);
			Vector3d vector3d = Vector3d.CrossProduct(nurbsCurve.TangentAt(nurbsCurve.Domain.Min), point3d2 - point3d);
			double length = nurbsCurve.GetLength();
			if (double.IsNaN(length) || length < 0.0)
			{
				return UpdateError("Curve Length Failed");
			}
			num3 -= (Math.Abs(boundingBox.Max.X) - Math.Abs(boundingBox.Min.X)) / (length * 2.0);
			if (integer > 0 && integer % 2 != 0)
			{
				double num4 = num3;
				if (num4 > 1.0)
				{
					num4 -= 1.0;
				}
				if (!nurbsCurve.NormalizedLengthParameter(num4, out var t))
				{
					return UpdateError("Arc Length Failed");
				}
				Vector3d destination = Vector3d.CrossProduct(nurbsCurve.TangentAt(t), vector3d);
				Transform transform11 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, nurbsCurve.PointAt(t), vector3d, destination);
				CSData data = gem.Duplicate();
				CSDataHelper.Transform(data, transform11 * transform2 * transform4);
				cSDataArray.Append(data);
			}
			int num5 = integer / 2;
			for (int j = 0; j < num5; j++)
			{
				double num6 = 0.0;
				num6 = ((integer % 2 != 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + num) : (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox2.Min.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox2.Max.X) + num / 2.0) : (Math.Abs(boundingBox2.Min.X) + num / 2.0)));
				double num7 = num3 + (num6 + (double)j * (boundingBox2.Max.X - boundingBox2.Min.X + num)) / length;
				double t2 = 0.0;
				if (num7 > 1.0)
				{
					num7 -= 1.0;
				}
				if (!nurbsCurve.NormalizedLengthParameter(num7, out t2))
				{
					return UpdateError("Arc Length Failed");
				}
				Vector3d destination2 = Vector3d.CrossProduct(nurbsCurve.TangentAt(t2), vector3d);
				Transform transform12 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, nurbsCurve.PointAt(t2), vector3d, destination2);
				CSData data2 = gem.Duplicate();
				CSDataHelper.Transform(data2, transform12 * transform5 * transform7);
				cSDataArray.Append(data2);
			}
			int num8 = integer / 2;
			for (int k = 0; k < num8; k++)
			{
				double num9 = 0.0;
				num9 = ((integer % 2 != 0) ? ((!boolean2) ? (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num) : (Math.Abs(boundingBox.Min.X) + Math.Abs(boundingBox3.Max.X) + num)) : ((!boolean2) ? (Math.Abs(boundingBox3.Min.X) + num / 2.0) : (Math.Abs(boundingBox3.Max.X) + num / 2.0)));
				double num10 = num3 + (0.0 - (num9 + (double)k * (boundingBox3.Max.X - boundingBox3.Min.X + num)) / length);
				double t3 = 0.0;
				if (num10 < 0.0)
				{
					num10 += 1.0;
				}
				if (!nurbsCurve.NormalizedLengthParameter(num10, out t3))
				{
					return UpdateError("Arc Length Failed");
				}
				Vector3d destination3 = Vector3d.CrossProduct(nurbsCurve.TangentAt(t3), vector3d);
				Transform transform13 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, nurbsCurve.PointAt(t3), vector3d, destination3);
				CSData data3 = gem.Duplicate();
				CSDataHelper.Transform(data3, transform13 * transform8 * transform10);
				cSDataArray.Append(data3);
			}
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
