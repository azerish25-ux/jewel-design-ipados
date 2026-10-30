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
			BoundingBox boundingBox = _profile.GetBoundingBox(accurate: true);
			double x = boundingBox.Diagonal.X;
			double y = boundingBox.Diagonal.Y;
			Transform xform = Transform.Translation((0.0 - (boundingBox.Max.X + boundingBox.Min.X)) / 2.0, 0.0 - boundingBox.Min.Y, 0.0);
			_profile.Transform(xform);
			Transform xform2 = Transform.Scale(Plane.WorldXY, 1.0 / x, 1.0 / y, 1.0);
			_profile.Transform(xform2);
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
		if (!_gemsClosedLoop)
		{
			GemInfoUltimateShank g = ((gemList.Count <= 0) ? new GemInfoUltimateShank
			{
				ParamLeft = _profile.Domain.Min + _profile.Domain.Length * 0.4,
				ParamRight = _profile.Domain.Min + _profile.Domain.Length * 0.6,
				NormParam = (_doMirror ? (_positionStartInput - (_positionEndInput - _positionStartInput) * _direction) : _positionStartInput),
				SizeY = (_doMirror ? _yEnd : _yStart)
			} : new GemInfoUltimateShank
			{
				ParamLeft = gemList[0].ParamLeft,
				ParamRight = gemList[0].ParamRight,
				NormParam = gemList[0].NormParam - (gemList[0].SizeX / 2.0 + _channelSlopeLength) / crvLength * _direction,
				SizeY = (_doMirror ? _yEnd : _yStart)
			});
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
			GemInfoUltimateShank g4 = ((gemList.Count <= 0) ? new GemInfoUltimateShank
			{
				ParamLeft = _profile.Domain.Min + _profile.Domain.Length * 0.4,
				ParamRight = _profile.Domain.Min + _profile.Domain.Length * 0.6,
				NormParam = _positionEndInput,
				SizeY = _yEnd
			} : new GemInfoUltimateShank
			{
				ParamLeft = gemList[gemList.Count - 1].ParamLeft,
				ParamRight = gemList[gemList.Count - 1].ParamRight,
				NormParam = gemList[gemList.Count - 1].NormParam + (gemList[gemList.Count - 1].SizeX / 2.0 + _channelSlopeLength) / crvLength * _direction,
				SizeY = _yEnd
			});
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
			Brep[] array = Brep.JoinBreps(new List<Brep> { breps, breps2, breps4, breps3 }, CSApplicationHelper.Application.Tolerance);
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
			Brep[] array2 = Brep.JoinBreps(new List<Brep> { breps, breps2, breps4, breps3 }, CSApplicationHelper.Application.Tolerance);
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
		curveFromBrepEdges.NormalizedLengthParameter(_channelEndPulldownDistance / length, out var t);
		curveFromBrepEdges2.NormalizedLengthParameter(_channelEndPulldownDistance / length2, out var t2);
		Point3d point3d = curveFromBrepEdges.PointAt(t);
		Point3d to = curveFromBrepEdges2.PointAt(t2);
		Curve item = new LineCurve(point3d, to);
		curveFromBrepEdges.NormalizedLengthParameter((length - _channelEndPulldownDistance) / length, out var t3);
		curveFromBrepEdges2.NormalizedLengthParameter((length2 - _channelEndPulldownDistance) / length2, out var t4);
		Point3d point3d2 = curveFromBrepEdges.PointAt(t3);
		Point3d to2 = curveFromBrepEdges2.PointAt(t4);
		Curve item2 = new LineCurve(point3d2, to2);
		List<Curve> list7 = new List<Curve>();
		list5.Clear();
		list6.Clear();
		list7.Add(list3[0]);
		curveFromBrepEdges.ClosestPoint(list3[0].PointAtStart, out var t5, CSApplicationHelper.Application.Tolerance);
		list5.Add(t5);
		curveFromBrepEdges2.ClosestPoint(list3[0].PointAtEnd, out t5, CSApplicationHelper.Application.Tolerance);
		list6.Add(t5);
		if (_channelEndPulldownDistance > 0.01)
		{
			list7.Add(item);
			list5.Add(t);
			list6.Add(t2);
		}
		list7.Add(list3[1]);
		curveFromBrepEdges.ClosestPoint(list3[1].PointAtStart, out t5, CSApplicationHelper.Application.Tolerance);
		list5.Add(t5);
		curveFromBrepEdges2.ClosestPoint(list3[1].PointAtEnd, out t5, CSApplicationHelper.Application.Tolerance);
		list6.Add(t5);
		if (list3.Count > 3)
		{
			list7.Add(list3[list3.Count - 2]);
			curveFromBrepEdges.ClosestPoint(list3[list3.Count - 2].PointAtStart, out t5, CSApplicationHelper.Application.Tolerance);
			list5.Add(t5);
			curveFromBrepEdges2.ClosestPoint(list3[list3.Count - 2].PointAtEnd, out t5, CSApplicationHelper.Application.Tolerance);
			list6.Add(t5);
		}
		if (_channelEndPulldownDistance > 0.01)
		{
			list7.Add(item2);
			list5.Add(t3);
			list6.Add(t4);
		}
		list7.Add(list3[list3.Count - 1]);
		curveFromBrepEdges.ClosestPoint(list3[list3.Count - 1].PointAtStart, out t5, CSApplicationHelper.Application.Tolerance);
		list5.Add(t5);
		curveFromBrepEdges2.ClosestPoint(list3[list3.Count - 1].PointAtEnd, out t5, CSApplicationHelper.Application.Tolerance);
		list6.Add(t5);
		if (!CSUtilSweepRC.Sweep2(curveFromBrepEdges, curveFromBrepEdges2, list7, list5, list6, out breps4, false, true) || breps4 == null)
		{
			return new CSData();
		}
		Brep[] array3 = Brep.JoinBreps(new List<Brep> { breps, breps2, breps4, breps3 }, CSApplicationHelper.Application.Tolerance);
		if (array3.Length == 1)
		{
			return CSOnGeometryData.FromBrep(array3[0]);
		}
		return new CSData();
	}

	private void CalculatePointsOnCrv(GemInfoUltimateShank gem)
	{
		double num;
		for (num = gem.NormParam; num > 1.0; num -= 1.0)
		{
		}
		for (; num < 0.0; num += 1.0)
		{
		}
		_crv.NormalizedLengthParameter(num, out gem.Param);
		Point3d point3d = _crv.PointAt(gem.Param);
		Vector3d normal = _crv.TangentAt(gem.Param);
		gem.BaseParam = 0.0;
		if (_baseCrv == null)
		{
			return;
		}
		if (_vOrientationMethod == ValueListItem.GemOrientationMethodCurve)
		{
			_baseCrv.ClosestPoint(point3d, out gem.BaseParam);
			return;
		}
		CurveIntersections obj = Intersection.CurvePlane(plane: new Plane(point3d, normal), curve: _baseCrv, tolerance: 0.001);
		double num2 = 9999.0;
		foreach (IntersectionEvent item in obj)
		{
			double length = (item.PointA - point3d).Length;
			if (length < num2)
			{
				num2 = length;
				gem.BaseParam = item.ParameterA;
			}
		}
	}

	private void MakeEndProfileParts(GemInfoUltimateShank g, List<Curve> lstBase, List<Curve> lstLeft, List<Curve> lstCenter, List<Curve> lstRight, List<double> param, List<double> param1, out Curve profileOut)
	{
		profileOut = null;
		CalculatePointsOnCrv(g);
		ProfilePositionScale(g, out var pScale, out var pRail);
		Curve curve = _profile.DuplicateCurve();
		Transform transform = Transform.Scale(Plane.WorldXY, _profileWidth + g.SizeY, 1.0, 1.0);
		curve.Transform(pRail * pScale * transform);
		Curve crv = curve.Trim(curve.Domain.Min, g.ParamLeft);
		Curve crv2 = curve.Trim(g.ParamRight, curve.Domain.Max);
		Curve crv3 = curve.Trim(g.ParamLeft, g.ParamRight);
		CSUtilGeometry.TrimEndOfCrv(ref crv, _channelBlend);
		CSUtilGeometry.TrimStartAndEndOfCrv(ref crv3, _channelBlend);
		CSUtilGeometry.TrimStartOfCrv(ref crv2, _channelBlend);
		Curve item = CSUtilCurves.BlendCurve(crv, crv3);
		Curve item2 = CSUtilCurves.BlendCurve(crv3, crv2);
		Curve[] array = Curve.JoinCurves(new List<Curve> { crv2, crv }, CSApplicationHelper.Application.Tolerance);
		if (array != null && array.Length == 1)
		{
			Curve item3 = array[0];
			lstBase.Add(item3);
			lstLeft.Add(item);
			lstCenter.Add(crv3);
			lstRight.Add(item2);
			param.Add(g.Param);
			param1.Add(g.BaseParam);
			profileOut = curve;
		}
	}

	private void MakeGemProfileParts(GemInfoUltimateShank g, List<Curve> lstBase, List<Curve> lstLeft, List<Curve> lstCenter, List<Curve> lstRight, List<double> param, List<double> param1)
	{
		Point3d point3d = g.ProfileCurve.PointAt(g.ParamLeft);
		Point3d point3d2 = g.ProfileCurve.PointAt(g.ParamRight);
		double y = (point3d.Y + point3d2.Y) / 2.0 - g.SizeZ;
		Point3d point3d3 = new Point3d(0.0, y, 0.0);
		Curve crv = g.ProfileCurve.Trim(g.ProfileCurve.Domain.Min, g.ParamLeft);
		Curve crv4;
		Curve crv7;
		if (_channelWallDrop > _channelBlend)
		{
			Point3d point3d4 = new Point3d(point3d);
			point3d4.Y -= _channelWallDrop;
			Point3d point3d5 = new Point3d(point3d2);
			point3d5.Y -= _channelWallDrop;
			Curve crv2 = new LineCurve(point3d, point3d4);
			Curve crv3 = new LineCurve(point3d4, point3d3);
			CSUtilGeometry.TrimEndOfCrv(ref crv2, _channelBlend);
			CSUtilGeometry.TrimStartOfCrv(ref crv3, _channelBlend);
			Curve item = CSUtilCurves.BlendCurve(crv2, crv3);
			crv4 = Curve.JoinCurves(new List<Curve> { crv2, item, crv3 }, CSApplicationHelper.Application.Tolerance)[0];
			Curve crv5 = new LineCurve(point3d3, point3d5);
			Curve crv6 = new LineCurve(point3d5, point3d2);
			CSUtilGeometry.TrimEndOfCrv(ref crv5, _channelBlend);
			CSUtilGeometry.TrimStartOfCrv(ref crv6, _channelBlend);
			Curve item2 = CSUtilCurves.BlendCurve(crv5, crv6);
			crv7 = Curve.JoinCurves(new List<Curve> { crv5, item2, crv6 }, CSApplicationHelper.Application.Tolerance)[0];
		}
		else
		{
			crv4 = new LineCurve(point3d, point3d3);
			crv7 = new LineCurve(point3d3, point3d2);
		}
		Curve crv8 = g.ProfileCurve.Trim(g.ParamRight, g.ProfileCurve.Domain.Max);
		CSUtilGeometry.TrimEndOfCrv(ref crv, _channelBlend);
		CSUtilGeometry.TrimStartAndEndOfCrv(ref crv4, _channelBlend);
		CSUtilGeometry.TrimStartAndEndOfCrv(ref crv7, _channelBlend);
		CSUtilGeometry.TrimStartOfCrv(ref crv8, _channelBlend);
		Curve item3 = CSUtilCurves.BlendCurve(crv, crv4);
		Curve curve = CSUtilCurves.BlendCurve(crv4, crv7);
		Curve item4 = CSUtilCurves.BlendCurve(crv7, crv8);
		Curve[] array = Curve.JoinCurves(new List<Curve> { crv, crv8 }, CSApplicationHelper.Application.Tolerance);
		if (array == null || array.Length != 1)
		{
			return;
		}
		Curve curve2 = array[0];
		array = Curve.JoinCurves(new List<Curve> { crv4, item3 }, CSApplicationHelper.Application.Tolerance);
		if (array != null && array.Length == 1)
		{
			Curve curve3 = array[0];
			array = Curve.JoinCurves(new List<Curve> { item4, crv7 }, CSApplicationHelper.Application.Tolerance);
			if (array != null && array.Length == 1)
			{
				Curve curve4 = array[0];
				curve2.Transform(g.ProfileTransform);
				curve3.Transform(g.ProfileTransform);
				curve4.Transform(g.ProfileTransform);
				curve.Transform(g.ProfileTransform);
				lstBase.Add(curve2);
				lstLeft.Add(curve3);
				lstCenter.Add(curve);
				lstRight.Add(curve4);
				param.Add(g.Param);
				param1.Add(g.BaseParam);
			}
		}
	}

	private void ProfilePositionScale(GemInfoUltimateShank gem, out Transform pScale, out Transform pRail)
	{
		Point3d point3d = _crv.PointAt(gem.Param);
		Vector3d destination = _crv.TangentAt(gem.Param);
		Point3d point3d2 = _baseCrv.PointAt(gem.BaseParam);
		Vector3d destination2 = point3d - point3d2;
		double length = destination2.Length;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, length, 1.0);
		pRail = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, length, 0.0), Vector3d.YAxis, Vector3d.ZAxis, point3d, destination2, destination);
	}
}
