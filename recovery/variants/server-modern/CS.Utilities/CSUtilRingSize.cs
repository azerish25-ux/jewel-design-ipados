using System.Collections.Generic;
using System.Linq;
using CS.CSObjects.CSBlocks;
using CS.Parametric.Channels;
using CSDatas;
using CSRingSizes.RingSize;

namespace CS.Utilities;

public static class CSUtilRingSize
{
	public static RingSize CreateRingSizeTranslator(ERingSizeOption option)
	{
		return option switch
		{
			ERingSizeOption.Britain => new RingSizeUK(), 
			ERingSizeOption.UnitedStates => new RingSizeUSA(), 
			ERingSizeOption.France => new RingSizeFrance(), 
			ERingSizeOption.European => new RingSizeEur(), 
			ERingSizeOption.German => new RingSizeGerman(), 
			ERingSizeOption.Japan => new RingSizeJapan(), 
			ERingSizeOption.Swiss => new RingSizeSwiss(), 
			_ => new RingSizeUSA(), 
		};
	}

	public static List<UniversalRingSize> GetCurrentSizes()
	{
		List<CSOutputChannel> list = CSUtil.FindAllValidOutputRingRailSizes();
		CSOutputChannel cSOutputChannel = list.FirstOrDefault((CSOutputChannel _) => _.Parent.ImportGroup == null);
		if (cSOutputChannel != null)
		{
			list = new List<CSOutputChannel> { cSOutputChannel };
		}
		List<UniversalRingSize> translatedRingSizes = new List<UniversalRingSize>();
		list.ForEach(delegate(CSOutputChannel _)
		{
			CSData data = _.Data;
			if (data != null)
			{
				double value = 0.0;
				if (data.GetDouble(ref value))
				{
					if (_.Parent.ImportGroup == null)
					{
						translatedRingSizes.Add(GetSize(value));
					}
					else if (CSApplicationHelper.Application.Document.Graph.DisplayObjects().Any((CSBlock displayObject) => displayObject.ImportGroup.BlockIdentifiers.Contains(_.Parent.InstanceGuid)))
					{
						translatedRingSizes.Add(GetSize(value));
					}
				}
			}
		});
		return translatedRingSizes.OrderBy((UniversalRingSize _) => _.diameterMM).ToList();
	}

	public static UniversalRingSize GetSize(double radius)
	{
		return CSApplicationHelper.Application.RingSizeTranslator.GetSizeRadius(radius);
	}
}
