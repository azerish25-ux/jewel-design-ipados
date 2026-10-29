using System;
using System.Collections.Generic;
using System.Threading;

namespace GemvisionCommon.Helpers;

public static class RetryHelper
{
	private const int RetryCountMax = 3;

	private const int RetryIntervalMillisecondsDefault = 100;

	public static void Do(Action action, int retryCountMax = 3)
	{
		TimeSpan retryInterval = TimeSpan.FromMilliseconds(100.0);
		Do(action, retryInterval, retryCountMax);
	}

	public static void Do(Action action, TimeSpan retryInterval, int retryCountMax = 3)
	{
		Do(delegate
		{
			action();
			return (object)null;
		}, retryInterval, retryCountMax);
	}

	public static T Do<T>(Func<T> action, int retryCountMax = 3)
	{
		TimeSpan retryInterval = TimeSpan.FromMilliseconds(100.0);
		return Do(action, retryInterval, retryCountMax);
	}

	public static T Do<T>(Func<T> action, TimeSpan retryInterval, int retryCountMax = 3)
	{
		List<Exception> list = new List<Exception>();
		for (int i = 0; i < retryCountMax; i++)
		{
			try
			{
				if (i > 0)
				{
					Thread.Sleep(retryInterval);
				}
				return action();
			}
			catch (Exception item)
			{
				list.Add(item);
			}
		}
		throw new AggregateException(list);
	}
}
