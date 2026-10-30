// Generated declared XML property shapes only; no desktop runtime or inherited behavior.
#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace LegacyPortfolioSchema;
// Source SHA256 2d21f760ad48997207c0c7c187dcd3b71578e2a75dce69c93246536ca810626a
public class Project
{
 public string Address1 { get; set; }
 public string Address2 { get; set; }
 public string City { get; set; }
 public string Comments { get; set; }
 public string Country { get; set; }
 public int DefaultRingSizeIndex { get; set; }
 public ObservableCollection<Design> Designs { get; set; }
 public string Email { get; set; }
 public string FirstName { get; set; }
 public string LastName { get; set; }
 public string Name { get; set; }
 public string Path { get; set; }
 public string PhoneNumber { get; set; }
 public ProjectType ProjectType { get; set; }
 public string State { get; set; }
 public string Zip { get; set; }
}
// Source SHA256 7dc2449d7b5452ff88f2c5054526dad3a4fd759909b539c9b1821ebef5a667dc
public class Design
{
 public Guid BaseDesignInstance { get; set; }
 public List<int> ComponentIds { get; set; }
 public int ComponentTypeId { get; set; }
 public string CSUserDesignName { get; set; }
 public string Designer { get; set; }
 public int DesignId { get; set; }
 public int Version { get; set; }
 public Guid DesignInstance { get; set; }
 public string DesignType { get; set; }
 public int DesignTypeId { get; set; }
 public bool IsBaseDesign { get; set; }
 public bool IsFavorite { get; set; }
 public bool IsImageLoaded { get; set; }
 public bool IsSearchFavorite { get; set; }
 public int MaterialGem1 { get; set; }
 public int MaterialGem2 { get; set; }
 public int MaterialGem3 { get; set; }
 public int MaterialMetal1 { get; set; }
 public int MaterialMetal2 { get; set; }
 public string Name { get; set; }
 public string Path { get; set; }
 public long RenderFileCenterOffset { get; set; }
 public int? SelectedIterationId { get; set; }
 public string SeriesNumber { get; set; }
 public string SPINumber { get; set; }
 public int VariantParentDesignId { get; set; }
 public Collection<Design> Variants { get; set; }
}
// Source SHA256 c07f94c437afacdfda0ec43245a5c5ebbb0ec0a7d3434054d6f70547e75467e3
[Serializable]
public enum ProjectType
{
	Portfolio,
	Collection
}
