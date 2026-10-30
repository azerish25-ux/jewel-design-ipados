using System;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;

namespace DesignLibrary.LibraryFormat;

internal class Attribute
{
	private readonly int _attributeId;

	private readonly int _attributeTypeId;

	private readonly string _attributeName;

	private readonly int _attributeCsOrder;

	public Attribute(SqlDataReader dr)
	{
		_attributeId = Convert.ToInt32(dr["AttributeId"]);
		_attributeTypeId = Convert.ToInt32(dr["AttributeTypeId"]);
		_attributeName = dr["AttributeName"].ToString();
		_attributeCsOrder = Convert.ToInt32(dr["AttributeCSOrder"]);
	}

	public Attribute(BinaryReader br)
	{
		_attributeId = br.ReadInt32();
		_attributeTypeId = br.ReadInt32();
		_attributeName = br.ReadString();
		_attributeCsOrder = br.ReadInt32();
	}

	public void BinaryWrite(BinaryWriter bw)
	{
		bw.Write(_attributeId);
		bw.Write(_attributeTypeId);
		bw.Write(_attributeName);
		bw.Write(_attributeCsOrder);
	}

	public static SQLiteCommand SqlCreate()
	{
		return new SQLiteCommand("\r\n                CREATE TABLE IF NOT EXISTS [Attribute] (\r\n                    [AttributeId] INTEGER PRIMARY KEY NOT NULL,\r\n                    [AttributeTypeId] INTEGER NOT NULL,                        \r\n                    [AttributeName] VARCHAR(50) NOT NULL,         \r\n                    [AttributeCSOrder] INTEGER NOT NULL,\r\n                FOREIGN KEY([AttributeTypeId]) REFERENCES [AttributeType] ([AttributeTypeId])\r\n                );\r\n\r\n                DELETE FROM [Attribute];");
	}

	public static string[] SqlInsert()
	{
		return new string[5] { "Attribute", "AttributeId", "AttributeTypeId", "AttributeName", "AttributeCSOrder" };
	}

	public object[] SqlParams()
	{
		return new object[4] { _attributeId, _attributeTypeId, _attributeName, _attributeCsOrder };
	}

	public static string SqlSelect()
	{
		return "\r\n            SELECT AttributeId,AttributeTypeId,AttributeName,AttributeCSOrder FROM Attribute\r\n                    WHERE        \r\n                    ((SELECT COUNT(DesignId) FROM DesignAttribute WHERE (AttributeId = Attribute.AttributeId)) >=1) or \r\n                    ((SELECT COUNT(DesignVersionAttributeId) FROM DesignVersionAttribute WHERE (AttributeId = Attribute.AttributeId)) >=1) or \r\n                    ((SELECT COUNT(DesignVersionComponentAttributeId) FROM DesignVersionComponentAttribute WHERE (AttributeId = Attribute.AttributeId)) >=1) or \r\n                    (AttributeTypeID IN (15,17,19,28))\r\n            ORDER BY AttributeID";
	}
}
