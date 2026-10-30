using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using LZMA;
using SceneG;

namespace DesignLibrary;

public class CSGLMaker
{
	private const string PathDirDesigns = "\\\\172.22.10.60\\CSStorage\\CSDesigns\\";

	public static void MakeFiles(int designId, int version, bool loadShadow)
	{
		string strPath = "\\\\172.22.10.60\\CSStorage\\CSDesigns\\" + designId + "\\" + version + "\\RendersLibrary";
		byte[] array;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(1);
			for (int i = 0; i < 5; i++)
			{
				binaryWriter.Write((byte)0);
				binaryWriter.Write(0L);
				binaryWriter.Write(0L);
				binaryWriter.Write(0L);
			}
			List<FileData> list = new List<FileData>
			{
				PackImages(strPath, "P", memoryStream, loadShadow, 8),
				PackImages(strPath, "LD", memoryStream, loadShadow: false, 8),
				PackImages(strPath, "SV", memoryStream, loadShadow: false, 8),
				PackImages(strPath, "TF", memoryStream, loadShadow: false, 8)
			};
			string strPath2 = "\\\\172.22.10.60\\CSStorage\\CSDesigns\\" + designId + "\\" + version + "\\Design-Clean.csg";
			list.Add(AddFile(strPath2, memoryStream));
			long position = memoryStream.Position;
			memoryStream.Position = 0L;
			binaryWriter.Write(1);
			for (int j = 0; j < 5; j++)
			{
				binaryWriter.Write(list[j].CompType);
				binaryWriter.Write(list[j].FilePosition);
				binaryWriter.Write(list[j].CompressedSize);
				binaryWriter.Write(list[j].DecompressedSize);
			}
			memoryStream.Position = position;
			array = memoryStream.ToArray();
			memoryStream.Close();
		}
		byte[] array2;
		using (SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider())
		{
			array2 = sHA1CryptoServiceProvider.ComputeHash(array);
		}
		using Stream stream = new FileStream("\\\\172.22.10.60\\CSStorage\\CSDesigns\\" + designId + "\\" + version + "\\lib.csgl", FileMode.Create, FileAccess.Write);
		stream.Write(array, 0, array.Length);
		stream.Write(array2, 0, array2.Length);
	}

	private static FileData AddFile(string strPath, Stream sOut)
	{
		byte[] array = File.ReadAllBytes(strPath);
		FileData fileData = new FileData
		{
			CompType = 16,
			FilePosition = sOut.Position,
			DecompressedSize = array.Length
		};
		using (MemoryStream inStream = new MemoryStream(array))
		{
			LzmaAlone.Encode(inStream, sOut, writeLength: false);
		}
		fileData.CompressedSize = sOut.Position - fileData.FilePosition;
		return fileData;
	}

	private static FileData PackImages(string strPath, string view, Stream sOut, bool loadShadow, byte compType)
	{
		if (!File.Exists(strPath + "\\" + view + "-00.png"))
		{
			return null;
		}
		GImageMerge gImageMerge = new GImageMerge();
		gImageMerge.LoadBase(strPath + "\\" + view + "-00.png", loadShadow);
		if (File.Exists(strPath + "\\" + view + "-01.png"))
		{
			gImageMerge.AddLayer("Metal01", strPath + "\\" + view + "-01.png");
		}
		if (File.Exists(strPath + "\\" + view + "-02.png"))
		{
			gImageMerge.AddLayer("Metal02", strPath + "\\" + view + "-02.png");
		}
		if (File.Exists(strPath + "\\" + view + "-03.png"))
		{
			gImageMerge.AddLayer("Gem01", strPath + "\\" + view + "-03.png");
		}
		if (File.Exists(strPath + "\\" + view + "-04.png"))
		{
			gImageMerge.AddLayer("Gem02", strPath + "\\" + view + "-04.png");
		}
		if (File.Exists(strPath + "\\" + view + "-05.png"))
		{
			gImageMerge.AddLayer("Gem03", strPath + "\\" + view + "-05.png");
		}
		byte[] array = gImageMerge.WriteFile();
		FileData fileData = new FileData
		{
			CompType = compType,
			FilePosition = sOut.Position,
			DecompressedSize = array.Length
		};
		switch (compType)
		{
		case 8:
		{
			using (Stream stream = new DeflateStream(sOut, CompressionLevel.Fastest, leaveOpen: true))
			{
				stream.Write(array, 0, array.Length);
			}
			break;
		}
		case 16:
		{
			using (MemoryStream inStream = new MemoryStream(array))
			{
				LzmaAlone.Encode(inStream, sOut, writeLength: false);
			}
			break;
		}
		}
		fileData.CompressedSize = sOut.Position - fileData.FilePosition;
		return fileData;
	}
}
