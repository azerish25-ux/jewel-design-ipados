using System;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlockUtil;

public static class CSUtilSMI
{
	public static bool Cutter2ChannelsFromCenter(OnCurve profile, OnXform xUpScaled, double channelWidth, double distance, double cuttingDepth, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width))
		{
			return false;
		}
		if (!Cutter2ChannelsFromCenter(height, channelWidth, distance, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, profileBB.Max().y - cuttingDepth, 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool Cutter2ChannelsFromCenter(double height, double channelWidth, double distance, ref OnCurve outputProfile)
	{
		double num = channelWidth / 2.0;
		double num2 = distance - num;
		if (distance <= num)
		{
			return CutterChannel(height, 2.0 * channelWidth, ref outputProfile);
		}
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(0.0, height, 0.0);
		on3dPointArray.Append(0.0 - num2, height, 0.0);
		on3dPointArray.Append(0.0 - num2, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num2 - channelWidth, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num2 - channelWidth, height + 1.0, 0.0);
		on3dPointArray.Append(num2 + channelWidth, height + 1.0, 0.0);
		on3dPointArray.Append(num2 + channelWidth, 0.0, 0.0);
		on3dPointArray.Append(num2, 0.0, 0.0);
		on3dPointArray.Append(num2, height, 0.0);
		on3dPointArray.Append(0.0, height, 0.0);
		outputProfile = new OnPolylineCurve(on3dPointArray);
		return true;
	}

	public static bool Cutter2ChannelsFromEdge(OnCurve profile, OnXform xUpScaled, double channelWidth, double distance, double cuttingDepth, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width))
		{
			return false;
		}
		if (!Cutter2ChannelsFromEdge(height, width, channelWidth, distance, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, profileBB.Max().y - cuttingDepth, 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool Cutter2ChannelsFromEdge(double height, double width, double channelWidth, double distance, ref OnCurve outputProfile)
	{
		double num = width / 2.0;
		double num2 = channelWidth / 2.0;
		double num3 = distance - num2;
		if (2.0 * (num3 + channelWidth) >= width)
		{
			return CutterChannel(height, 2.0 * channelWidth, ref outputProfile);
		}
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(0.0, height, 0.0);
		on3dPointArray.Append(0.0 - num + num3 + channelWidth, height, 0.0);
		on3dPointArray.Append(0.0 - num + num3 + channelWidth, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num + num3, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num + num3, height + 1.0, 0.0);
		on3dPointArray.Append(num - num3, height + 1.0, 0.0);
		on3dPointArray.Append(num - num3, 0.0, 0.0);
		on3dPointArray.Append(num - num3 - channelWidth, 0.0, 0.0);
		on3dPointArray.Append(num - num3 - channelWidth, height, 0.0);
		on3dPointArray.Append(0.0, height, 0.0);
		outputProfile = new OnPolylineCurve(on3dPointArray);
		return true;
	}

	public static bool CutterChamfer(OnCurve profile, OnXform xUpScaled, double angleD, double cuttingDepth, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width))
		{
			return false;
		}
		if (!CutterChamfer(height, width, angleD, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, profileBB.Max().y - cuttingDepth, 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool CutterChamfer(double height, double width, double angleD, ref OnCurve outputProfile)
	{
		if (angleD == 0.0)
		{
			return Rectangle(height + 1.0, width + 2.0, ref outputProfile);
		}
		double num = CSUtilMath.ToRadians(angleD);
		double num2 = width / 2.0;
		On3dPoint point = new On3dPoint(0.0 - num2, 0.0, 0.0);
		On3dPoint point2 = new On3dPoint(num2, 0.0, 0.0);
		On3dVector on3dVector = new On3dVector(OnUtil.On_xaxis);
		on3dVector.Rotate(num, OnUtil.On_zaxis);
		On3dVector on3dVector2 = new On3dVector(OnUtil.On_xaxis) * -1.0;
		on3dVector2.Rotate(0.0 - num, OnUtil.On_zaxis);
		On3dPoint outpoint = null;
		if (!CSUtilMath.LineInt(out outpoint, point, on3dVector, point2, on3dVector2))
		{
			return false;
		}
		if (outpoint.y > height)
		{
			height = outpoint.y;
		}
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(0.0, outpoint.y, 0.0);
		on3dPointArray.Append(0.0 - num2, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num2 - 1.0, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num2 - 1.0, height + 1.0, 0.0);
		on3dPointArray.Append(num2 + 1.0, height + 1.0, 0.0);
		on3dPointArray.Append(num2 + 1.0, 0.0, 0.0);
		on3dPointArray.Append(num2, 0.0, 0.0);
		on3dPointArray.Append(0.0, outpoint.y, 0.0);
		outputProfile = new OnPolylineCurve(on3dPointArray);
		return true;
	}

	public static bool CutterChamferWithFlat(OnCurve profile, OnXform xUpScaled, double angleD, double depth, double cuttingDepth, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width))
		{
			return false;
		}
		if (!CutterChamferWithFlat(height, width, angleD, depth, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, profileBB.Max().y - cuttingDepth, 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool CutterChamferWithFlat(double height, double width, double angleD, double depth, ref OnCurve outputProfile)
	{
		if (angleD == 0.0 || depth == 0.0)
		{
			return Rectangle(height + 1.0, width + 2.0, ref outputProfile);
		}
		double num = CSUtilMath.ToRadians(angleD);
		double num2 = width / 2.0;
		if (depth > height)
		{
			height = depth;
		}
		On3dPoint point = new On3dPoint(0.0 - num2, 0.0, 0.0);
		On3dPoint on3dPoint = new On3dPoint(num2, 0.0, 0.0);
		On3dPoint point2 = new On3dPoint(num2, depth, 0.0);
		On3dVector on3dVector = new On3dVector(OnUtil.On_xaxis);
		on3dVector.Rotate(num, OnUtil.On_zaxis);
		On3dVector on3dVector2 = new On3dVector(OnUtil.On_xaxis) * -1.0;
		on3dVector2.Rotate(0.0 - num, OnUtil.On_zaxis);
		On3dVector vector = new On3dVector(OnUtil.On_xaxis);
		On3dPoint outpoint = null;
		if (!CSUtilMath.LineInt(out outpoint, point, on3dVector, on3dPoint, on3dVector2))
		{
			return false;
		}
		On3dPoint outpoint2 = null;
		if (!CSUtilMath.LineInt(out outpoint2, point, on3dVector, point2, vector))
		{
			return false;
		}
		On3dPoint outpoint3 = null;
		if (!CSUtilMath.LineInt(out outpoint3, on3dPoint, on3dVector2, point2, vector))
		{
			return false;
		}
		double num3 = (outpoint2.y + outpoint3.y) / 2.0;
		if (num3 > outpoint.y)
		{
			return CutterChamfer(height, width, angleD, ref outputProfile);
		}
		if (num3 > height)
		{
			height = num3;
		}
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(0.0, num3, 0.0);
		on3dPointArray.Append(outpoint2.x, num3, 0.0);
		on3dPointArray.Append(0.0 - num2, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num2 - 1.0, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num2 - 1.0, height + 1.0, 0.0);
		on3dPointArray.Append(num2 + 1.0, height + 1.0, 0.0);
		on3dPointArray.Append(num2 + 1.0, 0.0, 0.0);
		on3dPointArray.Append(num2, 0.0, 0.0);
		on3dPointArray.Append(outpoint3.x, num3, 0.0);
		on3dPointArray.Append(0.0, num3, 0.0);
		outputProfile = new OnPolylineCurve(on3dPointArray);
		return true;
	}

	public static bool CutterChannel(double height, double channelWidth, ref OnCurve outputProfile)
	{
		double num = channelWidth / 2.0;
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(0.0, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num, height + 1.0, 0.0);
		on3dPointArray.Append(num, height + 1.0, 0.0);
		on3dPointArray.Append(num, 0.0, 0.0);
		on3dPointArray.Append(0.0, 0.0, 0.0);
		outputProfile = new OnPolylineCurve(on3dPointArray);
		return true;
	}

	public static bool CutterChannelFromCenter(OnCurve profile, OnXform xUpScaled, double width, double cuttingDepth, double distance, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width2 = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width2))
		{
			return false;
		}
		if (!CutterChannel(height, width, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(distance, profileBB.Max().y - cuttingDepth, 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool CutterChannelFromEdge(OnCurve profile, OnXform xUpScaled, double width, double cuttingDepth, double distance, bool flip, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width2 = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width2))
		{
			return false;
		}
		if (!CutterChannel(height, width, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		if (flip)
		{
			onXform.Translation(width2 / 2.0 - distance, profileBB.Max().y - cuttingDepth, 0.0);
		}
		else
		{
			onXform.Translation(0.0 - width2 / 2.0 + distance, profileBB.Max().y - cuttingDepth, 0.0);
		}
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool CutterComfortFit(OnCurve profile, OnXform xUpScaled, double edgeHeight, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width))
		{
			return false;
		}
		if (!CutterComfortFit(height, width, edgeHeight, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, 0.0 - (profileBB.Max().y + 1.0), 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool CutterComfortFit(double height, double width, double edgeHeight, ref OnCurve outputProfile)
	{
		if (edgeHeight == 0.0)
		{
			return Rectangle(height + 1.0, width + 2.0, ref outputProfile);
		}
		double num = width / 2.0;
		On3dPoint p = new On3dPoint(0.0 - num, num, 0.0);
		On3dPoint q = new On3dPoint(0.0, 0.0, 0.0);
		On3dPoint r = new On3dPoint(num, num, 0.0);
		OnArcCurve onArcCurve = new OnArc(p, q, r);
		OnXform onXform = new OnXform();
		onXform.Scale(1.0, edgeHeight / num, 1.0);
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		onArcCurve.GetNurbForm(ref nurbs_curve);
		nurbs_curve.Transform(onXform);
		nurbs_curve.Translate(new On3dVector(0.0, height + 1.0, 0.0));
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(num, edgeHeight + height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, edgeHeight + height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, edgeHeight + height + 1.0, 0.0);
		on3dPointArray.Append(0.0 - num, edgeHeight + height + 1.0, 0.0);
		OnPolylineCurve onPolylineCurve = new OnPolylineCurve(on3dPointArray);
		OnCurve[] obj = new OnCurve[2] { nurbs_curve, onPolylineCurve };
		OnCurve[] output = null;
		IOnCurve[] input_curves = obj;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return false;
		}
		if (output.Length != 1)
		{
			return false;
		}
		outputProfile = output[0];
		double t = 0.0;
		outputProfile.GetClosestPoint(new On3dPoint(0.0, 0.0, 0.0), ref t);
		outputProfile.ChangeClosedCurveSeam(t);
		return true;
	}

	public static bool CutterHollow(OnCurve profile, OnXform xUpScaled, double width, double depth, double cuttingDepth, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width2 = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width2))
		{
			return false;
		}
		if (!CutterHollow(height, width, depth, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, 0.0 - (profileBB.Max().y + 1.0) + cuttingDepth, 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool CutterHollow(double height, double width, double depth, ref OnCurve outputProfile)
	{
		if (depth == 0.0)
		{
			return Rectangle(height + 1.0, width, ref outputProfile);
		}
		double num = width / 2.0;
		if (Math.Abs(depth) > num)
		{
			depth = ((!(depth < 0.0)) ? num : (0.0 - num));
		}
		On3dPoint p = new On3dPoint(0.0 - num, height + 1.0 - depth, 0.0);
		On3dPoint q = new On3dPoint(0.0, height + 1.0, 0.0);
		On3dPoint r = new On3dPoint(num, height + 1.0 - depth, 0.0);
		OnArcCurve onArcCurve = new OnArc(p, q, r);
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(num, height + 1.0 - depth, 0.0);
		on3dPointArray.Append(num, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num, height + 1.0 - depth, 0.0);
		OnPolylineCurve onPolylineCurve = new OnPolylineCurve(on3dPointArray);
		OnCurve[] obj = new OnCurve[2] { onArcCurve, onPolylineCurve };
		OnCurve[] output = null;
		IOnCurve[] input_curves = obj;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return false;
		}
		if (output.Length != 1)
		{
			return false;
		}
		outputProfile = output[0];
		double t = 0.0;
		outputProfile.GetClosestPoint(new On3dPoint(0.0, 0.0, 0.0), ref t);
		outputProfile.ChangeClosedCurveSeam(t);
		return true;
	}

	public static bool CutterInsideRound(OnCurve profile, OnXform xUpScaled, double edgeHeight, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width))
		{
			return false;
		}
		if (!CutterInsideRound(height, width, edgeHeight, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, 0.0 - (profileBB.Max().y + 1.0), 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool CutterInsideRound(double height, double width, double edgeHeight, ref OnCurve outputProfile)
	{
		if (edgeHeight == 0.0)
		{
			return Rectangle(height + 1.0, width + 2.0, ref outputProfile);
		}
		double num = width / 2.0;
		On3dPoint p = new On3dPoint(0.0 - num, edgeHeight + height + 1.0, 0.0);
		On3dPoint q = new On3dPoint(0.0, height + 1.0, 0.0);
		On3dPoint r = new On3dPoint(num, edgeHeight + height + 1.0, 0.0);
		OnArcCurve onArcCurve = new OnArc(p, q, r);
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(num, edgeHeight + height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, edgeHeight + height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, edgeHeight + height + 1.0, 0.0);
		on3dPointArray.Append(0.0 - num, edgeHeight + height + 1.0, 0.0);
		OnPolylineCurve onPolylineCurve = new OnPolylineCurve(on3dPointArray);
		OnCurve[] obj = new OnCurve[2] { onArcCurve, onPolylineCurve };
		OnCurve[] output = null;
		IOnCurve[] input_curves = obj;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return false;
		}
		if (output.Length != 1)
		{
			return false;
		}
		outputProfile = output[0];
		double t = 0.0;
		outputProfile.GetClosestPoint(new On3dPoint(0.0, 0.0, 0.0), ref t);
		outputProfile.ChangeClosedCurveSeam(t);
		return true;
	}

	public static bool CutterInsideRoundWithFlat(OnCurve profile, OnXform xUpScaled, double middleWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width))
		{
			return false;
		}
		if (!CutterInsideRoundWithFlat(height, width, middleWidth, edgeHeight, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, 0.0 - (profileBB.Max().y + 1.0), 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool CutterInsideRoundWithFlat(double height, double width, double middleWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		if (edgeHeight == 0.0)
		{
			return Rectangle(height + 1.0, width + 2.0, ref outputProfile);
		}
		if (middleWidth == 0.0)
		{
			return CutterInsideRound(height, width, edgeHeight, ref outputProfile);
		}
		double num = width / 2.0;
		double num2 = middleWidth / 2.0;
		if (num2 > num)
		{
			num2 = num;
		}
		if (num - num2 < edgeHeight)
		{
			edgeHeight = num - num2;
		}
		double num3 = num - num2;
		On3dPoint p = new On3dPoint(0.0 - num + 2.0 * num3, edgeHeight + height + 1.0, 0.0);
		On3dPoint q = new On3dPoint(0.0 - num + num3, height + 1.0, 0.0);
		On3dPoint r = new On3dPoint(0.0 - num, edgeHeight + height + 1.0, 0.0);
		OnArcCurve onArcCurve = new OnArc(p, q, r);
		onArcCurve.Trim(new OnInterval(onArcCurve.Domain().Mid(), onArcCurve.Domain().Max()));
		On3dPoint p2 = new On3dPoint(num, edgeHeight + height + 1.0, 0.0);
		On3dPoint q2 = new On3dPoint(num - num3, height + 1.0, 0.0);
		On3dPoint r2 = new On3dPoint(num - 2.0 * num3, edgeHeight + height + 1.0, 0.0);
		OnArcCurve onArcCurve2 = new OnArc(p2, q2, r2);
		onArcCurve2.Trim(new OnInterval(onArcCurve2.Domain().Min(), onArcCurve2.Domain().Mid()));
		OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(num - num3, height + 1.0, 0.0), new On3dPoint(0.0 - num + num3, height + 1.0, 0.0));
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(num, edgeHeight + height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, edgeHeight + height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, edgeHeight + height + 1.0, 0.0);
		on3dPointArray.Append(0.0 - num, edgeHeight + height + 1.0, 0.0);
		OnPolylineCurve onPolylineCurve = new OnPolylineCurve(on3dPointArray);
		OnCurve[] obj = new OnCurve[4] { onArcCurve, onLineCurve, onArcCurve2, onPolylineCurve };
		OnCurve[] output = null;
		IOnCurve[] input_curves = obj;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return false;
		}
		if (output.Length != 1)
		{
			return false;
		}
		outputProfile = output[0];
		double t = 0.0;
		outputProfile.GetClosestPoint(new On3dPoint(0.0, 0.0, 0.0), ref t);
		outputProfile.ChangeClosedCurveSeam(t);
		return true;
	}

	public static bool CutterOval(OnCurve profile, OnXform xUpScaled, double edgeHeight, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width))
		{
			return false;
		}
		if (!CutterOval(height, width, edgeHeight, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, profileBB.Max().y, 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool CutterOval(double height, double width, double edgeHeight, ref OnCurve outputProfile)
	{
		if (edgeHeight == 0.0)
		{
			return Rectangle(height + 1.0, width + 2.0, ref outputProfile);
		}
		double num = width / 2.0;
		On3dPoint p = new On3dPoint(0.0 - num, 0.0, 0.0);
		On3dPoint q = new On3dPoint(0.0, num, 0.0);
		On3dPoint r = new On3dPoint(num, 0.0, 0.0);
		OnArcCurve onArcCurve = new OnArc(p, q, r);
		OnXform onXform = new OnXform();
		onXform.Scale(1.0, edgeHeight / num, 1.0);
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		onArcCurve.GetNurbForm(ref nurbs_curve);
		nurbs_curve.Transform(onXform);
		nurbs_curve.Translate(new On3dVector(0.0, 0.0 - edgeHeight, 0.0));
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(0.0 - num, 0.0 - edgeHeight, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, 0.0 - edgeHeight, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, 0.0 - edgeHeight, 0.0);
		on3dPointArray.Append(num, 0.0 - edgeHeight, 0.0);
		OnPolylineCurve onPolylineCurve = new OnPolylineCurve(on3dPointArray);
		OnCurve[] obj = new OnCurve[2] { nurbs_curve, onPolylineCurve };
		OnCurve[] output = null;
		IOnCurve[] input_curves = obj;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return false;
		}
		if (output.Length != 1)
		{
			return false;
		}
		outputProfile = output[0];
		double t = 0.0;
		outputProfile.GetClosestPoint(OnUtil.On_origin, ref t);
		outputProfile.ChangeClosedCurveSeam(t);
		return true;
	}

	public static bool CutterTopCenter(OnCurve profile, OnXform xUpScaled, double width, double depth, double cuttingDepth, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width2 = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width2))
		{
			return false;
		}
		if (!CutterTopCenter(height, width, depth, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, profileBB.Max().y - cuttingDepth, 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool CutterTopCenter(double height, double width, double depth, ref OnCurve outputProfile)
	{
		if (depth == 0.0)
		{
			return Rectangle(height + 1.0, width, ref outputProfile);
		}
		double num = width / 2.0;
		if (Math.Abs(depth) > num)
		{
			depth = ((!(depth < 0.0)) ? num : (0.0 - num));
		}
		On3dPoint r = new On3dPoint(num, depth, 0.0);
		On3dPoint q = new On3dPoint(OnUtil.On_origin);
		OnArcCurve onArcCurve = new OnArc(new On3dPoint(0.0 - num, depth, 0.0), q, r);
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(0.0 - num, depth, 0.0);
		on3dPointArray.Append(0.0 - num, height + 1.0, 0.0);
		on3dPointArray.Append(num, height + 1.0, 0.0);
		on3dPointArray.Append(num, depth, 0.0);
		OnPolylineCurve onPolylineCurve = new OnPolylineCurve(on3dPointArray);
		OnCurve[] obj = new OnCurve[2] { onArcCurve, onPolylineCurve };
		OnCurve[] output = null;
		IOnCurve[] input_curves = obj;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return false;
		}
		if (output.Length != 1)
		{
			return false;
		}
		outputProfile = output[0];
		double t = 0.0;
		outputProfile.GetClosestPoint(new On3dPoint(0.0, 0.0, 0.0), ref t);
		outputProfile.ChangeClosedCurveSeam(t);
		return true;
	}

	public static bool CutterTopEdge(OnCurve profile, OnXform xUpScaled, double edgeHeight, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width))
		{
			return false;
		}
		if (!CutterTopEdge(height, width, edgeHeight, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, profileBB.Max().y, 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool CutterTopEdge(double height, double width, double edgeHeight, ref OnCurve outputProfile)
	{
		if (edgeHeight == 0.0)
		{
			return Rectangle(height + 1.0, width + 2.0, ref outputProfile);
		}
		double num = width / 2.0;
		On3dPoint p = new On3dPoint(0.0 - num, 0.0 - edgeHeight, 0.0);
		On3dPoint q = new On3dPoint(0.0, 0.0, 0.0);
		On3dPoint r = new On3dPoint(num, 0.0 - edgeHeight, 0.0);
		OnArcCurve onArcCurve = new OnArc(p, q, r);
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(0.0 - num, 0.0 - edgeHeight, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, 0.0 - edgeHeight, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, 0.0 - edgeHeight, 0.0);
		on3dPointArray.Append(num, 0.0 - edgeHeight, 0.0);
		OnPolylineCurve onPolylineCurve = new OnPolylineCurve(on3dPointArray);
		OnCurve[] obj = new OnCurve[2] { onArcCurve, onPolylineCurve };
		OnCurve[] output = null;
		IOnCurve[] input_curves = obj;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return false;
		}
		if (output.Length != 1)
		{
			return false;
		}
		outputProfile = output[0];
		double t = 0.0;
		outputProfile.GetClosestPoint(OnUtil.On_origin, ref t);
		outputProfile.ChangeClosedCurveSeam(t);
		return true;
	}

	public static bool CutterTopEdgeWithFlat(OnCurve profile, OnXform xUpScaled, double middleWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width))
		{
			return false;
		}
		if (!CutterTopEdgeWithFlat(height, width, middleWidth, edgeHeight, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, profileBB.Max().y, 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool CutterTopEdgeWithFlat(double height, double width, double middleWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		if (edgeHeight == 0.0)
		{
			return Rectangle(height + 1.0, width + 2.0, ref outputProfile);
		}
		if (middleWidth == 0.0)
		{
			return CutterTopEdge(height, width, edgeHeight, ref outputProfile);
		}
		double num = width / 2.0;
		double num2 = middleWidth / 2.0;
		if (num2 > num)
		{
			num2 = num;
		}
		if (num - num2 < edgeHeight)
		{
			edgeHeight = num - num2;
		}
		double num3 = num - num2;
		On3dPoint p = new On3dPoint(0.0 - num + 2.0 * num3, 0.0 - edgeHeight, 0.0);
		On3dPoint q = new On3dPoint(0.0 - num + num3, 0.0, 0.0);
		On3dPoint r = new On3dPoint(0.0 - num, 0.0 - edgeHeight, 0.0);
		OnArcCurve onArcCurve = new OnArc(p, q, r);
		onArcCurve.Trim(new OnInterval(onArcCurve.Domain().Mid(), onArcCurve.Domain().Max()));
		On3dPoint p2 = new On3dPoint(num, 0.0 - edgeHeight, 0.0);
		On3dPoint q2 = new On3dPoint(num - num3, 0.0, 0.0);
		On3dPoint r2 = new On3dPoint(num - 2.0 * num3, 0.0 - edgeHeight, 0.0);
		OnArcCurve onArcCurve2 = new OnArc(p2, q2, r2);
		onArcCurve2.Trim(new OnInterval(onArcCurve2.Domain().Min(), onArcCurve2.Domain().Mid()));
		OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(num - num3, 0.0, 0.0), new On3dPoint(0.0 - num + num3, 0.0, 0.0));
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(0.0 - num, 0.0 - edgeHeight, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, 0.0 - edgeHeight, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, 0.0 - edgeHeight, 0.0);
		on3dPointArray.Append(num, 0.0 - edgeHeight, 0.0);
		OnPolylineCurve onPolylineCurve = new OnPolylineCurve(on3dPointArray);
		OnCurve[] obj = new OnCurve[4] { onArcCurve, onLineCurve, onArcCurve2, onPolylineCurve };
		OnCurve[] output = null;
		IOnCurve[] input_curves = obj;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return false;
		}
		if (output.Length != 1)
		{
			return false;
		}
		outputProfile = output[0];
		double t = 0.0;
		outputProfile.GetClosestPoint(OnUtil.On_origin, ref t);
		outputProfile.ChangeClosedCurveSeam(t);
		return true;
	}

	public static bool CutterTopWave(OnCurve profile, OnXform xUpScaled, double middleWidth, double outsideWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width))
		{
			return false;
		}
		if (!CutterTopWave(height, width, middleWidth, outsideWidth, edgeHeight, ref outputProfile))
		{
			return false;
		}
		if (edgeHeight / middleWidth > 15.0 / 52.0)
		{
			edgeHeight = 15.0 / 52.0 * middleWidth;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, profileBB.Max().y - edgeHeight, 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool CutterTopWave(double height, double width, double middleWidth, double outsideWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		if (edgeHeight / middleWidth > 15.0 / 52.0)
		{
			edgeHeight = 15.0 / 52.0 * middleWidth;
		}
		double num = width / 2.0;
		double num2 = middleWidth / 2.0;
		double num3 = outsideWidth / 2.0;
		if (num2 + outsideWidth > num)
		{
			num = num2 + outsideWidth;
		}
		if (edgeHeight == 0.0)
		{
			return Rectangle(height + 1.0, 2.0 * num + 2.0, ref outputProfile);
		}
		On3dPoint on3dPoint = new On3dPoint(0.0 - num2, 0.0, 0.0);
		On3dPoint q = new On3dPoint(0.0, edgeHeight, 0.0);
		On3dPoint on3dPoint2 = new On3dPoint(num2, 0.0, 0.0);
		OnArcCurve onArcCurve = new OnArc(on3dPoint, q, on3dPoint2);
		On3dPoint on3dPoint3 = new On3dPoint(new OnCircle(on3dPoint, q, on3dPoint2).Center());
		if (!CSUtilMath.LineInt(out var outpoint, on3dPoint, on3dPoint - on3dPoint3, new On3dPoint(0.0 - num3 + (0.0 - num2), 0.0, 0.0), OnUtil.On_yaxis))
		{
			return false;
		}
		double num4 = outpoint.DistanceTo(on3dPoint);
		On3dPoint on3dPoint4 = new On3dPoint(outpoint.x - Math.Abs(outpoint.x - on3dPoint.x), on3dPoint.y, 0.0);
		On3dPoint q2 = new On3dPoint(outpoint.x, outpoint.y - num4, 0.0);
		On3dPoint r = new On3dPoint(on3dPoint);
		OnArcCurve onArcCurve2 = new OnArc(on3dPoint4, q2, r);
		if (!CSUtilMath.LineInt(out var outpoint2, on3dPoint2, on3dPoint2 - on3dPoint3, new On3dPoint(num3 + num2, 0.0, 0.0), OnUtil.On_yaxis))
		{
			return false;
		}
		double num5 = outpoint2.DistanceTo(on3dPoint2);
		On3dPoint p = new On3dPoint(on3dPoint2);
		On3dPoint q3 = new On3dPoint(outpoint2.x, outpoint2.y - num5, 0.0);
		On3dPoint on3dPoint5 = new On3dPoint(outpoint2.x + Math.Abs(outpoint2.x - on3dPoint2.x), on3dPoint2.y, 0.0);
		OnArcCurve onArcCurve3 = new OnArc(p, q3, on3dPoint5);
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(on3dPoint4.x, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, edgeHeight + height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, edgeHeight + height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, 0.0, 0.0);
		on3dPointArray.Append(on3dPoint5.x, 0.0, 0.0);
		OnPolylineCurve onPolylineCurve = new OnPolylineCurve(on3dPointArray);
		OnCurve[] obj = new OnCurve[4] { onArcCurve2, onArcCurve, onArcCurve3, onPolylineCurve };
		OnCurve[] output = null;
		IOnCurve[] input_curves = obj;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return false;
		}
		if (output.Length != 1)
		{
			return false;
		}
		outputProfile = output[0];
		double t = 0.0;
		outputProfile.GetClosestPoint(new On3dPoint(0.0, edgeHeight, 0.0), ref t);
		outputProfile.ChangeClosedCurveSeam(t);
		return true;
	}

	public static bool CutterTopWave2(OnCurve profile, OnXform xUpScaled, double middleWidth, double insideWidth, double outsideWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width))
		{
			return false;
		}
		if (!CutterTopWave2(height, width, middleWidth, insideWidth, outsideWidth, edgeHeight, ref outputProfile))
		{
			return false;
		}
		if (edgeHeight / middleWidth > 15.0 / 52.0)
		{
			edgeHeight = 15.0 / 52.0 * middleWidth;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, profileBB.Max().y - edgeHeight, 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool CutterTopWave2(double height, double width, double middleWidth, double insideWidth, double outsideWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		if (edgeHeight / middleWidth > 15.0 / 52.0)
		{
			edgeHeight = 15.0 / 52.0 * middleWidth;
		}
		double num = width / 2.0;
		double num2 = middleWidth / 2.0;
		double num3 = insideWidth / 2.0;
		double num4 = outsideWidth / 2.0;
		if (num2 + insideWidth + outsideWidth > num)
		{
			num = num2 + insideWidth + outsideWidth;
		}
		if (edgeHeight == 0.0)
		{
			return Rectangle(height + 1.0, 2.0 * num + 2.0, ref outputProfile);
		}
		On3dPoint on3dPoint = new On3dPoint(0.0 - num2, 0.0, 0.0);
		On3dPoint q = new On3dPoint(0.0, edgeHeight, 0.0);
		On3dPoint on3dPoint2 = new On3dPoint(num2, 0.0, 0.0);
		OnArcCurve onArcCurve = new OnArc(on3dPoint, q, on3dPoint2);
		On3dPoint on3dPoint3 = new On3dPoint(new OnCircle(on3dPoint, q, on3dPoint2).Center());
		if (!CSUtilMath.LineInt(out var outpoint, on3dPoint, on3dPoint - on3dPoint3, new On3dPoint(0.0 - num3 + (0.0 - num2), 0.0, 0.0), OnUtil.On_yaxis))
		{
			return false;
		}
		double num5 = outpoint.DistanceTo(on3dPoint);
		On3dPoint on3dPoint4 = new On3dPoint(outpoint.x - Math.Abs(outpoint.x - on3dPoint.x), on3dPoint.y, 0.0);
		On3dPoint q2 = new On3dPoint(outpoint.x, outpoint.y - num5, 0.0);
		On3dPoint r = new On3dPoint(on3dPoint);
		OnArcCurve onArcCurve2 = new OnArc(on3dPoint4, q2, r);
		if (!CSUtilMath.LineInt(out var outpoint2, on3dPoint4, on3dPoint4 - outpoint, new On3dPoint(0.0 - num4 + (0.0 - insideWidth) + (0.0 - num2), 0.0, 0.0), OnUtil.On_yaxis))
		{
			return false;
		}
		double num6 = outpoint2.DistanceTo(on3dPoint4);
		On3dPoint on3dPoint5 = new On3dPoint(outpoint2.x - Math.Abs(outpoint2.x - on3dPoint4.x), on3dPoint4.y, 0.0);
		On3dPoint on3dPoint6 = new On3dPoint(outpoint2.x, outpoint2.y + num6, 0.0);
		On3dPoint r2 = new On3dPoint(on3dPoint4);
		OnArcCurve onArcCurve3 = new OnArc(on3dPoint5, on3dPoint6, r2);
		if (!CSUtilMath.LineInt(out var outpoint3, on3dPoint2, on3dPoint2 - on3dPoint3, new On3dPoint(num3 + num2, 0.0, 0.0), OnUtil.On_yaxis))
		{
			return false;
		}
		double num7 = outpoint3.DistanceTo(on3dPoint2);
		On3dPoint p = new On3dPoint(on3dPoint2);
		On3dPoint q3 = new On3dPoint(outpoint3.x, outpoint3.y - num7, 0.0);
		On3dPoint on3dPoint7 = new On3dPoint(outpoint3.x + Math.Abs(outpoint3.x - on3dPoint2.x), on3dPoint2.y, 0.0);
		OnArcCurve onArcCurve4 = new OnArc(p, q3, on3dPoint7);
		if (!CSUtilMath.LineInt(out var outpoint4, on3dPoint7, on3dPoint7 - outpoint3, new On3dPoint(num4 + insideWidth + num2, 0.0, 0.0), OnUtil.On_yaxis))
		{
			return false;
		}
		double num8 = outpoint4.DistanceTo(on3dPoint7);
		On3dPoint p2 = new On3dPoint(on3dPoint7);
		On3dPoint on3dPoint8 = new On3dPoint(outpoint4.x, outpoint4.y + num8, 0.0);
		On3dPoint on3dPoint9 = new On3dPoint(outpoint4.x + Math.Abs(outpoint4.x - on3dPoint7.x), on3dPoint7.y, 0.0);
		OnArcCurve onArcCurve5 = new OnArc(p2, on3dPoint8, on3dPoint9);
		double num9 = (on3dPoint6.y + on3dPoint8.y) / 2.0;
		if (num9 > height)
		{
			height = num9;
		}
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(on3dPoint5.x, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num - 1.0, edgeHeight + height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, edgeHeight + height + 1.0, 0.0);
		on3dPointArray.Append(num + 1.0, 0.0, 0.0);
		on3dPointArray.Append(on3dPoint9.x, 0.0, 0.0);
		OnPolylineCurve onPolylineCurve = new OnPolylineCurve(on3dPointArray);
		OnCurve[] obj = new OnCurve[6] { onArcCurve3, onArcCurve2, onArcCurve, onArcCurve4, onArcCurve5, onPolylineCurve };
		OnCurve[] output = null;
		IOnCurve[] input_curves = obj;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output))
		{
			return false;
		}
		if (output.Length != 1)
		{
			return false;
		}
		outputProfile = output[0];
		double t = 0.0;
		outputProfile.GetClosestPoint(new On3dPoint(0.0, edgeHeight, 0.0), ref t);
		outputProfile.ChangeClosedCurveSeam(t);
		return true;
	}

	public static bool Rectangle(double height, double width, ref OnCurve outputProfile)
	{
		double num = width / 2.0;
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(0.0, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num, height, 0.0);
		on3dPointArray.Append(num, height, 0.0);
		on3dPointArray.Append(num, 0.0, 0.0);
		on3dPointArray.Append(0.0, 0.0, 0.0);
		outputProfile = new OnPolylineCurve(on3dPointArray);
		return true;
	}

	public static bool Shell(OnCurve profile, OnXform xUpScaled, double midHeight, double outHeight, double outWidth, double outFilletPercentage, ref OnCurve outputProfile)
	{
		profile.Transform(xUpScaled.Inverse());
		OnBoundingBox profileBB = new OnBoundingBox();
		double height = 0.0;
		double width = 0.0;
		if (!GetProfileDimensions(profile, ref profileBB, ref height, ref width))
		{
			return false;
		}
		if (!Shell(midHeight, width, outHeight, outWidth, outFilletPercentage, ref outputProfile))
		{
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0, 0.0 - midHeight, 0.0);
		outputProfile.Transform(xUpScaled * onXform);
		return true;
	}

	public static bool Shell(double midHeight, double midWidth, double outHeight, double outWidth, double outFilletPercentage, ref OnCurve outputProfile)
	{
		double num = outWidth / 2.0 * (outFilletPercentage / 100.0);
		double num2 = midWidth / 2.0;
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(0.0, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num2 - outWidth, 0.0, 0.0);
		on3dPointArray.Append(0.0 - num2 - outWidth, midHeight + outHeight, 0.0);
		On3dPointArray on3dPointArray2 = new On3dPointArray();
		on3dPointArray2.Append(0.0 - num2, midHeight + outHeight, 0.0);
		on3dPointArray2.Append(0.0 - num2, midHeight, 0.0);
		on3dPointArray2.Append(num2, midHeight, 0.0);
		on3dPointArray2.Append(num2, midHeight + outHeight, 0.0);
		On3dPointArray on3dPointArray3 = new On3dPointArray();
		on3dPointArray3.Append(num2 + outWidth, midHeight + outHeight, 0.0);
		on3dPointArray3.Append(num2 + outWidth, 0.0, 0.0);
		on3dPointArray3.Append(0.0, 0.0, 0.0);
		OnPolylineCurve onPolylineCurve = new OnPolylineCurve(on3dPointArray);
		OnPolylineCurve onPolylineCurve2 = new OnPolylineCurve(on3dPointArray2);
		OnPolylineCurve onPolylineCurve3 = new OnPolylineCurve(on3dPointArray3);
		OnCurve[] array = new OnCurve[5] { onPolylineCurve, null, onPolylineCurve2, null, onPolylineCurve3 };
		if (num < 0.01)
		{
			On3dPointArray on3dPointArray4 = new On3dPointArray();
			on3dPointArray4.Append(0.0 - num2 - outWidth, midHeight + outHeight, 0.0);
			on3dPointArray4.Append(0.0 - num2, midHeight + outHeight, 0.0);
			OnPolylineCurve onPolylineCurve4 = new OnPolylineCurve(on3dPointArray4);
			On3dPointArray on3dPointArray5 = new On3dPointArray();
			on3dPointArray5.Append(num2, midHeight + outHeight, 0.0);
			on3dPointArray5.Append(num2 + outWidth, midHeight + outHeight, 0.0);
			OnPolylineCurve onPolylineCurve5 = new OnPolylineCurve(on3dPointArray5);
			array[1] = onPolylineCurve4;
			array[3] = onPolylineCurve5;
		}
		else
		{
			On3dPointArray on3dPointArray6 = new On3dPointArray();
			on3dPointArray6.Append(0.0 - num2 - outWidth, midHeight + outHeight, 0.0);
			on3dPointArray6.Append(0.0 - num2 - outWidth / 2.0, midHeight + outHeight + num, 0.0);
			on3dPointArray6.Append(0.0 - num2, midHeight + outHeight, 0.0);
			OnArcCurve onArcCurve = new OnArc(on3dPointArray6[0], on3dPointArray6[1], on3dPointArray6[2]);
			On3dPointArray on3dPointArray7 = new On3dPointArray();
			on3dPointArray7.Append(num2, midHeight + outHeight, 0.0);
			on3dPointArray7.Append(num2 + outWidth / 2.0, midHeight + outHeight + num, 0.0);
			on3dPointArray7.Append(num2 + outWidth, midHeight + outHeight, 0.0);
			OnArcCurve onArcCurve2 = new OnArc(on3dPointArray7[0], on3dPointArray7[1], on3dPointArray7[2]);
			array[1] = onArcCurve;
			array[3] = onArcCurve2;
		}
		OnCurve[] output = null;
		IOnCurve[] input_curves = array;
		if (!RhUtil.RhinoMergeCurves(input_curves, ref output, CSUtil.Tolerance / 2.0))
		{
			return false;
		}
		outputProfile = output[0];
		return true;
	}

	private static bool GetProfileDimensions(OnCurve profile, ref OnBoundingBox profileBB, ref double height, ref double width)
	{
		profileBB = new OnBoundingBox();
		if (!profile.GetTightBoundingBox(ref profileBB))
		{
			return false;
		}
		height = Math.Abs(profileBB.Max().y - profileBB.Min().y);
		width = Math.Abs(profileBB.Max().x - profileBB.Min().x);
		return true;
	}
}
