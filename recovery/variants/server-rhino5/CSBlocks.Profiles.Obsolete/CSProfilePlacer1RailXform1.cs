using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Profiles.Obsolete;

[Obsolete]
[CSBlock("ProfilePlacer1RailXform_V1", "{0D140D70-56C1-4ca5-8BC2-2233B555B06C}")]
public sealed class CSProfilePlacer1RailXform1 : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iParameter { get; }

	public CSInputChannel iMethod { get; }

	public CSOutputChannel oXform { get; }

	public CSOutputChannel oParam { get; }

	public CSProfilePlacer1RailXform1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iParameter = new CSInputChannel(GetPropertyName(() => iParameter), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMethod = new CSInputChannel(GetPropertyName(() => iMethod), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(GetPropertyName(() => oXform), this, ChannelDataType.Transform, ChannelArrayType.Individual);
		oParam = new CSOutputChannel(GetPropertyName(() => oParam), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_0147: Expected O, but got Unknown
		//IL_0147: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		//IL_01e8: Expected O, but got Unknown
		//IL_01e8: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Expected O, but got Unknown
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Expected O, but got Unknown
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Expected O, but got Unknown
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Expected O, but got Unknown
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Expected O, but got Unknown
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Expected O, but got Unknown
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Expected O, but got Unknown
		//IL_03e2: Expected O, but got Unknown
		//IL_03e2: Expected O, but got Unknown
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Expected O, but got Unknown
		OnNurbsCurve val = iRail.GetFlattenArray().GetArrayItem(0).GetNurbsCurve()
			.Convert();
		if (val == null)
		{
			return UpdateMissing("Profile Placer Simple: Missing Nurbs Curve");
		}
		double value = 0.0;
		iParameter.GetDouble(ref value);
		int value2 = 0;
		iMethod.GetInteger(ref value2);
		double num = 0.0;
		if (!((OnCurve)val).GetNormalizedArcLengthPoint(value, ref num))
		{
			return UpdateError("Get Arc Length Failed");
		}
		oParam.SetData(new CSNumberData(num));
		On3dPoint val2 = ((OnCurve)val).PointAt(num);
		On3dVector destination = ((OnCurve)val).TangentAt(num);
		OnXform xform = new OnXform();
		switch (value2)
		{
		case 0:
		{
			On3dVector destination2 = On3dVector.op_Implicit((IOn3dPoint)(object)(val2 * -1.0));
			destination2.Unitize();
			xform = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dVector)new On3dVector(0.0, -1.0, 0.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)destination, (IOn3dVector)(object)destination2);
			break;
		}
		case 1:
		{
			On3dVector destination2 = On3dVector.op_Implicit((IOn3dPoint)(object)(val2 * -1.0));
			destination2.y = 0.0;
			destination2.Unitize();
			xform = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dVector)new On3dVector(0.0, -1.0, 0.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)destination, (IOn3dVector)(object)destination2);
			break;
		}
		case 2:
		{
			On3dVector destination2 = new On3dVector(0.0, 0.0, -1.0);
			xform = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dVector)new On3dVector(0.0, -1.0, 0.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)destination, (IOn3dVector)(object)destination2);
			break;
		}
		case 3:
		{
			On3dVector destination2 = new On3dVector(0.0, 0.0, -1.0);
			OnPlane val4 = new OnPlane();
			if (((OnCurve)val).IsPlanar(val4))
			{
				destination2 = new On3dVector(val4.Normal());
			}
			xform = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dVector)new On3dVector(0.0, -1.0, 0.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)destination, (IOn3dVector)(object)destination2);
			break;
		}
		case 4:
		{
			On3dVector destination2 = new On3dVector(0.0, 0.0, -1.0);
			OnPlane val3 = new OnPlane();
			if (((OnCurve)val).IsPlanar(val3))
			{
				destination2 = new On3dVector(val3.Normal());
			}
			xform = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(1.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, -1.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)destination, (IOn3dVector)(object)destination2);
			break;
		}
		}
		oXform.SetData(new CSXformData(xform.Convert()));
		return base.Update();
	}
}
