using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using GV.CounterSketch.Studio.Controls;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

internal class PermissionsHelper
{
	public static bool FileHasWritePermissions(string filePath)
	{
		try
		{
			FileIOPermission perm = new FileIOPermission(FileIOPermissionAccess.Write, Path.GetDirectoryName(filePath));
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(perm);
			if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
			{
				using (FileStream fileStream = File.Create(Path.Combine(filePath), 10, FileOptions.DeleteOnClose))
				{
					fileStream.Close();
				}
				return true;
			}
			return false;
		}
		catch (UnauthorizedAccessException)
		{
			return false;
		}
	}

	public static bool FolderHasReadPermissions(string folderPath)
	{
		try
		{
			FileIOPermission perm = new FileIOPermission(FileIOPermissionAccess.Read, folderPath);
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(perm);
			if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
			{
				return true;
			}
			return false;
		}
		catch (UnauthorizedAccessException)
		{
			return false;
		}
	}

	public static bool FolderHasWritePermissions(string folderPath)
	{
		try
		{
			FileIOPermission perm = new FileIOPermission(FileIOPermissionAccess.Write, folderPath);
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(perm);
			if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
			{
				using (FileStream fileStream = File.Create(Path.Combine(folderPath, Path.GetRandomFileName()), 10, FileOptions.DeleteOnClose))
				{
					fileStream.Close();
				}
				return true;
			}
			return false;
		}
		catch (UnauthorizedAccessException)
		{
			return false;
		}
	}

	public static bool VerifyFileAccess(string path)
	{
		if (!FileHasWritePermissions(path))
		{
			DisplayPathErrorMessage(path);
			return false;
		}
		return true;
	}

	public static bool VerifyFolderAccess(string path)
	{
		if (!FolderHasWritePermissions(path))
		{
			DisplayPathErrorMessage(path);
			return false;
		}
		return true;
	}

	private static void DisplayPathErrorMessage(string path)
	{
		string message = $"We are have trouble accessing the following file or directory,\nplease make sure you have permissions to Read and Write to this location:\n\n{path}";
		WindowMessage windowMessage = new WindowMessage();
		windowMessage.Width = 700.0;
		windowMessage.Height = 650.0;
		windowMessage.Message = message;
		windowMessage.Topmost = true;
		windowMessage.ShowInTaskbar = true;
		windowMessage.WindowStartupLocation = WindowStartupLocation.CenterScreen;
		Window mainWindow = Application.Current.MainWindow;
		if (mainWindow != null)
		{
			windowMessage.Owner = mainWindow;
		}
		windowMessage.ShowDialog();
	}
}
