using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

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
		int value = 0;
		iMode.GetInteger(ref value);
		IOn3dVector normal;
		switch (value)
		{
		case 0:
			normal = new On3dVector(OnUtil.On_xaxis);
			break;
		case 1:
			normal = new On3dVector(OnUtil.On_xaxis) * -1.0;
			break;
		case 2:
			normal = new On3dVector(OnUtil.On_yaxis);
			break;
		case 3:
			normal = new On3dVector(OnUtil.On_yaxis) * -1.0;
			break;
		case 4:
			normal = new On3dVector(OnUtil.On_zaxis);
			break;
		case 5:
			normal = new On3dVector(OnUtil.On_zaxis) * -1.0;
			break;
		default:
			return UpdateError("Invalid mode");
		}
		OnInterval extents = new OnInterval(-200.0, 200.0);
		OnPlaneSurface onPlaneSurface = new OnPlaneSurface(new OnPlane(OnUtil.On_origin, normal));
		onPlaneSurface.SetExtents(0, extents);
		onPlaneSurface.SetExtents(1, extents);
		oCutter.SetData(new CSOnGeometryData(onPlaneSurface.Convert()));
		return base.Update();
	}
}
