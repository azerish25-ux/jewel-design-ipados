using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GV.CounterSketch.Studio.Controls.MarkupConverter;

internal class HtmlParser
{
	internal const string HtmlHeader = "Version:1.0\r\nStartHTML:{0:D10}\r\nEndHTML:{1:D10}\r\nStartFragment:{2:D10}\r\nEndFragment:{3:D10}\r\nStartSelection:{4:D10}\r\nEndSelection:{5:D10}\r\n";

	internal const string HtmlStartFragmentComment = "<!--StartFragment-->";

	internal const string HtmlEndFragmentComment = "<!--EndFragment-->";

	internal const string XhtmlNamespace = "http://www.w3.org/1999/xhtml";

	private readonly HtmlLexicalAnalyzer _htmlLexicalAnalyzer;

	private readonly XmlDocument _document;

	private readonly Stack<XmlElement> _openedElements;

	private readonly Stack<XmlElement> _pendingInlineElements;

	private HtmlParser(string inputString)
	{
		_document = new XmlDocument();
		_openedElements = new Stack<XmlElement>();
		_pendingInlineElements = new Stack<XmlElement>();
		_htmlLexicalAnalyzer = new HtmlLexicalAnalyzer(inputString);
		_htmlLexicalAnalyzer.GetNextContentToken();
	}

	internal static XmlElement ParseHtml(string htmlString)
	{
		return new HtmlParser(htmlString).ParseHtmlContent();
	}

	internal static string ExtractHtmlFromClipboardData(string htmlDataString)
	{
		int num = htmlDataString.IndexOf("StartHTML:");
		if (num < 0)
		{
			return "ERROR: Urecognized html header";
		}
		num = int.Parse(htmlDataString.Substring(num + "StartHTML:".Length, "0123456789".Length));
		if (num < 0 || num > htmlDataString.Length)
		{
			return "ERROR: Urecognized html header";
		}
		int num2 = htmlDataString.IndexOf("EndHTML:");
		if (num2 < 0)
		{
			return "ERROR: Urecognized html header";
		}
		num2 = int.Parse(htmlDataString.Substring(num2 + "EndHTML:".Length, "0123456789".Length));
		if (num2 > htmlDataString.Length)
		{
			num2 = htmlDataString.Length;
		}
		return htmlDataString.Substring(num, num2 - num);
	}

