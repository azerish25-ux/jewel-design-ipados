using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Properties;
using GV.CounterSketch.Studio.Utilities.Helpers;
using Localization;

namespace GV.CounterSketch.Studio.Database;

public static class CSDatabaseUpdateDAL
{
	public static void ApplicationDatabaseSchemaUpdate(string thisAppVersion)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		string commandText;
		switch (thisAppVersion)
		{
		case "10":
			commandText = Resources.CSApplicationVersionX_15;
			break;
		case "11":
			commandText = Resources.CSApplicationVersionX_15;
			break;
		case "12":
			commandText = Resources.CSApplicationVersionX_15;
			break;
		case "15":
			commandText = Resources.CSApplicationVersion15_16;
			break;
		default:
			commandText = Resources.CSApplicationDatabaseSQL;
			if (File.Exists(UIHelper.CSApplicationDatabasePath))
			{
				File.SetAttributes(UIHelper.CSApplicationDatabasePath, FileAttributes.Normal);
				File.Delete(UIHelper.CSApplicationDatabasePath);
			}
			break;
		}
		SQLiteConnection val = new SQLiteConnection($"Data Source={UIHelper.CSApplicationDatabasePath}");
		try
		{
			((DbConnection)(object)val).Open();
			SQLiteCommand val2 = val.CreateCommand();
			try
			{
				((DbCommand)(object)val2).CommandText = commandText;
				((DbCommand)(object)val2).ExecuteNonQuery();
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			if (((DbConnection)(object)val).State == ConnectionState.Open)
			{
				((DbConnection)(object)val).Close();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		UIHelper.RaiseApplicationMessageEvent("ApplicationDatabaseSchemaUpdate", ApplicationMessageType.Debug, Strings.ANewCounterSketchApplicationDBCreated + ".\n", messageIsModal: false);
	}
}
