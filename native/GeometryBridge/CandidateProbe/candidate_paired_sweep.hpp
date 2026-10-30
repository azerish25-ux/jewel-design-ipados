#pragma once
#include <Geom_Curve.hxx>
#include <Geom_BSplineCurve.hxx>
#include <Geom_Surface.hxx>
#include <GeomFill_LocationLaw.hxx>
#include <TopoDS_Shape.hxx>
#include <vector>
namespace jewel::candidate {
enum class PairedCorrespondence {ExplicitLinear=0,SectionPlanes=1};
// Experimental explicit correspondence, not a verified RhinoSweep2 implementation.
occ::handle<GeomFill_LocationLaw> make_paired_location(const occ::handle<Geom_Curve>& first,const occ::handle<Geom_Curve>& second,const std::vector<double>& a,const std::vector<double>& b,bool maintainHeight,PairedCorrespondence correspondence=PairedCorrespondence::ExplicitLinear);
// Full profiles must meet rail points at compatible normalized curve parameters.
struct PairedSweepResult {occ::handle<Geom_Surface> surface;TopoDS_Shape shape;double approximationError;};
PairedSweepResult make_paired_sweep(const occ::handle<Geom_Curve>& first,const occ::handle<Geom_Curve>& second,const std::vector<occ::handle<Geom_BSplineCurve>>& profiles,const std::vector<double>& a,const std::vector<double>& b,bool maintainHeight,double tolerance,PairedCorrespondence correspondence=PairedCorrespondence::ExplicitLinear);
}
