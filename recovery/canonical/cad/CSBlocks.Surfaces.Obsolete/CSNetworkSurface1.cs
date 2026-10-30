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
					OnNurbsCurve onNurbsCurve = arrayItem.GetNurbsCurve().Convert();
					if (onNurbsCurve != null)
					{
						list.Add(onNurbsCurve);
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
				list2.Add(RhUtil.RhinoApp().ActiveDoc().AddCurveObject(list[j]));
			}
			List<Guid> list3 = new List<Guid>();
			for (int k = 0; k < list2.Count; k++)
			{
				list3.Add(list2[k].Attributes().m_uuid);
			}
			for (int l = 0; l < list2.Count; l++)
			{
				list2[l].Select();
			}
			CSUtil.EnableDrawing(which: false);
			CSApplicationHelper.Application.InRunScriptBlock = true;
			RhUtil.RhinoApp().RunMenuScript("-_NetworkSrf _Enter");
			RhUtil.RhinoApp().ActiveDoc().UnselectAll();
			RhUtil.RhinoApp().RunMenuScript("-_SelLast _Enter");
			CSApplicationHelper.Application.InRunScriptBlock = false;
			CSUtil.EnableDrawing(which: true);
			MRhinoObjectIterator mRhinoObjectIterator = new MRhinoObjectIterator(IRhinoObjectIterator.object_state.undeleted_objects, IRhinoObjectIterator.object_category.active_and_reference_objects);
			mRhinoObjectIterator.EnableSelectedFilter();
			OnBrep onBrep = null;
			Guid object_uuid = default(Guid);
			for (MRhinoObject mRhinoObject = mRhinoObjectIterator.First(); mRhinoObject != null; mRhinoObject = mRhinoObjectIterator.Next())
			{
				if (mRhinoObject.Geometry() != null)
				{
					onBrep = mRhinoObject.Geometry().BrepForm();
					object_uuid = mRhinoObject.Attributes().m_uuid;
				}
				if (onBrep != null)
				{
					break;
				}
			}
			if (onBrep != null)
			{
				oSurface.SetData(new CSOnGeometryData(onBrep.Convert()));
				RhUtil.RhinoApp().ActiveDoc().DeleteObject(new MRhinoObjRef(object_uuid));
			}
			for (int m = 0; m < list3.Count; m++)
			{
				RhUtil.RhinoApp().ActiveDoc().DeleteObject(new MRhinoObjRef(list3[m]));
			}
		}
		return base.Update();
	}
}
