using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Geomagic.Utilities;

public static class Extensions
{
	public static IList<T> CreateInstancesOfSubclass<T>(this Assembly Assem)
	{
		List<T> result = new List<T>();
		(from x in Assem.GetTypes()
			where x.IsPublic && !x.IsAbstract && x.IsSubclassOf(typeof(T)) && x.HasDefaultConstructor()
			select x).ToList().ForEach(delegate(Type x)
		{
			result.Add((T)Activator.CreateInstance(x));
		});
		return result;
	}

	public static bool HasDefaultConstructor(this Type t)
	{
		return t.GetConstructors().Any((ConstructorInfo x) => x.GetParameters().Length == 0);
	}

	public static bool HasConstructor(this Type t, params Type[] Arguments)
	{
		return t.GetConstructor(Arguments) != null;
	}

	public static bool IsSet(this string Str)
	{
		return !string.IsNullOrEmpty(Str);
	}

	public static bool IsNullOrEmpty(this string Str)
	{
		return string.IsNullOrEmpty(Str);
	}
}
