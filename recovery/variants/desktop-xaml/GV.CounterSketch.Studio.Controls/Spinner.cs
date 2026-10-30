using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace GV.CounterSketch.Studio.Controls;

public partial class Spinner : UserControl, IComponentConnector
{
	private Storyboard _storyboard;

	public static readonly DependencyProperty UseHighlightColorProperty = DependencyProperty.Register("UseHighlightColor", typeof(bool), typeof(Spinner), new PropertyMetadata(true));

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
}
