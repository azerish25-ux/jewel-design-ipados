using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlocks.Surfaces;

[CSBlock("NetworkSurfaceAdvanced", "{8A5AFEE3-DB2A-4214-A99A-47C055A9DD4E}")]
public sealed class CSNetworkSurfaceAdvanced : CSBlock
{
	public readonly CSInputChannel InUCurves;

	public readonly CSInputChannel InVCurves;

	public readonly CSInputChannel InContinuityModeStartU;

	public readonly CSInputChannel InContinuityModeEndU;

	public readonly CSInputChannel InContinuityModeStartV;

	public readonly CSInputChannel InContinuityModeEndV;

	public readonly CSOutputChannel OutSurface;

	public CSNetworkSurfaceAdvanced(string identifier)
		: base(identifier)
	{
		InUCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InVCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InContinuityModeStartU = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InContinuityModeEndU = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InContinuityModeStartV = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InContinuityModeEndV = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InUCurves.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("U Curves");
		}
		List<Curve> curves = NetworkSurfaceHelper.GetCurves(flattenArray);
		if (curves.Count <= 0)
		{
			return UpdateMissing("Invalid U Curve Data");
		}
		CSDataArray flattenArray2 = InVCurves.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("V Curves");
		}
		List<Curve> curves2 = NetworkSurfaceHelper.GetCurves(flattenArray2);
		if (curves2.Count <= 0)
		{
			return UpdateMissing("Invalid V Curve Data");
		}
		ValueListItem whichItem = ValueListItem.ContinuityModeTan;
		InContinuityModeStartU.GetValueListItem(ref whichItem);
		if (!NetworkSurfaceHelper.GetContinuityMode(whichItem, out var continuityMode))
		{
			return UpdateError("Invalid continuity mode (U Start)");
		}
		ValueListItem whichItem2 = ValueListItem.ContinuityModeTan;
		InContinuityModeEndU.GetValueListItem(ref whichItem2);
		if (!NetworkSurfaceHelper.GetContinuityMode(whichItem2, out var continuityMode2))
		{
			return UpdateError("Invalid continuity mode (U End)");
		}
		ValueListItem whichItem3 = ValueListItem.ContinuityModeTan;
		InContinuityModeStartV.GetValueListItem(ref whichItem3);
		if (!NetworkSurfaceHelper.GetContinuityMode(whichItem3, out var continuityMode3))
		{
			return UpdateError("Invalid continuity mode (V Start)");
		}
		ValueListItem whichItem4 = ValueListItem.ContinuityModeTan;
		InContinuityModeEndV.GetValueListItem(ref whichItem4);
		if (!NetworkSurfaceHelper.GetContinuityMode(whichItem4, out var continuityMode4))
		{
			return UpdateError("Invalid continuity mode (V End)");
		}
		int error;
		NurbsSurface nurbsSurface = NurbsSurface.CreateNetworkSurface(curves, continuityMode, continuityMode2, curves2, continuityMode3, continuityMode4, 0.001, 0.01, CSApplicationHelper.Application.AngleToleranceRadians, out error);
		if (nurbsSurface == null)
		{
			string error2 = NetworkSurfaceHelper.GetError(error);
			return UpdateError($"CreateNetworkSurface failed: {error2}");
		}
		Brep brep = nurbsSurface.ToBrep();
		if (brep == null)
		{
			return UpdateError("Convert to Brep failed");
		}
		if (!(Interop.ToOnBrep(brep) is OnBrep brep2))
		{
			return UpdateError("Convert to OnBrep failed");
		}
		OutSurface.SetData(new CSOnGeometryData(brep2.Convert()));
		return base.Update();
	}
}
