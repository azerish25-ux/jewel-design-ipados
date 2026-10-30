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
		CSData data = iPositiveCurve.Data;
		if (data == null)
		{
			return UpdateMissing("Missing position curve data");
		}
		OnCurve onCurve = data.GetNurbsCurve().Convert();
		if (onCurve == null)
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
			OnCurve onCurve2 = flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onCurve2 == null)
			{
				return UpdateError("Invalid negative curve");
			}
			OnCurve[] array = RhUtil.RhinoCurveBooleanDifference(onCurve, onCurve2);
			if (array != null && array.Length != 0)
			{
				OnNurbsCurve nurbs_curve = new OnNurbsCurve();
				if (array[0] != null)
				{
					array[0].GetNurbForm(ref nurbs_curve);
				}
				if (nurbs_curve != null && nurbs_curve.IsValid())
				{
					onCurve = nurbs_curve;
				}
			}
		}
		value.TryGetInverse(out var inverseTransform);
		onCurve.Transform(inverseTransform.Convert());
		OnCurve onCurve3 = onCurve.DuplicateCurve();
		onCurve3.Transform(value.Convert());
		oOuput.SetData(new CSOnGeometryData(onCurve3.Convert(), value, applyTransform: false));
		return base.Update();
	}
}
