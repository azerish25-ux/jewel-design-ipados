using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Profiles;

[CSBlock("ProfilePlacerNextToGem", "{9F12CB8B-1E05-499a-ADC5-A3174585B071}")]
public sealed class CSProfilePlacerNextToGem : CSBlock
{
	public readonly CSInputChannel iGem;

	public readonly CSInputChannel iProfile;

	public readonly CSInputChannel iPosition;

	public readonly CSInputChannel iScaleX;

	public readonly CSInputChannel iScaleY;

	public readonly CSInputChannel iOffsetScaledY;

	public readonly CSInputChannel iOffsetScaledZ;

	public readonly CSOutputChannel oProfile;

	public CSProfilePlacerNextToGem(string identifier)
		: base(identifier)
	{
		iGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iScaleX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iScaleY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOffsetScaledY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOffsetScaledZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iGem.GetFlattenArray();
		CSData data = iProfile.Data;
		if (data == null)
		{
			return UpdateMissing("Missing profile data");
		}
		CSDataHelper.GetNurbsCurve(data, out var value);
		if (value == null)
		{
			return UpdateMissing("Missing profile curve");
		}
		double value2 = 0.0;
		iPosition.GetDouble(ref value2);
		double value3 = 100.0;
		iScaleX.GetDouble(ref value3);
		value3 /= 100.0;
		double value4 = 100.0;
		iScaleY.GetDouble(ref value4);
		value4 /= 100.0;
		double value5 = 0.0;
		iOffsetScaledY.GetDouble(ref value5);
		double value6 = 0.0;
		iOffsetScaledZ.GetDouble(ref value6);
		CSDataArray cSDataArray = new CSDataArray();
		BoundingBox boundingBox = value.GetBoundingBox(accurate: true);
		if (!boundingBox.IsValid)
		{
			return UpdateError("GetTightBoundingBox failed");
		}
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (flattenArray.GetArrayItem(i) is CSGemData cSGemData)
			{
				double xScaleFactor = cSGemData.GemXSize * value3 / (Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox.Min.X));
				double yScaleFactor = cSGemData.GemZSize * value4 / (Math.Abs(boundingBox.Max.Y) - Math.Abs(boundingBox.Min.Y));
				Transform transform = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, 1.0);
				double dy = cSGemData.GemZSize * value5;
				double dz = cSGemData.GemYSize * value6;
				Transform transform2 = Transform.Translation(0.0, dy, dz);
				CSDataHelper.GetNurbsCurve(cSGemData, out var value7);
				double t = 0.0;
				if (!value7.NormalizedLengthParameter(value2, out t))
				{
					return UpdateError("GetNormalizedArcLengthPoint failed");
				}
				Point3d unset = Point3d.Unset;
				Vector3d unset2 = Vector3d.Unset;
				unset2 = value7.TangentAt(t);
				unset = value7.PointAt(t);
				if (unset == Point3d.Unset || unset2 == Vector3d.Unset)
				{
					return UpdateError("Ev1Der failed");
				}
				Vector3d destination = Vector3d.CrossProduct(cSGemData.Up, unset2);
				Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.YAxis, Vector3d.ZAxis, unset, cSGemData.Up, destination);
				CSData data2 = data.Duplicate();
				CSDataHelper.Transform(data2, transform3 * transform2 * transform);
				cSDataArray.Append(data2);
			}
		}
		oProfile.SetData(cSDataArray);
		return base.Update();
	}
}
