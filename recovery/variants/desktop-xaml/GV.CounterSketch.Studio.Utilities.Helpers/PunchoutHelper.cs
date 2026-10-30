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
		Diamond diamond = new Diamond
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
		if (diamond.Shape == "ROUND" && diamond.Width == 0m)
		{
			diamond.Width = diamond.Length;
		}
		if (diamond.Shape == "EMERALD/OCTAGON")
		{
			diamond.Shape = "EMERALD CUT";
		}
		if (diamond.Shape == "ANTIQUE CUSHION")
		{
			diamond.Shape = "CUSHION";
		}
		return diamond;
	}

	internal static async Task RetrieveStoneTray()
	{
		StullerAPIClient proxy = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)));
		try
		{
			await Task.Run(delegate
			{
				CartResponse cart = proxy.GetCart(_sessionId);
				if (cart != null)
				{
					List<Diamond> stonesInTray = new List<Diamond>();
					cart.CartItems?.ForEach(delegate(CartItemResponse i)
					{
						if (i.CartItemType == CartItemType.LargeDiamond)
						{
							DiamondRequest request = new DiamondRequest
							{
								PageSize = 1,
								Page = 1,
								SerialNumbers = new List<int> { i.SerialNumber.GetValueOrDefault() }
							};
							proxy.FetchDiamondResponse(request).Diamonds.ForEach(delegate(Diamond d)
							{
								stonesInTray.Add(d);
							});
						}
						if (i.CartItemType == CartItemType.LargeGemstone)
						{
							GemstoneRequest request2 = new GemstoneRequest
							{
								PageSize = 1,
								Page = 1,
								SerialNumbers = new List<int> { i.SerialNumber.GetValueOrDefault() }
							};
							proxy.FetchGemstoneResponse(request2).GemStones.ForEach(delegate(GemStone g)
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
		using StullerAPIClient stullerAPIClient = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)));
		CartResponse cart = stullerAPIClient.GetCart(_sessionId);
		if (cart != null)
		{
			CartItemResponse cartItemResponse = cart.CartItems?.FirstOrDefault((CartItemResponse _) => _.SerialNumber == diamond.SerialNumber);
			if (cartItemResponse != null)
			{
				stullerAPIClient.DeleteCartItem(new DeleteCartItemRequest
				{
					CartItemId = cartItemResponse.Id
				});
			}
		}
	}
}
