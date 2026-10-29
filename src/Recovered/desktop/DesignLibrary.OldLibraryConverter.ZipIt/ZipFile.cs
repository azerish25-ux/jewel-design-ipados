using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DesignLibrary.OldLibraryConverter.ZipIt;

internal class ZipFile
{
	private const uint CentralDirectoryHeaderSigniature = 1129532450u;

	private const uint EndOfCentralDirSignature = 1129532451u;

	private const uint LocalFileHeaderSignature = 1129532449u;

	private const uint VersionHeaderSignature = 307448865u;

	private readonly List<LocalFile> _localFiles = new List<LocalFile>();

	public string ZipFilename = string.Empty;

	public uint LibVersion;

	public uint LibVersion1;

	public int BitCount;

	private Stream _zipFileStream;

	public ZipFile(int bcount)
	{
		BitCount = bcount;
	}

	public int LocalFilesCount()
	{
		return _localFiles.Count();
	}

	public string rFilename(int i)
	{
		return _localFiles[i].RFileName;
	}

	public uint rUncompressedSize(int i)
	{
		return _localFiles[i].RUncompressedSize;
	}

	public bool ZipFileClose()
	{
		if (_zipFileStream != null)
		{
			_zipFileStream.Close();
			_zipFileStream.Dispose();
		}
		ZipFilename = string.Empty;
		return true;
	}

	public bool ZipFileExtractBytes(int n, ref byte[] data)
	{
		return _localFiles[n].LocalFileByteRead(ref data);
	}

	public bool ZipFileGetBytes(int n, ref byte[] data)
	{
		return _localFiles[n].LocalFileCompByteRead(ref data);
	}

	public bool ZipFileOpenRead(string newFilename)
	{
		ZipFileClose();
		ZipFilename = newFilename;
		_zipFileStream = new FileStream(ZipFilename, FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(_zipFileStream);
		ushort num = 0;
		_localFiles.Clear();
		while (_zipFileStream.Position + 4 < _zipFileStream.Length)
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
				LocalFile localFile = new LocalFile(_zipFileStream);
				localFile.LocalFileHeaderRead();
				_localFiles.Add(localFile);
				break;
			}
			case 1129532450u:
				_localFiles[num].CenteralDirectoryRead(BitCount);
				num++;
				break;
			case 1129532451u:
			{
				binaryReader.ReadUInt16();
				binaryReader.ReadUInt16();
				binaryReader.ReadUInt16();
				ushort num2 = binaryReader.ReadUInt16();
				binaryReader.ReadUInt32();
				if (BitCount != 0)
				{
					binaryReader.ReadInt64();
				}
				else
				{
					binaryReader.ReadInt32();
				}
				ushort count = binaryReader.ReadUInt16();
				new string(binaryReader.ReadChars(count));
				if (_localFiles.Count != num2)
				{
					return false;
				}
				break;
			}
			default:
				return false;
			}
		}
		if (_localFiles.Count != num)
		{
			return false;
		}
		return true;
	}

	~ZipFile()
	{
		if (_zipFileStream != null)
		{
			_zipFileStream.Close();
			_zipFileStream.Dispose();
		}
	}
}
