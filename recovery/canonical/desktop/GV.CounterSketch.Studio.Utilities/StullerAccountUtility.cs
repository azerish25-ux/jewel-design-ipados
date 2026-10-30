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
				using StullerAPIClient stullerAPIClient = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)));
				CustomerResponse customerResponse = stullerAPIClient.FetchCustomerResponse();
				Customer customer = customerResponse.Customer;
				ShipToAccount shipToAccount = customerResponse?.Customer.ShipToAccounts.FirstOrDefault((ShipToAccount account) => account.AccountNumber == UIHelper.CounterSketchOptions.ShipToAccountNumber);
				if (shipToAccount != null)
				{
					UIHelper.CounterSketchOptions.AllowFreezePriceForFreehandParts = customer.AllowFreezePriceForFreehandParts;
					UIHelper.CounterSketchOptions.HasStullerShowcaseMarkups = customerResponse.Customer.HasShowcaseMarkUps;
					UIHelper.CounterSketchOptions.JewelerShowcaseURL = customer.JewelerShowcaseUrl;
					UIHelper.CounterSketchOptions.ShipToName = shipToAccount.CompanyName;
					UIHelper.CounterSketchOptions.ShipToAccountNumber = shipToAccount.AccountNumber;
					UIHelper.CounterSketchOptions.ShipToAddress1 = shipToAccount.AddressLine1;
					UIHelper.CounterSketchOptions.ShipToAddress2 = shipToAccount.AddressLine2;
					UIHelper.CounterSketchOptions.ShipToCity = shipToAccount.City;
					UIHelper.CounterSketchOptions.ShipToState = shipToAccount.State;
					UIHelper.CounterSketchOptions.ShipToZip = shipToAccount.Zip;
					UIHelper.CounterSketchOptions.ShipToCountry = shipToAccount.Country;
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
