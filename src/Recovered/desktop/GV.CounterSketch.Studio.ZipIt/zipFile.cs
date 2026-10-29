using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GV.CounterSketch.Studio.ZipIt;

public class zipFile
{
	private const uint VersionHeaderSignature = 307448865u;

	private const uint LocalFileHeaderSignature = 1129532449u;

	private const uint CentralDirectoryHeaderSigniature = 1129532450u;

	private const uint EndOfCentralDirSignature = 101010256u;

	public string zipFilename = "";

	public uint LibVersion;

	public uint LibVersion1;

	private Stream zipFS;

	private readonly List<localFile> LocalFiles = new List<localFile>();

	private string FileComment;

	private bool OpenForWrite;

	public zipFile()
	{
	}

	public zipFile(string newFilename)
	{
		ZipFileOpenRead(newFilename);
	}

	public void CreateDirForFile(string sFilename)
	{
		string text = sFilename.Substring(0, sFilename.LastIndexOf("\\"));
		if (!Directory.Exists(text))
		{
			while (!Directory.Exists(text))
			{
				text = text.Substring(0, text.LastIndexOf("\\"));
			}
			while (sFilename.IndexOf("\\", text.Length + 1) > 0)
			{
				text = sFilename.Substring(0, sFilename.IndexOf("\\", text.Length + 1));
				Directory.CreateDirectory(text);
			}
		}
	}

	public int LocalFilesCount()
	{
		return LocalFiles.Count();
	}

	public uint rCRC32(int i)
	{
		return LocalFiles[i].rCRC32;
	}

	public string rFilename(int i)
	{
		return LocalFiles[i].rFileName;
	}

	public long rLocalFileOffset(int i)
	{
		return LocalFiles[i].rRelativeOffsetOfLocalHeader;
	}

	public uint rUncompressedSize(int i)
	{
		return LocalFiles[i].rUncompressedSize;
	}

	public bool ZipFileAddFile(string strdir, string strfilename, bool compress)
	{
		if (!OpenForWrite)
		{
			return false;
		}
		localFile localFile2 = new localFile(zipFS, strfilename);
		localFile2.Compress = compress;
		LocalFiles.Add(localFile2);
		localFile2.LocalFileHeaderWrite();
		localFile2.LocalFileWrite(strdir + strfilename);
		return true;
	}

	public bool ZipFileAddFileFromZip(zipFile zf, int file, string strfilename)
	{
		if (!OpenForWrite)
		{
			return false;
		}
		localFile localFile2 = new localFile(zipFS, strfilename);
		LocalFiles.Add(localFile2);
		localFile2.LocalFileHeaderWrite();
		localFile2.LocalFileWriteFromZip(zf.LocalFiles[file]);
		return true;
	}

	public bool ZipFileClose()
	{
		if (!OpenForWrite)
		{
			if (zipFS != null)
			{
				zipFS.Close();
				zipFS.Dispose();
			}
			zipFilename = "";
			return true;
		}
		long num = (uint)zipFS.Position;
		BinaryWriter binaryWriter = new BinaryWriter(zipFS);
		FileComment = "";
		for (int i = 0; i < LocalFiles.Count; i++)
		{
			LocalFiles[i].CenteralDirectoryWrite(zipFS);
		}
		uint value = 101010256u;
		ushort value2 = 0;
		ushort value3 = 0;
		ushort value4 = (ushort)LocalFiles.Count;
		ushort value5 = (ushort)LocalFiles.Count;
		uint value6 = (uint)(zipFS.Position - num);
		ushort num2 = (ushort)FileComment.Length;
		binaryWriter.Write(value);
		binaryWriter.Write(value2);
		binaryWriter.Write(value3);
		binaryWriter.Write(value4);
		binaryWriter.Write(value5);
		binaryWriter.Write(value6);
		binaryWriter.Write(num);
		binaryWriter.Write(num2);
		binaryWriter.Write(FileComment.ToCharArray(), 0, num2);
		binaryWriter.Close();
		zipFS.Close();
		zipFS.Dispose();
		zipFilename = "";
		return true;
	}

