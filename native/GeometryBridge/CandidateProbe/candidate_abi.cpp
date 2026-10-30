#include "candidate_abi.h"
#include <BRepPrimAPI_MakeBox.hxx>
#include <BRepBuilderAPI_MakeEdge.hxx>
#include <BRepAdaptor_Curve.hxx>
#include <BndLib_Add3dCurve.hxx>
#include <Bnd_Box.hxx>
#include <Geom_Circle.hxx>
#include <Geom_Line.hxx>
#include <IntTools_EdgeEdge.hxx>
#include <cstddef>
#include <ElCLib.hxx>
#include <vector>
#include <GeomAPI_ProjectPointOnCurve.hxx>
#include <BRep_Tool.hxx>
#include <Geom_TrimmedCurve.hxx>
#include <Geom_BSplineCurve.hxx>
#include <GeomConvert.hxx>
#include <GeomConvert_CompCurveToBSplineCurve.hxx>
#include <Convert_ParameterisationType.hxx>
#include <GeomAbs_CurveType.hxx>
#include <GCPnts_AbscissaPoint.hxx>
#include <TopoDS.hxx>
#include <gp_Ax2.hxx>
#include <gp_Dir.hxx>
#include <gp_Pnt.hxx>
#include <BRepBuilderAPI_Copy.hxx>
#include <BRepBuilderAPI_Transform.hxx>
#include <BRepBuilderAPI_GTransform.hxx>
#include <gp_GTrsf.hxx>
#include <BRepAlgoAPI_Common.hxx>
#include <BRepAlgoAPI_Fuse.hxx>
#include <BRepAlgoAPI_Cut.hxx>
#include <BRepCheck_Analyzer.hxx>
#include <BRepGProp.hxx>
#include <GProp_GProps.hxx>
#include <NCollection_List.hxx>
#include <Standard_Failure.hxx>
#include <TopExp_Explorer.hxx>
#include <gp_Trsf.hxx>
#include <gp_Vec.hxx>
#include <algorithm>
#include <atomic>
#include <cmath>
#include <cstring>
#include <exception>
#include <limits>
#include <memory>
#include <mutex>
#include <new>
#include <unordered_map>

namespace {
struct Failure { int32_t code;const char* message; };
[[noreturn]] void fail(int32_t code,const char* message) { throw Failure{code,message}; }
struct ShapeRecord { TopoDS_Shape shape;uint32_t references=1; };
struct Context {
    std::mutex mutex;
    bool closed=false;
    uint32_t references=1;
    std::unordered_map<uint64_t,ShapeRecord> shapes;
};
std::mutex registryMutex;
std::unordered_map<uint64_t,std::shared_ptr<Context>> contexts;
std::atomic<uint64_t> nextIdentity{1};

uint64_t identity()
{
    auto current=nextIdentity.load(std::memory_order_relaxed);
    while(current!=std::numeric_limits<uint64_t>::max()) {
        if(nextIdentity.compare_exchange_weak(current,current+1,std::memory_order_relaxed))return current;
    }
    fail(JD_CANDIDATE_LIFETIME_LIMIT,"Candidate identity space exhausted");
}
int32_t report(int32_t status,const char* message,char* diagnostic,uint32_t capacity) noexcept
{
    if(diagnostic&&capacity) {
        const auto length=std::min(std::strlen(message),static_cast<size_t>(capacity-1));
        std::memcpy(diagnostic,message,length);diagnostic[length]=0;
    }
    return status;
}
template<class Body> int32_t protect(Body&& body,char* diagnostic,uint32_t capacity) noexcept
{
    try {body();return report(JD_CANDIDATE_OK,"",diagnostic,capacity);}
    catch(const Failure& error) {return report(error.code,error.message,diagnostic,capacity);}
    catch(const Standard_Failure& error) {return report(JD_CANDIDATE_KERNEL_FAILURE,error.GetMessageString()?error.GetMessageString():"Native kernel failure",diagnostic,capacity);}
    catch(const std::bad_alloc&) {return report(JD_CANDIDATE_OUT_OF_MEMORY,"Native allocation failed",diagnostic,capacity);}
    catch(const std::exception& error) {return report(JD_CANDIDATE_INTERNAL_ERROR,error.what(),diagnostic,capacity);}
    catch(...) {return report(JD_CANDIDATE_INTERNAL_ERROR,"Unknown native exception",diagnostic,capacity);}
}
std::shared_ptr<Context> findContext(uint64_t id)
{
    std::lock_guard<std::mutex> lock(registryMutex);
    const auto found=contexts.find(id);
    if(found==contexts.end())fail(JD_CANDIDATE_INVALID_CONTEXT,"Unknown or released context");
    return found->second;
}
template<class Body> void withContext(uint64_t id,Body&& body)
{
    auto context=findContext(id);
    std::lock_guard<std::mutex> lock(context->mutex);
    if(context->closed)fail(JD_CANDIDATE_INVALID_CONTEXT,"Released context");
    body(*context);
}
ShapeRecord& findShape(Context& context,uint64_t id)
{
    const auto found=context.shapes.find(id);
    if(found==context.shapes.end())fail(JD_CANDIDATE_INVALID_SHAPE,"Unknown, foreign or released shape");
    return found->second;
}
void outputHandle(uint64_t* output)
{
    if(!output)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Missing output handle");
    *output=0;
}
uint64_t store(Context& context,const TopoDS_Shape& shape)
{
    if(shape.IsNull()||!BRepCheck_Analyzer(shape).IsValid())fail(JD_CANDIDATE_KERNEL_FAILURE,"Kernel produced null or invalid BRep");
    const auto id=identity();
    context.shapes.emplace(id,ShapeRecord{shape,1});return id;
}
template<class Operation> TopoDS_Shape booleanShape(const TopoDS_Shape& lhs,const TopoDS_Shape& rhs,double fuzzy)
{
    NCollection_List<TopoDS_Shape> arguments,tools;arguments.Append(lhs);tools.Append(rhs);
    Operation operation;operation.SetArguments(arguments);operation.SetTools(tools);
    operation.SetNonDestructive(true);operation.SetRunParallel(false);operation.SetFuzzyValue(fuzzy);operation.Build();
    if(!operation.IsDone()||operation.HasErrors())fail(JD_CANDIDATE_KERNEL_FAILURE,"Native boolean failed");
    return operation.Shape();
}
}

