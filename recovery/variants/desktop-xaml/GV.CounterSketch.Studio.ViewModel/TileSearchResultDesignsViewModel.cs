using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.ViewModel;

public class TileSearchResultDesignsViewModel : TileSearchResultViewModelBase
{
	public TileSearchResultDesignsViewModel()
	{
		base.MinimumSearchResultSize = 150;
	}

	public override void GetDesignSearchResultsByTileCategory(bool noUpdate = false)
	{
		if (base.SelectedSearchTileCategory == null)
		{
			return;
		}
		List<DesignSearch> results = null;
		Task.Run(delegate
		{
			if (base.SelectedSearchTileCategory != null)
			{
				EventHelper.RecordEvent(EventHelper.EventTypes.Search_Tile, "Tile", base.SelectedSearchTileCategory.Title);
				switch (base.SelectedSearchTileCategory.Id)
				{
				case 0:
					results = SearchHelper.GetAllBaseRingDesigns();
					break;
				case 3:
					results = SearchHelper.GetPendantDesigns();
					break;
				case 2:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				{
					int collectionId = 0;
					switch (base.SelectedSearchTileCategory.Id)
					{
					case 2:
						collectionId = 170;
						break;
					case 4:
						collectionId = 5;
						break;
					case 5:
						collectionId = 53;
						break;
					case 6:
						collectionId = 263;
						break;
					case 7:
						collectionId = 207;
						break;
					case 8:
						collectionId = 49;
						break;
					case 9:
						collectionId = 78;
						break;
					case 10:
						collectionId = 272;
						break;
					case 11:
						collectionId = 55;
						break;
					}
					results = SearchHelper.GetDesignsByCollectionId(collectionId);
					break;
				}
				case 12:
					results = SearchHelper.Get3CDesigns();
					break;
				case 13:
					results = SearchHelper.GetThreeStoneDesigns();
					break;
				case 14:
					results = SearchHelper.GetMensDesigns();
					break;
				case 15:
					results = SearchHelper.GetDesignsByCollectionId((int)base.SelectedSearchTileCategory.PackageAttributeId);
					break;
				case 17:
					results = SearchHelper.GetNewDesigns();
					break;
				case 19:
					results = SearchHelper.GetEverAndEverDesigns();
					break;
				case 1:
				case 16:
				case 18:
					break;
				}
			}
		}).ContinueWith(delegate
		{
			if (results != null)
			{
				base.BaseSearchResults = new ObservableCollection<DesignSearch>(results);
				if (!ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
				{
					base.SearchResults = new ObservableCollection<DesignSearch>(results);
				}
				else
				{
					GemSearchFilterSizeChanged();
				}
				IsKeywordSearch = false;
				base.IsPackageSearch = false;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}
}
