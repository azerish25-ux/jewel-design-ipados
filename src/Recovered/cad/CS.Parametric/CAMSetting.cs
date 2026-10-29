using System.Collections.Generic;

namespace CS.Parametric;

public sealed class CAMSetting
{
	public DesignInfo DesignInfo { get; set; } = new DesignInfo();

	public List<CAMValue> Values { get; set; } = new List<CAMValue>();
}
