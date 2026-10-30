using System;
using CSDatas;
using RMA.OpenNURBS;

namespace CS;

public sealed class PreviewOutput
{
	public readonly CSData data;

	public readonly OnXform form;

	public PreviewOutput(CSData data)
	{
		this.data = data;
	}

	public PreviewOutput(CSData idata, OnXform iform)
	{
		data = idata;
		if (form != null && !form.IsIdentity() && form.IsValid())
		{
			form = new OnXform(iform);
		}
	}

	public bool SameObject(PreviewOutput obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (obj.data != data)
		{
			return false;
		}
		if (obj.form != null && form == null)
		{
			return false;
		}
		if (obj.form == null && form != null)
		{
			return false;
		}
		if (obj.form != null && form != null)
		{
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					if (Math.Abs(obj.form.get_m_xform(i, j) - form.get_m_xform(i, j)) > double.Epsilon)
					{
						return false;
					}
				}
			}
		}
		return true;
	}
}
