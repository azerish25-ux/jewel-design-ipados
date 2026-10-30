using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Common;
using GV.CounterSketch.Common.Utilities.Helpers;
using GV.CounterSketch.Studio.Utilities;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Localization;
using Microsoft.Win32;

namespace GV.CounterSketch.Studio.ViewModel;

public class ThemePickerViewModel : ViewModelBase
{
	private ColorTheme _customColorTheme;

	private string _customLogoPath;

	private ColorPickerViewModel _highlightAndAccentColorPickerVM;

	private ColorTheme _selectedColorTheme;

	private ImageTheme _selectedImageTheme;

	private ColorPickerViewModel _viewPortBackgroundColorPickerVM;

	private string _viewPortBackgroundPath;

	private bool _isViewportBitmapStretched;

	private readonly string _backgroundImagePath;

	public RelayCommand CommandLoadCustomLogo { get; }

	public RelayCommand CommandSetViewPortBackground { get; }

	public RelayCommand CommandResetCustomLogo { get; }

	public RelayCommand CommandResetThemes { get; }

	public RelayCommand CommandResetViewPortBackgroundColor { get; }

	public RelayCommand CommandResetViewPortGraphic { get; }

	public RelayCommand CommandSetViewPortBackgroundColor { get; }

	public RelayCommand CommandApplyCustomColorTheme { get; set; }

	public RelayCommand CommandResetHightlightAccentColor { get; set; }

	public ColorTheme CustomColorTheme
	{
		get
		{
			return _customColorTheme;
		}
		set
		{
			_customColorTheme = value;
			UIHelper.CounterSketchOptions.ThemingLastCustomColorTheme = _customColorTheme;
			UIHelper.CounterSketchOptions.WriteSettings();
			((ObservableObject)this).RaisePropertyChanged("CustomColorTheme");
		}
	}

	public string CustomLogoPath
	{
		get
		{
			return _customLogoPath;
		}
		set
		{
			_customLogoPath = value;
			((ObservableObject)this).RaisePropertyChanged("CustomLogoPath");
		}
	}

	public ImageTheme DefaultTheme { get; set; } = new ImageTheme("Default [No Images]");

	public ColorPickerViewModel HighlightAndAccentColorPickerVM
	{
		get
		{
			return _highlightAndAccentColorPickerVM;
		}
		set
		{
			_highlightAndAccentColorPickerVM = value;
			((ObservableObject)this).RaisePropertyChanged("HighlightAndAccentColorPickerVM");
		}
	}

	public ColorTheme SelectedColorTheme
	{
		get
		{
			return _selectedColorTheme;
		}
		set
		{
			_selectedColorTheme = value;
			if (_selectedColorTheme != null)
			{
				ThemeHelper.SaveAndApplyUpdatedCustomTheme(SelectedColorTheme);
				UIHelper.CounterSketchOptions.ThemingColorTheme = _selectedColorTheme;
			}
			((ObservableObject)this).RaisePropertyChanged("SelectedColorTheme");
		}
	}

	public ImageTheme SelectedImageTheme
	{
		get
		{
			return _selectedImageTheme;
		}
		set
		{
			if (value != null)
			{
				_selectedImageTheme = value;
				UIHelper.CounterSketchOptions.ThemingImageTheme = _selectedImageTheme;
			}
			else
			{
				_selectedImageTheme = DefaultTheme;
			}
			((ObservableObject)this).RaisePropertyChanged("SelectedImageTheme");
		}
	}

	public ColorPickerViewModel ViewPortBackgroundColorPickerVM
	{
		get
		{
			return _viewPortBackgroundColorPickerVM;
		}
		set
		{
			_viewPortBackgroundColorPickerVM = value;
			((ObservableObject)this).RaisePropertyChanged("ViewPortBackgroundColorPickerVM");
		}
	}

	public string ViewPortBackgroundPath
	{
		get
		{
			return _viewPortBackgroundPath;
		}
		set
		{
			_viewPortBackgroundPath = value;
			((ObservableObject)this).RaisePropertyChanged("ViewPortBackgroundPath");
		}
	}

