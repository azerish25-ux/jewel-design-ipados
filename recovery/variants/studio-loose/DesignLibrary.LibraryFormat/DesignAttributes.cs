using System;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;

namespace DesignLibrary.LibraryFormat;

public class DesignAttributes
{
	private readonly int _designId;

	private readonly int _componentId;

	private readonly int _attributeId;

	private readonly string _attributeValue;

	public DesignAttributes(SqlDataReader dr)
	{
		_designId = Convert.ToInt32(dr["DesignId"]);
		_componentId = Convert.ToInt32(dr["ComponentId"]);
		_attributeId = Convert.ToInt32(dr["AttributeId"]);
		_attributeValue = dr["AttributeValue"].ToString();
	}

	public DesignAttributes(BinaryReader br)
	{
		_designId = br.ReadInt32();
		_componentId = br.ReadInt32();
		_attributeId = br.ReadInt32();
		_attributeValue = br.ReadString();
	}

	public DesignAttributes(int designId, int componentId, int attributeId)
	{
		_designId = designId;
		_componentId = componentId;
		_attributeId = attributeId;
		_attributeValue = string.Empty;
	}

	public void BinaryWrite(BinaryWriter bw)
	{
		bw.Write(_designId);
		bw.Write(_componentId);
		bw.Write(_attributeId);
		bw.Write(_attributeValue);
	}

	public static SQLiteCommand SqlCreate()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		return new SQLiteCommand("\r\n                CREATE TABLE IF NOT EXISTS [DesignAttributes] (\r\n                \t[DesignAttributeId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,\r\n                    [DesignId] INTEGER NOT NULL,\r\n                    [ComponentId] INTEGER,\r\n                    [AttributeId] INTEGER,\r\n                    [AttributeValue] VARCHAR(50),\r\n\t            FOREIGN KEY ([DesignId]) REFERENCES [Designs] ([DesignId]),\r\n\t            FOREIGN KEY ([ComponentId]) REFERENCES [Component] ([ComponentId]),\r\n\t            FOREIGN KEY ([AttributeId]) REFERENCES [Attribute] ([AttributeId])\r\n                );\r\n\r\n                DELETE FROM [DesignAttributes];");
	}

	public static string SqlIndex()
	{
		return "\r\n                CREATE INDEX IF NOT EXISTS [IDX_DesignAttributes_DesignId] ON [DesignAttributes](\r\n                [DesignId]  ASC\r\n                )";
	}

	public static string[] SqlInsert()
	{
		return new string[5] { "DesignAttributes", "DesignId", "ComponentId", "AttributeId", "AttributeValue" };
	}

	public object[] SqlParams()
	{
		return new object[4] { _designId, _componentId, _attributeId, _attributeValue };
	}

	public static string SqlSelect()
	{
		return "\r\n                SELECT \r\n                    DesignId,\r\n                    0 as ComponentId,\r\n                    AttributeId,\r\n                    AttributeValue \r\n                FROM DesignAttribute WHERE \r\n                {0}\r\n\r\n                UNION\r\n\r\n                SELECT        \r\n\t                DesignId,\r\n                    0 as ComponentId,\r\n\t                AttributeId,\r\n\t                AttributeValue\r\n                FROM \r\n\t                DesignVersions INNER JOIN\r\n                    DesignVersionAttribute ON DesignVersions.DesignVersionId = DesignVersionAttribute.DesignVersionId WHERE\r\n                {2}\r\n\r\n                UNION\r\n\r\n                SELECT        \r\n\t                DesignVersions.DesignId,\r\n\t                DesignVersionComponents.ComponentId,\r\n\t                DesignVersionComponentAttribute.AttributeId,\r\n\t                DesignVersionComponentAttribute.AttributeValue\r\n                FROM \r\n\t                DesignVersions INNER JOIN\r\n                    DesignVersionComponents ON DesignVersions.DesignVersionId = DesignVersionComponents.DesignVersionId INNER JOIN\r\n                    DesignVersionComponentAttribute ON DesignVersionComponents.DesignVersionComponentId = DesignVersionComponentAttribute.DesignVersionComponentId WHERE\r\n                {2}";
	}
}
