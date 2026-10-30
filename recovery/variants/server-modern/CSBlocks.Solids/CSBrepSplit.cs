using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

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
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		OnBrep val = InSurface.GetBrep();
		if (val == null)
		{
			OnNurbsSurface nurbsSurface = InSurface.GetNurbsSurface();
			if (nurbsSurface != null)
			{
				val = ((OnGeometry)nurbsSurface).BrepForm();
			}
		}
		if (val == null)
		{
			return UpdateMissing("Missing surface");
		}
		OnBrep val2 = InSplittingSurface.GetBrep();
		if (val2 == null)
		{
			OnNurbsSurface nurbsSurface2 = InSplittingSurface.GetNurbsSurface();
			if (nurbsSurface2 != null)
			{
				val2 = ((OnGeometry)nurbsSurface2).BrepForm();
			}
		}
		if (val2 == null)
		{
			return UpdateMissing("Missing splitting surface");
		}
		OnBrep[] array = default(OnBrep[]);
		if (!RhUtil.RhinoBrepSplit((IOnBrep)(object)val, (IOnBrep)(object)val2, 0.01, ref array))
		{
			return UpdateError("RhinoBrepSplit failed");
		}
		CSDataArray cSDataArray = new CSDataArray();
		OnBrep[] array2 = array;
		foreach (OnBrep brep in array2)
		{
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)brep.Convert()));
		}
		OutPieces.SetData(cSDataArray);
		if (array.Length == 1)
		{
			OutSmallBoundingBox.SetData(new CSOnGeometryData((GeometryBase)(object)array[0].Convert()));
			OutLargeBoundingBox.SetData(new CSOnGeometryData((GeometryBase)(object)array[0].Convert()));
			return base.Update();
		}
		OnBrep val3 = null;
		double num = 0.0;
		OnBrep val4 = null;
		double num2 = double.MaxValue;
		array2 = array;
		foreach (OnBrep val5 in array2)
		{
			OnBoundingBox val6 = new OnBoundingBox();
			val5.ShrinkSurfaces();
			((OnGeometry)val5).GetTightBoundingBox(ref val6);
			double num3 = val6.Volume();
			if (num3 > num)
			{
				val3 = val5;
				num = num3;
			}
			if (num3 < num2)
			{
				val4 = val5;
				num2 = num3;
			}
		}
		if (val4 != null)
		{
			OutSmallBoundingBox.SetData(new CSOnGeometryData((GeometryBase)(object)val4.Convert()));
		}
		if (val3 != null)
		{
			OutLargeBoundingBox.SetData(new CSOnGeometryData((GeometryBase)(object)val3.Convert()));
		}
		return base.Update();
	}
}
