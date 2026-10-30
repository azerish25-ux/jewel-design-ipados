using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Shank.Helpers;

[CSBlock("FrenchSetProductionGeometrySorter", "{E0EDBF3F-252E-41A3-A59A-EF1F27263DA0}")]
public sealed class CSFrenchSetProductionGeometrySorter : CSBlock
{
	public readonly CSInputChannel InGeometryData;

	public readonly CSOutputChannel OutShankProfile;

	public readonly CSOutputChannel OutChannelProfile;

	public readonly CSOutputChannel OutScallopProfile;

	public readonly CSOutputChannel OutGraverBrep;

	public readonly CSOutputChannel OutTemplateGem;

	public readonly CSOutputChannel OutGemCutter;

	public readonly CSOutputChannel OutPilotHoleCutter;

	public CSFrenchSetProductionGeometrySorter(string identifier)
		: base(identifier)
	{
		InGeometryData = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutShankProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutChannelProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutScallopProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutGraverBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutTemplateGem = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutGemCutter = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutPilotHoleCutter = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InGeometryData.GetFlattenArray();
		List<Curve> list = new List<Curve>();
		Curve geometry = null;
		Curve geometry2 = null;
		Brep geometry3 = null;
		Brep geometry4 = null;
		Brep geometry5 = null;
		Brep geometry6 = null;
		foreach (CSData item in flattenArray.Array)
		{
			GeometryBase geometry7 = item.GetGeometry();
			if (geometry7 != null)
			{
				string text = geometry7.GetUserString("CSName").ToLower();
				if (text == "gem" || text == "diamond_round")
				{
					geometry4 = (Brep)(object)((geometry7 is Brep) ? geometry7 : null);
				}
				switch (text)
				{
				case "top":
				case "side":
				case "bottom":
					list.Add((Curve)(object)((geometry7 is Curve) ? geometry7 : null));
					break;
				}
				if (text == "graver")
				{
					geometry3 = (Brep)(object)((geometry7 is Brep) ? geometry7 : null);
				}
				if (text == "scallop")
				{
					geometry2 = (Curve)(object)((geometry7 is Curve) ? geometry7 : null);
				}
				if (text == "channel")
				{
					geometry = (Curve)(object)((geometry7 is Curve) ? geometry7 : null);
				}
				if (text == "pilothole")
				{
					geometry5 = (Brep)(object)((geometry7 is Brep) ? geometry7 : null);
				}
				if (text == "gemcutter")
				{
					geometry6 = (Brep)(object)((geometry7 is Brep) ? geometry7 : null);
				}
			}
		}
		CSDataArray cSDataArray = new CSDataArray();
		foreach (Curve item2 in list)
		{
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)item2));
		}
		OutShankProfile.SetData(cSDataArray);
		OutChannelProfile.SetData(new CSOnGeometryData((GeometryBase)(object)geometry));
		OutScallopProfile.SetData(new CSOnGeometryData((GeometryBase)(object)geometry2));
		OutGraverBrep.SetData(new CSOnGeometryData((GeometryBase)(object)geometry3));
		OutTemplateGem.SetData(new CSOnGeometryData((GeometryBase)(object)geometry4));
		OutPilotHoleCutter.SetData(new CSOnGeometryData((GeometryBase)(object)geometry5));
		OutGemCutter.SetData(new CSOnGeometryData((GeometryBase)(object)geometry6));
		return base.Update();
	}
}
