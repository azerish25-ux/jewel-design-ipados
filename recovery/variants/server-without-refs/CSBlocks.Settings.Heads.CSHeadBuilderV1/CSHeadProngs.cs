using System;
using System.Collections.Generic;
using System.Linq;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1;

[CSBlock("HeadProngs", "{00E763B9-11A6-4D75-B41D-524CF3089970}")]
public sealed class CSHeadProngs : CSBlock
{
	public readonly CSInputChannel InGems;

	public readonly CSInputChannel InProngLayout;

	public readonly CSInputChannel InProngRails;

	public readonly CSInputChannel InHeadTop;

	public readonly CSInputChannel InHeadBase;

	public readonly CSInputChannel InEnableVProngs;

	public readonly CSInputChannel InProngVBlendAmount;

	public readonly CSInputChannel InProngTopProfile;

	public readonly CSInputChannel InProngTopX;

	public readonly CSInputChannel InProngTopY;

	public readonly CSInputChannel InProngTopNudge;

	public readonly CSInputChannel InProngVTopAngle;

	public readonly CSInputChannel InEnableBase;

	public readonly CSInputChannel InProngBaseProfile;

	public readonly CSInputChannel InProngBaseX;

	public readonly CSInputChannel InProngBaseY;

	public readonly CSInputChannel InProngBaseNudge;

	public readonly CSInputChannel InProngVBaseAngle;

	public readonly CSInputChannel InProngDomeHeight;

	public readonly CSInputChannel InProngDomeSettingDistance;

	public readonly CSInputChannel InProngDomeSettingAngle;

	public readonly CSInputChannel InBaseStyle;

	public readonly CSInputChannel InBasePointSize;

	public readonly CSInputChannel InEnableModeDoItYourself;

	public readonly CSInputChannel InProngBaseDomeHeight;

	public readonly CSInputChannel InFastMulti;

	public readonly CSOutputChannel OutObjects;

	public readonly CSOutputChannel OutCurves;