extern "C" uint32_t jd_candidate_abi_version(void) {return 1;}
extern "C" int32_t jd_candidate_context_create(uint32_t version,uint64_t* output,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        outputHandle(output);
        if(version!=jd_candidate_abi_version())fail(JD_CANDIDATE_UNSUPPORTED_VERSION,"Unsupported candidate ABI version");
        auto context=std::make_shared<Context>();const auto id=identity();
        std::lock_guard<std::mutex> lock(registryMutex);contexts.emplace(id,std::move(context));*output=id;
    },diagnostic,capacity);
}
extern "C" int32_t jd_candidate_context_retain(uint64_t context,char* diagnostic,uint32_t capacity)
{
    return protect([&]{withContext(context,[&](Context& value){
        if(value.references==std::numeric_limits<uint32_t>::max())fail(JD_CANDIDATE_LIFETIME_LIMIT,"Context reference limit");
        ++value.references;
    });},diagnostic,capacity);
}
extern "C" int32_t jd_candidate_context_release(uint64_t context,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        auto value=findContext(context);
        std::lock_guard<std::mutex> contextLock(value->mutex);
        if(value->closed)fail(JD_CANDIDATE_INVALID_CONTEXT,"Released context");
        if(--value->references==0) {
            value->closed=true;value->shapes.clear();
            std::lock_guard<std::mutex> registryLock(registryMutex);contexts.erase(context);
        }
    },diagnostic,capacity);
}
extern "C" int32_t jd_candidate_context_live_shapes(uint64_t context,uint64_t* count,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        if(!count)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Missing count output");*count=0;
        withContext(context,[&](Context& value){*count=static_cast<uint64_t>(value.shapes.size());});
    },diagnostic,capacity);
}
extern "C" int32_t jd_candidate_box(uint64_t context,double x,double y,double z,uint64_t* output,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        outputHandle(output);
        if(!std::isfinite(x)||!std::isfinite(y)||!std::isfinite(z)||x<=0||y<=0||z<=0)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Box dimensions must be finite and positive");
        withContext(context,[&](Context& value){*output=store(value,BRepPrimAPI_MakeBox(x,y,z).Shape());});
    },diagnostic,capacity);
}
extern "C" int32_t jd_candidate_translate(uint64_t context,uint64_t shape,double x,double y,double z,uint64_t* output,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        outputHandle(output);
        if(!std::isfinite(x)||!std::isfinite(y)||!std::isfinite(z))fail(JD_CANDIDATE_INVALID_ARGUMENT,"Translation must be finite");
        withContext(context,[&](Context& value){
            const auto& input=findShape(value,shape).shape;gp_Trsf transform;transform.SetTranslation(gp_Vec(x,y,z));
            BRepBuilderAPI_Transform operation(input,transform,true,false);
            if(!operation.IsDone())fail(JD_CANDIDATE_KERNEL_FAILURE,"Native translation failed");
            *output=store(value,operation.Shape());
        });
    },diagnostic,capacity);
}
extern "C" int32_t jd_candidate_shape_clone(uint64_t context,uint64_t shape,uint64_t* output,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        outputHandle(output);
        withContext(context,[&](Context& value){
            BRepBuilderAPI_Copy operation(findShape(value,shape).shape,true,false);
            if(!operation.IsDone())fail(JD_CANDIDATE_KERNEL_FAILURE,"Native shape copy failed");
            *output=store(value,operation.Shape());
        });
    },diagnostic,capacity);
}
extern "C" int32_t jd_candidate_boolean(uint64_t context,int32_t operation,uint64_t lhs,uint64_t rhs,double fuzzy,uint64_t* output,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        outputHandle(output);
        if(operation!=JD_CANDIDATE_COMMON&&operation!=JD_CANDIDATE_FUSE&&operation!=JD_CANDIDATE_CUT)fail(JD_CANDIDATE_UNSUPPORTED_OPERATION,"Unsupported boolean operation");
        if(!std::isfinite(fuzzy)||fuzzy<0)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Fuzzy value must be finite and nonnegative");
        withContext(context,[&](Context& value){
            const auto& left=findShape(value,lhs).shape;const auto& right=findShape(value,rhs).shape;TopoDS_Shape result;
            switch(operation) {
                case JD_CANDIDATE_COMMON:result=booleanShape<BRepAlgoAPI_Common>(left,right,fuzzy);break;
                case JD_CANDIDATE_FUSE:result=booleanShape<BRepAlgoAPI_Fuse>(left,right,fuzzy);break;
                case JD_CANDIDATE_CUT:result=booleanShape<BRepAlgoAPI_Cut>(left,right,fuzzy);break;
            }
            *output=store(value,result);
        });
    },diagnostic,capacity);
}
extern "C" int32_t jd_candidate_shape_retain(uint64_t context,uint64_t shape,char* diagnostic,uint32_t capacity)
{
    return protect([&]{withContext(context,[&](Context& value){
        auto& record=findShape(value,shape);
        if(record.references==std::numeric_limits<uint32_t>::max())fail(JD_CANDIDATE_LIFETIME_LIMIT,"Shape reference limit");
        ++record.references;
    });},diagnostic,capacity);
}
extern "C" int32_t jd_candidate_shape_release(uint64_t context,uint64_t shape,char* diagnostic,uint32_t capacity)
{
    return protect([&]{withContext(context,[&](Context& value){
        auto& record=findShape(value,shape);if(--record.references==0)value.shapes.erase(shape);
    });},diagnostic,capacity);
}
extern "C" int32_t jd_candidate_shape_inspect(uint64_t context,uint64_t shape,double* volume,uint32_t* solids,uint32_t* valid,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        if(!volume||!solids||!valid)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Missing shape inspection output");
        *volume=0;*solids=0;*valid=0;
        withContext(context,[&](Context& value){
            const auto& input=findShape(value,shape).shape;GProp_GProps properties;
            BRepGProp::VolumeProperties(input,properties,true,false,false);const auto mass=properties.Mass();
            if(!std::isfinite(mass))fail(JD_CANDIDATE_KERNEL_FAILURE,"Nonfinite native mass");
            uint32_t count=0;for(TopExp_Explorer e(input,TopAbs_SOLID);e.More();e.Next()) {
                if(count==std::numeric_limits<uint32_t>::max())fail(JD_CANDIDATE_LIFETIME_LIMIT,"Solid count limit");++count;
            }
            *volume=mass;*solids=count;*valid=BRepCheck_Analyzer(input).IsValid()?1u:0u;
        });
    },diagnostic,capacity);
}

