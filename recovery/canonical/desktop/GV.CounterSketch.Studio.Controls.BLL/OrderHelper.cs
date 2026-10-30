using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.Controls.BLL;

public class OrderHelper
{
	public static void ClearOrderInformationFromDesignUserData()
	{
		UIHelper.CoreDataService.DocumentUserDataAddValue("OrderInfo", "OrderSummaryViewModel", string.Empty);
		UIHelper.CoreDataService.DocumentUserDataAddValue("OrderInfo", "GemReportViewModel", string.Empty);
	}

	public static bool DeserializeOrderInformationFromDesignUserData()
	{
		UIHelper.IsOrderDeserializing = true;
		OrderSummaryViewModel orderSummaryViewModel = null;
		GemReportViewModel gemReportViewModel = null;
		try
		{
			string text = UIHelper.CoreDataService.DocumentUserDataFindValue("OrderInfo", "OrderSummaryViewModel");
			if (!string.IsNullOrEmpty(text))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(OrderSummaryViewModel));
				if (text != null)
				{
					StringReader textReader = new StringReader(text);
					try
					{
						orderSummaryViewModel = xmlSerializer.Deserialize(textReader) as OrderSummaryViewModel;
					}
					catch (Exception)
					{
						DebuggingHelper.PrintTimedOutput("Error Trying to Deserialize OrderSummary VM; Trying XML for the data...");
						textReader = new StringReader(text);
						XElement xElement = XDocument.Parse(textReader.ReadToEnd()).Element("OrderSummaryViewModel");
						orderSummaryViewModel = new OrderSummaryViewModel();
						orderSummaryViewModel.AdditionalDesignNotes = xElement.Descendants("AdditionalDesignNotes").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerAddress1 = xElement.Descendants("CustomerAddress1").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerAddress2 = xElement.Descendants("CustomerAddress2").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerCity = xElement.Descendants("CustomerCity").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerCountry = xElement.Descendants("CustomerCountry").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerEmail = xElement.Descendants("CustomerEmail").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerFirstName = xElement.Descendants("CustomerFirstName").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerLastName = xElement.Descendants("CustomerLastName").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerMiddleInitial = xElement.Descendants("CustomerMiddleInitial").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerPhone1 = xElement.Descendants("CustomerPhone1").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerPhone2 = xElement.Descendants("CustomerPhone2").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerPostalCode = xElement.Descendants("CustomerPostalCode").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerReferenceNumber = xElement.Descendants("CustomerReferenceNumber").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerState = xElement.Descendants("CustomerState").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerReferenceNumber = xElement.Descendants("CustomerReferenceNumber").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerState = xElement.Descendants("CustomerState").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerSuffix = xElement.Descendants("CustomerSuffix").FirstOrDefault().Value;
						orderSummaryViewModel.CustomerTitle = xElement.Descendants("CustomerTitle").FirstOrDefault().Value;
						orderSummaryViewModel.EngravingMessage = xElement.Descendants("EngravingMessage").FirstOrDefault().Value;
						orderSummaryViewModel.IsCustomEngravingSelected = Convert.ToBoolean(xElement.Descendants("IsCustomEngravingSelected").FirstOrDefault().Value);
						orderSummaryViewModel.IsEngagementOptionSelected = Convert.ToBoolean(xElement.Descendants("IsEngagementOptionSelected").FirstOrDefault().Value);
						orderSummaryViewModel.OrderReference = xElement.Descendants("OrderReference").FirstOrDefault().Value;
						orderSummaryViewModel.PrimaryMetal = xElement.Descendants("PrimaryMetal").FirstOrDefault().Value;
						orderSummaryViewModel.QuoteEMail = xElement.Descendants("QuoteEMail").FirstOrDefault().Value;
						orderSummaryViewModel.QuotePhone = xElement.Descendants("QuotePhone").FirstOrDefault().Value;
						orderSummaryViewModel.QuotedDesignPath = xElement.Descendants("QuotedDesignPath").FirstOrDefault().Value;
						orderSummaryViewModel.QuotedDesignPath = xElement.Descendants("QuotedDesignPath").FirstOrDefault().Value;
						orderSummaryViewModel.RequestForQuoteFileName = xElement.Descendants("RequestForQuoteFileName").FirstOrDefault().Value;
						orderSummaryViewModel.SecondaryMetal = xElement.Descendants("SecondaryMetal").FirstOrDefault().Value;
						orderSummaryViewModel.SelectedEngravingFont = xElement.Descendants("SelectedEngravingFont").FirstOrDefault().Value;
						if (!string.IsNullOrWhiteSpace(xElement.Descendants("QuotedPrice").FirstOrDefault().Value))
						{
							orderSummaryViewModel.QuotedPrice = Convert.ToDouble(xElement.Descendants("QuotedPrice").FirstOrDefault().Value);
						}
						if (!string.IsNullOrWhiteSpace(xElement.Descendants("SelectedMoreThanARing").FirstOrDefault().Value))
						{
							orderSummaryViewModel.SelectedMoreThanARing = Convert.ToBoolean(xElement.Descendants("SelectedMoreThanARing").FirstOrDefault().Value);
						}
						if (!string.IsNullOrWhiteSpace(xElement.Descendants("SelectedMoreThanARingDigitalOnly").FirstOrDefault().Value))
						{
							orderSummaryViewModel.SelectedMoreThanARingDigitalOnly = Convert.ToBoolean(xElement.Descendants("SelectedMoreThanARingDigitalOnly").FirstOrDefault().Value);
						}
						if (!string.IsNullOrWhiteSpace(xElement.Descendants("ShowEngagementOptions").FirstOrDefault().Value))
						{
							orderSummaryViewModel.ShowEngagementOptions = Convert.ToBoolean(xElement.Descendants("ShowEngagementOptions").FirstOrDefault().Value);
						}
						if (!string.IsNullOrWhiteSpace(xElement.Descendants("ShowEngagementSendOptions").FirstOrDefault().Value))
						{
							orderSummaryViewModel.ShowEngagementSendOptions = Convert.ToBoolean(xElement.Descendants("ShowEngagementSendOptions").FirstOrDefault().Value);
						}
						if (!string.IsNullOrWhiteSpace(xElement.Descendants("ShowEngravingOptions").FirstOrDefault().Value))
						{
							orderSummaryViewModel.ShowEngravingOptions = Convert.ToBoolean(xElement.Descendants("ShowEngravingOptions").FirstOrDefault().Value);
						}
						if (!string.IsNullOrWhiteSpace(xElement.Descendants("SelectedEngraving").FirstOrDefault().Value))
						{
							orderSummaryViewModel.SelectedEngraving = (CSEnumEngraving)Enum.Parse(typeof(CSEnumEngraving), xElement.Descendants("SelectedEngraving").FirstOrDefault().Value, ignoreCase: true);
						}
						if (!string.IsNullOrWhiteSpace(xElement.Descendants("SelectedEngravingFillColor").FirstOrDefault().Value))
						{
							orderSummaryViewModel.SelectedEngravingFillColor = (CSEnumEngravingFillColor)Enum.Parse(typeof(CSEnumEngravingFillColor), xElement.Descendants("SelectedEngravingFillColor").FirstOrDefault().Value, ignoreCase: true);
						}
						if (!string.IsNullOrWhiteSpace(xElement.Descendants("SelectedEngravingFontSize").FirstOrDefault().Value))
						{
							orderSummaryViewModel.SelectedEngravingFontSize = (CSEnumEngravingFontsSize)Enum.Parse(typeof(CSEnumEngravingFontsSize), xElement.Descendants("SelectedEngravingFontSize").FirstOrDefault().Value, ignoreCase: true);
						}
						if (!string.IsNullOrWhiteSpace(xElement.Descendants("SelectedFinishing").FirstOrDefault().Value))
						{
							orderSummaryViewModel.SelectedFinishing = (CSEnumFinishing)Enum.Parse(typeof(CSEnumFinishing), xElement.Descendants("SelectedFinishing").FirstOrDefault().Value, ignoreCase: true);
						}
						if (!string.IsNullOrWhiteSpace(xElement.Descendants("SelectedHallmarking").FirstOrDefault().Value))
						{
							orderSummaryViewModel.SelectedHallmarking = (CSEnumHallmarking)Enum.Parse(typeof(CSEnumHallmarking), xElement.Descendants("SelectedHallmarking").FirstOrDefault().Value, ignoreCase: true);
						}
					}
				}
				string text2 = UIHelper.CoreDataService.DocumentUserDataFindValue("OrderInfo", "GemReportViewModel");
				XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(GemReportViewModel));
				if (text2 != null)
				{
					StringReader textReader2 = new StringReader(text2);
					gemReportViewModel = xmlSerializer2.Deserialize(textReader2) as GemReportViewModel;
				}
				if (orderSummaryViewModel != null && gemReportViewModel != null)
				{
					ViewModelLocator.Current.OrderSummaryVM.UpdateFromUserData(orderSummaryViewModel);
					ViewModelLocator.Current.GemReportVM.UpdateFromUserData(gemReportViewModel);
					return true;
				}
			}
		}
		catch (Exception)
		{
			return false;
		}
		finally
		{
			UIHelper.IsOrderDeserializing = false;
		}
		return false;
	}

	public static void SerializeOrderInformationIntoDesignUserData()
	{
		OrderSummaryViewModel orderSummaryVM = ViewModelLocator.Current.OrderSummaryVM;
		GemReportViewModel gemReportVM = ViewModelLocator.Current.GemReportVM;
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(OrderSummaryViewModel));
			StringWriter stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, orderSummaryVM);
			UIHelper.CoreDataService.DocumentUserDataAddValue("OrderInfo", "OrderSummaryViewModel", stringWriter.ToString());
			XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(GemReportViewModel));
			stringWriter = new StringWriter();
			xmlSerializer2.Serialize(stringWriter, gemReportVM);
			UIHelper.CoreDataService.DocumentUserDataAddValue("OrderInfo", "GemReportViewModel", stringWriter.ToString());
		}
		catch (Exception)
		{
		}
	}
}
