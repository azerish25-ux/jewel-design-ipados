using System.IO;
using GV.CounterSketch.Studio.ZipIt;

namespace GV.CounterSketch.Studio.Database;

public static class CSReadFromLibrary
{
	public static bool ExtractBytesFromLibrary(string fullLibraryPath, long offset, ref byte[] data)
	{
		if (!File.Exists(fullLibraryPath))
		{
			return false;
		}
		zipFile zipFile = new zipFile();
		zipFile.ZipFileOpenReadFast(fullLibraryPath);
		bool result = zipFile.ZipFileGetBytesFast(offset, ref data, checkCRC: false);
		zipFile.ZipFileClose();
		return result;
	}

	public static bool ExtractFileFromLibrary(string fullLibraryPath, long offset, string outFilename)
	{
		if (!File.Exists(fullLibraryPath))
		{
			return false;
		}
		bool flag = false;
		zipFile zipFile = new zipFile();
		zipFile.ZipFileOpenReadFast(fullLibraryPath);
		flag = zipFile.ZipFileExtractFileFast(offset, outFilename);
		zipFile.ZipFileClose();
		return flag;
	}
}
