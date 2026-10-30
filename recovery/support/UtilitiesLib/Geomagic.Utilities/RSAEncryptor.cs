using System;
using System.IO;
using System.Security.Cryptography;

namespace Geomagic.Utilities;

public class RSAEncryptor : IEncryptor
{
	private CipherMode _Mode = CipherMode.ECB;

	private PaddingMode _Padding = PaddingMode.Zeros;

	public CipherMode Mode
	{
		get
		{
			return _Mode;
		}
		set
		{
			_Mode = value;
		}
	}

	public PaddingMode Padding
	{
		get
		{
			return _Padding;
		}
		set
		{
			_Padding = value;
		}
	}

	public byte[] Encrypt(byte[] Salt, byte[] IV, byte[] Data)
	{
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Mode = Mode;
		rijndaelManaged.Padding = Padding;
		ICryptoTransform transform = rijndaelManaged.CreateEncryptor(Salt, IV);
		byte[] array = null;
		using MemoryStream memoryStream = new MemoryStream();
		using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
		cryptoStream.Write(Data, 0, Data.Length);
		cryptoStream.FlushFinalBlock();
		return memoryStream.ToArray();
	}

	public byte[] Encrypt(byte[] Salt, byte[] IV, string Source)
	{
		byte[] array = FromFile(Source);
		if (array == null)
		{
			return null;
		}
		return Encrypt(Salt, IV, array);
	}

	public void Encrypt(byte[] Salt, byte[] IV, byte[] Data, string Dest)
	{
		byte[] data = Encrypt(Salt, IV, Data);
		ToFile(data, Dest);
	}

	public void Encrypt(byte[] Salt, byte[] IV, string Source, string Dest)
	{
		byte[] array = FromFile(Source);
		if (array != null)
		{
			Encrypt(Salt, IV, array, Dest);
		}
	}

	public byte[] Decrypt(byte[] Salt, byte[] IV, byte[] EncryptedData)
	{
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Mode = Mode;
		rijndaelManaged.Padding = Padding;
		ICryptoTransform transform = rijndaelManaged.CreateDecryptor(Salt, IV);
		byte[] array = null;
		using MemoryStream stream = new MemoryStream(EncryptedData);
		using CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
		byte[] array2 = new byte[EncryptedData.Length];
		int num = cryptoStream.Read(array2, 0, array2.Length);
		array = new byte[num];
		Array.Copy(array2, 0, array, 0, num);
		return array;
	}

	public void Decrypt(byte[] Salt, byte[] IV, byte[] EncryptedData, string Dest)
	{
		byte[] data = Decrypt(Salt, IV, EncryptedData);
		ToFile(data, Dest);
	}

	public byte[] Decrypt(byte[] Salt, byte[] IV, string Source)
	{
		byte[] array = FromFile(Source);
		if (array == null)
		{
			return null;
		}
		return Decrypt(Salt, IV, array);
	}

	public void Decrypt(byte[] Salt, byte[] IV, string Source, string Dest)
	{
		byte[] array = FromFile(Source);
		if (array != null)
		{
			byte[] data = Decrypt(Salt, IV, array);
			ToFile(data, Dest);
		}
	}

	private void ToFile(byte[] Data, string Dest)
	{
		FileInfo fileInfo = new FileInfo(Dest);
		using FileStream fileStream = fileInfo.Open(FileMode.Create, FileAccess.ReadWrite);
		fileStream.Write(Data, 0, Data.Length);
	}

	private byte[] FromFile(string Source)
	{
		byte[] array = null;
		FileInfo fileInfo = new FileInfo(Source);
		using FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read);
		array = new byte[fileStream.Length];
		fileStream.Read(array, 0, (int)fileStream.Length);
		return array;
	}
}
