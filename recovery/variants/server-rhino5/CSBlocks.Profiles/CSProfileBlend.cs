using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlocks.Profiles;

[CSBlock("ProfileBlend", "{ABA6665E-7F86-4b30-BCF2-9554CCEF33C7}")]
public sealed class CSProfileBlend : CSBlock
{
	public readonly CSInputChannel iCurve1;

	public readonly CSInputChannel iCurve2;

	public readonly CSInputChannel iBlendAmount;

	public readonly CSInputChannel iBlendAmount1;

	public readonly CSInputChannel iBlendAmount2;

	public readonly CSInputChannel iNorth;

	public readonly CSInputChannel iDirection;

	public readonly CSOutputChannel oSurface;

	public CSProfileBlend(string identifier)
		: base(identifier)
	{
		iCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iBlendAmount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		iBlendAmount1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		iBlendAmount2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		iNorth = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		iDirection = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, -1);
		oSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Expected O, but got Unknown
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Expected O, but got Unknown
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Expected O, but got Unknown
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Expected O, but got Unknown
		//IL_0746: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		if (!iCurve1.GetValue(out Curve value))
		{
			return InputError();
		}
		if (!iCurve2.GetValue(out Curve value2))
		{
			return InputError();
		}
		if (!iBlendAmount.GetValue(out double value3))
		{
			return InputError();
		}
		if (!iBlendAmount1.GetValue(out double value4))
		{
			return InputError();
		}
		if (!iBlendAmount2.GetValue(out double value5))
		{
			return InputError();
		}
		if (!iNorth.GetValue(out bool value6))
		{
			return InputError();
		}
		if (!iDirection.GetValue(out int value7))
		{
			return InputError();
		}
		OnNurbsCurve val = ((OnCurve)Interop.ToOnCurve(value)).NurbsCurve();
		OnNurbsCurve val2 = ((OnCurve)Interop.ToOnCurve(value2)).NurbsCurve();
		val = val.Duplicate();
		val2 = val2.Duplicate();
		double ct = 0.0;
		double ct2 = 0.0;
		CSUtilSweep.GetClosestPoints((OnCurve)(object)val, (OnCurve)(object)val2, 40, ref ct, ref ct2);
		On3dPoint val3 = ((OnCurve)val).PointAt(ct);
		On3dPoint val4 = ((OnCurve)val2).PointAt(ct2);
		CSUtilSweep.MatchCurveDirection2((OnCurve)(object)val, (OnCurve)(object)val2, 40);
		if (value5 * value4 < 0.0)
		{
			((OnCurve)val2).Reverse();
		}
		OnPlane val5 = new OnPlane();
		OnPlane val6 = new OnPlane();
		((OnCurve)val).IsPlanar(val5);
		((OnCurve)val2).IsPlanar(val6);
		if (!val5.IsValid() || !val6.IsValid())
		{
			return UpdateError("Nonplanar curves");
		}
		On3dPoint val7;
		On3dPoint val8;
		if (((OnCurve)val).IsClosed() || ((OnCurve)val2).IsClosed())
		{
			val7 = val3;
			val8 = val4;
		}
		else
		{
			val7 = ((OnCurve)val).PointAtStart();
			val8 = ((OnCurve)val2).PointAtStart();
		}
		On3dVector val9 = val7 - (IOn3dPoint)(object)val8;
		val9.Unitize();
		On3dVector val10 = new On3dVector(val5.Normal());
		On3dVector val11 = new On3dVector(val6.Normal());
		if (val10 * (IOn3dVector)(object)val9 > 0.0)
		{
			val10.Reverse();
		}
		if (val11 * (IOn3dVector)(object)val9 > 0.0)
		{
			val11.Reverse();
		}
		if (Math.Abs(val10 * (IOn3dVector)(object)val9) < 0.01 && Math.Abs(val11 * (IOn3dVector)(object)val9) < 0.01)
		{
			val11 = new On3dVector((IOn3dVector)(object)val10);
		}
		On3dVector val12;
		switch (value7)
		{
		case -1:
			if (value6)
			{
				On3dVector val13 = new On3dVector(0.0, 0.0, -1.0);
				if (val10 * (IOn3dVector)(object)val13 > 0.0)
				{
					val10.Reverse();
				}
				if (val11 * (IOn3dVector)(object)val13 < 0.0)
				{
					val11.Reverse();
				}
			}
			break;
		case 0:
			val12 = new On3dVector(-1.0, 0.0, 0.0);
			goto IL_03b2;
		case 1:
			val12 = new On3dVector(1.0, 0.0, 0.0);
			goto IL_03b2;
		case 2:
			val12 = new On3dVector(0.0, -1.0, 0.0);
			goto IL_03b2;
		case 3:
			val12 = new On3dVector(0.0, 1.0, 0.0);
			goto IL_03b2;
		case 4:
			val12 = new On3dVector(0.0, 0.0, -1.0);
			goto IL_03b2;
		case 5:
			val12 = new On3dVector(0.0, 0.0, 1.0);
			goto IL_03b2;
		default:
			{
				val12 = new On3dVector();
				goto IL_03b2;
			}
			IL_03b2:
			if (val10 * (IOn3dVector)(object)val12 > 0.0)
			{
				val10.Reverse();
			}
			if (val11 * (IOn3dVector)(object)val12 < 0.0)
			{
				val11.Reverse();
			}
			break;
		}
		if (value5 < 0.0)
		{
			val11.Reverse();
			value5 = 0.0 - value5;
		}
		if (value4 < 0.0)
		{
			val10.Reverse();
			value4 = 0.0 - value4;
		}
		double num = val7.DistanceTo((IOn3dPoint)(object)val8);
		OnBrep[] breps = null;
		if (((OnCurve)val).IsClosed() || ((OnCurve)val2).IsClosed())
		{
			num = val3.DistanceTo((IOn3dPoint)(object)val4);
			On3dPointArray val14 = new On3dPointArray();
			((ArrayOn3dPoint)val14).Empty();
			((ArrayOn3dPoint)val14).Append((IOn3dPoint)(object)val3);
			if (value3 * value4 > CSApplicationHelper.Application.Tolerance)
			{
				((ArrayOn3dPoint)val14).Append((IOn3dPoint)(object)(val3 + (IOn3dVector)(object)(val10 * num * 0.2 * value3 * value4)));
				((ArrayOn3dPoint)val14).Append((IOn3dPoint)(object)(val3 + (IOn3dVector)(object)(val10 * num * 0.4 * value3 * value4)));
			}
			if (value3 * value5 > CSApplicationHelper.Application.Tolerance)
			{
				((ArrayOn3dPoint)val14).Append((IOn3dPoint)(object)(val4 - (IOn3dVector)(object)(val11 * num * 0.4 * value3 * value5)));
				((ArrayOn3dPoint)val14).Append((IOn3dPoint)(object)(val4 - (IOn3dVector)(object)(val11 * num * 0.2 * value3 * value5)));
			}
			((ArrayOn3dPoint)val14).Append((IOn3dPoint)(object)val4);
			OnBezierCurve val15 = new OnBezierCurve((IOn3dPointArray)(object)val14);
			OnNurbsCurve val16 = new OnNurbsCurve();
			val15.GetNurbForm(ref val16);
			val16.RemoveShortSegments(0.01);
			val16.RepairBadKnots();
			CSUtilSweep.Sweep1((IOnCurve)(object)val16, (IOnCurve)(object)val, (IOnCurve)(object)val2, out breps);
			((ArrayOn3dPoint)val14).Dispose();
			val15.Dispose();
			((OnObject)val16).Dispose();
		}
		else
		{
			On3dPoint val17 = ((OnCurve)val).PointAtEnd();
			On3dPoint val18 = ((OnCurve)val2).PointAtEnd();
			double num2 = val17.DistanceTo((IOn3dPoint)(object)val18);
			On3dPointArray val19 = new On3dPointArray();
			((ArrayOn3dPoint)val19).Empty();
			((ArrayOn3dPoint)val19).Append((IOn3dPoint)(object)val7);
			((ArrayOn3dPoint)val19).Append((IOn3dPoint)(object)(val7 + (IOn3dVector)(object)(val10 * num * 0.2)));
			((ArrayOn3dPoint)val19).Append((IOn3dPoint)(object)(val7 + (IOn3dVector)(object)(val10 * num * 0.4)));
			((ArrayOn3dPoint)val19).Append((IOn3dPoint)(object)(val8 - (IOn3dVector)(object)(val11 * num * 0.4)));
			((ArrayOn3dPoint)val19).Append((IOn3dPoint)(object)(val8 - (IOn3dVector)(object)(val11 * num * 0.2)));
			((ArrayOn3dPoint)val19).Append((IOn3dPoint)(object)val8);
			OnBezierCurve val20 = new OnBezierCurve((IOn3dPointArray)(object)val19);
			OnNurbsCurve val21 = new OnNurbsCurve();
			val20.GetNurbForm(ref val21);
			((ArrayOn3dPoint)val19).Empty();
			((ArrayOn3dPoint)val19).Append((IOn3dPoint)(object)val17);
			((ArrayOn3dPoint)val19).Append((IOn3dPoint)(object)(val17 + (IOn3dVector)(object)(val10 * num2 * 0.2)));
			((ArrayOn3dPoint)val19).Append((IOn3dPoint)(object)(val17 + (IOn3dVector)(object)(val10 * num2 * 0.4)));
			((ArrayOn3dPoint)val19).Append((IOn3dPoint)(object)(val18 - (IOn3dVector)(object)(val11 * num2 * 0.4)));
			((ArrayOn3dPoint)val19).Append((IOn3dPoint)(object)(val18 - (IOn3dVector)(object)(val11 * num2 * 0.2)));
			((ArrayOn3dPoint)val19).Append((IOn3dPoint)(object)val18);
			OnBezierCurve val22 = new OnBezierCurve((IOn3dPointArray)(object)val19);
			OnNurbsCurve val23 = new OnNurbsCurve();
			val22.GetNurbForm(ref val23);
			CSUtilSweep.Sweep2((IOnCurve)(object)val21, (IOnCurve)(object)val23, (IOnCurve)(object)val, (IOnCurve)(object)val2, out breps);
			((ArrayOn3dPoint)val19).Dispose();
			val20.Dispose();
			val22.Dispose();
			((OnObject)val21).Dispose();
			((OnObject)val23).Dispose();
		}
		OnBrep brep = CSUtilRhino.JoinAllBreps(breps, CSApplicationHelper.Application.Tolerance);
		OnBrep[] array = breps;
		for (int i = 0; i < array.Length; i++)
		{
			((OnObject)array[i]).Dispose();
		}
		oSurface.SetData(new CSOnGeometryData(brep.Convert()));
		return base.Update();
	}
}
