using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.ViewModel;

public class TileSearchResultPartsViewModel : TileSearchResultViewModelBase
{
	public TileSearchResultPartsViewModel()
	{
		base.MinimumSearchResultSize = 100;
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
			if (base.SelectedSearchTileCategory == null)
			{
				return;
			}
			EventHelper.RecordEvent(EventHelper.EventTypes.Search_Tile, "Tile", base.SelectedSearchTileCategory.Title);
			CounterSketchLibraryDataEntities1 context = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
			try
			{
				IQueryable<DesignSearch> source = context.DesignSearches.Where((DesignSearch design) => design.IsFreeHandPart == (long?)1L || design.IsFreeHandPartWithGem == (long?)1L || design.IsStaticFreeHandPart == (long?)1L);
				switch (base.SelectedSearchTileCategory.Id)
				{
				case 100:
					results = source.ToList();
					break;
				case 101:
					results = source.Where((DesignSearch d) => d.IsNewDesign == (long?)1L).ToList();
					break;
				default:
				{
					IQueryable<DesignSearch> source2 = context.DesignSearches.Where((DesignSearch design) => (design.IsFreeHandPart == (long?)1L || design.IsFreeHandPartWithGem == (long?)1L || design.IsStaticFreeHandPart == (long?)1L) && context.DesignAttributes.Any((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.DesignAttribute da) => da.AttributeId == (long?)(long)SelectedSearchTileCategory.Id && da.DesignId == design.DesignId));
					results = source2.ToList();
					break;
				}
				}
			}
			finally
			{
				if (context != null)
				{
					((IDisposable)context).Dispose();
				}
			}
		}).ContinueWith(delegate
		{
			base.SearchResults = new ObservableCollection<DesignSearch>(results);
			base.BaseSearchResults = new ObservableCollection<DesignSearch>(results);
			if (results.Count == 0)
			{
				ShowResultsOrMessage(resultsVisible: false);
			}
			IsKeywordSearch = false;
			base.IsPackageSearch = false;
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}
}
