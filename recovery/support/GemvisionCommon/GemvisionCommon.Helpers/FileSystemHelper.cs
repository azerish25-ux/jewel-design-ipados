using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GemvisionCommon.Helpers;

public static class FileSystemHelper
{
	private class FileCompare : IEqualityComparer<FileInfo>
	{
		public bool Equals(FileInfo f1, FileInfo f2)
		{
			return f1.Name == f2.Name && f1.Length == f2.Length;
		}

		public int GetHashCode(FileInfo fi)
		{
			string text = $"{fi.Name}{fi.Length}";
			return text.GetHashCode();
		}
	}

	public static bool CompareDirectoryContents(string pathDirLhs, string pathDirRhs)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(pathDirLhs);
		DirectoryInfo directoryInfo2 = new DirectoryInfo(pathDirRhs);
		IEnumerable<FileInfo> files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
		IEnumerable<FileInfo> files2 = directoryInfo2.GetFiles("*.*", SearchOption.AllDirectories);
		return files.SequenceEqual(files2, new FileCompare());
	}

	public static bool CopyDirectoryContents(string pathDirSource, string pathDirDestination)
	{
		string[] directories = Directory.GetDirectories(pathDirSource, "*", SearchOption.AllDirectories);
		foreach (string text in directories)
		{
			Directory.CreateDirectory(text.Replace(pathDirSource, pathDirDestination));
		}
		string[] files = Directory.GetFiles(pathDirSource, "*.*", SearchOption.AllDirectories);
		foreach (string text2 in files)
		{
			File.Copy(text2, text2.Replace(pathDirSource, pathDirDestination), overwrite: true);
		}
		return CompareDirectoryContents(pathDirSource, pathDirDestination);
	}

	public static bool CopyFile(string pathFileSource, string pathFileDestination, bool doOverwrite)
	{
		return RetryHelper.Do(() => CopyFileAux(pathFileSource, pathFileDestination, doOverwrite), 10);
	}

	public static bool CreateDirectory(string pathDir)
	{
		if (Directory.Exists(pathDir))
		{
			return true;
		}
		DirectoryInfo directoryInfo = Directory.CreateDirectory(pathDir);
		return directoryInfo.Exists;
	}

	public static bool DeleteDir(string pathDir, bool doRecursive = false)
	{
		return RetryHelper.Do(() => DeleteDirAux(pathDir, doRecursive), 10);
	}

	public static void DeleteDirectoryContents(string pathDir)
	{
		string[] files = Directory.GetFiles(pathDir);
		foreach (string pathFile in files)
		{
			DeleteFile(pathFile);
		}
		string[] directories = Directory.GetDirectories(pathDir);
		foreach (string pathDir2 in directories)
		{
			DeleteDir(pathDir2, doRecursive: true);
		}
	}

	public static bool DeleteFile(string pathFile)
	{
		return RetryHelper.Do(() => DeleteFileAux(pathFile), 10);
	}

	public static string GetTempDirPath(string applicationName = null)
	{
		string text = Path.GetTempPath();
		if (!string.IsNullOrWhiteSpace(applicationName))
		{
			text = Path.Combine(text, applicationName);
		}
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	public static string GetTempFilePath(string applicationName = null, string extFileTemp = null)
	{
		string tempDirPath = GetTempDirPath(applicationName);
		if (string.IsNullOrWhiteSpace(extFileTemp))
		{
			extFileTemp = "tmp";
		}
		else
		{
			string text = extFileTemp.Trim().TrimStart('.');
			extFileTemp = (string.IsNullOrWhiteSpace(text) ? "tmp" : text);
		}
		string path = $"{Guid.NewGuid()}.{extFileTemp}";
		return Path.Combine(tempDirPath, path);
	}

	public static bool ValidateDirectoryPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return false;
		}
		if (Directory.Exists(path))
		{
			return true;
		}
		string text;
		if (File.Exists(path))
		{
			text = Path.GetDirectoryName(path);
			if (string.IsNullOrWhiteSpace(text))
			{
				return false;
			}
		}
		else if (Path.HasExtension(path))
		{
			text = Path.GetDirectoryName(path);
			if (string.IsNullOrWhiteSpace(text))
			{
				return false;
			}
		}
		else
		{
			text = path;
		}
		return CreateDirectory(text);
	}

	private static bool CopyFileAux(string pathFileSource, string pathFileDestination, bool doOverwrite)
	{
		try
		{
			File.Copy(pathFileSource, pathFileDestination, doOverwrite);
		}
		catch (Exception)
		{
			return false;
		}
		return File.Exists(pathFileSource);
	}

	private static bool DeleteDirAux(string pathDir, bool doRecursive = false)
	{
		try
		{
			Directory.Delete(pathDir, doRecursive);
		}
		catch (Exception)
		{
			return false;
		}
		return !Directory.Exists(pathDir);
	}

	private static bool DeleteFileAux(string pathFile)
	{
		try
		{
			File.Delete(pathFile);
		}
		catch (Exception)
		{
			return false;
		}
		return !File.Exists(pathFile);
	}
}
