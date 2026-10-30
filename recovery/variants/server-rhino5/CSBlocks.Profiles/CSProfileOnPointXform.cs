using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSBlocks.Profiles;

[CSBlock("ProfileOnPointXform", "{A14C5E8A-8924-4842-B79E-FB1D26AEA485}")]
public sealed class CSProfileOnPointXform : CSBlock
{
	public readonly CSInputChannel InPoint;

	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InPlacementMethod;

	public readonly CSOutputChannel OutXform;

	public CSProfileOnPointXform(string identifier)
		: base(identifier)
	{
		InPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetPoint(InPoint, out var value);
		if (value == Point3d.Unset)
		{
			return UpdateMissing("Point");
		}
		NurbsCurve value2;
		if (!(InCurve.Data is ICSArray))
		{
			CSDataHelper.GetNurbsCurve(InCurve.Data, out value2);
		}
		else
		{
			value2 = null;
		}
		int value3 = 0;
		InPlacementMethod.GetInteger(ref value3);
		Vector3d vector3d = Vector3d.ZAxis;
		if (value2 != null)
		{
			if (!value2.ClosestPoint(value, out var t))
			{
				return UpdateError("Failed to get closest point");
			}
			vector3d = value2.TangentAt(t);
		}
		Transform transform = default(Transform);
		switch (value3)
		{
		case 0:
		{
			Vector3d destination9 = new Vector3d(value) * -1.0;
			transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, new Vector3d(Vector3d.YAxis) * -1.0, value, vector3d, destination9);
			break;
		}
		case 1:
		{
			Vector3d destination8 = new Vector3d(value) * -1.0;
			destination8.Y = 0.0;
			transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis * -1.0, value, vector3d, destination8);
			break;
		}
		case 2:
		{
			Vector3d destination7 = Vector3d.CrossProduct(vector3d, new Vector3d(value) * -1.0);
			transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis * -1.0, value, vector3d, destination7);
			break;
		}
		case 3:
		{
			Vector3d destination6 = Vector3d.CrossProduct(vector3d, new Vector3d(value) * -1.0);
			destination6.Y = 0.0;
			transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis * -1.0, value, vector3d, destination6);
			break;
		}
		case 4:
		{
			Vector3d destination5 = Vector3d.CrossProduct(vector3d, new Vector3d(value) * -1.0) * -1.0;
			transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis * -1.0, value, vector3d, destination5);
			break;
		}
		case 5:
		{
			Vector3d destination4 = Vector3d.CrossProduct(vector3d, new Vector3d(value) * -1.0) * -1.0;
			destination4.Y = 0.0;
			transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis * -1.0, value, vector3d, destination4);
			break;
		}
		case 6:
		{
			Vector3d xAxis = Vector3d.XAxis;
			transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis * -1.0, value, vector3d, xAxis);
			break;
		}
		case 7:
		{
			Vector3d destination3 = Vector3d.XAxis * -1.0;
			transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis * -1.0, value, vector3d, destination3);
			break;
		}
		case 8:
		{
			Vector3d yAxis = Vector3d.YAxis;
			transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis * -1.0, value, vector3d, yAxis);
			break;
		}
		case 9:
		{
			Vector3d destination2 = Vector3d.YAxis * -1.0;
			transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis * -1.0, value, vector3d, destination2);
			break;
		}
		case 10:
		{
			Vector3d zAxis = Vector3d.ZAxis;
			transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis * -1.0, value, vector3d, zAxis);
			break;
		}
		case 11:
		{
			Vector3d destination = Vector3d.ZAxis * -1.0;
			transform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis * -1.0, value, vector3d, destination);
			break;
		}
		}
		OutXform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
