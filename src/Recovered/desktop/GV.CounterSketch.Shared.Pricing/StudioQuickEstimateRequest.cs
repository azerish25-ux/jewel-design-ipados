using System.Collections.Generic;
using System.Runtime.Serialization;
using GV.CounterSketch.Shared.Pricing.Enums;

namespace GV.CounterSketch.Shared.Pricing;

[DataContract]
public class StudioQuickEstimateRequest
{
	[DataMember]
	public string AccountNumber { get; set; }

	[DataMember]
	public string Company { get; set; }

	[DataMember]
	public List<int> ComponentIds { get; set; }

	[DataMember]
	public string CultureInfo { get; set; }

	[DataMember]
	public StudioCurrency Currency { get; set; }

	[DataMember]
	public int DesignId { get; set; }

	[DataMember]
	public byte[] File { get; set; }

	[DataMember]
	public List<StudioGem> Gems { get; set; }

	[DataMember]
	public int? GemsNotPriced { get; set; }

	[DataMember]
	public decimal? GemsNotPricedWeight { get; set; }

	[DataMember]
	public int? GemsPriced { get; set; }

	[DataMember]
	public decimal? GemsPricedWeight { get; set; }

	[DataMember]
	public bool IncludeMountingOnly { get; set; }

	[DataMember]
	public bool? IsDebugFile { get; set; }

	[DataMember]
	public bool IsPatch { get; set; }

	[DataMember]
	public bool? IsRhino5File { get; set; }

	[DataMember]
	public int KeyId { get; set; }

	[DataMember]
	public string LibraryVersion { get; set; }

	[DataMember]
	public UserMarkup Markup { get; set; }

	[DataMember]
	public double? MetalWeightDisplayFactor { get; set; }

	[DataMember]
	public List<StudioMetal> Metels { get; set; }

	[DataMember]
	public string SPINumber { get; set; }

	[DataMember]
	public string StullerToken { get; set; }

	[DataMember]
	public bool Swap9K10K { get; set; }

	[DataMember]
	public double? Volume { get; set; }
}
