using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Localization;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class StullerStrings
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("Localization.StullerStrings", typeof(StullerStrings).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
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

	internal static string CamJobIsEmpty => ResourceManager.GetString("CamJobIsEmpty", resourceCulture);

	internal static string Casting => ResourceManager.GetString("Casting", resourceCulture);

	internal static string CompanyNameMustBeSpecifiedForOrderAction => ResourceManager.GetString("CompanyNameMustBeSpecifiedForOrderAction", resourceCulture);

	internal static string InProduction => ResourceManager.GetString("InProduction", resourceCulture);

	internal static string InvalidCustomerStoneValueForCamJobGemPrice => ResourceManager.GetString("InvalidCustomerStoneValueForCamJobGemPrice", resourceCulture);

	internal static string InvalidPriceForCamJobAssemblyPrice => ResourceManager.GetString("InvalidPriceForCamJobAssemblyPrice", resourceCulture);

	internal static string InvalidPriceForCamJobGemPrice => ResourceManager.GetString("InvalidPriceForCamJobGemPrice", resourceCulture);

	internal static string InvalidPriceForCamJobGemSettingPrice => ResourceManager.GetString("InvalidPriceForCamJobGemSettingPrice", resourceCulture);

	internal static string InvalidPriceForCamJobInsideEngravingPrice => ResourceManager.GetString("InvalidPriceForCamJobInsideEngravingPrice", resourceCulture);

	internal static string InvalidPriceForCamJobMetal => ResourceManager.GetString("InvalidPriceForCamJobMetal", resourceCulture);

	internal static string InvalidPriceForCamJobOutsideEngravingPrice => ResourceManager.GetString("InvalidPriceForCamJobOutsideEngravingPrice", resourceCulture);

	internal static string InvalidPriceForCamJobPrice => ResourceManager.GetString("InvalidPriceForCamJobPrice", resourceCulture);

	internal static string InvalidPriceForCamJobSpecialFinishPrice => ResourceManager.GetString("InvalidPriceForCamJobSpecialFinishPrice", resourceCulture);

	internal static string InvalidQuantity => ResourceManager.GetString("InvalidQuantity", resourceCulture);

	internal static string InvalidShipToAccountNumberForUser => ResourceManager.GetString("InvalidShipToAccountNumberForUser", resourceCulture);

	internal static string InvalidWeight => ResourceManager.GetString("InvalidWeight", resourceCulture);

	internal static string JobCompleted => ResourceManager.GetString("JobCompleted", resourceCulture);

	internal static string JobShipped => ResourceManager.GetString("JobShipped", resourceCulture);

	internal static string LengthOrWidthNotSpecified => ResourceManager.GetString("LengthOrWidthNotSpecified", resourceCulture);

	internal static string NoMetal => ResourceManager.GetString("NoMetal", resourceCulture);

	internal static string NotAuthorized => ResourceManager.GetString("NotAuthorized", resourceCulture);

	internal static string OrderPlaced => ResourceManager.GetString("OrderPlaced", resourceCulture);

	internal static string ProcessingModel => ResourceManager.GetString("ProcessingModel", resourceCulture);

	internal static string ProcessingOrder => ResourceManager.GetString("ProcessingOrder", resourceCulture);

	internal static string SerialNumber0IsNotAvailableForSale => ResourceManager.GetString("SerialNumber0IsNotAvailableForSale", resourceCulture);

	internal static string SetTypeNotSpecified => ResourceManager.GetString("SetTypeNotSpecified", resourceCulture);

	internal static string UserIsNotInRoleThatCanBeAuthenticated => ResourceManager.GetString("UserIsNotInRoleThatCanBeAuthenticated", resourceCulture);

	internal static string UserNameAndPasswordRequiredToAcessApi => ResourceManager.GetString("UserNameAndPasswordRequiredToAcessApi", resourceCulture);

	internal StullerStrings()
	{
	}
}
