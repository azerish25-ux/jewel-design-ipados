using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("CurveOffsetWithOrigin", "{74EC9B54-5D96-48A7-86C9-E13DCCA60A4B}")]
public sealed class CSCurveOffsetWithOrigin : CSBlock
{
	public readonly CSInputChannel iCurve;

	public readonly CSInputChannel iOffset;

	public readonly CSInputChannel iOrigin;

	public readonly CSInputChannel iZ;

	public readonly CSOutputChannel oCurve;

	public CSCurveOffsetWithOrigin(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOrigin = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray == null)
		{
			return UpdateMissing("Curves");
		}
		double value = 0.0;
		iOffset.GetDouble(ref value);
		CSDataArray cSDataArray = new CSDataArray();
		double value2 = 0.0;
		if (iZ != null)
		{
			iZ.GetDouble(ref value2);
		}
		CSDataArray flattenArray2 = iOrigin.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Origin");
		}
		if (!CSDataHelper.GetPoint(flattenArray2.GetArrayItem(0), out var value3))
		{
			return UpdateMissing("Origin");
		}
		Plane val = default(Plane);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetCurve(flattenArray.GetArrayItem(i), out var value4);
			if (value4 == null)
			{
				return UpdateMissing("Bad Curves");
			}
			Vector3d normalDir = Vector3d.ZAxis;
			if (value4.TryGetPlane(ref val))
			{
				normalDir = ((Plane)(ref val)).Normal;
				value = 0.0 - value;
			}
			Curve val2 = CSUtilGeometry.Offset(value4, normalDir, value3, value, (CurveOffsetCornerStyle)1);
			if (val2 == null)
			{
				return UpdateError("Unable to offset");
			}
			if (value2 != 0.0)
			{
				Transform val3 = Transform.Translation(0.0, 0.0, value2);
				((GeometryBase)val2).Transform(val3);
				cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val2, val3, applyTransform: false));
			}
			else
			{
				cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val2));
			}
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
