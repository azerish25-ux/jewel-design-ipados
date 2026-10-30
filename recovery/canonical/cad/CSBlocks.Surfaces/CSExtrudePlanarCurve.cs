using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Surfaces;

[CSBlock("ExtrudePlanarCurve", "{E83F327D-DDAD-48B3-8B53-312119A285ED}")]
public sealed class CSExtrudePlanarCurve : CSBlock
{
	public CSInputChannel InPlanarCurve { get; }

	public CSInputChannel InExtrudeDistance { get; }

	public CSOutputChannel OutSurface { get; }

	public CSExtrudePlanarCurve(string identifier)
		: base(identifier)
	{
		InPlanarCurve = new CSInputChannel(this, ChannelDataType.Curve | ChannelDataType.Profile, ChannelArrayType.Array);
		InExtrudeDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 20.0);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = CSDataHelper.GetFlattenArray(InPlanarCurve.Data);
		double num = InExtrudeDistance.Double;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(i), out var value) && value != null && value.IsPlanar() && value.TryGetPlane(out var plane))
			{
				NurbsCurve nurbsCurve = value.ToNurbsCurve();
				nurbsCurve.Translate(new Vector3d(plane.Normal) * num);
				NurbsCurve nurbsCurve2 = value.ToNurbsCurve();
				nurbsCurve2.Translate(new Vector3d(plane.Normal) * (0.0 - num));
				Brep brep = NurbsSurface.CreateRuledSurface(nurbsCurve, nurbsCurve2).ToBrep();
				if (brep != null)
				{
					cSDataArray.Append(new CSOnGeometryData(brep));
				}
			}
		}
		OutSurface.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}
}
