using System;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GV.CounterSketch.Common;

public class KeyProfile
{
	[Obsolete]
	public string _stullerApiBaseAddress = "https://www.stuller.com/api/v1/";

	public string _stullerApiDomainBase = "https://api.stuller.com/";

	[XmlIgnore]
	public bool IsDesignLibraryDisabled { get; set; }

	public Color AboutForegroundColor { get; set; }

	public string AboutImageFilename { get; set; }

	public Color AboutLabelForegroundColor { get; set; }

	public string ApplicationName { get; set; }

	public Color BaseBackground { get; set; }

	public bool BuyerInfoDefaultsEmpty { get; set; }

	public bool BuyerEMailHiddenOnPrompt { get; set; }

	public bool ExportDesignAllowed { get; set; }

	public int ExportDesignCountLimit { get; set; }

	public int ExportDesignCountResetIntervalDays { get; set; }

	public bool HideAdvancedStackOptions { get; set; }

	public bool HideDesignerNameText { get; set; }

	public bool HideFreehandMode { get; set; }

	public bool ForceHideFreehand { get; set; }

	public bool ForceHideCatalog { get; set; }

	public bool HideNEWRibbons { get; set; }

	public bool HideQuickEstimate { get; set; }

	public bool HidePackageInfoText { get; set; }

	public bool HideRedboxLogo { get; set; }

	public bool HideProjetPrinterSupport { get; set; }

	public bool HideSeriesNumberText { get; set; }

	public bool HideIsWhatsNew { get; set; }

	public Color HighlightAndAccent { get; set; }

	public Color HighlightForeground { get; set; }

	public string HomeButtonIconImage { get; set; }

	public string IconPath { get; set; }

	public Guid Key { get; set; }

	public int KeyProfileId { get; set; }

	public double MarkupDiamondSize1 { get; set; }

	public double MarkupDiamondSize2 { get; set; }

	public double MarkupDiamondSize3 { get; set; }

	public double MarkupDiamondSize4 { get; set; }

	public double MarkupDiamondSize5 { get; set; }

	public double MarkupGemSize1 { get; set; }

	public double MarkupGemSize2 { get; set; }

	public double MarkupGemSize3 { get; set; }

	public double MarkupGemSize4 { get; set; }

	public double MarkupGemSize5 { get; set; }

	public double MarkupProfileDefined1 { get; set; }

	public string MarkupProfileDefined1Name { get; set; }

	public double MarkupProfileDefined2 { get; set; }

	public string MarkupProfileDefined2Name { get; set; }

	public double MarkupProfileDefined3 { get; set; }

	public string MarkupProfileDefined3Name { get; set; }

	public double MarkupSetting { get; set; }

	public string OrderConfirmationPromptMessage { get; set; }

	public string ProfileName { get; set; }

	public double Range1Max { get; set; } = 2.95;

	public double Range2Max { get; set; } = 1.95;

	public double Range3Max { get; set; } = 0.95;

	public double Range4Max { get; set; } = 0.45;

	public string SplashImageFilename { get; set; }

	public string DiamondQualityLevel1 { get; set; }

	public string DiamondQualityLevel2 { get; set; }

	public string DiamondQualityLevel3 { get; set; }

	public string DiamondQualityLevel4 { get; set; }

	public string DiamondQualityLevel5 { get; set; }

	public Color TextBoxAndFrameBackground { get; set; }

	public Color ToolHeaderBackground { get; set; }

	public string TrainingMaterialsAddress { get; set; }

	public Color WindowFrameBackground { get; set; }

	[Obsolete]
	public string StullerApiBaseAddress
	{
		get
		{
			return _stullerApiBaseAddress;
		}
		set
		{
			_stullerApiBaseAddress = value;
		}
	}

	public string StullerApiDomainBase
	{
		get
		{
			return _stullerApiDomainBase;
		}
		set
		{
			_stullerApiDomainBase = value;
		}
	}

	public bool RequireEULAAgreement { get; set; }

	public void BackupSettings(string folder)
	{
		try
		{
			string text = Path.Combine(folder, "Backup_KeyProfile.xml");
			if (File.Exists(text))
			{
				File.SetAttributes(text, FileAttributes.Normal);
				File.Delete(text);
			}
			WriteSettings(text);
		}
		catch (Exception)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Error backing up Key Profile.");
		}
	}

	public static bool GetBackupSettings(string folder, out KeyProfile profile)
	{
		string text = Path.Combine(folder, "Backup_KeyProfile.xml");
		if (File.Exists(text))
		{
			profile = GetKeyProfileFromFile(text);
			return true;
		}
		profile = new KeyProfile();
		profile.SetValuesToDefaultCounterSketch();
		return false;
	}

	public static KeyProfile GetKeyProfileFromFile(string pathToFile)
	{
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(KeyProfile));
			using FileStream stream = File.OpenRead(pathToFile);
			if (xmlSerializer.Deserialize(stream) is KeyProfile result)
			{
				return result;
			}
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Error Reading Key Profile Settings from stream.  Default Settings will be used.\n" + ex.Message);
		}
		return new KeyProfile();
	}

	public static KeyProfile GetKeyProfileFromString(string keyProfileData)
	{
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(KeyProfile));
			using TextReader textReader = new StringReader(keyProfileData);
			if (xmlSerializer.Deserialize(textReader) is KeyProfile result)
			{
				return result;
			}
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Error Reading Key Profile Settings from stream.  Default Settings will be used.\n" + ex);
		}
		KeyProfile keyProfile = new KeyProfile();
		keyProfile.SetValuesToDefaultCounterSketch();
		return keyProfile;
	}

	public string GetProfileData()
	{
		XmlSerializer xmlSerializer = new XmlSerializer(GetType());
		StringBuilder stringBuilder = new StringBuilder();
		using (TextWriter textWriter = new StringWriter(stringBuilder))
		{
			xmlSerializer.Serialize(textWriter, this);
		}
		return stringBuilder.ToString();
	}

	public void SetValuesToDefaultCounterSketch()
	{
		AboutForegroundColor = Colors.White;
		AboutLabelForegroundColor = (Color)ColorConverter.ConvertFromString("#FFBBDDF6");
		BaseBackground = (Color)ColorConverter.ConvertFromString("#FF3d6db5");
		HighlightAndAccent = (Color)ColorConverter.ConvertFromString("#FF4E8499");
		TextBoxAndFrameBackground = (Color)ColorConverter.ConvertFromString("#FFCBCBCB");
		ToolHeaderBackground = (Color)ColorConverter.ConvertFromString("#FF737373");
		WindowFrameBackground = (Color)ColorConverter.ConvertFromString("#FFA1A1A1");
		ProfileName = "Default";
		KeyProfileId = 1;
		Key = new Guid("{490F217C-EFA4-4907-B93A-742AF1368D74}");
		ApplicationName = "CounterSketch Studio";
		SplashImageFilename = "CounterSketch Splash.png";
		IconPath = "CounterSketch.ico";
	}

	public void WriteSettings(string pathToSaveTo)
	{
		using FileStream stream = File.Open(pathToSaveTo, FileMode.Create, FileAccess.Write);
		new XmlSerializer(typeof(KeyProfile)).Serialize(stream, this);
	}
}
