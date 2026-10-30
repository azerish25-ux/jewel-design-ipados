namespace Geomagic.Utilities;

public class StandardDialog
{
	public static IDialog Dialog { get; set; }

	static StandardDialog()
	{
		Dialog = new DefaultDialog();
	}

	public static bool? Show(string Message)
	{
		return Dialog.Show(Message);
	}

	public static bool? Show(string Message, DialogRoles Role)
	{
		return Dialog.Show(Message, Role);
	}

	public static bool? Show(string Message, object Owner, DialogRoles Role)
	{
		return Dialog.Show(Message, Owner, Role);
	}

	public static bool? Show(string Message, object Owner, string Title, DialogRoles Role)
	{
		return Dialog.Show(Message, Owner, Title, Role);
	}
}
