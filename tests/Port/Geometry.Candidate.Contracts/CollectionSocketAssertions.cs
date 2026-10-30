using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Graph;
using Oracle=CandidateGeometryContractChecks.Recovered.Consumer;
namespace CandidateGeometryContractChecks;
internal static class CollectionSocketAssertions
{
    private static void Check(bool v,string message){if(!v)throw new Exception(message);}
    private static void Unsupported(Action action){try{action();}catch(NotSupportedException){return;}throw new Exception("Expected explicit unsupported native collection path");}
    public static string Run()
    {
        var choices=new[]{new SerializedChannelLink(-1,-5),new(1,0),new(2,-1),new(99,3)};int count=0;
        for(int length=0;length<=5;length++)for(int pattern=0;pattern<(1<<(2*length));pattern++){
            var links=new SerializedChannelLink[length];int n=pattern;for(int i=0;i<length;i++){links[i]=choices[n&3];n>>=2;}
            foreach(int records in new[]{0,3,10})foreach(int offset in new[]{0,2,int.MaxValue}){
                var actual=LegacyTransformInputLayout.Reconstruct(links,records,offset,0);
                var expected=Oracle.CSBlockExpandableInput.Observe(links,records,offset,0);
                Check(actual.AllChannels.SequenceEqual(expected)&&actual.OperationChannels.SequenceEqual(expected.Take(expected.Length-1)),"Source collection constructor replay mismatch");count++;
            }
        }
        Check(count==12285,"Missing collection constructor cases");
        Unsupported(()=>TransformProducerCandidate.AddScalars(LegacyChannelValue.FromArray(new[]{LegacyChannelValue.FromNumber(1)}),LegacyChannelValue.Empty));
        using var stream=typeof(CollectionSocketAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();stream.CopyTo(memory);var document=LegacyCsg4Codec.Read(memory.ToArray());
        using var context=CandidateContext.Create();using var graph=new CandidateRailGraphSession(context,document,new(1e-7,1e-7,1e-7));
        Unsupported(()=>graph.Generate(61));Unsupported(()=>graph.Generate(116));
        Check(context.DiagnosticLiveShapeCount==0,"Unimplemented collection path fabricated or leaked geometry");
        return "PASS collection socket constructors:"+count+" exact-source zero-fixed-input cases; array data stays explicit until native ownership integration, no native execution claim.";
    }
}
