namespace GV.CounterSketch.Studio.Utilities;

public class RhinoPosition
{
	public int Height { get; set; }

	public int Left { get; set; }

	public int PreviousHeight { get; set; }

	public int PreviousLeft { get; set; }

	public int PreviousTop { get; set; }

	public int PreviousWidth { get; set; }

	public int Top { get; set; }

	public int Width { get; set; }

	public RhinoPosition(RhinoPosition rhinoPosition)
	{
		PreviousTop = rhinoPosition.Top;
		PreviousLeft = rhinoPosition.Left;
		PreviousWidth = rhinoPosition.Width;
		PreviousHeight = rhinoPosition.Height;
	}

	public RhinoPosition()
	{
	}

	public void ClearPosition()
	{
		PreviousTop = Top;
		PreviousLeft = Left;
		PreviousWidth = Width;
		PreviousHeight = Height;
		Top = 0;
		Left = 0;
		Width = 0;
		Height = 0;
	}

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
		if (obj.GetType() != typeof(RhinoPosition))
		{
			return false;
		}
		return Equals((RhinoPosition)obj);
	}

	public bool Equals(RhinoPosition other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (other.Top == Top && other.Left == Left && other.Width == Width)
		{
			return other.Height == Height;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((((Top * 397) ^ Left) * 397) ^ Width) * 397) ^ Height;
	}
}
