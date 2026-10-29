using System.ComponentModel;

namespace GemvisionCommon.Utilities.Networking.Enums;

public enum NetworkConnectionResult
{
	[Description("")]
	NoError = 0,
	[Description("Access Denied")]
	ErrorAccessDenied = 5,
	[Description("Bad Network Path")]
	ErrorBadNetPath = 53,
	[Description("Bad Net Name")]
	ErrorBadNetName = 67,
	[Description("Already Assigned")]
	ErrorAlreadyAssigned = 85,
	[Description("Invalid Password")]
	ErrorInvalidPassword = 86,
	[Description("Invalid Parameter")]
	ErrorInvalidParameter = 87,
	[Description("More Data")]
	ErrorMoreData = 234,
	[Description("No More Items")]
	ErrorNoMoreItems = 259,
	[Description("Invalid Address")]
	ErrorInvalidAddress = 487,
	[Description("Bad Device")]
	ErrorBadDevice = 1200,
	[Description("No Net Or Bad Path")]
	ErrorNoNetOrBadPath = 1203,
	[Description("Bad Provider")]
	ErrorBadProvider = 1204,
	[Description("Cannot Open Profile")]
	ErrorCannotOpenProfile = 1205,
	[Description("Bad Profile")]
	ErrorBadProfile = 1206,
	[Description("Extended Error")]
	ErrorExtendedError = 1208,
	[Description("Invalid Password")]
	ErrorInvalidPassword2 = 1216,
	[Description("Session Credential Conflict")]
	ErrorSessionCredentialConflict = 1219,
	[Description("No Network")]
	ErrorNoNetwork = 1222,
	[Description("Cancelled")]
	ErrorCancelled = 1223,
	[Description("Extended Error")]
	ErrorNotConnected = 2250,
	[Description("Open Files")]
	ErrorOpenFiles = 2401,
	[Description("Device In Use")]
	ErrorDeviceInUse = 2404
}
