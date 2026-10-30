using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Surfaces;

[CSBlock("Sphere", "{B0FAD426-1FB5-4bfa-A10E-A586C65D6975}")]
public sealed class CSSphere : CSBlock
{
	public readonly CSInputChannel iRadius;

	public readonly CSOutputChannel oSurface;

	public CSSphere(string identifier)
		: base(identifier)
	{
		iRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		double value = 1.0;
		iRadius.GetDouble(ref value);
		Sphere val = default(Sphere);
		((Sphere)(ref val))._002Ector(Point3d.Origin, value);
		Mesh geometry = ((!CSApplicationHelper.Application.ProductionMeshes) ? Mesh.CreateFromSphere(val, Math.Max(10, (int)value * 30), Math.Max((int)value * 30, 10)) : Mesh.CreateFromSphere(val, Math.Max(40, (int)value * 30 * 4), Math.Max((int)value * 30 * 4, 40)));
		oSurface.SetData(new CSOnGeometryData((GeometryBase)(object)geometry));
		return base.Update();
	}
}
