using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CS.Pattern;

[Serializable]
[XmlRoot("ListOfUserData")]
public sealed class ListOfUserData : List<UniqueUserData>
{
	public UniqueUserData ContainsString(string s)
	{
		using (Enumerator enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UniqueUserData current = enumerator.Current;
				if (current.userDataString.CompareTo(s) == 0)
				{
					return current;
				}
			}
		}
		return null;
	}
}
