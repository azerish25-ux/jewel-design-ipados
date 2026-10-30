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
		zipFile zipFile2 = new zipFile();
		zipFile2.ZipFileOpenReadFast(fullLibraryPath);
		bool result = zipFile2.ZipFileGetBytesFast(offset, ref data, checkCRC: false);
		zipFile2.ZipFileClose();
		return result;
	}

	public static bool ExtractFileFromLibrary(string fullLibraryPath, long offset, string outFilename)
	{
		if (!File.Exists(fullLibraryPath))
		{
			return false;
		}
		bool flag = false;
		zipFile obj = new zipFile();
		obj.ZipFileOpenReadFast(fullLibraryPath);
		flag = obj.ZipFileExtractFileFast(offset, outFilename);
		obj.ZipFileClose();
		return flag;
	}
}
