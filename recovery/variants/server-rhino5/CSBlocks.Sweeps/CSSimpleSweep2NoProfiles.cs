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

namespace CSBlocks.Sweeps;

[CSBlock("SimpleSweep2NoProfiles", "{C6AC4503-FF5C-4712-86B0-CA7779DA392E}")]
public sealed class CSSimpleSweep2NoProfiles : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iRail2 { get; }

	public CSInputChannel iAdjustSeam { get; }

	public CSInputChannel iProfileCount { get; }

	public CSOutputChannel oBrep { get; }

	public CSSimpleSweep2NoProfiles(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iRail2 = new CSInputChannel(GetPropertyName(() => iRail2), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iAdjustSeam = new CSInputChannel(GetPropertyName(() => iAdjustSeam), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iProfileCount = new CSInputChannel(GetPropertyName(() => iProfileCount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		bool value = false;
		iAdjustSeam.GetBoolean(ref value);
		int value2 = 1;
		iProfileCount.GetInteger(ref value2);
		CSOnGeometryData outputData = null;
		CSDataArray flattenArray = iRail.GetFlattenArray();
		CSDataArray flattenArray2 = iRail2.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing rail 1");
		}
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing rail 2");
		}
		if (flattenArray.GetArrayCount() != flattenArray2.GetArrayCount())
		{
			return UpdateError("Mismatched rails");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem == null)
			{
				continue;
			}
			NurbsCurve nurbsCurve = arrayItem.GetNurbsCurve();
			if (nurbsCurve == null || !nurbsCurve.IsValid)
			{
				continue;
			}
			CSData arrayItem2 = flattenArray2.GetArrayItem(i);
			if (arrayItem2 == null)
			{
				continue;
			}
			NurbsCurve nurbsCurve2 = arrayItem2.GetNurbsCurve();
			if (nurbsCurve2 != null && nurbsCurve2.IsValid)
			{
				CSDataHelper.GetTransformNoArray(arrayItem, out var value3);
				CSDataHelper.GetTransformNoArray(arrayItem2, out var value4);
				bool flag = CSBlockUtil.CSUtilMath.MatrixCompare(value3, value4);
				if (!SimpleSweep2NoProfile(xUp: (!(value3.IsValid && value4.IsValid && flag)) ? Transform.Identity : value3, rail1: nurbsCurve, rail2: nurbsCurve2, outputData: ref outputData, adjust: value, profileCount: value2))
				{
					return UpdateError("Fail Sweep 2");
				}
				cSDataArray.Append(outputData);
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oBrep.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oBrep.SetData(cSDataArray);
		}
		return base.Update();
	}

	private static bool SimpleSweep2NoProfile(Curve rail1, Curve rail2, ref CSOnGeometryData outputData, bool adjust, int profileCount, Transform xUp)
	{
		if (rail1 == null || rail2 == null)
		{
			return false;
		}
		if (adjust)
		{
			CSUtilSweepRC.MatchCurveDirection2(rail1, rail2, 40);
		}
		if (!CSUtilSweep.Sweep2NoProfile((IOnCurve)(object)rail1.Convert(), (IOnCurve)(object)rail2.Convert(), out var breps, rail1.IsClosed, profileCount))
		{
			CS.CSUtil.PrintLine("Failed Sweep2");
			return false;
		}
		if (breps == null)
		{
			return false;
		}
		if (breps.Length < 1)
		{
			return false;
		}
		outputData = new CSOnGeometryData(breps[0].Convert(), xUp, applyTransform: false);
		return true;
	}
}
