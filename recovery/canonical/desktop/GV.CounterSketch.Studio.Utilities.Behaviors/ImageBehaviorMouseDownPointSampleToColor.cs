using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to sample the color under mouse for the image when the mouse is pressed. ")]
public class ImageBehaviorMouseDownPointSampleToColor : Behavior<Image>
{
	public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ImageBehaviorMouseDownPointSampleToColor));

	public Color SelectedColor
	{
		get
		{
			return (Color)GetValue(SelectedColorProperty);
		}
		set
		{
			SetValue(SelectedColorProperty, value);
		}
	}

	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
		base.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		base.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
		base.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
	}

	private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
	{
		SamplePixelForColor();
	}

	private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed)
		{
			SamplePixelForColor();
		}
	}

	private void SamplePixelForColor()
	{
		Point position = Mouse.GetPosition(base.AssociatedObject);
		RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)base.AssociatedObject.ActualWidth, (int)base.AssociatedObject.ActualHeight, 96.0, 96.0, PixelFormats.Default);
		renderTargetBitmap.Render(base.AssociatedObject);
		if (position.X < (double)renderTargetBitmap.PixelWidth && position.Y < (double)renderTargetBitmap.PixelHeight)
		{
			CroppedBitmap croppedBitmap = new CroppedBitmap(renderTargetBitmap, new Int32Rect((int)position.X, (int)position.Y, 1, 1));
			byte[] array = new byte[4];
			croppedBitmap.CopyPixels(array, 4, 0);
			SelectedColor = Color.FromArgb(byte.MaxValue, array[2], array[1], array[0]);
		}
	}
}
