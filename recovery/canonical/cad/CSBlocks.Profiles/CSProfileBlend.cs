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
		OnNurbsCurve onNurbsCurve = ((OnCurve)Interop.ToOnCurve(value)).NurbsCurve();
		OnNurbsCurve onNurbsCurve2 = ((OnCurve)Interop.ToOnCurve(value2)).NurbsCurve();
		onNurbsCurve = onNurbsCurve.Duplicate();
		onNurbsCurve2 = onNurbsCurve2.Duplicate();
		double ct = 0.0;
		double ct2 = 0.0;
		CSUtilSweep.GetClosestPoints(onNurbsCurve, onNurbsCurve2, 40, ref ct, ref ct2);
		On3dPoint on3dPoint = onNurbsCurve.PointAt(ct);
		On3dPoint on3dPoint2 = onNurbsCurve2.PointAt(ct2);
		CSUtilSweep.MatchCurveDirection2(onNurbsCurve, onNurbsCurve2, 40);
		if (value5 * value4 < 0.0)
		{
			onNurbsCurve2.Reverse();
		}
		OnPlane onPlane = new OnPlane();
		OnPlane onPlane2 = new OnPlane();
		onNurbsCurve.IsPlanar(onPlane);
		onNurbsCurve2.IsPlanar(onPlane2);
		if (!onPlane.IsValid() || !onPlane2.IsValid())
		{
			return UpdateError("Nonplanar curves");
		}
		On3dPoint on3dPoint3;
		On3dPoint on3dPoint4;
		if (onNurbsCurve.IsClosed() || onNurbsCurve2.IsClosed())
		{
			on3dPoint3 = on3dPoint;
			on3dPoint4 = on3dPoint2;
		}
		else
		{
			on3dPoint3 = onNurbsCurve.PointAtStart();
			on3dPoint4 = onNurbsCurve2.PointAtStart();
		}
		On3dVector on3dVector = on3dPoint3 - on3dPoint4;
		on3dVector.Unitize();
		On3dVector on3dVector2 = new On3dVector(onPlane.Normal());
		On3dVector on3dVector3 = new On3dVector(onPlane2.Normal());
		if (on3dVector2 * on3dVector > 0.0)
		{
			on3dVector2.Reverse();
		}
		if (on3dVector3 * on3dVector > 0.0)
		{
			on3dVector3.Reverse();
		}
		if (Math.Abs(on3dVector2 * on3dVector) < 0.01 && Math.Abs(on3dVector3 * on3dVector) < 0.01)
		{
			on3dVector3 = new On3dVector(on3dVector2);
		}
		On3dVector on3dVector4;
		switch (value7)
		{
		case -1:
			if (value6)
			{
				On3dVector on3dVector5 = new On3dVector(0.0, 0.0, -1.0);
				if (on3dVector2 * on3dVector5 > 0.0)
				{
					on3dVector2.Reverse();
				}
				if (on3dVector3 * on3dVector5 < 0.0)
				{
					on3dVector3.Reverse();
				}
			}
			break;
		case 0:
			on3dVector4 = new On3dVector(-1.0, 0.0, 0.0);
			goto IL_03b2;
		case 1:
			on3dVector4 = new On3dVector(1.0, 0.0, 0.0);
			goto IL_03b2;
		case 2:
			on3dVector4 = new On3dVector(0.0, -1.0, 0.0);
			goto IL_03b2;
		case 3:
			on3dVector4 = new On3dVector(0.0, 1.0, 0.0);
			goto IL_03b2;
		case 4:
			on3dVector4 = new On3dVector(0.0, 0.0, -1.0);
			goto IL_03b2;
		case 5:
			on3dVector4 = new On3dVector(0.0, 0.0, 1.0);
			goto IL_03b2;
		default:
			{
				on3dVector4 = new On3dVector();
				goto IL_03b2;
			}
			IL_03b2:
			if (on3dVector2 * on3dVector4 > 0.0)
			{
				on3dVector2.Reverse();
			}
			if (on3dVector3 * on3dVector4 < 0.0)
			{
				on3dVector3.Reverse();
			}
			break;
		}
		if (value5 < 0.0)
		{
			on3dVector3.Reverse();
			value5 = 0.0 - value5;
		}
		if (value4 < 0.0)
		{
			on3dVector2.Reverse();
			value4 = 0.0 - value4;
		}
		double num = on3dPoint3.DistanceTo(on3dPoint4);
		OnBrep[] breps = null;
		if (onNurbsCurve.IsClosed() || onNurbsCurve2.IsClosed())
		{
			num = on3dPoint.DistanceTo(on3dPoint2);
			On3dPointArray on3dPointArray = new On3dPointArray();
			on3dPointArray.Empty();
			on3dPointArray.Append(on3dPoint);
			if (value3 * value4 > CSApplicationHelper.Application.Tolerance)
			{
				on3dPointArray.Append(on3dPoint + on3dVector2 * num * 0.2 * value3 * value4);
				on3dPointArray.Append(on3dPoint + on3dVector2 * num * 0.4 * value3 * value4);
			}
			if (value3 * value5 > CSApplicationHelper.Application.Tolerance)
			{
				on3dPointArray.Append(on3dPoint2 - on3dVector3 * num * 0.4 * value3 * value5);
				on3dPointArray.Append(on3dPoint2 - on3dVector3 * num * 0.2 * value3 * value5);
			}
			on3dPointArray.Append(on3dPoint2);
			OnBezierCurve onBezierCurve = new OnBezierCurve(on3dPointArray);
			OnNurbsCurve nurbs_curve = new OnNurbsCurve();
			onBezierCurve.GetNurbForm(ref nurbs_curve);
			nurbs_curve.RemoveShortSegments(0.01);
			nurbs_curve.RepairBadKnots();
			CSUtilSweep.Sweep1(nurbs_curve, onNurbsCurve, onNurbsCurve2, out breps);
			on3dPointArray.Dispose();
			onBezierCurve.Dispose();
			nurbs_curve.Dispose();
		}
		else
		{
			On3dPoint on3dPoint5 = onNurbsCurve.PointAtEnd();
			On3dPoint on3dPoint6 = onNurbsCurve2.PointAtEnd();
			double num2 = on3dPoint5.DistanceTo(on3dPoint6);
			On3dPointArray on3dPointArray2 = new On3dPointArray();
			on3dPointArray2.Empty();
			on3dPointArray2.Append(on3dPoint3);
			on3dPointArray2.Append(on3dPoint3 + on3dVector2 * num * 0.2);
			on3dPointArray2.Append(on3dPoint3 + on3dVector2 * num * 0.4);
			on3dPointArray2.Append(on3dPoint4 - on3dVector3 * num * 0.4);
			on3dPointArray2.Append(on3dPoint4 - on3dVector3 * num * 0.2);
			on3dPointArray2.Append(on3dPoint4);
			OnBezierCurve onBezierCurve2 = new OnBezierCurve(on3dPointArray2);
			OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
			onBezierCurve2.GetNurbForm(ref nurbs_curve2);
			on3dPointArray2.Empty();
			on3dPointArray2.Append(on3dPoint5);
			on3dPointArray2.Append(on3dPoint5 + on3dVector2 * num2 * 0.2);
			on3dPointArray2.Append(on3dPoint5 + on3dVector2 * num2 * 0.4);
			on3dPointArray2.Append(on3dPoint6 - on3dVector3 * num2 * 0.4);
			on3dPointArray2.Append(on3dPoint6 - on3dVector3 * num2 * 0.2);
			on3dPointArray2.Append(on3dPoint6);
			OnBezierCurve onBezierCurve3 = new OnBezierCurve(on3dPointArray2);
			OnNurbsCurve nurbs_curve3 = new OnNurbsCurve();
			onBezierCurve3.GetNurbForm(ref nurbs_curve3);
			CSUtilSweep.Sweep2(nurbs_curve2, nurbs_curve3, onNurbsCurve, onNurbsCurve2, out breps);
			on3dPointArray2.Dispose();
			onBezierCurve2.Dispose();
			onBezierCurve3.Dispose();
			nurbs_curve2.Dispose();
			nurbs_curve3.Dispose();
		}
		OnBrep brep = CSUtilRhino.JoinAllBreps(breps, CSApplicationHelper.Application.Tolerance);
		OnBrep[] array = breps;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Dispose();
		}
		oSurface.SetData(new CSOnGeometryData(brep.Convert()));
		return base.Update();
	}
}
