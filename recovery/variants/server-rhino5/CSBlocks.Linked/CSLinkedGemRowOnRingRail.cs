using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Linked;

[CSBlock("LinkedGemRowOnRingRail", "{46C4BF34-35F6-45A8-9067-E1F75CE0F006}")]
public sealed class CSLinkedGemRowOnRingRail : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemCount;

	public readonly CSInputChannel InGemSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemNudge;

	public readonly CSInputChannel InLastRightRailPoint;

	public readonly CSInputChannel InLastRightRailVector;

	public readonly CSOutputChannel OutGem;

	public readonly CSOutputChannel OutRightRailPoint;

	public readonly CSOutputChannel OutRightRailVector;

	public CSLinkedGemRowOnRingRail(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Any);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InRail.NurbsCurve;
		if (nurbsCurve == null)
		{
			return UpdateMissing("Rail Input Missing");
		}
		CSGemData gem = InGemDefinition.Gem;
		if (gem == null)
		{
			return UpdateMissing("Gem Definition Input Missing");
		}
		double num = InGemCount.Double;
		double num2 = InGemSpacing.Double;
		double dz = InGemZOffset.Double;
		double dy = InGemYOffset.Double;
		double angleDegrees = InGemRotation.Double;
		double num3 = InGemNudge.Double;
		bool flag = true;
		Point3d point3d = InLastRightRailPoint.Point3d;
		if (point3d == Point3d.Unset)
		{
			flag = false;
		}
		Vector3d value = Vector3d.Unset;
		CSDataHelper.GetVector(InLastRightRailVector.Data, out value);
		if (value == Vector3d.Unset)
		{
			flag = false;
		}
		Transform x = default(Transform);
		if (!CSUtilGeometry.OrientToCurveNalp(nurbsCurve, 0.0, ref x))
		{
			return UpdateError("OrientToCurveNalp Failed");
		}
		Transform transform = Transform.Translation(0.0, 0.0, dz);
		Transform transform2 = Transform.Translation(0.0, dy, 0.0);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), Vector3d.ZAxis, Point3d.Origin);
		CSData cSData = gem.Duplicate();
		CSDataHelper.Transform(cSData, transform3);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Bounding Box Failed");
		}
		double num4 = boundingBox.Max.Y - boundingBox.Min.Y;
		double x2 = boundingBox.Min.X;
		double x3 = boundingBox.Max.X;
		Point3d point3d2 = new Point3d(x2 * (1.0 - num3 / 100.0), 0.0, 0.0);
		point3d2.Transform(x * transform);
		Transform transform4;
		if (flag)
		{
			double length = new Vector3d(point3d2).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(point3d, value, length, ref intersectPoint);
			transform4 = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), point3d2, new Point3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0), intersectPoint, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			transform4 = Transform.Identity;
		}
		Point3d point3d3 = new Point3d(x3 * (1.0 - num3 / 100.0), 0.0, 0.0);
		point3d3.Transform(transform4 * x * transform);
		Vector3d vectorStore = new Vector3d(point3d3);
		vectorStore.Unitize();
		double num5 = num4 + num2;
		double num6 = num5 * (num - 1.0) / 2.0;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; (double)i < num; i++)
		{
			Transform transform5 = Transform.Translation(0.0, num6 - (double)i * num5, 0.0);
			CSData data = gem.Duplicate();
			CSDataHelper.Transform(data, transform5 * transform4 * x * transform * transform2 * transform3);
			cSDataArray.Append(data);
		}
		OutGem.SetData(cSDataArray);
		OutRightRailPoint.SetData(new CSPointData(point3d3));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
