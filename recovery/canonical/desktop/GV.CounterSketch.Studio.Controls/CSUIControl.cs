using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Shared.Utilities.Helpers;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.Controls;

public class CSUIControl : UserControl
{
	public static readonly DependencyProperty CategoryNameProperty = DependencyProperty.Register("CategoryName", typeof(string), typeof(CSUIControl), new PropertyMetadata(string.Empty));

	public static readonly DependencyProperty ControlColorBrushProperty = DependencyProperty.Register("ControlColorBrush", typeof(SolidColorBrush), typeof(CSUIControl), new UIPropertyMetadata(new SolidColorBrush(Colors.White)));

	public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register("DisplayText", typeof(string), typeof(CSUIControl));

	public static readonly DependencyProperty DisplayTitleProperty = DependencyProperty.Register("DisplayTitle", typeof(string), typeof(CSUIControl));

	public static readonly DependencyProperty GemCaratProperty = DependencyProperty.Register("GemCarat", typeof(string), typeof(CSUIControl));

	public static readonly DependencyProperty GemDepthProperty = DependencyProperty.Register("GemDepth", typeof(double), typeof(CSUIControl), new UIPropertyMetadata(0.0));

	public static readonly DependencyProperty GemHeightProperty = DependencyProperty.Register("GemHeight", typeof(double), typeof(CSUIControl), new UIPropertyMetadata(0.0));

	public static readonly DependencyProperty GemPercentageProperty = DependencyProperty.Register("GemPercentage", typeof(double), typeof(CSUIControl), new UIPropertyMetadata(0.0));

	public static readonly DependencyProperty GemsProperty = DependencyProperty.Register("Gems", typeof(ObservableCollection<GemDataItem>), typeof(CSUIControl));

	public static readonly DependencyProperty GemTableIndexProperty = DependencyProperty.Register("GemTableIndex", typeof(int), typeof(CSUIControl), new UIPropertyMetadata(0));

	public static readonly DependencyProperty GemTableItemCountProperty = DependencyProperty.Register("GemTableItemCount", typeof(int), typeof(CSUIControl), new UIPropertyMetadata(0));

	public static readonly DependencyProperty GemWidthProperty = DependencyProperty.Register("GemWidth", typeof(double), typeof(CSUIControl), new UIPropertyMetadata(0.0));

	public static readonly DependencyProperty ListCountProperty = DependencyProperty.Register("ListCount", typeof(int), typeof(CSUIControl));

	public static readonly DependencyProperty ListIndexProperty = DependencyProperty.Register("ListIndex", typeof(int), typeof(CSUIControl));

	public static readonly DependencyProperty ListItemStringsProperty = DependencyProperty.Register("ListItemStrings", typeof(List<string>), typeof(CSUIControl), new PropertyMetadata(new List<string>()));

	public static readonly DependencyProperty ListItemsProperty = DependencyProperty.Register("ListItems", typeof(ObservableCollection<ValueListDisplayItem>), typeof(CSUIControl), new PropertyMetadata(new ObservableCollection<ValueListDisplayItem>()));

	public static readonly DependencyProperty HeadStyleListItemsProperty = DependencyProperty.Register("HeadStyleListItems", typeof(ObservableCollection<ValueListDisplayItem>), typeof(CSUIControl), new PropertyMetadata(new ObservableCollection<ValueListDisplayItem>()));

