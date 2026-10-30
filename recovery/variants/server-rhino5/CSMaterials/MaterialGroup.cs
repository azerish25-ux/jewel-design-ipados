using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace CSMaterials;

[DataContract]
public sealed class MaterialGroup : ICloneable
{
	private string _name;

	public bool IsConstructionGroup
	{
		get
		{
			if (Classification == MaterialClassification.Finger || Classification == MaterialClassification.Cutting || Classification == MaterialClassification.Creation)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsDefaultMaterialGroup
	{
		get
		{
			if (Classification == MaterialClassification.GemAccent || Classification == MaterialClassification.GemSide || Classification == MaterialClassification.GemCenter || Classification == MaterialClassification.MetalPrimary || Classification == MaterialClassification.MetalSecondary)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsDesignerMaterialGroup
	{
		get
		{
			if (Classification == MaterialClassification.GemDesigner || Classification == MaterialClassification.MetalDesigner)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsEditableMaterialGroup
	{
		get
		{
			if (Classification == MaterialClassification.GemAccent || Classification == MaterialClassification.GemSide || Classification == MaterialClassification.GemCenter || Classification == MaterialClassification.GemDesigner || Classification == MaterialClassification.MetalPrimary || Classification == MaterialClassification.MetalDesigner || Classification == MaterialClassification.MetalSecondary)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsGemMaterialGroup
	{
		get
		{
			if (Classification == MaterialClassification.GemAccent || Classification == MaterialClassification.GemSide || Classification == MaterialClassification.GemCenter || Classification == MaterialClassification.GemDesigner || Classification == MaterialClassification.GemUser)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsMetalMaterialGroup
	{
		get
		{
			if (Classification == MaterialClassification.MetalPrimary || Classification == MaterialClassification.MetalDesigner || Classification == MaterialClassification.MetalSecondary)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsUserMaterialGroup
	{
		get
		{
			if (Classification == MaterialClassification.GemUser || Classification == MaterialClassification.MetalUser)
			{
				return true;
			}
			return false;
		}
	}

	[DataMember]
	public string AlphaOrdinal { get; set; }

	[DataMember]
	public MaterialClassification Classification { get; set; }

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public bool IsGemNumberingOnly { get; set; }

	[DataMember]
	public ObservableCollection<MaterialGroupItem> MaterialGroupItems { get; set; }

	[DataMember]
	public string MaterialName { get; set; }

	[DataMember]
	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = (string.IsNullOrEmpty(value) ? value : value.Trim());
		}
	}

	public MaterialGroup()
	{
		MaterialGroupItems = new ObservableCollection<MaterialGroupItem>();
		Id = Guid.NewGuid();
	}

	public object Clone()
	{
		return new MaterialGroup
		{
			Id = Id,
			Classification = Classification,
			MaterialName = MaterialName,
			Name = Name,
			MaterialGroupItems = new ObservableCollection<MaterialGroupItem>(MaterialGroupItems.Select((MaterialGroupItem item) => item.Clone() as MaterialGroupItem))
		};
	}
}
