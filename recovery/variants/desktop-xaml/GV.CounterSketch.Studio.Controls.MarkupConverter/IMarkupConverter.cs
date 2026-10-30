namespace GV.CounterSketch.Studio.Controls.MarkupConverter;

public interface IMarkupConverter
{
	string ConvertHtmlToXaml(string htmlText);

	string ConvertRtfToHtml(string rtfText);

	string ConvertXamlToHtml(string xamlText);
}
