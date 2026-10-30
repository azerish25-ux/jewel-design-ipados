namespace Geomagic.Utilities;

public class DebugDialog
{
	public static IDialog Dialog { get; set; }

	static DebugDialog()
	{
		Dialog = new DefaultDialog();
	}

	public static bool? Show(string Message)
	{
		return null;
	}

	public static bool? Show(string Message, DialogRoles Role)
	{
		return null;
	}

	public static bool? Show(string Message, object Owner, DialogRoles Role)
	{
		return null;
	}

	public static bool? Show(string Message, object Owner, string Title, DialogRoles Role)
	{
		return null;
	}
}
