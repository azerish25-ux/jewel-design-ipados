using System.Security.Cryptography;

namespace Geomagic.Utilities;

public interface IEncryptor
{
	CipherMode Mode { get; set; }

	PaddingMode Padding { get; set; }

	byte[] Encrypt(byte[] Salt, byte[] IV, byte[] Data);

	byte[] Encrypt(byte[] Salt, byte[] IV, string Source);

	void Encrypt(byte[] Salt, byte[] IV, byte[] Data, string Dest);

	void Encrypt(byte[] Salt, byte[] IV, string Source, string Dest);

	byte[] Decrypt(byte[] Salt, byte[] IV, byte[] Data);

	byte[] Decrypt(byte[] Salt, byte[] IV, string Source);

	void Decrypt(byte[] Salt, byte[] IV, byte[] Data, string Dest);

	void Decrypt(byte[] Salt, byte[] IV, string Source, string Dest);
}
