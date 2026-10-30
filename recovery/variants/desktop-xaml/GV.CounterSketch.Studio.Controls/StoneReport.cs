using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Controls;

public partial class StoneReport : UserControl, IComponentConnector
{
	public static readonly DependencyProperty IsStandaloneProperty = DependencyProperty.Register("IsStandalone", typeof(bool), typeof(StoneReport));

	public bool IsStandalone
	{
		get
		{
			return (bool)GetValue(IsStandaloneProperty);
		}
		set
		{
			SetValue(IsStandaloneProperty, value);
		}
	}

	public StoneReport()
	{
		InitializeComponent();
	}
}
