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
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		data = idata;
		if (form != null && !form.IsIdentity() && form.IsValid())
		{
			form = new OnXform((IOnXform)(object)iform);
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
					if (Math.Abs(obj.form[i, j] - form[i, j]) > double.Epsilon)
					{
						return false;
					}
				}
			}
		}
		return true;
	}
}
