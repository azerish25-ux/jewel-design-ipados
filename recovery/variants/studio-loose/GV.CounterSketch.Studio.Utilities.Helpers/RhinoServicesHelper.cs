using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using GV.CounterSketch.Shared.Utilities;
using GV.CounterSketch.Studio.Model;
using GV.CounterSketch.Studio.RhinoServicesReference;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class RhinoServicesHelper : IDisposable
{
	public delegate void UIControlsUpdatedDelegate(object s, UpdateUIControlCompletedEventArgs e);

	private static int _clientHitCounter;

	private static bool _clientOpening;

	private static int _failedConnectionCount;

	private static int _faultCounter;

	private static bool _killingApplication;

	private static RhinoServicesClient _rhinoServicesClient;

	private static int _runCounter;

	private readonly InstanceContext _instanceContext;

	public UIControlsUpdatedDelegate UIControlsUpdated;

	public bool IsConnectedToRhino
	{
		get
		{
			RhinoServicesClient rhinoServicesClient = _rhinoServicesClient;
			if (rhinoServicesClient == null)
			{
				return false;
			}
			return rhinoServicesClient.State == CommunicationState.Opened;
		}
	}

	public RhinoServicesClient Client
	{
		get
		{
			try
			{
				if (UIHelper.IsApplicationClosing)
				{
					return _rhinoServicesClient;
				}
				if (_killingApplication)
				{
					GlobalErrorHandler.WriteWarningToEventLog("Application is being killed... wait indefinitely.");
					return _rhinoServicesClient;
				}
				_clientHitCounter++;
				while (_clientOpening)
				{
					GlobalErrorHandler.WriteWarningToEventLog("Client is Opening...wait.");
					Thread.Sleep(1000);
				}
				if (_rhinoServicesClient == null)
				{
					Open("Client");
				}
				else if (_rhinoServicesClient.State != CommunicationState.Opened)
				{
					GlobalErrorHandler.WriteErrorToEventLog($"Rhino Client Not Opened: {_rhinoServicesClient.State}");
					Open("Client");
				}
			}
			catch (FaultException ex)
			{
				GlobalErrorHandler.WriteErrorToEventLog("Rhino Client Fault Error: " + ex);
			}
			catch (CommunicationException ex2)
			{
				GlobalErrorHandler.WriteErrorToEventLog("Rhino Client Communication Error: " + ex2);
			}
			catch (Exception ex3)
			{
				GlobalErrorHandler.WriteErrorToEventLog("Rhino Client Exception Error: " + ex3);
			}
			return _rhinoServicesClient;
		}
	}

	public static int ServerProcessId { get; set; }

	public bool IsSetUIFullBusy { get; set; }

	public bool IsSetUIQuickBusy { get; set; }

	public event EventHandler RhinoClientOpened;

	public RhinoServicesHelper()
	{
		_instanceContext = new InstanceContext(new StudioUIServiceCallback());
	}

	public void Dispose()
	{
		Close("Dispose");
	}

	public void Close([CallerMemberName] string caller = null)
	{
		GlobalErrorHandler.WriteInfoToEventLog("Closing Rhino Services Client, from: " + caller);
		try
		{
			if (_rhinoServicesClient != null && _rhinoServicesClient.State != CommunicationState.Closing && _rhinoServicesClient.State != CommunicationState.Closed)
			{
				if (_rhinoServicesClient.State != CommunicationState.Faulted)
				{
					_rhinoServicesClient.Close();
				}
				else
				{
					_rhinoServicesClient.Abort();
				}
			}
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Error Closing Rhino Services Client: " + ex);
		}
	}

	public void Open([CallerMemberName] string caller = null)
	{
		GlobalErrorHandler.WriteInfoToEventLog("Opening Rhino Services Client, from: " + caller);
		GlobalErrorHandler.WriteMethodInfo("Open");
		if (UIHelper.IsApplicationClosing)
		{
			GlobalErrorHandler.WriteInfoToEventLog("Application is Closing, Not ReOpening Service to Rhino");
			return;
		}
		try
		{
			_clientOpening = true;
			GlobalErrorHandler.WriteInfoToEventLog("Begin Open() Client", 2);
			if (_rhinoServicesClient != null)
			{
				CommunicationState state = _rhinoServicesClient.State;
				if (state == CommunicationState.Opening || state == CommunicationState.Opened)
				{
					GlobalErrorHandler.WriteInfoToEventLog($"RhinoServicesClient asked to Open, but already {state}.");
					_clientOpening = false;
					return;
				}
				_ = 5;
			}
			_runCounter++;
			GlobalErrorHandler.WriteInfoToEventLog("Rhino Client Hit Count: " + _clientHitCounter, 2);
			GlobalErrorHandler.WriteInfoToEventLog("Rhino Client Opened Count: " + _runCounter, 2);
			Stopwatch stopwatch = new Stopwatch();
			Stopwatch stopwatch2 = new Stopwatch();
			stopwatch.Start();
			stopwatch2.Start();
			IsSetUIFullBusy = false;
			IsSetUIQuickBusy = false;
			stopwatch2.Reset();
			stopwatch2.Start();
			GlobalErrorHandler.WriteInfoToEventLog("Get Binding", 2);
			NetTcpBinding netTcpBinding = ServiceHostExtensions.GetNetTcpBinding();
			GlobalErrorHandler.WriteInfoToEventLog("Got Binding: " + stopwatch2.ElapsedMilliseconds + " ms.", 2);
			EndpointAddress remoteAddress = new EndpointAddress("net.tcp://localhost:8556/RhinoServices");
			stopwatch2.Reset();
			stopwatch2.Start();
			GlobalErrorHandler.WriteInfoToEventLog("Creating RhinoServicesClient Client", 2);
			_rhinoServicesClient = new RhinoServicesClient(_instanceContext, netTcpBinding, remoteAddress);
			_rhinoServicesClient.UpdateUIControlCompleted += delegate(object s, UpdateUIControlCompletedEventArgs e)
			{
				UIControlsUpdated(s, e);
			};
			foreach (OperationDescription operation in _rhinoServicesClient.Endpoint.Contract.Operations)
			{
				DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
				if (dataContractSerializerOperationBehavior != null)
				{
					dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = int.MaxValue;
				}
			}
			GlobalErrorHandler.WriteInfoToEventLog("Created RhinoServicesClient Client: " + stopwatch2.ElapsedMilliseconds + " ms.", 2);
			stopwatch2.Reset();
			stopwatch2.Start();
			GlobalErrorHandler.WriteInfoToEventLog("Opening Rhino Proxy", 2);
			_rhinoServicesClient.Open();
			GlobalErrorHandler.WriteInfoToEventLog("Rhino Proxy Opened: " + stopwatch2.ElapsedMilliseconds + " ms.", 2);
			RhinoServicesClient rhinoServicesClient = _rhinoServicesClient;
			((ICommunicationObject)rhinoServicesClient).Faulted += Faulted;
			((ICommunicationObject)rhinoServicesClient).Opening += Opening;
			((ICommunicationObject)rhinoServicesClient).Closing += Closing;
			stopwatch.Stop();
			GlobalErrorHandler.WriteInfoToEventLog("End Open() Client: " + stopwatch.ElapsedMilliseconds + " ms.", 2);
			_failedConnectionCount = 0;
			_clientOpening = false;
			_rhinoServicesClient.UpdateStudioUICallbackChannel();
			RaiseRhinoClientOpenedEvent();
		}
		catch (Exception ex)
		{
			_clientOpening = false;
			GlobalErrorHandler.WriteInfoToEventLog("Error connecting to CSServer, checking if process is still alive, " + ex);
			CheckServerExistence("Open");
		}
	}

	public void RaiseRhinoClientOpenedEvent()
	{
		this.RhinoClientOpened?.Invoke(null, new EventArgs());
	}

	private static bool CheckServerExistence([CallerMemberName] string caller = null)
	{
		GlobalErrorHandler.WriteInfoToEventLog("CheckServerExistence, from: " + caller);
		bool result = false;
		if (ServerProcessId != 0)
		{
			if (!Process.GetProcesses().Any((Process x) => x.Id == ServerProcessId))
			{
				_killingApplication = true;
				GlobalErrorHandler.WriteInfoToEventLog("Server process does not exist, prompting to force close the application.");
				UIHelper.PromptToForceCloseApplication();
			}
			else if (_failedConnectionCount++ >= 5)
			{
				GlobalErrorHandler.WriteInfoToEventLog("Server has not responded to connection for 5+ attempts, prompting to force close the application.");
				UIHelper.PromptToForceCloseApplication();
			}
		}
		else
		{
			result = true;
		}
		return result;
	}

	private void Closing(object sender, EventArgs e)
	{
		GlobalErrorHandler.WriteInfoToEventLog("Rhino Client Closing.");
	}

	private void Faulted(object sender, EventArgs e)
	{
		_faultCounter++;
		GlobalErrorHandler.WriteInfoToEventLog($"Rhino Client Faulted. Fault Count: {_faultCounter}");
		if (CheckServerExistence("Faulted") && !UIHelper.IsApplicationClosing)
		{
			Open("Faulted");
		}
	}

	private void Opening(object sender, EventArgs e)
	{
		GlobalErrorHandler.WriteInfoToEventLog("Rhino Client Opening.");
	}

	~RhinoServicesHelper()
	{
		Dispose();
	}
}
