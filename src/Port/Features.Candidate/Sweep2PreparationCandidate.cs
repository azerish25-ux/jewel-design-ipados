using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;

/// <summary>Owned input geometry for explicit Sweep2. No surface/BRep construction is performed here.</summary>
public sealed class PreparedSweep2Candidate:IDisposable
{
    public CandidateShape Rail1 {get;}
    public CandidateShape Rail2 {get;}
    public IReadOnlyList<CandidateShape> Profiles {get;}
    public IReadOnlyList<double> Parameters1 {get;}
    public IReadOnlyList<double> Parameters2 {get;}
    public IReadOnlyList<int> SourceProfileOrder {get;}
    public IReadOnlyList<CandidateProfileOutputInfo?> ProfileMetadata {get;}
    public bool RequestedClosed {get;}
    public bool EffectiveClosed {get;}
    public bool MaintainHeight {get;}
    private bool disposed;
    internal PreparedSweep2Candidate(CandidateShape first,CandidateShape second,CandidateShape[] profiles,double[] a,double[] b,int[] order,CandidateProfileOutputInfo?[] metadata,bool requested,bool closed,bool height)
    {Rail1=first;Rail2=second;Profiles=Array.AsReadOnly(profiles);Parameters1=Array.AsReadOnly(a);Parameters2=Array.AsReadOnly(b);SourceProfileOrder=Array.AsReadOnly(order);ProfileMetadata=Array.AsReadOnly(metadata);RequestedClosed=requested;EffectiveClosed=closed;MaintainHeight=height;}
    public void Dispose(){if(disposed)return;disposed=true;foreach(var profile in Profiles)profile.Dispose();Rail1.Dispose();Rail2.Dispose();}
}
public static class Sweep2PreparationCandidate
{
    public static readonly Guid TypeGuid=new("f19ea1ff-57e7-4922-9ff7-87535b49c1c6");
    public static IReadOnlyList<int> StableOrder(IReadOnlyList<double> parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);var order=Enumerable.Range(0,parameters.Count).ToArray();
        for(int pass=0;pass<order.Length;pass++)for(int i=0;i<order.Length-1;i++)if(parameters[order[i]]>parameters[order[i+1]])(order[i],order[i+1])=(order[i+1],order[i]);
        return Array.AsReadOnly(order);
    }
    public static PreparedSweep2Candidate Prepare(CandidateContext context,CandidateShape rail1,CandidateShape rail2,IReadOnlyList<CandidateShape> profiles,
        IReadOnlyList<double> parameters1,IReadOnlyList<double> parameters2,bool closed,bool maintainHeight,IReadOnlyList<CandidateProfileOutputInfo?>? metadata=null)
    {
        ArgumentNullException.ThrowIfNull(context);ArgumentNullException.ThrowIfNull(rail1);ArgumentNullException.ThrowIfNull(rail2);ArgumentNullException.ThrowIfNull(profiles);ArgumentNullException.ThrowIfNull(parameters1);ArgumentNullException.ThrowIfNull(parameters2);
        if(profiles.Count==0||profiles.Count!=parameters1.Count||profiles.Count!=parameters2.Count)throw new ArgumentException("Explicit Sweep2 requires one parameter on each rail per profile.");
        if(profiles.Count>LegacyArrayData.MaximumItems)throw new NotSupportedException("Sweep profile count exceeds candidate limit.");
        if(metadata is not null&&metadata.Count!=profiles.Count)throw new ArgumentException("Profile metadata count mismatch.");
        var a=parameters1.ToArray();var b=parameters2.ToArray();
        if(a.Concat(b).Any(x=>!double.IsFinite(x)))throw new NotSupportedException("Nonfinite sweep parameters require original native failure comparison.");
        var owned=new List<CandidateShape>();
        try{
            bool bothClosed=context.IsCurveClosed(rail1)&&context.IsCurveClosed(rail2);
            var first=bothClosed?context.RelocateClosedCurveSeam(rail1,a[0]):context.Clone(rail1);owned.Add(first);
            var second=bothClosed?context.RelocateClosedCurveSeam(rail2,b[0]):context.Clone(rail2);owned.Add(second);
            var domain1=context.InspectCurve(first);var domain2=context.InspectCurve(second);
            if(bothClosed){
                a[0]=domain1.FirstParameter;b[0]=domain2.FirstParameter;
                for(int i=1;i<profiles.Count;i++){
                    a[i]=context.ClosestPoint(first,context.PointAt(rail1,parameters1[i])).Parameter;
                    b[i]=context.ClosestPoint(second,context.PointAt(rail2,parameters2[i])).Parameter;
                }
            }else{
                for(int i=0;i<profiles.Count;i++){
                    if(a[i]<domain1.FirstParameter)a[i]=domain1.FirstParameter;if(a[i]>domain1.LastParameter)a[i]=domain1.LastParameter;
                    if(b[i]<domain2.FirstParameter)b[i]=domain2.FirstParameter;if(b[i]>domain2.LastParameter)b[i]=domain2.LastParameter;
                }
            }
            var order=StableOrder(a).ToArray();var copies=new CandidateShape[profiles.Count];var infos=new CandidateProfileOutputInfo?[profiles.Count];
            var sortedA=new double[profiles.Count];var sortedB=new double[profiles.Count];
            for(int i=0;i<order.Length;i++){
                int source=order[i];context.InspectCurve(profiles[source]);copies[i]=context.Clone(profiles[source]);owned.Add(copies[i]);sortedA[i]=a[source];sortedB[i]=b[source];infos[i]=metadata?[source];
            }
            return new(first,second,copies,sortedA,sortedB,order,infos,closed,bothClosed&&closed,maintainHeight);
        }catch{foreach(var shape in owned)shape.Dispose();throw;}
    }
    public static PreparedSweep2Candidate FromNode(CandidateContext context,CandidateRailGraphSession graph,int recordIndex)
        =>FromNodeCore(context,graph,recordIndex,true);
    internal static PreparedSweep2Candidate FromNodeCore(CandidateContext context,CandidateRailGraphSession graph,int recordIndex,bool generateDependencies)
    {
        ArgumentNullException.ThrowIfNull(context);ArgumentNullException.ThrowIfNull(graph);
        var document=graph.Values.Document;if((uint)recordIndex>=(uint)document.Records.Count)throw new ArgumentOutOfRangeException(nameof(recordIndex));
        var record=document.Records[recordIndex];
        if(record.TypeGuid!=TypeGuid||record.Version!=new LegacyCsg4Version(1,0)||record.SimpleBlock is not{} block)throw new NotSupportedException("SimpleSweep2 exact1.0 header is not decoded.");
        var roots=block.ObjectIndices.Take(8).Where(x=>x>=0).Distinct().ToArray();if(generateDependencies&&roots.Length>0)graph.Generate(roots);
        LegacyChannelValue Read(int input)=>input>=block.ObjectIndices.Count||block.ObjectIndices[input]<0?LegacyChannelValue.Missing:graph.ReadOutput(block.ObjectIndices[input],block.ChannelIndices[input]);
        IReadOnlyList<LegacyChannelValue> Flat(LegacyChannelValue value)=>value.IsMissing?Array.Empty<LegacyChannelValue>():value.Array is null?new[]{value}:LegacyArrayData.Flatten(value).Array!;
        double[] Parameters(LegacyChannelValue value){
            if(value.Array is not{} items)return Array.Empty<double>();var values=new double[items.Count];
            for(int i=0;i<items.Count;i++)if(items[i].IsMissing||!items[i].GetDouble(ref values[i]))throw new NotSupportedException("Sweep parameter array contains missing/non-numeric data.");
            return values;
        }
        var first=Flat(Read(0));var second=Flat(Read(1));var profiles=Flat(Read(2));
        if(first.Count==0||second.Count==0)throw new NotSupportedException("Missing Sweep2 rail.");
        var a=Parameters(Read(3));var b=Parameters(Read(4));
        if(a.Length==0&&b.Length==0)throw new NotSupportedException("No-parameter Sweep2 requires its separate source path.");
        bool closed=false,height=false;Read(5).GetBoolean(ref closed);Read(6).GetBoolean(ref height);
        using var snapshot=graph.CaptureValue(LegacyChannelValue.FromArray(new[]{first[0],second[0],LegacyChannelValue.FromArray(profiles)}));
        var retained=new List<CandidateShape>();
        try{
            var rail1=snapshot.RetainCurve(first[0]);retained.Add(rail1);var rail2=snapshot.RetainCurve(second[0]);retained.Add(rail2);
            var curves=new CandidateShape[profiles.Count];var metadata=new CandidateProfileOutputInfo?[profiles.Count];
            for(int i=0;i<profiles.Count;i++){
                curves[i]=snapshot.RetainCurve(profiles[i]);retained.Add(curves[i]);
                try{metadata[i]=snapshot.ReadProfileMetadata(profiles[i]);}catch(NotSupportedException){metadata[i]=null;}
            }
            return Prepare(context,rail1,rail2,curves,a,b,closed,height,metadata);
        }finally{foreach(var shape in retained)shape.Dispose();}
    }
}
