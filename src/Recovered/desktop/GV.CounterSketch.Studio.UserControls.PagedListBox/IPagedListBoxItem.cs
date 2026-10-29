namespace GV.CounterSketch.Studio.UserControls.PagedListBox;

public interface IPagedListBoxItem
{
	string Abbreviation { get; set; }

	string ImagePath { get; set; }

	string Description { get; set; }

	bool IsFavorite { get; set; }

	bool IsReadOnly { get; set; }

	string PrimaryText { get; set; }

	string SecondaryText { get; set; }
}
