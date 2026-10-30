using System;

namespace Geomagic.Utilities;

public class DefaultCaseParser<T> : ICaseParser<T>
{
	public bool Read(string Source, out T Obj)
	{
		bool result = true;
		Obj = default(T);
		try
		{
			Obj = XmlSerialize.FromFile<T>(Source);
		}
		catch (Exception)
		{
			result = false;
		}
		return result;
	}

	public bool Write(string Dest, T Obj)
	{
		bool result = true;
		Obj.GetType();
		try
		{
			XmlSerialize.ToFile(Obj, Dest);
		}
		catch
		{
			result = false;
		}
		return result;
	}
}
