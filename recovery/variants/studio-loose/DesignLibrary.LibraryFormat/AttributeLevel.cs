using System;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;

namespace DesignLibrary.LibraryFormat;

internal class AttributeLevel
{
	private readonly int _attributeLevelId;

	private readonly string _name;

	public AttributeLevel(SqlDataReader dr)
	{
		_attributeLevelId = Convert.ToInt32(dr["AttributeLevelId"]);
		_name = dr["Name"].ToString();
	}

	public AttributeLevel(BinaryReader br)
	{
		_attributeLevelId = br.ReadInt32();
		_name = br.ReadString();
	}

	public void BinaryWrite(BinaryWriter bw)
	{
		bw.Write(_attributeLevelId);
		bw.Write(_name);
	}

	public static SQLiteCommand SqlCreate()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		return new SQLiteCommand("\r\n                CREATE TABLE IF NOT EXISTS [AttributeLevel] (\r\n                    [AttributeLevelId] INTEGER PRIMARY KEY NOT NULL,\r\n                    [Name] VARCHAR(50) );\r\n                DELETE FROM [AttributeLevel];");
	}

	public static string[] SqlInsert()
	{
		return new string[3] { "AttributeLevel", "AttributeLevelId", "Name" };
	}

	public object[] SqlParams()
	{
		return new object[2] { _attributeLevelId, _name };
	}

	public static string SqlSelect()
	{
		return "SELECT AttributeLevelId,Name FROM AttributeLevel ORDER BY AttributeLevelId";
	}
}
