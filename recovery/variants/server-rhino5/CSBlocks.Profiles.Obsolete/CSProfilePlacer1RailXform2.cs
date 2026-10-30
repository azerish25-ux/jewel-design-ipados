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
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Expected O, but got Unknown
		//IL_0314: Expected O, but got Unknown
		//IL_0314: Expected O, but got Unknown
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Expected O, but got Unknown
		//IL_03b5: Expected O, but got Unknown
		//IL_03b5: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Expected O, but got Unknown
		//IL_0449: Expected O, but got Unknown
		//IL_0449: Expected O, but got Unknown
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Expected O, but got Unknown
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Expected O, but got Unknown
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Expected O, but got Unknown
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Expected O, but got Unknown
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Expected O, but got Unknown
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Expected O, but got Unknown
		//IL_0643: Expected O, but got Unknown
		//IL_0643: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Expected O, but got Unknown
		//IL_06d7: Expected O, but got Unknown
		//IL_06d7: Expected O, but got Unknown
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Expected O, but got Unknown
		//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Expected O, but got Unknown
		//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Expected O, but got Unknown
		//IL_0840: Expected O, but got Unknown
		//IL_0840: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Expected O, but got Unknown
		//IL_04fc: Expected O, but got Unknown
		//IL_04fc: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_05af: Expected O, but got Unknown
		//IL_05af: Expected O, but got Unknown
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Expected O, but got Unknown
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0890: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a3: Expected O, but got Unknown
		//IL_075e: Unknown result type (might be due to invalid IL or missing references)
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Expected O, but got Unknown
		//IL_07ae: Expected O, but got Unknown
		//IL_07ae: Expected O, but got Unknown
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
		On3dPoint val2 = ((OnCurve)rail).PointAt(num);
		On3dVector val3 = ((OnCurve)rail).TangentAt(num);
		int value5 = 0;
		InMethod.GetInteger(ref value5);
		OnXform val5;
		switch (value5)
		{
		case 0:
		{
			On3dVector val4 = On3dVector.op_Implicit((IOn3dPoint)(object)(val2 * -1.0));
			val4.Unitize();
			val5 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dVector)new On3dVector(0.0, -1.0, 0.0), (IOn3dPoint)(object)val2, (IOn3dVector)(object)val3, (IOn3dVector)(object)val4);
			break;
		}
		case 1:
		{
			On3dVector val4 = On3dVector.op_Implicit((IOn3dPoint)(object)(val2 * -1.0));
			val4.y = 0.0;
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
			On3dVector val4 = new On3dVector((IOn3dPoint)(object)val2)
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
