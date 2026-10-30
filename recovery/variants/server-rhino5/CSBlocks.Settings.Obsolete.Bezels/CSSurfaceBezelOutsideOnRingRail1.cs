using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Settings.Obsolete.Bezels;

[Obsolete]
[CSBlock("SurfaceBezelOutsideOnRingRail_V1", "{D7741502-5975-4aca-8995-15302B797A5B}")]
public sealed class CSSurfaceBezelOutsideOnRingRail1 : CSBlock
{
	public CSInputChannel iOutsideRail { get; }

	public CSInputChannel iOutsideProfile { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iSeatHeight { get; }

	public CSInputChannel iTube { get; }

	public CSOutputChannel oBrep { get; }

	public CSOutputChannel oCurve { get; }

	public CSSurfaceBezelOutsideOnRingRail1(string identifier)
		: base(identifier)
	{
		iOutsideRail = new CSInputChannel(GetPropertyName(() => iOutsideRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOutsideProfile = new CSInputChannel(GetPropertyName(() => iOutsideProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSeatHeight = new CSInputChannel(GetPropertyName(() => iSeatHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTube = new CSInputChannel(GetPropertyName(() => iTube), this, ChannelDataType.Surface, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iOutsideRail.GetFlattenArray();
		OnNurbsCurve nurbsCurve = iOutsideProfile.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing outside profile curve");
		}
		double value = 0.0;
		iPlacement.GetDouble(ref value);
		double value2 = 0.0;
		iAngle.GetDouble(ref value2);
		double value3 = 0.0;
		iSeatHeight.GetDouble(ref value3);
		OnNurbsSurface nurbsSurface = iTube.GetNurbsSurface();
		CSData outputSurfaceData = null;
		CSDataArray cSDataArray = new CSDataArray();
		CSData outputCurveData = null;
		CSDataArray cSDataArray2 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value4);
			if (!value4.IsValid)
			{
				value4 = Transform.Identity;
			}
			value4.TryGetInverse(out var inverseTransform);
			OnNurbsCurve val = arrayItem.GetNurbsCurve().Convert();
			if (val != null)
			{
				OnNurbsCurve val2 = val.Duplicate();
				val2.Transform((IOnXform)(object)inverseTransform.Convert());
				OnNurbsCurve val3 = nurbsCurve.Duplicate();
				if (val3 == null)
				{
					return UpdateError("Duplicate failed");
				}
				if (!BezelOutsideOnRingRail(value4.Convert(), val2, val3, value, value2, value3, nurbsSurface, ref outputSurfaceData, ref outputCurveData))
				{
					return UpdateError("BezelOutsideOnRingRail failed");
				}
				cSDataArray.Append(outputSurfaceData);
				cSDataArray2.Append(outputCurveData);
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oBrep.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oBrep.SetData(cSDataArray);
		}
		if (cSDataArray2.GetArrayCount() == 1)
		{
			oCurve.SetData(cSDataArray2.GetArrayItem(0));
		}
		else
		{
			oCurve.SetData(cSDataArray2);
		}
		return base.Update();
	}

	private static bool BezelOutsideOnRingRail(OnXform xUp, OnNurbsCurve curveNC, OnNurbsCurve profileNC, double placement, double angle, double seatHeight, OnNurbsSurface tube, ref CSData outputSurfaceData, ref CSData outputCurveData)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		if (tube == null)
		{
			return false;
		}
		On3dPoint val = new On3dPoint();
		On3dPoint val2 = new On3dPoint();
		On3dPoint val3 = new On3dPoint();
		val2 = ((OnCurve)profileNC).PointAtStart();
		On3dVector val4 = new On3dVector((IOn3dPoint)(object)val2);
		val4.Reverse();
		OnXform val5 = new OnXform();
		val5.Translation((IOn3dVector)(object)val4);
		OnXform val6 = new OnXform();
		val6.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_xaxis, OnUtil.On_origin);
		OnXform val7 = new OnXform();
		val7.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), OnUtil.On_zaxis, OnUtil.On_origin);
		profileNC.Transform((IOnXform)(object)(val7 * (IOnXform)(object)val6 * (IOnXform)(object)val5));
		val3 = ((OnCurve)profileNC).PointAtEnd();
		double num = CSBlockUtil.CSUtilMath.AngleOfPointD(val3.y, 0.0 - val3.z);
		OnXform val8 = new OnXform();
		val8.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle) - num, OnUtil.On_xaxis, OnUtil.On_origin);
		profileNC.Transform((IOnXform)(object)val8);
		OnLineCurve val9 = OnLineCurve.op_Implicit((IOnLine)new OnLine(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(0.0, 0.0, -50.0)));
		val9.Transform((IOnXform)(object)xUp);
		OnCurve[] array = null;
		On3dPointArray val10 = null;
		if (!RhUtil.RhinoCurveBrepIntersect((IOnCurve)(object)val9, (IOnBrep)(object)((OnGeometry)tube).BrepForm(), CSApplicationHelper.Application.Tolerance, ref array, ref val10))
		{
			return false;
		}
		if (array.Length != 0)
		{
			OnCurve[] array2 = array;
			foreach (OnCurve val11 in array2)
			{
				((ArrayOn3dPoint)val10).Append((IOn3dPoint)(object)val11.PointAtStart());
				((ArrayOn3dPoint)val10).Append((IOn3dPoint)(object)val11.PointAtEnd());
			}
		}
		double num2 = 10.0;
		for (int j = 0; j < ((ArrayOn3dPoint)val10).Count(); j++)
		{
			double num3 = ((OnCurve)val9).PointAtStart().DistanceTo((IOn3dPoint)(object)((ArrayOn3dPoint)val10)[j]);
			if (j == 0 || num3 < num2)
			{
				num2 = num3;
			}
		}
		num2 += placement + seatHeight;
		double num4 = num2 / (0.0 - ((OnCurve)profileNC).PointAtEnd().z);
		val5.Scale(num4, num4, num4);
		profileNC.Transform((IOnXform)(object)val5);
		val = ((OnCurve)curveNC).PointAtStart();
		val2 = ((OnCurve)profileNC).PointAtStart();
		val5.Translation((IOn3dVector)(object)(val - (IOn3dPoint)(object)val2));
		profileNC.Transform((IOnXform)(object)val5);
		OnBrep val12 = ((OnGeometry)CSUtilRailRevolve.RailRevolve((IOnNurbsCurve)(object)profileNC, (IOnNurbsCurve)(object)curveNC, 0)).BrepForm();
		((OnGeometry)val12).Transform((IOnXform)(object)xUp);
		outputSurfaceData = new CSOnGeometryData(val12.Convert(), xUp.Convert(), applyTransform: false);
		On3dPoint val13 = ((OnCurve)profileNC).PointAtStart();
		val13.z = 0.0;
		On3dPoint obj = ((OnCurve)profileNC).PointAtEnd();
		obj.z = 0.0;
		double num5 = val13.DistanceTo(OnUtil.On_origin);
		num4 = obj.DistanceTo(OnUtil.On_origin) / num5;
		val5.Scale(num4, num4, 1.0);
		curveNC.Transform((IOnXform)(object)val5);
		val5.Translation(0.0, 0.0, 0.0 - num2);
		curveNC.Transform((IOnXform)(object)(xUp * (IOnXform)(object)val5));
		outputCurveData = new CSOnGeometryData(curveNC.Convert(), xUp.Convert() * val5.Convert(), applyTransform: false);
		return true;
	}
}
