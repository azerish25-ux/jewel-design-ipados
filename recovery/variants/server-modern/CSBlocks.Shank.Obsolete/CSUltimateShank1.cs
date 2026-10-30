using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Shank.Obsolete;

[Obsolete]
[CSBlock("UltimateShank_V1", "{D30454B0-9674-4238-9525-3EA79077249D}")]
public sealed class CSUltimateShank1 : CSBlock
{
	private sealed class GemInfoUltimateShank
	{
		public double SizeX;

		public double SizeY;

		public double SizeZ;

		public double NormParam;

		public double Param;

		public double BaseParam;

		public Transform ProfileTransform;

		public Curve ProfileCurve;

		public double ParamLeft;

		public double ParamRight;
	}

	private readonly Brep _srf;

	private Curve _crv;

	private Curve _baseCrv;

	private ValueListItem _vOrientationMethod = ValueListItem.GemOrientationMethodAxisY;

	private double _yStart;

	private double _yEnd;

	private double _positionStartInput;

	private double _positionMiddle;

	private double _positionEndInput;

	private bool _doMirror;

	private Curve _profile;

	private double _profileWidth;

	private bool _channelEnabled;

	private double _channelWallDrop;

	private double _channelWallGemOverlap;

	private double _channelSlopeLength;

	private double _channelBlend;

	private double _channelEndPulldownDistance;

	private bool _gemsClosedLoop;

	private double _direction;

	public CSInputChannel InCurve { get; }

	public CSInputChannel InBaseCurve { get; }

	public CSInputChannel InOrientationMethod { get; }

	public CSInputChannel InDoMirror { get; }

