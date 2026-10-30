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
[CSBlock("BezelSweepNoProfile_V1", "{10E55353-06CA-4fec-80AC-7C10C17D8A3D}")]
public sealed class CSBezelSweepNoProfile1 : CSBlock
{
	public readonly CSInputChannel InRail1;

	public readonly CSInputChannel InRail2;

	public readonly CSOutputChannel OutBrep;

	public CSBezelSweepNoProfile1(string identifier)
		: base(identifier)
	{
		InRail1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InRail2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InRail1.GetFlattenArray();
		CSDataArray flattenArray2 = InRail2.GetFlattenArray();
		if (flattenArray.GetArrayCount() != flattenArray2.GetArrayCount())
		{
			return UpdateError("Rail count mismatch");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value);
			if (!((Transform)(ref value)).IsValid)
			{
				value = Transform.Identity;
			}
			CSData arrayItem2 = flattenArray2.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem2, out var value2);
			if (!((Transform)(ref value2)).IsValid)
			{
				value2 = Transform.Identity;
			}
			OnNurbsCurve val = arrayItem.GetNurbsCurve().Convert();
			if (val == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			OnNurbsCurve val2 = arrayItem2.GetNurbsCurve().Convert();
			if (val2 == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			On3dPoint obj = ((OnCurve)val).PointAtStart();
			On3dPoint val3 = ((OnCurve)val2).PointAtStart();
			if (obj.DistanceTo((IOn3dPoint)(object)val3) < CSApplicationHelper.Application.Tolerance)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			OnBrep srf = null;
			if (BezelSweepNoProfile((IOnCurve)(object)val, (IOnCurve)(object)val2, ref srf))
			{
				cSDataArray.Append(CSBlockUtil.CSUtilMath.MatrixCompare(value, value2) ? new CSOnGeometryData((GeometryBase)(object)srf.Convert(), value, applyTransform: false) : new CSOnGeometryData((GeometryBase)(object)srf.Convert()));
			}
			else
			{
				cSDataArray.Append(new CSData());
			}
		}
		OutBrep.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}

	private static bool BezelSweepNoProfile(IOnCurve rail1, IOnCurve rail2, ref OnBrep srf)
	{
		if (rail1 == null || rail2 == null)
		{
			return false;
		}
		if (!CSUtilSweep.Sweep2NoProfile(rail1, rail2, out var breps))
		{
			CS.CSUtil.PrintLine("Failed Sweep2NoProfile");
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
		if (!((OnObject)breps[0]).IsValid())
		{
			return false;
		}
		srf = breps[0];
		return true;
	}
}
