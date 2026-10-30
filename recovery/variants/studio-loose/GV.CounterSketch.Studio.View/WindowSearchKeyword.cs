using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.View;

public class WindowSearchKeyword : Window, IComponentConnector
{
	internal Grid RootGrid;

	internal Button button;

	internal Viewbox CSButtonPrint_Default;

	internal ListBox KeywordsListBox;

	private bool _contentLoaded;

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
		int tempInt;
		return (from o in (from _ in keywords
				where !string.IsNullOrEmpty(_) && !int.TryParse(_, out tempInt)
				select CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_.ToLower().Trim())).Distinct()
			orderby o
			select o).ToList();
	}

	private void button_Click(object sender, RoutedEventArgs e)
	{
		KeywordsListBox.Print(saveOnly: false);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/windowsearchkeyword.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			RootGrid = (Grid)target;
			break;
		case 2:
			button = (Button)target;
			button.Click += button_Click;
			break;
		case 3:
			CSButtonPrint_Default = (Viewbox)target;
			break;
		case 4:
			KeywordsListBox = (ListBox)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