	public void ZipFileCreate(string newFilename)
	{
		OpenForWrite = true;
		zipFilename = newFilename;
		CreateDirForFile(zipFilename);
		zipFS = new FileStream(zipFilename, FileMode.Create, FileAccess.Write);
	}

	public bool ZipFileExtractBytes(int n, ref byte[] data, bool checkCRC)
	{
		if (OpenForWrite)
		{
			return false;
		}
		return LocalFiles[n].LocalFileByteRead(ref data, checkCRC);
	}

	public bool ZipFileExtractFile(int n, string outFilename)
	{
		if (OpenForWrite)
		{
			return false;
		}
		return LocalFiles[n].LocalFileRead(outFilename);
	}

	public bool ZipFileExtractFileFast(long Offset, string outFilename)
	{
		if (OpenForWrite)
		{
			return false;
		}
		if (Offset > zipFS.Length)
		{
			return false;
		}
		zipFS.Seek(Offset, SeekOrigin.Begin);
		BinaryReader binaryReader = new BinaryReader(zipFS);
		LocalFiles.Clear();
		if (binaryReader.ReadUInt32() == 1129532449)
		{
			localFile localFile2 = new localFile(zipFS);
			localFile2.LocalFileHeaderRead();
			LocalFiles.Add(localFile2);
			return localFile2.LocalFileRead(outFilename);
		}
		return false;
	}

	public bool ZipFileGetBytesFast(long Offset, ref byte[] data, bool checkCRC)
	{
		if (OpenForWrite)
		{
			return false;
		}
		zipFS.Seek(Offset, SeekOrigin.Begin);
		BinaryReader binaryReader = new BinaryReader(zipFS);
		LocalFiles.Clear();
		if (Offset > binaryReader.BaseStream.Length)
		{
			return false;
		}
		if (binaryReader.ReadUInt32() == 1129532449)
		{
			localFile obj = new localFile(zipFS);
			obj.LocalFileHeaderRead();
			return obj.LocalFileByteRead(ref data, checkCRC);
		}
		return false;
	}

	public bool ZipFileOpenRead(string newFilename)
	{
		ZipFileClose();
		OpenForWrite = false;
		zipFilename = newFilename;
		zipFS = new FileStream(zipFilename, FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(zipFS);
		ushort num = 0;
		LocalFiles.Clear();
		while (zipFS.Position + 4 < zipFS.Length)
		{
			switch (binaryReader.ReadUInt32())
			{
			case 307448865u:
				binaryReader.ReadUInt32();
				LibVersion = binaryReader.ReadUInt32();
				LibVersion1 = binaryReader.ReadUInt32();
				break;
			case 1129532449u:
			{
				localFile localFile2 = new localFile(zipFS);
				localFile2.LocalFileHeaderRead();
				LocalFiles.Add(localFile2);
				break;
			}
			case 1129532450u:
				LocalFiles[num].CenteralDirectoryRead();
				num++;
				break;
			case 101010256u:
			{
				binaryReader.ReadUInt16();
				binaryReader.ReadUInt16();
				binaryReader.ReadUInt16();
				ushort num2 = binaryReader.ReadUInt16();
				binaryReader.ReadUInt32();
				binaryReader.ReadInt64();
				ushort count = binaryReader.ReadUInt16();
				char[] value = binaryReader.ReadChars(count);
				FileComment = new string(value);
				if (LocalFiles.Count != num2)
				{
					return false;
				}
				break;
			}
			default:
				return false;
			}
		}
		if (LocalFiles.Count != num)
		{
			return false;
		}
		return true;
	}

	public bool ZipFileOpenReadFast(string newFilename)
	{
		ZipFileClose();
		OpenForWrite = false;
		zipFilename = newFilename;
		zipFS = new FileStream(zipFilename, FileMode.Open, FileAccess.Read);
		LocalFiles.Clear();
		return true;
	}

	private static string HexIntToString(uint v)
	{
		string text = "00000000" + v.ToString("X").ToUpper();
		return text.Substring(text.Length - 8);
	}

	~zipFile()
	{
		if (zipFS != null)
		{
			zipFS.Close();
			zipFS.Dispose();
		}
	}
}
