using GV.CounterSketch.Studio.UserControls.PagedListBox;

namespace GV.CounterSketch.Studio.Model.Objects;

public class LookBookPage : IPagedListBoxItem
{
	public string Abbreviation { get; set; }

	public bool IsBlankPage { get; set; }

	public bool IsContentPage { get; set; }

	public bool IsCoverPage { get; set; }

	public bool IsOpeningPage { get; set; }

	public bool IsBackPage { get; set; }

	public string Description { get; set; }

	public string ImagePath { get; set; }

	public bool IsFavorite { get; set; }

	public bool IsReadOnly { get; set; }

	public string PrimaryText { get; set; }

	public string SecondaryText { get; set; }
}
