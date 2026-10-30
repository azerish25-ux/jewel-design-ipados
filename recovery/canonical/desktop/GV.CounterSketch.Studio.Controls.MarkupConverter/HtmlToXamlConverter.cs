using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Xml;

namespace GV.CounterSketch.Studio.Controls.MarkupConverter;

public static class HtmlToXamlConverter
{
	public const string Xaml_Background = "Background";

	public const string Xaml_BorderBrush = "BorderBrush";

	public const string Xaml_BorderThickness = "BorderThickness";

	public const string Xaml_Brushes_Black = "Black";

	public const string Xaml_FlowDocument = "FlowDocument";

	public const string Xaml_FontFamily = "FontFamily";

	public const string Xaml_FontSize = "FontSize";

	public const string Xaml_FontSize_Large = "18pt";

	public const string Xaml_FontSize_Medium = "16pt";

	public const string Xaml_FontSize_Small = "12pt";

	public const string Xaml_FontSize_XLarge = "20pt";

	public const string Xaml_FontSize_XSmall = "10pt";

	public const string Xaml_FontSize_XXLarge = "22pt";

	public const string Xaml_FontSize_XXSmall = "8pt";

	public const string Xaml_FontStyle = "FontStyle";

	public const string Xaml_FontWeight = "FontWeight";

	public const string Xaml_FontWeight_Bold = "Bold";

	public const string Xaml_Foreground = "Foreground";

	public const string Xaml_Hyperlink = "Hyperlink";

	public const string Xaml_Hyperlink_NavigateUri = "NavigateUri";

	public const string Xaml_Hyperlink_TargetName = "TargetName";

	public const string Xaml_LineBreak = "LineBreak";

	public const string Xaml_List = "List";

	public const string Xaml_List_MarkerStyle = "MarkerStyle";

	public const string Xaml_List_MarkerStyle_Box = "Box";

	public const string Xaml_List_MarkerStyle_Circle = "Circle";

	public const string Xaml_List_MarkerStyle_Decimal = "Decimal";

	public const string Xaml_List_MarkerStyle_Disc = "Disc";

	public const string Xaml_List_MarkerStyle_LowerLatin = "LowerLatin";

	public const string Xaml_List_MarkerStyle_LowerRoman = "LowerRoman";

	public const string Xaml_List_MarkerStyle_None = "None";

	public const string Xaml_List_MarkerStyle_Square = "Square";

	public const string Xaml_List_MarkerStyle_UpperLatin = "UpperLatin";

	public const string Xaml_List_MarkerStyle_UpperRoman = "UpperRoman";

	public const string Xaml_ListItem = "ListItem";

	public const string Xaml_Margin = "Margin";

	public const string Xaml_Padding = "Padding";

	public const string Xaml_Paragraph = "Paragraph";

	public const string Xaml_Run = "Run";

	public const string Xaml_Section = "Section";

	public const string Xaml_Span = "Span";

	public const string Xaml_Table = "Table";

	public const string Xaml_TableCell = "TableCell";

	public const string Xaml_TableCell_BorderBrush = "BorderBrush";

	public const string Xaml_TableCell_BorderThickness = "BorderThickness";

	public const string Xaml_TableCell_ColumnSpan = "ColumnSpan";

	public const string Xaml_TableCell_RowSpan = "RowSpan";

	public const string Xaml_TableColumn = "TableColumn";

	public const string Xaml_TableRow = "TableRow";

	public const string Xaml_TableRowGroup = "TableRowGroup";

	public const string Xaml_TextAlignment = "TextAlignment";

	public const string Xaml_TextDecorations = "TextDecorations";

	public const string Xaml_TextDecorations_Underline = "Underline";

	public const string Xaml_TextIndent = "TextIndent";

	public const string Xaml_Width = "Width";

	private static readonly string _xamlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

	private static XmlElement InlineFragmentParentElement;

	public static string ConvertHtmlToXaml(string htmlString, bool asFlowDocument)
	{
		XmlElement xmlElement = HtmlParser.ParseHtml(htmlString);
		string localName = (asFlowDocument ? "FlowDocument" : "Section");
		XmlElement xmlElement2 = new XmlDocument().CreateElement(null, localName, _xamlNamespace);
		CssStylesheet stylesheet = new CssStylesheet(xmlElement);
		List<XmlElement> sourceContext = new List<XmlElement>(10);
		InlineFragmentParentElement = null;
		AddBlock(xmlElement2, xmlElement, new Hashtable(), stylesheet, sourceContext);
		if (!asFlowDocument)
		{
			xmlElement2 = ExtractInlineFragment(xmlElement2);
		}
		xmlElement2.SetAttribute("xml:space", "preserve");
		return xmlElement2.OuterXml;
	}

	public static string GetAttribute(XmlElement element, string attributeName)
	{
		attributeName = attributeName.ToLower();
		for (int i = 0; i < element.Attributes.Count; i++)
		{
			if (element.Attributes[i].Name.ToLower() == attributeName)
			{
				return element.Attributes[i].Value;
			}
		}
		return null;
	}

	internal static string UnQuote(string value)
	{
		if ((value.StartsWith("\"") && value.EndsWith("\"")) || (value.StartsWith("'") && value.EndsWith("'")))
		{
			value = value.Substring(1, value.Length - 2).Trim();
		}
		return value;
	}

