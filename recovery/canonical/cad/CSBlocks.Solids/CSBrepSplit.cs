using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Solids;

[CSBlock("BrepSplit", "{409816A9-9161-4a2f-B230-3446B20240BB}")]
public sealed class CSBrepSplit : CSBlock
{
	public CSInputChannel InSurface { get; }

	public CSInputChannel InSplittingSurface { get; }

	public CSOutputChannel OutSmallBoundingBox { get; }

	public CSOutputChannel OutLargeBoundingBox { get; }

	public CSOutputChannel OutPieces { get; }

	public CSBrepSplit(string identifier)
		: base(identifier)
	{
		InSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		InSplittingSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		OutSmallBoundingBox = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutLargeBoundingBox = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutPieces = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		OnBrep onBrep = InSurface.GetBrep();
		if (onBrep == null)
		{
			OnNurbsSurface nurbsSurface = InSurface.GetNurbsSurface();
			if (nurbsSurface != null)
			{
				onBrep = nurbsSurface.BrepForm();
			}
		}
		if (onBrep == null)
		{
			return UpdateMissing("Missing surface");
		}
		OnBrep onBrep2 = InSplittingSurface.GetBrep();
		if (onBrep2 == null)
		{
			OnNurbsSurface nurbsSurface2 = InSplittingSurface.GetNurbsSurface();
			if (nurbsSurface2 != null)
			{
				onBrep2 = nurbsSurface2.BrepForm();
			}
		}
		if (onBrep2 == null)
		{
			return UpdateMissing("Missing splitting surface");
		}
		if (!RhUtil.RhinoBrepSplit(onBrep, onBrep2, 0.01, out var pieces))
		{
			return UpdateError("RhinoBrepSplit failed");
		}
		CSDataArray cSDataArray = new CSDataArray();
		OnBrep[] array = pieces;
		foreach (OnBrep brep in array)
		{
			cSDataArray.Append(new CSOnGeometryData(brep.Convert()));
		}
		OutPieces.SetData(cSDataArray);
		if (pieces.Length == 1)
		{
			OutSmallBoundingBox.SetData(new CSOnGeometryData(pieces[0].Convert()));
			OutLargeBoundingBox.SetData(new CSOnGeometryData(pieces[0].Convert()));
			return base.Update();
		}
		OnBrep onBrep3 = null;
		double num = 0.0;
		OnBrep onBrep4 = null;
		double num2 = double.MaxValue;
		array = pieces;
		foreach (OnBrep onBrep5 in array)
		{
			OnBoundingBox tight_bbox = new OnBoundingBox();
			onBrep5.ShrinkSurfaces();
			onBrep5.GetTightBoundingBox(ref tight_bbox);
			double num3 = tight_bbox.Volume();
			if (num3 > num)
			{
				onBrep3 = onBrep5;
				num = num3;
			}
			if (num3 < num2)
			{
				onBrep4 = onBrep5;
				num2 = num3;
			}
		}
		if (onBrep4 != null)
		{
			OutSmallBoundingBox.SetData(new CSOnGeometryData(onBrep4.Convert()));
		}
		if (onBrep3 != null)
		{
			OutLargeBoundingBox.SetData(new CSOnGeometryData(onBrep3.Convert()));
		}
		return base.Update();
	}
}
