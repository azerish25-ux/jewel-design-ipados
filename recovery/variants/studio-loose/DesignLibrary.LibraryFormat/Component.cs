using System;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;

namespace DesignLibrary.LibraryFormat;

internal class Component
{
	private readonly int _componentId;

	private readonly string _name;

	public Component(SqlDataReader dr)
	{
		_componentId = Convert.ToInt32(dr["ComponentId"]);
		_name = dr["Name"].ToString();
	}

	public Component(BinaryReader br)
	{
		_componentId = br.ReadInt32();
		_name = br.ReadString();
	}

	public void BinaryWrite(BinaryWriter bw)
	{
		bw.Write(_componentId);
		bw.Write(_name);
	}

	public static SQLiteCommand SqlCreate()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		return new SQLiteCommand("\r\n                CREATE TABLE IF NOT EXISTS [Component] (\r\n                    [ComponentId] INTEGER PRIMARY KEY NOT NULL,\r\n                    [Name] VARCHAR(50) );\r\n                DELETE FROM [Component];");
	}

	public static string[] SqlInsert()
	{
		return new string[3] { "Component", "ComponentId", "Name" };
	}

	public object[] SqlParams()
	{
		return new object[2] { _componentId, _name };
	}

	public static string SqlSelect()
	{
		return "SELECT ComponentId,Name FROM Component ORDER BY ComponentId";
	}
}
