using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;

/// <summary>Owns a real stored profile import. Selection here uses stored values, not active graph evaluation.</summary>
public sealed class StoredMatrixProfileCandidate:IDisposable
{
    public CandidateShape Curve {get;}
    public string? Name {get;}
    public IReadOnlyList<double> SweepParameters {get;}
    private StoredMatrixProfileCandidate(CandidateShape curve,string? name,double[] sweep)
    {Curve=curve;Name=name;SweepParameters=Array.AsReadOnly(sweep);}
    public void Dispose()=>Curve.Dispose();
    public static StoredMatrixProfileCandidate Build(CandidateContext context,LegacyCsg4MatrixProfiles group,int index)
    {
        ArgumentNullException.ThrowIfNull(context);ArgumentNullException.ThrowIfNull(group);
        if((uint)index>=(uint)group.Curves.Count)throw new ArgumentOutOfRangeException(nameof(index));
        var curve=group.Curves[index];
        if(curve.Dimension is not (2 or 3))throw new NotSupportedException("Candidate profile import supports2D/3D controls.");
        int width=curve.Dimension+(curve.IsRational?1:0);var points=new CandidatePoint3[curve.ControlVertexCount];
        var weights=curve.IsRational?new double[curve.ControlVertexCount]:null;
        for(int i=0;i<points.Length;i++){
            int start=i*width;double w=weights is null?1:curve.ControlValues[start+curve.Dimension];
            if(weights is not null)weights[i]=w;
            points[i]=new(curve.ControlValues[start]/w,curve.ControlValues[start+1]/w,curve.Dimension==3?curve.ControlValues[start+2]/w:0);
        }
        var knots=curve.Knots;var full=new double[knots.Count+2];double first=knots[0],last=knots[^1];
        // Pinned ON_SuperfluousKnot convention, including non-clamped vectors.
        int order=curve.Order,count=curve.ControlVertexCount;
        if(order>2&&count>=2*order-2&&count>=6){
            if(knots[0]<knots[order-2])first-=knots[count-order+1]-knots[count-order];
            if(knots[count-1]<knots[^1])last+=knots[order+1]-knots[order];
        }
        full[0]=first;full[^1]=last;for(int i=0;i<knots.Count;i++)full[i+1]=knots[i];
        var definition=new CandidateNurbsDefinition(checked((uint)(order-1)),points,full,weights);
        var sweep=group.SweepParameters[index].OrderBy(x=>x).ToArray();
        var native=context.CreateNurbs(definition);
        try{return new(native,index<group.Names.Count?group.Names[index]:null,sweep);}
        catch{native.Dispose();throw;}
    }
    public static StoredMatrixProfileCandidate FromStoredChooser(CandidateContext context,LegacyCsg4Document document,int chooserIndex)
    {
        ArgumentNullException.ThrowIfNull(document);
        if((uint)chooserIndex>=(uint)document.Records.Count)throw new ArgumentOutOfRangeException(nameof(chooserIndex));
        var chooser=document.Records[chooserIndex];
        if(chooser.TypeGuid!=LegacyCsg4Codec.ChooserTypeGuid||chooser.Version!=new LegacyCsg4Version(1,1)||chooser.Scalar is not{} value||value.ObjectIndices.Count<3)
            throw new NotSupportedException("Stored chooser profile is not decoded.");
        int source=value.ObjectIndices[2];
        if((uint)source>=(uint)document.Records.Count||document.Records[source].MatrixProfiles is not{} profiles)
            throw new NotSupportedException("Stored chooser does not reference a decoded matrix-profile group.");
        if(!double.IsFinite(value.Value)||value.Value<int.MinValue||value.Value>int.MaxValue)throw new NotSupportedException("Invalid stored chooser index.");
        return Build(context,profiles,(int)value.Value);
    }
}
