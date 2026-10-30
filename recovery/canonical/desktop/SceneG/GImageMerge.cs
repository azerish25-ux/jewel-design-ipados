using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace SceneG;

public class GImageMerge
{
	private class Layer
	{
		public string Name;

		public byte MaxR;

		public byte MaxG;

		public byte MaxB;

		public byte[] DiffImage;

		public unsafe void SetValues(byte[] bImage, int resBaseX, int resBaseY, string sourceFile)
		{
			Bitmap bitmap = new Bitmap(sourceFile);
			DiffImage = new byte[resBaseX * resBaseY * 3];
			MaxR = 0;
			MaxG = 0;
			MaxB = 0;
			int num = 0;
			int num2 = 0;
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			for (int i = 0; i < resBaseY; i++)
			{
				byte* ptr = (byte*)(void*)bitmapData.Scan0 + i * bitmapData.Stride;
				for (int j = 0; j < resBaseX; j++)
				{
					byte b = *(ptr++);
					byte b2 = *(ptr++);
					byte b3 = *(ptr++);
					if (*(ptr++) > 0)
					{
						int num3 = b3 - bImage[num++];
						if (num3 < 0)
						{
							num3 = 0;
						}
						if (num3 > MaxR)
						{
							MaxR = (byte)num3;
						}
						DiffImage[num2++] = (byte)num3;
						int num4 = b2 - bImage[num++];
						if (num4 < 0)
						{
							num4 = 0;
						}
						if (num4 > MaxG)
						{
							MaxG = (byte)num4;
						}
						DiffImage[num2++] = (byte)num4;
						int num5 = b - bImage[num++];
						if (num5 < 0)
						{
							num5 = 0;
						}
						if (num5 > MaxB)
						{
							MaxB = (byte)num5;
						}
						DiffImage[num2++] = (byte)num5;
						num++;
					}
					else
					{
						DiffImage[num2++] = 0;
						DiffImage[num2++] = 0;
						DiffImage[num2++] = 0;
						num += 4;
					}
				}
			}
			bitmap.UnlockBits(bitmapData);
			bitmap.Dispose();
		}
	}

	private readonly List<Layer> _layers = new List<Layer>();

	private int _sizeX;

	private int _sizeY;

	private byte[] _bImage;

	public void AddLayer(string name, string filename)
	{
		Layer layer = new Layer
		{
			Name = name
		};
		layer.SetValues(_bImage, _sizeX, _sizeY, filename);
		_layers.Add(layer);
	}

	public unsafe void LoadBase(string basefilename, bool AddShadow)
	{
		Bitmap bitmap = new Bitmap(basefilename);
		Bitmap cSS_Shadow = Resource1.CSS_Shadow;
		Bitmap bitmap2 = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
		Graphics graphics = Graphics.FromImage(bitmap2);
		if (AddShadow)
		{
			graphics.DrawImage(cSS_Shadow, 0, 0, bitmap.Width, bitmap.Height);
		}
		graphics.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
		graphics.Dispose();
		cSS_Shadow.Dispose();
		bitmap.Dispose();
		_sizeX = bitmap2.Width;
		_sizeY = bitmap2.Height;
		_bImage = new byte[_sizeX * _sizeY * 4];
		BitmapData bitmapData = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
		int num = 0;
		for (int i = 0; i < _sizeY; i++)
		{
			byte* ptr = (byte*)(void*)bitmapData.Scan0 + i * bitmapData.Stride;
			for (int j = 0; j < _sizeX; j++)
			{
				byte b = *(ptr++);
				byte b2 = *(ptr++);
				byte b3 = *(ptr++);
				byte b4 = *(ptr++);
				if (b4 == 0)
				{
					b3 = 0;
					b = 0;
					b2 = 0;
				}
				_bImage[num++] = b3;
				_bImage[num++] = b2;
				_bImage[num++] = b;
				_bImage[num++] = b4;
			}
		}
		bitmap2.UnlockBits(bitmapData);
	}

	public byte[] WriteFile()
	{
		MemoryStream memoryStream = new MemoryStream();
		WriteStream(memoryStream);
		memoryStream.Flush();
		byte[] result = memoryStream.ToArray();
		memoryStream.Close();
		return result;
	}

	public void WriteStream(Stream st)
	{
		st.WriteByte(1);
		Write(st, _sizeX);
		Write(st, _sizeY);
		Write(st, _layers.Count);
		foreach (Layer layer in _layers)
		{
			Write(st, layer.Name);
			st.WriteByte(layer.MaxR);
			st.WriteByte(layer.MaxG);
			st.WriteByte(layer.MaxB);
		}
		for (int i = 0; i < _sizeY; i++)
		{
			for (int j = 0; j < _sizeX; j++)
			{
				byte b = _bImage[(i * _sizeX + j) * 4 + 3];
				st.WriteByte(b);
				if (b <= 0)
				{
					continue;
				}
				st.WriteByte(_bImage[(i * _sizeX + j) * 4]);
				st.WriteByte(_bImage[(i * _sizeX + j) * 4 + 1]);
				st.WriteByte(_bImage[(i * _sizeX + j) * 4 + 2]);
				foreach (Layer layer2 in _layers)
				{
					st.WriteByte(layer2.DiffImage[(i * _sizeX + j) * 3]);
					st.WriteByte(layer2.DiffImage[(i * _sizeX + j) * 3 + 1]);
					st.WriteByte(layer2.DiffImage[(i * _sizeX + j) * 3 + 2]);
				}
			}
		}
	}

	private static void Write(Stream st, int v)
	{
		byte[] bytes = BitConverter.GetBytes(v);
		st.Write(bytes, 0, bytes.Length);
	}

	private static void Write(Stream st, string v)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(v);
		st.WriteByte((byte)bytes.Length);
		st.Write(bytes, 0, bytes.Length);
	}
}
