using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class ResourceHelper
{
	public static bool ResourceExists(string resourcePath)
	{
		return ResourceExists(Assembly.GetExecutingAssembly(), resourcePath);
	}

	public static bool ResourceExists(Assembly assembly, string resourcePath)
	{
		return GetResourcePaths(assembly).Contains(resourcePath.ToLowerInvariant());
	}

	public static IEnumerable<object> GetResourcePaths(Assembly assembly)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		string baseName = assembly.GetName().Name + ".g";
		ResourceManager resourceManager = new ResourceManager(baseName, assembly);
		try
		{
			ResourceSet resourceSet = resourceManager.GetResourceSet(currentCulture, createIfNotExists: true, tryParents: true);
			foreach (DictionaryEntry item in resourceSet)
			{
				yield return item.Key;
			}
		}
		finally
		{
			resourceManager.ReleaseAllResources();
		}
	}
}
