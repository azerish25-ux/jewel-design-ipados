using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GV.CounterSketch.Studio.Database;

public static class DBHelper
{
	public class TableClass
	{
		private Dictionary<Type, string> dataMapper => new Dictionary<Type, string>
		{
			{
				typeof(int),
				"BIGINT"
			},
			{
				typeof(string),
				"NVARCHAR(500)"
			},
			{
				typeof(bool),
				"BIT"
			},
			{
				typeof(DateTime),
				"DATETIME"
			},
			{
				typeof(float),
				"FLOAT"
			},
			{
				typeof(decimal),
				"DECIMAL(18,0)"
			},
			{
				typeof(Guid),
				"UNIQUEIDENTIFIER"
			}
		};

		public List<KeyValuePair<string, Type>> Fields { get; set; } = new List<KeyValuePair<string, Type>>();

		public string ClassName { get; set; } = string.Empty;

		public TableClass(Type t)
		{
			ClassName = t.Name;
			PropertyInfo[] properties = t.GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				KeyValuePair<string, Type> item = new KeyValuePair<string, Type>(propertyInfo.Name, propertyInfo.PropertyType);
				Fields.Add(item);
			}
		}

		public string CreateTableScript()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("CREATE TABLE " + ClassName);
			stringBuilder.AppendLine("(");
			stringBuilder.AppendLine("\t ID BIGINT,");
			for (int i = 0; i < Fields.Count; i++)
			{
				KeyValuePair<string, Type> keyValuePair = Fields[i];
				if (dataMapper.ContainsKey(keyValuePair.Value))
				{
					stringBuilder.Append("\t " + keyValuePair.Key + " " + dataMapper[keyValuePair.Value]);
				}
				else
				{
					stringBuilder.Append("\t " + keyValuePair.Key + " BIGINT");
				}
				if (i != Fields.Count - 1)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(Environment.NewLine);
			}
			stringBuilder.AppendLine(")");
			return stringBuilder.ToString();
		}
	}

	public static void PrintClassTableSQL(Type t)
	{
		new TableClass(t);
	}
}