extern "C" int32_t jd_candidate_circle(uint64_t context,double radius,double ox,double oy,double oz,double nx,double ny,double nz,double xx,double xy,double xz,uint64_t* output,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        outputHandle(output);
        for(double value:{radius,ox,oy,oz,nx,ny,nz,xx,xy,xz})if(!std::isfinite(value))fail(JD_CANDIDATE_INVALID_ARGUMENT,"Circle data must be finite");
        if(radius<=0)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Circle radius must be positive");
        withContext(context,[&](Context& value){
            gp_Ax2 frame(gp_Pnt(ox,oy,oz),gp_Dir(nx,ny,nz),gp_Dir(xx,xy,xz));
            occ::handle<Geom_Circle> circle=new Geom_Circle(frame,radius);
            // The untrimmed GeomConvert circle path forces SetPeriodic afterward.
            // Its trimmed path preserves the converter's clamped four-span form.
            occ::handle<Geom_TrimmedCurve> fullPeriod=new Geom_TrimmedCurve(circle,0,2*std::acos(-1.0));
            auto curve=GeomConvert::CurveToBSplineCurve(fullPeriod,Convert_TgtThetaOver2_4);
            if(curve.IsNull()||curve->Degree()!=2||curve->NbPoles()!=9||!curve->IsRational()||curve->IsPeriodic())fail(JD_CANDIDATE_KERNEL_FAILURE,"Unexpected four-span circle representation");
            BRepBuilderAPI_MakeEdge operation(curve);
            if(!operation.IsDone())fail(JD_CANDIDATE_KERNEL_FAILURE,"Native curve edge creation failed");
            *output=store(value,operation.Shape());
        });
    },diagnostic,capacity);
}
extern "C" int32_t jd_candidate_curve_inspect(uint64_t context,uint64_t shape,double* first,double* last,double* length,uint32_t* degree,uint32_t* poles,uint32_t* rational,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        if(!first||!last||!length||!degree||!poles||!rational)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Missing curve inspection output");
        *first=0;*last=0;*length=0;*degree=0;*poles=0;*rational=0;
        withContext(context,[&](Context& value){
            const auto& input=findShape(value,shape).shape;
            if(input.ShapeType()!=TopAbs_EDGE)fail(JD_CANDIDATE_UNSUPPORTED_OPERATION,"Curve inspection requires an edge");
            BRepAdaptor_Curve curve(TopoDS::Edge(input));
            if(curve.GetType()!=GeomAbs_BSplineCurve)fail(JD_CANDIDATE_UNSUPPORTED_OPERATION,"Candidate inspection requires a B-spline curve");
            const auto start=curve.FirstParameter(),end=curve.LastParameter();
            // Explicit candidate inspection precision; default quadrature was insufficient on a quarter arc.
            const auto arcLength=GCPnts_AbscissaPoint::Length(curve,start,end,1e-10);
            if(!std::isfinite(start)||!std::isfinite(end)||!std::isfinite(arcLength)||end<=start||arcLength<=0)fail(JD_CANDIDATE_KERNEL_FAILURE,"Invalid native curve information");
            *first=start;*last=end;*length=arcLength;*degree=static_cast<uint32_t>(curve.Degree());*poles=static_cast<uint32_t>(curve.NbPoles());*rational=curve.IsRational()?1u:0u;
        });
    },diagnostic,capacity);
}
extern "C" int32_t jd_candidate_curve_point(uint64_t context,uint64_t shape,double parameter,double* x,double* y,double* z,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        if(!x||!y||!z)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Missing curve point output");*x=0;*y=0;*z=0;
        if(!std::isfinite(parameter))fail(JD_CANDIDATE_INVALID_ARGUMENT,"Curve parameter must be finite");
        withContext(context,[&](Context& value){
            const auto& input=findShape(value,shape).shape;
            if(input.ShapeType()!=TopAbs_EDGE)fail(JD_CANDIDATE_UNSUPPORTED_OPERATION,"Curve evaluation requires an edge");
            BRepAdaptor_Curve curve(TopoDS::Edge(input));const auto point=curve.Value(parameter);
            if(!std::isfinite(point.X())||!std::isfinite(point.Y())||!std::isfinite(point.Z()))fail(JD_CANDIDATE_KERNEL_FAILURE,"Nonfinite native curve point");
            *x=point.X();*y=point.Y();*z=point.Z();
        });
    },diagnostic,capacity);
}