	private static XmlNode AddBlock(XmlElement xamlParentElement, XmlNode htmlNode, Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		if (htmlNode is XmlComment)
		{
			DefineInlineFragmentParent((XmlComment)htmlNode, null);
		}
		else if (htmlNode is XmlText)
		{
			htmlNode = AddImplicitParagraph(xamlParentElement, htmlNode, inheritedProperties, stylesheet, sourceContext);
		}
		else if (htmlNode is XmlElement)
		{
			XmlElement xmlElement = (XmlElement)htmlNode;
			string localName = xmlElement.LocalName;
			if (xmlElement.NamespaceURI != "http://www.w3.org/1999/xhtml")
			{
				return xmlElement;
			}
			sourceContext.Add(xmlElement);
			switch (localName.ToLower())
			{
			case "html":
			case "body":
			case "div":
			case "form":
			case "pre":
			case "blockquote":
			case "caption":
			case "center":
			case "cite":
				AddSection(xamlParentElement, xmlElement, inheritedProperties, stylesheet, sourceContext);
				break;
			case "p":
			case "h1":
			case "h2":
			case "h3":
			case "h4":
			case "h5":
			case "h6":
			case "nsrtitle":
			case "textarea":
			case "dd":
			case "dl":
			case "dt":
			case "tt":
				AddParagraph(xamlParentElement, xmlElement, inheritedProperties, stylesheet, sourceContext);
				break;
			case "ol":
			case "ul":
			case "dir":
			case "menu":
				AddList(xamlParentElement, xmlElement, inheritedProperties, stylesheet, sourceContext);
				break;
			case "li":
				htmlNode = AddOrphanListItems(xamlParentElement, xmlElement, inheritedProperties, stylesheet, sourceContext);
				break;
			case "img":
				AddImage(xamlParentElement, xmlElement, inheritedProperties, stylesheet, sourceContext);
				break;
			case "table":
				AddTable(xamlParentElement, xmlElement, inheritedProperties, stylesheet, sourceContext);
				break;
			default:
				htmlNode = AddImplicitParagraph(xamlParentElement, xmlElement, inheritedProperties, stylesheet, sourceContext);
				break;
			case "style":
			case "meta":
			case "head":
			case "title":
			case "script":
				break;
			}
			sourceContext.RemoveAt(sourceContext.Count - 1);
		}
		return htmlNode;
	}

	private static void AddBreak(XmlElement xamlParentElement, string htmlElementName)
	{
		XmlElement newChild = xamlParentElement.OwnerDocument.CreateElement(null, "LineBreak", _xamlNamespace);
		xamlParentElement.AppendChild(newChild);
		if (htmlElementName == "hr")
		{
			XmlText newChild2 = xamlParentElement.OwnerDocument.CreateTextNode("----------------------");
			xamlParentElement.AppendChild(newChild2);
			newChild = xamlParentElement.OwnerDocument.CreateElement(null, "LineBreak", _xamlNamespace);
			xamlParentElement.AppendChild(newChild);
		}
	}

	private static void AddSection(XmlElement xamlParentElement, XmlElement htmlElement, Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		bool flag = false;
		for (XmlNode xmlNode = htmlElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			if (xmlNode is XmlElement && HtmlSchema.IsBlockElement(((XmlElement)xmlNode).LocalName.ToLower()))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			AddParagraph(xamlParentElement, htmlElement, inheritedProperties, stylesheet, sourceContext);
			return;
		}
		Hashtable localProperties;
		Hashtable elementProperties = GetElementProperties(htmlElement, inheritedProperties, out localProperties, stylesheet, sourceContext);
		XmlElement xmlElement = xamlParentElement.OwnerDocument.CreateElement(null, "Section", _xamlNamespace);
		ApplyLocalProperties(xmlElement, localProperties, isBlock: true);
		if (!xmlElement.HasAttributes)
		{
			xmlElement = xamlParentElement;
		}
		for (XmlNode xmlNode2 = htmlElement.FirstChild; xmlNode2 != null; xmlNode2 = AddBlock(xmlElement, xmlNode2, elementProperties, stylesheet, sourceContext)?.NextSibling)
		{
		}
		if (xmlElement != xamlParentElement)
		{
			xamlParentElement.AppendChild(xmlElement);
		}
	}

	private static void AddParagraph(XmlElement xamlParentElement, XmlElement htmlElement, Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		Hashtable localProperties;
		Hashtable elementProperties = GetElementProperties(htmlElement, inheritedProperties, out localProperties, stylesheet, sourceContext);
		XmlElement xmlElement = xamlParentElement.OwnerDocument.CreateElement(null, "Paragraph", _xamlNamespace);
		ApplyLocalProperties(xmlElement, localProperties, isBlock: true);
		for (XmlNode xmlNode = htmlElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			AddInline(xmlElement, xmlNode, elementProperties, stylesheet, sourceContext);
		}
		xamlParentElement.AppendChild(xmlElement);
	}

	private static XmlNode AddImplicitParagraph(XmlElement xamlParentElement, XmlNode htmlNode, Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		XmlElement xmlElement = xamlParentElement.OwnerDocument.CreateElement(null, "Paragraph", _xamlNamespace);
		XmlNode result = null;
		while (htmlNode != null)
		{
			if (htmlNode is XmlComment)
			{
				DefineInlineFragmentParent((XmlComment)htmlNode, null);
			}
			else if (htmlNode is XmlText)
			{
				if (htmlNode.Value.Trim().Length > 0)
				{
					AddTextRun(xmlElement, htmlNode.Value);
				}
			}
			else if (htmlNode is XmlElement)
			{
				if (HtmlSchema.IsBlockElement(((XmlElement)htmlNode).LocalName.ToLower()))
				{
					break;
				}
				AddInline(xmlElement, (XmlElement)htmlNode, inheritedProperties, stylesheet, sourceContext);
			}
			result = htmlNode;
			htmlNode = htmlNode.NextSibling;
		}
		if (xmlElement.FirstChild != null)
		{
			xamlParentElement.AppendChild(xmlElement);
		}
		return result;
	}

	private static void AddInline(XmlElement xamlParentElement, XmlNode htmlNode, Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		if (htmlNode is XmlComment)
		{
			DefineInlineFragmentParent((XmlComment)htmlNode, xamlParentElement);
		}
		else if (htmlNode is XmlText)
		{
			AddTextRun(xamlParentElement, htmlNode.Value);
		}
		else
		{
			if (!(htmlNode is XmlElement))
			{
				return;
			}
			XmlElement xmlElement = (XmlElement)htmlNode;
			if (xmlElement.NamespaceURI != "http://www.w3.org/1999/xhtml")
			{
				return;
			}
			string text = xmlElement.LocalName.ToLower();
			sourceContext.Add(xmlElement);
			switch (text)
			{
			case "a":
				AddHyperlink(xamlParentElement, xmlElement, inheritedProperties, stylesheet, sourceContext);
				break;
			case "img":
				AddImage(xamlParentElement, xmlElement, inheritedProperties, stylesheet, sourceContext);
				break;
			case "br":
			case "hr":
				AddBreak(xamlParentElement, text);
				break;
			default:
				if (HtmlSchema.IsInlineElement(text) || HtmlSchema.IsBlockElement(text))
				{
					AddSpanOrRun(xamlParentElement, xmlElement, inheritedProperties, stylesheet, sourceContext);
				}
				break;
			}
			sourceContext.RemoveAt(sourceContext.Count - 1);
		}
	}

