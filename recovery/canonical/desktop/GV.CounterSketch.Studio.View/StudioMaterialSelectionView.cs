using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.View;

public class StudioMaterialSelectionView : UserControl, IComponentConnector, IStyleConnector
{
	public static readonly DependencyProperty IsControlRightAlignedProperty = DependencyProperty.Register("IsControlRightAligned", typeof(bool), typeof(StudioMaterialSelectionView), new PropertyMetadata(false));

	private bool _contentLoaded;

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

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/view/studiomaterialselectionview.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		_contentLoaded = true;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IStyleConnector.Connect(int connectionId, object target)
	{
		if (connectionId == 1)
		{
			((Button)target).Click += UnAssignButton_OnClick;
		}
	}
}
