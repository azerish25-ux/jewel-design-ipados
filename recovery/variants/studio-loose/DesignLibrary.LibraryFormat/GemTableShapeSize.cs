using System;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;

namespace DesignLibrary.LibraryFormat;

internal class GemTableShapeSize
{
	private readonly int _gemTableShapeSizeId;

	private readonly int _gemTableTypeId;

	private readonly int _gemTableShapeId;

	private readonly double _carat;

	private readonly double _width;

	private readonly double _height;

	private readonly double _depth;

	public GemTableShapeSize(SqlDataReader dr)
	{
		_gemTableShapeSizeId = Convert.ToInt32(dr["GemTableShapeSizeId"]);
		_gemTableTypeId = Convert.ToInt32(dr["GemTableTypeId"]);
		_gemTableShapeId = Convert.ToInt32(dr["GemTableShapeId"]);
		_carat = Convert.ToDouble(dr["Carat"]);
		_width = Convert.ToDouble(dr["Width"]);
		_height = Convert.ToDouble(dr["Height"]);
		_depth = Convert.ToDouble(dr["Depth"]);
	}

	public GemTableShapeSize(BinaryReader br)
	{
		_gemTableShapeSizeId = br.ReadInt32();
		_gemTableTypeId = br.ReadInt32();
		_gemTableShapeId = br.ReadInt32();
		_carat = br.ReadDouble();
		_width = br.ReadDouble();
		_height = br.ReadDouble();
		_depth = br.ReadDouble();
	}

	public void BinaryWrite(BinaryWriter bw)
	{
		bw.Write(_gemTableShapeSizeId);
		bw.Write(_gemTableTypeId);
		bw.Write(_gemTableShapeId);
		bw.Write(_carat);
		bw.Write(_width);
		bw.Write(_height);
		bw.Write(_depth);
	}

	public static SQLiteCommand SqlCreate()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		return new SQLiteCommand("\r\n                CREATE TABLE IF NOT EXISTS [GemTableShapeSize] (\r\n                    [GemTableShapeSizeId] INTEGER PRIMARY KEY NOT NULL,\r\n                    [GemTableTypeId] INTEGER NOT NULL,\r\n                    [GemTableShapeId] INTEGER NOT NULL,\r\n                    [Carat] NUMERIC NOT NULL,\r\n                    [Width] NUMERIC NOT NULL,\r\n                    [Height] NUMERIC NOT NULL,\r\n                    [Depth] NUMERIC NOT NULL);\r\n                DELETE FROM [GemTableShapeSize];");
	}

	public static string SqlIndex()
	{
		return "\r\n                CREATE INDEX IF NOT EXISTS [IDX_GemTableShapeSize_ShapeWidthHeight] ON [GemTableShapeSize](GemTableShapeId,Width,Height)";
	}

	public static string[] SqlInsert()
	{
		return new string[8] { "GemTableShapeSize", "GemTableShapeSizeId", "GemTableTypeId", "GemTableShapeId", "Carat", "Width", "Height", "Depth" };
	}

	public object[] SqlParams()
	{
		return new object[7] { _gemTableShapeSizeId, _gemTableTypeId, _gemTableShapeId, _carat, _width, _height, _depth };
	}

	public static string SqlSelect()
	{
		return "\r\n                SELECT\r\n                    GemTableShapeSizeId,\r\n                    GemTableTypeId,\r\n                    GemTableShapeId,\r\n                    Carat,\r\n                    Width,\r\n                    Height,\r\n                    Depth\r\n                FROM GemTableShapeSize ORDER BY GemTableShapeSizeId";
	}
}
