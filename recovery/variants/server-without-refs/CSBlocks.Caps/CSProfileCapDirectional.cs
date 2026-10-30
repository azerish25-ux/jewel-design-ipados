using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Caps;

[CSBlock("ProfileCapDirectional", "{45ebe88f-ca61-4e2d-85e0-d0bf67940c85}")]
public sealed class CSProfileCapDirectional : CSBlock
{
	public CSInputChannel iDomeEnabled { get; }

	public CSInputChannel iCurve { get; }

	public CSInputChannel iDomeThickness { get; }

	public CSInputChannel iDirectionCurves { get; }

	public CSInputChannel iWhichEndOfCurves { get; }

	public CSInputChannel iDirection { get; }

	public CSOutputChannel oBrep { get; }

	public CSProfileCapDirectional(string identifier)
		: base(identifier)
	{
		iDomeEnabled = new CSInputChannel(GetPropertyName(() => iDomeEnabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iDomeThickness = new CSInputChannel(GetPropertyName(() => iDomeThickness), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iDirectionCurves = new CSInputChannel(GetPropertyName(() => iDirectionCurves), this, ChannelDataType.Curve, ChannelArrayType.Array);
		iWhichEndOfCurves = new CSInputChannel(GetPropertyName(() => iWhichEndOfCurves), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		iDirection = new CSInputChannel(GetPropertyName(() => iDirection), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public static NurbsSurface RailRevolve(NurbsCurve domecrv, NurbsCurve profile, Vector3d[] v)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox boundingBox = ((GeometryBase)profile).GetBoundingBox(true);
		NurbsSurface val = NurbsSurface.Create(3, true, domecrv.Order, profile.Order, domecrv.Points.Count, profile.Points.Count);
		int count = profile.Points.Count;
		int count2 = domecrv.Points.Count;
		Point3d val3 = default(Point3d);
		for (int i = 0; i < count2; i++)
		{
			ControlPoint val2 = domecrv.Points[i];
			Point3d location = ((ControlPoint)(ref val2)).Location;
			((Point3d)(ref val3))._002Ector(location);
			((Point3d)(ref val3)).Z = 0.0;
			Point3d val4 = val3;
			double num = ((Point3d)(ref val4)).DistanceTo(Point3d.Origin);
			for (int j = 0; j < count; j++)
			{
				val2 = profile.Points[j];
				Point3d p = ((ControlPoint)(ref val2)).Location;
				((Point3d)(ref p)).X = ((Point3d)(ref p)).X * num;
				((Point3d)(ref p)).Y = ((Point3d)(ref p)).Y * num;
				((Point3d)(ref p)).Z = ((Point3d)(ref location)).Z;
				SetTangent(ref p, v, boundingBox);
				val2 = domecrv.Points[i];
				double weight = ((ControlPoint)(ref val2)).Weight;
				val2 = profile.Points[j];
				double num2 = weight * ((ControlPoint)(ref val2)).Weight;
				val.Points.SetControlPoint(i, j, new ControlPoint(p, num2));
			}
		}
		for (int k = 0; k < domecrv.Knots.Count; k++)
		{
			val.KnotsU[k] = domecrv.Knots[k];
		}
		for (int l = 0; l < profile.Knots.Count; l++)
		{
			val.KnotsV[l] = profile.Knots[l];
		}
		return val;
	}

	public override bool Update()
	{
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		oBrep.SetData(null);
		bool value = false;
		iDomeEnabled.GetBoolean(ref value);
		if (!value)
		{
			return base.Update();
		}
		CSDataHelper.GetCurve(iCurve, out var value2);
		if (value2 == null)
		{
			return base.Update();
		}
		CSData data = iDirectionCurves.Data;
		if (data == null)
		{
			return base.Update();
		}
		if (!(data is CSDataArray cSDataArray))
		{
			return base.Update();
		}
		double value3 = 0.5;
		if (iDomeThickness != null)
		{
			iDomeThickness.GetDouble(ref value3);
		}
		int value4 = 0;
		iWhichEndOfCurves.GetInteger(ref value4);
		int value5 = 1;
		iDirection.GetInteger(ref value5);
		if (value5 == -1)
		{
			value4 = 1 - value4;
		}
		CSDataHelper.GetCurve(cSDataArray.GetArrayItem(0), out var value6);
		CSDataHelper.GetCurve(cSDataArray.GetArrayItem(1), out var value7);
		CSDataHelper.GetCurve(cSDataArray.GetArrayItem(2), out var value8);
		CSDataHelper.GetCurve(cSDataArray.GetArrayItem(3), out var value9);
		if (value4 == 1)
		{
			value6.Reverse();
			value7.Reverse();
			value8.Reverse();
			value9.Reverse();
		}
		Plane val = default(Plane);
		((Plane)(ref val))._002Ector(value9.PointAtStart, value6.PointAtStart, value8.PointAtStart);
		if (value4 == 1)
		{
			Curve obj = value6;
			value6 = value9;
			value9 = obj;
			Curve obj2 = value7;
			value7 = value8;
			value8 = obj2;
			value3 = 0.0 - value3;
		}
		NurbsCurve val2 = value2.ToNurbsCurve();
		Transform val3 = Transform.ChangeBasis(Plane.WorldXY, val);
		((GeometryBase)val2).Transform(val3);
		BoundingBox boundingBox = ((GeometryBase)val2).GetBoundingBox(true);
		Transform val4 = Transform.Translation(new Vector3d(-((BoundingBox)(ref boundingBox)).Center));
		((GeometryBase)val2).Transform(val4);
		Transform val5 = val4 * val3;
		Transform val6 = default(Transform);
		((Transform)(ref val5)).TryGetInverse(ref val6);
		((GeometryBase)value6).Transform(val5);
		((GeometryBase)value7).Transform(val5);
		((GeometryBase)value8).Transform(val5);
		((GeometryBase)value9).Transform(val5);
		Vector3d tangentAtStart = value6.TangentAtStart;
		Vector3d tangentAtStart2 = value7.TangentAtStart;
		Vector3d tangentAtStart3 = value8.TangentAtStart;
		Vector3d tangentAtStart4 = value9.TangentAtStart;
		NurbsCurve domecrv = CSUtilGeometry.CreateDomeRC(value3, 1.0);
		Vector3d[] v = (Vector3d[])(object)new Vector3d[4] { tangentAtStart, tangentAtStart2, tangentAtStart3, tangentAtStart4 };
		NurbsSurface val7 = RailRevolve(domecrv, val2, v);
		((GeometryBase)val7).Transform(val6);
		oBrep.SetData(new CSOnGeometryData((GeometryBase)(object)((Surface)val7).ToBrep(), val6, applyTransform: false));
		return base.Update();
	}

	private static void SetTangent(ref Point3d p, Vector3d[] v, BoundingBox BB)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		double x = ((Point3d)(ref p)).X;
		double y = ((Point3d)(ref p)).Y;
		double num = x;
		Point3d min = ((BoundingBox)(ref BB)).Min;
		x = num - ((Point3d)(ref min)).X;
		double num2 = y;
		min = ((BoundingBox)(ref BB)).Min;
		y = num2 - ((Point3d)(ref min)).Y;
		double num3 = x;
		Vector3d diagonal = ((BoundingBox)(ref BB)).Diagonal;
		x = num3 / ((Vector3d)(ref diagonal)).X;
		double num4 = y;
		diagonal = ((BoundingBox)(ref BB)).Diagonal;
		y = num4 / ((Vector3d)(ref diagonal)).Y;
		Vector3d val = v[0] * (1.0 - x) * (1.0 - y) + v[1] * (1.0 - x) * y + v[2] * x * y + v[3] * x * (1.0 - y);
		((Point3d)(ref p)).X = ((Point3d)(ref p)).X + ((Vector3d)(ref val)).X * ((Point3d)(ref p)).Z;
		((Point3d)(ref p)).Y = ((Point3d)(ref p)).Y + ((Vector3d)(ref val)).Y * ((Point3d)(ref p)).Z;
	}
}
