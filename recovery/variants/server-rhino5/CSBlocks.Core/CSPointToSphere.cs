using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("PointToSphere", "{6D7C8434-F26E-4b1e-896E-8AD1AA4CD7CA}")]
public sealed class CSPointToSphere : CSBlock
{
	public CSInputChannel iObjects { get; }

	public CSInputChannel iRadius { get; }

	public CSInputChannel iEndRadius { get; }

	public CSInputChannel iMiddleRadius { get; }

	public CSOutputChannel oObjects { get; }

	public CSPointToSphere(string identifier)
		: base(identifier)
	{
		iObjects = new CSInputChannel(GetPropertyName(() => iObjects), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		iRadius = new CSInputChannel(GetPropertyName(() => iRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndRadius = new CSInputChannel(GetPropertyName(() => iEndRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iMiddleRadius = new CSInputChannel(GetPropertyName(() => iMiddleRadius), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(GetPropertyName(() => oObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		double value = 1.0;
		iRadius.GetDouble(ref value);
		double value2 = value;
		iEndRadius.GetDouble(ref value2);
		double value3 = (value + value2) / 2.0;
		iMiddleRadius.GetDouble(ref value3);
		CSDataArray cSDataArray = new CSDataArray();
		Point3d value4 = Point3d.Unset;
		Polyline polyline = new Polyline();
		polyline.Add(new Point3d(0.0, value, 0.0));
		polyline.Add(new Point3d(1.0, value3, 0.0));
		polyline.Add(new Point3d(2.0, value2, 0.0));
		bool flag = true;
		int arrayCount = CSDataHelper.GetArrayCount(iObjects);
		for (int i = 0; i < arrayCount; i++)
		{
			if (CSDataHelper.GetFlattenArray(iObjects.GetArrayItem(i)).GetArrayCount() > 2)
			{
				flag = false;
				break;
			}
		}
		if (arrayCount == 0 || flag)
		{
			CSDataArray flattenArray = iObjects.GetFlattenArray();
			double num = flattenArray.GetArrayCount();
			for (int j = 0; (double)j < num; j++)
			{
				if (CSDataHelper.GetPoint(flattenArray.GetArrayItem(j), out value4))
				{
					double num2 = value;
					if (num > 1.0)
					{
						double num3 = (double)j / (num - 1.0);
						num2 = polyline.PointAt(num3 * 1.99).Y;
					}
					Sphere sphere = new Sphere(value4, num2);
					int val = 8 + (int)(num2 * 16.0);
					val = Math.Min(val, 60);
					if (CSApplicationHelper.Application.ProductionMeshes)
					{
						val *= 2;
					}
					cSDataArray.Append(new CSOnGeometryData(Mesh.CreateFromSphere(sphere, val, val)));
				}
			}
		}
		else
		{
			for (int k = 0; k < arrayCount; k++)
			{
				CSDataArray flattenArray2 = CSDataHelper.GetFlattenArray(iObjects.GetArrayItem(k));
				double num4 = flattenArray2.GetArrayCount();
				for (int l = 0; (double)l < num4; l++)
				{
					double num5 = value;
					if (num4 > 1.0)
					{
						double num6 = (double)l / (num4 - 1.0);
						num5 = polyline.PointAt(num6 * 1.99).Y;
					}
					if (CSDataHelper.GetPoint(flattenArray2.GetArrayItem(l), out value4))
					{
						Sphere sphere2 = new Sphere(value4, num5);
						int val2 = 8 + (int)(num5 * 16.0);
						val2 = Math.Min(val2, 60);
						if (CSApplicationHelper.Application.ProductionMeshes)
						{
							val2 *= 2;
						}
						cSDataArray.Append(new CSOnGeometryData(Mesh.CreateFromSphere(sphere2, val2, val2)));
					}
				}
			}
		}
		oObjects.SetData(cSDataArray);
		return base.Update();
	}
}
