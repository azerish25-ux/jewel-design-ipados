using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.Surfaces;

[CSBlock("NetworkSurface", "{878610F2-8433-4738-86A0-8186218C3E41}")]
public sealed class CSNetworkSurface : CSBlock
{
	public CSInputChannel InCurves { get; }

	public CSInputChannel InContinuityMode { get; }

	public CSOutputChannel OutSurface { get; }

	public CSNetworkSurface(string identifier)
		: base(identifier)
	{
		InCurves = new CSInputChannel(GetPropertyName(() => InCurves), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InContinuityMode = new CSInputChannel(GetPropertyName(() => InContinuityMode), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(GetPropertyName(() => OutSurface), this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InCurves.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curves");
		}
		List<Curve> curves = NetworkSurfaceHelper.GetCurves(flattenArray);
		if (curves.Count <= 0)
		{
			return UpdateMissing("Invalid Curve Data");
		}
		ValueListItem whichItem = ValueListItem.ContinuityModeTan;
		InContinuityMode.GetValueListItem(ref whichItem);
		if (!NetworkSurfaceHelper.GetContinuityMode(whichItem, out var continuityMode))
		{
			return UpdateError("Invalid continuity mode");
		}
		int error = default(int);
		NurbsSurface val = NurbsSurface.CreateNetworkSurface((IEnumerable<Curve>)curves, continuityMode, 0.001, 0.01, CSApplicationHelper.Application.AngleToleranceRadians, ref error);
		if (val == null)
		{
			string error2 = NetworkSurfaceHelper.GetError(error);
			return UpdateError($"CreateNetworkSurface failed: {error2}");
		}
		Brep val2 = ((Surface)val).ToBrep();
		if (val2 == null)
		{
			return UpdateError("Convert to Brep failed");
		}
		OutSurface.SetData(new CSOnGeometryData((GeometryBase)(object)val2));
		return base.Update();
	}
}
