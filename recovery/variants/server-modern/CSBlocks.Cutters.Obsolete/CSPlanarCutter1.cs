using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Cutters.Obsolete;

[Obsolete]
[CSBlock("PlanarCutter_V1", "{952D8A42-6A92-449D-9966-F347F453A8A3}")]
public sealed class CSPlanarCutter1 : CSBlock
{
	public CSInputChannel iMode { get; }

	public CSOutputChannel oCutter { get; }

	public CSPlanarCutter1(string identifier)
		: base(identifier)
	{
		iMode = new CSInputChannel(GetPropertyName(() => iMode), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCutter = new CSOutputChannel(GetPropertyName(() => oCutter), this, ChannelDataType.Surface, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		int value = 0;
		iMode.GetInteger(ref value);
		IOn3dVector val;
		switch (value)
		{
		case 0:
			val = (IOn3dVector)new On3dVector(OnUtil.On_xaxis);
			break;
		case 1:
			val = (IOn3dVector)(object)(new On3dVector(OnUtil.On_xaxis) * -1.0);
			break;
		case 2:
			val = (IOn3dVector)new On3dVector(OnUtil.On_yaxis);
			break;
		case 3:
			val = (IOn3dVector)(object)(new On3dVector(OnUtil.On_yaxis) * -1.0);
			break;
		case 4:
			val = (IOn3dVector)new On3dVector(OnUtil.On_zaxis);
			break;
		case 5:
			val = (IOn3dVector)(object)(new On3dVector(OnUtil.On_zaxis) * -1.0);
			break;
		default:
			return UpdateError("Invalid mode");
		}
		OnInterval val2 = new OnInterval(-200.0, 200.0);
		OnPlaneSurface val3 = new OnPlaneSurface((IOnPlane)new OnPlane(OnUtil.On_origin, val));
		val3.SetExtents(0, (IOnInterval)(object)val2);
		val3.SetExtents(1, (IOnInterval)(object)val2);
		oCutter.SetData(new CSOnGeometryData((GeometryBase)(object)((OnSurface)(object)val3).Convert()));
		return base.Update();
	}
}
