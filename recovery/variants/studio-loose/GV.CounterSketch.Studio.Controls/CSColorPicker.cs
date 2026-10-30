using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GV.CounterSketch.Studio.Controls;

public class CSColorPicker : UserControl, IComponentConnector
{
	private bool _isMouseDown;

	private Color _selectedColor = Colors.Transparent;

	internal Image ColorImage;

	internal Canvas CanvImage;

	internal Ellipse ellipsePixel;

	internal Canvas CanvPreview;

	internal Image imageLogo;

	internal Ellipse ellipsePixelPreview;

	internal TextBox txtAlpha;

	internal TextBox txtAlphaHex;

	internal TextBox txtRed;

	internal TextBox txtRedHex;

	internal TextBox txtGreen;

	internal TextBox txtGreenHex;

	internal TextBox txtBlue;

	internal TextBox txtBlueHex;

	internal TextBox txtAll;

	internal Border AlphaBorder;

	internal Slider AlphaSlider;

	private bool _contentLoaded;

	public Color SelectedColor
	{
		get
		{
			return _selectedColor;
		}
		private set
		{
			if (_selectedColor != value)
			{
				_selectedColor = value;
				CreateAlphaLinearBrush();
				UpdateTextBoxes();
				UpdateInk();
			}
		}
	}

	public ImageSource CustomLogoImageSource
	{
		get
		{
			return imageLogo.Source;
		}
		set
		{
			imageLogo.Source = value;
		}
	}

	public CSColorPicker()
	{
		InitializeComponent();
		base.Loaded += CSColorPicker_Loaded;
	}

	private void CSColorPicker_Loaded(object sender, RoutedEventArgs e)
	{
		SelectedColor = Colors.Black;
	}

	private void CreateAlphaLinearBrush()
	{
		Color startColor = Color.FromArgb(0, SelectedColor.R, SelectedColor.G, SelectedColor.B);
		Color endColor = Color.FromArgb(byte.MaxValue, SelectedColor.R, SelectedColor.G, SelectedColor.B);
		LinearGradientBrush background = new LinearGradientBrush(startColor, endColor, new Point(0.0, 0.0), new Point(1.0, 0.0));
		AlphaBorder.Background = background;
	}

	private void CanvPreview_MouseMove(object sender, MouseEventArgs e)
	{
		if (!_isMouseDown)
		{
			return;
		}
		try
		{
			if (imageLogo != null && imageLogo.Source != null)
			{
				double height = imageLogo.Source.Height;
				double num = imageLogo.Source.Width / 150.0 * (double)(int)Mouse.GetPosition(imageLogo).X;
				double num2 = height / 30.0 * (double)(int)Mouse.GetPosition(imageLogo).Y;
				CroppedBitmap croppedBitmap = new CroppedBitmap(imageLogo.Source as BitmapSource, new Int32Rect((int)num, (int)num2, 1, 1));
				byte[] array = new byte[4];
				try
				{
					croppedBitmap.CopyPixels(array, 4, 0);
				}
				catch
				{
				}
				ellipsePixelPreview.SetValue(Canvas.LeftProperty, Mouse.GetPosition(imageLogo).X - 5.0 + 15.0);
				ellipsePixelPreview.SetValue(Canvas.TopProperty, Mouse.GetPosition(imageLogo).Y - 5.0 + 60.0);
				CanvPreview.InvalidateVisual();
				SelectedColor = Color.FromArgb((byte)AlphaSlider.Value, array[2], array[1], array[0]);
			}
		}
		catch (Exception)
		{
		}
	}

	private void CanvPreview_MouseDown(object sender, MouseButtonEventArgs e)
	{
		_isMouseDown = true;
	}

	private void CanvPreview_MouseUp(object sender, MouseButtonEventArgs e)
	{
		_isMouseDown = false;
	}

