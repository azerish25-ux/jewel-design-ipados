using System;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;

namespace DesignLibrary.LibraryFormat;

public class Designs
{
	public readonly int DesignId;

	public readonly int Version;

	private readonly int _designVersionId;

	private readonly string _stullerNo;

	private readonly string _seriesNumber;

	private readonly string _name;

	private readonly string _description;

	private readonly string _designer;

	private readonly int _designTypeId;

	private readonly DateTime _creationDate;

	private readonly DateTime _lastUpdateDate;

	private readonly int _firstReleaseLibraryId;

	public Designs(SqlDataReader dr)
	{
		DesignId = Convert.ToInt32(dr["DesignId"]);
		Version = Convert.ToInt32(dr["Version"]);
		_designVersionId = Convert.ToInt32(dr["DesignVersionId"]);
		_stullerNo = dr["CsNo"].ToString();
		_seriesNumber = dr["SeriesNumber"].ToString();
		_name = dr["Name"].ToString();
		_description = dr["Description"].ToString();
		_designer = dr["CSDisplayName"].ToString();
		_designTypeId = Convert.ToInt32(dr["DesignTypeID"]);
		_creationDate = Convert.ToDateTime(dr["SubmittedTimeStamp"]);
		_lastUpdateDate = Convert.ToDateTime(dr["FileTimeStamp"]);
		_firstReleaseLibraryId = Convert.ToInt32(dr["FirstReleaseLibraryId"]);
	}

	public Designs(BinaryReader br)
	{
		DesignId = br.ReadInt32();
		Version = br.ReadInt32();
		_designVersionId = br.ReadInt32();
		_stullerNo = br.ReadString();
		_seriesNumber = br.ReadString();
		_name = br.ReadString();
		_description = br.ReadString();
		_designer = br.ReadString();
		_designTypeId = br.ReadInt32();
		_creationDate = new DateTime(br.ReadInt64());
		_lastUpdateDate = new DateTime(br.ReadInt64());
		_firstReleaseLibraryId = br.ReadInt32();
	}

	public void BinaryWrite(BinaryWriter bw)
	{
		bw.Write(DesignId);
		bw.Write(Version);
		bw.Write(_designVersionId);
		bw.Write(_stullerNo);
		bw.Write(_seriesNumber);
		bw.Write(_name);
		bw.Write(_description);
		bw.Write(_designer);
		bw.Write(_designTypeId);
		bw.Write(_creationDate.Ticks);
		bw.Write(_lastUpdateDate.Ticks);
		bw.Write(_firstReleaseLibraryId);
	}

	public static SQLiteCommand SqlCreate()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		return new SQLiteCommand("\r\n                CREATE TABLE IF NOT EXISTS [Designs] (\r\n                    [DesignId] INTEGER PRIMARY KEY NOT NULL,\r\n                    [Version] INTEGER NOT NULL,\r\n                    [DesignVersionId] INTEGER NOT NULL,\r\n                    [StullerNo] VARCHAR(30),\r\n                    [SeriesNumber] VARCHAR(30),\r\n                    [Name] VARCHAR(255),\r\n                    [Description] VARCHAR(255),\r\n                    [Designer] VARCHAR(50),\r\n                    [DesignTypeId] INTEGER,\r\n                    [CreationDate] DATETIME,\r\n                    [LastUpdateDate] DATETIME,\r\n                    [FirstReleaseLibraryId] INTEGER,\r\n                    [CSGLFound] INTEGER DEFAULT 0);\r\n                DELETE FROM [Designs];");
	}

	public static string[] SqlInsert()
	{
		return new string[13]
		{
			"Designs", "DesignId", "Version", "DesignVersionId", "StullerNo", "SeriesNumber", "Name", "Description", "Designer", "DesignTypeId",
			"CreationDate", "LastUpdateDate", "FirstReleaseLibraryId"
		};
	}

	public object[] SqlParams()
	{
		return new object[12]
		{
			DesignId, Version, _designVersionId, _stullerNo, _seriesNumber, _name, _description, _designer, _designTypeId, _creationDate,
			_lastUpdateDate, _firstReleaseLibraryId
		};
	}

	public static string SqlSelect()
	{
		return "\r\n                select \r\n\t                DesignId,\r\n\t                (select version from designversions where designversions.designversionid=designs.DesignVersionInNewLibrary) as Version,\r\n\t                DesignVersionInNewLibrary as DesignVersionId,\r\n\t                CsNo,\r\n\t                SeriesNumber,\r\n\t                Name,\r\n\t                Description,\r\n\t                (select CSDisplayName from designers where designs.DesignerId=Designers.DesignerId) as CSDisplayName,\r\n\t                DesignTypeId,\r\n\t                (select SubmittedTimeStamp from designversions where designversions.designversionid=designs.DesignVersionInNewLibrary) as SubmittedTimeStamp,\r\n\t                (select CleanedFiletimeStamp from designversions where designversions.designversionid=designs.DesignVersionInNewLibrary) as FileTimeStamp,\r\n                    FirstReleaseLibraryId\r\n\r\n                 FROM \r\n                    designs\r\n                 WHERE \r\n                    {3}\r\n                 ORDER BY\r\n                    designid";
	}
}
