namespace GV.CounterSketch.Studio.Database;

public class SearchResultInfo
{
	public int ComponentTypeId;

	public string Designer;

	public int DesignID;

	public long FileOffset;

	public string LibraryFileName;

	public int MaterialGem1;

	public int MaterialGem2;

	public int MaterialMetal1;

	public int MaterialMetal2;

	public string Name;

	public long RenderFileOffset;

	public long RenderFileBaseOffset;

	public long RenderFileSideOffset;

	private string _stullerSeriesNumber;

	public string StullerModelNumber { get; set; }

	public string StullerSeriesNumber
	{
		get
		{
			if (!string.IsNullOrEmpty(_stullerSeriesNumber.Trim()))
			{
				return ":" + _stullerSeriesNumber;
			}
			return _stullerSeriesNumber;
		}
		set
		{
			_stullerSeriesNumber = value;
		}
	}

	public double HiddenOpacity { get; set; }

	public bool IsMarkedAsHidden { get; set; }

	public bool IsVisible { get; set; }
}