extern "C" int32_t jd_candidate_curve_length_parameter(uint64_t context,uint64_t shape,double fraction,double tolerance,double* parameter,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        if(!parameter)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Missing curve parameter output");*parameter=0;
        if(!std::isfinite(fraction)||fraction<0||fraction>1||!std::isfinite(tolerance)||tolerance<=0)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Length fraction/tolerance outside supported bounds");
        withContext(context,[&](Context& value){
            const auto& input=findShape(value,shape).shape;
            if(input.ShapeType()!=TopAbs_EDGE)fail(JD_CANDIDATE_UNSUPPORTED_OPERATION,"Length mapping requires an edge");
            BRepAdaptor_Curve curve(TopoDS::Edge(input));const double first=curve.FirstParameter(),last=curve.LastParameter();
            if(!std::isfinite(first)||!std::isfinite(last)||last<=first)fail(JD_CANDIDATE_KERNEL_FAILURE,"Invalid curve domain");
            if(fraction==0){*parameter=first;return;}if(fraction==1){*parameter=last;return;}
            const double length=GCPnts_AbscissaPoint::Length(curve,tolerance);
            if(!std::isfinite(length)||length<=0)fail(JD_CANDIDATE_KERNEL_FAILURE,"Invalid integrated curve length");
            GCPnts_AbscissaPoint solver(tolerance,curve,length*fraction,first);
            if(!solver.IsDone())fail(JD_CANDIDATE_KERNEL_FAILURE,"Curve length parameter did not converge");
            const double result=solver.Parameter();
            if(!std::isfinite(result)||result<first||result>last)fail(JD_CANDIDATE_KERNEL_FAILURE,"Length solver returned invalid parameter");
            *parameter=result;
        });
    },diagnostic,capacity);
}

extern "C" int32_t jd_candidate_affine(uint64_t context,uint64_t shape,const double* matrix,uint32_t count,uint64_t* output,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        outputHandle(output);
        if(!matrix||count!=12)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Expected12 affine coefficients");
        gp_GTrsf transform;
        for(uint32_t i=0;i<12;i++){
            if(!std::isfinite(matrix[i]))fail(JD_CANDIDATE_INVALID_ARGUMENT,"Affine coefficients must be finite");
            transform.SetValue(static_cast<int>(i/4)+1,static_cast<int>(i%4)+1,matrix[i]);
        }
        if(!std::isfinite(transform.VectorialPart().Determinant())||transform.IsSingular())fail(JD_CANDIDATE_INVALID_ARGUMENT,"Unsupported singular or overflowing affine matrix");
        transform.SetForm();
        withContext(context,[&](Context& value){
            BRepBuilderAPI_GTransform operation(findShape(value,shape).shape,transform,true);
            if(!operation.IsDone())fail(JD_CANDIDATE_KERNEL_FAILURE,"Native affine transformation failed");
            *output=store(value,operation.Shape());
        });
    },diagnostic,capacity);
}

extern "C" int32_t jd_candidate_curve_closest(uint64_t context,uint64_t shape,double px,double py,double pz,double* parameter,double* x,double* y,double* z,double* distance,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        if(!parameter||!x||!y||!z||!distance)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Missing closest point output");
        *parameter=0;*x=0;*y=0;*z=0;*distance=0;
        if(!std::isfinite(px)||!std::isfinite(py)||!std::isfinite(pz))fail(JD_CANDIDATE_INVALID_ARGUMENT,"Projection point must be finite");
        withContext(context,[&](Context& value){
            const auto& input=findShape(value,shape).shape;
            if(input.ShapeType()!=TopAbs_EDGE)fail(JD_CANDIDATE_UNSUPPORTED_OPERATION,"Closest point requires an edge");
            double first=0,last=0;auto curve=BRep_Tool::Curve(TopoDS::Edge(input),first,last);
            if(curve.IsNull()||!std::isfinite(first)||!std::isfinite(last)||last<=first)fail(JD_CANDIDATE_KERNEL_FAILURE,"Invalid located curve/domain");
            const gp_Pnt query(px,py,pz);GeomAPI_ProjectPointOnCurve projection(query,curve,first,last);
            if(!projection.Extrema().IsDone())fail(JD_CANDIDATE_KERNEL_FAILURE,"Native curve projection did not converge");
            double bestParameter=first;gp_Pnt best=curve->Value(first);double bestDistance=query.Distance(best);
            auto consider=[&](double candidateParameter,const gp_Pnt& point){
                const double candidateDistance=query.Distance(point);
                if(!std::isfinite(candidateParameter)||!std::isfinite(point.X())||!std::isfinite(point.Y())||!std::isfinite(point.Z())||!std::isfinite(candidateDistance))fail(JD_CANDIDATE_KERNEL_FAILURE,"Invalid native projection candidate");
                if(candidateDistance<bestDistance){bestParameter=candidateParameter;best=point;bestDistance=candidateDistance;}
            };
            consider(last,curve->Value(last));
            for(int i=1;i<=projection.NbPoints();i++)consider(projection.Parameter(i),projection.Point(i));
            if(!std::isfinite(bestDistance)||bestParameter<first||bestParameter>last)fail(JD_CANDIDATE_KERNEL_FAILURE,"Invalid closest point result");
            *parameter=bestParameter;*x=best.X();*y=best.Y();*z=best.Z();*distance=bestDistance;
        });
    },diagnostic,capacity);
}

