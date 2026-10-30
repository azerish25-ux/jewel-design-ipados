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
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
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
		if (((Point3d)(ref value)).DistanceTo(value2) < 0.01)
		{
			return UpdateError("Point to close");
		}
		if (((Point3d)(ref value)).DistanceTo(value3) < 0.01)
		{
			return UpdateError("Point to close");
		}
		if (((Point3d)(ref value)).DistanceTo(value4) < 0.01)
		{
			return UpdateError("Point to close");
		}
		if (((Point3d)(ref value2)).DistanceTo(value3) < 0.01)
		{
			return UpdateError("Point to close");
		}
		if (((Point3d)(ref value2)).DistanceTo(value4) < 0.01)
		{
			return UpdateError("Point to close");
		}
		if (((Point3d)(ref value3)).DistanceTo(value4) < 0.01)
		{
			return UpdateError("Point to close");
		}
		NurbsSurface val = NurbsSurface.CreateFromCorners(value, value2, value3, value4);
		if (val == null)
		{
			return UpdateError("Error creating surface");
		}
		Brep val2 = ((Surface)val).ToBrep();
		oSurface.SetData((val2 != null) ? new CSOnGeometryData((GeometryBase)(object)val2) : new CSData());
		return base.Update();
	}
}
