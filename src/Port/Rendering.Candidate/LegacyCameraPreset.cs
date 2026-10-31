using JewelDesign.Geometry.Candidate;

namespace JewelDesign.Rendering.Candidate;

public enum LegacyCameraView { Perspective, Front, Top, Side }
public enum LegacyCameraPlane { WorldXY, WorldZX, WorldYZ }

/// <summary>
/// Original managed camera command decisions, without native frustum or viewport execution.
/// Unknown names retain the original perspective pose/state and parallel projection request.
/// </summary>
public sealed class LegacyCameraPreset
{
    public string NormalizedName { get; }
    public LegacyCameraView View { get; }
    public LegacyCameraPlane ConstructionPlane { get; }
    public CandidatePoint3 CameraLocation { get; }
    public CandidatePoint3 Target => new(0, 0, 0);
    public CandidateVector3 Up { get; }
    public double LensMillimeters => 50;
    public bool SymmetricFrustum => true;
    public bool UseParallelProjection => NormalizedName != "perspective";

    private LegacyCameraPreset(string name, LegacyCameraView view,
        LegacyCameraPlane plane, CandidatePoint3 camera, CandidateVector3 up)
    {
        NormalizedName = name;
        View = view;
        ConstructionPlane = plane;
        CameraLocation = camera;
        Up = up;
    }

    public static LegacyCameraPreset Resolve(string cameraViewName)
    {
        // Keep the source null failure and invariant normalization at the command boundary.
        var name = cameraViewName.Trim().ToLowerInvariant();
        var (view, plane, camera, up) = name switch
        {
            "top" => (LegacyCameraView.Top, LegacyCameraPlane.WorldXY,
                new CandidatePoint3(0, 0, 80), new CandidateVector3(0, 1, 0)),
            "front" => (LegacyCameraView.Front, LegacyCameraPlane.WorldZX,
                new CandidatePoint3(0, -80, 0), new CandidateVector3(0, 0, 1)),
            "side" => (LegacyCameraView.Side, LegacyCameraPlane.WorldYZ,
                new CandidatePoint3(80, 0, 0), new CandidateVector3(0, 0, 1)),
            _ => (LegacyCameraView.Perspective, LegacyCameraPlane.WorldXY,
                new CandidatePoint3(45, -75, 50), new CandidateVector3(0, 0, 1))
        };
        return new LegacyCameraPreset(name, view, plane, camera, up);
    }
}
