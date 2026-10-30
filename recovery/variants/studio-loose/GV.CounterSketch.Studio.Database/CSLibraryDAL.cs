using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
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
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		string text = $"Data Source={UIHelper.CSLibraryDatabasePath};BinaryGUID=No";
		Connection = new SQLiteConnection(text);
		((DbConnection)(object)Connection).Open();
	}

	public void Dispose()
	{
		if (Connection != null && ((DbConnection)(object)Connection).State == ConnectionState.Open)
		{
			((DbConnection)(object)Connection).Close();
		}
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public void GetDesignInfoFromSPINumber(string spiNumber, out int designId, out int version)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		SQLiteCommand val = new SQLiteCommand("SELECT DesignId,Version FROM Designs WHERE  StullerNo = @SPINumber", Connection);
		try
		{
			val.Parameters.Add(new SQLiteParameter("@SPINumber", (object)spiNumber));
			SQLiteDataReader val2 = val.ExecuteReader();
			try
			{
				if (((DbDataReader)(object)val2).Read())
				{
					designId = int.Parse(((DbDataReader)(object)val2)["DesignId"].ToString());
					version = int.Parse(((DbDataReader)(object)val2)["Version"].ToString());
					return;
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		designId = 0;
		version = 0;
	}

	public List<int> GetFeaturedIterationIndexesForDesignId(long designId)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		List<int> list = new List<int>();
		SQLiteCommand val = new SQLiteCommand("\r\n                SELECT DISTINCT IterationIndex\r\n                FROM DesignIterationRenders\r\n                WHERE  DesignIterationRenders.DesignId = @DesignId ", Connection);
		try
		{
			val.Parameters.Add(new SQLiteParameter("@DesignId", (object)designId));
			SQLiteDataReader val2 = val.ExecuteReader();
			try
			{
				while (((DbDataReader)(object)val2).HasRows && ((DbDataReader)(object)val2).Read())
				{
					if (((DbDataReader)(object)val2)["IterationIndex"] != null)
					{
						list.Add(int.Parse(((DbDataReader)(object)val2)["IterationIndex"].ToString()));
					}
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return list;
	}

	public string GetLibraryVersion()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		SQLiteCommand val = new SQLiteCommand("SELECT Max(Version) FROM LibraryVersion", Connection);
		try
		{
			string text = ((DbCommand)(object)val).ExecuteScalar().ToString();
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
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public int GetModelCount()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		SQLiteCommand val = new SQLiteCommand("SELECT COUNT(DesignId) FROM Designs", Connection);
		try
		{
			object obj = ((DbCommand)(object)val).ExecuteScalar();
			if (obj != null)
			{
				return int.Parse(obj.ToString());
			}
			return 0;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
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
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		SQLiteCommand val = new SQLiteCommand(Connection);
		try
		{
			((DbCommand)(object)val).CommandText = "DELETE FROM DesignSearch;INSERT INTO DesignSearch SELECT * FROM View_DesignSearch;";
			((DbCommand)(object)val).ExecuteNonQuery();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(NameOfConnectionString);
		try
		{
			List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> source = ((IQueryable<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute>)counterSketchLibraryDataEntities.Attributes).Where((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute attribute) => attribute.AttributeTypeId == 9 || attribute.AttributeTypeId == 10 || attribute.AttributeTypeId == 12 || attribute.AttributeTypeId == 13).ToList();
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
			((DbContext)counterSketchLibraryDataEntities).SaveChanges();
		}
		finally
		{
			((IDisposable)counterSketchLibraryDataEntities)?.Dispose();
		}
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			if (((DbConnection)(object)Connection).State == ConnectionState.Open)
			{
				((DbConnection)(object)Connection).Close();
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
