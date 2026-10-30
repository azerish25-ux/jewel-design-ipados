using System.IO;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class FileFolderHelper
{
	public static void CopyDirectory(string sourcePath, string destinationPath)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(sourcePath);
		DirectoryInfo directoryInfo2 = new DirectoryInfo(destinationPath);
		if (!directoryInfo2.Exists)
		{
			directoryInfo2.Create();
		}
		FileInfo[] files = directoryInfo.GetFiles();
		foreach (FileInfo fileInfo in files)
		{
			fileInfo.CopyTo(Path.Combine(directoryInfo2.FullName, fileInfo.Name));
		}
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		foreach (DirectoryInfo directoryInfo3 in directories)
		{
			string destinationPath2 = Path.Combine(directoryInfo2.FullName, directoryInfo3.Name);
			CopyDirectory(directoryInfo3.FullName, destinationPath2);
		}
	}

	public static void DelTree(string path)
	{
		if (path != null)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			FileInfo[] files = directoryInfo.GetFiles();
			foreach (FileInfo obj in files)
			{
				File.SetAttributes(obj.FullName, FileAttributes.Normal);
				obj.Delete();
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			for (int i = 0; i < directories.Length; i++)
			{
				directories[i].Delete(recursive: true);
			}
		}
	}

	public static string GetTempDirectory()
	{
		string text;
		do
		{
			text = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
		}
		while (Directory.Exists(text));
		Directory.CreateDirectory(text);
		return text;
	}
}
