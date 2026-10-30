using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.UDT;

[CSBlock("TaperObject", "{62F266F8-73ED-4b4a-A765-044366F2802A}")]
public sealed class CSTaperObject : CSBlock
{
	private sealed class TaperObject : CSSpaceMorph
	{
		public BoundingBox _boundingBox = BoundingBox.Unset;

		public double _xMaxTaper = 1.0;

		public double _yMaxTaper = 1.0;

		public double _zMaxTaper = 1.0;

		public override Point3d MorphPoint(Point3d inputPoint)
		{
			_ = new Point3d[8]
			{
				_boundingBox.Corner(minX: true, minY: true, minZ: true),
				_boundingBox.Corner(minX: true, minY: true, minZ: false),
				_boundingBox.Corner(minX: true, minY: false, minZ: true),
				_boundingBox.Corner(minX: true, minY: false, minZ: false),
				_boundingBox.Corner(minX: false, minY: true, minZ: true),
				_boundingBox.Corner(minX: false, minY: true, minZ: false),
				_boundingBox.Corner(minX: false, minY: false, minZ: true),
				_boundingBox.Corner(minX: false, minY: false, minZ: false)
			};
			Point3d[] array = new Point3d[8]
			{
				_boundingBox.Corner(minX: true, minY: true, minZ: true),
				_boundingBox.Corner(minX: true, minY: true, minZ: false),
				_boundingBox.Corner(minX: true, minY: false, minZ: true),
				_boundingBox.Corner(minX: true, minY: false, minZ: false),
				_boundingBox.Corner(minX: false, minY: true, minZ: true),
				_boundingBox.Corner(minX: false, minY: true, minZ: false),
				_boundingBox.Corner(minX: false, minY: false, minZ: true),
				_boundingBox.Corner(minX: false, minY: false, minZ: false)
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (i == 3 || i == 7 || i == 1 || i == 7)
				{
					array[i].X *= _zMaxTaper;
					array[i].Y *= _zMaxTaper;
				}
				if (i == 3 || i == 7 || i == 2 || i == 6)
				{
					array[i].X *= _yMaxTaper;
					array[i].Z *= _yMaxTaper;
				}
				if (i > 4 && i < 8)
				{
					array[i].Z *= _xMaxTaper;
					array[i].Y *= _xMaxTaper;
				}
			}
			Line line = new Line(new Point3d(_boundingBox.Min.X, _boundingBox.Min.Y, _boundingBox.Min.Z), new Point3d(_boundingBox.Max.X, _boundingBox.Min.Y, _boundingBox.Min.Z));
			Line line2 = new Line(new Point3d(_boundingBox.Min.X, _boundingBox.Min.Y, _boundingBox.Min.Z), new Point3d(_boundingBox.Min.X, _boundingBox.Max.Y, _boundingBox.Min.Z));
			Line line3 = new Line(new Point3d(_boundingBox.Min.X, _boundingBox.Min.Y, _boundingBox.Min.Z), new Point3d(_boundingBox.Min.X, _boundingBox.Min.Y, _boundingBox.Max.Z));
			double num = line.ClosestParameter(inputPoint);
			double num2 = line2.ClosestParameter(inputPoint);
			double num3 = line3.ClosestParameter(inputPoint);
			Point3d point3d = array[0] * (1.0 - num) + array[4] * num;
			Point3d point3d2 = array[1] * (1.0 - num) + array[5] * num;
			Point3d point3d3 = array[2] * (1.0 - num) + array[6] * num;
			Point3d point3d4 = array[3] * (1.0 - num) + array[7] * num;
			Point3d point3d5 = point3d * (1.0 - num2) + point3d3 * num2;
			Point3d point3d6 = point3d2 * (1.0 - num2) + point3d4 * num2;
			return point3d5 * (1.0 - num3) + point3d6 * num3;
		}
	}

	public CSInputChannel InObject { get; }

	public CSInputChannel InScaleMaxX { get; }

	public CSInputChannel InScaleMaxY { get; }

	public CSInputChannel InScaleMaxZ { get; }

	public CSInputChannel InXform { get; }

	public CSOutputChannel OutObject { get; }

	public CSTaperObject(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InScaleMaxX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InScaleMaxY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InScaleMaxZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InXform = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (InObject == null)
		{
			return UpdateMissing("Missing Object");
		}
		CSData data = InObject.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Object");
		}
		data = data.Duplicate();
		Transform value;
		if (InXform != null)
		{
			CSDataHelper.GetTransformNoArray(InXform.Data, out value);
			if (!value.IsValid)
			{
				value = Transform.Identity;
			}
		}
		else
		{
			value = Transform.Identity;
		}
		if (!value.IsValid)
		{
			return UpdateError("Invalid Xform");
		}
		TaperObject taperObject = new TaperObject
		{
			Tolerance = 0.05
		};
		InScaleMaxX.GetDouble(ref taperObject._xMaxTaper);
		InScaleMaxY.GetDouble(ref taperObject._yMaxTaper);
		InScaleMaxZ.GetDouble(ref taperObject._zMaxTaper);
		value.TryGetInverse(out var inverseTransform);
		CSDataHelper.Transform(data, inverseTransform);
		data.GetTightBoundingBox(ref taperObject._boundingBox);
		if (taperObject._boundingBox.Diagonal.Z < 0.01)
		{
			taperObject._zMaxTaper = 1.0;
		}
		if (taperObject._boundingBox.Diagonal.Y < 0.01)
		{
			taperObject._yMaxTaper = 1.0;
		}
		if (taperObject._boundingBox.Diagonal.X < 0.01)
		{
			taperObject._xMaxTaper = 1.0;
		}
		data.Morph(taperObject);
		CSDataHelper.Transform(data, value);
		OutObject.SetData(data);
		return base.Update();
	}
}
