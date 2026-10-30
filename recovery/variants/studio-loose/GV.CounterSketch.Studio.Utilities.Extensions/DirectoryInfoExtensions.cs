using System.IO;

namespace GV.CounterSketch.Studio.Utilities.Extensions;

public static class DirectoryInfoExtensions
{
	public static void Empty(this DirectoryInfo directory)
	{
		FileInfo[] files = directory.GetFiles();
		for (int i = 0; i < files.Length; i++)
		{
			files[i].Delete();
		}
		DirectoryInfo[] directories = directory.GetDirectories();
		for (int i = 0; i < directories.Length; i++)
		{
			directories[i].Delete(recursive: true);
		}
	}
}
