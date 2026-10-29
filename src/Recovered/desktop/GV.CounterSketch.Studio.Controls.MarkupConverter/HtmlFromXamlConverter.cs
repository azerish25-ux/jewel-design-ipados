using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace GV.CounterSketch.Studio.Controls.MarkupConverter;

public static class HtmlFromXamlConverter
{
	public static string ConvertXamlToHtml(string xamlString, bool asFullDocument)
	{
		XmlTextReader xamlReader = new XmlTextReader(new StringReader(xamlString));
		StringBuilder stringBuilder = new StringBuilder(100);
		XmlTextWriter htmlWriter = new XmlTextWriter(new StringWriter(stringBuilder));
		if (!WriteFlowDocument(xamlReader, htmlWriter, asFullDocument))
		{
			return "";
		}
		return stringBuilder.ToString();
	}

	private static bool WriteFlowDocument(XmlTextReader xamlReader, XmlTextWriter htmlWriter, bool asFullDocument)
	{
		if (!ReadNextToken(xamlReader))
		{
			return false;
		}
		if (xamlReader.NodeType != XmlNodeType.Element || xamlReader.Name != "FlowDocument")
		{
			return false;
		}
		StringBuilder inlineStyle = new StringBuilder();
		if (asFullDocument)
		{
			htmlWriter.WriteStartElement("HTML");
			htmlWriter.WriteStartElement("BODY");
		}
		WriteFormattingProperties(xamlReader, htmlWriter, inlineStyle);
		WriteElementContent(xamlReader, htmlWriter, inlineStyle);
		if (asFullDocument)
		{
			htmlWriter.WriteEndElement();
			htmlWriter.WriteEndElement();
		}
		return true;
	}

	private static void WriteFormattingProperties(XmlTextReader xamlReader, XmlTextWriter htmlWriter, StringBuilder inlineStyle)
	{
		inlineStyle.Remove(0, inlineStyle.Length);
		if (!xamlReader.HasAttributes)
		{
			return;
		}
		bool flag = false;
		while (xamlReader.MoveToNextAttribute())
		{
			string text = null;
			switch (xamlReader.Name)
			{
			case "Background":
				text = "background-color:" + ParseXamlColor(xamlReader.Value) + ";";
				break;
			case "FontFamily":
				text = "font-family:" + xamlReader.Value + ";";
				break;
			case "FontStyle":
				text = "font-style:" + xamlReader.Value.ToLower() + ";";
				break;
			case "FontWeight":
				text = "font-weight:" + xamlReader.Value.ToLower() + ";";
				break;
			case "FontSize":
				text = "font-size:" + xamlReader.Value + ";";
				break;
			case "Foreground":
				text = "color:" + ParseXamlColor(xamlReader.Value) + ";";
				break;
			case "TextDecorations":
				text = "text-decoration:underline;";
				break;
			case "Padding":
				text = "padding:" + ParseXamlThickness(xamlReader.Value) + ";";
				break;
			case "Margin":
				text = "margin:" + ParseXamlThickness(xamlReader.Value) + ";";
				break;
			case "BorderThickness":
				text = "border-width:" + ParseXamlThickness(xamlReader.Value) + ";";
				flag = true;
				break;
			case "BorderBrush":
				text = "border-color:" + ParseXamlColor(xamlReader.Value) + ";";
				flag = true;
				break;
			case "TextIndent":
				text = "text-indent:" + xamlReader.Value + ";";
				break;
			case "TextAlignment":
				text = "text-align:" + xamlReader.Value + ";";
				break;
			case "Width":
				text = "width:" + xamlReader.Value + ";";
				break;
			case "ColumnSpan":
				htmlWriter.WriteAttributeString("COLSPAN", xamlReader.Value);
				break;
			case "RowSpan":
				htmlWriter.WriteAttributeString("ROWSPAN", xamlReader.Value);
				break;
			}
			if (text != null)
			{
				inlineStyle.Append(text);
			}
		}
		if (flag)
		{
			inlineStyle.Append("border-style:solid;mso-element:para-border-div;");
		}
		xamlReader.MoveToElement();
	}

	private static string ParseXamlColor(string color)
	{
		if (color.StartsWith("#"))
		{
			color = "#" + color.Substring(3);
		}
		return color;
	}

