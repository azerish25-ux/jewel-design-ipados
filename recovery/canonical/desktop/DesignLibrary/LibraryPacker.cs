using System.Collections.Generic;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;
using DesignLibrary.LibraryFormat;
using LZMA;

namespace DesignLibrary;

public static class LibraryPacker
{
	public static void CreateSqLiteDBFromZBinFile(string zBinfilename, string dbFilename)
	{
		List<LibraryVersion> inList;
		List<AttributeLevel> inList2;
		List<Component> inList3;
		List<AttributeType> inList4;
		List<Attribute> inList5;
		List<DesignGroup> inList6;
		List<GemTableShapeSize> inList7;
		List<Designs> inList8;
		List<DesignComponents> inList9;
		List<DesignAttributes> inList10;
		List<DesignGroupDesign> inList11;
		List<DesignGemTables> inList12;
		List<DesignGems> inList13;
		if (File.Exists(zBinfilename))
		{
			using Stream stream = new MemoryStream();
			using (Stream stream2 = new FileStream(zBinfilename, FileMode.Open, FileAccess.Read))
			{
				LzmaAlone.Decode(stream2, stream);
				stream2.Close();
			}
			stream.Position = 0L;
			BinaryReader binaryReader = new BinaryReader(stream);
			binaryReader.ReadInt32();
			inList = TableHelper.ReadFile<LibraryVersion>(binaryReader);
			inList2 = TableHelper.ReadFile<AttributeLevel>(binaryReader);
			inList3 = TableHelper.ReadFile<Component>(binaryReader);
			inList4 = TableHelper.ReadFile<AttributeType>(binaryReader);
			inList5 = TableHelper.ReadFile<Attribute>(binaryReader);
			inList6 = TableHelper.ReadFile<DesignGroup>(binaryReader);
			inList7 = TableHelper.ReadFile<GemTableShapeSize>(binaryReader);
			inList8 = TableHelper.ReadFile<Designs>(binaryReader);
			inList9 = TableHelper.ReadFile<DesignComponents>(binaryReader);
			inList10 = TableHelper.ReadFile<DesignAttributes>(binaryReader);
			inList11 = TableHelper.ReadFile<DesignGroupDesign>(binaryReader);
			inList12 = TableHelper.ReadFile<DesignGemTables>(binaryReader);
			inList13 = TableHelper.ReadFile<DesignGems>(binaryReader);
		}
		else
		{
			inList = new List<LibraryVersion>();
			inList2 = new List<AttributeLevel>();
			inList3 = new List<Component>();
			inList4 = new List<AttributeType>();
			inList5 = new List<Attribute>();
			inList6 = new List<DesignGroup>();
			inList7 = new List<GemTableShapeSize>();
			inList8 = new List<Designs>();
			inList9 = new List<DesignComponents>();
			inList10 = new List<DesignAttributes>();
			inList11 = new List<DesignGroupDesign>();
			inList12 = new List<DesignGemTables>();
			inList13 = new List<DesignGems>();
		}
		using SQLiteConnection sQLiteConnection = new SQLiteConnection("data source=" + dbFilename + ";Version=3");
		sQLiteConnection.Open();
		TableHelper.Writesql(sQLiteConnection, inList);
		TableHelper.Writesql(sQLiteConnection, inList2);
		TableHelper.Writesql(sQLiteConnection, inList3);
		TableHelper.Writesql(sQLiteConnection, inList4);
		TableHelper.Writesql(sQLiteConnection, inList5);
		TableHelper.Writesql(sQLiteConnection, inList6);
		TableHelper.Writesql(sQLiteConnection, inList7);
		TableHelper.Writesql(sQLiteConnection, inList8);
		TableHelper.Writesql(sQLiteConnection, inList9);
		TableHelper.Writesql(sQLiteConnection, inList10);
		TableHelper.Writesql(sQLiteConnection, inList11);
		TableHelper.Writesql(sQLiteConnection, inList12);
		TableHelper.Writesql(sQLiteConnection, inList13);
		GemAttribFix.Fix(sQLiteConnection);
		List<DesignSearch> inList14 = new List<DesignSearch>();
		TableHelper.Writesql(sQLiteConnection, inList14);
		sQLiteConnection.Close();
	}

