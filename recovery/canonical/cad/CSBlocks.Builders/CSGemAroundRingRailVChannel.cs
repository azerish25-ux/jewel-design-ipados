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
		CSDataArray flattenArray = InInnerRail.GetFlattenArray();
		if (flattenArray == null || CSDataHelper.GetArrayCount(flattenArray) < 1)
		{
			return UpdateMissing("Curve");
		}
		OnCurve onCurve = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		if (onCurve == null)
		{
			return UpdateMissing("Curve");
		}
		CSDataArray flattenArray2 = InOuterRail.GetFlattenArray();
		if (flattenArray2 == null || CSDataHelper.GetArrayCount(flattenArray2) < 1)
		{
			return UpdateMissing("Curve");
		}
		OnCurve onCurve2 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		if (onCurve2 == null)
		{
			return UpdateMissing("Curve");
		}
		CSData data = InProfile.Data;
		if (data == null)
		{
			return UpdateMissing("Profile");
		}
		OnCurve onCurve3 = data.GetNurbsCurve().Convert();
		if (onCurve3 == null)
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
		string text = GetParams(onCurve2, onCurve, value3, value11, out var innerParam, out var outerPoint, out var innerPoint, out var xOrient);
		if (!string.IsNullOrWhiteSpace(text))
		{
			return UpdateError(text);
		}
		text = GetParams(onCurve2, onCurve, value4, value11, out var innerParam2, out var outerPoint2, out var innerPoint2, out var xOrient2);
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
			text = GetParams(onCurve2, onCurve, value5, value11, out innerParam3, out outerPoint3, out innerPoint3, out xOrient3);
			if (!string.IsNullOrWhiteSpace(text))
			{
				return UpdateError(text);
			}
			text = GetParams(onCurve2, onCurve, value6, value11, out innerParam4, out outerPoint4, out innerPoint4, out xOrient4);
			if (!string.IsNullOrWhiteSpace(text))
			{
				return UpdateError(text);
			}
		}
		ScaleProfile(onCurve3, outerPoint, innerPoint, out var scaledProfile);
		ScaleProfile(onCurve3, outerPoint2, innerPoint2, out var scaledProfile2);
		OnCurve scaledProfile3 = null;
		OnCurve scaledProfile4 = null;
		if (value10)
		{
			ScaleProfile(onCurve3, outerPoint3, innerPoint3, out scaledProfile3);
			ScaleProfile(onCurve3, outerPoint4, innerPoint4, out scaledProfile4);
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
			List<IOnCurve> list = new List<IOnCurve> { crvBase, crvBase3, crvBase4, crvBase2 };
			List<double> list2 = new List<double> { innerParam, innerParam3, innerParam4, innerParam2 };
			List<double> list3 = new List<double> { value3, value5, value6, value4 };
			if (!CSUtilSweep.Sweep2(onCurve, onCurve2, list.ToArray(), list2.ToArray(), list3.ToArray(), out var breps, closed: false, maintainHeight: true))
			{
				return UpdateError("Sweep2");
			}
			if (breps == null || breps.Length != 1)
			{
				return UpdateError("NoReturnFromSweep2");
			}
			OnBrep onBrep = breps[0];
			list = new List<IOnCurve> { crvLeft, crvLeft3, crvLeft4, crvLeft2 };
			if (!CSUtilSweep.Sweep2(onCurve, onCurve2, list.ToArray(), list2.ToArray(), list3.ToArray(), out breps, closed: false, maintainHeight: true))
			{
				return UpdateError("Sweep2");
			}
			if (breps == null || breps.Length != 1)
			{
				return UpdateError("NoReturnFromSweep2");
			}
			OnBrep onBrep2 = breps[0];
			OnCurve curveFromBrepEdges = CSUtilBrepCrvs.GetCurveFromBrepEdges(onBrep2, crvLeft.PointAtEnd(), crvLeft2.PointAtEnd(), crvLeft3.PointAtEnd());
			list = new List<IOnCurve> { crvRight, crvRight3, crvRight4, crvRight2 };
			if (!CSUtilSweep.Sweep2(onCurve, onCurve2, list.ToArray(), list2.ToArray(), list3.ToArray(), out breps, closed: false, maintainHeight: true))
			{
				return UpdateError("Sweep2");
			}
			if (breps == null || breps.Length != 1)
			{
				return UpdateError("NoReturnFromSweep2");
			}
			OnBrep onBrep3 = breps[0];
			OnCurve curveFromBrepEdges2 = CSUtilBrepCrvs.GetCurveFromBrepEdges(onBrep3, crvRight.PointAtStart(), crvRight2.PointAtStart(), crvRight3.PointAtStart());
			double length = 0.0;
			curveFromBrepEdges.GetLength(ref length);
			double length2 = 0.0;
			curveFromBrepEdges2.GetLength(ref length2);
			double t = 0.0;
			curveFromBrepEdges.GetNormalizedArcLengthPoint(value9 / length, ref t);
			double t2 = 0.0;
			curveFromBrepEdges2.GetNormalizedArcLengthPoint(value9 / length2, ref t2);
			On3dPoint arg = curveFromBrepEdges.PointAt(t);
			On3dPoint arg2 = curveFromBrepEdges2.PointAt(t2);
			OnCurve item = new OnLineCurve(arg, arg2);
			double t3 = 0.0;
			curveFromBrepEdges.GetNormalizedArcLengthPoint((length - value9) / length, ref t3);
			double t4 = 0.0;
			curveFromBrepEdges2.GetNormalizedArcLengthPoint((length2 - value9) / length2, ref t4);
			On3dPoint arg3 = curveFromBrepEdges.PointAt(t3);
			On3dPoint arg4 = curveFromBrepEdges2.PointAt(t4);
			OnCurve item2 = new OnLineCurve(arg3, arg4);
			list2.Clear();
			double t5 = 0.0;
			curveFromBrepEdges.GetClosestPoint(crvCenter.PointAtStart(), ref t5, CSApplicationHelper.Application.Tolerance);
			list2.Add(t5);
			if (value9 > 0.01)
			{
				list2.Add(t);
			}
			curveFromBrepEdges.GetClosestPoint(crvCenter3.PointAtStart(), ref t5, CSApplicationHelper.Application.Tolerance);
			list2.Add(t5);
			curveFromBrepEdges.GetClosestPoint(crvCenter4.PointAtStart(), ref t5, CSApplicationHelper.Application.Tolerance);
			list2.Add(t5);
			if (value9 > 0.01)
			{
				list2.Add(t3);
			}
			curveFromBrepEdges.GetClosestPoint(crvCenter2.PointAtStart(), ref t5, CSApplicationHelper.Application.Tolerance);
			list2.Add(t5);
			list3.Clear();
			curveFromBrepEdges2.GetClosestPoint(crvCenter.PointAtEnd(), ref t5, CSApplicationHelper.Application.Tolerance);
			list3.Add(t5);
			if (value9 > 0.01)
			{
				list3.Add(t2);
			}
			curveFromBrepEdges2.GetClosestPoint(crvCenter3.PointAtEnd(), ref t5, CSApplicationHelper.Application.Tolerance);
			list3.Add(t5);
			curveFromBrepEdges2.GetClosestPoint(crvCenter4.PointAtEnd(), ref t5, CSApplicationHelper.Application.Tolerance);
			list3.Add(t5);
			if (value9 > 0.01)
			{
				list3.Add(t4);
			}
			curveFromBrepEdges2.GetClosestPoint(crvCenter2.PointAtEnd(), ref t5, CSApplicationHelper.Application.Tolerance);
			list3.Add(t5);
			list = new List<IOnCurve>();
			list.Add(crvCenter);
			if (value9 > 0.01)
			{
				list.Add(item);
			}
			list.Add(crvCenter3);
			list.Add(crvCenter4);
			if (value9 > 0.01)
			{
				list.Add(item2);
			}
			list.Add(crvCenter2);
			if (!CSUtilSweep.Sweep2(curveFromBrepEdges, curveFromBrepEdges2, list.ToArray(), list2.ToArray(), list3.ToArray(), out breps, closed: false, maintainHeight: true))
			{
				return UpdateError("Sweep2");
			}
			if (breps == null || breps.Length != 1)
			{
				return UpdateError("NoReturnFromSweep2");
			}
			OnBrep onBrep4 = breps[0];
			brep = CSUtilRhino.JoinAllBreps(new OnBrep[4] { onBrep, onBrep2, onBrep4, onBrep3 }, CSApplicationHelper.Application.Tolerance);
			scaledProfile.Transform(xOrient);
			scaledProfile2.Transform(xOrient2);
		}
		else
		{
			scaledProfile.Transform(xOrient);
			scaledProfile2.Transform(xOrient2);
			List<IOnCurve> list4 = new List<IOnCurve> { scaledProfile, scaledProfile2 };
			List<double> list5 = new List<double> { innerParam, innerParam2 };
			List<double> list6 = new List<double> { value3, value4 };
			if (!CSUtilSweep.Sweep2(onCurve, onCurve2, list4.ToArray(), list5.ToArray(), list6.ToArray(), out var breps2, closed: false, maintainHeight: true))
			{
				return UpdateError("Sweep2");
			}
			if (breps2 == null || breps2.Length != 1)
			{
				return UpdateError("NoReturnFromSweep2");
			}
			brep = breps2[0];
		}
		OutSurface.SetData(new CSOnGeometryData(brep.Convert()));
		OutStartProfile.SetData(new CSOnGeometryData(scaledProfile.Convert(), xOrient.Convert(), applyTransform: false));
		OutEndProfile.SetData(new CSOnGeometryData(scaledProfile2.Convert(), xOrient2.Convert(), applyTransform: false));
		return base.Update();
	}

	private string GetParams(OnCurve outerRail, OnCurve innerRail, double outerParam, int orientMethod, out double innerParam, out On3dPoint outerPoint, out On3dPoint innerPoint, out OnXform xOrient)
	{
		innerPoint = null;
		xOrient = null;
		innerParam = 0.0;
		outerPoint = outerRail.PointAt(outerParam);
		On3dVector dir = outerRail.TangentAt(outerParam);
		OnSurface surfaceB = MakeSrf(outerPoint, dir, orientMethod);
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		innerRail.IntersectSurface(surfaceB, ref x, CSApplicationHelper.Application.Tolerance);
		if (x.Count() == 0)
		{
			return "Fail intersect";
		}
		double num = 99999.0;
		for (int i = 0; i < x.Count(); i++)
		{
			double num2 = outerPoint.DistanceTo(x[i].get_m_pointA(0));
			if (num2 < num)
			{
				num = num2;
				innerParam = x[i].get_m_a(0);
			}
		}
		innerPoint = innerRail.PointAt(innerParam);
		On3dVector on3dVector = outerPoint - innerPoint;
		on3dVector.Unitize();
		On3dVector destination = outerRail.TangentAt(outerParam);
		xOrient = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, innerPoint, on3dVector, destination);
		return string.Empty;
	}

	private string MakeEndProfileParts(OnCurve profile, double paramLeft, double paramRight, double blend, OnXform xUp, out OnCurve crvBase, out OnCurve crvLeft, out OnCurve crvCenter, out OnCurve crvRight)
	{
		crvBase = null;
		OnCurve onCurve = profile.Duplicate();
		onCurve.Trim(new OnInterval(onCurve.Domain().Min(), paramLeft));
		OnCurve onCurve2 = profile.Duplicate();
		onCurve2.Trim(new OnInterval(paramRight, onCurve2.Domain().Max()));
		crvCenter = profile.Duplicate();
		crvCenter.Trim(new OnInterval(paramLeft, paramRight));
		CSUtilGeometry.TrimEndOfCrv(onCurve, blend);
		CSUtilGeometry.TrimStartAndEndOfCrv(crvCenter, blend);
		CSUtilGeometry.TrimStartOfCrv(onCurve2, blend);
		crvLeft = CSUtilCurves.BlendCurve(onCurve, crvCenter);
		crvRight = CSUtilCurves.BlendCurve(crvCenter, onCurve2);
		List<IOnCurve> obj = new List<IOnCurve> { onCurve2, onCurve };
		OnCurve[] output = null;
		if (!RhUtil.RhinoMergeCurves(obj.ToArray(), ref output, CSApplicationHelper.Application.Tolerance))
		{
			return "Error with Merge Curve";
		}
		if (output == null || output.Length != 1)
		{
			return "Error with Merge Curves";
		}
		crvBase = output[0];
		crvBase.Transform(xUp);
		crvLeft.Transform(xUp);
		crvCenter.Transform(xUp);
		crvRight.Transform(xUp);
		return string.Empty;
	}

	private string MakeGemProfileParts(OnCurve profile, double paramLeft, double paramRight, double blend, double wallDrop, double gemZ, OnXform xUp, out OnCurve crvBase, out OnCurve crvLeft, out OnCurve crvCenter, out OnCurve crvRight)
	{
		crvBase = null;
		crvLeft = null;
		crvRight = null;
		On3dPoint on3dPoint = profile.PointAt(paramLeft);
		On3dPoint on3dPoint2 = profile.PointAt(paramRight);
		double y = (on3dPoint.y + on3dPoint2.y) / 2.0 - gemZ;
		On3dPoint on3dPoint3 = new On3dPoint(0.0, y, 0.0);
		OnCurve onCurve = profile.Duplicate();
		onCurve.Trim(new OnInterval(onCurve.Domain().Min(), paramLeft));
		OnCurve onCurve4;
		OnCurve onCurve7;
		if (wallDrop > blend)
		{
			On3dPoint on3dPoint4 = new On3dPoint(on3dPoint);
			on3dPoint4.y -= wallDrop;
			On3dPoint on3dPoint5 = new On3dPoint(on3dPoint2);
			on3dPoint5.y -= wallDrop;
			OnCurve onCurve2 = new OnLineCurve(on3dPoint, on3dPoint4);
			OnCurve onCurve3 = new OnLineCurve(on3dPoint4, on3dPoint3);
			CSUtilGeometry.TrimEndOfCrv(onCurve2, blend);
			CSUtilGeometry.TrimStartOfCrv(onCurve3, blend);
			OnCurve item = CSUtilCurves.BlendCurve(onCurve2, onCurve3);
			List<IOnCurve> obj = new List<IOnCurve> { onCurve2, item, onCurve3 };
			OnCurve[] output = null;
			RhUtil.RhinoMergeCurves(obj.ToArray(), ref output, CSApplicationHelper.Application.Tolerance);
			onCurve4 = output[0];
			OnCurve onCurve5 = new OnLineCurve(on3dPoint3, on3dPoint5);
			OnCurve onCurve6 = new OnLineCurve(on3dPoint5, on3dPoint2);
			CSUtilGeometry.TrimEndOfCrv(onCurve5, blend);
			CSUtilGeometry.TrimStartOfCrv(onCurve6, blend);
			OnCurve item2 = CSUtilCurves.BlendCurve(onCurve5, onCurve6);
			List<IOnCurve> obj2 = new List<IOnCurve> { onCurve5, item2, onCurve6 };
			output = null;
			RhUtil.RhinoMergeCurves(obj2.ToArray(), ref output, CSApplicationHelper.Application.Tolerance);
			onCurve7 = output[0];
		}
		else
		{
			onCurve4 = new OnLineCurve(on3dPoint, on3dPoint3);
			onCurve7 = new OnLineCurve(on3dPoint3, on3dPoint2);
		}
		OnCurve onCurve8 = profile.Duplicate();
		onCurve8.Trim(new OnInterval(paramRight, onCurve8.Domain().Max()));
		CSUtilGeometry.TrimEndOfCrv(onCurve, blend);
		CSUtilGeometry.TrimStartAndEndOfCrv(onCurve4, blend);
		CSUtilGeometry.TrimStartAndEndOfCrv(onCurve7, blend);
		CSUtilGeometry.TrimStartOfCrv(onCurve8, blend);
		OnCurve item3 = CSUtilCurves.BlendCurve(onCurve, onCurve4);
		crvCenter = CSUtilCurves.BlendCurve(onCurve4, onCurve7);
		OnCurve item4 = CSUtilCurves.BlendCurve(onCurve7, onCurve8);
		List<IOnCurve> obj3 = new List<IOnCurve> { onCurve, onCurve8 };
		OnCurve[] output2 = null;
		if (!RhUtil.RhinoMergeCurves(obj3.ToArray(), ref output2, CSApplicationHelper.Application.Tolerance))
		{
			return "Error with Merge Curves";
		}
		if (output2 == null || output2.Length != 1)
		{
			return "Error with Merge Curves";
		}
		crvBase = output2[0];
		List<IOnCurve> obj4 = new List<IOnCurve> { onCurve4, item3 };
		output2 = null;
		if (!RhUtil.RhinoMergeCurves(obj4.ToArray(), ref output2, CSApplicationHelper.Application.Tolerance))
		{
			return "Error with Merge Curves";
		}
		if (output2 == null || output2.Length != 1)
		{
			return "Error with Merge Curves";
		}
		crvLeft = output2[0];
		List<IOnCurve> obj5 = new List<IOnCurve> { item4, onCurve7 };
		output2 = null;
		if (!RhUtil.RhinoMergeCurves(obj5.ToArray(), ref output2, CSApplicationHelper.Application.Tolerance))
		{
			return "Error with Merge Curves";
		}
		if (output2 == null || output2.Length != 1)
		{
			return "Error with Merge Curves";
		}
		crvRight = output2[0];
		crvBase.Transform(xUp);
		crvLeft.Transform(xUp);
		crvCenter.Transform(xUp);
		crvRight.Transform(xUp);
		return string.Empty;
	}

	private OnSurface MakeSrf(On3dPoint center, On3dVector dir, int orientMethod)
	{
		On3dVector on3dVector;
		On3dVector on3dVector2;
		if (orientMethod == 0)
		{
			on3dVector = new On3dVector(center.x, 0.0, center.z);
			on3dVector.Unitize();
			on3dVector2 = new On3dVector(0.0, 1.0, 0.0);
		}
		else
		{
			OnPlane onPlane = new OnPlane(center, dir);
			on3dVector2 = new On3dVector(onPlane.GetXaxis());
			on3dVector = new On3dVector(onPlane.GetYaxis());
		}
		On3dPoint a = new On3dPoint(center + on3dVector2 * 50.0 + on3dVector * 50.0);
		On3dPoint b = new On3dPoint(center + on3dVector2 * 50.0 - on3dVector * 50.0);
		On3dPoint c = new On3dPoint(center - on3dVector2 * 50.0 - on3dVector * 50.0);
		On3dPoint d = new On3dPoint(center - on3dVector2 * 50.0 + on3dVector * 50.0);
		return RhUtil.RhinoCreateSurfaceFromCorners(a, b, ref c, ref d);
	}

	private void ScaleProfile(OnCurve profile, On3dPoint outerPoint, On3dPoint innerPoint, out OnCurve scaledProfile)
	{
		double y_scale_factor = (outerPoint - innerPoint).Length();
		scaledProfile = profile.Duplicate();
		OnXform onXform = new OnXform();
		onXform.Scale(1.0, y_scale_factor, 0.0);
		scaledProfile.Transform(onXform);
	}
}
