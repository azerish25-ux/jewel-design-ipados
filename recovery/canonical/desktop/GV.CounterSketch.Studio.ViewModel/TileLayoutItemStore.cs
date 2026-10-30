using System.Collections.Generic;
using GV.CounterSketch.Studio.Model;

namespace GV.CounterSketch.Studio.ViewModel;

public class TileLayoutItemStore
{
	public List<TileItem> AvailableTiles { get; set; }

	public List<TileItem> Tiles { get; set; }
}
