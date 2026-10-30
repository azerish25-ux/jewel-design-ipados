using System.Data;
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
		using (SQLiteConnection sQLiteConnection = new SQLiteConnection($"Data Source={UIHelper.CSApplicationDatabasePath}"))
		{
			sQLiteConnection.Open();
			using (SQLiteCommand sQLiteCommand = sQLiteConnection.CreateCommand())
			{
				sQLiteCommand.CommandText = commandText;
				sQLiteCommand.ExecuteNonQuery();
			}
			if (sQLiteConnection.State == ConnectionState.Open)
			{
				sQLiteConnection.Close();
			}
		}
		UIHelper.RaiseApplicationMessageEvent("ApplicationDatabaseSchemaUpdate", ApplicationMessageType.Debug, Strings.ANewCounterSketchApplicationDBCreated + ".\n", messageIsModal: false);
	}
}
