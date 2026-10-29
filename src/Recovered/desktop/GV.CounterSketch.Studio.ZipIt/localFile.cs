using System.IO;

namespace GV.CounterSketch.Studio.ZipIt;

internal class localFile
{
	private ushort VersionMadeBy;

	private ushort VersionNeededToExtract;

	private ushort GeneralPurposeBitFlag;

	private ushort CompressionMethod;

	private uint CompressedSize;

	private ushort DiskNumberStart;

	private ushort InternalFileAttributes;

	private uint ExternalFileAttributes;

	private uint RelativeOffsetOfLocalHeader;

	private byte[] ExtraField;

	private string FileComment;

	private uint CRC32FilePos;

	private long DataLocation;

	private readonly Stream ZipFS;

	public string rFileName { get; private set; }

	public uint rUncompressedSize { get; private set; }

	public uint rCRC32 { get; private set; }

	public long rRelativeOffsetOfLocalHeader => RelativeOffsetOfLocalHeader;

	public Stream LocalFileGetStream
	{
		get
		{
			ZipFS.Seek(DataLocation, SeekOrigin.Begin);
			return ZipFS;
		}
	}

	public bool Compress
	{
		get
		{
			return CompressionMethod == 8;
		}
		set
		{
			if (value)
			{
				CompressionMethod = 8;
			}
			else
			{
				CompressionMethod = 0;
			}
		}
	}

	public localFile(Stream pZipFS)
	{
		ZipFS = pZipFS;
	}

	public localFile(Stream pZipFS, string strfilename)
	{
		VersionMadeBy = 0;
		VersionNeededToExtract = 20;
		GeneralPurposeBitFlag = 2;
		CompressionMethod = 8;
		DiskNumberStart = 0;
		InternalFileAttributes = 0;
		ExternalFileAttributes = 0u;
		rFileName = strfilename;
		FileComment = "";
		ExtraField = new byte[0];
		ZipFS = pZipFS;
	}

	public bool CenteralDirectoryRead()
	{
		BinaryReader binaryReader = new BinaryReader(ZipFS);
		VersionMadeBy = binaryReader.ReadUInt16();
		ushort num = binaryReader.ReadUInt16();
		if (VersionNeededToExtract != num)
		{
			return false;
		}
		num = binaryReader.ReadUInt16();
		if (GeneralPurposeBitFlag != num)
		{
			return false;
		}
		num = binaryReader.ReadUInt16();
		if (CompressionMethod != num)
		{
			return false;
		}
		uint num2 = binaryReader.ReadUInt32();
		if (rCRC32 != num2)
		{
			return false;
		}
		num2 = binaryReader.ReadUInt32();
		if (CompressedSize != num2)
		{
			return false;
		}
		num2 = binaryReader.ReadUInt32();
		if (rUncompressedSize != num2)
		{
			return false;
		}
		ushort num3 = binaryReader.ReadUInt16();
		if (rFileName.Length != num3)
		{
			return false;
		}
		ushort count = binaryReader.ReadUInt16();
		ushort count2 = binaryReader.ReadUInt16();
		DiskNumberStart = binaryReader.ReadUInt16();
		InternalFileAttributes = binaryReader.ReadUInt16();
		ExternalFileAttributes = binaryReader.ReadUInt32();
		long num4 = binaryReader.ReadInt64();
		if (RelativeOffsetOfLocalHeader != num4)
		{
			return false;
		}
		string text = new string(binaryReader.ReadChars(num3));
		if (rFileName != text)
		{
			return false;
		}
		binaryReader.ReadBytes(count);
		char[] value = binaryReader.ReadChars(count2);
		FileComment = new string(value);
		return true;
	}

	public bool CenteralDirectoryWrite(Stream CRCStream)
	{
		BinaryWriter binaryWriter = new BinaryWriter(CRCStream);
		uint value = 1129532450u;
		ushort num = (ushort)rFileName.Length;
		ushort num2 = (ushort)ExtraField.Length;
		ushort num3 = (ushort)FileComment.Length;
		binaryWriter.Write(value);
		binaryWriter.Write(VersionMadeBy);
		binaryWriter.Write(VersionNeededToExtract);
		binaryWriter.Write(GeneralPurposeBitFlag);
		binaryWriter.Write(CompressionMethod);
		binaryWriter.Write(rCRC32);
		binaryWriter.Write(CompressedSize);
		binaryWriter.Write(rUncompressedSize);
		binaryWriter.Write(num);
		binaryWriter.Write(num2);
		binaryWriter.Write(num3);
		binaryWriter.Write(DiskNumberStart);
		binaryWriter.Write(InternalFileAttributes);
		binaryWriter.Write(ExternalFileAttributes);
		binaryWriter.Write(RelativeOffsetOfLocalHeader);
		binaryWriter.Write(rFileName.ToCharArray(), 0, num);
		binaryWriter.Write(ExtraField, 0, num2);
		binaryWriter.Write(FileComment.ToCharArray(), 0, num3);
		return true;
	}

