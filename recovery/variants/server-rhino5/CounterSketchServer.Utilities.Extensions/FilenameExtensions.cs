using System;
using System.Globalization;
using System.IO;

namespace CounterSketchServer.Utilities.Extensions;

public static class FilenameExtensions
{
	public static string AppendTimeStampToFilename(this string fileName)
	{
		return Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture) + Path.GetExtension(fileName);
	}
}
