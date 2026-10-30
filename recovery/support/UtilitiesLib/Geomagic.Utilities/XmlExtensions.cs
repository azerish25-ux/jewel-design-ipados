using System.Xml;

namespace Geomagic.Utilities;

public static class XmlExtensions
{
	public static string Escape(this XmlWriter Writer, string Str)
	{
		string text = string.Copy(Str);
		text = text.Replace("&", "&amp;");
		text = text.Replace("<", "&lt;");
		text = text.Replace(">", "&gt;");
		text = text.Replace("'", "&apos;");
		return text.Replace("\"", "&quot;");
	}

	public static string Unescape(this XmlWriter Writer, string Str)
	{
		string text = string.Copy(Str);
		text = text.Replace("&amp;", "&");
		text = text.Replace("&lt;", "<");
		text = text.Replace("&gt;", ">");
		text = text.Replace("&apos;", "'");
		return text.Replace("&quot;", "\"");
	}
}
