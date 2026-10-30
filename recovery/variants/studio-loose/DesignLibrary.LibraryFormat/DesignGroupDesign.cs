using System;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;

namespace DesignLibrary.LibraryFormat;

internal class DesignGroupDesign
{
	private readonly int _designGroupId;

	private readonly int _designId;

	private readonly int _rank;

	public DesignGroupDesign(SqlDataReader dr)
	{
		_designGroupId = Convert.ToInt32(dr["DesignGroupId"]);
		_designId = Convert.ToInt32(dr["DesignId"]);
		_rank = Convert.ToInt32(dr["Rank"]);
	}

	public DesignGroupDesign(BinaryReader br)
	{
		_designGroupId = br.ReadInt32();
		_designId = br.ReadInt32();
		_rank = br.ReadInt32();
	}

	public void BinaryWrite(BinaryWriter bw)
	{
		bw.Write(_designGroupId);
		bw.Write(_designId);
		bw.Write(_rank);
	}

	public static SQLiteCommand SqlCreate()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		return new SQLiteCommand("\r\n                CREATE TABLE IF NOT EXISTS [DesignGroupDesigns] (\r\n                    [DesignGroupDesignId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,\r\n                    [DesignGroupId] INTEGER,\r\n                    [DesignId] INTEGER,\r\n                    [Rank] INTEGER,\r\n   \t            FOREIGN KEY ([DesignGroupId]) REFERENCES [DesignGroup] ([DesignGroupId]),\r\n   \t            FOREIGN KEY ([DesignId]) REFERENCES [Designs] ([DesignId])\r\n                );\r\n\r\n                DELETE FROM [DesignGroupDesigns];");
	}

	public static string SqlIndex()
	{
		return "\r\n                CREATE INDEX IF NOT EXISTS [IDX_DesignGroupDesigns_DesignGroupId] ON [DesignGroupDesigns](\r\n                [DesignGroupId]  ASC\r\n                )";
	}

	public static string[] SqlInsert()
	{
		return new string[4] { "DesignGroupDesigns", "DesignGroupId", "DesignId", "Rank" };
	}

	public object[] SqlParams()
	{
		return new object[3] { _designId, _designGroupId, _rank };
	}

	public static string SqlSelect()
	{
		return "\r\n                SELECT \r\n                    DesignGroupDesigns.DesignGroupId,\r\n                    DesignId,\r\n                    DesignGroupDesigns.Rank \r\n\t            FROM \r\n                    DesignGroupDesigns INNER JOIN DesignGroup ON DesignGroupDesigns.DesignGroupID=DesignGroup.DesignGroupID \r\n                WHERE\r\n                    {0}";
	}
}
