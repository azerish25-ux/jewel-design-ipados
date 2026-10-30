#include <BRepPrimAPI_MakeBox.hxx>
#include <BRepPrimAPI_MakeCylinder.hxx>
#include <BRepAlgoAPI_Cut.hxx>
#include <BRepAlgoAPI_Common.hxx>
#include <BRepAlgoAPI_Fuse.hxx>
#include <BRepCheck_Analyzer.hxx>
#include <BRepGProp.hxx>
#include <BRepMesh_IncrementalMesh.hxx>
#include <BRep_Tool.hxx>
#include <TopoDS.hxx>
#include <TopExp_Explorer.hxx>
#include <NCollection_List.hxx>
#include <Poly_Triangulation.hxx>
#include <Geom_Circle.hxx>
#include <Geom_BSplineCurve.hxx>
#include <GeomConvert.hxx>
#include <GeomAdaptor_Curve.hxx>
#include <GCPnts_AbscissaPoint.hxx>
#include <GProp_GProps.hxx>
#include <gp_Ax2.hxx>
#include <gp_Pnt.hxx>
#include <gp_Dir.hxx>
#include <Standard_Failure.hxx>
#include <cmath>
#include <cstdio>
#include <cstring>
#include <stdexcept>
#include <sstream>

void run_candidate_abi_contracts();
void run_candidate_curve_contracts();
void run_rotation_reference_contracts();
std::string run_candidate_sweep_contracts();
std::string run_candidate_paired_sweep_contracts();

