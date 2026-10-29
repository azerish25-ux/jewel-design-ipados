using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Pricing;

[DataContract]
public class StudioQuickEstimateResult
{
	[DataMember]
	public IEnumerable<StudioGem> GemsNotFound { get; set; }

	[DataMember]
	public int GemsNotPriced { get; set; }

	[DataMember]
	public decimal GemsNotPricedWeight { get; set; }

	[DataMember]
	public int GemsPriced { get; set; }

	[DataMember]
	public decimal GemsPricedWeight { get; set; }

	[DataMember]
	public DataTable PricingTable { get; set; }

	[DataMember]
	public bool IncompleteQuote { get; set; }
}
