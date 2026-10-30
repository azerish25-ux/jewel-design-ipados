using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.SMI;

[CSBlock("SMIPosaluxCuttingDepth", "{41008634-A8C9-487d-B17A-F12E6B8AA8FF}")]
public sealed class CSSMIPosaluxCuttingDepth : CSBlock
{
	public CSInputChannel iBand { get; }

	public CSInputChannel iCutter { get; }

	public CSInputChannel iRadius { get; }

	public CSInputChannel iCuttingDepth { get; }

	public CSOutputChannel oObject { get; }

	public CSSMIPosaluxCuttingDepth(string identifier)
		: base(identifier)
	{
		iBand = new CSInputChannel(GetPropertyName(() => iBand), this, ChannelDataType.Any, ChannelArrayType.Any);
		iCutter = new CSInputChannel(GetPropertyName(() => iCutter), this, ChannelDataType.Any, ChannelArrayType.Any);
		iRadius = new CSInputChannel(GetPropertyName(() => iRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCuttingDepth = new CSInputChannel(GetPropertyName(() => iCuttingDepth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(GetPropertyName(() => oObject), this, ChannelDataType.Any, ChannelArrayType.Any);
	}

	public override bool Update()
	{
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		//IL_0178: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		CSData data = iBand.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Band Data");
		}
		OnBrep val = data.GetBrep().Convert();
		if (val == null)
		{
			return UpdateMissing("Missing Band");
		}
		CSData data2 = iCutter.Data;
		if (data2 == null)
		{
			return UpdateMissing("Missing Cutter Data");
		}
		double value = 76.2;
		iRadius.GetDouble(ref value);
		double value2 = 0.0;
		iCuttingDepth.GetDouble(ref value2);
		CSDataHelper.GetTransformNoArray(data2, out var value3);
		if (!((Transform)(ref value3)).IsValid)
		{
			return UpdateError("Invalid Xform");
		}
		Transform val2 = default(Transform);
		((Transform)(ref value3)).TryGetInverse(ref val2);
		if (!((Transform)(ref val2)).IsValid)
		{
			return UpdateError("Invalid Xform");
		}
		On3dPoint val3 = new On3dPoint(0.0, value, 0.0);
		val3.Transform((IOnXform)(object)value3.Convert());
		On3dPoint val4 = new On3dPoint(0.0, -100.0, 100.0);
		On3dPoint val5 = new On3dPoint(0.0, 100.0, 100.0);
		On3dPoint val6 = new On3dPoint(0.0, 100.0, -100.0);
		On3dPoint val7 = new On3dPoint(0.0, -100.0, -100.0);
		OnBrep val8 = ((OnGeometry)RhUtil.RhinoCreateSurfaceFromCorners((IOn3dPoint)val4, (IOn3dPoint)(object)val5, ref val6, ref val7)).BrepForm();
		((OnGeometry)val8).Transform((IOnXform)(object)value3.Convert());
		OnCurve[] array = default(OnCurve[]);
		On3dPointArray val9 = default(On3dPointArray);
		if (!RhUtil.RhinoIntersectBreps((IOnBrep)(object)val, (IOnBrep)(object)val8, CSApplicationHelper.Application.Tolerance, ref array, ref val9))
		{
			return UpdateError("RhinoIntersectBreps failed");
		}
		if (array.Length == 0 && ((ArrayOn3dPoint)val9).Count() <= 0)
		{
			return UpdateError("RhinoIntersectBreps failed");
		}
		double num = 0.0;
		bool flag = false;
		OnCurve[] array2 = array;
		foreach (OnCurve val10 in array2)
		{
			double num2 = 0.0;
			if (val10.GetClosestPoint((IOn3dPoint)(object)val3, ref num2))
			{
				double num3 = val3.DistanceTo((IOn3dPoint)(object)val10.PointAt(num2));
				if (num3 <= num || !flag)
				{
					num = num3;
					flag = true;
				}
			}
		}
		foreach (On3dPoint item in (ArrayOn3dPoint)val9)
		{
			On3dPoint val11 = item;
			double num4 = val3.DistanceTo((IOn3dPoint)(object)val11);
			if (num4 <= num || !flag)
			{
				num = num4;
				flag = true;
			}
		}
		num -= value;
		OnXform val12 = new OnXform();
		val12.Translation(0.0, 0.0 - num + value2, 0.0);
		CSData data3 = data2.Duplicate();
		CSDataHelper.Transform(data3, value3 * val12.Convert() * val2);
		oObject.SetData(data3);
		return base.Update();
	}
}
