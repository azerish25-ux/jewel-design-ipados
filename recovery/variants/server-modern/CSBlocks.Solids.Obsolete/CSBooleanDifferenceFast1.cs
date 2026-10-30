using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Solids.Obsolete;

[Obsolete]
[CSBlock("BooleanDifferenceFast_V1", "{A83E685D-24AC-4654-B9E2-BE1696BC888A}")]
public sealed class CSBooleanDifferenceFast1 : CSBlock
{
	public CSOutputChannel oSurface { get; }

	public CSInputChannel iSurfaceLHS { get; }

	public CSInputChannel iSurfaceRHS { get; }

	public CSInputChannel iFlip { get; }

	public CSBooleanDifferenceFast1(string identifier)
		: base(identifier)
	{
		iSurfaceLHS = new CSInputChannel(GetPropertyName(() => iSurfaceLHS), this, ChannelDataType.Brep, ChannelArrayType.Array);
		iSurfaceRHS = new CSInputChannel(GetPropertyName(() => iSurfaceRHS), this, ChannelDataType.Brep, ChannelArrayType.Array);
		iFlip = new CSInputChannel(GetPropertyName(() => iFlip), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(GetPropertyName(() => oSurface), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		CSDataArray flattenArray = iSurfaceLHS.GetFlattenArray();
		CSDataArray flattenArray2 = iSurfaceRHS.GetFlattenArray();
		bool value = false;
		iFlip.GetBoolean(ref value);
		List<OnBrep> input = GetInput(flattenArray);
		List<OnBrep> input2 = GetInput(flattenArray2);
		bool flag = false;
		Arrayint val = new Arrayint();
		OnBrep[] array3 = default(OnBrep[]);
		if (value)
		{
			IOnBrep[] array = (IOnBrep[])(object)input.ToArray();
			IOnBrep[] array2 = array;
			array = (IOnBrep[])(object)input2.ToArray();
			RhUtil.RhinoBooleanIntersection(array2, array, 0.01, ref flag, ref array3);
		}
		else
		{
			IOnBrep[] array = (IOnBrep[])(object)input.ToArray();
			IOnBrep[] array4 = array;
			array = (IOnBrep[])(object)input2.ToArray();
			RhUtil.RhinoBooleanDifference(array4, array, 0.01, ref flag, ref array3, ref val);
		}
		if (!flag || array3 == null)
		{
			oSurface.SetData(iSurfaceLHS.Data.Duplicate());
			return base.Update();
		}
		CSDataArray cSDataArray = new CSDataArray();
		OnBrep[] array5 = array3;
		foreach (OnBrep val2 in array5)
		{
			val2.ShrinkSurfaces();
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val2.Convert()));
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oSurface.SetData(cSDataArray.GetArrayItem(0));
		}
		else if (cSDataArray.GetArrayCount() > 1)
		{
			oSurface.SetData(cSDataArray);
		}
		else
		{
			oSurface.SetData(iSurfaceLHS.Data.Duplicate());
		}
		return base.Update();
	}

	private static List<OnBrep> GetInput(CSDataArray datas)
	{
		List<OnBrep> list = new List<OnBrep>();
		for (int i = 0; i < datas.GetArrayCount(); i++)
		{
			CSData arrayItem = datas.GetArrayItem(i);
			if (arrayItem == null)
			{
				continue;
			}
			OnBrep val = arrayItem.GetBrep().Convert();
			if (val != null)
			{
				val.SplitKinkyFaces();
				val.FlipReversedSurfaces();
				val.ShrinkSurfaces();
				val.Compact();
				if (val.SolidOrientation() == -1)
				{
					val.Flip();
				}
				((OnGeometry)val).ClearBoundingBox();
				val.SetVertexTolerances();
				val.SetTrimIsoFlags();
				val.SetTrimTypeFlags();
				val.SetTrimTolerances();
				val.SetTrimBoundingBoxes();
				list.Add(val);
			}
		}
		return list;
	}
}
