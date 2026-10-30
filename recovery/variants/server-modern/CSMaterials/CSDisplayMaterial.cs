using System;
using System.Drawing;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace CSMaterials;

public sealed class CSDisplayMaterial : IDisposable
{
	private Color _color;

	public DisplayMaterial Mat { get; private set; }

	public string Name { get; }

	public CSDisplayMaterial(Color inputColor, string name)
	{
		Name = name;
		UpdateMaterial(inputColor);
	}

	public CSDisplayMaterial(string fileName, string name)
	{
		Name = name;
		UpdateMaterial(fileName);
	}

	public CSDisplayMaterial(string fileNameEmap, double emapAmount, Color color, double glossAmount, double trans, string name)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		Name = name;
		DisplayMaterial val = new DisplayMaterial
		{
			Ambient = Color.Black,
			Diffuse = color,
			Specular = Color.White,
			Shine = glossAmount * Material.MaxShine,
			Transparency = trans,
			IsTwoSided = false
		};
		Texture val2 = new Texture
		{
			FileName = fileNameEmap,
			TextureType = (TextureType)1,
			TextureCombineMode = (TextureCombineMode)3
		};
		double num = default(double);
		double num2 = default(double);
		double num3 = default(double);
		double num4 = default(double);
		double num5 = default(double);
		val2.GetAlphaBlendValues(ref num, ref num2, ref num3, ref num4, ref num5);
		val2.SetAlphaBlendValues(emapAmount, num2, num3, num4, num5);
		val.SetEnvironmentTexture(val2, true);
		Mat = new DisplayMaterial(val);
	}

	public CSDisplayMaterial(Color color, Color ambient, double trans, string name)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		Name = name;
		Mat = new DisplayMaterial
		{
			Diffuse = color,
			Emission = Color.Black,
			Ambient = ambient,
			Transparency = trans,
			BackDiffuse = color,
			BackEmission = Color.Black,
			BackAmbient = ambient,
			BackTransparency = trans,
			IsTwoSided = false
		};
	}

	public CSDisplayMaterial(string fileNameEmap, double emapAmount, string fileNameTrans, double transAmount, Color color, Color specular, double glossAmount, double trans, string name, double tileTrans, double tileEnviroment)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		Name = name;
		DisplayMaterial val = new DisplayMaterial
		{
			Ambient = Color.Black,
			Diffuse = color,
			Specular = specular,
			IsTwoSided = false,
			Shine = glossAmount * Material.MaxShine,
			Transparency = trans
		};
		Texture val2 = new Texture
		{
			FileName = fileNameEmap,
			TextureType = (TextureType)1,
			TextureCombineMode = (TextureCombineMode)3
		};
		double num = default(double);
		double num2 = default(double);
		double num3 = default(double);
		double num4 = default(double);
		double num5 = default(double);
		val2.GetAlphaBlendValues(ref num, ref num2, ref num3, ref num4, ref num5);
		val2.SetAlphaBlendValues(emapAmount, num2, num3, num4, num5);
		if (tileEnviroment != 1.0)
		{
			val2.ApplyUvwTransform = true;
			val2.UvwTransform = Transform.Scale(Plane.WorldXY, tileEnviroment, tileEnviroment, 1.0);
		}
		Texture val3 = new Texture
		{
			FileName = fileNameTrans,
			TextureType = (TextureType)3,
			TextureCombineMode = (TextureCombineMode)3
		};
		val3.GetAlphaBlendValues(ref num, ref num2, ref num3, ref num4, ref num5);
		val3.SetAlphaBlendValues(transAmount, num2, num3, num4, num5);
		if (tileTrans != 1.0)
		{
			val3.ApplyUvwTransform = true;
			val3.UvwTransform = Transform.Scale(Plane.WorldXY, tileTrans, tileTrans, 1.0);
		}
		val.SetEnvironmentTexture(val2, true);
		val.SetTransparencyTexture(val3, true);
		Mat = new DisplayMaterial(val);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public CSDisplayMaterial Duplicate()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		return new CSDisplayMaterial(_color, Name)
		{
			Mat = new DisplayMaterial(Mat)
		};
	}

	public override string ToString()
	{
		return Name;
	}

	public void UpdateMaterial(Color colorFore)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		_color = colorFore;
		DisplayMaterial mat = Mat;
		if (mat != null)
		{
			mat.Dispose();
		}
		Mat = new DisplayMaterial
		{
			Diffuse = _color
		};
	}

	public void UpdateMaterial(string texturePath)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		DisplayMaterial val = new DisplayMaterial
		{
			Ambient = Color.Black,
			Shine = 0.0
		};
		Texture val2 = new Texture
		{
			FileName = texturePath,
			TextureCombineMode = (TextureCombineMode)2
		};
		double num = default(double);
		double num2 = default(double);
		double num3 = default(double);
		double num4 = default(double);
		double num5 = default(double);
		val2.GetAlphaBlendValues(ref num, ref num2, ref num3, ref num4, ref num5);
		val2.SetAlphaBlendValues(1.0, num2, num3, num4, num5);
		val.SetBitmapTexture(val2, true);
		DisplayMaterial mat = Mat;
		if (mat != null)
		{
			mat.Dispose();
		}
		Mat = new DisplayMaterial(val);
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			Mat.Dispose();
		}
	}
}
