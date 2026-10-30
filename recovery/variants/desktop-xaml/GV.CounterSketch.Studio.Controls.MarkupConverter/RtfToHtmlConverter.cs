using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GV.CounterSketch.Studio.Controls.MarkupConverter;

public static class RtfToHtmlConverter
{
	private const string FlowDocumentFormat = "<FlowDocument>{0}</FlowDocument>";

	public static string ConvertRtfToHtml(string rtfText)
	{
		return HtmlFromXamlConverter.ConvertXamlToHtml($"<FlowDocument>{ConvertRtfToXaml(rtfText)}</FlowDocument>", asFullDocument: false);
	}

	private static string ConvertRtfToXaml(string rtfText)
	{
		RichTextBox richTextBox = new RichTextBox();
		if (string.IsNullOrEmpty(rtfText))
		{
			return "";
		}
		TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using StreamWriter streamWriter = new StreamWriter(memoryStream);
			streamWriter.Write(rtfText);
			streamWriter.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			textRange.Load(memoryStream, DataFormats.Rtf);
		}
		using MemoryStream memoryStream2 = new MemoryStream();
		textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
		textRange.Save(memoryStream2, DataFormats.Xaml);
		memoryStream2.Seek(0L, SeekOrigin.Begin);
		using StreamReader streamReader = new StreamReader(memoryStream2);
		return streamReader.ReadToEnd();
	}
}
