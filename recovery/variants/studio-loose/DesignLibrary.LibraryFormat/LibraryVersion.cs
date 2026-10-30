using System;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;

namespace DesignLibrary.LibraryFormat;

internal class LibraryVersion
{
	public int Version;

	public int KeyProfileId;

	public LibraryVersion(SqlDataReader dr)
	{
		Version = Convert.ToInt32(dr["Version"]);
		KeyProfileId = Convert.ToInt32(dr["KeyProfileId"]);
	}

	public LibraryVersion(BinaryReader br)
	{
		Version = br.ReadInt32();
		KeyProfileId = br.ReadInt32();
	}

	public LibraryVersion(int version, int keyProfileId)
	{
		Version = version;
		KeyProfileId = keyProfileId;
	}

	public void BinaryWrite(BinaryWriter bw)
	{
		bw.Write(Version);
		bw.Write(KeyProfileId);
	}

	public static SQLiteCommand SqlCreate()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		return new SQLiteCommand("\r\n                CREATE TABLE IF NOT EXISTS [LibraryVersion] ( \r\n                    [Version] INTEGER,\r\n                    [KeyProfileId] INTEGER\r\n                );\r\n                DELETE FROM [LibraryVersion];");
	}

	public static string[] SqlInsert()
	{
		return new string[3] { "LibraryVersion", "Version", "KeyProfileId" };
	}

	public object[] SqlParams()
	{
		return new object[2] { Version, KeyProfileId };
	}

	public static string SqlSelect()
	{
		return " SELECT Version,KeyProfileId From LibraryVersion";
	}
}
