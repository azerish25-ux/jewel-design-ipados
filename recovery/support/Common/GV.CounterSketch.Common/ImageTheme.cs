using System.IO;
using GV.CounterSketch.Common.Utilities.Helpers;

namespace GV.CounterSketch.Common;

public class ImageTheme
{
	public string SearchBookLeftImagePath
	{
		get
		{
			if (string.IsNullOrWhiteSpace(Name))
			{
				return string.Empty;
			}
			return $"{Path.Combine(ThemeHelper.ImageThemeRootPath, Name)}_BookLeft.jpg";
		}
	}

	public string SearchBookRightImagePath
	{
		get
		{
			if (string.IsNullOrWhiteSpace(Name))
			{
				return string.Empty;
			}
			return $"{Path.Combine(ThemeHelper.ImageThemeRootPath, Name)}_BookRight.jpg";
		}
	}

	public string SearchGridImagePath
	{
		get
		{
			if (string.IsNullOrWhiteSpace(Name))
			{
				return string.Empty;
			}
			return $"{Path.Combine(ThemeHelper.ImageThemeRootPath, Name)}_Grid.jpg";
		}
	}

	public string SearchExplorerImagePath
	{
		get
		{
			if (string.IsNullOrWhiteSpace(Name))
			{
				return string.Empty;
			}
			return $"{Path.Combine(ThemeHelper.ImageThemeRootPath, Name)}_Grid.jpg";
		}
	}

	public string ViewportImagePath
	{
		get
		{
			if (string.IsNullOrWhiteSpace(Name))
			{
				return string.Empty;
			}
			return $"{Path.Combine(ThemeHelper.ImageThemeRootPath, Name)}_SketchPad.jpg";
		}
	}

	public string Name { get; set; }

	public ImageTheme()
	{
	}

	public ImageTheme(string name)
	{
		Name = name;
	}
}
