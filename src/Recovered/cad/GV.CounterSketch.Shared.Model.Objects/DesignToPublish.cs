using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Model.Objects;

[DataContract]
public sealed class DesignToPublish
{
	[DataMember]
	public Guid CustomDesignId { get; set; }

	[DataMember]
	public CustomDesignRenderTypeEnum CustomDesignRenderType { get; set; }

	[DataMember]
	public int DesignId { get; set; }

	[DataMember]
	public string Path { get; set; }

	[DataMember]
	public string RenderFolderPath { get; set; }

	[DataMember]
	public List<CustomDesignRenderItem> Renders { get; set; }
}
