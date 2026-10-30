using System;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlockUtil;

public static class CSUtilSMI
{
	public static bool Cutter2ChannelsFromCenter(OnCurve profile, OnXform xUpScaled, double channelWidth, double distance, double cuttingDepth, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, profileBB.Max().y - cuttingDepth, 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool Cutter2ChannelsFromCenter(double height, double channelWidth, double distance, ref OnCurve outputProfile)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		double num = channelWidth / 2.0;
		double num2 = distance - num;
		if (distance <= num)
		{
			return CutterChannel(height, 2.0 * channelWidth, ref outputProfile);
		}
		On3dPointArray val = new On3dPointArray();
		((ArrayOn3dPoint)val).Append(0.0, height, 0.0);
		((ArrayOn3dPoint)val).Append(0.0 - num2, height, 0.0);
		((ArrayOn3dPoint)val).Append(0.0 - num2, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(0.0 - num2 - channelWidth, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(0.0 - num2 - channelWidth, height + 1.0, 0.0);
		((ArrayOn3dPoint)val).Append(num2 + channelWidth, height + 1.0, 0.0);
		((ArrayOn3dPoint)val).Append(num2 + channelWidth, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(num2, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(num2, height, 0.0);
		((ArrayOn3dPoint)val).Append(0.0, height, 0.0);
		outputProfile = (OnCurve)new OnPolylineCurve((IOn3dPointArray)(object)val);
		return true;
	}

	public static bool Cutter2ChannelsFromEdge(OnCurve profile, OnXform xUpScaled, double channelWidth, double distance, double cuttingDepth, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, profileBB.Max().y - cuttingDepth, 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool Cutter2ChannelsFromEdge(double height, double width, double channelWidth, double distance, ref OnCurve outputProfile)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		double num = width / 2.0;
		double num2 = channelWidth / 2.0;
		double num3 = distance - num2;
		if (2.0 * (num3 + channelWidth) >= width)
		{
			return CutterChannel(height, 2.0 * channelWidth, ref outputProfile);
		}
		On3dPointArray val = new On3dPointArray();
		((ArrayOn3dPoint)val).Append(0.0, height, 0.0);
		((ArrayOn3dPoint)val).Append(0.0 - num + num3 + channelWidth, height, 0.0);
		((ArrayOn3dPoint)val).Append(0.0 - num + num3 + channelWidth, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(0.0 - num + num3, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(0.0 - num + num3, height + 1.0, 0.0);
		((ArrayOn3dPoint)val).Append(num - num3, height + 1.0, 0.0);
		((ArrayOn3dPoint)val).Append(num - num3, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(num - num3 - channelWidth, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(num - num3 - channelWidth, height, 0.0);
		((ArrayOn3dPoint)val).Append(0.0, height, 0.0);
		outputProfile = (OnCurve)new OnPolylineCurve((IOn3dPointArray)(object)val);
		return true;
	}

	public static bool CutterChamfer(OnCurve profile, OnXform xUpScaled, double angleD, double cuttingDepth, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, profileBB.Max().y - cuttingDepth, 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool CutterChamfer(double height, double width, double angleD, ref OnCurve outputProfile)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Expected O, but got Unknown
		if (angleD == 0.0)
		{
			return Rectangle(height + 1.0, width + 2.0, ref outputProfile);
		}
		double num = CSUtilMath.ToRadians(angleD);
		double num2 = width / 2.0;
		On3dPoint point = new On3dPoint(0.0 - num2, 0.0, 0.0);
		On3dPoint point2 = new On3dPoint(num2, 0.0, 0.0);
		On3dVector val = new On3dVector(OnUtil.On_xaxis);
		val.Rotate(num, OnUtil.On_zaxis);
		On3dVector val2 = new On3dVector(OnUtil.On_xaxis) * -1.0;
		val2.Rotate(0.0 - num, OnUtil.On_zaxis);
		On3dPoint outpoint = null;
		if (!CSUtilMath.LineInt(out outpoint, (IOn3dPoint)(object)point, (IOn3dVector)(object)val, (IOn3dPoint)(object)point2, (IOn3dVector)(object)val2))
		{
			return false;
		}
		if (outpoint.y > height)
		{
			height = outpoint.y;
		}
		On3dPointArray val3 = new On3dPointArray();
		((ArrayOn3dPoint)val3).Append(0.0, outpoint.y, 0.0);
		((ArrayOn3dPoint)val3).Append(0.0 - num2, 0.0, 0.0);
		((ArrayOn3dPoint)val3).Append(0.0 - num2 - 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val3).Append(0.0 - num2 - 1.0, height + 1.0, 0.0);
		((ArrayOn3dPoint)val3).Append(num2 + 1.0, height + 1.0, 0.0);
		((ArrayOn3dPoint)val3).Append(num2 + 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val3).Append(num2, 0.0, 0.0);
		((ArrayOn3dPoint)val3).Append(0.0, outpoint.y, 0.0);
		outputProfile = (OnCurve)new OnPolylineCurve((IOn3dPointArray)(object)val3);
		return true;
	}

	public static bool CutterChamferWithFlat(OnCurve profile, OnXform xUpScaled, double angleD, double depth, double cuttingDepth, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, profileBB.Max().y - cuttingDepth, 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool CutterChamferWithFlat(double height, double width, double angleD, double depth, ref OnCurve outputProfile)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Expected O, but got Unknown
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
		On3dPoint val = new On3dPoint(num2, 0.0, 0.0);
		On3dPoint point2 = new On3dPoint(num2, depth, 0.0);
		On3dVector val2 = new On3dVector(OnUtil.On_xaxis);
		val2.Rotate(num, OnUtil.On_zaxis);
		On3dVector val3 = new On3dVector(OnUtil.On_xaxis) * -1.0;
		val3.Rotate(0.0 - num, OnUtil.On_zaxis);
		On3dVector vector = new On3dVector(OnUtil.On_xaxis);
		On3dPoint outpoint = null;
		if (!CSUtilMath.LineInt(out outpoint, (IOn3dPoint)(object)point, (IOn3dVector)(object)val2, (IOn3dPoint)(object)val, (IOn3dVector)(object)val3))
		{
			return false;
		}
		On3dPoint outpoint2 = null;
		if (!CSUtilMath.LineInt(out outpoint2, (IOn3dPoint)(object)point, (IOn3dVector)(object)val2, (IOn3dPoint)(object)point2, (IOn3dVector)(object)vector))
		{
			return false;
		}
		On3dPoint outpoint3 = null;
		if (!CSUtilMath.LineInt(out outpoint3, (IOn3dPoint)(object)val, (IOn3dVector)(object)val3, (IOn3dPoint)(object)point2, (IOn3dVector)(object)vector))
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
		On3dPointArray val4 = new On3dPointArray();
		((ArrayOn3dPoint)val4).Append(0.0, num3, 0.0);
		((ArrayOn3dPoint)val4).Append(outpoint2.x, num3, 0.0);
		((ArrayOn3dPoint)val4).Append(0.0 - num2, 0.0, 0.0);
		((ArrayOn3dPoint)val4).Append(0.0 - num2 - 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val4).Append(0.0 - num2 - 1.0, height + 1.0, 0.0);
		((ArrayOn3dPoint)val4).Append(num2 + 1.0, height + 1.0, 0.0);
		((ArrayOn3dPoint)val4).Append(num2 + 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val4).Append(num2, 0.0, 0.0);
		((ArrayOn3dPoint)val4).Append(outpoint3.x, num3, 0.0);
		((ArrayOn3dPoint)val4).Append(0.0, num3, 0.0);
		outputProfile = (OnCurve)new OnPolylineCurve((IOn3dPointArray)(object)val4);
		return true;
	}

	public static bool CutterChannel(double height, double channelWidth, ref OnCurve outputProfile)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		double num = channelWidth / 2.0;
		On3dPointArray val = new On3dPointArray();
		((ArrayOn3dPoint)val).Append(0.0, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(0.0 - num, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(0.0 - num, height + 1.0, 0.0);
		((ArrayOn3dPoint)val).Append(num, height + 1.0, 0.0);
		((ArrayOn3dPoint)val).Append(num, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(0.0, 0.0, 0.0);
		outputProfile = (OnCurve)new OnPolylineCurve((IOn3dPointArray)(object)val);
		return true;
	}

	public static bool CutterChannelFromCenter(OnCurve profile, OnXform xUpScaled, double width, double cuttingDepth, double distance, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(distance, profileBB.Max().y - cuttingDepth, 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool CutterChannelFromEdge(OnCurve profile, OnXform xUpScaled, double width, double cuttingDepth, double distance, bool flip, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		if (flip)
		{
			val.Translation(width2 / 2.0 - distance, profileBB.Max().y - cuttingDepth, 0.0);
		}
		else
		{
			val.Translation(0.0 - width2 / 2.0 + distance, profileBB.Max().y - cuttingDepth, 0.0);
		}
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool CutterComfortFit(OnCurve profile, OnXform xUpScaled, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, 0.0 - (profileBB.Max().y + 1.0), 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool CutterComfortFit(double height, double width, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_007f: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Expected O, but got Unknown
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		if (edgeHeight == 0.0)
		{
			return Rectangle(height + 1.0, width + 2.0, ref outputProfile);
		}
		double num = width / 2.0;
		On3dPoint val = new On3dPoint(0.0 - num, num, 0.0);
		On3dPoint val2 = new On3dPoint(0.0, 0.0, 0.0);
		On3dPoint val3 = new On3dPoint(num, num, 0.0);
		OnArcCurve obj = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)val, (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3));
		OnXform val4 = new OnXform();
		val4.Scale(1.0, edgeHeight / num, 1.0);
		OnNurbsCurve val5 = new OnNurbsCurve();
		((OnCurve)obj).GetNurbForm(ref val5);
		val5.Transform((IOnXform)(object)val4);
		((OnGeometry)val5).Translate((IOn3dVector)new On3dVector(0.0, height + 1.0, 0.0));
		On3dPointArray val6 = new On3dPointArray();
		((ArrayOn3dPoint)val6).Append(num, edgeHeight + height + 1.0, 0.0);
		((ArrayOn3dPoint)val6).Append(num + 1.0, edgeHeight + height + 1.0, 0.0);
		((ArrayOn3dPoint)val6).Append(num + 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val6).Append(0.0 - num - 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val6).Append(0.0 - num - 1.0, edgeHeight + height + 1.0, 0.0);
		((ArrayOn3dPoint)val6).Append(0.0 - num, edgeHeight + height + 1.0, 0.0);
		OnPolylineCurve val7 = new OnPolylineCurve((IOn3dPointArray)val6);
		OnCurve[] obj2 = new OnCurve[2]
		{
			(OnCurve)val5,
			(OnCurve)val7
		};
		OnCurve[] array = null;
		IOnCurve[] array2 = (IOnCurve[])(object)obj2;
		if (!RhUtil.RhinoMergeCurves(array2, ref array))
		{
			return false;
		}
		if (array.Length != 1)
		{
			return false;
		}
		outputProfile = array[0];
		double num2 = 0.0;
		outputProfile.GetClosestPoint((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), ref num2);
		outputProfile.ChangeClosedCurveSeam(num2);
		return true;
	}

	public static bool CutterHollow(OnCurve profile, OnXform xUpScaled, double width, double depth, double cuttingDepth, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, 0.0 - (profileBB.Max().y + 1.0) + cuttingDepth, 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool CutterHollow(double height, double width, double depth, ref OnCurve outputProfile)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00ad: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		if (depth == 0.0)
		{
			return Rectangle(height + 1.0, width, ref outputProfile);
		}
		double num = width / 2.0;
		if (Math.Abs(depth) > num)
		{
			depth = ((!(depth < 0.0)) ? num : (0.0 - num));
		}
		On3dPoint val = new On3dPoint(0.0 - num, height + 1.0 - depth, 0.0);
		On3dPoint val2 = new On3dPoint(0.0, height + 1.0, 0.0);
		On3dPoint val3 = new On3dPoint(num, height + 1.0 - depth, 0.0);
		OnArcCurve val4 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)val, (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3));
		On3dPointArray val5 = new On3dPointArray();
		((ArrayOn3dPoint)val5).Append(num, height + 1.0 - depth, 0.0);
		((ArrayOn3dPoint)val5).Append(num, 0.0, 0.0);
		((ArrayOn3dPoint)val5).Append(0.0 - num, 0.0, 0.0);
		((ArrayOn3dPoint)val5).Append(0.0 - num, height + 1.0 - depth, 0.0);
		OnPolylineCurve val6 = new OnPolylineCurve((IOn3dPointArray)val5);
		OnCurve[] obj = new OnCurve[2]
		{
			(OnCurve)val4,
			(OnCurve)val6
		};
		OnCurve[] array = null;
		IOnCurve[] array2 = (IOnCurve[])(object)obj;
		if (!RhUtil.RhinoMergeCurves(array2, ref array))
		{
			return false;
		}
		if (array.Length != 1)
		{
			return false;
		}
		outputProfile = array[0];
		double num2 = 0.0;
		outputProfile.GetClosestPoint((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), ref num2);
		outputProfile.ChangeClosedCurveSeam(num2);
		return true;
	}

	public static bool CutterInsideRound(OnCurve profile, OnXform xUpScaled, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, 0.0 - (profileBB.Max().y + 1.0), 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool CutterInsideRound(double height, double width, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0099: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		if (edgeHeight == 0.0)
		{
			return Rectangle(height + 1.0, width + 2.0, ref outputProfile);
		}
		double num = width / 2.0;
		On3dPoint val = new On3dPoint(0.0 - num, edgeHeight + height + 1.0, 0.0);
		On3dPoint val2 = new On3dPoint(0.0, height + 1.0, 0.0);
		On3dPoint val3 = new On3dPoint(num, edgeHeight + height + 1.0, 0.0);
		OnArcCurve val4 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)val, (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3));
		On3dPointArray val5 = new On3dPointArray();
		((ArrayOn3dPoint)val5).Append(num, edgeHeight + height + 1.0, 0.0);
		((ArrayOn3dPoint)val5).Append(num + 1.0, edgeHeight + height + 1.0, 0.0);
		((ArrayOn3dPoint)val5).Append(num + 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val5).Append(0.0 - num - 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val5).Append(0.0 - num - 1.0, edgeHeight + height + 1.0, 0.0);
		((ArrayOn3dPoint)val5).Append(0.0 - num, edgeHeight + height + 1.0, 0.0);
		OnPolylineCurve val6 = new OnPolylineCurve((IOn3dPointArray)val5);
		OnCurve[] obj = new OnCurve[2]
		{
			(OnCurve)val4,
			(OnCurve)val6
		};
		OnCurve[] array = null;
		IOnCurve[] array2 = (IOnCurve[])(object)obj;
		if (!RhUtil.RhinoMergeCurves(array2, ref array))
		{
			return false;
		}
		if (array.Length != 1)
		{
			return false;
		}
		outputProfile = array[0];
		double num2 = 0.0;
		outputProfile.GetClosestPoint((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), ref num2);
		outputProfile.ChangeClosedCurveSeam(num2);
		return true;
	}

	public static bool CutterInsideRoundWithFlat(OnCurve profile, OnXform xUpScaled, double middleWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, 0.0 - (profileBB.Max().y + 1.0), 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool CutterInsideRoundWithFlat(double height, double width, double middleWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_00dc: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_0175: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
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
		On3dPoint val = new On3dPoint(0.0 - num + 2.0 * num3, edgeHeight + height + 1.0, 0.0);
		On3dPoint val2 = new On3dPoint(0.0 - num + num3, height + 1.0, 0.0);
		On3dPoint val3 = new On3dPoint(0.0 - num, edgeHeight + height + 1.0, 0.0);
		OnArcCurve val4 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)val, (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3));
		((OnCurve)val4).Trim((IOnInterval)new OnInterval(((OnCurve)val4).Domain().Mid(), ((OnCurve)val4).Domain().Max()));
		On3dPoint val5 = new On3dPoint(num, edgeHeight + height + 1.0, 0.0);
		On3dPoint val6 = new On3dPoint(num - num3, height + 1.0, 0.0);
		On3dPoint val7 = new On3dPoint(num - 2.0 * num3, edgeHeight + height + 1.0, 0.0);
		OnArcCurve val8 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)val5, (IOn3dPoint)(object)val6, (IOn3dPoint)(object)val7));
		((OnCurve)val8).Trim((IOnInterval)new OnInterval(((OnCurve)val8).Domain().Min(), ((OnCurve)val8).Domain().Mid()));
		OnLineCurve val9 = new OnLineCurve((IOn3dPoint)new On3dPoint(num - num3, height + 1.0, 0.0), (IOn3dPoint)new On3dPoint(0.0 - num + num3, height + 1.0, 0.0));
		On3dPointArray val10 = new On3dPointArray();
		((ArrayOn3dPoint)val10).Append(num, edgeHeight + height + 1.0, 0.0);
		((ArrayOn3dPoint)val10).Append(num + 1.0, edgeHeight + height + 1.0, 0.0);
		((ArrayOn3dPoint)val10).Append(num + 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val10).Append(0.0 - num - 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val10).Append(0.0 - num - 1.0, edgeHeight + height + 1.0, 0.0);
		((ArrayOn3dPoint)val10).Append(0.0 - num, edgeHeight + height + 1.0, 0.0);
		OnPolylineCurve val11 = new OnPolylineCurve((IOn3dPointArray)val10);
		OnCurve[] obj = new OnCurve[4]
		{
			(OnCurve)val4,
			(OnCurve)val9,
			(OnCurve)val8,
			(OnCurve)val11
		};
		OnCurve[] array = null;
		IOnCurve[] array2 = (IOnCurve[])(object)obj;
		if (!RhUtil.RhinoMergeCurves(array2, ref array))
		{
			return false;
		}
		if (array.Length != 1)
		{
			return false;
		}
		outputProfile = array[0];
		double num4 = 0.0;
		outputProfile.GetClosestPoint((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), ref num4);
		outputProfile.ChangeClosedCurveSeam(num4);
		return true;
	}

	public static bool CutterOval(OnCurve profile, OnXform xUpScaled, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, profileBB.Max().y, 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool CutterOval(double height, double width, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0087: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		if (edgeHeight == 0.0)
		{
			return Rectangle(height + 1.0, width + 2.0, ref outputProfile);
		}
		double num = width / 2.0;
		On3dPoint val = new On3dPoint(0.0 - num, 0.0, 0.0);
		On3dPoint val2 = new On3dPoint(0.0, num, 0.0);
		On3dPoint val3 = new On3dPoint(num, 0.0, 0.0);
		OnArcCurve obj = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)val, (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3));
		OnXform val4 = new OnXform();
		val4.Scale(1.0, edgeHeight / num, 1.0);
		OnNurbsCurve val5 = new OnNurbsCurve();
		((OnCurve)obj).GetNurbForm(ref val5);
		val5.Transform((IOnXform)(object)val4);
		((OnGeometry)val5).Translate((IOn3dVector)new On3dVector(0.0, 0.0 - edgeHeight, 0.0));
		On3dPointArray val6 = new On3dPointArray();
		((ArrayOn3dPoint)val6).Append(0.0 - num, 0.0 - edgeHeight, 0.0);
		((ArrayOn3dPoint)val6).Append(0.0 - num - 1.0, 0.0 - edgeHeight, 0.0);
		((ArrayOn3dPoint)val6).Append(0.0 - num - 1.0, height + 1.0, 0.0);
		((ArrayOn3dPoint)val6).Append(num + 1.0, height + 1.0, 0.0);
		((ArrayOn3dPoint)val6).Append(num + 1.0, 0.0 - edgeHeight, 0.0);
		((ArrayOn3dPoint)val6).Append(num, 0.0 - edgeHeight, 0.0);
		OnPolylineCurve val7 = new OnPolylineCurve((IOn3dPointArray)val6);
		OnCurve[] obj2 = new OnCurve[2]
		{
			(OnCurve)val5,
			(OnCurve)val7
		};
		OnCurve[] array = null;
		IOnCurve[] array2 = (IOnCurve[])(object)obj2;
		if (!RhUtil.RhinoMergeCurves(array2, ref array))
		{
			return false;
		}
		if (array.Length != 1)
		{
			return false;
		}
		outputProfile = array[0];
		double num2 = 0.0;
		outputProfile.GetClosestPoint(OnUtil.On_origin, ref num2);
		outputProfile.ChangeClosedCurveSeam(num2);
		return true;
	}

	public static bool CutterTopCenter(OnCurve profile, OnXform xUpScaled, double width, double depth, double cuttingDepth, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, profileBB.Max().y - cuttingDepth, 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool CutterTopCenter(double height, double width, double depth, ref OnCurve outputProfile)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		if (depth == 0.0)
		{
			return Rectangle(height + 1.0, width, ref outputProfile);
		}
		double num = width / 2.0;
		if (Math.Abs(depth) > num)
		{
			depth = ((!(depth < 0.0)) ? num : (0.0 - num));
		}
		On3dPoint val = new On3dPoint(num, depth, 0.0);
		On3dPoint val2 = new On3dPoint(OnUtil.On_origin);
		OnArcCurve val3 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)new On3dPoint(0.0 - num, depth, 0.0), (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val));
		On3dPointArray val4 = new On3dPointArray();
		((ArrayOn3dPoint)val4).Append(0.0 - num, depth, 0.0);
		((ArrayOn3dPoint)val4).Append(0.0 - num, height + 1.0, 0.0);
		((ArrayOn3dPoint)val4).Append(num, height + 1.0, 0.0);
		((ArrayOn3dPoint)val4).Append(num, depth, 0.0);
		OnPolylineCurve val5 = new OnPolylineCurve((IOn3dPointArray)val4);
		OnCurve[] obj = new OnCurve[2]
		{
			(OnCurve)val3,
			(OnCurve)val5
		};
		OnCurve[] array = null;
		IOnCurve[] array2 = (IOnCurve[])(object)obj;
		if (!RhUtil.RhinoMergeCurves(array2, ref array))
		{
			return false;
		}
		if (array.Length != 1)
		{
			return false;
		}
		outputProfile = array[0];
		double num2 = 0.0;
		outputProfile.GetClosestPoint((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), ref num2);
		outputProfile.ChangeClosedCurveSeam(num2);
		return true;
	}

	public static bool CutterTopEdge(OnCurve profile, OnXform xUpScaled, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, profileBB.Max().y, 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool CutterTopEdge(double height, double width, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0081: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		if (edgeHeight == 0.0)
		{
			return Rectangle(height + 1.0, width + 2.0, ref outputProfile);
		}
		double num = width / 2.0;
		On3dPoint val = new On3dPoint(0.0 - num, 0.0 - edgeHeight, 0.0);
		On3dPoint val2 = new On3dPoint(0.0, 0.0, 0.0);
		On3dPoint val3 = new On3dPoint(num, 0.0 - edgeHeight, 0.0);
		OnArcCurve val4 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)val, (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3));
		On3dPointArray val5 = new On3dPointArray();
		((ArrayOn3dPoint)val5).Append(0.0 - num, 0.0 - edgeHeight, 0.0);
		((ArrayOn3dPoint)val5).Append(0.0 - num - 1.0, 0.0 - edgeHeight, 0.0);
		((ArrayOn3dPoint)val5).Append(0.0 - num - 1.0, height + 1.0, 0.0);
		((ArrayOn3dPoint)val5).Append(num + 1.0, height + 1.0, 0.0);
		((ArrayOn3dPoint)val5).Append(num + 1.0, 0.0 - edgeHeight, 0.0);
		((ArrayOn3dPoint)val5).Append(num, 0.0 - edgeHeight, 0.0);
		OnPolylineCurve val6 = new OnPolylineCurve((IOn3dPointArray)val5);
		OnCurve[] obj = new OnCurve[2]
		{
			(OnCurve)val4,
			(OnCurve)val6
		};
		OnCurve[] array = null;
		IOnCurve[] array2 = (IOnCurve[])(object)obj;
		if (!RhUtil.RhinoMergeCurves(array2, ref array))
		{
			return false;
		}
		if (array.Length != 1)
		{
			return false;
		}
		outputProfile = array[0];
		double num2 = 0.0;
		outputProfile.GetClosestPoint(OnUtil.On_origin, ref num2);
		outputProfile.ChangeClosedCurveSeam(num2);
		return true;
	}

	public static bool CutterTopEdgeWithFlat(OnCurve profile, OnXform xUpScaled, double middleWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, profileBB.Max().y, 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool CutterTopEdgeWithFlat(double height, double width, double middleWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00c4: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_0145: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01ab: Expected O, but got Unknown
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Expected O, but got Unknown
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
		On3dPoint val = new On3dPoint(0.0 - num + 2.0 * num3, 0.0 - edgeHeight, 0.0);
		On3dPoint val2 = new On3dPoint(0.0 - num + num3, 0.0, 0.0);
		On3dPoint val3 = new On3dPoint(0.0 - num, 0.0 - edgeHeight, 0.0);
		OnArcCurve val4 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)val, (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3));
		((OnCurve)val4).Trim((IOnInterval)new OnInterval(((OnCurve)val4).Domain().Mid(), ((OnCurve)val4).Domain().Max()));
		On3dPoint val5 = new On3dPoint(num, 0.0 - edgeHeight, 0.0);
		On3dPoint val6 = new On3dPoint(num - num3, 0.0, 0.0);
		On3dPoint val7 = new On3dPoint(num - 2.0 * num3, 0.0 - edgeHeight, 0.0);
		OnArcCurve val8 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)val5, (IOn3dPoint)(object)val6, (IOn3dPoint)(object)val7));
		((OnCurve)val8).Trim((IOnInterval)new OnInterval(((OnCurve)val8).Domain().Min(), ((OnCurve)val8).Domain().Mid()));
		OnLineCurve val9 = new OnLineCurve((IOn3dPoint)new On3dPoint(num - num3, 0.0, 0.0), (IOn3dPoint)new On3dPoint(0.0 - num + num3, 0.0, 0.0));
		On3dPointArray val10 = new On3dPointArray();
		((ArrayOn3dPoint)val10).Append(0.0 - num, 0.0 - edgeHeight, 0.0);
		((ArrayOn3dPoint)val10).Append(0.0 - num - 1.0, 0.0 - edgeHeight, 0.0);
		((ArrayOn3dPoint)val10).Append(0.0 - num - 1.0, height + 1.0, 0.0);
		((ArrayOn3dPoint)val10).Append(num + 1.0, height + 1.0, 0.0);
		((ArrayOn3dPoint)val10).Append(num + 1.0, 0.0 - edgeHeight, 0.0);
		((ArrayOn3dPoint)val10).Append(num, 0.0 - edgeHeight, 0.0);
		OnPolylineCurve val11 = new OnPolylineCurve((IOn3dPointArray)val10);
		OnCurve[] obj = new OnCurve[4]
		{
			(OnCurve)val4,
			(OnCurve)val9,
			(OnCurve)val8,
			(OnCurve)val11
		};
		OnCurve[] array = null;
		IOnCurve[] array2 = (IOnCurve[])(object)obj;
		if (!RhUtil.RhinoMergeCurves(array2, ref array))
		{
			return false;
		}
		if (array.Length != 1)
		{
			return false;
		}
		outputProfile = array[0];
		double num4 = 0.0;
		outputProfile.GetClosestPoint(OnUtil.On_origin, ref num4);
		outputProfile.ChangeClosedCurveSeam(num4);
		return true;
	}

	public static bool CutterTopWave(OnCurve profile, OnXform xUpScaled, double middleWidth, double outsideWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, profileBB.Max().y - edgeHeight, 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool CutterTopWave(double height, double width, double middleWidth, double outsideWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Expected O, but got Unknown
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Expected O, but got Unknown
		//IL_0244: Expected O, but got Unknown
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Expected O, but got Unknown
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Expected O, but got Unknown
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
		On3dPoint val = new On3dPoint(0.0 - num2, 0.0, 0.0);
		On3dPoint val2 = new On3dPoint(0.0, edgeHeight, 0.0);
		On3dPoint val3 = new On3dPoint(num2, 0.0, 0.0);
		OnArcCurve val4 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)(object)val, (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3));
		On3dPoint val5 = new On3dPoint(new OnCircle((IOn3dPoint)(object)val, (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3).Center());
		if (!CSUtilMath.LineInt(out var outpoint, (IOn3dPoint)(object)val, (IOn3dVector)(object)(val - (IOn3dPoint)(object)val5), (IOn3dPoint)new On3dPoint(0.0 - num3 + (0.0 - num2), 0.0, 0.0), OnUtil.On_yaxis))
		{
			return false;
		}
		double num4 = outpoint.DistanceTo((IOn3dPoint)(object)val);
		On3dPoint val6 = new On3dPoint(outpoint.x - Math.Abs(outpoint.x - val.x), val.y, 0.0);
		On3dPoint val7 = new On3dPoint(outpoint.x, outpoint.y - num4, 0.0);
		On3dPoint val8 = new On3dPoint((IOn3dPoint)(object)val);
		OnArcCurve val9 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val7, (IOn3dPoint)(object)val8));
		if (!CSUtilMath.LineInt(out var outpoint2, (IOn3dPoint)(object)val3, (IOn3dVector)(object)(val3 - (IOn3dPoint)(object)val5), (IOn3dPoint)new On3dPoint(num3 + num2, 0.0, 0.0), OnUtil.On_yaxis))
		{
			return false;
		}
		double num5 = outpoint2.DistanceTo((IOn3dPoint)(object)val3);
		On3dPoint val10 = new On3dPoint((IOn3dPoint)(object)val3);
		On3dPoint val11 = new On3dPoint(outpoint2.x, outpoint2.y - num5, 0.0);
		On3dPoint val12 = new On3dPoint(outpoint2.x + Math.Abs(outpoint2.x - val3.x), val3.y, 0.0);
		OnArcCurve val13 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)val10, (IOn3dPoint)(object)val11, (IOn3dPoint)(object)val12));
		On3dPointArray val14 = new On3dPointArray();
		((ArrayOn3dPoint)val14).Append(val6.x, 0.0, 0.0);
		((ArrayOn3dPoint)val14).Append(0.0 - num - 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val14).Append(0.0 - num - 1.0, edgeHeight + height + 1.0, 0.0);
		((ArrayOn3dPoint)val14).Append(num + 1.0, edgeHeight + height + 1.0, 0.0);
		((ArrayOn3dPoint)val14).Append(num + 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val14).Append(val12.x, 0.0, 0.0);
		OnPolylineCurve val15 = new OnPolylineCurve((IOn3dPointArray)val14);
		OnCurve[] obj = new OnCurve[4]
		{
			(OnCurve)val9,
			(OnCurve)val4,
			(OnCurve)val13,
			(OnCurve)val15
		};
		OnCurve[] array = null;
		IOnCurve[] array2 = (IOnCurve[])(object)obj;
		if (!RhUtil.RhinoMergeCurves(array2, ref array))
		{
			return false;
		}
		if (array.Length != 1)
		{
			return false;
		}
		outputProfile = array[0];
		double num6 = 0.0;
		outputProfile.GetClosestPoint((IOn3dPoint)new On3dPoint(0.0, edgeHeight, 0.0), ref num6);
		outputProfile.ChangeClosedCurveSeam(num6);
		return true;
	}

	public static bool CutterTopWave2(OnCurve profile, OnXform xUpScaled, double middleWidth, double insideWidth, double outsideWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, profileBB.Max().y - edgeHeight, 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool CutterTopWave2(double height, double width, double middleWidth, double insideWidth, double outsideWidth, double edgeHeight, ref OnCurve outputProfile)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Expected O, but got Unknown
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Expected O, but got Unknown
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Expected O, but got Unknown
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Expected O, but got Unknown
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Expected O, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Expected O, but got Unknown
		//IL_0314: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Expected O, but got Unknown
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Expected O, but got Unknown
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_03c1: Expected O, but got Unknown
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Expected O, but got Unknown
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Expected O, but got Unknown
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
		On3dPoint val = new On3dPoint(0.0 - num2, 0.0, 0.0);
		On3dPoint val2 = new On3dPoint(0.0, edgeHeight, 0.0);
		On3dPoint val3 = new On3dPoint(num2, 0.0, 0.0);
		OnArcCurve val4 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)(object)val, (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3));
		On3dPoint val5 = new On3dPoint(new OnCircle((IOn3dPoint)(object)val, (IOn3dPoint)(object)val2, (IOn3dPoint)(object)val3).Center());
		if (!CSUtilMath.LineInt(out var outpoint, (IOn3dPoint)(object)val, (IOn3dVector)(object)(val - (IOn3dPoint)(object)val5), (IOn3dPoint)new On3dPoint(0.0 - num3 + (0.0 - num2), 0.0, 0.0), OnUtil.On_yaxis))
		{
			return false;
		}
		double num5 = outpoint.DistanceTo((IOn3dPoint)(object)val);
		On3dPoint val6 = new On3dPoint(outpoint.x - Math.Abs(outpoint.x - val.x), val.y, 0.0);
		On3dPoint val7 = new On3dPoint(outpoint.x, outpoint.y - num5, 0.0);
		On3dPoint val8 = new On3dPoint((IOn3dPoint)(object)val);
		OnArcCurve val9 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val7, (IOn3dPoint)(object)val8));
		if (!CSUtilMath.LineInt(out var outpoint2, (IOn3dPoint)(object)val6, (IOn3dVector)(object)(val6 - (IOn3dPoint)(object)outpoint), (IOn3dPoint)new On3dPoint(0.0 - num4 + (0.0 - insideWidth) + (0.0 - num2), 0.0, 0.0), OnUtil.On_yaxis))
		{
			return false;
		}
		double num6 = outpoint2.DistanceTo((IOn3dPoint)(object)val6);
		On3dPoint val10 = new On3dPoint(outpoint2.x - Math.Abs(outpoint2.x - val6.x), val6.y, 0.0);
		On3dPoint val11 = new On3dPoint(outpoint2.x, outpoint2.y + num6, 0.0);
		On3dPoint val12 = new On3dPoint((IOn3dPoint)(object)val6);
		OnArcCurve val13 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)(object)val10, (IOn3dPoint)(object)val11, (IOn3dPoint)(object)val12));
		if (!CSUtilMath.LineInt(out var outpoint3, (IOn3dPoint)(object)val3, (IOn3dVector)(object)(val3 - (IOn3dPoint)(object)val5), (IOn3dPoint)new On3dPoint(num3 + num2, 0.0, 0.0), OnUtil.On_yaxis))
		{
			return false;
		}
		double num7 = outpoint3.DistanceTo((IOn3dPoint)(object)val3);
		On3dPoint val14 = new On3dPoint((IOn3dPoint)(object)val3);
		On3dPoint val15 = new On3dPoint(outpoint3.x, outpoint3.y - num7, 0.0);
		On3dPoint val16 = new On3dPoint(outpoint3.x + Math.Abs(outpoint3.x - val3.x), val3.y, 0.0);
		OnArcCurve val17 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)val14, (IOn3dPoint)(object)val15, (IOn3dPoint)(object)val16));
		if (!CSUtilMath.LineInt(out var outpoint4, (IOn3dPoint)(object)val16, (IOn3dVector)(object)(val16 - (IOn3dPoint)(object)outpoint3), (IOn3dPoint)new On3dPoint(num4 + insideWidth + num2, 0.0, 0.0), OnUtil.On_yaxis))
		{
			return false;
		}
		double num8 = outpoint4.DistanceTo((IOn3dPoint)(object)val16);
		On3dPoint val18 = new On3dPoint((IOn3dPoint)(object)val16);
		On3dPoint val19 = new On3dPoint(outpoint4.x, outpoint4.y + num8, 0.0);
		On3dPoint val20 = new On3dPoint(outpoint4.x + Math.Abs(outpoint4.x - val16.x), val16.y, 0.0);
		OnArcCurve val21 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)val18, (IOn3dPoint)(object)val19, (IOn3dPoint)(object)val20));
		double num9 = (val11.y + val19.y) / 2.0;
		if (num9 > height)
		{
			height = num9;
		}
		On3dPointArray val22 = new On3dPointArray();
		((ArrayOn3dPoint)val22).Append(val10.x, 0.0, 0.0);
		((ArrayOn3dPoint)val22).Append(0.0 - num - 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val22).Append(0.0 - num - 1.0, edgeHeight + height + 1.0, 0.0);
		((ArrayOn3dPoint)val22).Append(num + 1.0, edgeHeight + height + 1.0, 0.0);
		((ArrayOn3dPoint)val22).Append(num + 1.0, 0.0, 0.0);
		((ArrayOn3dPoint)val22).Append(val20.x, 0.0, 0.0);
		OnPolylineCurve val23 = new OnPolylineCurve((IOn3dPointArray)val22);
		OnCurve[] obj = new OnCurve[6]
		{
			(OnCurve)val13,
			(OnCurve)val9,
			(OnCurve)val4,
			(OnCurve)val17,
			(OnCurve)val21,
			(OnCurve)val23
		};
		OnCurve[] array = null;
		IOnCurve[] array2 = (IOnCurve[])(object)obj;
		if (!RhUtil.RhinoMergeCurves(array2, ref array))
		{
			return false;
		}
		if (array.Length != 1)
		{
			return false;
		}
		outputProfile = array[0];
		double num10 = 0.0;
		outputProfile.GetClosestPoint((IOn3dPoint)new On3dPoint(0.0, edgeHeight, 0.0), ref num10);
		outputProfile.ChangeClosedCurveSeam(num10);
		return true;
	}

	public static bool Rectangle(double height, double width, ref OnCurve outputProfile)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		double num = width / 2.0;
		On3dPointArray val = new On3dPointArray();
		((ArrayOn3dPoint)val).Append(0.0, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(0.0 - num, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(0.0 - num, height, 0.0);
		((ArrayOn3dPoint)val).Append(num, height, 0.0);
		((ArrayOn3dPoint)val).Append(num, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(0.0, 0.0, 0.0);
		outputProfile = (OnCurve)new OnPolylineCurve((IOn3dPointArray)(object)val);
		return true;
	}

	public static bool Shell(OnCurve profile, OnXform xUpScaled, double midHeight, double outHeight, double outWidth, double outFilletPercentage, ref OnCurve outputProfile)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		((OnGeometry)profile).Transform((IOnXform)(object)xUpScaled.Inverse());
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
		OnXform val = new OnXform();
		val.Translation(0.0, 0.0 - midHeight, 0.0);
		((OnGeometry)outputProfile).Transform((IOnXform)(object)(xUpScaled * (IOnXform)(object)val));
		return true;
	}

	public static bool Shell(double midHeight, double midWidth, double outHeight, double outWidth, double outFilletPercentage, ref OnCurve outputProfile)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0139: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		double num = outWidth / 2.0 * (outFilletPercentage / 100.0);
		double num2 = midWidth / 2.0;
		On3dPointArray val = new On3dPointArray();
		((ArrayOn3dPoint)val).Append(0.0, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(0.0 - num2 - outWidth, 0.0, 0.0);
		((ArrayOn3dPoint)val).Append(0.0 - num2 - outWidth, midHeight + outHeight, 0.0);
		On3dPointArray val2 = new On3dPointArray();
		((ArrayOn3dPoint)val2).Append(0.0 - num2, midHeight + outHeight, 0.0);
		((ArrayOn3dPoint)val2).Append(0.0 - num2, midHeight, 0.0);
		((ArrayOn3dPoint)val2).Append(num2, midHeight, 0.0);
		((ArrayOn3dPoint)val2).Append(num2, midHeight + outHeight, 0.0);
		On3dPointArray val3 = new On3dPointArray();
		((ArrayOn3dPoint)val3).Append(num2 + outWidth, midHeight + outHeight, 0.0);
		((ArrayOn3dPoint)val3).Append(num2 + outWidth, 0.0, 0.0);
		((ArrayOn3dPoint)val3).Append(0.0, 0.0, 0.0);
		OnPolylineCurve val4 = new OnPolylineCurve((IOn3dPointArray)(object)val);
		OnPolylineCurve val5 = new OnPolylineCurve((IOn3dPointArray)(object)val2);
		OnPolylineCurve val6 = new OnPolylineCurve((IOn3dPointArray)val3);
		OnCurve[] array = (OnCurve[])(object)new OnCurve[5]
		{
			(OnCurve)val4,
			default(OnCurve),
			(OnCurve)val5,
			default(OnCurve),
			(OnCurve)val6
		};
		if (num < 0.01)
		{
			On3dPointArray val7 = new On3dPointArray();
			((ArrayOn3dPoint)val7).Append(0.0 - num2 - outWidth, midHeight + outHeight, 0.0);
			((ArrayOn3dPoint)val7).Append(0.0 - num2, midHeight + outHeight, 0.0);
			OnPolylineCurve val8 = new OnPolylineCurve((IOn3dPointArray)val7);
			On3dPointArray val9 = new On3dPointArray();
			((ArrayOn3dPoint)val9).Append(num2, midHeight + outHeight, 0.0);
			((ArrayOn3dPoint)val9).Append(num2 + outWidth, midHeight + outHeight, 0.0);
			OnPolylineCurve val10 = new OnPolylineCurve((IOn3dPointArray)val9);
			array[1] = (OnCurve)(object)val8;
			array[3] = (OnCurve)(object)val10;
		}
		else
		{
			On3dPointArray val11 = new On3dPointArray();
			((ArrayOn3dPoint)val11).Append(0.0 - num2 - outWidth, midHeight + outHeight, 0.0);
			((ArrayOn3dPoint)val11).Append(0.0 - num2 - outWidth / 2.0, midHeight + outHeight + num, 0.0);
			((ArrayOn3dPoint)val11).Append(0.0 - num2, midHeight + outHeight, 0.0);
			OnArcCurve val12 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)(object)((ArrayOn3dPoint)val11)[0], (IOn3dPoint)(object)((ArrayOn3dPoint)val11)[1], (IOn3dPoint)(object)((ArrayOn3dPoint)val11)[2]));
			On3dPointArray val13 = new On3dPointArray();
			((ArrayOn3dPoint)val13).Append(num2, midHeight + outHeight, 0.0);
			((ArrayOn3dPoint)val13).Append(num2 + outWidth / 2.0, midHeight + outHeight + num, 0.0);
			((ArrayOn3dPoint)val13).Append(num2 + outWidth, midHeight + outHeight, 0.0);
			OnArcCurve val14 = OnArcCurve.op_Implicit((IOnArc)new OnArc((IOn3dPoint)(object)((ArrayOn3dPoint)val13)[0], (IOn3dPoint)(object)((ArrayOn3dPoint)val13)[1], (IOn3dPoint)(object)((ArrayOn3dPoint)val13)[2]));
			array[1] = (OnCurve)(object)val12;
			array[3] = (OnCurve)(object)val14;
		}
		OnCurve[] array2 = null;
		IOnCurve[] array3 = (IOnCurve[])(object)array;
		if (!RhUtil.RhinoMergeCurves(array3, ref array2, CSUtil.Tolerance / 2.0))
		{
			return false;
		}
		outputProfile = array2[0];
		return true;
	}

	private static bool GetProfileDimensions(OnCurve profile, ref OnBoundingBox profileBB, ref double height, ref double width)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		profileBB = new OnBoundingBox();
		if (!((OnGeometry)profile).GetTightBoundingBox(ref profileBB))
		{
			return false;
		}
		height = Math.Abs(profileBB.Max().y - profileBB.Min().y);
		width = Math.Abs(profileBB.Max().x - profileBB.Min().x);
		return true;
	}
}
