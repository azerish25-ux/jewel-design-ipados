using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

namespace GV.CounterSketch.Studio.CounterSketchThemeServiceReference;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class CounterSketchThemeServiceClient : ClientBase<ICounterSketchThemeService>, ICounterSketchThemeService
{
	private BeginOperationDelegate onBeginAssignCustomerThemeByKeyIdDelegate;

	private EndOperationDelegate onEndAssignCustomerThemeByKeyIdDelegate;

	private SendOrPostCallback onAssignCustomerThemeByKeyIdCompletedDelegate;

	private BeginOperationDelegate onBeginCreateCustomerThemeDelegate;

	private EndOperationDelegate onEndCreateCustomerThemeDelegate;

	private SendOrPostCallback onCreateCustomerThemeCompletedDelegate;

	private BeginOperationDelegate onBeginDeleteCustomerThemeDelegate;

	private EndOperationDelegate onEndDeleteCustomerThemeDelegate;

	private SendOrPostCallback onDeleteCustomerThemeCompletedDelegate;

	private BeginOperationDelegate onBeginGetAvailableCustomerThemesDelegate;

	private EndOperationDelegate onEndGetAvailableCustomerThemesDelegate;

	private SendOrPostCallback onGetAvailableCustomerThemesCompletedDelegate;

	private BeginOperationDelegate onBeginGetAvailableCustomerThemesByKeyIdDelegate;

	private EndOperationDelegate onEndGetAvailableCustomerThemesByKeyIdDelegate;

	private SendOrPostCallback onGetAvailableCustomerThemesByKeyIdCompletedDelegate;

	private BeginOperationDelegate onBeginGetCustomersDelegate;

	private EndOperationDelegate onEndGetCustomersDelegate;

	private SendOrPostCallback onGetCustomersCompletedDelegate;

	private BeginOperationDelegate onBeginGetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeDelegate;

	private EndOperationDelegate onEndGetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeDelegate;

	private SendOrPostCallback onGetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeCompletedDelegate;

	private BeginOperationDelegate onBeginGetCustomersByUserTypeDelegate;

	private EndOperationDelegate onEndGetCustomersByUserTypeDelegate;

	private SendOrPostCallback onGetCustomersByUserTypeCompletedDelegate;

	private BeginOperationDelegate onBeginGetCustomerUserTypesDelegate;

	private EndOperationDelegate onEndGetCustomerUserTypesDelegate;

	private SendOrPostCallback onGetCustomerUserTypesCompletedDelegate;

	private BeginOperationDelegate onBeginRemoveAssignedCustomerThemeByKeyIdDelegate;

	private EndOperationDelegate onEndRemoveAssignedCustomerThemeByKeyIdDelegate;

	private SendOrPostCallback onRemoveAssignedCustomerThemeByKeyIdCompletedDelegate;

	private BeginOperationDelegate onBeginUpdateCustomerThemeDelegate;

	private EndOperationDelegate onEndUpdateCustomerThemeDelegate;

	private SendOrPostCallback onUpdateCustomerThemeCompletedDelegate;

	public event EventHandler<AssignCustomerThemeByKeyIdCompletedEventArgs> AssignCustomerThemeByKeyIdCompleted;

	public event EventHandler<CreateCustomerThemeCompletedEventArgs> CreateCustomerThemeCompleted;

	public event EventHandler<DeleteCustomerThemeCompletedEventArgs> DeleteCustomerThemeCompleted;

	public event EventHandler<GetAvailableCustomerThemesCompletedEventArgs> GetAvailableCustomerThemesCompleted;

	public event EventHandler<GetAvailableCustomerThemesByKeyIdCompletedEventArgs> GetAvailableCustomerThemesByKeyIdCompleted;

	public event EventHandler<GetCustomersCompletedEventArgs> GetCustomersCompleted;

	public event EventHandler<GetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeCompletedEventArgs> GetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeCompleted;

	public event EventHandler<GetCustomersByUserTypeCompletedEventArgs> GetCustomersByUserTypeCompleted;

