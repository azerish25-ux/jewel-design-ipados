using System;
using EventHelperLibrary;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class EventHelper
{
	public enum EventTypes
	{
		AnyGemAnyColor,
		Application_VersionChanged,
		Application_UnregisteredComputer,
		Backup_Created,
		Catalog_Created,
		Catalog_PDFOpened,
		Catalog_PrintAtFedEx,
		Customize_Axis,
		Customize_CommonCenter,
		Customize_Curves,
		Customize_DocumentSpin,
		Customize_Grid,
		Customize_ViewTop,
		Customize_ViewSide,
		Customize_ViewThrough,
		Customize_ViewPerspective,
		Customize_TryOn,
		Design_Estimated,
		Design_Exported,
		Design_Imported,
		Design_Quoted,
		Design_Opened,
		Design_Rendered,
		Design_Weighed,
		Error,
		LiveChat,
		Order_Engraving,
		Order_MoreThanARingBook,
		Order_MoreThanARingDigital,
		Part_Duplicated,
		Part_Flipped,
		Part_Imported,
		Part_Mirrored,
		Part_MetalLayerChanged,
		Part_Opened,
		Part_UserImageReplaced,
		Power_Down,
		Power_Up,
		Render_GroundPlaneOnAlpha,
		Render_Options,
		Render_Props,
		Render_PropsOnAlpha,
		Render_SaveToFolder,
		Search_CenterShape,
		Search_CenterSize,
		Search_CenterSetting,
		Search_DesignElement,
		Search_Gem1,
		Search_Gem2,
		Search_Gem3,
		Search_Keyword,
		Search_Metal1,
		Search_Metal2,
		Search_SideShape,
		Search_SideSetting,
		Search_Style,
		Search_Tile,
		Security_USBKeyUsed,
		Security_InvalidSerialCode,
		Security_StullerCredentialsInvalid,
		WhatsYourStyle,
		Design_Ordered,
		Shop_CenterShape,
		Shop_CaratRange,
		Order_Stuller_Item,
		Print3D_ProjetPrintModel,
		Print3D_ProjetBadModel,
		Security_KeyManipulated,
		Security_KeyProfileOverrideAttempted,
		Security_KeyProfileOverridden,
		Library_Updated,
		API_Design_Open,
		Feature_Export_Parts,
		Feature_Export_Design,
		Feature_Export_DesignWithParts,
		International_Export_Parts,
		International_Export_Design,
		International_Export_DesignWithParts
	}

	private static bool _enabled;

	private static EventSystem _eventSystem;

	public static void FlushEvents()
	{
		if (!_enabled)
		{
			return;
		}
		try
		{
			_eventSystem.FlushEvents();
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Unable to Flush Events: " + ex);
		}
	}

	public static void Initialize()
	{
		try
		{
			_eventSystem = new EventSystem("CounterSketchStudio", "CounterSketch Studio", Enum.GetNames(typeof(EventTypes)));
			_enabled = true;
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Unable to Initialize Event System: " + ex);
		}
	}

	public static void RecordEvent(EventTypes eventType, string data = null, string value = null)
	{
		if (!_enabled)
		{
			return;
		}
		try
		{
			_eventSystem.RecordEvent(eventType, UIHelper.CounterSketchOptions.KeyId, UIHelper.VersionString, data, value);
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Unable to Record Event: " + ex);
		}
	}

	public static void RecordImmediateEvent(EventTypes eventType, string data = null, string value = null)
	{
		if (!_enabled)
		{
			return;
		}
		try
		{
			_eventSystem.RecordImmediateEvent(eventType, UIHelper.CounterSketchOptions.KeyId, UIHelper.VersionString, data, value);
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Unable to Record Immediate Event: " + ex);
		}
	}
}
