using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.View;

public partial class WindowSearchKeyword : Window, IComponentConnector
{
	public WindowSearchKeyword()
	{
		InitializeComponent();
		KeywordsListBox.ItemsSource = BuildCollectionFromDesignKeywords(SearchHelper.SearchKeywords);
		if (SecurityHelper.IsStreaming)
		{
			button.Visibility = Visibility.Collapsed;
		}
	}

	public List<string> BuildCollectionFromDesignKeywords(List<string> keywords)
	{
		return (from o in (from _ in keywords
				where !string.IsNullOrEmpty(_) && !int.TryParse(_, out var _)
				select CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_.ToLower().Trim())).Distinct()
			orderby o
			select o).ToList();
	}

	private void button_Click(object sender, RoutedEventArgs e)
	{
		KeywordsListBox.Print(saveOnly: false);
	}
}