namespace {
occ::handle<Geom_BSplineCurve> copiedEdgeSpline(Context& value,uint64_t shape,double& first,double& last)
{
    const auto& input=findShape(value,shape).shape;
    if(input.ShapeType()!=TopAbs_EDGE)fail(JD_CANDIDATE_UNSUPPORTED_OPERATION,"Curve operation requires an edge");
    auto curve=BRep_Tool::Curve(TopoDS::Edge(input),first,last);
    // Affine modification wraps its exact spline in Geom_TrimmedCurve.
    // Keep the edge interval while copying the actual basis geometry.
    for(;;){
        auto trimmed=occ::down_cast<Geom_TrimmedCurve>(curve);
        if(trimmed.IsNull())break;
        curve=trimmed->BasisCurve();
    }
    auto spline=occ::down_cast<Geom_BSplineCurve>(curve);
    if(spline.IsNull())fail(JD_CANDIDATE_UNSUPPORTED_OPERATION,"Candidate operation requires a NURBS curve");
    if(!std::isfinite(first)||!std::isfinite(last)||last<=first)fail(JD_CANDIDATE_KERNEL_FAILURE,"Invalid edge curve domain");
    auto copy=occ::down_cast<Geom_BSplineCurve>(spline->Copy());
    if(copy.IsNull())fail(JD_CANDIDATE_KERNEL_FAILURE,"Native curve copy failed");return copy;
}
uint64_t storeCurveEdge(Context& value,const occ::handle<Geom_BSplineCurve>& curve)
{
    BRepBuilderAPI_MakeEdge operation(curve);
    if(!operation.IsDone())fail(JD_CANDIDATE_KERNEL_FAILURE,"Native trimmed/reversed edge failed");
    return store(value,operation.Shape());
}
}
extern "C" int32_t jd_candidate_curve_trim(uint64_t context,uint64_t shape,double first,double last,uint64_t* output,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        outputHandle(output);
        if(!std::isfinite(first)||!std::isfinite(last)||last<=first)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Trim interval must be finite and increasing");
        withContext(context,[&](Context& value){
            double domainFirst=0,domainLast=0;auto curve=copiedEdgeSpline(value,shape,domainFirst,domainLast);
            if(first<domainFirst||last>domainLast)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Trim interval outside edge domain");
            curve->Segment(first,last);*output=storeCurveEdge(value,curve);
        });
    },diagnostic,capacity);
}
extern "C" int32_t jd_candidate_curve_reverse(uint64_t context,uint64_t shape,uint64_t* output,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        outputHandle(output);
        withContext(context,[&](Context& value){
            double first=0,last=0;auto curve=copiedEdgeSpline(value,shape,first,last);
            curve->Segment(first,last);curve->Reverse();*output=storeCurveEdge(value,curve);
        });
    },diagnostic,capacity);
}

extern "C" int32_t jd_candidate_curve_join(uint64_t context,uint64_t first,uint64_t second,double tolerance,uint64_t* output,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        outputHandle(output);
        if(!std::isfinite(tolerance)||tolerance<=0)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Join tolerance must be finite and positive");
        withContext(context,[&](Context& value){
            double a0=0,a1=0,b0=0,b1=0;auto a=copiedEdgeSpline(value,first,a0,a1);auto b=copiedEdgeSpline(value,second,b0,b1);
            a->Segment(a0,a1);b->Segment(b0,b1);
            const double gap=a->Value(a1).Distance(b->Value(b0));
            if(!std::isfinite(gap)||gap>tolerance)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Ordered curve endpoints are disconnected");
            GeomConvert_CompCurveToBSplineCurve operation(a);
            if(!operation.Add(b,tolerance,true,true,0))fail(JD_CANDIDATE_KERNEL_FAILURE,"Native curve join failed");
            auto joined=operation.BSplineCurve();if(joined.IsNull())fail(JD_CANDIDATE_KERNEL_FAILURE,"Native curve join returned null");
            *output=storeCurveEdge(value,joined);
        });
    },diagnostic,capacity);
}

