namespace Geomagic.Utilities;

public interface IDialog
{
	bool? Show(string Message);

	bool? Show(string Message, DialogRoles Role);

	bool? Show(string Message, object Owner, DialogRoles Role);

	bool? Show(string Message, object Owner, string Title, DialogRoles Role);
}
