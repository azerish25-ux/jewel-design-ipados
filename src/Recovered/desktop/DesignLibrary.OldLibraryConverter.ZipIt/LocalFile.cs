using System.IO;
using System.IO.Compression;

namespace DesignLibrary.OldLibraryConverter.ZipIt;

internal class LocalFile
{
	private readonly Stream _zipFileStream;

	private ushort _versionMadeBy;

	private ushort _versionNeededToExtract;

	private ushort _generalPurposeBitFlag;

	private ushort _compressionMethod;

	private uint _crc32;

	private uint _compressedSize;

	private ushort _diskNumberStart;

	private ushort _internalFileAttributes;

	private uint _externalFileAttributes;

	private long _relativeOffsetOfLocalHeader;

	private byte[] _extraField;

	private string _fileComment;

	private long _dataLocation;

	public string RFileName { get; private set; }

	public uint RUncompressedSize { get; private set; }

	public LocalFile(Stream pZipFs)
	{
		_zipFileStream = pZipFs;
	}

	public bool CenteralDirectoryRead(int bitcount)
	{
		BinaryReader binaryReader = new BinaryReader(_zipFileStream);
		_versionMadeBy = binaryReader.ReadUInt16();
		ushort num = binaryReader.ReadUInt16();
		if (_versionNeededToExtract != num)
		{
			return false;
		}
		num = binaryReader.ReadUInt16();
		if (_generalPurposeBitFlag != num)
		{
			return false;
		}
		num = binaryReader.ReadUInt16();
		if (_compressionMethod != num)
		{
			return false;
		}
		uint num2 = binaryReader.ReadUInt32();
		if (_crc32 != num2)
		{
			return false;
		}
		num2 = binaryReader.ReadUInt32();
		if (_compressedSize != num2)
		{
			return false;
		}
		num2 = binaryReader.ReadUInt32();
		if (RUncompressedSize != num2)
		{
			return false;
		}
		ushort num3 = binaryReader.ReadUInt16();
		if (RFileName.Length != num3)
		{
			return false;
		}
		ushort count = binaryReader.ReadUInt16();
		ushort count2 = binaryReader.ReadUInt16();
		_diskNumberStart = binaryReader.ReadUInt16();
		_internalFileAttributes = binaryReader.ReadUInt16();
		_externalFileAttributes = binaryReader.ReadUInt32();
		long num4 = ((bitcount == 0) ? binaryReader.ReadInt32() : binaryReader.ReadInt64());
		if (_relativeOffsetOfLocalHeader != num4)
		{
			return false;
		}
		string text = new string(binaryReader.ReadChars(num3));
		if (RFileName != text)
		{
			return false;
		}
		binaryReader.ReadBytes(count);
		char[] value = binaryReader.ReadChars(count2);
		_fileComment = new string(value);
		return true;
	}

	public bool LocalFileByteRead(ref byte[] data)
	{
		Stream stream = null;
		_zipFileStream.Seek(_dataLocation, SeekOrigin.Begin);
		if (_compressionMethod == 8)
		{
			stream = new DeflateStream(_zipFileStream, CompressionMode.Decompress, leaveOpen: true);
		}
		else if (_compressionMethod == 0)
		{
			stream = _zipFileStream;
		}
		if (stream == null)
		{
			return false;
		}
		int rUncompressedSize = (int)RUncompressedSize;
		data = new byte[rUncompressedSize];
		stream.Read(data, 0, rUncompressedSize);
		if (_compressionMethod == 8)
		{
			stream.Close();
			stream.Dispose();
		}
		return true;
	}

	public bool LocalFileCompByteRead(ref byte[] data)
	{
		_zipFileStream.Seek(_dataLocation, SeekOrigin.Begin);
		int compressedSize = (int)_compressedSize;
		data = new byte[compressedSize];
		_zipFileStream.Read(data, 0, compressedSize);
		return true;
	}

	public bool LocalFileHeaderRead()
	{
		BinaryReader binaryReader = new BinaryReader(_zipFileStream);
		_relativeOffsetOfLocalHeader = _zipFileStream.Position - 4;
		_versionNeededToExtract = binaryReader.ReadUInt16();
		_generalPurposeBitFlag = binaryReader.ReadUInt16();
		_compressionMethod = binaryReader.ReadUInt16();
		_crc32 = binaryReader.ReadUInt32();
		_compressedSize = binaryReader.ReadUInt32();
		RUncompressedSize = binaryReader.ReadUInt32();
		ushort count = binaryReader.ReadUInt16();
		ushort count2 = binaryReader.ReadUInt16();
		char[] value = binaryReader.ReadChars(count);
		RFileName = new string(value);
		_extraField = binaryReader.ReadBytes(count2);
		_dataLocation = _zipFileStream.Position;
		_zipFileStream.Seek(_compressedSize, SeekOrigin.Current);
		_versionMadeBy = 0;
		_diskNumberStart = 0;
		_internalFileAttributes = 0;
		_externalFileAttributes = 0u;
		_fileComment = string.Empty;
		return true;
	}
}