extern "C" int32_t jd_candidate_curve_tangent(uint64_t context,uint64_t shape,double parameter,double* x,double* y,double* z,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        if(!x||!y||!z)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Missing tangent output");
        *x=0;*y=0;*z=0;
        if(!std::isfinite(parameter))fail(JD_CANDIDATE_INVALID_ARGUMENT,"Tangent parameter must be finite");
        withContext(context,[&](Context& value){
            const auto& input=findShape(value,shape).shape;
            if(input.ShapeType()!=TopAbs_EDGE)fail(JD_CANDIDATE_UNSUPPORTED_OPERATION,"Tangent evaluation requires an edge");
            BRepAdaptor_Curve curve(TopoDS::Edge(input));
            const double first=curve.FirstParameter(),last=curve.LastParameter();
            if(!std::isfinite(first)||!std::isfinite(last)||last<=first)fail(JD_CANDIDATE_KERNEL_FAILURE,"Invalid tangent curve domain");
            if(parameter<first||parameter>last)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Tangent parameter outside edge domain");
            const auto evaluated=curve.EvalD1(parameter);
            const auto& derivative=evaluated.D1;
            if(!std::isfinite(derivative.X())||!std::isfinite(derivative.Y())||!std::isfinite(derivative.Z()))
                fail(JD_CANDIDATE_KERNEL_FAILURE,"Nonfinite native derivative");
            const double magnitude=std::hypot(derivative.X(),derivative.Y(),derivative.Z());
            if(!std::isfinite(magnitude)||magnitude<=0)fail(JD_CANDIDATE_KERNEL_FAILURE,"Native tangent is undefined");
            *x=derivative.X()/magnitude;*y=derivative.Y()/magnitude;*z=derivative.Z()/magnitude;
        });
    },diagnostic,capacity);
}