	public CSHeadProngs(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InProngLayout = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InProngRails = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InHeadTop = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InHeadBase = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InEnableVProngs = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 125);
		InProngVBlendAmount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProngTopProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProngTopX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 2.0);
		InProngTopY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 2.0);
		InProngTopNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProngVTopAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEnableBase = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProngBaseProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProngBaseX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 2.0);
		InProngBaseY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 2.0);
		InProngBaseNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProngVBaseAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProngDomeHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.5);
		InProngDomeSettingDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProngDomeSettingAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBaseStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 122);
		InBasePointSize = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.1);
		InEnableModeDoItYourself = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProngBaseDomeHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InFastMulti = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutObjects = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		List<CSGemData> gemList = InGems.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Missing Gems");
		}
		bool boolean = InFastMulti.Boolean;
		Curve curve = InProngTopProfile.Curve;
		if (curve == null)
		{
			return UpdateMissing("Prong Top Profile");
		}
		double num = InProngTopX.Double;
		double num2 = InProngTopY.Double;
		double num3 = InProngTopNudge.Double;
		double num4 = InProngVTopAngle.Double;
		double num5 = 0.0 - num2 * num3;
		HeadStyle integer = (HeadStyle)InProngLayout.Integer;
		ValueListItem valueListItem = InEnableVProngs.ValueListItem;
		bool boolean2 = InEnableBase.Boolean;
		bool boolean3 = InEnableModeDoItYourself.Boolean;
		double prongVBlend = InProngVBlendAmount.Double;
		Curve val;
		double prongBaseX;
		double num6;
		double prongVBaseAngle;
		double prongBaseOffset;
		if (boolean2)
		{
			val = InProngBaseProfile.Curve;
			if (val == null)
			{
				return UpdateMissing("Prong Base Profile");
			}
			prongBaseX = InProngBaseX.Double;
			num6 = InProngBaseY.Double;
			double num7 = InProngBaseNudge.Double;
			prongVBaseAngle = InProngVBaseAngle.Double;
			prongBaseOffset = 0.0 - num6 * num7;
		}
		else
		{
			val = curve.DuplicateCurve();
			prongBaseX = num;
			num6 = num2;
			prongVBaseAngle = num4;
			prongBaseOffset = num5;
		}
		double prongDomeHeight = InProngDomeHeight.Double;
		double prongDomeSettingDistance = InProngDomeSettingDistance.Double;
		double prongDomeSettingAngle = InProngDomeSettingAngle.Double;
		double prongBaseDomeHeight = InProngBaseDomeHeight.Double;
		ValueListItem valueListItem2 = InBaseStyle.ValueListItem;
		double basePointSize = InBasePointSize.Double;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		int num8 = gemList.Count;
		if (boolean)
		{
			num8 = 1;
		}
		double paramTop = default(double);
		double paramBase = default(double);
		for (int i = 0; i < num8; i++)
		{
			CSDataArray cSDataArray3 = new CSDataArray();
			CSDataArray cSDataArray4 = new CSDataArray();
			CSGemData cSGemData = gemList[i];
			if (cSGemData == null)
			{
				return UpdateError("gemDatas is invalid");
			}
			if (!cSGemData.GetGemXformRC(out var _, out var xUp))
			{
				continue;
			}
			Curve curve2 = InHeadTop.Curve;
			if (curve2 == null)
			{
				return UpdateError("Head Top Crv is invalid");
			}
			cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)curve2, xUp));
			Curve curve3 = InHeadBase.Curve;
			if (curve3 == null)
			{
				return UpdateError("Head Base Crv is invalid");
			}
			cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)curve3, xUp));
			List<Curve> curveList = InProngRails.CurveList;
			if (curveList == null)
			{
				return UpdateMissing("ProngRailArray input incorrect type");
			}
			List<CSHeadStyles.HeadProngInfo> list = CSHeadStyles.GetHeadProngInfo(integer).ToList();
			if (list.Count <= 0)
			{
				return UpdateError("ProngRailArray is invalid");
			}
			for (int j = 0; j < list.Count; j++)
			{
				CSHeadStyles.HeadProngInfo headProngInfo = list[j];
				Curve val2 = curveList[j];
				if (val2 == null)
				{
					CS.CSUtil.Print("Missing Prong Rail\n");
					continue;
				}
				curve2.ClosestPoint(val2.PointAtStart, ref paramTop);
				curve3.ClosestPoint(val2.PointAtEnd, ref paramBase);
				if ((headProngInfo.Type == CSHeadStyles.EHeadProngType.Straight && valueListItem == ValueListItem.HeadBuilderProngOffsetLookup) || valueListItem == ValueListItem.HeadBuilderProngOffsetForce)
				{
					if (!MakeVProngs(val2, curve2, paramTop, num4, num2, num5, prongVBlend, curve3, paramBase, prongVBaseAngle, num6, prongBaseOffset, valueListItem2, prongBaseDomeHeight, prongDomeHeight, prongDomeSettingDistance, prongDomeSettingAngle, xUp, boolean3, out var outCurves, out var outProng, out var error))
					{
						return UpdateError(error);
					}
					foreach (CSOnGeometryData item in outCurves)
					{
						cSDataArray3.Append(item);
					}
					if (outProng != null)
					{
						cSDataArray4.Append(outProng);
					}
					continue;
				}
				if (!MakeProfileSweepProngs(j, curveList, val2, cSGemData, curve2, curve, paramTop, num, num2, num5, curve3, val, paramBase, prongBaseX, num6, prongBaseOffset, valueListItem2, basePointSize, prongDomeHeight, prongDomeSettingDistance, prongDomeSettingAngle, prongBaseDomeHeight, xUp, boolean3, headProngInfo, out var outCurves2, out var outProngs, out var error2))
				{
					return UpdateError(error2);
				}
				foreach (CSOnGeometryData item2 in outCurves2)
				{
					cSDataArray3.Append(item2);
				}
				foreach (CSOnGeometryData item3 in outProngs)
				{
					cSDataArray4.Append(item3);
				}
			}
			cSDataArray.Append(cSDataArray3);
			cSDataArray2.Append(cSDataArray4);
		}
		OutObjects.SetData(cSDataArray2);
		OutCurves.SetData(cSDataArray);
		return base.Update();
	}

	private static bool ArrangeGeometry(CSHeadStyles.HeadProngInfo headProngInfo, Transform xUp, Vector3d headTopTan, Vector3d headTopNrm, double prongTopX, GeometryBase[] arrGeometry, Brep cutter, ref List<CSOnGeometryData> outputGeometries)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		switch (headProngInfo.Arrangment)
		{
		case CSHeadStyles.EHeadProngArrangment.Single:
		{
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase geometry4 = CutGeometry(array[i], cutter, xUp, xUp);
				outputGeometries.Add(new CSOnGeometryData(geometry4));
			}
			break;
		}
		case CSHeadStyles.EHeadProngArrangment.Double:
		{
			double num3 = headProngInfo.Overlap * prongTopX;
			Transform val4 = Transform.Translation(headTopTan * 1.0 * num3);
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase geometry5 = CutGeometry(array[i], cutter, xUp * val4, xUp);
				outputGeometries.Add(new CSOnGeometryData(geometry5));
			}
			Transform val5 = Transform.Translation(headTopTan * -1.0 * num3);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase geometry6 = CutGeometry(array[i], cutter, xUp * val5, xUp);
				outputGeometries.Add(new CSOnGeometryData(geometry6));
			}
			break;
		}
		case CSHeadStyles.EHeadProngArrangment.Triple:
		{
			double num = headProngInfo.Overlap * prongTopX;
			Transform val = Transform.Translation(headTopTan * 1.0 * num);
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase geometry = CutGeometry(array[i], cutter, xUp * val, xUp);
				outputGeometries.Add(new CSOnGeometryData(geometry));
			}
			Transform val2 = Transform.Translation(headTopTan * -1.0 * num);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase geometry2 = CutGeometry(array[i], cutter, xUp * val2, xUp);
				outputGeometries.Add(new CSOnGeometryData(geometry2));
			}
			double num2 = headProngInfo.Overlap * prongTopX * 2.0 * Math.Sqrt(3.0) / 2.0;
			Transform val3 = Transform.Translation(headTopNrm * num2);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase geometry3 = CutGeometry(array[i], cutter, xUp * val3, xUp);
				outputGeometries.Add(new CSOnGeometryData(geometry3));
			}
			break;
		}
		default:
			return false;
		}
		return true;
	}

	private static bool CreateProngDome(Curve prongProfile, double prongX, double prongY, double angle, double prongDomeHeight, double prongDomeSettingDistance, double prongDomeSettingAngle, double fixScale, ref Brep dome)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Expected O, but got Unknown
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox boundingBox = ((GeometryBase)prongProfile).GetBoundingBox(true);
		Plane worldXY = Plane.WorldXY;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num = prongX / ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		Transform val = Transform.Scale(worldXY, num, prongY / ((Vector3d)(ref diagonal)).Y * fixScale, 0.0);
		Transform val2 = Transform.Translation(0.0, (0.0 - prongY) / 2.0, 0.0);
		Curve val3 = prongProfile.DuplicateCurve();
		((GeometryBase)val3).Transform(val2 * val);
		LineCurve val4 = new LineCurve(new Point3d(0.0, -50.0, 0.0), new Point3d(0.0, 50.0, 0.0));
		CurveIntersections val5 = Intersection.CurveCurve(val3, (Curve)(object)val4, 0.01, 0.01);
		if (val5.Count <= 1)
		{
			return false;
		}
		double parameterA = val5[0].ParameterA;
		double parameterA2 = val5[1].ParameterA;
		Interval domain = val3.Domain;
		if (Math.Abs(parameterA - ((Interval)(ref domain)).Min) > 0.0001)
		{
			domain = val3.Domain;
			if (Math.Abs(parameterA - ((Interval)(ref domain)).Max) > 0.0001)
			{
				Point3d val6 = val3.PointAt(parameterA2);
				val3.ChangeClosedCurveSeam(parameterA);
				val3.ClosestPoint(val6, ref parameterA2);
			}
		}
		Curve[] array = val3.Split(parameterA2);
		Curve val7 = array[0];
		Curve val8 = array[1];
		if (val7 == null || val8 == null)
		{
			return false;
		}
		domain = ((Curve)val4).Domain;
		Vector3d val9 = ((Curve)val4).TangentAt(((Interval)(ref domain)).Mid);
		((Vector3d)(ref val9)).Unitize();
		domain = val7.Domain;
		Vector3d val10 = val7.TangentAt(((Interval)(ref domain)).Mid);
		((Vector3d)(ref val10)).Unitize();
		if (val9 * val10 < 0.0)
		{
			val7.Reverse();
		}
		domain = val8.Domain;
		Vector3d val11 = val8.TangentAt(((Interval)(ref domain)).Mid);
		((Vector3d)(ref val11)).Unitize();
		if (val9 * val11 < 0.0)
		{
			val8.Reverse();
		}
		Vector3d zAxis = Vector3d.ZAxis;
		bool num2 = prongDomeHeight > 0.0;
		if (!num2)
		{
			((Vector3d)(ref zAxis)).Z = 0.0 - ((Vector3d)(ref zAxis)).Z;
			prongDomeHeight = 0.0 - prongDomeHeight;
		}
		((Vector3d)(ref zAxis)).Rotate(0.0 - angle, Vector3d.XAxis);
		((Vector3d)(ref zAxis)).Unitize();
		Point3d val12 = ((BoundingBox)(ref boundingBox)).Center;
		((Point3d)(ref val12)).Transform(val2 * val);
		if (num2)
		{
			val12 += Vector3d.ZAxis * prongDomeHeight;
			val12 += Vector3d.YAxis * prongDomeSettingDistance * fixScale;
		}
		else
		{
			val12 += zAxis * (prongDomeHeight / Math.Cos(angle));
			val12 += Vector3d.YAxis * prongDomeSettingDistance;
		}
		Vector3d val13 = Vector3d.YAxis * -1.0;
		((Vector3d)(ref val13)).Rotate(CSBlockUtil.CSUtilMath.ToRadians(prongDomeSettingAngle), Vector3d.XAxis);
		((Vector3d)(ref val13)).Unitize();
		Vector3d vectorEnd = default(Vector3d);
		((Vector3d)(ref vectorEnd))._002Ector(val13);
		((Vector3d)(ref vectorEnd)).Reverse();
		Curve val14 = (Curve)(object)CSUtilCurves.BlendCurve(val7.PointAtStart, val12, zAxis, val13);
		Curve val15 = (Curve)(object)CSUtilCurves.BlendCurve(val7.PointAtEnd, val12, zAxis, vectorEnd);
		Curve[] array2 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2] { val14, val15 });
		if (array2 == null)
		{
			return false;
		}
		if (array2.Length == 0)
		{
			return false;
		}
		LineCurve val16 = new LineCurve(new Point3d(-50.0, 0.0, 0.0), new Point3d(50.0, 0.0, 0.0));
		CurveIntersections val17 = Intersection.CurveCurve(val7, (Curve)(object)val16, 0.01, 0.01);
		if (val17.Count <= 0)
		{
			return false;
		}
		double parameterA3 = val17[0].ParameterA;
		Curve[] array3 = val7.Split(parameterA3);
		Curve val18 = array3[0];
		Curve val19 = array3[1];
		if (val18 == null || val19 == null)
		{
			return false;
		}
		val19.Reverse();
		CurveIntersections val20 = Intersection.CurveCurve(val8, (Curve)(object)val16, 0.01, 0.01);
		if (!((IEnumerable<IntersectionEvent>)val20).Any())
		{
			return false;
		}
		double parameterA4 = val20[0].ParameterA;
		Curve[] array4 = val8.Split(parameterA4);
		Curve val21 = array4[0];
		Curve val22 = array4[1];
		if (val21 == null || val22 == null)
		{
			return false;
		}
		val22.Reverse();
		double[] array5 = new double[1];
		domain = val18.Domain;
		array5[0] = ((Interval)(ref domain)).Min;
		double[] array6 = new double[1];
		domain = val19.Domain;
		array6[0] = ((Interval)(ref domain)).Min;
		if (!CSUtilSweepRC.Sweep2(val18, val19, array2, array5, array6, out var breps, closed: false, maintainHeight: false))
		{
			return false;
		}
		double[] array7 = new double[1];
		domain = val21.Domain;
		array7[0] = ((Interval)(ref domain)).Min;
		double[] array8 = new double[1];
		domain = val21.Domain;
		array8[0] = ((Interval)(ref domain)).Min;
		if (!CSUtilSweepRC.Sweep2(val21, val22, array2, array7, array8, out var breps2, closed: false, maintainHeight: false))
		{
			return false;
		}
		Brep val23 = CSUtilRhino.JoinAllBreps_BackwardCompatibility(new List<Brep> { breps, breps2 }.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (val23 == null)
		{
			return false;
		}
		Transform val24 = Transform.Translation(0.0, prongY / 2.0, 0.0);
		((GeometryBase)val23).Transform(val24);
		dome = val23;
		return true;
	}

	private static bool CreateProngExtension(double prongExtensionHeight, ref Curve prongRail, ref Curve prongTopProfile)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		Curve obj = prongRail;
		Interval domain = prongRail.Domain;
		Vector3d val = obj.TangentAt(((Interval)(ref domain)).Min);
		((Vector3d)(ref val)).Unitize();
		Point3d pointAtStart = prongRail.PointAtStart;
		Point3d val2 = pointAtStart - val * prongExtensionHeight;
		Transform val3 = Transform.Translation(val2 - pointAtStart);
		((GeometryBase)prongTopProfile).Transform(val3);
		LineCurve val4 = new LineCurve(pointAtStart, val2);
		Curve val5 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2]
		{
			prongRail,
			(Curve)val4
		})[0];
		Vector3d val6 = val5.PointAtStart - val2;
		if (((Vector3d)(ref val6)).Length > 0.01)
		{
			val5.Reverse();
		}
		prongRail = val5;
		return true;
	}

	private static GeometryBase CutGeometry(GeometryBase g, Brep cut, Transform T, Transform xUp)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		Brep val = (Brep)(object)((g is Brep) ? g : null);
		if (val == null || cut == null)
		{
			GeometryBase obj = g.Duplicate();
			obj.Transform(T);
			return obj;
		}
		val = val.DuplicateBrep();
		((GeometryBase)val).Transform(T);
		Brep val2 = cut.DuplicateBrep();
		((GeometryBase)val2).Transform(xUp);
		Brep[] array = Brep.CreateBooleanDifference(val, val2, 0.001);
		if (array == null)
		{
			return (GeometryBase)(object)val.DuplicateBrep();
		}
		return (GeometryBase)(object)array[0];
	}

	private static NurbsSurface LoftDir(Curve c0, Curve c1, Vector3d v0, Vector3d v1)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		Brep val = Brep.CreateFromLoft((IEnumerable<Curve>)new List<Curve> { c0, c1 }, Point3d.Unset, Point3d.Unset, (LoftType)2, false)[0];
		if (val.Faces.Count != 1)
		{
			return null;
		}
		NurbsSurface val2 = ((Surface)val.Faces[0]).ToNurbsSurface();
		if (val2.Points.CountU != 4)
		{
			return null;
		}
		int countV = val2.Points.CountV;
		for (int i = 0; i < countV; i++)
		{
			ControlPoint controlPoint = val2.Points.GetControlPoint(0, i);
			Point3d val3 = ((ControlPoint)(ref controlPoint)).Location;
			controlPoint = val2.Points.GetControlPoint(3, i);
			Point3d location = ((ControlPoint)(ref controlPoint)).Location;
			double num = ((Point3d)(ref val3)).DistanceTo(location);
			val3 += v0 * num / 3.0;
			location += v1 * num / 3.0;
			val2.Points.SetControlPoint(1, i, val3);
			val2.Points.SetControlPoint(2, i, location);
		}
		return val2;
	}

	private static bool MakeProfileSweepProngs(int prongIndex, List<Curve> prongRailArray, Curve prongRail, CSGemData gem, Curve headTop, Curve inProngProfileTop, double paramTop, double prongTopX, double prongTopY, double prongTopOffset, Curve headBase, Curve inProngProfileBase, double paramBase, double prongBaseX, double prongBaseY, double prongBaseOffset, ValueListItem baseStyle, double basePointSize, double prongDomeHeight, double prongDomeSettingDistance, double prongDomeSettingAngle, double prongBaseDomeHeight, Transform xUp, bool enableDoItYourself, CSHeadStyles.HeadProngInfo headProngInfo, out List<CSOnGeometryData> outCurves, out List<CSOnGeometryData> outProngs, out string error)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0963: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_080c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Unknown result type (might be due to invalid IL or missing references)
		//IL_082c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Unknown result type (might be due to invalid IL or missing references)
		//IL_0863: Unknown result type (might be due to invalid IL or missing references)
		//IL_0865: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fe: Expected O, but got Unknown
		//IL_0900: Unknown result type (might be due to invalid IL or missing references)
		//IL_0907: Expected O, but got Unknown
		//IL_0922: Unknown result type (might be due to invalid IL or missing references)
		//IL_0927: Unknown result type (might be due to invalid IL or missing references)
		//IL_092b: Unknown result type (might be due to invalid IL or missing references)
		outCurves = new List<CSOnGeometryData>();
		outProngs = new List<CSOnGeometryData>();
		Point3d pointAtStart = prongRail.PointAtStart;
		Point3d pointAtEnd = prongRail.PointAtEnd;
		Vector3d val = CSBlockUtil.CSUtilMath.AverageTangentAt(headTop, paramTop);
		((Vector3d)(ref val)).Unitize();
		Vector3d headTopNrm = CSBlockUtil.CSUtilMath.AverageNormalAt(Vector3d.ZAxis, val);
		((Vector3d)(ref headTopNrm)).Unitize();
		BoundingBox boundingBox = ((GeometryBase)inProngProfileTop).GetBoundingBox(true);
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double x = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double y = ((Vector3d)(ref diagonal)).Y;
		Vector3d inputVector = new Vector3d(0.0, 0.0, -1.0);
		Curve obj = prongRail;
		Interval domain = prongRail.Domain;
		double num = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(inputVector, obj.TangentAt(((Interval)(ref domain)).Min), val);
		double num2 = 1.0 / Math.Cos(num);
		Transform val2 = Transform.Scale(Plane.WorldXY, prongTopX / x, prongTopY / y * num2, 0.0);
		Transform val3 = Transform.Translation(0.0, prongTopOffset, 0.0);
		Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtStart, val, Vector3d.ZAxis);
		Curve prongTopProfile = inProngProfileTop.DuplicateCurve();
		((GeometryBase)prongTopProfile).Transform(val4 * val3 * val2);
		Vector3d val5;
		if (baseStyle != ValueListItem.HeadBuilderProngHead)
		{
			val5 = val;
		}
		else
		{
			val5 = CSBlockUtil.CSUtilMath.AverageTangentAt(headBase, paramBase);
			((Vector3d)(ref val5)).Unitize();
		}
		BoundingBox boundingBox2 = ((GeometryBase)inProngProfileBase).GetBoundingBox(true);
		diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
		double x2 = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
		double y2 = ((Vector3d)(ref diagonal)).Y;
		Vector3d inputVector2 = new Vector3d(0.0, 0.0, -1.0);
		Curve obj2 = prongRail;
		domain = prongRail.Domain;
		double d = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(inputVector2, obj2.TangentAt(((Interval)(ref domain)).Max), val5);
		double num3 = 1.0 / Math.Cos(d);
		Transform val6 = Transform.Scale(Plane.WorldXY, prongBaseX / x2, prongBaseY / y2 * num3, 0.0);
		if (baseStyle == ValueListItem.HeadBuilderProngHeadCutToPoint)
		{
			prongBaseOffset = 0.0 - prongBaseY * num3 + basePointSize;
		}
		Transform val7 = Transform.Translation(0.0, prongBaseOffset, 0.0);
		Transform val8 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? val : val5, Vector3d.ZAxis);
		Curve prongBaseProfile = inProngProfileBase.DuplicateCurve();
		((GeometryBase)prongBaseProfile).Transform(val8 * val7 * val6);
		Brep dome = null;
		Brep dome2 = null;
		if (CSApplicationHelper.Application.IsCamMode && !enableDoItYourself)
		{
			double gemZMax = gem.GemZMax;
			Point3d pointAtStart2 = prongRail.PointAtStart;
			if (gemZMax > ((Point3d)(ref pointAtStart2)).Z)
			{
				double gemZMax2 = gem.GemZMax;
				pointAtStart2 = prongRail.PointAtStart;
				if (!CreateProngExtension(gemZMax2 - ((Point3d)(ref pointAtStart2)).Z, ref prongRail, ref prongTopProfile))
				{
					error = "Unable to create CAM prong extension.";
					return false;
				}
			}
			if (!CreateProngDome(inProngProfileTop, prongTopX, prongTopY, num, prongDomeHeight, prongDomeSettingDistance, prongDomeSettingAngle, num2, ref dome))
			{
				error = "Unable to create top dome";
				return false;
			}
			pointAtStart = prongRail.PointAtStart;
			val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtStart, val, Vector3d.ZAxis);
			((GeometryBase)dome).Transform(val4 * val3);
		}
		else if (prongDomeHeight <= 0.0)
		{
			if (!CreateProngExtension(0.0, ref prongRail, ref prongTopProfile))
			{
				error = "Unable to create top prong cap.";
				return false;
			}
		}
		else
		{
			if (!CreateProngDome(inProngProfileTop, prongTopX, prongTopY, num, prongDomeHeight, prongDomeSettingDistance, prongDomeSettingAngle, num2, ref dome))
			{
				error = "Unable to create top dome";
				return false;
			}
			((GeometryBase)dome).Transform(val4 * val3);
		}
		if (prongBaseDomeHeight > 0.0)
		{
			if (!ReduceProngRailLength(ref prongRail, ref prongBaseProfile, prongBaseDomeHeight))
			{
				error = "ReduceProngRailLength failed";
				return false;
			}
			headBase.ClosestPoint(prongRail.PointAtEnd, ref paramBase);
			val5 = CSBlockUtil.CSUtilMath.AverageTangentAt(headBase, paramBase);
			((Vector3d)(ref val5)).Unitize();
			Vector3d inputVector3 = new Vector3d(0.0, 0.0, -1.0);
			Curve obj3 = prongRail;
			domain = prongRail.Domain;
			d = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(inputVector3, obj3.TangentAt(((Interval)(ref domain)).Max), val5);
			if (!CreateProngDome(inProngProfileBase, prongBaseX, prongBaseY, d, 0.0 - prongBaseDomeHeight, 0.0, 0.0, num3, ref dome2))
			{
				error = "Unable to create base dome";
				return false;
			}
			pointAtEnd = prongRail.PointAtEnd;
			Transform val9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? val : val5, Vector3d.ZAxis);
			prongBaseProfile = inProngProfileBase.DuplicateCurve();
			((GeometryBase)prongBaseProfile).Transform(val9 * val7 * val6);
			((GeometryBase)dome2).Transform(val9 * val7);
		}
		GeometryBase[] arrGeometry = (GeometryBase[])(object)new GeometryBase[3]
		{
			(GeometryBase)prongRail,
			(GeometryBase)prongTopProfile,
			(GeometryBase)prongBaseProfile
		};
		if (!ArrangeGeometry(headProngInfo, xUp, val, headTopNrm, prongTopX, arrGeometry, null, ref outCurves))
		{
			error = "Invalid head style";
			return false;
		}
		if (enableDoItYourself)
		{
			if (!ArrangeGeometry(arrGeometry: (GeometryBase[])(object)((dome2 == null) ? new GeometryBase[1] { (GeometryBase)dome } : new GeometryBase[2]
			{
				(GeometryBase)dome,
				(GeometryBase)dome2
			}), headProngInfo: headProngInfo, xUp: xUp, headTopTan: val, headTopNrm: headTopNrm, prongTopX: prongTopX, cutter: null, outputGeometries: ref outProngs))
			{
				error = "Invalid head style";
				return false;
			}
		}
		else
		{
			Curve[] profiles = (Curve[])(object)new Curve[2] { prongTopProfile, prongBaseProfile };
			double[] array = new double[2];
			domain = prongRail.Domain;
			array[0] = ((Interval)(ref domain)).Min;
			domain = prongRail.Domain;
			array[1] = ((Interval)(ref domain)).Max;
			double[] parameters = array;
			if (!CSUtilSweepRC.Sweep1(prongRail, profiles, parameters, out var breps, closed: false, globalShapeBlend: false))
			{
				error = "Failed Sweep1";
				return false;
			}
			if (breps == null || breps.Length == 0)
			{
				error = "Failed Sweep1: No results";
				return false;
			}
			List<Brep> list = new List<Brep>();
			if (dome != null)
			{
				list.Add(dome);
			}
			if (dome2 != null)
			{
				list.Add(dome2);
			}
			list.AddRange(breps);
			Brep val10 = CSUtilRhino.JoinAllBreps_BackwardCompatibility(list.ToArray(), 0.01);
			if (val10 == null)
			{
				error = "Failed JoinBreps";
				return false;
			}
			Brep brep;
			if (dome2 == null)
			{
				brep = CSUtilRhino.CapPlanarHoles(val10, 0.01);
				if (brep == null)
				{
					error = "Failed CapPlanarHoles";
					return false;
				}
			}
			else
			{
				brep = val10;
			}
			Brep brep2 = null;
			if (baseStyle == ValueListItem.HeadBuilderProngHeadCutToPoint)
			{
				List<Point3d> list2;
				if (prongRailArray.Count > 2)
				{
					Curve val11 = prongRailArray[(prongIndex + 1) % prongRailArray.Count];
					Curve val12 = prongRailArray[(prongIndex - 1 + prongRailArray.Count) % prongRailArray.Count];
					Vector3d val13 = default(Vector3d);
					((Vector3d)(ref val13))._002Ector(val11.PointAtStart);
					Vector3d val14 = default(Vector3d);
					((Vector3d)(ref val14))._002Ector(prongRail.PointAtStart);
					Vector3d val15 = default(Vector3d);
					((Vector3d)(ref val15))._002Ector(val12.PointAtStart);
					((Vector3d)(ref val13)).Z = 0.0;
					((Vector3d)(ref val14)).Z = 0.0;
					((Vector3d)(ref val15)).Z = 0.0;
					((Vector3d)(ref val13)).Unitize();
					((Vector3d)(ref val14)).Unitize();
					((Vector3d)(ref val15)).Unitize();
					val13 *= 20.0;
					val14 *= 20.0;
					val15 *= 20.0;
					Point3d item = default(Point3d);
					((Point3d)(ref item))._002Ector(val13 + val14);
					Point3d item2 = default(Point3d);
					((Point3d)(ref item2))._002Ector(0.0, 0.0, 0.0);
					Point3d item3 = default(Point3d);
					((Point3d)(ref item3))._002Ector(val15 + val14);
					Point3d item4 = default(Point3d);
					((Point3d)(ref item4))._002Ector(val14 * -2.0);
					((Point3d)(ref item)).Z = 20.0;
					((Point3d)(ref item2)).Z = 20.0;
					((Point3d)(ref item3)).Z = 20.0;
					((Point3d)(ref item4)).Z = 20.0;
					list2 = new List<Point3d> { item, item2, item3, item4, item };
				}
				else
				{
					((Vector3d)(ref diagonal))._002Ector(prongRail.PointAtStart);
					((Vector3d)(ref diagonal)).Z = 0.0;
					Vector3d val16 = diagonal;
					((Vector3d)(ref val16)).Unitize();
					val16 *= 20.0;
					Vector3d val17 = default(Vector3d);
					((Vector3d)(ref val17))._002Ector(val16);
					((Vector3d)(ref val17)).Transform(Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Point3d.Origin));
					Point3d item5 = default(Point3d);
					((Point3d)(ref item5))._002Ector(val17);
					Point3d item6 = default(Point3d);
					((Point3d)(ref item6))._002Ector(0.0, 0.0, 0.0);
					Point3d item7 = default(Point3d);
					((Point3d)(ref item7))._002Ector(-val17);
					Point3d item8 = default(Point3d);
					((Point3d)(ref item8))._002Ector(val16 * -2.0);
					((Point3d)(ref item5)).Z = 20.0;
					((Point3d)(ref item6)).Z = 20.0;
					((Point3d)(ref item7)).Z = 20.0;
					((Point3d)(ref item8)).Z = 20.0;
					list2 = new List<Point3d> { item5, item6, item7, item8, item5 };
				}
				PolylineCurve val18 = new PolylineCurve((IEnumerable<Point3d>)list2);
				PolylineCurve val19 = new PolylineCurve((IEnumerable<Point3d>)list2);
				Transform val20 = Transform.Translation(0.0, 0.0, -40.0);
				((GeometryBase)val19).Transform(val20);
				brep2 = CSUtilRhino.CapPlanarHoles(((Surface)NurbsSurface.CreateRuledSurface((Curve)(object)val18, (Curve)(object)val19)).ToBrep(), 0.001);
				CSUtilGeometry.FixBrepSolidOrientation(ref brep, doFullClean: true);
				CSUtilGeometry.FixBrepSolidOrientation(ref brep2, doFullClean: true);
			}
			if (!ArrangeGeometry(headProngInfo, xUp, val, headTopNrm, prongTopX, (GeometryBase[])(object)new GeometryBase[1] { (GeometryBase)brep }, brep2, ref outProngs))
			{
				error = "Invalid head style";
				return false;
			}
		}
		error = "";
		return true;
	}

	private static Curve MakeProngCurve(Point3d domeStart0, Point3d domeStart1, Vector3d prongDir, double prongDomeHeight, double prongDomeSettingDistance)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = (domeStart0 + domeStart1) / 2.0;
		val += prongDir * prongDomeHeight;
		Vector3d val2 = domeStart0 - domeStart1;
		((Vector3d)(ref val2)).Unitize();
		val -= val2 * prongDomeSettingDistance;
		Vector3d vectorEnd = default(Vector3d);
		((Vector3d)(ref vectorEnd))._002Ector(val2);
		((Vector3d)(ref vectorEnd)).Reverse();
		Curve val3 = (Curve)(object)CSUtilCurves.BlendCurve(domeStart0, val, prongDir, val2);
		Curve val4 = (Curve)(object)CSUtilCurves.BlendCurve(domeStart1, val, prongDir, vectorEnd);
		return (Curve)(object)Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2] { val3, val4 })[0].Rebuild(10, 3, true);
	}

	private static bool MakeVProngDomes(IReadOnlyList<Curve> prongCurves, Vector3d prongDir, double prongDomeHeight, double prongDomeSettingDistance, double prongDomeSettingAngle, Curve trimPart, out Brep vDome)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		Curve val = prongCurves[0];
		Curve val2 = prongCurves[1];
		Curve val3 = prongCurves[2];
		Curve val4 = prongCurves[3];
		vDome = null;
		Interval domain = trimPart.Domain;
		double num = ((Interval)(ref domain)).Min;
		domain = trimPart.Domain;
		double max = ((Interval)(ref domain)).Max;
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		List<Curve> list3 = new List<Curve>();
		Curve val5 = MakeProngCurve(val2.PointAtStart, val2.PointAtEnd, prongDir, prongDomeHeight, prongDomeSettingDistance);
		list3.Add(val5);
		domain = val.Domain;
		list.Add(((Interval)(ref domain)).Min);
		domain = val3.Domain;
		list2.Add(((Interval)(ref domain)).Min);
		double num2 = default(double);
		while (trimPart.GetNextDiscontinuity((Continuity)2, num, max, ref num2))
		{
			Point3d val6 = trimPart.PointAt(num2);
			num = num2;
			double num3 = num2 + 0.001;
			double num4 = num2 - 0.001;
			Vector3d val7 = trimPart.TangentAt(num3);
			Vector3d val8 = trimPart.TangentAt(num4);
			Vector3d val9 = val7 + val8;
			((Vector3d)(ref val9)).Unitize();
			Vector3d val10 = Vector3d.CrossProduct(val9, Vector3d.ZAxis);
			LineCurve val11 = new LineCurve(val6 + 50.0 * val10, val6 - 50.0 * val10);
			CurveIntersections val12 = Intersection.CurveCurve(val, (Curve)(object)val11, 0.01, 0.01);
			CurveIntersections val13 = Intersection.CurveCurve(val3, (Curve)(object)val11, 0.01, 0.01);
			if (val12.Count != 0 && val13.Count != 0)
			{
				Curve item = MakeProngCurve(val12[0].PointA, val13[0].PointA, prongDir, prongDomeHeight, prongDomeSettingDistance);
				list3.Add(item);
				list.Add(val12[0].ParameterA);
				list2.Add(val13[0].ParameterA);
			}
		}
		Curve val14 = MakeProngCurve(val4.PointAtStart, val4.PointAtEnd, prongDir, prongDomeHeight, prongDomeSettingDistance);
		list3.Add(val14);
		domain = val.Domain;
		list.Add(((Interval)(ref domain)).Max);
		domain = val3.Domain;
		list2.Add(((Interval)(ref domain)).Max);
		Vector3d val15 = val5.PointAtEnd - val5.PointAtStart;
		Vector3d val16 = Vector3d.CrossProduct(val5.PointAtNormalizedLength(0.5) - val5.PointAtStart, val15);
		if (((Vector3d)(ref prongDir)).Z < 0.0)
		{
			val16 = -val16;
		}
		Brep val17 = ((Surface)LoftDir(val2, val5, prongDir, val16)).ToBrep();
		val15 = val14.PointAtEnd - val14.PointAtStart;
		val16 = Vector3d.CrossProduct(val14.PointAtNormalizedLength(0.5) - val14.PointAtStart, val15);
		if (((Vector3d)(ref prongDir)).Z < 0.0)
		{
			val16 = -val16;
		}
		Brep val18 = ((Surface)LoftDir(val4, val14, prongDir, -val16)).ToBrep();
		if (list3.Count > 2)
		{
			for (int i = 1; i < list3.Count - 2; i++)
			{
				if (Math.Abs(list[i] - list[i + 1]) < 0.001 || Math.Abs(list2[i] - list2[i + 1]) < 0.001)
				{
					list3.RemoveAt(i);
					list.RemoveAt(i);
					list2.RemoveAt(i);
					i--;
				}
			}
		}
		if (!CSUtilSweepRC.Sweep2(val, val3, list3.ToArray(), list.ToArray(), list2.ToArray(), out var breps, closed: false, maintainHeight: true))
		{
			return false;
		}
		if (breps == null)
		{
			return false;
		}
		Brep[] inputBreps = (Brep[])(object)new Brep[3] { val17, breps, val18 };
		vDome = CSUtilRhino.JoinAllBreps_BackwardCompatibility(inputBreps, 0.01);
		if (vDome == null)
		{
			return false;
		}
		return true;
	}

	private static bool MakeVProngProfile(Curve headProfile, double param, double vProngAngle, double prongY, double prongNudge, double blendAmount, out Curve vProng, out Curve[] prongCurves, out Curve trimPart)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Expected O, but got Unknown
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		vProng = null;
		prongCurves = null;
		trimPart = null;
		if (headProfile == null)
		{
			return false;
		}
		Point3d val = headProfile.PointAt(param);
		LineCurve val2 = new LineCurve(new Point3d(0.0, 0.0, ((Point3d)(ref val)).Z), new Point3d(((Point3d)(ref val)).X * 2.0, ((Point3d)(ref val)).Y * 2.0, ((Point3d)(ref val)).Z));
		LineCurve val3 = new LineCurve(new Point3d(0.0, 0.0, ((Point3d)(ref val)).Z), new Point3d(((Point3d)(ref val)).X * 2.0, ((Point3d)(ref val)).Y * 2.0, ((Point3d)(ref val)).Z));
		Transform val4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(vProngAngle), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)val2).Transform(val4);
		val4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - vProngAngle), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)val3).Transform(val4);
		CurveIntersections val5 = Intersection.CurveCurve(headProfile, (Curve)(object)val2, 0.01, 0.01);
		if (val5.Count != 1)
		{
			return false;
		}
		double parameterA = val5[0].ParameterA;
		val5 = Intersection.CurveCurve(headProfile, (Curve)(object)val3, 0.01, 0.01);
		if (val5.Count != 1)
		{
			return false;
		}
		double parameterA2 = val5[0].ParameterA;
		Interval domain;
		if (parameterA < parameterA2)
		{
			trimPart = headProfile.Trim(new Interval(parameterA, parameterA2));
		}
		else
		{
			domain = headProfile.Domain;
			Curve val6 = headProfile.Trim(new Interval(parameterA, ((Interval)(ref domain)).Max));
			domain = headProfile.Domain;
			Curve val7 = headProfile.Trim(new Interval(((Interval)(ref domain)).Min, parameterA2));
			Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2] { val6, val7 }, 0.01);
			trimPart = array[0];
		}
		Curve crv = CSUtilGeometry.Offset(trimPart, Vector3d.ZAxis, new Point3d(0.0, 0.0, ((Point3d)(ref val)).Z), 0.0 - prongNudge, (CurveOffsetCornerStyle)2);
		Curve crv2 = CSUtilGeometry.Offset(trimPart, Vector3d.ZAxis, new Point3d(0.0, 0.0, ((Point3d)(ref val)).Z), 0.0 - (prongY + prongNudge), (CurveOffsetCornerStyle)2);
		if (crv == null || crv2 == null)
		{
			return false;
		}
		double num = default(double);
		crv2.NormalizedLengthParameter(0.5, ref num);
		Point3d val8 = crv2.PointAt(num);
		Point3d pointAtStart = crv.PointAtStart;
		Point3d pointAtStart2 = crv2.PointAtStart;
		Curve crv3 = (Curve)new LineCurve(pointAtStart, pointAtStart2);
		Point3d pointAtEnd = crv.PointAtEnd;
		Point3d pointAtEnd2 = crv2.PointAtEnd;
		Curve crv4 = (Curve)new LineCurve(pointAtEnd, pointAtEnd2);
		double num2 = blendAmount;
		double num3 = blendAmount;
		double length = crv3.GetLength();
		double length2 = crv.GetLength();
		double length3 = crv2.GetLength();
		if (blendAmount > length * 0.48)
		{
			blendAmount = length * 0.48;
		}
		if (num2 > length2 * 0.48)
		{
			num2 = length2 * 0.48;
		}
		if (num3 > length3 * 0.48)
		{
			num3 = length3 * 0.48;
		}
		if (blendAmount > 0.0)
		{
			CSUtilGeometry.TrimStartOfCrv(ref crv, num2);
			CSUtilGeometry.TrimStartOfCrv(ref crv2, num3);
			CSUtilGeometry.TrimEndOfCrv(ref crv3, blendAmount);
			CSUtilGeometry.TrimStartOfCrv(ref crv3, blendAmount);
			Curve val9 = (Curve)(object)CSUtilCurves.BlendCurve(crv, crv3, doBlendFromStart1: true);
			Curve val10 = (Curve)(object)CSUtilCurves.BlendCurve(crv3, crv2);
			CSUtilGeometry.TrimEndOfCrv(ref crv, num2);
			CSUtilGeometry.TrimEndOfCrv(ref crv2, num3);
			CSUtilGeometry.TrimEndOfCrv(ref crv4, blendAmount);
			CSUtilGeometry.TrimStartOfCrv(ref crv4, blendAmount);
			Curve val11 = (Curve)(object)CSUtilCurves.BlendCurve(crv, crv4);
			Curve val12 = (Curve)(object)CSUtilCurves.BlendCurve(crv4, crv2, doBlendFromStart1: false, doBlendFromStart2: false);
			Curve[] array2 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[3] { val9, crv3, val10 }, 0.01);
			if (array2.Length != 1)
			{
				return false;
			}
			crv3 = array2[0];
			array2 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[3] { val11, crv4, val12 }, 0.01);
			if (array2.Length != 1)
			{
				return false;
			}
			crv4 = array2[0];
		}
		prongCurves = (Curve[])(object)new Curve[4] { crv, crv3, crv2, crv4 };
		Curve[] array3 = Curve.JoinCurves((IEnumerable<Curve>)prongCurves, 0.01);
		if (array3.Length != 1)
		{
			return false;
		}
		Curve val13 = array3[0];
		double num4 = default(double);
		val13.ClosestPoint(val8, ref num4);
		val13.ChangeClosedCurveSeam(num4);
		domain = val13.Domain;
		double min = ((Interval)(ref domain)).Min;
		Vector3d val14 = val13.TangentAt(min);
		Vector3d val15 = default(Vector3d);
		((Vector3d)(ref val15))._002Ector(((Point3d)(ref val8)).X, ((Point3d)(ref val8)).Y, 0.0);
		Vector3d val16 = Vector3d.CrossProduct(val14, val15);
		if (((Vector3d)(ref val16)).Z < 0.0)
		{
			val13.Reverse();
		}
		vProng = val13;
		return true;
	}

	private static bool MakeVProngs(Curve prongRail, Curve headTop, double paramTop, double prongVTopAngle, double prongTopY, double prongTopOffset, double prongVBlend, Curve headBase, double paramBase, double prongVBaseAngle, double prongBaseY, double prongBaseOffset, ValueListItem baseStyle, double prongBaseDomeHeight, double prongDomeHeight, double prongDomeSettingDistance, double prongDomeSettingAngle, Transform xUp, bool enableDoItYourself, out List<CSOnGeometryData> outCurves, out CSOnGeometryData outProng, out string error)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		outCurves = new List<CSOnGeometryData>();
		outProng = null;
		Curve obj = prongRail;
		Interval domain = prongRail.Domain;
		Vector3d prongDir = -obj.TangentAt(((Interval)(ref domain)).Min);
		((Vector3d)(ref prongDir)).Unitize();
		Curve geometry = prongRail;
		if (!MakeVProngProfile(headTop, paramTop, prongVTopAngle, prongTopY, prongTopOffset, prongVBlend, out var vProng, out var prongCurves, out var trimPart))
		{
			error = "Error Making Straight Prong Profile Top";
			return false;
		}
		if (!MakeVProngDomes(prongCurves, prongDir, prongDomeHeight, prongDomeSettingDistance, prongDomeSettingAngle, trimPart, out var vDome))
		{
			error = "Error Making V Prong Domes";
			return false;
		}
		if (!MakeVProngProfile(headBase, paramBase, prongVBaseAngle, prongBaseY, prongBaseOffset, prongVBlend, out var vProng2, out var prongCurves2, out trimPart))
		{
			error = "Error Making Straight Prong Profile Base";
			return false;
		}
		Brep vDome2 = null;
		if (!CSApplicationHelper.Application.IsCamMode && baseStyle == ValueListItem.HeadBuilderProngHead && prongBaseDomeHeight > 0.0)
		{
			Point3d pointAtEnd = prongRail.PointAtEnd;
			if (!ReduceProngRailLength(ref prongRail, ref vProng2, prongBaseDomeHeight))
			{
				error = "ReduceProngRailLength failed";
				return false;
			}
			Point3d pointAtEnd2 = prongRail.PointAtEnd;
			Curve obj2 = prongRail;
			domain = prongRail.Domain;
			Vector3d prongDir2 = obj2.TangentAt(((Interval)(ref domain)).Max);
			if (!MakeVProngDomes(prongCurves2, prongDir2, prongBaseDomeHeight, 0.0, 0.0, trimPart, out vDome2))
			{
				error = "Error Making V Prong Domes";
				return false;
			}
			Transform val = Transform.Translation(pointAtEnd2 - pointAtEnd);
			((GeometryBase)vDome2).Transform(val);
		}
		outCurves.Add(new CSOnGeometryData((GeometryBase)(object)prongRail, xUp));
		outCurves.Add(new CSOnGeometryData((GeometryBase)(object)geometry, xUp));
		outCurves.Add(new CSOnGeometryData((GeometryBase)(object)vProng, xUp));
		outCurves.Add(new CSOnGeometryData((GeometryBase)(object)vProng2, xUp));
		if (enableDoItYourself)
		{
			error = "";
			return true;
		}
		double num = default(double);
		vProng.ClosestPoint(prongRail.PointAtStart, ref num);
		vProng.ChangeClosedCurveSeam(num);
		vProng2.ClosestPoint(prongRail.PointAtEnd, ref num);
		vProng2.ChangeClosedCurveSeam(num);
		Curve[] profiles = (Curve[])(object)new Curve[2] { vProng, vProng2 };
		double[] array = new double[2];
		domain = prongRail.Domain;
		array[0] = ((Interval)(ref domain)).Min;
		domain = prongRail.Domain;
		array[1] = ((Interval)(ref domain)).Max;
		double[] parameters = array;
		if (!CSUtilSweepRC.Sweep1(prongRail, profiles, parameters, out var breps, closed: false, globalShapeBlend: true))
		{
			error = "Failed Sweep1";
			return false;
		}
		if (breps == null || breps.Length == 0)
		{
			error = "Failed Sweep1: No results";
			return false;
		}
		Brep brep = breps[0];
		CSUtilGeometry.FixBrepSolidOrientation(ref brep);
		Brep val2 = CSUtilRhino.JoinAllBreps_BackwardCompatibility((Brep[])(object)((vDome2 != null) ? new Brep[3] { brep, vDome, vDome2 } : new Brep[2] { brep, vDome }), 0.01);
		Brep brep2;
		if (vDome2 != null)
		{
			brep2 = val2;
		}
		else
		{
			brep2 = CSUtilRhino.CapPlanarHoles(val2, 0.01);
			if (brep2 == null)
			{
				error = "Failed CapPlanarHoles";
				return false;
			}
		}
		CSUtilGeometry.FixBrepSolidOrientation(ref brep2);
		((GeometryBase)brep2).Transform(xUp);
		outProng = new CSOnGeometryData((GeometryBase)(object)brep2, xUp, applyTransform: false);
		error = "";
		return true;
	}

	private static bool ReduceProngRailLength(ref Curve prongRail, ref Curve prongBaseProfile, double trimLength)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Curve obj = prongRail;
		Interval domain = prongRail.Domain;
		Curve[] array = obj.Split(((Interval)(ref domain)).Max - trimLength);
		prongRail = array[0];
		Curve val = array[1];
		Vector3d val2 = default(Vector3d);
		((Vector3d)(ref val2))._002Ector(val.PointAtStart - val.PointAtEnd);
		((GeometryBase)prongBaseProfile).Translate(val2);
		return true;
	}
}
