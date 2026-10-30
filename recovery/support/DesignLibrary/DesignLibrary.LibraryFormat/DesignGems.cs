using System;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;

namespace DesignLibrary.LibraryFormat;

public class DesignGems
{
	public readonly int DesignId;

	public readonly int ComponentId;

	public readonly int GemPlacement;

	private readonly string _name;

	private readonly double _gemX;

	private readonly double _gemXMax;

	private readonly double _gemXMin;

	private readonly double _gemY;

	private readonly double _gemYMax;

	private readonly double _gemYMin;

	private readonly double _gemZ;

	private readonly double _fFactor;

	private readonly double _gemW;

	private readonly double _gemWMax;

	private readonly double _gemWMin;

	private readonly string _mounting;

	private readonly double _mountingVolume;

	private readonly int _flags;

	public string Shape;

	public DesignGems(SqlDataReader dr)
	{
		DesignId = Convert.ToInt32(dr["DesignId"]);
		ComponentId = Convert.ToInt32(dr["ComponentId"]);
		_name = dr["Name"].ToString();
		Shape = dr["Shape"].ToString();
		_gemX = Convert.ToDouble(dr["GemX"]);
		_gemXMax = Convert.ToDouble(dr["GemXMax"]);
		_gemXMin = Convert.ToDouble(dr["GemXMin"]);
		_gemY = Convert.ToDouble(dr["GemY"]);
		_gemYMax = Convert.ToDouble(dr["GemYMax"]);
		_gemYMin = Convert.ToDouble(dr["GemYMin"]);
		_gemZ = Convert.ToDouble(dr["GemZ"]);
		_fFactor = Convert.ToDouble(dr["FFactor"]);
		_gemW = Convert.ToDouble(dr["GemW"]);
		_gemWMax = Convert.ToDouble(dr["GemWMax"]);
		_gemWMin = Convert.ToDouble(dr["GemWMin"]);
		_mounting = dr["Mounting"].ToString();
		_mountingVolume = Convert.ToDouble(dr["MountingVolume"]);
		_flags = Convert.ToInt32(dr["Flags"]);
		GemPlacement = Convert.ToInt32(dr["GemPlacement"]);
	}

	public DesignGems(BinaryReader br)
	{
		DesignId = br.ReadInt32();
		ComponentId = br.ReadInt32();
		_name = br.ReadString();
		Shape = br.ReadString();
		_gemX = br.ReadDouble();
		_gemXMax = br.ReadDouble();
		_gemXMin = br.ReadDouble();
		_gemY = br.ReadDouble();
		_gemYMax = br.ReadDouble();
		_gemYMin = br.ReadDouble();
		_gemZ = br.ReadDouble();
		_fFactor = br.ReadDouble();
		_gemW = br.ReadDouble();
		_gemWMax = br.ReadDouble();
		_gemWMin = br.ReadDouble();
		_mounting = br.ReadString();
		_mountingVolume = br.ReadDouble();
		_flags = br.ReadInt32();
		GemPlacement = br.ReadInt32();
	}

	public void BinaryWrite(BinaryWriter bw)
	{
		bw.Write(DesignId);
		bw.Write(ComponentId);
		bw.Write(_name);
		bw.Write(Shape);
		bw.Write(_gemX);
		bw.Write(_gemXMax);
		bw.Write(_gemXMin);
		bw.Write(_gemY);
		bw.Write(_gemYMax);
		bw.Write(_gemYMin);
		bw.Write(_gemZ);
		bw.Write(_fFactor);
		bw.Write(_gemW);
		bw.Write(_gemWMax);
		bw.Write(_gemWMin);
		bw.Write(_mounting);
		bw.Write(_mountingVolume);
		bw.Write(_flags);
		bw.Write(GemPlacement);
	}

	public static SQLiteCommand SqlCreate()
	{
		return new SQLiteCommand("\r\n                    CREATE TABLE IF NOT EXISTS [DesignGems] (\r\n\t                    [DesignGemId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,\r\n\t                    [DesignId] INTEGER NOT NULL,\r\n\t                    [ComponentId] INTEGER,\r\n\t                    [Name] NVARCHAR(100),\r\n\t                    [Shape] NVARCHAR(50),\r\n\t                    [GemX] FLOAT,\r\n\t                    [GemXMax] FLOAT,\r\n\t                    [GemXMin] FLOAT,\r\n\t                    [GemY] FLOAT,\r\n\t                    [GemYMax] FLOAT,\r\n\t                    [GemYMin] FLOAT,\r\n\t                    [GemZ] FLOAT,\r\n\t                    [FFactor] FLOAT,\r\n\t                    [GemW] FLOAT,\r\n\t                    [GemWMax] FLOAT,\r\n\t                    [GemWMin] FLOAT,\r\n\t                    [Mounting] NVARCHAR(50),\r\n\t                    [MountingVolume] FLOAT,\r\n\t                    [Flags] INTEGER,\r\n\t                    [GemPlacement] INTEGER,\r\n    \t            FOREIGN KEY ([DesignId]) REFERENCES [Designs] ([DesignId]),\r\n    \t            FOREIGN KEY ([ComponentId]) REFERENCES [Component] ([ComponentId])\r\n                    );\r\n\r\n                    DELETE FROM [DesignGems]");
	}

	public static string SqlIndex()
	{
		return "\r\n                CREATE INDEX IF NOT EXISTS [IDX_DesignGems_DesignId] ON [DesignGems](\r\n                [DesignId]  ASC\r\n                )";
	}

	public static string[] SqlInsert()
	{
		return new string[20]
		{
			"DesignGems", "DesignId", "ComponentId", "Name", "Shape", "GemX", "GemXMax", "GemXMin", "GemY", "GemYMax",
			"GemYMin", "GemZ", "FFactor", "GemW", "GemWMax", "GemWMin", "Mounting", "MountingVolume", "Flags", "GemPlacement"
		};
	}

	public object[] SqlParams()
	{
		return new object[19]
		{
			DesignId, ComponentId, _name, Shape, _gemX, _gemXMax, _gemXMin, _gemY, _gemYMax, _gemYMin,
			_gemZ, _fFactor, _gemW, _gemWMax, _gemWMin, _mounting, _mountingVolume, _flags, GemPlacement
		};
	}

	public static string SqlSelect()
	{
		return "\r\n             SELECT \r\n\t            (SELECT DesignId FROM DesignVersions WHERE DesignVersions.DesignVersionId=DesignVersionComponents.DesignVersionId) as DesignId,\r\n\t            ComponentId,\r\n\t            Name,\r\n\t            Shape,\r\n\t            GemX,\tGemXMax,\tGemXMin,\r\n\t            GemY,\tGemYMax,\tGemYMin,\r\n\t            GemZ,\r\n\t            FFactor,\r\n\t            GemW,\tGemWMax,\tGemWMin,\r\n                Mounting, MountingVolume,\r\n                Flags,\r\n\t            GemPlacement\r\n\r\n            FROM \r\n                DesignVersionComponentGems INNER JOIN DesignVersionComponents ON DesignVersionComponentGems.DesignVersionComponentId=DesignVersionComponents.DesignVersionComponentId \r\n            WHERE \r\n                {1} AND GemPlacement<3\r\n            ";
	}
}
