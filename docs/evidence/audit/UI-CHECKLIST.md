# CounterSketch UI and workflow checklist

68 grouped UI/workflow requirements, including explicit dormant/empty/unresolved surfaces. Raw declaration and binding catalogs remain in ui-surfaces.json; event/input hooks are in ui-event-inventory.json.

Every checkbox is an acceptance requirement, not a passed implementation test. Static evidence is distinct from runtime behavior.

## UI-001 Home, search, customize, freehand and order navigation

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Main mode setters update mode flags; NavigateToSection and breadcrumbs perform section changes. Freehand mode can skip its tab transition.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise home, search, customize, freehand and order navigation on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** MainViewModel; section viewmodels; feature/key profile

**Evidence:**

- [BreadCrumbViewModel.cs:52](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BreadCrumbViewModel.cs#L52>) — BreadCrumbViewModel.CommandNavigateToHome
- [BreadCrumbViewModel.cs:367](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BreadCrumbViewModel.cs#L367>) — BreadCrumbViewModel.NavigateToHome
- [BreadCrumbViewModel.cs:54](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BreadCrumbViewModel.cs#L54>) — BreadCrumbViewModel.CommandNavigateToSearch
- [BreadCrumbViewModel.cs:389](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BreadCrumbViewModel.cs#L389>) — BreadCrumbViewModel.NavigateToSearch
- [BreadCrumbViewModel.cs:56](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BreadCrumbViewModel.cs#L56>) — BreadCrumbViewModel.CommandNavigateToCustomize
- [BreadCrumbViewModel.cs:356](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BreadCrumbViewModel.cs#L356>) — BreadCrumbViewModel.NavigateToCustomize
- [BreadCrumbViewModel.cs:58](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BreadCrumbViewModel.cs#L58>) — BreadCrumbViewModel.CommandNavigateToOrder
- [BreadCrumbViewModel.cs:378](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BreadCrumbViewModel.cs#L378>) — BreadCrumbViewModel.NavigateToOrder
- [MainViewModel.cs:364](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L364>) — MainViewModel.CommandSetCustomizeModeActive
- [MainViewModel.cs:1329](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1329>) — MainViewModel.SetCustomizeModeActive
- [MainViewModel.cs:366](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L366>) — MainViewModel.CommandSetFreehandModeActive
- [MainViewModel.cs:1336](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1336>) — MainViewModel.SetFreehandModeActive
- [MainViewModel.cs:1341](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1341>) — MainViewModel.SetFreehandModeActive
- [MainViewModel.cs:368](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L368>) — MainViewModel.CommandSetOrderModeActive
- [MainViewModel.cs:1363](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1363>) — MainViewModel.SetOrderModeActive
- [MainViewModel.cs:382](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L382>) — MainViewModel.CommandSetSearchModeActive
- [MainViewModel.cs:1386](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1386>) — MainViewModel.SetSearchModeActive

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-002 Category, keyword, SPI and collection/package search

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Search entrypoints clear or restore keyword/category state and call active result viewmodel searches by tile category, keyword, SPI number, Ever & Ever, or selected package; choosing category ID 1 enters freehand.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Bundle a known library; search the same keywords, SPI and packages offline, clear the keyword, and verify exact item identities and prior-category restoration.

**Dependencies:** SearchHelper; library database; catalog thumbnails

**Evidence:**

- [TileSearchMainViewModel.cs:70](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L70>) — TileSearchMainViewModel.CommandChooseFeaturedModelBySPI
- [TileSearchMainViewModel.cs:330](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L330>) — TileSearchMainViewModel.ChooseFeaturedModelBySPI
- [TileSearchMainViewModel.cs:72](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L72>) — TileSearchMainViewModel.CommandClearSearchKeyword
- [TileSearchMainViewModel.cs:336](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L336>) — TileSearchMainViewModel.ClearSearchKeyword
- [TileSearchMainViewModel.cs:74](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L74>) — TileSearchMainViewModel.CommandPopulateSearchFromKeyword
- [TileSearchMainViewModel.cs:373](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L373>) — TileSearchMainViewModel.PopulateSearchFromKeyword
- [TileSearchMainViewModel.cs:76](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L76>) — TileSearchMainViewModel.CommandPopulateSearchResults
- [TileSearchMainViewModel.cs:242](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L242>) — TileSearchMainViewModel.PopulateSearchResults
- [TileSearchMainViewModel.cs:78](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L78>) — TileSearchMainViewModel.CommandPopulateSearchResultsFromEverAndEver
- [TileSearchMainViewModel.cs:391](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L391>) — TileSearchMainViewModel.PopulateSearchResultsFromEverAndEver
- [TileSearchMainViewModel.cs:80](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L80>) — TileSearchMainViewModel.CommandPopulateSearchResultsFromPackageKit
- [TileSearchMainViewModel.cs:406](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L406>) — TileSearchMainViewModel.PopulateSearchResultsFromPackageKit
- [TileSearchMainViewModel.cs:84](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L84>) — TileSearchMainViewModel.CommandTransitionToTileSearchCategoryState
- [TileSearchMainViewModel.cs:318](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L318>) — TileSearchMainViewModel.TransitionToTileSearchCategoryState
- [TileSearchResultViewModelBase.cs:788](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L788>) — TileSearchResultViewModelBase.CommandGotoPage
- [MainViewModel.cs:304](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L304>) — MainViewModel.CommandOpenSearchKeywordWindow
- [MainViewModel.cs:1919](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1919>) — MainViewModel.OpenSearchKeyword

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-003 Result filters and search refresh

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Result commands update selected filter criteria, reset active filters/results, apply gem-size filters and trigger result refresh; selected variant changes and filter search remain separate code paths.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise result filters and search refresh on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** SearchHelper; search attributes; library database

**Evidence:**

- [TileSearchResultViewModelBase.cs:420](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L420>) — TileSearchResultViewModelBase.CommandGemSearchFilterSizeChanged
- [TileSearchResultViewModelBase.cs:1204](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1204>) — TileSearchResultViewModelBase.GemSearchFilterSizeChanged
- [TileSearchResultViewModelBase.cs:432](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L432>) — TileSearchResultViewModelBase.CommandResetResults
- [TileSearchResultViewModelBase.cs:3942](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3942>) — TileSearchResultViewModelBase.ResetResults
- [TileSearchResultViewModelBase.cs:434](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L434>) — TileSearchResultViewModelBase.CommandResetSearchActiveSearchFilters
- [TileSearchResultViewModelBase.cs:3949](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3949>) — TileSearchResultViewModelBase.ResetSearchActiveSearchFilters
- [TileSearchResultViewModelBase.cs:436](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L436>) — TileSearchResultViewModelBase.CommandSelectedDesignSearchChanged
- [TileSearchResultViewModelBase.cs:3988](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3988>) — TileSearchResultViewModelBase.SelectedDesignSearchChanged
- [TileSearchResultViewModelBase.cs:438](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L438>) — TileSearchResultViewModelBase.CommandSelectedVariantSearchChanged
- [TileSearchResultViewModelBase.cs:3993](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3993>) — TileSearchResultViewModelBase.SelectedVariantSearchChanged
- [TileSearchResultViewModelBase.cs:442](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L442>) — TileSearchResultViewModelBase.CommandUpdateResultsWithSelectedFilterCriteria
- [TileSearchResultViewModelBase.cs:4054](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L4054>) — TileSearchResultViewModelBase.UpdateResultsWithSelectedFilterCriteria
- [TileSearchResultViewModelBase.cs:444](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L444>) — TileSearchResultViewModelBase.CommandUpdateSearchFromFilter
- [TileSearchResultViewModelBase.cs:4067](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L4067>) — TileSearchResultViewModelBase.UpdateSearchFromFilter
- [TileSearchResultViewModelBase.cs:1008](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1008>) — TileSearchResultViewModelBase.CommandTriggerSearchRefresh
- [TileSearchResultViewModelBase.cs:4048](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L4048>) — TileSearchResultViewModelBase.TriggerSearchRefresh

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-004 Result paging, image size and view history

- [ ] Implement and verify the acceptance criteria below.

**Observed:** First/previous/next/last/page commands alter result paging; tile/results back and forward transitions and image-size refresh are separate from loading the design.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise result paging, image size and view history on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** paged result collection; scroll viewer; search UI

**Evidence:**

- [TileSearchResultViewModelBase.cs:126](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L126>) — TileSearchResultViewModelBase._commandGotoPage
- [TileSearchResultViewModelBase.cs:1356](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1356>) — TileSearchResultViewModelBase.GotoPage
- [TileSearchResultViewModelBase.cs:344](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L344>) — TileSearchResultViewModelBase.CommandGotoFirstPage
- [TileSearchResultViewModelBase.cs:3642](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3642>) — TileSearchResultViewModelBase.GotoFirstPage
- [TileSearchResultViewModelBase.cs:346](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L346>) — TileSearchResultViewModelBase.CommandGotoPreviousPage
- [TileSearchResultViewModelBase.cs:3665](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3665>) — TileSearchResultViewModelBase.GotoPreviousPage
- [TileSearchResultViewModelBase.cs:350](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L350>) — TileSearchResultViewModelBase.CommandGotoNextPage
- [TileSearchResultViewModelBase.cs:3654](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3654>) — TileSearchResultViewModelBase.GotoNextPage
- [TileSearchResultViewModelBase.cs:352](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L352>) — TileSearchResultViewModelBase.CommandGotoLastPage
- [TileSearchResultViewModelBase.cs:3648](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3648>) — TileSearchResultViewModelBase.GotoLastPage
- [TileSearchResultViewModelBase.cs:515](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L515>) — TileSearchResultViewModelBase.CommandUpdateSearchResultImageSizes
- [TileSearchResultViewModelBase.cs:1551](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1551>) — TileSearchResultViewModelBase.UpdateSearchResultImageSizes
- [TileSearchResultViewModelBase.cs:1068](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1068>) — TileSearchResultViewModelBase.CommandNavigateBackToTiles
- [TileSearchResultViewModelBase.cs:1362](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1362>) — TileSearchResultViewModelBase.NavigateBackToTiles
- [TileSearchResultViewModelBase.cs:1070](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1070>) — TileSearchResultViewModelBase.CommandNavigateForwardToResults
- [TileSearchResultViewModelBase.cs:1372](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1372>) — TileSearchResultViewModelBase.NavigateForwardToResults

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-005 Design details, alternate images, shape and variant opening

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Selecting a search item opens information; details can move to adjacent designs, switch image, open selected gem shape, or load a design iteration/search object.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise design details, alternate images, shape and variant opening on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** LoadHelper; design metadata; thumbnails; core document service

**Evidence:**

- [TileSearchResultViewModelBase.cs:416](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L416>) — TileSearchResultViewModelBase.CommandOpenDesignWithSelectedShape
- [TileSearchResultViewModelBase.cs:3785](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3785>) — TileSearchResultViewModelBase.OpenDesignWithSelectedShape
- [TileSearchResultViewModelBase.cs:418](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L418>) — TileSearchResultViewModelBase.CommandCloseDesignInfo
- [TileSearchResultViewModelBase.cs:1198](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1198>) — TileSearchResultViewModelBase.CloseDesignInfo
- [TileSearchResultViewModelBase.cs:422](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L422>) — TileSearchResultViewModelBase.CommandNextDesignInfo
- [TileSearchResultViewModelBase.cs:3711](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3711>) — TileSearchResultViewModelBase.MoveToNextDesignInfo
- [TileSearchResultViewModelBase.cs:424](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L424>) — TileSearchResultViewModelBase.CommandPreviousDesignInfo
- [TileSearchResultViewModelBase.cs:3722](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3722>) — TileSearchResultViewModelBase.MoveToPreviousDesignInfo
- [TileSearchResultViewModelBase.cs:426](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L426>) — TileSearchResultViewModelBase.CommandOpenDesignFromDesignIteration
- [TileSearchResultViewModelBase.cs:3733](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3733>) — TileSearchResultViewModelBase.OpenDesignFromDesignIteration
- [TileSearchResultViewModelBase.cs:428](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L428>) — TileSearchResultViewModelBase.CommandOpenDesignFromDesignSearch
- [TileSearchResultViewModelBase.cs:3749](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3749>) — TileSearchResultViewModelBase.OpenDesignFromDesignSearch
- [TileSearchResultViewModelBase.cs:430](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L430>) — TileSearchResultViewModelBase.CommandSelectDesignSearch
- [TileSearchResultViewModelBase.cs:3983](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3983>) — TileSearchResultViewModelBase.SelectDesignSearch
- [TileSearchResultViewModelBase.cs:440](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L440>) — TileSearchResultViewModelBase.CommandSwitchDesignDetailImage
- [TileSearchResultViewModelBase.cs:1442](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1442>) — TileSearchResultViewModelBase.SwitchDesignDetailImage

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-006 Session picks, marked designs and adding to portfolio/collection

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Session picks can be toggled, removed or scrolled; marked results can be cleared, added to the active project or to a collection. Detail-page add is another entrypoint to project addition.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise session picks, marked designs and adding to portfolio/collection on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** ProjectManagementHelper; session selections; collections

**Evidence:**

- [TileSearchResultViewModelBase.cs:185](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L185>) — TileSearchResultViewModelBase.CommandToggleSessionPicks
- [TileSearchResultViewModelBase.cs:1170](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1170>) — TileSearchResultViewModelBase.ToggleSessionPicks
- [TileSearchResultViewModelBase.cs:348](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L348>) — TileSearchResultViewModelBase.CommandRemoveFromSessionPicks
- [TileSearchResultViewModelBase.cs:3936](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L3936>) — TileSearchResultViewModelBase.RemoveFromSessionPicks
- [TileSearchResultViewModelBase.cs:406](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L406>) — TileSearchResultViewModelBase.CommandPreviousSessionDesign
- [TileSearchResultViewModelBase.cs:1180](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1180>) — TileSearchResultViewModelBase.PreviousSessionDesign
- [TileSearchResultViewModelBase.cs:408](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L408>) — TileSearchResultViewModelBase.CommandNextSessionDesign
- [TileSearchResultViewModelBase.cs:1175](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1175>) — TileSearchResultViewModelBase.NextSessionDesign
- [TileSearchResultViewModelBase.cs:410](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L410>) — TileSearchResultViewModelBase.CommandAddMarkedDesignsToActiveProject
- [TileSearchResultViewModelBase.cs:1863](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1863>) — TileSearchResultViewModelBase.AddMarkedDesignsToActiveProject
- [TileSearchResultViewModelBase.cs:412](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L412>) — TileSearchResultViewModelBase.CommandAddMarkedDesignsToCollection
- [TileSearchResultViewModelBase.cs:1877](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1877>) — TileSearchResultViewModelBase.AddMarkedDesignsToCollection
- [TileSearchResultViewModelBase.cs:414](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L414>) — TileSearchResultViewModelBase.CommandClearMarkedDesigns
- [TileSearchResultViewModelBase.cs:1882](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1882>) — TileSearchResultViewModelBase.ClearMarkedDesigns
- [TileSearchResultViewModelBase.cs:1012](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1012>) — TileSearchResultViewModelBase.CommandAddDesignFromDesignDetailsToActiveProject
- [TileSearchResultViewModelBase.cs:1855](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchResultViewModelBase.cs#L1855>) — TileSearchResultViewModelBase.AddDesignFromDesignDetailsToActiveProject

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-007 Editable tile layout and custom launches

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Tile operations create/remove tiles, set four sizes, choose showcase tiles, change image/URL, save, reset or cancel layouts. Tile command creation dispatches package/category/freehand/user-parts searches, browser URLs, style quiz and stone shoppers.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Customize tile order, sizes, images and commands; save/restart offline and verify searches and local launch actions; external URLs clearly require connectivity.

**Dependencies:** serialized tile layout; local images; optional URL/browser

**Evidence:**

- [MainViewModel.cs:276](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L276>) — MainViewModel.CommandEditTileLayoutDesigns
- [MainViewModel.cs:1517](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1517>) — MainViewModel.EditTileLayoutDesigns
- [MainViewModel.cs:278](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L278>) — MainViewModel.CommandEditTileLayoutParts
- [MainViewModel.cs:1523](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1523>) — MainViewModel.EditTileLayoutParts
- [TileSearchMainViewModel.cs:84](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L84>) — TileSearchMainViewModel.CommandTransitionToTileSearchCategoryState
- [TileSearchMainViewModel.cs:318](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L318>) — TileSearchMainViewModel.TransitionToTileSearchCategoryState
- [TileViewViewModelBase.cs:184](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L184>) — TileViewViewModelBase.CommandAddCustomTile
- [TileViewViewModelBase.cs:361](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L361>) — TileViewViewModelBase.AddCustomTile
- [TileViewViewModelBase.cs:186](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L186>) — TileViewViewModelBase.CommandSetTileSizeMedium
- [TileViewViewModelBase.cs:660](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L660>) — TileViewViewModelBase.SetTileSizeMedium
- [TileViewViewModelBase.cs:188](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L188>) — TileViewViewModelBase.CommandSetTileSizeLarge
- [TileViewViewModelBase.cs:652](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L652>) — TileViewViewModelBase.SetTileSizeLarge
- [TileViewViewModelBase.cs:190](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L190>) — TileViewViewModelBase.CommandSetTileSizeSmall
- [TileViewViewModelBase.cs:668](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L668>) — TileViewViewModelBase.SetTileSizeSmall
- [TileViewViewModelBase.cs:192](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L192>) — TileViewViewModelBase.CommandSetTileSizeWide
- [TileViewViewModelBase.cs:676](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L676>) — TileViewViewModelBase.SetTileSizeWide
- [TileViewViewModelBase.cs:202](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L202>) — TileViewViewModelBase.CommandSelectTile
- [TileViewViewModelBase.cs:622](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L622>) — TileViewViewModelBase.SelectTile
- [TileViewViewModelBase.cs:232](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L232>) — TileViewViewModelBase.CommandEditTileImage
- [TileViewViewModelBase.cs:401](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L401>) — TileViewViewModelBase.EditTileImage
- [TileViewViewModelBase.cs:234](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L234>) — TileViewViewModelBase.CommandEditTileURL
- [TileViewViewModelBase.cs:414](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L414>) — TileViewViewModelBase.EditTileURL
- [TileViewViewModelBase.cs:236](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L236>) — TileViewViewModelBase.CommandRemoveTile
- [TileViewViewModelBase.cs:550](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L550>) — TileViewViewModelBase.RemoveTile
- [TileViewViewModelBase.cs:238](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L238>) — TileViewViewModelBase.CommandAddTile
- [TileViewViewModelBase.cs:374](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L374>) — TileViewViewModelBase.AddTile
- [TileViewViewModelBase.cs:279](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L279>) — TileViewViewModelBase.CommandSetShowcaseTile
- [TileViewViewModelBase.cs:627](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L627>) — TileViewViewModelBase.SetShowcaseTile
- [TileViewViewModelBase.cs:684](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L684>) — TileViewViewModelBase.WireupTile
- [TileViewViewModelBase.cs:194](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L194>) — TileViewViewModelBase.CommandResetLayout
- [TileViewViewModelBase.cs:570](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L570>) — TileViewViewModelBase.ResetLayout
- [TileViewViewModelBase.cs:198](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L198>) — TileViewViewModelBase.CommandSaveLayout
- [TileViewViewModelBase.cs:581](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L581>) — TileViewViewModelBase.SaveLayout
- [TileViewViewModelBase.cs:586](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L586>) — TileViewViewModelBase.SaveLayout
- [TileViewViewModelBase.cs:200](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L200>) — TileViewViewModelBase.CommandCancelLayout
- [TileViewViewModelBase.cs:389](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L389>) — TileViewViewModelBase.CancelLayout

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-008 Style quiz and guided browsing

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Main entrypoint creates the style-quiz browser, while NavigateFromStyleQuiz translates primary and secondary results into a search. Static code does not establish an offline copy of quiz content.

**Offline disposition:** Desktop implementation requires remote service/network state. For a fully offline iPad, supply a local substitute or defer external action until connected; do not represent live values as current.

**Acceptance:**

- In airplane mode, exercise style quiz and guided browsing on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** browser-hosted quiz; style-to-search mapping

**Evidence:**

- [MainViewModel.cs:332](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L332>) — MainViewModel.CommandOpenStyleQuizWindow
- [MainViewModel.cs:2281](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2281>) — MainViewModel.OpenStyleQuizWindow

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-009 Design stack, featured iterations and component visibility

- [ ] Implement and verify the acceptance criteria below.

**Observed:** The stack builds controls from model UIControl definitions, orders/categories them, resets range controls, selects featured design iterations, switches visible components and updates the gem report; pendant selection disables try-on.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- For every bundled control type and representative iteration, change/reset values and toggle components; save and reopen the editable document on PC.

**Dependencies:** UIControl metadata; CoreDataService; translation resources

**Evidence:**

- [DesignStackViewModel.cs:58](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/DesignStackViewModel.cs#L58>) — DesignStackViewModel.CommandResetUIControl
- [DesignStackViewModel.cs:271](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/DesignStackViewModel.cs#L271>) — DesignStackViewModel.ResetUIControl
- [DesignStackViewModel.cs:60](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/DesignStackViewModel.cs#L60>) — DesignStackViewModel.CommandSelectedFeaturedIterationChanged
- [DesignStackViewModel.cs:735](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/DesignStackViewModel.cs#L735>) — DesignStackViewModel.SelectedFeaturedIterationChanged
- [DesignStackViewModel.cs:62](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/DesignStackViewModel.cs#L62>) — DesignStackViewModel.CommandUpdateDesignStackCategoryFromComponents
- [DesignStackViewModel.cs:435](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/DesignStackViewModel.cs#L435>) — DesignStackViewModel.UpdateDesignStackCategoryFromComponents
- [DesignStackViewModel.cs:64](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/DesignStackViewModel.cs#L64>) — DesignStackViewModel.CommandUpdateVisibleDisplayComponents
- [DesignStackViewModel.cs:778](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/DesignStackViewModel.cs#L778>) — DesignStackViewModel.UpdateVisibleDisplayComponents
- [DesignStackViewModel.cs:341](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/DesignStackViewModel.cs#L341>) — DesignStackViewModel.PopulateDesignStackUIControls
- [DesignStackViewModel.cs:709](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/DesignStackViewModel.cs#L709>) — DesignStackViewModel.ProcessUIControlNotificationMessage

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-010 Open and close ordering from customization

- [ ] Implement and verify the acceptance criteria below.

**Observed:** OpenOrder/CloseOrder switch customization order visibility and move through the main order workflow; these are UI wrappers around order state.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise open and close ordering from customization on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** order summary viewmodel; customization state

**Evidence:**

- [CustomizeViewModel.cs:148](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizeViewModel.cs#L148>) — CustomizeViewModel.CommandOpenOrder
- [CustomizeViewModel.cs:244](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizeViewModel.cs#L244>) — CustomizeViewModel.OpenOrder
- [CustomizeViewModel.cs:150](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizeViewModel.cs#L150>) — CustomizeViewModel.CommandCloseOrder
- [CustomizeViewModel.cs:233](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizeViewModel.cs#L233>) — CustomizeViewModel.CloseOrder

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-011 Parts toolbox, gem, text and ring-rail guide

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Adding a part enters customize and shows the parts toolbox; built-in gem, rail guide and text imports use bundled CSG paths. The toolbox supports local user-parts and catalog categories.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Offline add each built-in part/text/gem and catalog part; verify font fallback and that editable parametric controls survive desktop reopening.

**Dependencies:** Parts/6853.csg; Parts/RingRailGuide.csg; Parts/FHTextTool.csg; font resources; CoreDataService

**Evidence:**

- [CustomizePartsListViewModel.cs:96](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L96>) — CustomizePartsListViewModel.CommandAddRingRailGuide
- [CustomizePartsListViewModel.cs:282](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L282>) — CustomizePartsListViewModel.AddRingRailGuide
- [CustomizePartsListViewModel.cs:198](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L198>) — CustomizePartsListViewModel.CommandAddPart
- [CustomizePartsListViewModel.cs:265](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L265>) — CustomizePartsListViewModel.AddPart
- [CustomizePartsListViewModel.cs:277](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L277>) — CustomizePartsListViewModel.AddPart
- [CustomizePartsListViewModel.cs:212](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L212>) — CustomizePartsListViewModel.CommandClosePartToolbox
- [CustomizePartsListViewModel.cs:292](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L292>) — CustomizePartsListViewModel.ClosePartToolbox
- [FreehandStackViewModel.cs:85](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L85>) — FreehandStackViewModel.CommandOpenPartsToolBoxWindow
- [FreehandStackViewModel.cs:423](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L423>) — FreehandStackViewModel.OpenPartsToolBoxWindow
- [TileViewPartsViewModel.cs:83](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewPartsViewModel.cs#L83>) — TileViewPartsViewModel.CommandAddRingRailGuide
- [TileViewPartsViewModel.cs:191](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewPartsViewModel.cs#L191>) — TileViewPartsViewModel.AddRingRailGuid
- [TileViewPartsViewModel.cs:85](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewPartsViewModel.cs#L85>) — TileViewPartsViewModel.CommandAddAGem
- [TileViewPartsViewModel.cs:186](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewPartsViewModel.cs#L186>) — TileViewPartsViewModel.AddAGem
- [TileViewPartsViewModel.cs:87](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewPartsViewModel.cs#L87>) — TileViewPartsViewModel.CommandAddText
- [TileViewPartsViewModel.cs:196](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewPartsViewModel.cs#L196>) — TileViewPartsViewModel.AddText
- [CustomizePartsListViewModel.cs:287](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L287>) — CustomizePartsListViewModel.AddTextTool
- [CustomizePartsListViewModel.cs:260](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L260>) — CustomizePartsListViewModel.AddAGem

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-012 Select, multi-select, highlight and show/hide parts

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Parts selection supports primary and secondary selections, Ctrl or explicit multi-select mode, delayed viewport selection/highlight and visibility toggles; submenu and design/freehand-stack selection are UI state.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise select, multi-select, highlight and show/hide parts on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** import group identifiers; CoreDataService viewport selection/highlight

**Evidence:**

- [CustomizePartsListViewModel.cs:152](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L152>) — CustomizePartsListViewModel.CommandToggleMultiSelectForPartsList
- [CustomizePartsListViewModel.cs:748](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L748>) — CustomizePartsListViewModel.ToggleMultiSelectForPartsList
- [CustomizePartsListViewModel.cs:180](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L180>) — CustomizePartsListViewModel.CommandShowHideSubMenu
- [CustomizePartsListViewModel.cs:714](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L714>) — CustomizePartsListViewModel.ShowhideSubMenu
- [CustomizePartsListViewModel.cs:186](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L186>) — CustomizePartsListViewModel.CommandTogglePartVisibility
- [CustomizePartsListViewModel.cs:753](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L753>) — CustomizePartsListViewModel.TogglePartVisibility
- [CustomizePartsListViewModel.cs:192](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L192>) — CustomizePartsListViewModel.CommandSelectedImportGroup
- [CustomizePartsListViewModel.cs:609](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L609>) — CustomizePartsListViewModel.SelectedImportGroup
- [CustomizePartsListViewModel.cs:194](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L194>) — CustomizePartsListViewModel.CommandSetDesignStackVisible
- [CustomizePartsListViewModel.cs:659](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L659>) — CustomizePartsListViewModel.SetDesignStackVisible
- [CustomizePartsListViewModel.cs:196](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L196>) — CustomizePartsListViewModel.CommandSetFreehandStackVisible
- [CustomizePartsListViewModel.cs:669](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L669>) — CustomizePartsListViewModel.SetFreehandStackVisible
- [CustomizePartsListViewModel.cs:204](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L204>) — CustomizePartsListViewModel.CommandExportSelectedImportGroupsToPath
- [CustomizePartsListViewModel.cs:492](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L492>) — CustomizePartsListViewModel.ExportSelectedImportGroupsToPath
- [CustomizePartsListViewModel.cs:206](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L206>) — CustomizePartsListViewModel.CommandExportSelectedImportGroupsToUserParts
- [CustomizePartsListViewModel.cs:517](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L517>) — CustomizePartsListViewModel.ExportSelectedImportGroupsToUserParts
- [CustomizePartsListViewModel.cs:216](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L216>) — CustomizePartsListViewModel.CommandHighlightImportGroup
- [CustomizePartsListViewModel.cs:525](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L525>) — CustomizePartsListViewModel.HighlightImportGroup

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-013 Group, ungroup, duplicate, rename, delete and reset parts

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Grouping sends selected identifiers to ImportGroupAddToPartGroup; breaking removes the UI group then calls the graph; delete may prompt for groups, removes cached controls and deletes graph nodes. Duplicating, renaming and resetting call the core graph service.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise group, ungroup, duplicate, rename, delete and reset parts on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** CoreDataService import-group graph; stable identifiers; undo/document persistence

**Evidence:**

- [CustomizePartsListViewModel.cs:182](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L182>) — CustomizePartsListViewModel.CommandResetPart
- [CustomizePartsListViewModel.cs:604](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L604>) — CustomizePartsListViewModel.ResetPart
- [CustomizePartsListViewModel.cs:184](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L184>) — CustomizePartsListViewModel.CommandDeletePart
- [CustomizePartsListViewModel.cs:425](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L425>) — CustomizePartsListViewModel.DeleteGroupOrPart
- [CustomizePartsListViewModel.cs:188](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L188>) — CustomizePartsListViewModel.CommandDuplicateImportGroup
- [CustomizePartsListViewModel.cs:461](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L461>) — CustomizePartsListViewModel.DuplicateSelected
- [CustomizePartsListViewModel.cs:200](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L200>) — CustomizePartsListViewModel.CommandAddPartToGroup
- [CustomizePartsListViewModel.cs:386](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L386>) — CustomizePartsListViewModel.AddToPartGroup
- [CustomizePartsListViewModel.cs:202](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L202>) — CustomizePartsListViewModel.CommandShowRenamePrompt
- [CustomizePartsListViewModel.cs:719](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L719>) — CustomizePartsListViewModel.ShowRenamePrompt
- [CustomizePartsListViewModel.cs:218](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L218>) — CustomizePartsListViewModel.CommandRenameImportGroup
- [CustomizePartsListViewModel.cs:584](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L584>) — CustomizePartsListViewModel.RenameImportGroup
- [CustomizePartsListViewModel.cs:220](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L220>) — CustomizePartsListViewModel.CommandBreakPartGroup
- [CustomizePartsListViewModel.cs:395](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L395>) — CustomizePartsListViewModel.BreakPartGroup

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-014 Part primary/secondary metal and common center

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Selected parts swap primary/secondary metal classification through the core service; common-center UI toggles MainVM state used by the transform workflow.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise part primary/secondary metal and common center on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** material classifications; CoreDataService transforms

**Evidence:**

- [CustomizePartsListViewModel.cs:190](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L190>) — CustomizePartsListViewModel.CommandTogglePrimarySecondaryMetal
- [CustomizePartsListViewModel.cs:761](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L761>) — CustomizePartsListViewModel.TogglePrimarySecondaryMetal
- [CustomizePartsListViewModel.cs:214](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L214>) — CustomizePartsListViewModel.CommandToggleCommonCenterPoint
- [CustomizePartsListViewModel.cs:736](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L736>) — CustomizePartsListViewModel.ToggleCommonCenterPoint
- [MainViewModel.cs:270](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L270>) — MainViewModel.CommandToggleFreehandCommonCenterMode
- [MainViewModel.cs:2161](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2161>) — MainViewModel.ToggleFreehandCommonCenterMode

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-015 Export selected parts and reusable personal parts library

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Selected import groups export as CSG to a chosen path or named temporary CSG then import into the user-parts library. Invalid filename characters and empty names are checked.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Export a grouped parametric part to CSG, import on iPad and PC, modify it and verify group hierarchy, metadata and controls remain editable.

**Dependencies:** CoreDataService.ImportGroupExport; CSG serializer; UserPartsViewModel

**Evidence:**

- [CustomizePartsListViewModel.cs:204](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L204>) — CustomizePartsListViewModel.CommandExportSelectedImportGroupsToPath
- [CustomizePartsListViewModel.cs:492](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L492>) — CustomizePartsListViewModel.ExportSelectedImportGroupsToPath
- [CustomizePartsListViewModel.cs:206](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L206>) — CustomizePartsListViewModel.CommandExportSelectedImportGroupsToUserParts
- [CustomizePartsListViewModel.cs:517](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L517>) — CustomizePartsListViewModel.ExportSelectedImportGroupsToUserParts

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-016 Personal CSG/3DM parts import, thumbnails and deletion

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Imports multi-selected CSG/3DM files to the personal library with CP/SP tags and GUID names; requests thumbnails, reloads disk contents, adds CSG with DocumentImportCSGPart or 3DM via InvokePartPatchWire; deletion removes model and preview files.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise personal csg/3dm parts import, thumbnails and deletion on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** local file access; CSG and 3DM readers; thumbnail generation; CoreDataService

**Evidence:**

- [UserPartsViewModel.cs:30](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UserPartsViewModel.cs#L30>) — UserPartsViewModel.CommandAddSelectedPart
- [UserPartsViewModel.cs:61](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UserPartsViewModel.cs#L61>) — UserPartsViewModel.AddSelectedPart
- [UserPartsViewModel.cs:32](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UserPartsViewModel.cs#L32>) — UserPartsViewModel.CommandDeleteUserPart
- [UserPartsViewModel.cs:146](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UserPartsViewModel.cs#L146>) — UserPartsViewModel.DeleteUserPart
- [UserPartsViewModel.cs:34](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UserPartsViewModel.cs#L34>) — UserPartsViewModel.CommandImportPart
- [UserPartsViewModel.cs:83](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UserPartsViewModel.cs#L83>) — UserPartsViewModel.ImportPart
- [UserPartsViewModel.cs:164](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UserPartsViewModel.cs#L164>) — UserPartsViewModel.ImportPart

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-017 Freehand transform controls, flip and mirror

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Freehand control population separates imported UIControls, caches transform controls and shows controls only for selected groups. Flip/mirror broadcasts to selected controls, encoding flip in Value1 and side/opposite/top/quad modes in Value2 before SetGraphValue.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Select one and multiple grouped parts; apply each flip/mirror mode, undo and redo, then round-trip the editable project to PC.

**Dependencies:** CSValueXFormObjectControl; CoreDataService; import-group graph

**Evidence:**

- [FreehandStackViewModel.cs:75](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L75>) — FreehandStackViewModel.CommandResetUIControl
- [FreehandStackViewModel.cs:222](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L222>) — FreehandStackViewModel.ResetUIControl
- [FreehandStackViewModel.cs:95](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L95>) — FreehandStackViewModel.CommandUpdateVisibleDisplayComponents
- [FreehandStackViewModel.cs:483](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L483>) — FreehandStackViewModel.UpdateVisibleDisplayComponents
- [FreehandStackViewModel.cs:169](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L169>) — FreehandStackViewModel.CommandFlipChanged
- [FreehandStackViewModel.cs:459](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L459>) — FreehandStackViewModel.UpdateGraphPartFlip
- [FreehandStackViewModel.cs:171](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L171>) — FreehandStackViewModel.CommandMirrorChanged
- [FreehandStackViewModel.cs:471](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L471>) — FreehandStackViewModel.UpdateGraphPartMirror
- [FreehandStackViewModel.cs:289](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L289>) — FreehandStackViewModel.UpdateFreehandStackControls
- [CSValueXFormObjectControl.cs:163](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueXFormObjectControl.cs#L163>) — CSValueXFormObjectControl.MirrorSelector_OnSelectionChanged
- [CSValueXFormObjectControl.cs:142](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueXFormObjectControl.cs#L142>) — CSValueXFormObjectControl.FlipSelector_OnSelectionChanged

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-018 Legacy save sorted import groups is a no-op

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CommandSaveSortedImportGroups is instantiated but its target method body is empty. This is an inventoried desktop command, not evidence of implemented reordering persistence.

**Offline disposition:** Do not invent behavior from this command; determine intended behavior with a licensed desktop fixture.

**Acceptance:**

- Verify whether dragging/reordering is saved elsewhere; do not claim this empty target provides persistence.

**Dependencies:**

**Evidence:**

- [FreehandStackViewModel.cs:83](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L83>) — FreehandStackViewModel.CommandSaveSortedImportGroups
- [FreehandStackViewModel.cs:441](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L441>) — FreehandStackViewModel.SaveSortedImportGroups

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-019 Material selectors, color families and Any Gem Any Color

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Material selection raises notifications except in per-gem assignment mode; Apply creates a material group for selected gems and clears the assigned tray stone; ClearAll clears custom assignments and repopulates the gem report.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Assign every metal/gem classification and color family offline, assign per-gem overrides, clear overrides and verify serialized group membership and render appearance.

**Dependencies:** metal/gem material catalogs; CoreDataService material groups; StoneTrayViewModel

**Evidence:**

- [MaterialOptionSelectorViewModel.cs:63](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MaterialOptionSelectorViewModel.cs#L63>) — MaterialOptionSelectorViewModel.CommandApplyAnyGemAnyColor
- [MaterialOptionSelectorViewModel.cs:285](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MaterialOptionSelectorViewModel.cs#L285>) — MaterialOptionSelectorViewModel.ApplyAnyGemAnyColor
- [MaterialOptionSelectorViewModel.cs:65](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MaterialOptionSelectorViewModel.cs#L65>) — MaterialOptionSelectorViewModel.CommandClearAllAnyGemAnyColor
- [MaterialOptionSelectorViewModel.cs:297](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MaterialOptionSelectorViewModel.cs#L297>) — MaterialOptionSelectorViewModel.ClearAllAnyGemAnyColor
- [MaterialOptionSelectorViewModel.cs:67](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MaterialOptionSelectorViewModel.cs#L67>) — MaterialOptionSelectorViewModel.CommandCloseAnyGemAnyColor
- [MaterialOptionSelectorViewModel.cs:303](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MaterialOptionSelectorViewModel.cs#L303>) — MaterialOptionSelectorViewModel.CloseAnyGemAnyColor
- [MaterialOptionSelectorViewModel.cs:69](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MaterialOptionSelectorViewModel.cs#L69>) — MaterialOptionSelectorViewModel.CommandGemColorTypeClicked
- [MaterialOptionSelectorViewModel.cs:308](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MaterialOptionSelectorViewModel.cs#L308>) — MaterialOptionSelectorViewModel.GemColorTypeClicked
- [StudioMaterialSelectionViewModel.cs:23](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StudioMaterialSelectionViewModel.cs#L23>) — StudioMaterialSelectionViewModel.CommandCloseAllOtherMaterialGroupOptions
- [StudioMaterialSelectionViewModel.cs:438](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StudioMaterialSelectionViewModel.cs#L438>) — StudioMaterialSelectionViewModel.CloseAllOtherMaterialGroupOptions
- [MaterialOptionSelectorViewModel.cs:274](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MaterialOptionSelectorViewModel.cs#L274>) — MaterialOptionSelectorViewModel.RaiseMaterialOptionSelectionChanged
- [MainViewModel.cs:298](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L298>) — MainViewModel.CommandOpenAnyGemAnyColorMaterialSelection
- [MainViewModel.cs:1740](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1740>) — MainViewModel.OpenAnyGemAnyColorMaterialSelection

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-020 Stone tray assignment, removal and design search

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Tray records are deduplicated by serial number; assignment sets the chosen diamond/material and can fail for unsupported stones. Clearing resets center-gem material; deleting calls PunchoutHelper and removes a record. Assignment-search opens the design category and closes the shopper.

**Offline disposition:** Desktop implementation requires remote service/network state. For a fully offline iPad, supply a local substitute or defer external action until connected; do not represent live values as current.

**Acceptance:**

- With locally cached stones and no network, assign/unassign/delete stones and search compatible designs; clearly identify cached stock/prices and preserve assignments on PC.

**Dependencies:** PunchoutHelper remote tray; serialized diamond records; CoreDataService

**Evidence:**

- [StoneTrayViewModel.cs:37](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StoneTrayViewModel.cs#L37>) — StoneTrayViewModel.AssignStoneSearchCommand
- [StoneTrayViewModel.cs:362](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StoneTrayViewModel.cs#L362>) — StoneTrayViewModel.AssignStoneSearch
- [StoneTrayViewModel.cs:39](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StoneTrayViewModel.cs#L39>) — StoneTrayViewModel.AssignStoneCommand
- [StoneTrayViewModel.cs:315](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StoneTrayViewModel.cs#L315>) — StoneTrayViewModel.AssignStone
- [StoneTrayViewModel.cs:41](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StoneTrayViewModel.cs#L41>) — StoneTrayViewModel.ClearAssignedStoneCommand
- [StoneTrayViewModel.cs:250](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StoneTrayViewModel.cs#L250>) — StoneTrayViewModel.ClearAssignedStone
- [StoneTrayViewModel.cs:43](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StoneTrayViewModel.cs#L43>) — StoneTrayViewModel.DeleteStoneCommand
- [StoneTrayViewModel.cs:265](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StoneTrayViewModel.cs#L265>) — StoneTrayViewModel.DeleteStone
- [StoneTrayViewModel.cs:280](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StoneTrayViewModel.cs#L280>) — StoneTrayViewModel.OpenStoneTray
- [StoneTrayViewModel.cs:221](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StoneTrayViewModel.cs#L221>) — StoneTrayViewModel.AddStonesToTray

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-021 Stone and diamond/gem shopping

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Main and search commands dispatch diamond and gemstone shoppers; gem report shoppers use selected gem dimensions/shape. Remote availability/content is not an offline catalog.

**Offline disposition:** Desktop implementation requires remote service/network state. For a fully offline iPad, supply a local substitute or defer external action until connected; do not represent live values as current.

**Acceptance:**

- In airplane mode, exercise stone and diamond/gem shopping on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** PunchoutHelper; Stuller website and APIs; browser

**Evidence:**

- [GemReportViewModel.cs:209](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L209>) — GemReportViewModel.CommandShopForDiamond
- [GemReportViewModel.cs:1015](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L1015>) — GemReportViewModel.ShopForDiamond
- [GemReportViewModel.cs:212](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L212>) — GemReportViewModel.CommandShopForGemstone
- [GemReportViewModel.cs:1020](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L1020>) — GemReportViewModel.ShopForGemstone
- [GemReportViewModel.cs:221](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L221>) — GemReportViewModel.CommandOpenStullerSite
- [GemReportViewModel.cs:871](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L871>) — GemReportViewModel.OpenStullerSite
- [MainViewModel.cs:953](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L953>) — MainViewModel.CommandShopForDiamond
- [MainViewModel.cs:2345](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2345>) — MainViewModel.ShopForDiamond
- [MainViewModel.cs:955](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L955>) — MainViewModel.CommandShopForGemstone
- [MainViewModel.cs:2350](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2350>) — MainViewModel.ShopForGemstone
- [TileSearchMainViewModel.cs:223](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L223>) — TileSearchMainViewModel.CommandShowStoneShopper
- [TileSearchMainViewModel.cs:420](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L420>) — TileSearchMainViewModel.ShowStoneShopper

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-022 Gem report grouping, sources, quantities and highlighting

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Gem report groups stones by material/shape or size option, propagates group sources/quality to stones, lets sources be purchase/customer-supplied/no-stone and highlights individual or grouped stones in the viewport. Source updates reset/revalidate quotes.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Compare report quantities, dimensions, carat totals, quality and source choices for mixed center/side/melee designs before and after PC round-trip.

**Dependencies:** CoreDataService gem enumeration; Gem and GemGroup metadata; order summary

**Evidence:**

- [GemReportViewModel.cs:185](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L185>) — GemReportViewModel.CommandUpdateSourceMeleeGems
- [GemReportViewModel.cs:1091](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L1091>) — GemReportViewModel.UpdateSourceMeleeGems
- [GemReportViewModel.cs:188](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L188>) — GemReportViewModel.CommandGemGroupSetSourceNoStone
- [GemReportViewModel.cs:630](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L630>) — GemReportViewModel.GemGroupSetSourceNoStone
- [GemReportViewModel.cs:191](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L191>) — GemReportViewModel.CommandGemGroupSetSourceProvided
- [GemReportViewModel.cs:637](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L637>) — GemReportViewModel.GemGroupSetSourceProvided
- [GemReportViewModel.cs:194](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L194>) — GemReportViewModel.CommandGemGroupSetSourceStuller
- [GemReportViewModel.cs:644](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L644>) — GemReportViewModel.GemGroupSetSourceStuller
- [GemReportViewModel.cs:197](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L197>) — GemReportViewModel.CommandGemSetSourceNoStone
- [GemReportViewModel.cs:651](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L651>) — GemReportViewModel.GemSetSourceNoStone
- [GemReportViewModel.cs:200](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L200>) — GemReportViewModel.CommandGemSetSourceProvided
- [GemReportViewModel.cs:657](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L657>) — GemReportViewModel.GemSetSourceProvided
- [GemReportViewModel.cs:203](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L203>) — GemReportViewModel.CommandGemSetSourceStuller
- [GemReportViewModel.cs:663](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L663>) — GemReportViewModel.GemSetSourceStuller
- [GemReportViewModel.cs:206](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L206>) — GemReportViewModel.CommandHighlightCancel
- [GemReportViewModel.cs:669](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L669>) — GemReportViewModel.HighlightCancel
- [GemReportViewModel.cs:215](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L215>) — GemReportViewModel.CommandHighlightGem
- [GemReportViewModel.cs:677](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L677>) — GemReportViewModel.HighlightGem
- [GemReportViewModel.cs:218](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L218>) — GemReportViewModel.CommandHighlightGemGroup
- [GemReportViewModel.cs:685](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L685>) — GemReportViewModel.HighlightGemGroup
- [GemReportViewModel.cs:227](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L227>) — GemReportViewModel.CommandUpdateGemGroups
- [GemReportViewModel.cs:480](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L480>) — GemReportViewModel.UpdateGemGroupsFromGems
- [GemReportViewModel.cs:230](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L230>) — GemReportViewModel.CommandUpdateGemsFromGemGroups
- [GemReportViewModel.cs:485](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L485>) — GemReportViewModel.UpdateGemsFromGemGroups
- [GemReportViewModel.cs:529](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L529>) — GemReportViewModel.CreateGemGroups
- [GemReportViewModel.cs:449](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L449>) — GemReportViewModel.UpdateFromUserData
- [MainViewModel.cs:398](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L398>) — MainViewModel.CommandViewGemInfo
- [MainViewModel.cs:2234](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2234>) — MainViewModel.ViewGemInfo

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-023 Customer portfolios create, rename, duplicate, delete and choose

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Portfolio creation writes customer contact details via ProjectManagementHelper; rename/copy/delete delegate to it; dialog commands select portfolio-manager visual states. Rename editing can be enabled.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise customer portfolios create, rename, duplicate, delete and choose on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** ProjectManagementHelper; local project directories; customer information

**Evidence:**

- [PortfolioManagerViewModel.cs:86](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L86>) — PortfolioManagerViewModel.CommandCreateNewProject
- [PortfolioManagerViewModel.cs:483](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L483>) — PortfolioManagerViewModel.CreateNewProject
- [PortfolioManagerViewModel.cs:90](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L90>) — PortfolioManagerViewModel.CommandDeleteSelectedProject
- [PortfolioManagerViewModel.cs:508](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L508>) — PortfolioManagerViewModel.DeleteSelectedProject
- [PortfolioManagerViewModel.cs:94](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L94>) — PortfolioManagerViewModel.CommandRenameSelectedProject
- [PortfolioManagerViewModel.cs:567](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L567>) — PortfolioManagerViewModel.RenameSelectedProject
- [PortfolioManagerViewModel.cs:100](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L100>) — PortfolioManagerViewModel.CommandSelectedProjectSaveAs
- [PortfolioManagerViewModel.cs:595](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L595>) — PortfolioManagerViewModel.SelectedProjectSaveAs
- [PortfolioManagerViewModel.cs:102](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L102>) — PortfolioManagerViewModel.CommandShowCreatePortfolio
- [PortfolioManagerViewModel.cs:603](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L603>) — PortfolioManagerViewModel.ShowCreatePortfolio
- [PortfolioManagerViewModel.cs:104](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L104>) — PortfolioManagerViewModel.CommandShowDeletePortfolioDialog
- [PortfolioManagerViewModel.cs:608](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L608>) — PortfolioManagerViewModel.ShowDeletePortfolioDialog
- [PortfolioManagerViewModel.cs:106](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L106>) — PortfolioManagerViewModel.CommandShowDuplicatePortfolioDialog
- [PortfolioManagerViewModel.cs:613](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L613>) — PortfolioManagerViewModel.ShowDuplicatePortfolioDialog
- [PortfolioManagerViewModel.cs:108](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L108>) — PortfolioManagerViewModel.CommandShowRenamePortfolioDialog
- [PortfolioManagerViewModel.cs:618](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L618>) — PortfolioManagerViewModel.ShowRenamePortfolioDialog
- [PortfolioManagerViewModel.cs:220](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L220>) — PortfolioManagerViewModel.CommandEnableProjectRename
- [PortfolioManagerViewModel.cs:636](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L636>) — PortfolioManagerViewModel.EnableProjectRename
- [MainViewModel.cs:318](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L318>) — MainViewModel.CommandOpenPortfolioManager
- [MainViewModel.cs:1814](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1814>) — MainViewModel.OpenPortfolioManager

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-024 Portfolio design reopen, delete, label and customer detail save

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Opening checks the design path exists then LoadHelper.LoadDesign and closes the manager. Deleting a design delegates to project management; labels and customer contact/comment fields save project data.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise portfolio design reopen, delete, label and customer detail save on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** ProjectManagementHelper; LoadHelper; project metadata; editable CSG documents

**Evidence:**

- [PortfolioManagerViewModel.cs:88](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L88>) — PortfolioManagerViewModel.CommandDeleteDesignFromPortfolio
- [PortfolioManagerViewModel.cs:503](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L503>) — PortfolioManagerViewModel.DeleteDesignFromPortfolio
- [PortfolioManagerViewModel.cs:92](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L92>) — PortfolioManagerViewModel.CommandOpenDesign
- [PortfolioManagerViewModel.cs:534](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L534>) — PortfolioManagerViewModel.OpenDesign
- [PortfolioManagerViewModel.cs:96](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L96>) — PortfolioManagerViewModel.CommandSaveProjectData
- [PortfolioManagerViewModel.cs:573](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L573>) — PortfolioManagerViewModel.SaveProjectData
- [PortfolioManagerViewModel.cs:98](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L98>) — PortfolioManagerViewModel.CommandSaveUserInfo
- [PortfolioManagerViewModel.cs:578](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L578>) — PortfolioManagerViewModel.SaveUserInfo
- [PortfolioManagerViewModel.cs:100](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L100>) — PortfolioManagerViewModel.CommandSelectedProjectSaveAs
- [PortfolioManagerViewModel.cs:595](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L595>) — PortfolioManagerViewModel.SelectedProjectSaveAs
- [PortfolioManagerViewModel.cs:110](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L110>) — PortfolioManagerViewModel.CommandUpdateDesignUserLabel
- [PortfolioManagerViewModel.cs:623](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PortfolioManagerViewModel.cs#L623>) — PortfolioManagerViewModel.UpdateDesignUserLabel

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-025 Save current design and quick portfolio

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Main SaveCurrentDesign routes to ProjectManagementHelper, quick portfolio prompts for a name and creates a customer portfolio; import chooses CSG files and copies/opens through the project helper.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Create a project, save design and freehand changes, kill/restart app offline, then open the exact files on the PC and continue parameter editing.

**Dependencies:** CSG serializer; ProjectManagementHelper; native file picker

**Evidence:**

- [MainViewModel.cs:209](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L209>) — MainViewModel.CommandCreateQuickPortfolio
- [MainViewModel.cs:1464](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1464>) — MainViewModel.CreateQuickPortfolio
- [MainViewModel.cs:292](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L292>) — MainViewModel.CommandImportDesign
- [MainViewModel.cs:1581](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1581>) — MainViewModel.ImportDesign
- [MainViewModel.cs:356](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L356>) — MainViewModel.CommandSaveCurrentDesign
- [MainViewModel.cs:1320](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1320>) — MainViewModel.SaveCurrentDesign

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-026 New session, undo, redo and order reset

- [ ] Implement and verify the acceptance criteria below.

**Observed:** New-session path checks document state and resets application data; Undo and Redo call the core service, and reset-order clears current order information separately.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise new session, undo, redo and order reset on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** CoreDataService undo/redo; project modified state; order state

**Evidence:**

- [MainViewModel.cs:296](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L296>) — MainViewModel.CommandNewSessionClick
- [MainViewModel.cs:1709](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1709>) — MainViewModel.NewSessionClick
- [MainViewModel.cs:352](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L352>) — MainViewModel.CommandRedo
- [MainViewModel.cs:1263](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1263>) — MainViewModel.Redo
- [MainViewModel.cs:354](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L354>) — MainViewModel.CommandResetOrderInformation
- [MainViewModel.cs:1312](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1312>) — MainViewModel.ResetOrderInformation
- [MainViewModel.cs:388](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L388>) — MainViewModel.CommandUndo
- [MainViewModel.cs:1404](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1404>) — MainViewModel.Undo

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-027 Backup create/restore, recovery path and portfolio locations

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Backup view commands call BackupHelper, pick CSB recovery files and folders, reset paths, and open the configured portfolio directories; statistics report portfolio count, backup size and free space.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Create CSB backup with projects and personal parts, restore to a clean offline installation and verify editable PC project content and metadata.

**Dependencies:** BackupHelper; local filesystem; CSB container; settings

**Evidence:**

- [BackupRestoreViewModel.cs:25](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L25>) — BackupRestoreViewModel.CommandResetRecoveryFileDirectory
- [BackupRestoreViewModel.cs:206](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L206>) — BackupRestoreViewModel.ResetRecoveryFileDirectory
- [BackupRestoreViewModel.cs:27](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L27>) — BackupRestoreViewModel.CommandResetPortfolioPathToDefault
- [BackupRestoreViewModel.cs:201](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L201>) — BackupRestoreViewModel.ResetPortfolioPathToDefault
- [BackupRestoreViewModel.cs:29](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L29>) — BackupRestoreViewModel.CommandResetBackupDirectory
- [BackupRestoreViewModel.cs:194](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L194>) — BackupRestoreViewModel.ResetBackupDirectory
- [BackupRestoreViewModel.cs:31](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L31>) — BackupRestoreViewModel.CommandRestoreBackup
- [BackupRestoreViewModel.cs:225](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L225>) — BackupRestoreViewModel.RestoreBackup
- [BackupRestoreViewModel.cs:33](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L33>) — BackupRestoreViewModel.CommandBrowseRecoveryLocation
- [BackupRestoreViewModel.cs:211](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L211>) — BackupRestoreViewModel.ResetRecoveryLocation
- [BackupRestoreViewModel.cs:35](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L35>) — BackupRestoreViewModel.CommandCreateBackup
- [BackupRestoreViewModel.cs:149](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L149>) — BackupRestoreViewModel.CreateBackup
- [BackupRestoreViewModel.cs:37](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L37>) — BackupRestoreViewModel.CommandBrowseBackupDirectory
- [BackupRestoreViewModel.cs:140](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L140>) — BackupRestoreViewModel.BrowseBackupDirectory
- [BackupRestoreViewModel.cs:39](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L39>) — BackupRestoreViewModel.CommandSelectPortfolioSaveLocation
- [BackupRestoreViewModel.cs:231](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L231>) — BackupRestoreViewModel.SelectPortfolioLocation
- [BackupRestoreViewModel.cs:41](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L41>) — BackupRestoreViewModel.CommandOpenDefaultPortfolioFolderLocation
- [BackupRestoreViewModel.cs:168](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L168>) — BackupRestoreViewModel.OpenDefaultPortfolioFolderLocation
- [BackupRestoreViewModel.cs:111](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L111>) — BackupRestoreViewModel.CommandOpenUserPortfolioFolderLocation
- [BackupRestoreViewModel.cs:181](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L181>) — BackupRestoreViewModel.OpenUserPortfolioFolderLocation
- [MainViewModel.cs:350](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L350>) — MainViewModel.CommandQuickBackup
- [MainViewModel.cs:1996](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1996>) — MainViewModel.QuickBackup
- [BackupRestoreViewModel.cs:133](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/BackupRestoreViewModel.cs#L133>) — BackupRestoreViewModel.UpdateBackupStatistics

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-028 Quote notes, attachments and preview

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Notes window opens/closes; clear-notes and clear-files reset local collections, attach uses a file dialog. More-than-a-ring preview is a modal window.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise quote notes, attachments and preview on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** local supporting files; order summary; preview controls

**Evidence:**

- [OrderSummaryViewModel.cs:694](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L694>) — OrderSummaryViewModel.CommandCloseDesignNotesWindow
- [OrderSummaryViewModel.cs:2771](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L2771>) — OrderSummaryViewModel.CloseDesignNotesWindow
- [OrderSummaryViewModel.cs:697](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L697>) — OrderSummaryViewModel.CommandOpenDesignNotesWindow
- [OrderSummaryViewModel.cs:2886](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L2886>) — OrderSummaryViewModel.OpenDesignNotesWindow
- [OrderSummaryViewModel.cs:700](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L700>) — OrderSummaryViewModel.CommandOpenMoreThanARingPreview
- [OrderSummaryViewModel.cs:2896](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L2896>) — OrderSummaryViewModel.OpenMoreThanARingPreview
- [OrderSummaryViewModel.cs:1786](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L1786>) — OrderSummaryViewModel.CommandAttachFile
- [OrderSummaryViewModel.cs:2210](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L2210>) — OrderSummaryViewModel.AttachFile
- [OrderSummaryViewModel.cs:1789](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L1789>) — OrderSummaryViewModel.CommandClearFiles
- [OrderSummaryViewModel.cs:2219](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L2219>) — OrderSummaryViewModel.ClearFiles
- [OrderSummaryViewModel.cs:1792](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L1792>) — OrderSummaryViewModel.CommandClearDesignNotes
- [OrderSummaryViewModel.cs:2224](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L2224>) — OrderSummaryViewModel.ClearDesignNotes

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-029 Quote request, stone validation and service pricing

- [ ] Implement and verify the acceptance criteria below.

**Observed:** SendQuote rejects empty canvas and inaccessible internet, shops nonserialized gems when enabled, validates assignments and obtains pricing asynchronously. This is not offline authoritative quoting.

**Offline disposition:** Desktop implementation requires remote service/network state. For a fully offline iPad, supply a local substitute or defer external action until connected; do not represent live values as current.

**Acceptance:**

- Offline compute an explicitly local estimate from versioned price tables; queue a quote request with stable idempotency and review stale price/stock before online submission.

**Dependencies:** Stuller pricing/order APIs; live stone inventory; CoreDataService volumes; buyer/account data

**Evidence:**

- [OrderSummaryViewModel.cs:712](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L712>) — OrderSummaryViewModel.CommandQuoteSubmit
- [OrderSummaryViewModel.cs:3138](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L3138>) — OrderSummaryViewModel.SendQuote
- [OrderSummaryViewModel.cs:3707](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L3707>) — OrderSummaryViewModel.ValidateStoneAssignments
- [OrderSummaryViewModel.cs:2779](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L2779>) — OrderSummaryViewModel.DoVolumeCalculation

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-030 Order submission confirmation and supporting CAD/report files

- [ ] Implement and verify the acceptance criteria below.

**Observed:** SendOrder populates buyer identity, preview and confirmation; supporting document builders encode CAD, pricing screenshot and stone report files for the request. Confirmation click sets ProceedWithOrder and closes the modal. CommandOrderSubmit itself has no initializer in the audited baseline.

**Offline disposition:** Desktop implementation requires remote service/network state. For a fully offline iPad, supply a local substitute or defer external action until connected; do not represent live values as current.

**Acceptance:**

- Prepare complete order offline with editable CAD and supporting files; on reconnection review totals and explicitly submit once, recording returned order identifiers.

**Dependencies:** Stuller order APIs; authentication/key profile; CSG and attachments; pricing image/report

**Evidence:**

- [OrderSummaryViewModel.cs:2918](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L2918>) — OrderSummaryViewModel.SendOrder
- [OrderSummaryViewModel.cs:2641](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L2641>) — OrderSummaryViewModel.AddDesignDoc
- [OrderSummaryViewModel.cs:2668](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L2668>) — OrderSummaryViewModel.AddSupportingDocs
- [WindowOrderConfirmationPrompt.cs:107](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.View/WindowOrderConfirmationPrompt.cs#L107>) — WindowOrderConfirmationPrompt.Order_Click

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-031 Quote print command is empty in baseline

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CommandPrintQuote is wired to PrintQuote, whose body is empty. Do not infer working quote printing from its binding or button.

**Offline disposition:** Determine actual print/report route before implementing parity.

**Acceptance:**

- Check desktop print pathways and fixture output; treat this specific command as no-op until runtime or another source route establishes behavior.

**Dependencies:**

**Evidence:**

- [OrderSummaryViewModel.cs:706](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L706>) — OrderSummaryViewModel.CommandPrintQuote
- [OrderSummaryViewModel.cs:2328](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L2328>) — OrderSummaryViewModel.PrintQuote

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-032 Quote/order history, saved design reopen, pricing and resubmit

- [ ] Implement and verify the acceptance criteria below.

**Observed:** History opens a local saved design into the active project, marks pricing image viewing, links reorder to Stuller CAD/CAM projects, and sends stored CSG bytes to the order service for resubmission.

**Offline disposition:** Desktop implementation requires remote service/network state. For a fully offline iPad, supply a local substitute or defer external action until connected; do not represent live values as current.

**Acceptance:**

- Browse cached history and reopen editable design offline; defer reorder/resubmit and never present a queued submission as accepted.

**Dependencies:** local order history; saved CSG; pricing images; Stuller order service

**Evidence:**

- [MainViewModel.cs:316](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L316>) — MainViewModel.CommandOpenQuoteHistory
- [MainViewModel.cs:1871](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1871>) — MainViewModel.OpenQuoteHistory
- [WindowOrderHistoryViewModel.cs:25](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowOrderHistoryViewModel.cs#L25>) — WindowOrderHistoryViewModel.CommandResubmitDesign
- [WindowOrderHistoryViewModel.cs:210](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowOrderHistoryViewModel.cs#L210>) — WindowOrderHistoryViewModel.ResubmitDesign
- [WindowOrderHistoryViewModel.cs:27](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowOrderHistoryViewModel.cs#L27>) — WindowOrderHistoryViewModel.CommandReorder
- [WindowOrderHistoryViewModel.cs:91](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowOrderHistoryViewModel.cs#L91>) — WindowOrderHistoryViewModel.Reorder
- [WindowOrderHistoryViewModel.cs:29](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowOrderHistoryViewModel.cs#L29>) — WindowOrderHistoryViewModel.CommandCloseWindow
- [WindowOrderHistoryViewModel.cs:77](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowOrderHistoryViewModel.cs#L77>) — WindowOrderHistoryViewModel.CloseWindow
- [WindowOrderHistoryViewModel.cs:31](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowOrderHistoryViewModel.cs#L31>) — WindowOrderHistoryViewModel.CommandOpenDesign
- [WindowOrderHistoryViewModel.cs:96](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowOrderHistoryViewModel.cs#L96>) — WindowOrderHistoryViewModel.OpenDesign
- [WindowOrderHistoryViewModel.cs:35](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowOrderHistoryViewModel.cs#L35>) — WindowOrderHistoryViewModel.CommandViewPricing
- [WindowOrderHistoryViewModel.cs:241](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowOrderHistoryViewModel.cs#L241>) — WindowOrderHistoryViewModel.ViewPricing
- [WindowOrderResultViewModel.cs:18](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowOrderResultViewModel.cs#L18>) — WindowOrderResultViewModel.CommandOpenSubmissionForm
- [WindowOrderResultViewModel.cs:71](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowOrderResultViewModel.cs#L71>) — WindowOrderResultViewModel.OpenSubmissionForm

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-033 Quick estimate display, edit rows/columns and print

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Estimate view toggles edit mode, persists excluded rows/columns, filters metal names, displays missing gem details, and prints the supplied FrameworkElement with optional screenshot. Color-selection RelayCommand properties are uninitialized; row exclusions are the implemented route.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Supply a saved estimate matrix offline; configure rows/columns, print/export with screenshot and missing-gem details; restart to verify persisted exclusions.

**Dependencies:** settings; quick estimate result matrix; FrameworkElement print; CoreDataService capture

**Evidence:**

- [QuickEstimateViewModel.cs:165](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/QuickEstimateViewModel.cs#L165>) — QuickEstimateViewModel.CommandPrintQuickEstimate
- [QuickEstimateViewModel.cs:707](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/QuickEstimateViewModel.cs#L707>) — QuickEstimateViewModel.PrintQuickEstimate
- [QuickEstimateViewModel.cs:167](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/QuickEstimateViewModel.cs#L167>) — QuickEstimateViewModel.CommandShowEditMode
- [QuickEstimateViewModel.cs:649](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/QuickEstimateViewModel.cs#L649>) — QuickEstimateViewModel.ShowEditMode
- [QuickEstimateViewModel.cs:169](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/QuickEstimateViewModel.cs#L169>) — QuickEstimateViewModel.CommandHideEditMode
- [QuickEstimateViewModel.cs:633](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/QuickEstimateViewModel.cs#L633>) — QuickEstimateViewModel.HideEditMode
- [QuickEstimateViewModel.cs:282](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/QuickEstimateViewModel.cs#L282>) — QuickEstimateViewModel.CommandSetQEColumnVisibile
- [QuickEstimateViewModel.cs:724](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/QuickEstimateViewModel.cs#L724>) — QuickEstimateViewModel.SetQEColumnVisibile
- [QuickEstimateViewModel.cs:284](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/QuickEstimateViewModel.cs#L284>) — QuickEstimateViewModel.CommandSetQEColumnCollapsed
- [QuickEstimateViewModel.cs:719](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/QuickEstimateViewModel.cs#L719>) — QuickEstimateViewModel.SetQEColumnCollapsed

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-034 Quick estimate generation and weights

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Main quick-estimate entry calls SendQuickEstimateShowcaseMarkups; estimate generation checks internet, excludes already ordered/empty designs, groups gems and requests service estimates. Weights derive from core material volumes and metal density helpers.

**Offline disposition:** Desktop implementation requires remote service/network state. For a fully offline iPad, supply a local substitute or defer external action until connected; do not represent live values as current.

**Acceptance:**

- Use versioned offline labor/metal/gem tables to calculate estimates and label date/currency; compare with saved desktop fixtures across primary/secondary metals.

**Dependencies:** Stuller pricing; CoreDataService volumes; metal density tables; exchange-rate settings

**Evidence:**

- [MainViewModel.cs:302](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L302>) — MainViewModel.CommandOpenQuickEstimateWindow
- [MainViewModel.cs:1856](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1856>) — MainViewModel.OpenQuickEstimateWindow
- [OrderSummaryViewModel.cs:3373](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L3373>) — OrderSummaryViewModel.SendQuickEstimateShowcaseMarkups
- [OrderSummaryViewModel.cs:3679](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L3679>) — OrderSummaryViewModel.UpdateWeightsFromVolumes

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-035 Design 3DM/STL export with stone subsets and mesh repair

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Export dialog retrieves used/allowed counts and bonuses from DesignExportService, applies feature/purchase or remaining-count checks, exports 3DM or STL with center/side/accent toggles and STL repair, records successful exports and refreshes counts. Both dialog entry and quota reporting depend on internet.

**Offline disposition:** Desktop implementation requires remote service/network state. For a fully offline iPad, supply a local substitute or defer external action until connected; do not represent live values as current.

**Acceptance:**

- Export selected stone subsets to STL/3DM offline using authorized local entitlement; verify geometry. Separately round-trip CSG editable state because mesh/3DM output alone does not prove parametric parity.

**Dependencies:** DesignExportService quotas/purchase entitlement; CoreDataService 3DM/STL writer; mesh repair

**Evidence:**

- [MainViewModel.cs:286](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L286>) — MainViewModel.CommandExportDesign
- [MainViewModel.cs:1545](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1545>) — MainViewModel.ExportDesign
- [OrderSummaryViewModel.cs:709](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L709>) — OrderSummaryViewModel.CommandExportSTL3DM
- [OrderSummaryViewModel.cs:2804](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L2804>) — OrderSummaryViewModel.ExportSTL3DM
- [WindowExportDesignViewModel.cs:84](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowExportDesignViewModel.cs#L84>) — WindowExportDesignViewModel.CommandBrowse
- [WindowExportDesignViewModel.cs:282](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowExportDesignViewModel.cs#L282>) — WindowExportDesignViewModel.Browse
- [WindowExportDesignViewModel.cs:86](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowExportDesignViewModel.cs#L86>) — WindowExportDesignViewModel.CommandExportDesign
- [WindowExportDesignViewModel.cs:297](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowExportDesignViewModel.cs#L297>) — WindowExportDesignViewModel.ExportDesign

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-036 Batch search-result image export

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Exports each selected DesignSearch image to a picked folder with progress and preview, then can open that folder. Main wrapper obtains the current results for this dialog.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise batch search-result image export on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** local thumbnails; DesignSearch.ExportImage; file access

**Evidence:**

- [MainViewModel.cs:284](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L284>) — MainViewModel.CommandExportSearchImages
- [MainViewModel.cs:1568](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1568>) — MainViewModel.ExportSearchImages
- [WindowExportSearchResultsViewModel.cs:45](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowExportSearchResultsViewModel.cs#L45>) — WindowExportSearchResultsViewModel.CommandView
- [WindowExportSearchResultsViewModel.cs:201](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowExportSearchResultsViewModel.cs#L201>) — WindowExportSearchResultsViewModel.View
- [WindowExportSearchResultsViewModel.cs:47](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowExportSearchResultsViewModel.cs#L47>) — WindowExportSearchResultsViewModel.CommandExport
- [WindowExportSearchResultsViewModel.cs:165](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowExportSearchResultsViewModel.cs#L165>) — WindowExportSearchResultsViewModel.Export
- [WindowExportSearchResultsViewModel.cs:49](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowExportSearchResultsViewModel.cs#L49>) — WindowExportSearchResultsViewModel.CommandPickFolder
- [WindowExportSearchResultsViewModel.cs:194](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowExportSearchResultsViewModel.cs#L194>) — WindowExportSearchResultsViewModel.PickFolder

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-037 Render dialog, still render and settings

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Render options enumerate bundled props/materials/environments; selection controls lay-flat mode, prop/ground-plane alpha, resolution, environment and save directory. Render delegates all selections to RenderGenerateRender. Defaults save to settings; reset rereads saved options; preview toggles Rhino safe frame and resolution.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Offline render catalog and freehand designs at every resolution, lay-flat and try-on, with/without props and alpha; compare image framing and export dimensions.

**Dependencies:** bundled props/environments/ground-plane materials; Rhino/render engine; settings; image encoder

**Evidence:**

- [MainViewModel.cs:378](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L378>) — MainViewModel.CommandGenerateRender
- [MainViewModel.cs:1576](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1576>) — MainViewModel.GenerateRender
- [MainViewModel.cs:380](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L380>) — MainViewModel.CommandOpenRenderDialogue
- [MainViewModel.cs:1887](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1887>) — MainViewModel.OpenRenderDialogue
- [WindowRenderOptionsViewModel.cs:56](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowRenderOptionsViewModel.cs#L56>) — WindowRenderOptionsViewModel.CommandRender
- [WindowRenderOptionsViewModel.cs:971](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowRenderOptionsViewModel.cs#L971>) — WindowRenderOptionsViewModel.RenderAndClose
- [WindowRenderOptionsViewModel.cs:62](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowRenderOptionsViewModel.cs#L62>) — WindowRenderOptionsViewModel.CommandSetRenderSaveLocation
- [WindowRenderOptionsViewModel.cs:1027](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowRenderOptionsViewModel.cs#L1027>) — WindowRenderOptionsViewModel.SetRenderSaveLocation
- [WindowRenderOptionsViewModel.cs:982](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowRenderOptionsViewModel.cs#L982>) — WindowRenderOptionsViewModel.UpdateRenderPreviewSquareInViewport
- [WindowRenderOptionsViewModel.cs:938](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowRenderOptionsViewModel.cs#L938>) — WindowRenderOptionsViewModel.ReadRenderOptionsFromCSOptions
- [WindowRenderOptionsViewModel.cs:58](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowRenderOptionsViewModel.cs#L58>) — WindowRenderOptionsViewModel.CommandSaveDefaults
- [WindowRenderOptionsViewModel.cs:1011](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowRenderOptionsViewModel.cs#L1011>) — WindowRenderOptionsViewModel.SaveDefaults
- [WindowRenderOptionsViewModel.cs:60](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowRenderOptionsViewModel.cs#L60>) — WindowRenderOptionsViewModel.CommandResetOptions
- [WindowRenderOptionsViewModel.cs:977](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowRenderOptionsViewModel.cs#L977>) — WindowRenderOptionsViewModel.Reset

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-038 Hand try-on, gender, skin, nail polish and finger placement

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Try-on turns the hand on/off, composes Male_/Female_ hand asset names for selected finger, passes right-hand/flipped/position offset to UpdateRhinoHand, and applies skin/nail materials; reset selects initial skin/polish and disables hand.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- For each supported finger/gender, switch hand side, flip and offset ring, skin/polish and render; verify document geometry is unchanged and presentation state is reproducible.

**Dependencies:** bundled try-on meshes and material swatches; CoreDataService; ring transform

**Evidence:**

- [MainViewModel.cs:266](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L266>) — MainViewModel.CommandDesignTryOn
- [MainViewModel.cs:1491](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1491>) — MainViewModel.DesignTryOn
- [MainViewModel.cs:310](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L310>) — MainViewModel.CommandOpenDesignTryOnWindow
- [MainViewModel.cs:1806](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1806>) — MainViewModel.OpenDesignTryOnWindow
- [WindowDesignTryOnViewModel.cs:231](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowDesignTryOnViewModel.cs#L231>) — WindowDesignTryOnViewModel.CommandCloseWindow
- [WindowDesignTryOnViewModel.cs:501](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowDesignTryOnViewModel.cs#L501>) — WindowDesignTryOnViewModel.CloseWindow
- [WindowDesignTryOnViewModel.cs:233](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowDesignTryOnViewModel.cs#L233>) — WindowDesignTryOnViewModel.CommandRemoveHand
- [WindowDesignTryOnViewModel.cs:731](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowDesignTryOnViewModel.cs#L731>) — WindowDesignTryOnViewModel.RemoveHand
- [WindowDesignTryOnViewModel.cs:235](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowDesignTryOnViewModel.cs#L235>) — WindowDesignTryOnViewModel.CommandUpdateHandGender
- [WindowDesignTryOnViewModel.cs:736](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowDesignTryOnViewModel.cs#L736>) — WindowDesignTryOnViewModel.UpdateHandGender
- [WindowDesignTryOnViewModel.cs:237](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowDesignTryOnViewModel.cs#L237>) — WindowDesignTryOnViewModel.CommandUpdateHandObjects
- [WindowDesignTryOnViewModel.cs:742](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowDesignTryOnViewModel.cs#L742>) — WindowDesignTryOnViewModel.UpdateHandObjects
- [WindowDesignTryOnViewModel.cs:243](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowDesignTryOnViewModel.cs#L243>) — WindowDesignTryOnViewModel.CommandUpdateSelectedRing
- [WindowDesignTryOnViewModel.cs:768](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowDesignTryOnViewModel.cs#L768>) — WindowDesignTryOnViewModel.UpdateSelectedRing
- [WindowDesignTryOnViewModel.cs:241](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowDesignTryOnViewModel.cs#L241>) — WindowDesignTryOnViewModel.CommandUpdateRhinoViewportObjects
- [WindowDesignTryOnViewModel.cs:753](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowDesignTryOnViewModel.cs#L753>) — WindowDesignTryOnViewModel.UpdateRhinoViewportObjects

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-039 Camera presets, spin, grid and control highlight

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Top/front/side/perspective commands call core viewport camera methods; spin start/stop, grid visibility and stack highlighting toggle core/presentation state. Distance measurement enters the native measurement workflow.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise camera presets, spin, grid and control highlight on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** CoreDataService viewport engine; camera state; measurement tools

**Evidence:**

- [MainViewModel.cs:268](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L268>) — MainViewModel.CommandDisableRhinoViewportGridlines
- [MainViewModel.cs:1505](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1505>) — MainViewModel.DisableRhinoViewportGridlines
- [MainViewModel.cs:272](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L272>) — MainViewModel.CommandDisableStackHighlight
- [MainViewModel.cs:1511](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1511>) — MainViewModel.DisableStackHighlight
- [MainViewModel.cs:280](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L280>) — MainViewModel.CommandEnableRhinoViewportGridlines
- [MainViewModel.cs:1531](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1531>) — MainViewModel.EnableRhinoViewportGridlines
- [MainViewModel.cs:282](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L282>) — MainViewModel.CommandEnableStackHighlight
- [MainViewModel.cs:1539](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1539>) — MainViewModel.EnableStackHighlight
- [MainViewModel.cs:370](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L370>) — MainViewModel.CommandSetRhinoCameraViewFront
- [MainViewModel.cs:2047](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2047>) — MainViewModel.SetRhinoCameraViewFront
- [MainViewModel.cs:372](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L372>) — MainViewModel.CommandSetRhinoCameraViewPerspective
- [MainViewModel.cs:2053](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2053>) — MainViewModel.SetRhinoCameraViewPerspective
- [MainViewModel.cs:374](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L374>) — MainViewModel.CommandSetRhinoCameraViewSide
- [MainViewModel.cs:2059](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2059>) — MainViewModel.SetRhinoCameraViewSide
- [MainViewModel.cs:376](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L376>) — MainViewModel.CommandSetRhinoCameraViewTop
- [MainViewModel.cs:2065](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2065>) — MainViewModel.SetRhinoCameraViewTop
- [MainViewModel.cs:384](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L384>) — MainViewModel.CommandSpinDesign
- [MainViewModel.cs:2146](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2146>) — MainViewModel.SpinDesign
- [MainViewModel.cs:386](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L386>) — MainViewModel.CommandSpinDesignStop
- [MainViewModel.cs:2154](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2154>) — MainViewModel.SpinDesignStop
- [MainViewModel.cs:412](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L412>) — MainViewModel.CommandPerformDistanceMeasurement
- [MainViewModel.cs:1991](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1991>) — MainViewModel.PerformDistanceMeasurement

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-040 Previous/next main section

- [ ] Implement and verify the acceptance criteria below.

**Observed:** MoveBackward decrements SectionPageNumber unless zero; MoveForward increments unless two, both via NavigateToSection. These are adjacent-section operations.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise previous/next main section on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** section page state

**Evidence:**

- [MainViewModel.cs:825](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L825>) — MainViewModel.CommandMoveForward
- [MainViewModel.cs:1701](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1701>) — MainViewModel.MoveForward
- [MainViewModel.cs:829](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L829>) — MainViewModel.CommandMoveBackward
- [MainViewModel.cs:1693](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1693>) — MainViewModel.MoveBackward

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-041 Theme colors, custom logo and viewport background

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Theme commands load/reset logo, set/reset background bitmap/color, apply/reset theme and accent, and persist stretch state. File paths and Windows color dialogs need local iPad document/color-picker equivalents.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise theme colors, custom logo and viewport background on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** local images; theme service/settings; CoreDataService viewport background

**Evidence:**

- [ThemePickerViewModel.cs:35](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L35>) — ThemePickerViewModel.CommandLoadCustomLogo
- [ThemePickerViewModel.cs:253](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L253>) — ThemePickerViewModel.LoadCustomLogo
- [ThemePickerViewModel.cs:37](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L37>) — ThemePickerViewModel.CommandSetViewPortBackground
- [ThemePickerViewModel.cs:311](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L311>) — ThemePickerViewModel.SetViewPortBackground
- [ThemePickerViewModel.cs:39](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L39>) — ThemePickerViewModel.CommandResetCustomLogo
- [ThemePickerViewModel.cs:268](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L268>) — ThemePickerViewModel.ResetCustomLogo
- [ThemePickerViewModel.cs:41](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L41>) — ThemePickerViewModel.CommandResetThemes
- [ThemePickerViewModel.cs:284](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L284>) — ThemePickerViewModel.ResetThemes
- [ThemePickerViewModel.cs:43](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L43>) — ThemePickerViewModel.CommandResetViewPortBackgroundColor
- [ThemePickerViewModel.cs:293](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L293>) — ThemePickerViewModel.ResetViewPortBackgroundColor
- [ThemePickerViewModel.cs:45](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L45>) — ThemePickerViewModel.CommandResetViewPortGraphic
- [ThemePickerViewModel.cs:303](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L303>) — ThemePickerViewModel.ResetViewPortGraphic
- [ThemePickerViewModel.cs:47](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L47>) — ThemePickerViewModel.CommandSetViewPortBackgroundColor
- [ThemePickerViewModel.cs:346](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L346>) — ThemePickerViewModel.SetViewPortBackgroundColor
- [ThemePickerViewModel.cs:49](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L49>) — ThemePickerViewModel.CommandApplyCustomColorTheme
- [ThemePickerViewModel.cs:224](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L224>) — ThemePickerViewModel.ApplyCustomColorTheme
- [ThemePickerViewModel.cs:51](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L51>) — ThemePickerViewModel.CommandResetHightlightAccentColor
- [ThemePickerViewModel.cs:275](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L275>) — ThemePickerViewModel.ResetHightlightAccentColor
- [ThemePickerViewModel.cs:161](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L161>) — ThemePickerViewModel.CommandViewportBitmapStretchedChanged
- [ThemePickerViewModel.cs:248](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ThemePickerViewModel.cs#L248>) — ThemePickerViewModel.ViewportBitmapStretchedChanged

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-042 Order screen section/source/finishing visibility

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Preference section command toggles selected UI section; options persistence turns disabled order-summary items into visibility rules for gem sourcing, hallmark and finishing controls.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Disable each section/option, save and restart offline; verify form visibility and validation stay consistent.

**Dependencies:** DisabledOrderSummaryItems settings; OrderSummaryViewModel

**Evidence:**

- [PreferenceOrderScreenOptionsViewModel.cs:15](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PreferenceOrderScreenOptionsViewModel.cs#L15>) — PreferenceOrderScreenOptionsViewModel.CommandToggleSection
- [PreferenceOrderScreenOptionsViewModel.cs:150](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/PreferenceOrderScreenOptionsViewModel.cs#L150>) — PreferenceOrderScreenOptionsViewModel.ToggleSection
- [OrderSummaryViewModel.cs:3114](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L3114>) — OrderSummaryViewModel.UpdateControlVisibilityFromCSOptions

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-043 General preferences, ring standards, language and performance

- [ ] Implement and verify the acceptance criteria below.

**Observed:** SaveOptions persists model/SPI labels, stack expansion and advanced options, freehand/estimate visibility, gem rounding, new ribbons, last portfolio, viewport handle scale, material/order preferences, parallel processing and language. Ring-size changes update core document setting; language may prompt application restart. Currency refresh requests exchange rate remotely.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Change every preference and restart in airplane mode; verify persistence and geometry/price units. Freeze exchange rate with an explicit date until connectivity returns.

**Dependencies:** settings storage; translation resources; core ring-size standards; optional exchange-rate service

**Evidence:**

- [WindowModalOptions.cs:463](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/WindowModalOptions.cs#L463>) — WindowModalOptions.SaveOptions
- [WindowModalOptions.cs:291](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/WindowModalOptions.cs#L291>) — WindowModalOptions.BtnCancelClick
- [MainViewModel.cs:320](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L320>) — MainViewModel.CommandOpenPreferencesWindow
- [MainViewModel.cs:1137](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1137>) — MainViewModel.OpenPreferencesWindow

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-044 Application/library updates, defer and new content

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Update commands apply downloaded application or library changes, update library database, reset readiness flags and persist delay; new-library view generates sample thumbnails and links to new designs/parts.

**Offline disposition:** Desktop implementation requires remote service/network state. For a fully offline iPad, supply a local substitute or defer external action until connected; do not represent live values as current.

**Acceptance:**

- Install signed prepackaged content updates locally and verify versioned schema migration; defer network download when offline.

**Dependencies:** downloaded update packages; library updater/database; external application installer

**Evidence:**

- [UpdateInfoViewModel.cs:94](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UpdateInfoViewModel.cs#L94>) — UpdateInfoViewModel.CommandApplyUpdates
- [UpdateInfoViewModel.cs:319](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UpdateInfoViewModel.cs#L319>) — UpdateInfoViewModel.ApplyUpdates
- [UpdateInfoViewModel.cs:96](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UpdateInfoViewModel.cs#L96>) — UpdateInfoViewModel.CommandDelayUpdates
- [UpdateInfoViewModel.cs:389](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UpdateInfoViewModel.cs#L389>) — UpdateInfoViewModel.DelayUpdates
- [UpdateInfoViewModel.cs:195](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UpdateInfoViewModel.cs#L195>) — UpdateInfoViewModel.CommandShowNewDesigns
- [UpdateInfoViewModel.cs:430](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UpdateInfoViewModel.cs#L430>) — UpdateInfoViewModel.ShowNewDesigns
- [UpdateInfoViewModel.cs:197](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UpdateInfoViewModel.cs#L197>) — UpdateInfoViewModel.CommandShowNewParts
- [UpdateInfoViewModel.cs:421](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UpdateInfoViewModel.cs#L421>) — UpdateInfoViewModel.ShowNewParts
- [UpdateInfoViewModel.cs:379](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UpdateInfoViewModel.cs#L379>) — UpdateInfoViewModel.ApplyLibraryUpdate
- [UpdateInfoViewModel.cs:441](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/UpdateInfoViewModel.cs#L441>) — UpdateInfoViewModel.GenerateNewLibraryInfo

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-045 User guides, release notes, resource images and about

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Main entrypoints show about, bundled/addendum user-guide files, release-note windows and resource image viewer. Actual browser URLs and file launch paths may need offline bundled equivalents.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- In airplane mode, exercise user guides, release notes, resource images and about on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** local documentation/resources; external file/browser launcher

**Evidence:**

- [MainViewModel.cs:300](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L300>) — MainViewModel.CommandOpenAboutWindow
- [MainViewModel.cs:1723](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1723>) — MainViewModel.OpenAboutWindow
- [MainViewModel.cs:326](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L326>) — MainViewModel.CommandOpenReleaseNotesLibraryWindow
- [MainViewModel.cs:1171](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1171>) — MainViewModel.OpenReleaseNotesLibraryWindow
- [MainViewModel.cs:328](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L328>) — MainViewModel.CommandOpenReleaseNotesWindow
- [MainViewModel.cs:1191](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1191>) — MainViewModel.OpenReleaseNotesWindow
- [MainViewModel.cs:330](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L330>) — MainViewModel.CommandOpenResourceImageView
- [MainViewModel.cs:1911](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1911>) — MainViewModel.OpenResourceImageView
- [MainViewModel.cs:338](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L338>) — MainViewModel.CommandOpenUserGuide
- [MainViewModel.cs:1950](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1950>) — MainViewModel.OpenUserGuide
- [MainViewModel.cs:340](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L340>) — MainViewModel.CommandOpenUserGuideAddendum
- [MainViewModel.cs:1974](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1974>) — MainViewModel.OpenUserGuideAddendum

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-046 Troubleshooting, logs, debug report, email and remote assistance

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Support commands open logs/troubleshooting, prepare email, send a quick debug report or launch TeamViewer. These target Windows processes and external communication; debug/user-level visibility gates apply.

**Offline disposition:** Desktop implementation requires remote service/network state. For a fully offline iPad, supply a local substitute or defer external action until connected; do not represent live values as current.

**Acceptance:**

- In airplane mode, exercise troubleshooting, logs, debug report, email and remote assistance on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** Windows process launching; debug logs; report upload/email; TeamViewer

**Evidence:**

- [MainViewModel.cs:334](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L334>) — MainViewModel.CommandOpenTeamViewer
- [MainViewModel.cs:1929](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1929>) — MainViewModel.OpenTeamViewer
- [MainViewModel.cs:336](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L336>) — MainViewModel.CommandOpenTroubleshootingWindow
- [MainViewModel.cs:1941](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1941>) — MainViewModel.OpenTroubleshootingWindow
- [MainViewModel.cs:358](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L358>) — MainViewModel.CommandSendEmail
- [MainViewModel.cs:2015](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2015>) — MainViewModel.SendEmail
- [MainViewModel.cs:362](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L362>) — MainViewModel.CommandSendQuickDebugReport
- [MainViewModel.cs:2028](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2028>) — MainViewModel.SendQuickDebugReport
- [MainViewModel.cs:396](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L396>) — MainViewModel.CommandViewCounterSketchLog
- [MainViewModel.cs:2226](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2226>) — MainViewModel.ViewCounterSketchLog

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-047 Account information, unregister and country override

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Account entrypoint opens account modal; unregister invokes the licensing/security workflow; alternate country code changes local/account behavior. Offline editability depends on licensing/profile policy outside this UI audit.

**Offline disposition:** Desktop implementation requires remote service/network state. For a fully offline iPad, supply a local substitute or defer external action until connected; do not represent live values as current.

**Acceptance:**

- In airplane mode, exercise account information, unregister and country override on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** SecurityHelper; key profile/licensing; account service

**Evidence:**

- [MainViewModel.cs:306](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L306>) — MainViewModel.CommandOpenAccountInfoWindow
- [MainViewModel.cs:1734](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1734>) — MainViewModel.OpenAccountInfoWindow
- [MainViewModel.cs:392](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L392>) — MainViewModel.CommandUnregisterComputer
- [MainViewModel.cs:2188](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2188>) — MainViewModel.UnregisterComputer
- [MainViewModel.cs:394](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L394>) — MainViewModel.CommandUseAlternateCountryCode
- [MainViewModel.cs:2219](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2219>) — MainViewModel.UseAlternateCountryCode

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-048 CAM forms, printer output and ProJet setup

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Main commands open CAM standards/mounting/stone forms, printer workflows for ProJet1200 and EnvisionTec, driver setup/instructions and entitlement/upsell checks. These are desktop device integrations, not iPad-ready paths.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Offline produce validated manufacturing files for equivalent print workflows; direct USB/device control requires supported iPad integration and real hardware validation.

**Dependencies:** native CAM/core geometry; printer drivers and USB; ProJet/EnvisionTec integrations

**Evidence:**

- [MainViewModel.cs:207](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L207>) — MainViewModel.CommandInstallProjetUsbDrivers
- [MainViewModel.cs:1669](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1669>) — MainViewModel.InstallProjetUsbDrivers
- [MainViewModel.cs:254](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L254>) — MainViewModel.CommandSetCamModeState
- [MainViewModel.cs:2037](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2037>) — MainViewModel.SetCamModeState
- [MainViewModel.cs:342](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L342>) — MainViewModel.CommandOpenCamStandards
- [MainViewModel.cs:1772](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1772>) — MainViewModel.OpenCamStandards
- [MainViewModel.cs:344](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L344>) — MainViewModel.CommandOpenCamMountingForm
- [MainViewModel.cs:1755](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1755>) — MainViewModel.OpenCamMountingForm
- [MainViewModel.cs:346](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L346>) — MainViewModel.CommandOpenCamStoneForm
- [MainViewModel.cs:1789](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1789>) — MainViewModel.OpenCamStoneForm
- [MainViewModel.cs:370](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L370>) — MainViewModel.CommandSetRhinoCameraViewFront
- [MainViewModel.cs:2047](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2047>) — MainViewModel.SetRhinoCameraViewFront
- [MainViewModel.cs:372](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L372>) — MainViewModel.CommandSetRhinoCameraViewPerspective
- [MainViewModel.cs:2053](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2053>) — MainViewModel.SetRhinoCameraViewPerspective
- [MainViewModel.cs:374](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L374>) — MainViewModel.CommandSetRhinoCameraViewSide
- [MainViewModel.cs:2059](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2059>) — MainViewModel.SetRhinoCameraViewSide
- [MainViewModel.cs:376](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L376>) — MainViewModel.CommandSetRhinoCameraViewTop
- [MainViewModel.cs:2065](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2065>) — MainViewModel.SetRhinoCameraViewTop
- [MainViewModel.cs:406](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L406>) — MainViewModel.CommandOpenPrintToProJet1200
- [MainViewModel.cs:1829](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1829>) — MainViewModel.OpenPrintToProject1200
- [MainViewModel.cs:408](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L408>) — MainViewModel.CommandOpenPrintToEnvisionTec
- [MainViewModel.cs:1824](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1824>) — MainViewModel.OpenPrintToEnvisionTec
- [MainViewModel.cs:410](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L410>) — MainViewModel.CommandCheckProjetAndUpsell
- [MainViewModel.cs:1419](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1419>) — MainViewModel.CheckProjetAndUpsell
- [MainViewModel.cs:895](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L895>) — MainViewModel.CommandOpenProjetInstructions
- [MainViewModel.cs:1839](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1839>) — MainViewModel.OpenProjetInstructions

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-049 Window controls, resolution presets and clipboard capture

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Main commands resize to preset desktop dimensions, toggle maximize state, close application and copy a FrameworkElement image to clipboard. These are host-specific presentation operations.

**Offline disposition:** Replace with iPad scene sizing, share sheet, clipboard and app lifecycle behavior.

**Acceptance:**

- In airplane mode, exercise window controls, resolution presets and clipboard capture on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** desktop WPF window manager; clipboard bitmap capture

**Evidence:**

- [MainViewModel.cs:250](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L250>) — MainViewModel.CommandCloseCounterSketch
- [MainViewModel.cs:1453](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1453>) — MainViewModel.CloseCounterSketch
- [MainViewModel.cs:252](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L252>) — MainViewModel.CommandCopyFrameworkElementToClipboard
- [MainViewModel.cs:1459](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L1459>) — MainViewModel.CopyFrameworkElementToClipboard
- [MainViewModel.cs:256](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L256>) — MainViewModel.CommandWindowSize1080
- [MainViewModel.cs:2259](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2259>) — MainViewModel.WindowSize1080
- [MainViewModel.cs:258](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L258>) — MainViewModel.CommandWindowSize1050
- [MainViewModel.cs:2252](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2252>) — MainViewModel.WindowSize1050
- [MainViewModel.cs:897](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L897>) — MainViewModel.CommandToggleWindowMaximized
- [MainViewModel.cs:2166](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2166>) — MainViewModel.ToggleWindowMaximized
- [ItemChooserViewModelBase.cs:29](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ItemChooserViewModelBase.cs#L29>) — ItemChooserViewModelBase.CommandUpdateMouseOverText

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-050 Feature-gated internal test commands

- [ ] Implement and verify the acceptance criteria below.

**Observed:** CommandExcuteTest invokes a named internal test switch. Static visibility/profile gates and test effects must be inspected before exposing any equivalent to production users.

**Offline disposition:** Retain only authorized diagnostics with local exportable logs.

**Acceptance:**

- In airplane mode, exercise feature-gated internal test commands on representative catalog and freehand designs; verify resulting UI state, saved document, and reopening on the desktop against the cited behavior.

**Dependencies:** internal test helpers; feature/user profile

**Evidence:**

- [MainViewModel.cs:938](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L938>) — MainViewModel.CommandExcuteTest
- [MainViewModel.cs:2298](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L2298>) — MainViewModel.ExecuteTest

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-051 Generic chooser confirm/cancel/clear and hover descriptions

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Chooser commands accept a chosen item, cancel or clear chooser state; material subclass assigns its hover command to update primary text/description. Generic chooser hover reset declaration is not sufficient evidence of wiring.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Open each material chooser, preview hover/focus descriptions, accept, cancel and clear; confirm downstream selection behavior and touch equivalents.

**Dependencies:** chooser VM state; modal event/messaging

**Evidence:**

- [ItemChooserViewModelBase.cs:138](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ItemChooserViewModelBase.cs#L138>) — ItemChooserViewModelBase.CommandOK
- [ItemChooserViewModelBase.cs:172](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ItemChooserViewModelBase.cs#L172>) — ItemChooserViewModelBase.OK
- [ItemChooserViewModelBase.cs:140](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ItemChooserViewModelBase.cs#L140>) — ItemChooserViewModelBase.CommandCancel
- [ItemChooserViewModelBase.cs:152](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ItemChooserViewModelBase.cs#L152>) — ItemChooserViewModelBase.Cancel
- [ItemChooserViewModelBase.cs:142](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ItemChooserViewModelBase.cs#L142>) — ItemChooserViewModelBase.CommandClear
- [ItemChooserViewModelBase.cs:157](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ItemChooserViewModelBase.cs#L157>) — ItemChooserViewModelBase.Clear
- [MaterialOptionSelectorViewModel.cs:328](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MaterialOptionSelectorViewModel.cs#L328>) — MaterialOptionSelectorViewModel.UpdateMouseOverText

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-052 Design image summary placeholder

- [ ] Implement and verify the acceptance criteria below.

**Observed:** PopulateImages deletes prior temporary top/right/front/perspective PNGs and starts a BackgroundWorker. Its workerThread_DoWork body is empty; completion assigns image paths and sends loaded. This does not establish functional four-view image capture.

**Offline disposition:** Inspect alternate screenshot routes before promising four-view report capture.

**Acceptance:**

- Verify whether valid images are produced through another path; do not count this empty worker as implemented capture.

**Dependencies:** temporary image paths; background worker

**Evidence:**

- [DesignImageSummaryViewModel.cs:31](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/DesignImageSummaryViewModel.cs#L31>) — DesignImageSummaryViewModel.CommandPopulateImages
- [DesignImageSummaryViewModel.cs:102](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/DesignImageSummaryViewModel.cs#L102>) — DesignImageSummaryViewModel.PopulateImages
- [DesignImageSummaryViewModel.cs:134](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/DesignImageSummaryViewModel.cs#L134>) — DesignImageSummaryViewModel.workerThread_DoWork

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-053 Range sliders, expanded limits, reset and fine/coarse steps

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Range controls send quick graph updates after a 300 ms debounce and full update on release; Ctrl multiplies and Shift divides step size by ten while dragging. Expand widens min/max and stores ExpandedSlider graph user data; reset restores original bounds/default and clears expansion flag. Dual-range has matching quick/full interaction.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Drag and type boundary/fine/coarse values, expand bounds, reset and undo; confirm identical graph values and limits after PC save/reopen.

**Dependencies:** UIControl min/max/step/default values; CSUIControl messenger; CoreDataService graph user data

**Evidence:**

- [CSValueRangeControl.cs:112](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueRangeControl.cs#L112>) — CSValueRangeControl.BtnExpandControlClick
- [CSValueRangeControl.cs:137](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueRangeControl.cs#L137>) — CSValueRangeControl.Reset
- [CSValueRangeControl.cs:227](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueRangeControl.cs#L227>) — CSValueRangeControl.SliderValueChanged
- [CSValueRangeControl.cs:202](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueRangeControl.cs#L202>) — CSValueRangeControl.SliderPreviewKeyDown
- [CSValueDualRangeControl.cs:185](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueDualRangeControl.cs#L185>) — CSValueDualRangeControl.SliderValueChanged

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-054 Control factory, lists, on/off, text/numeric input and outputs

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Control factory switches on model UIControlType to instantiate range, dual-range, ring-size, gem, shape, chooser, toggle, list, font, head style, output, input and transform controls. SetGraphValue sends quick/full UIControl notifications to the design stack, which updates the graph and marks the project modified.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Exercise every UIControlType present in the bundled library, including output-only controls, and verify complete metadata and editing semantics on PC.

**Dependencies:** complete UIControl schema; CoreDataService.SetUIControl; localized control names

**Evidence:**

- [CSCategoryControl.cs:187](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSCategoryControl.cs#L187>) — CSCategoryControl.CreateControlFromUIControlObject
- [CSUIControl.cs:579](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSUIControl.cs#L579>) — CSUIControl.SetGraphValue
- [CSValueInputControl.cs:72](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueInputControl.cs#L72>) — CSValueInputControl.TxtValue_OnLostFocus
- [CSValueOnOffControl.cs:44](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueOnOffControl.cs#L44>) — CSValueOnOffControl.SetControl
- [CSValueListControl.cs:70](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueListControl.cs#L70>) — CSValueListControl.SetControl

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-055 Ring size standards and selection

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Ring-size control populates display lookup from numeric bounds/step; selection updates LocalUIControl.Value1 and sends a full graph update. Document ring-size preference changes are sent by options to the core.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- For each supported regional standard, select smallest/default/largest size, switch standard and verify physical dimensions and editable size after desktop reopening.

**Dependencies:** ring-size conversion tables; UIControl; CoreDataService

**Evidence:**

- [CSValueRingSizeControl.cs:116](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueRingSizeControl.cs#L116>) — CSValueRingSizeControl.ComboBoxSelectionChanged
- [CSValueRingSizeControl.cs:136](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueRingSizeControl.cs#L136>) — CSValueRingSizeControl.UpdateRingSizeDisplayLookup

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-056 Gem shape, table sizes, custom dimensions and assigned stone fit

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Gem-size slider changes GemTableIndex and displays carat/height/width/depth with quick updates and full release. Lost focus on custom width/height/depth/percentage parses invariant-culture numbers and marks custom size. Shape and assigned tray-stone events refresh the control; reset restores defaults.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Change all shape/size-table/custom dimension inputs and assigned-stone fit, including percentage and unsupported size cases; compare numeric geometry and saved control state on PC.

**Dependencies:** gem size tables; stone fit metadata; CSUIControl; core geometry

**Evidence:**

- [CSValueGemSizeControl.cs:245](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueGemSizeControl.cs#L245>) — CSValueGemSizeControl.SldSizeValueChanged
- [CSValueGemSizeControl.cs:270](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueGemSizeControl.cs#L270>) — CSValueGemSizeControl.TxtDepthLostFocus
- [CSValueGemSizeControl.cs:280](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueGemSizeControl.cs#L280>) — CSValueGemSizeControl.TxtHeightLostFocus
- [CSValueGemSizeControl.cs:300](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueGemSizeControl.cs#L300>) — CSValueGemSizeControl.TxtWidthLostFocus
- [CSValueGemSizeControl.cs:290](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueGemSizeControl.cs#L290>) — CSValueGemSizeControl.TxtPercentageLostFocus
- [CSValueGemShapeControl.cs:169](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueGemShapeControl.cs#L169>) — CSValueGemShapeControl.Selector_OnSelectionChanged

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-057 Head style, font selection and profile chooser

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Font and head-style selectors update the local UIControl data-item index and graph; reset restores the default. Profile chooser populates DataItems, changes selection, and opens a copy-edit curve workflow through AccessCurveEditorCopy.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Use each supported font/head/profile, reset, save and reopen on PC; preserve font identity or declare substitution before design changes.

**Dependencies:** DataItem library; installed/bundled fonts; curve profiles; core graph

**Evidence:**

- [CSValueFontListControl.cs:138](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueFontListControl.cs#L138>) — CSValueFontListControl.CboItems_SelectionChanged
- [CSValueHeadStyleListControl.cs:136](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueHeadStyleListControl.cs#L136>) — CSValueHeadStyleListControl.Selector_OnSelectionChanged
- [CSValueChooserControl.cs:129](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueChooserControl.cs#L129>) — CSValueChooserControl.Selector_OnSelectionChanged
- [CSValueChooserControl.cs:114](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSValueChooserControl.cs#L114>) — CSValueChooserControl.ButtonCurveEditorCopyClicked

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-058 Curve profile editor control-point drag, mirroring and rebuilding

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Legacy curve editor draws NURBS polylines/edit points, changes control vertices or edit-point parameters from pointer motion, supports mirrored points/curve, anti-alias display and mouse-wheel zoom. Rebuild uses cubic degree and altered CV count, then sends CurveEditorUpdateCurveObjectFromUI. Native NURBS methods remain core dependencies.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Edit open/closed/clamped/unclamped profiles with each symmetry option, rebuild, undo/save and reopen on PC, verifying exact curve parameters and resulting solids.

**Dependencies:** NURBS representation and evaluator; CoreDataService CurveEditorNurbs functions; WinForms pointer canvas

**Evidence:**

- [CurveEditorForm.cs:202](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.LegacyUIControl/CurveEditorForm.cs#L202>) — CurveEditorForm.SetCurve
- [CurveEditorForm.cs:99](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.LegacyUIControl/CurveEditorForm.cs#L99>) — CurveEditorForm.DrawCurve
- [CurveEditorForm.cs:384](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.LegacyUIControl/CurveEditorForm.cs#L384>) — CurveEditorForm.frmCurveEditor_MouseWheel
- [CurveEditorForm.cs:691](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.LegacyUIControl/CurveEditorForm.cs#L691>) — CurveEditorForm.Rebuild
- [CurveEditorForm.cs:785](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.LegacyUIControl/CurveEditorForm.cs#L785>) — CurveEditorForm.UpdateCurve

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-059 Default metal/gem groups and account-specific material options

- [ ] Implement and verify the acceptance criteria below.

**Observed:** DefaultMaterialGroupOptions selects saved defaults for primary/secondary metal and center/side/accent gems, applies key-profile restrictions, then calls MaterialsSetMetal and MaterialsSetGem; account changes reload material choices. These are setter/event workflows beyond RelayCommands.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Change default materials/account profile offline, open another design and verify permitted choices/defaults plus saved per-design materials.

**Dependencies:** material tables and entitlement; saved defaults; CoreDataService material assignment

**Evidence:**

- [StudioMaterialSelectionViewModel.cs:94](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StudioMaterialSelectionViewModel.cs#L94>) — StudioMaterialSelectionViewModel.DefaultMaterialGroupOptions
- [StudioMaterialSelectionViewModel.cs:206](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StudioMaterialSelectionViewModel.cs#L206>) — StudioMaterialSelectionViewModel.ReloadMaterialsAfterAccountChanged

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-060 Keyboard shortcuts and touch/pointer adaptations

- [ ] Implement and verify the acceptance criteria below.

**Observed:** MainWindow handles Escape to tiles, Ctrl+S save, Ctrl+Z undo, Ctrl+Y redo, gated Ctrl+I/Shift debug and Ctrl+D/Shift debug windows; Windows key handling changes in streaming mode. Multi-select uses Ctrl and sliders use Ctrl/Shift, so iPad needs visible alternatives.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Verify hardware-keyboard shortcuts on iPad plus touch-accessible save/undo/redo/multiselect/fine-step actions and correct text-field focus.

**Dependencies:** keyboard/mouse input; touch equivalents; feature/user profile

**Evidence:**

- [MainWindow.cs:396](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio/MainWindow.cs#L396>) — MainWindow.MainWindow_KeyDown
- [MainWindow.cs:534](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio/MainWindow.cs#L534>) — MainWindow.MainWindow_MouseDown

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-061 Image zoom/pan and scrollable/touch lists

- [ ] Implement and verify the acceptance criteria below.

**Observed:** ZoomBorder applies scale/translation using mouse wheel and pointer events with reset; paged, scrollable and touch list controls implement selection/scroll/drag adapters. Raw handler bodies for all such controls are retained in ui-surfaces.json for touch reconstruction.

**Offline disposition:** Replace pointer event implementation with iPad pan/pinch/scroll interactions while preserving selection and reset semantics.

**Acceptance:**

- Check long catalogs, paged lists, image zoom reset and drag gestures with touch/Pencil/keyboard; preserve selected item while scrolling.

**Dependencies:** image ScaleTransform/TranslateTransform; WPF scrolling and pointer capture

**Evidence:**

- [ZoomBorder.cs:101](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/ZoomBorder.cs#L101>) — ZoomBorder.Child_MouseWheel
- [ZoomBorder.cs:89](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/ZoomBorder.cs#L89>) — ZoomBorder.Child_MouseMove
- [ZoomBorder.cs:53](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/ZoomBorder.cs#L53>) — ZoomBorder.Reset

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-062 Login credentials, serial-code validation and account options

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Login normalizes serial-code formatting then calls SecurityHelper.Authenticate(username,password bytes,serial) and closes on authenticated state. Account modal persists account settings and updates exchange/culture; login screens also provide cancel and password keyboard behavior.

**Offline disposition:** Desktop implementation requires remote service/network state. For a fully offline iPad, supply a local substitute or defer external action until connected; do not represent live values as current.

**Acceptance:**

- Establish a legitimate offline entitlement policy and secure cached credentials; after restart in airplane mode, open/edit/save authorized projects and visibly defer sign-in/account refresh.

**Dependencies:** authentication service; key/serial profile; account data; currency exchange

**Evidence:**

- [CSLoginControl.cs:186](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSLoginControl.cs#L186>) — CSLoginControl.Authenticate
- [CSLoginControl.cs:205](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/CSLoginControl.cs#L205>) — CSLoginControl.btnLogin_Click
- [WindowModalAccount.cs:248](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/WindowModalAccount.cs#L248>) — WindowModalAccount.SaveOptions
- [WindowModalAccount.cs:221](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/WindowModalAccount.cs#L221>) — WindowModalAccount.UpdateExchange

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-063 Timed project recovery and close handling

- [ ] Implement and verify the acceptance criteria below.

**Observed:** MainWindow configures timed backup timer, executes TimedBackupTimerOnTick and handles window closing with current project state. iPad background suspension requires explicit lifecycle save/recovery tests.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Modify a saved and unsaved project; suspend, terminate and restore the iPad app offline at each stage and verify recoverable editable data on desktop.

**Dependencies:** ProjectManagementHelper; timer; filesystem; app lifecycle

**Evidence:**

- [MainWindow.cs:144](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio/MainWindow.cs#L144>) — MainWindow.UpdateTimedBackupTimer
- [MainWindow.cs:824](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio/MainWindow.cs#L824>) — MainWindow.TimedBackupTimerOnTick
- [MainWindow.cs:239](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio/MainWindow.cs#L239>) — MainWindow.MainWindow_Closing

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-064 Finishing, hallmarking, engraving and More Than A Ring options

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Finishing/Hallmarking option getters filter settings/profile choices. Selecting finishing can reset/revalidate quote and stone settings; engraving exposes message, font, size and fill-color choices; More Than A Ring physical/digital/none choices are mutually coordinated in setters. Quote validation includes missing buyer/reference, stone assignment, expanded slider, supplied-stone, manual-size and freehand rules.

**Offline disposition:** Reimplement locally with bundled data and a compatible geometry/document engine; Windows UI and service calls require iPad equivalents.

**Acceptance:**

- Offline edit every finishing/hallmark/engraving/packaging choice, persist and reopen on PC; verify validation and exact fields in a deferred quote/order request.

**Dependencies:** order-summary property setters; enum/options tables; feature/key profile; core gem/manual-sizing status

**Evidence:**

- [OrderSummaryViewModel.cs:1558](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L1558>) — OrderSummaryViewModel.SelectedFinishing
- [OrderSummaryViewModel.cs:1591](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L1591>) — OrderSummaryViewModel.SelectedHallmarking
- [OrderSummaryViewModel.cs:1482](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L1482>) — OrderSummaryViewModel.SelectedEngraving
- [OrderSummaryViewModel.cs:1609](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L1609>) — OrderSummaryViewModel.SelectedMoreThanARing
- [OrderSummaryViewModel.cs:418](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L418>) — OrderSummaryViewModel.ValidationErrors

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-065 Internal resource-icon gallery and clipboard HTML

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Resource viewer enumerates DrawingBrush keys from DesignStackSliderIcons, formats names, renders icon images into inline-base64 HTML and copies selected content as HTML/text. This is an internal resource inspection route distinct from customer reports.

**Offline disposition:** Provide a local diagnostics asset browser if internal tooling parity is in scope.

**Acceptance:**

- Open the bundled icon dictionary offline, export/copy selected icons and verify labels and artwork.

**Dependencies:** resource dictionaries; DrawingBrush rendering; clipboard

**Evidence:**

- [WindowResourceImageView.cs:92](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.View/WindowResourceImageView.cs#L92>) — WindowResourceImageView.ComboBoxSelectionChanged
- [WindowResourceImageView.cs:125](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.View/WindowResourceImageView.cs#L125>) — WindowResourceImageView.CopyExecuted
- [WindowResourceImageView.cs:157](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.View/WindowResourceImageView.cs#L157>) — WindowResourceImageView.GetHTMLDumpString

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-066 Embedded shopping browser and return to mounting search

- [ ] Implement and verify the acceptance criteria below.

**Observed:** WindowBrowser loads its stored URI into Chromium and logs frame navigation. Search Mountings selects design category zero, enters search and closes the browser; message Close closes the window.

**Offline disposition:** Desktop implementation requires remote service/network state. For a fully offline iPad, supply a local substitute or defer external action until connected; do not represent live values as current.

**Acceptance:**

- From cached/offline shopping UI return to the matching mounting search without losing assigned stone; external browsing indicates offline state.

**Dependencies:** Chromium browser; remote shopper pages; search categories

**Evidence:**

- [WindowBrowser.cs:110](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/WindowBrowser.cs#L110>) — WindowBrowser.TheWindowLoaded
- [WindowBrowser.cs:102](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.Controls/WindowBrowser.cs#L102>) — WindowBrowser.ButtonSearchMountings_Click

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-067 Declared command properties without resolved instantiation

- [ ] Implement and verify the acceptance criteria below.

**Observed:** The baseline contains 44 RelayCommand declarations without a resolved new RelayCommand assignment. These include legacy feature labels and alternate routes; a declaration or supplemental XAML binding alone does not establish operational behavior. See raw commandInventory entries and global reference counts.

**Offline disposition:** Resolve intended behavior using a baseline executable/reference fixtures before including it as an implemented requirement.

**Acceptance:**

- For each listed declaration, identify setter/delegate/runtime invocation or explicitly retire as dormant; compare supplemental XAML build carefully.

**Dependencies:**

**Evidence:**

- [CustomizePartsListViewModel.cs:150](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L150>) — CustomizePartsListViewModel.CommandAddToPartGroup
- [CustomizePartsListViewModel.cs:208](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L208>) — CustomizePartsListViewModel.CommandMakeSelectedPartGroupParent
- [CustomizePartsListViewModel.cs:210](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/CustomizePartsListViewModel.cs#L210>) — CustomizePartsListViewModel.CommandSelectGroup
- [FreehandStackViewModel.cs:77](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L77>) — FreehandStackViewModel.CommandDeleteFreehandControl
- [FreehandStackViewModel.cs:79](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L79>) — FreehandStackViewModel.CommandDuplicateImportGroup
- [FreehandStackViewModel.cs:81](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L81>) — FreehandStackViewModel.CommandDuplicateSelectedImportGroup
- [FreehandStackViewModel.cs:87](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L87>) — FreehandStackViewModel.CommandSelectImportedGroupByIdentifier
- [FreehandStackViewModel.cs:89](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L89>) — FreehandStackViewModel.CommandSetImportGroupToPrimaryMetal
- [FreehandStackViewModel.cs:91](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L91>) — FreehandStackViewModel.CommandSetImportGroupToSecondaryMetal
- [FreehandStackViewModel.cs:93](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/FreehandStackViewModel.cs#L93>) — FreehandStackViewModel.CommandSetImportGroupVisibilityByIdentifier
- [GemReportViewModel.cs:224](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/GemReportViewModel.cs#L224>) — GemReportViewModel.CommandShopForStone
- [ItemChooserViewModelBase.cs:31](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/ItemChooserViewModelBase.cs#L31>) — ItemChooserViewModelBase.CommandResetMouseOverText
- [MainViewModel.cs:260](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L260>) — MainViewModel.CommandCreateBugWorkItem
- [MainViewModel.cs:262](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L262>) — MainViewModel.CommandCreateProductBacklogWorkItem
- [MainViewModel.cs:264](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L264>) — MainViewModel.CommandDesignFeedback
- [MainViewModel.cs:274](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L274>) — MainViewModel.CommandDownloadTutorials
- [MainViewModel.cs:288](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L288>) — MainViewModel.CommandFreeze
- [MainViewModel.cs:290](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L290>) — MainViewModel.CommandHideViewportControls
- [MainViewModel.cs:294](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L294>) — MainViewModel.CommandImportPart
- [MainViewModel.cs:308](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L308>) — MainViewModel.CommandOpenBackupOptions
- [MainViewModel.cs:312](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L312>) — MainViewModel.CommandOpenLink
- [MainViewModel.cs:314](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L314>) — MainViewModel.CommandOpenLiveChatWindow
- [MainViewModel.cs:322](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L322>) — MainViewModel.CommandOpenPrintCatalogHistoryWindow
- [MainViewModel.cs:324](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L324>) — MainViewModel.CommandOpenPrintCatalogWindow
- [MainViewModel.cs:348](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L348>) — MainViewModel.CommandOpenUsersOptions
- [MainViewModel.cs:360](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L360>) — MainViewModel.CommandSendFeedback
- [MainViewModel.cs:390](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L390>) — MainViewModel.CommandUnFreeze
- [MainViewModel.cs:400](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L400>) — MainViewModel.CommandViewNeedsTestingBugs
- [MainViewModel.cs:402](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L402>) — MainViewModel.CommandViewOpenBugs
- [MainViewModel.cs:404](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L404>) — MainViewModel.CommandViewProductBacklog
- [MainViewModel.cs:827](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/MainViewModel.cs#L827>) — MainViewModel.CommandMaximizeRestoreMainWindow
- [OrderSummaryViewModel.cs:703](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L703>) — OrderSummaryViewModel.CommandOrderSubmit
- [OrderSummaryViewModel.cs:715](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/OrderSummaryViewModel.cs#L715>) — OrderSummaryViewModel.CommandSaveQuote
- [QuickEstimateViewModel.cs:159](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/QuickEstimateViewModel.cs#L159>) — QuickEstimateViewModel.CommandSetMetalToWhite
- [QuickEstimateViewModel.cs:161](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/QuickEstimateViewModel.cs#L161>) — QuickEstimateViewModel.CommandSetMetalToYellow
- [QuickEstimateViewModel.cs:163](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/QuickEstimateViewModel.cs#L163>) — QuickEstimateViewModel.CommandSetMetalToAll
- [StoneTrayViewModel.cs:58](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/StoneTrayViewModel.cs#L58>) — StoneTrayViewModel.OpenStoneTrayCommand
- [TileSearchCategoryViewModel.cs:27](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchCategoryViewModel.cs#L27>) — TileSearchCategoryViewModel.CommandLaunchStyleQuiz
- [TileSearchMainViewModel.cs:82](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileSearchMainViewModel.cs#L82>) — TileSearchMainViewModel.CommandSetSelectedSearchTile
- [TileViewViewModelBase.cs:196](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L196>) — TileViewViewModelBase.CommandLoadLayout
- [TileViewViewModelBase.cs:204](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L204>) — TileViewViewModelBase.CommandSetTileSize
- [TileViewViewModelBase.cs:281](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L281>) — TileViewViewModelBase.CommandAddShowcaseTile
- [TileViewViewModelBase.cs:283](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/TileViewViewModelBase.cs#L283>) — TileViewViewModelBase.CommandAddCustomURLTile
- [WindowDesignTryOnViewModel.cs:239](<../../../recovery/canonical/desktop/GV.CounterSketch.Studio.ViewModel/WindowDesignTryOnViewModel.cs#L239>) — WindowDesignTryOnViewModel.CommandUpdateRhinoHand

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.

## UI-068 Supplemental markup commands absent from baseline

- [ ] Implement and verify the acceptance criteria below.

**Observed:** Recovered supplemental markup binds ThemePickerVM.CommandGetAvailableThemes, PartsToolBoxVM.CommandSelectTool/CommandReplaceImage/CommandPopulateParts and CommandSetBackgroundColor. No declarations/implementations for these names were found in the baseline C# tree. This is a build/content mismatch or unresolved implementation, not a confirmed working feature.

**Offline disposition:** Resolve build identity or explicitly specify/implement intended offline behavior before claiming parity.

**Acceptance:**

- Obtain matching baseline markup or controlled desktop fixtures and prove whether these bindings execute; do not infer behavior solely from names.

**Dependencies:** supplemental recovered XAML; missing baseline viewmodel members

**Evidence:**

- [GV.CounterSketch.Studio.Controls.WindowModalOptions.xaml:430](<../../../recovery/variants/desktop-xaml/GV.CounterSketch.Studio.Controls.WindowModalOptions.xaml#L430>) — CommandGetAvailableThemes
- [GV.CounterSketch.Studio.View.PartsToolBoxView.xaml:12](<../../../recovery/variants/desktop-xaml/GV.CounterSketch.Studio.View.PartsToolBoxView.xaml#L12>) — CommandSelectTool
- [GV.CounterSketch.Studio.View.PartsToolBoxView.xaml:18](<../../../recovery/variants/desktop-xaml/GV.CounterSketch.Studio.View.PartsToolBoxView.xaml#L18>) — CommandReplaceImage
- [GV.CounterSketch.Studio.View.PartsToolBoxView.xaml:308](<../../../recovery/variants/desktop-xaml/GV.CounterSketch.Studio.View.PartsToolBoxView.xaml#L308>) — CommandPopulateParts
- [GV.CounterSketch.Studio.View.TileViewDesigns.xaml:232](<../../../recovery/variants/desktop-xaml/GV.CounterSketch.Studio.View.TileViewDesigns.xaml#L232>) — CommandSetBackgroundColor
- [GV.CounterSketch.Studio.View.TileViewParts.xaml:247](<../../../recovery/variants/desktop-xaml/GV.CounterSketch.Studio.View.TileViewParts.xaml#L247>) — CommandSetBackgroundColor

**Unresolved:** Static decompilation establishes code paths, not successful runtime execution, visual fidelity, or iPad availability.; Data-dependent feature availability, entitlement, localization, and native geometry result require executable validation.
