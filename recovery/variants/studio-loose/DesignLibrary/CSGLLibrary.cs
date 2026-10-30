using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.IO.Compression;
using DesignLibrary.Enumerations;
using DesignLibrary.LibraryFormat;
using LZMA;

namespace DesignLibrary;

public static class CSGLLibrary
{
	private static string _dirCSGL;

	public static void CheckLibraryFiles(SQLiteConnection conn)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Expected O, but got Unknown
		string path = Path.Combine(_dirCSGL, "..\\LibraryCSGL");
		if (Directory.Exists(path))
		{
			Directory.Delete(path, recursive: true);
		}
		DirectoryInfo directoryInfo = new DirectoryInfo(_dirCSGL);
		((DbCommand)new SQLiteCommand("BEGIN", conn)).ExecuteNonQuery();
		((DbCommand)new SQLiteCommand("Update Designs SET CSGLFound=0", conn)).ExecuteNonQuery();
		SQLiteCommand val = new SQLiteCommand("Select Count(1) as CSGLFound from Designs where DesignId=@DesignId and Version=@Version", conn);
		val.Parameters.Add(new SQLiteParameter("DesignId"));
		val.Parameters.Add(new SQLiteParameter("Version"));
		SQLiteCommand val2 = new SQLiteCommand("UPDATE Designs SET CSGLFound=1 where DesignId=@DesignId and Version=@Version", conn);
		val2.Parameters.Add(new SQLiteParameter("DesignId"));
		val2.Parameters.Add(new SQLiteParameter("Version"));
		FileInfo[] files = directoryInfo.GetFiles("*.csgl");
		foreach (FileInfo obj in files)
		{
			bool flag = true;
			string name = obj.Name;
			int num = name.IndexOf("-");
			int num2 = name.IndexOf(".");
			if (num < 0 || num2 < 0)
			{
				flag = false;
			}
			int result = 0;
			int result2 = 0;
			if (flag)
			{
				string s = name.Substring(0, num);
				string s2 = name.Substring(num + 1, num2 - num - 1);
				if (!int.TryParse(s, out result))
				{
					flag = false;
				}
				if (!int.TryParse(s2, out result2))
				{
					flag = false;
				}
			}
			if (flag)
			{
				((DbParameter)(object)val.Parameters["DesignId"]).Value = result;
				((DbParameter)(object)val.Parameters["Version"]).Value = result2;
				object obj2 = ((DbCommand)(object)val).ExecuteScalar();
				int num3 = 0;
				if (obj2 != null && obj2 != DBNull.Value)
				{
					num3 = Convert.ToInt32(obj2);
				}
				if (num3 == 0)
				{
					flag = false;
				}
			}
			if (flag)
			{
				((DbParameter)(object)val2.Parameters["DesignId"]).Value = result;
				((DbParameter)(object)val2.Parameters["Version"]).Value = result2;
				((DbCommand)(object)val2).ExecuteNonQuery();
			}
		}
		((DbCommand)new SQLiteCommand("COMMIT", conn)).ExecuteNonQuery();
	}

	public static void CheckMissingFiles(IEnumerable<Designs> des, out List<string> neededfilenames, bool deleteUnFound)
	{
		neededfilenames = new List<string>();
		foreach (Designs de in des)
		{
			neededfilenames.Add(de.DesignId + "-" + de.Version + ".csgl");
		}
		FileInfo[] files = new DirectoryInfo(_dirCSGL).GetFiles("*.csgl");
		foreach (FileInfo fileInfo in files)
		{
			string name = fileInfo.Name;
			int count = neededfilenames.Count;
			bool flag = false;
			for (int j = 0; j < count; j++)
			{
				if (!(neededfilenames[j] != name))
				{
					flag = true;
					neededfilenames.RemoveAt(j);
					break;
				}
			}
			if (!(!flag && deleteUnFound))
			{
				continue;
			}
			try
			{
				if (File.Exists(fileInfo.FullName))
				{
					File.SetAttributes(fileInfo.FullName, FileAttributes.Normal);
					File.Delete(fileInfo.FullName);
				}
			}
			catch
			{
			}
		}
	}

	public static int CSGLsCount()
	{
		if (!Directory.Exists(_dirCSGL))
		{
			return 0;
		}
		return new DirectoryInfo(_dirCSGL).GetFiles("*.csgl").Length;
	}

	public static bool GetCSGToFile(long designId, long version, string filename)
	{
		string text = Path.Combine(_dirCSGL, designId + "-" + version + ".csgl");
		if (!File.Exists(text))
		{
			return false;
		}
		byte[] byteArray = GetByteArray(text, 4);
		File.WriteAllBytes(filename, byteArray);
		return true;
	}

	public static byte[] GetImageData(long designId, long version, EImageType imageType)
	{
		if (_dirCSGL != null)
		{
			string text = Path.Combine(_dirCSGL, designId + "-" + version + ".csgl");
			if (!File.Exists(text))
			{
				return new byte[1];
			}
			return GetByteArray(text, (int)imageType);
		}
		return null;
	}

	public static void PullInMissingFromDir(List<string> missingCSGLFiles, string sourceDir)
	{
		for (int i = 0; i < missingCSGLFiles.Count; i++)
		{
			string path = missingCSGLFiles[i];
			string text = Path.Combine(_dirCSGL, path);
			if (File.Exists(text))
			{
				missingCSGLFiles.RemoveAt(i);
				i--;
				continue;
			}
			string text2 = Path.Combine(sourceDir, path);
			if (File.Exists(text2))
			{
				File.Move(text2, text);
				missingCSGLFiles.RemoveAt(i);
				i--;
			}
		}
	}

	public static void SetLocalDir(string dirCSGL)
	{
		_dirCSGL = dirCSGL;
	}

	private static byte[] GetByteArray(string filename, int fileNumber)
	{
		Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(stream);
		binaryReader.ReadInt32();
		List<FileData> list = new List<FileData>();
		for (int i = 0; i < 5; i++)
		{
			FileData item = new FileData
			{
				CompType = binaryReader.ReadByte(),
				FilePosition = binaryReader.ReadInt64(),
				CompressedSize = binaryReader.ReadInt64(),
				DecompressedSize = binaryReader.ReadInt64()
			};
			list.Add(item);
		}
		FileData fileData = list[fileNumber];
		stream.Position = fileData.FilePosition;
		byte[] array = new byte[fileData.DecompressedSize];
		switch (fileData.CompType)
		{
		case 8:
		{
			using (Stream stream2 = new DeflateStream(stream, CompressionMode.Decompress))
			{
				stream2.Read(array, 0, (int)fileData.DecompressedSize);
			}
			break;
		}
		case 16:
		{
			using (MemoryStream memoryStream = new MemoryStream(array))
			{
				LzmaAlone.Decode(stream, (Stream)memoryStream, fileData.CompressedSize, fileData.DecompressedSize);
			}
			break;
		}
		}
		stream.Close();
		return array;
	}
}
