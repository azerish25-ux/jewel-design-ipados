using GemvisionCommon.Utilities.Networking.Enums;

namespace GemvisionCommon.Utilities.Networking.Structs;

public struct NetworkResource
{
	public ResourceScope Scope;

	public ResourceType ResourceType;

	public ResourceDisplayType DisplayType;

	public int Usage;

	public string LocalName;

	public string RemoteName;

	public string Comment;

	public string Provider;
}
