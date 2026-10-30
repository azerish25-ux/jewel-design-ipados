using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using GV.CounterSketch.Shared.Model;

namespace GV.CounterSketch.Studio.Controls;

[Serializable]
public partial class CSCategoryControl : UserControl, IComponentConnector
{
	public static readonly DependencyProperty CollapsedProperty = DependencyProperty.Register("Collapsed", typeof(bool), typeof(CSCategoryControl));

	public static readonly DependencyProperty ControlColorProperty = DependencyProperty.Register("ControlColor", typeof(Color), typeof(CSCategoryControl), new UIPropertyMetadata(Colors.Black));

	public static readonly DependencyProperty ControlColorBrushProperty = DependencyProperty.Register("ControlColorBrush", typeof(SolidColorBrush), typeof(CSCategoryControl), new UIPropertyMetadata(new SolidColorBrush(Colors.Black)));

	public static readonly DependencyProperty DeleteVisibilityProperty = DependencyProperty.Register("DeleteVisibility", typeof(Visibility), typeof(CSCategoryControl));

	public static readonly DependencyProperty IconImagePathProperty = DependencyProperty.Register("IconImagePath", typeof(string), typeof(CSCategoryControl));

	public static readonly DependencyProperty ImportGroupNameProperty = DependencyProperty.Register("ImportGroupName", typeof(string), typeof(CSCategoryControl));

	public static readonly DependencyProperty IsDesignOptionProperty = DependencyProperty.Register("IsDesignOption", typeof(bool), typeof(CSCategoryControl), new PropertyMetadata(false));

