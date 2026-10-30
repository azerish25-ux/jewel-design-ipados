using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;
using Localization;

namespace GV.CounterSketch.Studio.View;

public partial class WindowEula : Window, IComponentConnector
{
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
}
