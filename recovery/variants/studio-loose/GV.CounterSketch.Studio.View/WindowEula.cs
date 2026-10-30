using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;
using Localization;

namespace GV.CounterSketch.Studio.View;

public class WindowEula : Window, IComponentConnector
{
	internal RichTextBox richTextBox;

	internal RadioButton radAgree;

	internal RadioButton radNoAgree;

	internal Button ButtonClose;

	internal TextBlock textBlock;

	private bool _contentLoaded;

	public WindowEula()
	{
		InitializeComponent();
		if (SecurityHelper.IsStreaming)
		{
			base.Title = UntranslatedStrings.SoftwareAsAServiceAgreement;
		}
		if (richTextBox != null)
		{
			TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
			using FileStream stream = new FileStream(UIHelper.CounterSketchOptions.CurrentEulaPath, FileMode.Open, FileAccess.Read);
			textRange.Load(stream, DataFormats.Rtf);
		}
	}

	private void ButtonClick(object sender, RoutedEventArgs e)
	{
		if (radAgree.IsChecked == true)
		{
			base.DialogResult = true;
			UIHelper.CounterSketchOptions.LastEulaPathAgreedTo = UIHelper.CounterSketchOptions.CurrentEulaPath;
			UIHelper.CounterSketchOptions.HasAgreedToEula = true;
			UIHelper.CounterSketchOptions.WriteSettings();
		}
		Close();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/windoweula.xaml", UriKind.Relative);
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
			richTextBox = (RichTextBox)target;
			break;
		case 2:
			radAgree = (RadioButton)target;
			break;
		case 3:
			radNoAgree = (RadioButton)target;
			break;
		case 4:
			ButtonClose = (Button)target;
			ButtonClose.Click += ButtonClick;
			break;
		case 5:
			textBlock = (TextBlock)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
