using System.IO;
using System.Linq;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class KeyBypassHelper
{
	public static bool IsBypassKeyPresent()
	{
		bool result = false;
		foreach (string item in from driveInfo in DriveInfo.GetDrives()
			where driveInfo.DriveType == DriveType.Removable && driveInfo.IsReady
			select driveInfo.RootDirectory.FullName)
		{
			if (File.Exists(Path.Combine(item, "do_not_delete\\countersketch.lic")))
			{
				result = true;
				break;
			}
		}
		return result;
	}
}
