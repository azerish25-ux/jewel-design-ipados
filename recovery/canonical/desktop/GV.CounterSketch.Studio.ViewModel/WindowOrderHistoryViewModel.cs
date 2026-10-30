using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using GV.CounterSketch.Studio.Controls.BLL;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.StullerOrderServiceReference;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Localization;
using Stuller.Web.API.v2.Models;
using Stuller.Web.API.v2.Models.Cam;

namespace GV.CounterSketch.Studio.ViewModel;

public class WindowOrderHistoryViewModel : ViewModelBase
{
	public RelayCommand<int> CommandResubmitDesign { get; set; }

	public RelayCommand CommandReorder { get; set; }

	public RelayCommand CommandCloseWindow { get; set; }

	public RelayCommand<string> CommandOpenDesign { get; set; }

	public ObservableCollection<CSOrder> OrderHistoryCollection { get; set; }

	public RelayCommand<CSOrder> CommandViewPricing { get; set; }

	public WindowOrderHistoryViewModel()
	{
		if (base.IsInDesignMode)
		{
			PopulateTestOrderHistory();
		}
		else
		{
			InitializeCommands();
		}
	}

	public void PopulateOrderHistory()
	{
		GlobalErrorHandler.WriteMethodInfo("PopulateOrderHistory");
		using CSApplicationDAL cSApplicationDAL = new CSApplicationDAL();
		if (!UIHelper.IsInternetAccessible())
		{
			return;
		}
		ObservableCollection<CSOrder> orders = cSApplicationDAL.GetOrders();
		using (StullerAPIClient stullerAPIClient = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password))))
		{
			CamJobStatusRequest camJobStatusRequest = new CamJobStatusRequest();
			camJobStatusRequest.CamJobIds = orders.Select((CSOrder _) => _.CamRequestId).ToList();
			CamJobStatusResponse orderStatus = stullerAPIClient.FetchCamJobStatus(camJobStatusRequest);
			string sql = string.Empty;
			orders.ToList().ForEach(delegate(CSOrder _)
			{
				CamJobStatus camJobStatus = orderStatus.CamJobStatuses.FirstOrDefault((CamJobStatus c) => c.CamJobID == _.CamRequestId);
				if (camJobStatus != null)
				{
					sql = string.Concat(sql, "UPDATE Quote SET QuoteStatus = '", TranslationHelper.GetTranslatedStullerString(camJobStatus.Status), string.Empty, "|", camJobStatus.ScheduledShipDate, string.Empty, "' WHERE CAMRequestId = ", _.CamRequestId, ";");
				}
			});
			cSApplicationDAL.ExecuteNonQuery(sql);
		}
		OrderHistoryCollection = cSApplicationDAL.GetOrders();
	}

	private void CloseWindow()
	{
		Messenger.Default.Send<string, WindowOrderHistoryView>("close");
	}

	private void InitializeCommands()
	{
		CommandOpenDesign = new RelayCommand<string>(OpenDesign);
		CommandCloseWindow = new RelayCommand(CloseWindow);
		CommandResubmitDesign = new RelayCommand<int>(ResubmitDesign);
		CommandViewPricing = new RelayCommand<CSOrder>(ViewPricing);
		CommandReorder = new RelayCommand(Reorder);
	}

	private void Reorder()
	{
		Process.Start("https://www.stuller.com/cadcamservices/cadcamprojects/");
	}

	private void OpenDesign(string designPath)
	{
		if (Application.Current.MainWindow is MainWindow && File.Exists(designPath))
		{
			ProjectManagementHelper.ImportBaseDesignFromDiskToActiveProjectAndOpenDesign(designPath);
		}
	}

	private void PopulateTestOrderHistory()
	{
		List<CSOrder> list = new List<CSOrder>();
		list.Add(new CSOrder
		{
			CamRequestId = 1,
			ContactEMail = "me@nowhere.org",
			ContactPhone = "123-123-1234",
			CustomerAddress1 = "address1",
			CustomerAddress2 = "suite 3",
			CustomerCity = "city",
			CustomerState = "state",
			CustomerZip = "zip",
			CustomerFirstName = "bill",
			CustomerLastName = "bones",
			FirstName = "jjj",
			LastName = "kkkk",
			EMail = "email2@emails.com",
			QuoteShipDate = "[1/1/2016 11:45AM]",
			QuoteStatus = "Order Placed"
		});
		list.Add(new CSOrder
		{
			CamRequestId = 1,
			ContactEMail = "me@nowhere.org",
			ContactPhone = "123-123-1234",
			CustomerAddress1 = "address1",
			CustomerAddress2 = "suite 3",
			CustomerCity = "city",
			CustomerState = "state",
			CustomerZip = "zip",
			CustomerFirstName = "bill",
			CustomerLastName = "bones",
			FirstName = "jjj",
			LastName = "kkkk",
			EMail = "email2@emails.com",
			QuoteShipDate = "[1/1/2016 11:45AM]",
			QuoteStatus = "Order Placed"
		});
		list.Add(new CSOrder
		{
			CamRequestId = 1,
			ContactEMail = "me@nowhere.org",
			ContactPhone = "123-123-1234",
			CustomerAddress1 = "address1",
			CustomerAddress2 = "suite 3",
			CustomerCity = "city",
			CustomerState = "state",
			CustomerZip = "zip",
			CustomerFirstName = "bill",
			CustomerLastName = "bones",
			FirstName = "jjj",
			LastName = "kkkk",
			EMail = "email2@emails.com"
		});
		list.Add(new CSOrder
		{
			CamRequestId = 1,
			ContactEMail = "me@nowhere.org",
			ContactPhone = "123-123-1234",
			CustomerAddress1 = "address1",
			CustomerAddress2 = "suite 3",
			CustomerCity = "city",
			CustomerState = "state",
			CustomerZip = "zip",
			CustomerFirstName = "bill",
			CustomerLastName = "bones",
			FirstName = "jjj",
			LastName = "kkkk",
			EMail = "email2@emails.com"
		});
		list.Add(new CSOrder
		{
			CamRequestId = 1,
			ContactEMail = "me@nowhere.org",
			ContactPhone = "123-123-1234",
			CustomerAddress1 = "address1",
			CustomerAddress2 = "suite 3",
			CustomerCity = "city",
			CustomerState = "state",
			CustomerZip = "zip",
			CustomerFirstName = "bill",
			CustomerLastName = "bones",
			FirstName = "jjj",
			LastName = "kkkk",
			EMail = "email2@emails.com"
		});
		list.Add(new CSOrder
		{
			CamRequestId = 1,
			ContactEMail = "me@nowhere.org",
			ContactPhone = "123-123-1234",
			CustomerAddress1 = "address1",
			CustomerAddress2 = "suite 3",
			CustomerCity = "city",
			CustomerState = "state",
			CustomerZip = "zip",
			CustomerFirstName = "bill",
			CustomerLastName = "bones",
			FirstName = "jjj",
			LastName = "kkkk",
			EMail = "email2@emails.com"
		});
		OrderHistoryCollection = new ObservableCollection<CSOrder>(list);
	}

	private void ResubmitDesign(int camRequestId)
	{
		GlobalErrorHandler.WriteMethodInfo("ResubmitDesign");
		UIHelper.ShowWaitWindow(waitVisible: true, "Uploading Design File, Please Wait.");
		try
		{
			CSOrder cSOrder = OrderHistoryCollection.FirstOrDefault((CSOrder _) => _.CamRequestId == camRequestId);
			if (cSOrder == null)
			{
				return;
			}
			byte[] array = null;
			if (File.Exists(cSOrder.QuotedDesignPath))
			{
				using (FileStream fileStream = new FileStream(cSOrder.QuotedDesignPath, FileMode.Open, FileAccess.Read))
				{
					array = new byte[fileStream.Length];
					fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
					fileStream.Close();
				}
				using StullerOrderServiceClient stullerOrderServiceClient = new StullerOrderServiceClient("BasicHttpBinding_IStullerOrderService");
				stullerOrderServiceClient.SendCSGFileForOrder(array, Path.GetFileName(cSOrder.QuotedDesignPath));
				return;
			}
		}
		finally
		{
			UIHelper.ShowWaitWindow(waitVisible: false);
		}
	}

	private void ViewPricing(CSOrder orderObject)
	{
		orderObject.IsViewingPricingImage = true;
	}
}
