// Analytical auxiliary-spine feasibility experiment, not a RhinoSweep2 replacement.
#include <BRepFill_PipeShell.hxx>
#include <BRepFill_TypeOfContact.hxx>
#include <BRepBuilderAPI_MakeEdge.hxx>
#include <BRepBuilderAPI_MakeWire.hxx>
#include <BRepBuilderAPI_MakePolygon.hxx>
#include <BRepBuilderAPI_MakeVertex.hxx>
#include <BRepCheck_Analyzer.hxx>
#include <BRepExtrema_DistShapeShape.hxx>
#include <BRepGProp.hxx>
#include <Geom_BSplineCurve.hxx>
#include <GeomAPI_ProjectPointOnSurf.hxx>
#include <BRep_Tool.hxx>
#include <TopoDS.hxx>
#include <limits>
#include <GProp_GProps.hxx>
#include <NCollection_Array1.hxx>
#include <NCollection_List.hxx>
#include <TopExp_Explorer.hxx>
#include <TopoDS_Wire.hxx>
#include <TopoDS_Shape.hxx>
#include <gp_Pnt.hxx>
#include <algorithm>
#include <array>
#include <cmath>
#include <sstream>
#include <stdexcept>
#include <string>

namespace {
void require_sweep(bool condition,const char* message){if(!condition)throw std::runtime_error(message);}
TopoDS_Wire rail(double y,double first,double last,double growth=0)
{
    NCollection_Array1<gp_Pnt> poles(1,2);poles(1)=gp_Pnt(0,y,0);poles(2)=gp_Pnt(10,y+growth,0);
    NCollection_Array1<double> knots(1,2);knots(1)=first;knots(2)=last;
    NCollection_Array1<int> multiplicities(1,2);multiplicities(1)=2;multiplicities(2)=2;
    occ::handle<Geom_BSplineCurve> curve=new Geom_BSplineCurve(poles,knots,multiplicities,1,false);
    return BRepBuilderAPI_MakeWire(BRepBuilderAPI_MakeEdge(curve).Edge()).Wire();
}
TopoDS_Wire profile(double x,double height,double width=4)
{
    BRepBuilderAPI_MakePolygon polygon;
    polygon.Add(gp_Pnt(x,0,0));polygon.Add(gp_Pnt(x,width,0));
    polygon.Add(gp_Pnt(x,width,height));polygon.Add(gp_Pnt(x,0,height));polygon.Close();
    require_sweep(polygon.IsDone(),"Sweep experiment profile polygon failed");return polygon.Wire();
}
int count_sweep(const TopoDS_Shape& shape,TopAbs_ShapeEnum kind)
{int count=0;for(TopExp_Explorer e(shape,kind);e.More();e.Next())++count;return count;}
double residual(const TopoDS_Shape& shape,const gp_Pnt& point)
{
    BRepExtrema_DistShapeShape distance(BRepBuilderAPI_MakeVertex(point).Vertex(),shape);
    require_sweep(distance.IsDone(),"Sweep experiment point-distance query failed");return distance.Value();
}
struct SweepObservation{int profiles;int faces;double area;double maximumResidual;double untrimmedProjectionAtWorst;gp_Pnt worstPoint;};
SweepObservation build(const std::array<double,3>& heights,bool curvilinear,bool shifted=true,double widthGrowth=0)
{
    auto first=rail(0,shifted?2:0,shifted?12:10),second=rail(4,shifted?40:0,shifted?60:10,widthGrowth);
    BRepFill_PipeShell pipe(first);pipe.Set(second,curvilinear,BRepFill_NoContact);
    pipe.SetTolerance(1e-7,1e-7,1e-7);
    const std::array<double,3> stations={0,3,10};
    std::array<TopoDS_Wire,3> sections;
    for(int i=0;i<3;i++){
        sections[i]=profile(stations[i],heights[i],4+widthGrowth*stations[i]/10);
        pipe.Add(sections[i],BRepBuilderAPI_MakeVertex(gp_Pnt(stations[i],0,0)).Vertex(),false,false);
    }
    NCollection_List<TopoDS_Shape> retained;pipe.Profiles(retained);
    require_sweep(retained.Size()==3,"NoContact lost a section before construction");
    require_sweep(pipe.IsReady()&&pipe.Build(),"Auxiliary-spine native surface construction failed");
    auto shape=pipe.Shape();require_sweep(!shape.IsNull()&&BRepCheck_Analyzer(shape).IsValid(),"Auxiliary-spine native BRep invalid");
    require_sweep(count_sweep(shape,TopAbs_SHELL)==1,"Sweep experiment expected one connected shell");
    require_sweep(count_sweep(shape,TopAbs_SOLID)==0,"Experiment silently capped the source shell");
    double worst=0;gp_Pnt worstPoint;
    auto observe=[&](const gp_Pnt& point){double distance=residual(shape,point);if(distance>worst){worst=distance;worstPoint=point;}};
    // Independent geometric samples: both rails and every side of all three supplied sections.
    for(int j=0;j<=20;j++){double x=j*.5;observe(gp_Pnt(x,0,0));observe(gp_Pnt(x,4+widthGrowth*x/10,0));}
    for(int i=0;i<3;i++)for(int j=0;j<=10;j++){
        double fraction=j/10.0,x=stations[i],h=heights[i],width=4+widthGrowth*x/10;
        for(const auto& point:std::array<gp_Pnt,4>{gp_Pnt(x,width*fraction,0),gp_Pnt(x,width,h*fraction),gp_Pnt(x,width*(1-fraction),h),gp_Pnt(x,0,h*(1-fraction))})observe(point);
    }
    require_sweep(std::isfinite(worst),"Nonfinite sweep experiment residual");
    // The native input wires remain independently valid after construction.
    require_sweep(BRepCheck_Analyzer(first).IsValid()&&BRepCheck_Analyzer(second).IsValid(),"Sweep experiment modified input rails");
    for(const auto& section:sections)require_sweep(BRepCheck_Analyzer(section).IsValid(),"Sweep experiment modified input section");
    double projectionDistance=std::numeric_limits<double>::infinity();
    for(TopExp_Explorer e(shape,TopAbs_FACE);e.More();e.Next()){
        GeomAPI_ProjectPointOnSurf projection(worstPoint,BRep_Tool::Surface(TopoDS::Face(e.Current())));
        if(projection.NbPoints()>0)projectionDistance=std::min(projectionDistance,projection.LowerDistance());
    }
    require_sweep(std::isfinite(projectionDistance),"Independent sweep surface projection unavailable");
    GProp_GProps properties;BRepGProp::SurfaceProperties(shape,properties);
    return {static_cast<int>(retained.Size()),count_sweep(shape,TopAbs_FACE),properties.Mass(),worst,projectionDistance,worstPoint};
}
int border_profile_count()
{
    auto first=rail(0,2,12),second=rail(4,40,60);
    BRepFill_PipeShell pipe(first);pipe.Set(second,true,BRepFill_ContactOnBorder);
    const std::array<double,3> stations={0,3,10};
    TopoDS_Wire last;
    for(double x:stations){last=profile(x,x==3?2:1);pipe.Add(last,BRepBuilderAPI_MakeVertex(gp_Pnt(x,0,0)).Vertex(),false,false);}
    NCollection_List<TopoDS_Shape> retained;pipe.Profiles(retained);
    require_sweep(retained.Size()==1&&retained.First().IsSame(last),"Pinned ContactOnBorder section replacement behavior changed; re-review sweep mapping");
    return static_cast<int>(retained.Size());
}
}
std::string run_candidate_sweep_contracts()
{
    // Tangent-plane correspondence preserves all three sections in these analytical cases.
    auto constant=build({1,1,1},false);
    require_sweep(constant.faces==4&&std::abs(constant.area-100)<=1e-6&&constant.maximumResidual<=1e-6,"Tangent-plane constant sweep misses analytic area/topology/contact");
    auto varying=build({1,2,1},false);
    require_sweep(varying.maximumResidual<=1e-6,"Tangent-plane varying sweep misses a supplied section or rail");
    auto equalDomains=build({1,1,1},false,false);
    require_sweep(equalDomains.maximumResidual<=1e-6&&std::abs(equalDomains.area-100)<=1e-6,"Tangent-plane equal-domain sweep mismatch");
    auto variableWidth=build({1,1,1},false,true,2);
    require_sweep(variableWidth.maximumResidual<=1e-6&&std::abs(variableWidth.area-(110+std::sqrt(104.0)))<=1e-6,"Variable-width straight-rail sweep misses analytic contact/area");
    // Measure the alternative mode without pretending its out-of-tolerance geometry passes.
    auto arcLength=build({1,1,1},true);
    int border=border_profile_count();
    std::ostringstream result;result.precision(17);
    result<<"native-sweep-feasibility=observed; tangent-plane-three-section-shell=pass"
          <<"; three-section-shell-area="<<constant.area
          <<"; constant-section-max-residual="<<constant.maximumResidual
          <<"; varying-section-max-residual="<<varying.maximumResidual
          <<"; variable-width-max-residual="<<variableWidth.maximumResidual
          <<"; variable-width-area="<<variableWidth.area
          <<"; arc-length-guide-max-residual="<<arcLength.maximumResidual
          <<"; arc-length-guide-area="<<arcLength.area
          <<"; arc-length-untrimmed-projection="<<arcLength.untrimmedProjectionAtWorst
          <<"; arc-length-worst-sample="<<arcLength.worstPoint.X()<<","<<arcLength.worstPoint.Y()<<","<<arcLength.worstPoint.Z()
          <<"; arc-length-guide-contact="<<(arcLength.maximumResidual<=1e-6?"analytic-pass":"rejected")
          <<"; contact-on-border-retained-profiles="<<border<<"/3"
          <<"; arbitrary-two-rail-correspondence=unverified; ";
    return result.str();
}
