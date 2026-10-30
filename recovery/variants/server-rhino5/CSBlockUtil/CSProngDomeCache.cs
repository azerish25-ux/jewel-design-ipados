using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RMA.OpenNURBS;

namespace CSBlockUtil;

public sealed class CSProngDomeCache
{
	private sealed class ProngDomeCacheItem
	{
		private readonly OnNurbsSurface _prongDome;

		private readonly IOnCurve _profile;

		private readonly double _diameter;

		private readonly double _filletPer;

		private readonly double _taperPer;

		public OnNurbsSurface ProngDome => _prongDome.Duplicate();

		public ProngDomeCacheItem(OnNurbsSurface prongDome, IOnCurve profile, double diameter, double filletPer, double taperPer)
		{
			_prongDome = prongDome;
			_profile = profile;
			_diameter = diameter;
			_filletPer = filletPer;
			_taperPer = taperPer;
		}

		public bool Compare(IOnCurve profile, double diameter, double filletPer, double taperPer, double tolerance = 0.001)
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

	private ConcurrentBag<ProngDomeCacheItem> _prongDomeCache = new ConcurrentBag<ProngDomeCacheItem>();

	public void Clear()
	{
		_prongDomeCache = new ConcurrentBag<ProngDomeCacheItem>();
	}

	public bool Create(IOnNurbsCurve profile, double diameter, double filletPer, double taperPer, out OnNurbsSurface prongDome)
	{
		if (_prongDomeCache == null)
		{
			_prongDomeCache = new ConcurrentBag<ProngDomeCacheItem>();
		}
		using (IEnumerator<ProngDomeCacheItem> enumerator = _prongDomeCache.Where((ProngDomeCacheItem prongDomeCacheItem2) => prongDomeCacheItem2.Compare((IOnCurve)(object)profile, diameter, filletPer, taperPer)).GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				ProngDomeCacheItem current = enumerator.Current;
				prongDome = current.ProngDome;
				return true;
			}
		}
		prongDome = null;
		if (!CSUtilGeometry.CreateProngDome(profile, diameter, filletPer, taperPer, out var dome))
		{
			return false;
		}
		ProngDomeCacheItem prongDomeCacheItem = new ProngDomeCacheItem(dome, (IOnCurve)(object)profile, diameter, filletPer, taperPer);
		_prongDomeCache.Add(prongDomeCacheItem);
		prongDome = prongDomeCacheItem.ProngDome;
		return true;
	}
}
