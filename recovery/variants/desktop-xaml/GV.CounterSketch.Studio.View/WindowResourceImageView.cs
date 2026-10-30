using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.View;

public partial class WindowResourceImageView : Window, IComponentConnector
{
	public static readonly DependencyProperty HTMLDumpProperty = DependencyProperty.Register("HTMLDump", typeof(string), typeof(WindowResourceImageView));

	public static readonly DependencyProperty MethodDumpProperty = DependencyProperty.Register("MethodDump", typeof(string), typeof(WindowResourceImageView));

	public static readonly DependencyProperty ResourceDictionariesProperty = DependencyProperty.Register("ResourceDictionaries", typeof(ObservableCollection<string>), typeof(WindowResourceImageView));

	public static readonly DependencyProperty ResourceImagesProperty = DependencyProperty.Register("ResourceImages", typeof(ObservableCollection<KeyValuePair<string, string>>), typeof(WindowResourceImageView));

	public string HTMLDump
	{
		get
		{
			return (string)GetValue(HTMLDumpProperty);
		}
		set
		{
			SetValue(HTMLDumpProperty, value);
		}
	}

	public string MethodDump
	{
		get
		{
			return (string)GetValue(MethodDumpProperty);
		}
		set
		{
			SetValue(MethodDumpProperty, value);
		}
	}

	public ObservableCollection<string> ResourceDictionaries
	{
		get
		{
			return (ObservableCollection<string>)GetValue(ResourceDictionariesProperty);
		}
		set
		{
			SetValue(ResourceDictionariesProperty, value);
		}
	}

	public ObservableCollection<KeyValuePair<string, string>> ResourceImages
	{
		get
		{
			return (ObservableCollection<KeyValuePair<string, string>>)GetValue(ResourceImagesProperty);
		}
		set
		{
			SetValue(ResourceImagesProperty, value);
		}
	}

	public WindowResourceImageView()
	{
		InitializeComponent();
		ResourceDictionaries = new ObservableCollection<string> { "DesignStackSliderIcons", "None" };
		base.ContentRendered += WindowResourceImageViewContentRendered;
	}

	private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.AddedItems == null || e.AddedItems.Count <= 0)
		{
			return;
		}
		ResourceImages = new ObservableCollection<KeyValuePair<string, string>>();
		object obj = e.AddedItems[0];
		if (obj != null && (string)obj != "None")
		{
			foreach (object key in new ResourceDictionary
			{
				Source = new Uri($"/CounterSketch Studio;component/ResourceDictionaries/{obj}.xaml", UriKind.RelativeOrAbsolute)
			}.Keys)
			{
				string text = key.ToString();
				if (Application.Current.TryFindResource(text) is DrawingBrush)
				{
					string text2 = text.Replace("CSIcon", "");
					text2 = text2.Replace("__old_", " [old]");
					text2 = text2.Replace("_", " : ");
					ResourceImages.Add(new KeyValuePair<string, string>(text, text2));
				}
			}
		}
		base.Dispatcher.Invoke(delegate
		{
			resourceList.UpdateLayout();
		});
		HTMLDump = GetHTMLDumpString(resourceList).ToString();
		MethodDump = GetMethodDumpString(resourceList).ToString();
	}

	private void CopyExecuted(object sender, ExecutedRoutedEventArgs e)
	{
		if (sender is ListBox listBoxSender)
		{
			StringBuilder hTMLDumpString = GetHTMLDumpString(listBoxSender);
			string htmlDataForClipboard = GetHtmlDataForClipboard(hTMLDumpString.ToString());
			Clipboard.Clear();
			DataObject dataObject = new DataObject();
			dataObject.SetData(DataFormats.Html, htmlDataForClipboard);
			dataObject.SetData(DataFormats.Text, hTMLDumpString.ToString());
			Clipboard.SetDataObject(dataObject);
		}
	}

	private static string GetHtmlDataForClipboard(string html)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Format:HTML Format\r\nVersion:1.0\r\nStartHTML:<<<<<<<1\r\nEndHTML:<<<<<<<2\r\nStartFragment:<<<<<<<3\r\nEndFragment:<<<<<<<4\r\nStartSelection:<<<<<<<3\r\nEndSelection:<<<<<<<3");
		int length = stringBuilder.Length;
		stringBuilder.Append("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\"><!--StartFragment-->");
		int length2 = stringBuilder.Length;
		stringBuilder.Append(html);
		int length3 = stringBuilder.Length;
		stringBuilder.Append("<!--EndFragment-->");
		int length4 = stringBuilder.Length;
		stringBuilder.Replace("<<<<<<<1", $"{length,8}");
		stringBuilder.Replace("<<<<<<<2", $"{length4,8}");
		stringBuilder.Replace("<<<<<<<3", $"{length2,8}");
		stringBuilder.Replace("<<<<<<<4", $"{length3,8}");
		return stringBuilder.ToString();
	}

	private static StringBuilder GetHTMLDumpString(ListBox listBoxSender)
	{
		IOrderedEnumerable<Image> source = from item in listBoxSender.FindChildren<Image>()
			where item.Name == "resourceImage"
			orderby item.Tag
			select item;
		StringBuilder htmlString = new StringBuilder();
		StringBuilder methodBuilderString = new StringBuilder();
		source.ForEach(delegate(Image image)
		{
			string arg = ImageHelper.ConvertBitmapToBase64String(ImageHelper.ConvertRenderTargetBitmapToBitmap(ImageHelper.CreateBitmapFromFrameworkElement(image)));
			htmlString.AppendLine($"<img src=\"data:image/gif;base64,{arg}\" width=\"50\" height=\"60\" >     {image.Tag}<br/>");
			methodBuilderString.Append(string.Format("CheckSliderIcon(uiCatagoryName, \"{0}\", uiControlName, \"{1}\", ref path);", image.Tag.ToString().Substring(0, image.Tag.ToString().IndexOf(":")).Trim(), image.Tag.ToString().Substring(image.Tag.ToString().IndexOf(":") + 1).Trim()));
		});
		return htmlString;
	}

	private static StringBuilder GetMethodDumpString(ListBox listBoxSender)
	{
		IOrderedEnumerable<Image> source = from item in listBoxSender.FindChildren<Image>()
			where item.Name == "resourceImage"
			orderby item.Tag.ToString().Substring(0, item.Tag.ToString().IndexOf(":")).Trim()
				.Length descending, item.Tag.ToString().Substring(item.Tag.ToString().IndexOf(":") + 1).Trim()
				.Length descending
			select item;
		StringBuilder methodBuilderString = new StringBuilder();
		source.ForEach(delegate(Image image)
		{
			methodBuilderString.AppendLine(string.Format("CheckSliderIcon(uiCatagoryName, \"{0}\", uiControlName, \"{1}\", ref path);", image.Tag.ToString().Substring(0, image.Tag.ToString().IndexOf(":")).Trim(), image.Tag.ToString().Substring(image.Tag.ToString().IndexOf(":") + 1).Trim()));
		});
		return methodBuilderString;
	}

	private void WindowResourceImageViewContentRendered(object sender, EventArgs e)
	{
		comboSelectedResourceDictionary.SelectedIndex = 0;
	}
}