	private static string ParseXamlThickness(string thickness)
	{
		string[] array = thickness.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			if (double.TryParse(array[i], NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
			{
				array[i] = Math.Ceiling(result).ToString();
			}
			else
			{
				array[i] = "1";
			}
		}
		return array.Length switch
		{
			1 => thickness, 
			2 => array[1] + " " + array[0], 
			4 => array[1] + " " + array[2] + " " + array[3] + " " + array[0], 
			_ => array[0], 
		};
	}

	private static void WriteElementContent(XmlTextReader xamlReader, XmlTextWriter htmlWriter, StringBuilder inlineStyle)
	{
		bool flag = false;
		if (xamlReader.IsEmptyElement)
		{
			if (htmlWriter != null && !flag && inlineStyle.Length > 0)
			{
				htmlWriter.WriteAttributeString("STYLE", inlineStyle.ToString());
				inlineStyle.Remove(0, inlineStyle.Length);
			}
			flag = true;
			return;
		}
		while (ReadNextToken(xamlReader) && xamlReader.NodeType != XmlNodeType.EndElement)
		{
			switch (xamlReader.NodeType)
			{
			case XmlNodeType.Element:
				if (xamlReader.Name.Contains("."))
				{
					AddComplexProperty(xamlReader, inlineStyle);
					break;
				}
				if (htmlWriter != null && !flag && inlineStyle.Length > 0)
				{
					htmlWriter.WriteAttributeString("STYLE", inlineStyle.ToString());
					inlineStyle.Remove(0, inlineStyle.Length);
				}
				flag = true;
				WriteElement(xamlReader, htmlWriter, inlineStyle);
				break;
			case XmlNodeType.Comment:
				if (htmlWriter != null)
				{
					if (!flag && inlineStyle.Length > 0)
					{
						htmlWriter.WriteAttributeString("STYLE", inlineStyle.ToString());
					}
					htmlWriter.WriteComment(xamlReader.Value);
				}
				flag = true;
				break;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.SignificantWhitespace:
				if (htmlWriter != null)
				{
					if (!flag && inlineStyle.Length > 0)
					{
						htmlWriter.WriteAttributeString("STYLE", inlineStyle.ToString());
					}
					htmlWriter.WriteString(xamlReader.Value);
				}
				flag = true;
				break;
			}
		}
	}

	private static void AddComplexProperty(XmlTextReader xamlReader, StringBuilder inlineStyle)
	{
		if (inlineStyle != null && xamlReader.Name.EndsWith(".TextDecorations"))
		{
			inlineStyle.Append("text-decoration:underline;");
		}
		WriteElementContent(xamlReader, null, null);
	}

	private static void WriteElement(XmlTextReader xamlReader, XmlTextWriter htmlWriter, StringBuilder inlineStyle)
	{
		if (htmlWriter == null)
		{
			WriteElementContent(xamlReader, null, null);
			return;
		}
		string text = null;
		switch (xamlReader.Name)
		{
		case "Run":
		case "Span":
			text = "SPAN";
			break;
		case "InlineUIContainer":
			text = "SPAN";
			break;
		case "Bold":
			text = "B";
			break;
		case "Italic":
			text = "I";
			break;
		case "Paragraph":
			text = "P";
			break;
		case "BlockUIContainer":
			text = "DIV";
			break;
		case "Section":
			text = "DIV";
			break;
		case "Table":
			text = "TABLE";
			break;
		case "TableColumn":
			text = "COL";
			break;
		case "TableRowGroup":
			text = "TBODY";
			break;
		case "TableRow":
			text = "TR";
			break;
		case "TableCell":
			text = "TD";
			break;
		case "List":
			switch (xamlReader.GetAttribute("MarkerStyle"))
			{
			case null:
			case "None":
			case "Disc":
			case "Circle":
			case "Square":
			case "Box":
				text = "UL";
				break;
			default:
				text = "OL";
				break;
			}
			break;
		case "ListItem":
			text = "LI";
			break;
		default:
			text = null;
			break;
		}
		if (htmlWriter != null && text != null)
		{
			htmlWriter.WriteStartElement(text);
			WriteFormattingProperties(xamlReader, htmlWriter, inlineStyle);
			WriteElementContent(xamlReader, htmlWriter, inlineStyle);
			htmlWriter.WriteEndElement();
		}
		else
		{
			WriteElementContent(xamlReader, null, null);
		}
	}

	private static bool ReadNextToken(XmlReader xamlReader)
	{
		while (xamlReader.Read())
		{
			switch (xamlReader.NodeType)
			{
			case XmlNodeType.None:
			case XmlNodeType.Element:
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.SignificantWhitespace:
			case XmlNodeType.EndElement:
				return true;
			case XmlNodeType.Whitespace:
				if (xamlReader.XmlSpace == XmlSpace.Preserve)
				{
					return true;
				}
				break;
			case XmlNodeType.Comment:
				return true;
			}
		}
		return false;
	}
}
