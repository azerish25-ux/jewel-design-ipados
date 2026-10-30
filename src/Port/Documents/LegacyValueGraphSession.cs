using JewelDesign.Domain;
namespace JewelDesign.Documents;

public readonly record struct LegacyProfileSelection(int GroupRecordIndex,int ProfileIndex);

/// <summary>Known missing, empty, numeric, profile, curve/BRep-reference or affine channel data. Unknown producers throw rather than becoming empty.</summary>
public sealed class LegacyChannelValue
{
    private readonly double number;
    public bool HasNumber {get;}
    public LegacyProfileSelection? Profile {get;}
    public LegacyCurveSelection? Curve {get;}
    public LegacyBrepSelection? Brep {get;}
    public LegacyAffineTransform? Transform {get;}
    public IReadOnlyList<LegacyChannelValue>? Array {get;}
    public bool IsMissing {get;}
    public static LegacyChannelValue Missing {get;}=new(true);
    public static LegacyChannelValue Empty {get;}=new();
    private LegacyChannelValue(bool missing){IsMissing=missing;}
    private LegacyChannelValue(){}
    private LegacyChannelValue(double value){HasNumber=true;number=value;}
    private LegacyChannelValue(LegacyProfileSelection profile){Profile=profile;}
    private LegacyChannelValue(LegacyCurveSelection curve){Curve=curve;}
    private LegacyChannelValue(LegacyBrepSelection brep){Brep=brep;}
    private LegacyChannelValue(LegacyAffineTransform transform){Transform=transform;}
    private LegacyChannelValue(LegacyChannelValue[] items){Array=System.Array.AsReadOnly(items);}
    public static LegacyChannelValue FromArray(IEnumerable<LegacyChannelValue?> items)
    {
        ArgumentNullException.ThrowIfNull(items);var copy=new List<LegacyChannelValue>();
        foreach(var item in items){if(copy.Count>=LegacyArrayData.MaximumItems)throw new NotSupportedException("Array data exceeds explicit candidate item limit.");copy.Add(item??Missing);}
        return new(copy.ToArray());
    }
    public static LegacyChannelValue FromTransform(LegacyAffineTransform transform)=>new(transform);
    public static LegacyChannelValue FromCurve(int record,int channel,int item=0)=>new(new LegacyCurveSelection(record,channel,item));
    public static LegacyChannelValue FromBrep(int record,int channel,int item=0)=>new(new LegacyBrepSelection(record,channel,item));
    public static LegacyChannelValue FromNumber(double value)=>Number(value);
    internal static LegacyChannelValue Number(double value)=>new(value);
    internal static LegacyChannelValue ProfileData(int record,int profile)=>new(new LegacyProfileSelection(record,profile));
    public bool GetDouble(ref double value){if(!HasNumber)return false;value=number;return true;}
    public bool GetBoolean(ref bool value){if(!HasNumber)return false;value=number>.5;return true;}
}

/// <summary>Known scalar/profile dependency slice with explicit update/read phases, not the complete CS graph scheduler.</summary>
public sealed class LegacyValueGraphSession
{
    private readonly Dictionary<int,ScalarValueRuntime> states=new();
    private readonly ILegacyValueNodeProvider? provider;
    public LegacyCsg4Document Document {get;}
    public bool IsCamValueMode {get;set;}
    public LegacyValueGraphSession(LegacyCsg4Document document,ILegacyValueNodeProvider? provider=null)
    {
        ArgumentNullException.ThrowIfNull(document);Document=document;this.provider=provider;
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
            if(record.Scalar is null&&record.MatrixProfiles is null&&!(provider?.Supports(item.Node)??false))throw new NotSupportedException("Unsupported linked producer at record "+item.Node);
            marks[item.Node]=1;stack.Push((item.Node,true));
            if(provider?.Supports(item.Node)??false){
                var links=provider.Inputs(item.Node);for(int i=links.Count-1;i>=0;i--)if(links[i].ObjectIndex>=0)stack.Push((links[i].ObjectIndex,false));
            }
            if(record.Scalar is{} scalar){
                int inputs=record.TypeGuid==LegacyCsg4Codec.ChooserTypeGuid?3:2;
                for(int input=Math.Min(inputs,scalar.ObjectIndices.Count)-1;input>=0;input--)
                    if(scalar.ObjectIndices[input]>=0)stack.Push((scalar.ObjectIndices[input],false));
            }
        }
        // Preflight above rejects cycles/unsupported dependencies before applying overrides.
        foreach(int node in order){
            if(!states.ContainsKey(node)){if(provider?.Supports(node)??false)provider.Update(node);continue;}
            double value=0;bool available=ReadInput(node,1,new(),0).GetDouble(ref value);
            states[node]=states[node].ApplyOverride(available,value);
        }
    }
    public LegacyChannelValue ReadOutput(int record,int channel)=>ReadOutput(record,channel,new(),0);
    private LegacyCsg4Record Record(int index)=>(uint)index<(uint)Document.Records.Count?Document.Records[index]:throw new NotSupportedException("Channel reference outside document.");
    private LegacyChannelValue ReadInput(int node,int input,HashSet<(int,int)> path,int depth)
    {
        var scalar=Record(node).Scalar??throw new NotSupportedException("Scalar input header unavailable.");
        if(input>=scalar.ObjectIndices.Count||scalar.ObjectIndices[input]<0)return LegacyChannelValue.Missing;
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
            if(provider?.Supports(node)??false)return provider.ReadOutput(node,channel);
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
