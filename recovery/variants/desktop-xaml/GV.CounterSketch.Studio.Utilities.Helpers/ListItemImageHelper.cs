using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Model.Objects;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class ListItemImageHelper
{
	private static ValueListDisplayItem CreateListItem(int index, ListItemIndexPair listItemIndexPair, bool uiDisabled, string modifier, ref ResourceDictionary dictionary)
	{
		string name = Enum.GetName(typeof(ValueListItem), listItemIndexPair.ValueListItem);
		string name2 = (string.IsNullOrWhiteSpace(modifier) ? name : $"{name}_{modifier}");
		ValueListDisplayItem valueListDisplayItem = new ValueListDisplayItem
		{
			Index = index,
			Name = name2,
			Description = EnumHelper.GetEnumDescriptionFromValue(listItemIndexPair.ValueListItem),
			UIDisabled = uiDisabled
		};
		if (dictionary == null)
		{
			dictionary = Application.LoadComponent(new Uri("/ResourceDictionaries/ListDisplayResources.xaml", UriKind.Relative)) as ResourceDictionary;
		}
		if (dictionary == null)
		{
			return valueListDisplayItem;
		}
		string name3 = valueListDisplayItem.Name;
		if (name3 == null)
		{
			return valueListDisplayItem;
		}
		if (!dictionary.Contains(name3))
		{
			return valueListDisplayItem;
		}
		if (dictionary[name3] is DrawingBrush drawingBrush)
		{
			valueListDisplayItem.Icon = new DrawingImage(drawingBrush.Drawing);
		}
		return valueListDisplayItem;
	}

	public static List<ValueListDisplayItem> GetValueListItemsFromListIndexPairs(IEnumerable<ListItemIndexPair> listIndexPairs, string modifier, ref ResourceDictionary dictionary)
	{
		List<ValueListDisplayItem> list = new List<ValueListDisplayItem>();
		foreach (ListItemIndexPair listIndexPair in listIndexPairs)
		{
			list.Add(CreateListItem(listIndexPair.Index, listIndexPair, listIndexPair.UIDisabled, modifier, ref dictionary));
		}
		return list;
	}

	public static List<ValueListDisplayItem> GetValueListItemsFromListIndexPairs(IEnumerable<ListItemIndexPair> listIndexPairs, ref ResourceDictionary dictionary)
	{
		return GetValueListItemsFromListIndexPairs(listIndexPairs, null, ref dictionary);
	}
}
