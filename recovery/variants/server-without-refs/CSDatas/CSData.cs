using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Media;
using CSMaterials;
using Rhino.Geometry;

namespace CSDatas;

public class CSData : IDisposable
{
	protected bool _isDisposed;

	protected bool _isOutputObject;

	private Bitmap _bitmap;

	private MaterialGroup _currentMaterialGroup;

	public MaterialClassification DefaultDisplayMaterialClassification { get; set; }

	public bool Disabled { get; set; }

	public bool NormalizedToGem { private get; set; }

	public MaterialGroup CurrentMaterialGroup
	{
		get
		{
			if (_currentMaterialGroup != null && _currentMaterialGroup.Classification != MaterialClassification.Undefined)
			{
				return _currentMaterialGroup;
			}
			MaterialGroup materialGroup = MaterialGroupHelper.GetMaterialGroupByClassification(DefaultDisplayMaterialClassification).FirstOrDefault();
			if (materialGroup != null)
			{
				return materialGroup;
			}
			return _currentMaterialGroup;
		}
		set
		{
			_currentMaterialGroup = value;
		}
	}

	public CSData()
	{
		DefaultDisplayMaterialClassification = MaterialClassification.Undefined;
	}

	protected CSData(CSData copy)
	{
		DefaultDisplayMaterialClassification = copy.DefaultDisplayMaterialClassification;
		Disabled = copy.Disabled;
		_isOutputObject = copy._isOutputObject;
		NormalizedToGem = copy.NormalizedToGem;
		if (copy._bitmap != null)
		{
			_bitmap = new Bitmap(copy._bitmap);
		}
		_isDisposed = copy._isDisposed;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public virtual CSData Duplicate()
	{
		return new CSData(this);
	}

	public virtual bool GetAllMaterial(List<CSData> datas, Transform transform, MaterialClassification whicMaterialClassification, MaterialClassification searchMaterialClassification)
	{
		return false;
	}

	public virtual bool GetBoolean(ref bool value)
	{
		return false;
	}

	public virtual bool GetBoundingBox(ref BoundingBox boundingBox)
	{
		return false;
	}

	public virtual Brep GetBrep()
	{
		return null;
	}

	public virtual Curve GetCurve()
	{
		return null;
	}

	public virtual double GetCurveParameter(int i)
	{
		return 0.0;
	}

	public virtual int GetCurveParameterCount()
	{
		return 0;
	}

	public virtual NurbsCurve GetCurveSegment(int segmentNumber, int segmentCount, bool fromStart)
	{
		return null;
	}

	public virtual bool GetDouble(ref double value)
	{
		return false;
	}

	public virtual GeometryBase GetGeometry()
	{
		return null;
	}

	public Image GetImage(int sizeX, int sizeY, System.Drawing.Color color)
	{
		return GetImage(sizeX, sizeY, color, System.Drawing.Color.Black);
	}

	public virtual bool GetInteger(ref int value)
	{
		return false;
	}

	public virtual Mesh GetMesh()
	{
		MeshingParameters coarse = MeshingParameters.Coarse;
		return MeshObject(coarse);
	}

	public virtual bool GetNormalizedToGem()
	{
		return NormalizedToGem;
	}

	public virtual NurbsCurve GetNurbsCurve()
	{
		return null;
	}

	public virtual NurbsSurface GetNurbsSurface()
	{
		return null;
	}

	public virtual bool GetPoint(out Point3d point)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		point = Point3d.Unset;
		return false;
	}

	public virtual PointCollection GetPolyline(double precision)
	{
		return null;
	}

	public virtual bool GetString(ref string output)
	{
		return false;
	}

	public virtual Surface GetSurface()
	{
		return null;
	}

	[Obsolete]
	public virtual bool GetTightBoundingBox(ref BoundingBox boundingBox)
	{
		return false;
	}

	[Obsolete]
	public virtual bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow)
	{
		return false;
	}

	[Obsolete]
	public virtual bool GetTightBoundingBox(ref BoundingBox boundingBox, bool doGrow, Transform transform)
	{
		return false;
	}

	public virtual bool GetTightBoundingBoxNew(ref BoundingBox boundingBox)
	{
		return GetTightBoundingBox(ref boundingBox);
	}

	public virtual bool GetTightBoundingBoxNew(ref BoundingBox boundingBox, bool doGrow)
	{
		return GetTightBoundingBox(ref boundingBox, doGrow);
	}

	public virtual bool GetTightBoundingBoxNew(ref BoundingBox boundingBox, bool doGrow, Transform transform)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return GetTightBoundingBox(ref boundingBox, doGrow, transform);
	}

	public virtual bool GetUserString(string key, ref string value)
	{
		return false;
	}

	public virtual bool HasBrep()
	{
		return false;
	}

	public virtual bool HasMesh()
	{
		return false;
	}

	public virtual bool HasNurbsCurve()
	{
		return false;
	}

	public virtual bool HasNurbsSurface()
	{
		return false;
	}

	public virtual bool HasPoint()
	{
		return false;
	}

	public virtual Mesh MeshObject(MeshingParameters mp, bool productionMeshes = false, bool useDisplayMesh = true)
	{
		return null;
	}

	public virtual bool Morph(SpaceMorph spaceMorph)
	{
		return false;
	}

	public virtual string Report()
	{
		return string.Empty;
	}

	public void SetBitmap(Bitmap bitmap)
	{
		if (bitmap != null)
		{
			_bitmap = new Bitmap(bitmap);
		}
	}

	public virtual void SetMaterialClassificationOnDisplay(MaterialClassification materialClassification)
	{
		DefaultDisplayMaterialClassification = materialClassification;
	}

	public void SetMaterials(CSData csData)
	{
		DefaultDisplayMaterialClassification = csData.DefaultDisplayMaterialClassification;
		CurrentMaterialGroup = csData.CurrentMaterialGroup;
	}

	public virtual void SetOutputObject()
	{
		_isOutputObject = true;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
			_bitmap?.Dispose();
			_bitmap = null;
			_isDisposed = true;
		}
	}

	protected virtual NurbsCurve GetCurveSegment(int segmentNumber, bool fromStart)
	{
		return null;
	}

	protected virtual int GetCurveSegmentCount(bool fromStart)
	{
		return -1;
	}

	protected virtual Image GetImage(int sizeX, int sizeY, System.Drawing.Color color, System.Drawing.Color background)
	{
		if (_bitmap == null)
		{
			return GetBlankImage(sizeX, sizeY, color);
		}
		using Bitmap bitmap = new Bitmap(_bitmap);
		bitmap.MakeTransparent();
		float num = (float)_bitmap.Width / (float)_bitmap.Height;
		return bitmap.GetThumbnailImage((int)((float)sizeX * num), sizeY, ThumbnailCallback, IntPtr.Zero);
	}

	private static Bitmap GetBlankImage(int sizeX, int sizeY, System.Drawing.Color foreColor)
	{
		Bitmap bitmap = new Bitmap(sizeX, sizeY, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
		Graphics.FromImage(bitmap).DrawString(format: new StringFormat
		{
			Alignment = StringAlignment.Center,
			LineAlignment = StringAlignment.Center
		}, s: "NO\nIMAGE", font: new Font("Verdana", 7f), brush: new SolidBrush(foreColor), layoutRectangle: new RectangleF(0f, 0f, sizeX, sizeY));
		return bitmap;
	}

	private static bool ThumbnailCallback()
	{
		return true;
	}

	~CSData()
	{
		Dispose(disposing: false);
	}
}
