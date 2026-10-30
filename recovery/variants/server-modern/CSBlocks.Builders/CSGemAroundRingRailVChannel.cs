using System.Collections.Generic;
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

namespace CSBlocks.Builders;

[CSBlock("GemAroundRingRailVChannel", "{11070D1C-CCEF-4EFC-A441-CA7B39510EB4}")]
public sealed class CSGemAroundRingRailVChannel : CSBlock
{
	public readonly CSInputChannel InInnerRail;

	public readonly CSInputChannel InOuterRail;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InParamLeft;

	public readonly CSInputChannel InParamRight;

	public readonly CSInputChannel InParamStart;

	public readonly CSInputChannel InParamEnd;

	public readonly CSInputChannel InGemParamStart;

	public readonly CSInputChannel InGemParamEnd;

	public readonly CSInputChannel InGemSizeVector;

	public readonly CSInputChannel InBlend;

	public readonly CSInputChannel InWallDrop;

	public readonly CSInputChannel InPullDownDistance;

	public readonly CSInputChannel InShowChannel;

	public readonly CSInputChannel InOrientMethod;

	public readonly CSOutputChannel OutSurface;

	public readonly CSOutputChannel OutStartProfile;

	public readonly CSOutputChannel OutEndProfile;

	public CSGemAroundRingRailVChannel(string identifier)
		: base(identifier)
	{
		InInnerRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InOuterRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InParamLeft = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InParamRight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InParamStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InParamEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemParamStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemParamEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemSizeVector = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InWallDrop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPullDownDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShowChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InOrientMethod = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutStartProfile = new CSOutputChannel(this, ChannelDataType.Profile, ChannelArrayType.Individual);
		OutEndProfile = new CSOutputChannel(this, ChannelDataType.Profile, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_0983: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Expected O, but got Unknown
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Expected O, but got Unknown
		CSDataArray flattenArray = InInnerRail.GetFlattenArray();
		if (flattenArray == null || CSDataHelper.GetArrayCount(flattenArray) < 1)
		{
			return UpdateMissing("Curve");
		}
		OnCurve val = (OnCurve)(object)flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		if (val == null)
		{
			return UpdateMissing("Curve");
		}
		CSDataArray flattenArray2 = InOuterRail.GetFlattenArray();
		if (flattenArray2 == null || CSDataHelper.GetArrayCount(flattenArray2) < 1)
		{
			return UpdateMissing("Curve");
		}
		OnCurve val2 = (OnCurve)(object)flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		if (val2 == null)
		{
			return UpdateMissing("Curve");
		}
		CSData data = InProfile.Data;
		if (data == null)
		{
			return UpdateMissing("Profile");
		}
		OnCurve val3 = (OnCurve)(object)data.GetNurbsCurve().Convert();
		if (val3 == null)
		{
			return UpdateMissing("Missing Profile Data");
		}
		double value = 0.0;
		InParamLeft.GetDouble(ref value);
		double value2 = 0.0;
		InParamRight.GetDouble(ref value2);
		double value3 = 0.0;
		InParamStart.GetDouble(ref value3);
		double value4 = 0.0;
		InParamEnd.GetDouble(ref value4);
		double value5 = 0.0;
		InGemParamStart.GetDouble(ref value5);
		double value6 = 0.0;
		InGemParamEnd.GetDouble(ref value6);
		double value7 = 0.0;
		InBlend.GetDouble(ref value7);
		double value8 = 0.0;
		InWallDrop.GetDouble(ref value8);
		double value9 = 0.0;
		InPullDownDistance.GetDouble(ref value9);
		bool value10 = true;
		InShowChannel.GetBoolean(ref value10);
		int value11 = 0;
		InOrientMethod.GetInteger(ref value11);
		string text = GetParams(val2, val, value3, value11, out var innerParam, out var outerPoint, out var innerPoint, out var xOrient);
		if (!string.IsNullOrWhiteSpace(text))
		{
			return UpdateError(text);
		}
		text = GetParams(val2, val, value4, value11, out var innerParam2, out var outerPoint2, out var innerPoint2, out var xOrient2);
		if (!string.IsNullOrWhiteSpace(text))
		{
			return UpdateError(text);
		}
		On3dPoint outerPoint3 = null;
		On3dPoint innerPoint3 = null;
		double innerParam3 = 0.0;
		OnXform xOrient3 = null;
		On3dPoint outerPoint4 = null;
		On3dPoint innerPoint4 = null;
		double innerParam4 = 0.0;
		OnXform xOrient4 = null;
		if (value10)
		{
			text = GetParams(val2, val, value5, value11, out innerParam3, out outerPoint3, out innerPoint3, out xOrient3);
			if (!string.IsNullOrWhiteSpace(text))
			{
				return UpdateError(text);
			}
			text = GetParams(val2, val, value6, value11, out innerParam4, out outerPoint4, out innerPoint4, out xOrient4);
			if (!string.IsNullOrWhiteSpace(text))
			{
				return UpdateError(text);
			}
		}
		ScaleProfile(val3, outerPoint, innerPoint, out var scaledProfile);
		ScaleProfile(val3, outerPoint2, innerPoint2, out var scaledProfile2);
		OnCurve scaledProfile3 = null;
		OnCurve scaledProfile4 = null;
		if (value10)
		{
			ScaleProfile(val3, outerPoint3, innerPoint3, out scaledProfile3);
			ScaleProfile(val3, outerPoint4, innerPoint4, out scaledProfile4);
		}
		OnBrep brep;
		if (value10)
		{
			On3dVector onVector = new On3dVector(OnUtil.On_UNSET_VECTOR);
			if (!InGemSizeVector.GetVector(ref onVector))
			{
				return UpdateError("Missing Gem Size Vector");
			}
			MakeEndProfileParts(scaledProfile, value, value2, value7, xOrient, out var crvBase, out var crvLeft, out var crvCenter, out var crvRight);
			MakeEndProfileParts(scaledProfile2, value, value2, value7, xOrient2, out var crvBase2, out var crvLeft2, out var crvCenter2, out var crvRight2);
			MakeGemProfileParts(scaledProfile3, value, value2, value7, value8, onVector.z, xOrient3, out var crvBase3, out var crvLeft3, out var crvCenter3, out var crvRight3);
			MakeGemProfileParts(scaledProfile4, value, value2, value7, value8, onVector.z, xOrient4, out var crvBase4, out var crvLeft4, out var crvCenter4, out var crvRight4);
			List<IOnCurve> list = new List<IOnCurve>
			{
				(IOnCurve)(object)crvBase,
				(IOnCurve)(object)crvBase3,
				(IOnCurve)(object)crvBase4,
				(IOnCurve)(object)crvBase2
			};
			List<double> list2 = new List<double> { innerParam, innerParam3, innerParam4, innerParam2 };
			List<double> list3 = new List<double> { value3, value5, value6, value4 };
			if (!CSUtilSweep.Sweep2((IOnCurve)(object)val, (IOnCurve)(object)val2, list.ToArray(), list2.ToArray(), list3.ToArray(), out var breps, closed: false, maintainHeight: true))
			{
				return UpdateError("Sweep2");
			}
			if (breps == null || breps.Length != 1)
			{
				return UpdateError("NoReturnFromSweep2");
			}
			OnBrep val4 = breps[0];
			list = new List<IOnCurve>
			{
				(IOnCurve)(object)crvLeft,
				(IOnCurve)(object)crvLeft3,
				(IOnCurve)(object)crvLeft4,
				(IOnCurve)(object)crvLeft2
			};
			if (!CSUtilSweep.Sweep2((IOnCurve)(object)val, (IOnCurve)(object)val2, list.ToArray(), list2.ToArray(), list3.ToArray(), out breps, closed: false, maintainHeight: true))
			{
				return UpdateError("Sweep2");
			}
			if (breps == null || breps.Length != 1)
			{
				return UpdateError("NoReturnFromSweep2");
			}
			OnBrep val5 = breps[0];
			OnCurve curveFromBrepEdges = CSUtilBrepCrvs.GetCurveFromBrepEdges((IOnBrep)(object)val5, crvLeft.PointAtEnd(), crvLeft2.PointAtEnd(), crvLeft3.PointAtEnd());
			list = new List<IOnCurve>
			{
				(IOnCurve)(object)crvRight,
				(IOnCurve)(object)crvRight3,
				(IOnCurve)(object)crvRight4,
				(IOnCurve)(object)crvRight2
			};
			if (!CSUtilSweep.Sweep2((IOnCurve)(object)val, (IOnCurve)(object)val2, list.ToArray(), list2.ToArray(), list3.ToArray(), out breps, closed: false, maintainHeight: true))
			{
				return UpdateError("Sweep2");
			}
			if (breps == null || breps.Length != 1)
			{
				return UpdateError("NoReturnFromSweep2");
			}
			OnBrep val6 = breps[0];
			OnCurve curveFromBrepEdges2 = CSUtilBrepCrvs.GetCurveFromBrepEdges((IOnBrep)(object)val6, crvRight.PointAtStart(), crvRight2.PointAtStart(), crvRight3.PointAtStart());
			double num = 0.0;
			curveFromBrepEdges.GetLength(ref num);
			double num2 = 0.0;
			curveFromBrepEdges2.GetLength(ref num2);
			double num3 = 0.0;
			curveFromBrepEdges.GetNormalizedArcLengthPoint(value9 / num, ref num3);
			double num4 = 0.0;
			curveFromBrepEdges2.GetNormalizedArcLengthPoint(value9 / num2, ref num4);
			On3dPoint obj = curveFromBrepEdges.PointAt(num3);
			On3dPoint val7 = curveFromBrepEdges2.PointAt(num4);
			OnCurve item = (OnCurve)new OnLineCurve((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val7);
			double num5 = 0.0;
			curveFromBrepEdges.GetNormalizedArcLengthPoint((num - value9) / num, ref num5);
			double num6 = 0.0;
			curveFromBrepEdges2.GetNormalizedArcLengthPoint((num2 - value9) / num2, ref num6);
			On3dPoint obj2 = curveFromBrepEdges.PointAt(num5);
			On3dPoint val8 = curveFromBrepEdges2.PointAt(num6);
			OnCurve item2 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)obj2, (IOn3dPoint)(object)val8);
			list2.Clear();
			double item3 = 0.0;
			curveFromBrepEdges.GetClosestPoint((IOn3dPoint)(object)crvCenter.PointAtStart(), ref item3, CSApplicationHelper.Application.Tolerance);
			list2.Add(item3);
			if (value9 > 0.01)
			{
				list2.Add(num3);
			}
			curveFromBrepEdges.GetClosestPoint((IOn3dPoint)(object)crvCenter3.PointAtStart(), ref item3, CSApplicationHelper.Application.Tolerance);
			list2.Add(item3);
			curveFromBrepEdges.GetClosestPoint((IOn3dPoint)(object)crvCenter4.PointAtStart(), ref item3, CSApplicationHelper.Application.Tolerance);
			list2.Add(item3);
			if (value9 > 0.01)
			{
				list2.Add(num5);
			}
			curveFromBrepEdges.GetClosestPoint((IOn3dPoint)(object)crvCenter2.PointAtStart(), ref item3, CSApplicationHelper.Application.Tolerance);
			list2.Add(item3);
			list3.Clear();
			curveFromBrepEdges2.GetClosestPoint((IOn3dPoint)(object)crvCenter.PointAtEnd(), ref item3, CSApplicationHelper.Application.Tolerance);
			list3.Add(item3);
			if (value9 > 0.01)
			{
				list3.Add(num4);
			}
			curveFromBrepEdges2.GetClosestPoint((IOn3dPoint)(object)crvCenter3.PointAtEnd(), ref item3, CSApplicationHelper.Application.Tolerance);
			list3.Add(item3);
			curveFromBrepEdges2.GetClosestPoint((IOn3dPoint)(object)crvCenter4.PointAtEnd(), ref item3, CSApplicationHelper.Application.Tolerance);
			list3.Add(item3);
			if (value9 > 0.01)
			{
				list3.Add(num6);
			}
			curveFromBrepEdges2.GetClosestPoint((IOn3dPoint)(object)crvCenter2.PointAtEnd(), ref item3, CSApplicationHelper.Application.Tolerance);
			list3.Add(item3);
			list = new List<IOnCurve>();
			list.Add((IOnCurve)(object)crvCenter);
			if (value9 > 0.01)
			{
				list.Add((IOnCurve)(object)item);
			}
			list.Add((IOnCurve)(object)crvCenter3);
			list.Add((IOnCurve)(object)crvCenter4);
			if (value9 > 0.01)
			{
				list.Add((IOnCurve)(object)item2);
			}
			list.Add((IOnCurve)(object)crvCenter2);
			if (!CSUtilSweep.Sweep2((IOnCurve)(object)curveFromBrepEdges, (IOnCurve)(object)curveFromBrepEdges2, list.ToArray(), list2.ToArray(), list3.ToArray(), out breps, closed: false, maintainHeight: true))
			{
				return UpdateError("Sweep2");
			}
			if (breps == null || breps.Length != 1)
			{
				return UpdateError("NoReturnFromSweep2");
			}
			OnBrep val9 = breps[0];
			brep = CSUtilRhino.JoinAllBreps((OnBrep[])(object)new OnBrep[4] { val4, val5, val9, val6 }, CSApplicationHelper.Application.Tolerance);
			((OnGeometry)scaledProfile).Transform((IOnXform)(object)xOrient);
			((OnGeometry)scaledProfile2).Transform((IOnXform)(object)xOrient2);
		}
		else
		{
			((OnGeometry)scaledProfile).Transform((IOnXform)(object)xOrient);
			((OnGeometry)scaledProfile2).Transform((IOnXform)(object)xOrient2);
			List<IOnCurve> list4 = new List<IOnCurve>
			{
				(IOnCurve)(object)scaledProfile,
				(IOnCurve)(object)scaledProfile2
			};
			List<double> list5 = new List<double> { innerParam, innerParam2 };
			List<double> list6 = new List<double> { value3, value4 };
			if (!CSUtilSweep.Sweep2((IOnCurve)(object)val, (IOnCurve)(object)val2, list4.ToArray(), list5.ToArray(), list6.ToArray(), out var breps2, closed: false, maintainHeight: true))
			{
				return UpdateError("Sweep2");
			}
			if (breps2 == null || breps2.Length != 1)
			{
				return UpdateError("NoReturnFromSweep2");
			}
			brep = breps2[0];
		}
		OutSurface.SetData(new CSOnGeometryData((GeometryBase)(object)brep.Convert()));
		OutStartProfile.SetData(new CSOnGeometryData((GeometryBase)(object)scaledProfile.Convert(), xOrient.Convert(), applyTransform: false));
		OutEndProfile.SetData(new CSOnGeometryData((GeometryBase)(object)scaledProfile2.Convert(), xOrient2.Convert(), applyTransform: false));
		return base.Update();
	}