	private static void AddSpanOrRun(XmlElement xamlParentElement, XmlElement htmlElement, Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		bool flag = false;
		for (XmlNode xmlNode = htmlElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			if (!(xmlNode is XmlElement))
			{
				continue;
			}
			string text = ((XmlElement)xmlNode).LocalName.ToLower();
			if (!HtmlSchema.IsInlineElement(text) && !HtmlSchema.IsBlockElement(text))
			{
				switch (text)
				{
				case "img":
				case "br":
				case "hr":
					break;
				default:
					continue;
				}
			}
			flag = true;
			break;
		}
		string localName = (flag ? "Span" : "Run");
		Hashtable localProperties;
		Hashtable elementProperties = GetElementProperties(htmlElement, inheritedProperties, out localProperties, stylesheet, sourceContext);
		XmlElement xmlElement = xamlParentElement.OwnerDocument.CreateElement(null, localName, _xamlNamespace);
		ApplyLocalProperties(xmlElement, localProperties, isBlock: false);
		for (XmlNode xmlNode2 = htmlElement.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
		{
			AddInline(xmlElement, xmlNode2, elementProperties, stylesheet, sourceContext);
		}
		xamlParentElement.AppendChild(xmlElement);
	}

	private static void AddTextRun(XmlElement xamlElement, string textData)
	{
		for (int i = 0; i < textData.Length; i++)
		{
			if (char.IsControl(textData[i]))
			{
				textData = textData.Remove(i--, 1);
			}
		}
		textData = textData.Replace('\u00a0', ' ');
		if (textData.Length > 0)
		{
			xamlElement.AppendChild(xamlElement.OwnerDocument.CreateTextNode(textData));
		}
	}

	private static void AddHyperlink(XmlElement xamlParentElement, XmlElement htmlElement, Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		string attribute = GetAttribute(htmlElement, "href");
		if (attribute == null)
		{
			AddSpanOrRun(xamlParentElement, htmlElement, inheritedProperties, stylesheet, sourceContext);
			return;
		}
		Hashtable localProperties;
		Hashtable elementProperties = GetElementProperties(htmlElement, inheritedProperties, out localProperties, stylesheet, sourceContext);
		XmlElement xmlElement = xamlParentElement.OwnerDocument.CreateElement(null, "Hyperlink", _xamlNamespace);
		ApplyLocalProperties(xmlElement, localProperties, isBlock: false);
		string[] array = attribute.Split('#');
		if (array.Length != 0 && array[0].Trim().Length > 0)
		{
			xmlElement.SetAttribute("NavigateUri", array[0].Trim());
		}
		if (array.Length == 2 && array[1].Trim().Length > 0)
		{
			xmlElement.SetAttribute("TargetName", array[1].Trim());
		}
		for (XmlNode xmlNode = htmlElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			AddInline(xmlElement, xmlNode, elementProperties, stylesheet, sourceContext);
		}
		xamlParentElement.AppendChild(xmlElement);
	}

	private static void DefineInlineFragmentParent(XmlComment htmlComment, XmlElement xamlParentElement)
	{
		if (htmlComment.Value == "StartFragment")
		{
			InlineFragmentParentElement = xamlParentElement;
		}
		else if (htmlComment.Value == "EndFragment" && InlineFragmentParentElement == null && xamlParentElement != null)
		{
			InlineFragmentParentElement = xamlParentElement;
		}
	}

	private static XmlElement ExtractInlineFragment(XmlElement xamlFlowDocumentElement)
	{
		if (InlineFragmentParentElement != null)
		{
			if (InlineFragmentParentElement.LocalName == "Span")
			{
				xamlFlowDocumentElement = InlineFragmentParentElement;
			}
			else
			{
				xamlFlowDocumentElement = xamlFlowDocumentElement.OwnerDocument.CreateElement(null, "Span", _xamlNamespace);
				while (InlineFragmentParentElement.FirstChild != null)
				{
					XmlNode firstChild = InlineFragmentParentElement.FirstChild;
					InlineFragmentParentElement.RemoveChild(firstChild);
					xamlFlowDocumentElement.AppendChild(firstChild);
				}
			}
		}
		return xamlFlowDocumentElement;
	}

	private static void AddImage(XmlElement xamlParentElement, XmlElement htmlElement, Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
	}

	private static void AddList(XmlElement xamlParentElement, XmlElement htmlListElement, Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		string text = htmlListElement.LocalName.ToLower();
		Hashtable localProperties;
		Hashtable elementProperties = GetElementProperties(htmlListElement, inheritedProperties, out localProperties, stylesheet, sourceContext);
		XmlElement xmlElement = xamlParentElement.OwnerDocument.CreateElement(null, "List", _xamlNamespace);
		if (text == "ol")
		{
			xmlElement.SetAttribute("MarkerStyle", "Decimal");
		}
		else
		{
			xmlElement.SetAttribute("MarkerStyle", "Disc");
		}
		ApplyLocalProperties(xmlElement, localProperties, isBlock: true);
		for (XmlNode xmlNode = htmlListElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			if (xmlNode is XmlElement && xmlNode.LocalName.ToLower() == "li")
			{
				sourceContext.Add((XmlElement)xmlNode);
				AddListItem(xmlElement, (XmlElement)xmlNode, elementProperties, stylesheet, sourceContext);
				sourceContext.RemoveAt(sourceContext.Count - 1);
			}
		}
		if (xmlElement.HasChildNodes)
		{
			xamlParentElement.AppendChild(xmlElement);
		}
	}

	private static XmlElement AddOrphanListItems(XmlElement xamlParentElement, XmlElement htmlLIElement, Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		XmlElement result = null;
		XmlNode lastChild = xamlParentElement.LastChild;
		XmlElement xmlElement;
		if (lastChild != null && lastChild.LocalName == "List")
		{
			xmlElement = (XmlElement)lastChild;
		}
		else
		{
			xmlElement = xamlParentElement.OwnerDocument.CreateElement(null, "List", _xamlNamespace);
			xamlParentElement.AppendChild(xmlElement);
		}
		XmlNode xmlNode = htmlLIElement;
		string text = xmlNode?.LocalName.ToLower();
		while (xmlNode != null && text == "li")
		{
			AddListItem(xmlElement, (XmlElement)xmlNode, inheritedProperties, stylesheet, sourceContext);
			result = (XmlElement)xmlNode;
			xmlNode = xmlNode.NextSibling;
			text = xmlNode?.LocalName.ToLower();
		}
		return result;
	}

	private static void AddListItem(XmlElement xamlListElement, XmlElement htmlLIElement, Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		Hashtable localProperties;
		Hashtable elementProperties = GetElementProperties(htmlLIElement, inheritedProperties, out localProperties, stylesheet, sourceContext);
		XmlElement xmlElement = xamlListElement.OwnerDocument.CreateElement(null, "ListItem", _xamlNamespace);
		for (XmlNode xmlNode = htmlLIElement.FirstChild; xmlNode != null; xmlNode = AddBlock(xmlElement, xmlNode, elementProperties, stylesheet, sourceContext)?.NextSibling)
		{
		}
		xamlListElement.AppendChild(xmlElement);
	}

	private static void AddTable(XmlElement xamlParentElement, XmlElement htmlTableElement, Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		Hashtable localProperties;
		Hashtable elementProperties = GetElementProperties(htmlTableElement, inheritedProperties, out localProperties, stylesheet, sourceContext);
		XmlElement cellFromSingleCellTable = GetCellFromSingleCellTable(htmlTableElement);
		if (cellFromSingleCellTable != null)
		{
			sourceContext.Add(cellFromSingleCellTable);
			for (XmlNode xmlNode = cellFromSingleCellTable.FirstChild; xmlNode != null; xmlNode = AddBlock(xamlParentElement, xmlNode, elementProperties, stylesheet, sourceContext)?.NextSibling)
			{
			}
			sourceContext.RemoveAt(sourceContext.Count - 1);
			return;
		}
		XmlElement xmlElement = xamlParentElement.OwnerDocument.CreateElement(null, "Table", _xamlNamespace);
		ArrayList arrayList = AnalyzeTableStructure(htmlTableElement, stylesheet);
		AddColumnInformation(htmlTableElement, xmlElement, arrayList, elementProperties, stylesheet, sourceContext);
		XmlNode xmlNode2 = htmlTableElement.FirstChild;
		while (xmlNode2 != null)
		{
			switch (xmlNode2.LocalName.ToLower())
			{
			case "tbody":
			case "thead":
			case "tfoot":
			{
				XmlElement xmlElement3 = xmlElement.OwnerDocument.CreateElement(null, "TableRowGroup", _xamlNamespace);
				xmlElement.AppendChild(xmlElement3);
				sourceContext.Add((XmlElement)xmlNode2);
				Hashtable localProperties2;
				Hashtable elementProperties2 = GetElementProperties((XmlElement)xmlNode2, elementProperties, out localProperties2, stylesheet, sourceContext);
				AddTableRowsToTableBody(xmlElement3, xmlNode2.FirstChild, elementProperties2, arrayList, stylesheet, sourceContext);
				if (xmlElement3.HasChildNodes)
				{
					xmlElement.AppendChild(xmlElement3);
				}
				sourceContext.RemoveAt(sourceContext.Count - 1);
				xmlNode2 = xmlNode2.NextSibling;
				break;
			}
			case "tr":
			{
				XmlElement xmlElement2 = xmlElement.OwnerDocument.CreateElement(null, "TableRowGroup", _xamlNamespace);
				xmlNode2 = AddTableRowsToTableBody(xmlElement2, xmlNode2, elementProperties, arrayList, stylesheet, sourceContext);
				if (xmlElement2.HasChildNodes)
				{
					xmlElement.AppendChild(xmlElement2);
				}
				break;
			}
			default:
				xmlNode2 = xmlNode2.NextSibling;
				break;
			}
		}
		if (xmlElement.HasChildNodes)
		{
			xamlParentElement.AppendChild(xmlElement);
		}
	}

	private static XmlElement GetCellFromSingleCellTable(XmlElement htmlTableElement)
	{
		XmlElement xmlElement = null;
		for (XmlNode xmlNode = htmlTableElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			switch (xmlNode.LocalName.ToLower())
			{
			case "tbody":
			case "thead":
			case "tfoot":
			{
				if (xmlElement != null)
				{
					return null;
				}
				for (XmlNode xmlNode3 = xmlNode.FirstChild; xmlNode3 != null; xmlNode3 = xmlNode3.NextSibling)
				{
					if (xmlNode3.LocalName.ToLower() == "tr")
					{
						if (xmlElement != null)
						{
							return null;
						}
						for (XmlNode xmlNode4 = xmlNode3.FirstChild; xmlNode4 != null; xmlNode4 = xmlNode4.NextSibling)
						{
							string text2 = xmlNode4.LocalName.ToLower();
							if (text2 == "td" || text2 == "th")
							{
								if (xmlElement != null)
								{
									return null;
								}
								xmlElement = (XmlElement)xmlNode4;
							}
						}
					}
				}
				break;
			}
			default:
			{
				if (!(xmlNode.LocalName.ToLower() == "tr"))
				{
					break;
				}
				if (xmlElement != null)
				{
					return null;
				}
				for (XmlNode xmlNode2 = xmlNode.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
				{
					string text = xmlNode2.LocalName.ToLower();
					if (text == "td" || text == "th")
					{
						if (xmlElement != null)
						{
							return null;
						}
						xmlElement = (XmlElement)xmlNode2;
					}
				}
				break;
			}
			}
		}
		return xmlElement;
	}

	private static void AddColumnInformation(XmlElement htmlTableElement, XmlElement xamlTableElement, ArrayList columnStartsAllRows, Hashtable currentProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		if (columnStartsAllRows != null)
		{
			for (int i = 0; i < columnStartsAllRows.Count - 1; i++)
			{
				XmlElement xmlElement = xamlTableElement.OwnerDocument.CreateElement(null, "TableColumn", _xamlNamespace);
				xmlElement.SetAttribute("Width", ((double)columnStartsAllRows[i + 1] - (double)columnStartsAllRows[i]).ToString());
				xamlTableElement.AppendChild(xmlElement);
			}
			return;
		}
		for (XmlNode xmlNode = htmlTableElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			if (xmlNode.LocalName.ToLower() == "colgroup")
			{
				AddTableColumnGroup(xamlTableElement, (XmlElement)xmlNode, currentProperties, stylesheet, sourceContext);
			}
			else if (xmlNode.LocalName.ToLower() == "col")
			{
				AddTableColumn(xamlTableElement, (XmlElement)xmlNode, currentProperties, stylesheet, sourceContext);
			}
			else if (xmlNode is XmlElement)
			{
				break;
			}
		}
	}

	private static void AddTableColumnGroup(XmlElement xamlTableElement, XmlElement htmlColgroupElement, Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		Hashtable localProperties;
		Hashtable elementProperties = GetElementProperties(htmlColgroupElement, inheritedProperties, out localProperties, stylesheet, sourceContext);
		for (XmlNode xmlNode = htmlColgroupElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			if (xmlNode is XmlElement && xmlNode.LocalName.ToLower() == "col")
			{
				AddTableColumn(xamlTableElement, (XmlElement)xmlNode, elementProperties, stylesheet, sourceContext);
			}
		}
	}

	private static void AddTableColumn(XmlElement xamlTableElement, XmlElement htmlColElement, Hashtable inheritedProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		GetElementProperties(htmlColElement, inheritedProperties, out var _, stylesheet, sourceContext);
		XmlElement newChild = xamlTableElement.OwnerDocument.CreateElement(null, "TableColumn", _xamlNamespace);
		xamlTableElement.AppendChild(newChild);
	}

	private static XmlNode AddTableRowsToTableBody(XmlElement xamlTableBodyElement, XmlNode htmlTRStartNode, Hashtable currentProperties, ArrayList columnStarts, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		XmlNode xmlNode = htmlTRStartNode;
		ArrayList activeRowSpans = null;
		if (columnStarts != null)
		{
			activeRowSpans = new ArrayList();
			InitializeActiveRowSpans(activeRowSpans, columnStarts.Count);
		}
		while (xmlNode != null && xmlNode.LocalName.ToLower() != "tbody")
		{
			if (xmlNode.LocalName.ToLower() == "tr")
			{
				XmlElement xmlElement = xamlTableBodyElement.OwnerDocument.CreateElement(null, "TableRow", _xamlNamespace);
				sourceContext.Add((XmlElement)xmlNode);
				Hashtable localProperties;
				Hashtable elementProperties = GetElementProperties((XmlElement)xmlNode, currentProperties, out localProperties, stylesheet, sourceContext);
				AddTableCellsToTableRow(xmlElement, xmlNode.FirstChild, elementProperties, columnStarts, activeRowSpans, stylesheet, sourceContext);
				if (xmlElement.HasChildNodes)
				{
					xamlTableBodyElement.AppendChild(xmlElement);
				}
				sourceContext.RemoveAt(sourceContext.Count - 1);
				xmlNode = xmlNode.NextSibling;
			}
			else if (xmlNode.LocalName.ToLower() == "td")
			{
				XmlElement xmlElement2 = xamlTableBodyElement.OwnerDocument.CreateElement(null, "TableRow", _xamlNamespace);
				xmlNode = AddTableCellsToTableRow(xmlElement2, xmlNode, currentProperties, columnStarts, activeRowSpans, stylesheet, sourceContext);
				if (xmlElement2.HasChildNodes)
				{
					xamlTableBodyElement.AppendChild(xmlElement2);
				}
			}
			else
			{
				xmlNode = xmlNode.NextSibling;
			}
		}
		return xmlNode;
	}

	private static XmlNode AddTableCellsToTableRow(XmlElement xamlTableRowElement, XmlNode htmlTDStartNode, Hashtable currentProperties, ArrayList columnStarts, ArrayList activeRowSpans, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		XmlNode xmlNode = htmlTDStartNode;
		double num = 0.0;
		int i = 0;
		int num2 = 0;
		while (xmlNode != null && xmlNode.LocalName.ToLower() != "tr" && xmlNode.LocalName.ToLower() != "tbody" && xmlNode.LocalName.ToLower() != "thead" && xmlNode.LocalName.ToLower() != "tfoot")
		{
			if (xmlNode.LocalName.ToLower() == "td" || xmlNode.LocalName.ToLower() == "th")
			{
				XmlElement xmlElement = xamlTableRowElement.OwnerDocument.CreateElement(null, "TableCell", _xamlNamespace);
				sourceContext.Add((XmlElement)xmlNode);
				Hashtable localProperties;
				Hashtable elementProperties = GetElementProperties((XmlElement)xmlNode, currentProperties, out localProperties, stylesheet, sourceContext);
				ApplyPropertiesToTableCellElement((XmlElement)xmlNode, xmlElement);
				if (columnStarts != null)
				{
					for (; i < activeRowSpans.Count && (int)activeRowSpans[i] > 0; i++)
					{
						activeRowSpans[i] = (int)activeRowSpans[i] - 1;
					}
					_ = (double)columnStarts[i];
					num = GetColumnWidth((XmlElement)xmlNode);
					num2 = CalculateColumnSpan(i, num, columnStarts);
					int rowSpan = GetRowSpan((XmlElement)xmlNode);
					xmlElement.SetAttribute("ColumnSpan", num2.ToString());
					for (int j = i; j < i + num2; j++)
					{
						activeRowSpans[j] = rowSpan - 1;
					}
					i += num2;
				}
				AddDataToTableCell(xmlElement, xmlNode.FirstChild, elementProperties, stylesheet, sourceContext);
				if (xmlElement.HasChildNodes)
				{
					xamlTableRowElement.AppendChild(xmlElement);
				}
				sourceContext.RemoveAt(sourceContext.Count - 1);
				xmlNode = xmlNode.NextSibling;
			}
			else
			{
				xmlNode = xmlNode.NextSibling;
			}
		}
		return xmlNode;
	}

	private static void AddDataToTableCell(XmlElement xamlTableCellElement, XmlNode htmlDataStartNode, Hashtable currentProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		for (XmlNode xmlNode = htmlDataStartNode; xmlNode != null; xmlNode = AddBlock(xamlTableCellElement, xmlNode, currentProperties, stylesheet, sourceContext)?.NextSibling)
		{
		}
	}

	private static ArrayList AnalyzeTableStructure(XmlElement htmlTableElement, CssStylesheet stylesheet)
	{
		if (!htmlTableElement.HasChildNodes)
		{
			return null;
		}
		bool flag = true;
		ArrayList arrayList = new ArrayList();
		ArrayList activeRowSpans = new ArrayList();
		XmlNode xmlNode = htmlTableElement.FirstChild;
		double num = 0.0;
		while (xmlNode != null && flag)
		{
			switch (xmlNode.LocalName.ToLower())
			{
			case "tbody":
			{
				double num3 = AnalyzeTbodyStructure((XmlElement)xmlNode, arrayList, activeRowSpans, num, stylesheet);
				if (num3 > num)
				{
					num = num3;
				}
				else if (num3 == 0.0)
				{
					flag = false;
				}
				break;
			}
			case "tr":
			{
				double num2 = AnalyzeTRStructure((XmlElement)xmlNode, arrayList, activeRowSpans, num, stylesheet);
				if (num2 > num)
				{
					num = num2;
				}
				else if (num2 == 0.0)
				{
					flag = false;
				}
				break;
			}
			case "td":
				flag = false;
				break;
			}
			xmlNode = xmlNode.NextSibling;
		}
		if (flag)
		{
			arrayList.Add(num);
			VerifyColumnStartsAscendingOrder(arrayList);
		}
		else
		{
			arrayList = null;
		}
		return arrayList;
	}

	private static double AnalyzeTbodyStructure(XmlElement htmlTbodyElement, ArrayList columnStarts, ArrayList activeRowSpans, double tableWidth, CssStylesheet stylesheet)
	{
		double num = 0.0;
		bool flag = true;
		if (!htmlTbodyElement.HasChildNodes)
		{
			return num;
		}
		ClearActiveRowSpans(activeRowSpans);
		XmlNode xmlNode = htmlTbodyElement.FirstChild;
		while (xmlNode != null && flag)
		{
			switch (xmlNode.LocalName.ToLower())
			{
			case "tr":
			{
				double num2 = AnalyzeTRStructure((XmlElement)xmlNode, columnStarts, activeRowSpans, num, stylesheet);
				if (num2 > num)
				{
					num = num2;
				}
				break;
			}
			case "td":
				flag = false;
				break;
			}
			xmlNode = xmlNode.NextSibling;
		}
		ClearActiveRowSpans(activeRowSpans);
		if (!flag)
		{
			return 0.0;
		}
		return num;
	}

	private static double AnalyzeTRStructure(XmlElement htmlTRElement, ArrayList columnStarts, ArrayList activeRowSpans, double tableWidth, CssStylesheet stylesheet)
	{
		if (!htmlTRElement.HasChildNodes)
		{
			return 0.0;
		}
		bool flag = true;
		double num = 0.0;
		XmlNode xmlNode = htmlTRElement.FirstChild;
		int num2 = 0;
		double num3 = 0.0;
		if (num2 < activeRowSpans.Count && (double)columnStarts[num2] == num)
		{
			while (num2 < activeRowSpans.Count && (int)activeRowSpans[num2] > 0)
			{
				activeRowSpans[num2] = (int)activeRowSpans[num2] - 1;
				num2++;
				num = (double)columnStarts[num2];
			}
		}
		while (xmlNode != null && flag)
		{
			VerifyColumnStartsAscendingOrder(columnStarts);
			string text = xmlNode.LocalName.ToLower();
			if (text != null && text == "td")
			{
				if (num2 < columnStarts.Count)
				{
					if (num < (double)columnStarts[num2])
					{
						columnStarts.Insert(num2, num);
						activeRowSpans.Insert(num2, 0);
					}
				}
				else
				{
					columnStarts.Add(num);
					activeRowSpans.Add(0);
				}
				double columnWidth = GetColumnWidth((XmlElement)xmlNode);
				if (columnWidth != -1.0)
				{
					int rowSpan = GetRowSpan((XmlElement)xmlNode);
					int nextColumnIndex = GetNextColumnIndex(num2, columnWidth, columnStarts, activeRowSpans);
					if (nextColumnIndex != -1)
					{
						for (int i = num2; i < nextColumnIndex; i++)
						{
							activeRowSpans[i] = rowSpan - 1;
						}
						num2 = nextColumnIndex;
						num += columnWidth;
						if (num2 < activeRowSpans.Count && (double)columnStarts[num2] == num)
						{
							while (num2 < activeRowSpans.Count && (int)activeRowSpans[num2] > 0)
							{
								activeRowSpans[num2] = (int)activeRowSpans[num2] - 1;
								num2++;
								num = (double)columnStarts[num2];
							}
						}
					}
					else
					{
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
			}
			xmlNode = xmlNode.NextSibling;
		}
		if (flag)
		{
			return num;
		}
		return 0.0;
	}

	private static int GetRowSpan(XmlElement htmlTDElement)
	{
		string attribute = GetAttribute(htmlTDElement, "rowspan");
		if (attribute != null)
		{
			if (!int.TryParse(attribute, out var result))
			{
				return 1;
			}
			return result;
		}
		return 1;
	}

	private static int GetNextColumnIndex(int columnIndex, double columnWidth, ArrayList columnStarts, ArrayList activeRowSpans)
	{
		double num = (double)columnStarts[columnIndex];
		int num2 = columnIndex + 1;
		while (num2 < columnStarts.Count && (double)columnStarts[num2] < num + columnWidth && num2 != -1)
		{
			num2 = (((int)activeRowSpans[num2] <= 0) ? (num2 + 1) : (-1));
		}
		return num2;
	}

	private static void ClearActiveRowSpans(ArrayList activeRowSpans)
	{
		for (int i = 0; i < activeRowSpans.Count; i++)
		{
			activeRowSpans[i] = 0;
		}
	}

	private static void InitializeActiveRowSpans(ArrayList activeRowSpans, int count)
	{
		for (int i = 0; i < count; i++)
		{
			activeRowSpans.Add(0);
		}
	}

	private static double GetNextColumnStart(XmlElement htmlTDElement, double columnStart)
	{
		double num = -1.0;
		double columnWidth = GetColumnWidth(htmlTDElement);
		if (columnWidth == -1.0)
		{
			return -1.0;
		}
		return columnStart + columnWidth;
	}

	private static double GetColumnWidth(XmlElement htmlTDElement)
	{
		string text = null;
		double length = -1.0;
		text = GetAttribute(htmlTDElement, "width");
		if (text == null)
		{
			text = GetCssAttribute(GetAttribute(htmlTDElement, "style"), "width");
		}
		if (!TryGetLengthValue(text, out length) || length == 0.0)
		{
			length = -1.0;
		}
		return length;
	}

	private static int CalculateColumnSpan(int columnIndex, double columnWidth, ArrayList columnStarts)
	{
		int num = columnIndex;
		double num2 = 0.0;
		double num3 = 0.0;
		while (num2 < columnWidth && num < columnStarts.Count - 1)
		{
			num3 = (double)columnStarts[num + 1] - (double)columnStarts[num];
			num2 += num3;
			num++;
		}
		return num - columnIndex;
	}

	private static void VerifyColumnStartsAscendingOrder(ArrayList columnStarts)
	{
		for (int i = 0; i < columnStarts.Count; i++)
		{
			_ = (double)columnStarts[i];
		}
	}

	private static void ApplyLocalProperties(XmlElement xamlElement, Hashtable localProperties, bool isBlock)
	{
		bool flag = false;
		string top = "0";
		string bottom = "0";
		string left = "0";
		string right = "0";
		bool flag2 = false;
		string top2 = "0";
		string bottom2 = "0";
		string left2 = "0";
		string right2 = "0";
		string text = null;
		bool flag3 = false;
		string top3 = "0";
		string bottom3 = "0";
		string left3 = "0";
		string right3 = "0";
		IDictionaryEnumerator enumerator = localProperties.GetEnumerator();
		while (enumerator.MoveNext())
		{
			switch ((string)enumerator.Key)
			{
			case "font-family":
				xamlElement.SetAttribute("FontFamily", (string)enumerator.Value);
				break;
			case "font-style":
				xamlElement.SetAttribute("FontStyle", (string)enumerator.Value);
				break;
			case "font-weight":
				xamlElement.SetAttribute("FontWeight", (string)enumerator.Value);
				break;
			case "font-size":
				xamlElement.SetAttribute("FontSize", (string)enumerator.Value);
				break;
			case "color":
				SetPropertyValue(xamlElement, TextElement.ForegroundProperty, (string)enumerator.Value);
				break;
			case "background-color":
				SetPropertyValue(xamlElement, TextElement.BackgroundProperty, (string)enumerator.Value);
				break;
			case "text-decoration-underline":
				if (!isBlock && (string)enumerator.Value == "true")
				{
					xamlElement.SetAttribute("TextDecorations", "Underline");
				}
				break;
			case "text-decoration-none":
			case "text-decoration-overline":
			case "text-decoration-line-through":
			case "text-decoration-blink":
				if (isBlock)
				{
				}
				break;
			case "text-indent":
				if (isBlock)
				{
					xamlElement.SetAttribute("TextIndent", (string)enumerator.Value);
				}
				break;
			case "text-align":
				if (isBlock)
				{
					xamlElement.SetAttribute("TextAlignment", (string)enumerator.Value);
				}
				break;
			case "margin-top":
				flag = true;
				top = (string)enumerator.Value;
				break;
			case "margin-right":
				flag = true;
				right = (string)enumerator.Value;
				break;
			case "margin-bottom":
				flag = true;
				bottom = (string)enumerator.Value;
				break;
			case "margin-left":
				flag = true;
				left = (string)enumerator.Value;
				break;
			case "padding-top":
				flag2 = true;
				top2 = (string)enumerator.Value;
				break;
			case "padding-right":
				flag2 = true;
				right2 = (string)enumerator.Value;
				break;
			case "padding-bottom":
				flag2 = true;
				bottom2 = (string)enumerator.Value;
				break;
			case "padding-left":
				flag2 = true;
				left2 = (string)enumerator.Value;
				break;
			case "border-color-top":
				text = (string)enumerator.Value;
				break;
			case "border-color-right":
				text = (string)enumerator.Value;
				break;
			case "border-color-bottom":
				text = (string)enumerator.Value;
				break;
			case "border-color-left":
				text = (string)enumerator.Value;
				break;
			case "border-width-top":
				flag3 = true;
				top3 = (string)enumerator.Value;
				break;
			case "border-width-right":
				flag3 = true;
				right3 = (string)enumerator.Value;
				break;
			case "border-width-bottom":
				flag3 = true;
				bottom3 = (string)enumerator.Value;
				break;
			case "border-width-left":
				flag3 = true;
				left3 = (string)enumerator.Value;
				break;
			case "list-style-type":
				if (xamlElement.LocalName == "List")
				{
					xamlElement.SetAttribute("MarkerStyle", ((string)enumerator.Value).ToLower() switch
					{
						"disc" => "Disc", 
						"circle" => "Circle", 
						"none" => "None", 
						"square" => "Square", 
						"box" => "Box", 
						"lower-latin" => "LowerLatin", 
						"upper-latin" => "UpperLatin", 
						"lower-roman" => "LowerRoman", 
						"upper-roman" => "UpperRoman", 
						"decimal" => "Decimal", 
						_ => "Disc", 
					});
				}
				break;
			}
		}
		if (isBlock)
		{
			if (flag)
			{
				ComposeThicknessProperty(xamlElement, "Margin", left, right, top, bottom);
			}
			if (flag2)
			{
				ComposeThicknessProperty(xamlElement, "Padding", left2, right2, top2, bottom2);
			}
			if (text != null)
			{
				xamlElement.SetAttribute("BorderBrush", text);
			}
			if (flag3)
			{
				ComposeThicknessProperty(xamlElement, "BorderThickness", left3, right3, top3, bottom3);
			}
		}
	}

	private static void ComposeThicknessProperty(XmlElement xamlElement, string propertyName, string left, string right, string top, string bottom)
	{
		if (left[0] == '0' || left[0] == '-')
		{
			left = "0";
		}
		if (right[0] == '0' || right[0] == '-')
		{
			right = "0";
		}
		if (top[0] == '0' || top[0] == '-')
		{
			top = "0";
		}
		if (bottom[0] == '0' || bottom[0] == '-')
		{
			bottom = "0";
		}
		string value = ((!(left == right) || !(top == bottom)) ? (left + "," + top + "," + right + "," + bottom) : ((!(left == top)) ? (left + "," + top) : left));
		xamlElement.SetAttribute(propertyName, value);
	}

	private static void SetPropertyValue(XmlElement xamlElement, DependencyProperty property, string stringValue)
	{
		TypeConverter converter = TypeDescriptor.GetConverter(property.PropertyType);
		try
		{
			if (converter.ConvertFromInvariantString(stringValue) != null)
			{
				xamlElement.SetAttribute(property.Name, stringValue);
			}
		}
		catch (Exception)
		{
		}
	}

	private static Hashtable GetElementProperties(XmlElement htmlElement, Hashtable inheritedProperties, out Hashtable localProperties, CssStylesheet stylesheet, List<XmlElement> sourceContext)
	{
		Hashtable hashtable = new Hashtable();
		IDictionaryEnumerator enumerator = inheritedProperties.GetEnumerator();
		while (enumerator.MoveNext())
		{
			hashtable[enumerator.Key] = enumerator.Value;
		}
		string text = htmlElement.LocalName.ToLower();
		_ = htmlElement.NamespaceURI;
		localProperties = new Hashtable();
		switch (text)
		{
		case "i":
		case "italic":
		case "em":
			localProperties["font-style"] = "italic";
			break;
		case "b":
		case "bold":
		case "strong":
		case "dfn":
			localProperties["font-weight"] = "bold";
			break;
		case "u":
		case "underline":
			localProperties["text-decoration-underline"] = "true";
			break;
		case "font":
		{
			string attribute = GetAttribute(htmlElement, "face");
			if (attribute != null)
			{
				localProperties["font-family"] = attribute;
			}
			attribute = GetAttribute(htmlElement, "size");
			if (attribute != null)
			{
				double num = double.Parse(attribute) * 4.0;
				if (num < 1.0)
				{
					num = 1.0;
				}
				else if (num > 1000.0)
				{
					num = 1000.0;
				}
				localProperties["font-size"] = num.ToString();
			}
			attribute = GetAttribute(htmlElement, "color");
			if (attribute != null)
			{
				localProperties["color"] = attribute;
			}
			break;
		}
		case "samp":
			localProperties["font-family"] = "Courier New";
			localProperties["font-size"] = "8pt";
			localProperties["text-align"] = "Left";
			break;
		case "pre":
			localProperties["font-family"] = "Courier New";
			localProperties["font-size"] = "8pt";
			localProperties["text-align"] = "Left";
			break;
		case "blockquote":
			localProperties["margin-left"] = "16";
			break;
		case "h1":
			localProperties["font-size"] = "22pt";
			break;
		case "h2":
			localProperties["font-size"] = "20pt";
			break;
		case "h3":
			localProperties["font-size"] = "18pt";
			break;
		case "h4":
			localProperties["font-size"] = "16pt";
			break;
		case "h5":
			localProperties["font-size"] = "12pt";
			break;
		case "h6":
			localProperties["font-size"] = "10pt";
			break;
		case "ul":
			localProperties["list-style-type"] = "disc";
			break;
		case "ol":
			localProperties["list-style-type"] = "decimal";
			break;
		}
		HtmlCssParser.GetElementPropertiesFromCssAttributes(htmlElement, text, stylesheet, localProperties, sourceContext);
		enumerator = localProperties.GetEnumerator();
		while (enumerator.MoveNext())
		{
			hashtable[enumerator.Key] = enumerator.Value;
		}
		return hashtable;
	}

	private static string GetCssAttribute(string cssStyle, string attributeName)
	{
		if (cssStyle != null)
		{
			attributeName = attributeName.ToLower();
			string[] array = cssStyle.Split(';');
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(':');
				if (array2.Length == 2 && array2[0].Trim().ToLower() == attributeName)
				{
					return array2[1].Trim();
				}
			}
		}
		return null;
	}

	private static bool TryGetLengthValue(string lengthAsString, out double length)
	{
		length = double.NaN;
		if (lengthAsString != null)
		{
			lengthAsString = lengthAsString.Trim().ToLower();
			if (lengthAsString.EndsWith("pt"))
			{
				lengthAsString = lengthAsString.Substring(0, lengthAsString.Length - 2);
				if (double.TryParse(lengthAsString, NumberStyles.Any, CultureInfo.InvariantCulture, out length))
				{
					length = length * 96.0 / 72.0;
				}
				else
				{
					length = double.NaN;
				}
			}
			else if (lengthAsString.EndsWith("px"))
			{
				lengthAsString = lengthAsString.Substring(0, lengthAsString.Length - 2);
				if (!double.TryParse(lengthAsString, NumberStyles.Any, CultureInfo.InvariantCulture, out length))
				{
					length = double.NaN;
				}
			}
			else if (!double.TryParse(lengthAsString, NumberStyles.Any, CultureInfo.InvariantCulture, out length))
			{
				length = double.NaN;
			}
		}
		return !double.IsNaN(length);
	}

	private static string GetColorValue(string colorValue)
	{
		return colorValue;
	}

	private static void ApplyPropertiesToTableCellElement(XmlElement htmlChildNode, XmlElement xamlTableCellElement)
	{
		xamlTableCellElement.SetAttribute("BorderThickness", "1,1,1,1");
		xamlTableCellElement.SetAttribute("BorderBrush", "Black");
		string attribute = GetAttribute(htmlChildNode, "rowspan");
		if (attribute != null)
		{
			xamlTableCellElement.SetAttribute("RowSpan", attribute);
		}
	}
}
