using System;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;

namespace DesignLibrary.LibraryFormat;

internal class DesignGemTables
{
	private readonly int _designId;

	private readonly int _gemPlacmentId;

	private readonly int _gemTableShapeSizeId;

	public DesignGemTables(SqlDataReader dr)
	{
		_designId = Convert.ToInt32(dr["DesignId"]);
		_gemPlacmentId = Convert.ToInt32(dr["GemPlacementId"]);
		_gemTableShapeSizeId = Convert.ToInt32(dr["GemTableShapeSizeId"]);
	}

	public DesignGemTables(BinaryReader br)
	{
		_designId = br.ReadInt32();
		_gemPlacmentId = br.ReadInt32();
		_gemTableShapeSizeId = br.ReadInt32();
	}

	public void BinaryWrite(BinaryWriter bw)
	{
		bw.Write(_designId);
		bw.Write(_gemPlacmentId);
		bw.Write(_gemTableShapeSizeId);
	}

	public static SQLiteCommand SqlCreate()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		return new SQLiteCommand("\r\n                CREATE TABLE IF NOT EXISTS [DesignGemTables] (\r\n                    [DesignGemTableId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,\r\n                    [DesignId] INTEGER NOT NULL,\r\n                    [GemPlacementId] INTEGER NOT NULL,\r\n                    [GemTableShapeSizeId] INTEGER NOT NULL,\r\n  \t            FOREIGN KEY ([DesignId]) REFERENCES [Designs] ([DesignId]),\r\n                FOREIGN KEY ([GemTableShapeSizeId]) REFERENCES [GemTableShapeSize] ([GemTableShapeSizeId])\r\n                );\r\n\r\n                DELETE FROM [DesignGemTables];");
	}

	public static string SqlIndex()
	{
		return "\r\n                CREATE INDEX IF NOT EXISTS [IDX_DesignGemTables_All] ON [DesignGemTables](DesignId,GemPlacementId,GemTableShapeSizeId)";
	}

	public static string[] SqlInsert()
	{
		return new string[4] { "DesignGemTables", "DesignId", "GemPlacementId", "GemTableShapeSizeId" };
	}

	public object[] SqlParams()
	{
		return new object[3] { _designId, _gemPlacmentId, _gemTableShapeSizeId };
	}

	public static string SqlSelect()
	{
		return "\r\n                SELECT\r\n                      (SELECT DesignId FROM DesignVersions WHERE DesignVersions.DesignVersionId=DesignVersionGemTable.DesignVersionId) as DesignId,\r\n                      GemPlacementId,\r\n                      GemTableShapeSizeId\r\n                FROM\r\n                    DesignVersionGemTable\r\n                WHERE\r\n                    {1}";
	}
}
