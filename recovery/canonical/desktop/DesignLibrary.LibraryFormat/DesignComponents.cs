using System;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;

namespace DesignLibrary.LibraryFormat;

public class DesignComponents
{
	private readonly int _designId;

	private readonly int _componentId;

	public DesignComponents(SqlDataReader dr)
	{
		_designId = Convert.ToInt32(dr["DesignId"]);
		_componentId = Convert.ToInt32(dr["ComponentId"]);
	}

	public DesignComponents(BinaryReader br)
	{
		_designId = br.ReadInt32();
		_componentId = br.ReadInt32();
	}

	public void BinaryWrite(BinaryWriter bw)
	{
		bw.Write(_designId);
		bw.Write(_componentId);
	}

	public static SQLiteCommand SqlCreate()
	{
		return new SQLiteCommand("\r\n                CREATE TABLE IF NOT EXISTS [DesignComponents] (\r\n                \t[DesignComponentId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,\r\n                    [DesignId] INTEGER NOT NULL,\r\n                    [ComponentId] INTEGER NOT NULL,\r\n\t            FOREIGN KEY ([DesignId]) REFERENCES [Designs] ([DesignId]),\r\n\t            FOREIGN KEY ([ComponentId]) REFERENCES [Component] ([ComponentId])\r\n                );\r\n\r\n                DELETE FROM [DesignComponents];");
	}

	public static string SqlIndex()
	{
		return "\r\n                CREATE INDEX IF NOT EXISTS [IDX_DesignComponents_DesignId] ON [DesignComponents](\r\n                [DesignId]  ASC\r\n                )";
	}

	public static string[] SqlInsert()
	{
		return new string[3] { "DesignComponents", "DesignId", "ComponentId" };
	}

	public object[] SqlParams()
	{
		return new object[2] { _designId, _componentId };
	}

	public static string SqlSelect()
	{
		return "\r\n                SELECT\r\n                   (SELECT DesignId FROM DesignVersions WHERE DesignVersions.DesignVersionId=DesignVersionComponents.DesignVersionId) as DesignId,\r\n                   ComponentId\r\n                FROM\r\n                    DesignVersionComponents \r\n                WHERE\r\n                    {1}\r\n";
	}
}
