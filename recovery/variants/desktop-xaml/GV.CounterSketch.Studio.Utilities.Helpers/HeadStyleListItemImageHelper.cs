using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Model.Objects;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class HeadStyleListItemImageHelper
{
	private static ValueListDisplayItem CreateListItem(int index, HeadStyleListItemIndexPair listItemIndexPair, bool uiDisabled, ref ResourceDictionary dictionary)
	{
		string name = Enum.GetName(typeof(HeadStyle), listItemIndexPair.HeadStyleListItem);
		ValueListDisplayItem valueListDisplayItem = new ValueListDisplayItem
		{
			Index = index,
			Name = name,
			Description = EnumHelper.GetEnumDescriptionFromValue(listItemIndexPair.HeadStyleListItem),
			UIDisabled = uiDisabled
		};
		if (dictionary == null)
		{
			dictionary = Application.LoadComponent(new Uri("/ResourceDictionaries/HeadStyleListDisplayResources.xaml", UriKind.Relative)) as ResourceDictionary;
		}
		if (dictionary == null)
		{
			return valueListDisplayItem;
		}
		string name2 = valueListDisplayItem.Name;
		if (name2 == null)
		{
			return valueListDisplayItem;
		}
		if (!dictionary.Contains(name2))
		{
			return valueListDisplayItem;
		}
		if (dictionary[name2] is DrawingBrush drawingBrush)
		{
			valueListDisplayItem.Icon = new DrawingImage(drawingBrush.Drawing);
		}
		return valueListDisplayItem;
	}

	public static List<ValueListDisplayItem> GetValueHeadStyleListItemsFromListIndexPairs(IEnumerable<HeadStyleListItemIndexPair> listIndexPairs, ref ResourceDictionary dictionary)
	{
		List<ValueListDisplayItem> list = new List<ValueListDisplayItem>();
		foreach (HeadStyleListItemIndexPair listIndexPair in listIndexPairs)
		{
			list.Add(CreateListItem(listIndexPair.Index, listIndexPair, listIndexPair.UIDisabled, ref dictionary));
		}
		return list;
	}
}
