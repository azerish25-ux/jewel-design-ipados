using System;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;

namespace DesignLibrary.LibraryFormat;

internal class AttributeType
{
	private readonly int _attributeTypeId;

	private readonly string _attributeType;

	private readonly string _attributeCsName;

	private readonly int _attributeCsOrder;

	private readonly int _attributeCsDisplayType;

	private readonly int _attributeLevelId;

	public AttributeType(SqlDataReader dr)
	{
		_attributeTypeId = Convert.ToInt32(dr["AttributeTypeId"]);
		_attributeType = dr["AttributeType"].ToString();
		_attributeCsName = dr["AttributeCSName"].ToString();
		_attributeCsOrder = Convert.ToInt32(dr["AttributeCSOrder"]);
		_attributeCsDisplayType = Convert.ToInt32(dr["AttributeCSDisplayType"]);
		_attributeLevelId = Convert.ToInt32(dr["AttributeLevelId"]);
	}

	public AttributeType(BinaryReader br)
	{
		_attributeTypeId = br.ReadInt32();
		_attributeType = br.ReadString();
		_attributeCsName = br.ReadString();
		_attributeCsOrder = br.ReadInt32();
		_attributeCsDisplayType = br.ReadInt32();
		_attributeLevelId = br.ReadInt32();
	}

	public void BinaryWrite(BinaryWriter bw)
	{
		bw.Write(_attributeTypeId);
		bw.Write(_attributeType);
		bw.Write(_attributeCsName);
		bw.Write(_attributeCsOrder);
		bw.Write(_attributeCsDisplayType);
		bw.Write(_attributeLevelId);
	}

	public static SQLiteCommand SqlCreate()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		return new SQLiteCommand("\r\n                CREATE TABLE IF NOT EXISTS [AttributeType] (\r\n\t                [AttributeTypeId] INTEGER PRIMARY KEY NOT NULL,\r\n\t                [AttributeType] nvarchar(50) NOT NULL,\r\n\t                [AttributeCSName] nvarchar(50),\r\n\t                [AttributeCSOrder] integer,\r\n\t                [AttributeCSDisplayType] integer,\r\n\t                [AttributeLevelId] integer,\r\n                FOREIGN KEY ([AttributeLevelId]) REFERENCES [AttributeLevel] ([AttributeLevelId])\r\n                );\r\n\r\n\r\n\r\n\r\n                DELETE FROM [AttributeType];");
	}

	public static string[] SqlInsert()
	{
		return new string[7] { "AttributeType", "AttributeTypeId", "AttributeType", "AttributeCSName", "AttributeCSOrder", "AttributeCSDisplayType", "AttributeLevelId" };
	}

	public object[] SqlParams()
	{
		return new object[6] { _attributeTypeId, _attributeType, _attributeCsName, _attributeCsOrder, _attributeCsDisplayType, _attributeLevelId };
	}

	public static string SqlSelect()
	{
		return "\r\n            SELECT \r\n                AttributeTypeId,\r\n                AttributeType,\r\n                AttributeCSName,\r\n                AttributeCSOrder,\r\n                AttributeCSDisplayType,\r\n                AttributeLevelId FROM AttributeType ORDER BY AttributeTypeId";
	}
}
