using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.View;

public partial class StudioMaterialSelectionView : UserControl, IComponentConnector, IStyleConnector
{
	public static readonly DependencyProperty IsControlRightAlignedProperty = DependencyProperty.Register("IsControlRightAligned", typeof(bool), typeof(StudioMaterialSelectionView), new PropertyMetadata(false));

	public bool IsControlRightAligned
	{
		get
		{
			return (bool)GetValue(IsControlRightAlignedProperty);
		}
		set
		{
			SetValue(IsControlRightAlignedProperty, value);
		}
	}

	public StudioMaterialSelectionView()
	{
		InitializeComponent();
	}

	private void UnAssignButton_OnClick(object sender, RoutedEventArgs e)
	{
		e.Handled = true;
	}
}
