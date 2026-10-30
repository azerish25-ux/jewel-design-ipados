using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace SceneG;

public class GImageFileLoader
{
	public class Layer
	{
		public byte[] LookUpB;

		public byte[] LookUpG;

		public byte[] LookUpR;

		public byte MaxB;

		public byte MaxG;

		public byte MaxR;

		public string Name;
	}

	private static readonly object _LockObject = new object();

	private readonly byte[] _gImg;

	private int _gImgPtr;

	private List<Layer> _layers;

	private int _sizeX;

	private int _sizeY;

	public GImageFileLoader(byte[] imageByteArray)
	{
		if (imageByteArray[0] != 1)
		{
			_layers = new List<Layer>();
			_sizeX = 0;
			_sizeY = 0;
			_gImg = null;
		}
		else
		{
			_gImg = imageByteArray;
			_gImgPtr = 0;
			InitalizeLayers();
		}
	}

	public unsafe Bitmap GetImage()
	{
		if (_sizeX == 0 || _sizeY == 0)
		{
			return new Bitmap(1, 1, PixelFormat.Format32bppArgb);
		}
		lock (_LockObject)
		{
			Bitmap bitmap = new Bitmap(_sizeX, _sizeY, PixelFormat.Format32bppArgb);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			int gImgPtr = _gImgPtr;
			for (int i = 0; i < _sizeY; i++)
			{
				byte* ptr = (byte*)(void*)bitmapData.Scan0 + i * bitmapData.Stride;
				for (int j = 0; j < _sizeX; j++)
				{
					byte b = _gImg[gImgPtr++];
					if (b > 0)
					{
						byte b3;
						byte b2 = (b3 = _gImg[gImgPtr++]);
						byte b5;
						byte b4 = (b5 = _gImg[gImgPtr++]);
						byte b7;
						byte b6 = (b7 = _gImg[gImgPtr++]);
						foreach (Layer layer in _layers)
						{
							byte b8 = (byte)(b3 + layer.LookUpR[_gImg[gImgPtr++]]);
							byte b9 = (byte)(b5 + layer.LookUpG[_gImg[gImgPtr++]]);
							byte b10 = (byte)(b7 + layer.LookUpB[_gImg[gImgPtr++]]);
							if (b8 > b2)
							{
								b2 = b8;
							}
							if (b9 > b4)
							{
								b4 = b9;
							}
							if (b10 > b6)
							{
								b6 = b10;
							}
						}
						*(ptr++) = b6;
						*(ptr++) = b4;
						*(ptr++) = b2;
						*(ptr++) = b;
					}
					else
					{
						*(ptr++) = 0;
						*(ptr++) = 0;
						*(ptr++) = 0;
						*(ptr++) = 0;
					}
				}
			}
			bitmap.UnlockBits(bitmapData);
			GC.AddMemoryPressure(bitmapData.Height * bitmapData.Stride);
			return bitmap;
		}
	}

	public bool IsLayerPresent(string layerName)
	{
		return _layers.Any((Layer l) => l.Name.ToLower() == layerName.ToLower());
	}

	public void SetLayer(string name, double r, double g, double b)
	{
		int i;
		for (i = 0; i < _layers.Count && _layers[i].Name != name; i++)
		{
		}
		if (!(_layers[i].Name != name))
		{
			Layer layer = _layers[i];
			layer.LookUpR = new byte[layer.MaxR + 1];
			layer.LookUpG = new byte[layer.MaxG + 1];
			layer.LookUpB = new byte[layer.MaxB + 1];
			for (int j = 0; j <= layer.MaxR; j++)
			{
				layer.LookUpR[j] = (byte)(r * (double)j);
			}
			for (int k = 0; k <= layer.MaxG; k++)
			{
				layer.LookUpG[k] = (byte)(g * (double)k);
			}
			for (int l = 0; l <= layer.MaxB; l++)
			{
				layer.LookUpB[l] = (byte)(b * (double)l);
			}
		}
	}

	private static int GetInt(byte[] mem, ref int ptr)
	{
		int result = BitConverter.ToInt32(mem, ptr);
		ptr += 4;
		return result;
	}

	private static string GetString(byte[] mem, ref int ptr)
	{
		byte b = mem[ptr++];
		string result = Encoding.ASCII.GetString(mem, ptr, b);
		ptr += b;
		return result;
	}

	private void InitalizeLayers()
	{
		int ptr = 1;
		_sizeX = GetInt(_gImg, ref ptr);
		_sizeY = GetInt(_gImg, ref ptr);
		int num = GetInt(_gImg, ref ptr);
		_layers = new List<Layer>(num);
		for (int i = 0; i < num; i++)
		{
			Layer item = new Layer
			{
				Name = GetString(_gImg, ref ptr),
				MaxR = _gImg[ptr++],
				MaxG = _gImg[ptr++],
				MaxB = _gImg[ptr++]
			};
			_layers.Add(item);
		}
		_gImgPtr = ptr;
	}
}