	public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double), typeof(CSUIControl));

	public static readonly DependencyProperty MaxValue2Property = DependencyProperty.Register("MaxValue2", typeof(double), typeof(CSUIControl));

	public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(double), typeof(CSUIControl));

	public static readonly DependencyProperty MinValue2Property = DependencyProperty.Register("MinValue2", typeof(double), typeof(CSUIControl));

	public static readonly DependencyProperty OnOffProperty = DependencyProperty.Register("OnOff", typeof(bool), typeof(CSUIControl));

	public static readonly DependencyProperty ProfilePolyLineColorProperty = DependencyProperty.Register("ProfilePolyLineColor", typeof(System.Windows.Media.Color), typeof(CSUIControl), new UIPropertyMetadata(Colors.DodgerBlue));

	public static readonly DependencyProperty ProfilesProperty = DependencyProperty.Register("Profiles", typeof(ObservableCollection<ValueChooserDataItem>), typeof(CSUIControl));

	public static readonly DependencyProperty ShowInSetupProperty = DependencyProperty.Register("ShowInSetup", typeof(bool), typeof(CSUIControl));

	public static readonly DependencyProperty StepValueProperty = DependencyProperty.Register("StepValue", typeof(double), typeof(CSUIControl));

	public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(CSUIControl));

	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(CSUIControl));

	public static readonly DependencyProperty Value2Property = DependencyProperty.Register("Value2", typeof(double), typeof(CSUIControl));

	public static readonly DependencyProperty ValueGenericProperty = DependencyProperty.Register("ValueGeneric", typeof(object), typeof(CSUIControl));

	private readonly System.Windows.Media.Color _colorDefault = System.Windows.Media.Color.FromRgb(64, 64, 64);

	private readonly System.Drawing.Color _colorOldHighlight = System.Drawing.Color.FromArgb(255, 255, 26, 100);

	public static readonly DependencyProperty HasWarningMessageProperty = DependencyProperty.Register("HasWarningMessage", typeof(bool), typeof(CSUIControl), new PropertyMetadata(false));

	public static readonly DependencyProperty WarningMessageProperty = DependencyProperty.Register("WarningMessage", typeof(string), typeof(CSUIControl), new PropertyMetadata(string.Empty));

	public string CategoryName
	{
		get
		{
			return (string)GetValue(CategoryNameProperty);
		}
		set
		{
			SetValue(CategoryNameProperty, value);
		}
	}

	public SolidColorBrush ControlColorBrush
	{
		get
		{
			return (SolidColorBrush)GetValue(ControlColorBrushProperty);
		}
		set
		{
			SetValue(ControlColorBrushProperty, value);
			ControlColorBrush.Freeze();
		}
	}

	public string DisplayText
	{
		get
		{
			return (string)GetValue(DisplayTextProperty);
		}
		set
		{
			SetValue(DisplayTextProperty, value);
		}
	}

	public string DisplayTitle
	{
		get
		{
			return (string)GetValue(DisplayTitleProperty);
		}
		set
		{
			SetValue(DisplayTitleProperty, value);
		}
	}

	public string GemCarat
	{
		get
		{
			return (string)GetValue(GemCaratProperty);
		}
		set
		{
			SetValue(GemCaratProperty, value);
		}
	}

	public double GemDepth
	{
		get
		{
			return (double)GetValue(GemDepthProperty);
		}
		set
		{
			SetValue(GemDepthProperty, value);
		}
	}

	public double GemHeight
	{
		get
		{
			return (double)GetValue(GemHeightProperty);
		}
		set
		{
			SetValue(GemHeightProperty, value);
		}
	}

	public double GemPercentage
	{
		get
		{
			return (double)GetValue(GemPercentageProperty);
		}
		set
		{
			SetValue(GemPercentageProperty, value);
		}
	}

	public ObservableCollection<GemDataItem> Gems
	{
		get
		{
			return (ObservableCollection<GemDataItem>)GetValue(GemsProperty);
		}
		set
		{
			SetValue(GemsProperty, value);
		}
	}

	public int GemTableIndex
	{
		get
		{
			return (int)GetValue(GemTableIndexProperty);
		}
		set
		{
			SetValue(GemTableIndexProperty, value);
		}
	}

	public int GemTableItemCount
	{
		get
		{
			return (int)GetValue(GemTableItemCountProperty);
		}
		set
		{
			SetValue(GemTableItemCountProperty, value);
		}
	}

	public double GemWidth
	{
		get
		{
			return (double)GetValue(GemWidthProperty);
		}
		set
		{
			SetValue(GemWidthProperty, value);
		}
	}

	public int ListCount
	{
		get
		{
			return (int)GetValue(ListCountProperty);
		}
		set
		{
			SetValue(ListCountProperty, value);
		}
	}

	public int ListIndex
	{
		get
		{
			return (int)GetValue(ListIndexProperty);
		}
		set
		{
			SetValue(ListIndexProperty, value);
		}
	}

	public List<string> ListItemStrings
	{
		get
		{
			return (List<string>)GetValue(ListItemStringsProperty);
		}
		set
		{
			SetValue(ListItemStringsProperty, value);
		}
	}

	public ObservableCollection<ValueListDisplayItem> ListItems
	{
		get
		{
			return (ObservableCollection<ValueListDisplayItem>)GetValue(ListItemsProperty);
		}
		set
		{
			SetValue(ListItemsProperty, value);
		}
	}

	public ObservableCollection<ValueListDisplayItem> HeadStyleListItems
	{
		get
		{
			return (ObservableCollection<ValueListDisplayItem>)GetValue(HeadStyleListItemsProperty);
		}
		set
		{
			SetValue(HeadStyleListItemsProperty, value);
		}
	}

	public double MaxValue
	{
		get
		{
			return (double)GetValue(MaxValueProperty);
		}
		set
		{
			SetValue(MaxValueProperty, value);
		}
	}

	public double MaxValue2
	{
		get
		{
			return (double)GetValue(MaxValue2Property);
		}
		set
		{
			SetValue(MaxValue2Property, value);
		}
	}

	public double MinValue
	{
		get
		{
			return (double)GetValue(MinValueProperty);
		}
		set
		{
			SetValue(MinValueProperty, value);
		}
	}

	public double MinValue2
	{
		get
		{
			return (double)GetValue(MinValue2Property);
		}
		set
		{
			SetValue(MinValue2Property, value);
		}
	}

	public bool OnOff
	{
		get
		{
			return (bool)GetValue(OnOffProperty);
		}
		set
		{
			SetValue(OnOffProperty, value);
		}
	}

	public System.Windows.Media.Color ProfilePolyLineColor
	{
		get
		{
			return (System.Windows.Media.Color)GetValue(ProfilePolyLineColorProperty);
		}
		set
		{
			SetValue(ProfilePolyLineColorProperty, value);
		}
	}

	public ObservableCollection<ValueChooserDataItem> Profiles
	{
		get
		{
			return (ObservableCollection<ValueChooserDataItem>)GetValue(ProfilesProperty);
		}
		set
		{
			SetValue(ProfilesProperty, value);
		}
	}

	public double StepValue
	{
		get
		{
			return (double)GetValue(StepValueProperty);
		}
		set
		{
			if (value <= 0.0)
			{
				value = 0.0001;
			}
			SetValue(StepValueProperty, value);
		}
	}

	public string Title
	{
		get
		{
			return (string)GetValue(TitleProperty);
		}
		set
		{
			SetValue(TitleProperty, value);
			if (LocalUIControl.CurrentUIControlType != UIControlType.CSValueOnOff && !string.IsNullOrWhiteSpace(value) && value.Contains(":"))
			{
				DisplayTitle = Title.Substring(Title.IndexOf(":", StringComparison.Ordinal) + 1).TrimStart();
			}
			else
			{
				DisplayTitle = value;
			}
		}
	}

	public double Value
	{
		get
		{
			return (double)GetValue(ValueProperty);
		}
		set
		{
			SetValue(ValueProperty, value);
		}
	}

	public double Value2
	{
		get
		{
			return (double)GetValue(Value2Property);
		}
		set
		{
			SetValue(Value2Property, value);
		}
	}

	public object ValueGeneric
	{
		get
		{
			return GetValue(ValueGenericProperty);
		}
		set
		{
			SetValue(ValueGenericProperty, value);
		}
	}

	public IEnumerable<ComponentsEnum> AssignedDisplayComponents => LocalUIControl.AssignedDisplayComponents;

	public string PathToIconImage => GetIconForUIControl();

	public string SpecKey
	{
		get
		{
			if (string.IsNullOrEmpty(LocalUIControl.ControlSpecifierValue))
			{
				return string.Empty;
			}
			return LocalUIControl.ControlSpecifier;
		}
	}

	public string SpecKeyTrimNumber
	{
		get
		{
			if (string.IsNullOrEmpty(LocalUIControl.ControlSpecifierValue))
			{
				return string.Empty;
			}
			return LocalUIControl.ControlSpecifier;
		}
	}

	public string SpecValue
	{
		get
		{
			if (string.IsNullOrEmpty(LocalUIControl.ControlSpecifier))
			{
				return string.Empty;
			}
			return LocalUIControl.ControlSpecifierValue;
		}
	}

	public string SpecValueTrimNumber
	{
		get
		{
			string[] array = ((!string.IsNullOrEmpty(LocalUIControl.ControlSpecifier)) ? LocalUIControl.ControlSpecifierValue : string.Empty).Split(' ');
			StringBuilder stringBuilder = new StringBuilder();
			int result;
			for (int i = 0; i <= array.Length - 1 && (i > array.Length - 1 || !int.TryParse(array[i], out result)); i++)
			{
				stringBuilder.Append(array[i] + " ");
			}
			return stringBuilder.ToString().Trim();
		}
	}

	public List<GemTableSizeInfo> GemTableSizeInfoList { get; set; }

	public System.Windows.Media.Color SolidColor { get; set; }

	public int SortIndex { get; set; }

	protected internal bool AllowSystemKeyCapture { get; set; }

	protected internal double DefaultControlValue { get; set; }

	protected internal double DefaultControlValue2 { get; set; }

	protected internal object DefaultControlValueGeneric { get; set; }

	protected internal bool IsInitializing { get; set; }

	protected internal bool IsMouseDown { get; set; }

	protected internal bool IsUpdatingControlSource { get; set; }

	protected internal UIControl LocalUIControl { get; set; }

	protected internal double OriginalStep { get; set; }

	public bool HasWarningMessage
	{
		get
		{
			return (bool)GetValue(HasWarningMessageProperty);
		}
		set
		{
			SetValue(HasWarningMessageProperty, value);
		}
	}

	public string WarningMessage
	{
		get
		{
			return (string)GetValue(WarningMessageProperty);
		}
		set
		{
			SetValue(WarningMessageProperty, value);
		}
	}

	public CSUIControl()
	{
		base.MouseEnter += CSUIControl_MouseEnter;
		base.MouseLeave += CSUIControl_MouseLeave;
	}

	public void DisableUIControl()
	{
		if (LocalUIControl.IsUIDisabled)
		{
			base.IsHitTestVisible = false;
			base.Opacity = 0.5;
		}
		else
		{
			base.IsHitTestVisible = true;
			base.Opacity = 1.0;
		}
	}

	public virtual void SetControl()
	{
	}

	public virtual void SetGraphValue(bool isQuickSet)
	{
		Messenger.Default.Send<NotificationMessage<UIControl>, DesignStackViewModel>(isQuickSet ? new NotificationMessage<UIControl>(LocalUIControl, "SetUIControlQuick") : new NotificationMessage<UIControl>(LocalUIControl, "SetUIControlFull"));
	}

	public virtual void UpdateUIControlSource(UIControl uiControl)
	{
		LocalUIControl = uiControl;
		DisableUIControl();
	}

	protected string CaratToQuarterCaratDisplay(double carat)
	{
		if (LocalUIControl.IsLargeGemEnabled && UIHelper.CounterSketchOptions.IsQuarterCaratLargeStoneRoundingEnabled)
		{
			return DoubleToFractionHelper.Convert((decimal)carat);
		}
		return carat.ToString("##.##");
	}

	protected string GetIconForUIControl()
	{
		string englishControlName = LocalUIControl.EnglishControlName;
		string englishCategoryName = LocalUIControl.EnglishCategoryName;
		string iconForUIControl = UISliderIconHelper.GetIconForUIControl(englishControlName, englishCategoryName);
		SolidColor = UISliderIconHelper.SolidColor;
		ControlColorBrush = UISliderIconHelper.ControlColorBrush;
		return iconForUIControl;
	}

	protected double RoundToQuaterIncrement(double original)
	{
		return Math.Round(original * 4.0, MidpointRounding.ToEven) / 4.0;
	}

	private void CSUIControl_MouseEnter(object sender, MouseEventArgs e)
	{
		if (!UIHelper.CounterSketchOptions.DisableViewportHighlight && UIHelper.BlockInstanceGuidToHighlight != LocalUIControl.BlockInstanceGuid)
		{
			UIHelper.HighlightTimer.Change(400, 0);
			UIHelper.CoreDataService.ClearHighlightedGems();
			UIHelper.BlockInstanceGuidToHighlight = LocalUIControl.BlockInstanceGuid;
			System.Drawing.Color color = System.Drawing.Color.FromArgb(SolidColor.A, SolidColor.R, SolidColor.G, SolidColor.B);
			if (color.R == 0 && color.G == 0 && color.B == 0)
			{
				color = _colorOldHighlight;
			}
			UIHelper.CoreDataService.ViewportSetHighlightColor(color);
		}
	}

	private void CSUIControl_MouseLeave(object sender, MouseEventArgs e)
	{
		if (UIHelper.BlockInstanceGuidToHighlight == LocalUIControl.BlockInstanceGuid)
		{
			UIHelper.BlockInstanceGuidToHighlight = Guid.Empty;
		}
		UIHelper.HighlightTimer.Change(0, 0);
		UIHelper.CoreDataService.HighlightRhinoDocumentGraphItem(LocalUIControl.BlockInstanceGuid, isHighlighted: false);
	}
}