	internal static string AddHtmlClipboardHeader(string htmlString)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = "Version:1.0\r\nStartHTML:{0:D10}\r\nEndHTML:{1:D10}\r\nStartFragment:{2:D10}\r\nEndFragment:{3:D10}\r\nStartSelection:{4:D10}\r\nEndSelection:{5:D10}\r\n".Length + 6 * ("0123456789".Length - "{0:D10}".Length);
		int num2 = num + htmlString.Length;
		int num3 = htmlString.IndexOf("<!--StartFragment-->", 0);
		num3 = ((num3 < 0) ? num : (num + num3 + "<!--StartFragment-->".Length));
		int num4 = htmlString.IndexOf("<!--EndFragment-->", 0);
		num4 = ((num4 < 0) ? num2 : (num + num4));
		stringBuilder.AppendFormat("Version:1.0\r\nStartHTML:{0:D10}\r\nEndHTML:{1:D10}\r\nStartFragment:{2:D10}\r\nEndFragment:{3:D10}\r\nStartSelection:{4:D10}\r\nEndSelection:{5:D10}\r\n", num, num2, num3, num4, num3, num4);
		stringBuilder.Append(htmlString);
		return stringBuilder.ToString();
	}

	private void InvariantAssert(bool condition, string message)
	{
		if (!condition)
		{
			throw new Exception("Assertion error: " + message);
		}
	}

	private XmlElement ParseHtmlContent()
	{
		XmlElement xmlElement = _document.CreateElement("html", "http://www.w3.org/1999/xhtml");
		OpenStructuringElement(xmlElement);
		while (_htmlLexicalAnalyzer.NextTokenType != HtmlTokenType.EOF)
		{
			if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.OpeningTagStart)
			{
				_htmlLexicalAnalyzer.GetNextTagToken();
				if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.Name)
				{
					string text = _htmlLexicalAnalyzer.NextToken.ToLower();
					_htmlLexicalAnalyzer.GetNextTagToken();
					XmlElement xmlElement2 = _document.CreateElement(text, "http://www.w3.org/1999/xhtml");
					ParseAttributes(xmlElement2);
					if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.EmptyTagEnd || HtmlSchema.IsEmptyElement(text))
					{
						AddEmptyElement(xmlElement2);
					}
					else if (HtmlSchema.IsInlineElement(text))
					{
						OpenInlineElement(xmlElement2);
					}
					else if (HtmlSchema.IsBlockElement(text) || HtmlSchema.IsKnownOpenableElement(text))
					{
						OpenStructuringElement(xmlElement2);
					}
				}
			}
			else if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.ClosingTagStart)
			{
				_htmlLexicalAnalyzer.GetNextTagToken();
				if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.Name)
				{
					string htmlElementName = _htmlLexicalAnalyzer.NextToken.ToLower();
					_htmlLexicalAnalyzer.GetNextTagToken();
					CloseElement(htmlElementName);
				}
			}
			else if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.Text)
			{
				AddTextContent(_htmlLexicalAnalyzer.NextToken);
			}
			else if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.Comment)
			{
				AddComment(_htmlLexicalAnalyzer.NextToken);
			}
			_htmlLexicalAnalyzer.GetNextContentToken();
		}
		if (xmlElement.FirstChild is XmlElement && xmlElement.FirstChild == xmlElement.LastChild && xmlElement.FirstChild.LocalName.ToLower() == "html")
		{
			xmlElement = (XmlElement)xmlElement.FirstChild;
		}
		return xmlElement;
	}

	private XmlElement CreateElementCopy(XmlElement htmlElement)
	{
		XmlElement xmlElement = _document.CreateElement(htmlElement.LocalName, "http://www.w3.org/1999/xhtml");
		for (int i = 0; i < htmlElement.Attributes.Count; i++)
		{
			XmlAttribute xmlAttribute = htmlElement.Attributes[i];
			xmlElement.SetAttribute(xmlAttribute.Name, xmlAttribute.Value);
		}
		return xmlElement;
	}

	private void AddEmptyElement(XmlElement htmlEmptyElement)
	{
		InvariantAssert(_openedElements.Count > 0, "AddEmptyElement: Stack of opened elements cannot be empty, as we have at least one artificial root element");
		_openedElements.Peek().AppendChild(htmlEmptyElement);
	}

	private void OpenInlineElement(XmlElement htmlInlineElement)
	{
		_pendingInlineElements.Push(htmlInlineElement);
	}

	private void OpenStructuringElement(XmlElement htmlElement)
	{
		if (HtmlSchema.IsBlockElement(htmlElement.LocalName))
		{
			while (_openedElements.Count > 0 && HtmlSchema.IsInlineElement(_openedElements.Peek().LocalName))
			{
				XmlElement htmlElement2 = _openedElements.Pop();
				InvariantAssert(_openedElements.Count > 0, "OpenStructuringElement: stack of opened elements cannot become empty here");
				_pendingInlineElements.Push(CreateElementCopy(htmlElement2));
			}
		}
		if (_openedElements.Count > 0)
		{
			XmlElement xmlElement = _openedElements.Peek();
			if (HtmlSchema.ClosesOnNextElementStart(xmlElement.LocalName, htmlElement.LocalName))
			{
				_openedElements.Pop();
				xmlElement = ((_openedElements.Count > 0) ? _openedElements.Peek() : null);
			}
			xmlElement?.AppendChild(htmlElement);
		}
		_openedElements.Push(htmlElement);
	}

	private bool IsElementOpened(string htmlElementName)
	{
		foreach (XmlElement openedElement in _openedElements)
		{
			if (openedElement.LocalName == htmlElementName)
			{
				return true;
			}
		}
		return false;
	}

	private void CloseElement(string htmlElementName)
	{
		InvariantAssert(_openedElements.Count > 0, "CloseElement: Stack of opened elements cannot be empty, as we have at least one artificial root element");
		if (_pendingInlineElements.Count > 0 && _pendingInlineElements.Peek().LocalName == htmlElementName)
		{
			XmlElement newChild = _pendingInlineElements.Pop();
			InvariantAssert(_openedElements.Count > 0, "CloseElement: Stack of opened elements cannot be empty, as we have at least one artificial root element");
			_openedElements.Peek().AppendChild(newChild);
		}
		else
		{
			if (!IsElementOpened(htmlElementName))
			{
				return;
			}
			while (_openedElements.Count > 1)
			{
				XmlElement xmlElement = _openedElements.Pop();
				if (xmlElement.LocalName == htmlElementName)
				{
					break;
				}
				if (HtmlSchema.IsInlineElement(xmlElement.LocalName))
				{
					_pendingInlineElements.Push(CreateElementCopy(xmlElement));
				}
			}
		}
	}

	private void AddTextContent(string textContent)
	{
		OpenPendingInlineElements();
		InvariantAssert(_openedElements.Count > 0, "AddTextContent: Stack of opened elements cannot be empty, as we have at least one artificial root element");
		XmlElement xmlElement = _openedElements.Peek();
		XmlText newChild = _document.CreateTextNode(textContent);
		xmlElement.AppendChild(newChild);
	}

	private void AddComment(string comment)
	{
		OpenPendingInlineElements();
		InvariantAssert(_openedElements.Count > 0, "AddComment: Stack of opened elements cannot be empty, as we have at least one artificial root element");
		XmlElement xmlElement = _openedElements.Peek();
		XmlComment newChild = _document.CreateComment(comment);
		xmlElement.AppendChild(newChild);
	}

	private void OpenPendingInlineElements()
	{
		if (_pendingInlineElements.Count > 0)
		{
			XmlElement xmlElement = _pendingInlineElements.Pop();
			OpenPendingInlineElements();
			InvariantAssert(_openedElements.Count > 0, "OpenPendingInlineElements: Stack of opened elements cannot be empty, as we have at least one artificial root element");
			_openedElements.Peek().AppendChild(xmlElement);
			_openedElements.Push(xmlElement);
		}
	}

	private void ParseAttributes(XmlElement xmlElement)
	{
		while (_htmlLexicalAnalyzer.NextTokenType != HtmlTokenType.EOF && _htmlLexicalAnalyzer.NextTokenType != HtmlTokenType.TagEnd && _htmlLexicalAnalyzer.NextTokenType != HtmlTokenType.EmptyTagEnd)
		{
			if (_htmlLexicalAnalyzer.NextTokenType == HtmlTokenType.Name)
			{
				string nextToken = _htmlLexicalAnalyzer.NextToken;
				_htmlLexicalAnalyzer.GetNextEqualSignToken();
				_htmlLexicalAnalyzer.GetNextAtomToken();
				string nextToken2 = _htmlLexicalAnalyzer.NextToken;
				xmlElement.SetAttribute(nextToken, nextToken2);
			}
			_htmlLexicalAnalyzer.GetNextTagToken();
		}
	}
}
