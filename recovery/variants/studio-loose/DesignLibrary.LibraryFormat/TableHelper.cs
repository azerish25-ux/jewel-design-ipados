using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace DesignLibrary.LibraryFormat;

public static class TableHelper
{
	public static List<T> ReadFile<T>(BinaryReader br)
	{
		List<T> list = new List<T>();
		ConstructorInfo constructor = typeof(T).GetConstructor(new Type[1] { typeof(BinaryReader) });
		if (constructor == null)
		{
			return null;
		}
		object[] parameters = new object[1] { br };
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			list.Add((T)constructor.Invoke(parameters));
		}
		return list;
	}

	public static List<T> ReadFromSql<T>(SqlConnection conn, int LibraryVersionId)
	{
		List<T> list = new List<T>();
		Type typeFromHandle = typeof(T);
		SqlCommand sqlCommand = new SqlCommand(string.Format(typeFromHandle.GetMethod("SqlSelect").Invoke(null, null).ToString(), sqlDesign(LibraryVersionId), sqlDesignVersion(LibraryVersionId), sqlDesignVersionTable(LibraryVersionId), sqlDesignVersionInLibrary(LibraryVersionId)), conn);
		ConstructorInfo constructor = typeFromHandle.GetConstructor(new Type[1] { typeof(SqlDataReader) });
		if (constructor == null)
		{
			return null;
		}
		using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
		object[] parameters = new object[1] { sqlDataReader };
		while (sqlDataReader.Read())
		{
			list.Add((T)constructor.Invoke(parameters));
		}
		return list;
	}

	public static void WriteList<T>(BinaryWriter bw, List<T> outList)
	{
		MethodInfo method = typeof(T).GetMethod("BinaryWrite");
		int count = outList.Count;
		bw.Write(count);
		object[] parameters = new object[1] { bw };
		for (int i = 0; i < count; i++)
		{
			method.Invoke(outList[i], parameters);
		}
	}

	public static void Writesql<T>(SQLiteConnection sqlc, List<T> inList)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		Type typeFromHandle = typeof(T);
		SQLiteCommand val = (SQLiteCommand)typeFromHandle.GetMethod("SqlCreate").Invoke(null, null);
		val.Connection = sqlc;
		((DbCommand)val).ExecuteNonQuery();
		SQLiteCommand val2 = BuildInsert((string[])typeFromHandle.GetMethod("SqlInsert").Invoke(null, null));
		val2.Connection = sqlc;
		MethodInfo method = typeFromHandle.GetMethod("SqlParams");
		((DbCommand)new SQLiteCommand("BEGIN", sqlc)).ExecuteNonQuery();
		int count = inList.Count;
		for (int i = 0; i < count; i++)
		{
			object[] array = (object[])method.Invoke(inList[i], null);
			for (int j = 0; j < array.Length; j++)
			{
				((DbParameter)(object)val2.Parameters[j]).Value = array[j];
			}
			((DbCommand)(object)val2).ExecuteNonQuery();
		}
		MethodInfo method2 = typeFromHandle.GetMethod("SqlIndex");
		if (method2 != null)
		{
			((DbCommand)new SQLiteCommand((string)method2.Invoke(null, null), sqlc)).ExecuteNonQuery();
		}
		((DbCommand)new SQLiteCommand("COMMIT", sqlc)).ExecuteNonQuery();
	}

	private static SQLiteCommand BuildInsert(string[] values)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		string text = "INSERT INTO " + values[0] + " ( " + values[1];
		for (int i = 2; i < values.Length; i++)
		{
			text = text + "," + values[i];
		}
		text = text + ") VALUES (@" + values[1];
		for (int j = 2; j < values.Length; j++)
		{
			text = text + ",@" + values[j];
		}
		text += ")";
		SQLiteCommand val = new SQLiteCommand(text);
		for (int k = 1; k < values.Length; k++)
		{
			val.Parameters.Add(new SQLiteParameter(values[k]));
		}
		return val;
	}

	private static string sqlDesign(int LibraryVersionId)
	{
		return " DesignId IN (SELECT LibraryDesigns.DesignId FROM LibraryDesigns WHERE LibraryId=" + LibraryVersionId + ") ";
	}

	private static string sqlDesignVersion(int LibraryVersionId)
	{
		return " DesignVersionId IN (SELECT LibraryDesigns.DesignVersionId FROM LibraryDesigns WHERE LibraryId=" + LibraryVersionId + ") ";
	}

	private static string sqlDesignVersionTable(int LibraryVersionId)
	{
		return " DesignVersions.DesignVersionId IN (SELECT LibraryDesigns.DesignVersionId FROM LibraryDesigns WHERE LibraryId=" + LibraryVersionId + ") ";
	}

	private static string sqlDesignVersionInLibrary(int LibraryVersionId)
	{
		return " DesignVersionInNewLibrary IN (SELECT LibraryDesigns.DesignVersionId FROM LibraryDesigns WHERE LibraryId=" + LibraryVersionId + ") ";
	}
}
