using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Pricing;

[DataContract]
public sealed class UserMarkup
{
	public decimal MiscMarkupsFlatTotal
	{
		get
		{
			decimal total = default(decimal);
			MiscMarkupsFlat.ForEach(delegate(decimal markup)
			{
				total += markup;
			});
			return total;
		}
	}

	[DataMember]
	public decimal GemMarkupPercentage { get; set; }

	[DataMember]
	public double MarkupDiamondSize1 { get; set; }

	[DataMember]
	public double MarkupDiamondSize2 { get; set; }

	[DataMember]
	public double MarkupDiamondSize3 { get; set; }

	[DataMember]
	public double MarkupDiamondSize4 { get; set; }

	[DataMember]
	public double MarkupDiamondSize5 { get; set; }

	[DataMember]
	public double MarkupGemSize1 { get; set; }

	[DataMember]
	public double MarkupGemSize2 { get; set; }

	[DataMember]
	public double MarkupGemSize3 { get; set; }

	[DataMember]
	public double MarkupGemSize4 { get; set; }

	[DataMember]
	public double MarkupGemSize5 { get; set; }

	[DataMember]
	public List<decimal> MiscMarkupsFlat { get; set; }

	[DataMember]
	public decimal MountingMarkupPercentage { get; set; }

	[DataMember]
	public decimal SettingMarkupFlat { get; set; }

	[DataMember]
	public decimal TaxPercentage { get; set; }
}