	public RelayCommand CommandViewportBitmapStretchedChanged { get; set; }

	public bool IsViewportBitmapStretched
	{
		get
		{
			return _isViewportBitmapStretched;
		}
		set
		{
			_isViewportBitmapStretched = value;
			((ObservableObject)this).RaisePropertyChanged("IsViewportBitmapStretched");
		}
	}

	public ThemePickerViewModel()
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		_backgroundImagePath = Path.Combine(UIHelper.CSDataRootPath, "ViewportBackground.jpg");
		if (!((ViewModelBase)this).IsInDesignMode)
		{
			InitializeColorPickerViewModels();
			CustomLogoPath = UIHelper.CounterSketchOptions.CustomLogoPath;
			IsViewportBitmapStretched = UIHelper.CounterSketchOptions.IsViewportBitmapStretched;
			ViewPortBackgroundPath = UIHelper.CounterSketchOptions.ViewportGraphicPath;
			CommandApplyCustomColorTheme = new RelayCommand((Action)ApplyCustomColorTheme, false);
			CommandResetViewPortGraphic = new RelayCommand((Action)ResetViewPortGraphic, false);
			CommandSetViewPortBackground = new RelayCommand((Action)SetViewPortBackground, false);
			CommandSetViewPortBackgroundColor = new RelayCommand((Action)SetViewPortBackgroundColor, false);
			CommandResetViewPortBackgroundColor = new RelayCommand((Action)ResetViewPortBackgroundColor, false);
			CommandResetHightlightAccentColor = new RelayCommand((Action)ResetHightlightAccentColor, false);
			CommandLoadCustomLogo = new RelayCommand((Action)LoadCustomLogo, false);
			CommandResetCustomLogo = new RelayCommand((Action)ResetCustomLogo, false);
			CommandResetThemes = new RelayCommand((Action)ResetThemes, false);
			CommandViewportBitmapStretchedChanged = new RelayCommand((Action)ViewportBitmapStretchedChanged, false);
		}
	}

	public void ApplyBackgroundImageToViewport(string filename, bool stretch)
	{
		if (!string.IsNullOrWhiteSpace(filename))
		{
			ViewPortBackgroundPath = filename;
			int height = RhinoWindowHelper.CurrentRhinoPosition.Height;
			int width = RhinoWindowHelper.CurrentRhinoPosition.Width;
			int height2 = height;
			int width2 = width;
			if (height % 2 != 0)
			{
				height2 = (height / 2 + 1) * 2;
			}
			if (width % 2 != 0)
			{
				width2 = (width / 2 + 1) * 2;
			}
			ResizeImage(filename, _backgroundImagePath, height2, width2, stretch);
			UIHelper.CoreDataService.ViewportSetBackgroundAndColor(UIHelper.CounterSketchOptions.ViewportBackgroundColor, _backgroundImagePath);
		}
		else
		{
			UIHelper.CoreDataService.ViewportSetBackgroundAndColor(UIHelper.CounterSketchOptions.ViewportBackgroundColor, string.Empty);
		}
	}

	public void ApplyCustomColorTheme()
	{
		CustomColorTheme.HighlightAndAccent = HighlightAndAccentColorPickerVM.SelectedColor;
		UIHelper.CounterSketchOptions.HighlightAccentSavedColor = HighlightAndAccentColorPickerVM.SelectedColor;
		UIHelper.CounterSketchOptions.WriteSettings();
		SelectedColorTheme = CustomColorTheme;
	}

	public void InitializeColorPickerViewModels()
	{
		CustomColorTheme = ThemeHelper.DefaultColorTheme;
		HighlightAndAccentColorPickerVM = new ColorPickerViewModel();
		UpdateColorPickerViewModelsUsingCustomColorTheme();
		ViewPortBackgroundColorPickerVM = new ColorPickerViewModel(Colors.White, 1.0, 1.0);
		if (!string.IsNullOrWhiteSpace(UIHelper.CounterSketchOptions.ViewportBackgroundColor))
		{
			ViewPortBackgroundColorPickerVM.UpdateSelectedColor((Color)ColorConverter.ConvertFromString(UIHelper.CounterSketchOptions.ViewportBackgroundColor));
			return;
		}
		ViewPortBackgroundColorPickerVM.UpdateSelectedColor(Colors.White);
		ViewPortBackgroundColorPickerVM.SaturationAdjustment = 1.0;
		ViewPortBackgroundColorPickerVM.BrightnessAdjustment = 1.0;
	}

	public void ViewportBitmapStretchedChanged()
	{
		ApplyBackgroundImageToViewport(UIHelper.CounterSketchOptions.ViewportGraphicPath, IsViewportBitmapStretched);
	}

	private void LoadCustomLogo()
	{
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Filter = "Compatible Graphic Files (*.bmp;*.gif;*.jpg;*.png)|*.bmp;*.gif;*.jpg;*.png",
			Multiselect = false,
			Title = Strings.Browse
		};
		if (openFileDialog.ShowDialog() == true && openFileDialog.FileNames[0] != null)
		{
			UIHelper.CounterSketchOptions.CustomLogoPath = openFileDialog.FileNames[0];
			CustomLogoPath = openFileDialog.FileNames[0];
		}
	}

	private void ResetCustomLogo()
	{
		UIHelper.CounterSketchOptions.CustomLogoPath = null;
		UIHelper.CounterSketchOptions.WriteSettings();
		CustomLogoPath = string.Empty;
	}

	private void ResetHightlightAccentColor()
	{
		ThemeHelper.Initialize(UIHelper.KeyProfile, UIHelper.CSThemePath);
		HighlightAndAccentColorPickerVM.SelectedColor = UIHelper.KeyProfile.HighlightAndAccent;
		SelectedColorTheme = ThemeHelper.DefaultColorTheme;
		UIHelper.CounterSketchOptions.HighlightAccentSavedColor = SelectedColorTheme.HighlightAndAccent;
		UIHelper.CounterSketchOptions.WriteSettings();
	}

	private void ResetThemes()
	{
		ResetHightlightAccentColor();
		SelectedImageTheme = DefaultTheme;
		ResetViewPortGraphic();
		ResetViewPortBackgroundColor();
		ResetCustomLogo();
	}

	private void ResetViewPortBackgroundColor()
	{
		UIHelper.CounterSketchOptions.ViewportBackgroundColor = Colors.White.ToString();
		UIHelper.CounterSketchOptions.WriteSettings();
		ViewPortBackgroundColorPickerVM.UpdateSelectedColor(Colors.White);
		ViewPortBackgroundColorPickerVM.SaturationAdjustment = 1.0;
		ViewPortBackgroundColorPickerVM.BrightnessAdjustment = 1.0;
		ViewportBitmapStretchedChanged();
	}

	private void ResetViewPortGraphic()
	{
		ViewPortBackgroundPath = string.Empty;
		UIHelper.CounterSketchOptions.ViewportGraphicPath = ViewPortBackgroundPath;
		UIHelper.CounterSketchOptions.WriteSettings();
		ViewportBitmapStretchedChanged();
	}

	private void SetViewPortBackground()
	{
		string text = UIHelper.CSRootPath + "\\ImageThemes";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			InitialDirectory = text,
			Filter = Strings.CompatibleGraphicFiles + " (*.bmp;*.gif;*.jpg;*.png)|*.bmp;*.gif;*.jpg;*.png",
			Multiselect = false,
			Title = Strings.Browse
		};
		if (openFileDialog.ShowDialog() == true && openFileDialog.FileNames[0] != null)
		{
			string text2 = openFileDialog.FileNames[0];
			string text3 = Path.Combine(UIHelper.CSDataRootPath, "Temp\\");
			if (!Directory.Exists(text3))
			{
				Directory.CreateDirectory(text3);
			}
			string text4 = Path.Combine(text3, Path.GetFileName(text2));
			if (File.Exists(text4))
			{
				File.SetAttributes(text4, FileAttributes.Normal);
				File.Delete(text4);
			}
			File.Copy(text2, text4);
			UIHelper.CounterSketchOptions.ViewportGraphicPath = text4;
			UIHelper.CounterSketchOptions.WriteSettings();
			ApplyBackgroundImageToViewport(text4, IsViewportBitmapStretched);
		}
	}

	private void SetViewPortBackgroundColor()
	{
		UIHelper.CounterSketchOptions.ViewportBackgroundColor = ViewPortBackgroundColorPickerVM.SelectedColor.ToString();
		UIHelper.CounterSketchOptions.WriteSettings();
		ViewportBitmapStretchedChanged();
	}

	private void UpdateColorPickerViewModelsUsingCustomColorTheme()
	{
		HighlightAndAccentColorPickerVM.UpdateSelectedColor(CustomColorTheme.HighlightAndAccent);
	}

	private void ResizeImage(string inputPath, string outputPath, int height, int width, bool stretchImageToFit)
	{
		byte[] imageData = LoadImageData(inputPath);
		ImageSource image = ((!stretchImageToFit) ? CreateImage(imageData, 0, 0) : CreateImage(imageData, width, height));
		imageData = GetEncodedImageData(image, ".jpg");
		if (File.Exists(outputPath))
		{
			File.SetAttributes(outputPath, FileAttributes.Normal);
			File.Delete(outputPath);
		}
		SaveImageData(imageData, outputPath);
	}

	private static void SaveImageData(byte[] imageData, string filePath)
	{
		FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		binaryWriter.Write(imageData);
		binaryWriter.Close();
		fileStream.Close();
	}

	private static ImageSource CreateImage(byte[] imageData, int decodePixelWidth, int decodePixelHeight)
	{
		if (imageData == null)
		{
			return null;
		}
		BitmapImage bitmapImage = new BitmapImage();
		bitmapImage.BeginInit();
		if (decodePixelWidth > 0)
		{
			bitmapImage.DecodePixelWidth = decodePixelWidth;
		}
		if (decodePixelHeight > 0)
		{
			bitmapImage.DecodePixelHeight = decodePixelHeight;
		}
		bitmapImage.StreamSource = new MemoryStream(imageData);
		bitmapImage.CreateOptions = BitmapCreateOptions.None;
		bitmapImage.CacheOption = BitmapCacheOption.Default;
		bitmapImage.EndInit();
		return bitmapImage;
	}

	internal byte[] GetEncodedImageData(ImageSource image, string preferredFormat)
	{
		byte[] array = null;
		BitmapEncoder bitmapEncoder = null;
		switch (preferredFormat.ToLower())
		{
		case ".jpg":
		case ".jpeg":
			bitmapEncoder = new JpegBitmapEncoder();
			break;
		case ".bmp":
			bitmapEncoder = new BmpBitmapEncoder();
			break;
		case ".png":
			bitmapEncoder = new PngBitmapEncoder();
			break;
		case ".tif":
		case ".tiff":
			bitmapEncoder = new TiffBitmapEncoder();
			break;
		case ".gif":
			bitmapEncoder = new GifBitmapEncoder();
			break;
		case ".wmp":
			bitmapEncoder = new WmpBitmapEncoder();
			break;
		}
		if (image is BitmapSource)
		{
			MemoryStream memoryStream = new MemoryStream();
			if (bitmapEncoder != null)
			{
				bitmapEncoder.Frames.Add(BitmapFrame.Create(image as BitmapSource));
				bitmapEncoder.Save(memoryStream);
			}
			memoryStream.Seek(0L, SeekOrigin.Begin);
			array = new byte[memoryStream.Length];
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			binaryReader.Read(array, 0, (int)memoryStream.Length);
			binaryReader.Close();
			memoryStream.Close();
		}
		return array;
	}

	private static byte[] LoadImageData(string filePath)
	{
		FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		byte[] result = binaryReader.ReadBytes((int)fileStream.Length);
		binaryReader.Close();
		fileStream.Close();
		return result;
	}
}
