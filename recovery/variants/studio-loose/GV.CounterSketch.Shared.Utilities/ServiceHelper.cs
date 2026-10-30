using System;
using System.ServiceModel;

namespace GV.CounterSketch.Shared.Utilities;

public class ServiceHelper
{
	public static void Execute<TService>(TService serviceInstance, Action<TService> action) where TService : ICommunicationObject, IDisposable
	{
		bool flag = false;
		try
		{
			if (serviceInstance.State == CommunicationState.Faulted)
			{
				serviceInstance.Abort();
				serviceInstance.Open();
			}
			action(serviceInstance);
			flag = true;
		}
		catch (FaultException)
		{
		}
		catch (CommunicationException)
		{
		}
		finally
		{
			if (!flag)
			{
				serviceInstance.Abort();
				serviceInstance.Close();
				serviceInstance.Open();
			}
		}
	}
}
