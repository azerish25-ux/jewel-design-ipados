using System.Windows;
using System.Windows.Media;

namespace GV.CounterSketch.Common;

public class ColorTheme
{
	public int Version => 1;

	public LinearGradientBrush BackgroundGradient => new LinearGradientBrush(new GradientStopCollection(5)
	{
		new GradientStop(ToolHeaderBackground, 0.0),
		new GradientStop(GradientMidpoint1, 0.1),
		new GradientStop(WindowFrameBackground, 0.4),
		new GradientStop(GradientMidpoint2, 0.7),
		new GradientStop(TextBoxAndFrameBackground, 1.0)
	}, new Point(0.217838, 0.0), new Point(0.217838, 1.0));

	private Color GradientMidpoint1 => ToolHeaderBackground * 0.5f + WindowFrameBackground * 0.5f;

	private Color GradientMidpoint2 => WindowFrameBackground * 0.5f + TextBoxAndFrameBackground * 0.5f;

	public string Name { get; set; }

	public Color WindowFrameBackground { get; set; }

	public Color BaseBackground { get; set; }

	public Color ToolHeaderBackground { get; set; }

	public Color TextBoxAndFrameBackground { get; set; }

	public Color HighlightAndAccent { get; set; }

	public bool IsWindowFrameBackgroundGreyscale { get; set; }

	public bool IsBaseBackgroundGreyscale { get; set; }

	public bool IsToolHeaderBackgroundGreyscale { get; set; }

	public bool IsTextBoxAndFrameBackgroundGreyscale { get; set; }

	public Color HighlightForeground { get; set; }

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != typeof(ColorTheme))
		{
			return false;
		}
		return Equals((ColorTheme)obj);
	}

	public bool Equals(ColorTheme other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (other.WindowFrameBackground.Equals(WindowFrameBackground) && other.BaseBackground.Equals(BaseBackground) && other.ToolHeaderBackground.Equals(ToolHeaderBackground) && other.TextBoxAndFrameBackground.Equals(TextBoxAndFrameBackground))
		{
			return other.HighlightAndAccent.Equals(HighlightAndAccent);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((((((WindowFrameBackground.GetHashCode() * 397) ^ BaseBackground.GetHashCode()) * 397) ^ ToolHeaderBackground.GetHashCode()) * 397) ^ TextBoxAndFrameBackground.GetHashCode()) * 397) ^ HighlightAndAccent.GetHashCode();
	}
}
