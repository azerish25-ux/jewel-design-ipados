using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media.Imaging;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to adjust the DPI of the image to match CatalogGenertation image ratio.")]
public class ImageBehaviorAdjustDpi : Behavior<Image>
{
	private BitmapFrame _bitmapFrame;

	private double _currentPercentageAdjustment;

	protected override void OnAttached()
	{
		base.AssociatedObject.SizeChanged += AssociatedObject_SizeChanged;
	}

	protected override void OnDetaching()
	{
		base.AssociatedObject.SizeChanged -= AssociatedObject_SizeChanged;
	}

	private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (_bitmapFrame == null || _bitmapFrame != base.AssociatedObject.Source as BitmapFrame)
		{
			_bitmapFrame = base.AssociatedObject.Source as BitmapFrame;
			if (_bitmapFrame != null)
			{
				_currentPercentageAdjustment = _bitmapFrame.DpiX / 300.0;
				base.AssociatedObject.Height = _bitmapFrame.Height * _currentPercentageAdjustment;
				base.AssociatedObject.Width = _bitmapFrame.Width * _currentPercentageAdjustment;
			}
			else
			{
				base.AssociatedObject.Height = 1.0;
				base.AssociatedObject.Width = 1.0;
			}
			base.AssociatedObject.UpdateLayout();
		}
	}
}
