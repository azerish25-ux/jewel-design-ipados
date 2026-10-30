using System;

namespace Geomagic.Utilities;

public class CaseEventArgs<T> : EventArgs
{
	private T _Case;

	public T Case
	{
		get
		{
			return _Case;
		}
		set
		{
			_Case = value;
		}
	}

	public CaseEventArgs()
	{
	}

	public CaseEventArgs(T Case)
	{
		this.Case = Case;
	}
}
