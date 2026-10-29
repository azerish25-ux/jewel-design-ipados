using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Model.Objects;

[DataContract]
public class QuickEstimateData
{
	[DataMember]
	public List<double> CostMounting { get; set; }

	[DataMember]
	public string CurrentCultureCode { get; set; }

	[DataMember]
	public List<int> GemsFound { get; set; }

	[DataMember]
	public List<double> GemsFoundWeight { get; set; }

	[DataMember]
	public List<int> GemsNotFound { get; set; }

	[DataMember]
	public List<double> GemsNotFoundWeight { get; set; }

	[DataMember]
	public List<double> MetalWeight { get; set; }

	[DataMember]
	public List<double> TotalCost { get; set; }

	public QuickEstimateData()
	{
		CostMounting = new List<double>();
		GemsFound = new List<int>();
		GemsFoundWeight = new List<double>();
		GemsNotFound = new List<int>();
		GemsNotFoundWeight = new List<double>();
		MetalWeight = new List<double>();
		TotalCost = new List<double>();
		CurrentCultureCode = string.Empty;
	}
}
