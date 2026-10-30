using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Profiles;

[CSBlock("ProfilePlacer1RailXform", "{0619AF39-94B6-42DC-B50B-D72FB5073631}")]
public sealed class CSProfilePlacer1RailXform : CSBlock
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

	public CSProfilePlacer1RailXform(string identifier)
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
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Expected O, but got Unknown
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Expected O, but got Unknown
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Expected O, but got Unknown
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Expected O, but got Unknown
		//IL_0487: Expected O, but got Unknown
		//IL_0487: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Expected O, but got Unknown
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Expected O, but got Unknown
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Expected O, but got Unknown
		//IL_0681: Expected O, but got Unknown
		//IL_0681: Expected O, but got Unknown
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Expected O, but got Unknown
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Expected O, but got Unknown
		//IL_0715: Expected O, but got Unknown
		//IL_0715: Expected O, but got Unknown
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Expected O, but got Unknown
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Expected O, but got Unknown
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0801: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Expected O, but got Unknown
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_085a: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Expected O, but got Unknown
		//IL_088a: Expected O, but got Unknown
		//IL_088a: Expected O, but got Unknown
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Expected O, but got Unknown
		//IL_053a: Expected O, but got Unknown
		//IL_053a: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Expected O, but got Unknown
		//IL_05ed: Expected O, but got Unknown
		//IL_05ed: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Expected O, but got Unknown
		//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08da: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ed: Expected O, but got Unknown
		//IL_079c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ec: Expected O, but got Unknown
		//IL_07ec: Expected O, but got Unknown
		//IL_07ec: Expected O, but got Unknown
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
		OnNurbsCurve val = rail.Duplicate();
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
			if (((OnCurve)rail).IsClosed() && !value4)
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
		double num = value2;
		if (value2 >= 1.0)
		{
			num = ((OnCurve)rail).Domain().Max();
		}
		else if (value2 <= 0.0)
		{
			num = ((OnCurve)rail).Domain().Min();
		}
		else if (!((OnCurve)rail).GetNormalizedArcLengthPoint(value2, ref num))
		{
			return UpdateError("GetNormalizedArcLengthPoint failed");
		}
		double num2 = num;
		double number = value2;
		if (nurbsCurve != null)
		{
			((OnCurve)val).GetClosestPoint((IOn3dPoint)(object)((OnCurve)rail).PointAt(num), ref num2);
			double num3 = 1.0;
			((OnCurve)val).GetLength(ref num3);
			((OnCurve)val).Trim((IOnInterval)new OnInterval(((OnCurve)val).Domain().Min(), num2));
			double num4 = 1.0;
			((OnCurve)val).GetLength(ref num4);
			number = num4 / num3;
		}
		OutParam.SetData(new CSNumberData(num2));
		OutRightRailNormParam.SetData(new CSNumberData(number));
		Point3d origin = Point3d.Origin;
		if (arrayItem is ICSTransform iCSTransform && iCSTransform.GetTransform(out var value5))
		{
			((Point3d)(ref origin)).Transform(value5);
		}
		On3dPoint val2 = ((OnCurve)rail).PointAt(num);
		On3dVector val3 = ((OnCurve)rail).TangentAt(num);
		int value6 = 0;
		InMethod.GetInteger(ref value6);
		OnXform val5;
		switch (value6)
		{
		case 0:
		{
			On3dVector val4 = new On3dVector((IOn3dVector)(object)((val2 - (IOn3dPoint)(object)origin.Convert()) * -1.0));
			val4.Unitize();
			val5 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dVector)new On3dVector(0.0, -1.0, 0.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
			break;
		}
		case 1:
		{
			On3dVector val4 = new On3dVector((IOn3dVector)(object)((val2 - (IOn3dPoint)(object)origin.Convert()) * -1.0))
			{
				y = 0.0
			};
			val4.Unitize();
			val5 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dVector)new On3dVector(0.0, -1.0, 0.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
			break;
		}
		case 2:
		{
			On3dVector val4 = new On3dVector(0.0, 0.0, -1.0);
			val5 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dVector)new On3dVector(0.0, -1.0, 0.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
			break;
		}
		case 3:
		{
			On3dVector val4 = new On3dVector(0.0, 0.0, -1.0);
			OnPlane val7 = new OnPlane();
			if (((OnCurve)rail).IsPlanar(val7))
			{
				val4 = new On3dVector(val7.Normal());
			}
			val5 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dVector)new On3dVector(0.0, -1.0, 0.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
			break;
		}
		case 4:
		{
			On3dVector val4 = new On3dVector(0.0, 0.0, -1.0);
			OnPlane val8 = new OnPlane();
			if (((OnCurve)rail).IsPlanar(val8))
			{
				val4 = new On3dVector(val8.Normal());
			}
			val5 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(1.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, -1.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
			break;
		}
		case 5:
		{
			On3dVector val4 = new On3dVector(0.0, 0.0, 1.0);
			val5 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dVector)new On3dVector(0.0, -1.0, 0.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
			break;
		}
		case 6:
		{
			On3dVector val4 = new On3dVector(0.0, -1.0, 0.0);
			val5 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dVector)new On3dVector(0.0, -1.0, 0.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
			break;
		}
		case 7:
		{
			On3dVector val4 = new On3dVector(0.0, 0.0, -1.0);
			OnPlane val6 = new OnPlane();
			if (((OnCurve)rail).IsPlanar(val6))
			{
				val4 = new On3dVector(val6.Normal());
			}
			if (val4.z > 0.0)
			{
				val4 *= -1.0;
			}
			val5 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dVector)new On3dVector(0.0, -1.0, 0.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
			break;
		}
		case 8:
		{
			On3dVector val4 = new On3dVector((IOn3dVector)(object)(val2 - (IOn3dPoint)(object)origin.Convert()))
			{
				y = 0.0
			};
			val4.Unitize();
			val5 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(1.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 1.0, 0.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
			break;
		}
		default:
			return UpdateError("Unknown Profile Method");
		}
		On3dPoint onPoint = null;
		if (InPivot.GetPoint(ref onPoint))
		{
			On3dVector val9 = OnUtil.ON_CrossProduct(OnUtil.On_zaxis, (IOn3dVector)(object)val3);
			On3dVector val10 = val2 - (IOn3dPoint)(object)onPoint;
			val10.z = 0.0;
			OnXform val11 = new OnXform();
			val11.Rotation((IOn3dVector)(object)val9, (IOn3dVector)(object)val10, (IOn3dPoint)(object)val2);
			val5 = val11 * (IOnXform)(object)val5;
		}
		OutXform.SetData(new CSXformData(val5.Convert()));
		return base.Update();
	}
}
