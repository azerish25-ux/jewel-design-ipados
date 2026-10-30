using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("CurveBoolean", "{DA5627AC-1CDE-438a-B7A5-11EF4175D1FB}")]
public sealed class CSCurveBoolean : CSBlock
{
	public readonly CSInputChannel iPositiveCurve;

	public readonly CSInputChannel iNegativeCurve;

	public readonly CSOutputChannel oOuput;

	public CSCurveBoolean(string identifier)
		: base(identifier)
	{
		iPositiveCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iNegativeCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		oOuput = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		CSData data = iPositiveCurve.Data;
		if (data == null)
		{
			return UpdateMissing("Missing position curve data");
		}
		OnCurve val = (OnCurve)(object)data.GetNurbsCurve().Convert();
		if (val == null)
		{
			return UpdateMissing("Missing position curve");
		}
		CSDataHelper.GetTransformNoArray(data, out var value);
		if (!value.IsValid)
		{
			value = Transform.Identity;
		}
		CSDataArray flattenArray = iNegativeCurve.GetFlattenArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			OnCurve val2 = (OnCurve)(object)flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val2 == null)
			{
				return UpdateError("Invalid negative curve");
			}
			OnCurve[] array = RhUtil.RhinoCurveBooleanDifference((IOnCurve)(object)val, (IOnCurve)(object)val2);
			if (array != null && array.Length != 0)
			{
				OnNurbsCurve val3 = new OnNurbsCurve();
				if (array[0] != null)
				{
					array[0].GetNurbForm(ref val3);
				}
				if (val3 != null && ((OnObject)val3).IsValid())
				{
					val = (OnCurve)(object)val3;
				}
			}
		}
		value.TryGetInverse(out var inverseTransform);
		((OnGeometry)val).Transform((IOnXform)(object)inverseTransform.Convert());
		OnCurve val4 = val.DuplicateCurve();
		((OnGeometry)val4).Transform((IOnXform)(object)value.Convert());
		oOuput.SetData(new CSOnGeometryData(val4.Convert(), value, applyTransform: false));
		return base.Update();
	}
}
