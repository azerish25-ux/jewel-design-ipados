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
		if (((ViewModelBase)this).IsInDesignMode)
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		GlobalErrorHandler.WriteMethodInfo("PopulateOrderHistory");
		using CSApplicationDAL cSApplicationDAL = new CSApplicationDAL();
		if (!UIHelper.IsInternetAccessible())
		{
			return;
		}
		ObservableCollection<CSOrder> orders = cSApplicationDAL.GetOrders();
		StullerAPIClient val = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)), "https://api.stuller.com/v2/", (Action<string>)null);
		try
		{
			CamJobStatusRequest val2 = new CamJobStatusRequest();
			val2.CamJobIds = orders.Select((CSOrder _) => _.CamRequestId).ToList();
			CamJobStatusResponse orderStatus = val.FetchCamJobStatus(val2);
			string sql = string.Empty;
			orders.ToList().ForEach(delegate(CSOrder _)
			{
				CamJobStatus val3 = orderStatus.CamJobStatuses.FirstOrDefault((CamJobStatus c) => c.CamJobID == _.CamRequestId);
				if (val3 != null)
				{
					sql = string.Concat(sql, "UPDATE Quote SET QuoteStatus = '", TranslationHelper.GetTranslatedStullerString(val3.Status), string.Empty, "|", val3.ScheduledShipDate, string.Empty, "' WHERE CAMRequestId = ", _.CamRequestId, ";");
				}
			});
			cSApplicationDAL.ExecuteNonQuery(sql);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		OrderHistoryCollection = cSApplicationDAL.GetOrders();
	}

	private void CloseWindow()
	{
		Messenger.Default.Send<string, WindowOrderHistoryView>("close");
	}

	private void InitializeCommands()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		CommandOpenDesign = new RelayCommand<string>((Action<string>)OpenDesign, false);
		CommandCloseWindow = new RelayCommand((Action)CloseWindow, false);
		CommandResubmitDesign = new RelayCommand<int>((Action<int>)ResubmitDesign, false);
		CommandViewPricing = new RelayCommand<CSOrder>((Action<CSOrder>)ViewPricing, false);
		CommandReorder = new RelayCommand((Action)Reorder, false);
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
