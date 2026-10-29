using System.Collections.Generic;
using GV.CounterSketch.Shared;

namespace CS.Parametric.Interfaces;

public interface IFontSource
{
	Font? MissingRestoreFont { get; }

	IEnumerable<Font> UnavailableFonts { get; }

	IEnumerable<int> UnrecognizedFontValues { get; }
}
