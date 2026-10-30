using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Settings;

[CSBlock("Prong", "{5C4DF6ED-5115-47f4-97AF-6014EEC8799B}")]
public sealed class CSProng : CSBlock
{
	public readonly CSOutputChannel oSurface;

	public readonly CSInputChannel InDiameter;

	public readonly CSInputChannel InHeight;

	public readonly CSInputChannel InFillet;

	public readonly CSInputChannel InTaper;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InMeshedProngs;

	public CSProng(string identifier)
		: base(identifier)
	{
		InDiameter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InFillet = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InTaper = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InMeshedProngs = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		oSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		OnNurbsCurve nurbs_curve = InProfile.NurbsCurve.Convert();
		if (nurbs_curve == null)
		{
			OnCircle onCircle = new OnCircle(OnUtil.On_xy_plane, OnUtil.On_origin, 1.0);
			nurbs_curve = new OnNurbsCurve();
			onCircle.GetNurbForm(ref nurbs_curve);
			if (nurbs_curve == null)
			{
				return UpdateMissing("Profile");
			}
		}
		double diameter = InDiameter.Double;
		double num = InHeight.Double;
		double num2 = InFillet.Double;
		num2 /= 100.0;
		double num3 = InTaper.Double;
		num3 /= 100.0;
		bool boolean = InMeshedProngs.Boolean;
		if (CSApplicationHelper.Application.IsCamMode)
		{
			num += CSApplicationHelper.Application.CamProngExtensionHeight;
		}
		if (boolean)
		{
			if (!CSUtilGeometry.CreateMeshProng(nurbs_curve, diameter, num, num2, num3, out var meshSurface))
			{
				return UpdateError("CreateMeshProng failed");
			}
			oSurface.SetData(new CSOnGeometryData(meshSurface.Convert()));
		}
		else
		{
			if (!CSUtilGeometry.CreateProng(nurbs_curve, diameter, num, num2, num3, out var nurbsSurface))
			{
				return UpdateError("CreateProng failed");
			}
			oSurface.SetData(new CSOnGeometryData(nurbsSurface.Convert()));
		}
		return base.Update();
	}
}
