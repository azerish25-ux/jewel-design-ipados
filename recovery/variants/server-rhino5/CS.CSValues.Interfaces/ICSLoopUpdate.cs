using CS.Parametric.Channels;

namespace CS.CSValues.Interfaces;

public interface ICSLoopUpdate
{
	CSInputChannel InDisplayUpdate { get; set; }

	void LoopUpdate();
}
