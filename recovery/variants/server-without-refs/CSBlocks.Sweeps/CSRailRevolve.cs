using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Sweeps;

[CSBlock("RailRevolve", "{C2482A30-5FCB-417f-994D-3CC31B7A9573}")]
public sealed class CSRailRevolve : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iProfile { get; }

	public CSInputChannel iDirection { get; }

	public CSOutputChannel oSurface { get; }

	public CSRailRevolve(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iDirection = new CSInputChannel(GetPropertyName(() => iDirection), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(GetPropertyName(() => oSurface), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		OnNurbsCurve nurbsCurve = iRail.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing Rail");
		}
		OnNurbsCurve nurbsCurve2 = iProfile.GetNurbsCurve();
		if (nurbsCurve2 == null)
		{
			return UpdateMissing("Missing Profile");
		}
		int value = 0;
		iDirection.GetInteger(ref value);
		OnNurbsSurface val = CSUtilRailRevolve.RailRevolve((IOnNurbsCurve)(object)nurbsCurve, (IOnNurbsCurve)(object)nurbsCurve2, value);
		if (val == null)
		{
			return UpdateError("RailRevolve failed");
		}
		OnGeometry obj = (OnGeometry)(object)val;
		if (((OnGeometry)val).HasBrepForm())
		{
			OnBrep val2 = ((OnGeometry)val).BrepForm();
			if (val2 != null)
			{
				obj = (OnGeometry)(object)val2;
			}
		}
		oSurface.SetData(new CSOnGeometryData(((OnObject)(object)obj).Convert()));
		return base.Update();
	}
}
