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
		if (iCurves == null)
		{
			return UpdateMissing("Curves missing");
		}
		NurbsCurve[] array = new NurbsCurve[CSDataHelper.GetArrayCount(iCurves)];
		int arrayCount = CSDataHelper.GetArrayCount(iCurves);
		for (int i = 0; i < arrayCount; i++)
		{
			CSDataHelper.GetNurbsCurve(iCurves.GetArrayItem(i), out array[i]);
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		iCurves.Data.GetTightBoundingBox(ref boundingBox);
		PlaneSurface planeSurface = new PlaneSurface(Plane.WorldXY, new Interval(0.0, 1.0), new Interval(0.0, 1.0));
		Transform xform = Transform.Scale(Plane.WorldXY, boundingBox.Diagonal.X + 2.0, boundingBox.Diagonal.Y + 2.0, boundingBox.Diagonal.Z + 2.0);
		planeSurface.Transform(xform);
		planeSurface.Translate(new Vector3d(boundingBox.Min.X - 1.0, boundingBox.Min.Y - 1.0, boundingBox.Min.Z));
		Brep[] array2 = Brep.CreatePlanarBreps(array);
		Brep[] array3 = null;
		Brep[] array4 = null;
		Vector3d vector3d = new Vector3d(0.0, 0.0, 1.0);
		double value = 0.0;
		iHeight.GetDouble(ref value);
		if ((value > 0.01 || value < 0.01) && array2 != null)
		{
			array3 = new Brep[array.Length];
			array4 = new Brep[array2.Length];
			vector3d = new Vector3d(0.0, 0.0, value);
		}
		if (array3 != null)
		{
			for (int j = 0; j < array3.Length; j++)
			{
				if (array[j] != null)
				{
					Surface surface = Surface.CreateExtrusion(array[j], vector3d);
					if (surface != null)
					{
						array3[j] = surface.ToBrep();
					}
				}
			}
		}
		if (array4 != null)
		{
			for (int k = 0; k < array2.Length; k++)
			{
				array4[k] = array2[k].DuplicateBrep();
				array4[k].Translate(vector3d);
			}
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (array2 != null)
		{
			for (int l = 0; l < array2.Length; l++)
			{
				cSDataArray.Append(new CSOnGeometryData(array2[l]));
			}
		}
		if (array3 != null)
		{
			for (int m = 0; m < array3.Length; m++)
			{
				if (array3[m] != null)
				{
					cSDataArray.Append(new CSOnGeometryData(array3[m]));
				}
			}
		}
		if (array4 != null)
		{
			for (int n = 0; n < array4.Length; n++)
			{
				if (array4[n] != null)
				{
					cSDataArray.Append(new CSOnGeometryData(array4[n]));
				}
			}
		}
		oSolid.SetData(cSDataArray);
		return base.Update();
	}
}
