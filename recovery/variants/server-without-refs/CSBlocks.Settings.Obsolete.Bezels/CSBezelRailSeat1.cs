using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Settings.Obsolete.Bezels;

[Obsolete]
[CSBlock("BezelRailSeat_V1", "{9DAF23C3-6347-4674-A7F4-328BD7126363}")]
public sealed class CSBezelRailSeat1 : CSBlock
{
	public CSInputChannel iCurve { get; }

	public CSInputChannel iSeatAngle { get; }

	public CSInputChannel iSeatLength { get; }

	public CSOutputChannel oCurve { get; }

	public CSBezelRailSeat1(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iSeatAngle = new CSInputChannel(GetPropertyName(() => iSeatAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSeatLength = new CSInputChannel(GetPropertyName(() => iSeatLength), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		double value = 0.0;
		iSeatAngle.GetDouble(ref value);
		double value2 = 0.0;
		iSeatLength.GetDouble(ref value2);
		CSDataArray cSDataArray = new CSDataArray();
		Transform transform = default(Transform);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value3);
			if (!((Transform)(ref value3)).IsValid)
			{
				value3 = Transform.Identity;
			}
			((Transform)(ref value3)).TryGetInverse(ref transform);
			OnNurbsCurve val = arrayItem.GetNurbsCurve().Convert();
			if (val == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			OnNurbsCurve obj = val.Duplicate();
			obj.Transform((IOnXform)(object)transform.Convert());
			double num = CSBlockUtil.CSUtilMath.ToRadians(value);
			double distance = (0.0 - Math.Sin(num)) * value2;
			double num2 = (0.0 - Math.Cos(num)) * value2;
			OnXform val2 = new OnXform();
			val2.Translation(0.0, 0.0, num2);
			OnCurve val3 = CSUtilGeometry.OriginOffsetXY((IOnCurve)(object)obj, distance, closed: true);
			if (val3 == null)
			{
				CS.CSUtil.PrintLine("Bezel Rail Seat Offset Curve Unable to Create Output");
				cSDataArray.Append(new CSData());
			}
			else
			{
				((OnGeometry)val3).Transform((IOnXform)(object)val2);
				cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val3.Convert(), value3));
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oCurve.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oCurve.SetData(cSDataArray);
		}
		return base.Update();
	}
}