namespace {
double checkedSegmentLength(double sx,double sy,double sz,double ex,double ey,double ez)
{
    for(double coordinate:{sx,sy,sz,ex,ey,ez})if(!std::isfinite(coordinate))
        fail(JD_CANDIDATE_INVALID_ARGUMENT,"Segment coordinates must be finite");
    const double length=std::hypot(ex-sx,ey-sy,ez-sz);
    if(!std::isfinite(length)||length<=0)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Segment must have finite positive length");
    return length;
}
jd_candidate_point3 eventPoint(const gp_Pnt& point)
{
    if(!std::isfinite(point.X())||!std::isfinite(point.Y())||!std::isfinite(point.Z()))
        fail(JD_CANDIDATE_KERNEL_FAILURE,"Nonfinite native intersection point");
    return {point.X(),point.Y(),point.Z()};
}
}
extern "C" int32_t jd_candidate_line(uint64_t context,double sx,double sy,double sz,double ex,double ey,double ez,uint64_t* output,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        outputHandle(output);const double length=checkedSegmentLength(sx,sy,sz,ex,ey,ez);
        withContext(context,[&](Context& value){
            occ::handle<Geom_Line> line=new Geom_Line(gp_Pnt(sx,sy,sz),gp_Dir(ex-sx,ey-sy,ez-sz));
            occ::handle<Geom_TrimmedCurve> segment=new Geom_TrimmedCurve(line,0,length);
            auto spline=GeomConvert::CurveToBSplineCurve(segment);
            if(spline.IsNull()||spline->Degree()!=1||spline->NbPoles()!=2)fail(JD_CANDIDATE_KERNEL_FAILURE,"Invalid native linear NURBS");
            *output=storeCurveEdge(value,spline);
        });
    },diagnostic,capacity);
}
extern "C" int32_t jd_candidate_curve_line_intersections(uint64_t context,uint64_t shape,double sx,double sy,double sz,double ex,double ey,double ez,double tolerance,jd_candidate_curve_line_event* events,uint32_t event_capacity,uint32_t* count,double* effective_fuzzy,char* diagnostic,uint32_t capacity)
{
    static_assert(sizeof(jd_candidate_curve_line_event)==136,"Candidate intersection layout");
    static_assert(offsetof(jd_candidate_curve_line_event,curve_start_point)==40,"Candidate point layout");
    return protect([&]{
        if(!count||!effective_fuzzy)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Missing intersection count/tolerance output");
        *count=0;*effective_fuzzy=0;
        if(!events&&event_capacity)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Null intersection buffer with positive capacity");
        checkedSegmentLength(sx,sy,sz,ex,ey,ez);
        if(!std::isfinite(tolerance)||tolerance<=0)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Intersection tolerance must be finite positive");
        withContext(context,[&](Context& value){
            const auto& shapeValue=findShape(value,shape).shape;
            if(shapeValue.ShapeType()!=TopAbs_EDGE)fail(JD_CANDIDATE_UNSUPPORTED_OPERATION,"Curve-line intersection requires an edge");
            const auto curveEdge=TopoDS::Edge(shapeValue);
            BRepBuilderAPI_MakeEdge segment(gp_Pnt(sx,sy,sz),gp_Pnt(ex,ey,ez));
            if(!segment.IsDone())fail(JD_CANDIDATE_KERNEL_FAILURE,"Native segment construction failed");
            const auto lineEdge=segment.Edge();
            BRepAdaptor_Curve curve(curveEdge),line(lineEdge);
            const double aFirst=curve.FirstParameter(),aLast=curve.LastParameter();
            const double bFirst=line.FirstParameter(),bLast=line.LastParameter();
            if(!std::isfinite(aFirst)||!std::isfinite(aLast)||!std::isfinite(bFirst)||!std::isfinite(bLast)||aLast<=aFirst||bLast<=bFirst)
                fail(JD_CANDIDATE_KERNEL_FAILURE,"Invalid native intersection domains");
            // Exact two-pole, non-rational linear NURBS are not approximations.
            // The general NURBS path classified a partial straight overlap as a point.
            // Use the kernel's analytic line/line solver and restore source parameters.
            auto solvingEdge=curveEdge;double solverFirst=aFirst,solverLast=aLast;bool reverseLinear=false;
            const bool linearNurbs=curve.GetType()==GeomAbs_BSplineCurve&&curve.Degree()==1&&curve.NbPoles()==2&&!curve.IsRational();
            if(linearNurbs){
                const auto startPoint=curve.Value(aFirst),endPoint=curve.Value(aLast);
                reverseLinear=gp_Dir(gp_Vec(startPoint,endPoint)).Dot(line.Line().Direction())<0;
                // Pinned line/line overlap clipping assumes aligned directions.
                BRepBuilderAPI_MakeEdge linear(reverseLinear?endPoint:startPoint,reverseLinear?startPoint:endPoint);
                if(!linear.IsDone())fail(JD_CANDIDATE_KERNEL_FAILURE,"Exact linear solver edge failed");
                solvingEdge=linear.Edge();BRepAdaptor_Curve solverCurve(solvingEdge);
                solverFirst=solverCurve.FirstParameter();solverLast=solverCurve.LastParameter();
                if(!std::isfinite(solverFirst)||!std::isfinite(solverLast)||solverLast<=solverFirst)
                    fail(JD_CANDIDATE_KERNEL_FAILURE,"Invalid linear solver domain");
            }
            auto sourceParameter=[&](double parameter){
                if(!linearNurbs)return parameter;
                if(parameter==solverFirst)return reverseLinear?aLast:aFirst;
                if(parameter==solverLast)return reverseLinear?aFirst:aLast;
                const double offset=(parameter-solverFirst)*(aLast-aFirst)/(solverLast-solverFirst);
                return reverseLinear?aLast-offset:aFirst+offset;
            };
            IntTools_EdgeEdge operation(solvingEdge,lineEdge);
            operation.SetFuzzyValue(tolerance);operation.UseQuickCoincidenceCheck(false);operation.Perform();
            if(!operation.IsDone())fail(JD_CANDIDATE_KERNEL_FAILURE,"Native edge intersection failed");
            *effective_fuzzy=operation.FuzzyValue();
            std::vector<jd_candidate_curve_line_event> found;
            auto endpointRoundoff=[&](double parameter,double first,double last){
                const double slack=16*std::numeric_limits<double>::epsilon()*std::max({1.0,std::abs(first),std::abs(last)});
                if(parameter<first&&first-parameter<=slack)return first;
                if(parameter>last&&parameter-last<=slack)return last;
                return parameter;
            };
            auto append=[&](uint32_t kind,double a0,double a1,double b0,double b1){
                a0=endpointRoundoff(a0,aFirst,aLast);a1=endpointRoundoff(a1,aFirst,aLast);
                b0=endpointRoundoff(b0,bFirst,bLast);b1=endpointRoundoff(b1,bFirst,bLast);
                if(!std::isfinite(a0)||!std::isfinite(a1)||!std::isfinite(b0)||!std::isfinite(b1)||a0>a1||b0>b1||
                    a0<aFirst||a1>aLast||b0<bFirst||b1>bLast)
                    fail(JD_CANDIDATE_KERNEL_FAILURE,"Native intersection returned invalid ranges");
                if(found.size()>=65536)fail(JD_CANDIDATE_UNSUPPORTED_OPERATION,"Candidate intersection event limit exceeded");
                jd_candidate_curve_line_event event{};
                event.kind=kind;event.curve_start=a0;event.curve_end=a1;
                event.line_start=(b0-bFirst)/(bLast-bFirst);event.line_end=(b1-bFirst)/(bLast-bFirst);
                event.curve_start_point=eventPoint(curve.Value(a0));event.curve_end_point=eventPoint(curve.Value(a1));
                event.line_start_point=eventPoint(line.Value(b0));event.line_end_point=eventPoint(line.Value(b1));
                found.push_back(event);
            };
            const auto& parts=operation.CommonParts();
            for(int i=1;i<=parts.Length();i++){
                const auto& part=parts.Value(i);
                if(!part.Edge1().IsSame(solvingEdge)||!part.Edge2().IsSame(lineEdge))
                    fail(JD_CANDIDATE_KERNEL_FAILURE,"Native intersection changed parameter ownership");
                if(part.Type()==TopAbs_VERTEX){
                    append(JD_CANDIDATE_POINT,sourceParameter(part.VertexParameter1()),sourceParameter(part.VertexParameter1()),part.VertexParameter2(),part.VertexParameter2());
                }else if(part.Type()==TopAbs_EDGE){
                    const auto& ranges=part.Ranges2();
                    if(ranges.IsEmpty())fail(JD_CANDIDATE_KERNEL_FAILURE,"Native overlap missing second range");
                    if(linearNurbs){
                        // The pinned solver clips only one end when B lies inside A.
                        // For exact lines, intersect finite intervals in native line coordinates.
                        const auto solverLine=BRepAdaptor_Curve(solvingEdge).Line();
                        const double p0=ElCLib::Parameter(solverLine,line.Value(bFirst));
                        const double p1=ElCLib::Parameter(solverLine,line.Value(bLast));
                        const double begin=std::max(solverFirst,std::min(p0,p1)),end=std::min(solverLast,std::max(p0,p1));
                        if(end<begin)fail(JD_CANDIDATE_KERNEL_FAILURE,"Native overlap has no finite common interval");
                        const double sourceFirst=sourceParameter(begin),sourceLast=sourceParameter(end);
                        const double lineFirst=ElCLib::Parameter(line.Line(),ElCLib::Value(begin,solverLine));
                        const double lineLast=ElCLib::Parameter(line.Line(),ElCLib::Value(end,solverLine));
                        append(JD_CANDIDATE_OVERLAP,std::min(sourceFirst,sourceLast),std::max(sourceFirst,sourceLast),std::min(lineFirst,lineLast),std::max(lineFirst,lineLast));
                    }else{
                        for(int j=1;j<=ranges.Length();j++)append(JD_CANDIDATE_OVERLAP,part.Range1().First(),part.Range1().Last(),ranges.Value(j).First(),ranges.Value(j).Last());
                    }
                }else fail(JD_CANDIDATE_UNSUPPORTED_OPERATION,"Unknown native intersection event");
            }
            std::stable_sort(found.begin(),found.end(),[](const auto& a,const auto& b){
                return a.curve_start<b.curve_start||(a.curve_start==b.curve_start&&a.line_start<b.line_start);
            });
            *count=static_cast<uint32_t>(found.size());
            if(!events&&!event_capacity)return;
            if(event_capacity<found.size())fail(JD_CANDIDATE_INVALID_ARGUMENT,"Intersection buffer too small");
            std::copy(found.begin(),found.end(),events);
        });
    },diagnostic,capacity);
}