	public bool LocalFileByteRead(ref byte[] data, bool checkCRC)
	{
		Stream stream = null;
		ZipFS.Seek(DataLocation, SeekOrigin.Begin);
		if (CompressionMethod == 8)
		{
			stream = new DeflateStream(ZipFS, CompressionMode.Decompress, leaveOpen: true);
		}
		else if (CompressionMethod == 0)
		{
			stream = ZipFS;
		}
		if (stream == null)
		{
			return false;
		}
		Stream stream2 = null;
		stream2 = ((!checkCRC) ? stream : new CrcCalculatorStream(stream, leaveOpen: true));
		int num = (int)rUncompressedSize;
		data = new byte[num];
		stream2.Read(data, 0, num);
		if (checkCRC)
		{
			stream2.Close();
		}
		if (CompressionMethod == 8)
		{
			stream.Close();
			stream.Dispose();
		}
		if (!checkCRC)
		{
			return true;
		}
		uint crc = (uint)((CrcCalculatorStream)stream2).Crc;
		stream2.Dispose();
		return rCRC32 == crc;
	}

	public bool LocalFileHeaderRead()
	{
		BinaryReader binaryReader = new BinaryReader(ZipFS);
		RelativeOffsetOfLocalHeader = (uint)((int)ZipFS.Position - 4);
		VersionNeededToExtract = binaryReader.ReadUInt16();
		GeneralPurposeBitFlag = binaryReader.ReadUInt16();
		CompressionMethod = binaryReader.ReadUInt16();
		if (CompressionMethod != 8 && CompressionMethod != 0)
		{
			return false;
		}
		rCRC32 = binaryReader.ReadUInt32();
		CompressedSize = binaryReader.ReadUInt32();
		rUncompressedSize = binaryReader.ReadUInt32();
		ushort count = binaryReader.ReadUInt16();
		ushort count2 = binaryReader.ReadUInt16();
		char[] value = binaryReader.ReadChars(count);
		rFileName = new string(value);
		ExtraField = binaryReader.ReadBytes(count2);
		DataLocation = ZipFS.Position;
		ZipFS.Seek(CompressedSize, SeekOrigin.Current);
		VersionMadeBy = 0;
		DiskNumberStart = 0;
		InternalFileAttributes = 0;
		ExternalFileAttributes = 0u;
		FileComment = "";
		return true;
	}

	public bool LocalFileHeaderWrite()
	{
		BinaryWriter binaryWriter = new BinaryWriter(ZipFS);
		uint value = 1129532449u;
		RelativeOffsetOfLocalHeader = (uint)ZipFS.Position;
		ushort num = (ushort)rFileName.Length;
		ushort num2 = (ushort)ExtraField.Length;
		binaryWriter.Write(value);
		binaryWriter.Write(VersionNeededToExtract);
		binaryWriter.Write(GeneralPurposeBitFlag);
		binaryWriter.Write(CompressionMethod);
		CRC32FilePos = (uint)ZipFS.Position;
		binaryWriter.Write(rCRC32);
		binaryWriter.Write(CompressedSize);
		binaryWriter.Write(rUncompressedSize);
		binaryWriter.Write(num);
		binaryWriter.Write(num2);
		binaryWriter.Write(rFileName.ToCharArray(), 0, num);
		binaryWriter.Write(ExtraField, 0, num2);
		return true;
	}

	public bool LocalFileRead(string outFilename)
	{
		Stream stream = null;
		ZipFS.Seek(DataLocation, SeekOrigin.Begin);
		if (CompressionMethod == 8)
		{
			stream = new DeflateStream(ZipFS, CompressionMode.Decompress, leaveOpen: true);
		}
		else if (CompressionMethod == 0)
		{
			stream = ZipFS;
		}
		if (stream == null)
		{
			return false;
		}
		int num = 4096;
		CrcCalculatorStream crcCalculatorStream = new CrcCalculatorStream(stream, leaveOpen: true);
		FileStream fileStream = new FileStream(outFilename, FileMode.Create, FileAccess.Write);
		int num2 = (int)rUncompressedSize;
		byte[] buffer = new byte[num];
		while (num2 > 0)
		{
			int num3 = ((num2 <= num) ? num2 : num);
			crcCalculatorStream.Read(buffer, 0, num3);
			fileStream.Write(buffer, 0, num3);
			num2 -= num3;
		}
		fileStream.Close();
		crcCalculatorStream.Close();
		if (CompressionMethod == 8)
		{
			stream.Close();
			stream.Dispose();
		}
		uint crc = (uint)crcCalculatorStream.Crc;
		crcCalculatorStream.Dispose();
		if (rCRC32 != crc)
		{
			return false;
		}
		return true;
	}

