using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("PlaneXYCurvesToSolid", "{B357A67A-3B9C-47c6-BC0D-7DE7531030C9}")]
public sealed class CSPlaneXYCurvesToSolid : CSBlock
{
	public readonly CSInputChannel iCurves;

	public readonly CSInputChannel iHeight;

	public readonly CSOutputChannel oSolid;

	public CSPlaneXYCurvesToSolid(string identifier)
		: base(identifier)
	{
		iCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		iHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSolid = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		if (iCurves == null)
		{
			return UpdateMissing("Curves missing");
		}
		NurbsCurve[] array = (NurbsCurve[])(object)new NurbsCurve[CSDataHelper.GetArrayCount(iCurves)];
		int arrayCount = CSDataHelper.GetArrayCount(iCurves);
		for (int i = 0; i < arrayCount; i++)
		{
			CSDataHelper.GetNurbsCurve(iCurves.GetArrayItem(i), out array[i]);
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		iCurves.Data.GetTightBoundingBox(ref boundingBox);
		PlaneSurface val = new PlaneSurface(Plane.WorldXY, new Interval(0.0, 1.0), new Interval(0.0, 1.0));
		Plane worldXY = Plane.WorldXY;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num = ((Vector3d)(ref diagonal)).X + 2.0;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num2 = ((Vector3d)(ref diagonal)).Y + 2.0;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		Transform val2 = Transform.Scale(worldXY, num, num2, ((Vector3d)(ref diagonal)).Z + 2.0);
		((GeometryBase)val).Transform(val2);
		Point3d min = ((BoundingBox)(ref boundingBox)).Min;
		double num3 = ((Point3d)(ref min)).X - 1.0;
		min = ((BoundingBox)(ref boundingBox)).Min;
		double num4 = ((Point3d)(ref min)).Y - 1.0;
		min = ((BoundingBox)(ref boundingBox)).Min;
		((GeometryBase)val).Translate(new Vector3d(num3, num4, ((Point3d)(ref min)).Z));
		Brep[] array2 = Brep.CreatePlanarBreps((IEnumerable<Curve>)(object)array);
		Brep[] array3 = null;
		Brep[] array4 = null;
		Vector3d val3 = default(Vector3d);
		((Vector3d)(ref val3))._002Ector(0.0, 0.0, 1.0);
		double value = 0.0;
		iHeight.GetDouble(ref value);
		if ((value > 0.01 || value < 0.01) && array2 != null)
		{
			array3 = (Brep[])(object)new Brep[array.Length];
			array4 = (Brep[])(object)new Brep[array2.Length];
			((Vector3d)(ref val3))._002Ector(0.0, 0.0, value);
		}
		if (array3 != null)
		{
			for (int j = 0; j < array3.Length; j++)
			{
				if (array[j] != null)
				{
					Surface val4 = Surface.CreateExtrusion((Curve)(object)array[j], val3);
					if (val4 != null)
					{
						array3[j] = val4.ToBrep();
					}
				}
			}
		}
		if (array4 != null)
		{
			for (int k = 0; k < array2.Length; k++)
			{
				array4[k] = array2[k].DuplicateBrep();
				((GeometryBase)array4[k]).Translate(val3);
			}
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (array2 != null)
		{
			for (int l = 0; l < array2.Length; l++)
			{
				cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)array2[l]));
			}
		}
		if (array3 != null)
		{
			for (int m = 0; m < array3.Length; m++)
			{
				if (array3[m] != null)
				{
					cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)array3[m]));
				}
			}
		}
		if (array4 != null)
		{
			for (int n = 0; n < array4.Length; n++)
			{
				if (array4[n] != null)
				{
					cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)array4[n]));
				}
			}
		}
		oSolid.SetData(cSDataArray);
		return base.Update();
	}
}
