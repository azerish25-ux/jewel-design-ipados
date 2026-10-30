using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Localization;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class UntranslatedStrings
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("Localization.UntranslatedStrings", typeof(UntranslatedStrings).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string AddFile => ResourceManager.GetString("AddFile", resourceCulture);

	public static string CAMRequest => ResourceManager.GetString("CAMRequest", resourceCulture);

	public static string ClearAll => ResourceManager.GetString("ClearAll", resourceCulture);

	public static string ControlRangeExpandedWarning => ResourceManager.GetString("ControlRangeExpandedWarning", resourceCulture);

	public static string DesignRangesExpandedWarning1 => ResourceManager.GetString("DesignRangesExpandedWarning1", resourceCulture);

	public static string DesignRangesExpandedWarning2 => ResourceManager.GetString("DesignRangesExpandedWarning2", resourceCulture);

	public static string Diamond => ResourceManager.GetString("Diamond", resourceCulture);

	public static string DoYouWantToSaveTheCurrentDesign => ResourceManager.GetString("DoYouWantToSaveTheCurrentDesign", resourceCulture);

	public static string FindYourGem => ResourceManager.GetString("FindYourGem", resourceCulture);

	public static string First => ResourceManager.GetString("First", resourceCulture);

	public static string Last => ResourceManager.GetString("Last", resourceCulture);

	public static string MountingPortfolioStorage => ResourceManager.GetString("MountingPortfolioStorage", resourceCulture);

	public static string Next => ResourceManager.GetString("Next", resourceCulture);

	public static string Open => ResourceManager.GetString("Open", resourceCulture);

	public static string Previous => ResourceManager.GetString("Previous", resourceCulture);

	public static string ReorderDesign => ResourceManager.GetString("ReorderDesign", resourceCulture);

	public static string ResendMissingDesignToCAM => ResourceManager.GetString("ResendMissingDesignToCAM", resourceCulture);

	public static string SaveAs => ResourceManager.GetString("SaveAs", resourceCulture);

	public static string SoftwareAsAServiceAgreement => ResourceManager.GetString("SoftwareAsAServiceAgreement", resourceCulture);

	public static string StullerShowcaseNotSetup => ResourceManager.GetString("StullerShowcaseNotSetup", resourceCulture);

	public static string SupportingDocumentNote => ResourceManager.GetString("SupportingDocumentNote", resourceCulture);

	public static string SupportingDocuments => ResourceManager.GetString("SupportingDocuments", resourceCulture);

	public static string ViewPricing => ResourceManager.GetString("ViewPricing", resourceCulture);

	internal UntranslatedStrings()
	{
	}
}
