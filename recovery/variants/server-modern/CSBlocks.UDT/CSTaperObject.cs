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
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_041c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0451: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_0466: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_0479: Unknown result type (might be due to invalid IL or missing references)
			//IL_047e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0482: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04be: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04da: Unknown result type (might be due to invalid IL or missing references)
			//IL_04df: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_0504: Unknown result type (might be due to invalid IL or missing references)
			//IL_0509: Unknown result type (might be due to invalid IL or missing references)
			_ = new Point3d[8]
			{
				((BoundingBox)(ref _boundingBox)).Corner(true, true, true),
				((BoundingBox)(ref _boundingBox)).Corner(true, true, false),
				((BoundingBox)(ref _boundingBox)).Corner(true, false, true),
				((BoundingBox)(ref _boundingBox)).Corner(true, false, false),
				((BoundingBox)(ref _boundingBox)).Corner(false, true, true),
				((BoundingBox)(ref _boundingBox)).Corner(false, true, false),
				((BoundingBox)(ref _boundingBox)).Corner(false, false, true),
				((BoundingBox)(ref _boundingBox)).Corner(false, false, false)
			};
			Point3d[] array = (Point3d[])(object)new Point3d[8]
			{
				((BoundingBox)(ref _boundingBox)).Corner(true, true, true),
				((BoundingBox)(ref _boundingBox)).Corner(true, true, false),
				((BoundingBox)(ref _boundingBox)).Corner(true, false, true),
				((BoundingBox)(ref _boundingBox)).Corner(true, false, false),
				((BoundingBox)(ref _boundingBox)).Corner(false, true, true),
				((BoundingBox)(ref _boundingBox)).Corner(false, true, false),
				((BoundingBox)(ref _boundingBox)).Corner(false, false, true),
				((BoundingBox)(ref _boundingBox)).Corner(false, false, false)
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (i == 3 || i == 7 || i == 1 || i == 7)
				{
					ref Point3d reference = ref array[i];
					((Point3d)(ref reference)).X = ((Point3d)(ref reference)).X * _zMaxTaper;
					ref Point3d reference2 = ref array[i];
					((Point3d)(ref reference2)).Y = ((Point3d)(ref reference2)).Y * _zMaxTaper;
				}
				if (i == 3 || i == 7 || i == 2 || i == 6)
				{
					ref Point3d reference3 = ref array[i];
					((Point3d)(ref reference3)).X = ((Point3d)(ref reference3)).X * _yMaxTaper;
					ref Point3d reference4 = ref array[i];
					((Point3d)(ref reference4)).Z = ((Point3d)(ref reference4)).Z * _yMaxTaper;
				}
				if (i > 4 && i < 8)
				{
					ref Point3d reference5 = ref array[i];
					((Point3d)(ref reference5)).Z = ((Point3d)(ref reference5)).Z * _xMaxTaper;
					ref Point3d reference6 = ref array[i];
					((Point3d)(ref reference6)).Y = ((Point3d)(ref reference6)).Y * _xMaxTaper;
				}
			}
			Point3d val = ((BoundingBox)(ref _boundingBox)).Min;
			double x = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref _boundingBox)).Min;
			double y = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref _boundingBox)).Min;
			Point3d val2 = new Point3d(x, y, ((Point3d)(ref val)).Z);
			val = ((BoundingBox)(ref _boundingBox)).Max;
			double x2 = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref _boundingBox)).Min;
			double y2 = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref _boundingBox)).Min;
			Line val3 = default(Line);
			((Line)(ref val3))._002Ector(val2, new Point3d(x2, y2, ((Point3d)(ref val)).Z));
			val = ((BoundingBox)(ref _boundingBox)).Min;
			double x3 = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref _boundingBox)).Min;
			double y3 = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref _boundingBox)).Min;
			Point3d val4 = new Point3d(x3, y3, ((Point3d)(ref val)).Z);
			val = ((BoundingBox)(ref _boundingBox)).Min;
			double x4 = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref _boundingBox)).Max;
			double y4 = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref _boundingBox)).Min;
			Line val5 = default(Line);
			((Line)(ref val5))._002Ector(val4, new Point3d(x4, y4, ((Point3d)(ref val)).Z));
			val = ((BoundingBox)(ref _boundingBox)).Min;
			double x5 = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref _boundingBox)).Min;
			double y5 = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref _boundingBox)).Min;
			Point3d val6 = new Point3d(x5, y5, ((Point3d)(ref val)).Z);
			val = ((BoundingBox)(ref _boundingBox)).Min;
			double x6 = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref _boundingBox)).Min;
			double y6 = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref _boundingBox)).Max;
			Line val7 = default(Line);
			((Line)(ref val7))._002Ector(val6, new Point3d(x6, y6, ((Point3d)(ref val)).Z));
			double num = ((Line)(ref val3)).ClosestParameter(inputPoint);
			double num2 = ((Line)(ref val5)).ClosestParameter(inputPoint);
			double num3 = ((Line)(ref val7)).ClosestParameter(inputPoint);
			Point3d val8 = array[0] * (1.0 - num) + array[4] * num;
			Point3d val9 = array[1] * (1.0 - num) + array[5] * num;
			Point3d val10 = array[2] * (1.0 - num) + array[6] * num;
			Point3d val11 = array[3] * (1.0 - num) + array[7] * num;
			Point3d val12 = val8 * (1.0 - num2) + val10 * num2;
			Point3d val13 = val9 * (1.0 - num2) + val11 * num2;
			return val12 * (1.0 - num3) + val13 * num3;
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
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
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
			if (!((Transform)(ref value)).IsValid)
			{
				value = Transform.Identity;
			}
		}
		else
		{
			value = Transform.Identity;
		}
		if (!((Transform)(ref value)).IsValid)
		{
			return UpdateError("Invalid Xform");
		}
		TaperObject taperObject = new TaperObject();
		((SpaceMorph)taperObject).Tolerance = 0.05;
		TaperObject taperObject2 = taperObject;
		InScaleMaxX.GetDouble(ref taperObject2._xMaxTaper);
		InScaleMaxY.GetDouble(ref taperObject2._yMaxTaper);
		InScaleMaxZ.GetDouble(ref taperObject2._zMaxTaper);
		Transform transform = default(Transform);
		((Transform)(ref value)).TryGetInverse(ref transform);
		CSDataHelper.Transform(data, transform);
		data.GetTightBoundingBox(ref taperObject2._boundingBox);
		Vector3d diagonal = ((BoundingBox)(ref taperObject2._boundingBox)).Diagonal;
		if (((Vector3d)(ref diagonal)).Z < 0.01)
		{
			taperObject2._zMaxTaper = 1.0;
		}
		diagonal = ((BoundingBox)(ref taperObject2._boundingBox)).Diagonal;
		if (((Vector3d)(ref diagonal)).Y < 0.01)
		{
			taperObject2._yMaxTaper = 1.0;
		}
		diagonal = ((BoundingBox)(ref taperObject2._boundingBox)).Diagonal;
		if (((Vector3d)(ref diagonal)).X < 0.01)
		{
			taperObject2._xMaxTaper = 1.0;
		}
		data.Morph((SpaceMorph)(object)taperObject2);
		CSDataHelper.Transform(data, value);
		OutObject.SetData(data);
		return base.Update();
	}
}
