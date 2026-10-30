using System;
using System.Data.Common;
using System.Data.SQLite;

namespace DesignLibrary;

public static class GemAttribFix
{
	public static void Fix(SQLiteConnection sqlc)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		((DbCommand)new SQLiteCommand("BEGIN", sqlc)).ExecuteNonQuery();
		SQLiteCommand val = new SQLiteCommand("SELECT DesignId,ComponentId,Shape,GemPlacement FROM DesignGems WHERE GemPlacement IN (1,2)", sqlc);
		SQLiteCommand val2 = new SQLiteCommand("INSERT INTO DesignAttributes (DesignId,ComponentId,AttributeId) VALUES (@DesignId,@ComponentId,@AttributeId)", sqlc);
		val2.Parameters.Add(new SQLiteParameter("DesignId"));
		val2.Parameters.Add(new SQLiteParameter("ComponentId"));
		val2.Parameters.Add(new SQLiteParameter("AttributeId"));
		SQLiteDataReader val3 = val.ExecuteReader();
		while (((DbDataReader)(object)val3).Read())
		{
			int num = Convert.ToInt32(((DbDataReader)(object)val3)["DesignId"]);
			int num2 = Convert.ToInt32(((DbDataReader)(object)val3)["ComponentId"]);
			string attName = ((DbDataReader)(object)val3)["Shape"].ToString();
			int num3 = Convert.ToInt32(((DbDataReader)(object)val3)["GemPlacement"]);
			if (num3 == 1)
			{
				int num4 = GemAttributeIdCenter(attName);
				if (num4 != 0)
				{
					((DbParameter)(object)val2.Parameters["DesignId"]).Value = num;
					((DbParameter)(object)val2.Parameters["ComponentId"]).Value = num2;
					((DbParameter)(object)val2.Parameters["AttributeId"]).Value = num4;
					((DbCommand)(object)val2).ExecuteNonQuery();
				}
			}
			if (num3 == 2)
			{
				int num5 = GemAttributeIdSide(attName);
				if (num5 != 0)
				{
					((DbParameter)(object)val2.Parameters["DesignId"]).Value = num;
					((DbParameter)(object)val2.Parameters["ComponentId"]).Value = num2;
					((DbParameter)(object)val2.Parameters["AttributeId"]).Value = num5;
					((DbCommand)(object)val2).ExecuteNonQuery();
				}
			}
		}
		val3 = new SQLiteCommand("select distinct DesignId,GemTableShapeId,GemPlacementId from DesignGemTables INNER JOIN GemTableShapeSize ON DesignGemTables.GemTableShapeSizeId=GemTableShapeSize.GemTableShapeSizeId", sqlc).ExecuteReader();
		while (((DbDataReader)(object)val3).Read())
		{
			int num6 = Convert.ToInt32(((DbDataReader)(object)val3)["DesignId"]);
			string attName2 = ((DbDataReader)(object)val3)["GemTableShapeId"].ToString();
			int num7 = Convert.ToInt32(((DbDataReader)(object)val3)["GemPlacementId"]);
			if (num7 == 1)
			{
				int num8 = GemAttributeIdCenter(attName2);
				if (num8 != 0)
				{
					((DbParameter)(object)val2.Parameters["DesignId"]).Value = num6;
					((DbParameter)(object)val2.Parameters["ComponentId"]).Value = 0;
					((DbParameter)(object)val2.Parameters["AttributeId"]).Value = num8;
					((DbCommand)(object)val2).ExecuteNonQuery();
				}
			}
			if (num7 == 2)
			{
				int num9 = GemAttributeIdSide(attName2);
				if (num9 != 0)
				{
					((DbParameter)(object)val2.Parameters["DesignId"]).Value = num6;
					((DbParameter)(object)val2.Parameters["ComponentId"]).Value = 0;
					((DbParameter)(object)val2.Parameters["AttributeId"]).Value = num9;
					((DbCommand)(object)val2).ExecuteNonQuery();
				}
			}
		}
		((DbCommand)new SQLiteCommand("COMMIT", sqlc)).ExecuteNonQuery();
		((DbCommand)new SQLiteCommand("delete from attribute where (select count(1) from designattributes where designattributes.attributeid=attribute.AttributeId)=0", sqlc)).ExecuteNonQuery();
	}

	private static int GemAttributeIdCenter(string attName)
	{
		switch (attName.ToLower())
		{
		case "1":
		case "baguette straight":
			return 23;
		case "29":
		case "baguette tapered":
			return 23;
		case "2":
		case "bullet":
			return 0;
		case "3":
		case "calf":
			return 24;
		case "4":
		case "cushion rotated 90":
			return 25;
		case "5":
		case "cushion square":
			return 279;
		case "6":
		case "cushion":
			return 25;
		case "7":
		case "emerald rotated 90":
			return 26;
		case "8":
		case "emerald square":
			return 26;
		case "9":
		case "emerald":
			return 26;
		case "10":
		case "half moon":
			return 28;
		case "11":
		case "heart":
			return 29;
		case "12":
		case "marquise rotated 90":
			return 30;
		case "13":
		case "marquise":
			return 30;
		case "14":
		case "octagon":
			return 0;
		case "15":
		case "oval rotated 90":
			return 31;
		case "16":
		case "oval":
			return 31;
		case "17":
		case "pear":
			return 32;
		case "18":
		case "princess":
			return 33;
		case "19":
		case "radiant square":
			return 81;
		case "20":
		case "radiant":
			return 81;
		case "21":
		case "round":
			return 82;
		case "22":
		case "square":
			return 33;
		case "23":
		case "triangle":
			return 83;
		case "24":
		case "trillion":
			return 83;
		case "25":
		case "trillion curved":
			return 83;
		case "26":
		case "trillion curved 2":
			return 83;
		case "27":
		case "trillion straight":
			return 83;
		case "28":
		case "trillion straignt 2":
			return 83;
		default:
			return 0;
		}
	}

	private static int GemAttributeIdSide(string attName)
	{
		switch (attName.ToLower())
		{
		case "1":
		case "baguette straight":
			return 86;
		case "29":
		case "baguette tapered":
			return 86;
		case "2":
		case "bullet":
			return 0;
		case "3":
		case "calf":
			return 87;
		case "4":
		case "cushion rotated 90":
			return 88;
		case "5":
		case "cushion square":
			return 280;
		case "6":
		case "cushion":
			return 88;
		case "7":
		case "emerald rotated 90":
			return 89;
		case "8":
		case "emerald square":
			return 89;
		case "9":
		case "emerald":
			return 89;
		case "10":
		case "half moon":
			return 91;
		case "11":
		case "heart":
			return 92;
		case "12":
		case "marquise rotated 90":
			return 93;
		case "13":
		case "marquise":
			return 93;
		case "14":
		case "octagon":
			return 0;
		case "15":
		case "oval rotated 90":
			return 94;
		case "16":
		case "oval":
			return 94;
		case "17":
		case "pear":
			return 95;
		case "18":
		case "princess":
			return 96;
		case "19":
		case "radiant square":
			return 97;
		case "20":
		case "radiant":
			return 97;
		case "21":
		case "round":
			return 98;
		case "22":
		case "square":
			return 96;
		case "23":
		case "triangle":
			return 99;
		case "24":
		case "trillion":
			return 99;
		case "25":
		case "trillion curved":
			return 99;
		case "26":
		case "trillion curved 2":
			return 99;
		case "27":
		case "trillion straight":
			return 99;
		case "28":
		case "trillion straignt 2":
			return 99;
		default:
			return 0;
		}
	}
}
