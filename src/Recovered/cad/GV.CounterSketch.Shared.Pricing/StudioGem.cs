using System.Runtime.Serialization;
using GV.CounterSketch.Shared.Pricing.Enums;

namespace GV.CounterSketch.Shared.Pricing;

[DataContract]
public sealed class StudioGem
{
	[DataMember]
	public StudioGemColorQuality? GemColorQualityOption { get; set; }

	[DataMember(IsRequired = false)]
	public int GemIndex { get; set; }

	[DataMember(IsRequired = false)]
	public string GemMountingType { get; set; }

	[DataMember(IsRequired = false)]
	public GemMaterial GemMaterialOption { get; set; }

	[DataMember(IsRequired = false)]
	public string GemNumber { get; set; }

	[DataMember(IsRequired = true)]
	public StudioGemShape GemShapeOption { get; set; }

	[DataMember(IsRequired = true)]
	public StudioGemType GemTypeOption { get; set; }

	[DataMember(IsRequired = false)]
	public decimal? Height { get; set; }

	[DataMember(IsRequired = false)]
	public bool IsCenterStone { get; set; }

	[DataMember(IsRequired = false)]
	public bool IsSideStone { get; set; }

	[DataMember(IsRequired = false)]
	public bool IsLargeStone { get; set; }

	[DataMember(IsRequired = false)]
	public bool IsMeleeStone { get; set; }

	[DataMember]
	public string ItemNumber { get; set; }

	[DataMember(IsRequired = true)]
	public decimal Length { get; set; }

	[DataMember(IsRequired = true)]
	public int Quantity { get; set; }

	[DataMember(IsRequired = true)]
	public StudioSetType SetTypeOption { get; set; }

	[DataMember(IsRequired = false)]
	public decimal? Weight { get; set; }

	[DataMember(IsRequired = true)]
	public decimal Width { get; set; }

	[DataMember]
	public string Image { get; set; }

	public override bool Equals(object obj)
	{
		if (obj.GetType() != typeof(StudioGem))
		{
			return false;
		}
		return Equals((StudioGem)obj);
	}

	public bool Equals(StudioGem other)
	{
		if (other.GemColorQualityOption.Equals(GemColorQualityOption) && object.Equals(other.GemShapeOption, GemShapeOption) && object.Equals(other.GemTypeOption, GemTypeOption) && object.Equals(other.ItemNumber, ItemNumber) && other.Length == Length && object.Equals(other.SetTypeOption, SetTypeOption) && other.Width == Width)
		{
			return other.Weight == Weight;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ((((((((((((((GemColorQualityOption.HasValue ? GemColorQualityOption.Value.GetHashCode() : 0) * 397) ^ GemShapeOption.GetHashCode()) * 397) ^ GemTypeOption.GetHashCode()) * 397) ^ ((ItemNumber != null) ? ItemNumber.GetHashCode() : 0)) * 397) ^ Length.GetHashCode()) * 397) ^ SetTypeOption.GetHashCode()) * 397) ^ Width.GetHashCode()) * 397) ^ Weight.GetHashCode();
	}
}
