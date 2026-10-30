using System;
using System.Windows;
using System.Windows.Media;

namespace GV.CounterSketch.Shared.Utilities.Helpers;

public static class UISliderIconHelper
{
	public static Color ColorDefault = default(Color);

	public static SolidColorBrush ControlColorBrush = new SolidColorBrush();

	public static Color SolidColor;

	private static bool isSliderIconResourceDictionaryLoaded;

	public static string GetIconForUIControl(string title, string categoryName)
	{
		if (!isSliderIconResourceDictionaryLoaded)
		{
			ResourceDictionary item = new ResourceDictionary
			{
				Source = new Uri("pack://application:,,,/SharedTypes;component/ResourceDictionaries/DesignStackSliderIcons.xaml", UriKind.RelativeOrAbsolute)
			};
			Application.Current.Resources.MergedDictionaries.Add(item);
			isSliderIconResourceDictionaryLoaded = true;
		}
		string filePath = string.Empty;
		string empty = string.Empty;
		string empty2 = string.Empty;
		if (title != null && title.Contains(":"))
		{
			empty = ((!string.IsNullOrWhiteSpace(title)) ? title.Substring(0, title.IndexOf(':')) : string.Empty);
			empty2 = ((!string.IsNullOrWhiteSpace(title)) ? title.Substring(title.IndexOf(':') + 1) : string.Empty);
			empty = empty.Replace(" ", "").Replace("-", "").Replace("/", "")
				.ToLowerInvariant();
			empty2 = empty2.Replace(" ", "").Replace("-", "").Replace("/", "")
				.ToLowerInvariant();
		}
		else if (title != null && title.Contains("-"))
		{
			empty = ((!string.IsNullOrWhiteSpace(title)) ? title.Substring(0, title.IndexOf('-')) : string.Empty);
			empty2 = ((!string.IsNullOrWhiteSpace(title)) ? title.Substring(title.IndexOf('-') + 1) : string.Empty);
			empty = empty.Replace(" ", "").Replace("-", "").Replace("/", "")
				.ToLowerInvariant();
			empty2 = empty2.Replace(" ", "").Replace("-", "").Replace("/", "")
				.ToLowerInvariant();
		}
		else
		{
			empty = categoryName?.Replace(" ", "").Replace("-", "").Replace("/", "")
				.ToLowerInvariant();
			empty2 = title?.Replace(" ", "").Replace("-", "").Replace("/", "")
				.ToLowerInvariant();
			empty = empty?.Replace(" ", "").Replace("-", "").Replace("/", "")
				.ToLowerInvariant();
			empty2 = empty2?.Replace(" ", "").Replace("-", "").Replace("/", "")
				.ToLowerInvariant();
		}
		CheckSliderIcon(empty, "Setting", empty2, "CenterProngMoveLeftRight", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "GalleryRailMoveFrontBack", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "GalleryRailMoveLeftRight", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "CenterProngMoveFrontBack", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "OuterProngMoveFrontBack", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "OuterProngMoveLeftRight", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ChannelEndBottomLength", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngVerticalLocation", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "GalleryRailMoveUpDown", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "BezelBottomThickness", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngBottomThickness", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "GalleryRailThickness", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "GalleryRailDiameter", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "GalleryRailPosition", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "BezelHeightRelative", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "GalleryRailLocation", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngProfileHeight", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ChannelBottomShape", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "VProngBottomLength", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "GalleryRailHeight", ref filePath);
		CheckSliderIcon(empty, "Pendant", empty2, "BailOpeningLength", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngProfileWidth", ref filePath);
		CheckSliderIcon(empty, "Pendant", empty2, "BailOpeningDepth", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngEndLocation", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngBottomWidth", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "GalleryRailMoveY", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "GalleryRailMoveX", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "GalleryRailWidth", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "GalleryRailCount", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ChannelEndLength", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "CenterProngMoveX", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "CenterProngMoveY", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "GalleryRailDepth", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngMoveUpDown", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ChannelEndShape", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "GalleryRailBend", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ChannelEndAngle", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "OuterProngMoveY", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "OuterProngMoveX", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "CenterProngSize", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "BezelDomeHeight", ref filePath);
		CheckSliderIcon(empty, "Channel", empty2, "EndBottomLength", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "BezelSeatLength", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngThickness", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "BezelThickness", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "BezelSeatDepth", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "BezelSeatAngle", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngLocation", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ChannelHeight", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProfileHeight", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngDiameter", ref filePath);
		CheckSliderIcon(empty, "Rondell", empty2, "WallThickness", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ChannelLength", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "BezelLocation", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProfileWidth", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "VProngLength", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ChannelDepth", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ChannelWidth", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngEndMove", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngOverlap", ref filePath);
		CheckSliderIcon(empty, "Pendant", empty2, "ProfileWidth", ref filePath);
		CheckSliderIcon(empty, "Rondell", empty2, "InsideHeight", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ChannelTaper", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngLayout", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngHeight", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "NotchLength", ref filePath);
		CheckSliderIcon(empty, "Channel", empty2, "BottomShape", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngLength", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngRotate", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "BezelHeight", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngStyle", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngWidth", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngNudge", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "NotchWidth", ref filePath);
		CheckSliderIcon(empty, "Pendant", empty2, "DropLength", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "BezelAngle", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngTwist", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngTaper", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngMove", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngBend", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngDome", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngTilt", ref filePath);
		CheckSliderIcon(empty, "Pendant", empty2, "DropWidth", ref filePath);
		CheckSliderIcon(empty, "Channel", empty2, "EndLength", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngSize", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "ProngRoll", ref filePath);
		CheckSliderIcon(empty, "Channel", empty2, "EndShape", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "BarWidth", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "Diameter", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "Location", ref filePath);
		CheckSliderIcon(empty, "Channel", empty2, "EndAngle", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "Overlap", ref filePath);
		CheckSliderIcon(empty, "Channel", empty2, "Length", ref filePath);
		CheckSliderIcon(empty, "Pendant", empty2, "Length", ref filePath);
		CheckSliderIcon(empty, "Pendant", empty2, "Height", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "Height", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "Spread", ref filePath);
		CheckSliderIcon(empty, "Channel", empty2, "Height", ref filePath);
		CheckSliderIcon(empty, "Pendant", empty2, "Scale", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "Width", ref filePath);
		CheckSliderIcon(empty, "Channel", empty2, "Taper", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "Style", ref filePath);
		CheckSliderIcon(empty, "Channel", empty2, "Depth", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "Angle", ref filePath);
		CheckSliderIcon(empty, "Channel", empty2, "Width", ref filePath);
		CheckSliderIcon(empty, "Pendant", empty2, "Width", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "Bend", ref filePath);
		CheckSliderIcon(empty, "Setting", empty2, "Move", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "BeadHeightAbove", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "ProfilePosition", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "InsideDiameter", ref filePath);
		CheckSliderIcon(empty, "Signet", empty2, "InsetDiameter", ref filePath);
		CheckSliderIcon(empty, "Collar", empty2, "OutsideHeight", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "ProfileHeight", ref filePath);
		CheckSliderIcon(empty, "Collar", empty2, "DomeThickness", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "MilgrainSize", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "InsideHeight", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "ProfileWidth", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "BeadSpacing", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "ProfileSize", ref filePath);
		CheckSliderIcon(empty, "Collar", empty2, "OusideWidth", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "InsideWidth", ref filePath);
		CheckSliderIcon(empty, "Signet", empty2, "InsetLength", ref filePath);
		CheckSliderIcon(empty, "Signet", empty2, "InsetWidth", ref filePath);
		CheckSliderIcon(empty, "Collar", empty2, "RailHeight", ref filePath);
		CheckSliderIcon(empty, "Collar", empty2, "BottomTrim", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "BeadRotate", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "BeadLength", ref filePath);
		CheckSliderIcon(empty, "Collar", empty2, "RailWidth", ref filePath);
		CheckSliderIcon(empty, "Collar", empty2, "Thickness", ref filePath);
		CheckSliderIcon(empty, "Collar", empty2, "TopHeight", ref filePath);
		CheckSliderIcon(empty, "Signet", empty2, "TopLength", ref filePath);
		CheckSliderIcon(empty, "Signet", empty2, "TopHeight", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "BeadTwist", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "Diameter", ref filePath);
		CheckSliderIcon(empty, "Signet", empty2, "TopWidth", ref filePath);
		CheckSliderIcon(empty, "Collar", empty2, "TopWidth", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "BeadDome", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "BeadSize", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "Length", ref filePath);
		CheckSliderIcon(empty, "Collar", empty2, "Height", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "Width", ref filePath);
		CheckSliderIcon(empty, "Signet", empty2, "Inset", ref filePath);
		CheckSliderIcon(empty, "Collar", empty2, "Width", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "Start", ref filePath);
		CheckSliderIcon(empty, "Collar", empty2, "Move", ref filePath);
		CheckSliderIcon(empty, "Detail", empty2, "End", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "ProfileMoveFrontBack", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "ProfileMoveSideSide", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "ProfileOrientation", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "MilgrainStartEnd", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "MilgrainPosition", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "ProfilePosition", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "BottomThickness", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "ProfileLocation", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "SegmentSpacing", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "MoveFrontBack", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "SideThickness", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "MilgrainStart", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "MilgrainCurve", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "ProfileTwist", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "DesignLength", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "BridgeLength", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "SegmentCount", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "TopThickness", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "BridgeHeight", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "ProfileRoll", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "DesignWidth", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "MilgrainEnd", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "ProfileTilt", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "BridgeWidth", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "ProfileMove", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "BottomWidth", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "MoveUpDown", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "EndCapDome", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "BridgeSpan", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "BridgeMove", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "GemOffset", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "TopWidth", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "Overlap", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "Spread", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "Height", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "Taper", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "Split", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "Angle", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "Blend", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "Width", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "Bend", ref filePath);
		CheckSliderIcon(empty, "Shank", empty2, "Span", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "ProfileMoveFrontBack", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "ProfileMoveSideSide", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "ProfileOrientation", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "VerticalPlacement", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "MilgrainStartEnd", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "MilgrainPosition", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "BottomThickness", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "ProfileDistance", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "ProfileLocation", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "ProfilePosition", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "SegmentSpacing", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "InsideDiameter", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "WallThickness", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "MilgrainStart", ref filePath);
		CheckSliderIcon(empty, "Bail", empty2, "ProfileHeight", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "MilgrainCurve", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "SideThickness", ref filePath);
		CheckSliderIcon(empty, "Bail", empty2, "BottomOpening", ref filePath);
		CheckSliderIcon(empty, "Bail", empty2, "OpeningLength", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "TopThickness", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "BridgeHeight", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "InsideHeight", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "BridgeLength", ref filePath);
		CheckSliderIcon(empty, "Bail", empty2, "ProfileWidth", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "ProfileTwist", ref filePath);
		CheckSliderIcon(empty, "Bead", empty2, "HolePosition", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "SegmentCount", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "DesignLength", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "ProfileMove", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "ProfileTilt", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "DesignWidth", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "BottomWidth", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "BridgeWidth", ref filePath);
		CheckSliderIcon(empty, "Bead", empty2, "CutoutDepth", ref filePath);
		CheckSliderIcon(empty, "Bead", empty2, "CutoutWidth", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "ProfileRoll", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "MilgrainEnd", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "SupportBend", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "InsideWidth", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "BridgeSpan", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "EndCapDome", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "BridgeMove", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "MoveUpDown", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "GemOffset", ref filePath);
		CheckSliderIcon(empty, "Bead", empty2, "Thickness", ref filePath);
		CheckSliderIcon(empty, "Bead", empty2, "Diameter", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "Diameter", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "Distance", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "Location", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "TopWidth", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "Opening", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "Overlap", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "Offset", ref filePath);
		CheckSliderIcon(empty, "Bail", empty2, "Length", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "Height", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "Height", ref filePath);
		CheckSliderIcon(empty, "Bead", empty2, "Length", ref filePath);
		CheckSliderIcon(empty, "Bead", empty2, "Height", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "Spread", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "Angle", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "Angle", ref filePath);
		CheckSliderIcon(empty, "Bead", empty2, "Width", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "Width", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "Width", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "Split", ref filePath);
		CheckSliderIcon(empty, "Bail", empty2, "Depth", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "Taper", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "Blend", ref filePath);
		CheckSliderIcon(empty, "Bail", empty2, "Width", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "Size", ref filePath);
		CheckSliderIcon(empty, "Halo", empty2, "Move", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "Span", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "Bend", ref filePath);
		CheckSliderIcon(empty, "Band", empty2, "Bow", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "BaguetteGirdleThickness", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "BaguetteBottomWidth", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "BaguetteThickness", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "BaguetteTopWidth", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "VerticalLocation", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "BaguettePosition", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "BaguetteHeight", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "BaguetteLength", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "ClusterOffset", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "WallThickness", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "MoveFrontBack", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "MoveLeftRight", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "BaguetteAngle", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "SpacingMethod", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "BaguetteTilt", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "NumberOfRows", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "LocationEnd", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "ClusterSize", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "EndDiameter", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "MoveUpDown", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Placement", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "StartSize", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Position", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Location", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Rotation", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "EndSize", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Spacing", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Rotate", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Height", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Offset", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Start", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "MoveX", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Twist", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Nudge", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Angle", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Taper", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "MoveZ", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "MoveY", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Count", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Roll", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Tilt", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Move", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "Span", ref filePath);
		CheckSliderIcon(empty, "Gem", empty2, "End", ref filePath);
		if (string.IsNullOrWhiteSpace(filePath))
		{
			SetColors((Color)ColorConverter.ConvertFromString(GetColorBrushByCategoryName(GetStandardCategoryName(categoryName))), setDefaultColor: true);
		}
		if (!string.IsNullOrWhiteSpace(filePath))
		{
			Application.Current.TryFindResource(filePath);
		}
		return filePath;
	}

	private static void CheckSliderIcon(string uiCategoryName, string categoryName, string uiControlName, string controlName, ref string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath) && uiCategoryName != null)
		{
			if (uiCategoryName.EndsWith("s"))
			{
				uiCategoryName = uiCategoryName.Substring(0, uiCategoryName.Length - 1);
			}
			if (uiCategoryName.Contains(categoryName.ToLowerInvariant()) && uiControlName.Contains(controlName.ToLowerInvariant()))
			{
				filePath = $"CSIcon{categoryName}_{controlName}";
				SetColors((Color)ColorConverter.ConvertFromString(GetColorBrushByCategoryName(categoryName)), setDefaultColor: true);
			}
		}
	}

	private static string GetColorBrushByCategoryName(string categoryName)
	{
		string result = "#000000";
		switch (categoryName)
		{
		case "Gem":
			result = "#A1C7FF";
			break;
		case "Pen":
			result = "#94EECE";
			break;
		case "Rondell":
			result = "#EFDB93";
			break;
		case "Channel":
			result = "#DCA673";
			break;
		case "Pendant":
			result = "#94EECE";
			break;
		case "Collar":
			result = "#9BFB8C";
			break;
		case "Setting":
			result = "#A848C6";
			break;
		case "Bead":
			result = "#EFDB93";
			break;
		case "Bail":
			result = "#94EECE";
			break;
		case "Detail":
			result = "#EFDB93";
			break;
		case "Shank":
			result = "#94EECE";
			break;
		case "Halo":
			result = "#A1C7FF";
			break;
		case "Band":
			result = "#94EECE";
			break;
		}
		return result;
	}

	private static string GetStandardCategoryName(string categoryName)
	{
		if (categoryName.ToLowerInvariant().Contains("gem"))
		{
			return "Gem";
		}
		if (categoryName.ToLowerInvariant().Contains("pen"))
		{
			return "Pen";
		}
		if (categoryName.ToLowerInvariant().Contains("Rondell"))
		{
			return "Rondell";
		}
		if (categoryName.ToLowerInvariant().Contains("channel"))
		{
			return "Channel";
		}
		if (categoryName.ToLowerInvariant().Contains("pendant"))
		{
			return "Pendant";
		}
		if (categoryName.ToLowerInvariant().Contains("setting"))
		{
			return "Setting";
		}
		if (categoryName.ToLowerInvariant().Contains("bead"))
		{
			return "Bead";
		}
		if (categoryName.ToLowerInvariant().Contains("collar"))
		{
			return "Collar";
		}
		if (categoryName.ToLowerInvariant().Contains("bail"))
		{
			return "Bail";
		}
		if (categoryName.ToLowerInvariant().Contains("detail"))
		{
			return "Detail";
		}
		if (categoryName.ToLowerInvariant().Contains("shank"))
		{
			return "Shank";
		}
		if (categoryName.ToLowerInvariant().Contains("halo"))
		{
			return "Halo";
		}
		if (categoryName.ToLowerInvariant().Contains("band"))
		{
			return "Band";
		}
		return string.Empty;
	}

	private static void SetColors(Color color, bool setDefaultColor = false)
	{
		if (setDefaultColor || !(SolidColor != ColorDefault))
		{
			ControlColorBrush = new SolidColorBrush(color);
			SolidColor = color;
		}
	}
}
