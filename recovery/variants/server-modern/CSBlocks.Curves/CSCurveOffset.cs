using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("CurveOffset", "{A838AA68-6518-4843-AF94-5758B5AD09CB}")]
public sealed class CSCurveOffset : CSBlock
{
	public CSInputChannel iCurve { get; }

	public CSInputChannel iOffset { get; }

	public CSInputChannel iZ { get; }

	public CSOutputChannel oCurve { get; }

	public CSCurveOffset(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOffset = new CSInputChannel(GetPropertyName(() => iOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(GetPropertyName(() => iZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
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
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			OnNurbsCurve val = flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val == null)
			{
				return UpdateMissing("Bad Curves");
			}
			IOn3dPoint origin = OnUtil.On_origin;
			IOn3dVector normalDir = OnUtil.On_zaxis;
			OnPlane val2 = new OnPlane();
			if (((OnCurve)val).IsPlanar(val2))
			{
				OnPlane val3 = new OnPlane(OnUtil.On_origin, OnUtil.On_zaxis);
				OnXform val4 = new OnXform();
				val4.ChangeBasis((IOnPlane)(object)val3, (IOnPlane)(object)val2);
				OnXform val5 = new OnXform();
				val5 = val4.Inverse();
				OnNurbsCurve obj = val.Duplicate();
				obj.Transform((IOnXform)(object)val4);
				OnBoundingBox val6 = new OnBoundingBox();
				((OnGeometry)obj).GetBoundingBox(ref val6);
				On3dPoint obj2 = val6.Center();
				obj2.Transform((IOnXform)(object)val5);
				origin = (IOn3dPoint)(object)obj2;
				normalDir = val2.Normal();
				value = 0.0 - value;
			}
			OnCurve val7 = CSUtilGeometry.OffsetOld((IOnCurve)(object)val, normalDir, origin, value, closed: true);
			if (val7 == null)
			{
				return UpdateError("Unable to offset");
			}
			OnNurbsCurve val8 = new OnNurbsCurve();
			val7.GetNurbForm(ref val8);
			if (value2 != 0.0)
			{
				OnXform val9 = new OnXform();
				val9.Translation(0.0, 0.0, value2);
				val8.Transform((IOnXform)(object)val9);
				cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val8.Convert(), val9.Convert(), applyTransform: false));
			}
			else
			{
				cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val8.Convert()));
			}
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