	private void CanvImage_MouseMove(object sender, MouseEventArgs e)
	{
		if (!_isMouseDown)
		{
			return;
		}
		try
		{
			CroppedBitmap croppedBitmap = new CroppedBitmap(ColorImage.Source as BitmapSource, new Int32Rect((int)Mouse.GetPosition(CanvImage).X, (int)Mouse.GetPosition(CanvImage).Y, 1, 1));
			byte[] array = new byte[4];
			try
			{
				croppedBitmap.CopyPixels(array, 4, 0);
			}
			catch (Exception)
			{
			}
			ellipsePixel.SetValue(Canvas.LeftProperty, Mouse.GetPosition(CanvImage).X - 5.0);
			ellipsePixel.SetValue(Canvas.TopProperty, Mouse.GetPosition(CanvImage).Y - 5.0);
			CanvImage.InvalidateVisual();
			SelectedColor = Color.FromArgb((byte)AlphaSlider.Value, array[2], array[1], array[0]);
		}
		catch (Exception)
		{
		}
	}

	private void CanvImage_MouseDown(object sender, MouseButtonEventArgs e)
	{
		_isMouseDown = true;
	}

	private void CanvImage_MouseUp(object sender, MouseButtonEventArgs e)
	{
		_isMouseDown = false;
	}

	private void UpdateTextBoxes()
	{
		txtAlpha.Text = SelectedColor.A.ToString();
		txtAlphaHex.Text = SelectedColor.A.ToString("X");
		txtRed.Text = SelectedColor.R.ToString();
		txtRedHex.Text = SelectedColor.R.ToString("X");
		txtGreen.Text = SelectedColor.G.ToString();
		txtGreenHex.Text = SelectedColor.G.ToString("X");
		txtBlue.Text = SelectedColor.B.ToString();
		txtBlueHex.Text = SelectedColor.B.ToString("X");
		txtAll.Text = $"#{txtAlphaHex.Text}{txtRedHex.Text}{txtGreenHex.Text}{txtBlueHex.Text}";
	}

	private void UpdateInk()
	{
		CanvPreview.Background = new SolidColorBrush(SelectedColor);
	}

	private void AlphaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		SelectedColor = Color.FromArgb((byte)AlphaSlider.Value, SelectedColor.R, SelectedColor.G, SelectedColor.B);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/cscolorpicker.xaml", UriKind.Relative);
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
			ColorImage = (Image)target;
			break;
		case 2:
			CanvImage = (Canvas)target;
			CanvImage.MouseDown += CanvImage_MouseDown;
			CanvImage.MouseUp += CanvImage_MouseUp;
			CanvImage.MouseMove += CanvImage_MouseMove;
			break;
		case 3:
			ellipsePixel = (Ellipse)target;
			break;
		case 4:
			CanvPreview = (Canvas)target;
			CanvPreview.MouseDown += CanvPreview_MouseDown;
			CanvPreview.MouseMove += CanvPreview_MouseMove;
			CanvPreview.MouseUp += CanvPreview_MouseUp;
			break;
		case 5:
			imageLogo = (Image)target;
			break;
		case 6:
			ellipsePixelPreview = (Ellipse)target;
			break;
		case 7:
			txtAlpha = (TextBox)target;
			break;
		case 8:
			txtAlphaHex = (TextBox)target;
			break;
		case 9:
			txtRed = (TextBox)target;
			break;
		case 10:
			txtRedHex = (TextBox)target;
			break;
		case 11:
			txtGreen = (TextBox)target;
			break;
		case 12:
			txtGreenHex = (TextBox)target;
			break;
		case 13:
			txtBlue = (TextBox)target;
			break;
		case 14:
			txtBlueHex = (TextBox)target;
			break;
		case 15:
			txtAll = (TextBox)target;
			break;
		case 16:
			AlphaBorder = (Border)target;
			break;
		case 17:
			AlphaSlider = (Slider)target;
			AlphaSlider.ValueChanged += AlphaSlider_ValueChanged;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
