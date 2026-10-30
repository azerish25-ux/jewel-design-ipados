using System;
using System.Diagnostics;
using System.Globalization;
using System.Timers;

namespace CS.Utilities.Helpers;

public sealed class ActivityHelper
{
	private static readonly Stopwatch ActiveTimer;

	private static readonly Timer IdleTimer;

	private static readonly Stopwatch OpenTimer;

	private static TimeSpan _activeTimeOffset;

	private static TimeSpan _openTimeOffset;

	public static TimeSpan ActiveTime => ActiveTimer.Elapsed.Add(_activeTimeOffset);

	public static bool IsActive
	{
		get
		{
			if (ActiveTimer != null)
			{
				return ActiveTimer.IsRunning;
			}
			return false;
		}
	}

	public static TimeSpan OpenTime => OpenTimer.Elapsed.Add(_openTimeOffset);

	static ActivityHelper()
	{
		ActiveTimer = new Stopwatch();
		IdleTimer = new Timer();
		OpenTimer = new Stopwatch();
		_activeTimeOffset = new TimeSpan(0L);
		_openTimeOffset = new TimeSpan(0L);
		IdleTimer.Interval = 120000.0;
		IdleTimer.Elapsed += _idleTimer_Elapsed;
	}

	~ActivityHelper()
	{
		if (ActiveTimer != null)
		{
			ActiveTimer.Stop();
		}
		if (IdleTimer != null)
		{
			IdleTimer.Stop();
			IdleTimer.Elapsed -= _idleTimer_Elapsed;
		}
		if (OpenTimer != null)
		{
			OpenTimer.Stop();
		}
	}

	public static string ActiveTimeString()
	{
		return string.Format("{0}:{1}", ActiveTime.Days * 24 + ActiveTime.Hours, ActiveTime.ToString("mm\\:ss"));
	}

	public static void DesignClicked()
	{
		ActiveTimer.Start();
		IdleTimer.Start();
	}

	public static void DesignCreated()
	{
		ResetActivity();
	}

	public static void DesignGenerating()
	{
		ActiveTimer.Start();
		IdleTimer.Start();
	}

	public static void DesignOpened()
	{
		_openTimeOffset = new TimeSpan(0L);
		_activeTimeOffset = new TimeSpan(0L);
		string text = CSApplicationHelper.Application.Document.DocumentUserData.FindValue("ACTIVITY", "OPEN");
		string text2 = CSApplicationHelper.Application.Document.DocumentUserData.FindValue("ACTIVITY", "ACTIVE");
		if (text != null)
		{
			_openTimeOffset = TimeSpan.FromSeconds(double.Parse(text, CultureInfo.InvariantCulture));
		}
		if (text2 != null)
		{
			_activeTimeOffset = TimeSpan.FromSeconds(double.Parse(text2, CultureInfo.InvariantCulture));
		}
		if (_openTimeOffset < _activeTimeOffset)
		{
			_openTimeOffset = _activeTimeOffset;
		}
		OpenTimer.Reset();
		OpenTimer.Start();
		ActiveTimer.Reset();
		ActiveTimer.Start();
		IdleTimer.Start();
	}

	public static void DesignSaving()
	{
		CSApplicationHelper.Application.Document.DocumentUserData.AddKey("ACTIVITY", "OPEN", OpenTimer.Elapsed.Add(_openTimeOffset).TotalSeconds.ToString(CultureInfo.InvariantCulture));
		CSApplicationHelper.Application.Document.DocumentUserData.AddKey("ACTIVITY", "ACTIVE", ActiveTimer.Elapsed.Add(_activeTimeOffset).TotalSeconds.ToString(CultureInfo.InvariantCulture));
	}

	public static string OpenTimeString()
	{
		return string.Format("{0}:{1}", OpenTime.Days * 24 + OpenTime.Hours, OpenTime.ToString("mm\\:ss"));
	}

	public static void ResetActivity()
	{
		_activeTimeOffset = new TimeSpan(0L);
		_openTimeOffset = new TimeSpan(0L);
		ActiveTimer.Reset();
		OpenTimer.Reset();
		OpenTimer.Start();
		IdleTimer.Start();
	}

	private static void _idleTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		ActiveTimer.Stop();
		IdleTimer.Stop();
	}
}
