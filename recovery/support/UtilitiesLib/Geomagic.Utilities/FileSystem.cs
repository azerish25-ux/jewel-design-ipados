using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Geomagic.Utilities;

public static class FileSystem
{
	public enum SpecialFolder
	{
		Desktop = 0,
		StartMenuPrograms = 2,
		MyDocuments = 5,
		Favorites = 6,
		Startup = 7,
		Recent = 8,
		SendTo = 9,
		StartMenu = 11,
		MyMusic = 13,
		MyVideos = 14,
		NetworkShortcuts = 19,
		Fonts = 20,
		Templates = 21,
		TemporaryInternetFiles = 32,
		Cookies = 33,
		History = 34,
		CommonApplicationData = 35,
		Windows = 36,
		System = 37,
		ProgramFiles = 38,
		MyPictures = 39,
		CommonProgramFiles = 43,
		PublicDocuments = 46,
		CommonMusic = 53,
		CommonPictures = 54,
		CommonVideos = 55,
		Executing = 100
	}

	public enum SpecialFolderOption
	{
		None = 0,
		DoNotVerify = 0x4000,
		Create = 0x8000
	}

	[DllImport("shfolder.dll", CharSet = CharSet.Auto)]
	internal static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);

	public static string GetFolderPath(SpecialFolder Folder, SpecialFolderOption Option)
	{
		string empty = string.Empty;
		if (Folder == SpecialFolder.Executing)
		{
			empty = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}
		else
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			SHGetFolderPath(IntPtr.Zero, (int)Folder, IntPtr.Zero, 0, stringBuilder);
			empty = stringBuilder.ToString();
		}
		empty = FormatFolderPath(empty);
		switch (Option)
		{
		case SpecialFolderOption.Create:
			if (!Directory.Exists(empty))
			{
				Directory.CreateDirectory(empty);
			}
			break;
		case SpecialFolderOption.None:
			if (!Directory.Exists(empty))
			{
				empty = string.Empty;
			}
			break;
		}
		return empty;
	}

	public static string GetFolderPath(SpecialFolder Folder)
	{
		return GetFolderPath(Folder, SpecialFolderOption.None);
	}

	public static string FormatFolderPath(string Path)
	{
		if (string.IsNullOrEmpty(Path))
		{
			return Path;
		}
		string text = Path;
		string text2 = System.IO.Path.DirectorySeparatorChar.ToString();
		if (!text.EndsWith(text2))
		{
			text += text2;
		}
		return text;
	}

	public static bool Exists(string Path)
	{
		if (!Directory.Exists(Path))
		{
			return File.Exists(Path);
		}
		return true;
	}

	public static bool IsFolder(string Path)
	{
		return !System.IO.Path.HasExtension(Path);
	}

	public static bool IsSubFolder(string Root, string Path)
	{
		return IsSubFolder(Root, Path, IncludeRoot: false, int.MaxValue);
	}

	public static bool IsSubFolder(string Root, string Path, bool IncludeRoot)
	{
		return IsSubFolder(Root, Path, IncludeRoot, int.MaxValue);
	}

	public static bool IsSubFolder(string Root, string Path, bool IncludeRoot, int MaxDepth)
	{
		if (!Directory.Exists(Root))
		{
			throw new DirectoryNotFoundException("Root directory not found.");
		}
		if (MaxDepth < 0)
		{
			throw new ArgumentOutOfRangeException("MaxDepth");
		}
		string normalPath = GetNormalPath(System.IO.Path.GetDirectoryName(Root));
		string normalPath2 = GetNormalPath(System.IO.Path.GetDirectoryName(Path));
		if (normalPath.Equals(normalPath2, StringComparison.InvariantCultureIgnoreCase))
		{
			if (IncludeRoot)
			{
				return true;
			}
			return false;
		}
		return FindSubFolder(normalPath, normalPath2, 0, MaxDepth);
	}

	public static string GetNormalPath(string RelativeOrAbsolute)
	{
		return GetNormalPath(string.Empty, RelativeOrAbsolute);
	}

	public static string GetNormalPath(string Root, string RelativeOrAbsolute)
	{
		if (string.IsNullOrEmpty(RelativeOrAbsolute))
		{
			return string.Empty;
		}
		string path = RelativeOrAbsolute;
		if (!Path.IsPathRooted(RelativeOrAbsolute))
		{
			path = Path.Combine(Root, RelativeOrAbsolute);
		}
		return Path.GetFullPath(path);
	}

	private static bool FindSubFolder(string Root, string Path, int Depth, int MaxDepth)
	{
		if (Depth <= MaxDepth)
		{
			string[] directories = Directory.GetDirectories(Root);
			string[] array = directories;
			foreach (string value in array)
			{
				if (Path.Equals(value, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
				if (FindSubFolder(Root, Path, Depth + 1, MaxDepth))
				{
					return true;
				}
			}
		}
		return false;
	}
}