	public event EventHandler<GetCustomerUserTypesCompletedEventArgs> GetCustomerUserTypesCompleted;

	public event EventHandler<RemoveAssignedCustomerThemeByKeyIdCompletedEventArgs> RemoveAssignedCustomerThemeByKeyIdCompleted;

	public event EventHandler<UpdateCustomerThemeCompletedEventArgs> UpdateCustomerThemeCompleted;

	public CounterSketchThemeServiceClient()
	{
	}

	public CounterSketchThemeServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public CounterSketchThemeServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public CounterSketchThemeServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public CounterSketchThemeServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public bool AssignCustomerThemeByKeyId(int keyId, int themeId)
	{
		return base.Channel.AssignCustomerThemeByKeyId(keyId, themeId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginAssignCustomerThemeByKeyId(int keyId, int themeId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginAssignCustomerThemeByKeyId(keyId, themeId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndAssignCustomerThemeByKeyId(IAsyncResult result)
	{
		return base.Channel.EndAssignCustomerThemeByKeyId(result);
	}

	private IAsyncResult OnBeginAssignCustomerThemeByKeyId(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int keyId = (int)inValues[0];
		int themeId = (int)inValues[1];
		return BeginAssignCustomerThemeByKeyId(keyId, themeId, callback, asyncState);
	}

	private object[] OnEndAssignCustomerThemeByKeyId(IAsyncResult result)
	{
		bool flag = EndAssignCustomerThemeByKeyId(result);
		return new object[1] { flag };
	}

	private void OnAssignCustomerThemeByKeyIdCompleted(object state)
	{
		if (this.AssignCustomerThemeByKeyIdCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.AssignCustomerThemeByKeyIdCompleted(this, new AssignCustomerThemeByKeyIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void AssignCustomerThemeByKeyIdAsync(int keyId, int themeId)
	{
		AssignCustomerThemeByKeyIdAsync(keyId, themeId, null);
	}

	public void AssignCustomerThemeByKeyIdAsync(int keyId, int themeId, object userState)
	{
		if (onBeginAssignCustomerThemeByKeyIdDelegate == null)
		{
			onBeginAssignCustomerThemeByKeyIdDelegate = OnBeginAssignCustomerThemeByKeyId;
		}
		if (onEndAssignCustomerThemeByKeyIdDelegate == null)
		{
			onEndAssignCustomerThemeByKeyIdDelegate = OnEndAssignCustomerThemeByKeyId;
		}
		if (onAssignCustomerThemeByKeyIdCompletedDelegate == null)
		{
			onAssignCustomerThemeByKeyIdCompletedDelegate = OnAssignCustomerThemeByKeyIdCompleted;
		}
		InvokeAsync(onBeginAssignCustomerThemeByKeyIdDelegate, new object[2] { keyId, themeId }, onEndAssignCustomerThemeByKeyIdDelegate, onAssignCustomerThemeByKeyIdCompletedDelegate, userState);
	}

	public int CreateCustomerTheme(CustomerTheme customerTheme)
	{
		return base.Channel.CreateCustomerTheme(customerTheme);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginCreateCustomerTheme(CustomerTheme customerTheme, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginCreateCustomerTheme(customerTheme, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public int EndCreateCustomerTheme(IAsyncResult result)
	{
		return base.Channel.EndCreateCustomerTheme(result);
	}

	private IAsyncResult OnBeginCreateCustomerTheme(object[] inValues, AsyncCallback callback, object asyncState)
	{
		CustomerTheme customerTheme = (CustomerTheme)inValues[0];
		return BeginCreateCustomerTheme(customerTheme, callback, asyncState);
	}

	private object[] OnEndCreateCustomerTheme(IAsyncResult result)
	{
		int num = EndCreateCustomerTheme(result);
		return new object[1] { num };
	}

	private void OnCreateCustomerThemeCompleted(object state)
	{
		if (this.CreateCustomerThemeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.CreateCustomerThemeCompleted(this, new CreateCustomerThemeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void CreateCustomerThemeAsync(CustomerTheme customerTheme)
	{
		CreateCustomerThemeAsync(customerTheme, null);
	}

	public void CreateCustomerThemeAsync(CustomerTheme customerTheme, object userState)
	{
		if (onBeginCreateCustomerThemeDelegate == null)
		{
			onBeginCreateCustomerThemeDelegate = OnBeginCreateCustomerTheme;
		}
		if (onEndCreateCustomerThemeDelegate == null)
		{
			onEndCreateCustomerThemeDelegate = OnEndCreateCustomerTheme;
		}
		if (onCreateCustomerThemeCompletedDelegate == null)
		{
			onCreateCustomerThemeCompletedDelegate = OnCreateCustomerThemeCompleted;
		}
		InvokeAsync(onBeginCreateCustomerThemeDelegate, new object[1] { customerTheme }, onEndCreateCustomerThemeDelegate, onCreateCustomerThemeCompletedDelegate, userState);
	}

	public bool DeleteCustomerTheme(int themeId)
	{
		return base.Channel.DeleteCustomerTheme(themeId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginDeleteCustomerTheme(int themeId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginDeleteCustomerTheme(themeId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndDeleteCustomerTheme(IAsyncResult result)
	{
		return base.Channel.EndDeleteCustomerTheme(result);
	}

	private IAsyncResult OnBeginDeleteCustomerTheme(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int themeId = (int)inValues[0];
		return BeginDeleteCustomerTheme(themeId, callback, asyncState);
	}

	private object[] OnEndDeleteCustomerTheme(IAsyncResult result)
	{
		bool flag = EndDeleteCustomerTheme(result);
		return new object[1] { flag };
	}

	private void OnDeleteCustomerThemeCompleted(object state)
	{
		if (this.DeleteCustomerThemeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.DeleteCustomerThemeCompleted(this, new DeleteCustomerThemeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void DeleteCustomerThemeAsync(int themeId)
	{
		DeleteCustomerThemeAsync(themeId, null);
	}

	public void DeleteCustomerThemeAsync(int themeId, object userState)
	{
		if (onBeginDeleteCustomerThemeDelegate == null)
		{
			onBeginDeleteCustomerThemeDelegate = OnBeginDeleteCustomerTheme;
		}
		if (onEndDeleteCustomerThemeDelegate == null)
		{
			onEndDeleteCustomerThemeDelegate = OnEndDeleteCustomerTheme;
		}
		if (onDeleteCustomerThemeCompletedDelegate == null)
		{
			onDeleteCustomerThemeCompletedDelegate = OnDeleteCustomerThemeCompleted;
		}
		InvokeAsync(onBeginDeleteCustomerThemeDelegate, new object[1] { themeId }, onEndDeleteCustomerThemeDelegate, onDeleteCustomerThemeCompletedDelegate, userState);
	}

	public CustomerTheme[] GetAvailableCustomerThemes()
	{
		return base.Channel.GetAvailableCustomerThemes();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetAvailableCustomerThemes(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetAvailableCustomerThemes(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public CustomerTheme[] EndGetAvailableCustomerThemes(IAsyncResult result)
	{
		return base.Channel.EndGetAvailableCustomerThemes(result);
	}

	private IAsyncResult OnBeginGetAvailableCustomerThemes(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetAvailableCustomerThemes(callback, asyncState);
	}

	private object[] OnEndGetAvailableCustomerThemes(IAsyncResult result)
	{
		CustomerTheme[] array = EndGetAvailableCustomerThemes(result);
		return new object[1] { array };
	}

	private void OnGetAvailableCustomerThemesCompleted(object state)
	{
		if (this.GetAvailableCustomerThemesCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetAvailableCustomerThemesCompleted(this, new GetAvailableCustomerThemesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetAvailableCustomerThemesAsync()
	{
		GetAvailableCustomerThemesAsync(null);
	}

	public void GetAvailableCustomerThemesAsync(object userState)
	{
		if (onBeginGetAvailableCustomerThemesDelegate == null)
		{
			onBeginGetAvailableCustomerThemesDelegate = OnBeginGetAvailableCustomerThemes;
		}
		if (onEndGetAvailableCustomerThemesDelegate == null)
		{
			onEndGetAvailableCustomerThemesDelegate = OnEndGetAvailableCustomerThemes;
		}
		if (onGetAvailableCustomerThemesCompletedDelegate == null)
		{
			onGetAvailableCustomerThemesCompletedDelegate = OnGetAvailableCustomerThemesCompleted;
		}
		InvokeAsync(onBeginGetAvailableCustomerThemesDelegate, null, onEndGetAvailableCustomerThemesDelegate, onGetAvailableCustomerThemesCompletedDelegate, userState);
	}

	public CustomerTheme[] GetAvailableCustomerThemesByKeyId(int keyId)
	{
		return base.Channel.GetAvailableCustomerThemesByKeyId(keyId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetAvailableCustomerThemesByKeyId(int keyId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetAvailableCustomerThemesByKeyId(keyId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public CustomerTheme[] EndGetAvailableCustomerThemesByKeyId(IAsyncResult result)
	{
		return base.Channel.EndGetAvailableCustomerThemesByKeyId(result);
	}

	private IAsyncResult OnBeginGetAvailableCustomerThemesByKeyId(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int keyId = (int)inValues[0];
		return BeginGetAvailableCustomerThemesByKeyId(keyId, callback, asyncState);
	}

	private object[] OnEndGetAvailableCustomerThemesByKeyId(IAsyncResult result)
	{
		CustomerTheme[] array = EndGetAvailableCustomerThemesByKeyId(result);
		return new object[1] { array };
	}

	private void OnGetAvailableCustomerThemesByKeyIdCompleted(object state)
	{
		if (this.GetAvailableCustomerThemesByKeyIdCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetAvailableCustomerThemesByKeyIdCompleted(this, new GetAvailableCustomerThemesByKeyIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetAvailableCustomerThemesByKeyIdAsync(int keyId)
	{
		GetAvailableCustomerThemesByKeyIdAsync(keyId, null);
	}

	public void GetAvailableCustomerThemesByKeyIdAsync(int keyId, object userState)
	{
		if (onBeginGetAvailableCustomerThemesByKeyIdDelegate == null)
		{
			onBeginGetAvailableCustomerThemesByKeyIdDelegate = OnBeginGetAvailableCustomerThemesByKeyId;
		}
		if (onEndGetAvailableCustomerThemesByKeyIdDelegate == null)
		{
			onEndGetAvailableCustomerThemesByKeyIdDelegate = OnEndGetAvailableCustomerThemesByKeyId;
		}
		if (onGetAvailableCustomerThemesByKeyIdCompletedDelegate == null)
		{
			onGetAvailableCustomerThemesByKeyIdCompletedDelegate = OnGetAvailableCustomerThemesByKeyIdCompleted;
		}
		InvokeAsync(onBeginGetAvailableCustomerThemesByKeyIdDelegate, new object[1] { keyId }, onEndGetAvailableCustomerThemesByKeyIdDelegate, onGetAvailableCustomerThemesByKeyIdCompletedDelegate, userState);
	}

	public Customer[] GetCustomers()
	{
		return base.Channel.GetCustomers();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetCustomers(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetCustomers(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Customer[] EndGetCustomers(IAsyncResult result)
	{
		return base.Channel.EndGetCustomers(result);
	}

	private IAsyncResult OnBeginGetCustomers(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetCustomers(callback, asyncState);
	}

	private object[] OnEndGetCustomers(IAsyncResult result)
	{
		Customer[] array = EndGetCustomers(result);
		return new object[1] { array };
	}

	private void OnGetCustomersCompleted(object state)
	{
		if (this.GetCustomersCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetCustomersCompleted(this, new GetCustomersCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetCustomersAsync()
	{
		GetCustomersAsync(null);
	}

	public void GetCustomersAsync(object userState)
	{
		if (onBeginGetCustomersDelegate == null)
		{
			onBeginGetCustomersDelegate = OnBeginGetCustomers;
		}
		if (onEndGetCustomersDelegate == null)
		{
			onEndGetCustomersDelegate = OnEndGetCustomers;
		}
		if (onGetCustomersCompletedDelegate == null)
		{
			onGetCustomersCompletedDelegate = OnGetCustomersCompleted;
		}
		InvokeAsync(onBeginGetCustomersDelegate, null, onEndGetCustomersDelegate, onGetCustomersCompletedDelegate, userState);
	}

	public Customer[] GetCustomersAssignedToCustomerThemeByThemeIdAndUserType(int themeId, string userType)
	{
		return base.Channel.GetCustomersAssignedToCustomerThemeByThemeIdAndUserType(themeId, userType);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetCustomersAssignedToCustomerThemeByThemeIdAndUserType(int themeId, string userType, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetCustomersAssignedToCustomerThemeByThemeIdAndUserType(themeId, userType, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Customer[] EndGetCustomersAssignedToCustomerThemeByThemeIdAndUserType(IAsyncResult result)
	{
		return base.Channel.EndGetCustomersAssignedToCustomerThemeByThemeIdAndUserType(result);
	}

	private IAsyncResult OnBeginGetCustomersAssignedToCustomerThemeByThemeIdAndUserType(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int themeId = (int)inValues[0];
		string userType = (string)inValues[1];
		return BeginGetCustomersAssignedToCustomerThemeByThemeIdAndUserType(themeId, userType, callback, asyncState);
	}

	private object[] OnEndGetCustomersAssignedToCustomerThemeByThemeIdAndUserType(IAsyncResult result)
	{
		Customer[] array = EndGetCustomersAssignedToCustomerThemeByThemeIdAndUserType(result);
		return new object[1] { array };
	}

	private void OnGetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeCompleted(object state)
	{
		if (this.GetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeCompleted(this, new GetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeAsync(int themeId, string userType)
	{
		GetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeAsync(themeId, userType, null);
	}

	public void GetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeAsync(int themeId, string userType, object userState)
	{
		if (onBeginGetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeDelegate == null)
		{
			onBeginGetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeDelegate = OnBeginGetCustomersAssignedToCustomerThemeByThemeIdAndUserType;
		}
		if (onEndGetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeDelegate == null)
		{
			onEndGetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeDelegate = OnEndGetCustomersAssignedToCustomerThemeByThemeIdAndUserType;
		}
		if (onGetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeCompletedDelegate == null)
		{
			onGetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeCompletedDelegate = OnGetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeCompleted;
		}
		InvokeAsync(onBeginGetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeDelegate, new object[2] { themeId, userType }, onEndGetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeDelegate, onGetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeCompletedDelegate, userState);
	}

	public Customer[] GetCustomersByUserType(string userType)
	{
		return base.Channel.GetCustomersByUserType(userType);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetCustomersByUserType(string userType, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetCustomersByUserType(userType, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Customer[] EndGetCustomersByUserType(IAsyncResult result)
	{
		return base.Channel.EndGetCustomersByUserType(result);
	}

	private IAsyncResult OnBeginGetCustomersByUserType(object[] inValues, AsyncCallback callback, object asyncState)
	{
		string userType = (string)inValues[0];
		return BeginGetCustomersByUserType(userType, callback, asyncState);
	}

	private object[] OnEndGetCustomersByUserType(IAsyncResult result)
	{
		Customer[] array = EndGetCustomersByUserType(result);
		return new object[1] { array };
	}

	private void OnGetCustomersByUserTypeCompleted(object state)
	{
		if (this.GetCustomersByUserTypeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetCustomersByUserTypeCompleted(this, new GetCustomersByUserTypeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetCustomersByUserTypeAsync(string userType)
	{
		GetCustomersByUserTypeAsync(userType, null);
	}

	public void GetCustomersByUserTypeAsync(string userType, object userState)
	{
		if (onBeginGetCustomersByUserTypeDelegate == null)
		{
			onBeginGetCustomersByUserTypeDelegate = OnBeginGetCustomersByUserType;
		}
		if (onEndGetCustomersByUserTypeDelegate == null)
		{
			onEndGetCustomersByUserTypeDelegate = OnEndGetCustomersByUserType;
		}
		if (onGetCustomersByUserTypeCompletedDelegate == null)
		{
			onGetCustomersByUserTypeCompletedDelegate = OnGetCustomersByUserTypeCompleted;
		}
		InvokeAsync(onBeginGetCustomersByUserTypeDelegate, new object[1] { userType }, onEndGetCustomersByUserTypeDelegate, onGetCustomersByUserTypeCompletedDelegate, userState);
	}

	public string[] GetCustomerUserTypes()
	{
		return base.Channel.GetCustomerUserTypes();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginGetCustomerUserTypes(AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginGetCustomerUserTypes(callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public string[] EndGetCustomerUserTypes(IAsyncResult result)
	{
		return base.Channel.EndGetCustomerUserTypes(result);
	}

	private IAsyncResult OnBeginGetCustomerUserTypes(object[] inValues, AsyncCallback callback, object asyncState)
	{
		return BeginGetCustomerUserTypes(callback, asyncState);
	}

	private object[] OnEndGetCustomerUserTypes(IAsyncResult result)
	{
		string[] array = EndGetCustomerUserTypes(result);
		return new object[1] { array };
	}

	private void OnGetCustomerUserTypesCompleted(object state)
	{
		if (this.GetCustomerUserTypesCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.GetCustomerUserTypesCompleted(this, new GetCustomerUserTypesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void GetCustomerUserTypesAsync()
	{
		GetCustomerUserTypesAsync(null);
	}

	public void GetCustomerUserTypesAsync(object userState)
	{
		if (onBeginGetCustomerUserTypesDelegate == null)
		{
			onBeginGetCustomerUserTypesDelegate = OnBeginGetCustomerUserTypes;
		}
		if (onEndGetCustomerUserTypesDelegate == null)
		{
			onEndGetCustomerUserTypesDelegate = OnEndGetCustomerUserTypes;
		}
		if (onGetCustomerUserTypesCompletedDelegate == null)
		{
			onGetCustomerUserTypesCompletedDelegate = OnGetCustomerUserTypesCompleted;
		}
		InvokeAsync(onBeginGetCustomerUserTypesDelegate, null, onEndGetCustomerUserTypesDelegate, onGetCustomerUserTypesCompletedDelegate, userState);
	}

	public bool RemoveAssignedCustomerThemeByKeyId(int keyId, int themeId)
	{
		return base.Channel.RemoveAssignedCustomerThemeByKeyId(keyId, themeId);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginRemoveAssignedCustomerThemeByKeyId(int keyId, int themeId, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginRemoveAssignedCustomerThemeByKeyId(keyId, themeId, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndRemoveAssignedCustomerThemeByKeyId(IAsyncResult result)
	{
		return base.Channel.EndRemoveAssignedCustomerThemeByKeyId(result);
	}

	private IAsyncResult OnBeginRemoveAssignedCustomerThemeByKeyId(object[] inValues, AsyncCallback callback, object asyncState)
	{
		int keyId = (int)inValues[0];
		int themeId = (int)inValues[1];
		return BeginRemoveAssignedCustomerThemeByKeyId(keyId, themeId, callback, asyncState);
	}

	private object[] OnEndRemoveAssignedCustomerThemeByKeyId(IAsyncResult result)
	{
		bool flag = EndRemoveAssignedCustomerThemeByKeyId(result);
		return new object[1] { flag };
	}

	private void OnRemoveAssignedCustomerThemeByKeyIdCompleted(object state)
	{
		if (this.RemoveAssignedCustomerThemeByKeyIdCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.RemoveAssignedCustomerThemeByKeyIdCompleted(this, new RemoveAssignedCustomerThemeByKeyIdCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void RemoveAssignedCustomerThemeByKeyIdAsync(int keyId, int themeId)
	{
		RemoveAssignedCustomerThemeByKeyIdAsync(keyId, themeId, null);
	}

	public void RemoveAssignedCustomerThemeByKeyIdAsync(int keyId, int themeId, object userState)
	{
		if (onBeginRemoveAssignedCustomerThemeByKeyIdDelegate == null)
		{
			onBeginRemoveAssignedCustomerThemeByKeyIdDelegate = OnBeginRemoveAssignedCustomerThemeByKeyId;
		}
		if (onEndRemoveAssignedCustomerThemeByKeyIdDelegate == null)
		{
			onEndRemoveAssignedCustomerThemeByKeyIdDelegate = OnEndRemoveAssignedCustomerThemeByKeyId;
		}
		if (onRemoveAssignedCustomerThemeByKeyIdCompletedDelegate == null)
		{
			onRemoveAssignedCustomerThemeByKeyIdCompletedDelegate = OnRemoveAssignedCustomerThemeByKeyIdCompleted;
		}
		InvokeAsync(onBeginRemoveAssignedCustomerThemeByKeyIdDelegate, new object[2] { keyId, themeId }, onEndRemoveAssignedCustomerThemeByKeyIdDelegate, onRemoveAssignedCustomerThemeByKeyIdCompletedDelegate, userState);
	}

	public bool UpdateCustomerTheme(CustomerTheme customerThemeUpdate)
	{
		return base.Channel.UpdateCustomerTheme(customerThemeUpdate);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IAsyncResult BeginUpdateCustomerTheme(CustomerTheme customerThemeUpdate, AsyncCallback callback, object asyncState)
	{
		return base.Channel.BeginUpdateCustomerTheme(customerThemeUpdate, callback, asyncState);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public bool EndUpdateCustomerTheme(IAsyncResult result)
	{
		return base.Channel.EndUpdateCustomerTheme(result);
	}

	private IAsyncResult OnBeginUpdateCustomerTheme(object[] inValues, AsyncCallback callback, object asyncState)
	{
		CustomerTheme customerThemeUpdate = (CustomerTheme)inValues[0];
		return BeginUpdateCustomerTheme(customerThemeUpdate, callback, asyncState);
	}

	private object[] OnEndUpdateCustomerTheme(IAsyncResult result)
	{
		bool flag = EndUpdateCustomerTheme(result);
		return new object[1] { flag };
	}

	private void OnUpdateCustomerThemeCompleted(object state)
	{
		if (this.UpdateCustomerThemeCompleted != null)
		{
			InvokeAsyncCompletedEventArgs e = (InvokeAsyncCompletedEventArgs)state;
			this.UpdateCustomerThemeCompleted(this, new UpdateCustomerThemeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
		}
	}

	public void UpdateCustomerThemeAsync(CustomerTheme customerThemeUpdate)
	{
		UpdateCustomerThemeAsync(customerThemeUpdate, null);
	}

	public void UpdateCustomerThemeAsync(CustomerTheme customerThemeUpdate, object userState)
	{
		if (onBeginUpdateCustomerThemeDelegate == null)
		{
			onBeginUpdateCustomerThemeDelegate = OnBeginUpdateCustomerTheme;
		}
		if (onEndUpdateCustomerThemeDelegate == null)
		{
			onEndUpdateCustomerThemeDelegate = OnEndUpdateCustomerTheme;
		}
		if (onUpdateCustomerThemeCompletedDelegate == null)
		{
			onUpdateCustomerThemeCompletedDelegate = OnUpdateCustomerThemeCompleted;
		}
		InvokeAsync(onBeginUpdateCustomerThemeDelegate, new object[1] { customerThemeUpdate }, onEndUpdateCustomerThemeDelegate, onUpdateCustomerThemeCompletedDelegate, userState);
	}
}
