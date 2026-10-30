using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;

namespace Localization;

public static class TranslationHelper
{
	private static readonly ResourceManager _designResourceManager;

	private static readonly ResourceManager _gemMetalStringResourceManager;

	private static readonly ResourceManager _partsResourceManager;

	private static readonly ResourceManager _renderMaterialStringResourceManager;

	private static readonly ResourceManager _stringResourceManager;

	private static readonly ResourceManager _stullerStringResourceManager;

	private static readonly Dictionary<string, Tuple<string, string>> _failedTranslationLookups;

	static TranslationHelper()
	{
		_designResourceManager = new ResourceManager(typeof(DesignStrings));
		_gemMetalStringResourceManager = new ResourceManager(typeof(GemMetalMaterials));
		_partsResourceManager = new ResourceManager(typeof(PartStrings));
		_renderMaterialStringResourceManager = new ResourceManager(typeof(RenderMaterialStrings));
		_stringResourceManager = new ResourceManager(typeof(Strings));
		_stullerStringResourceManager = new ResourceManager(typeof(StullerStrings));
		_failedTranslationLookups = new Dictionary<string, Tuple<string, string>>();
		if (_designResourceManager != null)
		{
			_designResourceManager.IgnoreCase = true;
		}
		if (_gemMetalStringResourceManager != null)
		{
			_gemMetalStringResourceManager.IgnoreCase = true;
		}
		if (_partsResourceManager != null)
		{
			_partsResourceManager.IgnoreCase = true;
		}
		if (_renderMaterialStringResourceManager != null)
		{
			_renderMaterialStringResourceManager.IgnoreCase = true;
		}
		if (_stringResourceManager != null)
		{
			_stringResourceManager.IgnoreCase = true;
		}
		if (_stullerStringResourceManager != null)
		{
			_stullerStringResourceManager.IgnoreCase = true;
		}
	}

	public static List<CultureInfo> GetAvailableCultureInfos()
	{
		List<CultureInfo> list = new List<CultureInfo>();
		CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
		CultureInfo[] array = cultures;
		foreach (CultureInfo cultureInfo in array)
		{
			try
			{
				if (_stringResourceManager.GetResourceSet(cultureInfo, createIfNotExists: true, tryParents: false) != null && !cultureInfo.EnglishName.Contains("English") && !cultureInfo.EnglishName.StartsWith("Invariant"))
				{
					list.Add(cultureInfo);
				}
			}
			catch (CultureNotFoundException)
			{
			}
		}
		CultureInfo cultureInfo2 = cultures.FirstOrDefault((CultureInfo _) => _.EnglishName.Contains("English"));
		if (cultureInfo2 != null)
		{
			list.Insert(0, cultureInfo2);
		}
		return list;
	}

