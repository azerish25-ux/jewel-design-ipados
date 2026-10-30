using System;
using System.Linq;
using System.Text;
using GV.CounterSketch.Studio.Utilities.Helpers;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.Utilities;

public class StullerAccountUtility
{
	public static void UpdateCustomerFromAccountNumber(string accountNumber)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		GlobalErrorHandler.WriteInfoToEventLog("Timing - Begin StullerAccountUtility.UpdateCustomerFromAccountNumber(" + accountNumber + ")", 2);
		if (UIHelper.IsInternetAccessible())
		{
			if (string.IsNullOrWhiteSpace(accountNumber))
			{
				GlobalErrorHandler.WriteErrorToEventLog("Error calling UpdateCustomerFromAccountNumber(): Missing Account Number!");
				return;
			}
			try
			{
				StullerAPIClient val = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)), "https://api.stuller.com/v2/", (Action<string>)null);
				try
				{
					CustomerResponse val2 = val.FetchCustomerResponse();
					Customer customer = val2.Customer;
					ShipToAccount val3 = ((val2 != null) ? val2.Customer.ShipToAccounts.FirstOrDefault((ShipToAccount account) => account.AccountNumber == UIHelper.CounterSketchOptions.ShipToAccountNumber) : null);
					if (val3 != null)
					{
						UIHelper.CounterSketchOptions.AllowFreezePriceForFreehandParts = customer.AllowFreezePriceForFreehandParts;
						UIHelper.CounterSketchOptions.HasStullerShowcaseMarkups = val2.Customer.HasShowcaseMarkUps;
						UIHelper.CounterSketchOptions.JewelerShowcaseURL = customer.JewelerShowcaseUrl;
						UIHelper.CounterSketchOptions.ShipToName = val3.CompanyName;
						UIHelper.CounterSketchOptions.ShipToAccountNumber = val3.AccountNumber;
						UIHelper.CounterSketchOptions.ShipToAddress1 = val3.AddressLine1;
						UIHelper.CounterSketchOptions.ShipToAddress2 = val3.AddressLine2;
						UIHelper.CounterSketchOptions.ShipToCity = val3.City;
						UIHelper.CounterSketchOptions.ShipToState = val3.State;
						UIHelper.CounterSketchOptions.ShipToZip = val3.Zip;
						UIHelper.CounterSketchOptions.ShipToCountry = val3.Country;
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				GlobalErrorHandler.WriteErrorToEventLog("Error calling UpdateCustomerFromAccountNumber():\n" + ex);
			}
		}
		GlobalErrorHandler.WriteInfoToEventLog("Timing - End StullerAccountUtility.UpdateCustomerFromAccountNumber(" + UIHelper.CounterSketchOptions.ShipToAccountNumber + ")", 2);
	}
}
