using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Surfaces.Obsolete;

[Obsolete]
[CSBlock("NetworkSurface_V1", "{46C5ECD4-B354-4927-B033-419142422457}")]
public sealed class CSNetworkSurface1 : CSBlock
{
	public CSInputChannel iArrayOfCurves { get; }

	public CSOutputChannel oSurface { get; }

	public CSNetworkSurface1(string identifier)
		: base(identifier)
	{
		iArrayOfCurves = new CSInputChannel(GetPropertyName(() => iArrayOfCurves), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(GetPropertyName(() => oSurface), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		lock (CSApplicationHelper.Application)
		{
			List<OnNurbsCurve> list = new List<OnNurbsCurve>();
			CSDataArray flattenArray = iArrayOfCurves.GetFlattenArray();
			if (flattenArray == null)
			{
				return UpdateMissing("NetworkSrf: Missing Input Curve\n");
			}
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				CSData arrayItem = flattenArray.GetArrayItem(i);
				if (arrayItem != null)
				{
					OnNurbsCurve val = arrayItem.GetNurbsCurve().Convert();
					if (val != null)
					{
						list.Add(val);
					}
				}
			}
			if (list.Count < 3)
			{
				return UpdateMissing("NetworkSrf: Missing Input Curve\n");
			}
			RhUtil.RhinoApp().ActiveDoc().UnselectAll();
			List<MRhinoCurveObject> list2 = new List<MRhinoCurveObject>();
			for (int j = 0; j < list.Count; j++)
			{
				list2.Add(RhUtil.RhinoApp().ActiveDoc().AddCurveObject((IOnCurve)(object)list[j]));
			}
			List<Guid> list3 = new List<Guid>();
			for (int k = 0; k < list2.Count; k++)
			{
				list3.Add(((IOn3dmObjectAttributes)((MRhinoObject)list2[k]).Attributes()).m_uuid);
			}
			for (int l = 0; l < list2.Count; l++)
			{
				((MRhinoObject)list2[l]).Select();
			}
			CSUtil.EnableDrawing(which: false);
			CSApplicationHelper.Application.InRunScriptBlock = true;
			RhUtil.RhinoApp().RunMenuScript("-_NetworkSrf _Enter");
			RhUtil.RhinoApp().ActiveDoc().UnselectAll();
			RhUtil.RhinoApp().RunMenuScript("-_SelLast _Enter");
			CSApplicationHelper.Application.InRunScriptBlock = false;
			CSUtil.EnableDrawing(which: true);
			MRhinoObjectIterator val2 = new MRhinoObjectIterator((object_state)7, (object_category)3);
			val2.EnableSelectedFilter();
			OnBrep val3 = null;
			Guid guid = default(Guid);
			for (MRhinoObject val4 = val2.First(); val4 != null; val4 = val2.Next())
			{
				if (val4.Geometry() != null)
				{
					val3 = val4.Geometry().BrepForm();
					guid = ((IOn3dmObjectAttributes)val4.Attributes()).m_uuid;
				}
				if (val3 != null)
				{
					break;
				}
			}
			if (val3 != null)
			{
				oSurface.SetData(new CSOnGeometryData((GeometryBase)(object)val3.Convert()));
				RhUtil.RhinoApp().ActiveDoc().DeleteObject((IRhinoObjRef)new MRhinoObjRef(guid));
			}
			for (int m = 0; m < list3.Count; m++)
			{
				RhUtil.RhinoApp().ActiveDoc().DeleteObject((IRhinoObjRef)new MRhinoObjRef(list3[m]));
			}
		}
		return base.Update();
	}
}
