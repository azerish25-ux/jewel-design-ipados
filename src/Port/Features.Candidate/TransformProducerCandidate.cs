using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;

/// <summary>Recovered input/default/arithmetic plans. Native Rhino matrix bit equivalence is not established.</summary>
public static class TransformProducerCandidate
{
    public static double AddScalars(LegacyChannelValue first,LegacyChannelValue second)
    {double x=0,y=0;first.GetDouble(ref x);second.GetDouble(ref y);return x+y;}
    public static LegacyAffineTransform Translation(LegacyChannelValue x,LegacyChannelValue y,LegacyChannelValue z,LegacyChannelValue mirror)
    {
        double dx=0,dy=0,dz=0,m=0;x.GetDouble(ref dx);y.GetDouble(ref dy);z.GetDouble(ref dz);mirror.GetDouble(ref m);
        if(m>.5){dx=0.0-dx;dz=0.0-dz;}
        return LegacyAffineTransform.Translation(dx,dy,dz);
    }
    public static LegacyAffineTransform ScaleToSize(CandidateCurveBounds bounds,LegacyChannelValue x,LegacyChannelValue y,LegacyChannelValue z)
    {
        double dx=bounds.Maximum.X-bounds.Minimum.X,dy=bounds.Maximum.Y-bounds.Minimum.Y,dz=bounds.Maximum.Z-bounds.Minimum.Z;
        if(dx<.01)dx=1;if(dy<.01)dy=1;if(dz<.01)dz=1;
        double tx=dx,ty=dy,tz=dz;x.GetDouble(ref tx);y.GetDouble(ref ty);z.GetDouble(ref tz);
        return LegacyAffineTransform.Scale(tx/dx,ty/dy,tz/dz);
    }
}
