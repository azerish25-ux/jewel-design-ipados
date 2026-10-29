namespace GV.CounterSketch.Studio.Utilities;

public interface IListItemConverter
{
	object Convert(object masterListItem);

	object ConvertBack(object targetListItem);
}
