using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Profiles.Obsolete;

[Obsolete]
[CSBlock("ProfilePlacer1RailXform_V2", "{177576E6-E97D-4b3a-84D3-AF5B4E4210A0}")]
public sealed class CSProfilePlacer1RailXform2 : CSBlock
{
	public CSInputChannel InRail { get; }

	public CSInputChannel InParameter { get; }

	public CSInputChannel InOffset { get; }

	public CSInputChannel InForceOpen { get; }

	public CSInputChannel InMethod { get; }

	public CSInputChannel InPivot { get; }

	public CSInputChannel InLastRightRailNormParam { get; }

	public CSInputChannel InFromObject { get; }

	public CSOutputChannel OutXform { get; }

	public CSOutputChannel OutParam { get; }

	public CSOutputChannel OutRightRailNormParam { get; }

	public CSProfilePlacer1RailXform2(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InParameter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InForceOpen = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InMethod = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPivot = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InLastRightRailNormParam = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InFromObject = new CSInputChannel(this, ChannelDataType.Point | ChannelDataType.Curve, ChannelArrayType.Individual);
		OutXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRightRailNormParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData arrayItem = InRail.GetFlattenArray().GetArrayItem(0);
		if (arrayItem == null)
		{
			return UpdateMissing("Missing rail data");
		}
		OnNurbsCurve rail = arrayItem.GetNurbsCurve().Convert();
		if (rail == null)
		{
			return UpdateMissing("Missing rail curve");
		}
		OnNurbsCurve onNurbsCurve = rail.Duplicate();
		double value = 0.0;
		InLastRightRailNormParam.GetDouble(ref value);
		double value2 = 0.0;
		InParameter.GetDouble(ref value2);
		double value3 = 0.0;
		InOffset.GetDouble(ref value3);
		value2 += value3 + value;
		bool value4 = false;
		InForceOpen.GetBoolean(ref value4);
		OnNurbsCurve nurbsCurve = InFromObject.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			if (rail.IsClosed() && !value4)
			{
				if (value2 < 0.0)
				{
					value2 += 1.0;
				}
				if (value2 > 1.0)
				{
					value2 -= 1.0;
				}
				if (value2 == 1.0)
				{
					value2 = 0.0;
				}
			}
			else
			{
				if (value2 < 0.0)
				{
					value2 = 0.0;
				}
				if (value2 > 1.0)
				{
					value2 = 1.0;
				}
			}
		}
		bool flip = false;
		CSUtilGeometry.AdjustRailFromCurve(nurbsCurve, ref rail, ref value2, ref flip);
		double t = value2;
		if (value2 >= 1.0)
		{
			t = rail.Domain().Max();
		}
		else if (value2 <= 0.0)
		{
			t = rail.Domain().Min();
		}
		else if (!rail.GetNormalizedArcLengthPoint(value2, ref t))
		{
			return UpdateError("GetNormalizedArcLengthPoint failed");
		}
		double t2 = t;
		double number = value2;
		if (nurbsCurve != null)
		{
			onNurbsCurve.GetClosestPoint(rail.PointAt(t), ref t2);
			double length = 1.0;
			onNurbsCurve.GetLength(ref length);
			onNurbsCurve.Trim(new OnInterval(onNurbsCurve.Domain().Min(), t2));
			double length2 = 1.0;
			onNurbsCurve.GetLength(ref length2);
			number = length2 / length;
		}
		OutParam.SetData(new CSNumberData(t2));
		OutRightRailNormParam.SetData(new CSNumberData(number));
		On3dPoint on3dPoint = rail.PointAt(t);
		On3dVector on3dVector = rail.TangentAt(t);
		int value5 = 0;
		InMethod.GetInteger(ref value5);
		OnXform onXform;
		switch (value5)
		{
		case 0:
		{
			On3dVector on3dVector2 = on3dPoint * -1.0;
			on3dVector2.Unitize();
			onXform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(0.0, 0.0, 1.0), new On3dVector(0.0, -1.0, 0.0), on3dPoint, on3dVector, on3dVector2);
			break;
		}
		case 1:
		{
			On3dVector on3dVector2 = on3dPoint * -1.0;
			on3dVector2.y = 0.0;
			on3dVector2.Unitize();
			onXform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(0.0, 0.0, 1.0), new On3dVector(0.0, -1.0, 0.0), on3dPoint, on3dVector, on3dVector2);
			break;
		}
		case 2:
		{
			On3dVector on3dVector2 = new On3dVector(0.0, 0.0, -1.0);
			onXform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(0.0, 0.0, 1.0), new On3dVector(0.0, -1.0, 0.0), on3dPoint, on3dVector, on3dVector2);
			break;
		}
		case 3:
		{
			On3dVector on3dVector2 = new On3dVector(0.0, 0.0, -1.0);
			OnPlane onPlane2 = new OnPlane();
			if (rail.IsPlanar(onPlane2))
			{
				on3dVector2 = new On3dVector(onPlane2.Normal());
			}
			onXform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(0.0, 0.0, 1.0), new On3dVector(0.0, -1.0, 0.0), on3dPoint, on3dVector, on3dVector2);
			break;
		}
		case 4:
		{
			On3dVector on3dVector2 = new On3dVector(0.0, 0.0, -1.0);
			OnPlane onPlane3 = new OnPlane();
			if (rail.IsPlanar(onPlane3))
			{
				on3dVector2 = new On3dVector(onPlane3.Normal());
			}
			onXform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(1.0, 0.0, 0.0), new On3dVector(0.0, 0.0, -1.0), on3dPoint, on3dVector, on3dVector2);
			break;
		}
		case 5:
		{
			On3dVector on3dVector2 = new On3dVector(0.0, 0.0, 1.0);
			onXform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(0.0, 0.0, 1.0), new On3dVector(0.0, -1.0, 0.0), on3dPoint, on3dVector, on3dVector2);
			break;
		}
		case 6:
		{
			On3dVector on3dVector2 = new On3dVector(0.0, -1.0, 0.0);
			onXform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(0.0, 0.0, 1.0), new On3dVector(0.0, -1.0, 0.0), on3dPoint, on3dVector, on3dVector2);
			break;
		}
		case 7:
		{
			On3dVector on3dVector2 = new On3dVector(0.0, 0.0, -1.0);
			OnPlane onPlane = new OnPlane();
			if (rail.IsPlanar(onPlane))
			{
				on3dVector2 = new On3dVector(onPlane.Normal());
			}
			if (on3dVector2.z > 0.0)
			{
				on3dVector2 *= -1.0;
			}
			onXform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(0.0, 0.0, 1.0), new On3dVector(0.0, -1.0, 0.0), on3dPoint, on3dVector, on3dVector2);
			break;
		}
		case 8:
		{
			On3dVector on3dVector2 = new On3dVector(on3dPoint)
			{
				y = 0.0
			};
			on3dVector2.Unitize();
			onXform = CSBlockUtil.CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(1.0, 0.0, 0.0), new On3dVector(0.0, 1.0, 0.0), on3dPoint, on3dVector, on3dVector2);
			break;
		}
		default:
			return UpdateError("Unknown Profile Method");
		}
		On3dPoint onPoint = null;
		if (InPivot.GetPoint(ref onPoint))
		{
			On3dVector start_dir = OnUtil.ON_CrossProduct(OnUtil.On_zaxis, on3dVector);
			On3dVector on3dVector3 = on3dPoint - onPoint;
			on3dVector3.z = 0.0;
			OnXform onXform2 = new OnXform();
			onXform2.Rotation(start_dir, on3dVector3, on3dPoint);
			onXform = onXform2 * onXform;
		}
		OutXform.SetData(new CSXformData(onXform.Convert()));
		return base.Update();
	}
}
