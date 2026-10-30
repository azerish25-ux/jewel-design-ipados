using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Settings.FrenchHalo;

[CSBlock("MakeBeads", "{DDBCBF77-7057-449B-94A6-D9409C62E703}")]
public sealed class CSMakeBeads : CSBlock
{
	public readonly CSInputChannel InBeadPositionScale;

	public readonly CSInputChannel InBeadScale;

	public readonly CSInputChannel InBeadScaleHeight;

	public readonly CSInputChannel InGems;

	public readonly CSOutputChannel OutBead;

	public readonly CSOutputChannel OutGem;

	public CSMakeBeads(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InBeadScale = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InBeadScaleHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InBeadPositionScale = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutBead = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		List<CSGemData> gemList = InGems.GemList;
		double num = InBeadScale.Double;
		double num2 = InBeadScaleHeight.Double;
		double num3 = InBeadPositionScale.Double;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		foreach (CSGemData item in gemList)
		{
			string value = "";
			if (item.GetUserString("IsBead", ref value) && value == "1")
			{
				NurbsSurface nurbsSurface = new Sphere(Point3d.Origin, 1.0).ToNurbsSurface();
				double num4 = ((item.GemXSize < item.GemYSize) ? (item.GemXSize / 2.0) : (item.GemYSize / 2.0));
				double num5 = num4 * num;
				double num6 = num4 * num * num2;
				Transform transform = Transform.Scale(Plane.WorldXY, num5, num5, num6);
				Transform transform2 = Transform.Translation(new Vector3d(item.ZMaxPoint) + new Vector3d(0.0, 0.0, 0.0 - num6 - item.GemZSize * num3));
				nurbsSurface.Transform(transform2 * transform);
				cSDataArray2.Append(new CSOnGeometryData(nurbsSurface));
			}
			else
			{
				cSDataArray.Append(item);
			}
		}
		OutGem.SetData(cSDataArray);
		OutBead.SetData(cSDataArray2);
		return base.Update();
	}
}
