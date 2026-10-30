using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("FlatSurface", "{3E63C8BF-93F5-4f13-A770-789649C63672}")]
public sealed class CSFlatSurface : CSBlock
{
	public readonly CSInputChannel iPoint1;

	public readonly CSInputChannel iPoint2;

	public readonly CSInputChannel iPoint3;

	public readonly CSInputChannel iPoint4;

	public readonly CSOutputChannel oSurface;

	public CSFlatSurface(string identifier)
		: base(identifier)
	{
		iPoint1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPoint2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPoint3 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPoint4 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!iPoint1.GetValue(out Point3d value))
		{
			return InputError();
		}
		if (!iPoint2.GetValue(out Point3d value2))
		{
			return InputError();
		}
		if (!iPoint3.GetValue(out Point3d value3))
		{
			return InputError();
		}
		if (!iPoint4.GetValue(out Point3d value4))
		{
			return InputError();
		}
		if (value.DistanceTo(value2) < 0.01)
		{
			return UpdateError("Point to close");
		}
		if (value.DistanceTo(value3) < 0.01)
		{
			return UpdateError("Point to close");
		}
		if (value.DistanceTo(value4) < 0.01)
		{
			return UpdateError("Point to close");
		}
		if (value2.DistanceTo(value3) < 0.01)
		{
			return UpdateError("Point to close");
		}
		if (value2.DistanceTo(value4) < 0.01)
		{
			return UpdateError("Point to close");
		}
		if (value3.DistanceTo(value4) < 0.01)
		{
			return UpdateError("Point to close");
		}
		NurbsSurface nurbsSurface = NurbsSurface.CreateFromCorners(value, value2, value3, value4);
		if (nurbsSurface == null)
		{
			return UpdateError("Error creating surface");
		}
		Brep brep = nurbsSurface.ToBrep();
		oSurface.SetData((brep != null) ? new CSOnGeometryData(brep) : new CSData());
		return base.Update();
	}
}
