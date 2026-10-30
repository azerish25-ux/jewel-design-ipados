using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.Controls;

public partial class SelectedItemsImageControl : UserControl, IComponentConnector
{
	public static readonly DependencyProperty HasSingleItemProperty = DependencyProperty.Register("HasSingleItem", typeof(bool), typeof(SelectedItemsImageControl));

	public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(List<string>), typeof(SelectedItemsImageControl), new UIPropertyMetadata(ItemsChangedCallback));

	public static readonly DependencyProperty NumberOfColumnsProperty = DependencyProperty.Register("NumberOfColumns", typeof(int), typeof(SelectedItemsImageControl), new PropertyMetadata(0));

	public static readonly DependencyProperty NumberOfItemsProperty = DependencyProperty.Register("NumberOfItems", typeof(int), typeof(SelectedItemsImageControl));

	public static readonly DependencyProperty NumberOfRowsProperty = DependencyProperty.Register("NumberOfRows", typeof(int), typeof(SelectedItemsImageControl), new PropertyMetadata(0));

	public bool HasSingleItem
	{
		get
		{
			return (bool)GetValue(HasSingleItemProperty);
		}
		set
		{
			SetValue(HasSingleItemProperty, value);
		}
	}

	public List<string> ItemsSource
	{
		get
		{
			return (List<string>)GetValue(ItemsSourceProperty);
		}
		set
		{
			SetValue(ItemsSourceProperty, value);
		}
	}

	public int NumberOfColumns
	{
		get
		{
			return (int)GetValue(NumberOfColumnsProperty);
		}
		set
		{
			SetValue(NumberOfColumnsProperty, value);
		}
	}

	public int NumberOfItems
	{
		get
		{
			return (int)GetValue(NumberOfItemsProperty);
		}
		set
		{
			SetValue(NumberOfItemsProperty, value);
		}
	}

	public int NumberOfRows
	{
		get
		{
			return (int)GetValue(NumberOfRowsProperty);
		}
		set
		{
			SetValue(NumberOfRowsProperty, value);
		}
	}

	private static void ItemsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is SelectedItemsImageControl selectedItemsImageControl)
		{
			if (selectedItemsImageControl.ItemsSource != null)
			{
				selectedItemsImageControl.HasSingleItem = selectedItemsImageControl.ItemsSource.Count == 1;
				selectedItemsImageControl.NumberOfItems = selectedItemsImageControl.ItemsSource.Count;
			}
			if (selectedItemsImageControl.NumberOfItems == 1)
			{
				selectedItemsImageControl.NumberOfRows = 1;
				selectedItemsImageControl.NumberOfColumns = 1;
			}
			else if (selectedItemsImageControl.NumberOfItems == 2)
			{
				selectedItemsImageControl.NumberOfRows = 1;
				selectedItemsImageControl.NumberOfColumns = 2;
			}
			else if (selectedItemsImageControl.NumberOfItems == 3)
			{
				selectedItemsImageControl.NumberOfRows = 2;
				selectedItemsImageControl.NumberOfColumns = 2;
			}
			else if (selectedItemsImageControl.NumberOfItems == 4)
			{
				selectedItemsImageControl.NumberOfRows = 2;
				selectedItemsImageControl.NumberOfColumns = 2;
			}
			else if (selectedItemsImageControl.NumberOfItems == 5)
			{
				selectedItemsImageControl.NumberOfRows = 2;
				selectedItemsImageControl.NumberOfColumns = 3;
			}
			else if (selectedItemsImageControl.NumberOfItems == 6)
			{
				selectedItemsImageControl.NumberOfRows = 2;
				selectedItemsImageControl.NumberOfColumns = 3;
			}
			else if (selectedItemsImageControl.NumberOfItems == 7)
			{
				selectedItemsImageControl.NumberOfRows = 3;
				selectedItemsImageControl.NumberOfColumns = 3;
			}
			else if (selectedItemsImageControl.NumberOfItems == 8)
			{
				selectedItemsImageControl.NumberOfRows = 3;
				selectedItemsImageControl.NumberOfColumns = 3;
			}
			else if (selectedItemsImageControl.NumberOfItems == 9)
			{
				selectedItemsImageControl.NumberOfRows = 3;
				selectedItemsImageControl.NumberOfColumns = 3;
			}
			else
			{
				selectedItemsImageControl.NumberOfRows = 3;
				selectedItemsImageControl.NumberOfColumns = 3;
			}
		}
	}

	public SelectedItemsImageControl()
	{
		InitializeComponent();
		if (ViewModelBase.IsInDesignModeStatic)
		{
			ItemsSource = new List<string> { "Images/ColorSwatchCircle.png", "Images/ColorSwatchCircle.png", "Images/ColorSwatchCircle.png" };
		}
	}
}
