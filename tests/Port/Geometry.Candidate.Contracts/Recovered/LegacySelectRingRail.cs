#nullable disable
// Exact source Update with test-only channel/data observation stubs.
using JewelDesign.Documents;
namespace CandidateGeometryContractChecks.Recovered.SelectRails;
internal sealed class CSData{internal LegacyChannelValue Value;internal CSData(LegacyChannelValue value){Value=value;}}
internal sealed class Input{internal CSData Data;internal Input(LegacyChannelValue value){Data=value.IsMissing?null:new(value);}internal bool GetBoolean(ref bool value)=>Data?.Value.GetBoolean(ref value)??false;}
internal sealed class Output{internal CSData Data;internal void SetData(CSData data){Data=data;}}
internal class CSBlock{public virtual bool Update()=>true;}
internal sealed class CSSelectRingRail:CSBlock
{
private readonly Input iRingRail1,iRingRail2,iUseRingRail2;
internal readonly Output oRingRail=new();
internal CSSelectRingRail(LegacyChannelValue first,LegacyChannelValue second,LegacyChannelValue control){iRingRail1=new(first);iRingRail2=new(second);iUseRingRail2=new(control);}
public override bool Update()
	{
		bool value = false;
		iUseRingRail2.GetBoolean(ref value);
		CSData data = iRingRail1.Data;
		CSData data2 = iRingRail2.Data;
		if (data == null && data2 == null)
		{
			oRingRail.SetData(null);
		}
		else if (data == null)
		{
			oRingRail.SetData(data2);
		}
		else if (data2 == null)
		{
			oRingRail.SetData(data);
		}
		else
		{
			oRingRail.SetData(value ? data2 : data);
		}
		return base.Update();
	}
}
