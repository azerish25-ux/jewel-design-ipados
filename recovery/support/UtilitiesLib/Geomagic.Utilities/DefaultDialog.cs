using System.Windows.Forms;

namespace Geomagic.Utilities;

public class DefaultDialog : IDialog
{
	public bool? Show(string Message, object Owner, string Title, DialogRoles Role)
	{
		IWin32Window owner = ToWindowOwner(Owner);
		MessageBoxButtons buttons = ToMessageBoxButtons(Role);
		DialogResult result = MessageBox.Show(owner, Message, Title, buttons);
		return ToNullableBoolean(result);
	}

	public bool? Show(string Message)
	{
		return Show(Message, null, string.Empty, DialogRoles.OK);
	}

	public bool? Show(string Message, DialogRoles Role)
	{
		return Show(Message, null, string.Empty, Role);
	}

	public bool? Show(string Message, object Owner, DialogRoles Role)
	{
		return Show(Message, Owner, string.Empty, Role);
	}

	private IWin32Window ToWindowOwner(object Owner)
	{
		IWin32Window win32Window = Owner as IWin32Window;
		if (win32Window != null)
		{
			return win32Window;
		}
		if (Application.OpenForms != null && Application.OpenForms.Count > 0)
		{
			win32Window = Application.OpenForms[0];
		}
		return win32Window;
	}

	private MessageBoxButtons ToMessageBoxButtons(DialogRoles Role)
	{
		MessageBoxButtons result = MessageBoxButtons.OK;
		switch (Role)
		{
		case DialogRoles.OKCancel:
			result = MessageBoxButtons.OKCancel;
			break;
		case DialogRoles.YesNo:
			result = MessageBoxButtons.YesNo;
			break;
		case DialogRoles.YesNoCancel:
			result = MessageBoxButtons.YesNoCancel;
			break;
		}
		return result;
	}

	private bool? ToNullableBoolean(DialogResult Result)
	{
		bool? result = false;
		switch (Result)
		{
		case DialogResult.Cancel:
			result = null;
			break;
		case DialogResult.OK:
			result = true;
			break;
		case DialogResult.Yes:
			result = true;
			break;
		}
		return result;
	}
}
