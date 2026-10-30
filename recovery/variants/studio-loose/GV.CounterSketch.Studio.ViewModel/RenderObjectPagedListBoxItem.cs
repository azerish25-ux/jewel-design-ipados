using GV.CounterSketch.Studio.UserControls.PagedListBox;

namespace GV.CounterSketch.Studio.ViewModel;

public class RenderObjectPagedListBoxItem : IPagedListBoxItem
{
	public string Abbreviation { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string ImagePath { get; set; }

	public bool IsFavorite { get; set; }

	public bool IsReadOnly { get; set; }

	public string PrimaryText { get; set; }

	public string SecondaryText { get; set; }
}