	public CSInputChannel InEnableDiameterMiddle { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InProfileWidth { get; }

	public CSInputChannel InChannelWallDrop { get; }

	public CSInputChannel InChannelWallGemOverlap { get; }

	public CSInputChannel InChannelBlend { get; }

	public CSInputChannel InChannelPullDown { get; }

	public CSInputChannel InUltimateShankInput { get; }

	public CSOutputChannel OutSurfaceOutput { get; }

	public CSOutputChannel OutProfileStart { get; }

	public CSOutputChannel OutProfileEnd { get; }

	public CSUltimateShank1(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(GetPropertyName(() => InCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InBaseCurve = new CSInputChannel(GetPropertyName(() => InBaseCurve), this, ChannelDataType.Curve, ChannelArrayType.Any);
		InOrientationMethod = new CSInputChannel(GetPropertyName(() => InOrientationMethod), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InDoMirror = new CSInputChannel(GetPropertyName(() => InDoMirror), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InEnableDiameterMiddle = new CSInputChannel(GetPropertyName(() => InEnableDiameterMiddle), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(GetPropertyName(() => InProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfileWidth = new CSInputChannel(GetPropertyName(() => InProfileWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallDrop = new CSInputChannel(GetPropertyName(() => InChannelWallDrop), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallGemOverlap = new CSInputChannel(GetPropertyName(() => InChannelWallGemOverlap), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelBlend = new CSInputChannel(GetPropertyName(() => InChannelBlend), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelPullDown = new CSInputChannel(GetPropertyName(() => InChannelPullDown), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InUltimateShankInput = new CSInputChannel(GetPropertyName(() => InUltimateShankInput), this, ChannelDataType.Any, ChannelArrayType.Individual);
		OutSurfaceOutput = new CSOutputChannel(GetPropertyName(() => OutSurfaceOutput), this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		OutProfileStart = new CSOutputChannel(GetPropertyName(() => OutProfileStart), this, ChannelDataType.Profile, ChannelArrayType.Individual);
		OutProfileEnd = new CSOutputChannel(GetPropertyName(() => OutProfileEnd), this, ChannelDataType.Profile, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		if (!CSDataHelper.GetCurve(InCurve, out _crv))
		{
			return UpdateMissing("Curve");
		}
		CSDataHelper.GetCurve(InBaseCurve, out _baseCrv);
		_vOrientationMethod = ValueListItem.GemOrientationMethodAxisY;
		InOrientationMethod.GetValueListItem(ref _vOrientationMethod);
		if (_srf == null && _vOrientationMethod == ValueListItem.GemOrientationMethodSurface)
		{
			_vOrientationMethod = ValueListItem.GemOrientationMethodAxisY;
		}
		if (_baseCrv == null && _vOrientationMethod == ValueListItem.GemOrientationMethodCurve)
		{
			_vOrientationMethod = ValueListItem.GemOrientationMethodAxisY;
		}
		_doMirror = false;
		InDoMirror.GetBoolean(ref _doMirror);
		bool value = false;
		InEnableDiameterMiddle.GetBoolean(ref value);
		CSDataHelper.GetCurve(InProfile, out _profile);
		if (_profile != null)
		{
			BoundingBox boundingBox = ((GeometryBase)_profile).GetBoundingBox(true);
			Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double x = ((Vector3d)(ref diagonal)).X;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double y = ((Vector3d)(ref diagonal)).Y;
			Point3d val = ((BoundingBox)(ref boundingBox)).Max;
			double x2 = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Min;
			double num = (0.0 - (x2 + ((Point3d)(ref val)).X)) / 2.0;
			val = ((BoundingBox)(ref boundingBox)).Min;
			Transform val2 = Transform.Translation(num, 0.0 - ((Point3d)(ref val)).Y, 0.0);
			((GeometryBase)_profile).Transform(val2);
			Transform val3 = Transform.Scale(Plane.WorldXY, 1.0 / x, 1.0 / y, 1.0);
			((GeometryBase)_profile).Transform(val3);
		}
		InProfileWidth.GetDouble(ref _profileWidth);
		InChannelWallDrop.GetDouble(ref _channelWallDrop);
		InChannelWallGemOverlap.GetDouble(ref _channelWallGemOverlap);
		InChannelBlend.GetDouble(ref _channelBlend);
		_channelEndPulldownDistance = 0.1;
		InChannelPullDown.GetDouble(ref _channelEndPulldownDistance);
		if (!(InUltimateShankInput.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Ultimate GemData");
		}
		double value2 = 0.0;
		List<GemInfoUltimateShank> list = new List<GemInfoUltimateShank>();
		if (!(cSDataArray.GetArrayItem(0) is CSDataArray cSDataArray2))
		{
			return UpdateMissing("Ultimate GemData Array");
		}
		for (int i = 0; i < cSDataArray2.GetArrayCount(); i++)
		{
			if (!(cSDataArray2.GetArrayItem(i) is CSDataArray cSDataArray3))
			{
				return UpdateMissing("Ultimate GemData GemArray");
			}
			GemInfoUltimateShank gemInfoUltimateShank = new GemInfoUltimateShank();
			cSDataArray3.GetArrayItem(0).GetDouble(ref gemInfoUltimateShank.SizeX);
			cSDataArray3.GetArrayItem(1).GetDouble(ref gemInfoUltimateShank.SizeY);
			cSDataArray3.GetArrayItem(2).GetDouble(ref gemInfoUltimateShank.SizeZ);
			cSDataArray3.GetArrayItem(3).GetDouble(ref gemInfoUltimateShank.NormParam);
			cSDataArray3.GetArrayItem(4).GetDouble(ref gemInfoUltimateShank.Param);
			cSDataArray3.GetArrayItem(5).GetDouble(ref gemInfoUltimateShank.BaseParam);
			((CSXformData)cSDataArray3.GetArrayItem(6)).GetTransform(out gemInfoUltimateShank.ProfileTransform);
			gemInfoUltimateShank.ProfileCurve = (cSDataArray3.GetArrayItem(7) as ICSCurve)?.GetCurve();
			cSDataArray3.GetArrayItem(8).GetDouble(ref gemInfoUltimateShank.ParamLeft);
			cSDataArray3.GetArrayItem(9).GetDouble(ref gemInfoUltimateShank.ParamRight);
			list.Add(gemInfoUltimateShank);
		}
		cSDataArray.GetArrayItem(1).GetDouble(ref _yStart);
		cSDataArray.GetArrayItem(2).GetDouble(ref _yEnd);
		cSDataArray.GetArrayItem(3).GetDouble(ref _positionStartInput);
		cSDataArray.GetArrayItem(4).GetDouble(ref _positionMiddle);
		cSDataArray.GetArrayItem(5).GetDouble(ref _positionEndInput);
		cSDataArray.GetArrayItem(6).GetDouble(ref value2);
		_channelEnabled = value2 > 0.5;
		cSDataArray.GetArrayItem(7).GetDouble(ref _channelSlopeLength);
		cSDataArray.GetArrayItem(8).GetDouble(ref value2);
		_gemsClosedLoop = value2 > 0.5;
		cSDataArray.GetArrayItem(9).GetDouble(ref _direction);
		double length = _crv.GetLength();
		OutSurfaceOutput.SetData(BuildChannel(list, length, value, out var profileStart, out var profileEnd));
		OutProfileStart.SetData(profileStart);
		OutProfileEnd.SetData(profileEnd);
		return base.Update();
	}

	private CSData BuildChannel(List<GemInfoUltimateShank> gemList, double crvLength, bool vEnableDiameterMiddle, out CSData profileStart, out CSData profileEnd)
	{
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c8: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Expected O, but got Unknown
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Unknown result type (might be due to invalid IL or missing references)
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0911: Unknown result type (might be due to invalid IL or missing references)
		profileStart = new CSData();
		profileEnd = new CSData();
		if (!_channelEnabled || _profile == null)
		{
			return new CSData();
		}
		List<Curve> list = new List<Curve>();
		List<Curve> list2 = new List<Curve>();
		List<Curve> list3 = new List<Curve>();
		List<Curve> list4 = new List<Curve>();
		List<double> list5 = new List<double>();
		List<double> list6 = new List<double>();
		Interval domain;
		if (!_gemsClosedLoop)
		{
			GemInfoUltimateShank g;
			if (gemList.Count > 0)
			{
				g = new GemInfoUltimateShank
				{
					ParamLeft = gemList[0].ParamLeft,
					ParamRight = gemList[0].ParamRight,
					NormParam = gemList[0].NormParam - (gemList[0].SizeX / 2.0 + _channelSlopeLength) / crvLength * _direction,
					SizeY = (_doMirror ? _yEnd : _yStart)
				};
			}
			else
			{
				GemInfoUltimateShank gemInfoUltimateShank = new GemInfoUltimateShank();
				domain = _profile.Domain;
				double min = ((Interval)(ref domain)).Min;
				domain = _profile.Domain;
				gemInfoUltimateShank.ParamLeft = min + ((Interval)(ref domain)).Length * 0.4;
				domain = _profile.Domain;
				double min2 = ((Interval)(ref domain)).Min;
				domain = _profile.Domain;
				gemInfoUltimateShank.ParamRight = min2 + ((Interval)(ref domain)).Length * 0.6;
				gemInfoUltimateShank.NormParam = (_doMirror ? (_positionStartInput - (_positionEndInput - _positionStartInput) * _direction) : _positionStartInput);
				gemInfoUltimateShank.SizeY = (_doMirror ? _yEnd : _yStart);
				g = gemInfoUltimateShank;
			}
			MakeEndProfileParts(g, list, list2, list3, list4, list5, list6, out var profileOut);
			profileStart = CSOnGeometryData.FromCurve(profileOut);
		}
		if (gemList.Count > 0)
		{
			MakeGemProfileParts(gemList[0], list, list2, list3, list4, list5, list6);
		}
		if (gemList.Count > 2)
		{
			if (_doMirror)
			{
				GemInfoUltimateShank g2 = gemList[(int)((double)gemList.Count * 0.5)];
				MakeGemProfileParts(g2, list, list2, list3, list4, list5, list6);
			}
			else if (vEnableDiameterMiddle)
			{
				GemInfoUltimateShank g3 = gemList[(int)Math.Round((double)gemList.Count * _positionMiddle)];
				MakeGemProfileParts(g3, list, list2, list3, list4, list5, list6);
			}
		}
		if (gemList.Count > 1)
		{
			MakeGemProfileParts(gemList[gemList.Count - 1], list, list2, list3, list4, list5, list6);
		}
		if (!_gemsClosedLoop)
		{
			GemInfoUltimateShank g4;
			if (gemList.Count > 0)
			{
				g4 = new GemInfoUltimateShank
				{
					ParamLeft = gemList[gemList.Count - 1].ParamLeft,
					ParamRight = gemList[gemList.Count - 1].ParamRight,
					NormParam = gemList[gemList.Count - 1].NormParam + (gemList[gemList.Count - 1].SizeX / 2.0 + _channelSlopeLength) / crvLength * _direction,
					SizeY = _yEnd
				};
			}
			else
			{
				GemInfoUltimateShank gemInfoUltimateShank2 = new GemInfoUltimateShank();
				domain = _profile.Domain;
				double min3 = ((Interval)(ref domain)).Min;
				domain = _profile.Domain;
				gemInfoUltimateShank2.ParamLeft = min3 + ((Interval)(ref domain)).Length * 0.4;
				domain = _profile.Domain;
				double min4 = ((Interval)(ref domain)).Min;
				domain = _profile.Domain;
				gemInfoUltimateShank2.ParamRight = min4 + ((Interval)(ref domain)).Length * 0.6;
				gemInfoUltimateShank2.NormParam = _positionEndInput;
				gemInfoUltimateShank2.SizeY = _yEnd;
				g4 = gemInfoUltimateShank2;
			}
			MakeEndProfileParts(g4, list, list2, list3, list4, list5, list6, out var profileOut2);
			profileEnd = CSOnGeometryData.FromCurve(profileOut2);
		}
		if (_direction < 0.0)
		{
			list.Reverse();
			list2.Reverse();
			list3.Reverse();
			list4.Reverse();
			list5.Reverse();
			list6.Reverse();
		}
		Brep breps;
		Brep breps2;
		Brep breps3;
		Brep breps4;
		if (_gemsClosedLoop)
		{
			if (!CSUtilSweepRC.Sweep2(_crv, _baseCrv, list, list5, list6, out breps, true, true) || breps == null)
			{
				return new CSData();
			}
			if (!CSUtilSweepRC.Sweep2(_crv, _baseCrv, list2, list5, list6, out breps2, true, true) || breps2 == null)
			{
				return new CSData();
			}
			if (!CSUtilSweepRC.Sweep2(_crv, _baseCrv, list4, list5, list6, out breps3, true, true) || breps3 == null)
			{
				return new CSData();
			}
			if (!CSUtilSweepRC.Sweep2(_crv, _baseCrv, list3, list5, list6, out breps4, true, true) || breps4 == null)
			{
				return new CSData();
			}
			Brep[] array = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep> { breps, breps2, breps4, breps3 }, CSApplicationHelper.Application.Tolerance);
			if (array.Length == 1)
			{
				return CSOnGeometryData.FromBrep(array[0]);
			}
			return new CSData();
		}
		if (!CSUtilSweepRC.Sweep2(_crv, _baseCrv, list, list5, list6, out breps, false, true) || breps == null)
		{
			return new CSData();
		}
		if (!CSUtilSweepRC.Sweep2(_crv, _baseCrv, list2, list5, list6, out breps2, false, true) || breps2 == null)
		{
			return new CSData();
		}
		if (!CSUtilSweepRC.Sweep2(_crv, _baseCrv, list4, list5, list6, out breps3, false, true) || breps3 == null)
		{
			return new CSData();
		}
		if (list.Count == 2)
		{
			if (!CSUtilSweepRC.Sweep2(_crv, _baseCrv, list3, list5, list6, out breps4, false, true) || breps4 == null)
			{
				return new CSData();
			}
			Brep[] array2 = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep> { breps, breps2, breps4, breps3 }, CSApplicationHelper.Application.Tolerance);
			if (array2.Length == 1)
			{
				return CSOnGeometryData.FromBrep(array2[0]);
			}
			return new CSData();
		}
		Curve curveFromBrepEdges = CSUtilBrepCrvs.GetCurveFromBrepEdges(breps2, list2[0].PointAtEnd, list2[list2.Count - 1].PointAtEnd, list2[1].PointAtEnd);
		if (curveFromBrepEdges == null)
		{
			return new CSData();
		}
		Curve curveFromBrepEdges2 = CSUtilBrepCrvs.GetCurveFromBrepEdges(breps3, list4[0].PointAtStart, list4[list4.Count - 1].PointAtStart, list4[1].PointAtStart);
		if (curveFromBrepEdges2 == null)
		{
			return new CSData();
		}
		double length = curveFromBrepEdges.GetLength();
		double length2 = curveFromBrepEdges2.GetLength();
		double num = default(double);
		curveFromBrepEdges.NormalizedLengthParameter(_channelEndPulldownDistance / length, ref num);
		double num2 = default(double);
		curveFromBrepEdges2.NormalizedLengthParameter(_channelEndPulldownDistance / length2, ref num2);
		Point3d val = curveFromBrepEdges.PointAt(num);
		Point3d val2 = curveFromBrepEdges2.PointAt(num2);
		Curve item = (Curve)new LineCurve(val, val2);
		double num3 = default(double);
		curveFromBrepEdges.NormalizedLengthParameter((length - _channelEndPulldownDistance) / length, ref num3);
		double num4 = default(double);
		curveFromBrepEdges2.NormalizedLengthParameter((length2 - _channelEndPulldownDistance) / length2, ref num4);
		Point3d val3 = curveFromBrepEdges.PointAt(num3);
		Point3d val4 = curveFromBrepEdges2.PointAt(num4);
		Curve item2 = (Curve)new LineCurve(val3, val4);
		List<Curve> list7 = new List<Curve>();
		list5.Clear();
		list6.Clear();
		list7.Add(list3[0]);
		double item3 = default(double);
		curveFromBrepEdges.ClosestPoint(list3[0].PointAtStart, ref item3, CSApplicationHelper.Application.Tolerance);
		list5.Add(item3);
		curveFromBrepEdges2.ClosestPoint(list3[0].PointAtEnd, ref item3, CSApplicationHelper.Application.Tolerance);
		list6.Add(item3);
		if (_channelEndPulldownDistance > 0.01)
		{
			list7.Add(item);
			list5.Add(num);
			list6.Add(num2);
		}
		list7.Add(list3[1]);
		curveFromBrepEdges.ClosestPoint(list3[1].PointAtStart, ref item3, CSApplicationHelper.Application.Tolerance);
		list5.Add(item3);
		curveFromBrepEdges2.ClosestPoint(list3[1].PointAtEnd, ref item3, CSApplicationHelper.Application.Tolerance);
		list6.Add(item3);
		if (list3.Count > 3)
		{
			list7.Add(list3[list3.Count - 2]);
			curveFromBrepEdges.ClosestPoint(list3[list3.Count - 2].PointAtStart, ref item3, CSApplicationHelper.Application.Tolerance);
			list5.Add(item3);
			curveFromBrepEdges2.ClosestPoint(list3[list3.Count - 2].PointAtEnd, ref item3, CSApplicationHelper.Application.Tolerance);
			list6.Add(item3);
		}
		if (_channelEndPulldownDistance > 0.01)
		{
			list7.Add(item2);
			list5.Add(num3);
			list6.Add(num4);
		}
		list7.Add(list3[list3.Count - 1]);
		curveFromBrepEdges.ClosestPoint(list3[list3.Count - 1].PointAtStart, ref item3, CSApplicationHelper.Application.Tolerance);
		list5.Add(item3);
		curveFromBrepEdges2.ClosestPoint(list3[list3.Count - 1].PointAtEnd, ref item3, CSApplicationHelper.Application.Tolerance);
		list6.Add(item3);
		if (!CSUtilSweepRC.Sweep2(curveFromBrepEdges, curveFromBrepEdges2, list7, list5, list6, out breps4, false, true) || breps4 == null)
		{
			return new CSData();
		}
		Brep[] array3 = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep> { breps, breps2, breps4, breps3 }, CSApplicationHelper.Application.Tolerance);
		if (array3.Length == 1)
		{
			return CSOnGeometryData.FromBrep(array3[0]);
		}
		return new CSData();
	}

	private void CalculatePointsOnCrv(GemInfoUltimateShank gem)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		double num;
		for (num = gem.NormParam; num > 1.0; num -= 1.0)
		{
		}
		for (; num < 0.0; num += 1.0)
		{
		}
		_crv.NormalizedLengthParameter(num, ref gem.Param);
		Point3d val = _crv.PointAt(gem.Param);
		Vector3d val2 = _crv.TangentAt(gem.Param);
		gem.BaseParam = 0.0;
		if (_baseCrv == null)
		{
			return;
		}
		if (_vOrientationMethod == ValueListItem.GemOrientationMethodCurve)
		{
			_baseCrv.ClosestPoint(val, ref gem.BaseParam);
			return;
		}
		Plane val3 = default(Plane);
		((Plane)(ref val3))._002Ector(val, val2);
		CurveIntersections obj = Intersection.CurvePlane(_baseCrv, val3, 0.001);
		double num2 = 9999.0;
		foreach (IntersectionEvent item in obj)
		{
			Vector3d val4 = item.PointA - val;
			double length = ((Vector3d)(ref val4)).Length;
			if (length < num2)
			{
				num2 = length;
				gem.BaseParam = item.ParameterA;
			}
		}
	}

	private void MakeEndProfileParts(GemInfoUltimateShank g, List<Curve> lstBase, List<Curve> lstLeft, List<Curve> lstCenter, List<Curve> lstRight, List<double> param, List<double> param1, out Curve profileOut)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		profileOut = null;
		CalculatePointsOnCrv(g);
		ProfilePositionScale(g, out var pScale, out var pRail);
		Curve val = _profile.DuplicateCurve();
		Transform val2 = Transform.Scale(Plane.WorldXY, _profileWidth + g.SizeY, 1.0, 1.0);
		((GeometryBase)val).Transform(pRail * pScale * val2);
		Interval domain = val.Domain;
		Curve crv = val.Trim(((Interval)(ref domain)).Min, g.ParamLeft);
		double paramRight = g.ParamRight;
		domain = val.Domain;
		Curve crv2 = val.Trim(paramRight, ((Interval)(ref domain)).Max);
		Curve crv3 = val.Trim(g.ParamLeft, g.ParamRight);
		CSUtilGeometry.TrimEndOfCrv(ref crv, _channelBlend);
		CSUtilGeometry.TrimStartAndEndOfCrv(ref crv3, _channelBlend);
		CSUtilGeometry.TrimStartOfCrv(ref crv2, _channelBlend);
		Curve item = (Curve)(object)CSUtilCurves.BlendCurve(crv, crv3);
		Curve item2 = (Curve)(object)CSUtilCurves.BlendCurve(crv3, crv2);
		Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve> { crv2, crv }, CSApplicationHelper.Application.Tolerance);
		if (array != null && array.Length == 1)
		{
			Curve item3 = array[0];
			lstBase.Add(item3);
			lstLeft.Add(item);
			lstCenter.Add(crv3);
			lstRight.Add(item2);
			param.Add(g.Param);
			param1.Add(g.BaseParam);
			profileOut = val;
		}
	}

	private void MakeGemProfileParts(GemInfoUltimateShank g, List<Curve> lstBase, List<Curve> lstLeft, List<Curve> lstCenter, List<Curve> lstRight, List<double> param, List<double> param1)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Expected O, but got Unknown
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = g.ProfileCurve.PointAt(g.ParamLeft);
		Point3d val2 = g.ProfileCurve.PointAt(g.ParamRight);
		double num = (((Point3d)(ref val)).Y + ((Point3d)(ref val2)).Y) / 2.0 - g.SizeZ;
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(0.0, num, 0.0);
		Curve profileCurve = g.ProfileCurve;
		Interval domain = g.ProfileCurve.Domain;
		Curve crv = profileCurve.Trim(((Interval)(ref domain)).Min, g.ParamLeft);
		Curve crv4;
		Curve crv7;
		if (_channelWallDrop > _channelBlend)
		{
			Point3d val4 = default(Point3d);
			((Point3d)(ref val4))._002Ector(val);
			((Point3d)(ref val4)).Y = ((Point3d)(ref val4)).Y - _channelWallDrop;
			Point3d val5 = default(Point3d);
			((Point3d)(ref val5))._002Ector(val2);
			((Point3d)(ref val5)).Y = ((Point3d)(ref val5)).Y - _channelWallDrop;
			Curve crv2 = (Curve)new LineCurve(val, val4);
			Curve crv3 = (Curve)new LineCurve(val4, val3);
			CSUtilGeometry.TrimEndOfCrv(ref crv2, _channelBlend);
			CSUtilGeometry.TrimStartOfCrv(ref crv3, _channelBlend);
			Curve item = (Curve)(object)CSUtilCurves.BlendCurve(crv2, crv3);
			crv4 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve> { crv2, item, crv3 }, CSApplicationHelper.Application.Tolerance)[0];
			Curve crv5 = (Curve)new LineCurve(val3, val5);
			Curve crv6 = (Curve)new LineCurve(val5, val2);
			CSUtilGeometry.TrimEndOfCrv(ref crv5, _channelBlend);
			CSUtilGeometry.TrimStartOfCrv(ref crv6, _channelBlend);
			Curve item2 = (Curve)(object)CSUtilCurves.BlendCurve(crv5, crv6);
			crv7 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve> { crv5, item2, crv6 }, CSApplicationHelper.Application.Tolerance)[0];
		}
		else
		{
			crv4 = (Curve)new LineCurve(val, val3);
			crv7 = (Curve)new LineCurve(val3, val2);
		}
		Curve profileCurve2 = g.ProfileCurve;
		double paramRight = g.ParamRight;
		domain = g.ProfileCurve.Domain;
		Curve crv8 = profileCurve2.Trim(paramRight, ((Interval)(ref domain)).Max);
		CSUtilGeometry.TrimEndOfCrv(ref crv, _channelBlend);
		CSUtilGeometry.TrimStartAndEndOfCrv(ref crv4, _channelBlend);
		CSUtilGeometry.TrimStartAndEndOfCrv(ref crv7, _channelBlend);
		CSUtilGeometry.TrimStartOfCrv(ref crv8, _channelBlend);
		Curve item3 = (Curve)(object)CSUtilCurves.BlendCurve(crv, crv4);
		Curve val6 = (Curve)(object)CSUtilCurves.BlendCurve(crv4, crv7);
		Curve item4 = (Curve)(object)CSUtilCurves.BlendCurve(crv7, crv8);
		Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve> { crv, crv8 }, CSApplicationHelper.Application.Tolerance);
		if (array == null || array.Length != 1)
		{
			return;
		}
		Curve val7 = array[0];
		array = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve> { crv4, item3 }, CSApplicationHelper.Application.Tolerance);
		if (array != null && array.Length == 1)
		{
			Curve val8 = array[0];
			array = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve> { item4, crv7 }, CSApplicationHelper.Application.Tolerance);
			if (array != null && array.Length == 1)
			{
				Curve val9 = array[0];
				((GeometryBase)val7).Transform(g.ProfileTransform);
				((GeometryBase)val8).Transform(g.ProfileTransform);
				((GeometryBase)val9).Transform(g.ProfileTransform);
				((GeometryBase)val6).Transform(g.ProfileTransform);
				lstBase.Add(val7);
				lstLeft.Add(val8);
				lstCenter.Add(val6);
				lstRight.Add(val9);
				param.Add(g.Param);
				param1.Add(g.BaseParam);
			}
		}
	}

	private void ProfilePositionScale(GemInfoUltimateShank gem, out Transform pScale, out Transform pRail)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = _crv.PointAt(gem.Param);
		Vector3d destination = _crv.TangentAt(gem.Param);
		Point3d val2 = _baseCrv.PointAt(gem.BaseParam);
		Vector3d destination2 = val - val2;
		double length = ((Vector3d)(ref destination2)).Length;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, length, 1.0);
		pRail = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, length, 0.0), Vector3d.YAxis, Vector3d.ZAxis, val, destination2, destination);
	}
}