extern "C" int32_t jd_candidate_curve_seam(uint64_t context,uint64_t shape,double seam,uint64_t* output,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        outputHandle(output);
        if(!std::isfinite(seam))fail(JD_CANDIDATE_INVALID_ARGUMENT,"Seam parameter must be finite");
        withContext(context,[&](Context& value){
            double first=0,last=0;auto curve=copiedEdgeSpline(value,shape,first,last);
            if(seam<first||seam>last)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Seam parameter outside edge domain");
            const double period=last-first,end=seam+period;
            if(!std::isfinite(period)||period<=0||!std::isfinite(end)||end<=seam)
                fail(JD_CANDIDATE_INVALID_ARGUMENT,"Unsupported seam domain");
            curve->Segment(first,last);
            if(!curve->IsClosed())fail(JD_CANDIDATE_INVALID_ARGUMENT,"Seam relocation requires a closed curve");
            curve->SetPeriodic();curve->SetOrigin(seam,0.0);
            // Native Segment inserts/clamps the endpoints and removes periodicity.
            curve->Segment(seam,end,0.0);
            const double slack=16*std::numeric_limits<double>::epsilon()*std::max({1.0,std::abs(seam),std::abs(end)});
            if(curve->IsPeriodic()||std::abs(curve->FirstParameter()-seam)>slack||std::abs(curve->LastParameter()-end)>slack)
                fail(JD_CANDIDATE_KERNEL_FAILURE,"Native seam domain was not preserved");
            *output=storeCurveEdge(value,curve);
        });
    },diagnostic,capacity);
}

extern "C" int32_t jd_candidate_curve_bounds(uint64_t context,uint64_t shape,double tolerance,double* min_x,double* min_y,double* min_z,double* max_x,double* max_y,double* max_z,double* padding,char* diagnostic,uint32_t capacity)
{
    return protect([&]{
        if(!min_x||!min_y||!min_z||!max_x||!max_y||!max_z||!padding)
            fail(JD_CANDIDATE_INVALID_ARGUMENT,"Missing curve bounds output");
        *min_x=0;*min_y=0;*min_z=0;*max_x=0;*max_y=0;*max_z=0;*padding=0;
        if(!std::isfinite(tolerance)||tolerance<=0)fail(JD_CANDIDATE_INVALID_ARGUMENT,"Bounds tolerance must be finite positive");
        withContext(context,[&](Context& value){
            const auto& input=findShape(value,shape).shape;
            if(input.ShapeType()!=TopAbs_EDGE)fail(JD_CANDIDATE_UNSUPPORTED_OPERATION,"Curve bounds require an edge");
            BRepAdaptor_Curve curve(TopoDS::Edge(input));
            const double first=curve.FirstParameter(),last=curve.LastParameter();
            if(!std::isfinite(first)||!std::isfinite(last)||last<=first)fail(JD_CANDIDATE_KERNEL_FAILURE,"Invalid bounds curve domain");
            Bnd_Box box;BndLib_Add3dCurve::AddOptimal(curve,first,last,tolerance,box);
            if(box.IsVoid()||box.IsOpen())fail(JD_CANDIDATE_KERNEL_FAILURE,"Native bounds are void or unbounded");
            const double gap=box.GetGap();
            if(!std::isfinite(gap)||gap<0)fail(JD_CANDIDATE_KERNEL_FAILURE,"Invalid native bounds padding");
            // Preserve numerical padding as metadata, not a physical radius increase.
            box.SetGap(0);double x0=0,y0=0,z0=0,x1=0,y1=0,z1=0;box.Get(x0,y0,z0,x1,y1,z1);
            for(double number:{x0,y0,z0,x1,y1,z1,x0-gap,y0-gap,z0-gap,x1+gap,y1+gap,z1+gap})
                if(!std::isfinite(number))fail(JD_CANDIDATE_KERNEL_FAILURE,"Nonfinite native bounds");
            if(x0>x1||y0>y1||z0>z1)fail(JD_CANDIDATE_KERNEL_FAILURE,"Reversed native bounds");
            *min_x=x0;*min_y=y0;*min_z=z0;*max_x=x1;*max_y=y1;*max_z=z1;*padding=gap;
        });
    },diagnostic,capacity);
}
