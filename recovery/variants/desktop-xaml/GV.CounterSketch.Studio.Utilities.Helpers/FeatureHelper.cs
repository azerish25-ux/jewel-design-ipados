using System.Collections.Generic;
using System.Linq;
using GV.CounterSketch.Studio.FeatureServiceReference;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class FeatureHelper
{
	private const string _securityKeyword = "G3mv1s10nK3yPr0f1l3Upd4t3r!";

	public static List<FeatureDTO> AvailableFeatures { get; set; }

	static FeatureHelper()
	{
		GetUserFeatures();
	}

	public static bool HasFeature(string feature)
	{
		if (UIHelper.CounterSketchOptions.UserTypeId >= 7)
		{
			GetUserFeatures();
		}
		if (AvailableFeatures != null)
		{
			return AvailableFeatures.Any((FeatureDTO _) => _.Name == feature);
		}
		return false;
	}

	public static bool HasExportFeature()
	{
		if (UIHelper.CounterSketchOptions.UserTypeId >= 7)
		{
			GetUserFeatures();
		}
		if (AvailableFeatures != null)
		{
			return AvailableFeatures.Any((FeatureDTO _) => _.FeatureId == 1);
		}
		return false;
	}

	private static void GetUserFeatures()
	{
		using FeatureServiceClient featureServiceClient = new FeatureServiceClient("BasicHttpsBinding_IFeatureService");
		AvailableFeatures = featureServiceClient.GetFeaturesForKey(UIHelper.CounterSketchOptions.KeyId, "G3mv1s10nK3yPr0f1l3Upd4t3r!").ToList();
	}
}
