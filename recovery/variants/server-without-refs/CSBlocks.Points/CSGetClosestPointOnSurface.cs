using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Points;

[CSBlock("GetClosestPointOnSurface", "{5C8727FF-F280-496F-8E7D-DE0A76D9E348}")]
public sealed class CSGetClosestPointOnSurface : CSBlock
{
	public readonly CSInputChannel iPoint;

	public readonly CSInputChannel iSurface;

	public readonly CSOutputChannel oSurfacePoint;

	public readonly CSOutputChannel oUValue;

	public readonly CSOutputChannel oVValue;

	public CSGetClosestPointOnSurface(string identifier)
		: base(identifier)
	{
		iPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		oSurfacePoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oUValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oVValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Invalid comparison between Unknown and I4
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		Point3d value = Point3d.Unset;
		if (!CSDataHelper.GetPoint(iPoint.Data, out value))
		{
			return UpdateMissing("Point");
		}
		CSDataHelper.GetBrep(iSurface.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Surface");
		}
		Point3d unset = Point3d.Unset;
		ComponentIndex val = default(ComponentIndex);
		double num = default(double);
		double num2 = default(double);
		Vector3d val2 = default(Vector3d);
		if (!value2.ClosestPoint(value, ref unset, ref val, ref num, ref num2, 9999.0, ref val2) || unset == Point3d.Unset)
		{
			return UpdateError("Unable to get closest point");
		}
		if ((int)((ComponentIndex)(ref val)).ComponentIndexType == 3)
		{
			BrepFace val3 = value2.Faces[((ComponentIndex)(ref val)).Index];
			double num3 = num;
			Interval val4 = ((Surface)val3).Domain(0);
			double num4 = num3 - ((Interval)(ref val4)).Min;
			val4 = ((Surface)val3).Domain(0);
			num = num4 / ((Interval)(ref val4)).Length;
			double num5 = num2;
			val4 = ((Surface)val3).Domain(1);
			double num6 = num5 - ((Interval)(ref val4)).Min;
			val4 = ((Surface)val3).Domain(1);
			num2 = num6 / ((Interval)(ref val4)).Length;
		}
		oSurfacePoint.SetData(new CSPointData(unset));
		oUValue.SetData(new CSNumberData(num));
		oVValue.SetData(new CSNumberData(num2));
		return base.Update();
	}
}
