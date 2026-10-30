using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Database;

public class CSLibraryDAL : IDisposable
{
	public SQLiteConnection Connection;

	private bool _disposed;

	public static string NameOfConnectionString => "CounterSketchLibraryDataEntities1";

	public CSLibraryDAL()
	{
		string connectionString = $"Data Source={UIHelper.CSLibraryDatabasePath};BinaryGUID=No";
		Connection = new SQLiteConnection(connectionString);
		Connection.Open();
	}

	public void Dispose()
	{
		if (Connection != null && Connection.State == ConnectionState.Open)
		{
			Connection.Close();
		}
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public void GetDesignInfoFromSPINumber(string spiNumber, out int designId, out int version)
	{
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT DesignId,Version FROM Designs WHERE  StullerNo = @SPINumber", Connection))
		{
			sQLiteCommand.Parameters.Add(new SQLiteParameter("@SPINumber", spiNumber));
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			if (sQLiteDataReader.Read())
			{
				designId = int.Parse(sQLiteDataReader["DesignId"].ToString());
				version = int.Parse(sQLiteDataReader["Version"].ToString());
				return;
			}
		}
		designId = 0;
		version = 0;
	}

	public List<int> GetFeaturedIterationIndexesForDesignId(long designId)
	{
		List<int> list = new List<int>();
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand("\r\n                SELECT DISTINCT IterationIndex\r\n                FROM DesignIterationRenders\r\n                WHERE  DesignIterationRenders.DesignId = @DesignId ", Connection))
		{
			sQLiteCommand.Parameters.Add(new SQLiteParameter("@DesignId", designId));
			using SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			while (sQLiteDataReader.HasRows && sQLiteDataReader.Read())
			{
				if (sQLiteDataReader["IterationIndex"] != null)
				{
					list.Add(int.Parse(sQLiteDataReader["IterationIndex"].ToString()));
				}
			}
		}
		return list;
	}

	public string GetLibraryVersion()
	{
		using SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT Max(Version) FROM LibraryVersion", Connection);
		try
		{
			string text = sQLiteCommand.ExecuteScalar().ToString();
			int.TryParse(text, out var result);
			if (UIHelper.CounterSketchOptions.LibraryVersion != result && result > UIHelper.CounterSketchOptions.LibraryVersion)
			{
				EventHelper.RecordEvent(EventHelper.EventTypes.Library_Updated, "OldLibrary:NewLibrary", $"{UIHelper.CounterSketchOptions.LibraryVersion}:{result}");
			}
			UIHelper.CounterSketchOptions.LibraryVersion = result;
			UIHelper.CounterSketchOptions.WriteSettings();
			return text;
		}
		catch (Exception)
		{
			return "0";
		}
	}

	public int GetModelCount()
	{
		using SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT COUNT(DesignId) FROM Designs", Connection);
		object obj = sQLiteCommand.ExecuteScalar();
		if (obj != null)
		{
			return int.Parse(obj.ToString());
		}
		return 0;
	}

	public int GetModelCountFile()
	{
		if (!Directory.Exists(UIHelper.CSModelLibraryCSGL))
		{
			return 0;
		}
		return new DirectoryInfo(UIHelper.CSModelLibraryCSGL).GetFiles("*.csgl").Length;
	}

	public void PopulateDesignSearch()
	{
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand(Connection))
		{
			sQLiteCommand.CommandText = "DELETE FROM DesignSearch;INSERT INTO DesignSearch SELECT * FROM View_DesignSearch;";
			sQLiteCommand.ExecuteNonQuery();
		}
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(NameOfConnectionString);
		List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> source = counterSketchLibraryDataEntities.Attributes.Where((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute attribute) => attribute.AttributeTypeId == 9 || attribute.AttributeTypeId == 10 || attribute.AttributeTypeId == 12 || attribute.AttributeTypeId == 13).ToList();
		foreach (DesignSearch design in (IEnumerable<DesignSearch>)counterSketchLibraryDataEntities.DesignSearches)
		{
			if (design.PrimaryDesignComponentCenterGemSetting != null)
			{
				design.DefaultSettingTypeOrder = source.First((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute x) => x.AttributeName == design.PrimaryDesignComponentCenterGemSetting).AttributeCSOrder;
			}
			else
			{
				design.DefaultSettingTypeOrder = 999L;
			}
			if (design.DefaultStyleElement != null)
			{
				design.DefaultStyleElementOrder = source.First((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute x) => x.AttributeName == design.DefaultStyleElement).AttributeCSOrder;
			}
			else
			{
				design.DefaultStyleElementOrder = 999L;
			}
			if (design.DefaultStyle != null)
			{
				design.DefaultStyleOrder = source.First((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute x) => x.AttributeName == design.DefaultStyle).AttributeCSOrder;
			}
			else
			{
				design.DefaultStyleOrder = 999L;
			}
			if (design.SecondaryStyleElement != null)
			{
				design.SecondaryStyleElementOrder = source.First((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute x) => x.AttributeName == design.SecondaryStyleElement).AttributeCSOrder;
			}
			else
			{
				design.SecondaryStyleElementOrder = 999L;
			}
		}
		counterSketchLibraryDataEntities.SaveChanges();
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			if (Connection.State == ConnectionState.Open)
			{
				Connection.Close();
			}
			Connection.Dispose();
			_disposed = true;
		}
	}

	~CSLibraryDAL()
	{
		Dispose(disposing: false);
	}
}