	private string GetParams(OnCurve outerRail, OnCurve innerRail, double outerParam, int orientMethod, out double innerParam, out On3dPoint outerPoint, out On3dPoint innerPoint, out OnXform xOrient)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		innerPoint = null;
		xOrient = null;
		innerParam = 0.0;
		outerPoint = outerRail.PointAt(outerParam);
		On3dVector dir = outerRail.TangentAt(outerParam);
		OnSurface val = MakeSrf(outerPoint, dir, orientMethod);
		ArrayOnX_EVENT val2 = new ArrayOnX_EVENT();
		innerRail.IntersectSurface((IOnSurface)(object)val, ref val2, CSApplicationHelper.Application.Tolerance);
		if (val2.Count() == 0)
		{
			return "Fail intersect";
		}
		double num = 99999.0;
		for (int i = 0; i < val2.Count(); i++)
		{
			double num2 = outerPoint.DistanceTo((IOn3dPoint)(object)val2[i][0]);
			if (num2 < num)
			{
				num = num2;
				innerParam = val2[i][0];
			}
		}
		innerPoint = innerRail.PointAt(innerParam);
		On3dVector val3 = outerPoint - (IOn3dPoint)(object)innerPoint;
		val3.Unitize();
		On3dVector destination = outerRail.TangentAt(outerParam);
		xOrient = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)innerPoint, (IOn3dVector)(object)val3, (IOn3dVector)(object)destination);
		return string.Empty;
	}

	private string MakeEndProfileParts(OnCurve profile, double paramLeft, double paramRight, double blend, OnXform xUp, out OnCurve crvBase, out OnCurve crvLeft, out OnCurve crvCenter, out OnCurve crvRight)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		crvBase = null;
		OnCurve val = profile.Duplicate();
		val.Trim((IOnInterval)new OnInterval(val.Domain().Min(), paramLeft));
		OnCurve val2 = profile.Duplicate();
		val2.Trim((IOnInterval)new OnInterval(paramRight, val2.Domain().Max()));
		crvCenter = profile.Duplicate();
		crvCenter.Trim((IOnInterval)new OnInterval(paramLeft, paramRight));
		CSUtilGeometry.TrimEndOfCrv(val, blend);
		CSUtilGeometry.TrimStartAndEndOfCrv(crvCenter, blend);
		CSUtilGeometry.TrimStartOfCrv(val2, blend);
		crvLeft = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val, (IOnCurve)(object)crvCenter);
		crvRight = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)crvCenter, (IOnCurve)(object)val2);
		List<IOnCurve> obj = new List<IOnCurve>
		{
			(IOnCurve)(object)val2,
			(IOnCurve)(object)val
		};
		OnCurve[] array = null;
		if (!RhUtil.RhinoMergeCurves(obj.ToArray(), ref array, CSApplicationHelper.Application.Tolerance))
		{
			return "Error with Merge Curve";
		}
		if (array == null || array.Length != 1)
		{
			return "Error with Merge Curves";
		}
		crvBase = array[0];
		((OnGeometry)crvBase).Transform((IOnXform)(object)xUp);
		((OnGeometry)crvLeft).Transform((IOnXform)(object)xUp);
		((OnGeometry)crvCenter).Transform((IOnXform)(object)xUp);
		((OnGeometry)crvRight).Transform((IOnXform)(object)xUp);
		return string.Empty;
	}

	private string MakeGemProfileParts(OnCurve profile, double paramLeft, double paramRight, double blend, double wallDrop, double gemZ, OnXform xUp, out OnCurve crvBase, out OnCurve crvLeft, out OnCurve crvCenter, out OnCurve crvRight)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		crvBase = null;
		crvLeft = null;
		crvRight = null;
		On3dPoint val = profile.PointAt(paramLeft);
		On3dPoint val2 = profile.PointAt(paramRight);
		double num = (val.y + val2.y) / 2.0 - gemZ;
		On3dPoint val3 = new On3dPoint(0.0, num, 0.0);
		OnCurve val4 = profile.Duplicate();
		val4.Trim((IOnInterval)new OnInterval(val4.Domain().Min(), paramLeft));
		OnCurve val9;
		OnCurve val12;
		if (wallDrop > blend)
		{
			On3dPoint val5 = new On3dPoint((IOn3dPoint)(object)val);
			val5.y -= wallDrop;
			On3dPoint val6 = new On3dPoint((IOn3dPoint)(object)val2);
			val6.y -= wallDrop;
			OnCurve val7 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val, (IOn3dPoint)(object)val5);
			OnCurve val8 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val5, (IOn3dPoint)(object)val3);
			CSUtilGeometry.TrimEndOfCrv(val7, blend);
			CSUtilGeometry.TrimStartOfCrv(val8, blend);
			OnCurve item = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val7, (IOnCurve)(object)val8);
			List<IOnCurve> obj = new List<IOnCurve>
			{
				(IOnCurve)(object)val7,
				(IOnCurve)(object)item,
				(IOnCurve)(object)val8
			};
			OnCurve[] array = null;
			RhUtil.RhinoMergeCurves(obj.ToArray(), ref array, CSApplicationHelper.Application.Tolerance);
			val9 = array[0];
			OnCurve val10 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val3, (IOn3dPoint)(object)val6);
			OnCurve val11 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val2);
			CSUtilGeometry.TrimEndOfCrv(val10, blend);
			CSUtilGeometry.TrimStartOfCrv(val11, blend);
			OnCurve item2 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val10, (IOnCurve)(object)val11);
			List<IOnCurve> obj2 = new List<IOnCurve>
			{
				(IOnCurve)(object)val10,
				(IOnCurve)(object)item2,
				(IOnCurve)(object)val11
			};
			array = null;
			RhUtil.RhinoMergeCurves(obj2.ToArray(), ref array, CSApplicationHelper.Application.Tolerance);
			val12 = array[0];
		}
		else
		{
			val9 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val, (IOn3dPoint)(object)val3);
			val12 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val3, (IOn3dPoint)(object)val2);
		}
		OnCurve val13 = profile.Duplicate();
		val13.Trim((IOnInterval)new OnInterval(paramRight, val13.Domain().Max()));
		CSUtilGeometry.TrimEndOfCrv(val4, blend);
		CSUtilGeometry.TrimStartAndEndOfCrv(val9, blend);
		CSUtilGeometry.TrimStartAndEndOfCrv(val12, blend);
		CSUtilGeometry.TrimStartOfCrv(val13, blend);
		OnCurve item3 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val4, (IOnCurve)(object)val9);
		crvCenter = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val9, (IOnCurve)(object)val12);
		OnCurve item4 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val12, (IOnCurve)(object)val13);
		List<IOnCurve> obj3 = new List<IOnCurve>
		{
			(IOnCurve)(object)val4,
			(IOnCurve)(object)val13
		};
		OnCurve[] array2 = null;
		if (!RhUtil.RhinoMergeCurves(obj3.ToArray(), ref array2, CSApplicationHelper.Application.Tolerance))
		{
			return "Error with Merge Curves";
		}
		if (array2 == null || array2.Length != 1)
		{
			return "Error with Merge Curves";
		}
		crvBase = array2[0];
		List<IOnCurve> obj4 = new List<IOnCurve>
		{
			(IOnCurve)(object)val9,
			(IOnCurve)(object)item3
		};
		array2 = null;
		if (!RhUtil.RhinoMergeCurves(obj4.ToArray(), ref array2, CSApplicationHelper.Application.Tolerance))
		{
			return "Error with Merge Curves";
		}
		if (array2 == null || array2.Length != 1)
		{
			return "Error with Merge Curves";
		}
		crvLeft = array2[0];
		List<IOnCurve> obj5 = new List<IOnCurve>
		{
			(IOnCurve)(object)item4,
			(IOnCurve)(object)val12
		};
		array2 = null;
		if (!RhUtil.RhinoMergeCurves(obj5.ToArray(), ref array2, CSApplicationHelper.Application.Tolerance))
		{
			return "Error with Merge Curves";
		}
		if (array2 == null || array2.Length != 1)
		{
			return "Error with Merge Curves";
		}
		crvRight = array2[0];
		((OnGeometry)crvBase).Transform((IOnXform)(object)xUp);
		((OnGeometry)crvLeft).Transform((IOnXform)(object)xUp);
		((OnGeometry)crvCenter).Transform((IOnXform)(object)xUp);
		((OnGeometry)crvRight).Transform((IOnXform)(object)xUp);
		return string.Empty;
	}

	private OnSurface MakeSrf(On3dPoint center, On3dVector dir, int orientMethod)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_012c: Expected O, but got Unknown
		On3dVector val;
		On3dVector val2;
		if (orientMethod == 0)
		{
			val = new On3dVector(center.x, 0.0, center.z);
			val.Unitize();
			val2 = new On3dVector(0.0, 1.0, 0.0);
		}
		else
		{
			OnPlane val3 = new OnPlane((IOn3dPoint)(object)center, (IOn3dVector)(object)dir);
			val2 = new On3dVector(val3.GetXaxis());
			val = new On3dVector(val3.GetYaxis());
		}
		On3dPoint val4 = new On3dPoint((IOn3dPoint)(object)(center + (IOn3dVector)(object)(val2 * 50.0) + (IOn3dVector)(object)(val * 50.0)));
		On3dPoint val5 = new On3dPoint((IOn3dPoint)(object)(center + (IOn3dVector)(object)(val2 * 50.0) - (IOn3dVector)(object)(val * 50.0)));
		On3dPoint val6 = new On3dPoint((IOn3dPoint)(object)(center - (IOn3dVector)(object)(val2 * 50.0) - (IOn3dVector)(object)(val * 50.0)));
		On3dPoint val7 = new On3dPoint((IOn3dPoint)(object)(center - (IOn3dVector)(object)(val2 * 50.0) + (IOn3dVector)(object)(val * 50.0)));
		return (OnSurface)(object)RhUtil.RhinoCreateSurfaceFromCorners((IOn3dPoint)val4, (IOn3dPoint)(object)val5, ref val6, ref val7);
	}

	private void ScaleProfile(OnCurve profile, On3dPoint outerPoint, On3dPoint innerPoint, out OnCurve scaledProfile)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		double num = (outerPoint - (IOn3dPoint)(object)innerPoint).Length();
		scaledProfile = profile.Duplicate();
		OnXform val = new OnXform();
		val.Scale(1.0, num, 0.0);
		((OnGeometry)scaledProfile).Transform((IOnXform)(object)val);
	}
}
