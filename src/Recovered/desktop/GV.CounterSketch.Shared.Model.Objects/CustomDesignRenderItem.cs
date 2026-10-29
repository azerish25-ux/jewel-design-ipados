using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Model.Objects;

[DataContract]
public class CustomDesignRenderItem
{
	[DataMember]
	public CustomDesignRenderTypeEnum CustomDesignRenderType { get; set; }

	[DataMember]
	public int DesignId { get; set; }

	[DataMember]
	public int Gem1 { get; set; }

	[DataMember]
	public int Gem2 { get; set; }

	[DataMember]
	public int Metal1 { get; set; }

	[DataMember]
	public int Metal2 { get; set; }

	[DataMember]
	public string Path { get; set; }

	public CustomDesignRenderItem(int designId, int metal1, int metal2, int gem1, int gem2, string path, CustomDesignRenderTypeEnum customDesignRenderType)
	{
		DesignId = designId;
		Metal1 = metal1;
		Metal2 = metal2;
		Gem1 = gem1;
		Gem2 = gem2;
		Path = path;
		CustomDesignRenderType = customDesignRenderType;
	}

	public CustomDesignRenderItem()
	{
	}
}
