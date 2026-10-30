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
		return true;
	}

	public static bool HasExportFeature()
	{
		return true;
	}

	private static void GetUserFeatures()
	{
		using FeatureServiceClient featureServiceClient = new FeatureServiceClient("BasicHttpsBinding_IFeatureService");
		AvailableFeatures = featureServiceClient.GetFeaturesForKey(UIHelper.CounterSketchOptions.KeyId, "G3mv1s10nK3yPr0f1l3Upd4t3r!").ToList();
	}
}
