using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.ViewModel;
using Localization;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class PunchoutHelper
{
	private static Guid _sessionId = Guid.NewGuid();

	private static WindowBrowser _windowBrowser;

	public static void CloseBrowser()
	{
		if (_windowBrowser != null)
		{
			_windowBrowser.Close();
			_windowBrowser = null;
		}
	}

	internal static void ShowDiamondShopper()
	{
		if (_windowBrowser != null)
		{
			_windowBrowser.Close();
		}
		string text = $"https://punchout-css-{_sessionId}-{UIHelper.CounterSketchOptions.MemberId}.jewelershowcase.com/countersketchstudio/largediamonds/";
		GlobalErrorHandler.WriteInfoToEventLog("Punchout URL: " + text);
		_windowBrowser = new WindowBrowser(new Uri(text), showShopperButtons: true)
		{
			Width = 1200.0,
			Height = 800.0,
			Title = Strings.FindYourStone,
			Owner = Application.Current.MainWindow
		};
		_windowBrowser.Closed += Browser_Closed;
		_windowBrowser.Show();
		_windowBrowser.BringIntoView();
		_windowBrowser.BringWindowToTop();
	}

	private static void Browser_Closed(object sender, EventArgs e)
	{
		_windowBrowser = null;
	}

	internal static void ShowGemstoneShopper()
	{
		if (_windowBrowser != null)
		{
			_windowBrowser.Close();
		}
		string text = $"https://punchout-css-{_sessionId}-{UIHelper.CounterSketchOptions.MemberId}.jewelershowcase.com/countersketchstudio/largegemstones/";
		GlobalErrorHandler.WriteInfoToEventLog("Punchout URL: " + text);
		_windowBrowser = new WindowBrowser(new Uri(text), showShopperButtons: true)
		{
			Width = 1200.0,
			Height = 800.0,
			Title = UntranslatedStrings.FindYourGem,
			Owner = Application.Current.MainWindow
		};
		_windowBrowser.Closed += Browser_Closed;
		_windowBrowser.Show();
		_windowBrowser.BringIntoView();
		_windowBrowser.BringWindowToTop();
	}

	internal static void Reset()
	{
		_sessionId = Guid.NewGuid();
	}

	internal static Diamond ConvertGemstoneToDiamond(GemStone stone)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		Diamond val = new Diamond
		{
			Active = stone.Active,
			AGTA = stone.AGTA,
			CaratWeight = stone.CaratWeight,
			CertificatePath = stone.CertificatePath,
			Certification = stone.Certification,
			CertificationNumber = stone.CertificationNumber,
			Clarity = stone.Clarity,
			Color = stone.Color,
			ColorCode = stone.ColorCode,
			ColorDescription = stone.ColorDescription,
			Comments = stone.Comments,
			Crown = stone.Crown,
			Cut = stone.Cut,
			Depth = stone.Depth,
			Description = stone.Description,
			Girdle = stone.Girdle,
			Height = (string.IsNullOrEmpty(stone.Height) ? 0m : decimal.Parse(stone.Height)),
			Images = stone.Images,
			LastUpdatedDate = stone.LastUpdatedDate,
			Length = (string.IsNullOrEmpty(stone.Length) ? 0m : decimal.Parse(stone.Length)),
			MMSize = stone.MMSize,
			Make = stone.Make,
			MatchedSetId = stone.MatchedSetId,
			NormalizedWeight = stone.NormalizedWeight,
			Pavillion = stone.Pavillion,
			Polish = stone.Polish,
			Price = stone.Price,
			PricePerCarat = stone.PricePerCarat,
			SerialNumber = stone.SerialNumber,
			Shape = stone.Shape,
			ShowcasePrice = stone.ShowcasePrice,
			ShowcasePricePerCarat = stone.ShowcasePricePerCarat,
			Size = stone.Size,
			StoneType = stone.StoneType,
			Symmetry = stone.Symmetry,
			Table = stone.Table,
			TotalDepth = stone.TotalDepth,
			Type = stone.Type,
			Videos = stone.Videos,
			WebAddress = stone.WebAddress,
			Width = (string.IsNullOrEmpty(stone.Width) ? 0m : decimal.Parse(stone.Width))
		};
		if (val.Shape == "ROUND" && val.Width == 0m)
		{
			val.Width = val.Length;
		}
		if (val.Shape == "EMERALD/OCTAGON")
		{
			val.Shape = "EMERALD CUT";
		}
		if (val.Shape == "ANTIQUE CUSHION")
		{
			val.Shape = "CUSHION";
		}
		return val;
	}

	internal static async Task RetrieveStoneTray()
	{
		StullerAPIClient proxy = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)), "https://api.stuller.com/v2/", (Action<string>)null);
		try
		{
			await Task.Run(delegate
			{
				CartResponse cart = proxy.GetCart((Guid?)_sessionId);
				if (cart != null)
				{
					List<Diamond> stonesInTray = new List<Diamond>();
					cart.CartItems?.ForEach(delegate(CartItemResponse i)
					{
						//IL_0001: Unknown result type (might be due to invalid IL or missing references)
						//IL_0007: Invalid comparison between Unknown and I4
						//IL_0081: Unknown result type (might be due to invalid IL or missing references)
						//IL_0087: Invalid comparison between Unknown and I4
						//IL_0009: Unknown result type (might be due to invalid IL or missing references)
						//IL_000e: Unknown result type (might be due to invalid IL or missing references)
						//IL_001a: Unknown result type (might be due to invalid IL or missing references)
						//IL_0026: Unknown result type (might be due to invalid IL or missing references)
						//IL_0046: Expected O, but got Unknown
						//IL_0089: Unknown result type (might be due to invalid IL or missing references)
						//IL_008e: Unknown result type (might be due to invalid IL or missing references)
						//IL_009a: Unknown result type (might be due to invalid IL or missing references)
						//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
						//IL_00c6: Expected O, but got Unknown
						if ((int)((CartItem)i).CartItemType == 3)
						{
							DiamondRequest val = new DiamondRequest
							{
								PageSize = 1,
								Page = 1,
								SerialNumbers = new List<int> { ((CartItem)i).SerialNumber.GetValueOrDefault() }
							};
							proxy.FetchDiamondResponse(val).Diamonds.ForEach(delegate(Diamond d)
							{
								stonesInTray.Add(d);
							});
						}
						if ((int)((CartItem)i).CartItemType == 4)
						{
							GemstoneRequest val2 = new GemstoneRequest
							{
								PageSize = 1,
								Page = 1,
								SerialNumbers = new List<int> { ((CartItem)i).SerialNumber.GetValueOrDefault() }
							};
							proxy.FetchGemstoneResponse(val2).GemStones.ForEach(delegate(GemStone g)
							{
								stonesInTray.Add(ConvertGemstoneToDiamond(g));
							});
						}
					});
					ViewModelLocator.Current.StoneTrayVM.AddStonesToTray(stonesInTray);
				}
			});
		}
		finally
		{
			if (proxy != null)
			{
				((IDisposable)proxy).Dispose();
			}
		}
	}

	internal static void RemoveStoneFromTray(Diamond diamond)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		StullerAPIClient val = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)), "https://api.stuller.com/v2/", (Action<string>)null);
		try
		{
			CartResponse cart = val.GetCart((Guid?)_sessionId);
			if (cart != null)
			{
				CartItemResponse val2 = cart.CartItems?.FirstOrDefault((CartItemResponse _) => ((CartItem)_).SerialNumber == diamond.SerialNumber);
				if (val2 != null)
				{
					val.DeleteCartItem(new DeleteCartItemRequest
					{
						CartItemId = val2.Id
					});
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