	public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(CSCategoryControl));

	public static readonly RoutedEvent TitleClickedEvent = EventManager.RegisterRoutedEvent("TitleClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CSCategoryControl));

	public bool Collapsed
	{
		get
		{
			return (bool)GetValue(CollapsedProperty);
		}
		set
		{
			SetValue(CollapsedProperty, value);
			if (value)
			{
				borderControls.Visibility = Visibility.Collapsed;
				return;
			}
			borderControls.Visibility = Visibility.Visible;
			BringIntoView();
		}
	}

	public Color ControlColor
	{
		get
		{
			return (Color)GetValue(ControlColorBrushProperty);
		}
		set
		{
			SetValue(ControlColorProperty, value);
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
			ControlColor = value.Color;
			SetValue(ControlColorBrushProperty, value);
			ControlColorBrush.Freeze();
		}
	}

	public Visibility DeleteVisibility
	{
		get
		{
			return (Visibility)GetValue(DeleteVisibilityProperty);
		}
		set
		{
			SetValue(DeleteVisibilityProperty, value);
		}
	}

	public string IconImagePath
	{
		get
		{
			return (string)GetValue(IconImagePathProperty);
		}
		set
		{
			SetValue(IconImagePathProperty, value);
		}
	}

	public string ImportGroupName
	{
		get
		{
			return (string)GetValue(ImportGroupNameProperty);
		}
		set
		{
			SetValue(ImportGroupNameProperty, value.ToUpper());
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
			SetValue(TitleProperty, value.ToUpper());
		}
	}

	public ItemCollection Children => lstControls.Items;

	public string CategoryName { get; set; }

	public string EnglishCategoryName { get; set; }

	public Guid ImportGroupIdentifier { get; set; }

	public Guid ImportGroupPartGroupId { get; set; }

	public event RoutedEventHandler TitleClicked
	{
		add
		{
			AddHandler(TitleClickedEvent, value);
		}
		remove
		{
			RemoveHandler(TitleClickedEvent, value);
		}
	}

	public CSCategoryControl()
	{
		base.Focusable = true;
		InitializeComponent();
	}

	public void AddChild(UIElement element)
	{
		AddChild(element, sort: true);
	}

	public void AddChild(UIElement element, bool sort)
	{
		CSUIControl cSUIControl = element as CSUIControl;
		if (cSUIControl != null)
		{
			cSUIControl.Tag = cSUIControl.SortIndex;
		}
		Children.Add(cSUIControl);
		if (sort)
		{
			Sort();
		}
	}

	public static UIElement CreateControlFromUIControlObject(UIControl uiControl)
	{
		CSUIControl cSUIControl = null;
		switch (uiControl.CurrentUIControlType)
		{
		case UIControlType.CSValueRange:
			cSUIControl = new CSValueRangeControl(uiControl);
			break;
		case UIControlType.CSValueRingSize:
			cSUIControl = new CSValueRingSizeControl(uiControl);
			break;
		case UIControlType.CSValueGem:
			cSUIControl = new CSValueGemControl(uiControl);
			break;
		case UIControlType.CSValueGemShape:
			cSUIControl = new CSValueGemShapeControl(uiControl);
			break;
		case UIControlType.CSValueChooser:
			cSUIControl = new CSValueChooserControl(uiControl);
			break;
		case UIControlType.CSValueOnOff:
			cSUIControl = new CSValueOnOffControl(uiControl);
			break;
		case UIControlType.CSValueDualRange:
			cSUIControl = new CSValueDualRangeControl(uiControl);
			break;
		case UIControlType.CSValueList:
			cSUIControl = new CSValueListControl(uiControl);
			break;
		case UIControlType.CSValueFontList:
			cSUIControl = new CSValueFontListControl(uiControl);
			break;
		case UIControlType.CSValueHeadStyleList:
			cSUIControl = new CSValueHeadStyleListControl(uiControl);
			break;
		case UIControlType.CSValueGemOutput:
			cSUIControl = new CSValueGemOutputControl(uiControl);
			break;
		case UIControlType.CSValueInput:
			cSUIControl = new CSValueInputControl(uiControl);
			break;
		case UIControlType.CSValueOutput:
			cSUIControl = new CSValueOutputControl(uiControl);
			break;
		case UIControlType.CSValueGemSize:
			cSUIControl = new CSValueGemSizeControl(uiControl);
			break;
		case UIControlType.CSValueXFormObject:
			cSUIControl = new CSValueXFormObjectControl(uiControl);
			break;
		}
		cSUIControl?.DisableUIControl();
		return cSUIControl;
	}

	public void RemoveChild(UIControl uiControl, bool sort)
	{
		if (uiControl != null)
		{
			Children.Remove(uiControl);
			if (sort)
			{
				Sort();
			}
		}
	}

	public void RemoveChildContaining(UIControl uiControl)
	{
		RemoveChild(uiControl, sort: true);
	}

	public void Sort()
	{
		lstControls.Items.SortDescriptions.Clear();
		lstControls.Items.SortDescriptions.Add(new SortDescription("Tag", ListSortDirection.Ascending));
	}

	public void UnregisterControlsFromMessenger()
	{
		foreach (CSUIControl item in (IEnumerable)Children)
		{
			if (item is CSValueRingSizeControl)
			{
				((CSValueRingSizeControl)item).UnregisterFromMessenger();
			}
		}
	}

	public void UnwireControlsFromStoneTrayViewModel()
	{
		foreach (CSUIControl item in (IEnumerable)Children)
		{
			if (item is CSValueGemControl)
			{
				((CSValueGemControl)item).UnwireFromStoneTrayViewModel();
			}
			if (item is CSValueGemSizeControl)
			{
				((CSValueGemSizeControl)item).UnwireFromStoneTrayViewModel();
			}
			if (item is CSValueGemShapeControl)
			{
				((CSValueGemShapeControl)item).UnwireFromStoneTrayViewModel();
			}
		}
	}

	internal void UpdateCSUIControlSource(UIControl uiControl)
	{
		UpdateCSUIControlSource(uiControl, sort: true);
	}

	internal void UpdateCSUIControlSource(UIControl uiControl, bool sort)
	{
		if (uiControl == null)
		{
			return;
		}
		if (ImportGroupIdentifier == uiControl.ImportGroupIdentifier && !string.IsNullOrWhiteSpace(ImportGroupName))
		{
			ImportGroupName = uiControl.ImportGroupName;
		}
		ImportGroupPartGroupId = uiControl.ImportGroupPartGroupId;
		if (Children != null)
		{
			CSUIControl cSUIControl = Children.Cast<CSUIControl>().ToList().FirstOrDefault((CSUIControl csuiControl) => csuiControl.LocalUIControl.EnglishControlName == uiControl.EnglishControlName && csuiControl.LocalUIControl.BlockInstanceGuid == uiControl.BlockInstanceGuid);
			if (cSUIControl != null)
			{
				cSUIControl.UpdateUIControlSource(uiControl);
			}
			else
			{
				AddChild(CreateControlFromUIControlObject(uiControl));
			}
		}
	}

	private void TxtTitleMouseDown(object sender, MouseButtonEventArgs e)
	{
		Collapsed = !Collapsed;
		RoutedEventArgs e2 = new RoutedEventArgs(TitleClickedEvent);
		RaiseEvent(e2);
		e.Handled = true;
	}
}
