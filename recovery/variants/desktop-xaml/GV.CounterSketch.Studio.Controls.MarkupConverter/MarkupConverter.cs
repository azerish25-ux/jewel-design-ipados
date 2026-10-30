namespace GV.CounterSketch.Studio.Controls.MarkupConverter;

public class MarkupConverter : IMarkupConverter
{
	public string ConvertXamlToHtml(string xamlText)
	{
		return HtmlFromXamlConverter.ConvertXamlToHtml(xamlText, asFullDocument: false);
	}

	public string ConvertHtmlToXaml(string htmlText)
	{
		return HtmlToXamlConverter.ConvertHtmlToXaml(htmlText, asFlowDocument: true);
	}

	public string ConvertRtfToHtml(string rtfText)
	{
		return RtfToHtmlConverter.ConvertRtfToHtml(rtfText);
	}
}
