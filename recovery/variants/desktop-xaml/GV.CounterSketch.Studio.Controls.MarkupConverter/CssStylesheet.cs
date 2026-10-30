using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GV.CounterSketch.Studio.Controls.MarkupConverter;

internal class CssStylesheet
{
	private class StyleDefinition
	{
		public readonly string Selector;

		public readonly string Definition;

		public StyleDefinition(string selector, string definition)
		{
			Selector = selector;
			Definition = definition;
		}
	}

	private List<StyleDefinition> _styleDefinitions;

	public CssStylesheet(XmlElement htmlElement)
	{
		if (htmlElement != null)
		{
			DiscoverStyleDefinitions(htmlElement);
		}
	}

	public void AddStyleDefinition(string selector, string definition)
	{
		selector = selector.Trim().ToLower();
		definition = definition.Trim().ToLower();
		if (selector.Length == 0 || definition.Length == 0)
		{
			return;
		}
		if (_styleDefinitions == null)
		{
			_styleDefinitions = new List<StyleDefinition>();
		}
		string[] array = selector.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (text.Length > 0)
			{
				_styleDefinitions.Add(new StyleDefinition(text, definition));
			}
		}
	}

	public void DiscoverStyleDefinitions(XmlElement htmlElement)
	{
		if (htmlElement.LocalName.ToLower() == "link")
		{
			return;
		}
		if (htmlElement.LocalName.ToLower() != "style")
		{
			for (XmlNode xmlNode = htmlElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode is XmlElement)
				{
					DiscoverStyleDefinitions((XmlElement)xmlNode);
				}
			}
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (XmlNode xmlNode2 = htmlElement.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
		{
			if (xmlNode2 is XmlText || xmlNode2 is XmlComment)
			{
				stringBuilder.Append(RemoveComments(xmlNode2.Value));
			}
		}
		int i = 0;
		while (i < stringBuilder.Length)
		{
			int num = i;
			for (; i < stringBuilder.Length && stringBuilder[i] != '{'; i++)
			{
				if (stringBuilder[i] == '@')
				{
					for (; i < stringBuilder.Length && stringBuilder[i] != ';'; i++)
					{
					}
					num = i + 1;
				}
			}
			if (i < stringBuilder.Length)
			{
				int num2 = i;
				for (; i < stringBuilder.Length && stringBuilder[i] != '}'; i++)
				{
				}
				if (i - num2 > 2)
				{
					AddStyleDefinition(stringBuilder.ToString(num, num2 - num), stringBuilder.ToString(num2 + 1, i - num2 - 2));
				}
				if (i < stringBuilder.Length)
				{
					i++;
				}
			}
		}
	}

	public string GetStyle(string elementName, List<XmlElement> sourceContext)
	{
		if (_styleDefinitions != null)
		{
			for (int num = _styleDefinitions.Count - 1; num >= 0; num--)
			{
				string[] array = _styleDefinitions[num].Selector.Split(' ');
				int num2 = array.Length - 1;
				_ = sourceContext.Count;
				string selectorLevel = array[num2].Trim();
				if (MatchSelectorLevel(selectorLevel, sourceContext[sourceContext.Count - 1]))
				{
					return _styleDefinitions[num].Definition;
				}
			}
		}
		return null;
	}

	private bool MatchSelectorLevel(string selectorLevel, XmlElement xmlElement)
	{
		if (selectorLevel.Length == 0)
		{
			return false;
		}
		int num = selectorLevel.IndexOf('.');
		int num2 = selectorLevel.IndexOf('#');
		string text = null;
		string text2 = null;
		string text3 = null;
		if (num >= 0)
		{
			if (num > 0)
			{
				text3 = selectorLevel.Substring(0, num);
			}
			text = selectorLevel.Substring(num + 1);
		}
		else if (num2 >= 0)
		{
			if (num2 > 0)
			{
				text3 = selectorLevel.Substring(0, num2);
			}
			text2 = selectorLevel.Substring(num2 + 1);
		}
		else
		{
			text3 = selectorLevel;
		}
		if (text3 != null && text3 != xmlElement.LocalName)
		{
			return false;
		}
		if (text2 != null && HtmlToXamlConverter.GetAttribute(xmlElement, "id") != text2)
		{
			return false;
		}
		if (text != null && HtmlToXamlConverter.GetAttribute(xmlElement, "class") != text)
		{
			return false;
		}
		return true;
	}

	private string RemoveComments(string text)
	{
		int num = text.IndexOf("/*");
		if (num < 0)
		{
			return text;
		}
		int num2 = text.IndexOf("*/", num + 2);
		if (num2 < 0)
		{
			return text.Substring(0, num);
		}
		return text.Substring(0, num) + " " + RemoveComments(text.Substring(num2 + 2));
	}
}
