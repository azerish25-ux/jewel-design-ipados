using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Bails.Obsolete;

[Obsolete]
[CSBlock("BailPlacerXform_V1", "{664ADC98-A7A3-4135-9ED2-71C8598E3FE5}")]
public sealed class CSBailPlacerXform1 : CSBlock
{
	public CSInputChannel InObject { get; }

	public CSOutputChannel OutXform { get; }

	public CSBailPlacerXform1(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InObject.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Mising Object");
		}
		List<Brep> list = new List<Brep>();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			Brep val = flattenArray.GetArrayItem(i)?.GetBrep();
			if (val != null)
			{
				list.Add(val);
			}
		}
		BoundingBox val2 = default(BoundingBox);
		foreach (Brep item2 in list)
		{
			((BoundingBox)(ref val2)).Union(((GeometryBase)item2).GetBoundingBox(false));
		}
		Point3d val3 = ((BoundingBox)(ref val2)).Min;
		double y = ((Point3d)(ref val3)).Y;
		val3 = ((BoundingBox)(ref val2)).Min;
		Point3d val4 = new Point3d(0.0, y, ((Point3d)(ref val3)).Z);
		val3 = ((BoundingBox)(ref val2)).Max;
		double y2 = ((Point3d)(ref val3)).Y;
		val3 = ((BoundingBox)(ref val2)).Min;
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(0.0, y2, ((Point3d)(ref val3)).Z);
		val3 = ((BoundingBox)(ref val2)).Max;
		double y3 = ((Point3d)(ref val3)).Y;
		val3 = ((BoundingBox)(ref val2)).Max;
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector(0.0, y3, ((Point3d)(ref val3)).Z);
		val3 = ((BoundingBox)(ref val2)).Min;
		double y4 = ((Point3d)(ref val3)).Y;
		val3 = ((BoundingBox)(ref val2)).Max;
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector(0.0, y4, ((Point3d)(ref val3)).Z);
		NurbsSurface val8 = NurbsSurface.CreateFromCorners(val4, val5, val6, val7);
		if (val8 == null)
		{
			return UpdateError("Intersection Failed");
		}
		Brep val9 = ((Surface)val8).ToBrep();
		if (val9 == null)
		{
			return UpdateError("Intersection Failed to Brep");
		}
		List<Curve> list2 = new List<Curve>();
		List<Point3d> list3 = new List<Point3d>();
		BoundingBox empty = BoundingBox.Empty;
		Curve[] array = default(Curve[]);
		Point3d[] collection = default(Point3d[]);
		foreach (Brep item3 in list)
		{
			if (Intersection.BrepBrep(item3, val9, CSApplicationHelper.Application.Tolerance, ref array, ref collection))
			{
				Curve[] array2 = array;
				foreach (Curve val10 in array2)
				{
					list2.Add(val10);
					((BoundingBox)(ref empty)).Union(((GeometryBase)val10).GetBoundingBox(true));
				}
				list3.AddRange(collection);
			}
		}
		val3 = ((BoundingBox)(ref empty)).Max;
		double num = ((Point3d)(ref val3)).Y - 0.001;
		val3 = ((BoundingBox)(ref val2)).Min;
		Point3d val11 = new Point3d(0.0, num, ((Point3d)(ref val3)).Z);
		val3 = ((BoundingBox)(ref empty)).Max;
		double y5 = ((Point3d)(ref val3)).Y;
		val3 = ((BoundingBox)(ref val2)).Min;
		((Point3d)(ref val5))._002Ector(0.0, y5, ((Point3d)(ref val3)).Z);
		val3 = ((BoundingBox)(ref empty)).Max;
		double y6 = ((Point3d)(ref val3)).Y;
		val3 = ((BoundingBox)(ref val2)).Max;
		((Point3d)(ref val6))._002Ector(0.0, y6, ((Point3d)(ref val3)).Z);
		val3 = ((BoundingBox)(ref empty)).Max;
		double num2 = ((Point3d)(ref val3)).Y - 0.001;
		val3 = ((BoundingBox)(ref val2)).Max;
		((Point3d)(ref val7))._002Ector(0.0, num2, ((Point3d)(ref val3)).Z);
		val8 = NurbsSurface.CreateFromCorners(val11, val5, val6, val7);
		foreach (Curve item4 in list2)
		{
			CurveIntersections val12 = Intersection.CurveSurface(item4, (Surface)(object)val8, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val12.Count <= 0)
			{
				continue;
			}
			Point3d item = val12[0].PointA;
			foreach (IntersectionEvent item5 in val12)
			{
				val3 = item5.PointA;
				if (((Point3d)(ref val3)).Y >= ((Point3d)(ref item)).Y)
				{
					item = item4.PointAt(item5.ParameterA);
				}
			}
			list3.Add(item);
		}
		if (list3.Count <= 0)
		{
			return UpdateError("Intersect Failed");
		}
		Point3d val13 = list3[0];
		foreach (Point3d item6 in list3)
		{
			Point3d current4 = item6;
			if (((Point3d)(ref current4)).Y > ((Point3d)(ref val13)).Y)
			{
				val13 = current4;
			}
		}
		Transform transform = Transform.Translation(((Point3d)(ref val13)).X, ((Point3d)(ref val13)).Y, 0.0);
		OutXform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
