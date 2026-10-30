using CSBlocks.Core;

namespace GV.CSStudioPatchWire;

public static class CSProfiles
{
	public static CSGroupOfMatrixProfiles GetBezelBlock()
	{
		CSGroupOfMatrixProfiles cSGroupOfMatrixProfiles = new CSGroupOfMatrixProfiles("ProfileCurves");
		cSGroupOfMatrixProfiles.AddObjectsFromLibraryPath("BezelProfiles");
		return cSGroupOfMatrixProfiles;
	}

	public static CSGroupOfMatrixProfiles GetGalleryRailBlock()
	{
		CSGroupOfMatrixProfiles cSGroupOfMatrixProfiles = new CSGroupOfMatrixProfiles("ProfileCurves");
		cSGroupOfMatrixProfiles.AddObjectsFromLibraryPath("GalleryRailProfiles");
		return cSGroupOfMatrixProfiles;
	}

	public static CSGroupOfMatrixProfiles GetProngBlock()
	{
		CSGroupOfMatrixProfiles cSGroupOfMatrixProfiles = new CSGroupOfMatrixProfiles("ProfileCurves");
		cSGroupOfMatrixProfiles.AddObjectsFromLibraryPath("ProngProfiles");
		return cSGroupOfMatrixProfiles;
	}
}