	public bool LocalFileWrite(string inFilename)
	{
		DataLocation = ZipFS.Position;
		Stream stream = null;
		int num = 4096;
		if (CompressionMethod == 8)
		{
			stream = new DeflateStream(ZipFS, CompressionMode.Compress, CompressionLevel.BestCompression, leaveOpen: true);
		}
		else if (CompressionMethod == 0)
		{
			stream = ZipFS;
		}
		if (stream == null)
		{
			return false;
		}
		Stream stream2 = new FileStream(inFilename, FileMode.Open, FileAccess.Read);
		rUncompressedSize = (uint)stream2.Length;
		CrcCalculatorStream crcCalculatorStream = new CrcCalculatorStream(stream2, leaveOpen: true);
		int num2 = (int)rUncompressedSize;
		byte[] buffer = new byte[num];
		while (num2 > 0)
		{
			int num3 = ((num2 <= num) ? num2 : num);
			crcCalculatorStream.Read(buffer, 0, num3);
			stream.Write(buffer, 0, num3);
			num2 -= num3;
		}
		crcCalculatorStream.Close();
		stream2.Close();
		if (CompressionMethod == 8)
		{
			stream.Close();
			stream.Dispose();
		}
		CompressedSize = (uint)(ZipFS.Position - DataLocation);
		rCRC32 = (uint)crcCalculatorStream.Crc;
		crcCalculatorStream.Dispose();
		long position = ZipFS.Position;
		ZipFS.Seek(CRC32FilePos, SeekOrigin.Begin);
		BinaryWriter binaryWriter = new BinaryWriter(ZipFS);
		binaryWriter.Write(rCRC32);
		binaryWriter.Write(CompressedSize);
		binaryWriter.Write(rUncompressedSize);
		ZipFS.Seek(position, SeekOrigin.Begin);
		return true;
	}

	public bool LocalFileWriteFromZip(localFile LF)
	{
		DataLocation = ZipFS.Position;
		Stream stream = null;
		Stream stream2 = null;
		bool flag = false;
		if (CompressionMethod == 8)
		{
			stream = new DeflateStream(ZipFS, CompressionMode.Compress, CompressionLevel.BestCompression, leaveOpen: true);
		}
		else if (CompressionMethod == 0)
		{
			stream = ZipFS;
		}
		if (stream == null)
		{
			return false;
		}
		if (LF.CompressionMethod == 8)
		{
			stream2 = new DeflateStream(LF.LocalFileGetStream, CompressionMode.Decompress, leaveOpen: true);
		}
		else if (LF.CompressionMethod == 0)
		{
			stream2 = LF.LocalFileGetStream;
		}
		if (stream == null)
		{
			return false;
		}
		rUncompressedSize = LF.rUncompressedSize;
		rCRC32 = LF.rCRC32;
		int num = 4096;
		int num2 = 0;
		num2 = (int)((!flag) ? rUncompressedSize : LF.CompressedSize);
		byte[] buffer = new byte[num];
		while (num2 > 0)
		{
			int num3 = ((num2 <= num) ? num2 : num);
			stream2.Read(buffer, 0, num3);
			stream.Write(buffer, 0, num3);
			num2 -= num3;
		}
		stream.Flush();
		if (!flag)
		{
			if (CompressionMethod == 8)
			{
				stream.Close();
			}
			if (LF.CompressionMethod == 8)
			{
				stream2.Close();
			}
		}
		CompressedSize = (uint)(ZipFS.Position - DataLocation);
		long position = ZipFS.Position;
		ZipFS.Seek(CRC32FilePos, SeekOrigin.Begin);
		BinaryWriter binaryWriter = new BinaryWriter(ZipFS);
		binaryWriter.Write(rCRC32);
		binaryWriter.Write(CompressedSize);
		binaryWriter.Write(rUncompressedSize);
		ZipFS.Seek(position, SeekOrigin.Begin);
		return true;
	}
}