	public static void GetDesignsFromZBinFile(string zBinfilename, out List<Designs> lstDesigns, out int libraryId)
	{
		if (File.Exists(zBinfilename))
		{
			using (Stream stream = new MemoryStream())
			{
				using (Stream stream2 = new FileStream(zBinfilename, FileMode.Open, FileAccess.Read))
				{
					LzmaAlone.Decode(stream2, stream);
					stream2.Close();
				}
				stream.Position = 0L;
				BinaryReader binaryReader = new BinaryReader(stream);
				binaryReader.ReadInt32();
				List<LibraryVersion> list = TableHelper.ReadFile<LibraryVersion>(binaryReader);
				libraryId = ((list.Count == 1) ? list[0].Version : 0);
				TableHelper.ReadFile<AttributeLevel>(binaryReader);
				TableHelper.ReadFile<Component>(binaryReader);
				TableHelper.ReadFile<AttributeType>(binaryReader);
				TableHelper.ReadFile<Attribute>(binaryReader);
				TableHelper.ReadFile<DesignGroup>(binaryReader);
				TableHelper.ReadFile<GemTableShapeSize>(binaryReader);
				lstDesigns = TableHelper.ReadFile<Designs>(binaryReader);
				return;
			}
		}
		libraryId = 0;
		lstDesigns = new List<Designs>();
	}

	public static void MakeZBinFile(int libraryVersionId, int keyProfileId)
	{
		List<LibraryVersion> outList;
		List<AttributeLevel> outList2;
		List<Component> outList3;
		List<AttributeType> outList4;
		List<Attribute> outList5;
		List<DesignGroup> outList6;
		List<GemTableShapeSize> outList7;
		List<Designs> outList8;
		List<DesignComponents> outList9;
		List<DesignAttributes> outList10;
		List<DesignGroupDesign> outList11;
		List<DesignGemTables> outList12;
		List<DesignGems> outList13;
		using (SqlConnection sqlConnection = new SqlConnection("Network Library=DBMSSOCN;Data Source=172.22.10.28;database=CounterSketchLibrary;User id=sa;Password=Welcome1;MultipleActiveResultSets=True;"))
		{
			sqlConnection.Open();
			outList = new List<LibraryVersion>
			{
				new LibraryVersion(libraryVersionId, keyProfileId)
			};
			outList2 = TableHelper.ReadFromSql<AttributeLevel>(sqlConnection, libraryVersionId);
			outList3 = TableHelper.ReadFromSql<Component>(sqlConnection, libraryVersionId);
			outList4 = TableHelper.ReadFromSql<AttributeType>(sqlConnection, libraryVersionId);
			outList5 = TableHelper.ReadFromSql<Attribute>(sqlConnection, libraryVersionId);
			outList6 = TableHelper.ReadFromSql<DesignGroup>(sqlConnection, libraryVersionId);
			outList7 = TableHelper.ReadFromSql<GemTableShapeSize>(sqlConnection, libraryVersionId);
			outList8 = TableHelper.ReadFromSql<Designs>(sqlConnection, libraryVersionId);
			outList9 = TableHelper.ReadFromSql<DesignComponents>(sqlConnection, libraryVersionId);
			outList10 = TableHelper.ReadFromSql<DesignAttributes>(sqlConnection, libraryVersionId);
			outList11 = TableHelper.ReadFromSql<DesignGroupDesign>(sqlConnection, libraryVersionId);
			outList12 = TableHelper.ReadFromSql<DesignGemTables>(sqlConnection, libraryVersionId);
			outList13 = TableHelper.ReadFromSql<DesignGems>(sqlConnection, libraryVersionId);
			sqlConnection.Close();
		}
		using Stream stream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(stream);
		binaryWriter.Write(1);
		TableHelper.WriteList(binaryWriter, outList);
		TableHelper.WriteList(binaryWriter, outList2);
		TableHelper.WriteList(binaryWriter, outList3);
		TableHelper.WriteList(binaryWriter, outList4);
		TableHelper.WriteList(binaryWriter, outList5);
		TableHelper.WriteList(binaryWriter, outList6);
		TableHelper.WriteList(binaryWriter, outList7);
		TableHelper.WriteList(binaryWriter, outList8);
		TableHelper.WriteList(binaryWriter, outList9);
		TableHelper.WriteList(binaryWriter, outList10);
		TableHelper.WriteList(binaryWriter, outList11);
		TableHelper.WriteList(binaryWriter, outList12);
		TableHelper.WriteList(binaryWriter, outList13);
		stream.Position = 0L;
		using (Stream stream2 = new FileStream("\\\\172.22.10.28\\CounterSketchServices\\ZBinFiles\\Library" + libraryVersionId + ".zbin", FileMode.Create, FileAccess.Write))
		{
			LzmaAlone.Encode(stream, stream2);
			stream2.Close();
		}
		stream.Close();
	}
}
