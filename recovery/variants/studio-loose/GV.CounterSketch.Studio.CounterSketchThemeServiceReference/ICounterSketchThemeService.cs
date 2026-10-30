using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace GV.CounterSketch.Studio.CounterSketchThemeServiceReference;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "CounterSketchThemeServiceReference.ICounterSketchThemeService")]
public interface ICounterSketchThemeService
{
	[OperationContract(Action = "http://tempuri.org/ICounterSketchThemeService/AssignCustomerThemeByKeyId", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/AssignCustomerThemeByKeyIdResponse")]
	bool AssignCustomerThemeByKeyId(int keyId, int themeId);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ICounterSketchThemeService/AssignCustomerThemeByKeyId", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/AssignCustomerThemeByKeyIdResponse")]
	IAsyncResult BeginAssignCustomerThemeByKeyId(int keyId, int themeId, AsyncCallback callback, object asyncState);

	bool EndAssignCustomerThemeByKeyId(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/ICounterSketchThemeService/CreateCustomerTheme", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/CreateCustomerThemeResponse")]
	int CreateCustomerTheme(CustomerTheme customerTheme);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ICounterSketchThemeService/CreateCustomerTheme", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/CreateCustomerThemeResponse")]
	IAsyncResult BeginCreateCustomerTheme(CustomerTheme customerTheme, AsyncCallback callback, object asyncState);

	int EndCreateCustomerTheme(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/ICounterSketchThemeService/DeleteCustomerTheme", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/DeleteCustomerThemeResponse")]
	bool DeleteCustomerTheme(int themeId);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ICounterSketchThemeService/DeleteCustomerTheme", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/DeleteCustomerThemeResponse")]
	IAsyncResult BeginDeleteCustomerTheme(int themeId, AsyncCallback callback, object asyncState);

	bool EndDeleteCustomerTheme(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/ICounterSketchThemeService/GetAvailableCustomerThemes", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/GetAvailableCustomerThemesResponse")]
	CustomerTheme[] GetAvailableCustomerThemes();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ICounterSketchThemeService/GetAvailableCustomerThemes", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/GetAvailableCustomerThemesResponse")]
	IAsyncResult BeginGetAvailableCustomerThemes(AsyncCallback callback, object asyncState);

	CustomerTheme[] EndGetAvailableCustomerThemes(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/ICounterSketchThemeService/GetAvailableCustomerThemesByKeyId", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/GetAvailableCustomerThemesByKeyIdResponse")]
	CustomerTheme[] GetAvailableCustomerThemesByKeyId(int keyId);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ICounterSketchThemeService/GetAvailableCustomerThemesByKeyId", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/GetAvailableCustomerThemesByKeyIdResponse")]
	IAsyncResult BeginGetAvailableCustomerThemesByKeyId(int keyId, AsyncCallback callback, object asyncState);

	CustomerTheme[] EndGetAvailableCustomerThemesByKeyId(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/ICounterSketchThemeService/GetCustomers", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/GetCustomersResponse")]
	Customer[] GetCustomers();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ICounterSketchThemeService/GetCustomers", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/GetCustomersResponse")]
	IAsyncResult BeginGetCustomers(AsyncCallback callback, object asyncState);

	Customer[] EndGetCustomers(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/ICounterSketchThemeService/GetCustomersAssignedToCustomerThemeByThemeIdAndUserType", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/GetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeResponse")]
	Customer[] GetCustomersAssignedToCustomerThemeByThemeIdAndUserType(int themeId, string userType);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ICounterSketchThemeService/GetCustomersAssignedToCustomerThemeByThemeIdAndUserType", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/GetCustomersAssignedToCustomerThemeByThemeIdAndUserTypeResponse")]
	IAsyncResult BeginGetCustomersAssignedToCustomerThemeByThemeIdAndUserType(int themeId, string userType, AsyncCallback callback, object asyncState);

	Customer[] EndGetCustomersAssignedToCustomerThemeByThemeIdAndUserType(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/ICounterSketchThemeService/GetCustomersByUserType", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/GetCustomersByUserTypeResponse")]
	Customer[] GetCustomersByUserType(string userType);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ICounterSketchThemeService/GetCustomersByUserType", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/GetCustomersByUserTypeResponse")]
	IAsyncResult BeginGetCustomersByUserType(string userType, AsyncCallback callback, object asyncState);

	Customer[] EndGetCustomersByUserType(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/ICounterSketchThemeService/GetCustomerUserTypes", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/GetCustomerUserTypesResponse")]
	string[] GetCustomerUserTypes();

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ICounterSketchThemeService/GetCustomerUserTypes", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/GetCustomerUserTypesResponse")]
	IAsyncResult BeginGetCustomerUserTypes(AsyncCallback callback, object asyncState);

	string[] EndGetCustomerUserTypes(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/ICounterSketchThemeService/RemoveAssignedCustomerThemeByKeyId", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/RemoveAssignedCustomerThemeByKeyIdResponse")]
	bool RemoveAssignedCustomerThemeByKeyId(int keyId, int themeId);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ICounterSketchThemeService/RemoveAssignedCustomerThemeByKeyId", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/RemoveAssignedCustomerThemeByKeyIdResponse")]
	IAsyncResult BeginRemoveAssignedCustomerThemeByKeyId(int keyId, int themeId, AsyncCallback callback, object asyncState);

	bool EndRemoveAssignedCustomerThemeByKeyId(IAsyncResult result);

	[OperationContract(Action = "http://tempuri.org/ICounterSketchThemeService/UpdateCustomerTheme", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/UpdateCustomerThemeResponse")]
	bool UpdateCustomerTheme(CustomerTheme customerThemeUpdate);

	[OperationContract(AsyncPattern = true, Action = "http://tempuri.org/ICounterSketchThemeService/UpdateCustomerTheme", ReplyAction = "http://tempuri.org/ICounterSketchThemeService/UpdateCustomerThemeResponse")]
	IAsyncResult BeginUpdateCustomerTheme(CustomerTheme customerThemeUpdate, AsyncCallback callback, object asyncState);

	bool EndUpdateCustomerTheme(IAsyncResult result);
}
