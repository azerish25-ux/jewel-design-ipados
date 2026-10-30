using JewelDesign.Domain;
namespace JewelDesign.Documents;

public readonly record struct LegacyProfileSelection(int GroupRecordIndex,int ProfileIndex);

/// <summary>Known empty, numeric or profile channel data. Unknown producers throw rather than becoming empty.</summary>
public sealed class LegacyChannelValue
{
    private readonly double number;
    public bool HasNumber {get;}
    public LegacyProfileSelection? Profile {get;}
    public static LegacyChannelValue Empty {get;}=new();
    private LegacyChannelValue(){}
    private LegacyChannelValue(double value){HasNumber=true;number=value;}
    private LegacyChannelValue(LegacyProfileSelection profile){Profile=profile;}
    internal static LegacyChannelValue Number(double value)=>new(value);
    internal static LegacyChannelValue ProfileData(int record,int profile)=>new(new LegacyProfileSelection(record,profile));
    public bool GetDouble(ref double value){if(!HasNumber)return false;value=number;return true;}
    public bool GetBoolean(ref bool value){if(!HasNumber)return false;value=number>.5;return true;}
}

/// <summary>Known scalar/profile dependency slice with explicit update/read phases, not the complete CS graph scheduler.</summary>
public sealed class LegacyValueGraphSession
{
    private readonly Dictionary<int,ScalarValueRuntime> states=new();
    public LegacyCsg4Document Document {get;}
    public bool IsCamValueMode {get;set;}
    public LegacyValueGraphSession(LegacyCsg4Document document)
    {
        ArgumentNullException.ThrowIfNull(document);Document=document;
        for(int i=0;i<document.Records.Count;i++)if(document.Records[i].Scalar is{} scalar)
            states.Add(i,new(new(scalar.Value,scalar.Value)));
    }
    public ScalarValueRuntime ReadState(int record)=>states.TryGetValue(record,out var state)?state:throw new NotSupportedException("Node has no decoded scalar runtime.");
    public void SetValue(int record,double value)=>states[record]=ReadState(record).SetValue(value);
    public void SetCamState(int record,bool absolute,double absoluteValue,double delta)=>states[record]=ReadState(record) with{AbsoluteCam=absolute,CamAbsolute=absoluteValue,CamDelta=delta};
    public void Generate(params int[] roots)
    {
        ArgumentNullException.ThrowIfNull(roots);
        var order=new List<int>();var marks=new Dictionary<int,byte>();var stack=new Stack<(int Node,bool Exit)>();
        var requested=roots.Length==0?states.Keys.ToArray():roots;
        for(int i=requested.Length-1;i>=0;i--)stack.Push((requested[i],false));
        while(stack.Count>0){
            var item=stack.Pop();
            if(item.Exit){marks[item.Node]=2;order.Add(item.Node);continue;}
            byte mark=marks.GetValueOrDefault(item.Node);
            if(mark==2)continue;
            if(mark==1)throw new InvalidOperationException("Cycle in supported value dependency slice.");
            var record=Record(item.Node);
            if(record.Scalar is null&&record.MatrixProfiles is null)throw new NotSupportedException("Unsupported linked producer at record "+item.Node);
            marks[item.Node]=1;stack.Push((item.Node,true));
            if(record.Scalar is{} scalar){
                int inputs=record.TypeGuid==LegacyCsg4Codec.ChooserTypeGuid?3:2;
                for(int input=Math.Min(inputs,scalar.ObjectIndices.Count)-1;input>=0;input--)
                    if(scalar.ObjectIndices[input]>=0)stack.Push((scalar.ObjectIndices[input],false));
            }
        }
        // Preflight above rejects cycles/unsupported dependencies before applying overrides.
        foreach(int node in order){
            if(!states.ContainsKey(node))continue;
            double value=0;bool available=ReadInput(node,1,new(),0).GetDouble(ref value);
            states[node]=states[node].ApplyOverride(available,value);
        }
    }
    public LegacyChannelValue ReadOutput(int record,int channel)=>ReadOutput(record,channel,new(),0);
    private LegacyCsg4Record Record(int index)=>(uint)index<(uint)Document.Records.Count?Document.Records[index]:throw new NotSupportedException("Channel reference outside document.");
    private LegacyChannelValue ReadInput(int node,int input,HashSet<(int,int)> path,int depth)
    {
        var scalar=Record(node).Scalar??throw new NotSupportedException("Scalar input header unavailable.");
        if(input>=scalar.ObjectIndices.Count||scalar.ObjectIndices[input]<0)return LegacyChannelValue.Empty;
        return ReadOutput(scalar.ObjectIndices[input],scalar.ChannelIndices[input],path,depth+1);
    }
    private LegacyChannelValue ReadOutput(int node,int channel,HashSet<(int,int)> path,int depth)
    {
        if(depth>1024)throw new NotSupportedException("Value dependency depth exceeds candidate limit.");
        if(!path.Add((node,channel)))throw new InvalidOperationException("Cycle during channel read.");
        try{
            var record=Record(node);
            if(record.MatrixProfiles is{} group)
                return channel>=0&&channel<group.Curves.Count?LegacyChannelValue.ProfileData(node,channel):LegacyChannelValue.Empty;
            if(record.Scalar is null||!states.TryGetValue(node,out var state))throw new NotSupportedException("Unsupported output producer at record "+node);
            bool on=true;bool hasBoolean=ReadInput(node,0,path,depth).GetBoolean(ref on);on=!hasBoolean||on;
            if(record.TypeGuid==LegacyCsg4Codec.ChooserTypeGuid&&channel==3){
                if(!on)return LegacyChannelValue.Empty;
                var scalar=record.Scalar;
                if(scalar.ObjectIndices.Count<3||scalar.ObjectIndices[2]<0)return LegacyChannelValue.Empty;
                double value=state.EffectiveValue(IsCamValueMode);
                if(!double.IsFinite(value)||value<int.MinValue||value>int.MaxValue)throw new NotSupportedException("Unsupported chooser integer conversion.");
                return ReadOutput(scalar.ObjectIndices[2],(int)value,path,depth+1);
            }
            var output=state.ReadNumber(on,channel,record.TypeGuid==LegacyCsg4Codec.OnOffTypeGuid,IsCamValueMode);
            return output.HasValue?LegacyChannelValue.Number(output.Value):LegacyChannelValue.Empty;
        }finally{path.Remove((node,channel));}
    }
    public LegacyCsg4Document ExportCurrentDocument()
    {
        var result=Document;
        foreach(var pair in states){
            var state=pair.Value;
            if(state.AbsoluteCam||state.CamAbsolute!=0||state.CamDelta!=0)throw new NotSupportedException("Runtime CAM state is not serialized by legacy1.x scalar schemas.");
            if(BitConverter.DoubleToInt64Bits(state.Numeric.RealValue)!=BitConverter.DoubleToInt64Bits(Document.Records[pair.Key].Scalar!.Value))
                result=result.WithScalarValue(pair.Key,state.Numeric.RealValue);
        }
        return result;
    }
}
