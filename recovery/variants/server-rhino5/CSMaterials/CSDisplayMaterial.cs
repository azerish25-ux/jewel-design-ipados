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
		Name = name;
		DisplayMaterial displayMaterial = new DisplayMaterial
		{
			Ambient = Color.Black,
			Diffuse = color,
			Specular = Color.White,
			Shine = glossAmount * Material.MaxShine,
			Transparency = trans,
			IsTwoSided = false
		};
		Texture texture = new Texture
		{
			FileName = fileNameEmap,
			TextureType = TextureType.Bitmap,
			TextureCombineMode = TextureCombineMode.Blend
		};
		texture.GetAlphaBlendValues(out var _, out var a, out var a2, out var a3, out var a4);
		texture.SetAlphaBlendValues(emapAmount, a, a2, a3, a4);
		displayMaterial.SetEnvironmentTexture(texture, front: true);
		Mat = new DisplayMaterial(displayMaterial);
	}

	public CSDisplayMaterial(Color color, Color ambient, double trans, string name)
	{
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
		Name = name;
		DisplayMaterial displayMaterial = new DisplayMaterial
		{
			Ambient = Color.Black,
			Diffuse = color,
			Specular = specular,
			IsTwoSided = false,
			Shine = glossAmount * Material.MaxShine,
			Transparency = trans
		};
		Texture texture = new Texture
		{
			FileName = fileNameEmap,
			TextureType = TextureType.Bitmap,
			TextureCombineMode = TextureCombineMode.Blend
		};
		texture.GetAlphaBlendValues(out var constant, out var a, out var a2, out var a3, out var a4);
		texture.SetAlphaBlendValues(emapAmount, a, a2, a3, a4);
		if (tileEnviroment != 1.0)
		{
			texture.ApplyUvwTransform = true;
			texture.UvwTransform = Transform.Scale(Plane.WorldXY, tileEnviroment, tileEnviroment, 1.0);
		}
		Texture texture2 = new Texture
		{
			FileName = fileNameTrans,
			TextureType = TextureType.Transparency,
			TextureCombineMode = TextureCombineMode.Blend
		};
		texture2.GetAlphaBlendValues(out constant, out a, out a2, out a3, out a4);
		texture2.SetAlphaBlendValues(transAmount, a, a2, a3, a4);
		if (tileTrans != 1.0)
		{
			texture2.ApplyUvwTransform = true;
			texture2.UvwTransform = Transform.Scale(Plane.WorldXY, tileTrans, tileTrans, 1.0);
		}
		displayMaterial.SetEnvironmentTexture(texture, front: true);
		displayMaterial.SetTransparencyTexture(texture2, front: true);
		Mat = new DisplayMaterial(displayMaterial);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public CSDisplayMaterial Duplicate()
	{
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
		_color = colorFore;
		Mat?.Dispose();
		Mat = new DisplayMaterial
		{
			Diffuse = _color
		};
	}

	public void UpdateMaterial(string texturePath)
	{
		DisplayMaterial displayMaterial = new DisplayMaterial
		{
			Ambient = Color.Black,
			Shine = 0.0
		};
		Texture texture = new Texture
		{
			FileName = texturePath,
			TextureCombineMode = TextureCombineMode.Decal
		};
		texture.GetAlphaBlendValues(out var _, out var a, out var a2, out var a3, out var a4);
		texture.SetAlphaBlendValues(1.0, a, a2, a3, a4);
		displayMaterial.SetBitmapTexture(texture, front: true);
		Mat?.Dispose();
		Mat = new DisplayMaterial(displayMaterial);
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			Mat.Dispose();
		}
	}
}
