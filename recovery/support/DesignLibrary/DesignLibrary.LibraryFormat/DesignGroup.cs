using System;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;

namespace DesignLibrary.LibraryFormat;

internal class DesignGroup
{
	private readonly int _designGroupId;

	private readonly string _name;

	private readonly int _rank;

	public DesignGroup(SqlDataReader dr)
	{
		_designGroupId = Convert.ToInt32(dr["DesignGroupId"]);
		_name = dr["Name"].ToString();
		_rank = Convert.ToInt32(dr["Rank"]);
	}

	public DesignGroup(BinaryReader br)
	{
		_designGroupId = br.ReadInt32();
		_name = br.ReadString();
		_rank = br.ReadInt32();
	}

	public void BinaryWrite(BinaryWriter bw)
	{
		bw.Write(_designGroupId);
		bw.Write(_name);
		bw.Write(_rank);
	}

	public static SQLiteCommand SqlCreate()
	{
		return new SQLiteCommand("\r\n                CREATE TABLE IF NOT EXISTS [DesignGroup] (\r\n                    [DesignGroupId] INTEGER PRIMARY KEY NOT NULL,\r\n                    [Name] VARCHAR(50),\r\n                    [Rank] INTEGER\r\n                );\r\n                DELETE FROM [DesignGroup];");
	}

	public static string[] SqlInsert()
	{
		return new string[4] { "DesignGroup", "DesignGroupId", "Name", "Rank" };
	}

	public object[] SqlParams()
	{
		return new object[3] { _designGroupId, _name, _rank };
	}

	public static string SqlSelect()
	{
		return "\r\n                SELECT \r\n                    DesignGroupId,\r\n                    Name,\r\n                    Rank \r\n\t            FROM \r\n                    DesignGroup \r\n                ORDER BY\r\n                    DesignGroupId";
	}
}
