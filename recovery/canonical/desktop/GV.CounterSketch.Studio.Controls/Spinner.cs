using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace GV.CounterSketch.Studio.Controls;

public class Spinner : UserControl, IComponentConnector
{
	private Storyboard _storyboard;

	public static readonly DependencyProperty UseHighlightColorProperty = DependencyProperty.Register("UseHighlightColor", typeof(bool), typeof(Spinner), new PropertyMetadata(true));

	internal Spinner UserControl;

	internal Grid grid;

	internal Canvas canvas;

	private bool _contentLoaded;

	public bool UseHighlightColor
	{
		get
		{
			return (bool)GetValue(UseHighlightColorProperty);
		}
		set
		{
			SetValue(UseHighlightColorProperty, value);
		}
	}

	public Spinner()
	{
		InitializeComponent();
		base.IsVisibleChanged += OnVisibleChanged;
	}

	public void StartAnimation()
	{
		_storyboard = (Storyboard)FindResource("canvasAnimation");
		_storyboard.Begin(canvas, isControllable: true);
	}

	public void StopAnimation()
	{
		_storyboard.Remove(canvas);
	}

	private void OnVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (base.IsVisible)
		{
			StartAnimation();
		}
		else
		{
			StopAnimation();
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/spinner.xaml", UriKind.Relative);
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
			UserControl = (Spinner)target;
			break;
		case 2:
			grid = (Grid)target;
			break;
		case 3:
			canvas = (Canvas)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
