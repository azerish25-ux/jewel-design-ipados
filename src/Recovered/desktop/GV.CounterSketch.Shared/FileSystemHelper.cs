using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace GV.CounterSketch.Shared;

public static class FileSystemHelper
{
	public static void CopyDirectory(string source, string destination, bool doOverwrite)
	{
		if (!Directory.Exists(destination))
		{
			Directory.CreateDirectory(destination);
		}
		string[] files = Directory.GetFiles(source);
		foreach (string obj in files)
		{
			string fileName = Path.GetFileName(obj);
			string text = Path.Combine(destination, fileName);
			FileInfo fileInfo = new FileInfo(text);
			if (fileInfo.Exists)
			{
				fileInfo.Attributes = FileAttributes.Normal;
			}
			File.Copy(obj, text, doOverwrite);
		}
		files = Directory.GetDirectories(source);
		foreach (string obj2 in files)
		{
			string fileName2 = Path.GetFileName(obj2);
			string destination2 = Path.Combine(destination, fileName2);
			CopyDirectory(obj2, destination2, doOverwrite);
		}
	}

	public static void CopyFile(string source, string destination)
	{
		byte[] bytes = File.ReadAllBytes(source);
		File.WriteAllBytes(destination, bytes);
	}

	public static bool DeleteFile(string filename)
	{
		int num = 0;
		bool flag;
		for (flag = File.Exists(filename); flag && num < 10; flag = File.Exists(filename))
		{
			try
			{
				File.Delete(filename);
				Thread.Sleep(100);
			}
			catch (Exception)
			{
				Thread.Sleep(100);
			}
			num++;
		}
		return !flag;
	}

	public static string GetTempPath(string application = "", string extension = ".tmp", string requestedFileName = "")
	{
		string text = extension;
		if (string.IsNullOrEmpty(text))
		{
			text = ".tmp";
		}
		if (text.Substring(0, 1) != ".")
		{
			text = "." + text;
		}
		string text2 = Path.GetTempPath();
		if (!string.IsNullOrWhiteSpace(application))
		{
			text2 = Path.Combine(text2, application);
		}
		if (!Directory.Exists(text2))
		{
			Directory.CreateDirectory(text2);
		}
		string fileName = requestedFileName;
		if (string.IsNullOrWhiteSpace(fileName) || Path.GetInvalidFileNameChars().Any((char invalidCharacter) => fileName.Contains(invalidCharacter)))
		{
			fileName = Guid.NewGuid().ToString();
		}
		return Path.Combine(text2, fileName + text);
	}
}