namespace {
void require(bool condition,const char* message) { if(!condition)throw std::runtime_error(message); }
void near(double actual,double expected,double tolerance,const char* message) { require(std::isfinite(actual)&&std::abs(actual-expected)<=tolerance,message); }
int count(const TopoDS_Shape& shape,TopAbs_ShapeEnum kind) { int n=0;for(TopExp_Explorer e(shape,kind);e.More();e.Next())++n;return n; }
double volume(const TopoDS_Shape& shape) { GProp_GProps properties;BRepGProp::VolumeProperties(shape,properties,true,false,false);return properties.Mass(); }
template<class Operation> TopoDS_Shape boolean(const TopoDS_Shape& lhs,const TopoDS_Shape& rhs)
{
    NCollection_List<TopoDS_Shape> arguments,tools;arguments.Append(lhs);tools.Append(rhs);
    Operation operation;operation.SetArguments(arguments);operation.SetTools(tools);operation.SetFuzzyValue(.01);operation.Build();
    require(operation.IsDone()&&!operation.HasErrors(),"OCCT boolean failed");
    require(!operation.Shape().IsNull(),"OCCT boolean returned null shape");return operation.Shape();
}
std::string run()
{
    run_candidate_abi_contracts();
    run_candidate_curve_contracts();
    run_rotation_reference_contracts();
    const std::string sweepExperiment=run_candidate_sweep_contracts()+run_candidate_paired_sweep_contracts();
    const double pi=std::acos(-1.0);
    auto outer=BRepPrimAPI_MakeCylinder(10.0,3.0).Shape();
    auto inner=BRepPrimAPI_MakeCylinder(gp_Ax2(gp_Pnt(0,0,-1),gp_Dir(0,0,1)),8.0,5.0).Shape();
    auto ring=boolean<BRepAlgoAPI_Cut>(outer,inner);
    require(BRepCheck_Analyzer(ring).IsValid(),"Annulus BRep invalid");
    require(count(ring,TopAbs_SOLID)==1,"Annulus solid count");
    near(volume(ring),108.0*pi,1e-6,"Annulus analytic volume");

    auto first=BRepPrimAPI_MakeBox(2.0,2.0,2.0).Shape();
    auto overlapping=BRepPrimAPI_MakeBox(gp_Pnt(1,0,0),2.0,2.0,2.0).Shape();
    auto common=boolean<BRepAlgoAPI_Common>(first,overlapping);
    auto fused=boolean<BRepAlgoAPI_Fuse>(first,overlapping);
    require(BRepCheck_Analyzer(common).IsValid()&&BRepCheck_Analyzer(fused).IsValid(),"Box boolean BRep invalid");
    near(volume(common),4.0,1e-7,"Box intersection analytic volume");
    near(volume(fused),12.0,1e-7,"Box union analytic volume");
    auto disjoint=BRepPrimAPI_MakeBox(gp_Pnt(10,0,0),1.0,1.0,1.0).Shape();
    auto empty=boolean<BRepAlgoAPI_Common>(first,disjoint);
    require(count(empty,TopAbs_SOLID)==0,"Disjoint intersection has solids");near(volume(empty),0.0,1e-9,"Disjoint intersection has volume");

    occ::handle<Geom_Circle> circle=new Geom_Circle(gp_Ax2(gp_Pnt(0,0,0),gp_Dir(0,0,1)),10.0);
    auto nurbs=GeomConvert::CurveToBSplineCurve(circle);
    require(!nurbs.IsNull()&&nurbs->IsRational(),"Circle not converted to rational NURBS");
    near(nurbs->Value(nurbs->FirstParameter()).Distance(nurbs->Value(nurbs->LastParameter())),0.0,1e-8,"NURBS seam mismatch");
    GeomAdaptor_Curve adaptor(nurbs);near(GCPnts_AbscissaPoint::Length(adaptor),20.0*pi,1e-6,"NURBS circle arc length");

    BRepMesh_IncrementalMesh mesher(ring,.05,false,.35,false);require(mesher.IsDone(),"Native mesher failed");
    int triangles=0;
    for(TopExp_Explorer e(ring,TopAbs_FACE);e.More();e.Next())
    {
        TopLoc_Location location;auto mesh=BRep_Tool::Triangulation(TopoDS::Face(e.Current()),location);
        require(!mesh.IsNull()&&mesh->NbTriangles()>0,"BRep face has no native triangulation");triangles+=mesh->NbTriangles();
    }
    int invalidFailures=0;for(int i=0;i<16;i++)
    {
        auto temporary=BRepPrimAPI_MakeBox(1.0,1.0,1.0).Shape();near(volume(temporary),1.0,1e-9,"Repeated native lifetime volume");
        try { BRepPrimAPI_MakeCylinder invalid(-1.0,2.0);invalid.Shape(); } catch(const Standard_Failure&) { ++invalidFailures; }
    }
    require(invalidFailures==16,"Invalid native cylinder did not fail");
    std::ostringstream report;report<<"PASS OCCT 8.0.1 native candidate: annulus-volume="<<volume(ring)<<"; solids="<<count(ring,TopAbs_SOLID)<<"; native-triangles="<<triangles<<"; box-common=4; box-union=12; disjoint-empty; rational-NURBS-length="<<GCPnts_AbscissaPoint::Length(adaptor)<<"; repeated-lifetimes=16; native-invalid-inputs=16; candidate-ABI-ownership=pass; native-handle-cycles=64; candidate-four-span-curves=pass; native-arc-length-samples=50; native-affine-copy=pass; native-closest-point=pass; native-trim-reverse=pass; native-ordered-join=pass; native-unit-tangents=pass; native-curve-line-events=pass; native-closed-seam=pass; native-curve-bounds=pass; native-uniform-affine-and-closure=pass; native-nurbs-construction=pass; published-rotation-reference=pass; ";report<<sweepExperiment;report<<"analytic fixtures only, no CounterSketch/PC parity claim.";return report.str();
}
}
extern "C" int jd_occt_candidate_run(char* output,unsigned int capacity)
{
    std::string result;int status=0;
    try {result=run();}
    catch(const Standard_Failure& error) {result=std::string("FAIL OCCT native exception: ")+(error.GetMessageString()?error.GetMessageString():"unknown");status=1;}
    catch(const std::exception& error) {result=std::string("FAIL OCCT candidate assertion: ")+error.what();status=2;}
    if(!output||capacity==0)return 3;
    if(result.size()+1>capacity){output[0]=0;return 4;}
    std::memcpy(output,result.c_str(),result.size()+1);return status;
}
