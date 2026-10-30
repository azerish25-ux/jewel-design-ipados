using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;

internal static class StoredProfileAssertions
{
    private static void Check(bool condition,string message){if(!condition)throw new Exception(message);}
    private static void Near(double actual,double expected)=>Check(double.IsFinite(actual)&&Math.Abs(actual-expected)<1e-6,"Stored profile mismatch: "+actual.ToString("R")+" expected "+expected.ToString("R"));
    private static void Point(CandidatePoint3 actual,CandidatePoint3 expected){Near(actual.X,expected.X);Near(actual.Y,expected.Y);Near(actual.Z,expected.Z);}
    // Independent evaluator: direct homogeneous recursion; omitted outer knots do not affect the valid domain.
    private static CandidatePoint3 Evaluate(LegacyNurbsCurveData curve,double parameter)
    {
        int p=curve.Order-1,n=curve.ControlVertexCount-1,width=curve.Dimension+(curve.IsRational?1:0);
        var u=new double[curve.Knots.Count+2];u[0]=curve.Knots[0];u[^1]=curve.Knots[^1];
        for(int i=0;i<curve.Knots.Count;i++)u[i+1]=curve.Knots[i];
        int span=p;
        if(parameter>=u[n+1])span=n;
        else while(span<n&&parameter>=u[span+1])span++;
        var d=new double[p+1][];
        for(int j=0;j<=p;j++){
            int index=(span-p+j)*width;
            d[j]=new[]{curve.ControlValues[index],curve.ControlValues[index+1],curve.ControlValues[index+2],curve.IsRational?curve.ControlValues[index+3]:1};
        }
        for(int r=1;r<=p;r++)for(int j=p;j>=r;j--){
            int k=span-p+j;double denominator=u[k+p-r+1]-u[k],alpha=denominator==0?0:(parameter-u[k])/denominator;
            for(int c=0;c<4;c++)d[j][c]=(1-alpha)*d[j-1][c]+alpha*d[j][c];
        }
        return new(d[p][0]/d[p][3],d[p][1]/d[p][3],d[p][2]/d[p][3]);
    }
    public static string Run()
    {
        using var stream=typeof(StoredProfileAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();stream.CopyTo(memory);var bytes=memory.ToArray();var document=LegacyCsg4Codec.Read(bytes);
        using var context=CandidateContext.Create();int count=0;
        foreach(var record in document.Records){
            if(record.MatrixProfiles is not{} group)continue;
            for(int index=0;index<group.Curves.Count;index++){
                var source=group.Curves[index];using var native=StoredMatrixProfileCandidate.Build(context,group,index);
                var info=context.InspectCurve(native.Curve);Near(info.FirstParameter,source.DomainStart);Near(info.LastParameter,source.DomainEnd);
                Check(info.Degree==source.Order-1&&info.PoleCount==source.ControlVertexCount,"Stored degree/control count changed");
                Check(info.IsRational==source.IsRational,"Stored rational representation lost");
                for(int sample=0;sample<=16;sample++){
                    double parameter=source.DomainStart+(source.DomainEnd-source.DomainStart)*sample/16.0;
                    Point(context.PointAt(native.Curve,parameter),Evaluate(source,parameter));
                }
                Check(native.Name==(index<group.Names.Count?group.Names[index]:null),"Stored name label changed");
                Check(native.SweepParameters.SequenceEqual(group.SweepParameters[index].OrderBy(x=>x)),"Profile sweep semantics");
                count++;
            }
        }
        Check(count==155&&context.DiagnosticLiveShapeCount==0,"Stored profile coverage/lifetime");
        using(var selected=StoredMatrixProfileCandidate.FromStoredChooser(context,document,6)){
            Point(context.PointAt(selected.Curve,0),Evaluate(document.Records[7].MatrixProfiles!.Curves[0],0));
            var settings=new OutsideRailCandidateSettings(1e-7,1e-7,1e-7);
            var parameters=new OutsideRailV1Parameters(document.Records[8].Scalar!.Value,document.Records[9].Scalar!.Value,document.Records[10].Scalar!.Value,document.Records[11].Scalar!.Value>.5);
            double radius=document.Records[2].Scalar!.Value;
            using var result=OutsideRailV1Candidate.Build(context,selected.Curve,radius,settings,parameters);
            Check(result.Diagnostics.Count==0&&context.IsCurveClosed(result.Curve),"Actual selected profile outside rail failed");
            var info=context.InspectCurve(result.Curve);var start=context.PointAt(result.Curve,info.FirstParameter);
            Near(start.X,0);Near(start.Y,0);Near(start.Z,radius+parameters.TopThickness);
            Check(context.TangentAt(result.Curve,info.FirstParameter).X>0,"Actual profile final orientation");
        }

        var editor=new LegacyCsg4EditSession(document);
        var settingsForReal=new OutsideRailCandidateSettings(1e-7,1e-7,1e-7);
        double realRadius=document.Records[2].Scalar!.Value;
        var realParameters=new OutsideRailV1Parameters(document.Records[8].Scalar!.Value,document.Records[9].Scalar!.Value,document.Records[10].Scalar!.Value,true);
        for(int choice=0;choice<5;choice++){
            editor.BeginEditTransaction();editor.SetValue(editor.GetParameter(6),choice);editor.EndEditTransaction();
            var reopened=LegacyCsg4Codec.Read(editor.ExportArchiveData());
            using var selected=StoredMatrixProfileCandidate.FromStoredChooser(context,reopened,6);
            Point(context.PointAt(selected.Curve,0),Evaluate(document.Records[7].MatrixProfiles!.Curves[choice],0));
            using var outside=OutsideRailV1Candidate.Build(context,selected.Curve,realRadius,settingsForReal,realParameters);
            Check(outside.Diagnostics.Count==0&&context.IsCurveClosed(outside.Curve),"Real profile choice used fallback: "+choice);
            var info=context.InspectCurve(outside.Curve);var start=context.PointAt(outside.Curve,info.FirstParameter);
            Near(start.Z,realRadius+realParameters.TopThickness);Near(start.X,0);
        }
        while(editor.CanUndo)Check(editor.Undo(),"Stored chooser undo failed");
        Check(editor.ExportArchiveData().AsSpan().SequenceEqual(bytes),"Stored chooser undo changed original archive");

        var mutablePoles=new[]{new CandidatePoint3(1,0,0),new CandidatePoint3(1,1,0),new CandidatePoint3(0,1,0)};
        var mutableKnots=new[]{0.0,0,0,1,1,1};var mutableWeights=new[]{1.0,1/Math.Sqrt(2),1};
        var definition=new CandidateNurbsDefinition(2,mutablePoles,mutableKnots,mutableWeights);
        mutablePoles[0]=new(double.NaN,0,0);mutableKnots[0]=double.NaN;mutableWeights[0]=0;
        using(var isolated=context.CreateNurbs(definition)){
            Point(context.PointAt(isolated,0),new(1,0,0));Point(context.PointAt(isolated,.5),new(1/Math.Sqrt(2),1/Math.Sqrt(2),0));
        }
        try{using var invalid=context.CreateNurbs(new(2,new[]{new CandidatePoint3(0,0,0)},new[]{0.0,1}));throw new Exception("Malformed NURBS accepted");}
        catch(CandidateGeometryException error){Check(error.Status==CandidateNativeStatus.InvalidArgument,"Malformed factory status");}
        Check(context.DiagnosticLiveShapeCount==0,"Selected profile pipeline retained entries");
        return "PASS stored native profiles:155 exact NURBS imports against independent homogeneous points, degrees/weights/domains including non-clamped knots, stored chooser edit/save/reopen/rebuild/undo and all5 actual-profile OutsideRail choices; no active graph/PC claim.";
    }
}
