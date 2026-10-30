using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;
using Microsoft.Win32;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class WpfPrintHelper
{
	private static readonly int documentHeight = 1056;

	private static readonly int documentWidth = 816;

	public static void Print(this FrameworkElement objectToPrint, bool saveOnly)
	{
		new PrintDialog();
		Mouse.OverrideCursor = Cursors.Wait;
		FixedDocument fixedDocument = new FixedDocument();
		try
		{
			objectToPrint.Width = documentWidth;
			objectToPrint.UpdateLayout();
			objectToPrint.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
			System.Windows.Size size = new System.Windows.Size(documentWidth, objectToPrint.DesiredSize.Height);
			objectToPrint.Measure(size);
			objectToPrint.Arrange(new Rect(size));
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(Convert.ToInt32((double)documentWidth * 3.125), Convert.ToInt32(objectToPrint.ActualHeight * 3.125), 300.0, 300.0, PixelFormats.Pbgra32);
			renderTargetBitmap.Render(objectToPrint);
			PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
			pngBitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
			Bitmap bitmap;
			using (MemoryStream stream = new MemoryStream())
			{
				pngBitmapEncoder.Save(stream);
				bitmap = new Bitmap(stream);
			}
			fixedDocument.DocumentPaginator.PageSize = new System.Windows.Size(documentWidth, documentHeight);
			int num = 0;
			int top = 0;
			int num2 = Convert.ToInt32((double)documentHeight * 3.125);
			while (num < bitmap.Height - num2)
			{
				num += num2;
				while (!(RowPixelDeviation(bitmap, num) < 1627500.0))
				{
					num--;
				}
				PageContent newPageContent = GeneratePage(bitmap, top, num, documentWidth, documentHeight);
				fixedDocument.Pages.Add(newPageContent);
				top = num;
			}
			PageContent newPageContent2 = GeneratePage(bitmap, top, bitmap.Height, documentWidth, documentHeight);
			fixedDocument.Pages.Add(newPageContent2);
		}
		finally
		{
			objectToPrint.Width = double.NaN;
			(WpfHelper.GetParentObject(objectToPrint) as FrameworkElement).InvalidateMeasure();
			(WpfHelper.GetParentObject(objectToPrint) as FrameworkElement).InvalidateArrange();
			(WpfHelper.GetParentObject(objectToPrint) as FrameworkElement).UpdateLayout();
			Mouse.OverrideCursor = null;
		}
		if (!saveOnly)
		{
			PrintPreview(fixedDocument, objectToPrint.TryFindParent<Window>());
			return;
		}
		SaveFileDialog saveFileDialog = new SaveFileDialog
		{
			FileName = string.Format("_{0}.xps", DateTime.Now.ToString("yyyyMMdd_hhss")),
			DefaultExt = "xps"
		};
		if (saveFileDialog.ShowDialog() != true)
		{
			return;
		}
		using XpsDocument xpsDocument = new XpsDocument(saveFileDialog.FileName, FileAccess.ReadWrite);
		XpsDocument.CreateXpsDocumentWriter(xpsDocument).Write(fixedDocument);
	}

	private static PageContent GeneratePage(Bitmap bitmap, int top, int bottom, double pageWidth, double pageHeight)
	{
		BitmapSource source = Imaging.CreateBitmapSourceFromHBitmap(bitmap.Clone(new Rectangle(0, top, bitmap.Width, bottom - top), System.Drawing.Imaging.PixelFormat.Format32bppArgb).GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bottom - top));
		System.Windows.Controls.Image image = new System.Windows.Controls.Image
		{
			Source = source,
			VerticalAlignment = VerticalAlignment.Top
		};
		image.Stretch = Stretch.Uniform;
		FixedPage fixedPage = new FixedPage
		{
			Width = pageWidth,
			Height = pageHeight
		};
		fixedPage.Children.Add(image);
		FixedPage.SetLeft(image, 20.0);
		FixedPage.SetTop(image, 20.0);
		image.Width = (double)documentWidth - 40.0;
		image.Height = (double)documentHeight - 40.0;
		return new PageContent
		{
			Child = fixedPage
		};
	}

	private static void PrintPreview(IDocumentPaginatorSource paginatorSource, Window window)
	{
		Window obj = new Window
		{
			Owner = (window ?? Application.Current.MainWindow.Owner),
			WindowStartupLocation = WindowStartupLocation.CenterOwner,
			Title = "Print Preview",
			Width = 600.0,
			Height = 800.0,
			ShowInTaskbar = false,
			Topmost = true,
			Resources = 
			{
				{
					(object)"EmptyScrollBarStyle",
					(object)new Style(typeof(ScrollBar))
				},
				{
					(object)"EmptyComboBoxStyle",
					(object)new Style(typeof(ComboBox))
				}
			}
		};
		DocumentViewer documentViewer = new DocumentViewer
		{
			Document = paginatorSource
		};
		ContentControl contentControl = documentViewer.FindChildren<ContentControl>().FirstOrDefault((ContentControl _) => _.Name == "PART_FindToolBarHost");
		if (contentControl != null)
		{
			contentControl.Visibility = Visibility.Collapsed;
		}
		obj.Content = documentViewer;
		documentViewer.FitToWidth();
		obj.ShowDialog();
	}

	private unsafe static double RowPixelDeviation(Bitmap bitmap, int rowNumber)
	{
		int num = 0;
		double num2 = 0.0;
		double num3 = 0.0;
		double result = 0.0;
		BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
		int stride = bitmapData.Stride;
		byte* ptr = (byte*)(void*)bitmapData.Scan0;
		ptr += stride * rowNumber;
		for (int i = 0; i < bitmap.Width; i++)
		{
			num++;
			byte blue = *ptr;
			byte green = ptr[1];
			byte red = ptr[3];
			int num4 = System.Drawing.Color.FromArgb(0, red, green, blue).ToArgb();
			num2 += (double)num4;
			double num5 = num2 / (double)num;
			num3 += Math.Pow((double)num4 - num5, 2.0);
			result = Math.Sqrt(num3 / (double)num);
			ptr += 3;
		}
		bitmap.UnlockBits(bitmapData);
		return result;
	}
}
