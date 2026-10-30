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
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = CSDataHelper.GetFlattenArray(InPlanarCurve.Data);
		double num = InExtrudeDistance.Double;
		CSDataArray cSDataArray = new CSDataArray();
		Plane val = default(Plane);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(i), out var value) && value != null && ((Curve)value).IsPlanar() && ((Curve)value).TryGetPlane(ref val))
			{
				NurbsCurve obj = ((Curve)value).ToNurbsCurve();
				((GeometryBase)obj).Translate(new Vector3d(((Plane)(ref val)).Normal) * num);
				NurbsCurve val2 = ((Curve)value).ToNurbsCurve();
				((GeometryBase)val2).Translate(new Vector3d(((Plane)(ref val)).Normal) * (0.0 - num));
				Brep val3 = ((Surface)NurbsSurface.CreateRuledSurface((Curve)(object)obj, (Curve)(object)val2)).ToBrep();
				if (val3 != null)
				{
					cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val3));
				}
			}
		}
		OutSurface.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}
}
