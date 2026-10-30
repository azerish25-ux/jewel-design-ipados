using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Studio.UserControls.PagedListBox;

namespace GV.CounterSketch.Studio.Model.Objects;

public class ValueChooserDataItem : DataItem, IPagedListBoxItem
{
	public string Abbreviation { get; set; }

	public string Description { get; set; }

	public string ImagePath { get; set; }

	public string PrimaryText { get; set; }

	public string SecondaryText { get; set; }

	public bool IsFavorite { get; set; }

	public bool IsReadOnly { get; set; }

	public ValueChooserDataItem(DataItem dataItem)
	{
		base.BitmapSourceByteArrayInfo = dataItem.BitmapSourceByteArrayInfo;
		base.VectorResourceName = dataItem.VectorResourceName;
		base.Identifier = dataItem.Identifier;
		base.IsItemDisabled = dataItem.IsItemDisabled;
		base.PolyPointCollection = dataItem.PolyPointCollection;
		base.HasImage = dataItem.HasImage;
	}
}
