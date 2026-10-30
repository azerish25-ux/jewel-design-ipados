using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RMA.OpenNURBS;

namespace CSBlockUtil;

public sealed class CSProngCache
{
	private sealed class ProngCacheItem
	{
		private readonly OnGeometry _prong;

		private readonly OnGeometry _prongBaseRail;

		private readonly IOnCurve _profile;

		private readonly double _diameter;

		private readonly double _height;

		private readonly double _filletPer;

		private readonly double _taperPer;

		public OnGeometry Prong => _prong.Duplicate();

		public OnGeometry ProngBaseRail => _prongBaseRail.Duplicate();

		public ProngCacheItem(OnGeometry prong, OnGeometry prongBaseRail, IOnCurve profile, double diameter, double height, double filletPer, double taperPer)
		{
			_prong = prong;
			_prongBaseRail = prongBaseRail;
			_profile = profile;
			_diameter = diameter;
			_height = height;
			_filletPer = filletPer;
			_taperPer = taperPer;
		}

		public bool Compare(IOnCurve profile, double diameter, double height, double filletPer, double taperPer, double tolerance = 0.001)
		{
			if (_profile.Domain().Min() - profile.Domain().Min() > tolerance)
			{
				return false;
			}
			if (_profile.Domain().Mid() - profile.Domain().Mid() > tolerance)
			{
				return false;
			}
			if (_profile.Domain().Max() - profile.Domain().Max() > tolerance)
			{
				return false;
			}
			if (Math.Abs(_diameter - diameter) > tolerance)
			{
				return false;
			}
			if (Math.Abs(_height - height) > tolerance)
			{
				return false;
			}
			if (Math.Abs(_filletPer - filletPer) > tolerance)
			{
				return false;
			}
			if (Math.Abs(_taperPer - taperPer) > tolerance)
			{
				return false;
			}
			return true;
		}
	}

	private ConcurrentBag<ProngCacheItem> _prongCache = new ConcurrentBag<ProngCacheItem>();

	public void Clear()
	{
		_prongCache = new ConcurrentBag<ProngCacheItem>();
	}

	public bool Create(IOnNurbsCurve profile, double diameter, double height, double filletPer, double taperPer, bool doMesh, out OnGeometry prong, out OnGeometry prongBaseRail)
	{
		if (CSUtil.IsCounterSketch)
		{
			if (_prongCache == null)
			{
				_prongCache = new ConcurrentBag<ProngCacheItem>();
			}
			using IEnumerator<ProngCacheItem> enumerator = _prongCache.Where((ProngCacheItem prongCacheItem2) => prongCacheItem2.Compare(profile, diameter, height, filletPer, taperPer)).GetEnumerator();
			if (enumerator.MoveNext())
			{
				ProngCacheItem current = enumerator.Current;
				prong = current.Prong;
				prongBaseRail = current.ProngBaseRail;
				return true;
			}
		}
		prong = null;
		prongBaseRail = null;
		ProngCacheItem prongCacheItem;
		if (doMesh)
		{
			if (!CSUtilGeometry.CreateMeshProng(profile, diameter, height, filletPer, taperPer, out var meshSurface, out var outRailLine))
			{
				return false;
			}
			prongCacheItem = new ProngCacheItem(meshSurface, new OnPolylineCurve(outRailLine), profile, diameter, height, filletPer, taperPer);
		}
		else
		{
			if (!CSUtilGeometry.CreateProng(profile, diameter, height, filletPer, taperPer, out var nurbsSurface, out var outputRail))
			{
				return false;
			}
			prongCacheItem = new ProngCacheItem(nurbsSurface, outputRail, profile, diameter, height, filletPer, taperPer);
		}
		_prongCache.Add(prongCacheItem);
		prong = prongCacheItem.Prong;
		prongBaseRail = prongCacheItem.ProngBaseRail;
		return true;
	}

	public bool CreateNew(IOnNurbsCurve profile, double diameter, double height, double filletPer, double taperPer, bool doMesh, out OnGeometry prong, out OnGeometry prongBaseRail, double bottomPercentage = 0.0)
	{
		if (CSUtil.IsCounterSketch)
		{
			if (_prongCache == null)
			{
				_prongCache = new ConcurrentBag<ProngCacheItem>();
			}
			using IEnumerator<ProngCacheItem> enumerator = _prongCache.Where((ProngCacheItem prongCacheItem2) => prongCacheItem2.Compare(profile, diameter, height, filletPer, taperPer)).GetEnumerator();
			if (enumerator.MoveNext())
			{
				ProngCacheItem current = enumerator.Current;
				prong = current.Prong;
				prongBaseRail = current.ProngBaseRail;
				return true;
			}
		}
		prong = null;
		prongBaseRail = null;
		ProngCacheItem prongCacheItem;
		if (doMesh)
		{
			if (!CSUtilGeometry.CreateMeshProngNew(profile, diameter, height, filletPer, taperPer, out var meshSurface, out var outRailLine, bottomPercentage))
			{
				return false;
			}
			prongCacheItem = new ProngCacheItem(meshSurface, new OnPolylineCurve(outRailLine), profile, diameter, height, filletPer, taperPer);
		}
		else
		{
			if (!CSUtilGeometry.CreateProngNew(profile, diameter, height, filletPer, taperPer, bottomPercentage, out var nurbsSurface, out var outputRail))
			{
				return false;
			}
			prongCacheItem = new ProngCacheItem(nurbsSurface, outputRail, profile, diameter, height, filletPer, taperPer);
		}
		_prongCache.Add(prongCacheItem);
		prong = prongCacheItem.Prong;
		prongBaseRail = prongCacheItem.ProngBaseRail;
		return true;
	}
}
