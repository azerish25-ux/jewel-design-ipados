using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Model.Objects;

[DataContract]
public class QuickEstimateRequestData
{
	[DataMember]
	public string Company { get; set; }

	[DataMember]
	public string CurrencyTypeOption { get; set; }

	[DataMember]
	public string DesignId { get; set; }

	[DataMember]
	public string FileName { get; set; }

	[DataMember]
	public int KeyId { get; set; }

	[DataMember]
	public double MarkupGems { get; set; }

	[DataMember]
	public double MarkupMounting { get; set; }

	[DataMember]
	public double MarkupServices { get; set; }

	[DataMember]
	public List<string> SelectedGem { get; set; }

	[DataMember]
	public List<string> SelectedMetals { get; set; }

	[DataMember]
	public string ShipToAccountNumber { get; set; }

	[DataMember]
	public string StullerIdNumber { get; set; }

	[DataMember]
	public string StullerToken { get; set; }

	[DataMember]
	public double TaxPercentage { get; set; }

	[DataMember]
	public double UserDefinedMarkup1 { get; set; }

	[DataMember]
	public double UserDefinedMarkup2 { get; set; }

	[DataMember]
	public double UserDefinedMarkup3 { get; set; }
}