	public static string GetEnglishStringFromTranslatedPartString(string translatedText)
	{
		if (string.IsNullOrWhiteSpace(translatedText))
		{
			return null;
		}
		try
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			string name = _partsResourceManager.GetResourceSet(Thread.CurrentThread.CurrentCulture, createIfNotExists: true, tryParents: true).OfType<DictionaryEntry>().FirstOrDefault((DictionaryEntry e) => e.Value.ToString() == translatedText)
				.Key.ToString();
			CultureInfo cultureInfo = new CultureInfo("en");
			Thread.CurrentThread.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
			string result = new ResourceManager(typeof(PartStrings)).GetString(name);
			Thread.CurrentThread.CurrentCulture = currentCulture;
			Thread.CurrentThread.CurrentUICulture = currentCulture;
			return result;
		}
		catch (Exception)
		{
			return translatedText;
		}
	}

	public static string GetTranslatedDesignOrPartString(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		try
		{
			string cleanResourceNameFromString = GetCleanResourceNameFromString(text);
			string text2 = _designResourceManager.GetString(cleanResourceNameFromString);
			if (text2 == null)
			{
				LogFailedTranslation("DesignResource", text, cleanResourceNameFromString);
				text2 = _partsResourceManager.GetString(cleanResourceNameFromString);
				if (text2 == null)
				{
					LogFailedTranslation("PartsResource", text, cleanResourceNameFromString);
				}
			}
			return text2 ?? text;
		}
		catch (Exception)
		{
			return text;
		}
	}

	public static string GetTranslatedDesignString(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		try
		{
			string cleanResourceNameFromString = GetCleanResourceNameFromString(text);
			string text2 = _designResourceManager.GetString(cleanResourceNameFromString);
			if (text2 == null)
			{
				LogFailedTranslation("DesignResource", text, cleanResourceNameFromString);
			}
			if (text2 == null)
			{
				text2 = text;
			}
			return text2;
		}
		catch (Exception)
		{
			return text;
		}
	}

	public static string GetTranslatedGemMaterial(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		try
		{
			string cleanResourceNameFromString = GetCleanResourceNameFromString(new CultureInfo("en-US", useUserOverride: false).TextInfo.ToTitleCase(text), returnFirst50AsKey: true, "Gem_");
			string text2 = _gemMetalStringResourceManager.GetString(cleanResourceNameFromString);
			if (text2 == null)
			{
				LogFailedTranslation("GemMetalResource", text, cleanResourceNameFromString);
			}
			if (text2 == null)
			{
				text2 = text;
			}
			return text2;
		}
		catch (Exception)
		{
			return text;
		}
	}

	public static string GetTranslatedMetalMaterial(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		try
		{
			string cleanResourceNameFromString = GetCleanResourceNameFromString(new CultureInfo("en-US", useUserOverride: false).TextInfo.ToTitleCase(text), returnFirst50AsKey: true, "Metal_");
			string text2 = _gemMetalStringResourceManager.GetString(cleanResourceNameFromString);
			if (text2 == null)
			{
				LogFailedTranslation("GemMetalResource", text, cleanResourceNameFromString);
			}
			if (text2 == null)
			{
				text2 = text;
			}
			return text2;
		}
		catch (Exception)
		{
			return text;
		}
	}

	public static string GetTranslatedPartString(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		try
		{
			string cleanResourceNameFromString = GetCleanResourceNameFromString(text);
			string text2 = _partsResourceManager.GetString(cleanResourceNameFromString);
			if (text2 == null)
			{
				LogFailedTranslation("PartsResource", text, cleanResourceNameFromString);
			}
			if (text2 == null)
			{
				text2 = text;
			}
			return text2;
		}
		catch (Exception)
		{
			return text;
		}
	}

	public static string GetTranslatedRenderMaterialString(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		try
		{
			string cleanResourceNameFromString = GetCleanResourceNameFromString(text);
			string text2 = _renderMaterialStringResourceManager.GetString(cleanResourceNameFromString);
			if (text2 == null)
			{
				LogFailedTranslation("RenderMaterialResource", text, cleanResourceNameFromString);
			}
			if (text2 == null)
			{
				text2 = text;
			}
			return text2;
		}
		catch (Exception)
		{
			return text;
		}
	}

	public static string GetTranslatedString(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		try
		{
			string cleanResourceNameFromString = GetCleanResourceNameFromString(text);
			if (cleanResourceNameFromString != null)
			{
				string text2 = _stringResourceManager.GetString(cleanResourceNameFromString);
				if (text2 == null)
				{
					LogFailedTranslation("StringResource", text, cleanResourceNameFromString);
				}
				if (text2 == null)
				{
					text2 = text;
				}
				return text2;
			}
		}
		catch (Exception)
		{
		}
		return text;
	}

	public static string GetTranslatedStullerString(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		try
		{
			string cleanResourceNameFromString = GetCleanResourceNameFromString(text);
			string text2 = _stullerStringResourceManager.GetString(cleanResourceNameFromString);
			if (text2 == null)
			{
				LogFailedTranslation("StullerStringResource", text, cleanResourceNameFromString);
			}
			if (text2 == null)
			{
				text2 = text;
			}
			return text2;
		}
		catch (Exception)
		{
			return text;
		}
	}

	public static void PrintFailedTranslations()
	{
		_failedTranslationLookups.OrderBy((KeyValuePair<string, Tuple<string, string>> _) => _.Key).ToList().ForEach(delegate
		{
		});
	}

	public static List<KeyValuePair<string, string>> ReadEmbeddedResource(CultureInfo selectedLanguage)
	{
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
		foreach (DictionaryEntry item in Strings.ResourceManager.GetResourceSet(selectedLanguage, createIfNotExists: true, tryParents: true))
		{
			list.Add(new KeyValuePair<string, string>(item.Key.ToString(), item.Value.ToString()));
		}
		return list;
	}

	public static List<KeyValuePair<string, string>> ReadStringResourceFile(string path)
	{
		if (!File.Exists(path))
		{
			return null;
		}
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
		using ResXResourceReader resXResourceReader = new ResXResourceReader(path);
		foreach (DictionaryEntry item in resXResourceReader)
		{
			list.Add(new KeyValuePair<string, string>(item.Key.ToString(), item.Value.ToString()));
		}
		return list;
	}

	public static void WriteStringResourceFile(string path, List<KeyValuePair<string, string>> strings)
	{
		if (File.Exists(path))
		{
			return;
		}
		ResXResourceWriter resxWriter = new ResXResourceWriter(path);
		try
		{
			strings.ForEach(delegate(KeyValuePair<string, string> _)
			{
				resxWriter.AddResource(_.Key, _.Value);
			});
		}
		finally
		{
			if (resxWriter != null)
			{
				((IDisposable)resxWriter).Dispose();
			}
		}
	}

	private static string GetCleanResourceNameFromString(string input, bool returnFirst50AsKey = false, string prefix = null)
	{
		try
		{
			string text = input.Replace(' ', '_');
			for (int i = 0; i < 10; i++)
			{
				if (text.StartsWith(i.ToString()))
				{
					text = text.Substring(1);
					text = GetTextOfInteger(i) + text;
				}
			}
			string text2 = Regex.Replace(text, "[^0-9a-zA-Z]+", "");
			if (prefix != null)
			{
				text2 = prefix + text2;
			}
			return returnFirst50AsKey ? text2.Substring(0, Math.Min(text2.Length, 50)) : text2;
		}
		catch (Exception)
		{
			return input;
		}
	}

	private static string GetTextOfInteger(int number)
	{
		return number switch
		{
			0 => "Zero", 
			1 => "One", 
			2 => "Two", 
			3 => "Three", 
			4 => "Four", 
			5 => "Five", 
			6 => "Six", 
			7 => "Seven", 
			8 => "Eight", 
			9 => "Nine", 
			_ => "X", 
		};
	}

	private static void LogFailedTranslation(string resourceName, string textToTranslate, string lookupUsed)
	{
		if (!string.IsNullOrWhiteSpace(textToTranslate))
		{
			string key = resourceName + lookupUsed;
			if (!_failedTranslationLookups.ContainsKey(key))
			{
				_failedTranslationLookups.Add(key, new Tuple<string, string>(lookupUsed, textToTranslate));
			}
		}
	}
}
